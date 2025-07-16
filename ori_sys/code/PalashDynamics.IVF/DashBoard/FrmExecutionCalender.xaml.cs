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

namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmExecutionCalender : ChildWindow
    {
        public SelectedDatesCollection selecteddates;

        public FrmExecutionCalender()
        {
            InitializeComponent();

        }
        public event RoutedEventHandler OnSaveButton_Click;
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }


        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ExeCal.SelectedDate != null)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("", "Please Select Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var r in selecteddates)
            {
                ExeCal.SelectedDates.Add(r.Date);
            }
        }
    }
}

