﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PV6900.Wpf.Shared.Services
{
    public class InnerLoopCountTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? (string)string.Empty : ((int)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(int.TryParse((string)value,out int result))
            {
                return result;
            }
            else { return 0; }
        }
    }
}
