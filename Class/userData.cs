using System.Text;
using System.ComponentModel;

namespace PwdManagement
{
    /// <summary>
    /// 用户数据
    /// </summary>
    public class userData : INotifyPropertyChanged
    {
        #region 属性

        public event PropertyChangedEventHandler PropertyChanged;
        
        private string _Name;

        private string _Data;

        private int _Level;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (Encoding.Default.GetBytes(value).Length <= 14)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
                OnPropertyChanged("Data");
            }
        }

        public int Level
        {
            get
            {
                return _Level;
            }
            set
            {
                _Level = value;
                OnPropertyChanged("Level");
            }
        }

        private bool _isEditable = true;
        public bool isEditable
        {
            get 
            {
                return _isEditable;
            }
            set
            {
                _isEditable = value;
                OnPropertyChanged("isEditable");
            }
        }

        private bool _isVisible;

        public bool isVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged("isVisible");
            }
        }

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

        #endregion

        #region 方法

        public userData(string name,string data,int level)
        {
            Name = name;
            Data = data;
            Level = level;
            if (Shell.currentLevel < 0 || Shell.currentLevel >= Shell.tabItem.Count)
                isVisible = false;
            else
                isVisible = (Shell.currentLevel == Level && Shell.userInfo.power >= Shell.tabItem[Shell.currentLevel].power);
            isEditable = true;
            isSelected = false;
        }

        protected void OnPropertyChanged(string caller = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        #endregion
    }
}
