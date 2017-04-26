using System.Windows;
using PwdManagement.Finger;
using System;
using System.IO;
using System.Drawing;
using PwdManagement.login;
using System.Threading;
using System.Collections.Generic;

namespace PwdManagement
{
    /// <summary>
    /// verityFingerprint.xaml 的交互逻辑
    /// </summary>
    public partial class verityFingerprint : Window
    {
        private static int num;
        private static int tempdata;
        private static IntPtr devicehandle;
        private static int bmp_width;
        private static int bmp_height;
        private static byte[] bmp_buffer;
        private static Thread getBmp;
        private static bool quit;
        private static bool init;
        private static List<Bitmap> data;
        public verityFingerprint()
        {
            InitializeComponent();
            
            devicehandle = IntPtr.Zero;
            init = false;
            quit = false;
            tempdata = -1;
            data = new List<Bitmap>();
            if (!Shell.userInfo.getMethodInfo(checkUser.methodtype.finger))
            {
                num = 1;
            }
            else
            {
                num = 1;
            }

            getBmp = new Thread(new ThreadStart(DoCapture));
            getBmp.IsBackground = true;
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if(init)
            {
                return;
            }
            var ret = ZKFPCap.sensorInit();
            if (ret != 0)
            {
                var x = new ResultWindow(ResultWindow.infotype.Error, "指纹设备连接失败", "返回");
                x.ShowDialog();
                ZKFPCap.sensorFree();
                this.Close();
                return;
            }
            devicehandle = ZKFPCap.sensorOpen(0);
            if ((int)devicehandle <= 0)
            {
                var x = new ResultWindow(ResultWindow.infotype.Error, "指纹设备连接失败", "返回");
                x.ShowDialog();
                ZKFPCap.sensorClose(devicehandle);
                ZKFPCap.sensorFree();
                this.Close();
                return;
            }
            this.btn.Dispatcher.Invoke(
                new Action(
                    delegate 
                    {
                        this.btn.Content = "请录入指纹...";
                    }
                )
            );
            bmp_width = ZKFPCap.sensorGetParameter(devicehandle, 1);
            bmp_height = ZKFPCap.sensorGetParameter(devicehandle, 2);
            bmp_buffer = new byte[ZKFPCap.sensorGetParameter(devicehandle, 106)];
            getBmp.Start();
            init = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            quit = true;
            try
            {
                getBmp.Join();
            }
            catch
            {
            }
            if (data.Count == 0)
            {
                this.Close();
                return;
            }
            if (!Shell.userInfo.getMethodInfo(checkUser.methodtype.finger))
            {
                Shell.userInfo.setMethoInfo(checkUser.methodtype.finger, true);
                Shell.userInfo.checkData[3] = finger.register(data.ToArray());
            }
            if (tempdata != -1)
            {
                Shell.userInfo.fingerValue = tempdata;
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            quit = true; 
            try
            {
                getBmp.Join();
            }
            catch
            {
            }
            this.Close();
        }

        private void DoCapture()
        {
            while (!quit)
            {
                int ret = ZKFPCap.sensorCapture(devicehandle, bmp_buffer, bmp_buffer == null ? 0 : bmp_buffer.Length);
                if (ret > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    ZKFPCap.GetBitmap(bmp_buffer, bmp_width, bmp_height, ref ms);
                    data.Add(new Bitmap(ms));
                    num--;
                    if (num > 0)
                    {
                        this.lb.Dispatcher.Invoke(
                            new Action(
                                delegate
                                {
                                    new ResultWindow(ResultWindow.infotype.Success, "成功录入指纹，您还需要录入" + num.ToString() + "次", "返回").ShowDialog();
                                }
                            )
                        );
                    }
                    else
                    {
                        if (Shell.userInfo.getMethodInfo(checkUser.methodtype.finger))
                        {
                            tempdata = (int)(finger.verify(Shell.userInfo.checkData[3], data[data.Count - 1]) + 0.5);
                            if (tempdata != -1)
                            {
                                this.lb.Dispatcher.Invoke(
                                    new Action(
                                        delegate
                                        {
                                            this.lb.Content = tempdata.ToString();
                                        }
                                    )
                                );
                            }
                        }
                        else
                        {
                            this.lb.Dispatcher.Invoke(
                                new Action(
                                    delegate
                                    {
                                        new ResultWindow(ResultWindow.infotype.Success, "注册完成，您可以继续录入以覆盖注册信息，或点击确认完成注册", "返回").ShowDialog();
                                    }
                                )
                            );
                        }
                    }
                    if(num < 0)
                    {
                        data.RemoveAt(0);
                    }
                }
            }
            ZKFPCap.sensorClose(devicehandle);
            ZKFPCap.sensorFree();
        }

        private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
