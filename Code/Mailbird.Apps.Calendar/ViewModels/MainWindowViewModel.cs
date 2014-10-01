﻿using System.Collections.ObjectModel;
﻿using DevExpress.Mvvm.POCO;
﻿using DevExpress.Xpf.Scheduler;
﻿using Mailbird.Apps.Calendar.Engine;
using System;
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
using Microsoft.Win32;
using Mailbird.Apps.Calendar.Enums;
using DevExpress.Xpf.Scheduler.Drawing;
using System.Windows.Input;


namespace Mailbird.Apps.Calendar.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, ISupportServices
    {
        #region PrivateProps

        private readonly ICalendarServicesFacade _facade;
        private readonly ICalendarCatalog _calendarsCatalog;
        private readonly Synchronizer _syncer;


        private string _searchText;
        private CalenderView _selectedCalenderView = CalenderView.Day;
        public ObservableCollection<AppointmentUI> _appointmentCollection { get; private set; }
        public ObservableCollection<UserInfoUI> _userCollections { get; private set; }

        protected DelegateCommand _createNewCalenderUICommand;
        protected DelegateCommand<RoutedEventArgs> _searchUICommand;


        #endregion PrivateProps

        #region PublicProps

        public CalenderPopupViewModel CalenderPopupViewModel { get; private set; }
        public AppointmentPopupViewModel AppointmentPopupViewModel { get; private set; }
        public AppointmentDetailsPopupViewModel AppointmentDetailsPopupVM { get; set; }


        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged(() => SearchText);
            }
        }

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

        public DelegateCommand<RoutedEventArgs> SearchUICommand
        {
            get
            {
                if (_searchUICommand == null)
                {
                    _searchUICommand = new DelegateCommand<RoutedEventArgs>(OnSearchExecute, true);
                }
                return _searchUICommand;
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

        public ObservableCollection<UIModels.UserInfoUI> UserCollection
        {
            get { return _userCollections; }
            private set
            {

                _userCollections = value;
                RaisePropertyChanged(() => UserCollection);
            }
        }


        #endregion PublicProps

        public MainWindowViewModel()
        {
            _facade = CalendarServicesFacade.GetInstance();
            _serviceContainer = new ServiceContainer(this);
            _calendarsCatalog = new CalendarsCatalog(_facade.GetLocalService());
            _syncer = new Synchronizer();

            CalenderPopupViewModel = new ViewModels.CalenderPopupViewModel()
            {
                InsertCalenderAction = InsertCalender,
                UpdateCalenderAction = UpdateCalender,
                DeleteCalenderAction = RemoveCalender
            };

            AppointmentPopupViewModel = new AppointmentPopupViewModel()
            {
                InsertAppointmentAction = InsertAppointment,
                UpdateAppointmentAction = UpdateAppointment,
                DeleteAppointmentAction = DeleteAppointment
            };

            AppointmentDetailsPopupVM = new AppointmentDetailsPopupViewModel()
            {
                UpdateAppointmentAction = this.OpenAppointmentPopUp,
                DeleteAppointmentAction = this.DeleteAppointment,
            };

            // Listen to storage changes
            _facade.GetLocalService().StorageChanged += OnLocal_StorageChanged;

            LoadResources();
            _syncer.AsyncSync();
        }



        protected void LoadResources()
        {
            LoadUsers();
            LoadAppointments();
        }


        //Appointments
        protected void LoadAppointments()
        {
            List<AppointmentUI> appointments = new List<AppointmentUI>();
            appointments.AddRange(_calendarsCatalog.GetAppointments().Select(m => m.Clone()).ToList());
            AppointmentCollection = new ObservableCollection<AppointmentUI>(appointments);
        }


        // Users
        protected void LoadUsers()
        {
            var usersVms = _calendarsCatalog.GetUsers().Select(m => m.Clone()).ToList();
            foreach (var userVm in usersVms)
            {
                userVm.PropertyChanged += userVm_PropertyChanged;
                LoadCalenders(userVm);
            }
            UserCollection = new ObservableCollection<UserInfoUI>(usersVms);
        }

        // Calenders
        protected void LoadCalenders(UserInfoUI userVm)
        {
            var calenderVms = _calendarsCatalog.GetCalendars().Where(m => m.UserId == userVm.Id)
                                               .Select(m => m.Clone()).ToList();
            foreach (var calvm in calenderVms)
            {
                calvm.PropertyChanged += Calender_PropertyChanged;
            }
            userVm.Calendars = new ObservableCollection<CalenderUI>(calenderVms);
        }


        #region Event Handlers


        /// <summary>
        /// To do when any changes are made on the storage.
        /// </summary>
        private void OnLocal_StorageChanged(object sender, Engine.EventArugs.LocalStorageChangedArgs e)
        {
            LoadResources();
        }

        /// <summary>
        /// Manage what to do when user is selected and deselcted
        /// </summary>
        void userVm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                // When a calendar has been selected or deselected 
                RaisePropertyChanged(() => AppointmentCollection);
            }
        }

        /// <summary>
        /// Manage what to do when calendar is selected and deselcted
        /// </summary>
        private void Calender_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                // When a calendar has been selected or deselected 
                RaisePropertyChanged(() => AppointmentCollection);
            }
        }


        #endregion



        // All controls deriving from the main wind should use these
        // function, Instead of having repeated implementations 
        #region Appointment CRUD Functions

        public void InsertAppointment(AppointmentUI appointment)
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
            // Updated has failed, if Null is retuned. Since the scheulder storage is updated already
            // fetch the unchanged appointment from local storage and update the scheduler storage
            var updatedAppointment = _calendarsCatalog.UpdateAppointment(appointment.Clone());
            if (updatedAppointment == null) { updatedAppointment = _calendarsCatalog.GetAppointment(appointment.Id); }
            if (updatedAppointment != null)
            {
                var toRemoveApt = AppointmentCollection.FirstOrDefault(m => (m.Id == updatedAppointment.Id));
                AppointmentCollection.Remove(toRemoveApt);
                AppointmentCollection.Add(updatedAppointment.Clone());
                RaisePropertyChanged(() => AppointmentCollection);
            }
        }


        public void DeleteAppointment(AppointmentUI appointment)
        {
            var toRemoveApt = AppointmentCollection.FirstOrDefault(m => (m.Id == appointment.Id));
            if (toRemoveApt != null)
            {
                if (_calendarsCatalog.RemoveAppointment(toRemoveApt.Clone()))
                {
                    AppointmentCollection.Remove(toRemoveApt);
                    RaisePropertyChanged(() => AppointmentCollection);
                }
            }
        }


        #endregion


        #region Calendar CRUD Functions

        public void InsertCalender(UIModels.CalenderUI calender)
        {
            var addedCalender = _calendarsCatalog.InsertCalendar(calender.Clone());
        }


        public void UpdateCalender(UIModels.CalenderUI calender)
        {
            var updatedCalender = _calendarsCatalog.UpdateCalendar(calender.Clone());
        }


        public void RemoveCalender(UIModels.CalenderUI calender)
        {
            _calendarsCatalog.RemoveCalendar(calender.Clone());
        }

        #endregion


        #region Modals and Pop functions


        /// <summary>
        /// Use this to edit an appoinment
        /// </summary>
        /// <param name="appointmentId"></param>
        public void OpenAppointmentPopUp(string appointmentId)
        {
            OpenAppointmentPopUp(appointmentId, null, null);
        }



        /// <summary>
        /// Use this to create a new appointment with start and end date time or default today.
        /// </summary>
        /// <param name="startDateTime">Prefered start date time</param>
        /// <param name="endDateTime">prefered end date time, should not be lesser than start date tiime</param>
        public void OpenAppointmentPopUp(DateTime? startDateTime = null, DateTime? endDateTime = null)
        {
            OpenAppointmentPopUp(null, startDateTime, endDateTime);
        }


        /// <summary>
        /// Open a appoinemtn form to create or edit.
        /// If a appoinemt is found the dates will be ignored.
        /// </summary>
        /// <param name="appointmentId">Appointment to load</param>
        /// <param name="startDateTime">Start time date</param>
        /// <param name="endDateTime">End date time</param>
        private void OpenAppointmentPopUp(string appointmentId, DateTime? startDateTime, DateTime? endDateTime)
        {
            UIModels.AppointmentUI selectedAppointment = _appointmentCollection.FirstOrDefault(m => (m.Id == (appointmentId ?? "")));

            AppointmentPopupViewModel.AvailableCalenders = _calendarsCatalog.GetCalendars().Select(m => m.Clone()).ToList();
            AppointmentPopupViewModel.SelectedAppointment = selectedAppointment;
            AppointmentPopupViewModel.StartDate = (selectedAppointment != null) ? selectedAppointment.StartDateTime : (startDateTime ?? DateTime.Now);
            AppointmentPopupViewModel.EndDate = (selectedAppointment != null) ? selectedAppointment.EndDateTime : (endDateTime ?? DateTime.Now);
            AppointmentPopupViewModel.LocationSuggestions = SearchMatchingLocation();

            _appointmentEditDialogService.ShowDialog(null, AppointmentPopupViewModel.Title, AppointmentPopupViewModel);
        }


        public void OpenCalenderPopUp(string calenderId)
        {
            CalenderPopupViewModel.AvailableUsers = new ObservableCollection<UserInfoUI>(_calendarsCatalog.GetUsers().Select(m => m.Clone()).ToList());
            CalenderPopupViewModel.SelectedCalender = _calendarsCatalog.GetCalendar(calenderId).Clone();
            _calenderEditDialogService.ShowDialog(null, CalenderPopupViewModel.Title, CalenderPopupViewModel);
        }


        public void OpenReminderPopup(string appointmentId)
        {
            var toRemindAppointment = AppointmentCollection.FirstOrDefault(m => (m.Id == appointmentId));
            if (toRemindAppointment != null)
            {
                DXDialog modal = new DXDialog("Reminder Alert !", DialogButtons.Ok)
                {
                    Content = new TextBlock() { Text = string.Format("{0} Starts at {1}", toRemindAppointment.Summary, toRemindAppointment.StartDateTime.ToString()) },
                    SizeToContent = System.Windows.SizeToContent.WidthAndHeight,
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
                };
            }
        }


        public void OpenAppointmentDetailsPopup(UIElement targetPlacement)
        {
            // Show deatils popup only if 1 appointment is selected
            if (targetPlacement != null && (targetPlacement is VisualAppointmentControl))
            {
                var selectedAppointmentVisual = (VisualAppointmentControl)targetPlacement;
                var selectedAppointment = selectedAppointmentVisual.GetAppointment();

                AppointmentDetailsPopupVM.SelectedAppointment = AppointmentCollection.FirstOrDefault(m => m.Id == selectedAppointment.Id);
                AppointmentDetailsPopupVM.PlacementTarget = selectedAppointmentVisual;
                AppointmentDetailsPopupVM.RefreshModal();
                AppointmentDetailsPopupVM.IsOpen = true;
                return;
            }
            AppointmentDetailsPopupVM.IsOpen = false;
        }


        #endregion



        #region API extension Test Functions

        /// <summary>
        /// This is a exmaple to export appoinments as .ICS
        /// </summary>
        public void OpenSendInvitationPopup(SchedulerControl scheduler)
        {
            var selectedAppointments = AppointmentCollection.Where(m => (scheduler.SelectedAppointments.Any(s => s.Id.ToString().Equals(m.Id)))).ToList();
            if (selectedAppointments != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "iCalendar files (*.ics)|*.ics";
                dialog.FilterIndex = 1;
                if (dialog.ShowDialog() == true)
                {
                    using (var stream = dialog.OpenFile())
                    {
                        var parser = new ICalenderAppoinmentParser();
                        parser.Serialize(stream, selectedAppointments.Select(m => m.Clone()).ToList());
                    }
                }

            }
        }


        /// <summary>
        /// This is a exmaple to import appoinments as .ICS
        /// </summary>
        public void OpenReceiveInvitationPopup()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "iCalendar files (*.ics)|*.ics";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() != true)
                return;

            var appointmentsToAdd = new List<Mailbird.Apps.Calendar.Engine.Metadata.Appointment>();

            foreach (var fileStream in dialog.OpenFiles())
            {
                var parser = new ICalenderAppoinmentParser();
                appointmentsToAdd.AddRange(parser.Deserialize(fileStream));
            }

            foreach (var apt in appointmentsToAdd)
            {
                InsertAppointment(apt.Clone());
            }
        }

        #endregion



        /// <summary>
        /// Helper function to fetch locatino suggestions
        /// </summary>
        /// <param name="searchContext"></param>
        /// <param name="returnAllIfNoMatch"></param>
        /// <returns></returns>
        public List<string> SearchMatchingLocation(string searchContext = null, bool returnAllIfNoMatch = true)
        {
            if (searchContext == null) { searchContext = ""; }
            searchContext = searchContext.Trim();

            List<string> result = _calendarsCatalog.GetLocations().Where(m => m.Contains(searchContext)).ToList();
            if (result.Count <= 0 && returnAllIfNoMatch) { result = _calendarsCatalog.GetLocations().ToList(); }
            return result;
        }


        /// <summary>
        /// To Do on search command executed
        /// </summary>
        /// <param name="parameter"></param>
        public void OnSearchExecute(RoutedEventArgs parameter)
        {
            if (parameter == null) { return; }
            if (((KeyEventArgs)parameter).Key != Key.Enter) { return; }

            parameter.Handled = true;
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                this.SelectedCalenderView = CalenderView.Agenda;
            }
        }


        /// <summary>
        /// This is a search helper fucntion
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        public bool IsAppointmentVisible(AppointmentUI appointment)
        {
            if (appointment == null) { return false; }
            var result = UserCollection.Where(m => m.IsSelected).SelectMany(m => m.Calendars)
                                       .Any(m => m.Id == appointment.CalendarId && m.IsSelected);
            if (!string.IsNullOrWhiteSpace(_searchText) && result)
            {
                result = ((appointment.Summary != null && appointment.Summary.Contains(_searchText)) ||
                          (appointment.Description != null && appointment.Description.Contains(_searchText)));
            }

            return result;
        }



        /// <summary>
        /// Used to fetch dialogs via devexpress services ....
        /// </summary>
        IServiceContainer _serviceContainer;
        public IServiceContainer ServiceContainer
        {
            get
            {
                if (_serviceContainer == null) { _serviceContainer = new ServiceContainer(this); }
                return _serviceContainer;
            }
        }

        IDialogService _appointmentEditDialogService { get { return _serviceContainer.GetService<IDialogService>("AppointmentPopupService"); } }
        IDialogService _calenderEditDialogService { get { return _serviceContainer.GetService<IDialogService>("CalenderPopupService"); } }


    }
}
