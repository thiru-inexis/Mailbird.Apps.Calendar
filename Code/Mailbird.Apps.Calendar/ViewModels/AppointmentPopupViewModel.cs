using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;
using Mailbird.Apps.Calendar.Infrastructure;
using Appointment = DevExpress.XtraScheduler.Appointment;
using ViewModelBase = Mailbird.Apps.Calendar.Infrastructure.ViewModelBase;
using DevExpress.Mvvm;
using Mailbird.Apps.Calendar.UIModels;
using Mailbird.Apps.Calendar.Enums;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;


namespace Mailbird.Apps.Calendar.ViewModels
{
    /// <summary>
    /// To be binded to the Appointment edit/add form
    /// </summary>
    public class AppointmentPopupViewModel : Infrastructure.ViewModelBase, ISupportServices
    {
        private UIModels.AppointmentUI _bm;
        private Appointment _appointment;
        private ObservableCollection<UIModels.ContactUI> _quests;

        protected DelegateCommand _insertUICommand;
        protected DelegateCommand _updateUICommand;
        protected DelegateCommand _cancelUICommand;
        protected DelegateCommand _deleteUICommand;
        protected DelegateCommand<ListBoxItem> _removeQuestUICommand;
        protected DelegateCommand<RoutedEventArgs> _addQuestUICommand;

        protected string _toAddQuestEmail;
        protected List<string> _startTimeSuggestions;
        protected List<string> _endTimeSuggestions;
        protected ReminderType _selectedReminderType;
        protected int _reminderDuration;
        private bool _isLocationPopupOpen;


        protected List<UIModels.CalenderUI> _availableCalenders;

        public CalenderUI DefaultCalender;


        public AppointmentPopupViewModel()
        {
            _bm = new UIModels.AppointmentUI();
            SelectedAppointments = new ObservableCollection<Appointment>();
            Quests = new ObservableCollection<ContactUI>();
            CreateReminderCollecton();
         
            GenerateStartTimeSuggestions();
            GenerateEndTimeSuggestions();
        }

    

        #region Bindable Properties


        public DelegateCommand InsertUICommand
        {
            get
            {
                if (IsNewAppointment && _insertUICommand == null)
                {
                    _insertUICommand = new DelegateCommand(OnInsertCommandInvoked, true);
                }
                return _insertUICommand;
            }
        }
        public DelegateCommand UpdateUICommand
        {
            get
            {
                if (!IsNewAppointment && _updateUICommand == null)
                {
                    _updateUICommand = new DelegateCommand(OnUpdateCommandInvoked, true);
                }
                return _updateUICommand;
            }
        }

        public DelegateCommand DeleteUICommand
        {
            get
            {
                if (!IsNewAppointment && _deleteUICommand == null)
                {
                    _deleteUICommand = new DelegateCommand(OnDeleteCommandInvoked, true);
                }
                return _deleteUICommand;
            }
        }
        public DelegateCommand CancelUICommand
        {
            get
            {
                if (_cancelUICommand == null)
                {
                    _cancelUICommand = new DelegateCommand(OnCancelCommandInvoked, true);
                }
                return _cancelUICommand;
            }
        }

        public DelegateCommand<ListBoxItem> RemoveQuestUICommand
        {
            get
            {
                if (_removeQuestUICommand == null)
                {
                    _removeQuestUICommand = new DelegateCommand<ListBoxItem>(OnRemoveQuestUICommand, true);
                }
                return _removeQuestUICommand;
            }
        }


        public DelegateCommand<RoutedEventArgs> AddQuestUICommand
        {
            get
            {
                if (_addQuestUICommand == null)
                {
                    _addQuestUICommand = new DelegateCommand<RoutedEventArgs>(OnAddQuestUICommand, true);
                }
                return _addQuestUICommand;
            }
        }




        private void OnInsertCommandInvoked()
        {
            InsertAppointmentAction(_bm);
            CancelPopUpAction();
        }
        private void OnUpdateCommandInvoked()
        {
            UpdateAppointmentAction(_bm);
            CancelPopUpAction();
        }
        private void OnDeleteCommandInvoked()
        {
            DeleteAppointmentAction(_bm);
            CancelPopUpAction();
        }
        private void OnCancelCommandInvoked()
        {
            CancelPopUpAction();
        }

        private void OnRemoveQuestUICommand(ListBoxItem paramter)
        {
            if (paramter == null) { return; }
            if (!(paramter.Content is ContactUI)) { return; }

            var elementToRemove = _quests.FirstOrDefault(m => m.Email.Equals((paramter.Content as ContactUI).Email, StringComparison.InvariantCultureIgnoreCase));
            if (elementToRemove == null) { return; }
            Quests.Remove(elementToRemove);
            //if (((KeyEventArgs)paramter).Key != Key.Enter) { return; }
        }

