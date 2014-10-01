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
    public class AppointmentStartEndTimeToShortStringConverter : IMultiValueConverter
    {
        private const string _12H_TIME_FORMAT_H = "%h"; // Escaped
        private const string _12H_TIME_FORMAT_HA = "ht";
        private const string _12H_TIME_FORMAT_HM = "h:mm";
        private const string _12H_TIME_FORMAT_HMA = "h:mmt";

        private const string _24H_TIME_FORMAT_H = "%H"; // Escaped
        private const string _24H_TIME_FORMAT_HM = "H:mm";


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = "";
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;

            if (values == null || values.Length == 0 || !(values[0] is Mailbird.Apps.Calendar.ViewModels.AppointmentDetailsPopupViewModel)) { return values; }
            if (values[0] is Mailbird.Apps.Calendar.ViewModels.AppointmentDetailsPopupViewModel)
            {
                var data = (values[0] as Mailbird.Apps.Calendar.ViewModels.AppointmentDetailsPopupViewModel);
                startTime = data.StartDateTime;
                endTime = data.EndDateTime;
            }


            var sysFormat = System.Globalization.DateTimeFormatInfo.InvariantInfo;
            bool is24HourClock = !sysFormat.ShortTimePattern.Contains("tt");


            var isStartEndSameMedian = ((startTime.TimeOfDay.Hours < 12 && endTime.TimeOfDay.Hours < 12) ||
                                        (startTime.TimeOfDay.Hours >= 12 && endTime.TimeOfDay.Hours >= 12));
            var showEndTime = (endTime.TimeOfDay.Subtract(startTime.TimeOfDay).TotalMinutes > 30);

            result = string.Format("{0}{1}",
                ((is24HourClock)
                ? startTime.ToString(string.Format("{0}", (showEndTime && startTime.Minute == 0) ? _24H_TIME_FORMAT_H : _24H_TIME_FORMAT_HM))
                : startTime.ToString(string.Format("{0}", (!showEndTime || !isStartEndSameMedian)
                ? (startTime.Minute == 0) ? _12H_TIME_FORMAT_HA : _12H_TIME_FORMAT_HMA
                : (startTime.Minute == 0) ? _12H_TIME_FORMAT_H : _12H_TIME_FORMAT_HM))),

                (!showEndTime
                ? ""
                : string.Format(" - {0}", endTime.ToString(
                is24HourClock
                ? (endTime.Minute == 0) ? _24H_TIME_FORMAT_H : _24H_TIME_FORMAT_HM
                : (endTime.Minute == 0) ? _12H_TIME_FORMAT_HA : _12H_TIME_FORMAT_HMA))));

            return result;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
