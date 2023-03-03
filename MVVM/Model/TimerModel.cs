using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace TimerToShutDownDesktopApp.MVVM.Model
{
    internal class TimerModel : INotifyPropertyChanged
    {
        private string _currentTime;

        public event PropertyChangedEventHandler PropertyChanged;
        public string CurrentTime
        {
            get { return _currentTime; }
            private set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTime)));
                }
            }
        }
        public TimerModel()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += timer_Tick;
            timer.Start();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
