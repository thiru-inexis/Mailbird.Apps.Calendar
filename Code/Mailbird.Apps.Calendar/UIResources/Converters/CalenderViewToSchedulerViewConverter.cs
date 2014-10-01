using System;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DevExpress.XtraScheduler;
using Mailbird.Apps.Calendar.UIModels;
using System.Collections.Generic;
using Mailbird.Apps.Calendar.Enums;

namespace Mailbird.Apps.Calendar.UIResources.Converters
{
    public class CalenderViewToSchedulerViewConverter : ChainedConverter
    {
        private static List<CalenderView> _calenderViewsMap = new List<CalenderView> { CalenderView.Day, CalenderView.WorkWeek, CalenderView.Week, CalenderView.Month };
        private static List<SchedulerViewType> _schedulerViewsMap = new List<SchedulerViewType> { SchedulerViewType.Day, SchedulerViewType.WorkWeek, SchedulerViewType.Week, SchedulerViewType.Month };

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DependencyProperty.UnsetValue;
            if (value != null && (value is CalenderView))
            {
                var searchVal = (CalenderView)value;
                if (_calenderViewsMap.Contains(searchVal))
                {
                    result = _schedulerViewsMap[_calenderViewsMap.IndexOf(searchVal)];
                }
            }

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DependencyProperty.UnsetValue;
            if (value != null && (value is SchedulerViewType))
            {
                var searchVal = (SchedulerViewType)value;
                if (_schedulerViewsMap.Contains(searchVal))
                {
                    result = _calenderViewsMap[_schedulerViewsMap.IndexOf(searchVal)];
                }
            }

            return result;
        }
    }
}
