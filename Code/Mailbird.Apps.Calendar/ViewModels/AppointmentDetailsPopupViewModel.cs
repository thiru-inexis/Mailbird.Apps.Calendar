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
using System.Windows;

namespace Mailbird.Apps.Calendar.ViewModels
{
    /// <summary>
    /// This model bounds to the appointment details popup/flyout
    /// tooltip, and is expected to open on appointment select
    /// </summary>
    public class AppointmentDetailsPopupViewModel : Infrastructure.ViewModelBase
    {
        #region Private Attributes

        private AppointmentUI _selectedAppointment;
        private UIElement _placementTarget;
        private bool _isOpen;


        #endregion

        protected DelegateCommand _updateUICommand;
        protected DelegateCommand _cancelUICommand;
        protected DelegateCommand _deleteUICommand;


        #region Public Properties [Bindable]

        public AppointmentUI SelectedAppointment
        {
            get { return _selectedAppointment; }
            set
            {
                _selectedAppointment = value;
                RaisePropertyChanged(() => SelectedAppointment);
            }
        }


        public string Summary
        {
            get { return _selectedAppointment.Summary; }
            set
            {
                _selectedAppointment.Summary = value;
                RaisePropertyChanged(() => Summary);
            }
        }

        public string Description
        {
            get { return _selectedAppointment.Description; }
            set
            {
                _selectedAppointment.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public string Location
        {
            get { return _selectedAppointment.Location; }
            set
            {
                _selectedAppointment.Location = value;
                RaisePropertyChanged(() => Location);
            }
        }

        public string BackgroundColor
        {
            get { return _selectedAppointment.BackgroundColor; }
            set
            {
                _selectedAppointment.BackgroundColor = value;
                RaisePropertyChanged(() => BackgroundColor);
            }
        }



        public DateTime StartDateTime
        {
            get { return _selectedAppointment.StartDateTime; }
            set
            {
                _selectedAppointment.StartDateTime = value;
                RaisePropertyChanged(() => StartDateTime);
            }
        }

        public DateTime EndDateTime
        {
            get { return _selectedAppointment.EndDateTime; }
            set
            {
                _selectedAppointment.EndDateTime = value;
                RaisePropertyChanged(() => EndDateTime);
            }
        }




        public UIElement PlacementTarget
        {
            get { return _placementTarget; }
            set
            {
                _placementTarget = value;
                RaisePropertyChanged(() => PlacementTarget);
            }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                RaisePropertyChanged(() => IsOpen);
            }
        }

        #endregion


        public DelegateCommand UpdateUICommand
        {
            get
            {
                if (_updateUICommand == null)
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
                if (_deleteUICommand == null)
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



        public AppointmentDetailsPopupViewModel(AppointmentUI selectedAppointment)
            : base()
        {
            _selectedAppointment = selectedAppointment;
            _placementTarget = null;
            _isOpen = false;
        }


        public AppointmentDetailsPopupViewModel()
            : this(new AppointmentUI())
        { }

        /// <summary>
        /// Force reload of bounded properties.
        /// Prefer to be called on open
        /// </summary>
        public void RefreshModal()
        {
            RaisePropertyChanged(() => Location);
            RaisePropertyChanged(() => Summary);
            RaisePropertyChanged(() => Description);
            RaisePropertyChanged(() => BackgroundColor);
            RaisePropertyChanged(() => PlacementTarget);
            RaisePropertyChanged(() => EndDateTime);
            RaisePropertyChanged(() => StartDateTime);
        }



        private void OnUpdateCommandInvoked()
        {
            CancelCalenderAction();
            UpdateAppointmentAction(_selectedAppointment.Id);
        }
        private void OnDeleteCommandInvoked()
        {
            CancelCalenderAction();
            DeleteAppointmentAction(_selectedAppointment);

        }
        private void OnCancelCommandInvoked()
        {
            CancelCalenderAction();
        }





        public Action<string> UpdateAppointmentAction { get; set; }
        public Action<AppointmentUI> DeleteAppointmentAction { get; set; }
        public void CancelCalenderAction()
        {
            IsOpen = false;
        }

    }
}
