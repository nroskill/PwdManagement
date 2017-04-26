using PwdManagement.login;
using PwdManagement.verify;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace PwdManagement
{
    /// <summary>
    /// changePower.xaml 的交互逻辑
    /// </summary>
    public partial class changeValue : Window
    {
        #region 界面控制相关
        public changeValue()
        {
            InitializeComponent();
        }

        private void Window_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #endregion

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (Shell.userInfo.getMethodInfo(checkUser.methodtype.pass))
            {
                new verifyPassword().ShowDialog();
            }
            else
            {
                if (!Shell.userInfo.isNewUser() && Shell.userInfo.power < 80)
                {
                    new ResultWindow(ResultWindow.infotype.Error, "权限不足，您需要满足辨识度不小于80", "返回").ShowDialog();
                    return;
                }
                new setPassword().ShowDialog();
            }
            rwData.writeFile();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(Environment.SystemDirectory + @"\ZKFPCap.dll"))
            {
                new ResultWindow(ResultWindow.infotype.Error, "缺少必备的组件，该功能不可用", "返回").ShowDialog();
                return;
            }
            if (!Shell.userInfo.getMethodInfo(checkUser.methodtype.finger) && !Shell.userInfo.isNewUser() && Shell.userInfo.power < 80)
            {
                new ResultWindow(ResultWindow.infotype.Error, "权限不足，您需要满足辨识度不小于80", "返回").ShowDialog();
                return;
            }
            new verityFingerprint().ShowDialog();
            rwData.writeFile();
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            verifyVoice.mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            var test = verifyVoice.mciSendString("record recsound", "", 0, 0);
            if (!File.Exists(Environment.SystemDirectory + @"\winmm.dll") || test != 0)
            {
                new ResultWindow(ResultWindow.infotype.Error, "缺少必备的组件，该功能不可用", "返回").ShowDialog();
                return;
            }
            verifyVoice.mciSendString("close recsound", "", 0, 0);
            if (!Shell.userInfo.getMethodInfo(checkUser.methodtype.voice) && !Shell.userInfo.isNewUser() && Shell.userInfo.power < 80)
            {
                new ResultWindow(ResultWindow.infotype.Error, "权限不足，您需要满足辨识度不小于80", "返回").ShowDialog();
                return;
            }
            new verifyVoice().ShowDialog();
            rwData.writeFile();
        }
        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\WPFMediaKit.dll"))
            {
                new ResultWindow(ResultWindow.infotype.Error, "缺少必备的组件，该功能不可用", "返回").ShowDialog();
                return;
            }
            if (!Shell.userInfo.getMethodInfo(checkUser.methodtype.face) && !Shell.userInfo.isNewUser() && Shell.userInfo.power < 80)
            {
                new ResultWindow(ResultWindow.infotype.Error, "权限不足，您需要满足辨识度不小于80", "返回").ShowDialog();
                return;
            }
            new verifyFace().ShowDialog();
            rwData.writeFile();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Shell.userInfo.power < ( 100 - Shell.userInfo.passPower) * 0.8 && Shell.userInfo.passValue != 100 )
            {
                new ResultWindow(ResultWindow.infotype.Error, "请先验证原密码或满足辨识度不小于" + ((100 - Shell.userInfo.passPower) * 0.8).ToString() + "%", "返回").ShowDialog();
                return;
            }
            new changePassword().ShowDialog();
            rwData.writeFile();
        }
    }
}
