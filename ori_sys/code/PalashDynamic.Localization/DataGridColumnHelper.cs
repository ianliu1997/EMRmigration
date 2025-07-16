using System.Windows;
using System.Windows.Controls;

namespace PalashDynamic.Localization
{
    public static class DataGridColumnHelper
    {
        public static readonly DependencyProperty HeaderBindingProperty = DependencyProperty.RegisterAttached(
            "HeaderBinding",
            typeof(object),
            typeof(DataGridColumnHelper),
            new PropertyMetadata(null, DataGridColumnHelper.HeaderBinding_PropertyChanged));

        public static object GetHeaderBinding(DependencyObject source)
        {
            return (object)source.GetValue(DataGridColumnHelper.HeaderBindingProperty);
        }

        public static void SetHeaderBinding(DependencyObject target, object value)
        {
            target.SetValue(DataGridColumnHelper.HeaderBindingProperty, value);
        }

        private static void HeaderBinding_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataGridColumn column = d as DataGridColumn;

            if (column == null) { return; }

            column.Header = e.NewValue;
        }
    }
}
