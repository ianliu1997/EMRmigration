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

namespace DataDrivenApplication
{
    public partial class SilverlightControl1 : UserControl
    {
        public SilverlightControl1()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SilverlightControl1_Loaded);
        }

        void SilverlightControl1_Loaded(object sender, RoutedEventArgs e)
        {
            List<ListItem> Items1 = new List<ListItem>
            {
                new ListItem()
                {
                    Title="Item1",
                    Items = new List<ListItem>
                    {
                          new ListItem()
                            {
                            Title="Item11",
                            Items = new List<ListItem>
                            {
                          new ListItem()
                            {
                            Title="Item11"
                            }
                            }
                            }
                    }
                }
            };


            foreach (var item in Items1)
            {
                AddNodeItems(item);
            }

            SampleSelection.ItemsSource = Items1;

        }

        List<ListItem> Items = new List<ListItem>();

        public void AddNodeItems(ListItem PItem)
        {
            Items.Add(PItem);
            if (PItem.Items != null && PItem.Items.Count > 0)
            foreach (var item in PItem.Items)
            {
                AddNodeItems(item);
            }
        }
    }



    public class ListItem
    {
        public string Title { get; set; }
        public List<ListItem> Items { get; set; }
    }
}
