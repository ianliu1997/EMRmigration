using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.IPD;
using System.Collections.Generic;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;
using System.Windows.Data;
using System.Net;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;
using System.Linq;
using System.Windows.Media;
using CIMS.Forms;
using System.Windows.Browser;
using System.Windows.Controls.Primitives;
using PalashDynamics.Pharmacy;

namespace PalashDynamics.IPD.Forms
{
    public partial class frmAdmissionList : UserControl
    {
        private bool ISCancel { get; set; }
        clsPatientGeneralVO patientDetails = null;
        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;
        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }
        #region Variable declaration
        public clsIPDAdmissionVO objAdm = new clsIPDAdmissionVO();
        bool IsPageLoded = false;
        bool UseAppDoctorID = true;
        int ClickedFlag = 0;
        WaitIndicator Indicatior = null;
        public string ModuleName { get; set; }
        long UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
        public string Action { get; set; }
        public bool chkDischargeApproval = false;
        string msgText;

        private bool _IsCallFromAdmissionForm = false;
        private string _NormalExpressstring = string.Empty;
        #endregion
        #region Pagging
        public PagedSortableCollectionView<clsIPDAdmissionVO> DataList { get; private set; }

        public int DataListPageSize
        {
            get { return DataList.PageSize; }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }
        #endregion
        public frmAdmissionList()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmAdmissionList_Loaded);
            this.DataContext = new clsIPDAdmissionVO()
            {
            };
            DataList = new PagedSortableCollectionView<clsIPDAdmissionVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.Source = DataList;
            dgDataPager.PageSize = DataListPageSize;
            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                this.DataContext = new clsIPDAdmissionVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                    DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                    DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID,
                };
                dtpFromDate1.SelectedDate = DateTime.Now;
                dtpToDate1.SelectedDate = DateTime.Now;                
                GetAdmissionList();
                grdSearch.Visibility = Visibility.Collapsed;
                grdSearchforUser.Visibility = Visibility.Visible;
            }
            else
            {                
                grdSearch.Visibility = Visibility.Visible;
                grdSearchforUser.Visibility = Visibility.Collapsed;
                GetAdmissionList();
            }
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "- Select -"));
            cmbWard.ItemsSource = null;
            cmbWard.ItemsSource = objList;
            cmbWard.SelectedValue = objList[0].ID;
        }

        public void GetPatientData()
        {
            clsGetPatientGeneralDetailsListBizActionVO BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();
            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();
            patientDetails = new clsPatientGeneralVO();
            BizActionObject.MRNo = txtMRNo.Text.Trim();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        clsGetPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetPatientGeneralDetailsListBizActionVO;
                        if (result.PatientDetailsList.Count > 0)
                        {
                            patientDetails.PatientID = result.PatientDetailsList[0].PatientID;
                            patientDetails.PatientUnitID = result.PatientDetailsList[0].UnitId;
                            Indicatior.Close();
                        }
                        else
                        {
                            Indicatior.Close();
                            msgText = "Please check MR Number";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            txtMRNo.Focus();
                            msgW1.Show();
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #region FillComboBox      
        private void FillUnit()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbListUnit.ItemsSource = null;
                    cmbListUnit.ItemsSource = objList.DeepCopy();
                    cmbListUnit.SelectedValue = UnitId;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillClass()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
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
                    cmbAdmListClass.ItemsSource = null;
                    cmbAdmListClass.ItemsSource = objList;

                    cmbAdmListClass.SelectedValue = objList[0].ID;
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                
            }
        }

        private void FillWard()
        {
            try
            {
                clsGetIPDWardByClassIDBizActionVO BizAction = new clsGetIPDWardByClassIDBizActionVO();
                BizAction.BedDetails = new clsIPDBedTransferVO();
                BizAction.BedDetails.ClassID = ((MasterListItem)cmbAdmListClass.SelectedItem).ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsGetIPDWardByClassIDBizActionVO objClass = ((clsGetIPDWardByClassIDBizActionVO)e.Result);

                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));

                        if (objClass != null)
                        {
                            if (objClass.BedList != null)
                            {
                                foreach (var item in objClass.BedList)
                                {
                                    objList.Add(new MasterListItem(item.WardID, item.Ward));
                                }
                            }
                        }
                        cmbWard.ItemsSource = null;
                        cmbWard.ItemsSource = objList;
                        cmbWard.SelectedValue = objList[0].ID;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                
            }
        }

        private void FillDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID; ;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbAdmListDoctor.ItemsSource = null;
                    cmbAdmListDoctor.ItemsSource = objList;
                    cmbAdmListDoctor.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        private void GetAdmissionList()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsGetIPDAdmissionListBizActionVO BizAction = new clsGetIPDAdmissionListBizActionVO();
            BizAction.AdmList = new List<clsIPDAdmissionVO>();
            BizAction.AdmDetails = new clsIPDAdmissionVO();
            if (!((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                if (dtpFromDate.SelectedDate != null)
                {
                    BizAction.AdmDetails.FromDate = dtpFromDate.SelectedDate.Value.Date;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    BizAction.AdmDetails.ToDate = dtpToDate.SelectedDate.Value.Date;
                }

                if (!string.IsNullOrEmpty(txtMRNo.Text))
                {                    
                    BizAction.AdmDetails.MRNo = txtMRNo.Text;
                }
                else
                {
                    Comman.SetDefaultHeader(_SelfMenuDetails);
                }
                if (cmbListUnit.SelectedItem != null)
                {
                    if (((MasterListItem)cmbListUnit.SelectedItem).ID > 0)
                    {
                        BizAction.AdmDetails.UnitId = ((MasterListItem)cmbListUnit.SelectedItem).ID;
                    }
                }
                else
                {
                    BizAction.AdmDetails.UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                }

                if (cmbAdmListClass.SelectedItem != null)
                {
                    if (((MasterListItem)cmbAdmListClass.SelectedItem).ID > 0)
                    {
                        BizAction.AdmDetails.BedCategoryID = ((MasterListItem)cmbAdmListClass.SelectedItem).ID;
                    }
                }
                if (txtFirstName.Text != null && txtFirstName.Text.Trim() != "")
                {

                    BizAction.AdmDetails.FirstName = txtFirstName.Text;
                   
                }
                if (txtlastName.Text != null && txtlastName.Text.Trim() != "")
                {

                    BizAction.AdmDetails.LastName = txtlastName.Text;

                }
                if (txtlastName.Text != null && txtlastName.Text.Trim() != "")
                {

                    BizAction.AdmDetails.LastName = txtlastName.Text;
                    
                }
                 if (txtOldRegistrationNo.Text != null && txtOldRegistrationNo.Text.Trim() != "")
                {


                    BizAction.AdmDetails.OldRegistrationNo =txtOldRegistrationNo.Text;
                }

                if (cmbWard.SelectedItem != null)
                {
                    if (((MasterListItem)cmbWard.SelectedItem).ID > 0)
                    {
                        BizAction.AdmDetails.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;
                    }
                }

                if (cmbAdmListDoctor.SelectedItem != null)
                {
                    if (((MasterListItem)cmbAdmListDoctor.SelectedItem).ID > 0)
                    {
                        BizAction.AdmDetails.DoctorID = ((MasterListItem)cmbAdmListDoctor.SelectedItem).ID;
                    }
                }

                BizAction.AdmDetails.IsMedicoLegalCase = (bool)chkMedicoLegalCase.IsChecked;
                BizAction.AdmDetails.IsCancelled = Convert.ToBoolean(chkCancelledAdmissions.IsChecked);
                BizAction.AdmDetails.IsNonPresence = Convert.ToBoolean(chkNonPresence.IsChecked);
                BizAction.AdmDetails.IsAllPatient = Convert.ToBoolean(chkAllPatients.IsChecked);
                BizAction.AdmDetails.CurrentAdmittedList = Convert.ToBoolean(chkCurrentAdmittedList.IsChecked);
            }
            else
            {
                if (dtpFromDate1.SelectedDate != null)
                {
                    BizAction.AdmDetails.FromDate = dtpFromDate1.SelectedDate.Value.Date;
                }
                if (dtpToDate1.SelectedDate != null)
                {
                    BizAction.AdmDetails.ToDate = dtpToDate1.SelectedDate.Value.Date;
                }

                if (patientDetails != null && !string.IsNullOrEmpty(txtMRNo1.Text))
                {
                    BizAction.AdmDetails.PatientId = patientDetails.PatientID;
                    BizAction.AdmDetails.PatientUnitID = patientDetails.PatientUnitID;
                }
                else
                {
                    Comman.SetDefaultHeader(_SelfMenuDetails);
                }
                BizAction.AdmDetails.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                BizAction.AdmDetails.IsMedicoLegalCase = (bool)chkMedicoLegalCase1.IsChecked;
                BizAction.AdmDetails.IsCancelled = Convert.ToBoolean(chkCancelledAdmissions1.IsChecked);
                BizAction.AdmDetails.IsNonPresence = Convert.ToBoolean(chkNonPresence1.IsChecked);
                BizAction.AdmDetails.IsAllPatient = Convert.ToBoolean(chkAllPatients1.IsChecked);
                BizAction.AdmDetails.CurrentAdmittedList = Convert.ToBoolean(chkCurrentAdmittedList1.IsChecked);
            }
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetIPDAdmissionListBizActionVO result = arg.Result as clsGetIPDAdmissionListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;
                    if (result.AdmList != null)
                    {
                        foreach (var item in result.AdmList)
                        {
                            item.IsEnableIPDBill = true;
                            item.hlMConsumption = true; //AJ Date 5/1/2017
                            if (item.IsClosed.Equals(true))
                            {
                                item.IsAdviseEnable = false;
                                item.IsCancelAdmEnable = false;
                                item.NextapColor = 5;
                            }
                            else if (item.IsDischarge.Equals(true))
                            {
                                item.IsAdviseEnable = false;
                                item.IsCancelAdmEnable = false;
                                item.NextapColor = 3;
                            }
                            if (item.IsNonPresence.Equals(true))
                            {
                                item.NextapColor = 4;
                            }

                            if (item.IsAppliedForApproval.Equals(false))
                            {
                                item.IsCheckBoxVisible = Convert.ToString(Visibility.Collapsed);
                            }
                            if (item.InterOrFinal == 1)
                            {
                                item.IsCancelAdmEnable = false;
                                item.IsEnableDischarge = true;
                            }
                            else
                            {
                                //item.IsEnableIPDBill = true;
                            }
                            if (item.IsDischarge.Equals(true))
                            {
                                //item.IsEnableIPDBill = true;
                            }
                            else
                            {
                                item.IsEnableDischarge = true;
                            }
                            //}
                            if (item.IsCancelled.Equals(true))
                            {
                                item.IsAdviseEnable = false;
                                item.IsCancelAdmEnable = false;
                                item.NextapColor = 1;
                                cmdConsent.IsEnabled = false;
                                item.IsEnableDischarge = false;
                                item.IsEnableIPDBill = false;
                                item.hlMConsumption = false;  //AJ Date 5/1/2017
                            }
                            DataList.Add(item);
                        }
                        cmdConsent.IsEnabled = false;
                        PagedCollectionView collection = new PagedCollectionView(result.AdmList);
                        dgPatientAdmissionList.ItemsSource = null;
                        dgPatientAdmissionList.ItemsSource = collection;
                        dgPatientAdmissionList.CanUserSortColumns = true;
                        dgPatientAdmissionList.SelectedItem = null;
                        dgDataPager.Source = null;
                        dgDataPager.Source = DataList;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        txtTotalAmissions.Text = result.TotalRows.ToString();   
                    }
                }
                Indicatior.Close();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        #region Added by Ashish Z. for Cancel the Admission
        private void CancelAdmission(long admissionID, long admissionUnitID, string PatientName)
        {
            try
            {
                clsCancelIPDAdmissionBizactionVO BizAction = new clsCancelIPDAdmissionBizactionVO();
                BizAction.AdmissionDetailsList = new List<clsIPDAdmissionVO>();
                BizAction.AdmissionID = admissionID;
                BizAction.AdmissionUnitID = admissionUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsCancelIPDAdmissionBizactionVO result = arg.Result as clsCancelIPDAdmissionBizactionVO;
                        if (result.SuccessStatus == 1)
                        {
                            msgText = "Bill is already prepared you can not cancel " + "'" + PatientName + "'s" + " admission";
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.Show();
                        }
                        else if (result.SuccessStatus == 2)
                        {
                            msgText = "You cannot cancel " + "'" + PatientName + "'s" + " admission, please first cancel charges";
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.Show();
                        }
                        else if (result.SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure, you want to cancel " + "'" + PatientName + "'s" + " Admission ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    UpdateCancelAdmission(admissionID, admissionUnitID);
                                }
                            };
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        msgText = "Error occured while processing.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateCancelAdmission(long admissionID, long admissionUnitID)
        {
            try
            {
                clsCancelIPDAdmissionBizactionVO BizAction = new clsCancelIPDAdmissionBizactionVO();
                BizAction.UpdateCancelAdmission = true;
                BizAction.AdmissionID = admissionID;
                BizAction.AdmissionUnitID = admissionUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsCancelIPDAdmissionBizactionVO result = arg.Result as clsCancelIPDAdmissionBizactionVO;

                        msgText = "Admission Cancelled Successfuly";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
                

        void frmAdmissionList_Loaded(object sender, RoutedEventArgs e)
        {
            Comman.SetDefaultHeader(_SelfMenuDetails);
            ((IApplicationConfiguration)App.Current).SelectedIPDPatient = null;

            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.RoleDetails.MenuList != null)//Added By Arati To View Link as Per User
            {
                List<clsMenuVO> MenuList = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.RoleDetails.MenuList;
                foreach (DataGridColumn obj in dgPatientAdmissionList.Columns)
                {


                }
            }
            cmdCompanyApprovedAmount.IsEnabled = false;
            cmdConsent.IsEnabled = false;
            FillUnit();
            FillClass();
            FillDoctor();
            #region Commented By Bhushanp 10/01/2017
            if (dgPatientAdmissionList != null)
            {
                if (dgPatientAdmissionList.Columns.Count > 0)
                {
                    //Commented By Bhushanp 10/01/2017 
                    //dgPatientAdmissionList.Columns[0].Header = "View IPD Bill";
                    //dgPatientAdmissionList.Columns[1].Header = "Discharge";
                    //dgPatientAdmissionList.Columns[2].Header = "Print";
                    //dgPatientAdmissionList.Columns[3].Header = "MR No.";
                    //dgPatientAdmissionList.Columns[4].Header = "IPD No.";
                    //dgPatientAdmissionList.Columns[5].Header = "Patient Name";
                    //dgPatientAdmissionList.Columns[6].Header = "Admission Date";
                    //dgPatientAdmissionList.Columns[7].Header = "Doctor Name";
                    //dgPatientAdmissionList.Columns[8].Header = "Company Name";
                    //dgPatientAdmissionList.Columns[9].Header = "Ward";
                    //dgPatientAdmissionList.Columns[10].Header = "Bed";
                    //dgPatientAdmissionList.Columns[11].Header = "Ref. Entity";
                    //dgPatientAdmissionList.Columns[12].Header = "Sanctioned Amount";
                    //dgPatientAdmissionList.Columns[13].Header = "Company Approved Amount";
                    //dgPatientAdmissionList.Columns[14].Header = "CreditLimitAmount";
                    //dgPatientAdmissionList.Columns[15].Header = "Bill Amount";
                    //dgPatientAdmissionList.Columns[16].Header = "Self Pay";
                    //dgPatientAdmissionList.Columns[17].Header = "Company Pay";
                    //dgPatientAdmissionList.Columns[18].Header = "Total Balance";
                    //dgPatientAdmissionList.Columns[19].Header = "Unbilled Charges";
                    //dgPatientAdmissionList.Columns[20].Header = "Effective Advance";
                    //dgPatientAdmissionList.Columns[21].Header = "EMR";
                    //dgPatientAdmissionList.Columns[22].Header = "Applied For Approval";
                    //dgPatientAdmissionList.Columns[23].Header = "Discharge Date";
                    //dgPatientAdmissionList.Columns[24].Header = "Advance Balance";
                }
            }
            #endregion
        }
        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetAdmissionList();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate != null)
            {
                if (dtpToDate.SelectedDate.Value < dtpFromDate.SelectedDate.Value)
                {
                    msgText = "From date can not be greater than to date.";
                    //}
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtMRNo.Text))
                    {
                        GetAdmissionList();
                    }                    
                    else
                    {
                        GetAdmissionList();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtMRNo.Text))
                {
                    GetAdmissionList();                    
                }
                else
                {
                    GetAdmissionList();
                }
            }
        }

        private void cmdCancelAdmission_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientAdmissionList.SelectedItem != null)
            {
                CancelAdmission((dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO).ID, (dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO).UnitId, (dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO).PatientName);
            }
            else
            {
                msgText = "Please select Patient for cancel the Admission";
                MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }
        }

        private void cmdFindPatient1_Click(object sender, RoutedEventArgs e)
        {
            ForUser = true;
            patientDetails = new clsPatientGeneralVO();
            if (txtMRNo1.Text.Length != 0)
            {
                IsViewClick = false;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo1.Text.Trim());                
            }            
        }
        bool ForUser = false;
        private void cmdFindPatient_Click(object sender, RoutedEventArgs e)
        {
            if (txtMRNo.Text.Length != 0)
            {
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

        private void FindPatient(long PatientID, long PatientUnitId, string MRNO)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
                BizAction.PatientDetails = new clsPatientVO();
                BizAction.PatientDetails.GeneralDetails.PatientID = PatientID;
                BizAction.PatientDetails.GeneralDetails.UnitId = PatientUnitId;
                BizAction.PatientDetails.GeneralDetails.MRNo = MRNO;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (!((clsGetPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                            {
                               
                            }
                            else
                            {
                                dgPatientAdmissionList.ItemsSource = null;
                                Indicatior.Close();
                                 msgText = "Please check MR Number";
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                if (ForUser)
                                { txtMRNo1.Focus(); }
                                else
                                    txtMRNo.Focus();
                                msgW1.Show();
                                Comman.SetDefaultHeader(_SelfMenuDetails);
                            }
                            Indicatior.Close();
                        }
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw ex;
            }
        }
       
        void cw_Closed(object sender, EventArgs e)
        {
            if ((bool)((OPDModule.Forms.PatientSearch)sender).DialogResult)
            {
                //  GetSelectedPatientDetails();
            }
            else
            {
                if (ForUser)
                {
                    txtMRNo1.Text = string.Empty;
                }
                else
                {
                    txtMRNo.Text = string.Empty;
                }
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }
       
        bool IsViewClick;        
        #region SelectionChanged
        private void cmbAdmListClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAdmListClass.SelectedItem != null)
            {
                if (((MasterListItem)cmbAdmListClass.SelectedItem).ID > 0)
                {
                    FillWard();
                }
            }
        }
        #endregion

        #region Checked Events

        private void chkAllPatients_Checked(object sender, RoutedEventArgs e)
        {
            if (chkCancelledAdmissions != null)
                chkCancelledAdmissions.IsChecked = false;
            if (chkCurrentAdmittedList != null)
                chkCurrentAdmittedList.IsChecked = false;
            if (chkMedicoLegalCase != null)
                chkMedicoLegalCase.IsChecked = false;
            if (chkNonPresence != null)
                chkNonPresence.IsChecked = false;
        }

        private void chkCurrentAdmittedList_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAllPatients != null)
                chkAllPatients.IsChecked = false;
            if (chkCancelledAdmissions != null)
                chkCancelledAdmissions.IsChecked = false;
            if (chkNonPresence != null)
                chkNonPresence.IsChecked = false;
            if (chkMedicoLegalCase != null)
                chkMedicoLegalCase.IsChecked = false;
        }

        private void chkCancelledAdmissions_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAllPatients != null)
                chkAllPatients.IsChecked = false;
            if (chkCurrentAdmittedList != null)
                chkCurrentAdmittedList.IsChecked = false;
            if (chkNonPresence != null)
                chkNonPresence.IsChecked = false;
            if (chkMedicoLegalCase != null)
                chkMedicoLegalCase.IsChecked = false;
        }

        private void chkNonPresence_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAllPatients != null)
                chkAllPatients.IsChecked = false;
            if (chkCurrentAdmittedList != null)
                chkCurrentAdmittedList.IsChecked = false;
            if (chkCancelledAdmissions != null)
                chkCancelledAdmissions.IsChecked = false;
            if (chkMedicoLegalCase != null)
                chkMedicoLegalCase.IsChecked = false;


        }

        private void chkMedicoLegalCase_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAllPatients != null)
                chkAllPatients.IsChecked = false;
            if (chkCurrentAdmittedList != null)
                chkCurrentAdmittedList.IsChecked = false;
            if (chkCancelledAdmissions != null)
                chkCancelledAdmissions.IsChecked = false;
            if (chkNonPresence != null)
                chkNonPresence.IsChecked = false;
        }

        #endregion

        private void Admission_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            if (dgPatientAdmissionList.SelectedItem != null)
            {
                if (((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).IsCancelled)
                {
                    cmdConsent.IsEnabled = false;
                    cmdViewConsent.IsEnabled = false;
                    cmdReferEntity.IsEnabled = false;
                    cmdAdviseDischarge.IsEnabled = false;
                    cmdCancelAdmission.IsEnabled = false;
                    cmdMedicoLegalCase.IsEnabled = false;                    
                }
                else
                {
                    cmdConsent.IsEnabled = true;
                    cmdViewConsent.IsEnabled = false;
                    cmdReferEntity.IsEnabled = true;
                    cmdMedicoLegalCase.IsEnabled = true;
                    cmdCancelAdmission.IsEnabled = ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).IsCancelAdmEnable;
                    cmdAdviseDischarge.IsEnabled = ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).IsAdviseEnable;
                }
                if (((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).IsNonPresence.Equals(true) && !((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).IsCancelled)
                {
                    cmdConfirmAdmission.IsEnabled = true;
                }
                else
                {
                    cmdConfirmAdmission.IsEnabled = false;
                }
                if (((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).IsMedicoLegalCase.Equals(true) && !((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).IsCancelled)
                {
                    cmdMedicoLegalCase.IsEnabled = true;
                }
                else
                {
                    cmdMedicoLegalCase.IsEnabled = false;
                }
            }
            else
            {
                cmdConsent.IsEnabled = false;
                cmdViewConsent.IsEnabled = false;
                cmdReferEntity.IsEnabled = false;
                cmdCancelAdmission.IsEnabled = false;
                cmdAdviseDischarge.IsEnabled = false;
            }

            if (((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem) != null) //Added by AJ Date 12/12/2016
            {
                if (((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).IsCancelled)
                {
                    cmdDischargeApproval.IsEnabled = false;
                }
                else
                {
                    checkBill();
                }
            }
            ((IApplicationConfiguration)App.Current).SelectedIPDPatient = ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem);           
        }

        List<SponsorCompanyDetails> SponsorCompanyDetail = new List<SponsorCompanyDetails>();

        

        private void hlEMR_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {

        }



        private void cmdConsent_Click(object sender, RoutedEventArgs e)
        {
            isFromReferEntity = false;
            if (dgPatientAdmissionList.ItemsSource != null)
            {
                if (dgPatientAdmissionList.SelectedItem != null)
                {
                    clsIPDAdmissionVO selectedPatient = ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem);
                    FrmConsent win = new FrmConsent(selectedPatient);
                    win.Show();
                }
            }
        }

        bool isForViewConsent = false;        
        private void txtMRNo_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox _txtBox = (TextBox)sender;
            _txtBox.SelectAll();
        }

        private void hlIPDBill_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientAdmissionList.SelectedItem != null)
            {
                clsIPDAdmissionVO objAdmission = dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.classID = objAdmission.classID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.ClassID = objAdmission.classID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.BillingToBedCategoryID = objAdmission.classID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionTypeID = objAdmission.AdmissionTypeID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.BillCount = objAdmission.BillCount;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = objAdmission.PatientId;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = objAdmission.PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = objAdmission.PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID = objAdmission.ID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID = objAdmission.UnitId;
                ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = objAdmission.CompanyID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID = objAdmission.GenderID;
            }
            frmIPDBills _IPDBills = new frmIPDBills();
            _IPDBills.IsPatientExist = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Patient Bill";
            ((IApplicationConfiguration)App.Current).OpenMainContent(_IPDBills);
        }

        private void hlDischarge_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientAdmissionList.SelectedItem != null)
            {
                clsIPDAdmissionVO objAdmission = dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.classID = objAdmission.classID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.ClassID = objAdmission.classID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.BillingToBedCategoryID = objAdmission.classID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionTypeID = objAdmission.AdmissionTypeID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.BillCount = objAdmission.BillCount;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = objAdmission.PatientId;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = objAdmission.PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = objAdmission.PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID = objAdmission.ID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID = objAdmission.UnitId;
                ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = objAdmission.CompanyID;
                #region Validation for Discharge Summary Commented By Bhushanp
                //clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO BizAction = new clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO();
                //BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID; ;
                //BizAction.AdmID = objAdmission.ID;
                //BizAction.AdmUnitID = objAdmission.UnitId;
                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //client.ProcessCompleted += (s, arg) =>
                //{
                //    if (arg.Error == null)
                //    {
                //        if (arg.Result != null)
                //        {
                //            if (((clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO)arg.Result).DischargeSummaryDetails != null)
                //            {
                //                frmPatientDischarge _PatientDischargeObject = new frmPatientDischarge();
                //                UserControl rootPage = Application.Current.RootVisual as UserControl;
                //                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //                mElement.Text = "Patient Discharge";
                //                ((IApplicationConfiguration)App.Current).OpenMainContent(_PatientDischargeObject);
                //            }
                //            else
                //            {
                //                MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                  new MessageBoxControl.MessageBoxChildWindow("", "You Have not able to Discharge Without Creating Discharge Summary.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgW1.Show();
                //            }
                //        }
                //        else
                //        {
                //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //              new MessageBoxControl.MessageBoxChildWindow("", "You Have not able to Discharge Without Creating Discharge Summary.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //            msgW1.Show();
                //        }
                //    }
                //    else
                //    {
                //        MessageBoxControl.MessageBoxChildWindow msgW1 =
                //          new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //        msgW1.Show();
                //    }

                //};

                //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //client.CloseAsync();
                #endregion                
                clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
                BizAction.RequestTypeID = 1;
                BizAction.IsRequest = true;
                BizAction.PatientID = objAdmission.PatientId;
                BizAction.PatientUnitID = objAdmission.PatientUnitID;
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
                        List<clsBillVO> objList =  new List<clsBillVO>();
                        objList = result.List;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        if (result.List != null)
                        {
                            var objList1 = (from item in objList
                                       where item.IsFreezed == false && item.Opd_Ipd_External_Id == objAdmission.AdmID &&
                                       item.Opd_Ipd_External_UnitId == objAdmission.AdmissionUnitID
                                       select item).ToList();
                            var objChkList = (from item in objList
                                       where item.Opd_Ipd_External_Id == objAdmission.AdmID &&
                                       item.Opd_Ipd_External_UnitId == objAdmission.AdmissionUnitID
                                       select item).ToList();
                            if (objList1 != null && objList1.Count > 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", "You are not able to Discharge Without final the Bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                            else if (objChkList != null && objChkList.Count == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "You are not able to Discharge Without creating the Bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                            else
                            {
                                chkDischargeApproval = true;
                                GetApprovalDepartmentList();
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
        bool isFromReferEntity = false;
        ChildWindow appoitmentForm = null;       

        private void txtMRNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetAdmissionList();
            }
        }
        private void dgPatientAdmissionList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsIPDAdmissionVO item = (clsIPDAdmissionVO)e.Row.DataContext;
            if (item.IsDischarge == true && item.IsCancelled == false)  // for Discharge Patient
                e.Row.Background = new SolidColorBrush(Colors.Magenta);
            else if (item.IsCancelled == true && item.IsDischarge == true)   // for Cancel Addmission Patient
                e.Row.Background = new SolidColorBrush(Colors.Blue);
            else if (item.IsClosed == true)
                e.Row.Background = new SolidColorBrush(Colors.Red);
            else
                e.Row.Background = null;
        }

        private void cmdDischargeApproval_Click(object sender, RoutedEventArgs e)
        {
           
                if (dgPatientAdmissionList.SelectedItem != null)
                {
                    clsIPDAdmissionVO objAdmission = dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO;
                    ((IApplicationConfiguration)App.Current).SelectedIPDPatient.classID = objAdmission.classID;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.ClassID = objAdmission.classID;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.BillingToBedCategoryID = objAdmission.classID;
                    ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionTypeID = objAdmission.AdmissionTypeID;
                    ((IApplicationConfiguration)App.Current).SelectedIPDPatient.BillCount = objAdmission.BillCount;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = objAdmission.PatientId;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = objAdmission.PatientUnitID;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = objAdmission.PatientUnitID;
                    ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID = objAdmission.ID;
                    ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID = objAdmission.UnitId;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = objAdmission.CompanyID;

                    frmDischargeApprovalDepartment _PatientDischargeObject = new frmDischargeApprovalDepartment();
                    _PatientDischargeObject.lblPatientName1.Text = objAdmission.PatientName;
                    _PatientDischargeObject.lblPatientGender1.Text = objAdmission.GenderName;
                    _PatientDischargeObject.lblAdmissionDate1.Text = objAdmission.AdmissionDate.ToString();
                    _PatientDischargeObject.lblAdmissionNo1.Text = objAdmission.IPDNO;
                    _PatientDischargeObject.lblMrNo1.Text = objAdmission.MRNo;
                    _PatientDischargeObject.OnCancelButton_Click += new RoutedEventHandler(dischargeApproval_OnCancelButton_Click);
                    _PatientDischargeObject.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Please Select a Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
          
            
        }

        public void GetApprovalDepartmentList()
        {
            if (dgPatientAdmissionList.SelectedItem != null)
            {
                clsGetListOfAdviseDischargeForApprovalBizActionVO BizAction = new clsGetListOfAdviseDischargeForApprovalBizActionVO();
                BizAction.AddAdviseDetails = new clsDiischargeApprovedByDepartmentVO();
                BizAction.AddAdviseDetails.AdmissionId = ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).ID;
                BizAction.AddAdviseDetails.AdmissionUnitID = ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                       clsGetListOfAdviseDischargeForApprovalBizActionVO result =  arg.Result as clsGetListOfAdviseDischargeForApprovalBizActionVO;
                       if (result.AddAdviseList.Count > 0)
                       {
                           cmdDischargeApproval.IsEnabled = false;
                           if (chkDischargeApproval)
                           {
                               foreach (var item in result.AddAdviseList)
                               {
                                   if (!item.ApprovalStatus)
                                   {
                                       chkDischargeApproval = false;
                                       break;
                                   }
                               }
                               if (chkDischargeApproval)
                               {
                                   chkDischargeApproval = false;
                                   frmPatientDischarge _PatientDischargeObject = new frmPatientDischarge();
                                   UserControl rootPage = Application.Current.RootVisual as UserControl;
                                   TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                   mElement.Text = "Patient Discharge";
                                   ((IApplicationConfiguration)App.Current).OpenMainContent(_PatientDischargeObject);
                               }
                               else
                               {
                                   MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("", "You are not able to Discharge Without getting a Approval Clearence.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                   msgW1.Show();
                                   chkDischargeApproval = false;
                               }
                           }
                       }
                       else
                       {
                           cmdDischargeApproval.IsEnabled = true;
                           if (chkDischargeApproval)
                           {
                               chkDischargeApproval = false;
                               frmPatientDischarge _PatientDischargeObject = new frmPatientDischarge();
                               UserControl rootPage = Application.Current.RootVisual as UserControl;
                               TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                               mElement.Text = "Patient Discharge";
                               ((IApplicationConfiguration)App.Current).OpenMainContent(_PatientDischargeObject);
                           }
                       }
                    }
                    else
                    {
                        cmdDischargeApproval.IsEnabled = true;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }


        void dischargeApproval_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            GetApprovalDepartmentList();
        }


        public void checkBill()
        {
            if (dgPatientAdmissionList.SelectedItem != null)
            {
              // Indicatior.Show();
                clsIPDAdmissionVO objAdmission = dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO;
                clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
                BizAction.RequestTypeID = 1;
                BizAction.IsRequest = true;
                BizAction.PatientID = objAdmission.PatientId;
                BizAction.PatientUnitID = objAdmission.PatientUnitID;
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
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        if (result.List != null)
                        {
                            objList = (from item in objList
                                       where item.Opd_Ipd_External_Id == objAdmission.ID &&
                                       item.Opd_Ipd_External_UnitId == objAdmission.UnitId
                                       select item).ToList();

                            if (objList != null && objList.Count > 0)
                            {
                                cmdDischargeApproval.IsEnabled = true;
                                GetApprovalDepartmentList();
                            }
                            else
                            {
                                cmdDischargeApproval.IsEnabled = false;
                            }
                        }
                        else
                        {
                            cmdDischargeApproval.IsEnabled = false;
                        }
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();                
            }
        }

        private void cmdPatientAdmissionReport_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientAdmissionList.SelectedItem != null)
            {
                string msgText = "";
                msgText = "Are You Sure \n You Want To Print Patient Admission Report?";
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                  new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                msgW2.OnMessageBoxClosed += (MessageBoxResult re) =>
                {
                    if (re == MessageBoxResult.Yes)
                    {
                        if (((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).ID > 0 && ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).UnitId > 0)
                        {
                            string URL = "../Reports/IPD/PatientAdmissionSheet.aspx?AdmissionID=" + ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).ID + "&AdmissionUnitID=" + ((clsIPDAdmissionVO)dgPatientAdmissionList.SelectedItem).UnitId;
                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                        }
                    }
                };
                msgW2.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        //Added by AJ Date 5/1/2017
        private void hlMConsumption_Click(object sender, RoutedEventArgs e)
        {

            if (dgPatientAdmissionList.SelectedItem != null)
            {
                clsIPDAdmissionVO objAdmission = dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO;
                ISCancel = objAdmission.IsCancelled;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = objAdmission.PatientId;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = objAdmission.PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = objAdmission.PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID = objAdmission.ID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID = objAdmission.UnitId;
                ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = objAdmission.CompanyID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientSourceID = objAdmission.PatientSourceID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.DoctorName = objAdmission.DoctorName;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.TariffID = objAdmission.TariffID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.DoctorID = objAdmission.DoctorID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientCategoryID = objAdmission.PatientCategoryID; 
            }

            if (ISCancel == false)
            {
                PatientAgainstMaterialConsumptionDetails MaterialConsumption = new PatientAgainstMaterialConsumptionDetails();
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Patient Details";
                ((IApplicationConfiguration)App.Current).OpenMainContent(MaterialConsumption);
            }

        }


        private void hlIPDPharmacyBill_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientAdmissionList.SelectedItem != null)
            {
                clsIPDAdmissionVO objAdmission = dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.classID = objAdmission.classID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.ClassID = objAdmission.classID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.BillingToBedCategoryID = objAdmission.classID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionTypeID = objAdmission.AdmissionTypeID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.BillCount = objAdmission.BillCount;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = objAdmission.PatientId;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = objAdmission.PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = objAdmission.PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID = objAdmission.ID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID = objAdmission.UnitId;
                ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = objAdmission.CompanyID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.TariffID = objAdmission.TariffID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.CompanyID = objAdmission.CompanyID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientSourceID = objAdmission.PatientSourceID;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.DoctorName = objAdmission.DoctorName;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient.DoctorID = objAdmission.DoctorID;  
            }
            frmPharmacyBill _IPDBills = new frmPharmacyBill();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Pharmacy Bill" + ':' + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName + '-' + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;
            ((IApplicationConfiguration)App.Current).OpenMainContent(_IPDBills);
        }

        //added by vikrant
        private void hlIPDEMR_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientAdmissionList.SelectedItem != null)
            {
                clsIPDAdmissionVO objAdmission = dgPatientAdmissionList.SelectedItem as clsIPDAdmissionVO;
                ((IApplicationConfiguration)App.Current).SelectedIPDPatient = objAdmission;

                ModuleName = "EMR";
                Action = "EMR.frmIPDEMR";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
        }
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        void c_OpenReadCompleted_maleSemenSurvival(object sender, OpenReadCompletedEventArgs e)
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
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                TextBlock Header = (TextBlock)rootPage.FindName("SampleHeader");
                Header.Text = "Patient Queue List";
                ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
                PART_MaximizeToggle.IsChecked = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
