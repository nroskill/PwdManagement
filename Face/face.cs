using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace PwdManagement.Face
{
    public class face
    {
        #region 属性

        // 初始化该用户人脸数
        private static int numOfFace = 0;
        private static int numOfTest = 0;
        private static int numOfTrain = 0;
        private static int col = 100;
        private static int row = 100;
        private static int invl = 10;
        public static double credit = 0;

        #endregion

        #region 方法
        private static byte[] writeInFile(double[, ,] bmpData)
        {
            var result = new byte[bmpData.Length + 4 * 2];
            Buffer.BlockCopy(format(numOfTest), 0, result, 0, 4);
            Buffer.BlockCopy(format(bmpData.GetLength(2)), 0, result, 4, 4);
            for (var i = 0; i < bmpData.GetLength(0); i++)
                for (var j = 0; j < bmpData.GetLength(1); j++)
                    for (var k = 0; k < bmpData.GetLength(2); k++)
                    {
                        var temp = formatByte(bmpData[i, j, k]);
                        Buffer.BlockCopy(temp, 0, result, 4 * 2 + (i * bmpData.GetLength(1) * bmpData.GetLength(2) + j * bmpData.GetLength(2) + k), 1);
                    }
            return result;
        }
        private static byte[] formatByte(double a)
        {
            var tmp = new byte[1];
            tmp[0] = Convert.ToByte(a.ToString("f0"));
            return tmp;
        }
        private static byte[] format(int a)
        {
            return BitConverter.GetBytes(a);
        }
        private static int getint(byte[] a)
        {
            return BitConverter.ToInt32(a, 0);
        }
        private static double getByte2Double(byte[] a)
        {
            return Convert.ToDouble(a[0].ToString());
        }
        private static double[, ,] readFromFile(byte[] Data)
        {
            var temp = new byte[4];
            Buffer.BlockCopy(Data, 0, temp, 0, 4);
            numOfTest = getint(temp);
            Array.Clear(temp, 0, 4);
            Buffer.BlockCopy(Data, 4, temp, 0, 4);
            var num = getint(temp);
            var bmpData = new double[100, 100, num];
            temp = new byte[1];
            for (var i = 0; i < 100; i++)
                for (var j = 0; j < 100; j++)
                    for (var k = 0; k < num; k++)
                    {
                        Buffer.BlockCopy(Data, 4 * 2 + (i * 100 * num + j * num + k), temp, 0, 1);
                        bmpData[i, j, k] = getByte2Double(temp);
                    }
            return bmpData;
        }
        private static double[, ,] getTrainLib()
        {
            // 读入至内存
            var Data = File.ReadAllBytes("TrainLib");
            // 进行处理
            var bmpData = readFromFile(Data);
            return bmpData;
        }
        /// <summary>
        /// 求一个像素点的灰度值
        /// </summary>
        /// <param name="cp">关于RGB颜色的数组 通过GetPixel方法调用</param>
        /// <returns>返回一个灰度值</returns>
        private static double rgb2Grey(Color cp)
        {
            return (int)((0.3 * cp.R + 0.59 * cp.G + 0.11 * cp.B));
        }
        /// <summary>
        /// 初始化人脸识别系统
        /// 初始化训练集并将人脸置入至系统
        /// </summary>
        /// <param name="Data">连拍的人脸合集</param>
        /// <returns>总数据集</returns>
        public static byte[] initLib(Bitmap[] Data)
        {
            // 获得训练集
            var TrainLib = getTrainLib();
            numOfTrain = TrainLib.GetLength(2);
            numOfTest = Data.Length;
            numOfFace = numOfTest + numOfTrain;

            // 进行拼接至完整训练集
            var re = new double[row, col, numOfFace];
            for (int i = 0; i < numOfFace; i++)
            {
                // 本人人脸
                if (i < numOfTest)
                {
                    var tmp = faceUniform(Data[i]);
                    for (int j = 0; j < row; j++)
                        for (int k = 0; k < col; k++)
                            re[j, k, i] = tmp[j, k];
                }
                // 非本人数据集
                else
                    for (int j = 0; j < row; j++)
                        for (int k = 0; k < col; k++)
                            re[j, k, i] = TrainLib[j, k, i - numOfTest];
            }

            var result = writeInFile(re);
            return result;
        }
        /// <summary>
        /// 将一张图片进行归一化处理，裁剪至Row*Col大小的人脸图像
        /// </summary>
        /// <param name="vf">一张人脸图像</param>
        /// <returns></returns>
        private static double[,] faceUniform(Bitmap vf)
        {
            // 剪裁中心区域的人脸图像
            var t2 = clip(vf, 300, 100, 200, 200);
            // 对该图像进行缩放
            var t1 = reshape(t2, 100, 100, 0);
            // 将verify face转化为普通数组，并进行灰度处理
            var vf_img = new double[row, col];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                    vf_img[i, j] = rgb2Grey(t1.GetPixel(j, i));
            return vf_img;
        }
        /// <summary>
        /// 人脸识别入口函数
        /// </summary>
        /// <param name="Data">总人脸集合</param>
        /// <param name="vf">验证照片</param>
        /// <returns>新的人脸集合</returns>
        public static byte[] faceStart(byte[] Data, Bitmap vf)
        {
            var bmpData = readFromFile(Data);
            var NewFace = faceProcess(bmpData, vf, numOfTest);
            var result = writeInFile(NewFace);
            return result;
        }
        /// <summary>
        /// 进行人脸图像处理函数
        /// </summary>
        /// <param name="ImgLib">前People张为该人脸,后面都为非人脸图像</param>
        /// <param name="vf"></param>
        /// <param name="People"></param>
        /// <returns></returns>
        private static double[, ,] faceProcess(double[, ,] ImgLib, Bitmap vf, int People)
        {
            // 获取本人人脸数目
            numOfTest = People;

            // 获取图片数量
            numOfFace = ImgLib.GetLength(2);

            // 对其进行面部归一化处理
            var vf_img = faceUniform(vf);

            // 构造字典
            // 构造字典数组
            var ir = (row + invl - 1) / invl;
            var ic = (col + invl - 1) / invl;
            var Dic = new double[ir * ic, numOfFace];

            // 提取字典信息
            // 隔invl列提取一列, 隔invl行提取一行
            for (int i = 0; i < numOfFace; i++)
                for (int j = 0; j < ic; j++)
                    for (int k = 0; k < ir; k++)
                        Dic[j * ir + k, i] = ImgLib[k * invl, j * invl, i];

            // 提取人脸测试照片信息
            // 隔invl列提取一列, 隔invl行提取一行
            var vf_face = new double[ir * ic, 1];
            for (int j = 0; j < ic; j++)
                for (int k = 0; k < ir; k++)
                    vf_face[j * ir + k, 0] = vf_img[k * invl, j * invl];

            // 字典对角化
            // t1 为 [col,col]大小的矩阵
            // Dic 为 [row*col,numOfFace]大小的矩阵
            var t1 = Matrix.multi(Matrix.trans(Dic, ir * ic, numOfFace), numOfFace, ir * ic, Dic, ir * ic, numOfFace);
            var t2 = Matrix.sqrt(Matrix.diag(t1, numOfFace), numOfFace, numOfFace);
            Dic = Matrix.multi(Dic, ir * ic, numOfFace, Matrix.inv(t2, numOfFace), numOfFace, numOfFace);

            // 进行SRC求解
            // 设定参数
            double e = 0.05;  // 设定阀值
            var r = vf_face; // 图像残差
            int max_iter = 100; // 设定最大迭代次数
            var x = new double[numOfFace, 1];  // 编码系数
            var y = new double[numOfFace, 1];  // 乘子
            var z = new double[numOfFace, 1];  // 分离系数
            var reconst = new double[ir * ic, 2]; // 重构人脸


            // 进行迭代求解L1范数s
            var r_er = Math.Sqrt(Matrix.sum(Matrix.pointMulti(r, r, ir * ic, 1), ir * ic, 1));
            // Matrix.print(t3, ir*ic,1);
            for (int iter = 0; iter < max_iter && r_er > e; iter++)
            {
                var s1 = Matrix.ATA(Dic, ir * ic, numOfFace);
                var s2 = Matrix.plus(s1, Matrix.eye(numOfFace), numOfFace, numOfFace);

                var m1 = Matrix.multi(Matrix.trans(Dic, ir * ic, numOfFace), numOfFace, ir * ic, vf_face, ir * ic, 1);
                var m2 = Matrix.plus(m1, z, numOfFace, 1);
                var m3 = Matrix.multiOne(y, numOfFace, 1, -1);
                var m4 = Matrix.plus(m2, m3, numOfFace, 1);

                x = Matrix.multi(Matrix.inv(s2, numOfFace), numOfFace, numOfFace, m4, numOfFace, 1);

                var p1 = Matrix.plus(x, y, numOfFace, 1);
                var p2 = Matrix.plusOne(p1, numOfFace, 1, -1);
                p2 = Matrix.minLimit(p2, numOfFace, 1, 0);
                var p3 = Matrix.plus(x, y, numOfFace, 1);
                var p4 = Matrix.multiOne(Matrix.plusOne(p3, numOfFace, 1, 1), numOfFace, 1, -1);
                p4 = Matrix.minLimit(p4, numOfFace, 1, 0);

                z = Matrix.sub(p2, p4, numOfFace, 1);
                y = Matrix.plus(y, Matrix.sub(x, z, numOfFace, 1), numOfFace, 1);
                r = Matrix.sub(vf_face, Matrix.multi(Dic, ir * ic, numOfFace, x, numOfFace, 1), ir * ic, 1);
            }

            // 进行稀疏重构
            var coeff = x;
            var error = new double[1, 2];

            // 对人脸进行匹配
            for (int i = 0; i < numOfTest; i++)
                for (int j = 0; j < ir * ic; j++)
                    reconst[j, 0] += Dic[j, i] * coeff[i, 0];
            // 对人脸进行误差计算
            var k1 = Matrix.sub(vf_face, reconst, ir * ic, 1);
            error[0, 0] = Math.Sqrt(Matrix.sum(Matrix.pointMulti(k1, k1, ir * ic, 1), ir * ic, 1));

            // 对非人脸进行匹配
            for (int i = numOfTest; i < numOfFace; i++)
                for (int j = 0; j < ir * ic; j++)
                    reconst[j, 1] += Dic[j, i] * coeff[i, 0];
            // 对非人脸进行误差计算
            var k2 = new double[ir * ic, 1];
            for (int i = 0; i < ir * ic; i++)
                k2[i, 0] = vf_face[i, 0] - reconst[i, 1];
            error[0, 1] = Math.Sqrt(Matrix.sum(Matrix.pointMulti(k2, k2, ir * ic, 1), ir * ic, 1));

            // 计算辨识度
            creditFace(error);

            if (credit > 90)
            {
                // 如果确认是本人则将图像保存至本人集合
                var NewLib = new double[row, col, numOfFace + 1];
                for (int k = 0; k < numOfFace + 1; k++)
                    for (int i = 0; i < row; i++)
                        for (int j = 0; j < col; j++)
                            if (k < numOfTest)
                                NewLib[i, j, k] = ImgLib[i, j, k];
                            else if (k == numOfTest)
                                NewLib[i, j, k] = vf_img[i, j];
                            else
                                NewLib[i, j, k] = ImgLib[i, j, k - 1];
                numOfTest++;
                return NewLib;
            }
            else
                return ImgLib;
        }
        private static void creditFace(double[,] error)
        {
            var max = Matrix.max(error, 1, 2);
            if (max == error[0, 0]) credit = 0;
            else
            {
                credit = Math.Log10(max - error[0, 0]) / Math.Log10(max);
                credit = credit * credit * credit;
                credit *= 100;
            }
        }
        /// <summary>
        ///  Resize图片 
        /// </summary>
        /// <param name="bmp">原始Bitmap </param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <param name="Mode">保留着，暂时未用</param>
        /// <returns>处理以后的图片</returns>

        private static Bitmap reshape(Bitmap bmp, int newW, int newH, int Mode)
        {
            Bitmap re = new Bitmap(newW, newH);
            Graphics g = Graphics.FromImage(re);
            // 插值算法的质量 
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
            g.Dispose();
            return re;

        }
        /// <summary>
        /// 剪裁
        /// </summary>
        /// <param name="vf">原始Bitmap</param>
        /// <param name="StartX">开始坐标X</param>
        /// <param name="StartY">开始坐标Y</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns>剪裁后的Bitmap</returns>
        public static Bitmap clip(Bitmap vf, int StartX, int StartY, int iWidth, int iHeight)
        {
            // 从指定的点开始裁剪至指定比例
            var re = new Bitmap(iHeight, iWidth);
            for (int i = 0; i < iWidth; i++)
                for (int j = 0; j < iWidth; j++)
                {
                    var px = i + StartX;
                    var py = j + StartY;
                    Color p = vf.GetPixel(px, py);
                    re.SetPixel(i, j, p);
                }
            return re;
        }
    }

        #endregion
}
