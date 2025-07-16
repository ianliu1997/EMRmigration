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
using CIMS;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Windows.Browser;
using PalashDynamics.Pharmacy.ItemSearch;


namespace PalashDynamics.Pharmacy
{
    public partial class GRNReturn : UserControl
    {
        #region Variable Declaration
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        bool IsPageLoaded = false;
        long GRNUnitID { get; set; }
        private ObservableCollection<clsGRNReturnDetailsVO> _GRNReturnAddedItems;
        public ObservableCollection<clsGRNReturnDetailsVO> GRNReturnAddedItems { get { return _GRNReturnAddedItems; } set { _GRNReturnAddedItems = value; } }
        private long _StoreID;
        public long StoreID
        {
            get
            { return _StoreID; }
            set
            { _StoreID = value; }
        }

        public clsGRNReturnVO GRNReturnMaster { get; set; }
        int ClickedFlag1 = 0;
        long GRNGRNID;
        long GRNStoreID;
        long GRNSupplierID;
        long GRNGRNUnitID;
        string GrnNo;
        #endregion

        #region 'Paging'

        public PagedSortableCollectionView<clsGRNReturnVO> DataList { get; private set; }

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
            FillGRNReturnSearchList();

        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        private void FillGRNReturnSearchList()
        {
            indicator.Show();

            dgGRNReturnList.ItemsSource = null;

            clsGetGRNReturnListBizActionVO BizAction = new clsGetGRNReturnListBizActionVO();
            //BizAction.IsActive = true;
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date.Date;

            if (cmbSearchSupplier.SelectedItem != null)
                BizAction.SupplierID = ((MasterListItem)cmbSearchSupplier.SelectedItem).ID;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetGRNReturnListBizActionVO result = e.Result as clsGetGRNReturnListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;

                    if (result.List != null)
                    {
                        DataList.Clear();
                        foreach (var item in result.List)
                        {
                            DataList.Add(item);
                        }

                        dgGRNReturnList.ItemsSource = null;
                        dgGRNReturnList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList;
                    }

                }
                indicator.Close();

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #endregion

