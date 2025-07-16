using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
using System.Net;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Input;
namespace PalashDynamics.Pharmacy
{
    public partial class MRPAdjustment : UserControl, IInitiateCIMS
    {
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":
                    CmdApprove.Visibility = Visibility.Collapsed;
                    CmdNew.Visibility = Visibility.Visible;
                    break;
                case "Approve":
                    CmdApprove.Visibility = Visibility.Visible;
                    CmdNew.Visibility = Visibility.Collapsed;
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

        #region "Variable Declaration"

        private SwivelAnimation objAnimation = null;
        string msgTitle = "Palash";
        string msgText = string.Empty;
        //public PagedSortableCollectionView<clsMRPAdjustmentVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsMRPAdjustmentMainVO> MasterList { get; private set; }
        List<clsMRPAdjustmentVO> MasterDetailsList;
        public ObservableCollection<clsMRPAdjustmentVO> MRPAdjustmentItems { get; set; }
        public clsAdjustmentStockVO MRPAdjustmentItem { get; set; }

        public long StoreID { get; set; }
        WaitIndicator Indicator;
        int ClickedFlag1 = 0;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        WaitIndicator objIndicator;
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

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Validation
        public Boolean IsValid()
        {
            if (cboStore.SelectedItem == null || ((MasterListItem)cboStore.SelectedItem).ID == 0)
            {
                msgText = "Please Select Store!";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                return false;
            }
            if (MRPAdjustmentItems.Count <= 0)
            {
                msgText = "MRP Adjustment Grid Cannot be Blank!";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                return false;
            }
            else
            {
                foreach (clsMRPAdjustmentVO item in this.MRPAdjustmentItems.ToList())
                {
                    if (!ISValidMRPandCostprice(item))
                    {
                        return false;
                    }
                }
                dtpDate.ClearValidationError();
                cboStore.ClearValidationError();
                dgMRPAdjustNew.UpdateLayout();
                return true;
            }
        }
        private Boolean ISValidMRPandCostprice(clsMRPAdjustmentVO item)
        {
            Boolean blnResult = true;
            //Scenarion For the Updated MRP
            if (item.UpdatedMRP > 0 && item.UpdatedPurchaseRate == 0)
            {
                if (item.UpdatedMRP < item.PurchaseRate)
                {
                    ShowMessageBox("Updated MRP must be greater than or equal to the Cost Price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    item.UpdatedMRP = item.PurchaseRate + 1;
                    blnResult = false;
                }
            }
            //Scenarion For the Updated CostPrice
            else if (item.UpdatedPurchaseRate > 0 && item.UpdatedMRP == 0)
            {
                if (item.UpdatedPurchaseRate > item.MRP)
                {
                    ShowMessageBox("Updated Cost price must be less than or equal to the MRP.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    item.UpdatedPurchaseRate = item.MRP - 1;
                    blnResult = false;
                }
            }
            //Scenario for the both updated value.
            else if (item.UpdatedPurchaseRate > 0 && item.UpdatedMRP > 0)
            {
                if (item.UpdatedMRP < item.UpdatedPurchaseRate)
                {
                    ShowMessageBox("Updated MRP must be greater than or equal to the updated cost price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    item.UpdatedMRP = item.UpdatedPurchaseRate + 1;
                    blnResult = false;
                }

            }

            else if (item.UpdatedPurchaseRate <= 0 && item.UpdatedMRP <= 0)
            {
                ShowMessageBox("Updated MRP or Updated cost price must be greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                blnResult = false;
            }
            else if (item.UpdatedMRP < 0)
            {
                ShowMessageBox("Updated MRP must be greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                blnResult = false;
            }
            else if (item.UpdatedPurchaseRate < 0)
            {
                ShowMessageBox("Updated cost price must be greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                blnResult = false;
            }
            dgMRPAdjustNew.UpdateLayout();
            return blnResult;
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

        public MRPAdjustment()
        {
            InitializeComponent();
            MasterDetailsList = new List<clsMRPAdjustmentVO>();
            objIndicator = new WaitIndicator();
            MRPAdjustmentItem = new clsAdjustmentStockVO();
            MRPAdjustmentItems = new ObservableCollection<clsMRPAdjustmentVO>();
            objAnimation = new PalashDynamics.Animations.SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            MasterList = new PagedSortableCollectionView<clsMRPAdjustmentMainVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 10;
            this.dataGrid1Pager.DataContext = MasterList;
            this.dgMRPAdjFrontMain.DataContext = MasterList;

            this.Loaded += new RoutedEventHandler(MRPAdjustment_Loaded);
            SetCommandButtonState("New");
            dgMRPAdjustNew.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgMRPAdjustNew_CellEditEnded);
            FillStoreCombobox();
        }
        #endregion

        #region On Refresh Event
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillMRPAdjustmentMainList();//FillMRPAdjustMentList();
        }
        #endregion

        #region Load Event

        void MRPAdjustment_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now;
            dtptoDate.SelectedDate = DateTime.Now;
            this.DataContext = new clsMRPAdjustmentVO();
            FillMRPAdjustmentMainList();
        }

        #endregion

        #region Fill Combobox

        public void FillStoreCombobox()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.IsActive = true;

            //BizAction.Parent = new KeyValue { Value = "ClinicID", Key = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString() };
            BizAction.MasterList = new List<MasterListItem>();
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --", true));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        cboStore.ItemsSource = null;
                        cboStoreforFrontPanel.ItemsSource = null;
                        cboStoreforFrontPanel.ItemsSource = objList;
                        cboStore.ItemsSource = objList;
                        if (objList.Count > 1)
                        {
                            cboStore.SelectedItem = objList[0];
                            cboStoreforFrontPanel.SelectedItem = objList[0];

                            //MasterList = new PagedSortableCollectionView<clsMRPAdjustmentVO>();
                            //MasterList = new PagedSortableCollectionView<clsMRPAdjustmentMainVO>();
                            //MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                            //PageSize = 10;
                            //this.dataGrid2Pager.DataContext = MasterList;
                            //this.dgMRPAdjFront.DataContext = MasterList;
                            //FillMRPAdjustmentMainList();//FillMRPAdjustMentList();
                        }
                        else
                        {
                            cboStore.SelectedItem = objList[0];
                        }
                    }

                    if (this.DataContext != null)
                    {
                        cboStore.SelectedValue = ((clsMRPAdjustmentVO)this.DataContext).StoreID;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception eX)
            {
                throw eX;
            }
        }
        #endregion

        #region Public Methods

        public void FillMRPAdjustmentMainList()
        {
            if (objIndicator == null) objIndicator = new WaitIndicator();
            objIndicator.Show();
            clsGetMRPAdustmentListBizActionVO bizactionVO = new clsGetMRPAdustmentListBizActionVO();
            bizactionVO.GetListCriteria = 1;
            bizactionVO.PagingEnabled = true;
            bizactionVO.MaximumRows = MasterList.PageSize;
            bizactionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            bizactionVO.AdjustmentList = new List<clsMRPAdjustmentVO>();

            if (cboStoreforFrontPanel.SelectedItem != null)
            {
                bizactionVO.StoreID = ((MasterListItem)cboStoreforFrontPanel.SelectedItem).ID;
            }

            bizactionVO.FromDate = dtpFromDate.SelectedDate;
            if (dtptoDate.SelectedDate != null)
                bizactionVO.ToDate = dtptoDate.SelectedDate.Value.Date.AddDays(1);
            bizactionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    objIndicator.Close();
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetMRPAdustmentListBizActionVO)args.Result) != null)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = Convert.ToInt32((((clsGetMRPAdustmentListBizActionVO)args.Result).TotalRows));
                            foreach (clsMRPAdjustmentMainVO item in ((clsGetMRPAdustmentListBizActionVO)args.Result).AdjustmentMainList)
                            {
                                MasterList.Add(item);
                            }

