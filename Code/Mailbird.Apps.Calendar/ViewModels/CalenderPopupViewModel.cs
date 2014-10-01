using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine;
using Mailbird.Apps.Calendar.Engine.Interfaces;
using DevExpress.Mvvm;
using System.ComponentModel;
using Mailbird.Apps.Calendar.UIModels;

namespace Mailbird.Apps.Calendar.ViewModels
{

    /// <summary>
    /// To the binded to the calendar edit/update modal popup
    /// </summary>
    public class CalenderPopupViewModel : Infrastructure.ViewModelBase,ISupportServices
    {
        #region Private Attributes

        private CalenderUI _selectedCalendar;
        private TimeZoneInfo _selectedTimeZone;
        private ObservableCollection<UIModels.UserInfoUI> _availableUsers;

        #endregion


        protected DelegateCommand _insertUICommand;
        protected DelegateCommand _updateUICommand;
        protected DelegateCommand _cancelUICommand;
        protected DelegateCommand _deleteUICommand;


        #region Public Properties [Bindable]

        public String Title 
        {
            get 
            {
                if (IsNewCalender) { return "New Calender"; }
                return string.Format("Edit - [{0}]", Summary);
            }
        }

        public bool IsNewCalender
        {
            get
            {
                return (_selectedCalendar == null || string.IsNullOrEmpty(_selectedCalendar.Id));
            }
        }

        public CalenderUI SelectedCalender
        {
            get { return _selectedCalendar; }
            set 
            {
                if (value == null) { value = new CalenderUI(); }
                _selectedCalendar = value;
                RaisePropertyChanged(() => SelectedCalender);
            }
        }

        public TimeZoneInfo SelectedTimeZone
        {
            get 
            {
                if (_selectedTimeZone == null) { _selectedTimeZone = TimeZoneInfo.Utc; }
                return _selectedTimeZone; }
            set
            {
                _selectedTimeZone = value;
                RaisePropertyChanged(() => SelectedTimeZone);
            }
        }

        public List<TimeZoneInfo> AvailableTimeZones
        {
            get { return TimeZoneInfo.GetSystemTimeZones().ToList(); }
        }

        public ObservableCollection<UserInfoUI> AvailableUsers
        {
            get { return _availableUsers; }
            set
            {
                _availableUsers = value;
                RaisePropertyChanged(() => AvailableUsers);
            }
        }

        public UIModels.UserInfoUI SelectedUser
        {
            get
            {
                if (AvailableUsers == null || !AvailableUsers.Any()) { return null; }
                if (_selectedCalendar.UserId == null)
                {
                    return AvailableUsers.FirstOrDefault();
                }
                return AvailableUsers.FirstOrDefault(m => (m.Id == _selectedCalendar.UserId));
            }
            set
            {
                // Dont not allow to set, if its in edit mode
                if (!IsNewCalender) { return; }
                _selectedCalendar.UserId = value.Id;
                RaisePropertyChanged(() => SelectedCalender);
            }
        }


        public string Summary
        {
            get { return _selectedCalendar.Summary; }
            set 
            {
                _selectedCalendar.Summary = value;
                RaisePropertyChanged(() => Summary); 
            }
        }

        public string Description
        {
            get { return _selectedCalendar.Description; }
            set
            {
                _selectedCalendar.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public string Location
        {
            get { return _selectedCalendar.Location; }
            set
            {
                _selectedCalendar.Location = value;
                RaisePropertyChanged(() => Location);
            }
        }

        public string BackgroundColor
        {
            get { return _selectedCalendar.BackgroundColor; }
            set
            {
                _selectedCalendar.BackgroundColor = value;
                RaisePropertyChanged(() => BackgroundColor);
            }
        }

        #endregion


        public DelegateCommand InsertUICommand
        {
            get
            {
                if (IsNewCalender && _insertUICommand == null)
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
                if (!IsNewCalender && _updateUICommand == null)
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
                if (!IsNewCalender && _deleteUICommand == null)
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


        public CalenderPopupViewModel()
            :base()
        {
            _selectedCalendar = new CalenderUI();
        }



        private void loadModal()
        {
            RaisePropertyChanged(() => SelectedCalender);
            RaisePropertyChanged(() => InsertUICommand);
            RaisePropertyChanged(() => UpdateUICommand);
            RaisePropertyChanged(() => DeleteUICommand);
            RaisePropertyChanged(() => CancelUICommand);
        }

        private void OnInsertCommandInvoked()
        {
            InsertCalenderAction(_selectedCalendar);
            CancelCalenderAction();
        }
        private void OnUpdateCommandInvoked()
        {
            UpdateCalenderAction(_selectedCalendar);
            CancelCalenderAction();
        }
        private void OnDeleteCommandInvoked()
        {
            DeleteCalenderAction(_selectedCalendar);
            CancelCalenderAction();
        }
        private void OnCancelCommandInvoked()
        {
            CancelCalenderAction();
        }


        public Action<UIModels.CalenderUI> InsertCalenderAction { get; set; }
        public Action<UIModels.CalenderUI> UpdateCalenderAction { get; set; }
        public Action<UIModels.CalenderUI> DeleteCalenderAction { get; set; }
        public void CancelCalenderAction()
        {
            _currentWindowService.Close();
        }



        /// <summary>
        /// Devexpress dialog services
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

        ICurrentWindowService _currentWindowService { get { return _serviceContainer.GetService<ICurrentWindowService>(); } }
       

    }
}
