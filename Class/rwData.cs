using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Security.Cryptography;
using System.Drawing;
using System.Threading;
using PwdManagement.login;

namespace PwdManagement
{
    /// <summary>
    /// 负责文件读写，使用前需对user和data赋值
    /// </summary>
    public class rwData
    {
        #region 生成bitmap对象

        /// <summary>
        /// 生成bitmap对象
        /// </summary>
        /// <param name="x">对应bitmap的二进制</param>
        /// <returns>bitmap对象</returns>
        public static Bitmap getBmp(byte[] data)
        {
            var temp = new byte[4];
            Buffer.BlockCopy(data, 0, temp, 0, 4);
            var W = getint(temp);
            Buffer.BlockCopy(data, 4, temp, 0, 4);
            var H = getint(temp);
            var img = new Bitmap(W, H);
            for (int i = 0; i < W; i++)
                for (int j = 0; j < H; j++)
                {
                    var val = Color.FromArgb((int)data[8 + i * H * 4 + j * 4 + 0], (int)data[8 + i * H * 4 + j * 4 + 1], (int)data[8 + i * H * 4 + j * 4 + 2], (int)data[8 + i * H * 4 + j * 4 + 3]);
                    img.SetPixel(i, j, val);
                }
            return img;
        }

        #endregion

        #region 生成bitmap的byte[]

        /// <summary>
        /// 生成bitmap的byte[]
        /// </summary>
        /// <param name="x">对应的bitmap对象</param>
        /// <returns>生成的byte[]</returns>
        public static byte[] getbyte(Bitmap img)
        {
            var W = img.Width;
            var H = img.Height;
            var data = new byte[ 8 + W * H * 4];
            Buffer.BlockCopy(format(W), 0, data, 0, 4);
            Buffer.BlockCopy(format(H), 0, data, 4, 4);
            for (int i = 0; i < W; i++)
                for (int j = 0; j < H; j++)
                {
                    data[ 8 + i * H * 4 + j * 4 + 0] = img.GetPixel(i, j).A;
                    data[ 8 + i * H * 4 + j * 4 + 1] = img.GetPixel(i, j).R;
                    data[ 8 + i * H * 4 + j * 4 + 2] = img.GetPixel(i, j).G;
                    data[ 8 + i * H * 4 + j * 4 + 3] = img.GetPixel(i, j).B;
                }
            return data;
        }

        #endregion

        #region 实现了一小坨为语音识别转换数据格式的方法

        public static byte[] convertToByteForVoice(List<byte[]> x)
        {
            var re = new List<byte>();
            re.AddRange(rwData.format(x.Count));
            foreach (var i in x)
            {
                re.AddRange(rwData.format(i.Length));
                re.AddRange(i);
            }
            return re.ToArray();
        }

        public static List<byte[]> convertToListForVoice(byte[] x)
        {
            var re = new List<byte[]>();
            var temp = new byte[4];
            var offset = 0;
            Buffer.BlockCopy(x, 0, temp, 0, 4);
            offset += 4;
            var num = getint(temp);
            for (var i = 0; i < num; i++)
            {
                temp = new byte[4];
                Buffer.BlockCopy(x, offset, temp, 0, 4);
                offset += 4;
                var length = getint(temp);

                temp = new byte[length];
                Buffer.BlockCopy(x, offset, temp, 0, length);
                offset += length;
                re.Add(temp);
            }
            return re;
        }

        #endregion

