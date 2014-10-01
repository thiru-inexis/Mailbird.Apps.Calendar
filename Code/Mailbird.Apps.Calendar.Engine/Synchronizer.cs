using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Engine.CalenderServices;
using System.Threading;
using System.Threading.Tasks;
using Mailbird.Apps.Calendar.Engine.Interfaces;


namespace Mailbird.Apps.Calendar.Engine
{
    public class Synchronizer
    {

        #region Constants

        private const int APPOINTMENT_SYNC_PERIOD = 1 * 60 * 1000;

        #endregion

        private ICalendarServicesFacade _facade;

        public bool IsSynchronozing { get; private set; }
        private CancellationTokenSource _syncCts;


        public Synchronizer()
        {
            IsSynchronozing = false;
            _facade = CalendarServicesFacade.GetInstance();
            _syncCts = new CancellationTokenSource();
        }


        /// <summary>
        /// Cancel an ongoing appointment sync
        /// </summary>
        public void CancelAsyncSync()
        {
            if (_syncCts != null) { _syncCts.Cancel(); }
        }



        public void AsyncSync()
        {

            try
            {
                if (_syncCts == null) { _syncCts = new CancellationTokenSource(); }
                var cancelToken = _syncCts.Token;


                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        cancelToken.ThrowIfCancellationRequested();
                        Sync();
                        Thread.Sleep(APPOINTMENT_SYNC_PERIOD);
                    }
                }, cancelToken);
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.WriteLine(exp.ToString());
            }
        }


        public void Sync()
        {
            this.IsSynchronozing = true;

            try
            {
                this.OutBoundSync();
                this.InBoundSync();
            }
            catch 
            {
                var local = _facade.GetLocalService();
                lock (local)
                {
                    local.RollBack();
                }
            }

            this.IsSynchronozing = false;
        }



        /// <summary>
        /// Local changes are synced with the live services
        /// </summary>
        private void OutBoundSync()
        { 
            var _localStorage = _facade.GetLocalService();
            var users = _localStorage.UserInfoRepo.Get();


            foreach (var user in users)
            {
                #region Calendar Sync

                var pendingCalenders = _localStorage.CalendarRepo.Get(false)                          
                    .Where(a => a.LocalStorageState != Enums.LocalStorageDataState.Unchanged && a.UserId == user.Id)
                    .Select(a => a).ToList();

                if (pendingCalenders.Any())
                {
                    lock (_localStorage)
                    {
                        var provider = _facade.GetUserService(user.Id);
                        lock (provider)
                        {
                            foreach (var calender in pendingCalenders)
                            {
                                if (calender.LocalStorageState == Enums.LocalStorageDataState.Added)
                                {
                                    var response = provider.InsertCalendar(calender);
                                    _localStorage.CalendarRepo.Delete(calender, Enums.LocalStorageDataState.Unchanged);
                                    _localStorage.CalendarRepo.Add(response, Enums.LocalStorageDataState.Unchanged);

                                    /// On insert the calendar key will changed. And hence its related apointments 
                                    /// calendarId refernce has to be updated.

                                    var relatedAppointments = _localStorage.AppointmentRepo.Get(false).Where(m => m.CalendarId == calender.Id);
                                    foreach (var appointment in relatedAppointments)
                                    { 
                                        appointment.CalendarId = response.Id;
                                        _localStorage.AppointmentRepo.Update(appointment, appointment.LocalStorageState);
                                    }
                                }
                                else if (calender.LocalStorageState == Enums.LocalStorageDataState.Modified)
                                {
                                    var response = provider.UpdateCalendar(calender);
                                    _localStorage.CalendarRepo.Delete(calender, Enums.LocalStorageDataState.Unchanged);
                                    _localStorage.CalendarRepo.Add(response, Enums.LocalStorageDataState.Unchanged);
                                }
                                else if (calender.LocalStorageState == Enums.LocalStorageDataState.Deleted)
                                {
                                    var response = provider.DeleteCalendar(calender);
                                    _localStorage.CalendarRepo.Delete(calender, Enums.LocalStorageDataState.Unchanged);
                                }
                            }
                        }

                        _localStorage.SaveChanges();
                    }
                }

                #endregion
            }



            foreach (var user in users)
            {
                #region Appointments Sync

                var pendingAppointments = (from app in _localStorage.AppointmentRepo.Get(false)
                                           join cal in _localStorage.CalendarRepo.Get(false)
                                           on app.CalendarId equals cal.Id
                                           where app.LocalStorageState != Enums.LocalStorageDataState.Unchanged
                                           select app).ToList();


                if (pendingAppointments.Any())
                {
                    lock (_localStorage)
                    {
                        var provider = _facade.GetUserService(user.Id);
                        lock (provider)
                        {
                            foreach (var app in pendingAppointments)
                            {
                                if (app.LocalStorageState == Enums.LocalStorageDataState.Added)
                                {
                                    var changedAppointment = provider.InsertAppointment(app);
                                    _localStorage.AppointmentRepo.Delete(app, Enums.LocalStorageDataState.Unchanged);
                                    _localStorage.AppointmentRepo.Add(changedAppointment, Enums.LocalStorageDataState.Unchanged);
                                }
                                else if (app.LocalStorageState == Enums.LocalStorageDataState.Modified)
                                {
                                    var changedAppointment = provider.UpdateAppointment(app);
                                    _localStorage.AppointmentRepo.Delete(app, Enums.LocalStorageDataState.Unchanged);
                                    _localStorage.AppointmentRepo.Add(changedAppointment, Enums.LocalStorageDataState.Unchanged);
                                }
                                else if (app.LocalStorageState == Enums.LocalStorageDataState.Deleted)
                                {
                                    var status = provider.DeleteAppointment(app);
                                    _localStorage.AppointmentRepo.Delete(app, Enums.LocalStorageDataState.Unchanged);
                                }
                            }
                        }

                        _localStorage.SaveChanges();
                    }
                }

                #endregion

            }
        }




        private void InBoundSync()
        {
            SyncToken syncToken = null;
            var _localStorage = _facade.GetLocalService();
            var users = _localStorage.UserInfoRepo.Get();

            #region CalenderSync

            foreach (var user in users)
            {
                var provider = _facade.GetUserService(user.Id);
                syncToken = _localStorage.SyncRepo.Get().FirstOrDefault(m => (m.TokenType == Enums.SyncTokenType.Calendar && m.ParentId == user.Id));
                var pendingCalenders = provider.FetchCalendars(syncToken);

                if (pendingCalenders.Result.Any())
                {
                    lock (_localStorage)
                    {
                        foreach (var calendar in pendingCalenders.Result)
                        {
                            var localItem = _localStorage.CalendarRepo.Get(calendar.Id, false);
                            if (localItem != null)
                            {
                                if(calendar.CalenderList.IsDeleted)
                                {
                                  _localStorage.CalendarRepo.Delete(localItem, Enums.LocalStorageDataState.Unchanged);
                                }
                                else
                                {
                                   _localStorage.CalendarRepo.Update(localItem, Enums.LocalStorageDataState.Unchanged);
                                }
                            }
                            else if (localItem == null && !calendar.CalenderList.IsDeleted)
                            {
                                _localStorage.CalendarRepo.Add(calendar, Enums.LocalStorageDataState.Unchanged);
                            }
                        }

                        syncToken = pendingCalenders.NextSyncToken;
                        var isExisting = (_localStorage.SyncRepo.Get(syncToken.Id ?? "") != null);
                        if (isExisting) { _localStorage.SyncRepo.Update(syncToken, Enums.LocalStorageDataState.Unchanged); }
                        else { _localStorage.SyncRepo.Add(syncToken, Enums.LocalStorageDataState.Unchanged); }

                        _localStorage.SaveChanges();
                    }
                }
            }

            #endregion



            #region Calender Appointments


            foreach (var user in users)
            {               
                var userCalendars = _localStorage.CalendarRepo.Get(false).Where(m => (m.UserId == user.Id)).ToList();
                foreach (var userCal in userCalendars)
                {
                    syncToken = _localStorage.SyncRepo.Get().FirstOrDefault(m => (m.TokenType == Enums.SyncTokenType.CalenderAppointments && m.ParentId == userCal.Id));
                    var provider = _facade.GetUserService(user.Id);
                    var pendingAppointments = provider.FetchAppointments(userCal, syncToken);

                    if (pendingAppointments.Result.Any())
                    {
                        lock (_localStorage)
                        {

                        foreach (var appointment in pendingAppointments.Result)
                        {
                            var localItem = _localStorage.AppointmentRepo.Get(appointment.Id, false);
                            if (localItem != null)
                            {
                                if (appointment.Status == Enums.AppointmentStatus.Cancelled)
                                {
                                   _localStorage.AppointmentRepo.Delete(localItem, Enums.LocalStorageDataState.Unchanged);
                                }
                                else
                                {
                                   _localStorage.AppointmentRepo.Update(localItem, Enums.LocalStorageDataState.Unchanged);
                                }
                            }
                            else if (localItem == null && appointment.Status != Enums.AppointmentStatus.Cancelled)
                            {
                                _localStorage.AppointmentRepo.Add(appointment, Enums.LocalStorageDataState.Unchanged);
                            }
                        }

                        syncToken = pendingAppointments.NextSyncToken;
                        var isExisting = (_localStorage.SyncRepo.Get(syncToken.Id ?? "") != null);
                        if (isExisting) { _localStorage.SyncRepo.Update(syncToken, Enums.LocalStorageDataState.Unchanged); }
                        else { _localStorage.SyncRepo.Add(syncToken, Enums.LocalStorageDataState.Unchanged); }

                        _localStorage.SaveChanges();

                        }
                    }
                }
            }


            #endregion

         
        }


    }
}
