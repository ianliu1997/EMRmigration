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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using PalashDynamics.Animations;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using System.ComponentModel;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.Pharmacy.ItemSearch;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Browser;
using PalashDynamics.Pharmacy.Inventory;
using OPDModule.Forms;
namespace PalashDynamics.Pharmacy
{
    public partial class StockAdjustment : UserControl, INotifyPropertyChanged, IInitiateCIMS
    {


        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":
                    CmdApprove.Visibility = Visibility.Collapsed;
                    CmdReject.Visibility = Visibility.Collapsed;
                    dgfrontStockAdj.Columns[7].Visibility= Visibility.Collapsed;  //Columns[7] = Actual Quantity
                    break;
                case "Approve":
                    CmdApprove.Visibility = Visibility.Visible;
                    CmdReject.Visibility = Visibility.Visible;
                    CmdNew.Visibility = Visibility.Collapsed;
                    CmdSave.Visibility = Visibility.Collapsed;
                    dgfrontStockAdj.Columns[7].Visibility = Visibility.Visible; //Columns[7] = Actual Quantity
                    break;
            }
        }


        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Variables

        private SwivelAnimation objAnimation = null;
        bool IsPageLoaded = false;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        double TotalTaxAmount = 0;
        double TotalRate = 0;
        double NetAmount = 0;
        Int64 ScrapId = 0;
        String FilterExpr;
        bool Flag = false;
        bool AFlag = true;
        bool ShowMsg = false;
        public PagedSortableCollectionView<clsAdjustmentStockMainVO> MasterList { get; private set; }
        public ObservableCollection<clsAdjustmentStockVO> StockItems { get; set; }
        public ObservableCollection<clsAdjustmentStockVO> StockItemsInMainGrid { get; set; }
        public clsAdjustmentStockVO StockItem { get; set; }
        public clsAdjustmentStockVO StockItemInDependentGrid { get; set; }
        public clsPhysicalItemsMainVO PhysicalStockMain;
        public List<clsPhysicalItemsVO> PhysicalStockList;
        public bool IsFromPhysicalStock;

        public long StoreID { get; set; }
        #endregion Variables

        #region Properties
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                OnPropertyChanged("PageSize");
            }
        }
        #endregion

        #region Validation
        public Boolean BackPanelValidation()
        {
            if (dtpDate.SelectedDate == null)
            {
                dtpDate.SetValidation("Please Enter Item Stock Adjustment Date");
                dtpDate.RaiseValidationError();
                dtpDate.Focus();
                return false;
            }

            if (dtpDate.SelectedDate != null)
            {
                if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date)
                {
                    dtpDate.SetValidation("Item Stock Adjustment Date can not be less than Today's Date");
                    dtpDate.RaiseValidationError();
                    dtpDate.Focus();
                    return false;
                }
                if (dtpDate.SelectedDate.Value.Date > DateTime.Now.Date)
                {
                    dtpDate.SetValidation("Item Stock Adjustment Date can not be greater than Today's Date");
                    dtpDate.RaiseValidationError();
                    dtpDate.Focus();
                    return false;
                }
            }
            if (txtRemark.Text == string.Empty)
            {
                txtRemark.SetValidation("Remark Is Required");
                txtRemark.RaiseValidationError();
                txtRemark.Focus();
                return false;
            }
            else
                txtRemark.ClearValidationError();

            if (cboStore.SelectedItem == null || ((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
            {
                msgText = "Please Select Store!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
                return false;
            }
            if (StockItems.Count <= 0)
            {
                msgText = "Stock Adjustment Grid Cannot be Blank!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
                return false;
            }
            else
            {

                for (int i = 0; i < this.StockItems.ToList<clsAdjustmentStockVO>().Count; i++)
                {
                    if (this.StockItems.ToList<clsAdjustmentStockVO>()[i].RadioStatusNo == true)
                    {
                        if (this.StockItems.ToList<clsAdjustmentStockVO>()[i].AdjustmentQunatitiy * this.StockItems.ToList<clsAdjustmentStockVO>()[i].BaseConversionFactor > this.StockItems.ToList<clsAdjustmentStockVO>()[i].AvailableStock * this.StockItems.ToList<clsAdjustmentStockVO>()[i].StockingToBaseCF)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please enter adjustment quantity less than or equal to available stock .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWin.Show();
                            this.StockItems.ToList<clsAdjustmentStockVO>()[i].AdjustmentQunatitiy = 0; //this.StockItems.ToList<clsAdjustmentStockVO>()[i].AvailableStock;
                            return false;

                        }
                    }
                    if (this.StockItems.ToList<clsAdjustmentStockVO>()[i].AdjustmentQunatitiy == 0 && IsFromPhysicalStock == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Adjustment Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                        return false;
                    }

                    if (this.StockItems.ToList<clsAdjustmentStockVO>()[i].AdjustmentQunatitiy > 99999)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Adjustment Quantity Should Not Be Greater Than 5 Digits For Item " + "'" + this.StockItems.ToList<clsAdjustmentStockVO>()[i].ItemName + " '.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                        return false;
                    }

                    if (this.StockItems.ToList<clsAdjustmentStockVO>()[i].SelectedUOM.ID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msg =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select UOM For " + this.StockItems.ToList<clsAdjustmentStockVO>()[i].ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msg.Show();
                        return false;
                    }
                }
                dtpDate.ClearValidationError();
                cboStore.ClearValidationError();
                return true;
            }
        }
        public Boolean FrontPanelValidation()
        {
            if (dtpFromDate.SelectedDate == null && dtptoDate.SelectedDate != null)
            {
                dtpFromDate.SetValidation("Please Enter From Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                return false;
            }
            else if (dtptoDate.SelectedDate == null && dtpFromDate.SelectedDate != null)
            {
                dtptoDate.SetValidation("Please Enter To Date");
                dtptoDate.RaiseValidationError();
                dtptoDate.Focus();
                return false;
            }
            else
            {
                dtptoDate.ClearValidationError();
                dtpFromDate.ClearValidationError();
                return true;
            }
        }

        #endregion

        #region Constructor

        public StockAdjustment()
        {
            InitializeComponent();
            StockItems = new ObservableCollection<clsAdjustmentStockVO>();
            StockItem = new clsAdjustmentStockVO();
            objAnimation = new PalashDynamics.Animations.SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(StockAdjustment_Loaded);
            dtpDate.SelectedDate = System.DateTime.Now;
            SetCommandButtonState("New");
            dgstockadjUpdate.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgstockadjUpdate_CellEditEnded);
            FillClinic();
            //FillStoreCombobox();
        }

        #endregion

        #region On Refresh Event
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetStockAdjustmentDatagridMain();
        }
        #endregion

        #region Load Event

        void StockAdjustment_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now;
            dtptoDate.SelectedDate = DateTime.Now;
            cmdItems.IsEnabled = true;

            this.DataContext = new clsAdjustmentStockMainVO();
            //By Anjali..................................................
            if (IsFromPhysicalStock == true)
            {
                objAnimation.Invoke(RotationType.Forward);

                StockItemInDependentGrid = new clsAdjustmentStockVO();
                StockItemsInMainGrid = new ObservableCollection<clsAdjustmentStockVO>();
                StockItems = new ObservableCollection<clsAdjustmentStockVO>();
                StockItem = new clsAdjustmentStockVO();
                dgstockadjUpdate.ItemsSource = null;

                //SetStockDatagrid();
                dtpDate.SelectedDate = System.DateTime.Now;
                SetCommandButtonState("ClickNew");

                this.DataContext = new clsAdjustmentStockMainVO();

                cboBackClinic.SelectedValue = PhysicalStockMain.UnitID;
                cboStore.SelectedValue = PhysicalStockMain.StoreID;
                cmdItems.IsEnabled = false;
                cboStore.IsEnabled = false;



                foreach (var item in PhysicalStockList)
                {

                    clsAdjustmentStockVO ObjAdjustment = new clsAdjustmentStockVO
                    {
                        ItemId = item.ItemID,
                        BatchId = item.BatchID,
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        BatchCode = item.BatchCode,
                        ExpiryDate = item.ExpiryDate,

                        AvailableStock = (double)item.AvailableStock,
                        StoreID = PhysicalStockMain.StoreID,
                        AdjustmentQunatitiy = Convert.ToDouble(String.Format("{0:0.000}", item.AdjustmentQunatity)),
                        intOperationType = item.intOperationType,
                        SelectedUOM = item.SelectedUOM,
                        //BaseQuantity = item.BaseQuantity,
                        UOM = item.UOM,
                        BaseUM = item.BaseUM,
                        BaseUMID = item.BaseUMID,
                        StockingUM = item.StockingUM,
                        StockingUMID = item.StockingUMID,
                        ConversionFactor = item.ConversionFactor,
                        BaseConversionFactor = item.BaseConversionFactor,
                        BaseQuantity = Convert.ToSingle(item.AdjustmentQunatity * item.BaseConversionFactor),
                        StockingToBaseCF = item.BaseConversionFactor

                    };
                    if (item.intOperationType == (int)InventoryStockOperationType.Addition)
                        ObjAdjustment.RadioStatusYes = true;
                    else if (item.intOperationType == (int)InventoryStockOperationType.Subtraction)
                        ObjAdjustment.RadioStatusNo = true;

                    if (StockItems.Where(stockAdjItems => stockAdjItems.ItemId == item.ItemID).Any() == true)
                    {
                        if (StockItems.Where(stockAdjItems => stockAdjItems.BatchId == item.BatchID).Any() == false)
                            this.StockItems.Add(ObjAdjustment);
                    }
                    else
                        StockItems.Add(ObjAdjustment);
                }

                dgstockadjUpdate.ItemsSource = null;
                dgstockadjUpdate.ItemsSource = StockItems;
                dgstockadjUpdate.UpdateLayout();


            }
            //...........................................................


        }

        #endregion

        #region Fill Combobox
        private void FillClinic()
        {

            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
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



                        cboBackClinic.ItemsSource = null;
                        cboBackClinic.ItemsSource = objList;

                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {
                            //for selecting unitid according to user login unit id
                            var res = from r in objList
                                      where r.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                                      select r;

                            cboBackClinic.IsEnabled = false;

                        }

                        if (objList.Count > 1)
                        {

                            foreach (var item in objList)
                            {
                                if (item.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                {
                                    cboBackClinic.SelectedValue = item.ID;
                                    //cboBackClinic.SelectedItem = item;
                                    //cboBackClinic.UpdateLayout();
                                    //cboBackClinic.UpdateLayout();//  ((MasterListItem)res.First());
                                    break;
                                }
                            }

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
        public void FillStoreCombobox(long ClinicID)
        {
            // clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            // BizAction.MasterTable = MasterTableNameList.M_Store;
            // BizAction.IsActive = true;
            // if (ClinicID > 0)
            // {
            //     BizAction.Parent = new KeyValue { Value = "ClinicID", Key = ClinicID.ToString() };
            // }

            //// BizAction.Parent = new KeyValue { Value = "ClinicID", Key = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString() };
            // BizAction.MasterList = new List<MasterListItem>();
            // Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            // PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            // client.ProcessCompleted += (s, args) =>
            // {

            //     if (args.Error == null && args.Result != null)
            //     {

            //         List<MasterListItem> objList = new List<MasterListItem>();


            //         objList.Add(new MasterListItem(0, "-- Select --", true));
            //         objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

            //         cboStore.ItemsSource = null;
            //         cboStoreforFrontPanel.ItemsSource = null;
            //         cboStoreforFrontPanel.ItemsSource = objList;
            //         cboStore.ItemsSource = objList;
            //         if (objList.Count > 1)
            //         {
            //             cboStore.SelectedItem = objList[0];
            //            //dgstockadj.ItemsSource = null; 
            //             //SetStockDatagrid();
            //             cboStoreforFrontPanel.SelectedItem = objList[0];


            //             if (IsFromPhysicalStock != true)
            //             {
            //                 MasterList = new PagedSortableCollectionView<clsAdjustmentStockMainVO>();
            //                 MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            //                 PageSize = 10;
            //                 this.dataGrid2Pager.DataContext = MasterList;
            //                 this.dgfrontStockAdj.DataContext = MasterList;
            //                 SetStockAdjustmentDatagridMain();
            //             }
            //             //dgfrontStockAdj.ItemsSource = null;


            //         }
            //         else
            //         {
            //             cboStore.SelectedItem = objList[0];
            //             cboStore.SelectedItem = objList[0];

            //         }


            //     }
            //     if(IsFromPhysicalStock==true)
            //     {
            //         cboStore.SelectedValue = PhysicalStockMain.StoreID;
            //     }
            //     else if (this.DataContext != null)
            //     {
            //         cboStore.SelectedValue = ((clsAdjustmentStockMainVO)this.DataContext).StoreID;
            //     }


            // };

            // client.ProcessAsync(BizAction, new clsUserVO());
            // client.CloseAsync();

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
                                 where item.ClinicId == ClinicID && item.Status == true && item.IsQuarantineStore == false//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true 
                                 select item;
                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, select);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cboStoreforFrontPanel.ItemsSource = result.ToList();
                            cboStoreforFrontPanel.SelectedItem = result.ToList()[0];
                            cboStore.ItemsSource = result.ToList();
                            cboStore.SelectedItem = result.ToList()[0];
                            if (IsFromPhysicalStock != true)
                            {
                                MasterList = new PagedSortableCollectionView<clsAdjustmentStockMainVO>();
                                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                                PageSize = 10;
                                this.dataGrid2Pager.DataContext = MasterList;
                                this.dgfrontStockAdj.DataContext = MasterList;
                                SetStockAdjustmentDatagridMain();
                            }
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores != null)
                        {
                            cboStoreforFrontPanel.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cboStoreforFrontPanel.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            cboStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cboStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            if (IsFromPhysicalStock != true)
                            {
                                MasterList = new PagedSortableCollectionView<clsAdjustmentStockMainVO>();
                                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                                PageSize = 10;
                                this.dataGrid2Pager.DataContext = MasterList;
                                this.dgfrontStockAdj.DataContext = MasterList;
                                SetStockAdjustmentDatagridMain();
                            }
                        }
                    }
                }
                if (IsFromPhysicalStock == true)
                {
                    cboStore.SelectedValue = PhysicalStockMain.StoreID;
                }
                else if (this.DataContext != null)
                {
                    cboStore.SelectedValue = ((clsAdjustmentStockMainVO)this.DataContext).StoreID;
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        #endregion

        #region Public Methods

        public void SetStockAdjustmentDatagrid()
        {
            clsGetStockAdustmentListBizActionVO bizactionVO = new clsGetStockAdustmentListBizActionVO();
            //bizactionVO.PagingEnabled = true;
            //bizactionVO.MaximumRows = MasterList.PageSize;
            //bizactionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            //bizactionVO.AdjustStock = new List<clsAdjustmentStockVO>();

            //if (cboStoreforFrontPanel.SelectedItem != null)
            //{
            //    bizactionVO.StoreID = ((MasterListItem)cboStoreforFrontPanel.SelectedItem).ID;
            //}

            //bizactionVO.FromDate = dtpFromDate.SelectedDate;
            //if (dtptoDate.SelectedDate != null)
            //  bizactionVO.ToDate = dtptoDate.SelectedDate.Value.Date.AddDays(1);
            //bizactionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            bizactionVO.StockAdjustmentID = ((clsAdjustmentStockMainVO)dgPhysicalItem.SelectedItem).ID;
            bizactionVO.StockAdjustmentUnitID = ((clsAdjustmentStockMainVO)dgPhysicalItem.SelectedItem).UnitID;

            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetStockAdustmentListBizActionVO)args.Result) != null)
                        {
                            //MasterList.Clear();
                            //MasterList.TotalItemCount = (int)(((clsGetStockAdustmentListBizActionVO)args.Result).TotalRows);
                            //foreach (clsAdjustmentStockVO item in ((clsGetStockAdustmentListBizActionVO)args.Result).AdjustStock)
                            //{
                            //    MasterList.Add(item);
                            //}


                            List<clsAdjustmentStockVO> objList = new List<clsAdjustmentStockVO>();

                            objList = ((clsGetStockAdustmentListBizActionVO)args.Result).AdjustStock;
                            dgfrontStockAdj.ItemsSource = null;
                            dgfrontStockAdj.ItemsSource = objList;


                        }

                    }

                };
                client.ProcessAsync(bizactionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }
        //public void SetStockDatagrid()
        //{
        //clsGetStockDetailsForStockAdjustmentBizActionVO bizactionVO = new clsGetStockDetailsForStockAdjustmentBizActionVO();
        //bizactionVO.StockList = new List<clsAdjustmentStockVO>();
        //bizactionVO.StoreID =((MasterListItem)cboStore.SelectedItem).ID; ;


        //try
        //{

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {

        //        if (args.Error == null && args.Result != null)
        //        {
        //            if (((clsGetStockDetailsForStockAdjustmentBizActionVO)args.Result)!=null)
        //            {
        //                //dgstockadj.ItemsSource =null;
        //                StockItemsInMainGrid = new ObservableCollection<clsAdjustmentStockVO>(((clsGetStockDetailsForStockAdjustmentBizActionVO)args.Result).StockList);

        //                var result = from item in StockItemsInMainGrid
        //                             where ((clsAdjustmentStockVO)item).StoreID == bizactionVO.StoreID
        //                             select item;
        //                //dgstockadj.ItemsSource = result;
        //            }

        //        }

        //    };
        //    client.ProcessAsync(bizactionVO, new clsUserVO());
        //    client.CloseAsync();
        //}
        //catch (Exception ex)
        //{

        //}
        //}
        #endregion


        #region Set Command Button State New/Save/Cancel
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    CmdPrint.IsEnabled = true;
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = false;
                    break;

                case "Save":
                    CmdPrint.IsEnabled = true;
                    CmdApprove.IsEnabled = true;
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = false;
                    break;

                case "Modify":

                    CmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    break;

                case "ClickNew":
                    CmdPrint.IsEnabled = false;
                    CmdApprove.IsEnabled = false;
                    CmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;

                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Click Event

        private void chkStatus_Checked(object sender, RoutedEventArgs e)
        {
            //dgstockadj.ItemsSource = StockItemsInMainGrid;
            //clsAdjustmentStockVO p1 = ((CheckBox)e.OriginalSource).DataContext as clsAdjustmentStockVO;
            //if (p1.Status == false)
            //{
            //    if (this.dgstockadj.SelectedItem is clsAdjustmentStockVO)
            //    {
            //        StockItems.Add(((clsAdjustmentStockVO)this.dgstockadj.SelectedItem));
            //    }
            //}
            //dgstockadjUpdate.ItemsSource = null;
            //dgstockadjUpdate.ItemsSource = StockItems;

        }

        private void chkStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            //clsAdjustmentStockVO p1 = ((CheckBox)e.OriginalSource).DataContext as clsAdjustmentStockVO;
            //if (p1.Status == true)
            //{
            //    clsAdjustmentStockVO person = this.dgstockadj.SelectedItem as clsAdjustmentStockVO;
            //    var item =
            //    from p in StockItems
            //    where p.ItemId == person.ItemId && p.BatchId == person.BatchId
            //    select p;
            //    if (((List<clsAdjustmentStockVO>)item.ToList<clsAdjustmentStockVO>()).Count > 0)
            //    {

            //            StockItems.Remove(((List<clsAdjustmentStockVO>)item.ToList<clsAdjustmentStockVO>())[0]);

            //    }
            //}


            //dgstockadjUpdate.ItemsSource = null;
            //dgstockadjUpdate.ItemsSource = StockItems;


        }



        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Forward);
            //var SelectedItem = ((List<MasterListItem>)cboStore.ItemsSource).First(s => s.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
            //cboStore.SelectedItem = (MasterListItem)SelectedItem;

            StockItemInDependentGrid = new clsAdjustmentStockVO();
            StockItemsInMainGrid = new ObservableCollection<clsAdjustmentStockVO>();
            StockItems = new ObservableCollection<clsAdjustmentStockVO>();
            StockItem = new clsAdjustmentStockVO();
            dgstockadjUpdate.ItemsSource = null;
            txtRemark.Text = string.Empty;
            //SetStockDatagrid();
            dtpDate.SelectedDate = System.DateTime.Now;
            SetCommandButtonState("ClickNew");

            this.DataContext = new clsAdjustmentStockMainVO();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
            {
                cboBackClinic.SelectedValue = ((clsAdjustmentStockMainVO)this.DataContext).UnitID;
            }
            cboStore.SelectedValue = ((clsAdjustmentStockMainVO)this.DataContext).StoreID;

            if (txtRemark.Text == string.Empty)
            {
                txtRemark.SetValidation("Remark Is Required");
                txtRemark.RaiseValidationError();
                txtRemark.Focus();

            }

        }
        WaitIndicator Indicator;
        int ClickedFlag1 = 0;
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 += 1;
            if (ClickedFlag1 == 1)
            {
                if (BackPanelValidation())
                {
                    Indicator = new WaitIndicator();
                    Indicator.Show();
                    clsAddStockAdjustmentBizActionVO bizactionVO = new clsAddStockAdjustmentBizActionVO();
                    bizactionVO.objMainStock = new clsAdjustmentStockMainVO();
                    bizactionVO.StockAdustmentItems = new List<clsAdjustmentStockVO>();
                    bizactionVO.DateTime = dtpDate.SelectedDate.Value;

                    if (cboStore.SelectedItem != null)
                        bizactionVO.StoreId = ((clsStoreVO)cboStore.SelectedItem).StoreId;

                    if (cboBackClinic.SelectedItem != null)
                        bizactionVO.UnitID = ((MasterListItem)cboBackClinic.SelectedItem).ID;
                    try
                    {
                        if (cboStore.SelectedItem != null)
                            bizactionVO.objMainStock.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                        bizactionVO.objMainStock.RequestedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        bizactionVO.objMainStock.Remark = txtRemark.Text;
                        //Added By Anjali............................................
                        if (IsFromPhysicalStock == true)
                        {
                            bizactionVO.objMainStock.PhysicalItemID = PhysicalStockMain.ID;
                            bizactionVO.objMainStock.PhysicalItemUnitID = PhysicalStockMain.UnitID;
                            bizactionVO.objMainStock.IsFromPST = true;
                        }
                        //................................................................

                        bizactionVO.StockAdustmentItems = ((List<clsAdjustmentStockVO>)StockItems.ToList<clsAdjustmentStockVO>());



                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            ClickedFlag1 = 0;
                            if (args.Error == null && args.Result != null)
                            {

                                if (((clsAddStockAdjustmentBizActionVO)args.Result).SuccessStatus == 1)
                                {
                                    SetCommandButtonState("Save");
                                    //SetStockAdjustmentDatagrid();
                                    msgText = "Record is successfully submitted!";

                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();

                                    if (IsFromPhysicalStock == true)
                                    {
                                        ModuleName = "PalashDynamics.Pharmacy";
                                        Action = "PalashDynamics.Pharmacy.frmPhysicalItemStock";
                                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                                        WebClient c2 = new WebClient();
                                        c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                                        c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                                    }
                                    else
                                    {
                                        SetStockAdjustmentDatagridMain();
                                        objAnimation.Invoke(RotationType.Backward);
                                    }



                                    //After Insertion Back to BackPanel and Setup Page

                                    Indicator.Close();
                                }


                            }

                        };
                        client.ProcessAsync(bizactionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        ClickedFlag1 = 0;
                        Indicator.Close();
                    }
                    finally
                    {
                        Indicator.Close();
                    }

                }
                else
                    ClickedFlag1 = 0;
            }
        }
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

            if (IsFromPhysicalStock == true)
            {
                ModuleName = "PalashDynamics.Pharmacy";
                Action = "PalashDynamics.Pharmacy.frmPhysicalItemStock";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                dtpDate.SelectedDate = null;
                StockItems = new ObservableCollection<clsAdjustmentStockVO>();
                dgstockadjUpdate.ItemsSource = null;
                //dgstockadj.ItemsSource = null;
                StockItem = new clsAdjustmentStockVO();
                objAnimation.Invoke(RotationType.Backward);
                SetCommandButtonState("New");
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
                ((IApplicationConfiguration)App.Current).SelectedPatient = null;



            }
            catch (Exception ex)
            {
                throw;
            }



        }
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            for (int i = 0; i < StockItems.Count; i++)
            {
                if (StockItemInDependentGrid.ItemId == StockItems[i].ItemId)
                {
                    if (StockItemInDependentGrid.BatchId == StockItems[i].BatchId)
                    {
                        StockItems[i].UpdatedBalance = StockItems[i].AvailableStock + StockItems[i].AdjustmentQunatitiy;
                        StockItems[i].OperationType = InventoryStockOperationType.Addition;
                        MessageBox.Show(StockItems[i].UpdatedBalance.ToString() + " " + StockItems[i].OperationType.ToString());
                        break;
                    }
                }
            }
        }

        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < StockItems.Count; i++)
            {
                if (StockItemInDependentGrid.ItemId == StockItems[i].ItemId)
                {
                    if (StockItemInDependentGrid.BatchId == StockItems[i].BatchId)
                    {
                        StockItems[i].UpdatedBalance = StockItems[i].AvailableStock - StockItems[i].AdjustmentQunatitiy;
                        StockItems[i].OperationType = InventoryStockOperationType.Subtraction;
                        MessageBox.Show(StockItems[i].UpdatedBalance.ToString() + " " + StockItems[i].OperationType.ToString());
                        break;
                    }
                }
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (FrontPanelValidation())
            {
                MasterList = new PagedSortableCollectionView<clsAdjustmentStockMainVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 10;
                this.dataGrid2Pager.DataContext = MasterList;
                this.dgfrontStockAdj.DataContext = MasterList;
                //SetStockAdjustmentDatagrid();
                SetStockAdjustmentDatagridMain();
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgstockadjUpdate.SelectedItem is clsAdjustmentStockVO)
            {
                clsAdjustmentStockVO person = ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).DeepCopy<clsAdjustmentStockVO>();
                StockItems.Remove(((clsAdjustmentStockVO)this.dgstockadjUpdate.SelectedItem));
                int i = 0;
                foreach (clsAdjustmentStockVO item in StockItemsInMainGrid)
                {
                    if (item.ItemId == person.ItemId && item.BatchId == person.BatchId)
                    {

                        item.Status = false;
                        StockItemsInMainGrid.Remove(item);
                        StockItemsInMainGrid.Insert(i, item);


                        break;
                    }
                    i++;
                }


            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (StockItemInDependentGrid != null)
            {
                if (((RadioButton)e.OriginalSource).Content.Equals("Add"))
                {
                    for (int i = 0; i < StockItems.Count; i++)
                    {
                        if (StockItemInDependentGrid.ItemId == StockItems[i].ItemId)
                        {
                            if (StockItemInDependentGrid.BatchId == StockItems[i].BatchId)
                            {
                                StockItems[i].UpdatedBalance = StockItems[i].AvailableStock + StockItems[i].AdjustmentQunatitiy;
                                StockItems[i].OperationType = InventoryStockOperationType.Addition;
                                MessageBox.Show(StockItems[i].UpdatedBalance.ToString() + " " + StockItems[i].OperationType.ToString());
                                break;
                            }
                        }
                    }
                }
                if (((RadioButton)e.OriginalSource).Content.Equals("Subtract"))
                {
                    for (int i = 0; i < StockItems.Count; i++)
                    {
                        if (StockItemInDependentGrid.ItemId == StockItems[i].ItemId)
                        {
                            if (StockItemInDependentGrid.BatchId == StockItems[i].BatchId)
                            {
                                //if (StockItems[i].AvailableStock < StockItems[i].AdjustmentQunatitiy)
                                //{
                                //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                //                   new MessageBoxControl.MessageBoxChildWindow("Greater Adjustment Quantity!", "Adjustment Quantity In The List Can't Be Greater Than Availbale Quantity. Please Enter Adjustment Quantiy Less Than Or Equal To Available Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                //    msgW3.Show();
                                //}
                                //else
                                //{
                                StockItems[i].UpdatedBalance = StockItems[i].AvailableStock - StockItems[i].AdjustmentQunatitiy;
                                StockItems[i].OperationType = InventoryStockOperationType.Subtraction;
                                MessageBox.Show(StockItems[i].UpdatedBalance.ToString() + " " + StockItems[i].OperationType.ToString());
                                break;
                                // }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Selection Changed
        private void cboStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsFromPhysicalStock != true)
                StockItems.Clear();
            if (cboStore.SelectedItem != null && ((clsStoreVO)cboStore.SelectedItem).StoreId != 0)
            {
                StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
            }
            StockItemsInMainGrid = new ObservableCollection<clsAdjustmentStockVO>();
            //SetStockDatagrid();
        }

        private void cboStore_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            //MasterList = new PagedSortableCollectionView<clsAdjustmentStockVO>();
            //MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            //PageSize = 10;
            //this.dataGrid2Pager.DataContext = MasterList;
            //this.dgfrontStockAdj.DataContext = MasterList;
            //SetStockAdjustmentDatagrid();
        }
        private void dgstockadjUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (cboStore.SelectedItem != null && ((clsStoreVO)cboStore.SelectedItem).StoreId != 0)
            {
                StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
            }
            StockItemInDependentGrid = ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).DeepCopy<clsAdjustmentStockVO>();


        }
        #endregion

        private void dgstockadjUpdate_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.DisplayIndex == 5)
            {
                if (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM != null && ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM.ID == ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseUMID && (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy % 1) != 0)
                {
                    ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy = 1;
                    string msgText = "Quantity Cannot be in fraction";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();

                }

                if (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM != null && ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM.ID > 0)
                {

                    if (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).RadioStatusNo == true)
                    {
                        if (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy * ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor > ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AvailableStock * ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).StockingToBaseCF)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please enter adjustment quantity less than or equal to available stock .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWin.Show();
                            //   ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy = 0;// ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AvailableStock;
                            // return false;

                        }
                        else
                        {
                            ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseQuantity = Convert.ToSingle(Convert.ToSingle(((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy) * ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor);
                        }
                    }
                    else
                    {
                        ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseQuantity = Convert.ToSingle(Convert.ToSingle(((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy) * ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor);
                    }
                }

                else
                {

                    CalculateConversionFactorCentral(((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM.ID, ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).StockingUMID);//By Umesh

                    if (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy * ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor > ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AvailableStock * ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).StockingToBaseCF)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                          new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Used Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        // ((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).UsedQty = (decimal)((clsMaterialConsumptionItemDetailsVO)dgItemList.SelectedItem).AvailableStock;
                    }
                }

                if (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy < 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please enter adjustment quantity greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                }
            }

        }

        private void cmdItems_Click(object sender, RoutedEventArgs e)
        {


            bool Result = true;
            if (cboStore.SelectedItem == null)
            {
                cboStore.TextBox.SetValidation("Please Select Store");
                cboStore.TextBox.RaiseValidationError();
                cboStore.Focus();
                Result = false;
            }
            else if (((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
            {
                cboStore.TextBox.SetValidation("Please Select Store");
                cboStore.TextBox.RaiseValidationError();
                cboStore.Focus();
                Result = false;
            }
            if (cboBackClinic.SelectedItem == null)
            {
                cboBackClinic.TextBox.SetValidation("Please Select Clinic");
                cboBackClinic.TextBox.RaiseValidationError();
                cboBackClinic.Focus();
                Result = false;
            }
            else if (((MasterListItem)cboBackClinic.SelectedItem).ID == 0)
            {
                cboBackClinic.TextBox.SetValidation("Please Select Clinic");
                cboBackClinic.TextBox.RaiseValidationError();
                cboBackClinic.Focus();
                Result = false;
            }



            if (Result == true)
            {


                ItemListNew win = new ItemListNew();
                win.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                //win.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                win.StoreID = StoreID;
                win.cmbStore.IsEnabled = false;
                win.ShowZeroStockBatches = false;// Commented by Ashish Z. on Dated 20092016        win.ShowZeroStockBatches = true;
                win.ShowBatches = true;
                win.IsFromStockAdjustment = true;
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);

                win.Show();
            }
        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //foreach (clsItemStockVO item in (((ItemList)sender).SelectedBatches))
            //{
            //    var result = from r in ((ItemList)sender).SelectedItems
            //                 where r.ID == item.ItemID
            //                 select r;

            //    String ItemCode = String.Empty, ItemName = String.Empty;

            //    if (result.Count() > 0)
            //    {
            //        ItemCode = ((clsItemMasterVO)result.First()).ItemCode;
            //        ItemName = ((clsItemMasterVO)result.First()).ItemName;
            //    }


            //    clsAdjustmentStockVO ObjAdjustment = new clsAdjustmentStockVO
            //    {
            //        ItemId=item.ItemID,
            //        BatchId=item.BatchID,
            //        ItemCode = ItemCode,
            //        ItemName = ItemName,
            //        BatchCode = item.BatchCode,
            //        ExpiryDate = item.ExpiryDate,
            //        AvailableStock = (double)item.AvailableStock,
            //        StoreID=StoreID

            //    };
            //    if (StockItems.Where(stockAdjItems => stockAdjItems.ItemId == item.ItemID).Any() == true)
            //    {
            //        if (StockItems.Where(stockAdjItems => stockAdjItems.BatchId == item.BatchID).Any() == false)
            //            this.StockItems.Add(ObjAdjustment);
            //    }
            //    else
            //    StockItems.Add(ObjAdjustment);
            //}

            foreach (var item in (((ItemListNew)sender).ItemBatchList))
            {
                //var result = from r in ((ItemList)sender).SelectedItems
                //             where r.ID == item.ItemID
                //             select r;

                String ItemCode = String.Empty, ItemName = String.Empty;

                //if (result.Count() > 0)
                //{
                //    ItemCode = ((clsItemMasterVO)result.First()).ItemCode;
                //    ItemName = ((clsItemMasterVO)result.First()).ItemName;
                //}

                ItemCode = item.ItemCode;
                ItemName = item.ItemName;

                clsAdjustmentStockVO ObjAdjustment = new clsAdjustmentStockVO
                {
                    ItemId = item.ItemID,
                    BatchId = item.BatchID,
                    ItemCode = ItemCode,
                    ItemName = ItemName,
                    BatchCode = item.BatchCode,
                    ExpiryDate = item.ExpiryDate,
                    AvailableStock = (double)item.AvailableStock,
                    AvailableStockInBase = (double)item.AvailableStockInBase,
                    StoreID = StoreID,
                    StockingUMID = item.SUM,
                    StockingUM = item.SUOM,
                    BaseUMID = item.BaseUM,
                    StockingToBaseCF = item.StockingToBaseCFForStkAdj,
                    BaseConversionFactor = item.StockingToBaseCFForStkAdj,
                    PurchaseToBaseCF = item.PurchaseToBaseCF,
                    SelectedUOM = new MasterListItem { ID = item.SUM, Description = item.SUOM }

                };
                if (StockItems.Where(stockAdjItems => stockAdjItems.ItemId == item.ItemID).Any() == true)
                {
                    if (StockItems.Where(stockAdjItems => stockAdjItems.BatchId == item.BatchID).Any() == false)
                        this.StockItems.Add(ObjAdjustment);
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Item Already Added.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                    }
                }
                else
                    StockItems.Add(ObjAdjustment);
            }

            dgstockadjUpdate.ItemsSource = null;
            dgstockadjUpdate.ItemsSource = StockItems;


        }

        private void cboBackClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboBackClinic.SelectedItem != null)
            {
                long clinicId = ((MasterListItem)cboBackClinic.SelectedItem).ID;

                FillStoreCombobox(clinicId);
            }
        }

        private void dgPhysicalItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPhysicalItem.SelectedItem != null)
                SetStockAdjustmentDatagrid();
        }

        public void SetStockAdjustmentDatagridMain()
        {
            clsGetStockAdustmentListMainBizActionVO bizactionVO = new clsGetStockAdustmentListMainBizActionVO();
            bizactionVO.PagingEnabled = true;
            bizactionVO.MaximumRows = MasterList.PageSize;
            bizactionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            bizactionVO.AdjustStock = new List<clsAdjustmentStockMainVO>();

            if (cboStoreforFrontPanel.SelectedItem != null)
            {
                bizactionVO.StoreID = ((clsStoreVO)cboStoreforFrontPanel.SelectedItem).StoreId;
            }

            bizactionVO.FromDate = dtpFromDate.SelectedDate;
            if (dtptoDate.SelectedDate != null)
                bizactionVO.ToDate = dtptoDate.SelectedDate.Value.Date.AddDays(1);

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                bizactionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId; //0;
            else
                bizactionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetStockAdustmentListMainBizActionVO)args.Result) != null)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetStockAdustmentListMainBizActionVO)args.Result).TotalRows);
                            foreach (clsAdjustmentStockMainVO item in ((clsGetStockAdustmentListMainBizActionVO)args.Result).AdjustStock)
                            {
                                MasterList.Add(item);
                            }

                            dgPhysicalItem.ItemsSource = null;
                            dgPhysicalItem.ItemsSource = MasterList;
                            dataGrid2Pager.Source = null;
                            dataGrid2Pager.PageSize = Convert.ToInt32(bizactionVO.MaximumRows);
                            dataGrid2Pager.Source = MasterList;
                            dgPhysicalItem.SelectedIndex = 0;
                        }

                    }

                };
                client.ProcessAsync(bizactionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPhysicalItem.SelectedItem != null)
                {
                    if (((clsAdjustmentStockMainVO)dgPhysicalItem.SelectedItem).IsApproved)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Stock Adjustment is already approved .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else if (((clsAdjustmentStockMainVO)dgPhysicalItem.SelectedItem).IsRejected)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "You Cannot Approve The Rejected Stock Adjustment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Approve Stock Adjustment ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                clsUpdateStockAdjustmentBizActionVO BizAction = new clsUpdateStockAdjustmentBizActionVO();
                                BizAction.IsForApproval = true;
                                BizAction.objMainStock = ((clsAdjustmentStockMainVO)dgPhysicalItem.SelectedItem);
                                BizAction.objMainStock.IsApproved = true;
                                BizAction.objMainStock.ApprovedDateTime = DateTime.Now;
                                BizAction.objMainStock.ApprovedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                if (dgfrontStockAdj.ItemsSource != null)
                                {
                                    BizAction.StockAdustmentItems = (List<clsAdjustmentStockVO>)dgfrontStockAdj.ItemsSource;
                                }

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                client.ProcessCompleted += (s, arg) =>
                                {
                                    if (arg.Error == null)
                                    {
                                        if (arg.Result != null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Stock Adjustment Approved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            mgbox.Show();
                                            SetStockAdjustmentDatagridMain();
                                        }
                                    }
                                };
                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            }
                        };
                        mgBox.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No Stock Adjustment is Selected \n Please select a Stock Adjustment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void dgstockadjUpdate_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (IsFromPhysicalStock == true)
            {
                for (int i = 0; i < dgstockadjUpdate.Columns.Count(); i++)
                {
                    dgstockadjUpdate.Columns[i].IsReadOnly = true;
                }

                DataGridColumn column2 = dgstockadjUpdate.Columns[7];
                FrameworkElement fe2 = column2.GetCellContent(e.Row);
                FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));
                if (result2 != null)
                {
                    DataGridCell cell = (DataGridCell)result2;
                    cell.IsEnabled = false;
                }


                DataGridColumn column = dgstockadjUpdate.Columns[6];
                FrameworkElement fe = column.GetCellContent(e.Row);
                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                if (result != null)
                {
                    DataGridCell cell = (DataGridCell)result;
                    cell.IsEnabled = false;
                }
                DataGridColumn column1 = dgstockadjUpdate.Columns[14];
                FrameworkElement fe1 = column1.GetCellContent(e.Row);
                FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));
                if (result1 != null)
                {
                    DataGridCell cell = (DataGridCell)result;
                    cell.IsEnabled = false;
                }

            }
            else
            {
            }
        }
        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = null;
            if (child != null)
            {
                parent = child.Parent;
            }
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;

        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgPhysicalItem.SelectedItem != null)
            {
                Nullable<DateTime> dtpF = null;
                Nullable<DateTime> dtpT = null;
                Nullable<DateTime> dtpP = null;
                dtpF = dtpT = dtpP = System.DateTime.Today;
                string URL = "../Reports/Inventory_New/VarianceStockreport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy")
                      + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") 
                      + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") 
                      + "&StoreID=" + ((clsStoreVO)cboStoreforFrontPanel.SelectedItem).StoreId
                      + "&LoginUnitID=" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                      + "&ID=" + (dgPhysicalItem.SelectedItem as clsAdjustmentStockMainVO).ID
                      + "&UnitID=" + (dgPhysicalItem.SelectedItem as clsAdjustmentStockMainVO).UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else
            {
                string msgText = "Please select a Request.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }

            //Nullable<DateTime> dtpF = null;
            //Nullable<DateTime> dtpT = null;
            //Nullable<DateTime> dtpP = null;

            //bool chkToDate = true;
            //string msgTitle = "";

            //if (dtpFromDate.SelectedDate != null)
            //{
            //    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
            //}

            //if (dtptoDate.SelectedDate != null)
            //{
            //    dtpT = dtptoDate.SelectedDate.Value.Date.Date;
            //    if (dtpF.Value > dtpT.Value)
            //    {
            //        dtptoDate.SelectedDate = dtpFromDate.SelectedDate;
            //        dtpT = dtpF;
            //        chkToDate = false;
            //    }
            //    else
            //    {
            //        dtpP = dtpT;
            //        dtpT = dtpT.Value.AddDays(1);
            //    }
            //}

            //if (dtpT != null)
            //{
            //    if (dtpF != null)
            //    {
            //        dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

            //    }
            //}

            //if (chkToDate == true)
            //{
            //    string URL;
            //    if (dtpF != null && dtpT != null && dtpP != null)
            //    {
            //        URL = "../Reports/Inventory_New/VarianceStockreport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&StoreID=" + ((clsStoreVO)cboStoreforFrontPanel.SelectedItem).StoreId + "&LoginUnitID=" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            //    }

            //}
            //else
            //{
            //    string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
            //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgWindow.Show();
            //}
        }

        private void CmdReject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPhysicalItem.SelectedItem != null)
                {
                    if (((clsAdjustmentStockMainVO)dgPhysicalItem.SelectedItem).IsApproved)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "You Cannot Reject The Approved Stock Adjustment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else if (((clsAdjustmentStockMainVO)dgPhysicalItem.SelectedItem).IsRejected)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Stock Adjustment is already rejected.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Reject Stock Adjustment ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                frmStockAdjumentCancellationWindow win1 = new frmStockAdjumentCancellationWindow();
                                win1.OnSaveButton_Click += new RoutedEventHandler(win1_OnSaveButton_Click);
                                win1.OnCancelButton_Click += new RoutedEventHandler(win1_OnCancelButton_Click);
                                win1.Show();
                            }
                        };
                        mgBox.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No Stock Adjustment is Selected \n Please select a Stock Adjustment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void win1_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                try
                {
                    clsUpdateStockAdjustmentBizActionVO BizAction = new clsUpdateStockAdjustmentBizActionVO();
                    BizAction.IsForApproval = false;
                    BizAction.objMainStock = ((clsAdjustmentStockMainVO)dgPhysicalItem.SelectedItem);
                    BizAction.objMainStock.IsRejected = true;
                    BizAction.objMainStock.RejectedDateTime = DateTime.Now;
                    BizAction.objMainStock.RejectedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    BizAction.objMainStock.ResonForRejection = ((frmStockAdjumentCancellationWindow)sender).txtAppReason.Text;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Stock Adjustment Rejected Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                mgbox.Show();
                                SetStockAdjustmentDatagridMain();
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);


                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        void win1_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbSUM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).UOMConversionList == null || ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }
        WaitIndicator Indicatior;
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).ItemId;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        Indicatior.Close();

                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];

                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgstockadjUpdate.SelectedItem != null)
                        {
                            ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
                        }


                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw;
            }


        }
        private void cmbSUM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        # region //By Umesh For Conversion Factor

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgstockadjUpdate.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).UOMConversionList;



                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    //objConversionVO.MainMRP = ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).MainMRP;
                    //objConversionVO.MainRate = ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = (float)((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy;

                    long BaseUOMID = ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseUMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;


                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    //((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).Rate = objConversionVO.Rate;
                    //((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).MRP = objConversionVO.MRP;


                    ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).Quantity = objConversionVO.Quantity;
                    ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    //((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    //((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;

                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void cmbPUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgstockadjUpdate.SelectedItem != null)
            {
                clsConversionsVO objConversion = new clsConversionsVO();

                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);


                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).StockingUMID);
                }
                else
                {
                    ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).ConversionFactor = 0;
                    ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor = 0;
                    //((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).MRP = ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).MainMRP;
                    //((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).Rate = ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).MainRate;
                }
            }
        }

        private void cmbPUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).UOMConversionList == null || ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);    //Already present on form need to check
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgstockadjUpdate.SelectedItem != null)
            {
                Conversion win = new Conversion();

                win.FillUOMConversions(((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).ItemId, ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM.ID);
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

            ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;
            if (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM != null && ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).SelectedUOM.ID == ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).BaseUMID && (((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy % 1) != 0)
            {
                ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).AdjustmentQunatitiy = 1;
                string msgText = "Quantity Cannot be in fraction";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();

            }
            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsAdjustmentStockVO)dgstockadjUpdate.SelectedItem).StockingUMID);
        }

        # endregion


    }
}
