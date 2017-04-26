using System;
using System.Drawing;
using System.Collections.Generic;

namespace PwdManagement.Finger
{
    public class finger
    {
        #region 属性

        const int m = 300;
        const int n = 300;

        #endregion

        #region 接口
        static public byte[] register(Bitmap[] data)
        {
            // 抽取第一张图片
            var fig_self = data[0];
            // 保存图片
            var saveImg = new double[300, 300];
            for (int i = 0; i < 300; i++)
                for (int j = 0; j < 300; j++)
                    saveImg[i, j] = rgb2Grey(fig_self.GetPixel(j, i));
            // 保存数组至byte[]
            var result = new byte[saveImg.Length];
            for (var i = 0; i < 300; i++)
                for (var j = 0; j < 300; j++)
                {
                    var temp = formatByte(saveImg[i, j]);
                    Buffer.BlockCopy(temp, 0, result, (i * 300 + j), 1);
                }
            return result;
        }

        static public double verify(byte[] Data, Bitmap fig_vf)
        {
            // 从byte[]字节流中取出内容
            var bmpData = new double[300, 300];
            var temp = new byte[1];
            for (var i = 0; i < 300; i++)
                for (var j = 0; j < 300; j++)
                {
                    Buffer.BlockCopy(Data, (i * 300 + j), temp, 0, 1);
                    bmpData[i, j] = getByte2Double(temp);
                }
            // 进行注册
            var true_fig = reg(bmpData);
            // 进行验证
            var sim = vertify(true_fig, fig_vf);
            sim *= 100;
            return sim;
        }

        #endregion

        #region 类型转换

        /// <summary>
        /// double to byte[]
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static byte[] formatByte(double a)
        {
            var tmp = new byte[1];
            tmp[0] = Convert.ToByte(a.ToString("f0"));
            return tmp;
        }
        private static double getByte2Double(byte[] a)
        {
            return Convert.ToDouble(a[0].ToString());
        }

        #endregion

        #region 指纹识别部分

        private static double rgb2Grey(Color cp)
        {
            return (int)((0.29900 * cp.R + 0.58700 * cp.G + 0.11400 * cp.B) + 0.5);
        }//图像二值化，在主函数运用
        /**********************************************************************
         * 基础代码部分结束
         */
        /**********************************************************************
         * 指纹识别实际区域
         */
        private static double[,] ImSkeleton(double[,] Im)
        {
            int m = 0;
            int n = 0;
            m = Im.GetLength(0);
            n = Im.GetLength(1);

            //删除点
            for (int nnn = 0; nnn < 7; nnn++)
            {
                double[,] Imd = new double[m, n];
                Matrix.clear(Imd);
                //第一步
                for (int i = 1; i < m - 1; i++)
                {
                    for (int j = 1; j < n - 1; j++)
                        if (Im[i, j] == 1)
                        {
                            double Np1 = Im[i - 1, j] + Im[i - 1, j + 1] + Im[i, j + 1] + Im[i + 1, j + 1] + Im[i + 1, j] + Im[i + 1, j - 1] + Im[i, j - 1] + Im[i - 1, j - 1];
                            double a = 0;
                            if (Np1 >= 2 && Np1 <= 6)
                            {
                                a = 0;
                                if (Im[i - 1, j] != Im[i - 1, j + 1])
                                    a = a + 0.5;
                                if (Im[i - 1, j + 1] != Im[i, j + 1])
                                    a = a + 0.5;
                                if (Im[i, j + 1] != Im[i + 1, j + 1])
                                    a = a + 0.5;
                                if (Im[i + 1, j + 1] != Im[i + 1, j])
                                    a = a + 0.5;
                                if (Im[i + 1, j] != Im[i + 1, j - 1])
                                    a = a + 0.5;
                                if (Im[i + 1, j - 1] != Im[i, j - 1])
                                    a = a + 0.5;
                                if (Im[i, j - 1] != Im[i - 1, j - 1])
                                    a = a + 0.5;
                                if (Im[i - 1, j - 1] != Im[i - 1, j])
                                    a = a + 0.5;
                                if (a == 1 && (Im[i - 1, j] * Im[i, j + 1] * Im[i + 1, j]) == 0 && (Im[i, j + 1] * Im[i + 1, j] * Im[i, j - 1]) == 0)
                                    Imd[i, j] = 1;
                            }
                        }
                }

                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        Im[i, j] = Im[i, j] - Imd[i, j];

                //第二部
                Matrix.clear(Imd);
                for (int i = 1; i < m - 1; i++)
                {
                    for (int j = 1; j < n - 1; j++)
                        if (Im[i, j] == 1)
                        {
                            double Np1 = Im[i - 1, j] + Im[i - 1, j + 1] + Im[i, j + 1] + Im[i + 1, j + 1] + Im[i + 1, j] + Im[i + 1, j - 1] + Im[i, j - 1] + Im[i - 1, j - 1];
                            if (Np1 >= 2 && Np1 <= 6)
                            {
                                double a = 0;
                                if (Im[i - 1, j] != Im[i - 1, j + 1])
                                    a = a + 0.5;
                                if (Im[i - 1, j + 1] != Im[i, j + 1])
                                    a = a + 0.5;
                                if (Im[i, j + 1] != Im[i + 1, j + 1])
                                    a = a + 0.5;
                                if (Im[i + 1, j + 1] != Im[i + 1, j])
                                    a = a + 0.5;
                                if (Im[i + 1, j] != Im[i + 1, j - 1])
                                    a = a + 0.5;
                                if (Im[i + 1, j - 1] != Im[i, j - 1])
                                    a = a + 0.5;
                                if (Im[i, j - 1] != Im[i - 1, j - 1])
                                    a = a + 0.5;
                                if (Im[i - 1, j - 1] != Im[i - 1, j])
                                    a = a + 0.5;
                                if (a == 1 && (Im[i - 1, j] * Im[i, j + 1] * Im[i, j + 1]) == 0
                                    && (Im[i - 1, j] * Im[i + 1, j] * Im[i, j - 1]) == 0)
                                    Imd[i, j] = 1;
                            }
                        }
                }
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        Im[i, j] = Im[i, j] - Imd[i, j];
            }
            return Im;
        }//进行图像框架处理，在distance进行运用

