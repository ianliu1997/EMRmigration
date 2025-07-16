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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.IPD;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.Administration.UserRights;
using PalashDynamics.ValueObjects.Log;


namespace CIMS.Forms
{
    public partial class frmApprovalRequests : UserControl
    {
        #region Variable Declaration
        SwivelAnimation _flip = null;
        clsBillVO SelectedBill { get; set; }
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
        // For Activity Log By Umesh
        int lineNumber = 0;
        List<LogInfo> LogInfoList = new List<LogInfo>();  // For the Activity Log List
        LogInfo LogInformation;
        Guid objGUID;
        bool IsAuditTrail = false;
        // Activity log end

        #endregion

        #region Load event
        public frmApprovalRequests()
        {
            InitializeComponent();
            //  _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

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
            FillRequestType();
            //FillBillSearchList();

            //By Anjali.....................
            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
            //........................................

            // FillBillType();
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
            //dgCharges.ItemsSource = ChargeList;
            //dgCharges.Focus();
            //dgCharges.UpdateLayout();
            //txtClinicalTotal.Text = "";
            //txtClinicalConcession.Text = "";
            //txtClinicalNetAmount.Text = "";
            //txtTotalRefundAmount.Text = "";
            SelectedBill = null;
        }
        #endregion

        #region FillDatatgrid

        private void FillRequestType()
        {

            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(1, "OPD"));
            objList.Add(new MasterListItem(0, "IPD"));
            cmbBillType.ItemsSource = null;
            cmbBillType.ItemsSource = objList;
            cmbBillType.SelectedItem = objList[1];
        }

