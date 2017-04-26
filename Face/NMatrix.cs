using System;
using System.Collections.Generic;
using System.Text;

namespace PwdManagement.Face
{
    public class NMatrix
    {
        // 将N阶矩阵求绝对值
        public static double[,] abs(double[,] data, int N)
        {
            var result = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    result[i, j] = Math.Abs(data[i, j]);
            return result;
        }
        // 将三阶矩阵降维至二阶
        public static double[,] dim3to2(double[, ,] data, int dim, int N)
        {
            double[,] result = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    result[i, j] = data[dim, i, j];
            return result;
        }
        // result = AT*A ;
        public static double[,] covMatrix(double[, ,] errFace, int numOfFace, int imgWidth, int imgHight)
        {
            double[,] covFace = new double[imgWidth, imgHight];
            for (int i = 0; i < numOfFace; i++)
            {
                var tmp = multi(trs(dim3to2(errFace, i, imgHight), imgHight), dim3to2(errFace, i, imgHight), imgHight);
                covFace = plus(covFace, tmp, imgHight);
            }
            covFace = div(covFace, numOfFace, imgHight);
            return covFace;

        }
        // 一行X一列的值
        public static double mul_one(double[,] d1, double[,] d2, int N)
        {
            double result = 0;
            for (int i = 0; i < N; i++)
                result += d1[0, i] * d2[i, 0];
            return result;
        }
        // N阶矩阵每一位除
        public static double[,] div(double[,] data, double num, int N)
        {
            var result = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (data[i, j] != 0)
                        result[i, j] = data[i, j] / num;
            return result;
        }

        // N阶矩阵转置
        public static double[,] trs(double[,] data, int N)
        {
            double[,] result = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    result[j, i] = data[i, j];
            return result;
        }
        // N阶矩阵加运算
        public static double[,] plus(double[,] d1, double[,] d2, int N)
        {
            var result = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    result[i, j] = d1[i, j] + d2[i, j];
            return result;
        }
        // N阶矩阵减运算
        public static double[,] sub(double[,] d1, double[,] d2, int N)
        {
            var result = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    result[i, j] = d1[i, j] - d2[i, j];
            return result;
        }
        // N阶矩阵相乘
        public static double[,] multi(double[,] d1, double[,] d2, int N)
        {
            double[,] result = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    for (int k = 0; k < N; k++)
                        result[i, j] += (d1[i, k] * d2[k, j]);
            return result;
        }
        // 求解矩阵特征向量和特征值
        public static double[, ,] eig(double[,] data, int N)
        {
            // 特征向量
            double[,] vect = new double[N, N];
            for (int i = 0; i < N; i++)
                vect[i, i] = 1;
            // 进行 Givens-Jacobi变换求特征值
            int k = 1, m = 2;
            // 设定阀值
            double thredhold = 1, sint, cost;
            int mm = 0;
            while (mm++ < 100)
            {
                // 构建单位矩阵
                double[,] G = new double[N, N];
                for (int i = 0; i < N; i++)
                    G[i, i] = 1;

                // 计算Givens-Jacobi算子
                int syb = Math.Sign(data[k, m]);
                if (data[k, k] == data[m, m])
                {
                    cost = Math.Cos(-Math.PI / 4);
                    sint = Math.Cos(-Math.PI / 4);
                }
                else
                {
                    double tant = 2 * data[k, m] / (data[k, k] - data[m, m]);
                    double t = Math.Sign(tant) * Math.Abs(tant) / (Math.Abs(tant) + Math.Sqrt(1 + tant * tant));
                    cost = 1 / (Math.Sqrt(1 + t * t));
                    sint = t / (Math.Sqrt(1 + t * t));
                }

                G[k, m] = -sint;
                G[m, k] = sint;
                G[k, k] = cost;
                G[m, m] = cost;

                // 进行旋转变换
                data = multi(trs(G, N), data, N);
                data = multi(data, G, N);
                vect = multi(vect, G, N);

                // 选取非主对角线最大值
                double max = 0;
                int max_x = 0, max_y = 0;
                for (int m1 = 0; m1 < N; m1++)
                    for (int m2 = m1 + 1; m2 < N; m2++)
                    {
                        if (Math.Abs(data[m1, m2]) > Math.Abs(max))
                        {
                            max_x = m1;
                            max_y = m2;
                            max = Math.Abs(data[m1, m2]);
                        }
                    }
                k = max_x;
                m = max_y;

                // 到达阀值
                if (max < thredhold) break;
            }
            // matrix_print(vect, N);
            double[, ,] result = new double[2, N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result[0, i, j] = vect[i, j];
                }
                result[1, i, i] = data[i, i];
            }
            return result;
        }
    }
}