        //输入的vector1,vector2都为二维数组，代表的是待识别指纹的分叉点和端点的坐标，vector3vector4是测试指纹样本的分叉点和端点坐标，最后返回的是相似度
        public static double vertify(double[,] vector_one, Bitmap v2)
        {
            //--------------------------------------------
            var vector = new double[300, 300];
            for (int i = 0; i < 300; i++)
                for (int j = 0; j < 300; j++)
                    vector[i, j] = rgb2Grey(v2.GetPixel(j, i));
            //--------------------------------------------
            var vector_two = reg(vector);//将图片“ydy2”进行特征点的求解

            //vector1为注册时的指纹特征点向量，vector1为验证时的指纹特征点向量，
            int size1 = 0;//样本的分叉点数目
            int size2 = 0;//样本的端点数目

            for (int i = 0; i < vector_one.GetLength(0); i++)
            {
                if (vector_one[i, 2] == 1)
                    size1++;
                if (vector_one[i, 2] == 2)
                    size2++;
            }
            int size3 = 0;//待测指纹的分叉点数目
            int size4 = 0;//待测指纹的端点数目
            for (int i = 0; i < vector_two.GetLength(0); i++)
            {
                if (vector_two[i, 2] == 1)
                    size3++;
                if (vector_two[i, 2] == 2)
                    size4++;
            }

            double[,] vector1 = new double[size1, 2];////样本的分叉点坐标
            double[,] vector2 = new double[size2, 2];//样本的端点坐标
            double[,] vector3 = new double[size3, 2];//待测的分叉点坐标
            double[,] vector4 = new double[size4, 2];//待测指纹的端点坐标
            double sim11 = 0;

            //将坐标赋值到各个向量中
            for (int i = 0; i < size1; i++)
            {
                vector1[i, 0] = vector_one[i, 0];
                vector1[i, 1] = vector_one[i, 1];
            }
            for (int i = size1; i < size2 + size1; i++)
            {
                vector2[i - size1, 0] = vector_one[i - size1, 0];
                vector2[i - size1, 1] = vector_one[i - size1, 1];
            }
            for (int i = 0; i < size3; i++)
            {
                vector3[i, 0] = vector_two[i, 0];
                vector3[i, 1] = vector_two[i, 1];
            }
            for (int i = size3; i < size3 + size4; i++)
            {
                vector4[i - size3, 0] = vector_two[i - size3, 0];
                vector4[i - size3, 1] = vector_two[i - size3, 1];
            }

            double[,] aver1 = new double[1, 2];
            double[,] aver2 = new double[1, 2];


            //计算待识别特征点的中心平均点
            for (int i = 0; i < size1; i++)
            {
                aver1[0, 0] = vector1[i, 0] + aver1[0, 0];
                aver1[0, 1] = vector1[i, 1] + aver1[0, 1];
            }
            for (int i = 0; i < size2; i++)
            {
                aver1[0, 0] = vector2[i, 0] + aver1[0, 0];
                aver1[0, 1] = vector2[i, 1] + aver1[0, 1];
            }
            aver1[0, 0] = aver1[0, 0] / (size1 + size3);
            aver1[0, 1] = aver1[0, 0] / (size1 + size3);

            //计算样本特征点的中心平均点
            for (int i = 0; i < size3; i++)
            {
                aver2[0, 0] = vector3[i, 0] + aver2[0, 0];
                aver2[0, 1] = vector3[i, 1] + aver2[0, 1];
            }
            for (int i = 0; i < size4; i++)
            {
                aver2[0, 0] = vector4[i, 0] + aver2[0, 0];
                aver2[0, 1] = vector4[i, 1] + aver2[0, 1];
            }
            aver2[0, 0] = aver2[0, 0] / (size3 + size4);
            aver2[0, 1] = aver2[0, 0] / (size3 + size4);



            double sum = 0;
            double sum1 = 0;
            double sum2 = 0;
            double sim = 0;
            double sim1 = 0;
            double sim2 = 0;
            if (size1 >= size3)
            {
                double[,] vector5 = new double[size3, 1];
                double[,] vector6 = new double[size3, 1];
                int i;
                for (i = 0; i < size3; i++)
                {
                    vector5[i, 0] = Math.Pow((aver1[0, 0] - vector1[i, 0]), 2) + Math.Pow((aver1[0, 1] - vector1[i, 1]), 2);
                    vector5[i, 0] = Math.Sqrt(vector5[i, 0]);//到各特征点的距离
                    vector6[i, 0] = Math.Pow((aver2[0, 0] - vector3[i, 0]), 2) + Math.Pow((aver2[0, 1] - vector3[i, 1]), 2);
                    vector6[i, 0] = Math.Sqrt(vector6[i, 0]);
                }
                i = size3 - 1;
                for (int j = 0; j < size3; j++)
                {
                    sum = sum + vector5[i, 0] * vector6[i, 0];
                    sum1 = sum1 + vector5[i, 0] * vector5[i, 0];
                    sum2 = sum2 + vector6[i, 0] * vector6[i, 0];
                }
                sim1 = Math.Sqrt(sum1);
                sim2 = Math.Sqrt(sum2);
                sim11 = sum / (sim1 * sim2);
            }

            if (size1 < size3)
            {
                double[,] vector5 = new double[size1, 1];
                double[,] vector6 = new double[size1, 1];
                int i;
                for (i = 0; i < size1; i++)
                {
                    vector5[i, 0] = Math.Pow((aver1[0, 0] - vector1[i, 0]), 2) + Math.Pow((aver1[0, 1] - vector1[i, 1]), 2);
                    vector5[i, 0] = Math.Sqrt(vector5[i, 0]);//到各特征点的距离
                    vector6[i, 0] = Math.Pow((aver2[0, 0] - vector3[i, 0]), 2) + Math.Pow((aver2[0, 1] - vector3[i, 1]), 2);
                    vector6[i, 0] = Math.Sqrt(vector6[i, 0]);
                }
                i = size1 - 1;
                for (int j = 0; j < size1; j++)
                {
                    sum = sum + vector5[i, 0] * vector6[i, 0];
                    sum1 = sum1 + vector5[i, 0] * vector5[i, 0];
                    sum2 = sum2 + vector6[i, 0] * vector6[i, 0];
                }
                sim1 = Math.Sqrt(sum1);
                sim2 = Math.Sqrt(sum2);
                sim11 = sum / (sim1 * sim2);
            }


            if (size2 >= size4)
            {
                double[,] vector7 = new double[size4, 1];
                double[,] vector8 = new double[size4, 1];
                int i;
                for (i = 0; i < size4; i++)
                {
                    vector7[i, 0] = Math.Pow((aver1[0, 0] - vector2[i, 0]), 2) + Math.Pow((aver1[0, 1] - vector2[i, 1]), 2);
                    vector7[i, 0] = Math.Sqrt(vector7[i, 0]);//到各特征点的距离
                    vector8[i, 0] = Math.Pow((aver2[0, 0] - vector4[i, 0]), 2) + Math.Pow((aver2[0, 1] - vector4[i, 1]), 2);
                    vector8[i, 0] = Math.Sqrt(vector8[i, 0]);
                }
                i = size4 - 1;
                for (int j = 0; j < size4; j++)
                {
                    sum = sum + vector7[i, 0] * vector8[i, 0];
                    sum1 = sum1 + vector7[i, 0] * vector7[i, 0];
                    sum2 = sum2 + vector8[i, 0] * vector8[i, 0];
                }
                sim1 = Math.Sqrt(sum1);
                sim2 = Math.Sqrt(sum2);
                sim11 = sum / (sim1 * sim2);
            }

            if (size2 < size4)
            {
                double[,] vector7 = new double[size2, 1];
                double[,] vector8 = new double[size2, 1];
                int i;
                for (i = 0; i < size2; i++)
                {
                    vector7[i, 0] = Math.Pow((aver1[0, 0] - vector2[i, 0]), 2) + Math.Pow((aver1[0, 1] - vector2[i, 1]), 2);
                    vector7[i, 0] = Math.Sqrt(vector7[i, 0]);//到各特征点的距离
                    vector8[i, 0] = Math.Pow((aver2[0, 0] - vector4[i, 0]), 2) + Math.Pow((aver2[0, 1] - vector4[i, 1]), 2);
                    vector8[i, 0] = Math.Sqrt(vector8[i, 0]);
                }
                i = size2 - 1;
                for (int j = 0; j < size2; j++)
                {
                    sum = sum + vector7[i, 0] * vector8[i, 0];
                    sum1 = sum1 + vector7[i, 0] * vector7[i, 0];
                    sum2 = sum2 + vector8[i, 0] * vector8[i, 0];
                }
                sim1 = Math.Sqrt(sum1);
                sim2 = Math.Sqrt(sum2);
                sim11 = sum / (sim1 * sim2);
            }
            sim11 = sim11 * 100;
            if (sim11 > 95)
                sim11 = (sim11 - 85) / 15;
            else if (sim11 > 90)
                sim11 = (sim11 - 85) / 25;
            else if (sim11 > 85)
                sim11 = (sim11 - 85) / 35;
            else
                sim11 = 0;
            return sim11;
        }

