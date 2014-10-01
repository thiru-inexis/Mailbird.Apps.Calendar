using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using DevExpress.XtraScheduler.Native;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    public class DateTimeToStringConverter : ChainedConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime)) { return value; }
            var param = ((parameter is string) && !string.IsNullOrEmpty((string)parameter)) ? (string)parameter : null;
            return (param == null) ? ((DateTime)value).ToShortDateString() : ((DateTime)value).ToString(param);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
