using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
using PalashDynamics.ValueObjects.Inventory.Quotation;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.RateContract;
using System.Text;
using System.Windows.Input;
using PalashDynamics.ValueObjects.Inventory.Indent;
using PalashDynamics.Pharmacy.Inventory;
using PalashDynamics.ValueObjects.Administration.UserRights;

namespace PalashDynamics.Pharmacy.Inventory
{
    //Added by priyanka bb
    public partial class frmApprovePO : UserControl
    {
        public frmApprovePO()
        {
            InitializeComponent();

            this.DataContext = new clsPurchaseOrderVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            grdpodetails.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(grdpodetails_CellEditEnded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPurchaseOrderVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 4;
            //=====================================================

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillCurrency();
            dtpDate.SelectedDate = DateTime.Now.Date;
            dtpDate.IsEnabled = false;
            CurrencyList = new List<MasterListItem>();
            if (!IsPageLoded)
            {
                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
                //FillStore1();
                FillDelivery();
                FillSchedule();
                FillTermsOfPayment();
                FillStore();
                FillModeofPayment();
                FillDeliveryDays();
                rdbIndent.IsChecked = true;

                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
                PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                grdpodetails.ItemsSource = null;
                grdpodetails.ItemsSource = null;
                purchaseOrderList = new ObservableCollection<clsPurchaseOrderVO>();
                dgPOList.ItemsSource = PurchaseOrderItems;
                dgPOList.ItemsSource = purchaseOrderList;
                FillPurchseOrderTerms();
                dtpDate.SelectedDate = DateTime.Now.Date;

                DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                dtpFromDate.SelectedDate = firstDayOfMonth; //DateTime.Now.Date;

                dtpToDate.SelectedDate = DateTime.Now.Date;
                rdbCash.IsChecked = true;
                SetCommandButtonState("New");
                IsPageLoded = true;
            }

        }

        #region Variable Declration
        private SwivelAnimation objAnimation;
        Boolean IsPageLoded = false;
        public ObservableCollection<clsPurchaseOrderDetailVO> PurchaseOrderItems { get; set; }
        public ObservableCollection<clsPurchaseOrderDetailVO> POIndentdetails { get; set; }
        public ObservableCollection<clsPurchaseOrderVO> purchaseOrderList { get; set; }
        public List<MasterListItem> PurchaseOrderTerms { get; set; }
        public long _EnquiryID { get; set; }
        public Boolean ISEditMode = false;
        public PurchaseOrderSource enmPurchaseOorder { get; set; }
        public CheckBox chkFreez;
        public string msgTitle = "Palash";
        public string msgText = String.Empty;
        WaitIndicator indicator = new WaitIndicator();
        public List<MasterListItem> CurrencyList { get; set; }

        #endregion

        public enum PurchaseOrderSource
        {
            Indent,
            Quotation
        };

        //Added By CDS
        public bool IsFromDirectApprove = false;

        public enum PaymentMode
        {
            Cash = 1,
            Cheque = 2,
            AccountTransfer = 3
        };


        #region 'Paging'

        public PagedSortableCollectionView<clsPurchaseOrderVO> DataList { get; private set; }

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


        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillPurchaseOrderDataGrid();
        }

        #endregion

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {

            lngSupplierID = -1;
            SetCommandButtonState("ClickNew");
            PurchaseOrderItems.Clear();
            POIndentdetails.Clear();
            cboStore.SelectedValue = (long)0;
            cmbStore.SelectedValue = (long)0;
            cmbDeliveryDays.SelectedValue = (long)0;
            //FillStore();
            cboSupplier.SelectedValue = (long)0;
            cmbSupplier.SelectedValue = (long)0;
            //FillSupplier();
            cboDelivery.SelectedValue = (long)0;
            cmbModeofPayment.SelectedValue = (long)0;
            cboTOP.SelectedValue = (long)0;

            FillDelivery();
            cmbSchedule.SelectedValue = (long)0;
            FillSchedule();
            FillTermsOfPayment();
            txtDiscountAmount.Text = string.Empty;
            txtGrossAmount.Text = string.Empty;
            txtGuarantee.Text = string.Empty;
            txtIndentNo.Text = string.Empty;
            txtNETAmount.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtVATAmount.Text = string.Empty;
            ISEditMode = false;
            if (cmbTermsNCondition.ItemsSource != null)
            {
                foreach (MasterListItem item in PurchaseOrderTerms)
                {
                    item.Status = false;
                }
                cmbTermsNCondition.SelectedItem = PurchaseOrderTerms[0];
            }

            BdrItemSearch.Visibility = Visibility.Collapsed;
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            PurchaseOrderItems.Clear();
            POIndentdetails.Clear();
            SetCommandButtonState("New");
            cboStore.SelectedValue = (long)0;
            cmbStore.SelectedValue = (long)0;
            FillStore();
            cboSupplier.SelectedValue = (long)0;
            cmbSupplier.SelectedValue = (long)0;

            cboDelivery.SelectedValue = (long)0;
            FillDelivery();
            cmbSchedule.SelectedValue = (long)0;
            FillSchedule();

            FillTermsOfPayment();
            txtDiscountAmount.Text = string.Empty;
            txtGrossAmount.Text = string.Empty;
            txtGuarantee.Text = string.Empty;
            txtNETAmount.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtVATAmount.Text = string.Empty;
            if (cmbTermsNCondition.ItemsSource != null)
            {
                foreach (MasterListItem item in PurchaseOrderTerms)
                {
                    item.Status = false;
                }
                cmbTermsNCondition.SelectedItem = PurchaseOrderTerms[0];
            }

            objAnimation.Invoke(RotationType.Backward);


        }



        private void FillSchedule()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Schedule;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cmbSchedule.ItemsSource = null;
                        cmbSchedule.ItemsSource = objList;
                        if (objList != null)
                        {
                            cmbSchedule.SelectedItem = objList[0];
                        }
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

        // OLD FILL Store 
        //private void FillStore()
        //{
        //    try
        //    {
        //        clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();

        //        //False when we want to fetch all items
        //        clsItemMasterVO obj = new clsItemMasterVO();
        //        List<clsStoreVO> objList = new List<clsStoreVO>();

        //        obj.RetrieveDataFlag = false;
        //        BizActionObj.ItemMatserDetails = new List<clsStoreVO>();

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            if (args.Error == null && args.Result != null)
        //            {
        //                BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
        //                clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };

        //                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
        //                {
        //                    var result = from item in BizActionObj.ItemMatserDetails
        //                                 where item.Status == true // item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId &&
        //                                 select item;
        //                    objList = (List<clsStoreVO>)result.ToList();
        //                }
        //                else
        //                {
        //                    var result = from item in BizActionObj.ItemMatserDetails
        //                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
        //                                 select item;
        //                    objList = (List<clsStoreVO>)result.ToList();
        //                }                      

        //                objList.Insert(0, Default);
        //                if (objList != null)
        //                {
        //                    cmbStore.ItemsSource = null;
        //                    cmbStore.ItemsSource = objList;
        //                    cboStore.ItemsSource = null;
        //                    cboStore.ItemsSource = objList;

        //                    cmbStore.SelectedItem = objList[0];
        //                    cboStore.SelectedItem = objList[0];
        //                }

        //                FillSupplier();
        //            }

        //        };

        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        // OLD FILL Store 

        List<clsStoreVO> objStoreList = new List<clsStoreVO>();
        ////// User Rights Wise Fill Store 
        private void FillStore()
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    clsStoreVO select = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                 select item;

                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, Default);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            //cmbSearchStore.ItemsSource = result.ToList();
                            //cmbSearchStore.SelectedItem = result.ToList()[0];
                            //cmbStore.ItemsSource = result.ToList();
                            //cmbStore.SelectedItem = result.ToList()[0];

