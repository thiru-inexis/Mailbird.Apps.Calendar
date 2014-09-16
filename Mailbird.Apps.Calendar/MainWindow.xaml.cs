using DevExpress.Xpf.Scheduler;
using System.Windows;
using DevExpress.XtraScheduler;
using Mailbird.Apps.Calendar.UIStyles;
using Mailbird.Apps.Calendar.ViewModels;
using Appointment = Mailbird.Apps.Calendar.Engine.Metadata.Appointment;
using System.Windows.Controls;
using Mailbird.Apps.Calendar.UIModels;
using DevExpress.XtraBars.Alerter;
using DevExpress.XtraBars.ToastNotifications;



namespace Mailbird.Apps.Calendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Scheduler.DayView.MoreButtonDownStyle = (Style)FindResource("MoreButtonDownStyle");
            Scheduler.DayView.MoreButtonUpStyle = (Style)FindResource("MoreButtonUpStyle");
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AppointmentSource")
            {
                Scheduler.Storage.RefreshData();
            }
        }

        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel) DataContext; }
        }
        
        private void Scheduler_OnEditAppointmentFormShowing(object sender, EditAppointmentFormEventArgs e)
        {
            e.Cancel = true;
            //ViewModel.OpenInnerFlyout(Scheduler);
            //FlyoutControl.Focus();
            ViewModel.OpenAppointmentPopUp(Scheduler);
        }

        private void UIElement_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!FlyoutControl.IsOpen) return;
            //ViewModel.CloseInnerFlyout();
        }

        private void SchedulerStorage_AppointmentsChanged(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            foreach (DevExpress.XtraScheduler.Appointment obj in e.Objects)
            {
                var app = new AppointmentUI
                {
                    Id = obj.Id.ToString(),
                    //AllDayEvent = obj.AllDay,
                    Description = obj.Description,
                    EndDateTime = obj.End,
                    //LabelId = obj.LabelId,
                    Location = obj.Location,
                    //ResourceId = obj.ResourceId,
                    StartDateTime = obj.Start,
                    //StatusId = obj.StatusId,
                    Subject = obj.Subject
                };
                ViewModel.UpdateAppointment(app);
            }
        }

        private void SchedulerStorage_OnAppointmentsDeleted(object sender, PersistentObjectsEventArgs e)
        {
            foreach (DevExpress.XtraScheduler.Appointment obj in e.Objects)
            {
                ViewModel.RemoveAppointment(obj.Id);
            }
        }



        //private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    //Scheduler.Storage.RefreshData();
        //}

        private void CalendersListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedCalender =  (((ListBoxItem)sender).Content as UIModels.CalenderUI);
            e.Handled = true;

            if (selectedCalender != null)
            {
                  ViewModel.OpenCalenderPopUp(selectedCalender.Id);

                //UIStyles.CalenderPopupContentStyle calenderModal = new UIStyles.CalenderPopupContentStyle();
                //ViewModel.CalenderPopupViewModel.SelectedCalender = selectedCalender;
                //calenderModal.DataContext = ViewModel.CalenderPopupViewModel;
                //calenderModal.Owner = Application.Current.MainWindow;
                //calenderModal.ShowDialogWindow();
            }
       
        }

        private void SchedulerStorage_ReminderAlert(object sender, ReminderEventArgs e)
        {
            //ToastNotification toast = new ToastNotification();
            //toast.a
            //AlertControl sample = new AlertControl();
            //AlertInfo info = new AlertInfo("Appointment Notification", " Sample Event");
            //sample.Show(Application.Current.MainWindow, info);

            ////try
            ////{

            if(e.AlertNotifications != null && e.AlertNotifications.Count > 0)
            {
               ViewModel.OpenReminderPopup(e.AlertNotifications[0].ActualAppointment.Id.ToString());
            }

            //e.AlertNotifications[0].Reminder.
            //    var form = new DevExpress.Xpf.Scheduler.UI.RemindersForm(Scheduler);
            //    form.OnReminderAlert(e.AlertNotifications);
            //    MessageBox.Show(e.AlertNotifications.Count.ToString());

        }


    }
}
