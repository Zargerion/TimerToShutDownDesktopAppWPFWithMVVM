
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;


namespace TimerToShutDownDesktopApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer Timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            Uri iconUri = new Uri("pack://application:,,,/Image/favicon.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);

        }
        
        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime d;
            d = DateTime.Now;
            labelTime.Content = $"{d.Hour}:{d.Minute}:{d.Second}";
        }

        private void ClickMinButton(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ClickExitButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        bool timer_control = new bool();
        int hours = new int();
        int mins = new int();
        int secs = new int();
        TextBox HHH = new TextBox();
        TextBox MMM = new TextBox();
        TextBox SSS = new TextBox();
        //static bool timerButton_control = true;
        
        static bool new_control = true;
        static TimeSpan remaningSec;
        public EventHandler eventHandler;

        DispatcherTimer TimerMain = new DispatcherTimer();


        private void ClickStartTimerButton(object sender, EventArgs e)
        {

            if (new_control)
            {
                TimerMain = new DispatcherTimer();
                TimerMain.Interval = new TimeSpan(0, 0, 1);
                new_control = false;
            }
            TimerMain.Stop();
            TimerMain = null;
            if(TimerMain == null)
            {
                TimerMain = new DispatcherTimer();
                TimerMain.Interval = new TimeSpan(0, 0, 1);
            }

            timer_control = true;
            

            string HHv = HH.Text;
            string mmv = MM.Text;
            string ssv = SS.Text;
            if (HHv == "")
            {
                HHv = "0";
            }
            if (mmv == "")
            {
                mmv = "0";
            }
            if (ssv == "")
            {
                ssv = "0";
            }
            if (int.Parse(HHv) + int.Parse(mmv) + int.Parse(ssv) > 23 + 59 + 59)
            {
                HHv = "23";
                mmv = "59";
                ssv = "59";
            }
            hours = int.Parse(HHv);
            mins = int.Parse((mmv));
            secs = int.Parse((ssv));

                
  


            HHH = (TextBox)FindName("HH");
            MMM = (TextBox)FindName("MM");
            SSS = (TextBox)FindName("SS");

                HHH.Clear();
           
                MMM.Clear();

                SSS.Clear();
            
            MMM.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            HHH.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            SSS.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            App.Current.Dispatcher.Invoke(() =>
            {
                MMM.UpdateLayout();
                HHH.UpdateLayout();
                SSS.UpdateLayout();
            });


                //timerButton_control = false;
                int seconds = hours * 3600 + mins * 60 + secs;

                remaningSec = TimeSpan.FromSeconds(seconds);

                

                

            //TimerMain.Tick += new EventHandler(MainTimerTickF);
            eventHandler = new EventHandler(delegate
            {

                if (remaningSec != TimeSpan.Zero && timer_control)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        labelTimer.Content = remaningSec.ToString(@"hh\:mm\:ss");
                    });
                    remaningSec = remaningSec.Add(TimeSpan.FromSeconds(-1));
                    
                }
                else
                {
                    Process.Start("shutdown", "/s /t 5");
                    TimerMain.Stop();
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        labelTimer.Content = "00:00:00";
                    });
                    TimerMain = null;
                    new_control = true;
                }

            });


            TimerMain.Tick += eventHandler;

            TimerMain.Start();

        }

        //private void MainTimerTickF(object sender, EventArgs e)
        //{
        //    if (remaningSec != TimeSpan.Zero && timer_control)
        //    {
        //        App.Current.Dispatcher.Invoke(() =>
        //        {
        //            labelTimer.Content = remaningSec.ToString(@"hh\:mm\:ss");
        //        });
        //        remaningSec = remaningSec.Add(TimeSpan.FromSeconds(-1));
        //    }
        //    else
        //    {
        //        MessageBox.Show("nice");
        //        TimerMain.Stop();
        //        App.Current.Dispatcher.Invoke(() =>
        //        {
        //            labelTimer.Content = "00:00:00";
        //        });
        //    }
        //}

        private void ClickDeleteTimerButton(object sender, RoutedEventArgs e)
        {
            remaningSec = TimeSpan.FromSeconds(0);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //timerButton_control = true;
            timer_control = false;
            TimerMain.Stop();
            App.Current.Dispatcher.Invoke(() =>
            {
                labelTimer.Content = "00:00:00";
            });
            TimerMain = null;
            new_control = true;
        }


        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);


        }


        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (sender is TextBox textBox)
        //    {
        //        textBox.Text = new string
        //            (
        //                 textBox
        //                 .Text
        //                 .Where
        //                 (ch =>
        //                    ch == '+' || ch == '-'
        //                    || (ch >= '0' && ch <= '9')
        //                    || (ch >= 'а' && ch <= 'я')
        //                    || (ch >= 'А' && ch <= 'Я')
        //                    || ch == 'ё' || ch == 'Ё'
        //                 )
        //                 .ToArray()
        //            );
        //        textBox.SelectionStart = e.Changes.First().Offset + 1;
        //        textBox.SelectionLength = 0;
        //    }
        //}

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    
        //    IconHelper.RemoveIcon(this);
        //    
        //}


        //public static class IconHelper
        //{
        //    [DllImport("user32.dll")]
        //    static extern int GetWindowLong(IntPtr hwnd, int index);

        //    [DllImport("user32.dll")]
        //    static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        //    [DllImport("user32.dll")]
        //    static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
        //        int x, int y, int width, int height, uint flags);

        //    [DllImport("user32.dll")]
        //    static extern IntPtr SendMessage(IntPtr hwnd, uint msg,
        //        IntPtr wParam, IntPtr lParam);

        //    //[System.Runtime.InteropServices.DllImport("gdi32.dll")]
        //    //static extern bool DeleteObject(IntPtr hObject);

        //    const int GWL_EXSTYLE = -20;
        //    const int WS_EX_DLGMODALFRAME = 0x0001;
        //    const int SWP_NOSIZE = 0x0001;
        //    const int SWP_NOMOVE = 0x0002;
        //    const int SWP_NOZORDER = 0x0004;
        //    const int SWP_FRAMECHANGED = 0x0020;
        //    const uint WM_SETICON = 0x0080;

        //    public static void RemoveIcon(Window window)
        //    {
        //        // Get this window's handle
        //        IntPtr hwnd = new WindowInteropHelper(window).Handle;

        //        // Change the extended window style to not show a window icon
        //        int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        //        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

        //        // Update the window's non-client area to reflect the changes
        //        SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
        //              SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        //    }

        //public static void SetTaskbarIcon(Window window)
        //{
        //    // Get the window handle
        //    IntPtr hwnd = new WindowInteropHelper(window).Handle;

        //    Bitmap bm = new Bitmap("C:/Users/щдуп/source/cs-tests/TimerToShutDownDesktopApp/Image/favicon.jpg");
        //    IntPtr hBitmap = bm.GetHbitmap();

        //    // Send the WM_SETICON message to set the taskbar icon
        //    SendMessage(hwnd, WM_SETICON, (IntPtr)0, hBitmap);
        //    SendMessage(hwnd, WM_SETICON, (IntPtr)1, hBitmap);

        //    DeleteObject(hBitmap);
        //}
        //}

    }
}
