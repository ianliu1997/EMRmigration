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
using System.Windows.Data;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using OPDModule.Forms;
using PalashDynamics.Pharmacy.Inventory;

namespace PalashDynamics.Pharmacy
{
    public partial class ExpiredItemReturn1Approval : UserControl
    {
        private SwivelAnimation objAnimation;
        PagedCollectionView pcvItemList;
        #region 'Paging'

        public PagedSortableCollectionView<clsExpiredItemReturnVO> ExpiredDataList { get; private set; }
        public PagedSortableCollectionView<clsExpiredItemReturnVO> ExpiredItemList { get; private set; }

        public ObservableCollection<clsExpiredItemReturnDetailVO> ExpiryAddedItems { get; set; }

        bool IsPageLoaded = false;
        private long _ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        public long ClinicID
        {
            get
            {
                return _ClinicID;
            }
            set
            {
                _ClinicID = value;
            }
        }

        public int DataListPageSize
        {
            get
            {
                return ExpiredDataList.PageSize;
            }
            set
            {
                if (value == ExpiredDataList.PageSize) return;
                ExpiredDataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        #endregion
        WaitIndicator Indicatior;
        /// <summary>
        /// constructor
        /// </summary>
        public ExpiredItemReturn1Approval()
        {
            Indicatior = new WaitIndicator();
            this.DataContext = new clsExpiredItemReturnVO();
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            ExpiryAddedItems = new ObservableCollection<clsExpiredItemReturnDetailVO>();

            ExpiredDataList = new PagedSortableCollectionView<clsExpiredItemReturnVO>();
            ExpiredDataList.OnRefresh += new EventHandler<RefreshEventArgs>(ExpiredDataList_OnRefresh);
            DataListPageSize = 15;
            dpdgExpiredList.PageSize = DataListPageSize;
            dpdgExpiredList.Source = ExpiredDataList;
            SetCommandButtonState("New");
        }

        /// <summary>
        /// Executes when fron panel grid refreshed
        /// </summary>
        /// <param name="sender"> front panel grid</param>
        /// <param name="e">front panel grid refreshed</param>
        void ExpiredDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetExpiredList();

        }
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    //cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdClose.IsEnabled = true;
                    //cmdSave.IsEnabled = false;
                    //cmbStore.SelectedValue = 0;

                    //cmdCancel.IsEnabled = false;
                    break;

                case "Save":
                    //cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    //cmdSave.IsEnabled = true;

                    //cmdCancel.IsEnabled = false;
                    break;

                case "Modify":
                    //cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    //cmdSave.IsEnabled = true;

                    // cmdCancel.IsEnabled = true;
                    break;

                case "ClickNew":
                    //cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdClose1.IsEnabled = true;
                    // cmdSave.IsEnabled = true;
                    cmbClinic.SelectedValue = 0;
                    cmbSupplier.SelectedValue = 0;

                    //cmdCancel.IsEnabled = true;

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// User control leaded
        /// </summary>
        /// <param name="sender">User Control</param>
        /// <param name="e">Loaded event</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {


            if (!IsPageLoaded)
            {
                this.DataContext = new clsExpiredItemReturnVO();
                dpreturnDt.SelectedDate = DateTime.Now;

                ExpiryItemReturnMaster = new clsExpiredItemReturnVO();

                this.txtNetAmt.DataContext = txtRemark.DataContext = txtTAmt.DataContext = txtTTaxAmt.DataContext = txtTVAT.DataContext = ExpiryItemReturnMaster;

                ExpiryAddedItems = new ObservableCollection<clsExpiredItemReturnDetailVO>();
                dgexpItem.ItemsSource = ExpiryAddedItems;

                dpSearchFromDate.SelectedDate = DateTime.Now;
                dpSearchToDate.SelectedDate = DateTime.Now;

                //FillClinic();
                FillStore();
                FillSupplier();
                GetExpiredList();

                IsPageLoaded = true;
            }
            dpreturnDt.SelectedDate = DateTime.Now.Date;
            dpreturnDt.IsEnabled = false;
        }




