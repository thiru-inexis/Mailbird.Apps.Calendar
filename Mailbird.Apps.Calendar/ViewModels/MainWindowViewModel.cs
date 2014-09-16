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
using Mailbird.Apps.Calendar.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Mailbird.Apps.Calendar.Engine.CalenderServices;
using DevExpress.Mvvm;
using ViewModelBase = Mailbird.Apps.Calendar.Infrastructure.ViewModelBase;
using Mailbird.Apps.Calendar.UIModels;
using Appointment = Mailbird.Apps.Calendar.Engine.Metadata.Appointment;
using DevExpress.Xpf.Core;
using System.Windows.Controls;


namespace Mailbird.Apps.Calendar.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region PrivateProps

       
        private readonly LocalCalenderService _local;
        private readonly GoogleCalendarService _google;
        private readonly ICalendarCatalog _calendarsCatalog;
        private readonly Synchronizer _syncer;

        //private List<AppointmentUI> _appointmentSource;
        //private List<CalenderUI> _calenderSource;


        private CalenderView _selectedCalenderView = CalenderView.Day;

        
        protected DelegateCommand _createNewCalenderUICommand;


      
        //private readonly ObservableCollection<TreeData> _treeData = new ObservableCollection<TreeData>();


        //private readonly Dictionary<object, AppointmentUI> _appointmentSource = new Dictionary<object, AppointmentUI>();
        //private readonly Dictionary<object, CalenderUI> _calenderSource = new Dictionary<object, CalenderUI>();


        public ObservableCollection<AppointmentUI> _appointmentCollection { get; private set; }
        public ObservableCollection<CalenderUI> _calenderCollection { get; private set; }




        #endregion PrivateProps

        #region PublicProps

        public AppointmentPopupViewModel AppointmentPopupViewModel { get; private set; }
        public CalenderPopupViewModel CalenderPopupViewModel { get; private set; }
        public FlyoutViewModel FlyoutViewModel { get; private set; }




        //public List<AppointmentUI> AppointmentSource
        //{
        //    get { return new List<AppointmentUI>(_appointmentSource); }
        //    private set
        //    {
        //        _appointmentSource = value;
        //        RaisePropertyChanged(() => AppointmentSource);
        //    }
        //}

        //public List<CalenderUI> CalendarSource
        //{
        //    get { return new List<CalenderUI>(_calenderSource); }
        //    private set
        //    {
        //        _calenderSource = value;
        //        RaisePropertyChanged(() => CalendarSource);
        //    }
        //}

        public bool IsAgendaViewActive
        {
            get { return (SelectedCalenderView == CalenderView.Agenda); }
        }

        public CalenderView SelectedCalenderView
        {
            get { return _selectedCalenderView; }
            set
            {
                _selectedCalenderView = value;
                RaisePropertyChanged(() => SelectedCalenderView);
                RaisePropertyChanged(() => IsAgendaViewActive); 
            }
        }

        public List<CalenderView> AvailableCalenderViews
        {
            get { return Enum.GetValues(typeof(CalenderView)).Cast<CalenderView>().ToList(); }
        }



        public DelegateCommand CreateNewCalenderUICommand
        {
            get
            {
                if (_createNewCalenderUICommand == null)
                {
                    _createNewCalenderUICommand = new DelegateCommand(() => OpenCalenderPopUp(null), true);
                }
                return _createNewCalenderUICommand;
            }
        }


        public ObservableCollection<AppointmentUI> AppointmentCollection
        {
            get { return _appointmentCollection; }
            private set
            {
                _appointmentCollection = value;
                RaisePropertyChanged(() => AppointmentCollection);
            }
        }

        public ObservableCollection<UIModels.CalenderUI> CalenderCollection
        {
            get { return _calenderCollection; }
            private set
            {

                _calenderCollection = value;
                RaisePropertyChanged(() => CalenderCollection);
            }
        }

        //public ObservableCollection<TreeData> TreeData
        //{
        //    get { return _treeData; }
        //}







        #endregion PublicProps

        public MainWindowViewModel()
        {
            _local = new LocalCalenderService();
            _google = new GoogleCalendarService();
            _calendarsCatalog = new CalendarsCatalog(_local);
            _syncer = new Synchronizer(_local, _google);

            LoadCalenders();
            LoadAppointments();

            //this.CalenderPopupViewModel = new CalenderPopupViewModel();

            //this.AppointmentPopupViewModel = new AppointmentPopupViewModel();
            FlyoutViewModel = new FlyoutViewModel
            {
                //AddAppointmentAction = AddAppointment,
                //UpdateAppointmentAction = UpdateAppointment,
                //RemoveAppointmentAction = RemoveAppointment
            };


          

            ////AddElementToTree(provider);
            //var calenders = _calendarsCatalog.GetCalendars();
            //foreach (var calendar in calenders)
            //{
            //    AddElementToTree(calendar);
            //}

            //AppointmentCollection = new ObservableCollection<Appointment>(_calendarsCatalog.GetAppointments());

            // Start Async appointment sync
            //_syncer.AsyncSync();
        }


        //Appointments
        protected void LoadAppointments()
        {
            List<AppointmentUI> appointments = new List<AppointmentUI>();
            List<Appointment> apts = new List<Appointment>();
            var calenders = this.CalenderCollection.Where(m => m.IsSelected);
            foreach (var calender in calenders)
            {
                //apts.AddRange(_calendarsCatalog.GetAppointments(calender.CalendarId));
                appointments.AddRange(_calendarsCatalog.GetAppointments(calender.Id).Select(m => m.Clone()).ToList());
            }

            AppointmentCollection = new ObservableCollection<AppointmentUI>(appointments);  
        }



        // Calenders


        protected void LoadCalenders()
        {
            var calenderVms = _calendarsCatalog.GetCalendars().Select(m => m.Clone()).ToList();
            foreach (var calvm in calenderVms)
            {
                calvm.PropertyChanged += Calender_PropertyChanged;
            }
            this.CalenderCollection = new ObservableCollection<CalenderUI>(calenderVms);
        }


                
        private void Calender_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                LoadAppointments();
            }
        }


        void CalenderCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (UIModels.CalenderUI model in e.NewItems)
                    model.PropertyChanged += this.Calender_PropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (UIModels.CalenderUI model in e.NewItems)
                    model.PropertyChanged -= this.Calender_PropertyChanged;
        }



        //#region Synchronization

        ///// <summary>
        ///// Cancel an ongoing appointment sync
        ///// </summary>
        //public void CancelAsyncSync()
        //{
        //    if (_syncCts != null) { _syncCts.Cancel(); }
        //}


        ///// <summary>
        ///// Asynchronous appointment sync
        ///// </summary>
        //public void AsyncSync()
        //{
        //    try
        //    {
        //        if (_syncCts == null) { _syncCts = new CancellationTokenSource(); }
        //        var cancelToken = _syncCts.Token;


        //        Task.Factory.StartNew(()=>
        //        {
        //            while (true)
        //            {
        //                cancelToken.ThrowIfCancellationRequested();

        //                //If any exceptions are throen from the sync, suppress them
        //                try
        //                {
        //                    Sync();
        //                }
        //                catch { }

        //                Thread.Sleep(APPOINTMENT_SYNC_PERIOD);
        //            }
        //        }, cancelToken);
        //    }
        //    catch (Exception exp)
        //    {
        //        System.Diagnostics.Debug.WriteLine(exp.ToString());
        //    }
        //}


        ///// <summary>
        ///// To Do on Sync
        ///// </summary>
        //public void Sync()
        //{
        //    // Add new appointments if, appointment id is not already in list
        //    var appointments = _calendarsCatalog.GetCalendarAppointments();
        //    foreach (var appointment in appointments)
        //    {
        //        if (!_appointments.ContainsKey(appointment.Id))
        //        {
        //            _appointments.Add(appointment.Id, appointment);

        //            Action<Appointment> addMethod = AppointmentCollection.Add;
        //            Application.Current.Dispatcher.BeginInvoke(addMethod, appointment);
        //            RaisePropertyChanged(() => AppointmentCollection);
        //        }
        //    }

        //    // For later use
        //    var calenders = _calendarsCatalog.GetCalendars().Select(m => m.Clone()).ToList();
        //    this.CalenderCollection = new ObservableCollection<UIModels.Calender>(calenders);
        //}


        //#endregion


        public void AddAppointment(AppointmentUI appointment)
        {
            var addedAppointment = _calendarsCatalog.InsertAppointment(appointment.Clone());
            if (addedAppointment != null)
            {
                AppointmentCollection.Add(addedAppointment.Clone());
                RaisePropertyChanged(() => AppointmentCollection);
            }
        }


        public void UpdateAppointment(AppointmentUI appointment)
        {
            // update event on context, if success update vm
            var updatedAppointment = _calendarsCatalog.UpdateAppointment(appointment.Clone());
            if (updatedAppointment != null)
            {
                var toRemoveApt = AppointmentCollection.FirstOrDefault(m => (m.Id == updatedAppointment.Id));
                AppointmentCollection.Remove(toRemoveApt);
                AppointmentCollection.Add(updatedAppointment.Clone());
                RaisePropertyChanged(() => AppointmentCollection);
            }
        }


        public void RemoveAppointment(object appintmentId)
        {
            var toRemoveApt = AppointmentCollection.FirstOrDefault(m => (m.Id == appintmentId.ToString()));
            if (toRemoveApt != null)
            {
                if (_calendarsCatalog.RemoveAppointment(toRemoveApt.Clone()))
                {
                    AppointmentCollection.Remove(toRemoveApt);
                    RaisePropertyChanged(() => AppointmentCollection);
                }
            }
        }


        //public void AppointmentOnViewChanged(Appointment appointment)
        //{
        //    var app = AppointmentCollection.First(f => f.Id == appointment.Id);
        //    appointment.ReminderInfo = app.ReminderInfo;
        //    appointment.CalendarId = app.CalendarId;
        //    UpdateAppointment(appointment.Id, appointment);
        //}

        //private void AddElementToTree(object element)
        //{
        //    if (element is ICalendarProvider)
        //    {
        //        TreeData.Add(new TreeData
        //        {
        //            DataType = TreeDataType.Provider,
        //            Data = element,
        //            Name = (element as ICalendarProvider).Name,
        //            ParentID = "0"
        //        });
        //    }
        //    if (element is Mailbird.Apps.Calendar.Engine.Metadata.Calendar)
        //    {
        //        TreeData.Add(new TreeData
        //        {
        //            DataType = TreeDataType.Calendar,
        //            Data = element,
        //            Name = (element as Mailbird.Apps.Calendar.Engine.Metadata.Calendar).Name,
        //            ParentID = (element as Mailbird.Apps.Calendar.Engine.Metadata.Calendar).Provider
        //        });
        //    }
        //}

        //public void OpenInnerFlyout(SchedulerControl scheduler)
        //{
        //    FlyoutViewModel.SelectedStartDateTime = scheduler.SelectedInterval.Start;
        //    FlyoutViewModel.SelectedEndDateTime = scheduler.SelectedInterval.End;
        //    FlyoutViewModel.IsOpen = true;
        //}

        //public void CloseInnerFlyout()
        //{
        //    if (FlyoutViewModel.IsEdited)
        //    {
        //        FlyoutViewModel.OkCommandeExecute();
        //    }
        //    else
        //    {
        //        FlyoutViewModel.IsOpen = false;
        //    }
        //}



        public void InsertCalender(UIModels.CalenderUI calender)
        {            
            var addedCalender = _calendarsCatalog.InsertCalendar(calender.Clone());
            if (addedCalender != null)
            {
                LoadCalenders();
            }
        }


        public void UpdateCalender(UIModels.CalenderUI calender)
        {
            var updatedCalender = _calendarsCatalog.UpdateCalendar(calender.Clone());
            if (updatedCalender != null)
            {
                LoadCalenders();
            }
        }


        public void RemoveCalender(UIModels.CalenderUI calender)
        {
            if (_calendarsCatalog.RemoveCalendar(calender.Clone()))
            {
                LoadCalenders();
            }
        }


        public List<string> SearchMatchingLocation(string searchContext = null, bool returnAllIfNoMatch = true)
        {
            if (searchContext == null) { searchContext = ""; }
            searchContext = searchContext.Trim();

            List<string> result = _calendarsCatalog.GetLocations().Where(m => m.Contains(searchContext)).ToList();
            if (result.Count <= 0 && returnAllIfNoMatch) { result = _calendarsCatalog.GetLocations().ToList(); }
            return result;
        }



        public void OpenAppointmentPopUp(SchedulerControl scheduler)
        {
            AppointmentPopupViewModel = new AppointmentPopupViewModel()
            {
                SelectedStartDateTime = scheduler.SelectedInterval.Start,
                SelectedEndDateTime = scheduler.SelectedInterval.End,
                AvailableCalenders = _calendarsCatalog.GetCalendars().Select(m => m.Clone()).ToList(),
                AddAppointmentAction = AddAppointment,
                UpdateAppointmentAction = UpdateAppointment,
                RemoveAppointmentAction = RemoveAppointment,
                LocationSuggestions = SearchMatchingLocation()
            };

            UIStyles.AppointmentPopupStyle modal = new UIStyles.AppointmentPopupStyle();
            modal.DataContext = AppointmentPopupViewModel;
            AppointmentPopupViewModel.CancelPopUpAction = new Action(() => modal.Close());
            modal.Owner = Application.Current.MainWindow;
            modal.ShowDialog();
        }


        public void OpenCalenderPopUp(string calenderId)
        {
            CalenderPopupViewModel = new ViewModels.CalenderPopupViewModel()
            {
                SelectedCalender = _calendarsCatalog.GetCalendar(calenderId).Clone(),
                InsertCalenderAction = InsertCalender,
                UpdateCalenderAction = UpdateCalender,
                DeleteCalenderAction = RemoveCalender
            };

            UIStyles.CalenderPopupContentStyle calenderModal = new UIStyles.CalenderPopupContentStyle();
            calenderModal.DataContext = CalenderPopupViewModel;
            CalenderPopupViewModel.CancelCalenderAction = new Action(() => calenderModal.Close());
            calenderModal.Owner = Application.Current.MainWindow;
            calenderModal.ShowDialog();
        }


        public void OpenReminderPopup(string appointmentId)
        {
            var toRemindAppointment = AppointmentCollection.FirstOrDefault(m => (m.Id == appointmentId));
            if (toRemindAppointment != null)
            {
                DXDialog modal = new DXDialog("Reminder Alert !", DialogButtons.Ok)
                {
                     Content = new TextBlock() { Text = string.Format("{0} Starts at {1}",toRemindAppointment.Subject, toRemindAppointment.StartDateTime.ToString()) },
                     SizeToContent = System.Windows.SizeToContent.WidthAndHeight,
                     Owner = Application.Current.MainWindow,
                     WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
                };
            }
        }
    }
}
