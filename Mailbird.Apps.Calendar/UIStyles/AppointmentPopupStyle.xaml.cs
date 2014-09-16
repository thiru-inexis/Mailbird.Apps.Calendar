using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mailbird.Apps.Calendar.UIStyles
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AppointmentPopupStyle
    {

        public AppointmentPopupStyle()
        {
            InitializeComponent();
        }

        private void ComboBoxEdit_KeyUpDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.IsDown)
            {
                ((DevExpress.Xpf.Editors.ComboBoxEdit)sender).IsPopupOpen = true;
            }
        }

        private void ComboBoxEdit_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ComboBoxEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var element = ((DevExpress.Xpf.Editors.ComboBoxEdit)sender);
            element.EditValue = element.SelectedText;
        }


        //public static CalenderPopupContentStyle GetInstance()
        //{
        //    if (_control == null) { _control = new CalenderPopupContentStyle(); }
        //    return _control;
        //}


    }
}
