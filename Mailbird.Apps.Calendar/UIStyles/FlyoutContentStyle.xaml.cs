using System.Windows;
using DevExpress.Xpf.Grid;

namespace Mailbird.Apps.Calendar.UIStyles
{
    partial class FlyoutContentStyle
    {
        public FlyoutContentStyle()
        {
            InitializeComponent();
        }
    }

    public class MyTreeListView : TreeListView
    {
        static MyTreeListView()
        {
            FocusedRowHandleProperty.OverrideMetadata(typeof(MyTreeListView), new FrameworkPropertyMetadata(null, B));
        }

        private static object B(DependencyObject d, object baseValue)
        {
            var view = (TreeListView)d;
            var rowHandle = (int)baseValue;
            var node = view.GetNodeByRowHandle(rowHandle);
            if (node == null || node.Level != 0) return baseValue;
            rowHandle++;
            return rowHandle < view.DataControl.VisibleRowCount ? B(view, rowHandle) : view.FocusedRowHandle;
        }
    }
}
