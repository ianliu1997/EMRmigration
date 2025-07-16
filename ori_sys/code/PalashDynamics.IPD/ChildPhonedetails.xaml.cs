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
using PalashDynamics.Animations;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Collections;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.IPD
{
    public partial class ChildPhonedetails : ChildWindow
    {
        public string PatientName = null;
        public string ContactNo = null;
        public string BedName = null;
        public string ClassName = null;
        public string WardName = null;
        public clsIPDBedReservationVO details { get; set; }

        string msgText = null;
        public ChildPhonedetails()
        {
            InitializeComponent();
           
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (Validation())
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }
            
            //SaveLog();
        }

        public bool Validation()
        {
            if (string.IsNullOrEmpty(txtFeedback.Text))
            {
                txtFeedback.SetValidation("Please Enter Patient Feedback");
                txtFeedback.RaiseValidationError();
                txtFeedback.Focus();
                return false;
            }
            else
            {
                txtFeedback.ClearValidationError();
                return true;
            }
        }

        public void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsAddIPDBedReservationBizActionVO bizActionVO = new clsAddIPDBedReservationBizActionVO();
                    bizActionVO.BedDetails = new clsIPDBedReservationVO();
                    bizActionVO.BedDetails.IsFromReminderLog = true;
                    bizActionVO.BedDetails.PatientID = details.PatientID;
                    bizActionVO.BedDetails.PatientUnitID = details.PatientUnitID;
                    bizActionVO.BedDetails.ContactNo1 = details.ContactNo1;
                    bizActionVO.BedDetails.BedCode = details.BedCode;
                    bizActionVO.BedDetails.ClassName = details.ClassName;
                    bizActionVO.BedDetails.Ward = details.Ward;
                    bizActionVO.BedDetails.CallingDate = Convert.ToDateTime(txtCallingDate.Text);
                    bizActionVO.BedDetails.CallingTime = Convert.ToDateTime(txtCallingTime.Value);
                    bizActionVO.BedDetails.Remark = txtFeedback.Text;
                    bizActionVO.BedDetails.Source = "Phone";
                    //bizActionVO.BedDetails.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //bizActionVO.BedDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //bizActionVO.BedDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    //bizActionVO.BedDetails.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    //bizActionVO.BedDetails.AddedDateTime = System.DateTime.Now;
                    //bizActionVO.BedDetails.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            msgText = "Phone Call log saved Succesfully";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            this.DialogResult = false;

                        }
                        else
                        {
                            msgText = "Error occurred while processing.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtPatientName.Text = details.PatientName;
            txtContactNo.Text = details.ContactNo1;
            txtBedName.Text = details.BedCode;
            txtClassName.Text = details.ClassName;
            txtwardName.Text = details.Ward;
            txtCallingDate.Text = DateTime.Now.ToShortDateString();
            txtCallingTime.Value = DateTime.Now.ToLocalTime();
            
        }
        
    }
}

