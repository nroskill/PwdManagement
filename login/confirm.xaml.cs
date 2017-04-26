using System.Windows;
using System.Windows.Input;

namespace PwdManagement.login
{
    /// <summary>
    /// confirm.xaml 的交互逻辑
    /// </summary>
    public partial class confirm : Window
    {
        public static bool result;

        public confirm()
        {
            InitializeComponent();
            result = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            result = true;
            this.Close();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            this.Close();
        }
    }
}