        //将待识别指纹的图像进行处理，得到特征点的坐标，是一个三维数组
        public static double[,] reg(double[,] tmp)
        {
            double M = 0;
            double fc = 0;
            for (int i = 0; i < 300; i++)
                for (int j = 0; j < 300; j++)
                    M = M + tmp[i, j];
            double M1 = M / (300 * 300);//求平均值
            int x;
            int y = 0;
            for (x = 0; x < 300; x++)
                for (y = 0; y < 300; y++)
                    fc = fc + (tmp[x, y] - M1) * (tmp[x, y] - M1);
            double fc1 = fc / (300 * 300); //求方差
            double c = Math.Sqrt((30 * (tmp[x - 1, y - 1] - M1) / fc1));  //标准差
            // Console.WriteLine(c);
            // correct

            for (x = 0; x < 300; x++)
                for (y = 0; y < 300; y++)
                    if (tmp[x, y] >= M1)
                        // @@@ 括号错误
                        tmp[x, y] = 150 + Math.Sqrt((2000 * (tmp[x, y] - M1)) / fc1);
                    else
                        tmp[x, y] = 150 - Math.Sqrt((2000 * (M1 - tmp[x, y])) / fc1);

            // 39

            //分割
            M = 10;
            int H = 300 / (int)M;
            int L = 300 / (int)M;
            double[,] aveg1 = new double[H, L];
            double[,] vaa = new double[H, L];
            for (x = 0; x < H; x++)
                for (y = 0; y < L; y++)
                {
                    double aveg = 0;
                    double va = 0;
                    for (int i = 0; i < (int)M; i++)
                        for (int j = 0; j < M; j++)
                            aveg = tmp[i + x * (int)M, j + y * (int)M] + aveg;
                    aveg1[x, y] = aveg / (M * M);

                    for (int i = 0; i < (int)M; i++)
                        // @@@ 这里需要对一定小的数判定为0
                        for (int j = 0; j < (int)M; j++)
                        {
                            va = (tmp[i + x * (int)M, j + y * (int)M] - aveg1[x, y]) * (tmp[i + x * (int)M, j + y * (int)M] - aveg1[x, y]) + va;
                            if (va < 0.001) va = 0;
                        }
                    vaa[x, y] = va / (M * M);//不知道为什么这的va和MATLAB的数据不一样，但是在这之前的tmp数组和mat中的I是一样的额
                    //对应56 57 行，这是晚上调试的时候发现的，但是很奇怪的是，今天下午调后面的数据尼玛居然是对的
                }

            double Gmean = 0;
            double Vmean = 0;
            for (x = 0; x < H; x++)
                for (y = 0; y < L; y++)
                {
                    Gmean = Gmean + aveg1[x, y];
                    Vmean = Vmean + vaa[x, y];
                }
            double Gmean1 = Gmean / (H * L);
            double Vmean1 = Vmean / (H * L);//这个数据也不对
            // correct - 78
            double gtemp = 0;
            double gtotle = 0;
            double vtotle = 0;
            double vtemp = 0;
            for (x = 0; x < H; x++)
                for (y = 0; y < L; y++)
                {
                    if (Gmean1 > aveg1[x, y])
                    {
                        gtemp = gtemp + 1;
                        gtotle = gtotle + aveg1[x, y];
                    }
                    if (Vmean1 < vaa[x, y]) // @@@ 这里是小于号
                    {
                        vtemp = vtemp + 1;
                        vtotle = vtotle + vaa[x, y];
                    }
                }
            double G1 = gtotle / gtemp;
            double V1 = vtotle / vtemp;
            double gtemp1 = 0;
            double gtotle1 = 0;
            double vtotle1 = 0;
            double vtemp1 = 0;
            for (x = 0; x < H; x++)
                for (y = 0; y < L; y++)
                {
                    if (G1 < aveg1[x, y])
                    {
                        gtemp1 = gtemp1 + 1;
                        gtotle1 = gtotle1 + aveg1[x, y];
                    }
                    // @@@ 这里是&& 而不是&
                    if (vaa[x, y] > 0 && vaa[x, y] < V1)
                    {
                        vtemp1 = vtemp1 + 1;
                        vtotle1 = vtotle1 + vaa[x, y];
                    }
                }
            double G2 = gtotle1 / gtemp1;
            double V2 = vtotle1 / vtemp1;

            double[,] moban = new double[H, L];
            double T1 = G2;
            double T2 = V2;
            double T3 = G1 - 100;
            double T4 = V2 - 10;
            for (x = 0; x < H; x++)
                for (y = 0; y < L; y++)
                {
                    if (aveg1[x, y] > T1 & vaa[x, y] < T2)
                        moban[x, y] = 1;
                    if (aveg1[x, y] < T3 & vaa[x, y] < T2)
                        moban[x, y] = 1;
                }
            for (x = 1; x < H - 1; x++)
                for (y = 1; y < L - 1; y++)
                {
                    if (moban[x, y] == 1)
                        if ((moban[x - 1, y] + moban[x - 1, y + 1] + moban[x, y + 1] + moban[x + 1, y + 1] + moban[x + 1, y] + moban[x + 1, y - 1] + moban[x, y - 1] + moban[x - 1, y - 1]) <= 4)
                            moban[x, y] = 0;
                }

            double[,] Icc = new double[m, n];
            // @@@ 这里有Icc = ones(m,n)
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    Icc[i, j] = 1;
            }
            for (x = 0; x < H; x++)
            {
                for (y = 0; y < L; y++)
                    if (moban[x, y] == 1)
                        for (int i = 0; i < M; i++)
                            for (int j = 0; j < M; j++)
                            {
                                tmp[i + x * (int)M, j + y * (int)M] = G1;
                                Icc[i + x * (int)M, j + y * (int)M] = 0;
                            }
            }

