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
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace DataDrivenApplication
{
    public partial class PatientSelector : ChildWindow
    {
        public PatientSelector()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(FormEditor_Loaded);
        }

        void FormEditor_Loaded(object sender, RoutedEventArgs e)
        {
            //DataTemplateServiceClient client = new DataTemplateServiceClient();
            //client.GetPatientListCompleted += (s, args) =>
            //{
            //    if (args.Error == null && args.Result != null)
            //    {
            //        tbTitle.ItemsSource = args.Result;
            //    }

            //};
            //client.GetPatientListAsync();

        }

        public event RoutedEventHandler OnOkButtonClick;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnCreateForm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnOkButtonClick != null)
            {
                OnOkButtonClick(tbTitle.SelectedItem, e);
            }
            this.DialogResult = true;
        }
    }
}

