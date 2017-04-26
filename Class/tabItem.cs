using System.ComponentModel;

namespace PwdManagement
{
    /// <summary>
    /// Shell界面中对应的tabitem，用于实现前后端分离，通常不需改动
    /// </summary>
    public class tabItem : INotifyPropertyChanged
    {
        private int _power;

        private int _level;

        private bool _isSelected;

        public bool isSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("isSelected");
            }
        }

        public string header
        {
            get
            {
                return power.ToString();
            }
        }

        public int level
        {
            get
            {
                return _level;
            }
            set
            {
                _level= value;
                OnPropertyChanged("level");
            }
        }

        public int power
        {
            get
            {
                return _power;
            }
            set
            {
                _power = value;
                OnPropertyChanged("power");
            }
        }

        public tabItem()
        {
            _power = 0;
            _level = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string caller = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
