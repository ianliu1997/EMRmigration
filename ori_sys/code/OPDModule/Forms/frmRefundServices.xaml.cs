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
using System.Text;
using PalashDynamics.ValueObjects.Administration.UserRights;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Log;
using PalashDynamics.ValueObjects.Administration;

namespace CIMS.Forms
{
    public partial class frmRefundServices : UserControl
    {
        #region Variable Declaration
        SwivelAnimation _flip = null;
        public clsBillVO SelectedBill { get; set; }
        ObservableCollection<clsChargeVO> ChargeList { get; set; }
        ObservableCollection<clsChargeVO> ApprovalChargeList { get; set; }
        public List<clsBillVO> ServiceItemSource { get; set; }
        long BillID = 0;
        long RefundID = 0;
        long UnitID = 0;
        WaitIndicator Indicatior = null;
        int ClickedFlag = 0;
        bool FirstApprovalRight = false;
        bool SecondApprovalRight = false;
        //Added By Bhushanp 22032017 For Refund Against Bill
        bool _IsAgainstBill = false;
        // For Activity Log By Umesh
        int lineNumber = 0;
        List<LogInfo> LogInfoList = new List<LogInfo>();  // For the Activity Log List
        LogInfo LogInformation;
        Guid objGUID;
        bool IsAuditTrail = false;
        long _ApprovalRequestID { get; set; }
        long _ApprovalRequestUnitID { get; set; }
        // Activity log end

        List<MasterListItem> RefundReasonList = new List<MasterListItem>();
        clsUserRightsVO objUser = new clsUserRightsVO();
        public bool IsRefundSerAfterSampleCollection = false;

        #endregion

        public enum RequestType
        {
            Concession = 1,
            Refund = 2
        }
        public bool IsFromRequestApproval = false;
        #region Load event
        public frmRefundServices()
        {
            InitializeComponent();
            FillReasonOfRefund();
            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID, false);

            _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            IsAuditTrail = ((IApplicationConfiguration)App.Current).CurrentUser.IsAuditTrail;// By Umesh For Enable/Disable Audit Trail

            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
        }

        private void RefundServices_Loaded(object sender, RoutedEventArgs e)
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

        #region Clear Data
        private void InitialiseForm()
        {

            ChargeList = new ObservableCollection<clsChargeVO>();
            ApprovalChargeList = new ObservableCollection<clsChargeVO>();
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

        private void FillReasonOfRefund()
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
                    if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        var res = from r in objList
                                  where r.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                                  select r;
                        cmbUnit.SelectedItem = ((MasterListItem)res.First());
                        cmbUnit.IsEnabled = false;
                    }
                    else
                        cmbUnit.SelectedItem = objList[0];
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
            //BizAction.IsActive = true;
            BizAction.RequestTypeID = (int)RequestType.Refund;
            BizAction.IsRequest = true;
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
            BizAction.IsFreeze = true;

            // Added By CDS
            BizAction.FromRefund = true;

            BizAction.BillType = BillTypes.Clinical;
            BizAction.BillNO = txtBillNO.Text;
            BizAction.OPDNO = txtOPDNO.Text;
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
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

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

