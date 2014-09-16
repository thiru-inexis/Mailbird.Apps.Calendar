using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    [ContentProperty("Converter")]
    public abstract class ChainedConverter: IValueConverter
    {
        public IValueConverter Converter { get; set; }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Converter != null)
                value = Converter.Convert(value, targetType, parameter, culture);
            return Convert(value, targetType, parameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Converter != null)
                value = Converter.ConvertBack(value, targetType, parameter, culture);
            return ConvertBack(value, targetType, parameter, culture);
        }


        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }


}
