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
using PalashDynamics.UserControls;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmSurgeryImages : ChildWindow
    {
        public long PatientID;
        public long PatientUnitID;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public long PlannedtreatmentID;
        public clsPlanTherapyVO SelectedTherapyDetails;
        public event RoutedEventHandler OnSaveButton_Click;

        WaitIndicator wait = null;
        public bool IsUpdate = false;
        long ID;
        long UnitID;
        bool IsEdit = false; 

        public frmSurgeryImages()
        {
            InitializeComponent();
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Images For :-(Cycle Code - " + SelectedTherapyDetails.Cyclecode + " )";
            SurgeryDate.SelectedDate = DateTime.Now.Date.Date;
            SurgeryTime.Value = DateTime.Now;
        }


        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

     


        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

