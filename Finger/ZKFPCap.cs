using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace PwdManagement.Finger
{
    #region 一些转bmp所需的结构体
    public struct BITMAPINFOHEADER
    {
        public int biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public int biCompression;
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
    }

    public struct BITMAPFILEHEADER
    {
        public ushort bfType;
        public int bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public int bfOffBits;
    }

    public struct MASK
    {
        public byte redmask;
        public byte greenmask;
        public byte bluemask;
        public byte rgbReserved;
    }

    #endregion
    public class ZKFPCap
    {
        public const string DllName = @"ZKFPCap.dll";
        
        #region sensorInit

        /// <summary>
        /// 初始化参数，首先要调用此接口
        /// </summary>
        /// <returns>0表示成功，其他表示失败</returns>
        [DllImport(DllName, EntryPoint = "sensorInit")]
        public static extern int sensorInit();

        #endregion

        #region sensorFree

        /// <summary>
        /// 释放初始化时申请的空间
        /// </summary>
        /// <returns>0</returns>
        [DllImport(DllName, EntryPoint = "sensorFree")]
        public static extern int sensorFree();

        #endregion

        #region sensorOpen

        /// <summary>
        /// 连接指定的指纹采集器，当同时插了U.are.U、ZK指纹采集器，优先连接U.are.U指纹采集器
        /// </summary>
        /// <param name="index">指纹采集器索引，从0开始</param>
        /// <returns>0表示连接失败，否则为>0的指纹采集器句柄</returns>
        [DllImport(DllName, EntryPoint = "sensorOpen")]
        public static extern IntPtr sensorOpen(int index);

        #endregion

        #region sensorClose

        /// <summary>
        /// 断开连接，释放sensorOpen申请的空间
        /// </summary>
        /// <param name="handle">sensorOpen返回的句柄</param>
        /// <returns>0表示成功，-2表示handle为空</returns>
        [DllImport(DllName, EntryPoint = "sensorClose")]
        public static extern int sensorClose(IntPtr handle);

        #endregion

        #region sensorGetCount

        /// <summary>
        /// 获取指纹采集器个数
        /// </summary>
        /// <returns>指纹采集器个数</returns>
        [DllImport(DllName, EntryPoint = "sensorGetCount")]
        public static extern int sensorGetCount();

        #endregion

        #region sensorGetVersion

        /// <summary>
        /// 读取版本号，目前版本为3.0.0.1
        /// </summary>
        /// <param name="version">以\0结束的字符串，推荐申请大小为16字节</param>
        /// <param name="len">version申请空间大小</param>
        /// <returns>0表示成功，-2表示version为空，-3表示申请空间大小小于实际空间大小</returns>
        [DllImport(DllName, EntryPoint = "sensorGetVersion")]
        public static extern int sensorGetVersion(byte[] version, int len);

        #endregion

        #region sensorGetParameter

        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="handle">sensorOpen返回的句柄</param>
        /// <param name="paramCode">读取参数编码，1表示图像宽度，2表示图像高度，106表示sensorCapture申请图像buffer大小，1015表示指纹采集器的VID和PID，低两字节为VID，高两字节为PID</param>
        /// <returns>>=0表示返回paramCode对应的参数值，-2表示handle为空，-5表示非法参数或不支持该功能</returns>
        [DllImport(DllName, EntryPoint = "sensorGetParameter")]
        public static extern int sensorGetParameter(IntPtr handle, int paramCode);

        #endregion

        #region sensorSetParameter

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="handle">sensorOpen返回的句柄</param>
        /// <param name="paramCode">设置参数编码，102表示控制GreenLED，0关闭，1打开，103表示控制RedLED，0关闭，1打开，104控制Beep，0关闭，1打开</param>
        /// <param name="paramValue">根据paramCode设置相应的参数值</param>
        /// <returns>0表示成功，-2表示handle为空，-5表示非法参数或不支持该功能</returns>
        [DllImport(DllName, EntryPoint = "sensorSetParameter")]
        public static extern int sensorSetParameter(IntPtr handle, int paramCode, int paramValue);

        #endregion

        #region sensorGetParameterEx

        /// <summary>
        /// 读取参数，兼容sensorGetParameter，但paramValue必需>=4字节，与sensorGetParameter区别是可以读取字符串格式的参数值
        /// </summary>
        /// <param name="handle">sensorOpen返回的句柄</param>
        /// <param name="paramCode">读取参数编码，1101表示制造商名称，1102表示指纹采集器名称，1103表示序列号</param>
        /// <param name="paramValue">返回paramCode对应的参数值，推荐申请大小为64字节</param>
        /// <param name="paramLen">传入值为paramValue申请空间大小，传出值为参数值对应的大小</param>
        /// <returns>0表示成功，-2表示handle或paramValue为空，-3表示申请空间大小小于返回参数值大小，-5表示非法参数或不支持该功能</returns>
        [DllImport(DllName, EntryPoint = "sensorGetParameterEx")]
        public static extern int sensorGetParameterEx(IntPtr handle, int paramCode, byte[] paramValue, ref int paramLen);

        #endregion

        #region sensorSetParameterEx

        /// <summary>
        /// 设置参数，兼容sensorSetParameter，但paramValue必需>=4字节
        /// </summary>
        /// <param name="handle">sensorOpen返回的句柄</param>
        /// <param name="paramCode">参数代码</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="paramLen">paramValue申请空间大小</param>
        /// <returns>0表示成功，-2handle或paramValue为空，-3申请空间大小小于实际空间大小，-5表示非法参数或不支持该功能</returns>
        [DllImport(DllName, EntryPoint = "sensorSetParameterEx")]
        public static extern int sensorSetParameterEx(IntPtr handle, int paramCode, byte[] paramValue, int paramLen);

        #endregion

        #region sensorCapture

        /// <summary>
        /// 从指纹采集器中采集图像
        /// </summary>
        /// <param name="handle">sensorOpen返回的句柄</param>
        /// <param name="imageBuffer">返回采集到的图像，申请空间大小调用sensorGetParameter(106)</param>
        /// <param name="imageBufferSize">imageBuffer申请的空间大小</param>
        /// <returns>>0表示采集到的图像的大小，0表示没有采集到图像，-2表示handle或imageBuffer为空，-3表示申请空间大小小于采集到图像空间大小</returns>
        [DllImport(DllName, EntryPoint = "sensorCapture")]
        public static extern int sensorCapture(IntPtr handle, byte[] imageBuffer, int imageBufferSize);

        #endregion

        #region 保存bmp格式图片所用方法

        /*******************************************
        * 函数名称：RotatePic       
        * 函数功能：旋转图片，目的是保存和显示的图片与按的指纹方向不同     
        * 函数入参：BmpBuf---旋转前的指纹字符串
        * 函数出参：ResBuf---旋转后的指纹字符串
        * 函数返回：无
        *********************************************/
        public static void RotatePic(byte[] BmpBuf, int width, int height, ref byte[] ResBuf)
        {
            int RowLoop = 0;
            int ColLoop = 0;
            int BmpBuflen = width * height;

            for (RowLoop = 0; RowLoop < BmpBuflen; )
            {
                for (ColLoop = 0; ColLoop < width; ColLoop++)
                {
                    ResBuf[RowLoop + ColLoop] = BmpBuf[BmpBuflen - RowLoop - width + ColLoop];
                }

                RowLoop = RowLoop + width;
            }
        }

        /*******************************************
        * 函数名称：StructToBytes       
        * 函数功能：将结构体转化成无符号字符串数组     
        * 函数入参：StructObj---被转化的结构体
        *           Size---被转化的结构体的大小
        * 函数出参：无
        * 函数返回：结构体转化后的数组
        *********************************************/
        public static byte[] StructToBytes(object StructObj, int Size)
        {
            int StructSize = Marshal.SizeOf(StructObj);
            byte[] GetBytes = new byte[StructSize];
            IntPtr StructPtr = Marshal.AllocHGlobal(StructSize);
            Marshal.StructureToPtr(StructObj, StructPtr, false);
            Marshal.Copy(StructPtr, GetBytes, 0, StructSize);
            Marshal.FreeHGlobal(StructPtr);

            if (Size == 14)
            {
                byte[] NewBytes = new byte[Size];
                int Count = 0;
                int Loop = 0;

                for (Loop = 0; Loop < StructSize; Loop++)
                {
                    if (Loop != 2 && Loop != 3)
                    {
                        NewBytes[Count] = GetBytes[Loop];
                        Count++;
                    }
                }

                return NewBytes;
            }
            else
            {
                return GetBytes;
            }
        }

        /*******************************************
        * 函数名称：GetBitmap       
        * 函数功能：将传进来的数据保存为图片     
        * 函数入参：buffer---图片数据
        *           nWidth---图片的宽度
        *           nHeight---图片的高度
        * 函数出参：无
        * 函数返回：无
        *********************************************/
        public static void GetBitmap(byte[] buffer, int nWidth, int nHeight, ref MemoryStream ms)
        {
            int ColorIndex = 0;
            ushort m_nBitCount = 8;
            int m_nColorTableEntries = 256;
            byte[] ResBuf = new byte[nWidth * nHeight];

            BITMAPFILEHEADER BmpHeader = new BITMAPFILEHEADER();
            BITMAPINFOHEADER BmpInfoHeader = new BITMAPINFOHEADER();
            MASK[] ColorMask = new MASK[m_nColorTableEntries];

            //图片头信息
            BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader);
            BmpInfoHeader.biWidth = nWidth;
            BmpInfoHeader.biHeight = nHeight;
            BmpInfoHeader.biPlanes = 1;
            BmpInfoHeader.biBitCount = m_nBitCount;
            BmpInfoHeader.biCompression = 0;
            BmpInfoHeader.biSizeImage = 0;
            BmpInfoHeader.biXPelsPerMeter = 0;
            BmpInfoHeader.biYPelsPerMeter = 0;
            BmpInfoHeader.biClrUsed = m_nColorTableEntries;
            BmpInfoHeader.biClrImportant = m_nColorTableEntries;

            //文件头信息
            BmpHeader.bfType = 0x4D42;
            BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4;
            BmpHeader.bfSize = BmpHeader.bfOffBits + ((((BmpInfoHeader.biWidth * BmpInfoHeader.biBitCount + 31) / 32) * 4) * BmpInfoHeader.biHeight);
            BmpHeader.bfReserved1 = 0;
            BmpHeader.bfReserved2 = 0;

            ms.Write(StructToBytes(BmpHeader, 14), 0, 14);
            ms.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)), 0, Marshal.SizeOf(BmpInfoHeader));

            //调试板信息
            for (ColorIndex = 0; ColorIndex < m_nColorTableEntries; ColorIndex++)
            {
                ColorMask[ColorIndex].redmask = (byte)ColorIndex;
                ColorMask[ColorIndex].greenmask = (byte)ColorIndex;
                ColorMask[ColorIndex].bluemask = (byte)ColorIndex;
                ColorMask[ColorIndex].rgbReserved = 0;

                ms.Write(StructToBytes(ColorMask[ColorIndex], Marshal.SizeOf(ColorMask[ColorIndex])), 0, Marshal.SizeOf(ColorMask[ColorIndex]));
            }

            //图片旋转，解决指纹图片倒立的问题
            RotatePic(buffer, nWidth, nHeight, ref ResBuf);

            ms.Write(ResBuf, 0, nWidth * nHeight);
        }

        /*******************************************
        * 函数名称：WriteBitmap       
        * 函数功能：将传进来的数据保存为图片     
        * 函数入参：buffer---图片数据
        *           nWidth---图片的宽度
        *           nHeight---图片的高度
        * 函数出参：无
        * 函数返回：无
        *********************************************/
        public static void WriteBitmap(byte[] buffer, int nWidth, int nHeight)
        {
            int ColorIndex = 0;
            ushort m_nBitCount = 8;
            int m_nColorTableEntries = 256;
            byte[] ResBuf = new byte[nWidth * nHeight];

            BITMAPFILEHEADER BmpHeader = new BITMAPFILEHEADER();
            BITMAPINFOHEADER BmpInfoHeader = new BITMAPINFOHEADER();
            MASK[] ColorMask = new MASK[m_nColorTableEntries];

            //图片头信息
            BmpInfoHeader.biSize = Marshal.SizeOf(BmpInfoHeader);
            BmpInfoHeader.biWidth = nWidth;
            BmpInfoHeader.biHeight = nHeight;
            BmpInfoHeader.biPlanes = 1;
            BmpInfoHeader.biBitCount = m_nBitCount;
            BmpInfoHeader.biCompression = 0;
            BmpInfoHeader.biSizeImage = 0;
            BmpInfoHeader.biXPelsPerMeter = 0;
            BmpInfoHeader.biYPelsPerMeter = 0;
            BmpInfoHeader.biClrUsed = m_nColorTableEntries;
            BmpInfoHeader.biClrImportant = m_nColorTableEntries;

            //文件头信息
            BmpHeader.bfType = 0x4D42;
            BmpHeader.bfOffBits = 14 + Marshal.SizeOf(BmpInfoHeader) + BmpInfoHeader.biClrUsed * 4;
            BmpHeader.bfSize = BmpHeader.bfOffBits + ((((BmpInfoHeader.biWidth * BmpInfoHeader.biBitCount + 31) / 32) * 4) * BmpInfoHeader.biHeight);
            BmpHeader.bfReserved1 = 0;
            BmpHeader.bfReserved2 = 0;

            Stream FileStream = File.Open("finger.bmp", FileMode.Create, FileAccess.Write);
            BinaryWriter TmpBinaryWriter = new BinaryWriter(FileStream);

            TmpBinaryWriter.Write(StructToBytes(BmpHeader, 14));
            TmpBinaryWriter.Write(StructToBytes(BmpInfoHeader, Marshal.SizeOf(BmpInfoHeader)));

            //调试板信息
            for (ColorIndex = 0; ColorIndex < m_nColorTableEntries; ColorIndex++)
            {
                ColorMask[ColorIndex].redmask = (byte)ColorIndex;
                ColorMask[ColorIndex].greenmask = (byte)ColorIndex;
                ColorMask[ColorIndex].bluemask = (byte)ColorIndex;
                ColorMask[ColorIndex].rgbReserved = 0;

                TmpBinaryWriter.Write(StructToBytes(ColorMask[ColorIndex], Marshal.SizeOf(ColorMask[ColorIndex])));
            }

            //图片旋转，解决指纹图片倒立的问题
            RotatePic(buffer, nWidth, nHeight, ref ResBuf);

            //写图片
            TmpBinaryWriter.Write(ResBuf);

            FileStream.Close();
            TmpBinaryWriter.Close();
        }

        #endregion
    }
}