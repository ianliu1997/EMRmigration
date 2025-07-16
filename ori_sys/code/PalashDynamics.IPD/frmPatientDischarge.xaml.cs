using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Billing;

namespace PalashDynamics.IPD.Forms
{
    public partial class frmPatientDischarge : UserControl
    {
        public PagedSortableCollectionView<clsIPDDischargeVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsIPDDischargeVO> MasterNonCensusList { get; private set; }
       

        #region Variable Declaration
        public string ModuleName { get; set; }
        public string Action { get; set; }
        clsPatientGeneralVO patientDetails = null;       
        bool IsViewClick;
        bool IsSearchClick = false;
        clsMenuVO _SelfMenuDetails = null;
        clsGetIPDPatientBizActionVO PatientVO;
        string msgText;
        public long PatientID = 0;
        public long _PatientID { get; set; }
        public long UnitID = 0;
        public long IPDAdmissionID = 0;
        public string IPDAdmissionNo = String.Empty;
        public long PatientUnitID = 0, BedID = 0;
        public long _PatientUnitID { get; set; }
        private string MRNO = null;
        public string PatientName = null;
        bool IsEMR = false;
        #endregion
        public void PreInitiate(ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #region Constructor
        public frmPatientDischarge()
        {
            InitializeComponent();
            
            this.Loaded += new RoutedEventHandler(frmPatientDischarge_Loaded);
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId != null)
                {
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;
                    UnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                    PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientUnitID;
                    PatientName = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
                    IPDAdmissionID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                    IPDAdmissionNo = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.IPDNO;
                    MRNO = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;
                    FindPatient(PatientID, UnitID, MRNO);
                }
            }
        }
        #endregion

        #region Loaded
        void frmPatientDischarge_Loaded(object sender, RoutedEventArgs e)
        {
            FillDoctor();
            FillDischageType();
            FillDischargeDestination();
            
            dtpDischargeDate.SelectedDate = DateTime.Now;
            tpDischargeTime.Value = DateTime.Now;
            CmdSave.IsEnabled = false;
        }
        #endregion

