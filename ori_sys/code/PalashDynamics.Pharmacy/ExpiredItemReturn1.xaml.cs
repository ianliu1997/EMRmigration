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
using System.Windows.Browser;

namespace PalashDynamics.Pharmacy
{
    public partial class ExpiredItemReturn1 : UserControl, IInitiateCIMS
    {
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":
                    cmdApprove.Visibility = Visibility.Collapsed;
                    cmdNew.Visibility = Visibility.Visible;
                    break;
                case "Approve":
                    cmdApprove.Visibility = Visibility.Visible;
                    cmdNew.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        List<clsExpiredItemReturnDetailVO> objDetailList;

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

        #region Approve Flow Variables

        public bool IsApproved = false;
        public bool ISFromAproveEIR = false;
        public clsExpiredItemReturnVO SelectedEIR = null;
        public Boolean ISEditAndAprove = false;
        public Boolean ISEditMode = false;

        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public ExpiredItemReturn1()
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
            objDetailList = new List<clsExpiredItemReturnDetailVO>();

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
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Save";
                    //cmdClose.IsEnabled = true;

                    //cmbStore.SelectedValue = 0;

                    cmdCancel.IsEnabled = false;
                    break;

                case "Save":
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = false;
                    break;

                case "Modify":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = true;
                    break;

                case "ClickNew":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdSave.Content = "Save";
                    //cmdClose1.IsEnabled = true;
                    // cmdSave.IsEnabled = true;
                    cmbStore.SelectedValue = 0;
                    cmbSupplier.SelectedValue = 0;

                    cmdCancel.IsEnabled = true;
                    cmdGetItems.IsEnabled = true;

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

                ////FillClinic();
                FillStore();
                //FillSupplier();  // Comment to handle Approval Flow & call from FillStore()
                GetExpiredList();

                IsPageLoaded = true;
            }

