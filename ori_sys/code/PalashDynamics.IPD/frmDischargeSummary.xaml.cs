using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;

using PalashDynamics.Collections;
using System.ComponentModel;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Administration.DischargeTemplateMaster;
using Liquid;
using System.Text;
using C1.Silverlight.Pdf;
using C1.Silverlight.RichTextBox;
using System.Windows.Printing;
using C1.Silverlight.RichTextBox.PdfFilter;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using PalashDynamics.Animations;
using C1.Silverlight;
using C1.Silverlight.RichTextBox.Documents;


namespace PalashDynamics.IPD.Forms
{
    public partial class frmDischargeSummary : UserControl
    {
        #region Variable Declaration
        SwivelAnimation objAnimation;
        public long PatientID = 0;
        public long UnitID = 0;
        public long IPDAdmissionID = 0;
        public string IPDAdmissionNo = String.Empty;
        private long PatientUnitID = 0, BedID = 0;
        private string MRNO = null;
        private bool FromBedStatus = false;
        public string PatientName = null;
        bool IsSearchClick = false;
        bool IsViewClick;
        long DoctorID = 0;
        long IPDAdmissionUnitID = 0;
        clsPatientGeneralVO patientDetails = null;
        public List<MasterListItem> objList = new List<MasterListItem>();
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        clsIPDDischargeSummaryVO AdmPatientDetails = null;
        clsIPDDischargeSummaryVO objDischargeModify = null;
        public PagedSortableCollectionView<clsDischargeTemplateMasterVO> MasterList { get; private set; }
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                OnPropertyChanged("PageSize");
            }
        }
        bool IsModify;
        bool IsFromAdmissionList = false;
        clsIPDDischargeSummaryVO PatientEntry = new clsIPDDischargeSummaryVO();
        StringBuilder strPatInfo;
        StringBuilder strDoctorPathInfo;
        WaitIndicator WaitNew = new WaitIndicator();

        #endregion

        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }
        bool IsViewPatientDischarge = false;
        #endregion

        public string ModuleName { get; set; }
        public string Action { get; set; }
        long PrintID = 0;
        bool OptionChecked { get; set; }
        public PagedSortableCollectionView<clsIPDDischargeSummaryVO> DataList { get; private set; }
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Constructor and Loaded
        public frmDischargeSummary()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId != null)
                {
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;
                    UnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                    PatientName = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
                    IPDAdmissionID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                    IPDAdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;        // Added on 21Feb2019 for Package Flow in IPD
                    IPDAdmissionNo = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.IPDNO;
                    DoctorID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.DoctorID;
                    FindPatient(PatientID, UnitID, null);
                }
            }
            this.Loaded += new RoutedEventHandler(frmDischargeSummary_Loaded);
            DataList = new PagedSortableCollectionView<clsIPDDischargeSummaryVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetDischargeSummaryList();

        }
        void frmDischargeSummary_Loaded(object sender, RoutedEventArgs e)
        {
            FillDischargeTemplate();
            SetCommandButtonState("Load");
            cmbDischargeTemplate.IsEnabled = false;
            OptionChecked = false;
            dtpFollowUpDate.SelectedDate = DateTime.Now;
            //grdFieldName.Visibility = Visibility.Collapsed;
            MasterList = new PagedSortableCollectionView<clsDischargeTemplateMasterVO>();
            PageSize = 50;
            MasterList.PageSize = PageSize;
            //grdRichTextBox.Visibility = Visibility.Visible;
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId > 0)
                {
                    IsFromAdmissionList = true;
                    FrontPanel.Visibility = Visibility.Collapsed;
                    BackPanel.Visibility = Visibility.Visible;
                    btnCancle.Visibility = Visibility.Visible;
                    btnCancle.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    grpSearchPlans.Visibility = Visibility.Collapsed;
                    CmddischargePatientListSearch.IsEnabled = false;
                    CmdPatientSearch.IsEnabled = false;
                    txtMRNo.IsEnabled = false;
                    patientDetails = new clsPatientGeneralVO();
                    patientDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;
                    patientDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientUnitID;
                    FindPatient(((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId, ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientUnitID, "");
                    GetDischargeSummary();
                }
            }
            if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html;
            GetDischargeSummaryList();
        }


        #endregion

        #region Patient Search Buttons Click

        private void CmdPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            IsViewClick = false;
            IsSearchClick = false;
            ModuleName = "PalashDynamics.IPD";
            Action = "PalashDynamics.IPD.IPDPatientSearch";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }
        string msgText;

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;


                if (myData is IInitiateCIMS)
                {
                    ((IInitiateCIMS)myData).Initiate(PatientTypes.IPD.ToString());
                }

                ChildWindow cw = new ChildWindow();
                cw = (ChildWindow)myData;
                if (IsViewClick == false)
                {
                    cw.Closed += new EventHandler(cw_Closed);
                }
                cw.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void cw_Closed(object sender, EventArgs e)
        {
            if ((bool)((PalashDynamics.IPD.IPDPatientSearch)sender).DialogResult)
            {
                GetSelectedPatientDetails();
                btnPrint.IsEnabled = true;
            }
            else
            {
                txtMRNo.Text = string.Empty;
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }
        private void GetSelectedPatientDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                PatientName = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                IPDAdmissionID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;
                //Added by bhushan 09/01/2017
                this.IPDAdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.AdmissionUnitID;
                IPDAdmissionNo = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionNo;
                DoctorID = ((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID;
                FindPatient(PatientID, UnitID, null);
            }
        }
        private void FindPatient(long PatientID, long PatientUnitId, string MRNO)
        {
            clsGetIPDPatientBizActionVO BizAction = new clsGetIPDPatientBizActionVO();
            BizAction.PatientDetails = new clsGetIPDPatientVO();
            if (IsSearchClick == true)
            {
                BizAction.PatientDetails.GeneralDetails.PatientID = Convert.ToInt64(null);
                BizAction.PatientDetails.GeneralDetails.UnitId = Convert.ToInt64(null);
                BizAction.PatientDetails.GeneralDetails.MRNo = txtMRNo.Text;
            }
            else
            {
                BizAction.PatientDetails.GeneralDetails.PatientID = PatientID;
                BizAction.PatientDetails.GeneralDetails.UnitId = PatientUnitId;
                BizAction.PatientDetails.GeneralDetails.MRNo = MRNO;
            }
            BizAction.PatientDetails.GeneralDetails.IsIPD = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        patientDetails = new clsPatientGeneralVO();
                        patientDetails = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;

                        if (!((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                        {
                            GetDischargeSummary();
                            if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo != null)
                            {
                                txtMRNo.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                                btnSave.IsEnabled = true;
                            }
                            if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDPatientName != null)
                            {
                                lblPatientName.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDPatientName;
                            }
                        }
                        else
                        {
                            msgText = "Please check MR number.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            txtMRNo.Focus();
                            msgW1.Show();
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
        private void CmddischargePatientListSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtMRNo.Text.Length != 0)
            {
                ClearControl();
                IsViewClick = false;
                IsSearchClick = true;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text);
            }
            else
            {
                ClearControl();
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNo.Focus();
                msgW1.Show();
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }

        private void GetIPDDischargeList(long PatientID, long PatientUnitID)
        {

        }
        #endregion

        #region Private Methods
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    btnSave.IsEnabled = false;
                    btnCancle.IsEnabled = false;
                    grpSearchPlans.Visibility = Visibility.Visible;
                    cmdNew.IsEnabled = true;
                    break;
                case "New":
                    btnSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    grpSearchPlans.Visibility = Visibility.Collapsed;
                    btnCancle.IsEnabled = true;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    btnSave.IsEnabled = false;
                    btnCancle.IsEnabled = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    btnSave.IsEnabled = false;
                    btnCancle.IsEnabled = true;
                    break;
                case "View":
                    btnSave.IsEnabled = true;
                    grpSearchPlans.Visibility = Visibility.Collapsed;
                    cmdNew.IsEnabled = false;
                    btnCancle.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        private void ClearControl()
        {
            lblPatientName.Text = String.Empty;
        }

        private void FillDischargeTemplate()
        {
            clsGetDischargeTemplateMasterListBizActionVO BizAction = new clsGetDischargeTemplateMasterListBizActionVO();
            BizAction.DischargeTemplateMasterList = new List<clsDischargeTemplateMasterVO>();
            BizAction.SearchExpression = string.Empty;
            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = 200;
            BizAction.StartRowIndex = 0;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.DischargeTemplateMasterList = (((clsGetDischargeTemplateMasterListBizActionVO)args.Result).DischargeTemplateMasterList);
                        objDischargeTemplateList = new List<clsDischargeTemplateMasterVO>();
                        objList.Add(new MasterListItem(0, "--Select--"));

                        foreach (clsDischargeTemplateMasterVO item in BizAction.DischargeTemplateMasterList)
                        {
                            objList.Add(new MasterListItem(item.ID, item.Description));
                        }
                        objDischargeTemplateList = BizAction.DischargeTemplateMasterList;
                        cmbDischargeTemplate.ItemsSource = null;
                        cmbDischargeTemplate.ItemsSource = objList;
                        cmbDischargeTemplate.SelectedValue = objList[0].ID;
                    }
                };
                client.ProcessAsync(BizAction, User);//new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveDischargeSummary()
        {
            clsAddIPDDischargeSummaryBizActionVO bizActionVO = new clsAddIPDDischargeSummaryBizActionVO();
            bizActionVO.DischargeSummary = new clsIPDDischargeSummaryVO();
            bizActionVO.DischargeSummaryList = new List<clsIPDDischargeSummaryVO>();
            bizActionVO.DischargeSummary.DischargeSummaryDetails = new clsDischargeSummaryDetailsVO();
            bizActionVO.DischargeSummary.DischargeSummaryDetailsList = new List<clsDischargeSummaryDetailsVO>();
            bizActionVO.DischargeSummary.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            bizActionVO.IsCheckBox = false;
            if (IsModify == false)
            {
                bizActionVO.IsModify = false;
                //if (AdmPatientDetails != null)
                if (patientDetails != null)
                {
                    //Added By Bhushan 09/01/2017
                    bizActionVO.DischargeSummary.AdmID = this.IPDAdmissionID;//patientDetails.IPDAdmissionID;
                    bizActionVO.DischargeSummary.AdmUnitID = this.IPDAdmissionUnitID;//patientDetails.PatientUnitID;
                    bizActionVO.DischargeSummary.PatientID = patientDetails.PatientID;

                    if (cmbDischargeTemplate.ItemsSource != null)
                    {
                        if (cmbDischargeTemplate.SelectedItem != null && !((MasterListItem)cmbDischargeTemplate.SelectedItem).ID.Equals(0))
                        {
                            bizActionVO.DischargeSummary.DischargeTemplateID = objDischargeTemplateList.SingleOrDefault(S => S.ID.Equals(((MasterListItem)cmbDischargeTemplate.SelectedItem).ID)).ID;
                            bizActionVO.DischargeSummary.DischargeTemplateUnitID = objDischargeTemplateList.SingleOrDefault(S => S.ID.Equals(((MasterListItem)cmbDischargeTemplate.SelectedItem).ID)).UnitID;
                        }
                    }

                    bizActionVO.DischargeSummary.PatientUnitID = patientDetails.PatientUnitID;
                    bizActionVO.DischargeSummary.FollowUpDate = dtpFollowUpDate.SelectedDate.Value;
                    bizActionVO.DischargeSummary.DoctorID = patientDetails.DoctorID;
                    bizActionVO.DischargeSummary.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    bizActionVO.DischargeSummary.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    bizActionVO.DischargeSummary.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    bizActionVO.DischargeSummary.AddedDateTime = System.DateTime.Now;
                    bizActionVO.DischargeSummary.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    if (objDischargeSummary.IsTextTemplate == true)
                    {
                        bizActionVO.DischargeSummary.TextDocument = richTextBox.Html;
                    }
                    else if (objDischargeSummary.IsTextTemplate == false)
                    {
                        foreach (clsDischargeSummaryDetailsVO item in DischargeSummaryDetailsList)
                        {
                            item.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                            bizActionVO.DischargeSummary.DischargeSummaryDetailsList.Add(item);
                        }
                    }
                }
            }
            else if (IsModify == true)
            {
                bizActionVO.IsModify = true;
                if (objDischargeModify != null)
                {
                    bizActionVO.DischargeSummary.ID = objDischargeModify.ID;// ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).ID; //
                    bizActionVO.DischargeSummary.AdmID = objDischargeModify.AdmID;// ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).AdmID; //
                    bizActionVO.DischargeSummary.AdmUnitID = objDischargeModify.AdmUnitID;// ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).AdmUnitID; //
                    bizActionVO.DischargeSummary.PatientID = objDischargeModify.PatientID;// ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).PatientID; //
                    bizActionVO.DischargeSummary.PatientUnitID = objDischargeModify.PatientUnitID;// ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).PatientUnitID; //
                    bizActionVO.DischargeSummary.DischargeTemplateID = objDischargeModify.DischargeTemplateID;// ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).DischargeTemplateID; //
                    bizActionVO.DischargeSummary.DischargeTemplateUnitID = objDischargeModify.DischargeTemplateUnitID;// ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).DischargeTemplateUnitID; //
                    bizActionVO.DischargeSummary.FollowUpDate = dtpFollowUpDate.SelectedDate.Value;// ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).FollowUpDate; //
                    bizActionVO.DischargeSummary.DoctorID = objDischargeModify.DoctorID;// ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).DoctorID; //

                    bizActionVO.DischargeSummary.UpdatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    bizActionVO.DischargeSummary.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    bizActionVO.DischargeSummary.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    bizActionVO.DischargeSummary.UpdatedDateTime = System.DateTime.Now;
                    bizActionVO.DischargeSummary.UpdatedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    if (!string.IsNullOrEmpty(objDischargeModify.TextDocument))
                    {
                        bizActionVO.DischargeSummary.TextDocument = richTextBox.Html;
                    }
                    else
                    {
                        foreach (clsDischargeSummaryDetailsVO item in DischargeSummaryDetailsList)
                        {
                            item.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                            string _itemstring = item.FieldDesc;
                            bizActionVO.DischargeSummary.DischargeSummaryDetailsList.Add(item);
                        }
                    }
                }
            }
            bizActionVO.DischargeSummary.IsCancel = (bool)chkIsCancel.IsChecked;
            bizActionVO.DischargeSummary.Date = DateTime.Now;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (IsModify == false)
                    {
                        msgText = "Discharge Summary Added Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                WaitNew.Show();
                                GetPatientsInfoInHTML(((clsAddIPDDischargeSummaryBizActionVO)arg.Result).DischargeSummary.DischargeSummaryID, this.UnitID, patientDetails.IPDAdmissionID, patientDetails.PatientUnitID);
                                cmbDischargeTemplate.SelectedValue = ((List<MasterListItem>)cmbDischargeTemplate.ItemsSource)[0].ID;
                                cmbDischargeTemplate.IsEnabled = false;
                                txtMRNo.Text = string.Empty;
                                objAnimation.Invoke(RotationType.Backward);
                                GetDischargeSummaryList();
                                ClearControls();
                                SetCommandButtonState("Load");
                            }
                        };
                        msgW1.Show();
                    }
                    else if (IsModify == true)
                    {
                        msgText = "Discharge Summary Modified Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                WaitNew.Show();
                                GetPatientsInfoInHTML(this.PrintID, this.UnitID, this.IPDAdmissionID, this.IPDAdmissionUnitID);
                                cmbDischargeTemplate.IsEnabled = false;
                                txtMRNo.Text = string.Empty;
                                objAnimation.Invoke(RotationType.Backward);
                                GetDischargeSummaryList();
                                ClearControls();
                                SetCommandButtonState("Load");
                            }
                        };
                        msgW1.Show();
                    }



                    //patientDetails = new clsPatientGeneralVO();
                    //IPDAdmissionUnitID = 0; IPDAdmissionID = 0;
                }
                else
                {
                    msgText = "Error occurred while adding Discharge Summary.";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetDischargeSummary()
        {
            clsGetIPDDischargeSummaryBizActionVO BizAction = new clsGetIPDDischargeSummaryBizActionVO();

            BizAction.DischargeSummaryList = new List<clsIPDDischargeSummaryVO>();
            BizAction.sortExpression = string.Empty;
            BizAction.PatientID = patientDetails.PatientID;
            BizAction.PatientUnitID = patientDetails.PatientUnitID;
            BizAction.AdmID = this.IPDAdmissionID;
            BizAction.AdmUnitID = this.IPDAdmissionUnitID;

            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {

                        if (BizAction.DischargeSummaryList != null)
                        {
                            BizAction = (((clsGetIPDDischargeSummaryBizActionVO)args.Result));

                            if (BizAction.DischargeSummaryList.Count > 0)
                            {
                                if (IsFromAdmissionList == true)
                                    cmbDischargeTemplate.IsEnabled = false;
                                else
                                    cmbDischargeTemplate.IsEnabled = true;

                                foreach (var item in BizAction.DischargeSummaryList)
                                {
                                    this.PrintID = item.ID;
                                    this.UnitID = item.UnitID;
                                    this.IPDAdmissionID = item.AdmID;
                                    this.IPDAdmissionUnitID = item.AdmUnitID;

                                }

                                objDischargeModify = new clsIPDDischargeSummaryVO();
                                objDischargeModify = BizAction.DischargeSummaryDetails;
                                clsIPDDischargeSummaryVO objDischargeSummary = new clsIPDDischargeSummaryVO();
                                if (objDischargeModify.IsCancel == false)
                                {
                                    IsModify = true;
                                    //chkIsCancel.IsEnabled = true;
                                    cmbDischargeTemplate.IsEnabled = false;
                                    if (!string.IsNullOrEmpty(objDischargeModify.TextDocument))
                                    {
                                        objDischargeSummary.TextDocument = objDischargeModify.TextDocument;
                                        //grdFieldName.Visibility = Visibility.Collapsed;
                                        //grdRichTextBox.Visibility = Visibility.Visible;
                                        cmbDischargeTemplate.SelectedValue = objDischargeModify.DischargeTemplateID;
                                        richTextBox.Html = objDischargeSummary.TextDocument;
                                    }
                                    else
                                    {

                                        DischargeSummaryDetailsList = new List<clsDischargeSummaryDetailsVO>();
                                        int count = 0;
                                        for (int index = 0; index < objDischargeModify.DischargeSummaryDetailsList.Count; index++)
                                        {
                                            objDischargeModify.DischargeSummaryDetailsList[index].RowID = index;
                                        }

                                        DischargeSummaryDetailsList = objDischargeModify.DischargeSummaryDetailsList;

                                        foreach (var item in DischargeSummaryDetailsList)
                                        {
                                            if (string.IsNullOrEmpty(item.ApplicableFont))
                                            {
                                                item.IsRichTextBox = "Collapsed";
                                                item.IsTextBox = "Collapsed";
                                                item.IsDate = "Collapsed";
                                                item.IsTime = "Collapsed";
                                                item.IsTextBox = "Collapsed";
                                                item.IsOption = "Collapsed";
                                                item.IsCheckBox = "Collapsed";

                                                if (item.BindingControl == 1)
                                                    item.IsDate = "Visible";

                                                else if (item.BindingControl == 2)
                                                    item.IsTime = "Visible";

                                                else if (item.BindingControl == 3)
                                                    item.IsTextBox = "Visible";

                                                else if (item.BindingControl == 4)
                                                    item.IsCheckBox = "Visible";

                                                else if (item.BindingControl == 5)
                                                    item.IsOption = "Visible";
                                            }

                                            else
                                            {
                                                item.IsDate = "Collapsed";
                                                item.IsTime = "Collapsed";
                                                item.IsTextBox = "Collapsed";
                                                item.IsOption = "Collapsed";
                                                item.IsCheckBox = "Collapsed";
                                                item.IsRichTextBox = "Visible";
                                            }
                                            cmbDischargeTemplate.SelectedItem = ((List<MasterListItem>)cmbDischargeTemplate.ItemsSource).Where(z => z.ID == item.DischargeTempDetailID).FirstOrDefault();
                                        }
                                        //grdFieldName.Visibility = Visibility.Visible;
                                        //grdRichTextBox.Visibility = Visibility.Visible;

                                        //dgDischargeDetails.ItemsSource = null;
                                        //dgDischargeDetails.ItemsSource = DischargeSummaryDetailsList;
                                    }

                                    if (BizAction.AdmPatientDetails != null)
                                    {
                                        AdmPatientDetails = new clsIPDDischargeSummaryVO();
                                        AdmPatientDetails = BizAction.AdmPatientDetails;
                                    }
                                }
                                else if (objDischargeModify.IsCancel == true)
                                {
                                    //chkIsCancel.IsEnabled = false;
                                    cmbDischargeTemplate.IsEnabled = false;
                                    IsModify = false;
                                    if (BizAction.AdmPatientDetails != null)
                                    {
                                        AdmPatientDetails = new clsIPDDischargeSummaryVO();
                                        AdmPatientDetails = BizAction.AdmPatientDetails;
                                    }
                                    //FillDischargeTemplate();
                                    txtMRNo.Focus();
                                }
                            }
                            else
                            {
                                cmbDischargeTemplate.IsEnabled = true;
                                //chkIsCancel.IsEnabled = false;
                                IsModify = false;
                                if (BizAction.AdmPatientDetails != null)
                                {
                                    AdmPatientDetails = new clsIPDDischargeSummaryVO();
                                    AdmPatientDetails = BizAction.AdmPatientDetails;
                                }
                                //FillDischargeTemplate();
                                txtMRNo.Focus();
                            }
                        }
                    }
                };
                client.ProcessAsync(BizAction, User);//new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void GetPatientsInfoInHTML(long PrintID, long UnitID, long AdmissionID, long AdmissionUnitID)
        {
            clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO BizAction = new clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO();

            BizAction.UnitID = UnitID;
            BizAction.AdmID = AdmissionID;
            BizAction.AdmUnitID = AdmissionUnitID;
            BizAction.PrintID = PrintID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO)arg.Result).DischargeSummaryDetails != null)
                        {
                            PatientEntry = new clsIPDDischargeSummaryVO();
                            PatientEntry = ((clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO)arg.Result).DischargeSummaryDetails;

                            strPatInfo = new StringBuilder();
                            strPatInfo.Append(PatientEntry.PatientInfoHTML);
                            strPatInfo = strPatInfo.Replace("[Education]",  "  " + PatientEntry.Education.ToString());
                            strPatInfo = strPatInfo.Replace("[DoctorName]", " : " + PatientEntry.DoctorName.ToString());
                            strPatInfo = strPatInfo.Replace("[PatDetails]", "    : " + PatientEntry.DisDestination.ToString());
                            strPatInfo = strPatInfo.Replace("[AgeSex]", "    : " + PatientEntry.AgeWithSex.ToString());
                            strPatInfo = strPatInfo.Replace("[Add]", "   : " + PatientEntry.Address.ToString());
                            strPatInfo = strPatInfo.Replace("[DateAdmission]", "    : " + PatientEntry.AdmDate);
                            if (PatientEntry.DischargeDate == DateTime.MinValue)
                            {
                                strPatInfo = strPatInfo.Replace("[DateDischarge]", "    : " + "Not Yet Discharged");
                            }
                            else
                            {
                                strPatInfo = strPatInfo.Replace("[DateDischarge]", "    : " + PatientEntry.DischargeDate);
                            }
                            strPatInfo = strPatInfo.Replace("[ClinicName]", "    " + PatientEntry.UnitName.Trim());
                            strPatInfo = strPatInfo.Replace("[AddressLine1]", "    " + PatientEntry.AdressLine1.Trim());
                            strPatInfo = strPatInfo.Replace("[AddressLine2]", "    " + PatientEntry.AdressLine1.Trim());
                            strPatInfo = strPatInfo.Replace("[EmailId]", "     " + PatientEntry.Email.Trim());
                            strPatInfo = strPatInfo.Replace("[TinNo]", "    : " + PatientEntry.TinNo.Trim());
                            strPatInfo = strPatInfo.Replace("[RegNo]", "    : " + PatientEntry.RegNo.Trim());
                            strPatInfo = strPatInfo.Replace("[MobNo]", "    " + PatientEntry.ContactNo.Trim());
                            strPatInfo = strPatInfo.Replace("[MobileNo]", "    : " + PatientEntry.MobileNo.Trim());
                            strPatInfo = strPatInfo.Replace("[TreatingDr]", "    : Dr." + PatientEntry.TreatingDr.Trim());
                            strPatInfo = strPatInfo.Replace("[DischargeDr]", "    : Dr." + PatientEntry.DoctorName.Trim());
                            strPatInfo = strPatInfo.Replace("[DischargeDrEducation]", "    : Dr." + PatientEntry.Education.Trim());
                            strPatInfo = strPatInfo.Replace("[MRNo]", "    : " + PatientEntry.MRNo.Trim());
                            strPatInfo = strPatInfo.Replace("[IPDNo]", "    : " + PatientEntry.IPDNO.Trim());

                            strDoctorPathInfo = new StringBuilder();
                            strDoctorPathInfo.Append(PatientEntry.PatientFooterInfoHTML);
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[DrDischarge]", "    " + PatientEntry.DoctorName.Trim());
                            strDoctorPathInfo = strDoctorPathInfo.Replace("[DrEducation]", "    " + PatientEntry.Education.Trim());                            
                        }
                        richTextBox.Html = PatientEntry.TextDocument;
                        richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
                        richTextBox.Html = richTextBox.Html.Replace("[%FOOTERINFO%]", strDoctorPathInfo.ToString());
                        PrintReport(PrintID, PatientEntry);
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }


        public PaperKind PaperKind { get; set; }
        public Thickness PrintMargin { get; set; }
        private void PrintReport(long PrintID, clsIPDDischargeSummaryVO PatientResultEntry)
        {
            try
            {
                richTextBox.Document.Margin = new Thickness(5, 5, 5, 5);
                PrintMargin = new Thickness(5, 0, 5, 5);

                //Printing
                var viewManager = new C1RichTextViewManager
                {
                    Document = richTextBox.Document,
                    PresenterInfo = richTextBox.ViewManager.PresenterInfo
                };
                var print = new PrintDocument();
                int presenter = 0;

                print.PrintPage += (s, printArgs) =>
                {
                    var element = (FrameworkElement)HeaderTemplate.LoadContent();
                    Grid grd = (Grid)element.FindName("PatientDetails");

                    if (grd != null)
                    {
                        grd.Visibility = Visibility.Visible;
                        grd.DataContext = PatientResultEntry;

                    }
                    element.DataContext = viewManager.Presenters[presenter];
                    printArgs.PageVisual = element;
                    printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;
                };

                var pdf = new C1PdfDocument(PaperKind.A4);

                PdfFilter.PrintDocument(richTextBox.Document, pdf, PrintMargin);
                pdf.CurrentPage = pdf.Pages.Count - 1;
                String appPath;

                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                appPath = "DischargeCard" + UnitID + "_" + PrintID + ".pdf";

                Stream FileStream = new MemoryStream();
                MemoryStream MemStrem = new MemoryStream();

                pdf.Save(MemStrem);
                FileStream.CopyTo(MemStrem);

                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                client.UploadReportFileForDischargeCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        WaitNew.Close();
                        ViewPDFReport(appPath);
                    }
                };
                client.UploadReportFileForDischargeAsync(appPath, MemStrem.ToArray());
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                WaitNew.Close();
            }
        }

        private void ViewPDFReport(string FileName)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../PatientDischargeTempReportDocuments");
            string fileName1 = address.ToString();
            fileName1 = fileName1 + "/" + FileName;
           // HtmlPage.Window.Invoke("Open", fileName1);
            HtmlPage.Window.Invoke("open", new object[] { fileName1, "", "" });
        }

        private void ClearControls()
        {
            txtMRNo.Text = string.Empty;
            lblPatientName.Text = string.Empty;
            cmbDischargeTemplate.SelectedValue = (long)0;
            richTextBox.Html = string.Empty;
            txtFrontMRNo.Text = string.Empty;
            dtpFromDate.SelectedDate = null;
            dtpToDate.SelectedDate = null;
        }



        private void GetDischargeSummaryList()
        {
            try
            {
                clsFillDataGridDischargeSummaryListBizActionVO BizAction = new clsFillDataGridDischargeSummaryListBizActionVO();
                if (dtpFromDate.SelectedDate != null)
                    BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
                if (dtpToDate.SelectedDate != null)
                    BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
                if (!string.IsNullOrEmpty(txtFrontMRNo.Text))
                    BizAction.MrNo = txtFrontMRNo.Text;

                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsFillDataGridDischargeSummaryListBizActionVO)arg.Result).DischargeSummaryList != null)
                        {
                            clsFillDataGridDischargeSummaryListBizActionVO result = arg.Result as clsFillDataGridDischargeSummaryListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;

                            if (result.DischargeSummaryList != null)
                            {
                                DataList.Clear();
                                foreach (var item in result.DischargeSummaryList)
                                {
                                    DataList.Add(item);
                                }
                                dgDischargeSummaryList.ItemsSource = null;
                                dgDischargeSummaryList.ItemsSource = DataList;
                                dataPager.Source = null;
                                dataPager.PageSize = BizAction.MaximumRows;
                                dataPager.Source = DataList;
                            }
                        }
                        else
                        {
                            dgDischargeSummaryList.ItemsSource = null;
                            dgDischargeSummaryList.UpdateLayout();
                        }
                    }
                    else
                    {
                        ShowMessageBox("Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }

                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception Ex)
            {
                throw;
            }
        }



        #endregion

        List<clsDischargeSummaryDetailsVO> DischargeSummaryDetailsList = new List<clsDischargeSummaryDetailsVO>();
        clsIPDDischargeSummaryVO objDischargeSummary;
        List<clsDischargeTemplateMasterVO> objDischargeTemplateList = null;
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var oldItem = e.RemovedItems.OfType<C1TabItem>().FirstOrDefault();
                if (oldItem == null) return; // richTextBoxTab and the others are null the first time around because InitializeComponent is running.

                if (oldItem == richTextBoxTab)
                {
                    htmlBox.Text = richTextBox.Html;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if (oldItem == htmlTab)
                {
                    richTextBox.Html = htmlBox.Text;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if (oldItem == rtfTab)
                {
                    richTextBox.Document = new RtfFilter().ConvertToDocument(rtfBox.Text);
                    htmlBox.Text = richTextBox.Html;
                }
            }
            catch { }
        }

        private void cmbDischargeTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDischargeTemplate.SelectedItem != null)
            {
                long ID = ((MasterListItem)cmbDischargeTemplate.SelectedItem).ID;
                if (!ID.Equals(0))
                {
                    var objDischarge = objDischargeTemplateList.SingleOrDefault(S => S.ID.Equals(ID));

                    if (objDischarge != null)
                    {
                        objDischargeSummary = new clsIPDDischargeSummaryVO();
                        if (objDischarge.IsTextTemplate == true)
                        {
                            objDischargeSummary.IsTextTemplate = true;
                            objDischargeSummary.TextDocument = objDischarge.DischargeTemplateDetails.TextData;
                            //grdFieldName.Visibility = Visibility.Collapsed;
                            //grdRichTextBox.Visibility = Visibility.Visible;
                            richTextBox.Html = objDischargeSummary.TextDocument;
                            richTextBox.IsEnabled = true;
                            cmbDischargeTemplate.IsEnabled = true;

                            string rtbstring = string.Empty;
                            if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                            {
                                rtbstring = richTextBox.Html;
                                rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "[%PATIENTINFO%]");
                                rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "");
                                //rtbstring = rtbstring.Insert(rtbstring.IndexOf("</body>"), "[%DOCTORINFO%]");
                                richTextBox.Html = rtbstring;
                            }

                        }
                        else
                        {
                            #region For Fields
                            //objDischargeSummary.IsTextTemplate = false;
                            //int Count = 0;
                            //DischargeSummaryDetailsList = new List<clsDischargeSummaryDetailsVO>();

                            //foreach (var item in objDischarge.DischargeTemplateDetailsList)
                            //{
                            //    clsDischargeSummaryDetailsVO objDetails = new clsDischargeSummaryDetailsVO();
                            //    objDetails.DischargeTempDetailID = item.ID;
                            //    objDetails.DischargeTempDetailUnitID = item.UnitID;
                            //    objDetails.FieldName = item.FieldName;
                            //    objDetails.DisChargeSummaryID = item.DisChargeTemplateID;
                            //    objDetails.ApplicableFont = item.ApplicableFont;
                            //    if (string.IsNullOrEmpty(objDetails.ApplicableFont))
                            //    {
                            //        objDetails.IsRichTextBox = "Collapsed";
                            //        objDetails.IsTextBox = "Collapsed";
                            //        objDetails.IsDate = "Collapsed";
                            //        objDetails.IsTime = "Collapsed";
                            //        objDetails.IsTextBox = "Collapsed";
                            //        objDetails.IsOption = "Collapsed";
                            //        objDetails.IsCheckBox = "Collapsed";

                            //        if (item.BindingControl == 1)
                            //            objDetails.IsDate = "Visible";

                            //        else if (item.BindingControl == 2)
                            //            objDetails.IsTime = "Visible";

                            //        else if (item.BindingControl == 3)
                            //            objDetails.IsTextBox = "Visible";

                            //        else if (item.BindingControl == 4)
                            //            objDetails.IsCheckBox = "Visible";

                            //        else if (item.BindingControl == 5)
                            //            objDetails.IsOption = "Visible";

                            //    }
                            //    else
                            //    {
                            //        objDetails.IsDate = "Collapsed";
                            //        objDetails.IsTime = "Collapsed";
                            //        objDetails.IsTextBox = "Collapsed";
                            //        objDetails.IsOption = "Collapsed";
                            //        objDetails.IsCheckBox = "Collapsed";
                            //        objDetails.IsRichTextBox = "Visible";
                            //    }
                            //    objDetails.RowID = Count;
                            //    Count++;
                            //    if (item.ParameterID == Convert.ToInt64(DischargeParameter.Diagnosis))
                            //    {
                            //        objDetails.ParameterName = item.ParameterName;
                            //    }
                            //    else if (item.ParameterID == Convert.ToInt64(DischargeParameter.Medication))
                            //    {
                            //        objDetails.ParameterName = item.ParameterName;
                            //    }
                            //    else if (item.ParameterID == Convert.ToInt64(DischargeParameter.Investigation))
                            //    {
                            //        objDetails.ParameterName = item.ParameterName;
                            //    }
                            //    else if (item.ParameterID == Convert.ToInt64(DischargeParameter.Clinical_Findings))
                            //    {
                            //        objDetails.ParameterName = item.ParameterName;
                            //    }
                            //    objDetails.ParameterID = item.ParameterID;
                            //    objDetails.BindingControl = item.BindingControl;
                            //    objDetails.DisChargeSummaryUnitID = item.DisChargeTemplateUnitID;
                            //    objDetails.FieldDesc = string.Empty;
                            //    objDetails.IsTextBoxEnable = item.IsTextBoxEnable;
                            //    DischargeSummaryDetailsList.Add(objDetails);
                            //}
                            ////grdFieldName.Visibility = Visibility.Visible;
                            //grdRichTextBox.Visibility = Visibility.Collapsed;
                            //dgDischargeDetails.ItemsSource = null;
                            //dgDischargeDetails.ItemsSource = DischargeSummaryDetailsList;
                            //dgDischargeDetails.UpdateLayout();
                            #endregion
                        }
                    }
                }
                else
                {
                    richTextBox.Html = string.Empty;
                    if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                        richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html;

                }
            }
            else
            {
                //grdFieldName.Visibility = Visibility.Visible;
                //grdRichTextBox.Visibility = Visibility.Visible;
                //dgDischargeDetails.ItemsSource = null;
            }
        }

        //private void dgDischargeDetails_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    DataGridRow row = e.Row;
        //    foreach (DataGridColumn col in dgDischargeDetails.Columns)
        //    {
        //        if (col.Header != null)
        //        {
        //            if (col.Header.Equals("Field Description"))
        //            {
        //                FrameworkElement cellContent = col.GetCellContent(row);
        //                Grid type = (Grid)cellContent;

        //                foreach (var item in type.Children)
        //                {
        //                    if (item.GetType().ToString() == "Liquid.RichTextBox" && item.Visibility == Visibility.Visible)
        //                    {
        //                        Liquid.RichTextBox RichTextBox = item as Liquid.RichTextBox;
        //                        if (RichTextBox != null)
        //                            RichTextBox.Loaded += new RoutedEventHandler(RichTextBox_Loaded);
        //                    }
        //                }

        //            }
        //        }
        //    }
        //}

        void RichTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            double FontSize = 20;
            Liquid.RichTextBox RichTextBox = sender as Liquid.RichTextBox;
            string FontName = DischargeSummaryDetailsList[Convert.ToInt32(RichTextBox.Tag)].ApplicableFont;
            RichTextBox.ApplyFormatting(Formatting.FontSize, FontSize);
            if (!string.IsNullOrEmpty(FontName))
                RichTextBox.ApplyFormatting(Formatting.FontFamily, FontName);
            RichTextBox.ReturnFocus();

        }


        #region Button Click Events
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMRNo.Text))
            {
                if (patientDetails != null)
                {
                    if (patientDetails.MRNo == txtMRNo.Text.Trim())
                    {
                        if (IsModify == true)
                        {
                            string msgTitle = "";
                            string msgText = "Are you sure you want to save the Discharge Summary details?";
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    SaveDischargeSummary();
                                }
                            };
                            msgW.Show();
                        }
                        else if (IsModify == false)
                        {
                            if (cmbDischargeTemplate.SelectedItem != null && ((MasterListItem)cmbDischargeTemplate.SelectedItem).ID > 0)
                            {
                                string msgTitle = "";
                                string msgText = "Are you sure you want to save the Discharge Summary details?";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        SaveDischargeSummary();
                                    }
                                };
                                msgW.Show();
                            }
                            else
                            {
                                msgText = "Please select discharge template";
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }
                        }
                    }
                    else
                    {
                        string msgTitle = "";
                        msgText = "Please enter valid MR Number";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWin.Show();
                    }
                }
                else
                {
                    string msgTitle = "";
                    msgText = "Please select patient.";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWin.Show();
                }
            }
            else
            {
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            string sPath = "DischargeCard" + UnitID + "_" + PrintID + ".pdf";
            ViewPDFReport(sPath);
            //frmPrintDischargeSummary win_PrintDischargeSummary = new frmPrintDischargeSummary();
            //win_PrintDischargeSummary.lPrintID = PrintID;
            //win_PrintDischargeSummary.lUnitID = UnitID;
            //win_PrintDischargeSummary.lAdmissionID = IPDAdmissionID;
            //win_PrintDischargeSummary.lAdmissionUnitID = IPDAdmissionUnitID;
            //win_PrintDischargeSummary.Show();
        }
        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            if (IsFromAdmissionList == true)
            {
                frmAdmissionList _AdmissionListObject = new frmAdmissionList();
                ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Admission List";
            }
            else
            {
                SetCommandButtonState("Cancel");
                SetCommandButtonState("Load");
                objAnimation.Invoke(RotationType.Backward);
            }
            ClearControls();
            GetDischargeSummaryList();
        }
        private void cmdParameter_Click(object sender, RoutedEventArgs e)
        {
            //if (dgDischargeDetails.ItemsSource != null)
            //{
            //    if (dgDischargeDetails.SelectedItem != null)
            //    {
            //        if (!string.IsNullOrEmpty(((clsDischargeSummaryDetailsVO)dgDischargeDetails.SelectedItem).ParameterName))
            //        {
            //            if (patientDetails != null)
            //            {
            //                frmEMR objEMR = new frmEMR(patientDetails, ((clsDischargeSummaryDetailsVO)dgDischargeDetails.SelectedItem));
            //                ChildWindow CW = new ChildWindow();
            //                CW.Content = objEMR;
            //                CW.Width = 800;
            //                CW.Height = 550;
            //                CW.Loaded += new RoutedEventHandler(CW_Loaded);
            //                CW.Closed += new EventHandler(CW_Closed);
            //                CW.Show();
            //            }
            //        }
            //    }
            //}
        }
        private void rb_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb1 = new RadioButton();
            rb1 = (RadioButton)sender;

            if (OptionChecked == true)
                OptionChecked = false;
            else
                OptionChecked = true;

            rb1.IsChecked = OptionChecked;
        }
        private void OnDischargeCloseButtonClick(object sender, EventArgs e)
        {

        }
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html;
            cmbDischargeTemplate.IsEnabled = true;
            txtMRNo.IsEnabled = true;
            txtMRNo.IsReadOnly = false;
            CmddischargePatientListSearch.IsEnabled = true;
            CmdPatientSearch.IsEnabled = true;
            chkIsCancel.IsChecked = false;
            chkIsCancel.IsEnabled = true;
            richTextBox.IsReadOnly = false;
            objAnimation.Invoke(RotationType.Forward);
            SetCommandButtonState("New");
        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {
                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
                    ShowMessageBox("From Date should be less than To Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    dtpFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Plase Select To Date");
                dtpToDate.RaiseValidationError();
                ShowMessageBox("Plase Select To Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                dtpToDate.SelectedDate = null;
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate == null)
            {
                dtpFromDate.SetValidation("Plase Select From Date");
                dtpFromDate.RaiseValidationError();
                ShowMessageBox("Plase Select From Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                dtpFromDate.SelectedDate = null;
                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (res)
            {
                GetDischargeSummaryList();
            }
        }

        private void txtFrontMRNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Enter)
            {
                if (txtMRNo.Text.Length <= 0)
                {
                    GetDischargeSummaryList();
                }
            }

        }
        #endregion

        #region LostFocus and KeyDown Events
        private void txtFieldRemark_LostFocus(object sender, RoutedEventArgs e)
        {
            Liquid.RichTextBox txtRichTextBox = ((Liquid.RichTextBox)sender);
            if (txtRichTextBox != null)
            {
                if (DischargeSummaryDetailsList != null && DischargeSummaryDetailsList.Count > 0)
                {
                    int Index = Convert.ToInt32(txtRichTextBox.Tag);
                    if (Index <= DischargeSummaryDetailsList.Count)
                    {
                        DischargeSummaryDetailsList[Index].FieldDesc = "";
                        DischargeSummaryDetailsList[Index].FieldDesc = txtRichTextBox.HTML;
                        DischargeSummaryDetailsList[Index].FieldText = txtRichTextBox.Save(Format.Text, RichTextSaveOptions.None);
                    }
                }
            }

        }
        private void textFieldRemark_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtTextBox = ((TextBox)sender);
            if (txtTextBox != null)
            {
                if (DischargeSummaryDetailsList != null && DischargeSummaryDetailsList.Count > 0)
                {
                    int Index = Convert.ToInt32(txtTextBox.Tag);
                    if (Index <= DischargeSummaryDetailsList.Count)
                    {
                        DischargeSummaryDetailsList[Index].FieldText = txtTextBox.Text;
                    }
                }
            }

        }
        private void txtMRNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (txtMRNo.Text.Length <= 0)
                {
                    lblPatientName.Text = String.Empty;
                    cmbDischargeTemplate.SelectedItem = objList[0];
                }
            }
        }
        //private void dgDischargeDetails_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (((DataGrid)sender).ItemsSource != null)
        //    {
        //        if (e.PlatformKeyCode == 9)
        //        {
        //            long Index = ((DataGrid)sender).SelectedIndex;
        //            if (DischargeSummaryDetailsList.Count != Index)
        //            {
        //                ((DataGrid)sender).SelectedIndex += 1;
        //                ((DataGrid)sender).Focus();
        //                ((DataGrid)sender).Focus();
        //            }
        //        }
        //    }
        //}
        public FrameworkElement element;
        public DataGridRow row;

        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj, String TextBoxName)
          where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    if (((TextBox)Child).Name.Equals(TextBoxName))
                    {
                        return (ChildControl)Child;
                    }
                    else
                    {
                        ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);
                    }
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }


            }
            return null;
        }
        #endregion

        private void hlkView_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            if (((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).IsCancel == false) // checked for IsFreezed or Not.
            {
                FindPatient(((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).PatientID, ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).PatientUnitID, ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).MRNo);
                this.IPDAdmissionID = ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).AdmID;
                this.IPDAdmissionUnitID = ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).AdmUnitID;
                richTextBox.IsReadOnly = false;
                cmbDischargeTemplate.SelectedValue = ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).DischargeTemplateID;
                chkIsCancel.IsChecked = ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).IsCancel;
                SetCommandButtonState("View");
                txtMRNo.IsEnabled = true;
                cmbDischargeTemplate.IsEnabled = true;
                chkIsCancel.IsEnabled = true;
                objAnimation.Invoke(RotationType.Forward);
            }
            else
            {
                if (dgDischargeSummaryList.SelectedItem != null)
                {
                    clsIPDDischargeSummaryVO objVo = (clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem;
                    txtMRNo.Text = objVo.MRNo;
                    lblPatientName.Text = objVo.PatientName;
                    cmbDischargeTemplate.SelectedValue = objVo.DischargeTemplateID;
                    richTextBox.Html = objVo.TextDocument;
                    chkIsCancel.IsChecked = ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).IsCancel;
                }
                SetCommandButtonState("View");
                btnSave.IsEnabled = false;
                CmddischargePatientListSearch.IsEnabled = false;
                CmdPatientSearch.IsEnabled = false;
                cmbDischargeTemplate.IsEnabled = false;
                txtMRNo.IsReadOnly = false;
                richTextBox.IsReadOnly = true;
                chkIsCancel.IsEnabled = false;
                objAnimation.Invoke(RotationType.Forward);
            }
        }

        private void hlkPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgDischargeSummaryList.SelectedItem != null)
            {
                string sPath = "DischargeCard" + ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).UnitID + "_" + ((clsIPDDischargeSummaryVO)dgDischargeSummaryList.SelectedItem).ID + ".pdf";
                ViewPDFReport(sPath);
            }
        }

    }
}
