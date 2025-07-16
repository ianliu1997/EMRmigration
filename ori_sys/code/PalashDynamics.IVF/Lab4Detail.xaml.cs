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
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF
{
    public partial class Lab4Detail : ChildWindow
    {
        public Lab4Detail()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler OnSaveButton_Click;
        public clsFemaleLabDay4CalculateDetailsVO Details { get; set; }
        public bool IsUpdate = false; 

        
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Details == null)
            {
                this.DataContext = new clsFemaleLabDay4CalculateDetailsVO();
            }
            else
            {
                this.DataContext = Details;
            }
        }

        private void cmdCalculate_Click(object sender, RoutedEventArgs e)
        {
            
            Details = (clsFemaleLabDay4CalculateDetailsVO)this.DataContext;
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

