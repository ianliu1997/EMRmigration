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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Billing;
using OPDModule;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.ValueObjects.Administration.UserRights;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Log;


namespace PalashDynamics.IPD
{
    public partial class IPDRefundServices : UserControl, IPassData
    {
        #region Variable Declaration
        SwivelAnimation _flip = null;
        public clsBillVO SelectedBill { get; set; }
        ObservableCollection<clsChargeVO> ChargeList { get; set; }
        ObservableCollection<clsChargeVO> ApprovalChargeList { get; set; }      // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        public List<clsBillVO> ServiceItemSource { get; set; }
        long BillID = 0;
        long RefundID = 0;
        //Added By Bhushanp 22032017 For Refund Against Bill
        bool _IsAgainstBill = false;
        long UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
        WaitIndicator Indicatior = null;
        double finalRefund = 0;
        double RefundAmountNew = 0;
        bool FirstApprovalRight = false;       // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        bool SecondApprovalRight = false;      // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        int ClickedFlag = 0;
        bool IsEditMode = false;
        // For Activity Log By Umesh
        int lineNumber = 0;                                                                 // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        List<LogInfo> LogInfoList = new List<LogInfo>();  // For the Activity Log List      // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        LogInfo LogInformation;                                                             // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        Guid objGUID;                                                                       // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        bool IsAuditTrail = false;                                                          // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        // Activity log end
        long _ApprovalRequestID { get; set; }
        long _ApprovalRequestUnitID { get; set; }

        List<MasterListItem> RefundReasonList = new List<MasterListItem>();                 // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        //clsUserRightsVO objUser = new clsUserRightsVO();                                    // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        public bool IsRefundSerAfterSampleCollection = false;                               // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD

        #endregion

        public void PassDataToForm(clsBillVO value, bool IsfromApprovalRequestWindow)
        {
            SelectedBill = value;
            // IsFromAdmissionList = true;
            IsFromRequestApproval = IsfromApprovalRequestWindow;
        }

        #region Load event

        public IPDRefundServices()
        {
            InitializeComponent();
            FillReasonOfRefund();                                                               // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID, false);      // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD

            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            IsAuditTrail = ((IApplicationConfiguration)App.Current).CurrentUser.IsAuditTrail;// By Umesh For Enable/Disable Audit Trail     // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD

            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
            this.DataContext = new clsBillVO()
            {
                UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                //DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                //DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID,
            };

        }

        private void IPDRefundServices_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");

            FillUnitList();
            FillBillSearchList();

