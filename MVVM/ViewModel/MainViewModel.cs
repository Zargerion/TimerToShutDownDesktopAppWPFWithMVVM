using TimerToShutDownDesktopApp.Core;

namespace TimerToShutDownDesktopApp.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public ClockViewModel ClockVM { get; set; }

        private string _myProperty1;
        public string MyProperty1
        {
            get { return _myProperty1; }
            set
            {
                if (_myProperty1 != value)
                {
                    _myProperty1 = value;
                    OnPropertyChanged(nameof(MyProperty1));
                }
            }
        }
        private string _myProperty2;
        public string MyProperty2
        {
            get { return _myProperty2; }
            set
            {
                if (_myProperty2 != value)
                {
                    _myProperty2 = value;
                    OnPropertyChanged(nameof(MyProperty2));
                }
            }
        }
        private string _myProperty3;
        public string MyProperty3
        {
            get { return _myProperty3; }
            set
            {
                if (_myProperty3 != value)
                {
                    _myProperty3 = value;
                    OnPropertyChanged(nameof(MyProperty3));
                }
            }
        }

        private object _CurrentView;
        public object CurrentView
        {
            get { return _CurrentView; }  
            set { _CurrentView = value; OnPropertyChanged(); }
        }
        public MainViewModel()
        {
            ClockVM = new ClockViewModel();
            CurrentView = ClockVM;
            MyProperty1 = MyProperty1;
            MyProperty2 = MyProperty2;
            MyProperty3 = MyProperty3;

        }
    }
}
