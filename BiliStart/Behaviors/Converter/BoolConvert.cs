﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace BiliStart.Behaviors.Converter
{
    public class BoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var value2= (bool)value;
            return value2;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
