using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace DataDrivenApplication
{
    public partial class StartPage : UserControl
    {
        public StartPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(StartPage_Loaded);
            FormSection.Fields = new ObservableCollection<Field>();
        }

        void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
           fdType.ItemsSource = Helper.GetFieldTypesList();
           trvParent.ItemsSource = FormSection.Fields;
        }

        Section FormSection = new Section();

        private void CreateForm_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void fdType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void AddField_Click(object sender, RoutedEventArgs e)
        {
            //if (fdType.SelectedItem != null)
            //{
            //    Field f = new Field();
            //    f.Title = fdTitle.Text;
            //    f.FieldDataType = ((FieldType)fdType.SelectedItem).DataType;
            //    if (trvParent.SelectedItem != null)
            //    {
            //        //f.Parent = (FieldDetail)trvParent.SelectedItem;
            //        if (((Field)trvParent.SelectedItem).DependentFields == null)
            //            ((Field)trvParent.SelectedItem).DependentFields = new ObservableCollection<Field>();
            //        ((Field)trvParent.SelectedItem).DependentFields.Add(f);

            //       // ClearTreeViewSelection(trvParent);
            //        trvParent.ItemsSource = null;
            //        trvParent.ItemsSource = FormSection.Fields;
            //    }
            //    else
            //    {
            //        FormSection.Fields.Add(f);
            //    }
            //}

          
        }

        public static void ClearTreeViewSelection(TreeView tv)
        {
            if (tv != null)
                ClearTreeViewItemsControlSelection(tv.Items, tv.ItemContainerGenerator);
        }
        private static void ClearTreeViewItemsControlSelection(ItemCollection ic, ItemContainerGenerator icg)
        {
            if ((ic != null) && (icg != null))
                for (int i = 0; i < ic.Count; i++)
                {
                    TreeViewItem tvi = icg.ContainerFromIndex(i) as TreeViewItem;
                    if (tvi != null)
                    {
                        ClearTreeViewItemsControlSelection(tvi.Items, tvi.ItemContainerGenerator);
                        tvi.IsSelected = false;
                    }
                }
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void trvParent_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }


    }
    public class Field
    {
        public string Title { get; set; }
        public DataType FieldDataType { get; set; }
        public object FieldSettings { get; set; }
        public object DefaultValue { get; set; }
        public ObservableCollection<Field> DependentFields { get; set; }
    }

    public class Section
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ObservableCollection<Field> Fields { get; set; }
    }

    public enum DataType
    {
        None = 0,
        Text = 1,
        Number = 2,
        Currency = 3,
        Boolean = 4,
        Double = 5,
        DateTime = 6,
        Lookup = 7,
        Choice = 8
    }

    public static  class Helper
    {
        public static List<FieldType> GetFieldTypesList()
        {
            List<FieldType> list = new List<FieldType>();
            //list.Add(new FieldType() { Id = 0, Title = "-- Select -- ", DataType =  DataType.None});
            //list.Add(new FieldType() { Id = 1, Title = "Text", DataType =  DataType.Text});
            //list.Add(new FieldType() { Id = 2, Title = "Number", DataType = DataType.Number });
            //list.Add(new FieldType() { Id = 3, Title = "Currency", DataType = DataType.Currency  });
            //list.Add(new FieldType() { Id = 4, Title = "Boolean", DataType = DataType.Boolean});
            //list.Add(new FieldType() { Id = 5, Title = "Double", DataType = DataType.Double });
            //list.Add(new FieldType() { Id = 6, Title = "DateTime", DataType = DataType.DateTime });
            //list.Add(new FieldType() { Id = 7, Title = "Lookup", DataType = DataType.Lookup});
            //list.Add(new FieldType() { Id = 8, Title = "Choice", DataType =  DataType.Choice});
            return list;
        }
    }


}