                    if (IsAuditTrail)   // By Umesh For Audit Trail
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 19 : Bill List " + "\r\n"
                                                + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                + "From Date : " + Convert.ToString(BizAction.FromDate) + " "
                                                + "To Date : " + Convert.ToString(BizAction.ToDate) + " "
                                                + "Request type ID : " + Convert.ToString(BizAction.RequestTypeID) + " "
                                                + "Is Request : " + Convert.ToString(BizAction.IsRequest) + " "
                                                + "Is Freeze : " + Convert.ToString(BizAction.IsFreeze) + " "
                                                + "FromRefund : " + Convert.ToString(BizAction.FromRefund) + " "
                                                + "Bill Type : " + Convert.ToString(BizAction.BillType) + " "
                                                + "Bill NO : " + Convert.ToString(BizAction.BillNO) + " "
                                                + "OPD NO : " + Convert.ToString(BizAction.OPDNO) + " "
                                                + "First Name : " + Convert.ToString(BizAction.FirstName) + " "
                                                + "Middle Name : " + Convert.ToString(BizAction.MiddleName) + " "
                                                + "Last Name : " + Convert.ToString(BizAction.LastName) + " "
                                                + "UnitID : " + Convert.ToString(BizAction.UnitID) + " "
                                                + "Costing Division ID : " + BizAction.CostingDivisionID + " "
                                                ;
                        foreach (clsBillVO item in DataList)
                        {
                            LogInformation.Message = LogInformation.Message + "Bill list"
                                                   + " , Date :" + Convert.ToString(item.Date) + " "
                                                   + " , Bill No :" + Convert.ToString(item.BillNo) + " "
                                                   + " , OPD NO : " + Convert.ToString(item.Opd_Ipd_External_No) + " "
                                                   + " , Name :" + Convert.ToString(item.PatientName) + " "
                                                   + " , Total Bill Amount : " + Convert.ToString(item.TotalBillAmount) + " "
                                                   + " , Total Concession Amount : " + Convert.ToString(item.TotalConcessionAmount) + " "
                                                   + " , Net Bill Amount : " + Convert.ToString(item.NetBillAmount) + " "
                                                   + " , Freezed: " + Convert.ToString(item.IsFreezed) + " "
                                                   + " , Total Paid Amount :" + Convert.ToString(item.PaidAmount) + " "
                                                   + " , Total Refund Amount :" + Convert.ToString(item.TotalRefund) + " "
                                                   + " , Bill Payment Type :" + Convert.ToString(item.BillPaymentTypeStr) + " "
                                                   + " , Authorised Person :" + Convert.ToString(item.AuthorityPerson) + " "
                                                   + "\r\n"
                                                   ;
                        }
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                    }

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
                            List<clsChargeVO> objList;
                            objList = ((clsGetChargeListBizActionVO)arg.Result).List;
                            if (IsAuditTrail && IsFromRequestApproval)   // By Umesh For Audit Trail
                            {
                                LogInformation = new LogInfo();
                                LogInformation.guid = objGUID;
                                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                LogInformation.TimeStamp = DateTime.Now;
                                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                LogInformation.Message = " 24 : Service List On Approval Requests  " + "\r\n"
                                                        + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                        + "Bill Unit Id : " + Convert.ToString(BizAction.UnitID) + " "
                                                        + "Patient Unit ID : " + Convert.ToString(BizAction.Opd_Ipd_External_UnitId) + " "
                                                        + "Visit ID : " + Convert.ToString(BizAction.Opd_Ipd_External_Id) + " "
                                                        + "Is Billed : " + Convert.ToString(BizAction.IsBilled) + " "
                                                        + "Bill ID : " + Convert.ToString(BizAction.BillID) + " "
                                                        + "Request Type ID : " + Convert.ToString(BizAction.RequestTypeID) + " "
                                                        ;
                            }
                            _IsAgainstBill = false;
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
                                _ApprovalRequestID = item.ApprovalRequestID;
                                _ApprovalRequestUnitID = item.ApprovalRequestUnitID;
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
                                item.RefundReason = RefundReasonList;
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
                                    item.SelectedRefundReason = RefundReasonList.FirstOrDefault(z => z.ID == 0);
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
                                    txtTotalRefundAmount.IsEnabled = true;
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
                                //dgCharges.Columns[3].Visibility = Visibility.Visible; // date 19032018 
                                //dgCharges.Columns[4].Visibility = Visibility.Collapsed;
                            }
                            else if (ChargeList.Where(t => t.ApprovalStatus == false && t.RefundID == 0 && t.IsSendForApproval == true).Any())
                            {
                                dgCharges.Columns[3].Visibility = Visibility.Collapsed;
                                dgCharges.Columns[4].Visibility = Visibility.Visible;
                                //dgCharges.Columns[4].IsReadOnly = true;
                            }
                            else
                            {
                                dgCharges.Columns[3].Visibility = Visibility.Collapsed;
                                dgCharges.Columns[4].Visibility = Visibility.Visible;
                                dgCharges.Columns[4].IsReadOnly = false;
                            }

                            //}
                            Indicatior.Close();
                        }
                    }
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


        //by Anjali.............
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
        #endregion
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
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
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

            }
            else
            {
                InitialiseForm();
                SetCommandButtonState("Cancel");
                _flip.Invoke(RotationType.Backward);
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
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {

                if (ChargeList.Where(t => t.SelectCharge == true && t.IsSendForApproval == false).Any() || ChargeList.Where(t => t.SelectCharge == true && t.IsSendForApproval == true && t.ApprovalStatus == false).Any())
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                new MessageBoxControl.MessageBoxChildWindow("", "Selected Service is not approved yet", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    RefundService();
                }
            }
        }

        private void RefundService()
        {
            if (ChargeList.Where(s => s.SelectCharge == true && s.IsCancelled == false).Any())
            {
                //if (ChargeList.Where(s => s.FirstApprovalChecked == true && s.SecondApprovalChecked == true).Any())
                //{
                ClickedFlag += 1;
                if (ClickedFlag == 1)
                {
                    RefundServicesPaymentDetails Win = new RefundServicesPaymentDetails();
                    Win.txtPaidAmount.IsReadOnly = true;
                    Win.OnSaveButton_Click += new RoutedEventHandler(RefundWin_OnSaveButton_Click);
                    Win.OnCancelButton_Click += new RoutedEventHandler(RefundWin_OnCancelButton_Click);
                    Win.txtPaidAmount.Text = txtTotalRefundAmount.Text;
                    Win.Show();
                    if (IsAuditTrail)   // By Umesh For Audit Trail
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 26 : Data For Payment Window To Refund Services  " + "\r\n"
                                                + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                + "Total Refund Amount : " + Convert.ToString(txtTotalRefundAmount.Text) + " "
                                                ;
                    }
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please select Service For Refund", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        void RefundWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((RefundServicesPaymentDetails)sender).DialogResult == true)
            {
                clsRefundVO RefundObj = new clsRefundVO();

                if (((RefundServicesPaymentDetails)sender).Payment != null)
                {
                    RefundObj.PaymentDetails = ((RefundServicesPaymentDetails)sender).Payment;
                }
                RefundObj.Date = DateTime.Now;

                RefundObj.BillID = BillID;
                //if (!string.IsNullOrEmpty(txtTotalRefundAmount.Text))
                //RefundObj.Amount = double.Parse(txtTotalRefundAmount.Text);
                RefundObj.Amount = Convert.ToDouble(((RefundServicesPaymentDetails)sender).txtPaidAmount.Text);

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    RefundObj.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                else
                    RefundObj.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

                clsAddRefundBizActionVO BizAction = new clsAddRefundBizActionVO();
                BizAction.Details = RefundObj;

                BizAction.IsRefundToAdvance = ((RefundServicesPaymentDetails)sender).IsRefundToAdvance;     // Refund to Advance 20042017

                BizAction.RefundToAdvancePatientID = ((clsBillVO)dgBillList.SelectedItem).PatientID;        // Refund to Advance 20042017
                BizAction.RefundToAdvancePatientUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;       // Refund to Advance 20042017
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
                            //Change By Bhushan 23022017

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
                                                                       if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstServices && ((clsBillVO)dgBillList.SelectedItem).LevelID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
                                                                       {
                                                                           PrintBill(RefundID, UnitID, PRID);
                                                                       }
                                                                       else if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstBill && ((clsBillVO)dgBillList.SelectedItem).LevelID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
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
                                    #region Commented By Bhushanp
                                    //PRID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;
                                    //UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    //    new MessageBoxControl.MessageBoxChildWindow("", "Refund details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                    //msgW1.OnMessageBoxClosed += (re) =>
                                    //    {
                                    //        if (re == MessageBoxResult.OK)
                                    //        {
                                    //            if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstServices)
                                    //            {
                                    //                PrintBill(RefundID, UnitID, PRID);
                                    //            }
                                    //            else if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstBill)
                                    //            {
                                    //                PrintBill(RefundID, UnitID, PRID);
                                    //            }
                                    //        }
                                    //    };

                                    //msgW1.Show();
                                    //txtTotalRefundAmount.Text = "0";
                                    //SetCommandButtonState("Load");
                                    //FillBillSearchList();
                                    //_flip.Invoke(RotationType.Backward);
                                    #endregion
                                    #region New By Bhushanp
                                    double tempRefundAmount = Convert.ToDouble(((RefundServicesPaymentDetails)sender).txtPaidAmount.Text);
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
                                                            if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstServices && ((clsBillVO)dgBillList.SelectedItem).LevelID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
                                                            {
                                                                PrintBill(RefundID, UnitID, PRID);
                                                            }
                                                            else if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstBill && ((clsBillVO)dgBillList.SelectedItem).LevelID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
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
                    SelectedBill = new clsBillVO();
                    SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                    BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                    txtTotalRefundAmount.Text = "0";
                    txtRefundedAmount.Text = "0";
                    rdbAgainstServices.IsChecked = true;
                    dgCharges.IsEnabled = true;
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
                        else if (objUser == null ? false : objUser.IsRefundSerAfterSampleCollection && objVO.IsSampleCollected)   // user wise
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
                        else if (objVO.IsSampleCollected)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Service not refund!, Sample Collection done for this Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                string URL = "../Reports/OPD/RefundServicesBill.aspx?RefundID=" + iRefundID + "&UnitID=" + UnitID + "&PrintDate=" + PrintDate + "&PrintFomatID=" + PrintID + "&IsFromIPD=" + 0;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
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
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                DateTime PrintDate = DateTime.Now;
                string URL = "../Reports/OPD/RefundServicesBillNew.aspx?RefundID=" + iRefundID + "&UnitID=" + UnitID + "&PrintDate=" + PrintDate.ToString("dd-MMM-yyyy") + "&IsFromIPD=" + 0;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void cmdSendApprove_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                if (ChargeList.Where(s => s.SelectCharge == true && s.IsCancelled == false).Any())
                {
                    if (IsAuditTrail)   // By Umesh For Audit Trail
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
                    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Are You Sure You Want To Send For Approval?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                    msgWD_1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please select Service", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                if (ChargeList.Where(s => s.SelectCharge == true && s.IsCancelled == false).Any())
                {
                    GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID, true);
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please select Service For Approval", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
        }

        int ApproveFlag = 0;
        public void GetUserRights(long UserId, bool IsFromApproveClick)
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

                        if (IsFromApproveClick && objUser.OpdAuthLvl != null)
                        {
                            if (objUser.OpdAuthLvl == SelectedBill.LevelID)
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
        private void SendForApproval()
        {
            try
            {
                clsSendRequestForApprovalVO BizAction = new clsSendRequestForApprovalVO();
                BizAction.IsRefundRequest = true;
                BizAction.Details = new clsRefundVO();
                BizAction.Details.BillID = SelectedBill.ID;
                BizAction.Details.BillUnitID = SelectedBill.UnitID;
                BizAction.Details.IsOPDBill = true;
                BizAction.Details.RequestTypeID = (int)RequestType.Refund;
                BizAction.Details.RequestType = Enum.GetName(typeof(RequestType), RequestType.Refund);
                BizAction.LogInfoList = new List<LogInfo>();    // By Umesh For AuditTrail
                BizAction.LogInfoList = LogInfoList;            // By Umesh For AuditTrail

                if (rdbAgainstBill.IsChecked == true)
                {
                    BizAction.IsAgainstBill = true;
                    BizAction.RefundAmount = Convert.ToDouble(txtTotalRefundAmount.Text);
                }

                if (BizAction.ChargeList == null)
                    BizAction.ChargeList = new List<clsChargeVO>();

                foreach (var item in ChargeList)
                {

                    if (item.SelectCharge == true && item.IsCancelled == false)
                        BizAction.ChargeList.Add(item);
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
            //}
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
                BizAction.RefundAmount = Convert.ToDouble(txtTotalRefundAmount.Text);// Added y Bhushanp 24032017

                BizAction.ApprovedByID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizAction.ApprovedByName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                if (objUser.OpdAuthLvl == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
                    BizAction.IsApproved = true;
                else
                    BizAction.IsApproved = false;
                BizAction.LevelID = objUser.OpdAuthLvl;
                BizAction.ApprovedDateTime = DateTime.Now;
                BizAction.LogInfoList = LogInfoList;
                if (IsAuditTrail)   // By Umesh For Audit Trail
                {
                    LogInformation = new LogInfo();
                    LogInformation.guid = objGUID;
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 25 : Approved Services " + "\r\n"
                                            + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                            + "Is Refund Request : " + Convert.ToString(BizAction.IsRefundRequest) + " "
                                            + "Is For Approval : " + Convert.ToString(BizAction.IsForApproval) + " "
                                            + "Approval Request ID : " + Convert.ToString(BizAction.ApprovalRequestID) + " "
                                            + "ApprovedBy ID : " + Convert.ToString(BizAction.ApprovedByID) + " "
                                            + "ApprovedBy Name : " + Convert.ToString(BizAction.ApprovedByName) + " "
                                            + "Is Approved : " + Convert.ToString(BizAction.IsApproved) + " "
                                            + "Level ID : " + Convert.ToString(BizAction.LevelID) + " "
                                            + "Approved DateTime : " + Convert.ToString(BizAction.ApprovedDateTime) +
                                            "Service list For Refund" + "\r\n";
                }
                //Change By Bhushanp 22032017 For Refund Against Bill

                foreach (var item in ChargeList)
                {
                    if (IsAuditTrail)  // By Umesh For Audit Trail
                    {
                        LogInformation.Message = LogInformation.Message + "Service list For Refund" + "\r\n"
                                               + " , Bill No :" + Convert.ToString(item.BillNo) + " "
                                               + " , Bill Date :" + Convert.ToString(item.Date) + " "
                                               + " , InitialRate : " + Convert.ToString(item.InitialRate) + " "
                                               + " , Is Send For Approval : " + Convert.ToString(item.IsSendForApproval) + " "
                                               + " , Is First Approval :" + Convert.ToString(item.IsFirstApproval) + " "
                                               + " , Is Second Approval : " + Convert.ToString(item.IsSecondApproval) + " "
                                               + " , Net Amount : " + Convert.ToString(item.NetAmt) + " "
                                               + " , Rate: " + Convert.ToString(item.Rate) + " "
                                               + " , Service Code :" + Convert.ToString(item.ServiceCode) + " "
                                               + " , ServiceName :" + Convert.ToString(item.ServiceName) + " "
                                               + " , Service Paid Amount :" + Convert.ToString(item.ServicePaidAmount) + " "
                                               + " , Settle Net Amount :" + Convert.ToString(item.SettleNetAmount) + " "
                                               + " , Total Amount :" + Convert.ToString(item.TotalAmount) + " "
                                               + "\r\n";
                    }
                    if (item.IsSendForApproval && item.IsCancelled == false)
                        BizAction.ApprovalChargeList.Add(item);
                }

                if (IsAuditTrail)   // By Umesh For Audit Trail
                {
                    LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                    LogInfoList.Add(LogInformation);
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (sa, args) =>
                {
                    ClickedFlag = 0;
                    if (args.Error == null)
                    {
                        if (objUser.OpdAuthLvl == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
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
                    ApproveFlag = 0;
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
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
                    //dgCharges.Columns[3].Visibility = Visibility.Visible; // date 19032018
                    //dgCharges.Columns[4].Visibility = Visibility.Collapsed;
                }
                else
                {
                    dgCharges.Columns[3].Visibility = Visibility.Collapsed;
                    dgCharges.Columns[4].Visibility = Visibility.Visible;
                    //dgCharges.Columns[4].IsReadOnly = false;
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
            if (IsAuditTrail)   // By Umesh For Audit Trail
            {
                LogInformation = new LogInfo();
                LogInformation.guid = objGUID;
                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                LogInformation.TimeStamp = DateTime.Now;
                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                if (IsFromRequestApproval)
                    LogInformation.Message = " 24 : Service List For Request Approval " + "\r\n";
                else
                {
                    LogInformation.Message = " 20 : Service List  " + "\r\n";
                }
                LogInformation.Message = LogInformation.Message + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                    + "Is Send For Approval : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).IsSendForApproval) + " "
                                                    + "Cancel Service : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).SelectCharge) + " "
                                                    + "Approval Remark : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).ApprovalRemark) + " "
                                                    + "Request Remark : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).ApprovalRequestRemark) + " "
                                                    + "Service Code : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).ServiceId) + " "
                                                    + "Service Name : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).ServiceName) + " "
                                                    + "Rate : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).Rate) + " "
                                                    + "Quantity : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).Quantity) + " "
                                                    + "Concession Percent : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).ConcessionPercent) + " "
                                                    + "Concession : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).Concession) + " "
                                                    + "Service Tax Percent : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).ServiceTaxPercent) + " "
                                                    + "Service Tax Amount : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).ServiceTaxAmount) + " "
                                                    + "Total Amount : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).TotalAmount) + " "
                                                    + "Service Net Amount : " + Convert.ToString(((clsChargeVO)(e.Row.DataContext)).NetAmount) + " "
                                                    + "Total : " + Convert.ToString(txtClinicalTotal.Text) + " "
                                                    + "Concession : " + Convert.ToString(txtClinicalConcession.Text) + " "
                                                    + "Total Net Amount : " + Convert.ToString(txtClinicalNetAmount.Text) + "\r\n"
                                            ;
                LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                LogInfoList.Add(LogInformation);
            }
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillBillSearchList();
            }
        }

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtBillNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillBillSearchList();
            }
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
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
            decimal RefundAmount = 0, RefundedAmount = 0;
            NetAmount = Convert.ToDecimal(txtClinicalNetAmount.Text);
            RefundAmount = Convert.ToDecimal(txtTotalRefundAmount.Text);
            // RefundedAmount = Convert.ToDecimal(txtRefundedAmount.Text);
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

            if (!this.IsFromRequestApproval && ChargeList != null && ChargeList.Count > 0)
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
        //    if (dgCharges.SelectedItem != null && (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList != null)
        //    {
        //        frmServiceTaxTransactionDetails frmTaxDetails = new frmServiceTaxTransactionDetails();
        //        frmTaxDetails.objChargeVO = dgCharges.SelectedItem as clsChargeVO;
        //        frmTaxDetails.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
        //        frmTaxDetails.Show();
        //    }
        //    else if (dgCharges.SelectedItem != null && (dgCharges.SelectedItem as clsChargeVO).ChargeTaxDetailsList == null)
        //    {
        //        FillTaxChargeList((dgCharges.SelectedItem as clsChargeVO).ID, (dgCharges.SelectedItem as clsChargeVO).UnitID);
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
        //    List<clsChargeTaxDetailsVO> ServiceTaxList = new List<clsChargeTaxDetailsVO>();
        //    foreach (var item in ((clsChargeVO)dgCharges.SelectedItem).ChargeTaxDetailsVO.TaxLinkingDetailsList.ToList())
        //    {
        //        clsChargeTaxDetailsVO ChargeTaxDetailsVO = new clsChargeTaxDetailsVO();
        //        ChargeTaxDetailsVO.ServiceId = item.ServiceId;
        //        ChargeTaxDetailsVO.TariffId = item.TariffId;
        //        ChargeTaxDetailsVO.ClassId = item.ClassId;
        //        ChargeTaxDetailsVO.TaxID = item.TaxID;
        //        ChargeTaxDetailsVO.Percentage = item.Percentage;
        //        ChargeTaxDetailsVO.TaxType = item.TaxType;
        //        ChargeTaxDetailsVO.IsTaxLimitApplicable = item.IsTaxLimitApplicable;
        //        ChargeTaxDetailsVO.TaxLimit = item.TaxLimit;
        //        ChargeTaxDetailsVO.ServiceName = item.ServiceName;
        //        ChargeTaxDetailsVO.TaxName = item.TaxName;
        //        ChargeTaxDetailsVO.Quantity = 1;
        //        ChargeTaxDetailsVO.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
        //        ChargeTaxDetailsVO.TotalAmount = 0;
        //        ServiceTaxList.Add(ChargeTaxDetailsVO);
        //    }

        //    double TotalTaxamount = 0;
        //    if (((clsChargeVO)dgCharges.SelectedItem) != null && ServiceTaxList != null && ServiceTaxList.Count() > 0)
        //    {
        //        foreach (var item in ServiceTaxList.ToList())
        //        {
        //            if (item.IsTaxLimitApplicable)
        //            {
        //                if (((clsChargeVO)dgCharges.SelectedItem).Rate > Convert.ToDouble(item.TaxLimit))
        //                {
        //                    ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxPercent = Convert.ToDouble(item.Percentage);
        //                    if (item.TaxType == 2) //Exclusive
        //                        TotalTaxamount += ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount;

        //                    item.ServiceTaxPercent = Convert.ToDouble(item.Percentage);
        //                    item.ServiceTaxAmount = ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount;
        //                    item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
        //                }
        //                else
        //                {
        //                    item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
        //                    item.ServiceTaxAmount = 0;
        //                }
        //            }
        //            else
        //            {
        //                ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxPercent = Convert.ToDouble(item.Percentage);
        //                if (item.TaxType == 2) //Exclusive
        //                    TotalTaxamount += ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount;

        //                item.ServiceTaxPercent = Convert.ToDouble(item.Percentage);
        //                item.ServiceTaxAmount = ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount;
        //                item.Rate = ((clsChargeVO)dgCharges.SelectedItem).Rate;
        //            }
        //        }
        //        ((clsChargeVO)dgCharges.SelectedItem).TotalServiceTaxAmount = TotalTaxamount;
        //        ((clsChargeVO)dgCharges.SelectedItem).ServiceTaxAmount = TotalTaxamount;

        //        CalculateClinicalSummary();
        //    }
        //    return ServiceTaxList;
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

        //Begin::Added by AniketK on 30-Jan-2019
        private void btnReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                frmRefundReceiptList receiptWin = new frmRefundReceiptList();
                receiptWin.ID = ((clsBillVO)dgBillList.SelectedItem).ID;
                receiptWin.UnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                receiptWin.BillPaymentType = ((clsBillVO)dgBillList.SelectedItem).BillPaymentType;
                receiptWin.Show();

            }
        }
        //End::Added by AniketK on 30-Jan-2019
    }
}

