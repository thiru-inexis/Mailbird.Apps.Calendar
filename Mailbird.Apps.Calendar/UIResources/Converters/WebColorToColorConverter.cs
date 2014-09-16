using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    public class WebColorToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return null;
            return (Color)ColorConverter.ConvertFromString((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color)) { return null; }
            var color = (Color)value;
            return string.Format("#{0}{1}{2}", color.R.ToString("X2"), color.G.ToString("X2"), color.B.ToString("X2"));
        }
    }
}
