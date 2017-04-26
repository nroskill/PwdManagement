using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace PwdManagement
{
    /// <summary>
    /// 实现了一大坨转换器
    /// </summary>
    /// 
    #region value[7]为true返回“验证”,false返回“设定”
    public class methodConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)1)) == ((byte)1) ? "验证" : "设定";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value[6]为true返回“验证”,false返回“设定”
    public class methodConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)2)) == ((byte)2) ? "验证" : "设定";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value[5]为true返回“验证”,false返回“设定”
    public class methodConverter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)4)) == ((byte)4) ? "验证" : "设定";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value[4]为true返回“验证”,false返回“设定”
    public class methodConverter4 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)8)) == ((byte)8) ? "验证" : "设定";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value[7]为true返回#6F5886,false返回#EA5050
    public class methodColorConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)1)) == ((byte)1) ? (new SolidColorBrush(Color.FromRgb(111, 88, 134))) : (new SolidColorBrush(Color.FromRgb(235, 79, 80)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value[6]为true返回#6F5886,false返回#EA5050
    public class methodColorConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)2)) == ((byte)2) ? (new SolidColorBrush(Color.FromRgb(111, 88, 134))) : (new SolidColorBrush(Color.FromRgb(235, 79, 80)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value[5]为true返回#6F5886,false返回#EA5050
    public class methodColorConverter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)4)) == ((byte)4) ? (new SolidColorBrush(Color.FromRgb(111, 88, 134))) : (new SolidColorBrush(Color.FromRgb(235, 79, 80)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value[4]为true返回#6F5886,false返回#EA5050
    public class methodColorConverter4 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)8)) == ((byte)8) ? (new SolidColorBrush(Color.FromRgb(111, 88, 134))) : (new SolidColorBrush(Color.FromRgb(235, 79, 80)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region 返回value[7]
    public class methodBoolConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)1)) == ((byte)1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region 返回value[6]
    public class methodBoolConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)2)) == ((byte)2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region 返回value[5]
    public class methodBoolConverter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)4)) == ((byte)4);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region 返回value[4]
    public class methodBoolConverter4 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return (((byte)value) & ((byte)8)) == ((byte)8);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region 返回(bool)value[7]
    public class passwordsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            return ((byte)value & (byte)1) != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value[7]为true返回Visible, false返回Collapsed
    public class ispasswordsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            if( ( ((byte)value) & ((byte)1) ) == ((byte)1) )
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region 返回value是否大于等于当前tabitem的power
    public class powerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            if (Shell.currentLevel < 0 || Shell.currentLevel >= Shell.tabItem.Count)
                return false;
            return ((int)value) >= Shell.tabItem[Shell.currentLevel].power;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region 若value大于等于当前tabitem的power，返回Visible，否则返回Collapsed
    public class visibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            if (Shell.currentLevel < 0 || Shell.currentLevel >= Shell.tabItem.Count)
                return Visibility.Collapsed;
            if (((int)value) >= Shell.tabItem[Shell.currentLevel].power )
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region 若value大于等于当前tabitem的power，返回Collapsed，否则返回Visible
    public class unvisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            if (Shell.currentLevel < 0 || Shell.currentLevel >= Shell.tabItem.Count)
                return Visibility.Visible;
            if (((int)value) >= Shell.tabItem[Shell.currentLevel].power)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region 返回value对应的复杂度（1-4）的颜色
    public class passpowerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            var r = passwordPower.power(value as string);
            if (r == 0)
                return new SolidColorBrush(Color.FromRgb(255, 255, 255));
            if (r == 1)
                return new SolidColorBrush(Color.FromRgb(255, 0, 0));
            if (r == 2)
                return new SolidColorBrush(Color.FromRgb(255, 255, 0));
            if (r == 3)
                return new SolidColorBrush(Color.FromRgb(0, 255, 0));
            return new SolidColorBrush(Color.FromRgb(0, 0, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value为true时返回Visible，否则返回Collapsed
    public class truevisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if( value == null )
                throw new Exception("转换器函数参数为空");
            if ( ((bool)value) == true)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value为false时返回Visible，否则返回Collapsed
    public class falsevisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            if (((bool)value) == true)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value为""时返回false，否则返回true
    public class nullboolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value as string == "")
                return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("不可能出现的异常");
        }
    }

    #endregion

    #region value为true时返回#F87912，否则返回#02B4FE
    public class isSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new Exception("转换器函数参数为空");
            if (((bool)value) == true)
                return new SolidColorBrush(Color.FromRgb(248, 121, 18));
            return new SolidColorBrush(Color.FromRgb(2, 180, 254));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception("不可能出现的异常");
        }
    }

    #endregion
}
