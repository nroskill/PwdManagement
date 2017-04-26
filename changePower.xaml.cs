using System.Windows;
using System.Windows.Input;
using PwdManagement.login;

namespace PwdManagement
{
    /// <summary>
    /// changePower.xaml 的交互逻辑
    /// </summary>
    public partial class changePower : Window
    {
        public changePower()
        {
            InitializeComponent();
            this.text1.Text = Shell.userInfo.passPower.ToString();
            this.text2.Text = Shell.userInfo.fingerPower.ToString();
            this.text3.Text = Shell.userInfo.voicePower.ToString();
            this.text4.Text = Shell.userInfo.facePower.ToString();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            int a = 0;
            if (!(int.TryParse(this.text1.Text, out a) && int.TryParse(this.text2.Text, out a) && int.TryParse(this.text3.Text, out a) && int.TryParse(this.text4.Text, out a)))
            {
                new ResultWindow(ResultWindow.infotype.Error, "存在非数字字符", "返回").ShowDialog();
                return;
            }
            int pass, finger, voice, face;
            pass = int.Parse(this.text1.Text);
            finger = int.Parse(this.text2.Text);
            voice = int.Parse(this.text3.Text);
            face = int.Parse(this.text4.Text);
            if(pass + finger + voice + face != 100)
            {
                new ResultWindow(ResultWindow.infotype.Error, "比例和不等于100", "返回").ShowDialog();
                return;
            }
            if (pass < 0 || pass > 100 || finger < 0 || finger > 100 || voice < 0 || voice > 100 || face < 0 || face > 100)
            {
                new ResultWindow(ResultWindow.infotype.Error, "比例值错误", "返回").ShowDialog();
                return;
            }
            Shell.userInfo.passPower = 0;
            Shell.userInfo.fingerPower = 0;
            Shell.userInfo.facePower = 0;
            Shell.userInfo.voicePower = 0;
            Shell.userInfo.passPower = int.Parse(this.text1.Text);
            Shell.userInfo.fingerPower = int.Parse(this.text2.Text);
            Shell.userInfo.voicePower = int.Parse(this.text3.Text);
            Shell.userInfo.facePower = int.Parse(this.text4.Text);
            this.Close();
        }
    }
}
