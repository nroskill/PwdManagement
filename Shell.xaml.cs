using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using PwdManagement.login;
using System.Text;

namespace PwdManagement
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : Window
    {
        #region 属性

        public static checkUser userInfo { get; set; }

        public static userData currentFocus { get; set; }

        public static int currentLevel { get; set; }

        public static ObservableCollection<tabItem> tabItem { get; set; }

        public static ObservableCollection<userData> userData { get; set; }

        #endregion

        #region 方法

        public Shell()
        {
            InitializeComponent();

            if (tabItem.Count == 0)
            {
                tabItem.Add(new tabItem() { power = 0, level = 0 });
                tabItem.Add(new tabItem() { power = 20, level = 1 });
                tabItem.Add(new tabItem() { power = 40, level = 2 });
                tabItem.Add(new tabItem() { power = 60, level = 3 });
                tabItem.Add(new tabItem() { power = 80, level = 4 });
            }

            if (userInfo.userFace != null)
            {
                this.face.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(userInfo.userFace.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }

            this.timeShow.Content = DateTime.Now.ToString("yyyy-MM-dd");
            
            freshVisible();
        }

        private void freshVisible()
        {
            if (Shell.currentLevel < 0 || Shell.currentLevel >= Shell.tabItem.Count)
            {
                userInfo.power = userInfo.power;
                return;
            }
            if(this.SeachText.Text == "")
                foreach(var i in userData)
                {
                    i.isVisible = (Shell.currentLevel == i.Level && Shell.userInfo.power >= Shell.tabItem[Shell.currentLevel].power);
                }
            else
                foreach(var i in userData)
                {
                    i.isVisible = (Shell.currentLevel == i.Level && Shell.userInfo.power >= Shell.tabItem[Shell.currentLevel].power) && i.Name.Contains(this.SeachText.Text);
                }
            userInfo.power = userInfo.power;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Close(object sender, RoutedEventArgs e)
        {
            clearExtraData();
            rwData.writeFile();
            Application.Current.Shutdown();
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            clearExtraData();
            userData.Add(new userData("","",currentLevel));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            currentFocus = (sender as Button).DataContext as userData;
            foreach(var i in userData)
            {
                i.isSelected = false;
            }
            ((sender as Button).DataContext as userData).isSelected = true;
        }

        private void changeValue_button_Click(object sender, RoutedEventArgs e)
        {
            new changeValue().ShowDialog();
            freshVisible();
        }

        private void changePower_button_Click(object sender, RoutedEventArgs e)
        {
            if(userInfo.power < 80)
            {
                new ResultWindow(ResultWindow.infotype.Error, "辨识度不足，需达到80%", "返回").ShowDialog();
                return;
            }
            new changePower().ShowDialog();
            freshVisible();
        }

        private void NameText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((sender as TextBox).DataContext as userData).isEditable = false;
            if ((sender as TextBox).Text != "")
                return;
            clearExtraData();
        }

        private void clearExtraData()
        {
            for (var i = 0; i < userData.Count; i++)
                if (userData[i].Name == "")
                    userData.RemoveAt(i);
        }

        private void remove_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentFocus == null)
            {
                new ResultWindow(ResultWindow.infotype.Error, "没有选中任何一项", "返回").ShowDialog();
                return;
            }
            new confirm().ShowDialog();
            if (!confirm.result)
                return;
            for (var i = 0; i < userData.Count; i++)
            {
                if (userData[i].isSelected == true)
                    userData.RemoveAt(i);
            }
            currentFocus = null;
        }

        private void tabitem_changed(object sender, RoutedEventArgs e)
        {
            currentFocus = null;
            currentLevel = ((sender as Button).DataContext as tabItem).level;
            foreach(var i in tabItem)
            {
                i.isSelected = false;
            }
            ((sender as Button).DataContext as tabItem).isSelected = true;
            freshVisible();
        }

        private void SeachText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            while (Encoding.Default.GetBytes(tb.Text).Length > 14)
            {
                tb.Text = tb.Text.Remove(tb.Text.Length - 1);
                tb.SelectionStart = tb.Text.Length;
            }
            freshVisible();
        }

        private void NameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            while (Encoding.Default.GetBytes(tb.Text).Length > 14)
            {
                tb.Text = tb.Text.Remove(tb.Text.Length - 1);
                tb.SelectionStart = tb.Text.Length;
            }
        }

        private void DataText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            while (Encoding.Default.GetBytes(tb.Text).Length > 140)
            {
                tb.Text = tb.Text.Remove(tb.Text.Length - 1);
                tb.SelectionStart = tb.Text.Length;
            }
        }

        #endregion
    }
}
