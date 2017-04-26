using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;

namespace PwdManagement
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class checkUser : INotifyPropertyChanged
    {
        #region 属性

        public event PropertyChangedEventHandler PropertyChanged;

        public List<byte[]> checkData;

        public Bitmap userFace;

        private string _userName;

        public enum methodtype
        {
            pass = 1,
            finger = 2,
            voice = 4,
            face = 8
        }
        /// <summary>
        /// 1 为 密码
        /// 2 为 指纹
        /// 4 为 声纹
        /// 8 为 人脸
        /// </summary>
        private byte _methodCount = (byte)0 ;
        public byte methodCount
        {
            get
            {
                return _methodCount;
            }
            set
            {
                _methodCount = value;
                OnPropertyChanged("methodCount");
            }
        }

        private int _facePower = 0;

        public int facePower
        {
            get
            {
                return _facePower;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new Exception("参数大小异常");
                _facePower = value;
                OnPropertyChanged("facePower");
                OnPropertyChanged("power");
            }
        }

        private int _passPower = 0;

        public int passPower
        {
            get
            {
                return _passPower;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new Exception("参数大小异常");
                _passPower = value;
                OnPropertyChanged("passPower");
                OnPropertyChanged("power");
            }
        }

        private int _fingerPower = 0;

        public int fingerPower
        {
            get
            {
                return _fingerPower;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new Exception("参数大小异常");
                _fingerPower = value;
                OnPropertyChanged("fingerPower");
                OnPropertyChanged("power");
            }
        }

        private int _voicePower = 0;

        public int voicePower
        {
            get
            {
                return _voicePower;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new Exception("参数大小异常");
                _voicePower = value;
                OnPropertyChanged("voicePower");
                OnPropertyChanged("power");
            }
        }

        private int _passValue;

        public int passValue
        {
            get
            {
                return _passValue;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new Exception("参数大小异常");
                _passValue = value;
                OnPropertyChanged("passValue");
                OnPropertyChanged("power");
            }
        }

        private int _faceValue;

        public int faceValue
        {
            get
            {
                return _faceValue;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new Exception("参数大小异常");
                _faceValue = value;
                OnPropertyChanged("faceValue");
                OnPropertyChanged("power");
            }
        }

        private int _voiceValue;

        public int voiceValue
        {
            get
            {
                return _voiceValue;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new Exception("参数大小异常");
                _voiceValue = value;
                OnPropertyChanged("voiceValue");
                OnPropertyChanged("power");
            }
        }

        private int _fingerValue;

        public int fingerValue
        {
            get
            {
                return _fingerValue;
            }
            set
            {
                if (value > 100 || value < 0)
                    throw new Exception("参数大小异常");
                _fingerValue = value;
                OnPropertyChanged("fingerValue");
                OnPropertyChanged("power");
            }
        }

        public int power
        {
            get
            {
                if ((passPower * passValue + facePower * faceValue + voicePower * voiceValue + fingerPower * fingerValue) > 10000)
                    throw new Exception("总辨识度超出了100");
                return (passPower * passValue + facePower * faceValue + voicePower * voiceValue + fingerPower * fingerValue) / 100;
            }
            set
            {
                OnPropertyChanged("power");
            }
        }

        public string userName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (Encoding.Default.GetBytes(value).Length <= 8)
                {
                    _userName = value;
                    OnPropertyChanged("userName");
                }
            }
        }

        #endregion

        #region 方法

        public checkUser()
        {
            userName = "";
            checkData = new List<byte[]>();
            for (var i = 0; i < 4; i++)
            {
                checkData.Add(new byte[1]);
            }
            passValue = 0;
            passPower = 0;
            faceValue = 0;
            facePower = 0;
            voiceValue = 0;
            voicePower = 0;
            fingerValue = 0;
            fingerPower = 0;
            userFace = null;
        }
        protected void OnPropertyChanged(string caller = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        /// <summary>
        /// 将所有验证方式权重重置，未设定的验证方式权重为0，已设定的验证方式权重为100/n，n为验证方式的数量
        /// </summary>
        public void resetPower()
        {
            var num = 0;
            passPower = 0;
            fingerPower = 0;
            voicePower = 0;
            facePower = 0;
            if (getMethodInfo(methodtype.pass))
            {
                num++;
            }
            if (getMethodInfo(methodtype.finger))
            {
                num++;
            }
            if (getMethodInfo(methodtype.voice))
            {
                num++;
            }
            if (getMethodInfo(methodtype.face))
            {
                num++;
            }
            if (getMethodInfo(methodtype.face))
            {
                facePower = 100 / num;
            }
            if (getMethodInfo(methodtype.finger))
            {
                fingerPower = 100 / num;
            }
            if (getMethodInfo(methodtype.voice))
            {
                voicePower = 100 / num;
            }
            if (getMethodInfo(methodtype.pass))
            {
                passPower = 100 / num;
            }
        }

        public bool getMethodInfo(methodtype x)
        {
            return (methodCount & (byte)x) != 0;
        }

        public void setMethoInfo(methodtype a, bool b)
        {
            if (b == true)
                methodCount = (byte)(methodCount | (byte)a);
            else
                methodCount = (byte)(methodCount & (byte.MaxValue - (byte)a));
            resetPower();
        }

        public bool isNewUser()
        {
            return methodCount == 0;
        }

        #endregion
    }
}
