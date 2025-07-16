using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using CIMS;
using System.Collections;
using PalashDynamics.Animations;
using System.Collections.ObjectModel;


using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
using PalashDynamics.ValueObjects.Inventory.Quotation;
using PalashDynamics.Collections;
using System.Text;
namespace PalashDynamics.Pharmacy
{
    public partial class ApprovePurchaseOrderAdminAccess : UserControl
    {
        public ApprovePurchaseOrderAdminAccess()
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
            dgPOList.Columns[0].Visibility = Visibility.Collapsed;
        }
        private SwivelAnimation objAnimation;
        public string _PurchaseOrderSource { get; set; }
        Boolean IsPageLoded = false;
        public ObservableCollection<clsPurchaseOrderDetailVO> PurchaseOrderItems { get; set; }
        public ObservableCollection<clsPurchaseOrderVO> purchaseOrderList { get; set; }
        public long _IndentID { get; set; }
        public long _IndentUnitID { get; set; }
        public long _EnquiryID { get; set; }
        public Boolean ISEditMode = false;
        public PurchaseOrderSource enmPurchaseOorder { get; set; }
        public CheckBox chkFreez;
        public string msgTitle = "";
        public string msgText = "";
        public enum PurchaseOrderSource
        {
            Indent,
            Quotation,

        };
        PurchaseOrderSource enumPurchaseOrderSource;


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
        WaitIndicator indicator = new WaitIndicator();

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
            SetCommandButtonState("ClickNew");
            PurchaseOrderItems.Clear();
            cboStore.SelectedValue = (long)0;
            cmbStore.SelectedValue = (long)0;
            FillStore();
            cboSupplier.SelectedValue = (long)0;
            cmbSupplier.SelectedValue = (long)0;
            //FillSupplier();
            cboDelivery.SelectedValue = (long)0;
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
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            PurchaseOrderItems.Clear();
            SetCommandButtonState("New");
            cboStore.SelectedValue = (long)0;
            cmbStore.SelectedValue = (long)0;
            FillStore();
            cboSupplier.SelectedValue = (long)0;
            cmbSupplier.SelectedValue = (long)0;
            FillSupplier();
            cboDelivery.SelectedValue = (long)0;
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