        #region Constructor
        public GRNReturn()
        {
            try
            {
                InitializeComponent();
                _flip = new PalashDynamics.Animations.SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

                //======================================================
                //Paging
                DataList = new PagedSortableCollectionView<clsGRNReturnVO>();
                DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                DataListPageSize = 15;
                dgDataPager.PageSize = DataListPageSize;
                dgDataPager.Source = DataList;
                cmbGRNSupplier.IsEnabled = false;
                //======================================================

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
       
        /// <summary>
        /// New button click
        /// </summary>
        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            InitializeForm();
        }

        private void InitializeForm()
        {

            StoreID = 0;
            _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
            grdGRNAdd.ItemsSource = GRNReturnAddedItems;
            GRNReturnAddedItems.Clear();
            GRNReturnMaster = new clsGRNReturnVO() { GoodReturnType = InventoryGoodReturnType.AgainstGRN, PaymentModeID = MaterPayModeList.Cash };
            CmdNew.IsEnabled = false;
            CmdPrint.IsEnabled = false;
            CmdClose.IsEnabled = true;
            rdbCash.IsChecked = true;
            rdbGRNReturn.IsChecked = true;
            txtGrnNo.Text = "";
            dpgrDt.SelectedDate = DateTime.Now;
            CalculateGRNSummary();
            _flip.Invoke(RotationType.Forward);
            this.DataContext = new clsGRNReturnVO();
            cmbGRNSupplier.SelectedValue = ((clsGRNReturnVO)this.DataContext).SupplierID;
            cmbSearchSupplier.SelectedValue = ((clsGRNReturnVO)this.DataContext).SupplierID;
        }

        /// <summary>
        /// Save button click
        /// </summary>
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 = ClickedFlag1 + 1;
            bool isValid = true;
            MessageBoxControl.MessageBoxChildWindow mgbx1 = null;
            if (GRNReturnMaster.GoodReturnType == InventoryGoodReturnType.Direct)
            {
                if (cmbGRNSupplier.SelectedItem == null)
                {
                    cmbGRNSupplier.TextBox.SetValidation("Please select the Supplier");
                    cmbGRNSupplier.RaiseValidationError();
                    isValid = false;
                    cmbGRNSupplier.Focus();
                    ClickedFlag1 = 0;
                    mgbx1 = new MessageBoxControl.MessageBoxChildWindow("", "Please select the Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx1.Show();
                    return;
                }
                else if (((MasterListItem)cmbGRNSupplier.SelectedItem).ID == 0)
                {
                    cmbGRNSupplier.TextBox.SetValidation("Please select the Supplier");
                    cmbGRNSupplier.RaiseValidationError();
                    isValid = false;
                    cmbGRNSupplier.Focus();
                    ClickedFlag1 = 0;
                    mgbx1 = new MessageBoxControl.MessageBoxChildWindow("", "Please select the Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx1.Show();
                    return;
                }
                else
                {
                    cmbGRNSupplier.ClearValidationError();
                    GRNReturnMaster.SupplierID = ((MasterListItem)cmbGRNSupplier.SelectedItem).ID;
                }
            }

            if (GRNReturnAddedItems.Count > 0 && GRNReturnMaster != null)
            {
                List<clsGRNReturnDetailsVO> objList = GRNReturnAddedItems.ToList<clsGRNReturnDetailsVO>();
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                if (objList != null && objList.Count > 0)
                {
                    foreach (var item in objList)
                    {
                        if (item.ReturnedQuantity == 0)
                        {
                            mgbx = new MessageBoxControl.MessageBoxChildWindow("Returned Quantity Zero!.", "Return Quantity In The List Can't Be Zero. Please Enter Return Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            isValid = false;
                            break;

                        }
                    }
                }

                if (isValid)
                {
                    SaveGoodsReturn();
                }
                else
                    ClickedFlag1 = 0;
            }
        }

        /// <summary>
        /// Saves GRN return
        /// </summary>
        private void SaveGoodsReturn()
        {
            PalashDynamics.UserControls.WaitIndicator wt = new UserControls.WaitIndicator();

            clsAddGRNReturnBizActionVO BizAction = new clsAddGRNReturnBizActionVO();
            GRNReturnMaster.Items = GRNReturnAddedItems.ToList<clsGRNReturnDetailsVO>();
            BizAction.Details = GRNReturnMaster;

            if (GRNReturnMaster.StoreID == 0)
                GRNReturnMaster.StoreID = GRNStoreID;
            if (GRNReturnMaster.SupplierID == 0)
                GRNReturnMaster.SupplierID = GRNSupplierID;

            //User Related Values
            BizAction.Details.UnitId = Convert.ToInt64(((clsUserLoginInfoVO)((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo).UnitId);
            BizAction.Details.CreatedUnitId = Convert.ToInt64(((clsUserLoginInfoVO)((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo).UnitId);
            BizAction.Details.Status = true;
            BizAction.Details.AddedBy = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
            BizAction.Details.AddedOn = ((clsUserLoginInfoVO)((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo).MachineName;
            BizAction.Details.AddedDateTime = Convert.ToDateTime(DateTime.Now);
            BizAction.Details.AddedWindowsLoginName = ((clsUserLoginInfoVO)((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo).Name;
            BizAction.Details.GRNUnitID = GRNGRNUnitID;

            if (dpgrDt.SelectedDate == null)
            {
                BizAction.Details.Date = DateTime.Now;
            }
            else
            {

                BizAction.Details.Date = Convert.ToDateTime(dpgrDt.SelectedDate);

            }


            //BizAction.Details.StoreID = 
            BizAction.Details.Time = (DateTime)BizAction.Details.Date;
            if (rdbCash.IsChecked == true)
                BizAction.Details.PaymentModeID = MaterPayModeList.Cash;
            else if (rdbCheque.IsChecked == true)
                BizAction.Details.PaymentModeID = MaterPayModeList.Credit;

            wt.Show();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, args) =>
            {
                ClickedFlag1 = 0;
                if (args.Error == null && args.Result != null)
                {
                    dgGRNReturnList.ItemsSource = null;
                    dgGRNReturnItems.ItemsSource = null;

                    _flip.Invoke(RotationType.Backward);
                    CmdNew.IsEnabled = true;
                    CmdPrint.IsEnabled = true;
                    StoreID = 0;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = false;
                    FillGRNReturnSearchList();
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash.", "GRN Return Saved Successfully With GRN Return No. " + ((clsAddGRNReturnBizActionVO)args.Result).Details.GRNReturnNO, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash.", "GRN Return Not Saved. \n Some Error Occured while saving information.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                }
                wt.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        /// <summary>
        /// Cancel button click
        /// </summary>
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

            //dgGRNReturnList.ItemsSource = null; Commented By Pallavi
            //dgGRNReturnItems.ItemsSource = null;

            _flip.Invoke(RotationType.Backward);
            CmdNew.IsEnabled = true;
            CmdPrint.IsEnabled = true;
            CmdSave.IsEnabled = false;
            CmdClose.IsEnabled = false;

        }

        /// <summary>
        /// User control loaded
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                this.DataContext = new clsGRNReturnVO();
                dtpFromDate.SelectedDate = DateTime.Now.Date;
                dtpToDate.SelectedDate = DateTime.Now.Date;
                //dtpGRNFromDate.SelectedDate = DateTime.Now.Date;
                //dtpGRNToDate.SelectedDate = DateTime.Now.Date;
                dpgrDt.SelectedDate = DateTime.Now;
                dpgrDt.IsEnabled = false;
                CmdNew.IsEnabled = true;
                CmdPrint.IsEnabled = true;
                CmdSave.IsEnabled = false;
                CmdClose.IsEnabled = false;
                FillSupplier();

                //======================================================
                //Paging
                FillGRNReturnSearchList();
                //======================================================
                IsPageLoaded = true;
            }
        }

        /// <summary>
        /// Fills supplier
        /// </summary>
        private void FillSupplier()
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

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);


                    cmbSearchSupplier.ItemsSource = null;
                    cmbSearchSupplier.ItemsSource = objList;

                    cmbSearchSupplier.SelectedItem = objList[0];

                    cmbGRNSupplier.ItemsSource = null;
                    cmbGRNSupplier.ItemsSource = objList;

                    cmbGRNSupplier.SelectedItem = objList[0];


                    if (this.DataContext != null)
                    {
                        cmbSearchSupplier.SelectedValue = ((clsGRNReturnVO)this.DataContext).SupplierID;
                        cmbGRNSupplier.SelectedValue = ((clsGRNReturnVO)this.DataContext).SupplierID;

                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void cmdGRNSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgGRNSearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dgGRNSearchList.SelectedItem != null)
            //{
            //    _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
            //    grdGRNAdd.ItemsSource = GRNReturnAddedItems;
            //    _GRNReturnAddedItems.Clear();

            //   // GRNReturnMaster = new clsGRNReturnVO();

            //    CalculateGRNSummary();

            //    dgGRNSearchItems.ItemsSource = null;

            //    FillGRNDetailslList(((clsGRNVO)dgGRNSearchList.SelectedItem).ID);
            //    GRNReturnMaster.GRNID = ((clsGRNVO)dgGRNSearchList.SelectedItem).ID;
            //    GRNReturnMaster.StoreID = ((clsGRNVO)dgGRNSearchList.SelectedItem).StoreID;
            //    GRNReturnMaster.SupplierID = ((clsGRNVO)dgGRNSearchList.SelectedItem).SupplierID;
            //    GRNUnitID = ((clsGRNVO)dgGRNSearchList.SelectedItem).UnitId; 

            //}
        }

        private void FillGRNDetailslList(long pGRNID)
        {
            //clsGetGRNDetailsListBizActionVO BizAction = new clsGetGRNDetailsListBizActionVO();
            ////BizAction.IsActive = true;
            //BizAction.GRNID = pGRNID;


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<clsGRNDetailsVO> objList = new List<clsGRNDetailsVO>();

            //        objList = ((clsGetGRNDetailsListBizActionVO)e.Result).List;
            //        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;

            //        dgGRNSearchItems.ItemsSource = null;
            //        dgGRNSearchItems.ItemsSource = objList;
            //    }
            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();
        }
        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion
        /// <summary>
        /// GRNRetunr grid edited
        /// </summary>
        private void grdGRNAdd_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (grdGRNAdd.SelectedItem != null)
            {
                clsGRNReturnDetailsVO objclsGRNReturnDetailsVO = (clsGRNReturnDetailsVO)grdGRNAdd.SelectedItem;
                if (e.Column.Header.ToString().Equals("Return Quantity Pack") && rdbDirectReturn.IsChecked == true && (((clsGRNReturnDetailsVO)grdGRNAdd.SelectedItem).AvailableQuantity + ((clsGRNReturnDetailsVO)grdGRNAdd.SelectedItem).FreeQuantity) < ((clsGRNReturnDetailsVO)grdGRNAdd.SelectedItem).ReturnedQuantity)
                {
                    ShowMessageBox("Return Quantity cannot be greater than Current Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    objclsGRNReturnDetailsVO.ReturnedQuantity = 0;
                }
                else if (e.Column.Header.ToString().Equals("Return Quantity Pack") && rdbGRNReturn.IsChecked == true && (objclsGRNReturnDetailsVO.ReturnPendingQty < objclsGRNReturnDetailsVO.ReturnedQuantity || (objclsGRNReturnDetailsVO.AvailableQuantity+ objclsGRNReturnDetailsVO.FreeQuantity) < objclsGRNReturnDetailsVO.ReturnedQuantity))
                {
                    if (objclsGRNReturnDetailsVO.ReturnPendingQty < objclsGRNReturnDetailsVO.ReturnedQuantity && !((objclsGRNReturnDetailsVO.AvailableQuantity + objclsGRNReturnDetailsVO.FreeQuantity) < objclsGRNReturnDetailsVO.ReturnedQuantity))
                    {
                        ShowMessageBox("Return Quantity cannot be greater than GRN Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        objclsGRNReturnDetailsVO.ReturnedQuantity = objclsGRNReturnDetailsVO.ReturnPendingQty;
                    }
                    if ((objclsGRNReturnDetailsVO.AvailableQuantity + objclsGRNReturnDetailsVO.FreeQuantity) < objclsGRNReturnDetailsVO.ReturnedQuantity)
                    {
                        ShowMessageBox("Return Quantity cannot be greater than Available Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        objclsGRNReturnDetailsVO.ReturnedQuantity = objclsGRNReturnDetailsVO.AvailableQuantity+ objclsGRNReturnDetailsVO.FreeQuantity;
                    }
                }
                else if (e.Column.Header.ToString().Equals("Return Quantity Pack"))
                    CalculateGRNSummary();
            }
        }

        /// <summary>
        /// Calculate GRN return summary
        /// </summary>
        private void CalculateGRNSummary()
        {
            GRNReturnMaster.NetAmount = GRNReturnMaster.TotalAmount = GRNReturnMaster.TotalVAT = 0;

            foreach (var item in GRNReturnAddedItems)
            {
                GRNReturnMaster.TotalAmount += item.Amount;
                GRNReturnMaster.TotalVAT += item.VATAmount;
                GRNReturnMaster.NetAmount += item.NetAmount;
            }

            //this.txtNetAmt.DataContext = txtTotAmt.DataContext = txtVAT.DataContext = txtRemark.DataContext = txtReason.DataContext =  GRNReturnMaster;
            this.txtNetAmt.DataContext = txtTotAmt.DataContext = txtVAT.DataContext = txtRemark.DataContext = GRNReturnMaster;
        }

        /// <summary>
        /// Fills GRN Return list
        /// </summary>
        private void cmdGRNReturnSearch_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
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

            if (dtpFromDate.SelectedDate != null && dtpFromDate.SelectedDate.Value.Date > DateTime.Now.Date)
            {
                dtpFromDate.SetValidation("From Date should not be greater than Today's Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
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
                //======================================================
                //Paging
                dgDataPager.PageIndex = 0;
                FillGRNReturnSearchList();
                //======================================================
            }
        }

        /// <summary>
        /// grn return grid selection changed
        /// </summary>
        private void dgGRNReturnList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgGRNReturnList.SelectedItem != null)
            {
                dgGRNReturnItems.ItemsSource = null;

                clsGetGRNReturnDetailsListBizActionVO BizAction = new clsGetGRNReturnDetailsListBizActionVO();
                BizAction.GRNReturnID = ((clsGRNReturnVO)dgGRNReturnList.SelectedItem).ID;
                BizAction.UnitID = ((clsGRNReturnVO)dgGRNReturnList.SelectedItem).UnitId;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<clsGRNReturnDetailsVO> objList = new List<clsGRNReturnDetailsVO>();

                        objList = ((clsGetGRNReturnDetailsListBizActionVO)args.Result).List;
                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;

                        dgGRNReturnItems.ItemsSource = null;
                        dgGRNReturnItems.ItemsSource = objList;
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
        }

        /// <summary>
        /// Deletes item
        /// </summary>
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            GRNReturnAddedItems.RemoveAt(grdGRNAdd.SelectedIndex);
            CalculateGRNSummary();
        }

        /// <summary>
        /// GRN Type radio button click
        /// </summary>
        private void rbGRNType_Checked(object sender, RoutedEventArgs e)
        {
            if (IsPageLoaded)
            {
                if (rdbDirectReturn.IsChecked == true)
                {
                    GRNReturnMaster.GoodReturnType = InventoryGoodReturnType.Direct;
                    //Added By Pallavi
                    cmbGRNSupplier.IsEnabled = true;
                    grdGRNAdd.ItemsSource = null;
                    txtGrnNo.Text = "";
                    if (_GRNReturnAddedItems != null)
                    {
                        GRNReturnMaster.GRNID = 0;
                        GRNReturnMaster.GRNUnitID = 0;
                        _GRNReturnAddedItems.Clear();
                        CalculateGRNSummary();
                    }

                }
                else if (rdbGRNReturn.IsChecked == true)
                {

                    GRNReturnMaster.GoodReturnType = InventoryGoodReturnType.AgainstGRN;
                    cmbGRNSupplier.IsEnabled = false;
                    FillSupplier();

                    if (_GRNReturnAddedItems != null)
                    {
                        _GRNReturnAddedItems.Clear();
                        CalculateGRNSummary();
                    }

                }
            }
        }

        /// <summary>
        /// Get items click
        /// </summary>
        private void lnkSearchItems_Click(object sender, RoutedEventArgs e)
        {
                if (IsPageLoaded)
                {
                    if (_GRNReturnAddedItems != null)
                    {
                        if (GRNReturnMaster.GoodReturnType == InventoryGoodReturnType.AgainstGRN)
                            _GRNReturnAddedItems.Clear();
                        CalculateGRNSummary();
                    }

                    if (GRNReturnMaster.GoodReturnType == InventoryGoodReturnType.AgainstGRN)
                    {
                        GRNSearchForm ObjGrnSearch = new GRNSearchForm();
                        ObjGrnSearch.IsSearchForGRNReturn = true;
                        ObjGrnSearch.OnSaveButton_Click += new RoutedEventHandler(GrnSearch_OnSaveButton_Click);
                        ObjGrnSearch.Show();
                    }
                    else if (GRNReturnMaster.GoodReturnType == InventoryGoodReturnType.Direct)
                    {
                        GRNReturnMaster.GRNID = 0;
                        GRNReturnMaster.SupplierID = 0;
                        ItemListNew Itemswin = new ItemListNew();
                        Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                        Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        Itemswin.ShowBatches = true;
                        Itemswin.StoreID = GRNReturnMaster.StoreID;
                        if (StoreID == 0)
                            Itemswin.AllowStoreSelection = true;
                        else
                            Itemswin.AllowStoreSelection = false;
                        Itemswin.ShowExpiredBatches = true;
                        Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                        Itemswin.Show();
                    }
                }
        }
       

        /// <summary>
        /// GRN search window ok button click
        /// </summary>
        void GrnSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            GRNSearchForm frmGRNSearch = (GRNSearchForm)sender;
            clsGRNVO objGRNVO = ((clsGRNVO)frmGRNSearch.dgGRNSearchList.SelectedItem);
            txtGrnNo.Text = String.Empty;
            if (frmGRNSearch != null && frmGRNSearch.dgGRNSearchList.SelectedItem != null)
            {
                GRNGRNID = objGRNVO.ID;
                GRNStoreID = objGRNVO.StoreID;
                GRNSupplierID = objGRNVO.SupplierID;
                GRNGRNUnitID = objGRNVO.UnitId;
                GrnNo = objGRNVO.GRNNO;
                GRNReturnMaster.GRNID = GRNGRNID;
                GRNReturnMaster.StoreID = GRNStoreID;
                GRNReturnMaster.SupplierID = GRNSupplierID;

                if (objGRNVO.PaymentModeID == MaterPayModeList.Cash)
                    rdbCash.IsChecked = true;
                else if (objGRNVO.PaymentModeID == MaterPayModeList.Credit)
                    rdbCheque.IsChecked = true;
                txtGrnNo.Text = GrnNo;

                if (frmGRNSearch.DialogResult == true && frmGRNSearch.SelectedGrnItems != null)
                {
                    foreach (var item in frmGRNSearch.SelectedGrnItems)
                    {
                        clsGRNReturnDetailsVO ObjAddItem = new clsGRNReturnDetailsVO();
                        GRNReturnAddedItems.Add(new clsGRNReturnDetailsVO()
                        {
                            ItemID = item.ItemID,
                            ItemName = item.ItemName,
                            BatchID = item.BatchID,
                            BatchCode = item.BatchCode,
                            ExpiryDate = item.ExpiryDate,
                            ReceivedQuantity = item.GRNReturnTotalQuantity / item.ConversionFactor,
                            AvailableQuantity = item.AvailableQuantity-item.FreeQuantity,
                            FreeQuantity = item.FreeQuantity,
                            Rate = item.Rate,
                            CDiscountPercent = item.CDiscountPercent,
                            SchDiscountPercent = item.SchDiscountPercent,
                            //TotalDiscAmt = item.CDiscountAmount + item.SchDiscountAmount, //commented by Ashish Z. on 120516
                            VATPercent = item.VATPercent,
                            VATAmount = item.VATAmount,
                            ItemTax = item.ItemTax,
                            ConversionFactor = item.ConversionFactor,
                            PendingQuantity = item.PendingQuantity,
                            ReturnPendingQty = item.PendingQuantity
                        });
                    }
                    CalculateGRNSummary();
                    CmdSave.IsEnabled = true;
                    grdGRNAdd.Focus();
                    grdGRNAdd.ItemsSource = GRNReturnAddedItems;
                    grdGRNAdd.UpdateLayout();
                    grdGRNAdd.SelectedIndex = GRNReturnAddedItems.Count - 1;
                }
            }
        }

        /// <summary>
        /// Fills supplier
        /// </summary>
        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew Itemswin = (ItemListNew)sender;

            if (Itemswin.ItemBatchList != null)
            {
                if (_GRNReturnAddedItems == null)
                    _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();

                GRNReturnMaster.StoreID = Itemswin.GRNReturnStoreId;
                if ((MasterListItem)cmbGRNSupplier.SelectedItem != null)
                    GRNReturnMaster.SupplierID = ((MasterListItem)cmbGRNSupplier.SelectedItem).ID;

                foreach (var item in Itemswin.ItemBatchList)
                {
                    if (_GRNReturnAddedItems.Where(grnItems => grnItems.ItemID == item.ItemID).Any() == true)
                    {
                        if (_GRNReturnAddedItems.Where(grnItems => grnItems.BatchID == item.BatchID).Any() == false)
                            _GRNReturnAddedItems.Add(new clsGRNReturnDetailsVO() { ItemID = item.ItemID, BatchID = item.BatchID, ItemName = item.ItemName, AvailableQuantity = item.AvailableStock, ExpiryDate = item.ExpiryDate, ReceivedQuantity = 0, BatchCode = item.BatchCode, Rate = item.PurchaseRate, VATPercent = item.VATPerc });
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", "Please select another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                    }
                    else
                        _GRNReturnAddedItems.Add(new clsGRNReturnDetailsVO() { ItemID = item.ItemID, BatchID = item.BatchID, ItemName = item.ItemName, AvailableQuantity = item.AvailableStock, ExpiryDate = item.ExpiryDate, ReceivedQuantity = 0, BatchCode = item.BatchCode, Rate = item.PurchaseRate, VATPercent = item.VATPerc });
                }

                CalculateGRNSummary();
                grdGRNAdd.Focus();
                grdGRNAdd.ItemsSource = GRNReturnAddedItems;
                grdGRNAdd.UpdateLayout();

                grdGRNAdd.SelectedIndex = GRNReturnAddedItems.Count - 1;
                CmdSave.IsEnabled = true;
                StoreID = Itemswin.StoreID;
            }
        }

        private void SerachGRNItems()
        {
            //_GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
            //grdGRNAdd.ItemsSource = GRNReturnAddedItems;
            //_GRNReturnAddedItems.Clear();


            //CalculateGRNSummary();

            //dgGRNSearchList.ItemsSource = null;
            //dgGRNSearchItems.ItemsSource = null;


            //clsGetGRNListBizActionVO BizAction = new clsGetGRNListBizActionVO();
            ////BizAction.IsActive = true;
            //if (dtpGRNFromDate.SelectedDate == null)
            //    BizAction.FromDate = null;
            //else
            //    BizAction.FromDate = dtpGRNFromDate.SelectedDate.Value.Date;

            //if (dtpGRNToDate.SelectedDate == null)
            //    BizAction.ToDate = null;
            //else
            //    BizAction.ToDate = dtpGRNToDate.SelectedDate.Value.Date;

            //if (cmbGRNSupplier.SelectedItem != null && ((MasterListItem)cmbGRNSupplier.SelectedItem).ID != 0)
            //    BizAction.SupplierID = ((MasterListItem)cmbGRNSupplier.SelectedItem).ID;
            //BizAction.IsPagingEnabled = true;
            //BizAction.MaximumRows = 10;

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, args) =>
            //{
            //    if (args.Error == null && args.Result != null)
            //    {
            //        List<clsGRNVO> objList = new List<clsGRNVO>();

            //        objList = ((clsGetGRNListBizActionVO)args.Result).List;
            //        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;


            //        dgGRNSearchList.ItemsSource = null;
            //        dgGRNSearchList.ItemsSource = objList;

            //    }
            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();
        }

        /// <summary>
        /// Fills supplier
        /// </summary>
        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgGRNReturnList.SelectedItem != null)
                {
                    string Parameters = "";
                    long GunitID = 0;
                    long ID = 0;
                    ID = ((clsGRNReturnVO)dgGRNReturnList.SelectedItem).ID;
                    GunitID = ((clsGRNReturnVO)dgGRNReturnList.SelectedItem).UnitId;

                    // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

                    string URL = "../Reports/InventoryPharmacy/GRNReturnPrint.aspx?ID=" + ID + "&UnitID=" + GunitID;

                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
