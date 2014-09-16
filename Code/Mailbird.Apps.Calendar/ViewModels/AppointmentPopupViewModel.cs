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


namespace Mailbird.Apps.Calendar.ViewModels
{
    public class AppointmentPopupViewModel : Infrastructure.ViewModelBase
    {
        private UIModels.AppointmentUI _bm;
        private Appointment _appointment;

        protected DelegateCommand _insertUICommand;
        protected DelegateCommand _updateUICommand;
        protected DelegateCommand _cancelUICommand;
        protected DelegateCommand _deleteUICommand;

        protected bool _isLocationPopupOpen;
        protected List<string> _startTimeSuggestions;
        protected List<string> _endTimeSuggestions;
        protected UIModels.ReminderType _selectedReminderType;
        protected int _reminderDuration;
        protected UIModels.CalenderUI _selectedCalender;

        protected List<UIModels.CalenderUI> _availableCalenders;

        public AppointmentPopupViewModel()
        {
            _bm = new UIModels.AppointmentUI();
            SelectedAppointments = new ObservableCollection<Appointment>();
            CreateReminderCollecton();
         
            GenerateStartTimeSuggestions();
            GenerateEndTimeSuggestions();
        }

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
            if(StartDate.Equals(EndDate))
            {
                suggestions = suggestions.Where(m => (m > StartTime)).ToList();
            }

            this.EndTimeSuggestions = suggestions.Select(m => (string)converter.Convert(m, null, null, null)).ToList();
        }

        private List<TimeSpan> GetTimeSuggestions()
        {
            var result = new List<TimeSpan>();
            var interval = new TimeSpan( TimeSpan.TicksPerMinute * 30);
            var temp = TimeSpan.Zero;

            while(temp.Ticks < TimeSpan.TicksPerDay)
            {
                result.Add(temp);
                temp = temp.Add(interval);
            }

            return result;
        }


        //public void OkCommandeExecute()
        //{
        //    IsOpen = false;
        //    if (CurrentAppointmentId != null)
        //    {
        //        var appointment = new Mailbird.Apps.Calendar.Engine.Metadata.Appointment
        //        {
        //            Id = CurrentAppointmentId.ToString(),
        //            Subject = Subject,
        //            Location = Location,
        //            StartTime = StartDate.Date + (AllDayEvent ? DateTime.Parse(DefaultTimeValue).TimeOfDay : DateTime.Parse(StartTime).TimeOfDay),
        //            EndTime = EndDate.Date + (AllDayEvent ? DateTime.Parse(DefaultTimeValue).TimeOfDay : DateTime.Parse(EndTime).TimeOfDay),
        //            AllDayEvent = AllDayEvent,
        //            LabelId = LabelId,
        //            Description = Description,
        //            StatusId = StatusId,
        //            //ResourceId = ResourceId,
        //            CalendarId = SelectedCalendar.Id
        //        };
        //        appointment.ReminderInfo = GetReminderInfo(appointment);
        //        UpdateAppointmentAction(CurrentAppointmentId, appointment);
        //    }
        //    else
        //    {
        //        var appointment = new Mailbird.Apps.Calendar.Engine.Metadata.Appointment
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Subject = Subject,
        //            Location = Location,
        //            StartTime = StartDate.Date + (AllDayEvent ? DateTime.Parse(DefaultTimeValue).TimeOfDay : DateTime.Parse(StartTime).TimeOfDay),
        //            EndTime = EndDate.Date + (AllDayEvent ? DateTime.Parse(DefaultTimeValue).TimeOfDay : DateTime.Parse(EndTime).TimeOfDay),
        //            AllDayEvent = AllDayEvent,
        //            LabelId = LabelId,
        //            Description = Description,
        //            StatusId = StatusId,
        //            //ResourceId = ResourceId,
        //            CalendarId = SelectedCalendar.Id
        //        };
        //        appointment.ReminderInfo = GetReminderInfo(appointment);
        //        AddAppointmentAction(appointment);
        //    }
        //}

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