                            dgMRPAdjFrontMain.ItemsSource = null;
                            dgMRPAdjFrontMain.ItemsSource = MasterList;
                        }
                    }
                };
                client.ProcessAsync(bizactionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                objIndicator.Close();
                throw ex;
            }
        }

        public void FillMRPAdjustMentList(long MRPAdjustmentID, long MRPAdjustmentUnitID)
        {
            if (objIndicator == null) objIndicator = new WaitIndicator();
            objIndicator.Show();
            clsGetMRPAdustmentListBizActionVO bizactionVO = new clsGetMRPAdustmentListBizActionVO();
            bizactionVO.GetListCriteria = 2;
            bizactionVO.MRPAdjustmentMainVO = new clsMRPAdjustmentMainVO();
            bizactionVO.MRPAdjustmentMainVO.ID = MRPAdjustmentID;
            bizactionVO.MRPAdjustmentMainVO.UnitID = MRPAdjustmentUnitID;


            bizactionVO.PagingEnabled = false;
            bizactionVO.MaximumRows = MasterList.PageSize;
            bizactionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            bizactionVO.AdjustmentList = new List<clsMRPAdjustmentVO>();

            if (cboStoreforFrontPanel.SelectedItem != null)
            {
                bizactionVO.StoreID = ((MasterListItem)cboStoreforFrontPanel.SelectedItem).ID;
            }

            bizactionVO.FromDate = dtpFromDate.SelectedDate;
            if (dtptoDate.SelectedDate != null)
                bizactionVO.ToDate = dtptoDate.SelectedDate.Value.Date.AddDays(1);
            bizactionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    objIndicator.Close();
                    if (args.Error == null && args.Result != null)
                    {
                        MasterDetailsList.Clear();
                        if (((clsGetMRPAdustmentListBizActionVO)args.Result) != null)
                        {
                            MasterDetailsList = ((clsGetMRPAdustmentListBizActionVO)args.Result).AdjustmentList.ToList();
                            dgMRPAdjFront.ItemsSource = null;
                            dgMRPAdjFront.ItemsSource = MasterDetailsList;
                        }
                    }
                };
                client.ProcessAsync(bizactionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                objIndicator.Close();
                throw ex;
            }
        }

        #endregion

        #region Set Command Button State New/Save/Cancel
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    break;

                case "Save":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    break;

                case "Cancel":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    break;

                case "ClickNew":
                    CmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Click Event

        /// <summary>
        /// New button click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            MRPAdjustmentItems.Clear();
            objAnimation.Invoke(RotationType.Forward);
            MRPAdjustmentItem = new clsAdjustmentStockVO();
            dgMRPAdjustNew.ItemsSource = null;
            this.DataContext = new clsMRPAdjustmentVO();
            dtpDate.SelectedDate = System.DateTime.Now;
            ((clsMRPAdjustmentVO)this.DataContext).AdjustmentDate = Convert.ToDateTime(dtpDate.SelectedDate);
            SetCommandButtonState("ClickNew");
            cboStore.SelectedValue = ((clsMRPAdjustmentVO)this.DataContext).StoreID;
        }

        /// <summary>
        /// Save button click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Save?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        CmdSave.IsEnabled = false;
                        Indicator = new WaitIndicator();
                        Indicator.Show();
                        clsAddMRPAdjustmentBizActionVO objBizactionVO = new clsAddMRPAdjustmentBizActionVO();
                        objBizactionVO.AddCriteria = 1; // 1=add, 2=Approve
                        objBizactionVO.MRPAdjustmentMainVO = new clsMRPAdjustmentMainVO();
                        objBizactionVO.MRPAdjustmentMainVO.StoreID = ((MasterListItem)cboStore.SelectedItem).ID;
                        objBizactionVO.MRPAdjustmentItems = this.MRPAdjustmentItems.ToList();
                        try
                        {
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, args) =>
                            {
                                CmdSave.IsEnabled = true;
                                if (args.Error == null && args.Result != null)
                                {
                                    Indicator.Close();
                                    if (((clsAddMRPAdjustmentBizActionVO)args.Result).SuccessStatus == 1)
                                    {
                                        SetCommandButtonState("Save");
                                        FillMRPAdjustmentMainList();//FillMRPAdjustMentList();
                                        msgText = "Record is successfully submitted!";
                                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        objAnimation.Invoke(RotationType.Backward);
                                    }
                                    else if (((clsAddMRPAdjustmentBizActionVO)args.Result).SuccessStatus == -2)
                                    {
                                        msgText = "Batch already exists for Item: " + ((clsAddMRPAdjustmentBizActionVO)args.Result).ItemName;
                                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    }
                                }
                            };
                            client.ProcessAsync(objBizactionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        catch (Exception ex)
                        {
                            CmdSave.IsEnabled = true;
                            Indicator.Close();
                        }
                    }
                };
                msgWD.Show();

            }

        }

        /// <summary>
        /// Close button click event get handled. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            if (FrontPanel.Visibility == Visibility.Visible)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Inventory Configuration";
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri("PalashDynamics.Administration" + ".xap", UriKind.Relative));
            }

        }
        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == "PalashDynamics.Administration" + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance("PalashDynamics.Administration.frmInventoryConfiguration") as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Search button click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (FrontPanelValidation())
            {
                //MasterList = new PagedSortableCollectionView<clsMRPAdjustmentVO>();
                //MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                //PageSize = 10;
                //this.dataGrid2Pager.DataContext = MasterList;
                //this.dgMRPAdjFront.DataContext = MasterList;

                MasterList = new PagedSortableCollectionView<clsMRPAdjustmentMainVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 10;
                this.dataGrid1Pager.DataContext = MasterList;
                this.dgMRPAdjFrontMain.DataContext = MasterList;
                FillMRPAdjustmentMainList();//FillMRPAdjustMentList();
            }
        }

        /// <summary>
        /// Delete from datagrid event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgMRPAdjustNew.SelectedItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Delete the selected Item ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        clsMRPAdjustmentVO person = ((clsMRPAdjustmentVO)dgMRPAdjustNew.SelectedItem).DeepCopy<clsMRPAdjustmentVO>();
                        MRPAdjustmentItems.RemoveAt(this.dgMRPAdjustNew.SelectedIndex);
                    }
                };
                msgWD.Show();
            }
        }

        /// <summary>
        /// Get Items button click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdItems_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cboStore.SelectedItem).ID == 0 || StoreID == 0)
            {
                msgText = "Please select the store";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            else
            {
                ItemListNew win = new ItemListNew();
                win.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                //win.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                win.StoreID = ((MasterListItem)cboStore.SelectedItem).ID;
                win.cmbStore.IsEnabled = false;
                win.ShowBatches = true;
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.Show();
            }
        }

        /// <summary>
        /// Item selection completed is handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew winItemList = (ItemListNew)sender;
            if (winItemList.DialogResult == true)
            {
                foreach (var item in (winItemList.ItemBatchList))
                {
                    clsMRPAdjustmentVO ObjAdjustment = new clsMRPAdjustmentVO();
                    ObjAdjustment.ItemId = item.ItemID;
                    ObjAdjustment.BatchId = item.BatchID;
                    ObjAdjustment.ItemCode = item.ItemCode;
                    ObjAdjustment.ItemName = item.ItemName;
                    ObjAdjustment.StoreID = StoreID;
                    ObjAdjustment.BatchCode = item.BatchCode == null ? string.Empty : item.BatchCode;
                    ObjAdjustment.UpdatedBatchCode = item.BatchCode == null ? string.Empty : item.BatchCode; ;
                    ObjAdjustment.MRP = item.MRP;
                    ObjAdjustment.UpdatedMRP = item.MRP;
                    ObjAdjustment.PurchaseRate = item.PurchaseRate;
                    ObjAdjustment.UpdatedPurchaseRate = item.PurchaseRate;
                    ObjAdjustment.ExpiryDate = item.ExpiryDate;
                    ObjAdjustment.ExpiryDateString = item.ExpiryDate == null ? string.Empty : item.ExpiryDate.Value.ToString("MMM-yyyy");
                    ObjAdjustment.UpdatedExpiryDate = item.ExpiryDate;
                    ObjAdjustment.IsBatcheRequired = item.BatchesRequired;

                    //clsMRPAdjustmentVO ObjAdjustment = new clsMRPAdjustmentVO
                    //{
                    //    ItemId = item.ItemID,
                    //    BatchId = item.BatchID,
                    //    ItemCode = item.ItemCode,
                    //    ItemName = item.ItemName,
                    //    StoreID = StoreID,
                    //    BatchCode = Convert.ToString(item.BatchCode),
                    //    UpdatedBatchCode = Convert.ToString(item.BatchCode),
                    //    MRP = item.MRP,
                    //    UpdatedMRP = item.MRP,
                    //    PurchaseRate = item.PurchaseRate,
                    //    UpdatedPurchaseRate = item.PurchaseRate,
                    //    ExpiryDate = item.ExpiryDate,
                    //    ExpiryDateString = item.ExpiryDate.Value.ToString("MMM-yyyy"),
                    //    UpdatedExpiryDate = item.ExpiryDate,
                    //};
                    if (MRPAdjustmentItems.Where(z => z.ItemId == item.ItemID).Any() == true)
                    {
                        if (MRPAdjustmentItems.Where(z => z.BatchId == item.BatchID).Any() == false)
                            this.MRPAdjustmentItems.Add(ObjAdjustment);
                    }
                    else
                        MRPAdjustmentItems.Add(ObjAdjustment);
                }
                dgMRPAdjustNew.ItemsSource = null;
                dgMRPAdjustNew.ItemsSource = MRPAdjustmentItems;
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strText = ((TextBox)sender).Text;

            if ((textBefore != null && !String.IsNullOrEmpty(strText)) && (!strText.IsValueDouble()))
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = string.Empty;
                selectionStart = selectionLength = 0;
            }
            else
            {
                ((TextBox)sender).Text = strText.Replace("-", "");
            }

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        #endregion

        #region Selection Changed

        /// <summary>
        /// Store combobox selection changed event is get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgMRPAdjustNew.ItemsSource = null;
            if (cboStore.SelectedItem != null && ((MasterListItem)cboStore.SelectedItem).ID != 0)
            {
                StoreID = ((MasterListItem)cboStore.SelectedItem).ID;
            }
        }

        #endregion

        #region Cell EditEnded Event

        /// <summary>
        /// Grid cell edit ended event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgMRPAdjustNew_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (dgMRPAdjustNew.SelectedItem != null)
            {
                clsMRPAdjustmentVO item = (clsMRPAdjustmentVO)dgMRPAdjustNew.SelectedItem;
                switch (e.Column.Header.ToString())
                {
                    case "Unit Updated MRP":
                        if (!ISValidMRPandCostprice(item))
                        {
                            break;
                        }
                        break;
                    case "Unit Updated Cost Price":
                        if (!ISValidMRPandCostprice(item))
                        {
                            break;
                        }
                        break;
                }
                dgMRPAdjustNew.UpdateLayout();
            }
        }

        #endregion

        private void dgMRPAdjFrontMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMRPAdjFrontMain.SelectedItem != null)
            {
                FillMRPAdjustMentList((dgMRPAdjFrontMain.SelectedItem as clsMRPAdjustmentMainVO).ID, (dgMRPAdjFrontMain.SelectedItem as clsMRPAdjustmentMainVO).UnitID);
            }
        }

        private bool ApproveValidation()
        {
            bool reasult = true;
            if (dgMRPAdjFrontMain.SelectedItem == null)
            {
                msgText = "Please select Request.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                reasult = false;
            }
            else if ((dgMRPAdjFrontMain.SelectedItem as clsMRPAdjustmentMainVO).IsApprove == true)
            {
                msgText = "Selected Request already Approved.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                reasult = false;
            }
            else if ((dgMRPAdjFrontMain.SelectedItem as clsMRPAdjustmentMainVO).IsReject == true)
            {
                msgText = "You cannot Approve Rejected Request.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                reasult = false;
            }

            return reasult;
        }

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            if (ApproveValidation())
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        CmdApprove.IsEnabled = false;
                        Indicator = new WaitIndicator();
                        Indicator.Show();
                        clsAddMRPAdjustmentBizActionVO objBizactionVO = new clsAddMRPAdjustmentBizActionVO();
                        objBizactionVO.AddCriteria = 2; // 1=add, 2=Approve
                        objBizactionVO.MRPAdjustmentMainVO = new clsMRPAdjustmentMainVO();
                        objBizactionVO.MRPAdjustmentMainVO.ID = (dgMRPAdjFrontMain.SelectedItem as clsMRPAdjustmentMainVO).ID;
                        objBizactionVO.MRPAdjustmentMainVO.UnitID = (dgMRPAdjFrontMain.SelectedItem as clsMRPAdjustmentMainVO).UnitID;
                        objBizactionVO.MRPAdjustmentMainVO.StoreID = (dgMRPAdjFrontMain.SelectedItem as clsMRPAdjustmentMainVO).StoreID;
                        objBizactionVO.MRPAdjustmentItems = this.MasterDetailsList.ToList();
                        try
                        {
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, args) =>
                            {
                                CmdApprove.IsEnabled = true;
                                Indicator.Close();
                                if (args.Error == null && args.Result != null)
                                {
                                    //if (((clsAddMRPAdjustmentBizActionVO)args.Result).SuccessStatus == 1)
                                    //{
                                    FillMRPAdjustmentMainList();//FillMRPAdjustMentList();
                                    msgText = "Record Approved Successfully!";
                                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    //}
                                    //else if (((clsAddMRPAdjustmentBizActionVO)args.Result).SuccessStatus == -1)
                                    //{
                                    //    msgText = "Batch already Exists for Item: " + ((clsAddMRPAdjustmentBizActionVO)args.Result).ItemName;
                                    //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    //}
                                }
                            };
                            client.ProcessAsync(objBizactionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        catch (Exception ex)
                        {
                            CmdApprove.IsEnabled = true;
                            Indicator.Close();
                        }
                    }
                };
                msgWD.Show();
            }
        }
    }
}