        #region MD5相关方法
        /// <summary>
        /// 生成salt和md5组合的byte[]
        /// </summary>
        /// <param name="pass">要加密的密钥</param>
        /// <returns>描述中的byte[]</returns>
        public static byte[] md5_create(string pass)
        {
            var salt = Guid.NewGuid().ToString();
            return rwData.format(salt, salt.Length).Concat(rwData.md5_encrypt(pass + salt)).ToArray();
        }
        /// <summary>
        /// 测试密钥是否匹配
        /// </summary>
        /// <param name="pass">要测试的密钥</param>
        /// <param name="x">对应的salt+md5组合的byte[]</param>
        /// <returns>匹配时返回true，否则返回false</returns>
        public static bool md5_test(string pass, byte[] x)
        {
            var saltbyte = new byte[36];
            if (x.Length < 52)
            {
                return false;
            }
            Buffer.BlockCopy(x, 0, saltbyte, 0, 36);
            var encryptbyte = md5_encrypt(pass + rwData.getstring(saltbyte));
            for (var i = 0; i < encryptbyte.Length; i++ )
            {
                if (encryptbyte[i] != x[i + 36])
                    return false;
            }
            return true;
        }
        public static byte[] md5_encrypt(string a)
        {
            return MD5.Create().ComputeHash(UTF8Encoding.UTF8.GetBytes(a));
        }

        #endregion

        #region AES加密和解密

        /// <summary>
        /// 用于AES加密的key
        /// </summary>
        private static string key = "ae125efkk4454eeff444ferfkny6oxi8";
        public static byte[] AES_Encrypt(byte[] toEncryptArray)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        }