        #region DataGrid Refresh Event
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

        }
        #endregion
        public void ToBackPanel()
        {
            
        }
        #region Button Clicked Events

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            frmAdmissionList _AdmissionListObject = new frmAdmissionList();
            ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Admission List";
        }
        private void cmdDisListPastDetails_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdPatientSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdPastDetails_Click(object sender, RoutedEventArgs e)
        {

        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (patientDetails != null)
            {
                if (IsValidate())
                {
                    if (patientDetails != null)
                    {
                        checkBill();
                    }
                }
            }
        }


        public bool GetApprovalDepartmentList(clsPatientGeneralVO _patientDetails)
        {
            bool flg = true;
            if (_patientDetails != null)
            {
                clsGetListOfAdviseDischargeForApprovalBizActionVO BizAction = new clsGetListOfAdviseDischargeForApprovalBizActionVO();
                BizAction.AddAdviseDetails = new clsDiischargeApprovedByDepartmentVO();
                BizAction.AddAdviseDetails.AdmissionId = _patientDetails.IPDAdmissionID;
                BizAction.AddAdviseDetails.AdmissionUnitID = _patientDetails.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetListOfAdviseDischargeForApprovalBizActionVO result = arg.Result as clsGetListOfAdviseDischargeForApprovalBizActionVO;
                        if (result.AddAdviseList.Count > 0)
                        {                                                       
                            foreach (var item in result.AddAdviseList)
                            {
                                if (!item.ApprovalStatus)
                                {
                                    flg = false;
                                    break;
                                }
                            }
                            if (!flg)
                            {
                                msgText = "Please check the Deparment Approval before Discharge.";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW.Show();
                            }
                            else
                            {
                                msgText = "Are you Sure You Want To Give Discharge To " + "'" + patientDetails.IPDPatientName + "'";
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        SaveDischarge();
                                    }
                                };
                                msgW1.Show();
                            }
                           
                        }
                        else
                        {
                            msgText = "Are you Sure You Want To Give Discharge To " + "'" + patientDetails.IPDPatientName + "'";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    SaveDischarge();
                                }
                            };
                            msgW1.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            return flg;
        }
        #endregion

        #region FillComboboxes

        private void FillDoctor()
        {
            clsGetDoctorListBizActionVO BizAction = new clsGetDoctorListBizActionVO();
            BizAction.DoctorDetailsList = new List<clsDoctorVO>();
            BizAction.IsComboBoxFill = true;
            BizAction.IsInternal = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorListBizActionVO)arg.Result).MasterList);
                    cmbDischargingDoctor.ItemsSource = objList;
                    cmbDischargingDoctor.SelectedItem = objList[0];

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDischageType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DischargeType;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbDischargeType.ItemsSource = null;
                    cmbDischargeType.ItemsSource = objList;
                    cmbDischargeType.SelectedItem = objList[0];
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }

        private void FillDischargeDestination()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DischargeDestination;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbDischargeDestination.ItemsSource = null;
                    cmbDischargeDestination.ItemsSource = objList;
                    cmbDischargeDestination.SelectedItem = objList[0];
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }

        private bool IsValidate()
        {
            bool isValid = true;
            if (((MasterListItem)cmbDischargingDoctor.SelectedItem).ID == 0)
            {
                cmbDischargingDoctor.TextBox.SetValidation("Please Select Doctor.");
                cmbDischargingDoctor.TextBox.RaiseValidationError();
                cmbDischargingDoctor.TextBox.Focus();
                isValid = false;
            }
            if (((MasterListItem)cmbDischargeType.SelectedItem).ID == 0)
            {
                cmbDischargeType.TextBox.SetValidation("Please, Select Discharge Type.");
                cmbDischargeType.TextBox.RaiseValidationError();
                cmbDischargeType.TextBox.Focus();
                isValid = false;
            }
            if (((MasterListItem)cmbDischargeDestination.SelectedItem).ID == 0)
            {
                cmbDischargeDestination.TextBox.SetValidation("Please Select Discharge Destination.");
                cmbDischargeDestination.TextBox.RaiseValidationError();
                cmbDischargeDestination.TextBox.Focus();
                isValid = false;
            }
            if (txtMRNo.Text != strMRNO)
            {
                txtMRNo.SetValidation("Please, Check MRNO.");
                txtMRNo.RaiseValidationError();
                txtMRNo.Focus();
                isValid = false;
            }
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsAllowDischargeRequest == true)
            {
                if (dgEmployeeDischarge.ItemsSource == null)
                {
                    msgText = "Discharge Request is not Generated for " + "'" + patientDetails.IPDPatientName + "'";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    isValid = false;
                }
                if (isValid == true)
                {
                    List<clsIPDDischargeVO> DischargeDeptRequestList = new List<clsIPDDischargeVO>();
                    DischargeDeptRequestList = ((List<clsIPDDischargeVO>)dgEmployeeDischarge.ItemsSource).ToList();

                    var item = from r in DischargeDeptRequestList
                               where r.Status == false
                               select r;
                    if (item != null && item.ToList().Count > 0)
                    {
                        msgText = "Discharge Approval for " + "'" + patientDetails.IPDPatientName + "'" + " is not Confirmed by All departments. \n Sorry, Patient can't be Discharge!";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        isValid = false;
                    }
                }
            }
            return isValid;
        }

        private void SaveDischarge()
        {
            try
            {
                clsAddIPDDischargeBizActionVO bizActionVO = new clsAddIPDDischargeBizActionVO();
                bizActionVO.DischargeDetails = new clsIPDDischargeVO();
                bizActionVO.DischargeDetails.PatientID = patientDetails.PatientID;
                bizActionVO.DischargeDetails.PatientUnitID = patientDetails.UnitId;
                bizActionVO.DischargeDetails.IPDAdmissionID = patientDetails.IPDAdmissionID;
                bizActionVO.DischargeDetails.IPDAdmissionNo = patientDetails.IPDAdmissionNo;
                if (dtpDischargeDate.SelectedDate != null)
                {
                    bizActionVO.DischargeDetails.DischargeDate = Convert.ToDateTime(dtpDischargeDate.SelectedDate);
                    bizActionVO.DischargeDetails.DischargeTime = Convert.ToDateTime(tpDischargeTime.Value);//Convert.ToDateTime(dtpDischargeDate.SelectedDate);
                }

                if (((MasterListItem)cmbDischargingDoctor.SelectedItem).ID != 0)
                    bizActionVO.DischargeDetails.DischargeDoctor = ((MasterListItem)cmbDischargingDoctor.SelectedItem).ID;
                if (((MasterListItem)cmbDischargeType.SelectedItem).ID != 0)
                    bizActionVO.DischargeDetails.DischargeType = ((MasterListItem)cmbDischargeType.SelectedItem).ID;
                if(((MasterListItem)cmbDischargeDestination.SelectedItem).ID != 0)
                    bizActionVO.DischargeDetails.DischargeDestination = ((MasterListItem)cmbDischargeDestination.SelectedItem).ID;
                if(chkIsDeathDischarge.IsChecked == true)
                    bizActionVO.DischargeDetails.IsDeathdischarge = true;
                else
                    bizActionVO.DischargeDetails.IsDeathdischarge = false;
                if(!String.IsNullOrEmpty(txtRemark.Text))
                    bizActionVO.DischargeDetails.DischargeRemark = txtRemark.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsAddIPDDischargeBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            msgText = "Patient Discharged Successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    patientDetails = new clsPatientGeneralVO();
                                    frmAdmissionList _AdmissionListObject = new frmAdmissionList();
                                    ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                    mElement.Text = "Admission List";
                                }
                            };
                            msgW1.Show();
                        }
                        else if (((clsAddIPDDischargeBizActionVO)args.Result).SuccessStatus == 2)
                        {
                            msgText = "'"+ PatientName + "'"+ "Already Discharged.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    patientDetails = new clsPatientGeneralVO();
                                }
                            };
                            msgW1.Show();
                        }
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
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillDischargeStatus()
        {
            clsGetDischargeStatusBizActionVO bizActionVO = new clsGetDischargeStatusBizActionVO();
            bizActionVO.DischargeDetails = new clsIPDDischargeVO();
            bizActionVO.DischargeList = new List<clsIPDDischargeVO>();
            bizActionVO.DischargeDetails.AdmID = IPDAdmissionID;
            bizActionVO.DischargeDetails.AdmUnitID = UnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    bizActionVO.DischargeList = (((clsGetDischargeStatusBizActionVO)args.Result).DischargeList);
                    if (bizActionVO.DischargeList.Count > 0)
                    {
                        dgEmployeeDischarge.ItemsSource = null;
                        dgEmployeeDischarge.ItemsSource = bizActionVO.DischargeList;
                            
                        dgEmployeeDischarge.SelectedIndex = -1;
                        dgDischargeAdviceConfirmedDataPager.Source = null;
                        dgDischargeAdviceConfirmedDataPager.PageSize = Convert.ToInt32(bizActionVO.MaximumRows);
                    }
                }
            };
            client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        #region Patient Search Buttons Click

        private void CmddischargeListSearch_Click(object sender, RoutedEventArgs e)
        {
            IsViewClick = false;
            IsSearchClick = false;
            ClearUI();
            #region Commented By CDS For Only Get Addmitted Patient List
            //ModuleName = "PalashDynamics.IPD";
            //Action = "PalashDynamics.IPD.IPDPatientSearch";
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //WebClient c = new WebClient();

            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            #endregion

            IPDPatientSearch frm = new IPDPatientSearch();
            frm.IsFromDischarge = true;            
            if (IsViewClick == false)
            {
                frm.Closed += new EventHandler(cw_Closed);
            }            
            frm.Show();
        }
        private void CmddischargePatientListSearch_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            if (txtMRNo.Text.Length != 0)
            {
                IsViewClick = false;
                IsSearchClick = true;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
            }
            else
            {
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNo.Focus();
                msgW1.Show();
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }

        private void CmdPatientListSearch_Click(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion
        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            
        }

        int DisAdm;
        private void rbtnDischargedList_Checked(object sender, RoutedEventArgs e)
        {
            DisAdm = 1;
        }

        private void rbtnExpectedDischargeList_Checked(object sender, RoutedEventArgs e)
        {
            DisAdm = 2;
        }

        private void rbtnAdvisedDischargeList_Checked(object sender, RoutedEventArgs e)
        {
            DisAdm = 3;
        }

        private void cmdCancelDischarge_Click(object sender, RoutedEventArgs e)
        {
            
        }



        #region Click On View,EMR and Discharge Summary

        private void cmdEMR_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void cmdDischargeSummary_Click(object sender, RoutedEventArgs e)
        {
            
            

        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
                       
        }
        #endregion

        private void txtMRNo_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void Date_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void txtMRNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (txtMRNo.Text.Length <= 0)
                {
                    ClearUI();
                    CmdSave.IsEnabled = false;
                }
                else
                {
                    CmdSave.IsEnabled = true;
                }
            }
            if (txtMRNo.Text.Length > 0)
                CmdSave.IsEnabled = true;
        }
        #region Private Methods
        string strMRNO = null;
        private void FindPatient(long PatientID, long PatientUnitID, string MRNO)
        {
            clsGetIPDPatientBizActionVO BizAction = new clsGetIPDPatientBizActionVO();
            BizAction.PatientDetails = new clsGetIPDPatientVO();

            if (IsSearchClick == true)
            {
                BizAction.PatientDetails.GeneralDetails.PatientID = Convert.ToInt64(null);
                BizAction.PatientDetails.GeneralDetails.UnitId = Convert.ToInt64(null);
            }
            else
            {
                BizAction.PatientDetails.GeneralDetails.PatientID = PatientID;
                BizAction.PatientDetails.GeneralDetails.PatientUnitID = PatientUnitID;
                BizAction.PatientDetails.GeneralDetails.UnitId = UnitID;
            }
            BizAction.PatientDetails.GeneralDetails.MRNo = MRNO;
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
                       
                        if (patientDetails.IsDischarged == true)
                        {
                            msgText = "' " + patientDetails.IPDPatientName + " '" + "Already Discharged.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    txtMRNo.Focus();
                                    ClearUI();
                                }
                            };
                            msgW1.Show();
                        }
                        else
                        {
                            if (!((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                            {
                                PatientName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDPatientName.ToString();
                                UnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;
                                PatientName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientName;
                                IPDAdmissionID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionID;
                                IPDAdmissionNo = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionNo;
                                PatientName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDPatientName.ToString();
                                PatientID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                                _PatientID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                                _PatientUnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;
                                if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo != null)
                                {
                                    strMRNO = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                                    txtMRNo.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                                    CmdSave.IsEnabled = true;
                                }
                                if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionNo != null)
                                {
                                    txtIPDNo.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionNo;
                                }
                                if (patientDetails.PatientID != null)
                                    patientDetails.PatientID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                                if (patientDetails.PatientUnitID != null)
                                    patientDetails.PatientUnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;
                                if (patientDetails.GenderID != null)
                                    patientDetails.GenderID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.GenderID;
                                if (patientDetails.ClassName != null)
                                {
                                    patientDetails.ClassName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.ClassName;
                                    txtBedCategory.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.ClassName;
                                }
                                if (patientDetails.WardName != null)
                                {
                                    txtWard.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.WardName;
                                    patientDetails.WardName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.WardName;
                                }
                                if (patientDetails.BedID != null && patientDetails.BedName != null)
                                {
                                    int BedId = Convert.ToInt32(((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.BedID);
                                    txtBed.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.BedName;
                                }
                                if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.RegistrationDate != null)
                                {
                                    txtAdmDate.Text = Convert.ToString(((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.RegistrationDate);
                                }
                                if (PatientName != null)
                                {
                                    lblPatientName.Text = PatientName;
                                }
                                if(patientDetails.DoctorName != null)
                                    txtDoctor.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.DoctorName;
                                if(patientDetails.Unit != null)
                                    txtUnit.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.Unit;
                                if (IsViewClick == true)
                                {
                                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                    ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = patientDetails.PatientID;
                                    ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = patientDetails.PatientUnitID;

                                    ModuleName = "OPDModule";
                                    Action = "OPDModule.PatientAndVisitDetails";
                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    WebClient c = new WebClient();
                                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                                }
                                FillDischargeStatus();
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
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        
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
            }
            else
            {
                txtMRNo.Text = "";
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }
        
        private void GetSelectedPatientDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                PatientName = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                IPDAdmissionID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;
                IPDAdmissionNo = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionNo;
                FindPatient(PatientID, PatientUnitID, MRNO);
            }
        }

        private void ClearUI()
        {
            txtIPDNo.Text = String.Empty;
            txtAdmDate.Text = String.Empty;
            txtBed.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtDoctor.Text = String.Empty;
            txtBedCategory.Text = String.Empty;
            txtWard.Text = String.Empty;
            lblPatientName.Text = String.Empty;
            dgEmployeeDischarge.ItemsSource = null;
            dgEmployeeDischarge.UpdateLayout();
            cmbDischargeDestination.SelectedItem = ((List<MasterListItem>)cmbDischargeDestination.ItemsSource).Where(z => z.ID == 0).FirstOrDefault();
            cmbDischargeType.SelectedItem = ((List<MasterListItem>)cmbDischargeType.ItemsSource).Where(z => z.ID == 0).FirstOrDefault();
            cmbDischargingDoctor.SelectedItem = ((List<MasterListItem>)cmbDischargingDoctor.ItemsSource).Where(z => z.ID == 0).FirstOrDefault();
        }
        #endregion

        public void checkBill()
        {           
            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.RequestTypeID = 1;
            BizAction.IsRequest = true;
            BizAction.PatientID = PatientID > 0 ? PatientID : _PatientID;
            BizAction.PatientUnitID = PatientUnitID > 0 ? PatientUnitID : _PatientUnitID;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = BizAction.PatientUnitID.Value;
            }
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

            BizAction.Opd_Ipd_External = 1;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, i) =>
            {
                if (i.Error == null && i.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = i.Result as clsGetBillSearchListBizActionVO;
                    List<clsBillVO> objList = new List<clsBillVO>();
                    objList = result.List;
                    if (result.List != null)
                    {
                        //item.Opd_Ipd_External_UnitId == UnitID
                        objList = (from item in objList
                                   where item.Opd_Ipd_External_Id == IPDAdmissionID &&
                                    item.Opd_Ipd_External_UnitId == 2
                                    select item).ToList();

                        if (objList != null && objList.Count > 0)
                        {

                            if (patientDetails.IsReadyForDischarged == false && patientDetails.IsDischarged == false)
                            {
                                msgText = "Please Finalize the Bill before Discharge.";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW.Show();
                                return;
                            }
                            else
                            {
                                bool flag = GetApprovalDepartmentList(patientDetails);
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "You are not able to Discharge Without creating the Bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "You are not able to Discharge Without creating the Bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();  
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();            
        }
    }
}
