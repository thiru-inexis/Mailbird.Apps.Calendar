﻿using System.Collections.ObjectModel;
﻿using DevExpress.Mvvm.POCO;
﻿using DevExpress.Xpf.Scheduler;
﻿using Mailbird.Apps.Calendar.Engine;
 ﻿using System;
using System.Collections.Generic;
﻿using System.Linq;
﻿using DevExpress.Xpf.Core.Native;
﻿using Mailbird.Apps.Calendar.Engine.Interfaces;
﻿using Mailbird.Apps.Calendar.Engine.Metadata;
﻿using Mailbird.Apps.Calendar.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Mailbird.Apps.Calendar.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constants

        private const int APPOINTMENT_SYNC_PERIOD = 1 * 60 * 1000;

        #endregion


        #region PrivateProps

        private readonly Dictionary<object, Appointment> _appointments = new Dictionary<object, Appointment>();

        private readonly CalendarsCatalog _calendarsCatalog = new CalendarsCatalog();

        private readonly ObservableCollection<TreeData> _treeData = new ObservableCollection<TreeData>();

        private CancellationTokenSource _syncCts = new CancellationTokenSource();

        #endregion PrivateProps

        #region PublicProps

        public FlyoutViewModel FlyoutViewModel { get; private set; }

        public ObservableCollection<Appointment> AppointmentCollection { get; private set; }

        public ObservableCollection<TreeData> TreeData
        {
            get { return _treeData; }
        }        

        #endregion PublicProps

        public MainWindowViewModel()
        {
            AppointmentCollection = new ObservableCollection<Appointment>();

            FlyoutViewModel = new FlyoutViewModel
            {
                AddAppointmentAction = AddAppointment,
                UpdateAppointmentAction = UpdateAppointment,
                RemoveAppointmentAction = RemoveAppointment
            };

            var providers = _calendarsCatalog.GetProviders;
            foreach (var provider in providers)
            {
                AddElementToTree(provider);
                var calenders = provider.GetCalendars();
                foreach (var calendar in calenders)
                {
                    AddElementToTree(calendar);
                }
            }

            // Start Async appointment sync
            AsyncSync();
        }



        #region Synchronization

        /// <summary>
        /// Cancel an ongoing appointment sync
        /// </summary>
        public void CancelAsyncSync()
        {
            if (_syncCts != null) { _syncCts.Cancel(); }
        }


        /// <summary>
        /// Asynchronous appointment sync
        /// </summary>
        public void AsyncSync()
        {
            try
            {
                if (_syncCts == null) { _syncCts = new CancellationTokenSource(); }
                var cancelToken = _syncCts.Token;


                Task.Factory.StartNew(()=>
                {
                    while (true)
                    {
                        cancelToken.ThrowIfCancellationRequested();

                        //If any exceptions are throen from the sync, suppress them
                        try
                        {
                            Sync();
                        }
                        catch { }

                        Thread.Sleep(APPOINTMENT_SYNC_PERIOD);
                    }
                }, cancelToken);
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.WriteLine(exp.ToString());
            }
        }


        /// <summary>
        /// To Do on Sync
        /// </summary>
        public void Sync()
        {
            // Add new appointments if, appointment id is not already in list
            var appointments = _calendarsCatalog.GetCalendarAppointments();
            foreach (var appointment in appointments)
            {
                if (!_appointments.ContainsKey(appointment.Id))
                {
                    _appointments.Add(appointment.Id, appointment);

                    Action<Appointment> addMethod = AppointmentCollection.Add;
                    Application.Current.Dispatcher.BeginInvoke(addMethod, appointment);
                    RaisePropertyChanged(() => AppointmentCollection);
                }
            }
            // For later use
            var calendars = _calendarsCatalog.GetCalendars();
        }


        #endregion


        public void AddAppointment(Appointment appointment)
        {
            AppointmentCollection.Add(appointment);
            if (appointment.Id == null || _appointments.ContainsKey(appointment.Id))
                appointment.Id = Guid.NewGuid();
            if (appointment.Calendar == null)
                appointment.Calendar = _calendarsCatalog.DefaultCalendar;
            _appointments.Add(appointment.Id, appointment);
            _calendarsCatalog.InsertAppointment(appointment);
        }

        public void UpdateAppointment(object appointmentId, Appointment appointment)
        {
            var appointmentToUpdate = _appointments[appointmentId];
            AppointmentCollection.Remove(appointmentToUpdate);
            AppointmentCollection.Add(appointment);
            _appointments[appointmentId] = appointment;
            _calendarsCatalog.UpdateAppointment(appointment);
        }

        public void RemoveAppointment(object appintmentId)
        {
            AppointmentCollection.Remove(_appointments[appintmentId]);
            _calendarsCatalog.RemoveAppointment(_appointments[appintmentId]);
            _appointments.Remove(appintmentId);
        }

        public void AppointmentOnViewChanged(Appointment appointment)
        {
            var app = AppointmentCollection.First(f => f.Id.ToString() == appointment.Id.ToString());
            appointment.ReminderInfo = app.ReminderInfo;
            appointment.Calendar = app.Calendar;
            UpdateAppointment(appointment.Id, appointment);
        }

        private void AddElementToTree(object element)
        {
            if (element is ICalendarProvider)
            {
                TreeData.Add(new TreeData
                {
                    DataType = TreeDataType.Provider,
                    Data = element,
                    Name = (element as ICalendarProvider).Name,
                    ParentID = "0"
                });
            }
            if (element is Mailbird.Apps.Calendar.Engine.Metadata.Calendar)
            {
                TreeData.Add(new TreeData
                {
                    DataType = TreeDataType.Calendar,
                    Data = element,
                    Name = (element as Mailbird.Apps.Calendar.Engine.Metadata.Calendar).Name,
                    ParentID = (element as Mailbird.Apps.Calendar.Engine.Metadata.Calendar).Provider
                });
            }
        }

        public void OpenInnerFlyout(SchedulerControl scheduler)
        {
            FlyoutViewModel.SelectedStartDateTime = scheduler.SelectedInterval.Start;
            FlyoutViewModel.SelectedEndDateTime = scheduler.SelectedInterval.End;
            FlyoutViewModel.IsOpen = true;
        }

        public void CloseInnerFlyout()
        {
            if (FlyoutViewModel.IsEdited)
            {
                FlyoutViewModel.OkCommandeExecute();
            }
            else
            {
                FlyoutViewModel.IsOpen = false;
            }
        }
    }
}