        private void OnAddQuestUICommand(RoutedEventArgs paramter)
        {
            if (paramter == null) { return; }
            if (((KeyEventArgs)paramter).Key != Key.Enter) { return; }

            paramter.Handled = true;
            if (!string.IsNullOrWhiteSpace(ToAddQuestEmail) && !_quests.Any(m => (m.Email.Equals(ToAddQuestEmail, StringComparison.InvariantCultureIgnoreCase))))
            {
                var newContact = new ContactUI()
                {
                    Email = ToAddQuestEmail,
                    FirstName = ToAddQuestEmail.Substring(0, ToAddQuestEmail.IndexOf("@")), // to test
                    LastName = "",
                    ProfileImgPath = ""
                };
                Quests.Add(newContact);
                ToAddQuestEmail = "";
            }
        }

        public UIModels.AppointmentUI SelectedAppointment
        {
            get { return _bm; }
            set 
            {
                if (value == null) { value = new UIModels.AppointmentUI(); }
                _bm = value;
            }
        }


        public String Title
        {
            get
            {
                if (IsNewAppointment) { return "New Appointment"; }
                return string.Format("Edit - [{0}]", Summary);
            }
        }


                
        public bool IsLocationPopupOpen
        {
            get { return _isLocationPopupOpen; }
            set
            {
                _isLocationPopupOpen = value;
                RaisePropertyChanged(() => IsLocationPopupOpen);
            }
        }

        public string ToAddQuestEmail
        {
            get { return _toAddQuestEmail; }
            set
            {
                _toAddQuestEmail = value;
                RaisePropertyChanged(() => ToAddQuestEmail);
            }
        }


        public ObservableCollection<UIModels.ContactUI> Quests
        {
            get { return _quests; }
            set
            {
                _quests = value;
                RaisePropertyChanged(() => Quests);
            }
        }


        public ReminderType SelectedReminderType
        {
            get { return _selectedReminderType; }
            set
            {
                _selectedReminderType = value;
                RaisePropertyChanged(() => SelectedReminderType);
                UpdateReminderUIwithLogic();
            }
        }


        public Engine.Enums.AppointmentTransparency SelectedTransparency
        {
            get { return _bm.Transparency; }
            set
            {
                _bm.Transparency = value;
                RaisePropertyChanged(() => SelectedTransparency);
            }
        }



        public List<UIModels.CalenderUI> AvailableCalenders
        {
            get { return _availableCalenders; }
            set
            {
                _availableCalenders = value;
                RaisePropertyChanged(() => AvailableCalenders);
                RaisePropertyChanged(() => SelectedCalender);
            }
        }

        public List<Engine.Enums.AppointmentTransparency> AvailableTransparencies
        {
            get { return Enum.GetValues(typeof(Engine.Enums.AppointmentTransparency)).Cast<Engine.Enums.AppointmentTransparency>().ToList(); }
        }



        public int ReminderDuration
        {
            get { return _reminderDuration; }
            set
            {
                if (value < 0) { return; }
                _reminderDuration = value;
                RaisePropertyChanged(() => ReminderDuration);
                UpdateReminderUIwithLogic();
            }
        }



        private void UpdateReminderUIwithLogic(bool hasUIPropertyChanged = false)
        {
            var LogicDic = new Dictionary<ReminderType, long>();
            LogicDic[ReminderType.Minutes] = TimeSpan.TicksPerMinute;
            LogicDic[ReminderType.Hours] = TimeSpan.TicksPerHour;
            LogicDic[ReminderType.Days] = TimeSpan.TicksPerDay;
            LogicDic[ReminderType.Weeks] = (TimeSpan.TicksPerDay * 7);

            if (hasUIPropertyChanged) { _bm.PreReminderDuration = new TimeSpan(LogicDic[SelectedReminderType] * ReminderDuration); }

            var bestMatchingType = ReminderType.Minutes;
            var bestMatchingRemDuration = 0;

            if (_bm.PreReminderDuration.TotalMinutes > 0)
            {
                foreach (var remType in LogicDic)
                {
                    double helper = (_bm.PreReminderDuration.TotalMinutes  % ((int)(remType.Value / TimeSpan.TicksPerMinute)));
                    if (helper == 0)
                    {
                        helper = (_bm.PreReminderDuration.TotalMinutes / ((int)(remType.Value / TimeSpan.TicksPerMinute)));
                        if (helper < bestMatchingRemDuration)
                        {
                            bestMatchingType = remType.Key;
                            bestMatchingRemDuration = (int)helper;
                        }
                    }
                }
            }

            _selectedReminderType = bestMatchingType;
            _reminderDuration = bestMatchingRemDuration;
            RaisePropertyChanged(() => SelectedReminderType);
            RaisePropertyChanged(() => ReminderDuration);
        }