        public static byte[] AES_Decrypt(byte[] toEncryptArray)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        }

        #endregion

        #region 实现了一大坨转byte[]的方法

        static public byte[] format(long a)
        {
            return BitConverter.GetBytes(a);
        }

        static public byte[] format(byte a)
        {
            byte[] r = new byte[1];
            r[0] = a;
            return r;
        }

        static public byte[] format(string a, int len)
        {
            byte[] r = Encoding.UTF8.GetBytes(a);
            if (r.Length > len)
                throw new Exception("Too long string");
            byte[] re = new byte[len];
            r.CopyTo(re, 0);
            for (var i = r.Length; i < len; i++)
                re[i] = (byte)0;
            return re;
        }

        static public byte[] format(double a)
        {
            return BitConverter.GetBytes(a);
        }

        static public byte[] format(int a)
        {
            return BitConverter.GetBytes(a);
        }

        #endregion

        #region 实现了一大坨从byte[]转回正常值的方法

        static public long getlong(byte[] a)
        {
            return BitConverter.ToInt64(a, 0);
        }

        static public int getint(byte[] a)
        {
            return BitConverter.ToInt32(a, 0);
        }

        static public double getdouble(byte[] a)
        {
            return BitConverter.ToDouble(a, 0);
        }

        static public string getstring(byte[] a)
        {
            return Encoding.UTF8.GetString(a).Trim('\0');
        }

        #endregion

        #region 实现了写文件的方法

        /// <summary>
        /// 写文件的方法
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        static public bool writeFile()
        {
            var path = Directory.GetCurrentDirectory() + @"\data";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = string.Format(@"{0}\{1}", path, Shell.userInfo.userName);

            List<byte> tempstr = new List<byte>();

            try
            {   
                #region 写入用户信息

                tempstr.AddRange(format(Shell.userInfo.userName, 8));

                if (Shell.userInfo.userFace == null)
                {
                    tempstr.AddRange(format(0));
                }
                else
                {
                    var face = getbyte(Shell.userInfo.userFace);
                    tempstr.AddRange(format(face.Length));
                    tempstr.AddRange(face);
                }

                tempstr.AddRange(format(Shell.userInfo.methodCount));

                for (var i = 0; i < 4 ; i++ )
                {
                    tempstr.AddRange(format(Shell.userInfo.checkData[i].Length));
                }

                tempstr.AddRange(format(Shell.userInfo.passPower));
                tempstr.AddRange(format(Shell.userInfo.fingerPower));
                tempstr.AddRange(format(Shell.userInfo.voicePower));
                tempstr.AddRange(format(Shell.userInfo.facePower));

                foreach (var i in Shell.userInfo.checkData)
                {
                    tempstr.AddRange(i);
                }

                #endregion

                #region 写入用户数据

                tempstr.AddRange(format(Shell.tabItem.Count));

                foreach(var i in Shell.tabItem)
                {
                    tempstr.AddRange(format(i.power));
                }

                tempstr.AddRange(format(Shell.userData.Count));

                foreach (var j in Shell.userData)
                {
                    tempstr.AddRange(format(j.Name, 14));
                    if (j.Data == null)
                        j.Data = "";
                    tempstr.AddRange(format(j.Data, 140));
                    tempstr.AddRange(format(j.Level));
                }

                #endregion

                var fs = new FileStream(path, FileMode.Create);
                var bw = new BinaryWriter(fs);

                bw.Write(AES_Encrypt(tempstr.ToArray()));

                bw.Close();
                fs.Close();
            }
            catch
            {
                new ResultWindow(ResultWindow.infotype.Error, "写入文件时发生异常", "返回").ShowDialog();
                return false;
            }

            return true;
        }

        #endregion

        #region 实现了读文件的方法

        /// <summary>
        /// 实现了读文件的方法
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        static public bool readFile()
        {
            var path = string.Format(@"{0}\data\{1}", Directory.GetCurrentDirectory(), Shell.userInfo.userName);

            if (!File.Exists(path)) 
            {
                return false;
            }

            try
            {
                var deFile = AES_Decrypt(File.ReadAllBytes(path));
                int pointer = 0;

                #region 读取用户信息

                byte[] temp;

                temp = new byte[8];
                Buffer.BlockCopy(deFile, pointer, temp, 0, 8);
                if( Shell.userInfo.userName != getstring(temp))
                {
                    throw new Exception("姓名不匹配");
                }
                pointer += 8;

                temp = new byte[4];
                Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                pointer += 4;
                var l = getint(temp);

                if (l != 0)
                {
                    temp = new byte[l];
                    Buffer.BlockCopy(deFile, pointer, temp, 0, l);
                    pointer += l;
                    Shell.userInfo.userFace = getBmp(temp);
                }

                temp = new byte[1];
                Buffer.BlockCopy(deFile, pointer, temp, 0, 1);
                Shell.userInfo.methodCount = temp[0];
                pointer += 1;

                List<int> t = new List<int>();
                temp = new byte[4];
                for (var i = 0; i < 4; i++ )
                {
                    Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                    t.Add(getint(temp));
                    pointer += 4;
                }

                temp = new byte[4];
                Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                Shell.userInfo.passPower = getint(temp);
                pointer += 4;
                Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                Shell.userInfo.fingerPower = getint(temp);
                pointer += 4;
                Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                Shell.userInfo.voicePower = getint(temp);
                pointer += 4;
                Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                Shell.userInfo.facePower = getint(temp);
                pointer += 4;

                for (var i = 0; i < 4; i++)
                {
                    Shell.userInfo.checkData[i] = new byte[t[i]];
                    Buffer.BlockCopy(deFile, pointer, Shell.userInfo.checkData[i], 0, t[i]);
                    pointer += t[i];
                }

                #endregion

                #region 读取用户数据

                temp = new byte[4];
                Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                var num = getint(temp);

                pointer += 4;
                for (var i = 0; i < num; i++ )
                {
                    temp = new byte[4];
                    Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                    Shell.tabItem.Add(new tabItem() { power = getint(temp), level = i });
                    pointer += 4;
                }

                temp = new byte[4];
                Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                num = getint(temp);

                pointer += 4;
                byte[] name;
                byte[] data;
                for (var i = 0; i < num; i++ )
                {
                    name = new byte[14];
                    Buffer.BlockCopy(deFile, pointer, name, 0, 14);
                    pointer += 14;
                    data = new byte[140];
                    Buffer.BlockCopy(deFile, pointer, data, 0, 140);
                    pointer += 140;
                    temp = new byte[4];
                    Buffer.BlockCopy(deFile, pointer, temp, 0, 4);
                    pointer += 4;
                    Shell.userData.Add(new userData(getstring(name), getstring(data), getint(temp)) { isEditable = false });
                }

                #endregion
            }
            catch
            {
                new ResultWindow(ResultWindow.infotype.Error, "读取文件时发生异常", "返回").ShowDialog();
                return false;
            }
            return true;
        }

        #endregion
    }
}
