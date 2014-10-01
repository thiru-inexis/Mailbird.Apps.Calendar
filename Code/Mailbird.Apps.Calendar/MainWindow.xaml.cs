using DevExpress.Xpf.Scheduler;
using System.Windows;
using System.Linq;
using DevExpress.XtraScheduler;
using Mailbird.Apps.Calendar.UIStyles;
using Mailbird.Apps.Calendar.ViewModels;
using Appointment = Mailbird.Apps.Calendar.Engine.Metadata.Appointment;
using System.Windows.Controls;
using Mailbird.Apps.Calendar.UIModels;
using DevExpress.XtraBars.Alerter;
using DevExpress.XtraBars.ToastNotifications;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using DevExpress.XtraScheduler.Drawing;
using System.Windows.Controls.Primitives;
using DevExpress.Xpf.Scheduler.Drawing;
using System.Windows.Media;
using DevExpress.Xpf.Core.Native;





namespace Mailbird.Apps.Calendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)DataContext; }
        }


        public MainWindow()
        {
            InitializeComponent();
            ApplySchedularStyles();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        /// <summary>
        /// Force style apply
        /// </summary>
        private void ApplySchedularStyles()
        {

            Style style = (Style)this.FindResource("DateHeaderStyle");
            Scheduler.DayView.DateHeaderStyle = style;
            Scheduler.WorkWeekView.DateHeaderStyle = style;
            Scheduler.WeekView.HorizontalWeekDateHeaderStyle = style;
            Scheduler.MonthView.HorizontalWeekDateHeaderStyle = style;

            Style dayCellStyle = (Style)this.FindResource("DayViewCellStyle");
            Scheduler.DayView.CellStyle = dayCellStyle;
            Scheduler.WorkWeekView.CellStyle = dayCellStyle;

            Style allDayAreaStyle = (Style)this.FindResource("AllDayAreaCellStyle");
            Scheduler.DayView.AllDayAreaCellStyle = allDayAreaStyle;
            Scheduler.WorkWeekView.AllDayAreaCellStyle = allDayAreaStyle;

            Scheduler.WeekView.VerticalWeekCellStyle = (Style)this.FindResource("VerticalWeekCellStyle");
            Scheduler.MonthView.HorizontalWeekCellStyle = (Style)this.FindResource("HorizontalWeekCellStyle");

            Scheduler.TimelineView.CellStyle = (Style)this.FindResource("TimelineViewCellStyle");

            StyleSelector verticalStyleSelector = (StyleSelector)FindResource("VerticalAppointmentStyleSelector");
            Scheduler.DayView.VerticalAppointmentStyleSelector = verticalStyleSelector;
            Scheduler.WorkWeekView.VerticalAppointmentStyleSelector = verticalStyleSelector;

            StyleSelector horizontalStyleSelector = (StyleSelector)FindResource("HorizontalAppointmentStyleSelector");
            Scheduler.DayView.HorizontalAppointmentStyleSelector = horizontalStyleSelector;
            Scheduler.WorkWeekView.HorizontalAppointmentStyleSelector = horizontalStyleSelector;
            Scheduler.WeekView.HorizontalAppointmentStyleSelector = horizontalStyleSelector;
            Scheduler.MonthView.HorizontalAppointmentStyleSelector = horizontalStyleSelector;


            ControlTemplate template = (ControlTemplate)this.FindResource("SelectionTemplate");
            for (var i = 0; i < Scheduler.Views.Count; i++)
            {
                Scheduler.Views[i].SelectionTemplate = template;
            }

            Scheduler.DayView.MoreButtonDownStyle = (Style)FindResource("MoreButtonDownStyle");
            Scheduler.DayView.MoreButtonUpStyle = (Style)FindResource("MoreButtonUpStyle");

        }


        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AppointmentCollection")
            {
                Scheduler.ActiveView.LayoutChanged();
                SchedulerAgenda.RefreshData();
            }
        }


        private void UIElement_GotFocus(object sender, RoutedEventArgs e)
        {
            // To Do
        }


        /// <summary>
        /// show custom edit form
        /// </summary>
        private void Scheduler_OnEditAppointmentFormShowing(object sender, EditAppointmentFormEventArgs e)
        {
            e.Cancel = true;
            if(Scheduler.SelectedAppointments != null && Scheduler.SelectedAppointments.Any())
            { 
                ViewModel.OpenAppointmentPopUp(Scheduler.SelectedAppointments[0].Id.ToString());
            }
            else 
            {
                ViewModel.OpenAppointmentPopUp(Scheduler.SelectedInterval.Start, Scheduler.SelectedInterval.End);
            }
        }


        /// <summary>
        /// Update changes on drag and drop
        /// </summary>
        private void SchedulerStorage_AppointmentsChanged(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            foreach (DevExpress.XtraScheduler.Appointment obj in e.Objects)
            {
                var modelToUpdate = ViewModel.AppointmentCollection.FirstOrDefault(m => m.Id == obj.Id.ToString());
                if (modelToUpdate != null)
                {
                    ViewModel.UpdateAppointment(modelToUpdate);
                }
            }
        }

        /// <summary>
        /// Update changes on delete key pressed on appointment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SchedulerStorage_OnAppointmentsDeleted(object sender, PersistentObjectsEventArgs e)
        {
            foreach (DevExpress.XtraScheduler.Appointment obj in e.Objects)
            {
                ViewModel.DeleteAppointment(new UIModels.AppointmentUI() { Id = obj.Id.ToString()});
            }
        }


        /// <summary>
        /// Handle this event to authorize delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SchedulerStorage_AppointmentDeleting(object sender, PersistentObjectCancelEventArgs e)
        { }


        /// <summary>
        /// Custom reminder notification pop up
        /// </summary>
        private void SchedulerStorage_ReminderAlert(object sender, ReminderEventArgs e)
        {
            if(e.AlertNotifications != null && e.AlertNotifications.Count > 0)
            {
               ViewModel.OpenReminderPopup(e.AlertNotifications[0].ActualAppointment.Id.ToString());
            }
        }

        /// <summary>
        /// Test function for SEND/RECEIVE .ICS files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            e.Handled = true;
            //ViewModel.OpenSendInvitationPopup(Scheduler);
            ViewModel.OpenReceiveInvitationPopup();
        }

        /// <summary>
        /// Cutom filter for hiding/showing appointments on scheduler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SchedulerStorage_FilterAppointment(object sender, PersistentObjectCancelEventArgs e)
        {
            var apt = (DevExpress.XtraScheduler.Appointment)e.Object;
            e.Cancel = !ViewModel.IsAppointmentVisible(ViewModel.AppointmentCollection.FirstOrDefault(m =>(m.Id == (string)apt.Id)));
        }


        /// <summary>
        /// Custom filter for grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridControl_CustomRowFilter(object sender, DevExpress.Xpf.Grid.RowFilterEventArgs e)
        {
            if (SchedulerAgenda.View.GetRowElementByRowHandle(e.ListSourceRowIndex) != null)
            {
                var sourceAppointment = (((SchedulerAgenda.View.GetRowElementByRowHandle(e.ListSourceRowIndex)).DataContext as RowData).Row as AppointmentUI);
                if (sourceAppointment != null)
                {
                    e.Visible = ViewModel.IsAppointmentVisible(sourceAppointment);
                    e.Handled = true;
                }
            }
        }


        /// <summary>
        /// Fired mouse's left click is released, hence an visual element has been selected.
        /// If the selected elements count == 1 and is an appointment, a details flyout is shown.
        /// </summary>
        private void Scheduler_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.AppointmentDetailsPopupVM.IsOpen = false;
            if (Scheduler.SelectedAppointments == null || Scheduler.SelectedAppointments.Count != 1) { return; }           

            VisualAppointmentControl selectedAppointmentVisual = null;
            // Iterate through each visual elements of the scheuler and get the selected appointment's visual
            LayoutHelper.ForEachElement(Scheduler, delegate(FrameworkElement fwElement)
            {
                if (fwElement is VisualAppointmentControl && 
                    ((VisualAppointmentControl)fwElement).GetAppointment().Id == Scheduler.SelectedAppointments[0].Id)
                {
                    selectedAppointmentVisual = (VisualAppointmentControl)fwElement;
                    return;
                }
            });
            ViewModel.OpenAppointmentDetailsPopup(selectedAppointmentVisual);
        }


        private void OnUserCalendars_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (!(sender is TreeViewItem)) { return; }
            if (!(sender as TreeViewItem).IsSelected) { return; }
            if ((sender as TreeViewItem).Header is UIModels.UserInfoUI) { return; }
            if ((sender as TreeViewItem).Header is UIModels.CalenderUI) 
            {
                ViewModel.OpenCalenderPopUp(((sender as TreeViewItem).Header as UIModels.CalenderUI).Id);
            }
        }

    }
}
