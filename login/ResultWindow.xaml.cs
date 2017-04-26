using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PwdManagement.login
{
    /// <summary>
    /// ResultWindow1.xaml 的交互逻辑
    /// </summary>
    public partial class ResultWindow : Window
    {
        public enum infotype
        { 
            Success = 0,
            Warning = 1,
            Error = 2
        }
        /// <summary>
        /// 自定义的提示窗口
        /// </summary>
        /// <param name="r">提示信息的类型</param>
        /// <param name="str1">主要信息</param>
        /// <param name="str2">返回按钮内容</param>
        public ResultWindow(infotype r, string str1, string str2)
        {
            InitializeComponent();
            var color = new Color();
            switch (r)
            {
                case infotype.Success:
                    color = Color.FromRgb(92, 213, 100);
                    break;
                case infotype.Error:
                    color = Color.FromRgb(241, 75, 49);
                    break;
                case infotype.Warning:
                    color = Color.FromRgb(255, 255, 0);
                    break;
            }
            this.lb.Background = new SolidColorBrush(color);
            this.btn.Background = new SolidColorBrush(color);
            this.tb.Text = str1;
            this.btn.Content = str2;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