        public UIModels.CalenderUI SelectedCalender
        {
            get 
            {
                if (AvailableCalenders == null || !AvailableCalenders.Any()) { return null; }
                if (_bm.CalendarId == null) 
                {
                   return AvailableCalenders.FirstOrDefault(m => m.IsPrimary);
                }
                return AvailableCalenders.FirstOrDefault(m => m.Id == _bm.CalendarId);
            }
            set
            {
                _bm.CalendarId = value.Id;
                RaisePropertyChanged(() => SelectedCalender);
            }
        }



        public bool IsNewAppointment
        {
            get
            {
                return (_bm == null || string.IsNullOrEmpty(_bm.Id));
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
                IsLocationPopupOpen = true;
                RaisePropertyChanged(() => Location);
            }
        }

        public DateTime StartDate
        {
            get { return _bm.StartDateTime; }
            set
            {
                var newEndDate = EndDate.Add(value.Subtract(StartDate));
                _bm.StartDateTime = value;
                RaisePropertyChanged(() => StartDate);
                RaisePropertyChanged(() => StartTime);
                EndDate = newEndDate;
            }
        }

        public TimeSpan StartTime
        {
            get { return _bm.StartDateTime.TimeOfDay; }
            set
            {
                StartDate = StartDate.Date.Add(value);
                RaisePropertyChanged(() => StartTime);
            }
        }

        public DateTime EndDate
        {
            get { return _bm.EndDateTime; }
            set
            {
                if (value < StartDate) { return; }
                _bm.EndDateTime = value;
                RaisePropertyChanged(() => EndDate);
                RaisePropertyChanged(() => EndTime);
                GenerateEndTimeSuggestions();
            }
        }

        public TimeSpan EndTime
        {
            get { return _bm.EndDateTime.TimeOfDay; }
            set
            {
                EndDate = EndDate.Date.Add(value);
                RaisePropertyChanged(() => EndTime);
            }
        }

        public bool IsAllDayAppointment
        {
            get { return _bm.IsAllDayAppointment; }
            set 
            {
                _bm.IsAllDayAppointment = value;
                RaisePropertyChanged(() => IsAllDayAppointment);
                RaisePropertyChanged(() => CanDisplayStartEndTimeEditor);
            }
        }


        public bool CanDisplayStartEndTimeEditor
        {
            get { return !IsAllDayAppointment; }
        }

        //public int LabelId
        //{
        //    get { return _labelId; }
        //    set { SetValue(ref _labelId, value, () => LabelId); }
        //}

        //public int StatusId
        //{
        //    get { return _statusId; }
        //    set { SetValue(ref _statusId, value, () => StatusId); }
        //}

        //public object ResourceId
        //{
        //    get { return _resourceId; }
        //    set { SetValue(ref _resourceId, value, () => ResourceId); }
        //}