            // correct
            //图像二值化
            double[,] temp = new double[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    temp[i, j] = 1.0 / 9;
                }

            double[,] Im = new double[300, 300];
            for (int i = 0; i < 300; i++)
                for (int j = 0; j < 300; j++)
                    Im[i, j] = tmp[i, j];

            double[,] In = new double[300, 300];
            for (int a = 1; a < m - 1; a++)
                for (int b = 1; b < n - 1; b++)
                {
                    In[a, b] = Im[a - 1, b - 1] * temp[0, 0] + Im[a - 1, b] * temp[0, 1] + Im[a - 1, b + 1] * temp[0, 2] + Im[a, b - 1] * temp[1, 0] + Im[a, b] * temp[1, 1] + Im[a, b + 1] * temp[1, 2] + Im[a + 1, b - 1] * temp[2, 0] + Im[a + 1, b] * temp[2, 1] + Im[a + 1, b + 1] * temp[2, 2];
                }

            // 167
            for (int i = 0; i < 300; i++)
                for (int j = 0; j < 300; j++)
                    tmp[i, j] = In[i, j];

            Im = new double[m, n];
            for (x = 4; x < m - 5; x++)
                for (y = 4; y < n - 5; y++)
                {
                    double sum1 = tmp[x, y - 4] + tmp[x, y - 2] + tmp[x, y + 2] + tmp[x, y + 4];
                    double sum2 = tmp[x - 2, y + 4] + tmp[x - 1, y + 2] + tmp[x + 1, y - 2] + tmp[x + 2, y - 4];
                    double sum3 = tmp[x - 2, y + 2] + tmp[x - 4, y + 4] + tmp[x + 2, y - 2] + tmp[x + 4, y - 4];
                    double sum4 = tmp[x - 2, y + 1] + tmp[x - 4, y + 2] + tmp[x + 2, y - 1] + tmp[x + 4, y - 2];
                    double sum5 = tmp[x - 2, y] + tmp[x - 4, y] + tmp[x + 2, y] + tmp[x + 4, y];
                    double sum6 = tmp[x - 4, y - 2] + tmp[x - 2, y - 1] + tmp[x + 2, y + 1] + tmp[x + 4, y + 2];
                    double sum7 = tmp[x - 4, y - 4] + tmp[x - 2, y - 2] + tmp[x + 2, y + 2] + tmp[x + 4, y + 4];
                    double sum8 = tmp[x - 2, y - 4] + tmp[x - 1, y - 2] + tmp[x + 1, y + 2] + tmp[x + 2, y + 4];
                    double[,] sumi = new double[1, 8] { { sum1, sum2, sum3, sum4, sum5, sum6, sum7, sum8 } };
                    double summax = Matrix.max(sumi, 1, 8);
                    double summin = Matrix.min(sumi, 1, 8);
                    double summ = Matrix.sum(sumi, 1, 8);
                    double b = summ / 8;
                    double sumf = 0;
                    if ((summax + summin + 4 * tmp[x, y]) > (3 * (sum1 + sum2 + sum3 + sum4 + sum5 + sum6 + sum7 + sum8) / 8))
                        sumf = summin;
                    else
                        sumf = summax;
                    if (sumf > b)
                        Im[x, y] = 128;
                    else
                        Im[x, y] = 255;

                }