            objAnimation.Invoke(RotationType.Backward);


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //FillClinic();
            if (!IsPageLoded)
            {

                //FillStore1();
                FillDelivery();
                FillSchedule();
                FillTermsOfPayment();
                FillStore();
                FillModeofPayment();
                // FillStore1();


                PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                grdpodetails.ItemsSource = null;
                grdpodetails.ItemsSource = PurchaseOrderItems;

                purchaseOrderList = new ObservableCollection<clsPurchaseOrderVO>();
                dgPOList.ItemsSource = null;
                dgPOList.ItemsSource = purchaseOrderList;

                dtpDate.SelectedDate = DateTime.Now.Date;
                dtpFromDate.SelectedDate = DateTime.Now.Date;
                dtpToDate.SelectedDate = DateTime.Now.Date;
                rdbCash.IsChecked = true;

                SetCommandButtonState("New");
                IsPageLoded = true;
            }

        }

        private void FillSchedule()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Schedule;
                BizAction.MasterList = new List<MasterListItem>();

                //if (pClinicID > 0)
                //{
                //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
                //}

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

        //private void FillStore()
        //{
        //    try
        //    {
        //        clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
        //        //False when we want to fetch all items
        //        clsItemMasterVO obj = new clsItemMasterVO();
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



        //                clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--select--" };
        //                BizActionObj.ItemMatserDetails.Insert(0, Default);

        //                List<clsStoreVO> objList = new List<clsStoreVO>();
        //                objList = BizActionObj.ItemMatserDetails;
        //                   if (objList != null)
        //                {
        //                    //objList.Insert(0, new clsStoreVO { StoreName = "--Select--" });
        //                    cmbStore.ItemsSource = null;
        //                    cmbStore.ItemsSource = objList;//BizActionObj.ItemMatserDetails;
        //                    cboStore.ItemsSource = null;
        //                    cboStore.ItemsSource = objList;//BizActionObj.ItemMatserDetails;



        //                        if (objList.Count > 1)
        //                        {
        //                            cmbStore.SelectedItem = objList[1];
        //                            cboStore.SelectedItem = objList[1];
        //                        }
        //                        else
        //                        {
        //                            cmbStore.SelectedItem = objList[0];
        //                            cboStore.SelectedItem = objList[0];
        //                        }


        //                }
        //                FillSupplier();


        //                //var result = from item in BizActionObj.ItemMatserDetails
        //                //             where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
        //                //             select item;

        //                //List<clsStoreVO> objList = (List<clsStoreVO>)result.ToList();
        //                //objList.Insert(0, new clsStoreVO { StoreName = " --Select-- " });

        //                //cmbSearchStore.ItemsSource = objList;
        //                //cmbstore.ItemsSource = objList;

        //                //cmbSearchStore.SelectedItem = objList[0];
        //                //cmbstore.SelectedItem = objList[0];







        //            }

        //        };

        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        //Added By Pallavi
        private void FillStore()
        {
            try
            {
                clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();

                //False when we want to fetch all items
                clsItemMasterVO obj = new clsItemMasterVO();
                List<clsStoreVO> objList = new List<clsStoreVO>();

                obj.RetrieveDataFlag = false;
                BizActionObj.ItemMatserDetails = new List<clsStoreVO>();


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;



                        clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                        //BizActionObj.ItemMatserDetails.Insert(0, Default);


                        #region Commented
                        //if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        //{
                        //    var result = from item in BizActionObj.ItemMatserDetails
                        //                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                        //                 select item;
                        //    objList = (List<clsStoreVO>)result.ToList();

                        //}
                        //else
                        //{
                        //    objList = BizActionObj.ItemMatserDetails;
                        //    var result1 = from item in objList
                        //                  where item.Status == true
                        //                  select item;
                        //    objList = (List<clsStoreVO>)result1.ToList();
                        //}
                        #endregion
                        var result = from item in BizActionObj.ItemMatserDetails
                                     where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                     select item;
                        objList = (List<clsStoreVO>)result.ToList();

                        objList.Insert(0, Default);
                        if (objList != null)
                        {
                            //objList.Insert(0, new clsStoreVO { StoreName = "--Select--" });
                            cmbStore.ItemsSource = null;
                            //cmbStore.ItemsSource = objList;//BizActionObj.ItemMatserDetails;
                            cmbStore.ItemsSource = objList;
                            cboStore.ItemsSource = null;
                            //cboStore.ItemsSource = objList;//BizActionObj.ItemMatserDetails;
                            cboStore.ItemsSource = objList;

                            //if (objList.Count > 1)
                            //{
                            //    cmbStore.SelectedItem = objList[1];
                            //    cboStore.SelectedItem = objList[1];
                            //}
                            //else
                            //{
                            cmbStore.SelectedItem = objList[0];
                            cboStore.SelectedItem = objList[0];
                            //}


                        }

                        FillSupplier();


                        //var result = from item in BizActionObj.ItemMatserDetails
                        //             where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                        //             select item;

                        //List<clsStoreVO> objList = (List<clsStoreVO>)result.ToList();
                        //objList.Insert(0, new clsStoreVO { StoreName = " --Select-- " });

                        //cmbSearchStore.ItemsSource = objList;
                        //cmbstore.ItemsSource = objList;

                        //cmbSearchStore.SelectedItem = objList[0];
                        //cmbstore.SelectedItem = objList[0];







                    }

                };

                client.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillStore1()
        {
            try
            {
                clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;

                long clinicId = User.UserLoginInfo.UnitId;
                clsGetMasterListBizActionVO BizAction1 = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction1.MasterTable = MasterTableNameList.M_Store;
                BizAction1.MasterList = new List<MasterListItem>();

                if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                {
                    if (clinicId > 0)
                    {
                        BizAction1.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
                    }
                }

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

                        //cmbBloodGroup.ItemsSource = null;
                        //cmbBloodGroup.ItemsSource = objList;
                        cmbStore.ItemsSource = null;
                        cmbStore.ItemsSource = objList;

                        if (objList.Count > 1)
                            cmbStore.SelectedItem = objList[1];
                        else
                            cmbStore.SelectedItem = objList[0];
                    }

                };
            }
            catch (Exception)
            {

                throw;
            }

        }

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

        private void FillDelivery()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Delivery;
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
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
                BizAction.MasterList = new List<MasterListItem>();



                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        //objList.Add(new MasterListItem(1, "All"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);


                        cboSupplier.ItemsSource = null;
                        cboSupplier.ItemsSource = objList;
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;

                        //if (objList.Count > 1)
                        //{
                        //    cboSupplier.SelectedItem = objList[1];
                        //    cmbSupplier.SelectedItem = objList[1];
                        //}
                        //else
                        //{
                        cboSupplier.SelectedItem = objList[0];
                        cmbSupplier.SelectedItem = objList[0];
                        //}
                        FillPurchaseOrderDataGrid();

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

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void cboClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void cmdIndent_Click(object sender, RoutedEventArgs e)
        {
            if (rdbIndent.IsChecked == true)
            {

                enmPurchaseOorder = PurchaseOrderSource.Indent;

                IndnetSearch1 objIndentSearch = new IndnetSearch1();
                objIndentSearch.IsOnlyItems = true;
                objIndentSearch.OnItemSelectionCompleted += new IndnetSearch1.ItemSelection(objIndentSearch_OnItemSelectionCompleted);
                objIndentSearch.Show();
            }
            else
            {
                if (cboStore.SelectedItem == null)
                {
                    msgText = "Please Select Store";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    cboStore.Focus();

                }
                else if (((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
                {
                    msgText = "Please Select Store";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    cboStore.Focus();

                }
                else
                {
                    ItemList Itemswin = new ItemList();
                    Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                    Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    Itemswin.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                    Itemswin.cmbStore.IsEnabled = false;
                    Itemswin.ShowBatches = false;
                    Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                    Itemswin.Show();
                }
            }





        }

        void objIndentSearch_OnItemSelectionCompleted(object sender, EventArgs e)
        {


            //IndentSearch objItemSearch = (IndentSearch)sender;
            IndnetSearch1 objItemSearch = (IndnetSearch1)sender;
            List<clsItemListByIndentId> objList = objItemSearch.ocSelectedItemList.ToList<clsItemListByIndentId>();
            //txtIndentNo.Text = 
            if (objList != null)
            {

                _IndentID = (long)(objList[0].IndentId == null ? 0 : objList[0].IndentId);
                _IndentUnitID = objList[0].IndentUnitID;
                txtIndentNo.Text = objList[0].IndentNumber;
            }


            // _IndentID = (long)(objItemSearch.SelectedIndent.ID == null ? 0 : objItemSearch.SelectedIndent.ID);
            ObservableCollection<clsItemListByIndentId> selectedIndentItems;
            //obj3 = new ObservableCollection<clsIssueItemDetailsVO>();
            objItemSearch.IsOnlyItems = true;
            selectedIndentItems = objItemSearch.ocSelectedItemList;
            //txtIndentNo.Text = selectedIndentItems;

            if (selectedIndentItems != null)
            {
                foreach (var item in selectedIndentItems)
                {
                    if (PurchaseOrderItems.Where(poItems => poItems.ItemID == item.ItemId).Any() == true)
                    {
                        if (_IndentID != item.IndentId)
                            PurchaseOrderItems.Select(poItems => { poItems.Quantity = poItems.Quantity + (decimal)item.BalanceQty; return poItems; }).ToList();
                    }
                    else
                        PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO { ItemID = item.ItemId, ItemName = item.ItemName, ItemCode = item.ItemCode, PUM = item.PUM, Quantity = Convert.ToDecimal(item.BalanceQty), Rate = (decimal)item.PurchaseRate, VATPercent = (decimal)item.VATPer });
                }
                grdpodetails.Focus();
                grdpodetails.UpdateLayout();

            }
            CalculateOpeningBalanceSummary();


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
                    //Added By Pallavi
                    bool isValid = true;
                    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                    if (PurchaseOrderItems == null || PurchaseOrderItems.Count == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "At least one item is compulsory for saving purchase order", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        isValid = false;
                        ClickedFlag1 = 0;
                        return;

                    }
                    if (((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
                    {
                        cboStore.TextBox.SetValidation("Please Select The Store");
                        cboStore.TextBox.RaiseValidationError();
                        cboStore.Focus();
                        isValid = false;
                        ClickedFlag1 = 0;
                        return;

                    }
                    else if (((clsStoreVO)cboStore.SelectedItem) == null)
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

                    if (((MasterListItem)cboSupplier.SelectedItem).ID == 0)
                    {
                        cboSupplier.TextBox.SetValidation("Please Select The Supplier");
                        cboSupplier.TextBox.RaiseValidationError();
                        cboSupplier.Focus();
                        isValid = false;
                        ClickedFlag1 = 0;
                        return;

                    }
                    else if (((MasterListItem)cboSupplier.SelectedItem) == null)
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

                    if (PurchaseOrderItems.Count > 0 && PurchaseOrderItems != null)
                    {
                        List<clsPurchaseOrderDetailVO> objList = PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>();

                        if (objList != null && objList.Count > 0)
                        {
                            foreach (var item in objList)
                            {
                                if (item.Quantity == 0)
                                {
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Quantity Zero!.", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                    isValid = false;
                                    break;


                                }
                                if (item.DiscountPercent > 100)
                                {
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow("", "Discount percent must not be greater than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

                    ////////////////////////////
                    if (isValid)
                    {
                        clsAddPurchaseOrderBizActionVO objBizActionVO = new clsAddPurchaseOrderBizActionVO();

                        objBizActionVO.PurchaseOrder = new clsPurchaseOrderVO();
                        objBizActionVO.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        objBizActionVO.PurchaseOrder.Date = dtpDate.SelectedDate.Value;
                        objBizActionVO.PurchaseOrder.Time = DateTime.Now;
                        objBizActionVO.PurchaseOrder.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                        objBizActionVO.PurchaseOrder.SupplierID = ((MasterListItem)cboSupplier.SelectedItem).ID;
                        objBizActionVO.IsEditMode = ISEditMode;
                        objBizActionVO.PurchaseOrder.ID = ((clsPurchaseOrderVO)this.DataContext).ID;
                        if (enmPurchaseOorder == PurchaseOrderSource.Indent)
                        {
                            objBizActionVO.PurchaseOrder.IndentID = _IndentID;
                            objBizActionVO.PurchaseOrder.IndentUnitID = _IndentUnitID;
                            objBizActionVO.PurchaseOrder.EnquiryID = 0;
                        }
                        else if (enmPurchaseOorder == PurchaseOrderSource.Quotation)
                        {
                            objBizActionVO.PurchaseOrder.IndentID = 0;
                            objBizActionVO.PurchaseOrder.EnquiryID = _EnquiryID;
                        }
                        objBizActionVO.PurchaseOrder.DeliveryDuration = ((MasterListItem)cboDelivery.SelectedItem).ID;
                        //if (rdbCash.IsChecked == true)
                        //{
                        //    objBizActionVO.PurchaseOrder.PaymentModeID = (long)PaymentMode.Cash;
                        //}
                        //else if (rdbCheque.IsChecked == true)
                        //{
                        //    objBizActionVO.PurchaseOrder.PaymentModeID = (long)PaymentMode.Cheque;
                        //}
                        //else if (rdbAccountTransfer.IsChecked == true)
                        //{
                        //    objBizActionVO.PurchaseOrder.PaymentModeID = (long)PaymentMode.AccountTransfer;
                        //}

                        objBizActionVO.PurchaseOrder.PaymentModeID = ((MasterListItem)cmbModeofPayment.SelectedItem).ID;
                        objBizActionVO.PurchaseOrder.PaymentTerms = ((MasterListItem)cboTOP.SelectedItem).ID;
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
                        objBizActionVO.PurchaseOrder.UpdatedOn = DateTime.Now.DayOfWeek.ToString();
                        objBizActionVO.PurchaseOrder.UpdatedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                        objBizActionVO.PurchaseOrder.UpdatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                        ObservableCollection<clsPurchaseOrderDetailVO> objObservable = (ObservableCollection<clsPurchaseOrderDetailVO>)grdpodetails.ItemsSource;
                        objBizActionVO.PurchaseOrder.Items = objObservable.ToList<clsPurchaseOrderDetailVO>();


                        // List<clsPurchaseOrderDetailVO> objPOItems=  (List<clsPurchaseOrderDetailVO>)grdpodetails.ItemsSource;
                        //objBizActionVO.PurchaseOrder.Items = new List<clsPurchaseOrderDetailVO>();
                        //objBizActionVO.PurchaseOrder.Items = objPOItems;//PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>();
                        //objBizActionVO.PurchaseOrderList = new List<clsPurchaseOrderDetailVO>();
                        //objBizActionVO.PurchaseOrderList = PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>();
                        if (objBizActionVO.PurchaseOrder.Items.Count > 0)
                        {


                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            Client.ProcessCompleted += (s, arg) =>
                            {
                                ClickedFlag1 = 0;
                                if (arg.Error == null && ((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder != null)
                                {
                                    if (arg.Result != null)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Purchase Order", "Purchase Order Number" + " " + ((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder.PONO + " " + "saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        SetCommandButtonState("Save");
                                        msgW1.Show();
                                        PurchaseOrderItems.Clear();
                                        objAnimation.Invoke(RotationType.Backward);
                                        FillPurchaseOrderDataGrid();

                                    }
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("Purchase Order", "Some error occurred while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }

                            };

                            Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                            Client.CloseAsync();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Purchase Order", "Please select Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                    }
                    else
                        ClickedFlag1 = 0;
                }
            }
            catch (Exception)
            {
                ClickedFlag1 = 0;
                throw;
            }





        }

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {

            //ItemList Itemswin = new ItemList();
            //Itemswin.ShowBatches = false;
            //Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
            //Itemswin.Show();

        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;

            if (Itemswin.SelectedItems != null)
            {


                foreach (var item in Itemswin.SelectedItems)
                {
                    //bool isExist = CheckForItemExistance(item.ID);
                    //if (!isExist)
                    //{

                    if (PurchaseOrderItems.Where(poItems => poItems.ItemID == item.ID).Any() == false)
                        PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO { ItemID = item.ID, ItemName = item.ItemName, ItemCode = item.ItemCode, PUM = item.PUOM, VATPercent = item.VatPer, Rate = item.PurchaseRate, Quantity = 1 });
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Product is alredy added!", "Please add another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                    //}


                }
                CalculateOpeningBalanceSummary();
                grdpodetails.Focus();
                grdpodetails.UpdateLayout();

                // dgAddGRNItems.SelectedIndex = GRNAddedItems.Count - 1;
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
                objBizActionVO.SearchStoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId == null ? 0 : ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                objBizActionVO.SearchSupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;

                objBizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                objBizActionVO.PagingEnabled = true;
                objBizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                objBizActionVO.MaximumRows = DataList.PageSize;


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
                            if (objPurchaseOrderList.PurchaseOrderList != null)
                            {
                                DataList.Clear();

                                DataList.TotalItemCount = objPurchaseOrderList.TotalRowCount;
                                foreach (var item in objPurchaseOrderList.PurchaseOrderList)
                                {
                                    if (item.IsApproveded == true)
                                    {
                                        item.IsApprovededEnable = false;
                                    }
                                    else
                                    {
                                        item.IsApprovededEnable = true;
                                    }

                                    DataList.Add(item);
                                }
                                dgPOList.ItemsSource = null;
                                dgPOList.ItemsSource = DataList;
                                grdpo.ItemsSource = null;
                                purchaseOrderList.Clear();
                                datapager.Source = DataList;
                                grdpo.ItemsSource = null;

                                if (CancelPO == true)
                                {
                                    msgText = "Purchase Order Deleted successfully .";

                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();
                                }

                            }

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
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = false;
                    //cboStore.SelectedValue = 0;
                    cmdPrint.IsEnabled = true;
                    break;

                case "Save":

                    cmdNew.IsEnabled = true;
                    cmdPrint.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = false;
                    cboStore.SelectedValue = 0;
                    cmbStore.SelectedValue = 0;
                    //cboStore.ItemsSource = null;
                    //cmbStore.ItemsSource = null;
                    //FillStore();
                    //cboSupplier.ItemsSource = null;
                    //cmbSupplier.ItemsSource = null;
                    //FillSupplier();
                    //cboDelivery.ItemsSource = null;
                    //FillDelivery();
                    //cmbSchedule.ItemsSource = null;
                    //FillSchedule();
                    //cboTOP.ItemsSource = null;
                    //FillTermsOfPayment();
                    cboSupplier.SelectedValue = 0;
                    cboSupplier.SelectedValue = 0;
                    cboDelivery.SelectedValue = 0;
                    cmbSchedule.SelectedValue = 0;
                    cboTOP.SelectedValue = 0;



                    break;

                case "Modify":
                    cmdPrint.IsEnabled = false;
                    cmdApprove.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdSave.Content = "Modify";
                    cmdCancel.IsEnabled = true;
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
        void grdpodetails_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.DisplayIndex == 6)
            {
                if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent > 100)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Discount Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent = 0;
                    return;
                }
                if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent == 100)
                {
                    if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent != 0)
                    {
                        orginalVatPer = ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent;
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                    }

                }
                if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent == 0)
                {
                    if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent < 100)
                    {
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = orginalVatPer;
                    }
                }
            }

            if (e.Column.DisplayIndex == 8)
            {
                if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent > 100)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("", "VAT Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                    return;
                }
                if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent == 100)
                {
                    if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent > 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                   new MessageBoxControl.MessageBoxChildWindow("", "Discount percentage 100%. Can't Add Vat Percent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                        return;
                    }
                }
            }
            //throw new NotImplementedException();
            if (grdpodetails.SelectedItem != null)
            {
                //if (e.Column.DisplayIndex == 3|| e.Column.DisplayIndex == 4 || e.Column.DisplayIndex == 8
                //   || e.Column.DisplayIndex == 10 || e.Column.DisplayIndex == 11||e.Column.DisplayIndex == 6)
                //    CalculateOpeningBalanceSummary();
                //   return;
                CalculateOpeningBalanceSummary();



            }



        }

        #endregion


        #region calculate summary
        private void CalculateOpeningBalanceSummary()
        {
            decimal VATAmount, Amount, NetAmount, DiscountAmount;

            VATAmount = Amount = NetAmount = DiscountAmount = 0;
            ObservableCollection<clsPurchaseOrderDetailVO> objObserv = (ObservableCollection<clsPurchaseOrderDetailVO>)grdpodetails.ItemsSource;
            List<clsPurchaseOrderDetailVO> objPOItems = objObserv.ToList<clsPurchaseOrderDetailVO>(); //((ObservableCollection<clsPurchaseOrderDetailVO>) PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>());
            //List<clsPurchaseOrderDetailVO> objPOItems = (List<clsPurchaseOrderDetailVO>)grdpodetails.ItemsSource;


            foreach (var item in objPOItems)
            {

                Amount += item.Amount;
                DiscountAmount += item.DiscountAmount;
                VATAmount += item.VATAmount;
                NetAmount += item.NetAmount;

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
            if (dgPOList.SelectedIndex != -1)
            {


                try
                {



                    clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();
                    clsPurchaseOrderVO objList = (clsPurchaseOrderVO)dgPOList.SelectedItem;

                    objBizActionVO.SearchID = objList.ID;
                    objBizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
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
                                grdpo.ItemsSource = obj.PurchaseOrderList;

                            }
                        }
                        else
                        {
                            //Indicatior.Close();
                            //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
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
                    if ((clsPurchaseOrderVO)dgPOList.SelectedItem != null)
                    {
                        objFreezPurchaseOrder.PurchaseOrder.ID = objSelectedPO.ID;
                        objFreezPurchaseOrder.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    }
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                //if (((clsPurchaseOrderVO)dgPOList.SelectedItem).Freezed == true)
                                //{

                                //}
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "Purchase Order Freezed Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                // ((clsPurchaseOrderVO)dgPOList.SelectedItem).IsApprovededEnable = true;

                                foreach (var item in DataList)
                                {
                                    if (item.ID == ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID)
                                    {
                                        item.Freezed = true;
                                        item.IsEnabledFreezed = false;
                                        // item.IsApproveded = true;
                                        item.IsApprovededEnable = true;
                                    }
                                    else
                                    {
                                        if (item.IsApproveded == true && item.ApprovedBy != "")
                                        {
                                            if (Approved == true)
                                            {
                                                item.IsApprovededEnable = false;
                                            }
                                            else
                                            {
                                                if (item.IsApprovededEnable != true)
                                                {
                                                    item.IsApprovededEnable = true;
                                                }
                                                else
                                                {
                                                    item.IsApprovededEnable = false;
                                                }

                                            }
                                        }
                                        else
                                        {
                                            item.IsApprovededEnable = true;
                                        }
                                    }


                                }
                                dgPOList.ItemsSource = null;
                                dgPOList.ItemsSource = DataList;
                            }
                        }
                        else
                        {



                            //Indicatior.Close();
                            //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
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

                    foreach (var item in DataList)
                    {
                        if (item.ID == ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID)
                        {
                            item.Freezed = false;
                            item.IsEnabledFreezed = true;
                            item.IsApproveded = false;
                        }
                        item.IsApprovededEnable = true;


                    }
                    dgPOList.ItemsSource = null;
                    dgPOList.ItemsSource = DataList;
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
                clsPurchaseOrderVO obj = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                if (obj != null)
                {
                    this.DataContext = obj;
                    ((clsPurchaseOrderVO)this.DataContext).StoreID = obj.StoreID;
                    ((clsPurchaseOrderVO)this.DataContext).ID = obj.ID;
                    _IndentID = ((clsPurchaseOrderVO)this.DataContext).IndentID;
                    _IndentUnitID = ((clsPurchaseOrderVO)this.DataContext).IndentUnitID;
                    if (((clsPurchaseOrderVO)this.DataContext).IndentNumber != null)
                        txtIndentNo.Text = ((clsPurchaseOrderVO)this.DataContext).IndentNumber;





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
                    }

                    //((clsStoreVO)cboStore.SelectedItem).StoreName = obj.StoreName;
                    //((MasterListItem)cboSupplier.SelectedItem).Description = obj.SupplierName;
                    // cboDelivery.SelectedValue = obj.DeliveryDuration;

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

                    //cmbSchedule.SelectedValue = obj.Schedule;

                    var results5 = from r in ((List<MasterListItem>)cmbSchedule.ItemsSource)
                                   where r.ID == obj.Schedule
                                   select r;

                    foreach (MasterListItem item in results5)
                    {
                        cmbSchedule.SelectedItem = item;
                    }
                    //switch (obj.PaymentMode)
                    //{
                    //case 1:
                    //    rdbCash.IsChecked = true;
                    //    break;
                    //case 2:
                    //    rdbCheque.IsChecked = true;
                    //    break;
                    //case 3:
                    //    rdbAccountTransfer.IsChecked = true;
                    //    break;
                    //default:
                    //    rdbCash.IsChecked = true;
                    //    rdbCheque.IsChecked = false;
                    //    rdbAccountTransfer.IsChecked = false;
                    //    break;

                    cmbModeofPayment.SelectedValue = obj.PaymentMode;
                    // };


                    if (obj.IsApproveded == true)
                    {
                        cmdSave.IsEnabled = true;
                        cmdNew.IsEnabled = false;
                        cmdPrint.IsEnabled = true;
                        cmdSave.Content = "Modify";
                        cmdApprove.IsEnabled = false;
                        cmdCancel.IsEnabled = true;
                        obj.IsEnabledFreezed = false;
                    }
                    else
                    {
                        cmdSave.IsEnabled = true;
                        SetCommandButtonState("Modify");
                    }
                    FillPODetails(obj.ID);
                    ISEditMode = true;
                    dgPOList.UpdateLayout();
                    dgPOList.Focus();

                    objAnimation.Invoke(RotationType.Forward);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillPODetails(long POID)
        {
            try
            {
                clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();
                clsPurchaseOrderVO objList = (clsPurchaseOrderVO)dgPOList.SelectedItem;

                objBizActionVO.SearchID = POID;
                objBizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
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

                            List<clsPurchaseOrderDetailVO> objListOfItems = (List<clsPurchaseOrderDetailVO>)obj.PurchaseOrderList;
                            if (objListOfItems != null)
                            {
                                foreach (var item in objListOfItems)
                                {
                                    PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO
                                    {
                                        ItemID = item.ItemID,
                                        ItemName = item.ItemName,
                                        ItemCode = item.ItemCode,
                                        Quantity = (decimal)item.Quantity,
                                        Rate = (decimal)item.Rate,
                                        Amount = (decimal)item.Amount,
                                        DiscountPercent = (decimal)item.DiscountPercent,
                                        DiscountAmount = (decimal)item.DiscountAmount,
                                        VATPercent = (decimal)item.VATPercent,
                                        VATAmount = (decimal)item.VATAmount,
                                        NetAmount = (decimal)item.NetAmount,
                                        Specification = item.Specification,
                                        PUM = item.PUM
                                    });
                                };
                                grdpodetails.Focus();
                                grdpodetails.UpdateLayout();
                            }
                            //PurchaseOrderItems =(ObservableCollection<clsPurchaseOrderDetailVO>) obj.PurchaseOrderList;

                            //grdpodetails.ItemsSource = null;
                            //grdpodetails.ItemsSource = obj.PurchaseOrderList;

                        }
                    }
                    else
                    {
                        //Indicatior.Close();
                        //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
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

        private void FillPODetails()
        {
            throw new NotImplementedException();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long POId = 0;
            long PUnitID = 0;
            clsPurchaseOrderVO obj = (clsPurchaseOrderVO)dgPOList.SelectedItem;


            if (obj != null)
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
                //msgText = "Please Select a PO to Print";

                //MessageBoxControl.MessageBoxChildWindow msgW =
                //      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                //msgW.Show();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Select a PO to Print.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        private void cmdDeleteFreeService_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (((clsPurchaseOrderVO)dgPOList.SelectedItem) != null)
                {
                    //if (((clsPurchaseOrderVO)dgPOList.SelectedItem).IsApprovededEnable == false)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgWD1 =
                    // new MessageBoxControl.MessageBoxChildWindow("", "Cannot cancel the P.O. The P.O. is Approved", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgWD1.Show();
                    //    foreach (var item in DataList)
                    //    {
                    //        //if (item.ID == ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID)
                    //        //{
                    //        //    item.Freezed = false;
                    //        //    item.IsEnabledFreezed = true;
                    //        //    item.IsApproveded = false;
                    //        //}
                    //        //item.IsApprovededEnable = true;

                    //        if (item.Freezed == true && item.IsApprovededEnable == false)
                    //        {
                    //            item.Freezed = true;
                    //            item.IsApprovededEnable = false;
                    //            item.IsApproveded = true;
                    //        }
                    //        else if (item.Freezed == true && item.IsApprovededEnable == true)
                    //        {
                    //            item.Freezed = true;
                    //            item.IsApprovededEnable = true;
                    //            item.IsApproveded = false;
                    //        }
                    //        else
                    //        {
                    //            item.Freezed = false;
                    //            item.IsApprovededEnable = true;
                    //            item.IsApproveded = false;
                    //        }


                    //    }
                    //    dgPOList.ItemsSource = null;
                    //    dgPOList.ItemsSource = DataList;
                    //    return;
                    //}
                    //else
                    //{
                    string msgTitle = "";
                    string msgText = "Are you sure you want to cancel the P.O. ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            POCancellationWindow Win = new POCancellationWindow();
                            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                            Win.OnCancelButton_Click += new RoutedEventHandler(Win_OnCancelButton_Click);
                            Win.Show();




                        }
                        else
                        {
                            foreach (var item in DataList)
                            {
                                if (item.ID == ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID)
                                {
                                    if (item.Freezed == true && item.IsApprovededEnable == false)
                                    {
                                        item.Freezed = true;
                                        item.IsApprovededEnable = false;
                                        item.IsApproveded = true;
                                    }
                                    else if (item.Freezed == true && item.IsApprovededEnable == true)
                                    {
                                        item.Freezed = true;
                                        item.IsApprovededEnable = true;
                                        item.IsApproveded = false;
                                    }
                                    else
                                    {
                                        item.Freezed = false;
                                        item.IsApprovededEnable = true;
                                        item.IsApproveded = false;
                                    }
                                }


                            }
                            dgPOList.ItemsSource = null;
                            dgPOList.ItemsSource = DataList;
                        }
                    };
                    msgWD.Show();


                    // }
                }

            }



            catch (Exception)
            {
                throw;
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (grdpodetails.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to delete the item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {


                        PurchaseOrderItems.RemoveAt(grdpodetails.SelectedIndex);
                        grdpodetails.Focus();
                        grdpodetails.UpdateLayout();
                        grdpodetails.SelectedIndex = PurchaseOrderItems.Count - 1;
                        CalculateOpeningBalanceSummary();
                    }
                    else
                    {
                        foreach (var item in DataList)
                        {
                            if (item.ID == ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID)
                            {
                                item.Freezed = false;
                                item.IsEnabledFreezed = true;
                                item.IsApproveded = false;
                            }
                            item.IsApprovededEnable = true;


                        }
                        dgPOList.ItemsSource = null;
                        dgPOList.ItemsSource = DataList;
                    }
                };
                msgWD.Show();
            }
            else
            {
                foreach (var item in DataList)
                {
                    if (item.ID == ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID)
                    {
                        item.Freezed = false;
                        item.IsEnabledFreezed = true;
                        item.IsApproveded = false;
                    }
                    item.IsApprovededEnable = true;


                }
                dgPOList.ItemsSource = null;
                dgPOList.ItemsSource = DataList;
            }

        }
        bool CancelPO = false;
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            clsUpdateRemarkForCancellationPO bizactionVO = new clsUpdateRemarkForCancellationPO();
            bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();
            Approved = true;
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
                        //if (((clsUpdatePurchaseOrderForApproval)args.Result).SuccessStatus == 1)
                        //{

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
                                //Indicatior.Close();
                                //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding P O details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }

                        };

                        Client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();






                        //After Insertion Back to BackPanel and Setup Page
                        // objAnimation.Invoke(RotationType.Backward);

                        //FetchData();
                        //  FillPurchaseOrderDataGrid();

                        // }

                    }

                };
                client.ProcessAsync(bizactionVO, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {

            }
        }
        void Win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdbIndent_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PurchaseOrderItems != null)
                    PurchaseOrderItems.Clear();
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        private void cboStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboStore.SelectedItem != null)
                {
                    if (rdbDirect.IsChecked == true)
                    {
                        if (PurchaseOrderItems != null)

                            PurchaseOrderItems.Clear();
                    }
                }

            }
            catch (Exception Ex)
            {
                throw;
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
                        txtIndentNo.Text = "";

                        _IndentID = 0;
                        _IndentUnitID = 0;
                        //((clsPurchaseOrderVO)this.DataContext).IndentID = 0;
                        //((clsPurchaseOrderVO)this.DataContext).IndentUnitID = 0;
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

            if (cmbSupplier.SelectedItem != null)
                objBizActionVO.SupplierId = ((MasterListItem)cboSupplier.SelectedItem).ID;

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


        long POID = 0;
        StringBuilder strError = new StringBuilder();
        List<long> IDS = new System.Collections.Generic.List<long>();
        List<long> PreviousIDS = new System.Collections.Generic.List<long>();
        bool Approved = false;
        private void Approve_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {

                //POID=((clsPurchaseOrderVO)dgPOList.SelectedItem).ID;

                //strError.Append(",");
                //strError.Append(POID);
                IDS.Add(((clsPurchaseOrderVO)dgPOList.SelectedItem).ID);

                if (((clsPurchaseOrderVO)dgPOList.SelectedItem).Freezed != true)
                {
                    msgText = "Are you sure you want to Freeze the  Details";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
                else
                {
                    //((CheckBox)sender).IsChecked = false;
                    //chkFreez.IsChecked = false;
                    //chkFreez.IsEnabled = true;
                    //((CheckBox)sender).IsEnabled = true;

                    foreach (var item in DataList)
                    {
                        if (item.ID == ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID)
                        {
                            if (item.Freezed == true)
                            {
                                item.Freezed = true;
                                item.IsApprovededEnable = true;
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
                            // item.IsApprovededEnable = false;
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

        private void cmdPOApprove_Click(object sender, RoutedEventArgs e)
        {



            if (IDS.Count > 0)
            {
                clsUpdatePurchaseOrderForApproval bizactionVO = new clsUpdatePurchaseOrderForApproval();
                bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();
                Approved = true;
                try
                {
                    bizactionVO.PurchaseOrder.ids = IDS;
                    bizactionVO.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    bizactionVO.PurchaseOrder.ApprovedBy = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            //if (((clsUpdatePurchaseOrderForApproval)args.Result).SuccessStatus == 1)
                            //{
                            msgText = "Purchase Order Approved successfully .";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            //After Insertion Back to BackPanel and Setup Page
                            // objAnimation.Invoke(RotationType.Backward);

                            //FetchData();
                            FillPurchaseOrderDataGrid();

                            // }



                        }

                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();

                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                Approved = false;
                msgText = "Please Select The Purchase Order to approve.";

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