        public string Description
        {
            get { return _bm.Description; }
            set
            {
                _bm.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        //public bool IsOpen
        //{
        //    get { return _isOpen; }
        //    set
        //    {
        //        if (SetValue(ref _isOpen, value, () => IsOpen) && value)
        //            ShowSelectedAppointment();
        //    }
        //}


        public List<string> LocationSuggestions { get; set; }


        //public List<TimeSpan> StartTimeSuggestions { get; set; }
        public List<string> StartTimeSuggestions 
        {
            get { return _startTimeSuggestions; }
            set
            {
                _startTimeSuggestions = value;
                RaisePropertyChanged(() => StartTimeSuggestions);
            }
        }
        public List<string> EndTimeSuggestions
        {
            get { return _endTimeSuggestions; }
            set
            {
                _endTimeSuggestions = value;
                RaisePropertyChanged(() => EndTimeSuggestions);
            }
        }


                
        public List<ReminderType> SupportedReminderTypes
        {
            get { 

              var result = Enum.GetValues(typeof(ReminderType)).Cast<ReminderType>().ToList();
            

                return result;
            }
        }



        public Appointment Appointment
        {
            get { return _appointment; }
            set { SetValue(ref _appointment, value, () => Appointment); }
        }

        //public KeyValuePair<int, string> SelectedReminder
        //{
        //    get { return _selectedReminder; } 
        //    set { SetValue(ref _selectedReminder, value, () => SelectedReminder); }
        //} 

        public List<KeyValuePair<int,string>> ReminderCollection { get; private set; }

        public List<KeyValuePair<int, string>> CalenderCollection 
        {
            get 
            {
                var val = new List<KeyValuePair<int, string>>();
                val.Add(new KeyValuePair<int, string>(0, "Birthdays"));
                val.Add(new KeyValuePair<int, string>(1, "Contacts' birthdays and events"));
                return val;
            }
            //private set; 
        }
        
        //public TreeData SelectedTreeItem
        //{
        //    get { return _selectedTreeItem; }
        //    set
        //    {
        //        if (SetValue(ref _selectedTreeItem, value, () => SelectedTreeItem))
        //        {
        //            if (value != null)
        //                _selectedCalendar = value.Data as Mailbird.Apps.Calendar.Engine.Metadata.Calendar;
        //        }
        //    }
        //}

        //public Mailbird.Apps.Calendar.Engine.Metadata.Calendar SelectedCalendar
        //{
        //    get { return _selectedCalendar ?? new Mailbird.Apps.Calendar.Engine.Metadata.Calendar() { Id = Guid.NewGuid().ToString(), Provider = "LocalCalendarsStorage", Name = "LocalCalendar" }; }
        //    set
        //    {
        //        if (SetValue(ref _selectedCalendar, value, () => SelectedCalendar))
        //            _selectedTreeItem = new TreeData
        //            {
        //                Data = value,
        //                DataType = TreeDataType.Calendar,
        //                Name = value.Name,
        //                ParentID = value.Provider
        //            };
        //        RaisePropertyChanged(() => SelectedTreeItem);
        //    }
        //}

        public ObservableCollection<Appointment> SelectedAppointments { get; private set; }

        #endregion


        #region Public Properties

        public Action<AppointmentUI> InsertAppointmentAction { get; set; }

        public Action<AppointmentUI> UpdateAppointmentAction { get; set; }

        public Action<AppointmentUI> DeleteAppointmentAction { get; set; }

        public void CancelPopUpAction ()
        {
            _currentWindowService.Close();
        }


        public DateTime SelectedStartDateTime { get; set; }

        public DateTime SelectedEndDateTime { get; set; }

        //public bool IsEdited { get { return CheckOnEdit(); } }




        private void AddQuestsEditorActivated(object sender, RoutedEventArgs e)
        {
           
        }

        #endregion


        #region Private members

        private object CurrentAppointmentId { get; set; }

        #endregion


        #region Private methods & helpers

        private void CreateReminderCollecton()
        {
            var dictionart = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(0, "None"),
                new KeyValuePair<int, string>(300, "5 Minutes"),
                new KeyValuePair<int, string>(600, "10 Minutes"),
                new KeyValuePair<int, string>(900, "15 Minutes"),
                new KeyValuePair<int, string>(1200, "20 Minutes"),
                new KeyValuePair<int, string>(1500, "25 Minutes"),
                new KeyValuePair<int, string>(1800, "30 Minutes"),
                new KeyValuePair<int, string>(3600, "1 Hours"),
                new KeyValuePair<int, string>(7200, "2 Hours"),
                new KeyValuePair<int, string>(10800, "3 Hours"),
                new KeyValuePair<int, string>(14400, "4 Hours"),
                new KeyValuePair<int, string>(18000, "5 Hours"),
                new KeyValuePair<int, string>(21600, "6 Hours"),
                new KeyValuePair<int, string>(43200, "0.5 Days"),
                new KeyValuePair<int, string>(86400, "1 Days"),
                new KeyValuePair<int, string>(172800, "2 Days")
            };
            //SelectedReminder = dictionart[0];
            ReminderCollection = dictionart;
        }

        private void GenerateStartTimeSuggestions()
        {
            var converter = new UIResources.Converters.TimeSpanToStringConverter();
            this.StartTimeSuggestions = GetTimeSuggestions().Select(m => (string)converter.Convert(m, null, null, null)).ToList();
        }

        private void GenerateEndTimeSuggestions()
        {
            var converter = new UIResources.Converters.TimeSpanToStringConverter();

            var suggestions = GetTimeSuggestions();
            if (StartDate.Equals(EndDate))
            {
                suggestions = suggestions.Where(m => (m > StartTime)).ToList();
            }

            this.EndTimeSuggestions = suggestions.Select(m => (string)converter.Convert(m, null, null, null)).ToList();
        }

        private List<TimeSpan> GetTimeSuggestions()
        {
            var result = new List<TimeSpan>();
            var interval = new TimeSpan(TimeSpan.TicksPerMinute * 30);
            var temp = TimeSpan.Zero;

            while (temp.Ticks < TimeSpan.TicksPerDay)
            {
                result.Add(temp);
                temp = temp.Add(interval);
            }

            return result;
        }

        #endregion




        IServiceContainer _serviceContainer;
        public IServiceContainer ServiceContainer
        {
            get
            {
                if (_serviceContainer == null) { _serviceContainer = new ServiceContainer(this); }
                return _serviceContainer;
            }
        }

        ICurrentWindowService _currentWindowService { get { return _serviceContainer.GetService<ICurrentWindowService>(); } }
       

    }
}
