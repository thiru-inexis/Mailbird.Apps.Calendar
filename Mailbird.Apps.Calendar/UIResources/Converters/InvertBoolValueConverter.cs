﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    public class InvertBoolValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
