using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Engine.CalenderServices;
using System.Threading;
using System.Threading.Tasks;

namespace Mailbird.Apps.Calendar.Engine
{
    public class Synchronizer
    {

        #region Constants

        private const int APPOINTMENT_SYNC_PERIOD = 1 * 60 * 1000;

        #endregion

        public bool IsSynchronozing { get; private set; }
        public LocalCalenderService _localStorage;
        public GoogleCalendarService _googleStorage;
        private CancellationTokenSource _syncCts;


        public Synchronizer(LocalCalenderService localProvider, GoogleCalendarService googleProvider)
        {
            IsSynchronozing = false;
            _localStorage = localProvider;
            _googleStorage = googleProvider;
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


        public void Sync()
        {
            this.IsSynchronozing = true;

            //this.OutBoundSync();
            this.InBoundSync();

            this.IsSynchronozing = false;
        }



        /// <summary>
        /// Local changes are synced with the live services
        /// </summary>
        //private void OutBoundSync()
        //{
        //    var pendingCalenders = _localStorage.CalendarRepo.Get()
        //                              .Where(a => a.LocalStorageState != Enums.LocalStorageDataState.Unchanged)
        //                              .Select(a => a).ToList();

        //    foreach (var calender in pendingCalenders)
        //    {
        //        if (calender.LocalStorageState == Enums.LocalStorageDataState.Added)
        //        {
        //            var response = _googleStorage.InsertCalendarList(calender);
        //            _localStorage.CalendarRepo.Delete(calender, Enums.LocalStorageDataState.Unchanged);
        //            _localStorage.CalendarRepo.Add(response, Enums.LocalStorageDataState.Unchanged);
        //        }
        //        else if (calender.LocalStorageState == Enums.LocalStorageDataState.Modified)
        //        {
        //            var response = _googleStorage.InsertCalendarList(calender);
        //            _localStorage.CalendarRepo.Delete(calender, Enums.LocalStorageDataState.Unchanged);
        //            _localStorage.CalendarRepo.Add(response, Enums.LocalStorageDataState.Unchanged);
        //        }
        //        else if (calender.LocalStorageState == Enums.LocalStorageDataState.Deleted)
        //        {
        //            var response = _googleStorage.InsertCalendarList(calender);
        //            _localStorage.CalendarRepo.Delete(calender, Enums.LocalStorageDataState.Unchanged);
        //        }
        //    }



        //    var pendingAppointments =_localStorage.AppointmentRepo.Get()
        //                                          .Where(a => a.LocalStorageState != Enums.LocalStorageDataState.Unchanged)
        //                                          .Select(a => a).ToList();


        //    foreach (var app in pendingAppointments)
        //    {
        //        if (app.LocalStorageState == Enums.LocalStorageDataState.Added)
        //        {
        //            var changedAppointment = _googleStorage.InsertAppointment(app);
        //            _localStorage.AppointmentRepo.Update(changedAppointment, Enums.LocalStorageDataState.Unchanged);
        //        }
        //        else if (app.LocalStorageState == Enums.LocalStorageDataState.Modified)
        //        {
        //            var changedAppointment = _googleStorage.UpdateAppointment(app);
        //            _localStorage.AppointmentRepo.Update(changedAppointment, Enums.LocalStorageDataState.Unchanged);
        //        }
        //        else if (app.LocalStorageState == Enums.LocalStorageDataState.Deleted)
        //        {
        //            var status = _googleStorage.DeleteAppointment(app);
        //           _localStorage.AppointmentRepo.Delete(app);
        //        }
        //    }
        //}




        private void InBoundSync()
        {
            SyncToken syncToken = null;
            string currentToken = null;
            string nextToken = null;

            lock (_localStorage)
            {



                #region Colors Sync

                syncToken = _localStorage.SyncRepo.Get().FirstOrDefault(m => m.TokenContext == Enums.SyncTokenType.ColorDefinition);
                if (syncToken == null)
                {
                    syncToken = new SyncToken()
                    {
                        TokenContext = Enums.SyncTokenType.ColorDefinition,
                        Token = null,
                        LocalStorageState = Enums.LocalStorageDataState.Unchanged
                    };
                }
                currentToken = (syncToken == null) ? null : syncToken.Token;
                nextToken = currentToken;

                var pendingColorDefs = _googleStorage.GetColors(out nextToken, currentToken);
                if (pendingColorDefs.Any())
                {
                    _localStorage.CalendarColorRepo.Clear();
                    foreach (var colorDef in pendingColorDefs[Enums.ColorType.Calendar])
                    {
                        _localStorage.CalendarColorRepo.Add(colorDef, Enums.LocalStorageDataState.Unchanged);
                    }

                    _localStorage.AppointmentColorRepo.Clear();
                    foreach (var colorDef in pendingColorDefs[Enums.ColorType.Appointment])
                    {
                        _localStorage.AppointmentColorRepo.Add(colorDef, Enums.LocalStorageDataState.Unchanged);
                    }
                }



                syncToken.Token = nextToken;
                if (string.IsNullOrEmpty(syncToken.Id))
                {
                    _localStorage.SyncRepo.Add(syncToken, Enums.LocalStorageDataState.Unchanged);
                }
                else
                {
                    _localStorage.SyncRepo.Update(syncToken, Enums.LocalStorageDataState.Unchanged);
                }

                #endregion


                _localStorage.SaveChanges();



                #region CalenderSync

                syncToken = _localStorage.SyncRepo.Get().FirstOrDefault(m => m.TokenContext == Enums.SyncTokenType.Calendar);
                if (syncToken == null)
                {
                    syncToken = new SyncToken()
                    {
                        TokenContext = Enums.SyncTokenType.Calendar,
                        Token = null,
                        LocalStorageState = Enums.LocalStorageDataState.Unchanged
                    };
                }
                currentToken = (syncToken == null) ? null : syncToken.Token;
                nextToken = currentToken;

                var pendingCalenders = _googleStorage.GetCalendars(out nextToken, currentToken, true);
                foreach (var calendar in pendingCalenders)
                {
                    var localItem = _localStorage.CalendarRepo.Get(calendar.Id);
                    if (localItem != null && calendar.CalenderList.IsDeleted)
                    {
                        _localStorage.CalendarRepo.Delete(localItem, Enums.LocalStorageDataState.Unchanged);
                    }
                    else if (localItem == null && !calendar.CalenderList.IsDeleted)
                    {
                        _localStorage.CalendarRepo.Add(calendar, Enums.LocalStorageDataState.Unchanged);
                    }
                }

                syncToken.Token = nextToken;
                if (string.IsNullOrEmpty(syncToken.Id))
                {
                    _localStorage.SyncRepo.Add(syncToken, Enums.LocalStorageDataState.Unchanged);
                }
                else
                {
                    _localStorage.SyncRepo.Update(syncToken, Enums.LocalStorageDataState.Unchanged);
                }


                #endregion


                _localStorage.SaveChanges();

                // Save changes to file ...


                #region Calender Appointments


                var calenders = _localStorage.CalendarRepo.Get();
                foreach (var calender in calenders)
                {

                    syncToken = null;
                    currentToken = null;
                    nextToken = null;


                    syncToken = _localStorage.SyncRepo.Get().FirstOrDefault(m => m.TokenContext == Enums.SyncTokenType.CalenderAppointments &&
                                                                                 m.CalenderId == calender.Id);
                    if (syncToken == null)
                    {
                        syncToken = new SyncToken()
                        {
                            CalenderId = calender.Id,
                            TokenContext = Enums.SyncTokenType.CalenderAppointments,
                            Token = null,
                            LocalStorageState = Enums.LocalStorageDataState.Unchanged
                        };
                    }
                    currentToken = (syncToken == null) ? null : syncToken.Token;
                    nextToken = currentToken;

                    var pendingAppointments = _googleStorage.GetAppointments(out nextToken, calender, currentToken, true);
                    foreach (var appointment in pendingAppointments)
                    {
                        var localItem = _localStorage.AppointmentRepo.Get(appointment.Id);
                        if (localItem != null && appointment.IsDeleted)
                        {
                            _localStorage.AppointmentRepo.Delete(localItem, Enums.LocalStorageDataState.Unchanged);
                        }
                        else if (localItem == null && !appointment.IsDeleted)
                        {
                            _localStorage.AppointmentRepo.Add(appointment, Enums.LocalStorageDataState.Unchanged);
                        }
                    }

                    syncToken.Token = nextToken;
                    if (string.IsNullOrEmpty(syncToken.Id))
                    {
                        _localStorage.SyncRepo.Add(syncToken, Enums.LocalStorageDataState.Unchanged);
                    }
                    else
                    {
                        _localStorage.SyncRepo.Update(syncToken, Enums.LocalStorageDataState.Unchanged);
                    }

                }



                #endregion





                _localStorage.SaveChanges();
                // save changes to file ...
            }
        }


    }
}
