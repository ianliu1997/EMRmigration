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
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using System.Windows.Controls.Data;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using OPDModule;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.Service.EmailServiceReference;

namespace OPDModule.Forms
{
    public partial class PatientVisitDetails : ChildWindow
    {
        public PatientVisitDetails()
        {
            InitializeComponent();
            //string MRNo;
            //long UnitID;
            //long PatientUnitId;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

      

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

          



    
    }
}

