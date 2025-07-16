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
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.UserControls;
namespace PalashDynamics.Pharmacy
{
    public partial class StoreMaster : UserControl, INotifyPropertyChanged
    {
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

        #region Validation
        public bool Validation(bool IsForNew)
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtStoreCode.Text.Trim()))
            {
                txtStoreCode.SetValidation("Please Enter Code");
                txtStoreCode.RaiseValidationError();
                txtStoreCode.Focus();
                result = false;
            }
            else
                txtStoreCode.ClearValidationError();
            if (ChkIsQuarantineStore.IsChecked == true)
            {
                if (cmbStore.SelectedItem == null)
                {
                    cmbStore.TextBox.SetValidation("Please Enter Parent Store");
                    cmbStore.TextBox.RaiseValidationError();
                    cmbStore.Focus();
                    result = false;
                }
                else if (((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)
                {
                    cmbStore.TextBox.SetValidation("Please Enter Parent Store");
                    cmbStore.TextBox.RaiseValidationError();
                    cmbStore.Focus();
                    result = false;
                }
                else
                    cmbStore.TextBox.ClearValidationError();

            }

            if (string.IsNullOrEmpty(txtStoreName.Text.Trim()))
            {
                txtStoreName.SetValidation("Please Enter Description");
                txtStoreName.RaiseValidationError();
                txtStoreName.Focus();
                result = false;
            }
            else
                txtStoreName.ClearValidationError();


            if (!IsForNew)
            {
                if (CmbClinic.SelectedItem == null)
                {
                    //msgText = "Please Select Clinic !";
                    //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgWindow.Show();

                    CmbClinic.TextBox.SetValidation("Please Select Clinic");
                    CmbClinic.TextBox.RaiseValidationError();
                    CmbClinic.Focus();
                    result = false;
                }
                else if (((MasterListItem)CmbClinic.SelectedItem).ID == 0)
                {
                    //msgText = "Please Select Clinic !";
                    //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //msgWindow.Show();

                    CmbClinic.TextBox.SetValidation("Please Select Clinic");
                    CmbClinic.TextBox.RaiseValidationError();
                    CmbClinic.Focus();
                    result = false;
                }
            }
            //else
            //{
            //    //txtStoreCode.ClearValidationError();
            //    //txtStoreName.ClearValidationError();
            //    //cmbStore.TextBox.ClearValidationError();
            //    //result = true;
            //}
            return result;
        }

        #endregion

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

        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = ""; // "PALASHDYNAMICS";
        string msgText = "";
        long StoreId;
        clsStoreVO getstoreinfo;
        public bool isModify = false;
        public PagedSortableCollectionView<clsStoreVO> MasterList { get; private set; }

        public PagedSortableCollectionView<clsStoreVO> CentralStoreList { get; private set; }
        public bool CentralStoreChecked;
        public bool CentralStoreSet = false;
        public bool IsCancel = true;
        public bool Status = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        //string textBefore = null;
        //int selectionStart = 0, selectionLength = 0;
        List<MasterListItem> ObjSig = new List<MasterListItem>();
        List<MasterListItem> objList1 = new List<MasterListItem>();
        WaitIndicator Indicatior = null;
        #endregion

        #region Constructor
        public StoreMaster()
        {

            InitializeComponent();
            this.DataContext = new clsStoreVO();
            StoreId = 0;
            FillClinic();
            FillItemCategory();
            FillCostCenterCode();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(StoreMaster_Loaded);
            SetCommandButtonState("Load");
            MasterList = new PagedSortableCollectionView<clsStoreVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            CentralStoreList = new PagedSortableCollectionView<clsStoreVO>();
            PageSize = 10;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdStore.DataContext = MasterList;
            SetupPage();

        }
        #endregion

        #region Refresh Event
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        #endregion

        #region Load Event
        void StoreMaster_Loaded(object sender, RoutedEventArgs e)
        {
            lblCategory.Visibility = Visibility.Collapsed;
            cboItemCategory.Visibility = Visibility.Collapsed;
            //  Validation();
        }
        #endregion

        #region PublicMethods

        public void SetupPage()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsGetStoreDetailsBizActionVO bizActionVO = new clsGetStoreDetailsBizActionVO();
            bizActionVO.StoreType = 2;
            bizActionVO.SearchExpression = txtSearch.Text;
            bizActionVO.PagingEnabled = true;
            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            getstoreinfo = new clsStoreVO();
            bizActionVO.ItemMatserDetails = new List<clsStoreVO>();
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.ItemMatserDetails = (((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails);
                        if (StoreId == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            MasterList.TotalItemCount = (int)(((clsGetStoreDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsStoreVO item in bizActionVO.ItemMatserDetails)
                            {
                                MasterList.Add(item);
                            }

                            #region Added by Saily  for IsCentral Store
                            CentralStoreList.Clear();
                            CentralStoreList.TotalItemCount = (int)(((clsGetStoreDetailsBizActionVO)args.Result).TotalRows);
                            foreach (clsStoreVO item in bizActionVO.ItemMatserDetails)
                            {
                                if (item.isCentralStore == true)
                                {
                                    //CentralStoreList.Add(item);
                                    CentralStoreSet = true;
                                }
                            }
                            #endregion

                        }

                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }
        }

        public void clsGetStoreWithCategory(long StoreId)
        {

            Indicatior = new WaitIndicator();
            Indicatior.Show();
            List<MasterListItem> AllCategoryList = new List<MasterListItem>();
            AllCategoryList = (List<MasterListItem>)cboItemCategory.ItemsSource;

            clsGetStoreWithCategoryDetailsBizActionVO bizActionVO = new clsGetStoreWithCategoryDetailsBizActionVO();
            bizActionVO.StoreId = StoreId;

            getstoreinfo = new clsStoreVO();
            //bizActionVO.ItemMatserCategoryDetails = new List<long>();
            List<long> categoryList = new List<long>();
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {

                        lblCategory.Visibility = Visibility.Collapsed;
                        cboItemCategory.Visibility = Visibility.Collapsed;
                        if ((((clsGetStoreWithCategoryDetailsBizActionVO)args.Result).IsForAll) == true)
                            chkAllItemSelection.IsChecked = true;
                        if ((((clsGetStoreWithCategoryDetailsBizActionVO)args.Result).IsForCategories) == true)
                        {
                            chkApplyCatWise.IsChecked = true;
                            lblCategory.Visibility = Visibility.Visible;
                            cboItemCategory.Visibility = Visibility.Visible;
                        }
                        bizActionVO.ItemMatserCategoryDetails = (((clsGetStoreWithCategoryDetailsBizActionVO)args.Result).ItemMatserCategoryDetails);
                        if (bizActionVO.ItemMatserCategoryDetails != null)
                        {
                            foreach (long item in bizActionVO.ItemMatserCategoryDetails)
                                categoryList.Add(item);
                        }
                        foreach (var allcatitem in AllCategoryList)
                            allcatitem.Status = false;
                        foreach (var allcatitem in AllCategoryList)
                        {
                            foreach (long item in bizActionVO.ItemMatserCategoryDetails)
                            {
                                if (allcatitem.ID == item)
                                    allcatitem.Status = true;
                            }
                        }

                        if (cmbStore.ItemsSource != null && grdStore.SelectedItem != null)
                        {
                            var SelectedParent = ((List<clsStoreVO>)cmbStore.ItemsSource).FirstOrDefault(S => S.StoreId == ((clsStoreVO)grdStore.SelectedItem).Parent);
                            //  if (SelectedParent != null)
                            cmbStore.SelectedItem = SelectedParent;
                            cmbStore.SelectedValue = SelectedParent.StoreId;
                            //else
                            //    cmbStore.SelectedItem = objlistStore[0];
                        }
                        Indicatior.Close();
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
            }

        }

        public void ClearUI()
        {
            txtStoreCode.Text = string.Empty;
            txtStoreName.Text = string.Empty;
            chkExpiryItemReturn.IsChecked = false;
            chkGoodsReceivedNote.IsChecked = false;
            chkGRNReturn.IsChecked = false;
            chkIndent.IsChecked = false;
            chkIssue.IsChecked = false;
            chkItemReturn.IsChecked = false;
            chkItemSaleReturn.IsChecked = false;
            chkItemsSale.IsChecked = false;
            chkOpeningBalance.IsChecked = false;
            chkReceiveIssue.IsChecked = false;
            chkReceiveItemReturn.IsChecked = false;
            MasterListItem Defaultc = ((List<MasterListItem>)CmbClinic.ItemsSource).FirstOrDefault(s => s.ID == 0);
            CmbClinic.SelectedItem = Defaultc;
            cboItemCategory.ItemsSource = null;
            cboItemCategory.ItemsSource = objList1;
            cboItemCategory.UpdateLayout();
            cboItemCategory.Visibility = Visibility.Collapsed;
            lblCategory.Visibility = Visibility.Collapsed;
            cboItemCategory.SelectedValue = (long)0;
            cmbCostCenterCode.SelectedValue = (long)0;

            List<MasterListItem> ItemList = new List<MasterListItem>();
            ItemList = (List<MasterListItem>)cboItemCategory.ItemsSource;
            if (ItemList != null && ItemList.Count > 0)
            {
                foreach (var item in ItemList)
                {
                    if (item.Status == true)
                    {
                        item.Status = false;
                    }
                }
            }

        }
        #endregion

        #region Fill Comboboxes

        public void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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

                    CmbClinic.ItemsSource = null;
                    CmbClinic.ItemsSource = objList;
                    CmbClinic.SelectedItem = objList[0];

                }

                //if (this.DataContext != null)
                //{
                //    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                //}


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        //private void FillStores(long clinicId)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_Store;
        //    BizAction.MasterList = new List<MasterListItem>();


        //    BizAction.Parent = new KeyValue { Value = "IsQuarantineStore", Key = "0" };

        //    //if (clinicId > 0)
        //    //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
        //    //else
        //    //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = null };
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();                   
        //            objList.Add(new MasterListItem(0, "--NA--"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);                   
        //            cmbStore.ItemsSource = null;
        //            cmbStore.ItemsSource = objList;  
        //          // cmbStore.SelectedItem = objList[0]; 


        //            if (grdStore.SelectedItem != null)
        //            {
        //                //cmbStore.SelectedValue = ((clsStoreVO)grdStore.SelectedItem).Parent;
        //            }
        //        }

        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        private void FillCostCenterCode()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_CostCenterCodes;
            BizAction.MasterList = new List<MasterListItem>();


            //   BizAction.Parent = new KeyValue { Value = "IsQuarantineStore", Key = "0" };

            //if (clinicId > 0)
            //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
            //else
            //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = null };
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbCostCenterCode.ItemsSource = null;
                    cmbCostCenterCode.ItemsSource = objList;
                    cmbCostCenterCode.SelectedItem = objList[0];
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        List<clsStoreVO> objlistStore = new List<clsStoreVO>();
        private void FillStores(long pClinicID)
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;

            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    objlistStore = new List<clsStoreVO>();
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "-Select--", ClinicId = pClinicID, Status = true, IsQuarantineStore = false, Parent = 0 };
                    //BizActionObj.ItemMatserDetails.Insert(0, Default);
                    objlistStore.Add(Default);
                    objlistStore.AddRange(((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails);

                    // cmbFromStore.ItemsSource = BizActionObj.ItemMatserDetails;


                    var result = from item in objlistStore
                                 where item.ClinicId == pClinicID && item.Status == true
                                 select item;


                    var result2 = from item in objlistStore
                                  where item.Status == true
                                  select item;

                    if (pClinicID > 0)
                    {
                        cmbStore.ItemsSource = null;
                        cmbStore.ItemsSource = (List<clsStoreVO>)result.ToList();
                        cmbStore.SelectedItem = objlistStore[0];
                    }
                    else
                    {
                        cmbStore.ItemsSource = null;
                        cmbStore.ItemsSource = (List<clsStoreVO>)result2.ToList();
                        cmbStore.SelectedItem = objlistStore[0];
                    }



                }

            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillItemCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ItemCategory;
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
                    objList1 = objList;
                    cboItemCategory.ItemsSource = null;
                    cboItemCategory.ItemsSource = objList1;
                    cboItemCategory.SelectedValue = (long)0;
                }
                if (ObjSig.Count > 0)
                {
                    if (ObjSig != null)
                    {
                        foreach (var item in ObjSig)
                        {
                            if (objList1 != null)
                            {
                                foreach (MasterListItem item1 in objList1)
                                {
                                    if (item.ID == item1.ID)
                                    {
                                        item1.Status = item.Status;
                                    }
                                }
                            }
                        }
                        cboItemCategory.ItemsSource = null;
                        cboItemCategory.ItemsSource = objList1;
                        cboItemCategory.UpdateLayout();
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #endregion Fill Comboboxes

        #region Set Command Button State New/Save/Modify/Cancel
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible;
                    break;

                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    //cmdClose.Visibility = Visibility.Visible;
                    //cmdCancel.Visibility = Visibility.Collapsed; 
                    break;

                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "FrontPanel":

                    cmdAdd.IsEnabled = true;
                    //cmdCancel.Visibility = Visibility.Collapsed;
                    //cmdClose.Visibility = Visibility.Visible;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;

                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    //cmdClose.Visibility = Visibility.Collapsed;
                    //cmdCancel.Visibility = Visibility.Visible; 
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Events

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Validation(true);
            ClearUI();
            chkAllItemSelection.Visibility = Visibility.Visible;
            this.grdBackPanel.DataContext = new clsStoreVO();
            isModify = false;
            SetCommandButtonState("New");
            try
            {
                objAnimation.Invoke(RotationType.Forward);
                //cmbStore.Text = "--NA--";
                txtSearch.Text = String.Empty;
                SetupPage();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private clsStoreVO CreateFormData()
        {
            clsStoreVO addNewStoreVO = new clsStoreVO();
            if (isModify == true)
            {
                addNewStoreVO.StoreId = StoreId;
                // addNewStoreVO = ((clsStoreVO)grdStore.SelectedItem);
                addNewStoreVO = (clsStoreVO)this.grdBackPanel.DataContext;
            }
            else
            {

                addNewStoreVO.StoreId = 0;
                addNewStoreVO = (clsStoreVO)this.grdBackPanel.DataContext;
                addNewStoreVO.Status = true;

            }
            if (ChkIsQuarantineStore.IsChecked == true)
                addNewStoreVO.IsQuarantineStore = true;
            addNewStoreVO.ClinicId = ((MasterListItem)CmbClinic.SelectedItem).ID;

            #region Commented By Shikha
            //if (chkOpeningBalance.IsChecked == true)
            //    addNewStoreVO.OpeningBalance = true;
            //else
            //    addNewStoreVO.OpeningBalance = false;

            //if (chkReceiveIssue.IsChecked == true)
            //    addNewStoreVO.ReceiveIssue = true;
            //else
            //    addNewStoreVO.ReceiveIssue = false;

            //if (chkReceiveItemReturn.IsChecked == true)
            //    addNewStoreVO.ReceiveItemReturn = true;
            //else
            //    addNewStoreVO.ReceiveItemReturn = false;

            //if (chkExpiryItemReturn.IsChecked == true)
            //    addNewStoreVO.ExpiryItemReturn = true;
            //else
            //    addNewStoreVO.ExpiryItemReturn = false;

            //if (chkGoodsReceivedNote.IsChecked == true)
            //    addNewStoreVO.GoodsReceivedNote = true;
            //else
            //    addNewStoreVO.GoodsReceivedNote = false;

            //if (chkGRNReturn.IsChecked == true)
            //    addNewStoreVO.GRNReturn = true;
            //else
            //    addNewStoreVO.GRNReturn = false;

            //if (chkIndent.IsChecked == true)
            //    addNewStoreVO.Indent = true;
            //else
            //    addNewStoreVO.Indent = false;

            //if (chkIssue.IsChecked == true)
            //    addNewStoreVO.Issue = true;
            //else
            //    addNewStoreVO.Issue = false;

            //if (chkItemReturn.IsChecked == true)
            //    addNewStoreVO.ItemReturn = true;
            //else
            //    addNewStoreVO.ItemReturn = false;

            //if (chkItemSaleReturn.IsChecked == true)
            //    addNewStoreVO.ItemSaleReturn = true;
            //else
            //    addNewStoreVO.ItemSaleReturn = false;

            //if (chkItemsSale.IsChecked == true)
            //    addNewStoreVO.ItemsSale = true;
            //else
            //    addNewStoreVO.ItemsSale = false;
            #endregion

            return addNewStoreVO;
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {

            if (Validation(false))
            {

                string msgTitle = "";
                string msgText = "Are you sure you want to Update ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
            else { Validation(false); }
            #region  Commented by shikha
            //    clsAddUpdateStoreDetailsBizActionVO bizactionVO = new clsAddUpdateStoreDetailsBizActionVO();
            //    clsStoreVO addNewStoreVO = new clsStoreVO();
            //    try
            //    {
            //        isModify = true;
            //        addNewStoreVO = CreateFormData();
            //        addNewStoreVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //        addNewStoreVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            //        addNewStoreVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
            //        addNewStoreVO.UpdatedDateTime = System.DateTime.Now;
            //        addNewStoreVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
            //        bizactionVO.ItemMatserDetails.Add(addNewStoreVO);
            //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //        client.ProcessCompleted += (s, args) =>
            //        {

            //            if (args.Error == null && args.Result != null)
            //            {

            //                if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 1)
            //                {
            //                    msgText = "Record is successfully submitted!";

            //                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                    msgWindow.Show();

            //                    //After Updation Back to BackPanel and Setup Page
            //                    StoreId = 0;
            //                    SetupPage();
            //                    objAnimation.Invoke(RotationType.Backward);
            //                    SetCommandButtonState("Modify");

            //                }
            //                else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 2)
            //                {
            //                    msgText = "Record cannot be save because CODE already exist!";
            //                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //                    msgWindow.Show();
            //                }
            //                else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 3)
            //                {
            //                    msgText = "Record cannot be save because DESCRIPTION already exist!";
            //                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //                    msgWindow.Show();
            //                }
            //            }

            //        };
            //        client.ProcessAsync(bizactionVO, new clsUserVO());
            //        client.CloseAsync();
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            #endregion
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {

                #region To add category in List

                long Significant = 0;
                bool IsValidate = true;
                List<MasterListItem> CategoryList = new List<MasterListItem>();
                List<long> ItemList1 = new List<long>();
                CategoryList = (List<MasterListItem>)cboItemCategory.ItemsSource;
                foreach (var item in CategoryList)
                {
                    if (item.Status == true)
                    {
                        ItemList1.Add(item.ID);
                    }
                }

                string ItemCatagoryID = "";
                List<MasterListItem> ItemList = new List<MasterListItem>();
                ItemList = (List<MasterListItem>)cboItemCategory.ItemsSource;
                if (ItemList != null && ItemList.Count > 0)
                {
                    foreach (var item in ItemList)
                    {
                        if (item.Status == true)
                        {
                            ItemCatagoryID = ItemCatagoryID + item.ID;
                            ItemCatagoryID = ItemCatagoryID + ",";
                        }
                    }

                    if (ItemCatagoryID.EndsWith(","))
                        ItemCatagoryID = ItemCatagoryID.Remove(ItemCatagoryID.Length - 1, 1);

                }

                #endregion

                clsAddUpdateStoreDetailsBizActionVO bizactionVO = new clsAddUpdateStoreDetailsBizActionVO();
                bizactionVO.IsForStatusUpdate = false;
                clsStoreVO addNewStoreVO = new clsStoreVO();
                try
                {
                    isModify = true;
                    addNewStoreVO = (clsStoreVO)this.DataContext;

                    addNewStoreVO = CreateFormData();
                    addNewStoreVO.ItemCatagoryID = ItemCatagoryID;
                    addNewStoreVO.CostCenterCodeID = ((MasterListItem)cmbCostCenterCode.SelectedItem).ID;
                    addNewStoreVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewStoreVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewStoreVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewStoreVO.AddedDateTime = System.DateTime.Now;
                    addNewStoreVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemCategoryID = ItemList1;
                    bizactionVO.ItemMatserDetails.Add(addNewStoreVO);

                    if (chkAllItemSelection.IsChecked == true)
                        bizactionVO.ApplyallItems = true;
                    else if (chkApplyCatWise.IsChecked == true)
                        bizactionVO.ApplyallItems = false;
                    else
                        bizactionVO.ApplyallItems = null;


                    if (cmbStore.SelectedItem != null)
                        addNewStoreVO.Parent = ((clsStoreVO)cmbStore.SelectedItem).StoreId;


                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully Updated!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                StoreId = 0;
                                SetupPage();
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");
                                cboItemCategory.SelectedValue = (long)0;  //By Umesh to Refresh combo
                            }
                            else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 4)
                            {
                                msgText = "Parent Store Already Having A Qurantine Store";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
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

            else
            {
                grdBackPanel.DataContext = ((clsStoreVO)grdStore.SelectedItem).DeepCopy();
                var SelectedItem = ((List<MasterListItem>)CmbClinic.ItemsSource).First(s => s.ID == ((clsStoreVO)grdStore.SelectedItem).ClinicId);
                CmbClinic.SelectedItem = (MasterListItem)SelectedItem;
                //  StoreId = ((clsStoreVO)grdStore.SelectedItem).StoreId;
                cmbStore.SelectedItem = ((clsStoreVO)grdStore.SelectedItem).ParentName;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

            if (Validation(false))
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";

                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();

                #region Commented
                //clsAddUpdateStoreDetailsBizActionVO bizactionVO = new clsAddUpdateStoreDetailsBizActionVO();
                //clsStoreVO addNewStoreVO = new clsStoreVO();
                //try
                //{
                //    isModify = false;

                //    addNewStoreVO = (clsStoreVO)this.DataContext;

                //    addNewStoreVO = CreateFormData();
                //    addNewStoreVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //    addNewStoreVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    addNewStoreVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                //    addNewStoreVO.AddedDateTime = System.DateTime.Now;
                //    addNewStoreVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                //    bizactionVO.ItemMatserDetails.Add(addNewStoreVO);
                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    client.ProcessCompleted += (s, args) =>
                //    {

                //        if (args.Error == null && args.Result != null)
                //        {
                //            if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 1)
                //            {
                //                msgText = "Record is successfully submitted!";

                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //                msgWindow.Show();
                //                //After Insertion Back to BackPanel and Setup Page
                //                StoreId = 0;
                //                SetupPage();
                //                objAnimation.Invoke(RotationType.Backward);
                //                SetCommandButtonState("Save");
                //            }
                //             else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 2)
                //            {
                //                msgText = "Record cannot be save because CODE already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }
                //            else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 3)
                //            {
                //                msgText = "Record cannot be save because DESCRIPTION already exist!";
                //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //                msgWindow.Show();
                //            }

                //        }

                //    };
                //    client.ProcessAsync(bizactionVO, new clsUserVO());
                //    client.CloseAsync();
                //}
                //catch (Exception ex)
                //{

                //}
                #endregion
            }
            else { Validation(false); }
        }

        void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {

                #region To add Significants in List

                long Significant = 0;
                bool IsValidate = true;
                List<MasterListItem> CategoryList = new List<MasterListItem>();
                List<long> ItemList1 = new List<long>();
                CategoryList = (List<MasterListItem>)cboItemCategory.ItemsSource;
                foreach (var item in CategoryList)
                {
                    if (item.Status == true)
                    {
                        ItemList1.Add(item.ID);
                    }
                }

                string ItemCatagoryID = "";
                List<MasterListItem> ItemList = new List<MasterListItem>();
                ItemList = (List<MasterListItem>)cboItemCategory.ItemsSource;
                if (ItemList != null && ItemList.Count > 0)
                {
                    foreach (var item in ItemList)
                    {
                        if (item.Status == true)
                        {
                            ItemCatagoryID = ItemCatagoryID + item.ID;
                            ItemCatagoryID = ItemCatagoryID + ",";
                        }
                    }

                    if (ItemCatagoryID.EndsWith(","))
                        ItemCatagoryID = ItemCatagoryID.Remove(ItemCatagoryID.Length - 1, 1);

                }



                #endregion


                clsAddUpdateStoreDetailsBizActionVO bizactionVO = new clsAddUpdateStoreDetailsBizActionVO();
                bizactionVO.IsForStatusUpdate = false;
                clsStoreVO addNewStoreVO = new clsStoreVO();
                try
                {
                    isModify = false;

                    addNewStoreVO = (clsStoreVO)this.DataContext;

                    addNewStoreVO = CreateFormData();
                    addNewStoreVO.ItemCatagoryID = ItemCatagoryID;
                    addNewStoreVO.CostCenterCodeID = ((MasterListItem)cmbCostCenterCode.SelectedItem).ID;
                    addNewStoreVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewStoreVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewStoreVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewStoreVO.AddedDateTime = System.DateTime.Now;
                    addNewStoreVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemCategoryID = ItemList1;
                    bizactionVO.ItemMatserDetails.Add(addNewStoreVO);

                    if (chkAllItemSelection.IsChecked == true)
                        bizactionVO.ApplyallItems = true;
                    else if (chkApplyCatWise.IsChecked == true)
                        bizactionVO.ApplyallItems = false;
                    else
                        bizactionVO.ApplyallItems = null;


                    if (cmbStore.SelectedItem != null)
                        addNewStoreVO.Parent = ((clsStoreVO)cmbStore.SelectedItem).StoreId;


                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                msgText = "Record is successfully submitted!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                                //After Insertion Back to BackPanel and Setup Page
                                StoreId = 0;
                                SetupPage();
                                chkAllItemSelection.Visibility = Visibility.Collapsed;
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Save");
                                cboItemCategory.SelectedValue = (long)0;  //By Umesh to Refresh combo
                            }
                            else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 2)
                            {
                                msgText = "Record cannot be save because CODE already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 3)
                            {
                                msgText = "Record cannot be save because DESCRIPTION already exist!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 4)
                            {
                                msgText = "Parent Store Already Having A Qurantine Store";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
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
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            StoreId = 0;
            chkAllItemSelection.Visibility = Visibility.Collapsed;

            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                ModuleName = "PalashDynamics.Administration";
                Action = "PalashDynamics.Administration.frmInventoryConfiguration";
                UserControl rootPage = Application.Current.RootVisual as UserControl;

                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Inventory Configuration";

                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                IsCancel = true;
            }
            ClearUI();
        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                UIElement myData = null;
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
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            //  Validation();
            SetCommandButtonState("View");
            if (((clsStoreVO)grdStore.SelectedItem).Status == false)
            {
                cmdModify.IsEnabled = false;
            }
            StoreId = ((clsStoreVO)grdStore.SelectedItem).StoreId;
            grdBackPanel.DataContext = ((clsStoreVO)grdStore.SelectedItem).DeepCopy();
            var SelectedItem = ((List<MasterListItem>)CmbClinic.ItemsSource).First(s => s.ID == ((clsStoreVO)grdStore.SelectedItem).ClinicId);
            CmbClinic.SelectedItem = (MasterListItem)SelectedItem;
            //var SelectedParentStore = ((List<MasterListItem>)cmbStore.ItemsSource).First(s => s.ID == ((clsStoreVO)grdStore.SelectedItem).StoreId);
            //cmbStore.SelectedItem = (MasterListItem)SelectedParentStore;
            // FillStores(((clsStoreVO)grdStore.SelectedItem).ClinicId);


            chkAllItemSelection.Visibility = Visibility.Visible;
            try
            {
                objAnimation.Invoke(RotationType.Forward);
                clsGetStoreWithCategory(StoreId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdStore.SelectedItem != null)
            {
                //string msgTitle = "";
                //string msgText = "Are you sure you want to Update the Status ?";

                //MessageBoxControl.MessageBoxChildWindow msgWD =
                //    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                //msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                //{
                //    if (res == MessageBoxResult.Yes)
                //    {
                        try
                        {
                            clsAddUpdateStoreDetailsBizActionVO bizactionVO = new clsAddUpdateStoreDetailsBizActionVO();
                            bizactionVO.IsForStatusUpdate = true;
                            bizactionVO.StoreID = ((clsStoreVO)grdStore.SelectedItem).StoreId;
                            bizactionVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);

                            //clsStoreVO addNewStoreVO = new clsStoreVO();
                            //addNewStoreVO.StoreId = ((clsStoreVO)grdStore.SelectedItem).StoreId;
                            //addNewStoreVO.ClinicId = ((clsStoreVO)grdStore.SelectedItem).ClinicId;
                            //addNewStoreVO.Code = ((clsStoreVO)grdStore.SelectedItem).Code;
                            //addNewStoreVO.StoreName = ((clsStoreVO)grdStore.SelectedItem).StoreName;
                            //addNewStoreVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                            //addNewStoreVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                            //addNewStoreVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            //addNewStoreVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                            //addNewStoreVO.UpdatedDateTime = System.DateTime.Now;
                            //addNewStoreVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                            //bizactionVO.ItemMatserDetails.Add(addNewStoreVO);
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, args) =>
                            {
                                if (args.Error == null && args.Result != null)
                                {
                                    StoreId = 0;
                                    SetupPage();
                                    msgText = "Status Updated Successfully";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();

                                    //After Updation Back to BackPanel and Setup Page
                                    objAnimation.Invoke(RotationType.Backward);
                                    //cmdAdd.IsEnabled = true;
                                    //cmdModify.IsEnabled = false;
                                    SetCommandButtonState("Modify");
                                }
                            };
                            client.ProcessAsync(bizactionVO, new clsUserVO());
                            client.CloseAsync();
                        }
                        catch (Exception ex)
                        {
                        }
                //    }
                //    else
                //    {
                //        ((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked = false;
                //    }
                //};
                //msgWD.Show();
            }
        }

        void msgWinStatus_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateStoreDetailsBizActionVO bizactionVO = new clsAddUpdateStoreDetailsBizActionVO();
                clsStoreVO addNewStoreVO = new clsStoreVO();
                if (grdStore.SelectedItem != null)
                {
                    try
                    {
                        addNewStoreVO.StoreId = ((clsStoreVO)grdStore.SelectedItem).StoreId;
                        addNewStoreVO.ClinicId = ((clsStoreVO)grdStore.SelectedItem).ClinicId;
                        addNewStoreVO.Code = ((clsStoreVO)grdStore.SelectedItem).Code;
                        addNewStoreVO.StoreName = ((clsStoreVO)grdStore.SelectedItem).StoreName;
                        addNewStoreVO.Status = ((clsStoreVO)grdStore.SelectedItem).Status;
                        addNewStoreVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        addNewStoreVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        addNewStoreVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                        addNewStoreVO.UpdatedDateTime = System.DateTime.Now;
                        addNewStoreVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                        bizactionVO.ItemMatserDetails.Add(addNewStoreVO);
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {

                            if (args.Error == null && args.Result != null)
                            {

                                if (((clsAddUpdateStoreDetailsBizActionVO)args.Result).SuccessStatus == 1)
                                {

                                    StoreId = 0;
                                    // SetupPage();
                                    msgText = "Status Updated Successfully";


                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();

                                    //After Updation Back to BackPanel and Setup Page
                                    objAnimation.Invoke(RotationType.Backward);
                                    SetCommandButtonState("Modify");

                                    {

                                    }
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
            }
            MasterList = new PagedSortableCollectionView<clsStoreVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 10;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdStore.DataContext = MasterList;
            SetupPage();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsStoreVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 10;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdStore.DataContext = MasterList;
            SetupPage();
        }

        #endregion Events

        #region Lost Focus Event
        private void txtStoreCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
        }
        #endregion

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            // bool setCentral = true;
            if (grdStore.SelectedItem != null)
            {
                //SetupPage();

                clsGetStoreDetailsBizActionVO bizActionVO = new clsGetStoreDetailsBizActionVO();
                bizActionVO.SearchExpression = txtSearch.Text;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                getstoreinfo = new clsStoreVO();
                bizActionVO.ItemMatserDetails = new List<clsStoreVO>();
                try
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            bizActionVO.ItemMatserDetails = (((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails);

                            ///Setup Page Fill DataGrid
                            if (StoreId == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                            {
                                #region Added by Saily  for IsCentral Store
                                CentralStoreList.Clear();
                                CentralStoreList.TotalItemCount = (int)(((clsGetStoreDetailsBizActionVO)args.Result).TotalRows);
                                CentralStoreSet = false;
                                foreach (clsStoreVO item in bizActionVO.ItemMatserDetails)
                                {
                                    if (item.isCentralStore == true)
                                    {
                                        //CentralStoreList.Add(item);
                                        CentralStoreSet = true;
                                    }
                                }
                                if (CentralStoreSet == true)
                                {
                                    //check if central store already ste.
                                    if (((clsStoreVO)grdStore.SelectedItem).isCentralStore == true)
                                    {
                                        //if true then display error message
                                        msgText = "Cannot set more than one Central Store for each Clinic";
                                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWindow.Show();
                                        grdStore.ItemsSource = bizActionVO.ItemMatserDetails;
                                    }
                                    else
                                    {
                                        //else set central store
                                        CentralStoreChecked = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                                        SetCentralStore();
                                    }
                                }
                                else
                                {
                                    CentralStoreChecked = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                                    SetCentralStore();
                                    //set store
                                }
                                #endregion
                            }
                        }
                    };
                    client.ProcessAsync(bizActionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                }

            }
        }

        private void SetCentralStore()
        {

            clsUpdateCentralStoreDetailsBizActionVO bizAction = new clsUpdateCentralStoreDetailsBizActionVO();
            clsStoreVO addNewStoreVO = new clsStoreVO();
            addNewStoreVO.StoreId = ((clsStoreVO)grdStore.SelectedItem).StoreId;
            addNewStoreVO.ClinicId = ((clsStoreVO)grdStore.SelectedItem).ClinicId;
            addNewStoreVO.Code = ((clsStoreVO)grdStore.SelectedItem).Code;
            addNewStoreVO.StoreName = ((clsStoreVO)grdStore.SelectedItem).StoreName;
            addNewStoreVO.isCentralStore = CentralStoreChecked; //Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
            addNewStoreVO.Status = ((clsStoreVO)grdStore.SelectedItem).Status;
            addNewStoreVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            addNewStoreVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            addNewStoreVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
            addNewStoreVO.UpdatedDateTime = System.DateTime.Now;
            addNewStoreVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
            bizAction.StoreMasterDetails = addNewStoreVO;

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client1.ProcessCompleted += (s1, args1) =>
            {
                if (args1.Error == null && args1.Result != null)
                {
                    if (((clsUpdateCentralStoreDetailsBizActionVO)args1.Result).SuccessStatus == 1)
                    {
                        StoreId = 0;
                        //SetupPage();
                        msgText = "Central Store Set Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();

                        //After Updation Back to BackPanel and Setup Page
                        objAnimation.Invoke(RotationType.Backward);
                        //cmdAdd.IsEnabled = true;
                        //cmdModify.IsEnabled = false;
                        SetCommandButtonState("Modify");
                    }
                    else if (((clsUpdateCentralStoreDetailsBizActionVO)args1.Result).SuccessStatus == 2)
                    {
                        if (((clsStoreVO)grdStore.SelectedItem).isCentralStore == false)
                        {
                            msgText = "Central Store Cancelled";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();

                            //After Updation Back to BackPanel and Setup Page
                            objAnimation.Invoke(RotationType.Backward);
                            //cmdAdd.IsEnabled = true;
                            //cmdModify.IsEnabled = false;
                            SetCommandButtonState("Modify");
                        }
                    }
                }
            };
            client1.ProcessAsync(bizAction, new clsUserVO());
            client1.CloseAsync();
        }

        private void msgClosed(MessageBoxResult result)
        {
            if (((clsStoreVO)grdStore.SelectedItem).isCentralStore == true)
            {
                ((clsStoreVO)grdStore.SelectedItem).isCentralStore = false;
                //grdStore.ItemsSource=;
                //chkAllItemSelection.IsChecked = false;

            }
            else
                ((clsStoreVO)grdStore.SelectedItem).isCentralStore = true;
        }

        private void chkApplyCatWise_Click(object sender, RoutedEventArgs e)
        {
            lblCategory.Visibility = Visibility.Visible;
            cboItemCategory.Visibility = Visibility.Visible;

        }

        private void CmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (CmbClinic.SelectedItem != null)
            {
                long clinicId = ((MasterListItem)CmbClinic.SelectedItem).ID;
                FillStores(clinicId);
            }
        }

        private void chkAllItemSelection_Click(object sender, RoutedEventArgs e)
        {
            lblCategory.Visibility = Visibility.Collapsed;
            cboItemCategory.Visibility = Visibility.Collapsed;
        }

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MasterList = new PagedSortableCollectionView<clsStoreVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 10;
                this.dataGrid2Pager.DataContext = MasterList;
                this.grdStore.DataContext = MasterList;
                SetupPage();
            }
        }

        private void ChkIsQuarantineStore_Click(object sender, RoutedEventArgs e)
        {
            //if (((CheckBox)sender).IsChecked == true)
            //{
            //  List<clsStoreVO> List1=  objlistStore;
            //    cmbStore.ItemsSource = null;
            //    cmbStore.ItemsSource = objlistStore.Where(m => m.IsQuarantineStore = false)
            //                                .Select(m => m.StoreId != ((objlistStore.Where(x => x.IsQuarantineStore = true).SingleOrDefault().Parent)));

            //   //// cmbStore.ItemsSource = objlistStore.Where(m => m.StoreId != objlistStore.Select(n => n.StoreId !=))



            //    cmbStore.SelectedValue = (long)0;
            //}
            //else
            //{
            //    cmbStore.ItemsSource = null;
            //    cmbStore.ItemsSource = objlistStore.Where(m => m.IsQuarantineStore = false);
            //    cmbStore.SelectedValue = (long)0;
            //}



        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }


    }
}