                            objStoreList = (List<clsStoreVO>)result.ToList();
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores.ToList().Count > 0)
                        {
                            //cmbSearchStore.ItemsSource = BizActionObj.ToStoreList.ToList();
                            //cmbSearchStore.SelectedItem = BizActionObj.ToStoreList.ToList()[0];
                            //cmbStore.ItemsSource = BizActionObj.ToStoreList.ToList();
                            //cmbStore.SelectedItem = BizActionObj.ToStoreList.ToList()[0];

                            objStoreList = (List<clsStoreVO>)NonQSAndUserDefinedStores.ToList();
                        }
                    }

                    //objStoreList.Insert(0, Default);
                    if (objStoreList != null)
                    {
                        cmbStore.ItemsSource = null;
                        cmbStore.ItemsSource = objStoreList;
                        cboStore.ItemsSource = null;
                        cboStore.ItemsSource = objStoreList;

                        cmbStore.SelectedItem = objStoreList[0];
                        cboStore.SelectedItem = objStoreList[0];

                    }

                    FillSupplier();

                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        // User Rights Wise Fill Store 


        //Commented By CDS
        //private void FillStore1()
        //{
        //    try
        //    {
        //        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;

        //        long clinicId = User.UserLoginInfo.UnitId;
        //        clsGetMasterListBizActionVO BizAction1 = new clsGetMasterListBizActionVO();
        //        //BizAction.IsActive = true;
        //        BizAction1.MasterTable = MasterTableNameList.M_Store;
        //        BizAction1.MasterList = new List<MasterListItem>();

        //        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
        //        {
        //            if (clinicId > 0)
        //            {
        //                BizAction1.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
        //            }
        //        }

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, e) =>
        //        {
        //            if (e.Error == null && e.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                objList.Add(new MasterListItem(0, "- Select -"));
        //                objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //                cmbStore.ItemsSource = null;
        //                cmbStore.ItemsSource = objList;

        //                if (objList.Count > 1)
        //                    cmbStore.SelectedItem = objList[1];
        //                else
        //                    cmbStore.SelectedItem = objList[0];
        //            }

        //        };
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        //Commented By CDS

        private void FillTermsOfPayment()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_TermsofPayment;
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

                        cboTOP.ItemsSource = null;
                        cboTOP.ItemsSource = objList;

                        if (objList.Count > 1)
                            cboTOP.SelectedItem = objList[1];
                        else
                            cboTOP.SelectedItem = objList[0];
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

        private void FillDeliveryDays()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_TermsofPayment;
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

                        cmbDeliveryDays.ItemsSource = null;
                        cmbDeliveryDays.ItemsSource = objList;
                        cmbDeliveryDays.SelectedItem = objList[0];
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

        private void FillDelivery()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Store;
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

                        cboDelivery.ItemsSource = null;
                        cboDelivery.ItemsSource = objList;
                        if (objList != null)
                        {
                            cboDelivery.SelectedItem = objList[0];
                        }
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

        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

                BizAction.IsFromPOGRN = true;
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        foreach (var item in objList)
                        {
                            item.Description = item.Code + " - " + item.Description;
                        }
                        cboSupplier.ItemsSource = null;
                        cboSupplier.ItemsSource = objList;
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;

                        cboSupplier.SelectedItem = objList[0];
                        cmbSupplier.SelectedItem = objList[0];
                        //FillPurchaseOrderDataGrid();
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

        public void FillModeofPayment()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ModeOfPayment;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbModeofPayment.ItemsSource = null;
                        cmbModeofPayment.ItemsSource = objList;
                        cmbModeofPayment.SelectedItem = objList[0];
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Fills the Currency Master.
        /// </summary>
        private void FillCurrency()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Currency;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CurrencyList = objList;
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FillPurchseOrderTerms()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_TermAndCondition;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        PurchaseOrderTerms = new List<MasterListItem>();
                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        PurchaseOrderTerms.Add(new MasterListItem(0, "-- Select --"));
                        PurchaseOrderTerms.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        foreach (var item in PurchaseOrderTerms)
                        {
                            item.Status = false;


                        }
                        cmbTermsNCondition.ItemsSource = null;
                        cmbTermsNCondition.ItemsSource = PurchaseOrderTerms;

                        if (PurchaseOrderTerms != null)
                        {
                            cmbTermsNCondition.SelectedItem = PurchaseOrderTerms[0];
                        }
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
        long lngSupplierID = -1;
        private void cmdIndent_Click(object sender, RoutedEventArgs e)
        {
            //grdpodetails.Columns[3].IsReadOnly = rdbIndent.IsChecked == true ? true : false;

            if (((clsStoreVO)cboStore.SelectedItem).StoreId == null || ((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else if (((MasterListItem)cboSupplier.SelectedItem).ID == 0 && rdbDirect.IsChecked == true) //|| cboSupplier.SelectedItem == null
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else if (rdbIndent.IsChecked == true)
            {
                // grdpodetails.Columns[3].IsReadOnly = rdbIndent.IsChecked == true ? true : false;
                enmPurchaseOorder = PurchaseOrderSource.Indent;
                SupplierIndentSearch objIndentSearch = new SupplierIndentSearch();
                objIndentSearch.IsOnlyItems = true;
                objIndentSearch.cmbToIndentStore.SelectedItem = (cboStore.SelectedItem);
                objIndentSearch.ToStoreID = ((PalashDynamics.ValueObjects.Inventory.clsStoreVO)(cboStore.SelectedItem)).StoreId;
                objIndentSearch.OnItemSelectionCompleted += new SupplierIndentSearch.ItemSelection(objIndentSearch_OnItemSelectionCompleted);
                objIndentSearch.Show();
            }
            else if (rdbDirect.IsChecked == true)
            {
                ItemList Itemswin = new ItemList();
                Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Itemswin.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                Itemswin.cmbStore.IsEnabled = false;
                Itemswin.ShowBatches = false;
                Itemswin.ShowQuantity = true;
                Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                Itemswin.Show();
            }
        }

        string IndentNo = String.Empty;

        void objIndentSearch_OnItemSelectionCompleted(object sender, EventArgs e)
        {

            String strItemIDs = string.Empty;
            SupplierIndentSearch objItemSearch = (SupplierIndentSearch)sender;
            ObservableCollection<clsItemListByIndentId> selectedIndentItems;

            objItemSearch.IsOnlyItems = true;
            selectedIndentItems = objItemSearch.ocSelectedItemList;
            if (lngSupplierID == -1 || lngSupplierID != selectedIndentItems[0].SupplierID)
            {
                PurchaseOrderItems.Clear();
                PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();

                POIndentdetails.Clear();
                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
            }

            lngSupplierID = selectedIndentItems[0].SupplierID;


            // Get the PO list for multiple indent
            if (selectedIndentItems != null && selectedIndentItems.Count > 0)
            {
                foreach (clsItemListByIndentId item in selectedIndentItems)
                {

                    var item1 = from r in POIndentdetails
                                where (r.ItemID == item.ItemId && r.IndentID == item.IndentId && r.IndentUnitID == item.IndentUnitID)
                                select r;
                    if (item1.ToList().Count == 0)
                    {

                        if (!txtIndentNo.Text.Trim().Contains(item.IndentNumber.Trim()))
                            txtIndentNo.Text = (txtIndentNo.Text + "," + item.IndentNumber.Trim()).Trim(',');

                        var results1 = from r in ((List<MasterListItem>)cboSupplier.ItemsSource)
                                       where r.ID == item.SupplierID
                                       select r;

                        foreach (MasterListItem Supplier in results1)
                        {
                            cboSupplier.SelectedItem = Supplier;
                        }

                        POIndentdetails.Add(new clsPurchaseOrderDetailVO { IndentID = Convert.ToInt64(item.IndentId), IndentUnitID = Convert.ToInt64(item.IndentUnitID), IndentDetailID = item.IndentDetailsID, IndentDetailUnitID = item.IndentUnitID, ItemID = item.ItemId, ItemName = item.ItemName, ItemGroup = item.ItemGroup, ItemCategory = item.ItemCategory, ItemCode = item.ItemCode, PUM = item.PUM, Quantity = Convert.ToDecimal(item.BalanceQty), Rate = (decimal)item.PurchaseRate, VATPercent = (decimal)item.VATPer, MRP = (decimal)item.MRP, IndentNumber = item.IndentNumber });

                        // if same indent item is already get added plz ignore that item
                        if (PurchaseOrderItems.Count > 0)
                        {
                            int Purchase = (from r in PurchaseOrderItems
                                            where (r.ItemID == item.ItemId)
                                            select r).ToList().Count;
                            if (Purchase > 0)
                            {
                                clsPurchaseOrderDetailVO obj = PurchaseOrderItems.Where(z => z.ItemID == item.ItemId).FirstOrDefault();

                                if (obj != null)
                                {
                                    obj.Quantity += Convert.ToDecimal(item.BalanceQty);
                                }
                            }

                            else
                            {
                                PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO { IndentID = Convert.ToInt64(item.IndentId), IndentUnitID = Convert.ToInt64(item.IndentUnitID), ItemID = item.ItemId, ItemGroup = item.ItemGroup, ItemCategory = item.ItemCategory, ItemName = item.ItemName, ItemCode = item.ItemCode, PUM = item.PUM, Quantity = Convert.ToDecimal(item.BalanceQty), Rate = (decimal)item.PurchaseRate, VATPercent = (decimal)item.VATPer, MRP = (decimal)item.MRP, IndentNumber = item.IndentNumber, RateContractID = -1, RateContractUnitID = -1, CurrencyList = CurrencyList, SelectedCurrency = CurrencyList.Where(z => z.ID == 1).FirstOrDefault() });
                                strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemId) + ",");
                            }
                        }
                        else
                        {
                            PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO { IndentID = Convert.ToInt64(item.IndentId), IndentUnitID = Convert.ToInt64(item.IndentUnitID), ItemID = item.ItemId, ItemName = item.ItemName, ItemGroup = item.ItemGroup, ItemCategory = item.ItemCategory, ItemCode = item.ItemCode, PUM = item.PUM, Quantity = Convert.ToDecimal(item.BalanceQty), Rate = (decimal)item.PurchaseRate, VATPercent = (decimal)item.VATPer, MRP = (decimal)item.MRP, IndentNumber = item.IndentNumber, RateContractID = -1, RateContractUnitID = -1, CurrencyList = CurrencyList, SelectedCurrency = CurrencyList.Where(z => z.ID == 1).FirstOrDefault() });
                            strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemId) + ",");
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Item Purchase Requisition combination already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgW1.Show();
                    }
                }
            }
            grdpodetails.ItemsSource = null;
            grdpodetails.ItemsSource = PurchaseOrderItems;

            grdpodetails.Focus();
            grdpodetails.UpdateLayout();
            CalculateOpeningBalanceSummary();
            CheckRateContractForItems(strItemIDs.Trim(','));
        }
        List<clsRateContractMasterVO> lstRateContract;
        List<clsRateContractItemDetailsVO> lstRateContractItemDetails;

        /// <summary>
        /// This method is used to check for the rate contract for the items 
        /// </summary>
        /// <param name="strItemIDs"></param>

        public void CheckRateContractForItems(string strItemIDs)
        {
            WaitIndicator indicator = new WaitIndicator();
            indicator.Show();
            clsGetRateContractAgainstSupplierAndItemBizActionVO objBizAction = new clsGetRateContractAgainstSupplierAndItemBizActionVO();
            objBizAction.ItemIDs = strItemIDs;
            if (cboSupplier.SelectedItem != null)
                objBizAction.SupplierID = ((MasterListItem)cboSupplier.SelectedItem).ID;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        objBizAction = args.Result as clsGetRateContractAgainstSupplierAndItemBizActionVO;
                        lstRateContract = new List<clsRateContractMasterVO>();
                        lstRateContractItemDetails = new List<clsRateContractItemDetailsVO>();
                        lstRateContract = objBizAction.RateContractMasterList;
                        lstRateContractItemDetails = objBizAction.RateContractItemDetailsList;
                        // Group Contract by ITem ID
                        GetRateContractDiscount();
                    }
                };

                client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                indicator.Close();
            }
        }

        private void GetRateContractDiscount()
        {
            var lstGroupByItemID = lstRateContractItemDetails.GroupBy(z => z.ItemID).ToList();
            for (int iCount = 0; iCount < lstGroupByItemID.Count; iCount++)
            {
                // Check weather the Item having a more than one Rate Contract.
                var lstContracts = lstGroupByItemID[iCount].ToList();
                if (lstContracts != null && lstContracts.Count > 1)
                {
                    List<clsPurchaseOrderDetailVO> lstPOItems = PurchaseOrderItems.Where(z => z.ItemID == lstContracts[0].ItemID).ToList();
                    foreach (clsPurchaseOrderDetailVO objPODetail in lstPOItems)
                    {
                        objPODetail.IsMultipleContract = true;
                    }
                }
                // If the Item Having a single rate contract then assign the discount for that item.
                else if (lstContracts != null && lstContracts.Count == 1)
                {
                    List<clsPurchaseOrderDetailVO> lstPODetail = PurchaseOrderItems.Where(POItems => POItems.ItemID == lstContracts[0].ItemID).ToList();
                    foreach (clsPurchaseOrderDetailVO objPODetail in lstPODetail)
                    {
                        if (objPODetail != null && objPODetail.RateContractID == -1)
                        {
                            objPODetail.RateContractID = lstContracts[0].RateContractID;
                            objPODetail.RateContractUnitID = lstContracts[0].RateContractUnitID;
                            objPODetail.RateContractCondition = lstContracts[0].Condition;
                            if (objPODetail.Quantity > 0)
                            {
                                switch (lstContracts[0].Condition)
                                {
                                    case "Between":
                                        if (Convert.ToDouble(objPODetail.Quantity) >= lstContracts[0].MinQuantity && Convert.ToDouble(objPODetail.Quantity) <= lstContracts[0].MaxQuantity)
                                        {
                                            objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                            objPODetail.ConditionFound = true;
                                        }
                                        break;
                                    case ">":
                                        if (Convert.ToDouble(objPODetail.Quantity) > lstContracts[0].Quantity)
                                        {
                                            objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                            objPODetail.ConditionFound = true;
                                        }
                                        break;
                                    case "<":
                                        if (Convert.ToDouble(objPODetail.Quantity) < lstContracts[0].Quantity)
                                        {
                                            objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                            objPODetail.ConditionFound = true;
                                        }
                                        break;
                                    case "No Limit":
                                        objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                        objPODetail.ConditionFound = true;

                                        break;
                                    case "=":
                                        if (Convert.ToDouble(objPODetail.Quantity) == lstContracts[0].Quantity)
                                        {
                                            objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                            objPODetail.ConditionFound = true;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            CalculateOpeningBalanceSummary();
        }
        /// <summary>
        /// On View link Click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkViewContractDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdpodetails.SelectedItem != null)
                {
                    clsPurchaseOrderDetailVO objPODEtails = (clsPurchaseOrderDetailVO)grdpodetails.SelectedItem;

                    if (lstRateContract != null && lstRateContractItemDetails != null && lstRateContractItemDetails.Count > 1 && lstRateContract.Count > 1)
                    {

                        //Show Rate Contract Details.
                        frmContractsForItems frmContract = new frmContractsForItems();
                        //frmContract.lstPurchaseOrderItems = PurchaseOrderItems.ToList();
                        frmContract.POItem = objPODEtails;
                        frmContract.lstRateContract = lstRateContract;
                        frmContract.lstRateContractItemDetails = lstRateContractItemDetails.Where(PODetails => PODetails.ItemID == objPODEtails.ItemID).ToList();
                        frmContract.OnOKButton_Click += new RoutedEventHandler(SaveRateContract_Click);
                        frmContract.Show();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        /// <summary>
        /// Save the discount for the item after selecting the valid condition.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SaveRateContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsPurchaseOrderDetailVO objPODetail = (clsPurchaseOrderDetailVO)grdpodetails.SelectedItem;
                frmContractsForItems frmContract = (frmContractsForItems)sender;
                if (frmContract.DialogResult == true && frmContract.dgRateContracts.SelectedItem != null)
                {
                    ItemWiseRateContracts objItemWiseRateContracts = ((ItemWiseRateContracts)frmContract.dgRateContracts.SelectedItem);
                    objPODetail.DiscountPercent = Convert.ToDecimal(objItemWiseRateContracts.DiscountPercent);
                    objPODetail.RateContractID = objItemWiseRateContracts.RateContractID;
                    objPODetail.RateContractUnitID = objItemWiseRateContracts.RateContractUnitID;
                    objPODetail.RateContractCondition = objItemWiseRateContracts.Condition;
                    CalculateOpeningBalanceSummary();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillPurchaseOrderDataGrid();
        }
        int ClickedFlag1 = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClickedFlag1 = ClickedFlag1 + 1;
                if (ClickedFlag1 == 1)
                {
                    bool isValid = true;
                    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                    if (PurchaseOrderItems == null || PurchaseOrderItems.Count == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, "At least one item is compulsory for saving purchase order", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        isValid = false;
                        ClickedFlag1 = 0;
                        return;

                    }
                    if (((clsStoreVO)cboStore.SelectedItem) == null || ((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
                    {
                        cboStore.TextBox.SetValidation("Please Select The Store");
                        cboStore.TextBox.RaiseValidationError();
                        cboStore.Focus();
                        isValid = false;
                        ClickedFlag1 = 0;
                        return;

                    }
                    else
                        cboStore.TextBox.ClearValidationError();

                    if (((MasterListItem)cboDelivery.SelectedItem) == null || ((MasterListItem)cboDelivery.SelectedItem).ID == 0)
                    {
                        cboDelivery.TextBox.SetValidation("Please Select The Delivery Location");
                        cboDelivery.TextBox.RaiseValidationError();
                        cboDelivery.Focus();
                        isValid = false;
                        ClickedFlag1 = 0;
                        return;

                    }
                    else
                        cboDelivery.TextBox.ClearValidationError();


                    if (((MasterListItem)cboSupplier.SelectedItem) == null || ((MasterListItem)cboSupplier.SelectedItem).ID == 0)
                    {
                        cboSupplier.TextBox.SetValidation("Please Select The Supplier");
                        cboSupplier.TextBox.RaiseValidationError();
                        cboSupplier.Focus();
                        isValid = false;
                        ClickedFlag1 = 0;
                        return;
                    }
                    else
                        cboSupplier.TextBox.ClearValidationError();
                    //////////Commented By YK 10032017////////For Unwanted popup msg ////////////////////
                    //if (PurchaseOrderItems.Where(z => z.SelectedCurrency == null).Any())
                    //{
                    //    //MessageBoxControl.MessageBoxChildWindow msgW4 =
                    //    //                          new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Please select Currency.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    //msgW4.Show();
                    //    //isValid = false;
                    //    //ClickedFlag1 = 0;
                    //    //return;
                    //}
                    //if (PurchaseOrderItems.GroupBy(z => z.SelectedCurrency.ID).Count() > 1)
                    //{
                    //    //MessageBoxControl.MessageBoxChildWindow msgW4 =
                    //    //                           new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Currency must be same for all items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    //msgW4.Show();
                    //    //isValid = false;
                    //    //ClickedFlag1 = 0;
                    //    //return;
                    //}
                    //////////////////////////////END////////////////////////////////////////////////////////
                    foreach (var item in PurchaseOrderItems)
                    {
                        if (item.ItemGroup != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
                        {
                            if (item.MRP < item.Rate)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW4 =
                                                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, "MRP must be greater than the cost price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW4.Show();
                                isValid = false;
                                ClickedFlag1 = 0;
                                return;
                            }
                        }
                    }
                    if (PurchaseOrderItems != null && PurchaseOrderItems.Count > 0)
                    {
                        List<clsPurchaseOrderDetailVO> objList = PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>();
                        if (objList != null && objList.Count > 0)
                        {
                            foreach (var item in objList)
                            {
                                if (item.Quantity == 0)
                                {
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                    isValid = false;
                                    break;
                                }
                                if (item.DiscountPercent > 100)
                                {
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Discount percent must not be greater than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                    isValid = false;
                                    break;
                                }
                            }
                            //return true;
                        }
                    }
                    else
                        isValid = true;
                    if (isValid)
                    {
                        msgTitle = "Palash";

                        if (ISEditMode == false)
                            msgText = "Are you sure you want to Save Purchase Order?";
                        else if (ISEditMode == true)
                            msgText = "Are you sure you want to Update Purchase Order?";

                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                        msgWin.Show();


                    }
                    else
                        ClickedFlag1 = 0;
                }
            }
            catch (Exception Ex)
            {
                ClickedFlag1 = 0;
                throw;
            }
        }

        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {

            if (result == MessageBoxResult.Yes)
            {
                SavePO();
            }
            else if (result == MessageBoxResult.No)
                ClickedFlag1 = 0;
        }

        private void SavePO()
        {
            clsAddPurchaseOrderBizActionVO objBizActionVO = new clsAddPurchaseOrderBizActionVO();
            ObservableCollection<clsPurchaseOrderDetailVO> objObservable = ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource));

            #region Test
            IApplicationConfiguration applicationConfiguration = ((IApplicationConfiguration)App.Current);
            ObservableCollection<clsPurchaseOrderDetailVO> ocPOItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
            foreach (clsPurchaseOrderDetailVO item in objObservable)
            {
                if (ocPOItems.Count > 0 && ocPOItems.Where(poItems => poItems.ItemID == item.ItemID && poItems.IndentID != item.IndentID).Any() == true)
                {
                    clsPurchaseOrderDetailVO obj = ocPOItems.Where(poItems => poItems.ItemID == item.ItemID && poItems.IndentID != item.IndentID).ToList().First();
                    obj.Quantity += Convert.ToDecimal(item.Quantity);
                }
                else
                {
                    ocPOItems.Add(new clsPurchaseOrderDetailVO { IndentID = Convert.ToInt64(item.IndentID), IndentUnitID = Convert.ToInt64(item.IndentUnitID), ItemID = item.ItemID, ItemName = item.ItemName, ItemCode = item.ItemCode, PUM = item.PUM, Quantity = Convert.ToDecimal(item.Quantity), Rate = (decimal)item.Rate, VATPercent = (decimal)item.VATPer, MRP = (decimal)item.MRP, IndentNumber = item.IndentNumber });
                }
                // if same indent item is get added
            }
            #endregion Test

            objBizActionVO.PurchaseOrder = new clsPurchaseOrderVO();
            objBizActionVO.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            objBizActionVO.PurchaseOrder.Date = dtpDate.SelectedDate.Value;
            objBizActionVO.PurchaseOrder.Time = DateTime.Now;
            objBizActionVO.PurchaseOrder.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
            objBizActionVO.PurchaseOrder.SupplierID = ((MasterListItem)cboSupplier.SelectedItem).ID;
            objBizActionVO.IsEditMode = ISEditMode;
            objBizActionVO.PurchaseOrder.EditForApprove = true;
            objBizActionVO.PurchaseOrder.ID = ((clsPurchaseOrderVO)this.DataContext).ID;
            if (enmPurchaseOorder == PurchaseOrderSource.Indent)
            {
                //objBizActionVO.PurchaseOrder.IndentID = _IndentID;
                //objBizActionVO.PurchaseOrder.IndentUnitID = _IndentUnitID;
                objBizActionVO.PurchaseOrder.EnquiryID = 0;


            }
            else if (enmPurchaseOorder == PurchaseOrderSource.Quotation)
            {
                objBizActionVO.PurchaseOrder.IndentID = 0;
                objBizActionVO.PurchaseOrder.EnquiryID = _EnquiryID;
            }

            if (rdbDirect.IsChecked == true)
            {
                objBizActionVO.PurchaseOrder.Direct = true;
            }

            objBizActionVO.PurchaseOrder.DeliveryDuration = ((MasterListItem)cboDelivery.SelectedItem).ID;

            objBizActionVO.PurchaseOrder.PaymentModeID = ((MasterListItem)cmbModeofPayment.SelectedItem).ID;
            objBizActionVO.PurchaseOrder.PaymentTerms = ((MasterListItem)cboTOP.SelectedItem).ID;
            objBizActionVO.PurchaseOrder.DeliveryDays = ((MasterListItem)cmbDeliveryDays.SelectedItem).ID;
            objBizActionVO.PurchaseOrder.Guarantee_Warrantee = ((clsPurchaseOrderVO)this.DataContext).Guarantee_Warrantee;
            objBizActionVO.PurchaseOrder.Schedule = ((MasterListItem)cmbSchedule.SelectedItem).ID;
            objBizActionVO.PurchaseOrder.Remarks = ((clsPurchaseOrderVO)this.DataContext).Remarks;
            objBizActionVO.PurchaseOrder.TotalAmount = ((clsPurchaseOrderVO)this.DataContext).TotalAmount;
            objBizActionVO.PurchaseOrder.TotalDiscount = ((clsPurchaseOrderVO)this.DataContext).TotalDiscount;
            objBizActionVO.PurchaseOrder.TotalVAT = ((clsPurchaseOrderVO)this.DataContext).TotalVAT;
            objBizActionVO.PurchaseOrder.TotalNet = ((clsPurchaseOrderVO)this.DataContext).TotalNet;
            objBizActionVO.PurchaseOrder.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            objBizActionVO.PurchaseOrder.AddedDateTime = DateTime.Now;
            objBizActionVO.PurchaseOrder.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
            objBizActionVO.PurchaseOrder.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
            objBizActionVO.PurchaseOrder.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            objBizActionVO.PurchaseOrder.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            objBizActionVO.PurchaseOrder.UpdatedDateTime = DateTime.Now;
            objBizActionVO.PurchaseOrder.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
            objBizActionVO.PurchaseOrder.UpdatedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
            objBizActionVO.PurchaseOrder.UpdatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            objBizActionVO.PurchaseOrder.Items = objObservable.ToList<clsPurchaseOrderDetailVO>();
            objBizActionVO.POIndentList = POIndentdetails.ToList();
            objBizActionVO.POTerms = new List<clsPurchaseOrderTerms>();
            foreach (var item in PurchaseOrderTerms)
            {
                if (item.Status == true && item.ID != 0)
                {
                    objBizActionVO.POTerms.Add(new clsPurchaseOrderTerms { TermsAndConditionID = item.ID, Status = item.Status });
                }
            }
            if (objBizActionVO.PurchaseOrder.Items.Count > 0)
            {
                WaitIndicator indicator = new WaitIndicator();
                indicator.Show();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag1 = 0;
                    if (arg.Error == null && ((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder != null)
                    {
                        if (arg.Result != null)
                        {
                            if (ISEditMode == false)
                                msgText = "Purchase Order Saved Successfully With PO Number";
                            else if (ISEditMode == true)
                                msgText = "Purchase Order Updated Successfully With PO Number";

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText + " " + ((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder.PONO, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            SetCommandButtonState("Save");
                            msgW1.Show();
                            PurchaseOrderItems.Clear();
                            POIndentdetails.Clear();
                            POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();

                            objAnimation.Invoke(RotationType.Backward);
                            FillPurchaseOrderDataGrid();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Some error occurred while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                indicator.Close();
                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Please select Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;
            String strItemIDs = string.Empty;
            if (Itemswin.SelectedItems != null)
            {
                foreach (var item in Itemswin.SelectedItems)
                {
                    if (PurchaseOrderItems.Where(poItems => poItems.ItemID == item.ID).Any() == false)
                        PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO { ItemID = item.ID, ItemName = item.ItemName, ItemCode = item.ItemCode, PUM = item.PUOM, VATPercent = item.VatPer, Rate = item.PurchaseRate, Quantity = Convert.ToDecimal(item.RequiredQuantity), MRP = item.MRP, RateContractID = -1, RateContractUnitID = -1, RateContractCondition = null, CurrencyList = CurrencyList, SelectedCurrency = CurrencyList.Where(z => z.ID == 1).FirstOrDefault() }); //, MRP = item.MRP
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Product is already added!", "Please add another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                }

                foreach (var item2 in PurchaseOrderItems)
                {
                    strItemIDs = String.Format(strItemIDs + Convert.ToString(item2.ItemID) + ",");

                }
                grdpodetails.ItemsSource = null;
                grdpodetails.ItemsSource = PurchaseOrderItems;
                CheckRateContractForItems(strItemIDs.Trim(','));
                CalculateOpeningBalanceSummary();
                grdpodetails.Focus();
                grdpodetails.UpdateLayout();
                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
            }
        }

        private void FillPurchaseOrderDataGrid()
        {
            try
            {
                indicator.Show();
                clsGetPurchaseOrderBizActionVO objBizActionVO = new clsGetPurchaseOrderBizActionVO();
                if (dtpFromDate.SelectedDate != null)
                    objBizActionVO.searchFromDate = dtpFromDate.SelectedDate.Value;
                if (dtpToDate.SelectedDate != null)
                    objBizActionVO.searchToDate = dtpToDate.SelectedDate.Value;
                if (cmbStore.SelectedItem != null)
                {
                    objBizActionVO.SearchStoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                }

                if (cmbSupplier.SelectedItem != null)
                {
                    objBizActionVO.SearchSupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                }
                if (chkCancelPO.IsChecked == true)
                {
                    objBizActionVO.CancelPO = true;
                }
                if (rdbUnApproved.IsChecked == true)
                {
                    objBizActionVO.UnApprovePo = true;
                }
                if (rdbApproved.IsChecked == true)
                {
                    objBizActionVO.ApprovePo = true;
                }
                objBizActionVO.Freezed = true;

                objBizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.PagingEnabled = true;
                objBizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                objBizActionVO.MaximumRows = DataList.PageSize;
                objBizActionVO.PONO = txtPONO.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetPurchaseOrderBizActionVO objPurchaseOrderList = ((clsGetPurchaseOrderBizActionVO)arg.Result);
                            dgPOList.ItemsSource = null;
                            DataList.Clear();
                            DataList.TotalItemCount = objPurchaseOrderList.TotalRowCount;
                            if (objPurchaseOrderList.PurchaseOrderList != null)
                            {
                                foreach (var item in objPurchaseOrderList.PurchaseOrderList)
                                {
                                    //if (item.IsApproveded == true)
                                    //{
                                    //    item.IsEnable = false;
                                    //}
                                    //else
                                    //{
                                    //    item.IsEnable = true;
                                    //}
                                    DataList.Add(item);
                                }
                                dgPOList.ItemsSource = null;
                                dgPOList.ItemsSource = DataList;
                                grdpo.ItemsSource = null;
                                purchaseOrderList.Clear();
                                datapager.Source = DataList;
                            }
                            txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();
                            indicator.Close();
                        }
                        else
                            indicator.Close();
                    }
                    else
                    {
                        System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Opening Balance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Modify";
                    cmdCancel.IsEnabled = false;
                    cmdPrint.IsEnabled = true;
                    if (chkCancelPO.IsChecked == true)
                    {
                        cmdApprove.IsEnabled = false;
                    }
                    else
                    {
                        cmdApprove.IsEnabled = true;
                    }
                    break;

                case "Save":

                    cmdNew.IsEnabled = true;
                    cmdPrint.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = false;
                    cboStore.SelectedValue = 0;
                    cmbStore.SelectedValue = 0;
                    cboSupplier.SelectedValue = 0;
                    cboSupplier.SelectedValue = 0;
                    cboDelivery.SelectedValue = 0;
                    cmbSchedule.SelectedValue = 0;
                    cboTOP.SelectedValue = 0;
                    break;

                case "Modify":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdSave.Content = "Modify";
                    cmdCancel.IsEnabled = true;
                    cmdApprove.IsEnabled = false;
                    break;
                case "Approve":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Modify";
                    cmdCancel.IsEnabled = true;
                    cmdApprove.IsEnabled = false;
                    break;
                case "ClickNew":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region On Data Grid Cell Edit
        decimal orginalVatPer = 0;
        private void UpdateAllItems(clsPurchaseOrderDetailVO objDetailCopy, clsPurchaseOrderDetailVO objPODetailsVO)
        {
            decimal dVatPercent = 0;
            if (objDetailCopy.DiscountPercent > 100)
                objDetailCopy.DiscountPercent = 0;
            else
                objDetailCopy.DiscountPercent = objPODetailsVO.DiscountPercent;
            if (objPODetailsVO.DiscountPercent == 100 && objPODetailsVO.VATPercent != 0)
            {
                dVatPercent = objPODetailsVO.VATPercent;
                objDetailCopy.VATPercent = 0;
            }
            else
                objDetailCopy.VATPercent = objPODetailsVO.VATPercent;
            if (objPODetailsVO.VATPercent == 0 && objPODetailsVO.DiscountPercent < 100)
            {
                objDetailCopy.VATPercent = dVatPercent;
            }
            else
                objDetailCopy.VATPercent = objPODetailsVO.VATPercent;
            if (objPODetailsVO.VATPercent > 100 || (objPODetailsVO.DiscountPercent == 100 && objPODetailsVO.VATPercent > 0))
            {
                objDetailCopy.VATPercent = 0;
            }
            else objDetailCopy.VATPercent = objPODetailsVO.VATPercent;
            if (objPODetailsVO.MRP < objPODetailsVO.Rate)
            {
                objDetailCopy.MRP = objDetailCopy.Rate + 1;
            }
            else
                objDetailCopy.MRP = objPODetailsVO.MRP;

            objDetailCopy.Rate = objPODetailsVO.Rate;
        }

        void grdpodetails_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            try
            {
                clsPurchaseOrderDetailVO objPODetailsVO = new clsPurchaseOrderDetailVO();
                objPODetailsVO = (clsPurchaseOrderDetailVO)grdpodetails.SelectedItem;
                int iRateContractCount = -1;
                objPODetailsVO.ConditionFound = false;
                if (lstRateContractItemDetails != null && lstRateContractItemDetails.Count > 0)
                {
                    if (objPODetailsVO.RateContractID > 0 && objPODetailsVO.RateContractUnitID > 0 && !String.IsNullOrEmpty(objPODetailsVO.RateContractCondition))
                        iRateContractCount = lstRateContractItemDetails.Where(z => z.ItemID == objPODetailsVO.ItemID && z.RateContractID == objPODetailsVO.RateContractID && z.RateContractUnitID == objPODetailsVO.RateContractUnitID && z.Condition.Trim().Equals(objPODetailsVO.RateContractCondition.Trim())).Count();
                }
                if (e.Column.Header.ToString().Equals("Pack Quantity"))
                {
                    if (lstRateContractItemDetails != null && lstRateContractItemDetails.Count > 0)
                    {

                        if (iRateContractCount == 1)
                        {
                            clsRateContractItemDetailsVO objRateContractDetails = lstRateContractItemDetails.Where(z => z.ItemID == objPODetailsVO.ItemID && z.RateContractID == objPODetailsVO.RateContractID && z.RateContractUnitID == objPODetailsVO.RateContractUnitID && z.Condition == objPODetailsVO.RateContractCondition).First();

                            switch (objRateContractDetails.Condition)
                            {
                                case "Between":
                                    if (Convert.ToDouble(objPODetailsVO.Quantity) >= objRateContractDetails.MinQuantity && Convert.ToDouble(objPODetailsVO.Quantity) <= objRateContractDetails.MaxQuantity)
                                    {
                                        objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                                        objPODetailsVO.ConditionFound = true;
                                    }
                                    break;
                                case ">":
                                    if (Convert.ToDouble(objPODetailsVO.Quantity) > objRateContractDetails.Quantity)
                                    {
                                        objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                                        objPODetailsVO.ConditionFound = true;
                                    }
                                    break;
                                case "<":
                                    if (Convert.ToDouble(objPODetailsVO.Quantity) < objRateContractDetails.Quantity)
                                    {
                                        objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                                        objPODetailsVO.ConditionFound = true;
                                    }
                                    break;
                                case "No Limit":
                                    objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                                    objPODetailsVO.ConditionFound = true;
                                    break;
                                case "=":
                                    if (Convert.ToDouble(objPODetailsVO.Quantity) == objRateContractDetails.Quantity)
                                    {
                                        objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                                        objPODetailsVO.ConditionFound = true;
                                    }
                                    break;
                            }
                        }
                        else if ((iRateContractCount > 1 || objPODetailsVO.IsMultipleContract) && !String.IsNullOrEmpty(objPODetailsVO.RateContractCondition) && objPODetailsVO.RateContractCondition.Trim().Equals("No Limit"))
                        {
                            objPODetailsVO.ConditionFound = true;
                        }
                        if (!objPODetailsVO.ConditionFound && !String.IsNullOrEmpty(objPODetailsVO.RateContractCondition) && !objPODetailsVO.RateContractCondition.Equals("No Limit") && iRateContractCount != -1)
                            if (!objPODetailsVO.ConditionFound && !objPODetailsVO.RateContractCondition.Equals("No Limit") && iRateContractCount != -1)
                            {
                                objPODetailsVO.DiscountPercent = 0;
                            }
                    }
                    //  else { objPODetailsVO.DiscountPercent = 0; }
                }
                if (e.Column.DisplayIndex == 8 || e.Column.Header.ToString().Equals("Discount %"))
                {
                    if (iRateContractCount > 0)
                    {
                        clsRateContractItemDetailsVO objRateContractDetails = lstRateContractItemDetails.Where(z => z.ItemID == objPODetailsVO.ItemID && z.RateContractID == objPODetailsVO.RateContractID && z.RateContractUnitID == objPODetailsVO.RateContractUnitID && z.Condition == objPODetailsVO.RateContractCondition).First();
                        if (objRateContractDetails != null && objRateContractDetails.DiscountPercent == Convert.ToDouble(objPODetailsVO.DiscountPercent))
                        { }
                        else if (objPODetailsVO.RateContractCondition.Equals("No Limit"))
                        {
                            objPODetailsVO.RateContractID = 0;
                            objPODetailsVO.RateContractUnitID = 0;
                        }
                        else
                        {
                            objPODetailsVO.RateContractID = 0;
                            objPODetailsVO.RateContractUnitID = 0;
                            objPODetailsVO.RateContractCondition = string.Empty;
                        }
                    }
                    else if (iRateContractCount != -1)
                    {
                        objPODetailsVO.RateContractID = 0;
                        objPODetailsVO.RateContractUnitID = 0;
                        objPODetailsVO.RateContractCondition = string.Empty;
                    }
                    if (objPODetailsVO.DiscountPercent < 0)
                    {
                        ShowMessageBox("Discount Percentage can not be Negative");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent = 0;
                        return;
                    }
                    if (objPODetailsVO.DiscountPercent > 100)
                    {
                        ShowMessageBox("Discount Percentage Greater Than 100%");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent = 0;
                        return;
                    }
                    if (objPODetailsVO.DiscountPercent == 100)
                    {
                        if (objPODetailsVO.VATPercent != 0)
                        {
                            orginalVatPer = objPODetailsVO.VATPercent;
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                        }
                    }
                    if (objPODetailsVO.VATPercent == 0)
                    {
                        if (objPODetailsVO.DiscountPercent < 100)
                        {
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = orginalVatPer;
                        }
                    }
                }

                if (e.Column.DisplayIndex == 10 || e.Column.Header.ToString().Equals("Discount Amount"))
                {
                    objPODetailsVO.RateContractID = 0;
                    objPODetailsVO.RateContractUnitID = 0;
                    objPODetailsVO.RateContractCondition = string.Empty;
                    if (objPODetailsVO.VATPercent < 0)
                    {
                        ShowMessageBox("VAT Percentage can not be Negative");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                        return;
                    }

                    if (objPODetailsVO.VATPercent > 100)
                    {
                        ShowMessageBox("VAT Percentage Greater Than 100%");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                        return;
                    }
                    if (objPODetailsVO.DiscountPercent == 100)
                    {
                        if (objPODetailsVO.VATPercent > 0)
                        {
                            ShowMessageBox("Discount percentage 100%. Can't Add Vat Percent");
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                            return;
                        }
                    }
                }

                if (e.Column.DisplayIndex == 7 || e.Column.Header.ToString().Equals("Pack M.R.P"))
                {
                    if (objPODetailsVO.MRP < 0)
                    {
                        ShowMessageBox("MRP can not be Negative");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MRP = 0;
                        return;
                    }

                    if (objPODetailsVO.ItemGroup != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
                    {
                        if (objPODetailsVO.MRP < objPODetailsVO.Rate)
                        {
                            ShowMessageBox("MRP must be greater than the cost price.");
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MRP = objPODetailsVO.Rate + 1;
                            return;
                        }
                    }
                }

                if (e.Column.DisplayIndex == 5 || e.Column.Header.ToString().Equals("Pack Cost Price"))
                {
                    if (objPODetailsVO.Rate < 0)
                    {
                        ShowMessageBox("Cost Price can not be Negative");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Rate = 0;
                        return;
                    }
                }
                // Commented ON 16-Octomber2013 asp per the discussion with Priyanka. 
                //int iCount = PurchaseOrderItems.Where(poitems => poitems.ItemID == objPODetailsVO.ItemID).Count();
                //for (int iIterate = 0; iIterate < iCount; iIterate++)
                //{
                //    clsPurchaseOrderDetailVO objDetail = new clsPurchaseOrderDetailVO();
                //    objDetail = PurchaseOrderItems.Where(poitems => poitems.ItemID == objPODetailsVO.ItemID).ToList()[iIterate];
                //    UpdateAllItems(objDetail, objPODetailsVO);
                //}
                //throw new NotImplementedException();

                if (grdpodetails.ItemsSource != null)
                {
                    CalculateOpeningBalanceSummary();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }


        private void ShowMessageBox(string strMessage)
        {
            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            msgW3.Show();
        }

        #endregion

        #region calculate summary
        private void CalculateOpeningBalanceSummary()
        {
            decimal VATAmount, Amount, NetAmount, DiscountAmount;
            VATAmount = Amount = NetAmount = DiscountAmount = 0;
            ObservableCollection<clsPurchaseOrderDetailVO> ocPODetails = ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource));
            if (ocPODetails != null)
            {
                foreach (var item in ocPODetails.ToList())
                {
                    Amount += item.Amount;
                    DiscountAmount += item.DiscountAmount;
                    VATAmount += item.VATAmount;
                    NetAmount += item.NetAmount;
                }
            }
            ((clsPurchaseOrderVO)this.DataContext).TotalAmount = Amount;
            ((clsPurchaseOrderVO)this.DataContext).TotalDiscount = DiscountAmount;
            ((clsPurchaseOrderVO)this.DataContext).TotalVAT = VATAmount;
            ((clsPurchaseOrderVO)this.DataContext).TotalNet = NetAmount;
        }
        #endregion

        private void cmdQuotation_Click(object sender, RoutedEventArgs e)
        {
            enmPurchaseOorder = PurchaseOrderSource.Quotation;
            QuotaionSearch OBJWindow = new QuotaionSearch();
            OBJWindow.onOKButton_Click += new RoutedEventHandler(OBJWindow_onOKButton_Click);
            OBJWindow.Show();
        }

        void OBJWindow_onOKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                QuotaionSearch objItemSearch = (QuotaionSearch)sender;
                if (objItemSearch.SelectedItems != null)
                {
                    List<clsQuotationDetailsVO> objList = objItemSearch.SelectedItems.ToList<clsQuotationDetailsVO>();
                    if (objList != null)
                    {
                        foreach (var item in objList)
                        {
                            PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO
                            {
                                ItemID = item.ItemID,
                                ItemName = item.ItemName,
                                ItemCode = item.ItemCode,
                                Quantity = (decimal)item.Quantity,
                                Rate = (decimal)item.Rate,
                                Amount = (decimal)item.Amount,
                                DiscountPercent = (decimal)item.ConcessionPercent,
                                DiscountAmount = (decimal)item.ConcessionAmount,
                                VATPercent = (decimal)item.TAXPercent,
                                VATAmount = (decimal)item.TAXAmount,
                                NetAmount = (decimal)item.NetAmount,
                                Specification = item.Specification,
                                PUM = item.PUM

                            });
                        }
                        grdpodetails.Focus();
                        grdpodetails.UpdateLayout();
                        CalculateOpeningBalanceSummary();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void dgPOList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grdpo.ItemsSource = null;
            FillDetails();
            if (dgPOList.SelectedItem != null)
            {
                if (chkCancelPO.IsChecked == false)
                {
                    if (((clsPurchaseOrderVO)dgPOList.SelectedItem).IsApproveded == true)
                    {
                        cmdApprove.IsEnabled = false;
                        CmdChangeApprove.IsEnabled = false;
                    }
                    else
                    {
                        cmdApprove.IsEnabled = true;
                        CmdChangeApprove.IsEnabled = true;
                    }
                }
            }

        }

        void FillDetails()
        {
            if (dgPOList.SelectedIndex != -1)
            {
                try
                {
                    clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();
                    clsPurchaseOrderVO objList = (clsPurchaseOrderVO)dgPOList.SelectedItem;

                    objBizActionVO.SearchID = objList.ID;
                    objBizActionVO.UnitID = objList.UnitId;//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    objBizActionVO.IsPagingEnabled = true;
                    objBizActionVO.StartIndex = 0;
                    objBizActionVO.MinRows = 20;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                clsGetPurchaseOrderDetailsBizActionVO obj = ((clsGetPurchaseOrderDetailsBizActionVO)arg.Result);
                                grdpo.ItemsSource = null;
                                PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
                                if (obj.PurchaseOrderList != null && obj.PurchaseOrderList.Count > 0)
                                {
                                    grdpo.ItemsSource = obj.PurchaseOrderList;
                                }
                                foreach (var item in obj.PurchaseOrderList)
                                {
                                    PurchaseOrderItems.Add(item);
                                }
                                foreach (var item2 in obj.PoIndentList)
                                {
                                    POIndentdetails.Add(item2);
                                }

                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Opening Balance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                    };

                    Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }
        private void Freez_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkCancelPO.IsChecked == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "P.O. is already cancel So you Can not Freez PO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD1.Show();

                    chkFreez = (CheckBox)sender;
                    chkFreez.IsChecked = false;
                }
                else
                {
                    chkFreez = (CheckBox)sender;
                    if (chkFreez != null)
                    {
                        if (chkFreez.IsChecked == true)
                        {
                            msgText = "Are you sure you want to Freeze the  Details";

                            MessageBoxControl.MessageBoxChildWindow msgW =
                                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                            msgW.Show();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsPurchaseOrderVO objSelectedPO = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                    clsFreezPurchaseOrderBizActionVO objFreezPurchaseOrder = new clsFreezPurchaseOrderBizActionVO();
                    objFreezPurchaseOrder.PurchaseOrder = new clsPurchaseOrderVO();
                    objFreezPurchaseOrder.PurchaseOrder.ID = objSelectedPO.ID;
                    objFreezPurchaseOrder.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "Purchase Order Freezed Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while Freezing details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };

                    Client.ProcessAsync(objFreezPurchaseOrder, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                else
                {
                    chkFreez.IsChecked = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtIndentNo.Text = "";

                clsPurchaseOrderVO obj = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                if (obj != null)
                {
                    this.DataContext = obj;
                    ((clsPurchaseOrderVO)this.DataContext).StoreID = obj.StoreID;
                    ((clsPurchaseOrderVO)this.DataContext).ID = obj.ID;

                    if (obj.Direct == true)
                    {
                        rdbDirect.IsChecked = true;
                        AttachItemSearchControl(obj.StoreID, obj.SupplierID);
                        BdrItemSearch.Visibility = Visibility.Visible;


                    }
                    else
                    {
                        BdrItemSearch.Visibility = Visibility.Collapsed;
                        rdbIndent.IsChecked = true;
                    }

                    var results = from r in ((List<clsStoreVO>)cboStore.ItemsSource)
                                  where r.StoreId == obj.StoreID
                                  select r;

                    foreach (clsStoreVO item in results)
                    {
                        cboStore.SelectedItem = item;
                    }

                    var results1 = from r in ((List<MasterListItem>)cboSupplier.ItemsSource)
                                   where r.ID == obj.SupplierID
                                   select r;

                    foreach (MasterListItem item in results1)
                    {
                        cboSupplier.SelectedItem = item;
                        lngSupplierID = item.ID;
                    }

                    var results3 = from r in ((List<MasterListItem>)cboDelivery.ItemsSource)
                                   where r.ID == obj.DeliveryDuration
                                   select r;

                    foreach (MasterListItem item in results3)
                    {
                        cboDelivery.SelectedItem = item;
                    }
                    //cboTOP.SelectedValue = obj.PaymentTerms;
                    var results4 = from r in ((List<MasterListItem>)cboTOP.ItemsSource)
                                   where r.ID == obj.PaymentTerms
                                   select r;

                    foreach (MasterListItem item in results4)
                    {
                        cboTOP.SelectedItem = item;
                    }

                    var results5 = from r in ((List<MasterListItem>)cmbSchedule.ItemsSource)
                                   where r.ID == obj.Schedule
                                   select r;

                    foreach (MasterListItem item in results5)
                    {
                        cmbSchedule.SelectedItem = item;
                    }


                    var results6 = from r in ((List<MasterListItem>)cmbDeliveryDays.ItemsSource)
                                   where r.ID == obj.DeliveryDays
                                   select r;

                    foreach (MasterListItem item in results6)
                    {
                        cmbDeliveryDays.SelectedItem = item;
                    }


                    cmbModeofPayment.SelectedValue = obj.PaymentMode;

                    if (chkCancelPO.IsChecked == false)
                    {
                        if (obj.IsApproveded == true)
                        {
                            SetCommandButtonState("Approve");
                        }
                        else
                        {
                            SetCommandButtonState("Modify");
                        }
                    }
                    else
                    {
                        SetCommandButtonState("Approve");
                    }

                    FillPODetails(obj.ID, obj.UnitId);

                    ISEditMode = true;
                    dgPOList.UpdateLayout();
                    dgPOList.Focus();
                    cmdPrint.IsEnabled = false;
                    txtIndentNo.Text = ((clsPurchaseOrderVO)dgPOList.SelectedItem).IndentNumber.Trim();
                    objAnimation.Invoke(RotationType.Forward);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Item Search Control
        //ItemSearch _ItemSearchRowControl = null;
        private void AttachItemSearchControl(long StoreID, long SupplierID)
        {
            BdrItemSearch.Visibility = Visibility.Visible;
            ItemSearchStackPanel.Children.Clear();
            //_ItemSearchRowControl = new ItemSearch(StoreID, SupplierID);
            //_ItemSearchRowControl.OnQuantityEnter_Click += new RoutedEventHandler(_ItemSearchRowControl_OnQuantityEnter_Click);
            //ItemSearchStackPanel.Children.Add(_ItemSearchRowControl);
        }

        void _ItemSearchRowControl_OnQuantityEnter_Click(object sender, RoutedEventArgs e)
        {
            String strItemIDs = string.Empty;//Only For PO
            ItemSearch _ItemSearchRowControl = (ItemSearch)sender;
            if (PurchaseOrderItems != null)
            {
                if (PurchaseOrderItems.Count.Equals(0))
                {
                    PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                }
            }
            else
            {
                PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
            }
            _ItemSearchRowControl.SelectedItems[0].RowID = PurchaseOrderItems.Count + 1;
            foreach (var item in _ItemSearchRowControl.SelectedItems)
            {
                if (PurchaseOrderItems.Where(poItems => poItems.ItemID == item.ID).Any() == false)
                {
                    PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO { ItemID = item.ItemID, ItemName = item.ItemName, ItemCode = item.ItemCode, PUM = item.PUOM, VATPercent = item.VatPer, Rate = item.PurchaseRate, Quantity = Convert.ToDecimal(item.RequiredQuantity), MRP = item.MRP, CurrencyList = CurrencyList, RateContractID = -1, RateContractUnitID = -1, RateContractCondition = null, SelectedCurrency = CurrencyList.Where(z => z.ID == 1).FirstOrDefault() }); //, MRP = item.MRP                
                    strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemID) + ",");
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please add another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            }

            grdpodetails.ItemsSource = null;
            grdpodetails.ItemsSource = PurchaseOrderItems;
            dgPOList.UpdateLayout();

            CalculateOpeningBalanceSummary();
            CheckRateContractForItems(strItemIDs.Trim(',')); //Only For PO
            _ItemSearchRowControl.SetFocus();
            _ItemSearchRowControl = null;
            _ItemSearchRowControl = new ItemSearch();

        }

        #endregion

        //OLD Commented By CDS
        //private void FillPODetails(long POID, long UnitID)
        //{
        //    try
        //    {
        //        clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();
        //        clsPurchaseOrderVO objList = (clsPurchaseOrderVO)dgPOList.SelectedItem;
        //        string strItemIDs = String.Empty;
        //        objBizActionVO.SearchID = POID;
        //        objBizActionVO.UnitID = UnitID; //((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //        objBizActionVO.IsPagingEnabled = true;
        //        objBizActionVO.StartIndex = 0;
        //        objBizActionVO.MinRows = 20;
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null)
        //            {
        //                if (arg.Result != null)
        //                {
        //                    clsGetPurchaseOrderDetailsBizActionVO obj = ((clsGetPurchaseOrderDetailsBizActionVO)arg.Result);
        //                    POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
        //                    List<clsPurchaseOrderDetailVO> objListOfItems = (List<clsPurchaseOrderDetailVO>)obj.PurchaseOrderList;
        //                    List<clsPurchaseOrderDetailVO> PoIndent = (List<clsPurchaseOrderDetailVO>)obj.PoIndentList;
        //                    PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
        //                    List<clsPurchaseOrderTerms> lstTerms = (List<clsPurchaseOrderTerms>)obj.POTerms;
        //                    if (objListOfItems != null)
        //                    {
        //                        foreach (var item in objListOfItems)
        //                        {
        //                            strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemID) + ",");
        //                            PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO
        //                            {
        //                                ItemID = item.ItemID,
        //                                ItemName = item.ItemName,
        //                                ItemCode = item.ItemCode,
        //                                Quantity = (decimal)item.Quantity,
        //                                Rate = (decimal)item.Rate,
        //                                Amount = (decimal)item.Amount,
        //                                MRP = (decimal)item.MRP,
        //                                DiscountPercent = (decimal)item.DiscountPercent,
        //                                DiscountAmount = (decimal)item.DiscountAmount,
        //                                VATPercent = (decimal)item.VATPercent,
        //                                VATAmount = (decimal)item.VATAmount,
        //                                NetAmount = (decimal)item.NetAmount,
        //                                Specification = item.Specification,
        //                                PUM = item.PUM,
        //                                RateContractID = item.RateContractID == 0 ? -1 : item.RateContractID,
        //                                RateContractUnitID = item.RateContractUnitID == 0 ? -1 : item.RateContractUnitID,
        //                                RateContractCondition = item.RateContractCondition,
        //                                IndentNumber = item.IndentNumber,
        //                                ItemGroup = item.ItemGroup,
        //                                ItemCategory = item.ItemCategory,
        //                                CurrencyList = CurrencyList,
        //                                SelectedCurrency = CurrencyList.Where(z => z.ID == item.SelectedCurrency.ID).FirstOrDefault()
        //                            });
        //                        };


        //                        foreach (var item in PoIndent)
        //                        {
        //                            POIndentdetails.Add(item);
        //                        }
        //                        foreach (var item in PurchaseOrderTerms)
        //                        {
        //                            foreach (var item2 in lstTerms)
        //                            {
        //                                if (item.ID == item2.TermsAndConditionID)
        //                                {
        //                                    item.Status = item2.Status;
        //                                }
        //                            }
        //                            cmbTermsNCondition.SelectedItem = PurchaseOrderTerms[0];
        //                        }
        //                        grdpodetails.ItemsSource = PurchaseOrderItems;
        //                        grdpodetails.Focus();
        //                        grdpodetails.UpdateLayout();
        //                        if (((clsPurchaseOrderVO)dgPOList.SelectedItem).Freezed == false)
        //                            CheckRateContractForItems(strItemIDs.Trim(','));
        //                        CalculateOpeningBalanceSummary();
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding P O details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW1.Show();
        //            }

        //        };

        //        Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        Client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //END

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long POId = 0;
            long PUnitID = 0;
            clsPurchaseOrderVO obj = (clsPurchaseOrderVO)dgPOList.SelectedItem;
            if (obj != null)
            {
                if (obj.POApproveLvlID == 2)  //Added by Ajit date 14/9/2016 check 2nd level Approval //***//
                {
                    this.DataContext = obj;
                    //((clsPurchaseOrderVO)this.DataContext).ID = obj.ID;
                    POId = obj.ID;
                    PUnitID = obj.UnitId;
                    //string URL = "../Reports/InventoryPharmacy/PurchaseOrderPrint.aspx?POId=" + POId + "&PUnitID =" + PUnitID;
                    string URL = "../Reports/InventoryPharmacy/PurchaseOrderPrint.aspx?POId=" + POId + "&PUnitID=" + PUnitID;

                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Higher Level Approval Required!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD1.Show();
                }
            }
            else
            {
                msgText = "Please Select a PO to Print";
                MessageBoxControl.MessageBoxChildWindow msgW =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }

        }

        private void cmdDeleteFreeService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((clsPurchaseOrderVO)dgPOList.SelectedItem) != null)
                {
                    if (((clsPurchaseOrderVO)dgPOList.SelectedItem).IsApproveded == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to cancel PO, PO is already Approved", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD1.Show();
                        return;
                    }
                    else
                    {


                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to cancel the P.O. ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                POCancellationWindow Win = new POCancellationWindow();
                                Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                                Win.Show();
                            }
                        };
                        msgWD.Show();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        bool CancelPO = false;
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateRemarkForCancellationPO bizactionVO = new clsUpdateRemarkForCancellationPO();
            bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();
            try
            {
                bizactionVO.PurchaseOrder.ID = ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID;
                bizactionVO.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizactionVO.PurchaseOrder.CancellationRemark = ((POCancellationWindow)sender).txtAppReason.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        clsCancelPurchaseOrderBizActionVO bizActionObj = new clsCancelPurchaseOrderBizActionVO();
                        bizActionObj.PurchaseOrder = new clsPurchaseOrderVO();
                        bizActionObj.PurchaseOrder.ID = ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID;

                        bizActionObj.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        Client.ProcessCompleted += (s1, arg1) =>
                        {
                            if (arg1.Error == null)
                            {
                                if (arg1.Result != null)
                                {
                                    CancelPO = true;
                                    FillPurchaseOrderDataGrid();
                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding P O details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        };

                        Client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                };
                client.ProcessAsync(bizactionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<clsPurchaseOrderDetailVO> lstDeletedPODetails = new List<clsPurchaseOrderDetailVO>();
        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (grdpodetails.SelectedItem != null)
            {

                string msgText = "Are you sure you want to delete the item ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {

                        lstDeletedPODetails.Add(PurchaseOrderItems[grdpodetails.SelectedIndex]);

                        DeleteItemRateContract();
                        String strIndentNo = ((clsPurchaseOrderDetailVO)(grdpodetails.SelectedItem)).IndentNumber;

                        ObservableCollection<clsPurchaseOrderDetailVO> obj = POIndentdetails.DeepCopy();
                        foreach (var item in obj)
                        {
                            if (item.ItemID == ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID)
                            {
                                POIndentdetails.Remove(item);
                            }
                        }

                        PurchaseOrderItems.RemoveAt(grdpodetails.SelectedIndex);

                        if (strIndentNo != "" && strIndentNo != null)
                        {
                            Boolean blnIsIndentExist = PurchaseOrderItems.Where(z => z.IndentNumber.Trim().Equals(strIndentNo.Trim())).Any();
                            if (txtIndentNo.Text != null && !blnIsIndentExist && txtIndentNo.Text.Trim().Contains(strIndentNo.Trim()))
                                txtIndentNo.Text = txtIndentNo.Text.Replace(strIndentNo, String.Empty).Trim(',');
                        }
                        grdpodetails.Focus();
                        grdpodetails.UpdateLayout();
                        grdpodetails.SelectedIndex = PurchaseOrderItems.Count - 1;
                        CalculateOpeningBalanceSummary();
                    }
                };
                msgWD.Show();
            }
        }
        /// <summary>
        /// On Item delete the Rate contract for the item deleted.
        /// </summary>
        private void DeleteItemRateContract()
        {
            if (lstRateContractItemDetails != null && lstRateContractItemDetails.Count > 0)
            {
                clsPurchaseOrderDetailVO objPODetails = ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem);
                List<clsRateContractItemDetailsVO> lstRateContractDetails = lstRateContractItemDetails.Where(RateContract => RateContract.ItemID == objPODetails.ItemID).ToList();
                if (lstRateContractDetails != null && lstRateContractDetails.Count > 0)
                {
                    foreach (clsRateContractItemDetailsVO objRateContractItem in lstRateContractDetails)
                    {
                        lstRateContractItemDetails.Remove(objRateContractItem);
                    }
                }
            }
        }

        private void rdbIndent_Checked(object sender, RoutedEventArgs e)
        {
            if (PurchaseOrderItems != null)
            {
                PurchaseOrderItems.Clear();
                POIndentdetails.Clear();

                txtIndentNo.Text = "";
                IndentNo = "";
                if (grdpodetails.ItemsSource != null)
                    CalculateOpeningBalanceSummary();
            }
        }
        private long StoreID { get; set; }
        private long SupplierID { get; set; }
        private void cboStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboStore.SelectedItem != null)
            {

                if (PurchaseOrderItems != null)
                {
                    txtIndentNo.Text = String.Empty;
                    PurchaseOrderItems.Clear();
                    POIndentdetails.Clear();
                    CalculateOpeningBalanceSummary();
                }
                StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                if (StoreID > 0 && SupplierID > 0 && rdbDirect.IsChecked == true)
                {
                    BdrItemSearch.Visibility = System.Windows.Visibility.Visible;
                    AttachItemSearchControl(StoreID, SupplierID);
                }

            }
        }

        private void rdbDirect_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsPageLoded)
                {
                    if (PurchaseOrderItems != null)
                    {
                        PurchaseOrderItems.Clear();
                        POIndentdetails.Clear();
                        txtIndentNo.Text = "";

                        //((clsPurchaseOrderVO)this.DataContext).IndentID = 0;
                        //((clsPurchaseOrderVO)this.DataContext).IndentUnitID = 0;
                        if (grdpodetails.ItemsSource != null)
                            CalculateOpeningBalanceSummary();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        private void cboSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClsGetSupplierDetailsBizActionVO objBizActionVO = new ClsGetSupplierDetailsBizActionVO();

            if (cboSupplier.SelectedItem != null)
            {
                objBizActionVO.SupplierId = ((MasterListItem)cboSupplier.SelectedItem).ID;
                SupplierID = ((MasterListItem)cboSupplier.SelectedItem).ID;
            }
            if (StoreID > 0 && SupplierID > 0 && rdbDirect.IsChecked == true)
            {
                BdrItemSearch.Visibility = System.Windows.Visibility.Visible;
                AttachItemSearchControl(StoreID, SupplierID);
            }

            if (objBizActionVO.SupplierId != lngSupplierID)
            {
                PurchaseOrderItems.Clear();
                POIndentdetails.Clear();
                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
                txtIndentNo.Text = String.Empty;
                CalculateOpeningBalanceSummary();
            }
            objBizActionVO.SupplierPaymentMode = true;
            objBizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            //objBizActionVO.PagingEnabled = true;
            //objBizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            //objBizActionVO.MaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        ClsGetSupplierDetailsBizActionVO objPurchaseOrderList = ((ClsGetSupplierDetailsBizActionVO)arg.Result);
                        if (objPurchaseOrderList.ModeOfPayment > 0)
                            cmbModeofPayment.SelectedValue = objPurchaseOrderList.ModeOfPayment;
                    }
                }
                else
                {
                    System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Opening Balance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }



        StringBuilder strError = new StringBuilder();
        List<long> IDS = new System.Collections.Generic.List<long>();
        List<long> PreviousIDS = new System.Collections.Generic.List<long>();
        bool Approved = false;
        private void Approve_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {

                IDS.Add(((clsPurchaseOrderVO)dgPOList.SelectedItem).ID);

                if (((clsPurchaseOrderVO)dgPOList.SelectedItem).Freezed != true)
                {
                    msgText = "Are you sure you want to Freeze the  Details";
                    if (dgPOList.Columns[8].IsReadOnly)
                    {
                        dgPOList.Columns[8].IsReadOnly = true;
                    }
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
                else
                {

                    foreach (var item in DataList)
                    {
                        if (item.ID == ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID)
                        {
                            if (item.Freezed == true)
                            {
                                item.Freezed = true;
                                //item.IsApprovededEnable = true;
                                item.IsApproveded = true;
                                item.IsEnabledFreezed = false;
                            }
                            else
                            {
                                item.Freezed = false;
                                item.IsApprovededEnable = true;
                                item.IsApproveded = false;
                                item.IsEnabledFreezed = true;
                            }


                        }
                        else
                        {

                        }

                    }
                    dgPOList.ItemsSource = null;
                    dgPOList.ItemsSource = DataList;
                }


            }
            else
            {
                PreviousIDS = IDS;
                IDS = new System.Collections.Generic.List<long>();
                foreach (var item in PreviousIDS)
                {
                    if (item != ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID)
                    {
                        IDS.Add(item);
                    }
                }

            }
        }

        private void FillPODetails(long POID, long UnitID)
        {
            try
            {
                clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();
                clsPurchaseOrderVO objList = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                string strItemIDs = String.Empty;
                objBizActionVO.SearchID = POID;
                objBizActionVO.UnitID = UnitID;    //((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.IsPagingEnabled = true;
                objBizActionVO.StartIndex = 0;
                objBizActionVO.MinRows = 20;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetPurchaseOrderDetailsBizActionVO obj = ((clsGetPurchaseOrderDetailsBizActionVO)arg.Result);
                            POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
                            List<clsPurchaseOrderDetailVO> objListOfItems = (List<clsPurchaseOrderDetailVO>)obj.PurchaseOrderList;
                            List<clsPurchaseOrderDetailVO> PoIndent = (List<clsPurchaseOrderDetailVO>)obj.PoIndentList;
                            PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                            List<clsPurchaseOrderTerms> lstTerms = (List<clsPurchaseOrderTerms>)obj.POTerms;
                            if (objListOfItems != null)
                            {
                                foreach (var item in objListOfItems)
                                {
                                    strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemID) + ",");
                                    PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO
                                    {
                                        IndentID = Convert.ToInt64(item.IndentID),
                                        IndentUnitID = Convert.ToInt64(item.IndentUnitID),
                                        IndentDetailID = item.IndentDetailID,
                                        IndentDetailUnitID = item.IndentUnitID,
                                        PoDetailsID = item.PoDetailsID,
                                        PoDetailsUnitID = item.PoDetailsUnitID,
                                        ItemID = item.ItemID,
                                        ItemName = item.ItemName,
                                        ItemCode = item.ItemCode,
                                        Quantity = (decimal)item.Quantity,
                                        Rate = (decimal)item.Rate,
                                        PRQuantity = Convert.ToDecimal(item.PRQTY),
                                        PendingQuantity = Convert.ToDecimal(item.PRPendingQty),
                                        PRPendingQuantity = Convert.ToDouble(item.PRPendingQty),
                                        TransUOM = item.PRUOM,
                                        //Rate = Convert.ToDecimal(Convert.ToDecimal(item.Rate) * Convert.ToDecimal(item.BaseConversionFactor)),
                                        Amount = (decimal)item.Amount,
                                        MRP = (decimal)item.MRP,
                                        //MRP = Convert.ToDecimal(Convert.ToDecimal(item.MRP) * Convert.ToDecimal(item.BaseConversionFactor)),
                                        DiscountPercent = (decimal)item.DiscountPercent,
                                        DiscountAmount = (decimal)item.DiscountAmount,
                                        // Added By CDS 
                                        ItemVATPercent = item.ItemVATPercent,
                                        ItemVATAmount = item.ItemVATAmount,
                                        POItemVatType = item.POItemVatType,
                                        POItemVatApplicationOn = item.POItemVatApplicationOn,
                                        POItemOtherTaxType = item.POItemOtherTaxType,
                                        POItemOtherTaxApplicationOn = item.POItemOtherTaxApplicationOn,
                                        CostRate = (decimal)item.Quantity * (decimal)item.Rate,
                                        VATPercent = (decimal)item.VATPercent,
                                        VATAmount = (decimal)item.VATAmount,
                                        OtherCharges = (decimal)item.OtherCharges,
                                        PODiscount = (decimal)item.PODiscount,
                                        ConversionFactor = item.ConversionFactor,
                                        BaseConversionFactor = Convert.ToSingle(item.BaseConversionFactor),
                                        BaseRate = item.BaseRate,
                                        BaseMRP = item.BaseMRP,
                                        BaseQuantity = item.BaseQuantity,
                                        BaseUOM = item.BaseUOM,
                                        SUOM = item.SUOM,
                                        MainRate = item.BaseRate,
                                        MainMRP = item.BaseMRP,
                                        SelectedUOM = new MasterListItem { ID = item.SelectedUOM.ID, Description = item.TransUOM },
                                        SUOMID = item.SUOMID,
                                        BaseUOMID = item.BaseUOMID,
                                        // End 
                                        //By Default Purchase UOMID
                                        POPUOMID = item.SelectedUOM.ID,
                                        Specification = item.Specification,
                                        PUM = item.PUM,
                                        RateContractID = item.RateContractID,
                                        RateContractUnitID = item.RateContractUnitID,
                                        RateContractCondition = item.RateContractCondition,
                                        IndentNumber = item.IndentNumber,
                                        ItemGroup = item.ItemGroup,
                                        ItemCategory = item.ItemCategory,
                                        CurrencyList = CurrencyList,
                                        PurchaseToBaseCF = item.PurchaseToBaseCF,
                                        StockingToBaseCF = item.StockingToBaseCF,
                                        SelectedCurrency = CurrencyList.Where(z => z.ID == item.SelectedCurrency.ID).FirstOrDefault(),
                                        NetAmount = (decimal)item.NetAmount
                                    });
                                };

                                //PagedCollectionView pcvPOItems = new PagedCollectionView(PurchaseOrderItems);
                                //pcvPOItems.GroupDescriptions.Add(new PropertyGroupDescription("IndentNumber"));
                                //grdpodetails.ItemsSource = pcvPOItems;

                                foreach (var item in PoIndent)
                                {
                                    POIndentdetails.Add(item);
                                }
                                foreach (var item in PurchaseOrderTerms)
                                {
                                    foreach (var item2 in lstTerms)
                                    {
                                        if (item.ID == item2.TermsAndConditionID)
                                        {
                                            item.Status = item2.Status;
                                        }
                                    }
                                    cmbTermsNCondition.SelectedItem = PurchaseOrderTerms[0];
                                }


                                grdpodetails.ItemsSource = PurchaseOrderItems;
                                grdpodetails.Focus();
                                grdpodetails.UpdateLayout();

                                //if (ISFromAprovePO == false)
                                //{
                                //    if (((clsPurchaseOrderVO)dgPOList.SelectedItem).Freezed == false)
                                //        CheckRateContractForItems(strItemIDs.Trim(','));
                                //}
                                //else
                                //{
                                //    CheckRateContractForItems(strItemIDs.Trim(','));
                                //}
                                //CalculateOpeningBalanceSummary();

                                bool isValid = true;
                                // Validation For Pending Quantiy By CDS On Save/Modify/ChangeAndApprovePO
                                if (PurchaseOrderItems != null && PurchaseOrderItems.Count > 0)
                                {
                                    List<clsPurchaseOrderDetailVO> objListPODetails = PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>();
                                    if (objList != null && objListPODetails.Count > 0)
                                    {
                                        foreach (var item in objListPODetails)
                                        {
                                            if ((item.PendingQuantity < item.Quantity * Convert.ToDecimal(item.BaseConversionFactor)) && rdbDirect.IsChecked == false)
                                            {
                                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity So PO Cannot Approve", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                msgW3.Show();

                                                item.Quantity = 0;
                                                item.PRPendingQuantity = Convert.ToDouble(item.PendingQuantity - item.Quantity * Convert.ToDecimal(item.BaseConversionFactor));
                                                CalculateOpeningBalanceSummary();

                                                isValid = false;
                                                ClickedFlag1 = 0;
                                                return;
                                            }
                                            //else
                                            //{
                                            //    if (rdbDirect.IsChecked != true)
                                            //    {
                                            //        item.PRPendingQuantity = Convert.ToDouble(item.PendingQuantity - item.Quantity * Convert.ToDecimal(item.BaseConversionFactor));
                                            //    }
                                            //}
                                        }
                                    }
                                }

                                if (IsFromDirectApprove == true && isValid == true)
                                {
                                    IDS = new System.Collections.Generic.List<long>();
                                    IDS.Add(((clsPurchaseOrderVO)dgPOList.SelectedItem).ID);
                                    if (IDS.Count > 0)
                                    {
                                        clsUpdatePurchaseOrderForApproval bizactionVO = new clsUpdatePurchaseOrderForApproval();
                                        bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();
                                        Approved = true;
                                        bizactionVO.PurchaseOrder.Freezed = (((clsPurchaseOrderVO)dgPOList.SelectedItem)).IsApproveded == true ? true : false;
                                        try
                                        {
                                            bizactionVO.PurchaseOrder.ids = IDS;
                                            bizactionVO.PurchaseOrder.UnitId = ((clsPurchaseOrderVO)dgPOList.SelectedItem).UnitId;  //((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                            bizactionVO.PurchaseOrder.ApprovedBy = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
                                            bizactionVO.PurchaseOrder.Items = PurchaseOrderItems.ToList();
                                            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                            PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                            client1.ProcessCompleted += (s1, args) =>
                                            {
                                                if (args.Error == null && args.Result != null)
                                                {
                                                    msgText = "Purchase Order Approved successfully .";
                                                    if (dgPOList.Columns[8].IsReadOnly)
                                                    {
                                                        dgPOList.Columns[8].IsReadOnly = true;
                                                    }
                                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                    msgWindow.Show();

                                                    if (cmbStore.ItemsSource != null && cmbSupplier.ItemsSource != null)
                                                    {
                                                        cmbStore.SelectedItem = ((List<clsStoreVO>)cmbStore.ItemsSource)[0];
                                                        cmbSupplier.SelectedItem = ((List<MasterListItem>)cmbSupplier.ItemsSource)[0];
                                                    }
                                                    FillPurchaseOrderDataGrid();
                                                }
                                            };
                                            client1.ProcessAsync(bizactionVO, new clsUserVO());
                                            client1.CloseAsync();
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    else
                                    {
                                        Approved = false;
                                        msgText = "Please select Approve PO checkbox to approve.";
                                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWindow.Show();
                                        foreach (var item in DataList)
                                        {
                                            item.IsApprovededEnable = true;
                                        }
                                        dgPOList.ItemsSource = null;
                                        dgPOList.ItemsSource = DataList;
                                    }
                                }

                            }

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding P O details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                };

                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdPOApprove_Click(object sender, RoutedEventArgs e)
        {
            if (ApproveValidation())
            {
                MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve PO with " + this.objUser.POApprovalLvl + " rights?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        //// added By Ashish Z. on 05042016
                        //IDS = new System.Collections.Generic.List<long>();
                        ////IDS.Add(((clsPurchaseOrderVO)dgPOList.SelectedItem).ID);
                        //IDS.Add(((clsPurchaseOrderVO)dgPOList.SelectedItem).ID);

                        //clsUpdatePurchaseOrderForApproval bizactionVO = new clsUpdatePurchaseOrderForApproval();
                        //bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();
                        //Approved = true;
                        //bizactionVO.PurchaseOrder.Freezed = ((clsPurchaseOrderVO)dgPOList.SelectedItem).IsApproveded == true ? true : false;
                        //try
                        //{
                        //    bizactionVO.PurchaseOrder.ids = IDS;
                        //    bizactionVO.PurchaseOrder.UnitId = ((clsPurchaseOrderVO)dgPOList.SelectedItem).UnitId;
                        //    bizactionVO.PurchaseOrder.POApproveLvlID = objUser.POApprovalLvlID;
                        //    bizactionVO.PurchaseOrder.ApprovedBy = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
                        //    bizactionVO.PurchaseOrder.ApprovedByID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

                        //    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        //    client.ProcessCompleted += (s1, args1) =>
                        //    {
                        //        if (args1.Error == null && args1.Result != null)
                        //        {
                        //            msgText = "Purchase Order Approved Successfully With";
                        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //               new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText + " " + this.objUser.POApprovalLvl, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //            msgW1.Show();
                        //            FillPurchaseOrderDataGrid();
                        //        }
                        //    };
                        //    client.ProcessAsync(bizactionVO, new clsUserVO());
                        //    client.CloseAsync();
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                        //added by Ashish Z on 280716
                        if (dgPOList.SelectedItem != null)
                        {
                            clsAddPurchaseOrderBizActionVO BizAction = new clsAddPurchaseOrderBizActionVO();
                            frmPurchaseOrder FrmPOrder = new frmPurchaseOrder();
                            FrmPOrder.IsApproveded = true;
                            FrmPOrder.ISFromAprovePO = true;
                            FrmPOrder.POApproveLvlID = objUser.POApprovalLvlID;
                            FrmPOrder.IsForDirectApprove = true;
                            FrmPOrder.SelectedPO = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                            mElement.Text = "Approve Purchase Order";
                            ((IApplicationConfiguration)App.Current).OpenMainContent(FrmPOrder);
                        }
                        //End on dated 280716
                    }
                };
                mgBox.Show();
                // End 05042016

                //FillPODetails(((clsPurchaseOrderVO)dgPOList.SelectedItem).ID, ((clsPurchaseOrderVO)dgPOList.SelectedItem).UnitId); COMMENTED BY Ashish Z. on 05042016

                //IDS = new System.Collections.Generic.List<long>();
                //IDS.Add(((clsPurchaseOrderVO)dgPOList.SelectedItem).ID);
                //if (IDS.Count > 0)
                //{
                //    clsUpdatePurchaseOrderForApproval bizactionVO = new clsUpdatePurchaseOrderForApproval();
                //    bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();
                //    Approved = true;
                //    bizactionVO.PurchaseOrder.Freezed = (((clsPurchaseOrderVO)dgPOList.SelectedItem)).IsApproveded == true ? true : false;
                //    try
                //    {
                //        bizactionVO.PurchaseOrder.ids = IDS;
                //        bizactionVO.PurchaseOrder.UnitId = ((clsPurchaseOrderVO)dgPOList.SelectedItem).UnitId;  //((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //        bizactionVO.PurchaseOrder.ApprovedBy = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
                //        bizactionVO.PurchaseOrder.Items = PurchaseOrderItems.ToList();                            
                //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //        client.ProcessCompleted += (s, args) =>
                //        {
                //            if (args.Error == null && args.Result != null)
                //            {
                //                msgText = "Purchase Order Approved successfully .";
                //                if (dgPOList.Columns[8].IsReadOnly)
                //                {
                //                    dgPOList.Columns[8].IsReadOnly = true;
                //                }
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();

                //                if (cmbStore.ItemsSource != null && cmbSupplier.ItemsSource != null)
                //                {
                //                    cmbStore.SelectedItem = ((List<clsStoreVO>)cmbStore.ItemsSource)[0];
                //                    cmbSupplier.SelectedItem = ((List<MasterListItem>)cmbSupplier.ItemsSource)[0];
                //                }
                //                FillPurchaseOrderDataGrid();
                //            }
                //        };
                //        client.ProcessAsync(bizactionVO, new clsUserVO());
                //        client.CloseAsync();
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
                //else
                //{
                //    Approved = false;
                //    msgText = "Please select Approve PO checkbox to approve.";
                //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgWindow.Show();
                //    foreach (var item in DataList)
                //    {
                //        item.IsApprovededEnable = true;
                //    }
                //    dgPOList.ItemsSource = null;
                //    dgPOList.ItemsSource = DataList;
                //}
            }
        }

        public bool ApproveValidation()
        {
            bool IsValid = true;
            string strMsg = string.Empty;
            if (dgPOList.SelectedItem == null)
            {
                strMsg = "Please Select PO";
                IsValid = false;
            }
            else if (((clsPurchaseOrderVO)dgPOList.SelectedItem).Freezed == false)
            {
                strMsg = "PO is not Freezed \n Please Freeze PO";
                IsValid = false;
            }
            else if (this.objUser.POApprovalLvlID == 1)
            {
                if (((clsPurchaseOrderVO)dgPOList.SelectedItem).POApproveLvlID == 1)
                {
                    strMsg = "PO is already Approved with Level 1";
                    IsValid = false;
                }
            }

            if (!IsValid)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
            return IsValid;

        }

        private void chkCancelPO_Click(object sender, RoutedEventArgs e)
        {
            if (chkCancelPO.IsChecked == true)
            {
                cmdApprove.IsEnabled = false;
                CmdChangeApprove.IsEnabled = false;

                dgPOList.Columns[25].Visibility = Visibility.Collapsed;
                dgPOList.Columns[10].IsReadOnly = true;
                dgPOList.Columns[9].IsReadOnly = true;

                //FrameworkElement fe = dgPOList.Columns[10].GetCellContent(e.Row);
                //FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                //var thisCell = (DataGridCell)parent;
                //CheckBox chk = thisCell.Content as CheckBox;
                //chk.IsEnabled = false;


            }
            else
            {
                cmdApprove.IsEnabled = true;
                CmdChangeApprove.IsEnabled = true;
                dgPOList.Columns[10].IsReadOnly = false;
                dgPOList.Columns[9].IsReadOnly = false;
            }

            FillPurchaseOrderDataGrid();
        }

        private void txtPO_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                FillPurchaseOrderDataGrid();
            }
            else if (e.Key == Key.Tab)
            {
                FillPurchaseOrderDataGrid();
            }
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStore.SelectedItem != null)
            {
                if (((clsStoreVO)cmbStore.SelectedItem).StoreId > 0)
                    FillPurchaseOrderDataGrid();
            }
        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSupplier.SelectedItem != null)
            {
                if ((cmbSupplier.SelectedItem as MasterListItem).ID > 0)
                    FillPurchaseOrderDataGrid();
            }
        }

        private void CmdChangeAndApprove_Click(object sender, RoutedEventArgs e)
        {
            if (ApproveValidation())
            {
                if (objUser.POApprovalLvlID > 0)
                {
                    ApproveFlag += 1;
                    if (ApproveFlag == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve PO with " + objUser.POApprovalLvl + " rights?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                        msgWindowUpdate.Show();
                    }
                }
            }


            //try
            //{
            //    if (dgPOList.SelectedItem != null)
            //    {
            //        if (((clsPurchaseOrderVO)dgPOList.SelectedItem).IsApproveded)
            //        {
            //            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "PO is already approved .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            mgbox.Show();
            //        }
            //        else
            //        {
            //            if (((clsPurchaseOrderVO)dgPOList.SelectedItem).Freezed)
            //            {
            //                if (objUser.POApprovalLvlID > 0)
            //                {
            //                    ApproveFlag += 1;
            //                    if (ApproveFlag == 1)
            //                    {
            //                        MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
            //                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve PO with " + objUser.POApprovalLvl + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //                        msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

            //                        msgWindowUpdate.Show();
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "PO is not Freezed \n Please Freeze PO", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                mgbox.Show();
            //            }
            //        }
            //    }
            //    else
            //    {
            //        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No PO is Selected \n Please select a PO", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        mgbox.Show();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        // Added By CDS 
        //private void CmdChangeAndApprove_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (dgPOList.SelectedItem != null)
        //        {
        //            if (((clsPurchaseOrderVO)dgPOList.SelectedItem).IsApproveded)
        //            {
        //                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "PO is already approved .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                mgbox.Show();
        //            }
        //            //else if (((clsPurchaseOrderVO)dgPOList.SelectedItem).IsCancelded)
        //            //{
        //            //    MessageBoxControl.MessageBoxChildWindow msgWD1 =
        //            //   new MessageBoxControl.MessageBoxChildWindow("Palash", "P.O. is alrady cancel So you Can not Change And Approve PO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            //    msgWD1.Show();
        //            //    //return;
        //            //}
        //            else
        //            {
        //                if (((clsPurchaseOrderVO)dgPOList.SelectedItem).Freezed)
        //                {
        //                    //MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Verify and Approve PO ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
        //                    //mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
        //                    //{
        //                    //    if (res == MessageBoxResult.Yes)
        //                    //    {
        //                    //GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);

        //                    if (objUser.POApprovalLvlID > 0)
        //                    {
        //                        ApproveFlag += 1;
        //                        if (ApproveFlag == 1)
        //                        {
        //                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
        //                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve PO with " + objUser.POApprovalLvl + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
        //                            msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

        //                            msgWindowUpdate.Show();
        //                        }
        //                        //    }
        //                    }


        //                    //clsAddPurchaseOrderBizActionVO BizAction = new clsAddPurchaseOrderBizActionVO();

        //                    //frmPurchaseOrder FrmPOrder = new frmPurchaseOrder();

        //                    //FrmPOrder.IsApproveded = true;
        //                    //FrmPOrder.ISFromAprovePO = true;
        //                    //FrmPOrder.SelectedPO = (clsPurchaseOrderVO)dgPOList.SelectedItem;

        //                    //UserControl rootPage = Application.Current.RootVisual as UserControl;
        //                    //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
        //                    //mElement.Text = "Purchase Order";
        //                    //((IApplicationConfiguration)App.Current).OpenMainContent(FrmPOrder);
        //                    //    }
        //                    //};
        //                    //mgBox.Show();
        //                }
        //                else
        //                {
        //                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "PO is not Freezed \n Please Freeze PO", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                    mgbox.Show();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No PO is Selected \n Please select a PO", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            mgbox.Show();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        clsUserRightsVO objUser = new clsUserRightsVO();
        int ApproveFlag = 0;
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
                        //if (objUser.POApprovalLvlID > 0)
                        //{
                        //ApproveFlag += 1;
                        //if (ApproveFlag == 1)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //      new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve the PO with " + objUser.POApprovalLvl + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        //    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                        //    msgWindowUpdate.Show();
                        //}
                        //}
                        FillPurchaseOrderDataGrid();
                        ButtonVisibility();
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

        public void ButtonVisibility()
        {
            if (objUser.POApprovalLvlID == 1)
            {
                cmdApprove.Visibility = Visibility.Visible;
                CmdChangeApprove.Visibility = Visibility.Visible;
            }
            else if (objUser.POApprovalLvlID == 2)
            {
                cmdApprove.Visibility = Visibility.Visible;
                //CmdChangeApprove.Visibility = Visibility.Collapsed;//   Visibility.Visible;// //  
                CmdChangeApprove.Visibility = Visibility.Visible;//   Visibility.Visible;// //  Modification rights given to level 2 user on 10/10/2018
            }
            else 
            {
                cmdApprove.Visibility = Visibility.Collapsed;
                CmdChangeApprove.Visibility = Visibility.Collapsed;
            }
        }
        private void msgWindowUpdate_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (dgPOList.SelectedItem != null)
                {
                    clsAddPurchaseOrderBizActionVO BizAction = new clsAddPurchaseOrderBizActionVO();
                    frmPurchaseOrder FrmPOrder = new frmPurchaseOrder();
                    FrmPOrder.IsApproveded = true;
                    FrmPOrder.ISFromAprovePO = true;
                    FrmPOrder.POApproveLvlID = objUser.POApprovalLvlID;
                    FrmPOrder.SelectedPO = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Approve Purchase Order";
                    ((IApplicationConfiguration)App.Current).OpenMainContent(FrmPOrder);
                }
                else
                    ApproveFlag = 0;
            }
            else
            {
                ApproveFlag = 0;
            }
        }


        private void rdbUnApproved_Click(object sender, RoutedEventArgs e)
        {
            FillPurchaseOrderDataGrid();
        }
        private void rdbApproved_Click(object sender, RoutedEventArgs e)
        {
            FillPurchaseOrderDataGrid();
        }
    }
}