            if (IsFromRequestApproval == true)
            {
                InitialiseForm();
                SetCommandButtonState("Save");
                _flip.Invoke(RotationType.Forward);
                if (SelectedBill != null)
                {
                    if (SelectedBill.IsFreezed == true)
                    {
                        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, true, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId);
                    }
                    else
                    {
                        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId);
                    }
                }
            }
        }

        #endregion

        #region "Paging"


        public PagedSortableCollectionView<clsBillVO> DataList { get; private set; }

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
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillBillSearchList();

        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 




        #endregion

        public bool IsFromRequestApproval = false;

        #region Clear Data

        private void InitialiseForm()
        {

            ChargeList = new ObservableCollection<clsChargeVO>();
            ApprovalChargeList = new ObservableCollection<clsChargeVO>();   // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
            dgCharges.ItemsSource = ChargeList;
            dgCharges.Focus();
            dgCharges.UpdateLayout();
            txtClinicalTotal.Text = "";
            txtClinicalConcession.Text = "";
            txtClinicalNetAmount.Text = "";
            txtTotalRefundAmount.Text = "";
            if (IsFromRequestApproval != true)
                SelectedBill = null;
        }

        #endregion

        #region FillDatatgrid

        private void FillReasonOfRefund()                                   // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ReasonOfRefundMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    RefundReasonList = objList;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillUnitList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objList;
                    cmbUnit.SelectedItem = objList[0];

                }
                if (this.DataContext != null)
                {
                    cmbUnit.SelectedValue = ((clsBillVO)this.DataContext).UnitID;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillBillSearchList()
        {
            //checkFreezColumn = false;
            indicator.Show();

            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();

            BizAction.RequestTypeID = (int)RequestType.Refund;
            BizAction.IsRequest = true;
            //BizAction.IsActive = true;
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
            BizAction.IsFreeze = true;

            // Added By CDS
            BizAction.FromRefund = true;
            BizAction.IsIPDBillList = true;
            BizAction.Opd_Ipd_External = 1;

            BizAction.BillType = BillTypes.Clinical;
            BizAction.BillNO = txtBillNO.Text;
            //BizAction.OPDNO = txtOPDNO.Text;
            BizAction.IPDNO = txtOPDNO.Text;
            BizAction.FirstName = txtFirstName.Text;
            BizAction.MiddleName = txtMiddleName.Text;
            BizAction.LastName = txtLastName.Text;
            if (cmbUnit.SelectedItem != null)
            {
                BizAction.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;
                    DataList.Clear();
                    if (result.List != null)
                    {

                        foreach (var item in result.List)
                        {
                            item.BillPaymentType = (BillPaymentTypes)(1);
                            DataList.Add(item);
                        }
                    }
                    dgBillList.ItemsSource = null;
                    dgBillList.ItemsSource = DataList;

                    dgDataPager.Source = null;
                    dgDataPager.PageSize = BizAction.MaximumRows;
                    dgDataPager.Source = DataList;

                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillChargeList(long PBillID, long pUnitID, bool pIsBilled, long pVisitID, long pPatientUnitID)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();

                BizAction.ID = 0;

                BizAction.Opd_Ipd_External_Id = pVisitID;
                BizAction.Opd_Ipd_External_UnitId = pPatientUnitID;
                BizAction.IsBilled = pIsBilled;
                BizAction.BillID = PBillID;
                BizAction.UnitID = pUnitID; // ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.RequestTypeID = (int)RequestType.Refund;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetChargeListBizActionVO)arg.Result).List != null)
                        {
                            List<clsChargeVO> objList;// = new List<clsChargeVO>();
                            objList = ((clsGetChargeListBizActionVO)arg.Result).List;
                            _IsAgainstBill = false;
                            dgCharges.IsEnabled = true;
                            foreach (var item in objList)
                            {
                                item.IsEnable = true;
                                if (item.IsCancelled == true)
                                {
                                    item.SelectCharge = true;
                                    item.IsEnable = false;
                                }
                                else if (item.IsSetForApproval == true)
                                {
                                    item.IsEnable = false;
                                }
                                if (item.ApprovalRequestID > 0 && item.SelectCharge == true && item.IsCancelled == false)     // Added on 27Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                {
                                    _ApprovalRequestID = item.ApprovalRequestID;
                                    _ApprovalRequestUnitID = item.ApprovalRequestUnitID;
                                }
                                ChargeList.Add(item);
                            }
                            foreach (var item in ChargeList)
                            {
                                if (ChargeList.Where(t => t.ApprovalRequestID == item.ApprovalRequestID)
                                              .Where(t => t.RefundID > 0).Any())
                                {
                                    item.IsChargeApproved = true;
                                }
                                else if (ChargeList.Where(t => t.ApprovalRequestID == item.ApprovalRequestID)
                                              .Where(t => t.RefundID == 0).Any())
                                {
                                    item.IsChargeApproved = false;
                                }
                                item.RefundReason = RefundReasonList;       // Begin : Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                if (item.RequestRefundReasonID > 0)
                                    item.SelectedRequestRefundReason = RefundReasonList.FirstOrDefault(z => z.ID == item.RequestRefundReasonID);
                                else
                                    item.SelectedRequestRefundReason = RefundReasonList.FirstOrDefault(z => z.ID == 0);

                                if (item.ApprovalRefundReasonID > 0)
                                    item.SelectedApprovalRefundReason = RefundReasonList.FirstOrDefault(z => z.ID == item.ApprovalRefundReasonID);
                                else
                                    item.SelectedApprovalRefundReason = RefundReasonList.FirstOrDefault(z => z.ID == 0);

                                if (item.RefundReasonID > 0)
                                    item.SelectedRefundReason = RefundReasonList.FirstOrDefault(z => z.ID == item.RefundReasonID);
                                else
                                    item.SelectedRefundReason = RefundReasonList.FirstOrDefault(z => z.ID == 0);    // End : Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                            }

                            if (SelectedBill != null)
                                if (SelectedBill.IsApproved == true && SelectedBill.LevelID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID && SelectedBill.RequestTypeID == (int)RequestType.Refund)
                                {
                                    if (ChargeList.Where(t => t.IsChargeApproved == false && t.ApprovalRequestID != 0 && t.RefundID == 0).Any())
                                    {
                                        txtTotalRefundAmount.IsEnabled = false;
                                        cmdSave.IsEnabled = true;
                                        cmdSendApprove.IsEnabled = false;
                                    }
                                    else
                                    {
                                        cmdSave.IsEnabled = false;
                                        cmdSendApprove.IsEnabled = true;
                                        dgCharges.Columns[3].Visibility = Visibility.Collapsed;
                                        dgCharges.Columns[4].Visibility = Visibility.Visible;
                                        dgCharges.Columns[4].IsReadOnly = false;
                                    }
                                }
                                else if (IsFromRequestApproval == true)
                                {
                                    cmdSave.IsEnabled = false;
                                    cmdSendApprove.IsEnabled = false;
                                }
                                else
                                {
                                    cmdSave.IsEnabled = false;
                                    cmdSendApprove.IsEnabled = true;
                                }
                            if (IsFromRequestApproval != true && SelectedBill.IsRequestSend == true && SelectedBill.IsApproved == false && SelectedBill.RequestTypeID == (int)RequestType.Refund)
                            {
                                cmdSave.IsEnabled = false;
                                cmdSendApprove.IsEnabled = false;
                            }
                            if (IsFromRequestApproval == true)//Added By YK
                            {
                                txtTotalRefundAmount.IsEnabled = true; //Added By YK
                            }
                            foreach (var item in objList)
                            {
                                if (item.ApprovalRequestID > 0)
                                {
                                    rdbAgainstBill.IsEnabled = false;
                                    rdbAgainstServices.IsEnabled = false;
                                    txtRefundedAmount.Text = "0";
                                    txtTotalRefundAmount.Text = Convert.ToString(item.RefundAmount);
                                    if (item.IsAgainstBill)
                                    {
                                        txtRefundedAmount.Text = Convert.ToString(item.RefundedAmount);
                                        if (item.IsRefund)
                                        {
                                            txtTotalRefundAmount.IsEnabled = true;
                                            txtTotalRefundAmount.Text = "0";
                                            cmdSendApprove.IsEnabled = true;
                                            cmdSave.IsEnabled = false;
                                        }
                                        item.SelectCharge = true;
                                        item.IsEnable = false;
                                        _IsAgainstBill = true;
                                        rdbAgainstBill.IsChecked = true;
                                        dgCharges.IsEnabled = false;
                                    }
                                    else
                                    {
                                        txtTotalRefundAmount.IsEnabled = false; //Added By Bhushanp 19052017
                                        dgCharges.IsEnabled = true;
                                    }
                                }
                            }
                            CalculateRefund();
                            dgCharges.ItemsSource = null;
                            dgCharges.ItemsSource = ChargeList;
                            dgCharges.Focus();
                            dgCharges.UpdateLayout();
                            CalculateClinicalSummary();
                            //Added By Bhushanp 22052017
                            if (ChargeList.Where(t => t.ApprovalStatus == true && t.RefundID == 0).Any())
                            {
                                //dgCharges.Columns[3].Visibility = Visibility.Visible;         // commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                //dgCharges.Columns[4].Visibility = Visibility.Collapsed;       // commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                            }
                            else if (ChargeList.Where(t => t.ApprovalStatus == false && t.RefundID == 0 && t.IsSendForApproval == true).Any())
                            {
                                dgCharges.Columns[3].Visibility = Visibility.Collapsed;
                                dgCharges.Columns[4].Visibility = Visibility.Visible;
                                //dgCharges.Columns[4].IsReadOnly = true;                       // commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                            }
                            else
                            {
                                dgCharges.Columns[3].Visibility = Visibility.Collapsed;
                                dgCharges.Columns[4].Visibility = Visibility.Visible;
                                dgCharges.Columns[4].IsReadOnly = false;
                            }
                        }
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                Indicatior.Close();
            }
        }

        #endregion


        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            //SetCommandButtonState("New");
            //_flip.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsFromRequestApproval == true)
            {
                ModuleName = "OPDModule";
                Action = "CIMS.Forms.frmApprovalRequests";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted_1);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                InitialiseForm();
                RefundAmountNew = 0;
                SetCommandButtonState("Cancel");
                _flip.Invoke(RotationType.Backward);
            }
        }
        void c2_OpenReadCompleted_1(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
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
                ((IApplicationConfiguration)App.Current).SelectedPatient = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                ClickedFlag += 1;
                if (ClickedFlag == 1)
                {
                    if (ChargeList.Where(t => t.SelectCharge == true && t.IsSendForApproval == false).Any() || ChargeList.Where(t => t.SelectCharge == true && t.IsSendForApproval == true && t.ApprovalStatus == false).Any())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow("", "Selected Service is not approved yet", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ClickedFlag = 0;
                    }
                    else
                    {
                        IPDRefundServicesPaymentDetails Win = new IPDRefundServicesPaymentDetails();
                        Win.txtPaidAmount.IsReadOnly = true;
                        Win.OnSaveButton_Click += new RoutedEventHandler(RefundWin_OnSaveButton_Click);
                        Win.OnCancelButton_Click += new RoutedEventHandler(RefundWin_OnCancelButton_Click);

                        ///////////Added By YK/////////////////
                        if (_IsAgainstBill)
                        {
                            Win.txtPaidAmount.Text = txtTotalRefundAmount.Text;
                        }
                        else
                        {
                            RefundAmountNew = 0;
                            getrefundForSave();
                            Win.txtPaidAmount.Text = RefundAmountNew.ToString(); ;
                        }

                        ////////////////END//////////////////////

                        //  Win.txtPaidAmount.Text = txtTotalRefundAmount.Text;

                        Win.Show();
                    }
                }
            }
        }

        void RefundWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((IPDRefundServicesPaymentDetails)sender).DialogResult == true)
            {
                clsRefundVO RefundObj = new clsRefundVO();

                if (((IPDRefundServicesPaymentDetails)sender).Payment != null)
                {
                    RefundObj.PaymentDetails = ((IPDRefundServicesPaymentDetails)sender).Payment;
                }
                RefundObj.Date = DateTime.Now;

                RefundObj.BillID = BillID;

                RefundObj.Amount = Convert.ToDouble(((IPDRefundServicesPaymentDetails)sender).txtPaidAmount.Text);

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    RefundObj.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                else
                    RefundObj.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;

                clsAddRefundBizActionVO BizAction = new clsAddRefundBizActionVO();

                BizAction.Details = RefundObj;

                BizAction.IsRefundToAdvance = ((IPDRefundServicesPaymentDetails)sender).IsRefundToAdvance;     // Refund to Advance 21042017

                BizAction.RefundToAdvancePatientID = ((clsBillVO)dgBillList.SelectedItem).PatientID;        // Refund to Advance 21042017
                BizAction.RefundToAdvancePatientUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;       // Refund to Advance 21042017
                //Added By Bhushanp 01062017
                RefundObj.ApprovalRequestID = _ApprovalRequestID;
                RefundObj.ApprovalRequestUnitID = _ApprovalRequestUnitID;
                if (IsAuditTrail)   // By Umesh For Audit Trail
                {
                    LogInformation = new LogInfo();
                    LogInformation.guid = objGUID;
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 27 : Data From Payment Window To Refund Services  " + "\r\n"
                                            + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                            + "Refund Date : " + Convert.ToString(RefundObj.Date) + " "
                                            + "Bill ID : " + Convert.ToString(RefundObj.BillID) + " "
                                            + "Costing Division ID : " + Convert.ToString(RefundObj.CostingDivisionID) + " "
                                            + "Refund Amount : " + Convert.ToString(RefundObj.PaymentDetails.RefundAmount) + " "
                                            + "Payment Mode ID : " + Convert.ToString(RefundObj.PaymentDetails.PaymentDetails[0].PaymentModeID) + " "
                                            + "Bank ID : " + Convert.ToString(RefundObj.PaymentDetails.PaymentDetails[0].BankID) + " "
                                            + "Number : " + Convert.ToString(RefundObj.PaymentDetails.PaymentDetails[0].Number) + " "
                                            + "Date : " + Convert.ToString(RefundObj.PaymentDetails.PaymentDetails[0].Date) + " "
                                            ;
                    LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                    LogInfoList.Add(LogInformation);
                }
                BizAction.LogInfoList = LogInfoList;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (sa, arg) =>
                {
                    ClickedFlag = 0;
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsAddRefundBizActionVO)arg.Result).Details != null)
                            {
                                clsAddRefundServiceChargeBizActionVO BizActionObj = new clsAddRefundServiceChargeBizActionVO();
                                BizActionObj.ChargeList = new List<clsChargeVO>();
                                DateTime Date = DateTime.Now;
                                RefundID = ((clsAddRefundBizActionVO)arg.Result).Details.ID;
                                if (SelectedBill.Date.Date == DateTime.Now.Date.Date)
                                    BizActionObj.IsUpdate = true;
                                else
                                    BizActionObj.IsUpdate = false;

                                long PRID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;

                                if (rdbAgainstBill.IsChecked == false)
                                {
                                    foreach (var item in ChargeList)
                                    {
                                        if (item.SelectCharge == true && item.IsCancelled == false)
                                        {
                                            item.RefundID = RefundID;
                                            item.IsCancelled = true;
                                            item.CancelledDate = Date;
                                            item.RefundAmount = item.NetAmount;
                                            BizActionObj.ChargeList.Add(item);
                                        }
                                    }
                                    //foreach (var item in ChargeList)
                                    //{
                                    //    if (item.SelectCharge == true && item.IsCancelled == false)
                                    //    {
                                    //        if (item.IsEnable == true)    //Added By Yk to spcific refund id in T_Charge for selected service refund 29 Mar 2017
                                    //        {
                                    //            item.RefundID = RefundID;
                                    //        }

                                    //        item.IsCancelled = true;
                                    //        item.CancelledDate = Date;
                                    //        item.RefundAmount = item.NetAmount;
                                    //        BizActionObj.ChargeList.Add(item);
                                    //    }
                                    //}
                                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");

                                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                    client1.ProcessCompleted += (a, args) =>
                                    {
                                        if (args.Error == null)
                                        {
                                            if (args.Result != null)
                                            {
                                                if (((clsAddRefundServiceChargeBizActionVO)args.Result != null))
                                                {
                                                    UnitID = ((clsAddRefundServiceChargeBizActionVO)args.Result).UnitID;
                                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                        new MessageBoxControl.MessageBoxChildWindow("", "Refund details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                                    msgW1.OnMessageBoxClosed += (re) =>
                                                    {
                                                        if (re == MessageBoxResult.OK)
                                                        {
                                                            //if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstServices)     // Commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                                            if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstServices && ((clsBillVO)dgBillList.SelectedItem).LevelID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)   // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                                            {
                                                                PrintBill(RefundID, UnitID, PRID);
                                                            }
                                                            //else if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstBill)    // Commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                                            else if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstBill && ((clsBillVO)dgBillList.SelectedItem).LevelID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)  // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                                            {
                                                                PrintReceipt(RefundID, PRID);
                                                            }
                                                        }
                                                    };
                                                    msgW1.Show();
                                                    txtTotalRefundAmount.Text = "0";
                                                    SetCommandButtonState("Load");
                                                    FillBillSearchList();
                                                    _flip.Invoke(RotationType.Backward);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Refund details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW1.Show();
                                        }
                                    };
                                    client1.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    client1.CloseAsync();
                                }
                                else
                                {
                                    #region New By Bhushanp
                                    double tempRefundAmount = Convert.ToDouble(((IPDRefundServicesPaymentDetails)sender).txtPaidAmount.Text);
                                    var results2 = from r in ChargeList
                                                   orderby r.NetAmount ascending
                                                   select r;


                                    foreach (var item in results2)
                                    {
                                        if (item.SelectCharge == true)
                                        {
                                            // item.IsCancelled = true;
                                            // item.CancelledDate = Date;
                                            if (tempRefundAmount > 0)
                                            {
                                                double RefundAmount = (item.NetAmount - item.TotalRefundAmount);
                                                if (RefundAmount < tempRefundAmount)
                                                {
                                                    item.RefundAmount = item.TotalRefundAmount + RefundAmount;
                                                    tempRefundAmount = tempRefundAmount - RefundAmount;
                                                    //   item.RefundID = RefundID;
                                                }
                                                else
                                                {
                                                    item.RefundAmount = item.TotalRefundAmount + (tempRefundAmount);
                                                    tempRefundAmount = 0;
                                                    // item.RefundID = RefundID;
                                                }
                                                //   item.RefundAmount = item.NetAmount;
                                            }
                                            else
                                            {
                                                item.RefundAmount = 0;
                                            }
                                            BizActionObj.ChargeList.Add(item);
                                        }
                                    }
                                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");

                                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                    client1.ProcessCompleted += (a, args) =>
                                    {
                                        if (args.Error == null)
                                        {
                                            if (args.Result != null)
                                            {
                                                if (((clsAddRefundServiceChargeBizActionVO)args.Result != null))
                                                {
                                                    UnitID = ((clsAddRefundServiceChargeBizActionVO)args.Result).UnitID;
                                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                        new MessageBoxControl.MessageBoxChildWindow("", "Refund details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                                    msgW1.OnMessageBoxClosed += (re) =>
                                                    {
                                                        if (re == MessageBoxResult.OK)
                                                        {
                                                            //if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstServices)   // Commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                                            if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstServices && ((clsBillVO)dgBillList.SelectedItem).LevelID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)   // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                                            {
                                                                PrintBill(RefundID, UnitID, PRID);
                                                            }
                                                            //else if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstBill)   // Commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                                            else if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstBill && ((clsBillVO)dgBillList.SelectedItem).LevelID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)   // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                                                            {
                                                                PrintReceipt(RefundID, PRID);
                                                            }
                                                        }
                                                    };
                                                    msgW1.Show();
                                                    txtTotalRefundAmount.Text = "0";
                                                    SetCommandButtonState("Load");
                                                    FillBillSearchList();
                                                    _flip.Invoke(RotationType.Backward);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Refund details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW1.Show();
                                        }
                                    };
                                    client1.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    client1.CloseAsync();
                                    #endregion
                                }
                            }
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }

        void RefundWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag = 0;
        }


        private void hlbView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgBillList.SelectedItem != null)
                {
                    InitialiseForm();
                    SetCommandButtonState("Save");
                    txtTotalRefundAmount.Text = "0";
                    txtRefundedAmount.Text = "0";
                    SelectedBill = new clsBillVO();
                    SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                    BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                    rdbAgainstServices.IsChecked = true;
                    dgCharges.IsEnabled = true;             // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                    if (((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
                    {
                        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, true, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId);
                    }
                    else
                    {
                        FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId);
                    }
                    _flip.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

        private void CalculateClinicalSummary()
        {
            double clinicalTotal, clinicalConcession, clinicalNetAmount;

            clinicalTotal = clinicalConcession = clinicalNetAmount = 0;
            for (int i = 0; i < ChargeList.Count; i++)
            {
                clinicalTotal += (ChargeList[i].TotalAmount);
                clinicalConcession += ChargeList[i].Concession;
                clinicalNetAmount += ChargeList[i].NetAmount;

            }
            txtClinicalTotal.Text = clinicalTotal.ToString();
            txtClinicalConcession.Text = clinicalConcession.ToString();
            txtClinicalNetAmount.Text = clinicalNetAmount.ToString();


        }

        private void CancelService_Click(object sender, RoutedEventArgs e)
        {
            if (dgCharges.SelectedItem != null)
            {
                clsChargeVO objVO = (clsChargeVO)dgCharges.SelectedItem;
                if (objVO.IsResultEntry == false)//Added By Yogesh k IsResultEntry,Isupload 2 7 16
                {
                    if (objVO.Isupload == false)
                    {
                        if (((clsChargeVO)dgCharges.SelectedItem).NetAmount <= 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Zero Bill Service Cannot Be Refund", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD_1.Show();
                            ((CheckBox)sender).IsChecked = false;
                        }
                        else if (objVO.IsSampleCollected == false)  //else                       
                        {
                            double RefundAmount = 0;
                            if (txtTotalRefundAmount.Text != null && txtTotalRefundAmount.Text.Length > 0)
                            {
                                RefundAmount = Convert.ToDouble(txtTotalRefundAmount.Text);
                            }
                            if (((CheckBox)sender).IsChecked == true && ((clsChargeVO)(dgCharges.SelectedItem)).IsCancelled == false)
                            {
                                RefundAmount = RefundAmount + ((clsChargeVO)(dgCharges.SelectedItem)).NetAmount;
                            }
                            else
                                RefundAmount = RefundAmount - ((clsChargeVO)(dgCharges.SelectedItem)).NetAmount;

                            txtTotalRefundAmount.Text = RefundAmount.ToString();
                        }
                        else if (objUser == null ? false : objUser.IsRefundSerAfterSampleCollection && objVO.IsSampleCollected)   // user wise      // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                        {
                            double RefundAmount = 0;
                            if (txtTotalRefundAmount.Text != null && txtTotalRefundAmount.Text.Length > 0)
                            {
                                RefundAmount = Convert.ToDouble(txtTotalRefundAmount.Text);
                            }
                            if (((CheckBox)sender).IsChecked == true && ((clsChargeVO)(dgCharges.SelectedItem)).IsCancelled == false)
                            {
                                RefundAmount = RefundAmount + ((clsChargeVO)(dgCharges.SelectedItem)).NetAmount;
                            }
                            else
                                RefundAmount = RefundAmount - ((clsChargeVO)(dgCharges.SelectedItem)).NetAmount;

                            txtTotalRefundAmount.Text = RefundAmount.ToString();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Service not refund Sample Collection done for this Test", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD_1.Show();
                            ((CheckBox)sender).IsChecked = false;
                        }

                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Service not refund Upload Report done for this Test", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD_1.Show();
                        ((CheckBox)sender).IsChecked = false;
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Service not refund Result entry done for this Test", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD_1.Show();
                    ((CheckBox)sender).IsChecked = false;
                }
            }
            #region Commented By Bhushanp 24042017
            //if (dgCharges.SelectedItem != null)
            //{
            //    double RefundAmount = 0;
            //    if (txtTotalRefundAmount.Text != null && txtTotalRefundAmount.Text.Length > 0)
            //    {
            //        RefundAmount = Convert.ToDouble(txtTotalRefundAmount.Text);
            //    }



            //    if (((CheckBox)sender).IsChecked == true)
            //    {

            //            RefundAmount = RefundAmount + ((clsChargeVO)(dgCharges.SelectedItem)).NetAmount;

            //    }
            //    else
            //        RefundAmount = RefundAmount - ((clsChargeVO)(dgCharges.SelectedItem)).NetAmount;

            //    txtTotalRefundAmount.Text = RefundAmount.ToString();
            //}
            #endregion
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {
                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
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
                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (res)
            {
                dgDataPager.PageIndex = 0;
                FillBillSearchList();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintBill(long iRefundID, long iUnitID, long PrintID)
        {
            if (iRefundID > 0)
            {
                long UnitID = iUnitID;
                DateTime PrintDate = DateTime.Now;
                string URL = "../Reports/OPD/RefundServicesBill.aspx?RefundID=" + iRefundID + "&UnitID=" + UnitID + "&PrintDate=" + PrintDate + "&PrintFomatID=" + PrintID + "&IsFromIPD=" + 1;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();    // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":

                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    cmdSendApprove.IsEnabled = false;
                    cmdApprove.IsEnabled = false;
                    //Added By Bhushanp 21032017
                    rdbAgainstBill.IsEnabled = false;
                    rdbAgainstServices.IsEnabled = false;
                    break;
                case "New":

                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdSendApprove.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdApprove.IsEnabled = false;
                    break;

                case "Save":

                    cmdNew.IsEnabled = false;
                    //cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    if (IsFromRequestApproval == true)
                    {
                        cmdApprove.IsEnabled = true;
                        cmdSendApprove.IsEnabled = false;
                        cmdSave.IsEnabled = false;
                    }
                    else
                    {
                        // cmdSendApprove.IsEnabled = true;
                        //cmdSave.IsEnabled = true;
                    }
                    //Added By Bhushanp 21032017
                    rdbAgainstBill.IsEnabled = true;
                    rdbAgainstServices.IsEnabled = true;
                    break;

                case "Cancel":

                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    cmdSendApprove.IsEnabled = false;
                    cmdApprove.IsEnabled = false;
                    //Added By Bhushanp 21032017
                    rdbAgainstBill.IsEnabled = false;
                    rdbAgainstServices.IsEnabled = false;
                    break;

                default:
                    break;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do  nothing
                }
                else
                    cmdSave.IsEnabled = false;
            }
        }

        #endregion

        private void PrintReceipt(long iRefundID, long PrintID)
        {
            if (iRefundID > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                //string URL = "../Reports/OPD/RefundAgainstBill.aspx?RefundID=" + iRefundID + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID;
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                DateTime PrintDate = DateTime.Now;
                string URL = "../Reports/OPD/RefundServicesBillNew.aspx?RefundID=" + iRefundID + "&UnitID=" + UnitID + "&PrintDate=" + PrintDate.ToString("dd-MMM-yyyy") + "&IsFromIPD=" + 1;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
        //By Anjali.....................................
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        clsUserRightsVO objUser = new clsUserRightsVO();
        public enum RequestType
        {
            Concession = 1,
            Refund = 2
        }
        private void cmdSendApprove_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                if (ChargeList.Where(s => s.SelectCharge == true && s.IsCancelled == false).Any() || (ChargeList.Where(s => s.SelectCharge == true).Any() && rdbAgainstBill.IsChecked == true))
                {
                    if (IsAuditTrail)   // By Umesh For Audit Trail     // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 21 : Send For Approve  "
                                                + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                + "Bill ID : " + Convert.ToString(SelectedBill.ID) + " "
                                                + "Bill Unit ID : " + Convert.ToString(SelectedBill.ID) + " "
                                                + "Is OPD Bill : " + Convert.ToString("true") + " "
                                                + "RequestType : " + Convert.ToString(Enum.GetName(typeof(RequestType), RequestType.Refund)) + " "
                                                ;
                        foreach (clsChargeVO item in ChargeList.ToList())
                        {
                            LogInformation.Message = LogInformation.Message
                                                        + "Cancel Service : " + Convert.ToString(item.SelectCharge) + " "
                                                        + "Approval Remark : " + Convert.ToString(item.ApprovalRemark) + " "
                                                        + "Request Remark : " + Convert.ToString(item.ApprovalRequestRemark) + " "
                                                        + "Service Code : " + Convert.ToString(item.ServiceId) + " "
                                                        + "Service Name : " + Convert.ToString(item.ServiceName) + " "
                                                        + "\r\n"
                                                ;
                        }
                        LogInformation.Message = LogInformation.Message
                                                        + "Total : " + Convert.ToString(txtClinicalTotal.Text) + " "
                                                        + "Concession : " + Convert.ToString(txtClinicalConcession.Text) + " "
                                                        + "Total Net Amount : " + Convert.ToString(txtClinicalNetAmount.Text) + " "
                                                        + "Total Refund Amount : " + Convert.ToString(txtTotalRefundAmount.Text) + " ".Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                    }
                    var t = from c in ChargeList
                            where c.IsSendForApproval == true && c.SelectCharge == true
                            select c as clsChargeVO;
                    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Are You Sure You Want To Send For Approval?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                    msgWD_1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please select Service For Request", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
        }

        private void msgWD_1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SendForApproval();
            }

        }
        private void SendForApproval()
        {
            try
            {
                clsSendRequestForApprovalVO BizAction = new clsSendRequestForApprovalVO();
                BizAction.IsRefundRequest = true;
                BizAction.Details = new clsRefundVO();
                BizAction.Details.BillID = SelectedBill.ID;
                BizAction.Details.BillUnitID = SelectedBill.UnitID;
                BizAction.Details.IsOPDBill = false;
                BizAction.Details.RequestTypeID = (int)RequestType.Refund;
                BizAction.Details.RequestType = Enum.GetName(typeof(RequestType), RequestType.Refund);
                //Added By Bhushanp 24032017
                if (rdbAgainstBill.IsChecked == true)
                {
                    BizAction.IsAgainstBill = true;
                    BizAction.RefundAmount = Convert.ToDouble(txtTotalRefundAmount.Text);
                }

                if (BizAction.ChargeList == null)
                    BizAction.ChargeList = new List<clsChargeVO>();

                foreach (var item in ChargeList)
                {
                    if (rdbAgainstBill.IsChecked == true)
                    {
                        if (item.SelectCharge == true)
                            BizAction.ChargeList.Add(item);
                    }
                    else
                    {
                        if (item.SelectCharge == true && item.IsCancelled == false)
                            BizAction.ChargeList.Add(item);
                    }
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (sa, args) =>
                {
                    ClickedFlag = 0;
                    if (args.Error == null)
                    {
                        if (args.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Approval Request Send successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            txtTotalRefundAmount.Text = "0";
                            SetCommandButtonState("Load");
                            FillBillSearchList();
                            _flip.Invoke(RotationType.Backward);
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while sending Request details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                if (ChargeList.Where(s => s.SelectCharge == true && s.IsCancelled == false).Any() || (ChargeList.Where(s => s.SelectCharge == true).Any() && rdbAgainstBill.IsChecked == true))
                {
                    //GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);           // Commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                    GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID, true);       // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please select Service For Approval", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
        }

        int ApproveFlag = 0;                                                // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        //public void GetUserRights(long UserId)                            // Commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        public void GetUserRights(long UserId, bool IsFromApproveClick)     // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;

                        //if (objUser.IpdAuthLvl != null)                               // Commented on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                        if (IsFromApproveClick && objUser.IpdAuthLvl != null)           // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                        {
                            if (objUser.IpdAuthLvl == SelectedBill.LevelID)           
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill is already approved with " + objUser.AuthLevelForRefundOPD + " rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                                msgWindowUpdate.Show();
                            }
                            else
                            {
                                ApproveFlag += 1;
                                if (ApproveFlag == 1)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve the request with " + objUser.AuthLevelForRefundOPD + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                                    msgWindowUpdate.Show();
                                }
                            }
                        }

                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void msgWindowUpdate_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ApproveRefund();
            }
            else
            {
                ApproveFlag = 0;
            }
        }
        private void ApproveRefund()
        {
            try
            {
                clsApproveSendRequestVO BizAction = new clsApproveSendRequestVO();
                BizAction.IsRefundRequest = true;
                BizAction.IsForApproval = true;
                if (BizAction.ApprovalChargeList == null)
                    BizAction.ApprovalChargeList = new List<clsChargeVO>();
                BizAction.ApprovalRequestID = SelectedBill.ApprovalRequestID;
                BizAction.ApprovalRequestUnitID = SelectedBill.ApprovalRequestUnitID;
                BizAction.ApprovedByID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizAction.ApprovedByName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                if (objUser.IpdAuthLvl == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
                    BizAction.IsApproved = true;
                else
                    BizAction.IsApproved = false;

                BizAction.RefundAmount = Convert.ToDouble(txtTotalRefundAmount.Text);// Added y Bhushanp 24032017
                BizAction.LevelID = objUser.IpdAuthLvl;
                BizAction.ApprovedDateTime = DateTime.Now;

                foreach (var item in ChargeList)
                {
                    if (rdbAgainstBill.IsChecked == true)
                    {
                        if (item.IsSendForApproval)
                            BizAction.ApprovalChargeList.Add(item);
                    }
                    else
                    {
                        if (item.IsSendForApproval && item.IsCancelled == false)
                            BizAction.ApprovalChargeList.Add(item);
                    }
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (sa, args) =>
                {
                    ClickedFlag = 0;
                    if (args.Error == null)
                    {
                        if (objUser.IpdAuthLvl == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Approved Sucessfully, you can refund service", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.Show();


                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Approved Sucessfully, still higher level approval is required", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.Show();

                        }
                        if (IsFromRequestApproval == true)
                        {
                            ModuleName = "OPDModule";
                            Action = "CIMS.Forms.frmApprovalRequests";
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            WebClient c2 = new WebClient();
                            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while Approving request.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    ApproveFlag = 0;        // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {


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

                //if (Menu != null && Menu.Parent == "Surrogacy")
                //    ((IInitiateCIMS)myData).Initiate("Surrogacy");
                //else
                //    ((IInitiateCIMS)myData).Initiate("REG");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);



            }
            catch (Exception ex)
            {
                throw;
            }



        }
        private void CalculateRefund()
        {
            if (!_IsAgainstBill)
            {
                double RefundAmount = 0;
                foreach (var t in ChargeList)
                {
                    if (t.SelectCharge == true && t.IsCancelled == false)
                    {
                        RefundAmount = RefundAmount + t.NetAmount;
                    }
                }
                txtTotalRefundAmount.Text = RefundAmount.ToString();
            }
        }
        private void dgCharges_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (IsFromRequestApproval == true)
            {
                if (((clsChargeVO)(e.Row.DataContext)).IsSendForApproval == false || ((clsChargeVO)(e.Row.DataContext)).RefundID > 0)
                {
                    e.Row.IsEnabled = false;
                }
                //dgCharges.Columns[2].IsReadOnly = false;
                //dgCharges.Columns[3].IsReadOnly = true;
                dgCharges.Columns[2].IsReadOnly = false;
                dgCharges.Columns[3].Visibility = Visibility.Collapsed;
                dgCharges.Columns[4].Visibility = Visibility.Visible;
                dgCharges.Columns[4].IsReadOnly = true;
            }
            else
            {
                // dgCharges.Columns[2].IsReadOnly = true;
                //if(((clsBillVO)dgBillList.SelectedItem).IsApproved==true)
                // dgCharges.Columns[3].IsReadOnly = false;
                //else
                //    dgCharges.Columns[3].IsReadOnly = true;

                dgCharges.Columns[2].IsReadOnly = true;
                if (((clsBillVO)dgBillList.SelectedItem).IsApproved == true && ((clsChargeVO)(e.Row.DataContext)).RefundID == 0)
                {
                    //dgCharges.Columns[3].Visibility = Visibility.Visible;         // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                    //dgCharges.Columns[4].Visibility = Visibility.Collapsed;       // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                }
                else
                {
                    dgCharges.Columns[3].Visibility = Visibility.Collapsed;
                    dgCharges.Columns[4].Visibility = Visibility.Visible;
                    //dgCharges.Columns[4].IsReadOnly = false;                      // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
                }


                if (((clsChargeVO)(e.Row.DataContext)).SelectCharge == false && ((clsBillVO)dgBillList.SelectedItem).IsApproved == true && ((clsBillVO)dgBillList.SelectedItem).RequestTypeID == (int)RequestType.Refund && ((clsChargeVO)(e.Row.DataContext)).IsSendForApproval == true)
                {
                    if (((clsChargeVO)(e.Row.DataContext)).IsChargeApproved == true && ((clsChargeVO)(e.Row.DataContext)).RefundID > 0)
                        e.Row.IsEnabled = false;
                    else if (((clsChargeVO)(e.Row.DataContext)).IsChargeApproved == true && ((clsChargeVO)(e.Row.DataContext)).RefundID == 0)
                        e.Row.IsEnabled = true;
                }
                if ((((clsBillVO)dgBillList.SelectedItem).IsApproved == true && ((clsBillVO)dgBillList.SelectedItem).RequestTypeID == (int)RequestType.Refund && ((clsChargeVO)(e.Row.DataContext)).RefundID == 0)) //|| (((clsBillVO)dgBillList.SelectedItem).IsApproved == true && ((clsBillVO)dgBillList.SelectedItem).RequestTypeID == (int)RequestType.Refund && ((clsChargeVO)(e.Row.DataContext)).RefundID > 0 &&  ((clsChargeVO)(e.Row.DataContext)).IsSendForApproval == true)
                {
                    if (((clsChargeVO)(e.Row.DataContext)).IsSendForApproval == false)
                        e.Row.IsEnabled = true;
                    //else if(((clsChargeVO)(e.Row.DataContext)).ApprovalStatus==false && ((clsChargeVO)(e.Row.DataContext)).IsSendForApproval==true)
                    //    e.Row.IsEnabled = false;
                    //else
                    //    e.Row.IsEnabled = false;
                }
                if (((clsBillVO)dgBillList.SelectedItem).IsApproved == true && ((clsBillVO)dgBillList.SelectedItem).RequestTypeID == (int)RequestType.Refund && ((clsChargeVO)(e.Row.DataContext)).RefundID > 0)
                {
                    e.Row.IsEnabled = false;
                }

            }
        }

        private double getrefundForSave()
        {
            //double RefundAmountNew = 0;
            if (!_IsAgainstBill)
            {

                foreach (var t in ChargeList)
                {
                    if (t.IsEnable == true && t.SelectCharge == true)
                    {
                        RefundAmountNew = RefundAmountNew + t.NetAmount;
                    }
                }

            }
            return RefundAmountNew;
        }

        private void rdbAgainstBill_Click(object sender, RoutedEventArgs e)
        {
            if (rdbAgainstBill.IsChecked == true)
            {
                foreach (clsChargeVO item in ChargeList)
                {
                    item.SelectCharge = true;
                }
                dgCharges.UpdateLayout();
                dgCharges.ItemsSource = null;
                dgCharges.ItemsSource = ChargeList;
                dgCharges.Columns[0].IsReadOnly = true;
                dgCharges.Columns[1].IsReadOnly = true;
                dgCharges.Columns[2].IsReadOnly = true;
                dgCharges.IsEnabled = false;
                txtTotalRefundAmount.Text = "0";
                txtTotalRefundAmount.IsEnabled = true;
            }
            else
            {
                foreach (clsChargeVO item in ChargeList)
                {
                    item.SelectCharge = false;
                }
                dgCharges.UpdateLayout();
                dgCharges.ItemsSource = null;
                dgCharges.ItemsSource = ChargeList;
                dgCharges.IsEnabled = true;
                dgCharges.Columns[0].IsReadOnly = false;
                dgCharges.Columns[1].IsReadOnly = false;
                dgCharges.Columns[2].IsReadOnly = false;
                txtTotalRefundAmount.Text = "0";
                txtTotalRefundAmount.IsEnabled = false;
            }
        }

        private bool CheckValidation()
        {
            bool IsValid = false;
            decimal NetAmount = 0;
            decimal RefundAmount = 0;
            NetAmount = Convert.ToDecimal(txtClinicalNetAmount.Text);
            RefundAmount = Convert.ToDecimal(txtTotalRefundAmount.Text);
            if (RefundAmount <= 0)
            {
                IsValid = false;
                txtTotalRefundAmount.SetValidation("Please Enter Refund Amount.");
                txtTotalRefundAmount.RaiseValidationError();
                txtTotalRefundAmount.Focus();
                return IsValid;
            }
            else if ((RefundAmount + Convert.ToDecimal(txtRefundedAmount.Text)) > NetAmount)
            {
                IsValid = false;
                txtTotalRefundAmount.SetValidation("Please Enter Valid Refund Amount.");
                txtTotalRefundAmount.RaiseValidationError();
                txtTotalRefundAmount.Focus();
                return IsValid;
            }
            else
            {
                txtTotalRefundAmount.ClearValidationError();
                IsValid = true;
            }

            if (!this.IsFromRequestApproval && ChargeList != null && ChargeList.Count > 0)      // Added on 26Feb2019 for Approval Remark Dropdown for Refund Services Flow in IPD
            {
                if (ChargeList.Where(z => z.SelectCharge && (z.SelectedRequestRefundReason.ID == null || z.SelectedRequestRefundReason.ID == 0)).Any())
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow("", "Request Remark is Mandatory.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    IsValid = false;
                    return IsValid;
                }
            }

            return IsValid;
        }

        #region GST Details added by CDS
        //private void hlbServiceTaxAmt_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (dgCharges.SelectedItem != null && (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList != null)
        //        {

        //            frmServiceTaxTransactionDetails frmTaxDetails = new frmServiceTaxTransactionDetails();
        //            frmTaxDetails.objChargeVO = dgCharges.SelectedItem as clsChargeVO;
        //            frmTaxDetails.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
        //            frmTaxDetails.Show();
        //        }
        //        else if (dgCharges.SelectedItem != null && (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList == null)
        //        {
        //           FillTaxChargeList((dgCharges.SelectedItem as clsChargeVO).ID, (dgCharges.SelectedItem as clsChargeVO).UnitID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //private void FillTaxChargeList(long ChargeID, long ChargeUnitID)
        //{
        //    try
        //    {
        //        clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();
        //        BizAction.ID = ChargeID;
        //        BizAction.UnitID = ChargeUnitID;
        //        BizAction.IsForTaxationDetails = true;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                clsChargeVO ChargeVO = new clsChargeVO();
        //                if (((clsGetChargeListBizActionVO)arg.Result).ChargeVO.ChargeTaxDetailsList != null)
        //                {
        //                    List<clsChargeTaxDetailsVO> objList = ((clsGetChargeListBizActionVO)arg.Result).ChargeVO.ChargeTaxDetailsList;

        //                    if (dgCharges.SelectedItem != null)
        //                    {
        //                        ChargeVO.TotalServiceTaxAmount = (dgCharges.SelectedItem as clsChargeVO).TotalServiceTaxAmount;
        //                        ChargeVO.ServiceName = (dgCharges.SelectedItem as clsChargeVO).ServiceName;
        //                    }
        //                    ChargeVO.ChargeTaxDetailsList = objList.ToList();
        //                }

        //                if (ChargeVO != null && ChargeVO.ChargeTaxDetailsList != null)
        //                {
        //                    frmServiceTaxTransactionDetails frmTaxDetails = new frmServiceTaxTransactionDetails();
        //                    frmTaxDetails.objChargeVO = ChargeVO;
        //                    frmTaxDetails.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
        //                    frmTaxDetails.Show();
        //                }
        //                else
        //                {
        //                    MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Tax is not applied for this service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                    msgW1.Show();
        //                }
        //            }
        //        };
        //        Client.ProcessAsync(BizAction, null);
        //        Client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        Indicatior.Close();
        //    }
        //}

        //void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //public List<clsChargeTaxDetailsVO> GetServiceTaxList(List<clsServiceTaxVO> objList)
        //{
        //    try
        //    {

        //        List<clsChargeTaxDetailsVO> ServiceTaxList = new List<clsChargeTaxDetailsVO>();
        //        foreach (var item in ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList.ToList())
        //        {
        //            clsChargeTaxDetailsVO ChargeTaxDetailsVO = new clsChargeTaxDetailsVO();
        //            ChargeTaxDetailsVO.ServiceId = item.ServiceId;
        //            ChargeTaxDetailsVO.TariffId = item.TariffId;
        //            ChargeTaxDetailsVO.ClassId = item.ClassId;
        //            ChargeTaxDetailsVO.TaxID = item.TaxID;
        //            ChargeTaxDetailsVO.Percentage = item.Percentage;
        //            ChargeTaxDetailsVO.TaxType = item.TaxType;
        //            ChargeTaxDetailsVO.IsTaxLimitApplicable = item.IsTaxLimitApplicable;
        //            ChargeTaxDetailsVO.TaxLimit = item.TaxLimit;
        //            ChargeTaxDetailsVO.ServiceName = item.ServiceName;
        //            ChargeTaxDetailsVO.TaxName = item.TaxName;
        //            ChargeTaxDetailsVO.Quantity = 1;
        //            ChargeTaxDetailsVO.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
        //            ChargeTaxDetailsVO.TotalAmount = 0;
        //            ServiceTaxList.Add(ChargeTaxDetailsVO);
        //        }

        //        double TotalTaxamount = 0;
        //        if (((clsChargeVO)dgCharges.SelectedItem) != null && ServiceTaxList != null && ServiceTaxList.Count() > 0)
        //        {
        //            foreach (var item in ServiceTaxList.ToList())
        //            {
        //                if (item.IsTaxLimitApplicable)
        //                {
        //                    if (((clsChargeVO)dgCharges.SelectedItem).Rate > Convert.ToDouble(item.TaxLimit))
        //                    {
        //                        ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxPercent = Convert.ToDouble(item.Percentage);
        //                        if (item.TaxType == 2) //Exclusive
        //                            TotalTaxamount += ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount;

        //                        item.ServiceTaxPercent = Convert.ToDouble(item.Percentage);
        //                        item.ServiceTaxAmount = ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount;
        //                        item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
        //                    }
        //                    else
        //                    {
        //                        item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
        //                        item.ServiceTaxAmount = 0;
        //                    }
        //                }
        //                else
        //                {
        //                    ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxPercent = Convert.ToDouble(item.Percentage);
        //                    if (item.TaxType == 2) //Exclusive
        //                        TotalTaxamount += ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount;

        //                    item.ServiceTaxPercent = Convert.ToDouble(item.Percentage);
        //                    item.ServiceTaxAmount = ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount;
        //                    item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
        //                }
        //            }
        //            ((clsChargeVO)dgCharges.SelectedItem).TotalServiceTaxAmount = TotalTaxamount;
        //            ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount = TotalTaxamount;

        //            CalculateClinicalSummary();
        //        }
        //        return ServiceTaxList;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}

        private void hlbServiceTaxAmt_Click(object sender, RoutedEventArgs e)
        {
            if (dgCharges.SelectedItem != null)
            {

                FillTaxChargeList((dgCharges.SelectedItem as clsChargeVO).ID, (dgCharges.SelectedItem as clsChargeVO).UnitID);

            }
        }

        void win_OnTotalTaxSaveButton_Click(object sender, RoutedEventArgs e)
        {
            frmServiceTaxTransactionDetails Itemswin = (frmServiceTaxTransactionDetails)sender;
            (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList = (Itemswin as frmServiceTaxTransactionDetails).objChargeVO.ChargeTaxDetailsList;
            (dgCharges.SelectedItem as clsChargeVO).TotalServiceTaxAmount = (Itemswin as frmServiceTaxTransactionDetails).objChargeVO.TotalServiceTaxAmount;
            CalculateClinicalSummary();
        }

        private void FillTaxChargeList(long ChargeID, long ChargeUnitID)
        {
            try
            {
                clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();
                BizAction.ID = ChargeID;
                BizAction.UnitID = ChargeUnitID;
                BizAction.IsForTaxationDetails = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsChargeVO ChargeVO = new clsChargeVO();
                        if (((clsGetChargeListBizActionVO)arg.Result).ChargeVO.ChargeTaxDetailsList != null)
                        {
                            List<clsChargeTaxDetailsVO> objList = ((clsGetChargeListBizActionVO)arg.Result).ChargeVO.ChargeTaxDetailsList;

                            //if (dgCharges.SelectedItem != null)
                            //{
                            //    ChargeVO.TotalServiceTaxAmount = (dgCharges.SelectedItem as clsChargeVO).TotalServiceTaxAmount;
                            //    ChargeVO.ServiceName = (dgCharges.SelectedItem as clsChargeVO).ServiceName;
                            //}
                            ChargeVO = (dgCharges.SelectedItem as clsChargeVO);
                            ChargeVO.ChargeTaxDetailsList = objList.ToList();
                        }

                        if (ChargeVO != null && ChargeVO.ChargeTaxDetailsList != null)
                        {
                            frmServiceTaxTransactionDetails frmTaxDetails = new frmServiceTaxTransactionDetails();
                            frmTaxDetails.objChargeVO = ChargeVO;
                            //frmTaxDetails.IsTaxReadOnly = false;
                            //frmTaxDetails.IsBillFreezed = IsEditMode == false ? false : (dgBillList.SelectedItem as clsBillVO).IsFreezed;
                            frmTaxDetails.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                            frmTaxDetails.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Tax is not applied for this service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                };
                Client.ProcessAsync(BizAction, null);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Indicatior.Close();
            }
        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            frmServiceTaxTransactionDetails Itemswin = (frmServiceTaxTransactionDetails)sender;
            (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList = (Itemswin as frmServiceTaxTransactionDetails).objChargeVO.ChargeTaxDetailsList;
            (dgCharges.SelectedItem as clsChargeVO).TotalServiceTaxAmount = (Itemswin as frmServiceTaxTransactionDetails).objChargeVO.TotalServiceTaxAmount;
            CalculateClinicalSummary();
        }



        #endregion

        //Begin:: Added by AniketK on 15JAN2018
        private void btnReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                frmIPDRefundReceiptList receiptWin = new frmIPDRefundReceiptList();
                receiptWin.ID = ((clsBillVO)dgBillList.SelectedItem).ID;
                receiptWin.UnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                receiptWin.BillPaymentType = ((clsBillVO)dgBillList.SelectedItem).BillPaymentType;
                receiptWin.Show();

            }
        }
        //End:: Added by AniketK on 15JAN2018
    }
}
