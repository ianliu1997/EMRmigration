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

namespace PalashDynamics.OperationTheatre
{
    public partial class CancelOTBooking : ChildWindow
    {
        int ClickedFlag = 0;
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;

        public CancelOTBooking()
        {
            InitializeComponent();
        }

        /// <summary>
        /// OK button click
        /// </summary>   

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                if (OnSaveButton_Click != null)
                {
                    this.DialogResult = true;
                    OnSaveButton_Click(this, new RoutedEventArgs());

                    this.Close();
                }
            }
        }

        /// <summary>
        /// Cancel Button Click
        /// </summary>
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            OnCancelButton_Click(this, new RoutedEventArgs());
            this.Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

