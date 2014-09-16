using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan)) return null;
            DateTime time = DateTime.Today.Add((TimeSpan)value);
            return time.ToString("hh:mm tt");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) { return null; }
            var result = DateTime.Today;

            DateTime.TryParseExact((string)value, new string[] { "hh:mm tt", "hh:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);

            return result.TimeOfDay;
        }
    }



    public class StringToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) { return null; }
            var result = DateTime.Today;
            DateTime.TryParseExact((string)value, new string[] { "hh:mm tt", "hh:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            return result.TimeOfDay;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (!(value is TimeSpan)) return null;
            DateTime time = DateTime.Today.Add((TimeSpan)value);
            return time.ToString("hh:mm tt");
        }
    }
}
