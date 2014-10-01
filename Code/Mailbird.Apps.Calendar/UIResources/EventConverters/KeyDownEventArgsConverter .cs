using DevExpress.Mvvm.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Mailbird.Apps.Calendar.UIResources.EventConverters
{
    /// <summary>
    /// This caputure a key press related event and binds the argument as a paramenter 
    /// to the command function
    /// </summary>
    public class KeyDownEventArgsConverter : IEventArgsConverter
    {
        public object Convert(object args)
        {
            //object sender, RoutedEventArgs e;
            if (!(args is RoutedEventArgs)) { return null; }
            return args;
        }
    }


    /// <summary>
    /// This caputure a button click on a list item and bind the item obeject
    /// to the command function
    /// </summary>
    public class ListBoxItemClickEventArgsConverter : IEventArgsConverter
    {
        public object Convert(object args)
        {
            if (!(args is ListBoxItem)) { return null; }
            return args;
        }
    }

}
