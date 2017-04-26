using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PwdManagement.Finger
{
    public class Matrix
    {
        public static double[,] abs(double[,] A, int R, int C)
        {
            var re = new double[R, C];
            for (int i = 0; i < R; i++)
            {
                for (int j = 0; j < C; j++)
                {
                    re[i, j] = Math.Abs(A[i, j]);
                }
            }
            return re;
        }
        public static double[,] plus(double[,] A1, double[,] A2, int R, int C)
        {
            var re = new double[R, C];
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    re[i, j] = A1[i, j] + A2[i, j];
            return re;
        }
        public static double[,] sub(double[,] A1, double[,] A2, int R, int C)
        {
            var re = new double[R, C];
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    re[i, j] = A1[i, j] - A2[i, j];
            return re;
        }
        /// <summary>
        /// 将 (R1,C1)矩阵 X (R2, C2)矩阵, 要求 C1==R2
        /// </summary>
        /// <param name="A1">左边矩阵</param>
        /// <param name="R1">左行</param>
        /// <param name="C1">左列</param>
        /// <param name="A2">右边矩阵</param>
        /// <param name="R2">右行</param>
        /// <param name="C2">右列</param>
        /// <returns>R1*C2的矩阵</returns>
        public static double[,] multi(double[,] A1, int R1, int C1, double[,] A2, int R2, int C2)
        {
            // 矩阵乘法合法性标志
            Debug.Assert(C1 == R2, "矩阵乘法规则使用错误");

            var re = new double[R1, C2];
            for (int i = 0; i < R1; i++)
                for (int j = 0; j < C2; j++)
                    for (int k = 0; k < C1; k++)
                        re[i, j] += A1[i, k] * A2[k, j];

            return re;
        }
        /// <summary>
        /// 矩阵转置运算 [R,C]矩阵 转置为 [C,R]
        /// </summary>
        /// <param name="A">待转置矩阵</param>
        /// <param name="R">行</param>
        /// <param name="C">列</param>
        /// <returns>转置后的[C,R]double数组</returns>
        public static double[,] trans(double[,] A, int R, int C)
        {
            var re = new double[C, R];
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    re[j, i] = A[i, j];
            return re;
        }
        /// <summary>
        /// 对矩阵中的每个元素开平方
        /// </summary>
        /// <param name="A">待开平方的矩阵</param>
        /// <param name="R">行</param>
        /// <param name="C">列</param>
        /// <returns>开平方后的[R,C]double数组</returns>
        public static double[,] sqrt(double[,] A, int R, int C)
        {
            var re = new double[R, C];
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    re[i, j] = Math.Sqrt(A[i, j]);
            return re;
        }
        /// <summary>
        /// 保留矩阵对角线元素，其它元素都置为0
        /// 要求：必须是N阶矩阵
        /// </summary>
        /// <param name="A">原矩阵</param>
        /// <param name="N">阶数</param>
        /// <returns>保留对角线后的double[N,N]矩阵</returns>
        public static double[,] diag(double[,] A, int N)
        {
            var re = new double[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (i == j)
                        re[i, j] = A[i, j];
            return re;
        }
        /// <summary>
        /// 采用高斯消去法求解矩阵逆
        /// 要求：N阶矩阵，对角线元素不为0
        /// </summary>
        /// <param name="A">矩阵</param>
        /// <param name="N">阶</param>
        /// <returns>double(N,N)类型矩阵逆</returns>
        public static double[,] inv(double[,] A, int N)
        {
            var re = new double[N, N];

            // 首先主对角线元素不为0
            for (int i = 0; i < N; i++)
                if (A[i, i] == 0)
                    Debug.Assert(A[i, i] != 0, "矩阵乘法规则使用错误");

            // 求逆方法: 将一个矩阵化简为单位矩阵的同时，将单位矩阵化简为逆矩阵
            var J = new double[N, 2 * N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < 2 * N; j++)
                    if (j < N)
                        J[i, j] = A[i, j];
                    else if ((j - N) == i)
                        J[i, j] = 1;

            // 化简为上阶梯形状 - 高斯消去法
            for (int j = 0; j < N; j++) // 列
            {
                // 将矩阵变为上阶梯形
                // 第j列先行元素下的值全为0
                for (int i = j + 1; i < N; i++) // 行
                {
                    // 某一行与该列先行元素的比值
                    var rate = J[i, j] / J[j, j];
                    for (int k = j; k < 2 * N; k++)
                        J[i, k] = (-rate) * J[j, k] + J[i, k];
                }
                // 且将主对角线第j行线性变换为先行元素为1
                // Console.WriteLine(j);
                var rt = J[j, j];
                for (int k = j; k < 2 * N; k++)
                    J[j, k] = J[j, k] / rt;
            }

            // 化简为上最简阶梯形状
            for (int j = N - 1; j >= 0; j--)  // 列
                // 将矩阵变为上阶梯形
                // 第j列先行元素下的值全为0
                for (int i = j - 1; i >= 0; i--) // 行
                {
                    // 某一行与该列先行元素的比值
                    var rate = J[i, j] / J[j, j];
                    for (int k = j; k < 2 * N; k++)
                        J[i, k] = (-rate) * J[j, k] + J[i, k];
                }

            // 将逆矩阵赋值给数组
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    re[i, j] = J[i, j + N];

            return re;
        }
        /// <summary>
        /// R*C 矩阵A1和A2的点乘
        /// 即对应元素对应位置相乘
        /// </summary>
        /// <param name="A1">左矩阵</param>
        /// <param name="A2">右矩阵</param>
        /// <param name="R">行数</param>
        /// <param name="C">列数</param>
        /// <returns>double[R,C]矩阵</returns>
        public static double[,] pointMulti(double[,] A1, double[,] A2, int R, int C)
        {
            var re = new double[R, C];
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    re[i, j] = A1[i, j] * A2[i, j];
            return re;
        }
        /// <summary>
        /// 求矩阵所有元素的和
        /// </summary>
        /// <param name="A">矩阵</param>
        /// <param name="R">行</param>
        /// <param name="C">列</param>
        /// <returns>double矩阵所有元素的和</returns>
        public static double sum(double[,] A, int R, int C)
        {
            double re = 0;
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    re += A[i, j];
            return re;
        }
        /// <summary>
        /// 获得一个N阶单位矩阵
        /// </summary>
        /// <param name="N">阶数</param>
        /// <returns>单位矩阵</returns>
        public static double[,] eye(int N)
        {
            var re = new double[N, N];
            for (int i = 0; i < N; i++)
                re[i, i] = 1;
            return re;
        }
        /// <summary>
        /// 矩阵的转置 X 矩阵本身 AT * A
        /// </summary>
        /// <param name="A">矩阵A</param>
        /// <param name="R">矩阵A的行</param>
        /// <param name="C">矩阵A的列</param>
        /// <returns>返回一个[C,C]矩阵</returns>
        public static double[,] ATA(double[,] A, int R, int C)
        {
            var A_t = trans(A, R, C);
            var re = multi(A_t, C, R, A, R, C);
            return re;
        }
        /// <summary>
        /// 对矩阵中的每个元素乘以k
        /// </summary>
        /// <param name="A">矩阵</param>
        /// <param name="R">行</param>
        /// <param name="C">列</param>
        /// <param name="k">乘子</param>
        /// <returns>返回一个结果矩阵</returns>
        public static double[,] multiOne(double[,] A, int R, int C, double k)
        {
            var re = new double[R, C];
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    re[i, j] = k * A[i, j];
            return re;
        }
        /// <summary>
        /// 对矩阵中的每个元素加上k
        /// </summary>
        /// <param name="A"></param>
        /// <param name="R"></param>
        /// <param name="C"></param>
        /// <param name="k"></param>
        /// <returns>返回一个结果矩阵</returns>
        public static double[,] plusOne(double[,] A, int R, int C, double k)
        {
            var re = new double[R, C];
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    re[i, j] = k + A[i, j];
            return re;
        }
        /// <summary>
        /// 将矩阵中所有小于k的数都置为k
        /// </summary>
        /// <param name="A"></param>
        /// <param name="R"></param>
        /// <param name="C"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double[,] minLimit(double[,] A, int R, int C, double k)
        {
            var re = new double[R, C];
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    if (A[i, j] < k)
                        re[i, j] = k;
                    else
                        re[i, j] = A[i, j];
            return re;
        }
        /// <summary>
        /// 返回数组最大值
        /// </summary>
        /// <param name="A"></param>
        /// <param name="R"></param>
        /// <param name="C"></param>
        /// <returns></returns>
        public static double max(double[,] A, int R, int C)
        {
            double re = double.MinValue;
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    if (A[i, j] > re)
                        re = A[i, j];
            return re;
        }


        public static double min(double[,] A, int R, int C)
        {
            double re = A[0, 0];
            for (int i = 0; i < R; i++)
                for (int j = 0; j < C; j++)
                    if (A[i, j] < re)
                        re = A[i, j];
            return re;
        }
        public static double[,] clear(double[,] A)
        {
            int H = A.GetLength(0);
            int W = A.GetLength(1);
            for (int i = 0; i < H; i++)
                for (int j = 0; j < W; j++)
                    A[i, j] = 0;
            return A;
        }
        /// <summary>
        /// A = B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public static void equal(double[,] A, double[,] B)
        {
            int H = B.GetLength(0);
            int W = B.GetLength(1);
            for (int i = 0; i < H; i++)
                for (int j = 0; j < W; j++)
                    A[i, j] = B[i, j];
        }
    }
}
