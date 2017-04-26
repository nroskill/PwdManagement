using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFMediaKit.DirectShow.Controls;
using System.IO;
using System.Drawing;
using PwdManagement.verify;
using PwdManagement.Face;
using PwdManagement.login;

namespace PwdManagement
{
    /// <summary>
    /// verifyFace.xaml 的交互逻辑
    /// </summary>
    public partial class photograph : Window
    {
        private static int num = 0;

        public photograph()
        {
            InitializeComponent();
            if (!Shell.userInfo.getMethodInfo(checkUser.methodtype.face))
            {
                num = 3;
            }
            else
            {
                num = 1;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (MultimediaUtil.VideoInputNames.Length > 0)
            {
                this.vce.VideoCaptureSource = MultimediaUtil.VideoInputNames[0];
            }
            else
            {
                new ResultWindow(ResultWindow.infotype.Error, "未检测到任何摄像头", "返回").ShowDialog();
                this.Close();
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (((string)(btn.Content)) == "拍照")
            {
                vce.Pause();
                btn.Content = "完成";
                btn.Visibility = Visibility.Visible;
                btn2.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                savePic();
                num--;
                if (num == 0)
                {
                    if (Shell.userInfo.getMethodInfo(checkUser.methodtype.face))
                    {
                        Shell.userInfo.checkData[3] = face.faceStart(Shell.userInfo.checkData[3], verifyFace.bm[0]);
                        verifyFace.tempdata = (int)(face.credit + 0.5);
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("由于是初次设定，您还需要拍照" + num.ToString() + "次");
                    btn.Content = "拍照";
                    btn2.Visibility = Visibility.Collapsed;
                    vce.Play();
                }
            }
        }

        private void savePic()
        {
            var bmp = new RenderTargetBitmap((int)vce.ActualWidth, (int)vce.ActualHeight, 96, 96, PixelFormats.Default);
            bmp.Render(vce);
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            var ms = new MemoryStream();
            encoder.Save(ms);
            verifyFace.bm.Add(new Bitmap(ms));
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            btn.Content = "拍照";
            btn2.Visibility = Visibility.Collapsed;
            vce.Play();
        }
    }
}
