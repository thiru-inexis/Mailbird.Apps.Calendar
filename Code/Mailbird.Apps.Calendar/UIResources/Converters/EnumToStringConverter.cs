using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DevExpress.XtraScheduler;
using Mailbird.Apps.Calendar.Engine.Extensions;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value ==  null || !(value is Enum)) return DependencyProperty.UnsetValue;
            return ((Enum)value).GetEnumDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
