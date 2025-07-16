using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.IPD;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmPatientConsentDetails : UserControl
    {
        #region Variable Declaration
        public string msgText { get; set; }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public string ConsentText;
        public List<MasterListItem> DataList { get; private set; }
        List<clsPatientFieldsConfigVO> objListDesc = null;
        private clsConsentDetailsVO selectedPatient;
        long VisitAdmID, VisitAdmUnitID, PatientID, PatientUnitID, ScheduleID;
        bool IsForOT = false, IsForIPD = false, IsForOPD = false;
        int OpdIpd;

        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Page Load

        public frmPatientConsentDetails()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmPatientConsentDetails_Loaded);
        }

        public frmPatientConsentDetails(long PatientID, long PatientUnitID, long VisitID, long UnitID)
        {
            InitializeComponent();
            this.VisitAdmID = VisitID;
            this.VisitAdmUnitID = UnitID;
            this.PatientID = PatientID;
            this.PatientUnitID = PatientUnitID;
            richToolbar.RichTextBox = richTextEditor;
            OpdIpd = 0;
            IsForOPD = true;
            getDetailsOfSelectedPatient();
            getDescription();
            this.Loaded += new RoutedEventHandler(frmPatientConsentDetails_Loaded);
        }

        public frmPatientConsentDetails(long ScheduleId, long PatientID, long PatientUnitID, long VisitID, long UnitID)
        {
            InitializeComponent();
            this.ScheduleID = ScheduleId;
            this.VisitAdmID = VisitID;
            this.VisitAdmUnitID = UnitID;
            this.PatientID = PatientID;
            this.PatientUnitID = PatientUnitID;
            richToolbar.RichTextBox = richTextEditor;
            OpdIpd = 0;
            IsForOPD = true;
            getDetailsOfSelectedPatient();
            getDescription();
            this.Loaded += new RoutedEventHandler(frmPatientConsentDetails_Loaded);
        }

        void frmPatientConsentDetails_Loaded(object sender, RoutedEventArgs e)
        {
            dtpDate.SelectedDate = DateTime.Now;
            FillConsentCombo();
            ConsentText = string.Empty;
            this.richToolbar.RichTextBox = richTextEditor;
        }

        #endregion

        #region Button Click
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextEditor.Text.Trim()))
            {
                string msgTitle = "Palash";
                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SaveVerification_Msg");
                //}
                //else
                //{
                    msgText = "Are you sure you want to save ?";
                //}
                MessageBoxChildWindow msgW = null;
                msgW = new MessageBoxChildWindow(msgTitle, msgText, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
            else
            {
                //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                //{
                //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ConsentValidation_Msg");
                //}
                //else
                //{
                    msgText = "Please enter consent";
                //}
                ShowMessageBox(msgText, MessageBoxButtons.Ok, MessageBoxIcon.Information);
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow cw = (ChildWindow)this.Parent;
            cw.DialogResult = false;
            //cw.Close();
        }

        private void AddConsentTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string template = ((MasterListItem)CmbOtTheatre.SelectedItem).TemplateName;
                if (!string.IsNullOrEmpty(template) && objListDesc != null)
                {
                    foreach (clsPatientFieldsConfigVO obj in objListDesc)
                    {
                        if (template.Contains(obj.FieldName))
                        {
                            string replaceDesc = getDataForSelectedPatient(obj.ID.ToString(), obj.FieldName);
                            template = template.Replace(("{" + obj.FieldName + "}"), replaceDesc);
                        }
                    }
                    richTextEditor.Html = template;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Fill Combo

        private void FillConsentCombo()
        {
            try
            {
                clsGetConsentByConsentTypeBizActionVO BizAction = new clsGetConsentByConsentTypeBizActionVO();
                BizAction.ConsentList = new List<clsConsentDetailsVO>();
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations != null)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.OTConsentTypeID > 0)
                    {
                        BizAction.ConsentTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OTConsentTypeID;
                    }
                }
                else
                {
                    BizAction.ConsentTypeID = 0;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-Select-"));

                        foreach (var item in ((clsGetConsentByConsentTypeBizActionVO)e.Result).ConsentList)
                        {
                            objList.Add(new MasterListItem(item.ID, item.Description, item.TemplateName));
                        }

                        CmbOtTheatre.ItemsSource = null;
                        CmbOtTheatre.ItemsSource = objList;
                        CmbOtTheatre.SelectedItem = objList[0];

                        DataList = objList;
                    }

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillComboWithConsent()
        {
            try
            {
                clsGetMasterListConsentBizActionVO BizAction = new clsGetMasterListConsentBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ConsentMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-Select-"));
                        objList.AddRange(((clsGetMasterListConsentBizActionVO)e.Result).MasterList);
                        CmbOtTheatre.ItemsSource = null;
                        CmbOtTheatre.ItemsSource = objList;
                        CmbOtTheatre.SelectedItem = objList[0];

                        DataList = objList;
                    }

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Private Method

        private void getDetailsOfSelectedPatient()
        {
            try
            {
                clsGetConsentDetailsBizActionVO BizAction = new clsGetConsentDetailsBizActionVO();
                BizAction.ConsentDetails = new clsConsentDetailsVO();
                BizAction.ConsentDetails.PatientID = PatientID;
                BizAction.ConsentDetails.PatientUnitID = PatientUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetConsentDetailsBizActionVO result = arg.Result as clsGetConsentDetailsBizActionVO;
                        selectedPatient = result.ConsentDetails;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch
            {
                throw;
            }
        }

        private void getDescription()
        {
            try
            {
                clsGetPatientConfigFieldsBizActionVO bizActionVO = new clsGetPatientConfigFieldsBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        objListDesc = new List<clsPatientFieldsConfigVO>();
                        objListDesc.AddRange(((clsGetPatientConfigFieldsBizActionVO)args.Result).OtPateintConfigFieldsMatserDetails);
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            { }
        }

        private string getDataForSelectedPatient(string SelectedID, string FiledName)
        {
            switch (SelectedID)
            {
                case "1": return ("{" + FiledName + "} : " + selectedPatient.MRNo);
                case "2": return ("{" + FiledName + "} : " + selectedPatient.PatientName);
                case "3": return ("{" + FiledName + "} : " + selectedPatient.PatientAddress);
                case "4": return ("{" + FiledName + "} : " + Convert.ToString(selectedPatient.PatientContachNo == 0 ? "NA" : selectedPatient.PatientContachNo.ToString()));
                case "5": return ("{" + FiledName + "} : " + selectedPatient.KinName);
                case "6": return ("{" + FiledName + "} : " + selectedPatient.KinAddress);
                case "7": return ("{" + FiledName + "} : " + Convert.ToString(selectedPatient.KinMobileNo == 0 ? "NA" : selectedPatient.KinMobileNo.ToString()));

                default: return "";
            }
        }

        private void Save()
        {
            try
            {
                clsSaveConsentDetailsBizActionVO BizAction = new clsSaveConsentDetailsBizActionVO();
                BizAction.ConsentDetails = new clsConsentDetailsVO();
                BizAction.ConsentDetails = GetDataToSave();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsSaveConsentDetailsBizActionVO)arg.Result).ConsentDetails != null)
                        {
                            //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                            //{
                            //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordSaved_Msg");
                            //}
                            //else
                            //{
                                msgText = "Record saved successfully.";
                            //}
                            MessageBoxChildWindow msgW1 =
                                new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgW1.Show();
                            ChildWindow cw = (ChildWindow)this.Parent;
                            cw.DialogResult = true;

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

                        MessageBoxChildWindow msgW1 =
                               new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private clsConsentDetailsVO GetDataToSave()
        {
            clsSaveConsentDetailsBizActionVO BizAction = new clsSaveConsentDetailsBizActionVO();
            BizAction.ConsentDetails = new clsConsentDetailsVO();

            BizAction.ConsentDetails.ScheduleID = ScheduleID;

            BizAction.ConsentDetails.PatientID = PatientID;

            BizAction.ConsentDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            BizAction.ConsentDetails.Date = DateTime.Now;

            BizAction.ConsentDetails.VisitAdmID = VisitAdmID;

            BizAction.ConsentDetails.VisitAdmUnitID = VisitAdmUnitID;

            BizAction.ConsentDetails.Opd_Ipd = OpdIpd;

            BizAction.ConsentDetails.ConsentID = ((MasterListItem)CmbOtTheatre.SelectedItem).ID;

            BizAction.ConsentDetails.Consent = richTextEditor.Html;

            return BizAction.ConsentDetails;
        }


        #endregion
    }
}