            // 204
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Icc[i, j] = Icc[i, j] * Im[i, j];
                }
            }

            for (int i = 0; i < m; i++)

                for (int j = 0; j < n; j++)
                    if (Icc[i, j] == 128)
                        Icc[i, j] = 0;
                    else
                        Icc[i, j] = 1;



            // @@@ Icc 1数目 C#:62149 matlab:62045
            // 我发现左右有两列是为0， 而matlab最右只有一列为0


            // 二值化后处理
            // @@@@ 这个后面你对一下matlab，起始位从0开始的这一点
            // 你好像都忘记改了
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    Im[i, j] = Icc[i, j];
                    In[i, j] = Im[i, j];
                }

            for (int i = 1; i < m - 1; i++)
                for (int j = 1; j < n - 1; j++)
                    if (Im[i, j] == 1)
                        if ((Im[i - 1, j] + Im[i - 1, j + 1] + Im[i, j + 1] + Im[i + 1, j + 1] + Im[i + 1, j] + Im[i + 1, j - 1] + Im[i, j - 1] + Im[i - 1, j - 1]) <= 4)
                            In[i, j] = 0;



            for (int i = 1; i < m - 1; i++)
                for (int j = 1; j < n - 1; j++)
                    if (Im[i, j] == 0)
                        if ((Im[i - 1, j] + Im[i - 1, j + 1] + Im[i, j + 1] + Im[i + 1, j + 1] + Im[i + 1, j] + Im[i + 1, j - 1] + Im[i, j - 1] + Im[i - 1, j - 1]) >= 5)
                            In[i, j] = 1;


            //反向
            for (int i = 1; i < m - 1; i++)
                for (int j = 1; j < n - 1; j++)
                    if (In[i, j] == 1)
                        In[i, j] = 0;
                    else
                        In[i, j] = 1;

            //细化

            Im = ImSkeleton(In);

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    tmp[i, j] = Im[i, j];
            }

            for (int nn = 0; nn < 8; nn++)
            {
                for (x = 1; x < m - 1; x++)
                    for (y = 1; y < n - 1; y++)
                    {
                        if (tmp[x, y] == 1)
                        {
                            if (tmp[x, y - 1] == 1 && tmp[x, y + 1] == 0)   //a
                                if ((tmp[x - 1, y] == 0 && tmp[x - 1, y + 1] == 1) || (tmp[x + 1, y] == 0 && tmp[x + 1, y + 1] == 1))
                                    tmp[x, y] = 1;
                            if (tmp[x - 1, y] == 0 && tmp[x + 1, y] == 1)  //b
                                if ((tmp[x - 1, y - 1] == 1 && tmp[x, y - 1] == 0) || (tmp[x - 1, y + 1] == 1 && tmp[x, y + 1] == 0))
                                    tmp[x, y] = 1;
                            if (tmp[x, y - 1] == 0 && tmp[x, y + 1] == 1)  //c
                                if ((tmp[x - 1, y - 1] == 1 & tmp[x - 1, y] == 0) || (tmp[x + 1, y] == 0 && tmp[x + 1, y - 1] == 1))
                                    tmp[x, y] = 1;
                            if (tmp[x - 1, y] == 1 && tmp[x + 1, y] == 0)    //d
                            {
                                if ((tmp[x + 1, y - 1] == 1 && tmp[x, y - 1] == 0) || (tmp[x + 1, y + 1] == 1 && tmp[x, y + 1] == 0))
                                    tmp[x, y] = 1;
                                else
                                    tmp[x, y] = 0;
                            }
                        }
                        else
                            tmp[x, y] = 0;

                    }
            }

            //特征提取
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    Im[i, j] = tmp[i, j];

            // correct!!!
            // ## 在此之前差不多,C#84176,mat :83027  0的个数
            double[, ,] tezheng = new double[m, n, 3];

            for (int i = 1; i < m - 1; i++)
            {
                for (int j = 1; j < n - 1; j++)
                    if (Im[i, j] == 1)
                    {
                        double a = 0;
                        if (Im[i - 1, j] != Im[i - 1, j + 1])
                            a = a + 1;
                        if (Im[i - 1, j + 1] != Im[i, j + 1])
                            a = a + 1;
                        if (Im[i, j + 1] != Im[i + 1, j + 1])
                            a = a + 1;
                        if (Im[i + 1, j + 1] != Im[i + 1, j])
                            a = a + 1;
                        if (Im[i + 1, j] != Im[i + 1, j - 1])
                            a = a + 1;
                        if (Im[i + 1, j - 1] != Im[i, j - 1])
                            a = a + 1;
                        if (Im[i, j - 1] != Im[i - 1, j - 1])
                            a = a + 1;
                        if (Im[i - 1, j - 1] != Im[i - 1, j])
                            a = a + 1;
                        if (a == 6)    //分叉点判断
                        {
                            tezheng[i, j, 0] = i;
                            tezheng[i, j, 1] = j;
                            tezheng[i, j, 2] = 1;

                            tmp[i, j] = 0;
                            tmp[i - 1, j] = 1;
                            tmp[i - 1, j + 1] = 1;
                            tmp[i, j + 1] = 1;
                            tmp[i + 1, j + 1] = 1;
                            tmp[i + 1, j] = 1;
                            tmp[i + 1, j - 1] = 1;
                            tmp[i, j - 1] = 1;
                            tmp[i - 1, j - 1] = 1;
                        }
                        if (a == 2)    //端点判断
                        {
                            tezheng[i, j, 0] = i;
                            tezheng[i, j, 1] = j;
                            tezheng[i, j, 2] = 2;
                            tmp[i, j] = 0;
                            tmp[i - 1, j] = 1;
                            tmp[i - 1, j + 1] = 1;
                            tmp[i, j + 1] = 1;
                            tmp[i + 1, j + 1] = 1;
                            tmp[i + 1, j] = 1;
                            tmp[i + 1, j - 1] = 1;
                            tmp[i, j - 1] = 1;
                            tmp[i - 1, j - 1] = 1;
                        }
                    }
            }

            //特征点为2的135，MAT才133,//特征点为1的有15，MAT15
            //正确
            //去除边界点
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    if (tezheng[i, j, 2] != 0) //消除端点
                    {
                        if (i - 17 < 0 || i + 17 > m || j - 17 < 0 || j + 17 > n)
                        {
                            tezheng[i, j, 2] = 0;
                            tmp[i - 1, j] = 0;
                            tmp[i - 1, j + 1] = 0;
                            tmp[i, j + 1] = 0;
                            tmp[i + 1, j + 1] = 0;
                            tmp[i + 1, j] = 0;
                            tmp[i + 1, j - 1] = 0;
                            tmp[i, j - 1] = 0;
                            tmp[i - 1, j - 1] = 0;
                        }
                        else
                        {
                            double aa = 0; double b = 0; double cc = 0; double d = 0;
                            aa = tmp[i + 1, j] + tmp[i + 2, j] + tmp[i + 3, j] + tmp[i + 4, j] + tmp[i + 5, j] + tmp[i + 6, j] + tmp[i + 7, j] + tmp[i + 8, j] + tmp[i + 9, j] + tmp[i + 10, j] + tmp[i + 11, j] + tmp[i + 12, j];
                            b = tmp[i - 1, j] + tmp[i - 2, j] + tmp[i + 3, j] + tmp[i - 4, j] + tmp[i - 5, j] + tmp[i - 6, j] + tmp[i - 7, j] + tmp[i - 8, j] + tmp[i - 9, j] + tmp[i - 10, j] + tmp[i - 11, j] + tmp[i - 12, j];
                            cc = tmp[i, j + 1] + tmp[i, j + 2] + tmp[i, j + 3] + tmp[i, j + 4] + tmp[i, j + 5] + tmp[i, j + 6] + tmp[i, j + 7] + tmp[i, j + 8] + tmp[i, j + 9] + tmp[i, j + 10] + tmp[i, j + 11] + tmp[i, j + 12];
                            d = tmp[i, j - 1] + tmp[i, j - 2] + tmp[i, j - 3] + tmp[i, j - 4] + tmp[i, j - 5] + tmp[i, j - 6] + tmp[i, j - 7] + tmp[i, j - 8] + tmp[i, j - 9] + tmp[i, j - 10] + tmp[i, j - 11] + tmp[i, j - 12];
                            if (aa <= 1 && b <= 1 && cc <= 1 && d <= 1)
                            {
                                tezheng[i, j, 2] = 0;//消除端点
                                tmp[i - 1, j] = 0;
                                tmp[i - 1, j + 1] = 0;
                                tmp[i, j + 1] = 0;
                                tmp[i + 1, j + 1] = 0;
                                tmp[i + 1, j] = 0;
                                tmp[i + 1, j - 1] = 0;
                                tmp[i, j - 1] = 0;
                                tmp[i - 1, j - 1] = 0;
                            }
                        }
                    }
            }
            //消除断点
            int loop1 = 0;
            int loop = 0;
            for (int i = 9; i < m - 10; i++)
            {
                for (int j = 9; j < n - 10; j++)
                {
                    if (tezheng[i, j, 2] == 2)
                    {
                        for (x = i - 10; x <= i + 10; x++)
                        {
                            for (y = j - 10; y <= j - 1; y++)
                            {
                                if (tezheng[x, y, 2] == 2)
                                {
                                    tezheng[i, j, 2] = 0;
                                    tezheng[x, y, 2] = 0;
                                    tmp[i - 1, j] = 0;
                                    tmp[i - 1, j + 1] = 0;
                                    tmp[i, j + 1] = 0;
                                    tmp[i + 1, j + 1] = 0;
                                    tmp[i + 1, j] = 0;
                                    tmp[i + 1, j - 1] = 0;
                                    tmp[i, j - 1] = 0;
                                    tmp[i - 1, j - 1] = 0;

                                    tmp[x - 1, y] = 0;
                                    tmp[x - 1, y + 1] = 0;
                                    tmp[x, y + 1] = 0;
                                    tmp[x + 1, y + 1] = 0;
                                    tmp[x + 1, y] = 0;
                                    tmp[x + 1, y - 1] = 0;
                                    tmp[x, y - 1] = 0;
                                    tmp[x - 1, y - 1] = 0;

                                }
                            }
                            for (x = i - 10; x <= i + 10; x++)
                            {
                                for (y = j + 1; y <= j + 10; y++)
                                {
                                    if (tezheng[x, y, 2] == 2)
                                    {
                                        tezheng[i, j, 2] = 0;
                                        tezheng[x, y, 2] = 0;
                                        tmp[i - 1, j] = 0;
                                        tmp[i - 1, j + 1] = 0;
                                        tmp[i, j + 1] = 0;
                                        tmp[i + 1, j + 1] = 0;
                                        tmp[i + 1, j] = 0;
                                        tmp[i + 1, j - 1] = 0;
                                        tmp[i, j - 1] = 0;
                                        tmp[i - 1, j - 1] = 0;

                                        tmp[x - 1, y] = 0;
                                        tmp[x - 1, y + 1] = 0;
                                        tmp[x, y + 1] = 0;
                                        tmp[x + 1, y + 1] = 0;
                                        tmp[x + 1, y] = 0;
                                        tmp[x + 1, y - 1] = 0;
                                        tmp[x, y - 1] = 0;
                                        tmp[x - 1, y - 1] = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //mat 1:12 2:37   c# 1:12 2:43
            //目前正确
            //去除毛刺
            for (int i = 9; i < m - 10; i++)
                for (int j = 9; j < n - 10; j++)
                    if (tezheng[i, j, 2] == 2)
                        for (x = i - 8; x <= i + 8; x++)
                            for (y = j - 8; y <= j + 8; y++)
                                if (tezheng[x, y, 2] == 1)
                                {
                                    tezheng[i, j, 2] = 0;
                                    tezheng[x, y, 2] = 0;
                                    tmp[i - 1, j] = 0;
                                    tmp[i - 1, j + 1] = 0;
                                    tmp[i, j + 1] = 0;
                                    tmp[i + 1, j + 1] = 0;
                                    tmp[i + 1, j] = 0;
                                    tmp[i + 1, j - 1] = 0;
                                    tmp[i, j - 1] = 0;
                                    tmp[i - 1, j - 1] = 0;

                                    tmp[x - 1, y] = 0;
                                    tmp[x - 1, y + 1] = 0;
                                    tmp[x, y + 1] = 0;
                                    tmp[x + 1, y + 1] = 0;
                                    tmp[x + 1, y] = 0;
                                    tmp[x + 1, y - 1] = 0;
                                    tmp[x, y - 1] = 0;
                                    tmp[x - 1, y - 1] = 0;
                                }

            //mat 1:6 2:31  c# 1:9  2:40
            //消除小桥

            for (int i = 9; i < m - 10; i++)
                for (int j = 9; j < n - 10; j++)
                    if (tezheng[i, j, 2] == 1)
                        for (x = i - 4; x <= i + 4; x++)
                        {
                            for (y = j - 4; y <= j - 1; y++)
                                if (tezheng[x, y, 2] == 1)
                                {
                                    tezheng[i, j, 2] = 0;
                                    tezheng[x, y, 2] = 0;
                                    tmp[i - 1, j] = 0;
                                    tmp[i - 1, j + 1] = 0;
                                    tmp[i, j + 1] = 0;
                                    tmp[i + 1, j + 1] = 0;
                                    tmp[i + 1, j] = 0;
                                    tmp[i + 1, j - 1] = 0;
                                    tmp[i, j - 1] = 0;
                                    tmp[i - 1, j - 1] = 0;

                                    tmp[x - 1, y] = 0;
                                    tmp[x - 1, y + 1] = 0;
                                    tmp[x, y + 1] = 0;
                                    tmp[x + 1, y + 1] = 0;
                                    tmp[x + 1, y] = 0;
                                    tmp[x + 1, y - 1] = 0;
                                    tmp[x, y - 1] = 0;
                                    tmp[x - 1, y - 1] = 0;
                                }

                            for (x = i - 4; x <= i + 4; x++)
                                for (y = j + 1; y <= j + 4; y++)
                                    if (tezheng[x, y, 2] == 1)
                                    {
                                        tezheng[i, j, 2] = 0;
                                        tezheng[x, y, 2] = 0;
                                        tmp[i - 1, j] = 0;
                                        tmp[i - 1, j + 1] = 0;
                                        tmp[i, j + 1] = 0;
                                        tmp[i + 1, j + 1] = 0;
                                        tmp[i + 1, j] = 0;
                                        tmp[i + 1, j - 1] = 0;
                                        tmp[i, j - 1] = 0;
                                        tmp[i - 1, j - 1] = 0;

                                        tmp[x - 1, y] = 0;
                                        tmp[x - 1, y + 1] = 0;
                                        tmp[x, y + 1] = 0;
                                        tmp[x + 1, y + 1] = 0;
                                        tmp[x + 1, y] = 0;
                                        tmp[x + 1, y - 1] = 0;
                                        tmp[x, y - 1] = 0;
                                        tmp[x - 1, y - 1] = 0;
                                    }
                        }


            //
            //将待处理的特征点保存
            int num1 = 0;
            int num2 = 0;
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    if (tezheng[i, j, 2] == 1)
                        num1 = num1 + 1;
                    if (tezheng[i, j, 2] == 2)
                        num2 = num2 + 1;
                }
            double[,] vector1 = new double[num1 + num2, 3];

            int k = 0;
            int f = num1;
            for (int i = 0; i < m; i++)
                for (int j = 0; j < m; j++)
                {
                    if (tezheng[i, j, 2] == 1)
                    {
                        vector1[k, 0] = tezheng[i, j, 0];//vector1中存放的是分叉点的横纵坐标，第一行是横坐标，第二行是纵坐标
                        vector1[k, 1] = tezheng[i, j, 1];
                        vector1[k, 2] = 1;
                        k = k + 1;
                    }
                    if (tezheng[i, j, 2] == 2)
                    {
                        vector1[f, 0] = tezheng[i, j, 0];//vector2中存放的是端点的横纵坐标，第一行是横坐标，第二行是纵坐标
                        vector1[f, 1] = tezheng[i, j, 1];
                        vector1[f, 2] = 2;
                        f = f + 1;
                    }

                }
            return vector1;

        }

        #endregion
    }
}
