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
using System.Collections.ObjectModel;

namespace PalashDynamics.IVF
{
    public partial class Lab1Detail : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public clsFemaleLabDay1CalculateDetailsVO Details { get; set; }
        public ObservableCollection<clsFemaleLabDay1CalculateDetailsVO> GetDetails { get; set; }
        public bool IsUpdate = false; 

        public Lab1Detail()
        {
            InitializeComponent();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Details == null)
            {
                this.DataContext = new clsFemaleLabDay1CalculateDetailsVO();
            }
            else
            {
                this.DataContext = Details;
            }
        }

        private void cmdCalculate_Click(object sender, RoutedEventArgs e)
        {
            Details = (clsFemaleLabDay1CalculateDetailsVO)this.DataContext;
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());


        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            
        }

        private void GetCalcDetails()
        {
            foreach (var item in GetDetails)
            {
                if (item.TwoPNClosed == true)
                    chkPNClosed.IsChecked = true;
                else
                    chkPNSeparated.IsChecked = true;

                if (item.NucleoliAlign == true)
                    chkNucleouAligned.IsChecked = true;
                else if (item.BeginningAlign == true)
                    chkBegining.IsChecked = true;
                else
                    chkScattered.IsChecked = true;

                if (item.CytoplasmHetero == true)
                    ChkHetero.IsChecked = true;
                else
                    chkHomo.IsChecked = true;

                if (item.NuclearMembrane == true)
                    chkNuclear.IsChecked = true;
                else
                    chkNuclear.IsChecked = false;


                    
            }
          
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

    }
}

