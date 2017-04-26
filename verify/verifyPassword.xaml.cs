using System.Windows;
using PwdManagement.login;

namespace PwdManagement.verify
{
    /// <summary>
    /// verifyPassword.xaml 的交互逻辑
    /// </summary>
    public partial class verifyPassword : Window
    {
        #region 界面控制
        public verifyPassword()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.textbox1.Text == "")
            {
                var r = new ResultWindow(ResultWindow.infotype.Error, "密码不能为空", "返回");
                r.ShowDialog();
                return;
            }
            if (rwData.md5_test(this.textbox1.Text, Shell.userInfo.checkData[0]) == false)
            {
                var r = new ResultWindow(ResultWindow.infotype.Error, "密码错误", "返回");
                r.ShowDialog();
                return;
            }
            Shell.userInfo.passValue = 100;
            this.Close();
            return;
        }

        private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}