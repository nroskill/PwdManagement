using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PwdManagement.Voice;
using System.Runtime.InteropServices;
using PwdManagement.login;
using System.IO;

namespace PwdManagement.verify
{
    /// <summary>
    /// verifyVoice.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class verifyVoice : Window
    {
        private static int tempdata;
        private static int num;
        private static List<byte[]> data;

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        public verifyVoice()
        {
            InitializeComponent();
            tempdata = -1;
            data = new List<byte[]>();
            if(Shell.userInfo.getMethodInfo(checkUser.methodtype.voice))
            {
                num = 1;
            }
            else
            {
                num = 5;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as Button;
            if(bt.Content.ToString() == "录制")
            {
                mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
                mciSendString("record recsound", "", 0, 0);
                bt.Content = "停止录制";
            }
            else if(bt.Content.ToString() == "停止录制")
            {
                var currentWav = Guid.NewGuid().ToString() + ".wav";
                var currentTxt = Guid.NewGuid().ToString() + ".txt";
                mciSendString("save recsound " + currentWav, "", 0, 0);
                mciSendString("close recsound", "", 0, 0);
                num--;

                if (num > 0)
                {
                    string[] tempArray1 = { currentWav };
                    string[] tempArray2 = { currentTxt };
                    Register.register(tempArray1, tempArray2);
                    data.Add(File.ReadAllBytes(currentTxt));
                    File.Delete(currentTxt);
                    File.Delete(currentWav);
                    bt.Content = "录制";
                    new ResultWindow(ResultWindow.infotype.Success, "由于是设定模式，您需要再录制" + num.ToString() + "次", "返回").ShowDialog();
                }
                else
                {
                    //验证模式
                    if (Shell.userInfo.getMethodInfo(checkUser.methodtype.voice))
                    {
                        var historydata = rwData.convertToListForVoice(Shell.userInfo.checkData[2]);
                        var txtName = new List<string>();
                        for (var i = 0; i < historydata.Count; i++)
                        {
                            txtName.Add(Guid.NewGuid().ToString() + ".txt");
                            File.WriteAllBytes(txtName[i], historydata[i]);
                        }
                        tempdata = (int)(Test.matchingDegree(txtName.ToArray(), currentWav, currentTxt) + 0.5);
                        foreach (var i in txtName)
                        {
                            File.Delete(i);
                        }
                        this.lb.Content = tempdata.ToString();
                    }
                    bt.Content = "重新录制";
                    File.Delete(currentTxt);
                    File.Delete(currentWav);
                }
            }
            else
            {
                data.Clear();
                mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
                mciSendString("record recsound", "", 0, 0);
                bt.Content = "停止录制";
                if (Shell.userInfo.getMethodInfo(checkUser.methodtype.voice))
                {
                    num = 1;
                }
                else
                {
                    num = 5;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mciSendString("close recsound", "", 0, 0);
            if (num > 0)
            {
                this.Close();
                return;
            }
            if (!Shell.userInfo.getMethodInfo(checkUser.methodtype.voice))
            {
                Shell.userInfo.setMethoInfo(checkUser.methodtype.voice, true);
                Shell.userInfo.checkData[2] = rwData.convertToByteForVoice(data);
            }
            if (tempdata != -1)
                Shell.userInfo.voiceValue = tempdata;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mciSendString("close recsound", "", 0, 0);
            this.Close();
        }

        private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
