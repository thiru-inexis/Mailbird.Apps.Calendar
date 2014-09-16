using System;
using System.Globalization;
using System.Windows.Data;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    public class InvertBoolValueConverter : ChainedConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