        /// <summary>
        /// New button click, flips the panel to backward
        /// </summary>
        /// <param name="sender">new button</param>
        /// <param name="e">new button click</param>
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new clsExpiredItemReturnVO();
            cmbSupplier.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).SupplierID;

            cmbClinic.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).StoreID;
            SetCommandButtonState("ClickNew");
            ResetControls();
            objAnimation.Invoke(RotationType.Forward);
        }
        /// <summary>
        /// resets the controls on the form to the default value
        /// </summary>
        private void ResetControls()
        {
            //dpConsumptionDate.SelectedDate = DateTime.Now;
            cmbSearchStore.SelectedValue = (long)0;
            cmbSupplier.SelectedValue = 0;
            cmbClinic.SelectedValue = 0;
            // cmbStore.SelectedValue = (long)0;

            //dgItemList.ItemsSource = null;

            //txtNoOfItems.Text = String.Empty;
            //txtTotalAmount.Text = String.Empty;
            //txtTotalVAT.Text = String.Empty;
            // txtRemark.Text = String.Empty;

        }
        /// <summary>
        /// Fills store combo box
        /// </summary>
        private void FillStore()
        {

            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            //obj.RetrieveDataFlag = false;
            ////By Anjali.......................
            //BizActionObj.StoreType = 1;
            ////..................................

            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            obj.RetrieveDataFlag = false;

            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true, IsQuarantineStore = true, Parent = 0 };
                    BizActionObj.ToStoreList.Insert(0, Default);

                    var result = from item in BizActionObj.ToStoreList
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.IsQuarantineStore == true
                                 select item;


                    cmbSearchStore.ItemsSource = result.ToList();
                    cmbSearchStore.SelectedItem = result.ToList()[0];

                    cmbClinic.ItemsSource = result.ToList();
                    cmbClinic.SelectedItem = result.ToList()[0];

                    if (this.DataContext != null)
                    {
                        cmbSearchStore.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).StoreID;
                        cmbClinic.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).StoreID;


                    }


                }

            };

            client.CloseAsync();

        }

        /// <summary>
        /// fills supplier combobox
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

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);


                    cmbSupplier.ItemsSource = null;
                    cmbSupplier.ItemsSource = objList;

                    cmbSupplier.SelectedItem = objList[0];
                    cmbSearchSupplier.ItemsSource = null;
                    cmbSearchSupplier.ItemsSource = objList;

                    cmbSearchSupplier.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbSupplier.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).SupplierID;
                        cmbSearchSupplier.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).SupplierID;


                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        /// <summary>
        /// Fetches expired item list & fills front panel grid
        /// </summary>
        private void GetExpiredList()
        {
            Indicatior.Show();
            try
            {
                clsGetExpiredListBizActionVO BizAction = new clsGetExpiredListBizActionVO();
                BizAction.ExpiredList = new List<clsExpiredItemReturnVO>();

                BizAction.FromDate = dpSearchFromDate.SelectedDate;
                BizAction.ToDate = dpSearchToDate.SelectedDate;

                if (BizAction.ToDate != null)
                    BizAction.ToDate = dpSearchToDate.SelectedDate.Value.AddDays(1);

                if (cmbSearchStore.SelectedItem != null)
                {
                    BizAction.StoreId = ((clsStoreVO)cmbSearchStore.SelectedItem).StoreId;

                }
                if (cmbSearchSupplier.SelectedItem != null)
                {
                    BizAction.SupplierID = ((MasterListItem)cmbSearchSupplier.SelectedItem).ID;
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = 0;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {


                        BizAction.ExpiredList = ((clsGetExpiredListBizActionVO)e.Result).ExpiredList;
                        ExpiredDataList.TotalItemCount = BizAction.TotalRows;

                        ExpiredDataList.Clear();
                        foreach (var item in BizAction.ExpiredList)
                        {
                            ExpiredDataList.Add(item);
                        }

                        dgExpiredList.ItemsSource = null;
                        dgExpiredList.ItemsSource = ExpiredDataList;

                        dpdgExpiredList.Source = null;
                        dpdgExpiredList.PageSize = BizAction.MaximumRows;
                        dpdgExpiredList.Source = ExpiredDataList;
                        txtTotalCountRecords.Text = ExpiredDataList.TotalItemCount.ToString();

                        Indicatior.Close();
                    }


                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception Ex)
            {
                Indicatior.Close();
                throw;
            }

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {

        }



        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetExpiredList();
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the Item details of expired entry on front panel grid
        /// </summary>
        /// <param name="sender">front panel first grid</param>
        /// <param name="e">selection changes</param>
        private void dgExpiredList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgExpiredItemList.ItemsSource = null;
            try
            {
                clsGetExpiredListForExpiredReturnBizActionVO bizActionVo = new clsGetExpiredListForExpiredReturnBizActionVO();

                if (dgExpiredList.SelectedItem != null)
                {
                    bizActionVo.ConsumptionID = ((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).ID;
                    bizActionVo.UnitID = ((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).UnitId;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, e1) =>
                    {
                        if (e1.Error == null && e1.Result != null)
                        {


                            bizActionVo.ItemList = ((clsGetExpiredListForExpiredReturnBizActionVO)e1.Result).ItemList;
                            dgExpiredItemList.ItemsSource = null;
                            dgExpiredItemList.ItemsSource = bizActionVo.ItemList;
                        }


                    };
                    Client.ProcessAsync(bizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }



            }
            catch (Exception Ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Save button click
        /// </summary>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            PalashDynamics.UserControls.WaitIndicator wt = new UserControls.WaitIndicator();
            bool flagIsValid = true;
            ClickedFlag1 = ClickedFlag1 + 1;
            if (ClickedFlag1 == 1)
            {


                List<clsExpiredItemReturnDetailVO> objList = ExpiryAddedItems.ToList<clsExpiredItemReturnDetailVO>();
                clsAddExpiryItemReturnBizActionVO BizAction = new clsAddExpiryItemReturnBizActionVO();
                BizAction.objExpiryItem = new clsExpiredItemReturnVO();
                BizAction.objExpiryItem.Time = DateTime.Now;
                BizAction.objExpiryItem = this.ExpiryItemReturnMaster;

                //User Related Values specified in DAL           
                BizAction.objExpiryItem.Status = true;

                if (dpreturnDt.SelectedDate.Value != null)
                {
                    BizAction.objExpiryItem.Date = dpreturnDt.SelectedDate.Value;
                }
                else
                {
                    BizAction.objExpiryItem.Date = DateTime.Now;
                }

                if (BizAction.objExpiryItem.Date.Value.Date < DateTime.Now.Date)
                {
                    dpreturnDt.SetValidation("Return Date should not be less than Today's Date");
                    dpreturnDt.RaiseValidationError();
                    flagIsValid = false; ;
                    ClickedFlag1 = 0;
                    return;
                }
                else
                {
                    dpreturnDt.ClearValidationError();
                }

                if (BizAction.objExpiryItem.Date.Value.Date > DateTime.Now.Date)
                {
                    dpreturnDt.SetValidation("Return Date should not be greater than Today's Date");
                    dpreturnDt.RaiseValidationError();
                    flagIsValid = false; ;
                    ClickedFlag1 = 0;
                    return;
                }
                else
                {
                    dpreturnDt.ClearValidationError();
                }
                if (cmbClinic.SelectedItem == null)
                {
                    cmbClinic.TextBox.SetValidation("Store can not be blank.");
                    cmbClinic.TextBox.RaiseValidationError();
                    cmbClinic.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Store can not be blank", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    flagIsValid = false; ;
                    ClickedFlag1 = 0;
                    return;
                }

                else if (((clsStoreVO)cmbClinic.SelectedItem).StoreId == 0)
                {
                    cmbClinic.TextBox.SetValidation("Store can not be blank.");
                    cmbClinic.TextBox.RaiseValidationError();
                    cmbClinic.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Store can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    flagIsValid = false;
                    ClickedFlag1 = 0;
                    return;

                }
                else
                {
                    cmbClinic.TextBox.ClearValidationError();
                }

                if (cmbSupplier.SelectedItem == null)
                {
                    cmbSupplier.TextBox.SetValidation("Supplier can not be blank.");
                    cmbSupplier.TextBox.RaiseValidationError();
                    cmbSupplier.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Supplier can not be blank", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    flagIsValid = false; ;
                    ClickedFlag1 = 0;
                    return;
                }

                else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
                {
                    cmbSupplier.TextBox.SetValidation("Supplier can not be blank.");
                    cmbSupplier.TextBox.RaiseValidationError();
                    cmbSupplier.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Supplier can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    flagIsValid = false;
                    ClickedFlag1 = 0;
                    return;
                }
                else
                {
                    cmbSupplier.TextBox.ClearValidationError();
                }


                if (objList != null && objList.Count > 0)
                {
                    foreach (var item in objList)
                    {
                        if (item.ReturnQty == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Zero Quantity.", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            flagIsValid = false;
                            ClickedFlag1 = 0;
                            return;


                        }
                        //Added By Somnath On 05/12/2011 To check the Return Rate Greater Than 0
                        else if (item.ReturnRate == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Zero ReturnRate.", "ReturnRate In The List Can't Be Zero. Please Enter ReturnRate Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            flagIsValid = false;
                            ClickedFlag1 = 0;
                            return;
                        }
                        else if (item.ReturnRate < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Negative ReturnRate.", "ReturnRate Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            flagIsValid = false;
                            ClickedFlag1 = 0;
                            return;
                        }
                        //else if (item.ReturnRate > item.PurchaseRate)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //     new MessageBoxControl.MessageBoxChildWindow("PALASH", "ReturnRate Can't Be greater than Purchase Rate.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();
                        //    item.ReturnRate = item.PurchaseRate;
                        //    flagIsValid = false;
                        //    ClickedFlag1 = 0;
                        //    return;
                        //}
                        else if (item.VATPercentage < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Vat %", "VAT Percentage Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            flagIsValid = false;
                            ClickedFlag1 = 0;
                            return;
                        }
                        else if (item.VATPercentage > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Vat %", "VAT Percentage Can't Be greater than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            flagIsValid = false;
                            ClickedFlag1 = 0;
                            return;
                        }
                    }

                }
                if (objList.Count == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                           new MessageBoxControl.MessageBoxChildWindow("", "You can not save without Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    flagIsValid = false;
                    ClickedFlag1 = 0;
                    return;
                }


                if (flagIsValid)
                {
                    BizAction.objExpiryItemDetailList = (List<clsExpiredItemReturnDetailVO>)ExpiryAddedItems.ToList<clsExpiredItemReturnDetailVO>();
                    wt.Show();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, args) =>
                    {
                        ClickedFlag1 = 0;
                        FillStore();
                        FillSupplier();
                        //cmbClinic.SelectedValue = 0;
                        //cmbSupplier.SelectedValue = 0;
                        if (args.Error == null && args.Result != null && ((clsAddExpiryItemReturnBizActionVO)args.Result).objExpiryItem != null)
                        {
                            ExpiryAddedItems.Clear();
                            ExpiryItemReturnMaster.NetAmount = ExpiryItemReturnMaster.OtherDeducution = ExpiryItemReturnMaster.TotalAmount = ExpiryItemReturnMaster.TotalOctriAmount = ExpiryItemReturnMaster.TotalTaxAmount = ExpiryItemReturnMaster.TotalVATAmount = 0;
                            ExpiryItemReturnMaster = null;
                            ExpiryItemReturnMaster = new clsExpiredItemReturnVO();
                            txtRemark.Text = string.Empty;
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry Items Return Saved Successfully with return no. " + ((clsAddExpiryItemReturnBizActionVO)args.Result).objExpiryItem.ExpiryReturnNo, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            mgbx.Show();
                            GetExpiredList();
                            cmdSave.IsEnabled = false;
                            cmdClose1.IsEnabled = false;
                            cmdNew.IsEnabled = true;
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Expiry Items Return Not Saved.", "Expiry Items Return Not Saved. \n Some Error Occured while saving information.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbx.Show();
                        }
                        wt.Close();
                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                    objAnimation.Invoke(RotationType.Backward);
                }
                else

                    ClickedFlag1 = 0;
            }
        }

        /// <summary>
        /// Close button click
        /// </summary>
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent(new InventoryDashBoard());
        }

        /// <summary>
        /// expired item grid edited
        /// </summary>
        private void dgexpItem_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (dgexpItem.SelectedItem != null)
            {
                if (e.Column.DisplayIndex == 8)
                {
                    if (((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).UOMConversionList == null || ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).UOMConversionList.Count == 0)
                    {

                        if (((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase < ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseQuantity)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be greater than Current Stock.\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();

                            ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase / ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseConversionFactor)));
                        }
                        else if (((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty < 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be less than 1 .\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();

                            ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase / ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseConversionFactor)));

                        }
                    }

                    else if (((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).SelectedUOM != null && ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).SelectedUOM.ID > 0)
                    {

                        CalculateConversionFactorCentral(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).SelectedUOM.ID, ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).SUOMID);

                    }
                    else
                    {
                        ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = 0;
                        ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).SingleQuantity = 0;

                        ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ConversionFactor = 0;
                        ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseConversionFactor = 0;

                        ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).MRP = ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).MainMRP;
                        ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).PurchaseRate = ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).MainRate;
                    }
                }






                if (e.Column.DisplayIndex == 11 && ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnRate < 0)
                {
                    //TextBox txtReturn = dgexpItem.Columns[7].GetCellContent(e.Row) as TextBox;

                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Negative Return Rate", "Return Rate cannot be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnRate = 0;
                }
                if (e.Column.DisplayIndex == 13 && ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).VATPercentage < 0)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Negative VAT Percentage", "VAT Percentage cannot be Negative..", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).VATPercentage = 0;
                }
                else if (e.Column.DisplayIndex == 13 && ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).VATPercentage > 100)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("VAT Percentage", "VAT Percentage cannot be greater than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).VATPercentage = 100;
                }


            }
            CalculateGRNSummary();
        }

        int ClickedFlag1 = 0;
        /// <summary>
        /// Clinic selection change 
        /// </summary>
        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null)
                {
                    if (ExpiryAddedItems != null)
                        ExpiryAddedItems.Clear();
                }

            }
            catch (Exception Ex)
            {
                throw;
            }
        }
        public clsExpiredItemReturnVO ExpiryItemReturnMaster { get; set; }

        /// <summary>
        /// Get Item button click
        /// </summary>
        private void txtGetItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null && ((clsStoreVO)cmbClinic.SelectedItem).StoreId != 0 && cmbSupplier.SelectedItem != null && ((MasterListItem)cmbSupplier.SelectedItem).ID != 0)
                {
                    ExpiryItemReturnMaster = new clsExpiredItemReturnVO();
                    ExpiredItemSearch win = new ExpiredItemSearch();
                    win.StoreID = ((clsStoreVO)cmbClinic.SelectedItem).StoreId;
                    win.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;

                    win.dtpFromDate.SelectedDate = dpreturnDt.SelectedDate;

                    win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                    win.Show();
                }
                else
                {
                    //Added By Somnath on 05/12/2011

                    if (((clsStoreVO)cmbClinic.SelectedItem).StoreId == 0 && ((MasterListItem)cmbSupplier.SelectedItem).ID != 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                    }
                    else if (cmbSupplier.SelectedItem != null && ((MasterListItem)cmbSupplier.SelectedItem).ID == 0 && ((clsStoreVO)cmbClinic.SelectedItem).StoreId != 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Store and Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw;
            }


        }

        /// <summary>
        /// Fills supplier
        /// </summary>
        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ExpiredItemSearch Itemswin = (ExpiredItemSearch)sender;
            if (Itemswin.SelectedItems != null)
            {
                foreach (var item in Itemswin.SelectedItems)
                {
                    IEnumerator<clsExpiredItemReturnDetailVO> list = (IEnumerator<clsExpiredItemReturnDetailVO>)ExpiryAddedItems.GetEnumerator();
                    bool IsExist = false;

                    while (list.MoveNext())
                    {
                        if (item.ItemID == ((clsExpiredItemReturnDetailVO)list.Current).ItemID)
                        {
                            if (item.BatchID == ((clsExpiredItemReturnDetailVO)list.Current).BatchID)
                            {
                                IsExist = true;
                                break;
                            }
                        }
                    }
                    if (IsExist == false)
                        ExpiryAddedItems.Add(new clsExpiredItemReturnDetailVO()
                        {
                            ItemID = item.ItemID,
                            ItemName = item.ItemName,
                            ItemCode = item.ItemCode,
                            AvailableStock = item.AvailableStock,
                            BatchCode = item.BatchCode,
                            BatchID = item.BatchID,
                            BatchExpiryDate = item.BatchExpiryDate,
                            UnitID = item.UnitID,
                            ReturnQty = item.ReturnQty,
                            VATPercentage = item.VATPercentage,
                            Conversion = item.Conversion,
                            TaxPercentage = item.TaxPercentage,


                            //By anjali...............................
                            // ConversionFactor = (Single)item.Conversion,
                            StockCF = item.StockCF,
                            ConversionFactor = 1,

                            // PurchaseRate = item.PurchaseRate,
                            PurchaseRate = item.PurchaseRate * item.StockCF,

                            BaseRate = Convert.ToSingle(item.PurchaseRate),
                            BaseMRP = Convert.ToSingle(item.MRP),
                            MainMRP = Convert.ToSingle(item.MRP),
                            MainRate = Convert.ToSingle(item.PurchaseRate),
                            SUOMID = item.StockUOMID,
                            StockUOM = item.StockUOM,
                            AvailableStockUOM = item.StockUOM,
                            SelectedUOM = new MasterListItem(item.StockUOMID, item.StockUOM),
                            BaseUOM = item.BaseUOM,
                            BaseUOMID = item.BaseUOMID,
                            BaseQuantity = (Single)(item.ReturnQty * item.StockCF),
                            BaseConversionFactor = item.BaseCF,
                            AvailableStockInBase = item.AvailableStockInBase

                            //............................................
                        }
                            );
                }

                dgexpItem.Focus();
                dgexpItem.UpdateLayout();

                dgexpItem.SelectedIndex = ExpiryAddedItems.Count - 1;

                ExpiryItemReturnMaster.StoreID = Itemswin.StoreID;
                ExpiryItemReturnMaster.SupplierID = Itemswin.SupplierID;
                if (ExpiryAddedItems.Count != 0)
                {
                    cmdSave.IsEnabled = true;
                }
                else
                {
                    cmdSave.IsEnabled = false;
                }
                CalculateGRNSummary();
            }
        }

        /// <summary>
        /// Calculate summary
        /// </summary>
        private void CalculateGRNSummary()
        {
            ExpiryItemReturnMaster.NetAmount = ExpiryItemReturnMaster.OtherDeducution = ExpiryItemReturnMaster.TotalAmount = ExpiryItemReturnMaster.TotalOctriAmount = ExpiryItemReturnMaster.TotalTaxAmount = ExpiryItemReturnMaster.TotalVATAmount = 0;

            foreach (var item in ExpiryAddedItems)
            {
                ExpiryItemReturnMaster.TotalAmount += item.Amount;
                ExpiryItemReturnMaster.TotalVATAmount += item.VATAmount;
                ExpiryItemReturnMaster.TotalTaxAmount += item.TaxAmount;
                ExpiryItemReturnMaster.NetAmount += item.NetAmount;

            }

            this.txtNetAmt.DataContext = txtRemark.DataContext = txtTAmt.DataContext = txtTTaxAmt.DataContext = txtTVAT.DataContext = ExpiryItemReturnMaster;
        }

        /// <summary>
        /// Deletes expired item
        /// </summary>
        private void cmdDeleteexpItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgexpItem.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ExpiryAddedItems.RemoveAt(dgexpItem.SelectedIndex);
                        CalculateGRNSummary();

                    }
                };

                msgWD.Show();
            }
        }

        private void cmdClose1_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            objAnimation.Invoke(RotationType.Backward);
        }

        private void ReturnRate_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtReturn = sender as TextBox;

            if (txtReturn.Text.Contains('.'))
            {
                txtReturn.Text = txtReturn.Text.Substring(0, txtReturn.Text.Length - txtReturn.Text.IndexOf('.'));
            }
            else if (txtReturn != null && txtReturn.Text.IsValueDouble() && !Convert.ToInt64(txtReturn.Text).ToString().IsPositiveNumber())
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("", "Return Rate cannot be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnRate = 0;
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter valid value.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnRate = 0;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgexpItem.SelectedItem != null)
            {
                Conversion win = new Conversion();

                win.FillUOMConversions(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ItemID, ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click1);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }
        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        void win_OnSaveButton_Click1(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;

            ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).SUOMID);



        }
        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgexpItem.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).UOMConversionList;
                    objConversionVO.UOMConvertLIst = UOMConvertLIst;


                    objConversionVO.MainMRP = ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).MainMRP;
                    objConversionVO.MainRate = ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).MainRate;


                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty);
                    long BaseUOMID = ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseUOMID;
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).PurchaseRate = objConversionVO.Rate;
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnRate = objConversionVO.MRP;

                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseMRP = objConversionVO.BaseMRP;


                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = objConversionVO.SingleQuantity;
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;



                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;


                    if (((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseQuantity > ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase)
                    {
                        float availQty = Convert.ToSingle(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase);

                        ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase / objConversionVO.BaseConversionFactor)));
                        string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity ";
                        ConversionsForAvailableStock();
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                    }
                    else if (((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty < 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be less than 1 .\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase / ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseConversionFactor)));

                    }
                    CalculateGRNSummary();


                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }
        private void ConversionsForAvailableStock()
        {
            ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseQuantity = Convert.ToSingle(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty) * ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseConversionFactor;
            //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseRate = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainRate * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;
            //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseMRP = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainMRP * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;
            CalculateGRNSummary();
        }

        private void CmdChangeAndApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgExpiredList.SelectedItem != null)
                {
                    if (((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).Approve)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Expired Item Return is already approved .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else if (((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).Reject)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Expired Item Return is already Rejected .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else
                    {
                        //if (((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).Freezed)
                        //{
                            MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Change and Approve Expired Item Return ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    //clsAddGRNBizActionVO BizAction = new clsAddGRNBizActionVO();

                                    ExpiredItemReturn1 ObjEIR = new ExpiredItemReturn1();

                                    ObjEIR.IsApproved = true;
                                    ObjEIR.ISFromAproveEIR = true;
                                    ObjEIR.SelectedEIR = (clsExpiredItemReturnVO)dgExpiredList.SelectedItem;

                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                    mElement.Text = "Approve Expired Item Return";
                                    ((IApplicationConfiguration)App.Current).OpenMainContent(ObjEIR);
                                }
                            };
                            mgBox.Show();
                        //}
                        //else
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Expired Item Return is not Freezed \n Please Freeze Expired Item Return", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    mgbox.Show();
                        //}
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No Expired Item Return is Selected \n Please select a Expired Item Return", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
