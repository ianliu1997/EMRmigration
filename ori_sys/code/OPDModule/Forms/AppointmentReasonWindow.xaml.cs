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
using OPDModule.Forms;
using CIMS.Forms;


namespace OPDModule.Forms
{
    public partial class AppointmentReasonWindow : ChildWindow
    {
        int ClickedFlag = 0;
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        WaitIndicator Indicatior = null;
        public string PatientName = null;
        public DateTime? FromDate;
        public DateTime? ToDate;
        public DateTime? AppointmentDate;
        string fromTime = " ";
        string toTime = " ";
        public AppointmentReasonWindow()
        {
            InitializeComponent();
           
        }
        private void frmAppointmentReason_Loaded(object sender, RoutedEventArgs e)
        {
                //Indicatior = new WaitIndicator();
                //Indicatior.Show();

              //  txtAppReason.Focus();
            DateTime aft = FromDate.Value; ;
            fromTime = aft.ToString("hh: mm tt");
            DateTime att = ToDate.Value;
            toTime = att.ToString("hh: mm tt");
            DateTime dt = AppointmentDate.Value.Date;
            string adt = dt.ToString("dd/MM/yy");
               // this.Title = "Appointment Cancel Reason-" + PatientName + " " + FromDate.ToString() + " " + ToDate.ToString();
            this.Title = PatientName + " " + adt + " " + fromTime + "-" + toTime;
           
        }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;           
            OnCancelButton_Click(this, new RoutedEventArgs());
            this.Close();

        }
         
        /// <summary>
         /// Purpose:Save appointment reason  
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                if (txtAppReason.Text.Trim() == "")  //Added by ajit date 5/10/2016
                {
                    txtAppReason.SetValidation("Please enter reason");
                    txtAppReason.RaiseValidationError();
                    txtAppReason.Focus();
                    ClickedFlag = 0;
                }
                else
                {
                    txtAppReason.ClearValidationError();
                    ClickedFlag = 0;
                    if (OnSaveButton_Click != null)
                    {
                        this.DialogResult = true;
                        OnSaveButton_Click(this, new RoutedEventArgs());

                        this.Close();
                    }
                }
            }
  
        }

  
        
    }
}

