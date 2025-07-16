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
using PalashDynamics.ValueObjects.Patient;



namespace DataDrivenApplication.Forms
{
    public partial class frmAttention : ChildWindow
    {
        public string AlertsExpanded { get; set; }

        public frmAttention()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(frmAttention_Loaded);
        }
        void frmAttention_Loaded(object sender, RoutedEventArgs e)
        {
            txtAlerts.Text = AlertsExpanded;
            
               
         }
        //public void Initiate(string Mode)
        //{
        //    //throw new NotImplementedException();
        //    switch (Mode)
        //    {
        //        case "MaleAttention":
        //            //isReg = true;
                    
        //               clsGetGetCoupleHeightAndWeightBizActionVO BizAction = new clsGetGetCoupleHeightAndWeightBizActionVO();
        //              BizAction.CoupleDetails = new clsCoupleVO();
        //               clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
        //              MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;

               
              
        //            frmAttention PatientAlert = new frmAttention();
        //            PatientAlert.AlertsExpanded = BizAction.CoupleDetails.MalePatient.Alerts;
        //            //PatientAlert.Show();
        //            break;
        //        case "FemaleAttention":

        //            //PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
        //            //PatientAlert.Show();
        //            break;
        //        default:

        //            break;
        //    }
        //}

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
       

    
        }
    }


