using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DevExpress.XtraScheduler;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    public class ListItemToViewTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;
            switch (value.ToString())
            {
                case "Day":
                    return 0;
                case "Week":
                    return 1;
                case "Month":
                    return 2;
                case "WorkWeek":
                    return 3;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;
            int index;
            if (!int.TryParse(value.ToString(), out index)) return DependencyProperty.UnsetValue;
            switch (index)
            {
                case 0:
                    return SchedulerViewType.Day ;
                case 1:
                    return SchedulerViewType.Week ;
                case 2:
                    return SchedulerViewType.Month ;
                case 3:
                    return SchedulerViewType.WorkWeek ;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }
    }
}
