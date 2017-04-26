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
using System.Drawing;
using System.IO;
using PwdManagement;
using PwdManagement.Face;

namespace PwdManagement.verify
{
    /// <summary>
    /// verifyFace.xaml 的交互逻辑
    /// </summary>
    public partial class verifyFace : Window
    {
        public static int tempdata;
        public static List<Bitmap> bm;
        public verifyFace()
        {
            InitializeComponent();
            tempdata = -1;
            bm = new List<Bitmap>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bm.Clear();
            var window = new photograph();
            window.ShowDialog();
            this.btn.Content = "重新拍照";
            if(bm.Count != 0)
                this.pic.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bm[0].GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            if(tempdata != -1)
            {
                this.lb.Content = tempdata.ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if( bm.Count == 0)
            {
                this.Close();
                return;
            }
            if (!Shell.userInfo.getMethodInfo(checkUser.methodtype.face)) 
            {
                Shell.userInfo.setMethoInfo(checkUser.methodtype.face, true);
                Shell.userInfo.checkData[3] = face.initLib(bm.ToArray());
                Shell.userInfo.userFace = face.clip(bm[0], 250, 50, 300, 300);
            }
            if (tempdata != -1 )
                Shell.userInfo.faceValue = tempdata;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
