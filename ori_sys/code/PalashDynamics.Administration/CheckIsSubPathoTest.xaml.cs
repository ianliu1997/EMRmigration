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

namespace PalashDynamics.Administration
{
    public partial class CheckIsSubPathoTest : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public bool Checked { get; set; }
        
        public CheckIsSubPathoTest()
        {
            InitializeComponent();
        }
       
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Checked = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Checked = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