        private void FillBillType()
        {

            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "-- All --"));
            objList.Add(new MasterListItem(1, "Concession"));
            objList.Add(new MasterListItem(2, "Refund"));

            cmbRequestType.ItemsSource = null;
            cmbRequestType.ItemsSource = objList;
            cmbRequestType.SelectedItem = objList[0];
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
                //if (this.DataContext != null)
                //{
                //    cmbUnit.SelectedValue = ((clsBillVO)this.DataContext).UnitID;

                //}
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        clsUserRightsVO objUser = new clsUserRightsVO();

        public void GetUserRights(long UserId)
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

                        FillBillSearchList();

                        //if (objUser.OpdAuthLvl != null)
                        //{
                        //    if (objUser.OpdAuthLvl == SelectedBill.LevelID)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill is already approved with " + objUser.AuthLevelForRefundOPD + " rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        //        msgWindowUpdate.Show();
                        //    }
                        //    else
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve the Refund with " + objUser.AuthLevelForRefundOPD + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        //        msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                        //        msgWindowUpdate.Show();
                        //    }
                        //}

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

        private void FillBillSearchList()
        {
            //checkFreezColumn = false;
            indicator.Show();

            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.IsFromApprovalRequestWindow = true;
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
            // BizAction.IsFreeze = true;

            // Added By CDS
            BizAction.FromRefund = true;
            if (rdoOPD.IsChecked == true)
            {
                BizAction.IsOPDBill = true;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                else
                    BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
            }
            else
            {
                BizAction.IsOPDBill = false;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                else
                    BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;
            }
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

            // BizAction.UserLevelID = objUser.OPDAuthLvtForConcessionID;
            //BizAction.IsFreeze = false;

            if (objUser.OPDAuthLvtForConcessionID > 0 && objUser.OpdAuthLvl == 0) //(Int64)RequestType.Concession
            {
                BizAction.UserRightsTypeID = (Int64)RequestType.Concession;
                BizAction.UserLevelID = objUser.OPDAuthLvtForConcessionID;
            }
            else if (objUser.OpdAuthLvl > 0 && objUser.OPDAuthLvtForConcessionID == 0)  //(Int64)RequestType.Refund
            {
                BizAction.UserRightsTypeID = (Int64)RequestType.Refund;
                BizAction.UserLevelID = objUser.OpdAuthLvl;
            }
            else
            {
                if (objUser.OPDAuthLvtForConcessionID > objUser.OpdAuthLvl)
                {
                    BizAction.UserLevelID = objUser.OPDAuthLvtForConcessionID;
                }
                else if (objUser.OpdAuthLvl > objUser.OPDAuthLvtForConcessionID)
                {
                    BizAction.UserLevelID = objUser.OpdAuthLvl;
                }
                else if (objUser.OpdAuthLvl == objUser.OPDAuthLvtForConcessionID)
                {
                    BizAction.UserLevelID = objUser.OpdAuthLvl;
                }

            }


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
                        if (IsAuditTrail)   // By Umesh For Audit Trail
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 23 : Bill List On Approval Requests  " + "\r\n"
                                                    + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                    + "Bill Unit Id : " + Convert.ToString(BizAction.UnitID) + " "
                                                    + "From Date : " + Convert.ToString(BizAction.FromDate) + " "
                                                    + "To Date : " + Convert.ToString(BizAction.ToDate) + " "
                                                    + "From Refund : " + Convert.ToString(BizAction.FromRefund) + " "
                                                    + "Is OPD Bill : " + Convert.ToString(BizAction.IsOPDBill) + " "
                                                    + "Costing Division ID : " + Convert.ToString(BizAction.CostingDivisionID) + " "
                                                    + "Bill Type : " + Convert.ToString(BizAction.BillType) + " "
                                                    + "Bill NO : " + Convert.ToString(BizAction.BillNO) + " "
                                                    + "OPD NO : " + Convert.ToString(BizAction.OPDNO) + " "
                                                    + "First Name : " + Convert.ToString(BizAction.FirstName) + " "
                                                    + "Last Name : " + Convert.ToString(BizAction.LastName) + " "
                                                    ;
                        }
                        foreach (var item in result.List)
                        {
                            if (IsAuditTrail)   // By Umesh For Audit Trail
                            {
                                LogInformation.Message = LogInformation.Message
                                                   + " , Date :" + Convert.ToString(item.Date) + " "
                                                   + " , Bill No :" + Convert.ToString(item.BillNo) + " "
                                                   + " , Request Type : " + Convert.ToString(item.RequestType) + " "
                                                   + " , OPD NO : " + Convert.ToString(item.Opd_Ipd_External_No) + " "
                                                   + " , Name :" + Convert.ToString(item.PatientName) + " "
                                                   + " , Total Bill Amount : " + Convert.ToString(item.TotalBillAmount) + " "
                                                   + " , Total Concession Amount : " + Convert.ToString(item.TotalConcessionAmount) + " "
                                                   + " , Net Bill Amount : " + Convert.ToString(item.NetBillAmount) + " "
                                                   + " , Freezed: " + Convert.ToString(item.IsFreezed) + " "
                                                   + " , Total Paid Amount :" + Convert.ToString(item.PaidAmount) + " "
                                    //+ " , Total Refund Amount :" + Convert.ToString(item.TotalRefund) + " "
                                    //+ " , Bill Payment Type :" + Convert.ToString(item.BillPaymentTypeStr) + " "
                                    //+ " , Authorised Person :" + Convert.ToString(item.AuthorityPerson) + " "
                                                   + "\r\n"
                                                   ;
                            }

                            if (IsAuditTrail)  // By Umesh For Audit Trail
                            {
                                LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                                LogInfoList.Add(LogInformation);
                            }
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
                BizAction.IsFromApprovalRequestWindow = true;
                BizAction.ID = 0;

                BizAction.Opd_Ipd_External_Id = pVisitID;
                BizAction.Opd_Ipd_External_UnitId = pPatientUnitID;
                BizAction.IsBilled = pIsBilled;
                BizAction.BillID = PBillID;
                BizAction.UnitID = pUnitID; // ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

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

                            foreach (var item in objList)
                            {
                                if (item.IsCancelled == true)
                                {
                                    item.SelectCharge = true;
                                    item.IsEnable = false;
                                }
                                ChargeList.Add(item);
                            }
                            //dgCharges.Focus();
                            //dgCharges.UpdateLayout();
                            CalculateClinicalSummary();
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
            InitialiseForm();
            SetCommandButtonState("Cancel");
            _flip.Invoke(RotationType.Backward);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            RefundService();
        }

        private void RefundService()
        {
            if (ChargeList.Where(s => s.SelectCharge == true && s.IsCancelled == false).Any())
            {
                if (ChargeList.Where(s => s.FirstApprovalChecked == true && s.SecondApprovalChecked == true).Any())
                {
                    ClickedFlag += 1;
                    if (ClickedFlag == 1)
                    {
                        RefundServicesPaymentDetails Win = new RefundServicesPaymentDetails();
                        Win.txtPaidAmount.IsReadOnly = true;
                        Win.OnSaveButton_Click += new RoutedEventHandler(RefundWin_OnSaveButton_Click);
                        Win.OnCancelButton_Click += new RoutedEventHandler(RefundWin_OnCancelButton_Click);
                        // Win.txtPaidAmount.Text = txtTotalRefundAmount.Text;
                        Win.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Selected Service Is Not Approved Yet", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
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

                clsAddRefundBizActionVO BizAction = new clsAddRefundBizActionVO();
                BizAction.Details = RefundObj;

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
                                foreach (var item in ChargeList)
                                {
                                    if (item.SelectCharge == true && item.IsCancelled == false)
                                    {
                                        item.RefundID = RefundID;
                                        item.IsCancelled = true;
                                        item.CancelledDate = Date;
                                        item.RefundAmount = item.NetAmount;

                                        BizActionObj.ChargeList.Add(item);
                                        // This Block Of Code Is Only For Package Services 

                                        //var _List = from obj in objList
                                        //            where (obj.PackageID.Equals(item.PackageID) && obj.ChildPackageService == true)
                                        //            select obj;

                                        //foreach (var objcharge in _List)
                                        //{
                                        //    objcharge.RefundID = RefundID;
                                        //    objcharge.IsCancelled = true;
                                        //    objcharge.CancelledDate = Date;
                                        //    objcharge.RefundAmount = objcharge.NetAmount;

                                        //    BizActionObj.ChargeList.Add(objcharge);
                                        //}
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
                                                                   if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstServices)
                                                                   {
                                                                       PrintBill(RefundID, UnitID, PRID);
                                                                   }
                                                                   else if (((clsBillVO)dgBillList.SelectedItem).BillPaymentType == BillPaymentTypes.AgainstBill)
                                                                   {
                                                                       PrintReceipt(RefundID, PRID);
                                                                   }
                                                               }
                                                           };


                                                       msgW1.Show();
                                                       // txtTotalRefundAmount.Text = "0";
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
        public enum RequestType
        {
            Concession = 1,
            Refund = 2
        }
        private void hlbView_Click(object sender, RoutedEventArgs e)
        {

            if (dgBillList.SelectedItem != null)
            {
                if (((clsBillVO)dgBillList.SelectedItem).RequestType == Enum.GetName(typeof(RequestType), RequestType.Refund) && ((clsBillVO)dgBillList.SelectedItem).IsOPDBill == true)
                {

                    frmRefundServices _IPDBills = new frmRefundServices();
                    _IPDBills.IsFromRequestApproval = true;
                    _IPDBills.SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                    _IPDBills.IsRefundSerAfterSampleCollection = objUser == null ? false : objUser.IsRefundSerAfterSampleCollection;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Patient Bill";
                    ((IApplicationConfiguration)App.Current).OpenMainContent(_IPDBills);
                }
                else if (((clsBillVO)dgBillList.SelectedItem).RequestType == Enum.GetName(typeof(RequestType), RequestType.Concession) && ((clsBillVO)dgBillList.SelectedItem).IsOPDBill == true)
                {
                    if (((clsBillVO)dgBillList.SelectedItem).LevelID == (objUser == null ? 0 : objUser.OPDAuthLvtForConcessionID))
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill is already approved with " + (objUser == null ? string.Empty : objUser.AuthLevelForConcenssionOPD) + " rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindowUpdate.Show();
                    }
                    else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID > 0 && (objUser == null ? 0 : objUser.OPDAuthLvtForConcessionID) > ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Invalid Level of Authorization ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindowUpdate.Show();
                    }
                    else
                    {
                        GetPatient(((clsBillVO)dgBillList.SelectedItem).PatientID, ((clsBillVO)dgBillList.SelectedItem).PatientUnitID);
                    }
                }
                else if (((clsBillVO)dgBillList.SelectedItem).RequestType == Enum.GetName(typeof(RequestType), RequestType.Refund) && ((clsBillVO)dgBillList.SelectedItem).IsOPDBill == false)
                {

                    ModuleName = "PalashDynamics.IPD";
                    Action = "PalashDynamics.IPD.IPDRefundServices";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    WebClient c2 = new WebClient();
                    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                    c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

                }
                else if (((clsBillVO)dgBillList.SelectedItem).RequestType == Enum.GetName(typeof(RequestType), RequestType.Concession) && ((clsBillVO)dgBillList.SelectedItem).IsOPDBill == false)
                {
                    GetAdmissionList(((clsBillVO)dgBillList.SelectedItem).PatientID, ((clsBillVO)dgBillList.SelectedItem).PatientUnitID);
                }
            }






            //try
            //{
            //    if (dgBillList.SelectedItem != null)
            //    {

            //        InitialiseForm();
            //        SetCommandButtonState("Save");
            //        SelectedBill = new clsBillVO();
            //        SelectedBill = (clsBillVO)dgBillList.SelectedItem;
            //        BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
            //        if (FirstApprovalRight == true || SecondApprovalRight == true)
            //        {
            //            cmdApprove.IsEnabled = true;
            //            cmdSendApprove.IsEnabled = false;
            //            cmdSave.IsEnabled = false;
            //        }
            //        else
            //        {
            //            cmdApprove.IsEnabled = false;
            //            cmdSave.IsEnabled = true;
            //            cmdSendApprove.IsEnabled = true;
            //        }
            //        if (((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
            //        {
            //            FillChargeList(SelectedBill.ID, SelectedBill.UnitID, true, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId);
            //        }
            //        else
            //        {
            //            FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id, SelectedBill.Opd_Ipd_External_UnitId);
            //        }
            //        _flip.Invoke(RotationType.Forward);
            //    }


            //}
            //catch (Exception ex)
            //{
            //    string err = ex.Message;
            //}
        }
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        private void GetPatient(long PatientID, long PatientUnitID)
        {
            clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
            BizAction.PatientDetails = new clsPatientVO();
            BizAction.PatientDetails.GeneralDetails = new clsPatientGeneralVO();
            BizAction.PatientDetails.GeneralDetails.PatientID = PatientID;
            BizAction.PatientDetails.GeneralDetails.UnitId = PatientUnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {


                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).SelectedPatient = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                    {
                        frmBill bill = new frmBill();
                        bill.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        bill.IsPatientExist = true;
                        bill.IsFromRequestApproval = true;
                        bill.SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                        mElement.Text = "Patient Bill";
                        ((IApplicationConfiguration)App.Current).OpenMainContent(bill);

                    }

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetAdmissionList(long PatientID, long PatientUnitID)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsGetIPDAdmissionListBizActionVO BizAction = new clsGetIPDAdmissionListBizActionVO();
            BizAction.AdmList = new List<clsIPDAdmissionVO>();
            BizAction.AdmDetails = new clsIPDAdmissionVO();
            BizAction.AdmDetails.PatientId = PatientID;
            BizAction.AdmDetails.PatientUnitID = PatientUnitID;
            BizAction.IsPagingEnabled = false;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;



            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    //clsGetServiceMasterListBizActionVO result = args.Result as clsGetServiceMasterListBizActionVO;
                    clsGetIPDAdmissionListBizActionVO result = arg.Result as clsGetIPDAdmissionListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;
                    if (result.AdmList != null)
                    {

                        clsIPDAdmissionVO objAdmission = result.AdmList[0];
                        ((IApplicationConfiguration)App.Current).SelectedIPDPatient = new clsIPDAdmissionVO();
                        ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        ((IApplicationConfiguration)App.Current).SelectedIPDPatient = result.AdmList[0];
                        ((IApplicationConfiguration)App.Current).SelectedIPDPatient.classID = objAdmission.classID;
                        //((IApplicationConfiguration)App.Current).SelectedPatient.ClassID = objAdmission.classID;
                        // ((IApplicationConfiguration)App.Current).SelectedPatient.BillingToBedCategoryID = objAdmission.classID;

                        //Addded By CDS For Admission Type
                        ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionTypeID = objAdmission.AdmissionTypeID;
                        ((IApplicationConfiguration)App.Current).SelectedIPDPatient.BillCount = objAdmission.BillCount;


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

                        ModuleName = "PalashDynamics.IPD";
                        Action = "PalashDynamics.IPD.frmIPDBills";
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        WebClient c2 = new WebClient();
                        c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

                    }

                }
                Indicatior.Close();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
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
                ((IPassData)myData).PassDataToForm((clsBillVO)dgBillList.SelectedItem, true);
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData as UIElement);
            }
            catch (Exception ex)
            {
                throw;
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
            //txtClinicalTotal.Text = clinicalTotal.ToString();
            //txtClinicalConcession.Text = clinicalConcession.ToString();
            //txtClinicalNetAmount.Text = clinicalNetAmount.ToString();


        }

        private void CancelService_Click(object sender, RoutedEventArgs e)
        {
            //if (dgCharges.SelectedItem != null)
            //{
            //    double RefundAmount = 0;
            //    if (txtTotalRefundAmount.Text != null && txtTotalRefundAmount.Text.Length > 0)
            //    {
            //        RefundAmount = Convert.ToDouble(txtTotalRefundAmount.Text);
            //    }
            //    if (((CheckBox)sender).IsChecked == true)
            //    {
            //        RefundAmount = RefundAmount + ((clsChargeVO)(dgCharges.SelectedItem)).NetAmount;
            //    }
            //    else
            //        RefundAmount = RefundAmount - ((clsChargeVO)(dgCharges.SelectedItem)).NetAmount;

            //    txtTotalRefundAmount.Text = RefundAmount.ToString();
            //}
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Serach_BillRequest();
        }

        public void Serach_BillRequest()
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
                    cmdSave.IsEnabled = true;
                    cmdSendApprove.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    cmdApprove.IsEnabled = true;
                    break;

                case "Cancel":

                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    cmdSendApprove.IsEnabled = false;
                    cmdApprove.IsEnabled = false;
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
            //if (iRefundID > 0)
            //{
            //    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            //    string URL = "../Reports/OPD/RefundAgainstBill.aspx?RefundID=" + iRefundID + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID;
            //    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            //}
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
            if (ChargeList.Where(s => s.SelectCharge == true && s.IsCancelled == false).Any())
            {
                if (ChargeList.Where(s => s.IsSetForApproval == true && s.IsCancelled == false && s.SelectCharge == true).Any())
                    SendForApproval();
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Selected Service Already sent For Approval", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Please select Service For Request", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }


        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            if (ChargeList.Where(s => (s.SecondApprovalChecked == true || s.FirstApprovalChecked == true) && s.IsCancelled == false).Any())
                ApproveRefund();
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Please select Service For Approval", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        private void SendForApproval()
        {
            try
            {
                clsSendRequestForApprovalVO BizAction = new clsSendRequestForApprovalVO();
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
                            new MessageBoxControl.MessageBoxChildWindow("", "Refund Request Send successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            //txtTotalRefundAmount.Text = "0";
                            SetCommandButtonState("Load");
                            FillBillSearchList();
                            _flip.Invoke(RotationType.Backward);
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Refund details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        private void ApproveRefund()
        {
            try
            {
                clsSendRequestForApprovalVO BizAction = new clsSendRequestForApprovalVO();
                BizAction.IsForApproval = true;
                if (BizAction.ApprovalChargeList == null)
                    BizAction.ApprovalChargeList = new List<clsChargeVO>();
                if (FirstApprovalRight == true)
                    BizAction.IsFirstApproval = true;
                else
                    BizAction.IsFirstApproval = false;
                if (SecondApprovalRight == true)
                    BizAction.IsSecondApproval = true;
                else
                    BizAction.IsSecondApproval = false;
                foreach (var item in ChargeList)
                {
                    if ((item.FirstApprovalChecked == true || item.SecondApprovalChecked == true) && item.IsCancelled == false)
                        BizAction.ApprovalChargeList.Add(item);
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (sa, args) =>
                {
                    ClickedFlag = 0;
                    if (args.Error == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Service Approved Sucessfully, Would you like to Refund it?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                RefundService();
                            }
                            // txtTotalRefundAmount.Text = "0";
                            SetCommandButtonState("Load");
                            FillBillSearchList();
                            _flip.Invoke(RotationType.Backward);
                        };
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Refund details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        private void cmbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // FillBillSearchList();
        }




        public bool IsFromAdmissionList { get; set; }

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
                Serach_BillRequest();
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
                Serach_BillRequest();
            }
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }
        int RejectionFlag = 0;
        private void cmdReject_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                if (!(dgBillList.SelectedItem as clsBillVO).IsFreezed)
                {
                    RejectionFlag += 1;

                    if (((clsBillVO)dgBillList.SelectedItem).RequestType == Enum.GetName(typeof(RequestType), RequestType.Refund))
                    {
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID > 0 && (objUser == null ? 0 : objUser.OPDAuthLvtForConcessionID) > ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID) //Added by ajit date 28/10/2016
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", "Invalid Level of Authorization for " + ((clsBillVO)dgBillList.SelectedItem).RequestType + ".", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            if (RejectionFlag == 1)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Are You Sure\n You Want To Reject Request? ", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        DeleteRequest();
                                    }
                                    else  //Added by ajit date 28/10/2016
                                    {
                                        RejectionFlag = 0;
                                    }

                                };
                                msgW.Show();
                            }
                        }
                    }
                    else
                    {
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID > 0 && (objUser == null ? 0 : objUser.OPDAuthLvtForConcessionID) > ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID) //Added by ajit date 28/10/2016
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", "Invalid Level of Authorization for " + ((clsBillVO)dgBillList.SelectedItem).RequestType + ".", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            if (RejectionFlag == 1)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Are You Sure\n You Want To Reject Request? ", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        DeleteRequest();
                                    }
                                    else  //Added by ajit date 28/10/2016
                                    {
                                        RejectionFlag = 0;
                                    }

                                };
                                msgW.Show();
                            }
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("", "Bill is freezed! Cannot reject selected Bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("", "Please select request for rejection", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        private void DeleteRequest()
        {
            try
            {
                clsDeleteApprovalRequestVO BizAction = new clsDeleteApprovalRequestVO();
                BizAction.ApprovalRequestID = ((clsBillVO)dgBillList.SelectedItem).ApprovalRequestID;
                BizAction.ApprovalRequestUnitID = ((clsBillVO)dgBillList.SelectedItem).ApprovalRequestUnitID;
                if (IsAuditTrail)   // By Umesh For Audit Trail
                {
                    LogInformation = new LogInfo();
                    LogInformation.guid = new Guid();
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 29 : Reject Approval Request " + "\r\n"
                                            + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                            + "Approval Request ID : " + Convert.ToString(BizAction.ApprovalRequestID) + " "
                                            + "Approval Request UnitID : " + Convert.ToString(BizAction.ApprovalRequestUnitID) + " "
                                            + "\r\n";
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
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Rejected Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            FillBillSearchList();
                            RejectionFlag = 0;
                        };
                        msgW.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        RejectionFlag = 0;
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
    }
}

