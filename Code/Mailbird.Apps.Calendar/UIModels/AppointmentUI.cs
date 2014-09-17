using Mailbird.Apps.Calendar.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.UIModels
{
    public class AppointmentUI : NotificationObject
    {
        private Engine.Metadata.Appointment _bm;
        private bool _isAllDayAppointment;

        public Engine.Metadata.Appointment BaseModel
        {
            get { return _bm; }
        }



        public string Id
        {
            get { return _bm.Id; }
            set
            {
                _bm.Id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Summary
        {
            get { return _bm.Summary; }
            set
            {
                _bm.Summary = value;
                RaisePropertyChanged(() => Summary);
            }
        }

        public string Location
        {
            get { return _bm.Location; }
            set
            {
                _bm.Location = value;
                RaisePropertyChanged(() => Location);
            }
        }

        public DateTime StartDateTime
        {
            get { return _bm.StartTime; }
            set
            {
                //var newEndDate = EndDateTime.Add(value.Subtract(StartDateTime));
                _bm.StartTime = value;
                RaisePropertyChanged(() => StartDateTime);
                //EndDateTime = newEndDate;
            }
        }

        public DateTime EndDateTime
        {
            get { return _bm.EndTime; }
            set
            {
                //if (value > StartDateTime) { return; }
                _bm.EndTime = value;
                RaisePropertyChanged(() => EndDateTime);
            }
        }


        public bool IsAllDayAppointment
        {
            get { return _isAllDayAppointment; }
            set
            {
                _isAllDayAppointment = value;
                RaisePropertyChanged(() => IsAllDayAppointment);
            }
        }


        public Engine.Enums.AppointmentTransparency Transparency
        {
            get { return _bm.Transparency; }
            set
            {
                _bm.Transparency = value;
                RaisePropertyChanged(() => Transparency);
            }
        }

        public TimeSpan PreReminderDuration { get; set; }


        public string ReminderInfo
        {
            get 
            { 
                string result = null;
                if (_bm.Reminders != null && _bm.Reminders.Count > 0)
                { 
                    var apt = new DevExpress.XtraScheduler.Appointment(DevExpress.XtraScheduler.AppointmentType.Normal, _bm.StartTime, _bm.EndTime);
                    apt.HasReminder = true;

                    foreach(var rem in _bm.Reminders)
                    {
                        var _rem = apt.CreateNewReminder();
                        _rem.TimeBeforeStart = rem.Duration;
                        apt.Reminders.Add(_rem);
                        var helper = DevExpress.XtraScheduler.Xml.ReminderCollectionXmlPersistenceHelper.CreateSaveInstance(apt, DevExpress.XtraScheduler.DateSavingType.LocalTime);
                        result = helper.ToXml();
                    }           
                }
                return result;
            }
            set 
            {
                var apt = new DevExpress.XtraScheduler.Appointment(DevExpress.XtraScheduler.AppointmentType.Normal, _bm.StartTime, _bm.EndTime);
                var helper = DevExpress.XtraScheduler.Xml.ReminderCollectionXmlPersistenceHelper.ObjectFromXml(apt, value, DevExpress.XtraScheduler.DateSavingType.LocalTime);
                _bm.Reminders.Clear();

                foreach (var rem in helper)
                {


                }

                var sample = value;
            }
        }

        public string Description
        {
            get { return _bm.Description; }
            set
            {
                _bm.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }


        public AppointmentUI()
            :base()
        {
            _bm = new Engine.Metadata.Appointment();
            _isAllDayAppointment = StartDateTime.TimeOfDay.Equals(EndDateTime.TimeOfDay).Equals(TimeSpan.Zero);
        }

        public AppointmentUI(Engine.Metadata.Appointment appointment)
            : this()
        {
            _bm = appointment;
            _isAllDayAppointment = StartDateTime.TimeOfDay.Equals(EndDateTime.TimeOfDay).Equals(TimeSpan.Zero);
        }


    }
}