        private void OnInsertCommandInvoked()
        {
            //InsertCalenderAction(_basemodel);
            //CancelCalenderAction();
        }
        private void OnUpdateCommandInvoked()
        {
            //UpdateCalenderAction(_basemodel);
            //CancelCalenderAction();
        }
        private void OnDeleteCommandInvoked()
        {
            //DeleteCalenderAction(_basemodel);
            //CancelCalenderAction();
        }
        private void OnCancelCommandInvoked()
        {
            //CancelCalenderAction();
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
                return string.Format("Edit - [{0}]", Subject);
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

        public UIModels.ReminderType SelectedReminderType
        {
            get { return _selectedReminderType; }
            set
            {
                _selectedReminderType = value;
                RaisePropertyChanged(() => SelectedReminderType);
                UpdateReminderUIwithLogic();
            }
        }



        public List<UIModels.CalenderUI> AvailableCalenders
        {
            get { return _availableCalenders; }
            set
            {
                _availableCalenders = value;
                RaisePropertyChanged(() => AvailableCalenders);
            }
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
            var LogicDic = new Dictionary<UIModels.ReminderType, long>();
            LogicDic[UIModels.ReminderType.Minutes] = TimeSpan.TicksPerMinute;
            LogicDic[UIModels.ReminderType.Hours] = TimeSpan.TicksPerHour;
            LogicDic[UIModels.ReminderType.Days] = TimeSpan.TicksPerDay;
            LogicDic[UIModels.ReminderType.Weeks] = (TimeSpan.TicksPerDay * 7);

            if (hasUIPropertyChanged) { _bm.PreReminderDuration = new TimeSpan(LogicDic[SelectedReminderType] * ReminderDuration); }

            var bestMatchingType = UIModels.ReminderType.Minutes;
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
            get { return _selectedCalender; }
            set
            {
                _selectedCalender = value;
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


        public string Subject
        {
            get { return _bm.Subject; }
            set 
            {
                _bm.Subject = value;
                RaisePropertyChanged(() => Subject);
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


                
        public List<UIModels.ReminderType> SupportedReminderTypes
        {
            get { 

              var result = Enum.GetValues(typeof( UIModels.ReminderType)).Cast<UIModels.ReminderType>().ToList();
            

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

        public Action<UIModels.AppointmentUI> AddAppointmentAction { get; set; }

        public Action<UIModels.AppointmentUI> UpdateAppointmentAction { get; set; }

        public Action<object> RemoveAppointmentAction { get; set; }

        public Action CancelPopUpAction { get; set; }


        public DateTime SelectedStartDateTime { get; set; }

        public DateTime SelectedEndDateTime { get; set; }

        //public bool IsEdited { get { return CheckOnEdit(); } }

        #endregion

        #region Private members

        private object CurrentAppointmentId { get; set; }

        #endregion

        #region Private methods & helpers

        //private void ShowSelectedAppointment()
        //{
        //    if (SelectedAppointments != null && SelectedAppointments.Any())
        //    {
        //        SetAppointment(SelectedAppointments.First());
        //    }
        //    else
        //    {
        //        Subject = null;
        //        Location = null;
        //        LabelId = 0;
        //        StatusId = 0;
        //        AllDayEvent = false;
        //        Description = null;
        //        //Resolve date time to format that is used on view
        //        StartDate = DateTime.ParseExact(SelectedStartDateTime.ToString("MM-dd-yyyy HH:mm:ss"), "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture).Date;
        //        EndDate = DateTime.ParseExact(SelectedStartDateTime.ToString("MM-dd-yyyy HH:mm:ss"), "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture).Date;
        //        StartTime = SelectedStartDateTime.TimeOfDay.ToString();
        //        EndTime = SelectedStartDateTime.TimeOfDay.ToString();
        //        SelectedReminder= ReminderCollection[0];
        //        CurrentAppointmentId = null;
        //    }
        //}

        //private void EndDateValidation()
        //{
        //    if (StartDate.Date > EndDate.Date)
        //    {
        //        _endDate = StartDate;
        //        RaisePropertyChanged(()=>EndDate);
        //    }

        //    if (StartDate.Date == EndDate.Date)
        //    {
        //        if (DateTime.Parse(StartTime).TimeOfDay > DateTime.Parse(EndTime).TimeOfDay)
        //        {
        //            _endTime = StartTime;
        //            RaisePropertyChanged(()=>EndTime);
        //        }
        //    }

        //    AllDayEventValidation();
        //}

        //private void AllDayEventValidation()
        //{
        //    if (!AllDayEvent) return;

        //    _startTime = DefaultTimeValue;
        //    RaisePropertyChanged(() => StartTime);
            
        //    _endTime = DefaultTimeValue;
        //    RaisePropertyChanged(() => EndTime);
            
        //    if (Math.Abs((EndDate - StartDate).TotalDays) < 1)
        //    {
        //        _endDate = EndDate.AddDays(1);
        //        RaisePropertyChanged(() => EndDate);
        //    }

        //    if ((int)Math.Abs((EndDate - StartDate).TotalDays) == 1)
        //    {
        //        if (DateTime.Parse(StartTime).TimeOfDay > DateTime.Parse(EndTime).TimeOfDay)
        //        {
        //            _endTime = StartTime;
        //            RaisePropertyChanged(() => EndTime);
        //        }
        //    }
        //}

        //private string GetReminderInfo(Mailbird.Apps.Calendar.Engine.Metadata.Appointment appointment)
        //{
        //    if (SelectedReminder.Key != 0)
        //    {
        //        var apt = new Appointment(AppointmentType.Normal, appointment.StartTime, appointment.EndTime);
        //        var reminder = apt.CreateNewReminder();
        //        reminder.TimeBeforeStart = TimeSpan.FromSeconds(SelectedReminder.Key);
        //        apt.Reminders.Add(reminder);
        //        var helper =
        //            ReminderCollectionXmlPersistenceHelper.CreateSaveInstance(apt, DateSavingType.LocalTime);
        //        return helper.ToXml();
        //    }
        //    return null;
        //}

        //private void SetAppointment(Appointment appointment)
        //{
        //    CurrentAppointmentId = appointment.Id;
        //    Subject = appointment.Subject;
        //    Location = appointment.Location;
        //    StartDate = appointment.Start.Date;
        //    StartTime = appointment.Start.ToString("MM-dd-yyyy HH:mm:ss");
        //    EndDate = appointment.End.Date;
        //    EndTime = appointment.End.ToString("MM-dd-yyyy HH:mm:ss");
        //    LabelId = appointment.LabelId;
        //    ResourceId = appointment.ResourceId;
        //    StatusId = appointment.StatusId;
        //    AllDayEvent = appointment.AllDay;
        //    Description = appointment.Description;
        //    //if appointment has reminder we look for the closest time to our defoult remanders
        //    if (appointment.Reminder != null)
        //    {
        //       var closestKeyPair =
        //            ReminderCollection.First(
        //                n => Math.Abs(appointment.Reminder.TimeBeforeStart.TotalSeconds - n.Key) < double.Epsilon);
        //        SelectedReminder = closestKeyPair;
        //    }
        //    else
        //    {
        //        SelectedReminder = ReminderCollection[0];
        //    }
        //    Appointment = appointment;
        //    SelectedCalendar = appointment.CustomFields["cfCalendar"] as Mailbird.Apps.Calendar.Engine.Metadata.Calendar;
        //}

        //private bool CheckOnEdit()
        //{
        //    if (CurrentAppointmentId == null)
        //        return !string.IsNullOrEmpty(Description)
        //               || !string.IsNullOrEmpty(Subject)
        //               || !string.IsNullOrEmpty(Location);

        //    return Subject != _appointment.Subject
        //               || Location != _appointment.Location
        //               || StartDate.Date != _appointment.Start.Date
        //               || EndDate.Date != _appointment.End.Date
        //               || StartTime != (AllDayEvent ? DefaultTimeValue :_appointment.Start.ToString("MM-dd-yyyy HH:mm:ss"))
        //               || EndTime != (AllDayEvent ? DefaultTimeValue :_appointment.End.ToString("MM-dd-yyyy HH:mm:ss"))
        //               || AllDayEvent != _appointment.AllDay
        //               || LabelId != _appointment.LabelId
        //               || StatusId != _appointment.StatusId
        //               || ResourceId != _appointment.ResourceId
        //               || (_appointment.Reminder != null && Math.Abs(_appointment.Reminder.TimeBeforeStart.TotalSeconds - SelectedReminder.Key) > double.Epsilon)
        //               || (_appointment.Reminder == null && SelectedReminder.Key != 0)
        //               || Description != _appointment.Description;
        //}
        #endregion
    }
}
