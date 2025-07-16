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
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using CIMS;
using MessageBoxControl;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmViewConsent : ChildWindow
    {
        long PatientID, ConsentID,ScheduleID;
        public string msgText = string.Empty;
        List<clsConsentDetailsVO> AllConsentList = new List<clsConsentDetailsVO>();

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        public frmViewConsent()
        {
            InitializeComponent();

        }

        public frmViewConsent(long PatientID, long ConsentID)
        {
            InitializeComponent();
            this.PatientID = PatientID;
            this.ConsentID = ConsentID;


        }

        public frmViewConsent(long ScheduleId, long PatientID, long ConsentID)
        {
            InitializeComponent();
            this.ScheduleID = ScheduleId;
            this.PatientID = PatientID;
            this.ConsentID = ConsentID;


        }

        void frmViewConsent_Loaded(object sender, RoutedEventArgs e)
        {
            GetPatientConsent();
        }


        public void GetPatientConsent()
        {
            clsGetPatientConsentsBizActionVO BizAction = new clsGetPatientConsentsBizActionVO();
            BizAction.ScheduleID = ScheduleID;
            BizAction.ConsentID = ConsentID;
            BizAction.PatientID = PatientID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetPatientConsentsBizActionVO)arg.Result).ConsentList != null)
                    {
                        foreach (var item in ((clsGetPatientConsentsBizActionVO)arg.Result).ConsentList )
                        {
                           // clsConsentDetailsVO ConsentObj = new clsConsentDetailsVO();
                            AllConsentList.Add(item);
                        }
                        dgViewConsent.ItemsSource = null;
                        dgViewConsent.ItemsSource = AllConsentList;
                        dgViewConsent.UpdateLayout();
                        //dgViewConsent.ItemsSource = ((clsGetPatientConsentsBizActionVO)arg.Result).ConsentList;
                    }
                }
                else
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                    //}
                    //else
                    //{
                        msgText = "Error occured while processing.";
                    //}

                    ShowMessageBox(msgText, MessageBoxButtons.Ok, MessageBoxIcon.Error);

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow cw = (ChildWindow)this.Parent;
            cw.DialogResult = false;           
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            clsConsentDetailsVO ObjDetails = new clsConsentDetailsVO();
            ObjDetails = (clsConsentDetailsVO)dgViewConsent.SelectedItem;

            if (ObjDetails != null)
            {
                string template = ObjDetails.Consent;
                frmPrintConsent win_PrintConsent = new frmPrintConsent(template, (clsConsentDetailsVO)dgViewConsent.SelectedItem);
                win_PrintConsent.Width = this.ActualWidth * 0.8;
                win_PrintConsent.Height = this.ActualHeight;
                win_PrintConsent.Show();
            }

        }

        private void cmdDeleteConsent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgViewConsent.SelectedItem != null)
                {
                    string msgTitle = "";
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteValidation_Msg");
                    msgText = "Are you sure you want to delete record ?";
                    int index = dgViewConsent.SelectedIndex;
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_OnMessageBoxClosed);
                    msgWD.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void msgWD_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsDeleteConsentBizActionVO BizAction = new clsDeleteConsentBizActionVO();
                BizAction.Consent = (clsConsentDetailsVO)dgViewConsent.SelectedItem;
                BizAction.Consent.ScheduleID = ScheduleID;
                BizAction.Consent.PatientID = PatientID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsDeleteConsentBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            //{
                            //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeletVerify_Msg");
                            //}
                            //else
                            //{
                                msgText = "Record deleted Successfully.";
                            //}
                        }
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        int index = dgViewConsent.SelectedIndex;
                        AllConsentList.RemoveAt(index);
                        dgViewConsent.ItemsSource = null;
                        dgViewConsent.ItemsSource = AllConsentList;
                        //dgViewConsent.UpdateLayout();
                        //GetPatientConsent();
                    }
                    else
                    {
                        //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                        //{
                        //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                            msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            //throw new NotImplementedException();
        }
    }
}

