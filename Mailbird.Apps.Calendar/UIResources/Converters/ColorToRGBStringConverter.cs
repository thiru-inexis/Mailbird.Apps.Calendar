using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    public class ColorToRGBStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color)) return null;
            var color = (Color) value;
            var rgbString = string.Format("#{0}{1}{2}", color.R.ToString("X2"), color.G.ToString("X2"), color.B.ToString("X2"));
            return rgbString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
