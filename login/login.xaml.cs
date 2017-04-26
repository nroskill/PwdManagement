using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;
using PwdManagement.login;

namespace PwdManagement
{
    /// <summary>
    /// Login_window.xaml 的交互逻辑
    /// </summary>
    public partial class Login_window : Window
    {
        public Login_window()
        {
            InitializeComponent();
            Shell.tabItem = new ObservableCollection<tabItem>();
            Shell.userData = new ObservableCollection<userData>();
            Shell.userInfo = new checkUser();
            Shell.currentFocus = null;
            Shell.currentLevel = -1;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private  void reg(object sender, RoutedEventArgs e)
        {
            string t = this.loginform.Text;
            if (t == "")
            {
                new ResultWindow(ResultWindow.infotype.Error, "请输入用户名", "返回").ShowDialog();
                return;
            }
            if (Encoding.Default.GetBytes(t).Length > 8)
            {
                new ResultWindow(ResultWindow.infotype.Error, "用户名过长", "返回").ShowDialog();
                return;
            }
            if (File.Exists(@"data\" + t))
            {
                new ResultWindow(ResultWindow.infotype.Error, "您注册的用户已存在", "返回").ShowDialog();
                return;
            }

            Shell.userInfo.userName = t;
            if (rwData.writeFile())
            {
                new ResultWindow(ResultWindow.infotype.Success, "注册成功", "确定").ShowDialog();
            }
        }

        private void login(object sender, RoutedEventArgs e)
        {
            string t = this.loginform.Text;
            if (t == "")
            {
                var window = new ResultWindow(ResultWindow.infotype.Error, "请输入用户名", "返回");
                window.ShowDialog();
                return;
            }

            if (!File.Exists(@"data\" + t))
            {
                var window = new ResultWindow(ResultWindow.infotype.Error, "不存在该用户", "返回");
                window.ShowDialog();
                return;
            }

            Shell.userInfo.userName = t;
            if (!rwData.readFile())
            {
                var window = new ResultWindow(ResultWindow.infotype.Error, "用户文件已损坏", "返回");
                window.ShowDialog();
                return;
            }

            var main_window = new Shell();
            main_window.Show();
            this.Close();
        }
    }
}