            ((clsExpiredItemReturnVO)this.DataContext).Date = DateTime.Now;

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
            this.ExpiryItemReturnMaster = new clsExpiredItemReturnVO();
            this.txtNetAmt.DataContext = txtRemark.DataContext = txtTAmt.DataContext = txtTTaxAmt.DataContext = txtTVAT.DataContext = ExpiryItemReturnMaster;
            cmbSupplier.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).SupplierID;

            ISEditMode = false;

            cmbStore.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).StoreID;
            SetCommandButtonState("ClickNew");
            ResetControls();
            objAnimation.Invoke(RotationType.Forward);
        }
        /// <summary>
        /// resets the controls on the form to the default value
        /// </summary>
        private void ResetControls()
        {
            ExpiryAddedItems.Clear();
            ExpiryAddedItems = new ObservableCollection<clsExpiredItemReturnDetailVO>();
            dgexpItem.ItemsSource = null;
            dgexpItem.ItemsSource = ExpiryAddedItems;
            dgexpItem.UpdateLayout();

            this.DataContext = new clsExpiredItemReturnVO();
            this.ExpiryItemReturnMaster = new clsExpiredItemReturnVO();
            this.txtNetAmt.DataContext = txtRemark.DataContext = txtTAmt.DataContext = txtTTaxAmt.DataContext = txtTVAT.DataContext = ExpiryItemReturnMaster;

            //dpConsumptionDate.SelectedDate = DateTime.Now;
            cmbSearchStore.SelectedValue = (long)0;
            cmbSupplier.SelectedValue = 0;
            cmbStore.SelectedValue = 0;
            // cmbStore.SelectedValue = (long)0;

            //dgItemList.ItemsSource = null;

            ((clsExpiredItemReturnVO)this.DataContext).Remark = ExpiryItemReturnMaster.Remark;
            txtRemark.Text = Convert.ToString(ExpiryItemReturnMaster.Remark);

            ((clsExpiredItemReturnVO)this.DataContext).TotalAmount = ExpiryItemReturnMaster.TotalAmount;
            txtTAmt.Text = ExpiryItemReturnMaster.TotalAmount.ToString("0.00");

            ((clsExpiredItemReturnVO)this.DataContext).TotalVATAmount = ExpiryItemReturnMaster.TotalVATAmount;
            txtTVAT.Text = ExpiryItemReturnMaster.TotalVATAmount.ToString("0.00");

            ((clsExpiredItemReturnVO)this.DataContext).TotalTaxAmount = ExpiryItemReturnMaster.TotalTaxAmount;
            txtTTaxAmt.Text = ExpiryItemReturnMaster.TotalTaxAmount.ToString("0.00");

            ((clsExpiredItemReturnVO)this.DataContext).NetAmount = ExpiryItemReturnMaster.NetAmount;
            txtNetAmt.Text = ExpiryItemReturnMaster.NetAmount.ToString("0.00");

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

                    cmbStore.ItemsSource = result.ToList();
                    cmbStore.SelectedItem = result.ToList()[0];

                    if (this.DataContext != null)
                    {
                        cmbSearchStore.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).StoreID;
                        cmbStore.SelectedValue = ((clsExpiredItemReturnVO)this.DataContext).StoreID;


                    }

                    FillSupplier();  // Call this function from FillStore() to handle Approval Flow
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


                    if (ISFromAproveEIR == true)
                    {
                        Update_Click(null, null);  // Call this function from here to handle Approval Flow
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
                    }
                    Indicatior.Close();

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
            if (dgExpiredList.SelectedItem != null)
            {
                long ID = 0;
                long UnitID = 0;
                ID = ((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).ID;
                UnitID = ((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).UnitId;

                string URL = "../Reports/InventoryPharmacy/ExpiredReturnItem.aspx?ID=" + ID + "&UnitID=" + UnitID;

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
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
            if (dgExpiredList.SelectedItem != null)
            {
                if ((dgExpiredList.SelectedItem as clsExpiredItemReturnVO).Approve == true)
                    cmdApprove.IsEnabled = false;
                else
                    cmdApprove.IsEnabled = true;

                if (Indicatior == null) Indicatior = new WaitIndicator();
                Indicatior.Show();
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
                            if (objDetailList == null) objDetailList = new List<clsExpiredItemReturnDetailVO>();
                            objDetailList = ((clsGetExpiredListForExpiredReturnBizActionVO)e1.Result).ItemList;
                            if (e1.Error == null && e1.Result != null && objDetailList != null)
                            {
                                //bizActionVo.ItemList = ((clsGetExpiredListForExpiredReturnBizActionVO)e1.Result).ItemList;
                                dgExpiredItemList.ItemsSource = null;
                                dgExpiredItemList.ItemsSource = objDetailList.ToList(); //bizActionVo.ItemList;
                            }
                            Indicatior.Close();
                        };
                        Client.ProcessAsync(bizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }

                }
                catch (Exception Ex)
                {
                    Indicatior.Close();
                    throw;
                }
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

                BizAction.objExpiryItem = this.ExpiryItemReturnMaster;

                if (ISEditMode == false)
                {
                    BizAction.objExpiryItem.Time = DateTime.Now;
                }

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
                if (cmbStore.SelectedItem == null)
                {
                    cmbStore.TextBox.SetValidation("Store can not be blank.");
                    cmbStore.TextBox.RaiseValidationError();
                    cmbStore.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Store can not be blank", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    flagIsValid = false; ;
                    ClickedFlag1 = 0;
                    return;
                }

                else if (((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)
                {
                    cmbStore.TextBox.SetValidation("Store can not be blank.");
                    cmbStore.TextBox.RaiseValidationError();
                    cmbStore.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Store can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    flagIsValid = false;
                    ClickedFlag1 = 0;
                    return;

                }
                else
                {
                    cmbStore.TextBox.ClearValidationError();
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
                        else if (item.PendingQty < (item.ReturnQty * item.BaseConversionFactor))
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be greater than Received Qty.\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            flagIsValid = false;
                            ClickedFlag1 = 0;
                            return;
                        }
                        //Added By Somnath On 05/12/2011 To check the Return Rate Greater Than 0
                        //else if (item.ReturnRate == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Zero ReturnRate.", "ReturnRate In The List Can't Be Zero. Please Enter ReturnRate Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();
                        //    flagIsValid = false;
                        //    ClickedFlag1 = 0;
                        //    return;
                        //}
                        //else if (item.ReturnRate < 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Negative ReturnRate.", "ReturnRate Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();
                        //    flagIsValid = false;
                        //    ClickedFlag1 = 0;
                        //    return;
                        //}
                        ////else if (item.ReturnRate > item.PurchaseRate)
                        ////{
                        ////    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        ////     new MessageBoxControl.MessageBoxChildWindow("PALASH", "ReturnRate Can't Be greater than Purchase Rate.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        ////    msgW3.Show();
                        ////    item.ReturnRate = item.PurchaseRate;
                        ////    flagIsValid = false;
                        ////    ClickedFlag1 = 0;
                        ////    return;
                        ////}
                        //else if (item.VATPercentage < 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Vat %", "VAT Percentage Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();
                        //    flagIsValid = false;
                        //    ClickedFlag1 = 0;
                        //    return;
                        //}
                        //else if (item.VATPercentage > 100)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Vat %", "VAT Percentage Can't Be greater than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();
                        //    flagIsValid = false;
                        //    ClickedFlag1 = 0;
                        //    return;
                        //}
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

                string msgTitle = "Palash";
                string msgText = "";

                if (ISFromAproveEIR == true)
                {
                    BizAction.objExpiryItem.EditForApprove = true;
                    BizAction.objExpiryItem.Approve = true;
                }
                else
                {
                    BizAction.objExpiryItem.EditForApprove = false;
                }

                if (flagIsValid)
                {
                    if (ISFromAproveEIR == true)
                    {
                        msgText = "Are you sure you want to Modify and Approve Expiry Items Return ?";
                    }
                    else
                    {
                        msgText = "Are you sure you want to Save Expiry Items Return ?";
                    }

                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWin.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            BizAction.objExpiryItem.SupplierID = (cmbSupplier.SelectedItem as MasterListItem).ID;
                            BizAction.objExpiryItemDetailList = (List<clsExpiredItemReturnDetailVO>)ExpiryAddedItems.ToList<clsExpiredItemReturnDetailVO>();
                            wt.Show();
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            Client.ProcessCompleted += (s, args) =>
                            {
                                ClickedFlag1 = 0;
                                //FillStore();
                                //FillSupplier();
                                ////cmbStore.SelectedValue = 0;
                                ////cmbSupplier.SelectedValue = 0;
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
                                    cmdCancel.IsEnabled = false;
                                    cmdNew.IsEnabled = true;

                                    SetCommandButtonState("Save");

                                    if (ISFromAproveEIR == true)
                                    {
                                        ExpiredItemReturn1Approval AprvEIR = new ExpiredItemReturn1Approval();

                                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                        mElement.Text = "Approve Expiry Items Return";
                                        ((IApplicationConfiguration)App.Current).OpenMainContent(AprvEIR);
                                    }
                                    else
                                        objAnimation.Invoke(RotationType.Backward);

                                    ISFromAproveEIR = false;
                                    ISEditMode = false;
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Expiry Items Return Not Saved.", "Expiry Items Return Not Saved. \n Some Error Occured while saving information.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    mgbx.Show();
                                }

                                FillStore();
                                wt.Close();
                            };

                            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            Client.CloseAsync();
                            //objAnimation.Invoke(RotationType.Backward);
                        }
                        else
                        {
                            ClickedFlag1 = 0;
                        }
                    };
                    msgWin.Show();


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
                clsExpiredItemReturnDetailVO objVO = new clsExpiredItemReturnDetailVO();
                objVO = dgexpItem.SelectedItem as clsExpiredItemReturnDetailVO;
                if (e.Column.Header.ToString().Equals("Return Quantity"))
                {
                    if (((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).UOMConversionList == null || ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).UOMConversionList.Count == 0)
                    {
                        if (objVO.PendingQty < (objVO.ReturnQty * objVO.BaseConversionFactor))
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be greater than Received Qty.\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();

                            objVO.ReturnQty = 0;
                        }
                        else if (objVO.AvailableStock < (objVO.ReturnQty * objVO.ConversionFactor))
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be greater than Current Stock.\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();

                            objVO.ReturnQty = 0;//Convert.ToSingle(Math.Floor(Convert.ToDouble(objVO.AvailableStockInBase / objVO.BaseConversionFactor)));
                        }
                        //else if (objVO.ReturnQty < 1)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be less than 1 .\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    mgbx.Show();

                        //   objVO.ReturnQty = Convert.ToSingle(Math.Floor(Convert.ToDouble(objVO.AvailableStockInBase /objVO.BaseConversionFactor)));

                        //}
                    }
                    else if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID > 0)
                    {

                        CalculateConversionFactorCentral(objVO.SelectedUOM.ID, objVO.StockUOMID);

                    }
                    else
                    {
                        objVO.ReturnQty = 0;
                        objVO.SingleQuantity = 0;

                        objVO.ConversionFactor = 0;
                        objVO.BaseConversionFactor = 0;

                        objVO.MRP = objVO.MainMRP;
                        objVO.PurchaseRate = objVO.MainRate;
                    }
                }

                //if (e.Column.DisplayIndex == 11 && ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnRate < 0)
                //{
                //    //TextBox txtReturn = dgexpItem.Columns[7].GetCellContent(e.Row) as TextBox;

                //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Negative Return Rate", "Return Rate cannot be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    mgbx.Show();
                //    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnRate = 0;
                //}
                //if (e.Column.DisplayIndex == 13 && ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).VATPercentage < 0)
                //{
                //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Negative VAT Percentage", "VAT Percentage cannot be Negative..", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    mgbx.Show();
                //    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).VATPercentage = 0;
                //}
                //else if (e.Column.DisplayIndex == 13 && ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).VATPercentage > 100)
                //{
                //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("VAT Percentage", "VAT Percentage cannot be greater than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    mgbx.Show();
                //    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).VATPercentage = 100;
                //}
            }
            CalculateGRNSummary();
        }

        int ClickedFlag1 = 0;

        public clsExpiredItemReturnVO ExpiryItemReturnMaster { get; set; }

        /// <summary>
        /// Get Item button click
        /// </summary>
        private void cmdGetItems_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStore.SelectedItem != null && ((clsStoreVO)cmbStore.SelectedItem).StoreId > 0)
            {
                ExpiredItemSearch win = new ExpiredItemSearch();
                win.IsFromDOS = false;
                win.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("", "Please select Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
            }

            //try
            //{
            //    if (cmbStore.SelectedItem != null && ((clsStoreVO)cmbStore.SelectedItem).StoreId != 0 && cmbSupplier.SelectedItem != null && ((MasterListItem)cmbSupplier.SelectedItem).ID != 0)
            //    {
            //        ExpiryItemReturnMaster = new clsExpiredItemReturnVO();
            //        ExpiredItemSearch win = new ExpiredItemSearch();
            //        win.StoreID = ((clsStoreVO)cmbClinic.SelectedItem).StoreId;
            //        win.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;

            //        win.dtpFromDate.SelectedDate = dpreturnDt.SelectedDate;

            //        win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
            //        win.Show();
            //    }
            //    else
            //    {
            //        //Added By Somnath on 05/12/2011

            //        if (((clsStoreVO)cmbClinic.SelectedItem).StoreId == 0 && ((MasterListItem)cmbSupplier.SelectedItem).ID != 0)
            //        {
            //            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            mgbx.Show();
            //        }
            //        else if (cmbSupplier.SelectedItem != null && ((MasterListItem)cmbSupplier.SelectedItem).ID == 0 && ((clsStoreVO)cmbClinic.SelectedItem).StoreId != 0)
            //        {
            //            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            mgbx.Show();
            //        }
            //        else
            //        {
            //            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Store and Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            mgbx.Show();
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    throw;
            //}


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
                            if (item.BatchID == ((clsExpiredItemReturnDetailVO)list.Current).BatchID && item.ReceivedID == ((clsExpiredItemReturnDetailVO)list.Current).ReceivedID
                                && item.ReceivedUnitID == ((clsExpiredItemReturnDetailVO)list.Current).ReceivedUnitID && item.ReceivedDetailID == ((clsExpiredItemReturnDetailVO)list.Current).ReceivedDetailID
                                && item.ReceivedDetailUnitID == ((clsExpiredItemReturnDetailVO)list.Current).ReceivedDetailUnitID)
                            {
                                IsExist = true;
                                break;
                            }
                        }
                    }
                    if (IsExist == false)
                        ExpiryAddedItems.Add(item);
                    //ExpiryAddedItems.Add(new clsExpiredItemReturnDetailVO()
                    //{
                    //    ItemID = item.ItemID,
                    //    ItemName = item.ItemName,
                    //    ItemCode = item.ItemCode,
                    //    AvailableStock = item.AvailableStock,
                    //    AvailableStockUOM = item.
                    //    BatchCode = item.BatchCode,
                    //    BatchID = item.BatchID,
                    //    BatchExpiryDate = item.BatchExpiryDate,
                    //    UnitID = item.UnitID,
                    //    ReturnQty = item.ReceivedQty,
                    //    SelectedUOM = new MasterListItem(item.TransactionUOMID, item.ReceivedQtyUOM),
                    //    VATPercentage = item.VATPercentage,
                    //    GRNItemVatApplicationOn = item.GRNItemVatApplicationOn,
                    //    GRNItemVatType = item.GRNItemVatType,
                    //    TaxPercentage = item.TaxPercentage,
                    //    OtherGRNItemTaxApplicationOn = item.OtherGRNItemTaxApplicationOn,
                    //    OtherGRNItemTaxType = item.OtherGRNItemTaxType,
                    //    MainMRP = item.MainMRP,
                    //    MainRate = item.MainRate,
                    //    ConversionFactor = item.ConversionFactor,
                    //    BaseConversionFactor = item.BaseConversionFactor,
                    //    StockUOMID=item.StockUOMID,
                    //    BaseUOMID=item.BaseUOMID,

                    //    ReceivedID=item.ReceivedID,


                    //    //Conversion = item.Conversion,

                    //    //ConversionFactor = item.ConversionFactor,
                    //    //ConversionFactor = 1,
                    //    //PurchaseRate = item.PurchaseRate * item.ConversionFactor,
                    //    //BaseRate = Convert.ToSingle(item.PurchaseRate),
                    //    //BaseMRP = Convert.ToSingle(item.MRP),

                    //    //StockUOMID = item.StockUOMID,
                    //    //StockUOM = item.StockUOM,
                    //    //AvailableStockUOM = item.StockUOM,

                    //    //BaseUOM = item.BaseUOM,
                    //    //BaseUOMID = item.BaseUOMID,
                    //    //BaseQuantity = (Single)(item.ReturnQty) * item.BaseCF,

                    //    //AvailableStockInBase = item.AvailableStockInBase,


                    //    //#region For Quarantine Items (Expired, DOS)

                    //    //// Use For Vat/Tax Calculations






                    //    //#endregion

                    //    ////............................................
                    //}
                    //    );
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
                ExpiryItemReturnMaster.TotalAmount += item.TotalPurchaseRate;
                ExpiryItemReturnMaster.TotalVATAmount += item.VATAmount;
                ExpiryItemReturnMaster.TotalTaxAmount += item.TaxAmount;
                ExpiryItemReturnMaster.NetAmount += item.NetAmount;

            }

            this.txtNetAmt.DataContext = txtRemark.DataContext = txtTAmt.DataContext = txtTTaxAmt.DataContext = txtTVAT.DataContext = ExpiryItemReturnMaster;

            txtTAmt.Text = String.Format("{0:0.00}", ExpiryItemReturnMaster.TotalAmount);
            txtTTaxAmt.Text = String.Format("{0:0.00}", ExpiryItemReturnMaster.TotalTaxAmount);
            txtTVAT.Text = String.Format("{0:0.00}", ExpiryItemReturnMaster.TotalVATAmount);
            txtNetAmt.Text = String.Format("{0:0.00}", ExpiryItemReturnMaster.NetAmount);
            txtRemark.Text = Convert.ToString(ExpiryItemReturnMaster.Remark);

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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (ISFromAproveEIR == true)
            {
                ExpiredItemReturn1Approval AprvEIR = new ExpiredItemReturn1Approval();

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Approve Expiry Items Return";
                ((IApplicationConfiguration)App.Current).OpenMainContent(AprvEIR);
            }
            else
            {
                SetCommandButtonState("New");
                this.DataContext = new clsExpiredItemReturnVO();
                this.ExpiryItemReturnMaster = new clsExpiredItemReturnVO();
                this.txtNetAmt.DataContext = txtRemark.DataContext = txtTAmt.DataContext = txtTTaxAmt.DataContext = txtTVAT.DataContext = ExpiryItemReturnMaster;
                ISEditMode = false;
                objAnimation.Invoke(RotationType.Backward);
                GetExpiredList();
            }
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

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).StockUOMID);



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
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).MRP = objConversionVO.MRP;

                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseMRP = objConversionVO.BaseMRP;


                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = objConversionVO.SingleQuantity;
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;



                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;



                    clsExpiredItemReturnDetailVO objVO = new clsExpiredItemReturnDetailVO();
                    objVO = dgexpItem.SelectedItem as clsExpiredItemReturnDetailVO;
                    if (objVO.PendingQty < (objVO.ReturnQty * objVO.BaseConversionFactor))
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be greater than Received Qty.\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();

                        objVO.ReturnQty = 0;
                    }
                    else if (objVO.AvailableStock < (objVO.ReturnQty * objVO.ConversionFactor))
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be greater than Current Stock.\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();

                        objVO.ReturnQty = 0;//Convert.ToSingle(Math.Floor(Convert.ToDouble(objVO.AvailableStockInBase / objVO.BaseConversionFactor)));
                    }


                    //if (((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseQuantity > ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStock * ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ConversionFactor)
                    //{
                    //    float availQty = Convert.ToSingle(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase);

                    //    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase / objConversionVO.BaseConversionFactor)));
                    //    string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity ";
                    //    ConversionsForAvailableStock();
                    //    MessageBoxControl.MessageBoxChildWindow msgWD =
                    //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgWD.Show();
                    //}
                    //else if (((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty < 1)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be less than 1 .\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    mgbx.Show();
                    //    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStockInBase / ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).BaseConversionFactor)));

                    //}
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmdApprove.Visibility == Visibility.Visible)
                {
                    cmdApprove.IsEnabled = false;
                }
                cmdGetItems.IsEnabled = false;

                ExpiryAddedItems.Clear();
                ExpiryAddedItems = new ObservableCollection<clsExpiredItemReturnDetailVO>();
                dgexpItem.ItemsSource = null;
                dgexpItem.ItemsSource = ExpiryAddedItems;
                dgexpItem.UpdateLayout();

                clsExpiredItemReturnVO obj = null;
                if (ISFromAproveEIR == true)
                {
                    obj = SelectedEIR;
                }
                else
                {
                    obj = (clsExpiredItemReturnVO)dgExpiredList.SelectedItem;
                }

                clsGetExpiredReturnDetailsBizActionVO bizActionVo = new clsGetExpiredReturnDetailsBizActionVO();

                //if (dgExpiredList.SelectedItem != null)
                //{

                bizActionVo.ExpiredID = obj.ID;
                bizActionVo.UnitID = obj.UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e1) =>
                {
                    if (e1.Error == null && e1.Result != null)
                    {
                        clsExpiredItemReturnVO objExpMain = new clsExpiredItemReturnVO();
                        objExpMain = ((clsGetExpiredReturnDetailsBizActionVO)e1.Result).ExpiredMainList;

                        //if (args.Error == null && args.Result != null && ((clsAddExpiryItemReturnBizActionVO)args.Result).objExpiryItem != null)

                        if (objExpMain != null)
                        {
                            this.ExpiryItemReturnMaster = objExpMain;

                            this.DataContext = ExpiryItemReturnMaster;
                            ((clsExpiredItemReturnVO)this.DataContext).ExpiryReturnNo = ExpiryItemReturnMaster.ExpiryReturnNo;
                            ((clsExpiredItemReturnVO)this.DataContext).Date = ExpiryItemReturnMaster.Date;

                            ((clsExpiredItemReturnVO)this.DataContext).StoreID = ExpiryItemReturnMaster.StoreID;
                            ((clsExpiredItemReturnVO)this.DataContext).SupplierID = ExpiryItemReturnMaster.SupplierID;

                            //if (cmbStore.ItemsSource != null)
                            //{
                            //    var results = from r in ((List<MasterListItem>)cmbStore.ItemsSource)
                            //                  where r.ID == obj.StoreID
                            //                  select r;

                            //    foreach (MasterListItem item in results)
                            //    {
                            //        cmbStore.SelectedItem = item;
                            //    }
                            //}

                            if (cmbStore.ItemsSource != null)
                            {
                                var results = from r in ((List<clsStoreVO>)cmbStore.ItemsSource)
                                              where r.StoreId == ExpiryItemReturnMaster.StoreID
                                              select r;

                                foreach (clsStoreVO item in results)
                                {
                                    cmbStore.SelectedItem = item;
                                }
                            }

                            if (cmbSupplier.ItemsSource != null)
                            {
                                var results1 = from r in ((List<MasterListItem>)cmbSupplier.ItemsSource)
                                               where r.ID == ExpiryItemReturnMaster.SupplierID
                                               select r;

                                foreach (MasterListItem item in results1)
                                {
                                    cmbSupplier.SelectedItem = item;
                                }
                            }

                            #region Commented I

                            //((clsGRNVO)this.DataContext).ID = obj.ID;
                            //((clsGRNVO)this.DataContext).POID = obj.POID;

                            //((clsGRNVO)this.DataContext).InvoiceNo = obj.InvoiceNo;
                            //((clsGRNVO)this.DataContext).InvoiceDate = obj.InvoiceDate;

                            //((clsGRNVO)this.DataContext).PaymentModeID = obj.PaymentModeID;

                            //if (obj.PaymentModeID == MaterPayModeList.Cash)
                            //{
                            //    rdbCashMode.IsChecked = true;
                            //}
                            //else if (obj.PaymentModeID == MaterPayModeList.Credit)
                            //{
                            //    rdbCreditMode.IsChecked = true;
                            //}

                            //if (obj.GRNType == InventoryGRNType.Direct)
                            //{
                            //    rdbDirectPur.IsChecked = true;
                            //    dgAddGRNItems.Columns[5].Visibility = Visibility.Collapsed;
                            //    dgAddGRNItems.Columns[6].Visibility = Visibility.Collapsed;
                            //    dgAddGRNItems.Columns[7].Visibility = Visibility.Collapsed;
                            //    dgAddGRNItems.Columns[8].Visibility = Visibility.Collapsed;
                            //    dgAddGRNItems.Columns[4].Visibility = Visibility.Visible;
                            //    dgAddGRNItems.Columns[16].IsReadOnly = true;
                            //    //dgAddGRNItems.Columns[18].IsReadOnly = true;  comented by Ashish z. for MRP Editable as per Client Requirement on 11-03-2016
                            //    dgAddGRNItems.Columns[28].Visibility = Visibility.Visible;
                            //}
                            //else if (obj.GRNType == InventoryGRNType.AgainstPO)
                            //{
                            //    rdbPo.IsChecked = true;
                            //    dgAddGRNItems.Columns[5].Visibility = Visibility.Visible;
                            //    dgAddGRNItems.Columns[6].Visibility = Visibility.Visible;
                            //    dgAddGRNItems.Columns[7].Visibility = Visibility.Visible;
                            //    dgAddGRNItems.Columns[8].Visibility = Visibility.Visible;
                            //    dgAddGRNItems.Columns[16].IsReadOnly = true;
                            //    //dgAddGRNItems.Columns[18].IsReadOnly = true;  comented by Ashish z. for MRP Editable as per Client Requirement on 11-03-2016
                            //    dgAddGRNItems.Columns[28].Visibility = Visibility.Visible;
                            //}

                            //if (obj.IsConsignment == true)
                            //{
                            //    chkConsignment.IsChecked = true;
                            //    chkConsignment.IsEnabled = false;
                            //}
                            //else
                            //{
                            //    chkConsignment.IsChecked = false;
                            //    chkConsignment.IsEnabled = true;
                            //}

                            //((clsGRNVO)this.DataContext).GatePassNo = obj.GatePassNo;
                            //txtGateEntryNo.Text = Convert.ToString(obj.GatePassNo);

                            //((clsGRNVO)this.DataContext).InvoiceDate = obj.InvoiceDate;
                            //dpInvDt.SelectedDate = obj.InvoiceDate;

                            //((clsGRNVO)this.DataContext).InvoiceNo = obj.InvoiceNo;
                            //txtinvno.Text = Convert.ToString(obj.InvoiceNo);

                            //((clsGRNVO)this.DataContext).TotalAmount = obj.TotalAmount;
                            //txttotcamt.Text = obj.TotalAmount.ToString("0.00");

                            //((clsGRNVO)this.DataContext).TotalCDiscount = obj.TotalCDiscount;
                            //txtcdiscamt.Text = obj.TotalCDiscount.ToString();

                            //((clsGRNVO)this.DataContext).TotalSchDiscount = obj.TotalSchDiscount;
                            //txthdiscamt.Text = obj.TotalSchDiscount.ToString();

                            //((clsGRNVO)this.DataContext).TotalTAxAmount = obj.TotalTAxAmount;
                            //txttaxamount.Text = obj.TotalTAxAmount.ToString();

                            //((clsGRNVO)this.DataContext).TotalVAT = obj.TotalVAT;
                            //txtVatamt.Text = String.Format("{0:0.00}", obj.TotalVAT);//obj.TotalVAT.ToString();

                            //((clsGRNVO)this.DataContext).NetAmount = obj.NetAmount;
                            //txtNetamt.Text = obj.NetAmount.ToString();

                            //((clsGRNVO)this.DataContext).Freezed = obj.Freezed;
                            //chkIsFinalized.IsChecked = obj.Freezed;

                            //((clsGRNVO)this.DataContext).Remarks = obj.Remarks;
                            //txtremarks.Text = Convert.ToString(obj.Remarks);

                            //if (cmbReceivedBy.ItemsSource != null)
                            //{
                            //    var results2 = from r in ((List<MasterListItem>)cmbReceivedBy.ItemsSource)
                            //                   where r.ID == obj.ReceivedByID
                            //                   select r;

                            //    ((MasterListItem)cmbReceivedBy.SelectedItem).ID = obj.ReceivedByID;
                            //}

                            #endregion

                            ((clsExpiredItemReturnVO)this.DataContext).Remark = ExpiryItemReturnMaster.Remark;
                            txtRemark.Text = Convert.ToString(ExpiryItemReturnMaster.Remark);

                            ((clsExpiredItemReturnVO)this.DataContext).TotalAmount = ExpiryItemReturnMaster.TotalAmount;
                            txtTAmt.Text = ExpiryItemReturnMaster.TotalAmount.ToString("0.00");

                            ((clsExpiredItemReturnVO)this.DataContext).TotalVATAmount = ExpiryItemReturnMaster.TotalVATAmount;
                            txtTVAT.Text = ExpiryItemReturnMaster.TotalVATAmount.ToString("0.00");

                            ((clsExpiredItemReturnVO)this.DataContext).TotalTaxAmount = ExpiryItemReturnMaster.TotalTaxAmount;
                            txtTTaxAmt.Text = ExpiryItemReturnMaster.TotalTaxAmount.ToString("0.00");

                            ((clsExpiredItemReturnVO)this.DataContext).NetAmount = ExpiryItemReturnMaster.NetAmount;
                            txtNetAmt.Text = ExpiryItemReturnMaster.NetAmount.ToString("0.00");

                            ISEditMode = true;

                            //FillEIRItems(obj.ID, obj.UnitId);

                            if (((clsGetExpiredReturnDetailsBizActionVO)e1.Result).ExpiredItemList != null)
                            {
                                List<clsExpiredItemReturnDetailVO> ExpList = new List<clsExpiredItemReturnDetailVO>();
                                ExpList = ((clsGetExpiredReturnDetailsBizActionVO)e1.Result).ExpiredItemList;

                                foreach (clsExpiredItemReturnDetailVO itemExp in ExpList)
                                {
                                    ExpiryAddedItems.Add(itemExp);
                                }
                            }


                            //bizActionVo.ExpiredItemList = ((clsGetExpiredReturnDetailsBizActionVO)e1.Result).ExpiredItemList; ;
                            dgexpItem.ItemsSource = null;
                            dgexpItem.ItemsSource = ExpiryAddedItems;   // bizActionVo.ExpiredItemList;

                            dgexpItem.UpdateLayout();
                            dgexpItem.Focus();

                            SetCommandButtonState("Modify");

                            #region Commented II

                            //if (ISFromAproveGRN == true)
                            //{
                            //    chkIsFinalized.IsChecked = SelectedGRN.Freezed;

                            //    //ViewAttachment(obj.FileName, obj.File);  // To  Bind (See) Image to View Invoice

                            //    if (obj.Freezed == true)
                            //    {
                            //        cmdSave.IsEnabled = false;
                            //        cmdDraft.IsEnabled = false;
                            //    }
                            //    else
                            //    {
                            //        cmdSave.IsEnabled = true;
                            //        cmdDraft.IsEnabled = true;
                            //    }
                            //}
                            //else
                            //{
                            //    chkIsFinalized.IsChecked = ((clsGRNVO)dgGRNList.SelectedItem).Freezed;

                            //    if (((clsGRNVO)dgGRNList.SelectedItem).Freezed == true)
                            //    {
                            //        cmdSave.IsEnabled = false;
                            //        cmdDraft.IsEnabled = false;
                            //    }
                            //    else
                            //    {
                            //        cmdSave.IsEnabled = true;
                            //        cmdDraft.IsEnabled = true;
                            //    }
                            //}

                            #endregion

                            if (ISFromAproveEIR == true)
                            {
                                cmdSave.Content = "Modify"; //"Modify";
                                cmdSave.IsEnabled = true;
                            }
                            else
                            {
                                cmdSave.IsEnabled = false;
                            }

                            objAnimation.Invoke(RotationType.Forward);

                        }

                    }


                };
                Client.ProcessAsync(bizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillEIRItems(long EIRID, long UnitID)
        {
            dgexpItem.ItemsSource = null;
            try
            {
                clsGetExpiredListForExpiredReturnBizActionVO bizActionVo = new clsGetExpiredListForExpiredReturnBizActionVO();

                //if (dgExpiredList.SelectedItem != null)
                //{
                bizActionVo.ConsumptionID = EIRID;  // ((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).ID;
                bizActionVo.UnitID = UnitID;  // ((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e1) =>
                {
                    if (e1.Error == null && e1.Result != null)
                    {


                        bizActionVo.ItemList = ((clsGetExpiredListForExpiredReturnBizActionVO)e1.Result).ItemList;
                        dgexpItem.ItemsSource = null;
                        dgexpItem.ItemsSource = bizActionVo.ItemList;

                        dgexpItem.UpdateLayout();
                        dgexpItem.Focus();
                    }


                };
                Client.ProcessAsync(bizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
                //}



            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbStore.SelectedItem != null)
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

        //*** Added by Ashish Z. for Direct Approval dated 03062015
        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            if (dgExpiredList.SelectedItem != null)
            {
                if (objDetailList != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("", "Are you sure you want to Approve ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            try
                            {
                                clsAddExpiryItemReturnBizActionVO objVO = new clsAddExpiryItemReturnBizActionVO();
                                objVO.objExpiryItem = new clsExpiredItemReturnVO();
                                objVO.objExpiryItemDetailList = new List<clsExpiredItemReturnDetailVO>();
                                objVO.IsForApproveClick = true;
                                objVO.objExpiryItemDetailList = this.objDetailList.ToList();
                                objVO.objExpiryItem.ID = (dgExpiredList.SelectedItem as clsExpiredItemReturnVO).ID;
                                objVO.objExpiryItem.UnitId = (dgExpiredList.SelectedItem as clsExpiredItemReturnVO).UnitId;

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client.ProcessCompleted += (s, e1) =>
                                {
                                    if ((e1.Result as clsAddExpiryItemReturnBizActionVO).SuccessStatus == -10)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash.", "Expired Item Return Already Approved!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                                        mgbx.Show();
                                        GetExpiredList();
                                    }
                                    else if (e1.Error == null && e1.Result != null)
                                    {
                                        GetExpiredList();
                                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash.", "Expired Item Return Approved Successfully ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        mgbx.Show();
                                    }
                                };
                                Client.ProcessAsync(objVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client.CloseAsync();
                            }
                            catch (Exception Ex)
                            {
                                throw;
                            }
                        }
                    };
                    msgWD.Show();
                }
            }
        }
        //***

    }
}
