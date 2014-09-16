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
    public class CalenderPopupViewModel : Infrastructure.ViewModelBase
    {
        #region Private Attributes
        private CalenderUI _basemodel;

        private TimeZoneInfo _selectedTimeZone;
        //private string _name;
        //private string _description;
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
                return (_basemodel == null || string.IsNullOrEmpty(_basemodel.Id));
            }
        }


        public CalenderUI SelectedCalender
        {
            get { return _basemodel; }
            set 
            {
                if (value == null) { value = new CalenderUI(); }
                _basemodel = value;
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



        public string Summary
        {
            get { return _basemodel.Summary; }
            set 
            {
                _basemodel.Summary = value;
                RaisePropertyChanged(() => Summary); 
            }
        }

        public string Description
        {
            get { return _basemodel.Description; }
            set
            {
                _basemodel.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public string Location
        {
            get { return _basemodel.Location; }
            set
            {
                _basemodel.Location = value;
                RaisePropertyChanged(() => Location);
            }
        }




        public string BackgroundColor
        {
            get { return _basemodel.BackgroundColor; }
            set
            {
                _basemodel.BackgroundColor = value;
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
            _basemodel = new CalenderUI();
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
            InsertCalenderAction(_basemodel);
            CancelCalenderAction();
        }
        private void OnUpdateCommandInvoked()
        {
            UpdateCalenderAction(_basemodel);
            CancelCalenderAction();
        }
        private void OnDeleteCommandInvoked()
        {
            DeleteCalenderAction(_basemodel);
            CancelCalenderAction();
        }
        private void OnCancelCommandInvoked()
        {
            CancelCalenderAction();
        }


        public Action<UIModels.CalenderUI> InsertCalenderAction { get; set; }
        public Action<UIModels.CalenderUI> UpdateCalenderAction { get; set; }
        public Action<UIModels.CalenderUI> DeleteCalenderAction { get; set; }
        public Action CancelCalenderAction { get; set; }

    }
}
