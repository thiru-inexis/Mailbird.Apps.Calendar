﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "71C91F94195388450D081E5728B91E84"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Core;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.DataSources;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.DataPager;
using DevExpress.Xpf.Editors.DateNavigator;
using DevExpress.Xpf.Editors.ExpressionEditor;
using DevExpress.Xpf.Editors.Filtering;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Editors.Popups;
using DevExpress.Xpf.Editors.Popups.Calendar;
using DevExpress.Xpf.Editors.RangeControl;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Editors.Settings.Extension;
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.Scheduler.Commands;
using DevExpress.Xpf.Scheduler.Reporting;
using DevExpress.Xpf.Scheduler.UI;
using Mailbird.Apps.Calendar.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Mailbird.Apps.Calendar {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : DevExpress.Xpf.Core.DXWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Menu;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.DateEdit DateSelector;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.DateEditPickerStyleSettings DateEditPicker;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Selectors;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox SelectorList;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.Flyout.FlyoutControl FlyoutControl;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Scheduler.SchedulerControl Scheduler;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Scheduler.ResourceMapping Name;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Mailbird.Apps.Calendar;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.Menu = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\MainWindow.xaml"
            this.Menu.GotFocus += new System.Windows.RoutedEventHandler(this.UIElement_GotFocus);
            
            #line default
            #line hidden
            return;
            case 3:
            this.DateSelector = ((DevExpress.Xpf.Editors.DateEdit)(target));
            
            #line 75 "..\..\MainWindow.xaml"
            this.DateSelector.GotFocus += new System.Windows.RoutedEventHandler(this.UIElement_GotFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DateEditPicker = ((DevExpress.Xpf.Editors.DateEditPickerStyleSettings)(target));
            return;
            case 5:
            this.Selectors = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 6:
            this.SelectorList = ((System.Windows.Controls.ListBox)(target));
            
            #line 87 "..\..\MainWindow.xaml"
            this.SelectorList.GotFocus += new System.Windows.RoutedEventHandler(this.UIElement_GotFocus);
            
            #line default
            #line hidden
            return;
            case 7:
            this.FlyoutControl = ((DevExpress.Xpf.Editors.Flyout.FlyoutControl)(target));
            return;
            case 8:
            this.Scheduler = ((DevExpress.Xpf.Scheduler.SchedulerControl)(target));
            
            #line 113 "..\..\MainWindow.xaml"
            this.Scheduler.EditAppointmentFormShowing += new DevExpress.Xpf.Scheduler.AppointmentFormEventHandler(this.Scheduler_OnEditAppointmentFormShowing);
            
            #line default
            #line hidden
            
            #line 114 "..\..\MainWindow.xaml"
            this.Scheduler.GotFocus += new System.Windows.RoutedEventHandler(this.UIElement_GotFocus);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 116 "..\..\MainWindow.xaml"
            ((DevExpress.Xpf.Scheduler.SchedulerStorage)(target)).AppointmentsChanged += new DevExpress.XtraScheduler.PersistentObjectsEventHandler(this.SchedulerStorage_AppointmentsChanged);
            
            #line default
            #line hidden
            
            #line 117 "..\..\MainWindow.xaml"
            ((DevExpress.Xpf.Scheduler.SchedulerStorage)(target)).AppointmentsDeleted += new DevExpress.XtraScheduler.PersistentObjectsEventHandler(this.SchedulerStorage_OnAppointmentsDeleted);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Name = ((DevExpress.Xpf.Scheduler.ResourceMapping)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

