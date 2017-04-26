using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PwdManagement.login;

namespace PwdManagement.verify
{
    /// <summary>
    /// changePassword.xaml 的交互逻辑
    /// </summary>
    public partial class changePassword : Window
    {
        public changePassword()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.textbox1.Text == "" || this.textbox2.Text == "")
            {
                var r = new ResultWindow(ResultWindow.infotype.Error, "密码不能为空", "返回");
                r.ShowDialog();
                return;
            }
            if (this.textbox1.Text != this.textbox2.Text)
            {
                var r = new ResultWindow(ResultWindow.infotype.Error, "两次输入不同", "返回");
                r.ShowDialog();
                return;
            }
            Shell.userInfo.checkData[0] = rwData.md5_create(textbox2.Text);
            this.Close();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
