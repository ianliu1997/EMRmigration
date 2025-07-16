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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Animations;
using System.Collections.ObjectModel;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Inventory.Indent;
using PalashDynamics.ValueObjects.Master;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.Administration.UserRights;

namespace PalashDynamics.Pharmacy.Inventory
{
    public partial class FrmStoreIndent : UserControl, IInitiateCIMS
    {
        # region Variable Declarations

        clsIndentMasterVO SelectedIndent { get; set; }
        string SelectedCommand = "Cancel";
        bool IsDirectIndent = false; //***//
        bool IsInterClinicIndent = false;
        private SwivelAnimation objAnimation;
        MessageBoxControl.MessageBoxChildWindow mgbx = null;

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

        public ObservableCollection<clsIndentDetailVO> IndentAddedItems { get; set; }
        public ObservableCollection<clsIndentDetailVO> DeleteIndentAddedItems { get; set; }
        bool IsPageLoaded = false;
        WaitIndicator Indicatior = null;
        WaitIndicator indicator = new WaitIndicator();

        bool UserStoreAssigned = false;

        List<clsStoreVO> UserStores = new List<clsStoreVO>();

        int ClickedFlag1 = 0;
        clsUserVO userVO = new clsUserVO();

        List<MasterListItem> UOMLIstNew = new List<MasterListItem>();
        WaitIndicator Indicator;
        bool IsConvertToPR = false;
        string IsOpenFor = "";
        public bool IsFromChangAndApprove = false;
        public bool IsFromConvertToPR = false;

        public bool IsFromDashBoard = false;
        public clsIndentMasterVO IndentDetail { get; set; }
        //    List<clsIndentDetailVO> objList = IndentAddedItems.ToList<clsIndentDetailVO>();
        public List<clsIndentDetailVO> LstIndent = null;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        bool IsAgainstPatient { get; set; }
        # endregion

        #region Initiate/ Constuctor/ Loaded
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":

                    CmdForward.Visibility = Visibility.Collapsed;
                    CmdApprove.Visibility = Visibility.Collapsed;
                    IsOpenFor = "New";

                    break;
                case "Approve":

                    CmdForward.Visibility = Visibility.Visible;
                    CmdApprove.Visibility = Visibility.Visible;
                    CmdNew.Visibility = Visibility.Collapsed;
                    CmdSave.Visibility = Visibility.Collapsed;
                    //dgIndentList.Columns[0].Visibility = Visibility.Collapsed;
                    IsOpenFor = "Approve";

                    break;
            }
        }

        public FrmStoreIndent()
        {
            InitializeComponent();
            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID); //***//    
            FillUOM();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            Loaded += new RoutedEventHandler(UserControl_Loaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            grdStoreIndent.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(grdStoreIndent_CellEditEnded);
            dtpAddIndentDate.SelectedDate = DateTime.Now.Date;
            dtpAddIndentDate.IsEnabled = false;
            //======================================================
            // FillUOM();
            DeleteIndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {

                Indicatior = new WaitIndicator();
                Indicatior.Show();                            
                //FillUOM();
                this.dtpFromDate.SelectedDate = DateTime.Now;
                this.dtpToDate.SelectedDate = DateTime.Now;
                this.rdbAll.IsChecked = true;

                //FillStores(ClinicID);

                FillIndentList();
                FillStore();
                Indicatior.Close();
                IsPageLoaded = true;
                IndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
                grdStoreIndent.ItemsSource = IndentAddedItems;

                if (IsFromConvertToPR)
                {
                    ConvertToPR();
                }
            }

        }
        #endregion

        #region 'Paging'

        public PagedSortableCollectionView<clsIndentMasterVO> DataList { get; private set; }

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
            FillIndentList();
        }



        # endregion

        # region FillMethods

        private void FillStore()
        {
            UserStoreAssigned = false;
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            BizActionObj.ToStoreList = new List<clsStoreVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "-Select--", Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.Status == true
                                  select item;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                 select item;

                    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();
                    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                    UserStores.Clear();

                    foreach (var item in UserUnit)
                    {
                        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                        {
                            UserStores = item.UserUnitStore;
                        }
                    }
                    var resultnew = from item in UserStores select item;
                    if (UserStores != null && UserStores.Count > 0)
                    {
                        UserStoreAssigned = true;
                    }

                    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                    StoreListForClinic.Insert(0, Default);
                    if (IsInterClinicIndent == true) //***//
                    {
                        cmbAddToStoreName.ItemsSource = result1.ToList(); 
                        cmbToStoreName.ItemsSource = result1.ToList();  
                        cmbToStoreName.SelectedItem = result1.ToList()[0];
                    }
                    else
                    {
                        cmbAddToStoreName.ItemsSource = StoreListForClinic.ToList(); //result1.ToList();
                        cmbToStoreName.ItemsSource = StoreListForClinic.ToList(); //result1.ToList();
                        cmbToStoreName.SelectedItem = StoreListForClinic.ToList()[0];
                    }         

                    //if (result1.ToList().Count > 0)
                    //{
                    //    cmbToStoreName.SelectedItem = result1.ToList()[0];
                    //    cmbAddToStoreName.SelectedItem = result1.ToList()[0];
                    //}

                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, Default);



                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        cmbFromStoreName.ItemsSource = result1.ToList();
                        cmbAddFromStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                        cmbAddFromStoreName.ItemsSource = result1.ToList();
                        if (result1.ToList().Count > 0)
                        {
                            cmbFromStoreName.SelectedItem = (result1.ToList())[0];
                            cmbAddFromStoreName.SelectedItem = (result1.ToList())[0];
                        }
                    }
                    else
                    {
                        cmbAddFromStoreName.ItemsSource = NonQSAndUserDefinedStores.ToList(); //StoreListForClinic;
                        cmbFromStoreName.ItemsSource = NonQSAndUserDefinedStores.ToList(); //StoreListForClinic;
                        if (StoreListForClinic.Count > 0)
                        {
                            cmbAddFromStoreName.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            cmbFromStoreName.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                        }
                    }

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCentralPurchaseStore)
                    {
                        cmbAddToStoreName.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IndentStoreID;
                        cmbAddToStoreName.IsEnabled = false;
                    }
                    else
                        cmbAddToStoreName.IsEnabled = true;
                }
                //FillUOM();

                #region commented by Ashish Z. for getting only user wise store in FromStore.
                //    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;

                //    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };

                //    BizActionObj.ItemMatserDetails.Insert(0, Default);

                //    var result1 = from item in BizActionObj.ItemMatserDetails
                //                  where item.Status == true
                //                  select item;

                //    var result = from item in BizActionObj.ItemMatserDetails
                //                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                //                 select item;

                //    //List<clsStoreVO> UserStores = new List<clsStoreVO>();
                //    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                //    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                //    UserStores.Clear();

                //    foreach (var item in UserUnit)
                //    {
                //        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                //        {    //item.StoreDetails       
                //            UserStores = item.UserUnitStore;

                //            //if (item.IsDefault == true)
                //            //{
                //            //    //item.UserUnitStore.Insert(0, Default);
                //            //    UserStores = item.UserUnitStore;
                //            //    break;
                //            //}
                //            //else if (item.IsDefault == false)
                //            //{
                //            //    //item.UserUnitStore.Insert(0, Default);
                //            //    UserStores = item.UserUnitStore;
                //            //    break;
                //            //}
                //        }
                //    }
                //    //UserStores.Insert(0, Default);
                //    var resultnew = from item in UserStores select item;
                //    if (UserStores != null && UserStores.Count > 0)
                //    {
                //        UserStoreAssigned = true;
                //    }

                //    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                //    StoreListForClinic.Insert(0, Default);

                //    //List<clsStoreVO> StoreListForClinicUser = (List<clsStoreVO>)resultnew.ToList(); //(List<clsStoreVO>)resultnew.ToList(); 
                //    // StoreListForClinicUser.Insert(0, Default);
                //    // cmbToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                //    cmbAddToStoreName.ItemsSource = result1.ToList();

                //    cmbToStoreName.ItemsSource = result1.ToList();
                //    if (result1.ToList().Count > 0)
                //    {
                //        cmbToStoreName.SelectedItem = result1.ToList()[0];
                //        cmbAddToStoreName.SelectedItem = result1.ToList()[0];
                //    }
                //    //cmbAddToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;

                //    //if (StoreListForClinicUser.Count > 0)
                //    //    cmbAddFromStoreName.SelectedItem = StoreListForClinicUser[0]; //StoreListForClinic[0];
                //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                //    {
                //        cmbFromStoreName.ItemsSource = result1.ToList();
                //        cmbAddFromStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                //        cmbAddFromStoreName.ItemsSource = result1.ToList();
                //        if (result1.ToList().Count > 0)
                //        {
                //            cmbFromStoreName.SelectedItem = (result1.ToList())[0];
                //            cmbAddFromStoreName.SelectedItem = (result1.ToList())[0];
                //        }
                //    }

                //    else
                //    {
                //        //Commented By Harish on 17 Apr
                //        //cmbFromStoreName.ItemsSource = (List<clsStoreVO>)result.ToList();
                //        //cmbAddFromStoreName.ItemsSource = (List<clsStoreVO>)result.ToList();

                //        //cmbAddFromStoreName.ItemsSource = StoreListForClinicUser; //StoreListForClinic;
                //        // Rohit
                //        //cmbAddFromStoreName.ItemsSource = result1.ToList();
                //        cmbAddFromStoreName.ItemsSource = StoreListForClinic;
                //        cmbFromStoreName.ItemsSource = StoreListForClinic;
                //        if (StoreListForClinic.Count > 0)
                //        {
                //            cmbAddFromStoreName.SelectedItem = StoreListForClinic[0];
                //            cmbFromStoreName.SelectedItem = StoreListForClinic[0];
                //        }
                //        // End
                //    }
                //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCentralPurchaseStore)
                //    {
                //        cmbAddToStoreName.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IndentStoreID;
                //        cmbAddToStoreName.IsEnabled = false;
                //    }
                //    else
                //        cmbAddToStoreName.IsEnabled = true;
                //}
                //FillUOM();
                #endregion
            };

            client.CloseAsync();

        }

        private void NewStoreFill()
        {
            UserStoreAssigned = false;
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            BizActionObj.ToStoreList = new List<clsStoreVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };

                    BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);

                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.Status == true
                                  select item;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    List<clsStoreVO> UserStores = new List<clsStoreVO>();
                    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                    foreach (var item in UserUnit)
                    {
                        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                        {
                            UserStores = item.UserUnitStore;
                            //if (item.IsDefault == true)
                            //{
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                            //else if (item.IsDefault == false)
                            //{
                            //    //item.UserUnitStore.Insert(0, Default);
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                        }
                    }

                    var resultnew = from item in UserStores select item;
                    if (UserStores != null && UserStores.Count > 0)
                    {
                        UserStoreAssigned = true;
                    }
                    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                    StoreListForClinic.Insert(0, Default);
                    List<clsStoreVO> StoreListForClinicUser = (List<clsStoreVO>)resultnew.ToList(); //(List<clsStoreVO>)resultnew.ToList(); 

                    if (IsInterClinicIndent == true) //***//
                    {
                        cmbAddToStoreName.ItemsSource = result1.ToList(); ; //result1.ToList();
                        cmbAddToStoreName.SelectedItem = result1.ToList()[0];
                    }
                    else
                    {
                        cmbAddToStoreName.ItemsSource = StoreListForClinic.ToList(); //result1.ToList();
                        cmbAddToStoreName.SelectedItem = StoreListForClinic.ToList()[0];
                    }

                    //cmbToStoreName.ItemsSource = result1.ToList();
                    //if (result1.ToList().Count > 0)
                    //{
                    //    //cmbToStoreName.SelectedItem = result1.ToList()[0];
                    //    cmbAddToStoreName.SelectedItem = result1.ToList()[0];
                    //}

                    //if (StoreListForClinicUser.Count > 0)
                    //    cmbAddFromStoreName.SelectedItem = StoreListForClinicUser[0]; //StoreListForClinic[0];

                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;
                    NonQSAndUserDefinedStores.ToList().Insert(0, Default);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        //cmbFromStoreName.ItemsSource = result1.ToList();
                        cmbAddFromStoreName.ItemsSource = result1.ToList();
                        if (result1.ToList().Count > 0)
                        {
                            //cmbFromStoreName.SelectedItem = (result1.ToList())[0];
                            cmbAddFromStoreName.SelectedItem = (result1.ToList())[0];
                        }
                    }
                    else
                    {
                        cmbAddFromStoreName.ItemsSource = NonQSAndUserDefinedStores.ToList();
                        //cmbFromStoreName.ItemsSource = StoreListForClinic;
                        if (StoreListForClinic.Count > 0)
                        {
                            cmbAddFromStoreName.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            //cmbFromStoreName.SelectedItem = StoreListForClinic[0];
                        }
                    }

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCentralPurchaseStore)
                    {
                        cmbAddToStoreName.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IndentStoreID;
                        cmbAddToStoreName.IsEnabled = false;
                    }
                    else
                        cmbAddToStoreName.IsEnabled = true;
                }
            };

            client.CloseAsync();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        private void FillIndentList()
        {
            try
            {
                indicator.Show();
                dgIndentList.ItemsSource = null;
                dgIndentDetailList.ItemsSource = null;

                clsGetIndenListBizActionVO BizAction = new clsGetIndenListBizActionVO();

                if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                {
                    if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
                    {
                        dtpFromDate.SetValidation("From Date should be less than To Date");
                        dtpFromDate.RaiseValidationError();
                        dtpFromDate.Focus();
                        indicator.Close();
                        return;
                    }
                    else
                    {
                        dtpFromDate.ClearValidationError();
                    }

                }
                if (this.dtpFromDate.SelectedDate != null && this.dtpToDate.SelectedDate != null)
                {
                    BizAction.FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
                    BizAction.ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;
                }
                if (this.cmbFromStoreName.SelectedItem != null)
                    BizAction.FromStoreID = ((clsStoreVO)this.cmbFromStoreName.SelectedItem).StoreId;
                if (this.cmbToStoreName.SelectedItem != null)
                    BizAction.ToStoreID = ((clsStoreVO)this.cmbToStoreName.SelectedItem).StoreId;

                BizAction.IndentNO = txtIndentNumber.Text;

                //Set Paging Variables
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                if ((bool)rdbAll.IsChecked)
                {
                    BizAction.CheckStatusType = false;
                }
                else if ((bool)rdbPending.IsChecked)
                {
                    BizAction.CheckStatusType = true;
                    //BizAction.IndentStatus = 3;
                    BizAction.IndentStatus = 1;
                    BizAction.IsFreezed = true;
                    BizAction.isApproved = true;
                }
                else if ((bool)rdbCompleted.IsChecked)
                {
                    BizAction.CheckStatusType = true;
                    BizAction.IndentStatus = 4;
                }
                else if ((bool)rdbApproved.IsChecked)
                {
                    BizAction.isApproved = true;
                }
                else if ((bool)rdbForwarded.IsChecked)
                {
                    BizAction.isFrowrded = true;
                }
                else if ((bool)rdbCancelled.IsChecked)
                {
                    BizAction.isCancelled = true;
                }
                else if ((bool)rdbFreezed.IsChecked)
                {
                    BizAction.IsFreezed = true;
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId; //0;
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

                //by Anjali............................
                //BizAction.isIndent = true;
                BizAction.isIndent = 1;
                //...................................

                //***//-----
                if (txtMrno.Text.Trim() != "")
                    BizAction.MRNo = txtMrno.Text.Trim();
                else
                    BizAction.MRNo = "";


                if (txtName.Text.Trim() != "")
                    BizAction.PatientName = txtName.Text.Trim();
                else
                    BizAction.PatientName = "";
                //-------

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            //dgIndentList.ItemsSource = ((clsGetIndenListBizActionVO)arg.Result).IndentList;

                            clsGetIndenListBizActionVO result = arg.Result as clsGetIndenListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;

                            if (result.IndentList != null)
                            {
                                DataList.Clear();
                                foreach (var item in result.IndentList)
                                {
                                    DataList.Add(item);
                                }

                                dgIndentList.ItemsSource = null;
                                dgIndentList.ItemsSource = DataList;

                                DataPager.Source = null;
                                DataPager.PageSize = BizAction.MaximumRows;
                                DataPager.Source = DataList;
                            }
                            txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();
                        }
                    }
                    indicator.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
            catch (Exception ex)
            {
                indicator.Close();
            }

        }


        private void FillUOM()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitOfMeasure;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        UOMLIstNew = new List<MasterListItem>();
                        UOMLIstNew.Add(new MasterListItem(0, "- Select -"));
                        UOMLIstNew.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        //FillIndentList();
                    }
                    if (IsFromDashBoard == true)
                    {
                        foreach (var item in LstIndent.ToList())
                        {
                            IEnumerator<clsIndentDetailVO> list = (IEnumerator<clsIndentDetailVO>)LstIndent.GetEnumerator();
                            bool IsExist = false;

                            while (list.MoveNext())
                            {
                                if (item.ID == ((clsIndentDetailVO)list.Current).ItemID)
                                {
                                    IsExist = true;
                                    break;
                                }
                            }
                            if (IsExist == false)
                                IndentAddedItems.Add(new clsIndentDetailVO() { ItemID = item.ItemID, ItemName = item.ItemName, RequiredQuantity = item.RequiredQuantity, ConversionFactor = Convert.ToSingle(item.ConversionFactor), UOMList = UOMLIstNew, SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID)), UOM = item.UOM, SUOM = item.SUOM, SUOMID = item.SUOMID });
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Item is alredy added!", "Please add another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        grdStoreIndent.ItemsSource = IndentAddedItems;
                        objAnimation.Invoke(RotationType.Forward);
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

        # endregion

        #region GridEvent
        void grdStoreIndent_CellEditEnded(object Sender, DataGridCellEditEndedEventArgs e)
        {
            if (grdStoreIndent.SelectedItem != null)
            {
                clsIndentDetailVO obj = new clsIndentDetailVO();
                obj = (clsIndentDetailVO)grdStoreIndent.SelectedItem;

                if (e.Column.Header.ToString().Equals("Indent Quantity")) // for Quantity
                {

                    obj.SingleQuantity = Convert.ToSingle(System.Math.Round(obj.SingleQuantity, 1));
                    if (((int)obj.SingleQuantity).ToString().Length > 5)
                    {
                        obj.SingleQuantity = 1;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Quantity Length Should Not Be Greater Than 5 Digits.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        return;
                    }
                    //if (Convert.ToDouble(grdStoreIndent.Columns[1].GetCellContent(obj.RequiredQuantity)) == 0)
                    //{
                    //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    mgbx.Show();
                    //    obj.RequiredQuantity = 1;
                    //    return;
                    //}
                    //obj.SingleQuantity = Convert.ToSingle(System.Math.Round(obj.SingleQuantity, 1));

                    if (obj.SelectedUOM != null && obj.SelectedUOM.ID == obj.BaseUOMID && (obj.SingleQuantity % 1) != 0)
                    {
                        obj.SingleQuantity = 0;
                        string msgText = "Indent Quantity Cannot Be In Fraction for Base UOM";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        return;
                    }

                    if (obj.SingleQuantity < 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Quantity Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        //obj.SingleQuantity = 1;
                        return;
                    }

                    if (obj.SelectedUOM != null && obj.SelectedUOM.ID > 0)
                    {
                        obj.StockingQuantity = Convert.ToDouble(obj.SingleQuantity) * obj.StockCF;  // StockCF
                        obj.RequiredQuantity = Convert.ToSingle(obj.SingleQuantity) * obj.ConversionFactor;  //BaseCF
                    }
                    else
                    {
                        CalculateConversionFactorCentral(obj.SelectedUOM.ID, obj.SUOMID);
                    }
                }
            }

            #region Commented by Ashish Z.
            //if (grdStoreIndent.SelectedItem != null)
            //{
            //    clsIndentDetailVO obj = new clsIndentDetailVO();
            //    obj = (clsIndentDetailVO)grdStoreIndent.SelectedItem;
            //    //if (Convert.ToDouble(grdStoreIndent.Columns[1].GetCellContent(obj.RequiredQuantity)) == 0)
            //    //{
            //    //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    //    mgbx.Show();
            //    //    obj.RequiredQuantity = 1;
            //    //    return;
            //    //}

            //    if (e.Column.DisplayIndex == 1) // for Quantity
            //    {
            //        //FillUOMConversions(((clsIndentDetailVO)grdStoreIndent.SelectedItem).ItemID);
            //        if (obj.SingleQuantity == 0)   //if (obj.RequiredQuantity == 0)
            //        {
            //            //MessageBoxControl.MessageBoxChildWindow mgbx = null;

            //            mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //            mgbx.Show();
            //            obj.SingleQuantity = 1;   //obj.RequiredQuantity = 1;

            //            if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.Description != ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOM)
            //            {
            //                ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor);
            //            }
            //            else
            //            {
            //                ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity);
            //            }

            //            return;
            //        }

            //        if (obj.SingleQuantity < 0)  //if (obj.RequiredQuantity < 0)
            //            //MessageBoxControl.MessageBoxChildWindow mgbx = null;

            //            mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //            mgbx.Show();
            //            obj.SingleQuantity = 1;   //obj.RequiredQuantity = 1;

            //            if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.Description != ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOM)
            //            {
            //                ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor);
            //            }
            //            else
            //            {
            //                ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity);
            //            }

            //            return;
            //        }

            //        if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.Description != ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOM)
            //        {
            //            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor); //BaseConversionFactor
            //        }
            //        else
            //        {
            //            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity);
            //        }


            //        if (SelectedCommand == "View")
            //        {
            //            if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID > 0)
            //            {
            //                ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockingQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockCF;
            //                ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor;
            //            }
            //            else
            //            {
            //                if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID > 0)
            //                {
            //                    CalculateConversionFactorCentral(((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID > 0)
            //            {
            //                ////((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
            //                //////objConversion = CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID);
            //                //CalculateConversionFactor(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);
            //                // Function Parameters
            //                // FromUOMID - Transaction UOM
            //                // ToUOMID - Stocking UOM
            //                CalculateConversionFactorCentral(((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
            //            }
            //            else
            //            {
            //                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = 0;
            //                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity = 0;

            //                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor = 0;
            //                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseConversionFactor = 0;
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        private void dgIndentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //* Added by - Ajit Jadhav
            //* Added Date - 7/9/2016
            //* Comments - Check SelectedItem and the fill indent details list
            if (dgIndentList.SelectedItem != null)
            {
                FillIndentDetailsList();
            }
            //if() By Umesh

            //clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
            //BizAction.IndentDetailList = new List<clsIndentDetailVO>();

            //if (dgIndentList.SelectedItem != null)
            //{
            //    if (((clsIndentMasterVO)dgIndentList.SelectedItem).IsApproved == true && IsOpenFor == "Approve")
            //    {
            //        CmdApprove.IsEnabled = false;
            //    }
            //    else if (((clsIndentMasterVO)dgIndentList.SelectedItem).IsApproved == false && IsOpenFor == "Approve")
            //    {
            //        CmdApprove.IsEnabled = true;
            //    }

            //    SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
            //    //   clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
            //    BizAction.IndentID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).ID);
            //     //  BizAction.IndentDetailList = new List<clsIndentDetailVO>();
            //    BizAction.UnitID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID);
            //}

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null)
            //    {
            //        if (arg.Result != null)
            //        {
            //            dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
            //            SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
            //        }
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        //* Added by - Ajit Jadhav
        //* Added Date - 7/9/2016
        private void FillIndentDetailsList()
        {
            clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
            BizAction.IndentDetailList = new List<clsIndentDetailVO>();

            if (dgIndentList.SelectedItem != null)
            {
                if (((clsIndentMasterVO)dgIndentList.SelectedItem).IsApproved == true && IsOpenFor == "Approve")
                {
                    CmdApprove.IsEnabled = false;
                }
                else if (((clsIndentMasterVO)dgIndentList.SelectedItem).IsApproved == false && IsOpenFor == "Approve")
                {
                    CmdApprove.IsEnabled = true;
                }

                SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
                //   clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
                BizAction.IndentID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).ID);
                //  BizAction.IndentDetailList = new List<clsIndentDetailVO>();
                BizAction.UnitID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID);
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                        SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        #endregion

        # region Validations

        private bool Validate()
        {

            if(IsAgainstPatient == true)
            {
                if (((clsStoreVO)cmbAddFromStoreName.SelectedItem).ClinicId != ((clsStoreVO)cmbAddToStoreName.SelectedItem).ClinicId)
                {
                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Cross Indent Should be not allowed against Patient Indent.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;                   
                }
            }

            // Changes done by Harish 17 Apr

            if (cmbAddFromStoreName.SelectedItem == null || ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            if (SelectedCommand == "View" && (SelectedIndent.IndentStatus == InventoryIndentStatus.Rejected || SelectedIndent.IndentStatus == InventoryIndentStatus.Cancelled || SelectedIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted))
            {
                string strMsg = string.Empty;
                if (SelectedIndent.IndentStatus == InventoryIndentStatus.Rejected) strMsg = "Indent is already Rejected, You cannot Modify it";
                else if (SelectedIndent.IndentStatus == InventoryIndentStatus.Cancelled) strMsg = "Indent is already Cancelled, You cannot Modify it";
                else if (SelectedIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted) strMsg = "Indent is forcefully closed, You cannot Modify it";

                if (!string.IsNullOrEmpty(strMsg))
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
                return false;
            }

            //else if ((long)cmbAddFromStoreName.SelectedValue == (long)0)
            //{ 
            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    return false;
            //}
            // Changes done by Harish 17 Apr
            if (cmbAddToStoreName.SelectedItem == null || ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "To Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            if (((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId == ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Same From  & To Store!Please select different stores", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }
            //else if ((long)cmbAddToStoreName.SelectedValue == (long)0)
            //{
            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    return false;
            //} 
            if ((DateTime?)dtpAddDueDate.SelectedDate == null)
            {
                dtpAddDueDate.SetValidation("Please Select Expected Delivery Date");
                dtpAddDueDate.Focus();
                dtpAddDueDate.RaiseValidationError();
                //mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Delivery Date can not be Empty. Please Select a Delivery Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //mgbx.Show();
                return false;
            }
            else
                dtpAddDueDate.ClearValidationError();
            if ((DateTime?)dtpAddDueDate.SelectedDate < DateTime.Now.Date)
            {
                dtpAddDueDate.SetValidation("Expected Delivery Date can not be Less Than Today's Date.");
                dtpAddDueDate.Focus();
                dtpAddDueDate.RaiseValidationError();
                //mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Delivery Date can not be Less Than Today's Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //mgbx.Show();
                return false;
            }
            else
                dtpAddDueDate.ClearValidationError();

            if ((DateTime?)dtpAddDueDate.SelectedDate > DateTime.Now.Date.AddMonths(6))
            {
                dtpAddDueDate.SetValidation("Expected delivery date should be within 6 months from Today's Date.");
                dtpAddDueDate.Focus();
                dtpAddDueDate.RaiseValidationError();
                //mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Delivery Date can not be Less Than Today's Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //mgbx.Show();
                return false;
            }
            else
                dtpAddDueDate.ClearValidationError();

            if ((DateTime?)dtpAddIndentDate.SelectedDate == null)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Date can not be Empty. Please Select a Indent Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }
            if (IndentAddedItems.Count == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Items List can not be Empty. Please Select Indent Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            //Added By Pallavi
            List<clsIndentDetailVO> objList = IndentAddedItems.ToList<clsIndentDetailVO>();
            if (objList != null && objList.Count > 0)
            {
                foreach (var item in objList)
                {
                    float fltOne = 1;
                    float fltZero = 0;
                    float Infinity = fltOne / fltZero;
                    float NaN = fltZero / fltZero;
                    if (item.StockCF <= 0 || item.StockCF == Infinity || item.StockCF == NaN)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        return false;
                    }
                    else if (item.ConversionFactor <= 0 || item.ConversionFactor == Infinity || item.ConversionFactor == NaN)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        return false;
                    }

                    if (item.SelectedUOM != null && item.SelectedUOM.ID == item.BaseUOMID && (item.SingleQuantity % 1) != 0)
                    {
                        item.SingleQuantity = 0;
                        string msgText = "Indent Quantity Cannot Be In Fraction for Base UOM";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        return false;
                    }
                    if (item.SingleQuantity == 0)   //if (item.RequiredQuantity == 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Quantity In The List Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        //item.SingleQuantity = 1;   //item.RequiredQuantity = 1;

                        return false;
                    }
                    if (item.SingleQuantity < 0)  //if (item.RequiredQuantity <= 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Quantity Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        //item.SingleQuantity = 1;   //item.RequiredQuantity = 0;
                        return false;
                    }
                    if (item.SelectedUOM.ID == 0)   //if (item.RequiredQuantity == 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select UOM for '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        return false;
                    }

                    //if (!item.SingleQuantity.ToString().IsItValidQuantity())
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW1;
                    //    msgW1 = new MessageBoxControl.MessageBoxChildWindow("Validation Error", "Please Enter Valid Quantity For Item " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgW1.Show();
                    //    item.SingleQuantity = 1;
                    //    return false;
                    //    break;
                    //}

                }

            }
            return true;

        }

        # endregion

        #region Private Methods
        public void ConvertToPR()
        {
            Indicator = new WaitIndicator();
            Indicator.Show();

            try
            {
                //if (dgIndentList.SelectedItem != null && ((clsIndentMasterVO)dgIndentList.SelectedItem).IsFreezed == true && ((clsIndentMasterVO)dgIndentList.SelectedItem).IsApproved == true && ((clsIndentMasterVO)dgIndentList.SelectedItem).IsConvertToPR == false)
                //{
                IsConvertToPR = true;

                SelectedIndent = IndentDetail;
                clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
                BizAction.IndentID = Convert.ToInt64(IndentDetail.ID);
                BizAction.UnitID = Convert.ToInt64(IndentDetail.UnitID);
                BizAction.IndentDetailList = new List<clsIndentDetailVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    Indicator.Close();
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            //acTextBox = new TextBox();
                            dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                            SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;

                            this.DataContext = new clsIndentDetailVO();
                            IndentAddedItems.Clear();

                            foreach (var item in SelectedIndent.IndentDetailsList)
                            {
                                if (UOMLIstNew.Count > 0)
                                {
                                    item.UOMList = UOMLIstNew;
                                    item.SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID));
                                    item.UOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID)).Description;

                                    item.SUOM = UOMLIstNew.Single(u => u.ID.Equals(item.SUOMID)).Description;
                                }
                                IndentAddedItems.Add(item);
                                //cmbAddFromStoreName.SelectedItem = item.fr;
                            }

                            if (cmbAddFromStoreName.ItemsSource != null && cmbAddToStoreName.ItemsSource != null)
                            {
                                var results = from r in ((List<clsStoreVO>)cmbAddFromStoreName.ItemsSource)
                                              where r.StoreId == this.SelectedIndent.FromStoreID  //r.ClinicId == this.SelectedIndent.IndentUnitID &&
                                              //&& r.ClinicId == this.SelectedIndent.UnitID
                                              select r;

                                //cmbAddFromStoreName.SelectedItem  = SelectedIndent.IndentDetailsList.
                                foreach (clsStoreVO item in results)
                                {
                                    cmbAddFromStoreName.SelectedItem = item;
                                }

                                var results1 = from r in ((List<clsStoreVO>)cmbAddToStoreName.ItemsSource)
                                               where r.StoreId == this.SelectedIndent.ToStoreID
                                               select r;

                                foreach (clsStoreVO item in results1)
                                {
                                    cmbAddToStoreName.SelectedItem = item;
                                }
                            }
                            //this.cmbAddFromStoreName.SelectedValue = SelectedIndent.FromStoreID;
                            //this.cmbAddToStoreName.SelectedValue = SelectedIndent.ToStoreID;
                            this.dtpAddIndentDate.SelectedDate = SelectedIndent.Date;
                            this.dtpAddDueDate.SelectedDate = SelectedIndent.DueDate;
                            //this.chkIsAuthorized.IsChecked = SelectedIndent.IsAuthorized;
                            //this.dtpAuthorizationDate.SelectedDate = SelectedIndent.AuthorizationDate;
                            this.IndentNumber.Text = SelectedIndent.IndentNumber;
                            this.txtRemark.Text = SelectedIndent.Remark;
                            this.chkFreezIndent.IsChecked = SelectedIndent.IsFreezed;
                            ViewIndentToPR();
                        }
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //}
                //else
                //{
                //    string msg = "";

                //    if (IndentDetail != null && IndentDetail.IsConvertToPR == true)
                //    {
                //        msg = "Selected Indent is already converted to Purchase Requisition";
                //    }
                //    else if (IndentDetail != null && IndentDetail.IsApproved == false)
                //    {
                //        msg = "Selected Indent is not Approved, you can't convert it to Purchase Requisition";
                //    }
                //    else if (IndentDetail != null && IndentDetail.IsFreezed == false)
                //    {
                //        msg = "Selected Indent is not freezed, you can't convert it to Purchase Requisition";
                //    }

                //    if (IndentDetail == null)
                //    {
                //        msg = "Please Select Indent to convert into Purchase Requisition";
                //    }

                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                new MessageBoxControl.MessageBoxChildWindow("Palash", msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW1.Show();

                // }
            }
            catch (Exception ex)
            {
                Indicator.Close();
                throw;
            }
            finally
            {

            }
        }

        public void ViewIndentToPR()
        {
            CmdPrint.IsEnabled = false;
            CmdNew.IsEnabled = false;
            CmdSave.IsEnabled = true;
            CmdCancel.IsEnabled = true;
            CmdForward.IsEnabled = false;
            CmdApprove.IsEnabled = false;
            SelectedCommand = "IndentToPR";
            objAnimation.Invoke(RotationType.Forward);

            chkFreezIndent.IsEnabled = false;
            grdStoreIndent.IsEnabled = false;
            CmdModify.IsEnabled = false;
            lnkAddItems.IsEnabled = false;

            BdrItemSearch.Visibility = System.Windows.Visibility.Collapsed;

            cmbAddFromStoreName.IsEnabled = false;
            cmbAddToStoreName.IsEnabled = true;

            CmdConvertToPR.IsEnabled = false;
        }

        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveIndent();
            }
            else if (result == MessageBoxResult.No)
                ClickedFlag1 = 0;
        }

        private void SaveIndent()
        {
            clsAddIndentBizActionVO BizAction = new clsAddIndentBizActionVO();

            BizAction.objIndent = new clsIndentMasterVO();
            BizAction.objIndent.Date = dtpAddIndentDate.SelectedDate; //Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
            BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
            BizAction.objIndent.IndentNumber = null;
            BizAction.objIndent.TransactionMovementID = 0;
            BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
            BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
            BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);
            BizAction.objIndent.IndentCreatedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
            //BizAction.objIndent.IsAuthorized = false;// chkIsAuthorized.IsChecked;
            //BizAction.objIndent.AuthorizedByID = 0;
            //BizAction.objIndent.AuthorizationDate =null; // dtpAuthorizationDate.SelectedDate;
            BizAction.objIndent.Remark = txtRemark.Text;
            BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
            BizAction.objIndent.Status = true;

            BizAction.objIndent.IsChangeAndApprove = false;

            BizAction.objIndent.IsFreezed = (bool)chkFreezIndent.IsChecked;
            BizAction.objIndent.IsForwarded = false;
            BizAction.objIndent.IsApproved = false;
            //BizAction.objIndent.IsIndent = true;

            if (this.IsConvertToPR == false)
            {
                BizAction.objIndent.InventoryIndentType = InventoryIndentType.Indent;
                BizAction.IsConvertToPR = false;
                //By Anjali..........................................
                BizAction.objIndent.IsIndent = 1;
                //BizAction.objIndent.IsIndent = true;
                //....................................................
            }

            if (this.IsConvertToPR == true)
            {
                BizAction.objIndent.ConvertToPRID = Convert.ToInt64(IndentDetail.ID); //Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).ID);

                BizAction.objIndent.InventoryIndentType = InventoryIndentType.PurchaseRequisition;
                BizAction.IsConvertToPR = true;
                BizAction.objIndent.IsApproved = true;
                //By Anjali..........................................
                //BizAction.objIndent.IsIndent = false;
                BizAction.objIndent.IsIndent = 0;
                //...................................................
            }

            BizAction.objIndentDetailList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();
            BizAction.objIndentDetailList.Select(indentItems => { indentItems.IndentUnitID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId; return indentItems; }).ToList();

            if (BizAction.IsConvertToPR == true)
            {
                foreach (clsIndentDetailVO item in BizAction.objIndentDetailList)
                {
                    if (item.SelectedUOM.Description == item.SUOM)
                    {
                        long lowerQty = 0;
                        long upperQty = 0;

                        //LowerRentDur = ((long)(ActualRentDuration / RentDurSlot)) * RentDurSlot;
                        //UpperRentDur = (LowerRentDur + RentDurSlot);

                        lowerQty = (long)(((long)(item.RequiredQuantity / item.ConversionFactor)) * item.ConversionFactor);

                        if (lowerQty == (long)(item.RequiredQuantity))
                            upperQty = lowerQty;
                        else
                            upperQty = (lowerQty + (long)item.ConversionFactor);

                        item.SingleQuantity = upperQty / item.ConversionFactor;
                        item.RequiredQuantity = upperQty;
                    }
                    else
                    {
                        //item.RequiredQuantity = item.SingleQuantity;
                    }
                }
            }

            if (IsAgainstPatient == true && ((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                BizAction.objIndent.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.objIndent.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                BizAction.objIndent.IsAgainstPatient = IsAgainstPatient;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag1 = 0;
                if (arg.Error == null && ((clsAddIndentBizActionVO)arg.Result).objIndent != null)
                {
                    if (arg.Result != null)
                    {
                        CmdPrint.IsEnabled = true;
                        CmdNew.IsEnabled = true;
                        CmdSave.IsEnabled = false;
                        CmdCancel.IsEnabled = false;
                        CmdForward.IsEnabled = true;
                        CmdModify.IsEnabled = false;
                        CmdApprove.IsEnabled = true;
                        SelectedCommand = "Save";
                        cmbAddFromStoreName.SelectedValue = (long)0;
                        cmbAddToStoreName.SelectedValue = (long)0;

                        chkFreezIndent.IsEnabled = true;
                        grdStoreIndent.IsEnabled = true;
                        CmdConvertToPR.IsEnabled = true;
                        lnkAddItems.IsEnabled = true;

                        string txtMsg = "";

                        if (this.IsConvertToPR == false)
                        {
                            FillIndentList();
                            objAnimation.Invoke(RotationType.Backward);
                            txtMsg = "Indent Saved Successfully With Indent Number ";
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", txtMsg + ((clsAddIndentBizActionVO)arg.Result).objIndent.IndentNumber, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbox.Show();
                            this.IsConvertToPR = false;
                        }
                        else if (this.IsConvertToPR == true)
                        {
                            ModuleName = "PalashDynamics.Pharmacy";
                            Action = "PalashDynamics.Pharmacy.StoreIndent";
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            WebClient c2 = new WebClient();
                            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                            txtMsg = "Indent Converted To Purchase Requisition Successfully With PR Number ";
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", txtMsg + ((clsAddIndentBizActionVO)arg.Result).objIndent.IndentNumber, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbox.Show();
                        }

                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while saving indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void ViewIndent()
        {
            CmdPrint.IsEnabled = false;
            CmdNew.IsEnabled = false;
            CmdSave.IsEnabled = false;
            CmdCancel.IsEnabled = true;
            CmdForward.IsEnabled = false;
            CmdApprove.IsEnabled = false;

            CmdConvertToPR.IsEnabled = false;

            SelectedCommand = "View";
            objAnimation.Invoke(RotationType.Forward);

            if (SelectedIndent.IsFreezed)
            {
                CmdModify.IsEnabled = false;
                chkFreezIndent.IsEnabled = false;
                lnkAddItems.IsEnabled = false;
            }
            else
            {
                chkFreezIndent.IsEnabled = true;
                grdStoreIndent.IsEnabled = true;
                CmdModify.IsEnabled = true;
                lnkAddItems.IsEnabled = true;
            }
            cmbAddToStoreName.IsEnabled = false;
        }

        private void ViewIndentForApprove()
        {
            CmdPrint.IsEnabled = false;
            CmdNew.IsEnabled = false;
            CmdSave.IsEnabled = false;
            CmdCancel.IsEnabled = true;
            CmdForward.IsEnabled = false;
            CmdApprove.IsEnabled = false;

            CmdConvertToPR.IsEnabled = false;

            SelectedCommand = "View";
            objAnimation.Invoke(RotationType.Forward);

            if (SelectedIndent.IsApproved)  //if (SelectedIndent.IsFreezed)
            {

                CmdModify.IsEnabled = false;
                chkFreezIndent.IsEnabled = false;
                lnkAddItems.IsEnabled = false;
                BdrItemSearch.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                chkFreezIndent.IsEnabled = true;
                grdStoreIndent.IsEnabled = true;
                CmdModify.IsEnabled = true;
                lnkAddItems.IsEnabled = true;
            }
        }

        private void UpdateForChangeAndApprove()
        {
            string msgTitle = "";
            string msgDesc = "";

            if ((bool)chkFreezIndent.IsChecked)
            {

                msgTitle = "Freeze and Update Indent";
                msgDesc = "Do You want to Freeze and Update the Indent?";
            }
            else
            {
                msgTitle = "Update Indent";
                msgDesc = "Do You want to Update the Indent?";
            }

            MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", msgDesc, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                    BizAction.IsForChangeAndApproveIndent = true;
                    BizAction.objIndent = new clsIndentMasterVO();
                    BizAction.objIndent.ID = SelectedIndent.ID;
                    BizAction.objIndent.Date = Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
                    BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
                    BizAction.objIndent.TransactionMovementID = 0;
                    BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
                    BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                    BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);
                    BizAction.objIndent.IndentCreatedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                    BizAction.objIndent.IsAuthorized = true;// chkIsAuthorized.IsChecked;
                    BizAction.objIndent.AuthorizedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                    BizAction.objIndent.AuthorizationDate = DateTime.Now;// dtpAuthorizationDate.SelectedDate;
                    BizAction.objIndent.Remark = txtRemark.Text;
                    BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
                    BizAction.objIndent.Status = true;
                    BizAction.objIndent.IsFreezed = (bool)chkFreezIndent.IsChecked;
                    BizAction.objIndent.IsForwarded = SelectedIndent.IsForwarded;
                    BizAction.objIndent.IsApproved = SelectedIndent.IsApproved;
                    BizAction.objIndent.IsChangeAndApprove = true; //}
                    BizAction.objIndent.IndentUnitID = SelectedIndent.IndentUnitID;
                    BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                    //   BizAction.objIndent.IsModify = true;
                    BizAction.objIndent.IndentDetailsList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();
                    BizAction.objIndent.DeletedIndentDetailsList = (List<clsIndentDetailVO>)DeleteIndentAddedItems.ToList<clsIndentDetailVO>();
                    //By Anjali............................................
                    //BizAction.objIndent.IsIndent = true;
                    BizAction.objIndent.IsIndent = 1;
                    //.................................................
                    BizAction.objIndent.InventoryIndentType = InventoryIndentType.Indent;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        ClickedFlag1 = 0;
                        if (arg.Error == null && ((clsUpdateIndentBizActionVO)arg.Result).objIndent != null)
                        {
                            if (arg.Result != null)
                            {
                                CmdPrint.IsEnabled = true;
                                CmdNew.IsEnabled = true;
                                CmdSave.IsEnabled = false;
                                CmdCancel.IsEnabled = false;
                                CmdModify.IsEnabled = false;
                                CmdForward.IsEnabled = true;
                                CmdApprove.IsEnabled = true;

                                chkFreezIndent.IsEnabled = true;
                                grdStoreIndent.IsEnabled = true;
                                CmdConvertToPR.IsEnabled = true;
                                lnkAddItems.IsEnabled = true;

                                SelectedCommand = "Modify";
                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                mgbox.Show();
                                mgbox.OnMessageBoxClosed += (MessageBoxResult res1) =>
                                {
                                    FillIndentList();
                                    objAnimation.Invoke(RotationType.Backward);
                                };
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while updating indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            mgbox.Show();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                }
                else
                    ClickedFlag1 = 0;
            };
            mgBox.Show();
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
                ((IInitiateCIMS)myData).Initiate("Approve");

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData as UIElement);


            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        # region Clicked Events
        public void CmdConvertToPR_Click(object sender, RoutedEventArgs e)
        {
            Indicator = new WaitIndicator();
            Indicator.Show();

            try
            {
                //if (dgIndentList.SelectedItem != null && ((clsIndentMasterVO)dgIndentList.SelectedItem).IsFreezed == true && ((clsIndentMasterVO)dgIndentList.SelectedItem).IsApproved == true && ((clsIndentMasterVO)dgIndentList.SelectedItem).IsConvertToPR == false)
                if (IndentDetail != null && IndentDetail.IsFreezed == true && IndentDetail.IsApproved == true && IndentDetail.IsConvertToPR == false)
                {
                    IsConvertToPR = true;

                    SelectedIndent = IndentDetail;
                    clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
                    BizAction.IndentID = Convert.ToInt64(IndentDetail.ID);
                    BizAction.UnitID = Convert.ToInt64(IndentDetail.UnitID);
                    BizAction.IndentDetailList = new List<clsIndentDetailVO>();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                //acTextBox = new TextBox();
                                dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                                SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;

                                this.DataContext = new clsIndentDetailVO();
                                IndentAddedItems.Clear();

                                foreach (var item in SelectedIndent.IndentDetailsList)
                                {
                                    if (UOMLIstNew.Count > 0)
                                    {
                                        item.UOMList = UOMLIstNew;
                                        item.SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID));
                                        item.UOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID)).Description;

                                        item.SUOM = UOMLIstNew.Single(u => u.ID.Equals(item.SUOMID)).Description;
                                    }
                                    IndentAddedItems.Add(item);
                                    //cmbAddFromStoreName.SelectedItem = item.fr;
                                }

                                var results = from r in ((List<clsStoreVO>)cmbAddFromStoreName.ItemsSource)
                                              where r.StoreId == this.SelectedIndent.FromStoreID  //r.ClinicId == this.SelectedIndent.IndentUnitID &&
                                              //&& r.ClinicId == this.SelectedIndent.UnitID
                                              select r;

                                //cmbAddFromStoreName.SelectedItem  = SelectedIndent.IndentDetailsList.
                                foreach (clsStoreVO item in results)
                                {
                                    cmbAddFromStoreName.SelectedItem = item;
                                }

                                var results1 = from r in ((List<clsStoreVO>)cmbAddToStoreName.ItemsSource)
                                               where r.StoreId == this.SelectedIndent.ToStoreID
                                               select r;

                                foreach (clsStoreVO item in results1)
                                {
                                    cmbAddToStoreName.SelectedItem = item;
                                }
                                //this.cmbAddFromStoreName.SelectedValue = SelectedIndent.FromStoreID;
                                //this.cmbAddToStoreName.SelectedValue = SelectedIndent.ToStoreID;
                                this.dtpAddIndentDate.SelectedDate = SelectedIndent.Date;
                                this.dtpAddDueDate.SelectedDate = SelectedIndent.DueDate;
                                //this.chkIsAuthorized.IsChecked = SelectedIndent.IsAuthorized;
                                //this.dtpAuthorizationDate.SelectedDate = SelectedIndent.AuthorizationDate;
                                this.IndentNumber.Text = SelectedIndent.IndentNumber;
                                this.txtRemark.Text = SelectedIndent.Remark;
                                this.chkFreezIndent.IsChecked = SelectedIndent.IsFreezed;
                                ViewIndentToPR();
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                }
                else
                {
                    string msg = "";

                    if (IndentDetail != null && IndentDetail.IsConvertToPR == true)
                    {
                        msg = "Selected Indent is already converted to Purchase Requisition";
                    }
                    else if (IndentDetail != null && IndentDetail.IsApproved == false)
                    {
                        msg = "Selected Indent is not Approved, you can't convert it to Purchase Requisition";
                    }
                    else if (IndentDetail != null && IndentDetail.IsFreezed == false)
                    {
                        msg = "Selected Indent is not freezed, you can't convert it to Purchase Requisition";
                    }

                    if (IndentDetail == null)
                    {
                        msg = "Please Select Indent to convert into Purchase Requisition";
                    }

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Indicator.Close();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            FillIndentList();
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {

            CmdPrint.IsEnabled = false;
            CmdNew.IsEnabled = false;
            CmdSave.IsEnabled = true;
            CmdCancel.IsEnabled = true;
            CmdForward.IsEnabled = false;
            CmdModify.IsEnabled = false;
            CmdApprove.IsEnabled = false;
            SelectedCommand = "New";

            chkFreezIndent.IsEnabled = true;
            grdStoreIndent.IsEnabled = true;
            CmdConvertToPR.IsEnabled = false;
            lnkAddItems.IsEnabled = true;

            cmbAddFromStoreName.IsEnabled = true;
            cmbAddToStoreName.IsEnabled = true;
            dtpAddDueDate.SelectedDate = null;
            if ((DateTime?)dtpAddDueDate.SelectedDate == null)
            {
                dtpAddDueDate.SetValidation("Please Select Expected Delivery Date");
                dtpAddDueDate.Focus();
                dtpAddDueDate.RaiseValidationError();
            }
            else
                dtpAddDueDate.ClearValidationError();


            //cmbAddFromStoreName.SelectedValue = 0;
            //cmbAddToStoreName.SelectedValue = 0;

            //SelectedIndent = new clsIndentMasterVO();
            //this.cmbAddFromStoreName.SelectedItem = new MasterListItem { ID = 0, Description = "--Select--" };
            //this.cmbAddToStoreName.SelectedItem = new MasterListItem { ID = 0, Description = "--Select--" };
            //cmbAddFromStoreName.SelectedItem = null;
            //cmbAddToStoreName.SelectedItem = null;
            this.dtpAddIndentDate.SelectedDate = DateTime.Now;
            //this.dtpAddDueDate.SelectedDate = DateTime.Now;
            //this.chkIsAuthorized.IsChecked = false;
            //this.dtpAuthorizationDate.SelectedDate = DateTime.Now;

            NewStoreFill();

            this.IndentNumber.Text = "";
            this.txtRemark.Text = "";
            grdStoreIndent.IsEnabled = true;
            chkFreezIndent.IsEnabled = true;
            lnkAddItems.IsEnabled = true;
            chkFreezIndent.IsChecked = false;
            this.DataContext = new clsIndentDetailVO();
            //if (GRNAddedItems != null)
            IndentAddedItems.Clear();

            this.IsConvertToPR = false;

            chkAgainstPatient.IsChecked = false;
            chkAgainstPatient.IsEnabled = true;
            txtMRNo.Text = "";
            txtPatientName.Text = "";

            objAnimation.Invoke(RotationType.Forward);

        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag = Validate();
                int Count = 0;
                //ClickedFlag1 += 1;
                //if (((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId > 0 && ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId > 0 && IndentAddedItems.Count != 0) //grdStoreIndent.ItemsSource != 0)
                //{
                //    foreach (clsStoreVO item in UserStores)
                //    {
                //        if (item.StoreId == ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId)
                //        {
                Count = 0;
                ClickedFlag1 += 1;
                if (ClickedFlag1 == 1)
                {

                    if (flag)
                    {

                        string msgTitle = string.Empty;
                        string msgText = string.Empty;

                        if ((bool)chkFreezIndent.IsChecked)
                        {
                            msgTitle = "Freeze and Save Indent";
                            msgText = "Do You want to Freeze and Save the Indent";
                        }
                        else
                        {
                            msgTitle = "Save Indent";
                            msgText = "Do You want to Save the Indent";
                        }

                        if ((bool)chkFreezIndent.IsChecked && this.IsConvertToPR == true)
                        {
                            msgTitle = "Convert To Purchase Requisition";
                            msgText = "Do You want to Convert Indent into Purchase Requisition";
                        }

                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                        msgWin.Show();


                    }
                    else
                        ClickedFlag1 = 0;
                }

            }
            catch (Exception ex)
            {
                ClickedFlag1 = 0;
                throw;
            }
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedCommand = "Cancel";
            CmdPrint.IsEnabled = true;
            CmdNew.IsEnabled = true;
            CmdSave.IsEnabled = false;
            CmdCancel.IsEnabled = false;
            CmdModify.IsEnabled = false;
            CmdForward.IsEnabled = true;
            CmdApprove.IsEnabled = true;

            chkFreezIndent.IsEnabled = true;
            grdStoreIndent.IsEnabled = true;
            CmdConvertToPR.IsEnabled = true;
            lnkAddItems.IsEnabled = true;

            IndentAddedItems.Clear();
            //cmbAddFromStoreName.SelectedValue = 0;
            //cmbAddToStoreName.SelectedValue = 0;
            cmbAddFromStoreName.SelectedItem = null;
            cmbAddToStoreName.SelectedItem = null;
            //dtpAddDueDate.SelectedDate = DateTime.Now;
            dtpAddDueDate.SelectedDate = null;
            dtpAddIndentDate.SelectedDate = DateTime.Now;
            //chkIsAuthorized.IsChecked = false;
            //dtpAuthorizationDate.SelectedDate = DateTime.Now;

            //Enable after Forward
            grdStoreIndent.IsEnabled = true;
            chkFreezIndent.IsEnabled = true;
            lnkAddItems.IsEnabled = true;
            this.cmbAddFromStoreName.IsEnabled = true;
            this.cmbAddToStoreName.IsEnabled = true;
            this.dtpAddIndentDate.IsEnabled = true;
            this.dtpAddDueDate.IsEnabled = true;
            //this.chkIsAuthorized.IsEnabled = true;
            //this.dtpAuthorizationDate.IsEnabled = true;
            this.txtRemark.IsEnabled = true;

            this.IsConvertToPR = false;


            if (IsFromConvertToPR)
            {
                ModuleName = "PalashDynamics.Pharmacy";
                Action = "PalashDynamics.Pharmacy.StoreIndent";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Approve Indent";
                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                FillIndentList();

                objAnimation.Invoke(RotationType.Backward);
            }
        }

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAddFromStoreName.SelectedItem != null && ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId != 0)
            {
                ItemList Itemswin = new ItemList();
                Itemswin.IndentAddedItems = this.IndentAddedItems.ToList();
                Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Itemswin.IsItemSuspend = false;  // for getting the Suspended Item also on Indent..
                //Itemswin.ShowZeroStockBatches = true;
                Itemswin.ShowZeroStockBatches = false;

                Itemswin.ShowBatches = false;
                //Itemswin.cmbStore.SelectedItem = cmbAddToStoreName.SelectedItem;
                Itemswin.cmbStore.IsEnabled = false;
                Itemswin.ShowQuantity = true;
                Itemswin.StoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
                Itemswin.ClinicID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).ClinicId;
                Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                Itemswin.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select From Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
            }

            #region Commented by Ashish Z. for fetch items mapped to Indenting store only
            //if (cmbAddToStoreName.SelectedItem != null && ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId != 0)
            //{
            //    ItemList Itemswin = new ItemList();
            //    Itemswin.IndentAddedItems = this.IndentAddedItems.ToList();
            //    Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
            //    Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            //    //Itemswin.ShowZeroStockBatches = true;
            //    Itemswin.ShowZeroStockBatches = false;

            //    Itemswin.ShowBatches = false;
            //    //Itemswin.cmbStore.SelectedItem = cmbAddToStoreName.SelectedItem;
            //    Itemswin.cmbStore.IsEnabled = false;
            //    Itemswin.ShowQuantity = true;
            //    Itemswin.StoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
            //    Itemswin.ClinicID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).ClinicId;
            //    Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
            //    Itemswin.Show();
            //}
            //else
            //{
            //    MessageBoxControl.MessageBoxChildWindow mgbx = null;
            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "To Store can not be Empty. Please Select To Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //}
            #endregion
        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;

            if (Itemswin.SelectedItems != null)
            {
                //if (GRNAddedItems == null)
                //    GRNAddedItems = new ObservableCollection<clsGRNDetailsVO>();

                foreach (var item in Itemswin.SelectedItems)
                {
                    IEnumerator<clsIndentDetailVO> list = (IEnumerator<clsIndentDetailVO>)IndentAddedItems.GetEnumerator();
                    bool IsExist = false;

                    while (list.MoveNext())
                    {
                        if (item.ID == ((clsIndentDetailVO)list.Current).ItemID)
                        {
                            IsExist = true;
                            break;
                        }
                    }
                    if (IsExist == false)
                    {
                        clsIndentDetailVO objVO = new clsIndentDetailVO();
                        objVO.ItemID = item.ID;
                        objVO.ItemName = item.ItemName;
                        objVO.RequiredQuantity = item.RequiredQuantity;
                        //objVO.ConversionFactor = Convert.ToSingle(item.ConversionFactor);
                        //objVO.SelectedUOM=new MasterListItem{ID=item.SUM ,Description=item.SUOM};
                        objVO.UOM = item.PUOM;
                        objVO.SUOM = item.SUOM;
                        objVO.SUOMID = item.SUM;
                        objVO.PUOMID = item.PUM;
                        objVO.BaseUOMID = item.BaseUM;
                        objVO.BaseUOM = item.BaseUMString;
                        objVO.SellingUOMID = item.SellingUM;
                        objVO.SellingUOM = item.SellingUMString;

                        //----------------------------//
                        objVO.SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM };
                        float CalculatedFromCF = item.PurchaseToBaseCF / item.StockingToBaseCF;
                        objVO.StockCF = item.PurchaseToBaseCF / item.StockingToBaseCF;
                        objVO.ConversionFactor = item.PurchaseToBaseCF;
                        objVO.TotalBatchAvailableStock = item.TotalBatchAvailableStock;
                        objVO.SUOM = item.StockUOM;
                        IndentAddedItems.Add(objVO);
                    }
                    //IndentAddedItems.Add(new clsIndentDetailVO()
                    //{
                    //    ItemID = item.ID,
                    //    ItemName = item.ItemName,
                    //    RequiredQuantity = item.RequiredQuantity,
                    //    ConversionFactor = Convert.ToSingle(item.ConversionFactor),
                    //    //UOMList = UOMLIstNew, 
                    //    //SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.PUM)), 
                    //    //SelectedUOM = new MasterListItem { ID = item.SUM, Description = item.SUOM },
                    //    SelectedUOM = new MasterListItem { ID = 0, Description = "--Select--" },
                    //    UOM = item.PUOM,
                    //    SUOM = item.SUOM,
                    //    SUOMID = item.SUM,

                    //    //MRP = (float)item.MRP,
                    //    //MainMRP = (float)item.MRP,
                    //    PUOMID = item.PUM,
                    //    //SUOMID = item.SUM,
                    //    BaseUOMID = item.BaseUM,
                    //    BaseUOM = item.BaseUMString,
                    //    SellingUOMID = item.SellingUM,
                    //    SellingUOM = item.SellingUMString
                    //});
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Item is alredy added!", "Please add another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }

                grdStoreIndent.Focus();
                grdStoreIndent.UpdateLayout();

                grdStoreIndent.SelectedIndex = IndentAddedItems.Count - 1;
            }
        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgIndentList.SelectedItem != null)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Inventory_New/StoreIndentPrint.aspx?IndentId=" + ((clsIndentMasterVO)dgIndentList.SelectedItem).ID + "&UnitID=" + (((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID)), "_blank");
            }
        }

        private void ViewIndent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null)
                {
                    this.IsConvertToPR = false;
                    clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();

                    SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
                    //   clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
                    BizAction.IndentID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).ID);
                    BizAction.UnitID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID);
                    BizAction.IndentDetailList = new List<clsIndentDetailVO>();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                //acTextBox = new TextBox();
                                dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                                SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;

                                this.DataContext = new clsIndentDetailVO();
                                IndentAddedItems.Clear();

                                foreach (var item in SelectedIndent.IndentDetailsList)
                                {
                                    if (UOMLIstNew.Count > 0)
                                    {
                                        item.UOMList = UOMLIstNew;
                                        item.SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID));
                                        item.UOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID)).Description;

                                        //item.SUOM = UOMLIstNew.Single(u => u.ID.Equals(item.SUOMID)).Description;
                                    }
                                    IndentAddedItems.Add(item);
                                    //cmbAddFromStoreName.SelectedItem = item.fr;
                                }

                                if (cmbAddFromStoreName.ItemsSource != null && cmbAddToStoreName.ItemsSource != null)
                                {
                                    var results = from r in ((List<clsStoreVO>)cmbAddFromStoreName.ItemsSource)
                                                  where r.StoreId == this.SelectedIndent.FromStoreID  //r.ClinicId == this.SelectedIndent.IndentUnitID &&
                                                  //&& r.ClinicId == this.SelectedIndent.UnitID
                                                  select r;

                                    //cmbAddFromStoreName.SelectedItem  = SelectedIndent.IndentDetailsList.
                                    foreach (clsStoreVO item in results)
                                    {
                                        cmbAddFromStoreName.SelectedItem = item;
                                    }

                                    var results1 = from r in ((List<clsStoreVO>)cmbAddToStoreName.ItemsSource)
                                                   where r.StoreId == this.SelectedIndent.ToStoreID
                                                   select r;

                                    foreach (clsStoreVO item in results1)
                                    {
                                        cmbAddToStoreName.SelectedItem = item;
                                    }
                                }
                                //this.cmbAddFromStoreName.SelectedValue = SelectedIndent.FromStoreID;
                                //this.cmbAddToStoreName.SelectedValue = SelectedIndent.ToStoreID;
                                this.dtpAddIndentDate.SelectedDate = SelectedIndent.Date;
                                this.dtpAddDueDate.SelectedDate = SelectedIndent.DueDate;
                                //this.chkIsAuthorized.IsChecked = SelectedIndent.IsAuthorized;
                                //this.dtpAuthorizationDate.SelectedDate = SelectedIndent.AuthorizationDate;
                                this.IndentNumber.Text = SelectedIndent.IndentNumber;
                                this.txtRemark.Text = SelectedIndent.Remark;
                                this.chkFreezIndent.IsChecked = SelectedIndent.IsFreezed;
                                if (SelectedIndent.IsAgainstPatient)
                                {
                                    chkAgainstPatient.IsChecked = SelectedIndent.IsAgainstPatient;
                                    txtMRNo.Text = SelectedIndent.MRNo;
                                    txtPatientName.Text = SelectedIndent.PatientName;
                                }
                                chkAgainstPatient.IsEnabled = false;
                                cmdAddPatient.IsEnabled = false;

                                if (IsFromChangAndApprove == true)
                                {
                                    // ViewIndent();
                                    ViewIndentForApprove();
                                }
                                else
                                    //if (SelectedIndent.IsApproved == false && IsOpenFor == "New")
                                    if (IsOpenFor == "New")
                                    {
                                        ViewIndent();
                                    }
                                    else if ((SelectedIndent.IsApproved == false || SelectedIndent.IsApproved == true) && IsOpenFor == "Approve")
                                    {
                                        ViewIndentForApprove();
                                    }
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void chkFreeze_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null && ((clsIndentMasterVO)dgIndentList.SelectedItem).IsFreezed == true)
                {
                    if (((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus != InventoryIndentStatus.Cancelled)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Freeze Indent Number " + SelectedIndent.IndentNumber + "", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                clsUpdateIndentOnlyForFreezeBizActionVO BizAction = new clsUpdateIndentOnlyForFreezeBizActionVO();
                                BizAction.IndentID = Convert.ToInt64(SelectedIndent.ID);

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                client.ProcessCompleted += (s, arg) =>
                                {
                                    if (arg.Error == null)
                                    {
                                        if (arg.Result != null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Freezed & Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            mgbox.Show();
                                        }
                                    }
                                };
                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client.CloseAsync();
                            }
                            else
                            {
                                ((clsIndentMasterVO)dgIndentList.SelectedItem).IsFreezed = false;
                            }
                        };

                        mgBox.Show();
                    }
                    else
                    {
                        ((clsIndentMasterVO)dgIndentList.SelectedItem).IsFreezed = false;
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent is Cancelled, you cannot freeze it", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClickedFlag1 = ClickedFlag1 + 1;
                if (ClickedFlag1 == 1)
                {
                    bool flag = Validate();
                    if (flag)
                    {
                        if (SelectedCommand == "Forward")
                        {
                            if (SelectedIndent.ToStoreID == ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "ToStore is not changed.\n Change ToStore then Modify it.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                mgBox.Show();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to forward indent ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                                        BizAction.IsForChangeAndApproveIndent = false;
                                        BizAction.objIndent = new clsIndentMasterVO();
                                        BizAction.objIndent.ID = SelectedIndent.ID;
                                        BizAction.objIndent.Date = SelectedIndent.Date;
                                        BizAction.objIndent.Time = SelectedIndent.Time;
                                        BizAction.objIndent.TransactionMovementID = SelectedIndent.TransactionMovementID;
                                        BizAction.objIndent.FromStoreID = SelectedIndent.FromStoreID;
                                        BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                                        BizAction.objIndent.DueDate = SelectedIndent.DueDate;
                                        BizAction.objIndent.IndentCreatedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                        BizAction.objIndent.IsAuthorized = SelectedIndent.IsAuthorized;
                                        BizAction.objIndent.AuthorizedByID = SelectedIndent.AuthorizedByID;
                                        BizAction.objIndent.AuthorizationDate = SelectedIndent.AuthorizationDate;
                                        BizAction.objIndent.Remark = SelectedIndent.Remark;
                                        BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
                                        BizAction.objIndent.Status = true;
                                        BizAction.objIndent.IsFreezed = SelectedIndent.IsFreezed;
                                        BizAction.objIndent.IsForwarded = true;
                                        BizAction.objIndent.IsApproved = SelectedIndent.IsApproved;
                                        BizAction.objIndent.IndentUnitID = SelectedIndent.IndentUnitID;
                                        BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                        //By Anjali......................................
                                        // BizAction.objIndent.IsIndent = true;
                                        BizAction.objIndent.IsIndent = 1;
                                        //.................................................
                                        BizAction.objIndent.InventoryIndentType = InventoryIndentType.Indent;

                                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                        client.ProcessCompleted += (s, arg) =>
                                        {
                                            ClickedFlag1 = 0;
                                            if (arg.Error == null && ((clsUpdateIndentBizActionVO)arg.Result).objIndent != null)
                                            {
                                                if (arg.Result != null)
                                                {
                                                    CmdPrint.IsEnabled = true;
                                                    CmdNew.IsEnabled = true;
                                                    CmdSave.IsEnabled = false;
                                                    CmdCancel.IsEnabled = false;
                                                    CmdModify.IsEnabled = false;
                                                    CmdForward.IsEnabled = true;
                                                    CmdApprove.IsEnabled = true;
                                                    //Enable after Forward
                                                    chkFreezIndent.IsEnabled = true;
                                                    lnkAddItems.IsEnabled = true;
                                                    this.cmbAddFromStoreName.IsEnabled = true;
                                                    this.cmbAddToStoreName.IsEnabled = true;
                                                    this.dtpAddIndentDate.IsEnabled = true;
                                                    this.dtpAddDueDate.IsEnabled = true;
                                                    //this.chkIsAuthorized.IsEnabled = true;
                                                    //this.dtpAuthorizationDate.IsEnabled = true;
                                                    this.txtRemark.IsEnabled = true;

                                                    chkFreezIndent.IsEnabled = true;
                                                    grdStoreIndent.IsEnabled = true;
                                                    CmdConvertToPR.IsEnabled = true;
                                                    lnkAddItems.IsEnabled = true;

                                                    SelectedCommand = "Modify";

                                                    FillIndentList();
                                                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Forwarded Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                    mgbox.OnMessageBoxClosed += (MessageBoxResult res1) =>
                                                    {
                                                        objAnimation.Invoke(RotationType.Backward);
                                                    };
                                                    mgbox.Show();
                                                }
                                            }
                                            else
                                            {
                                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while forwarding indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                                mgbox.Show();
                                            }
                                        };
                                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    }
                                    else
                                        ClickedFlag1 = 0;
                                };
                                mgBox.Show();
                            }
                        }
                        else
                        {
                            string msgTitle = "";
                            string msgDesc = "";

                            if ((bool)chkFreezIndent.IsChecked)
                            {
                                msgTitle = "Freeze and Update Indent";
                                msgDesc = "Do You want to Freeze and Update the Indent?";
                            }
                            else
                            {
                                msgTitle = "Update Indent";
                                msgDesc = "Do You want to Update the Indent?";
                            }

                            MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", msgDesc, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                                    BizAction.IsForChangeAndApproveIndent = false;
                                    BizAction.objIndent = new clsIndentMasterVO();
                                    BizAction.objIndent.ID = SelectedIndent.ID;
                                    BizAction.objIndent.Date = dtpAddIndentDate.SelectedDate; //Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
                                    BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
                                    BizAction.objIndent.TransactionMovementID = 0;
                                    BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
                                    BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                                    BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);
                                    BizAction.objIndent.IndentCreatedByID = SelectedIndent.IndentCreatedByID; //((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                    BizAction.objIndent.IsAuthorized = SelectedIndent.IsAuthorized;// chkIsAuthorized.IsChecked;
                                    BizAction.objIndent.AuthorizedByID = SelectedIndent.AuthorizedByID; //((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                    BizAction.objIndent.AuthorizationDate = SelectedIndent.AuthorizationDate; //DateTime.Now;// dtpAuthorizationDate.SelectedDate;
                                    BizAction.objIndent.Remark = txtRemark.Text;
                                    BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
                                    BizAction.objIndent.Status = true;
                                    BizAction.objIndent.IsFreezed = (bool)chkFreezIndent.IsChecked;
                                    BizAction.objIndent.IsForwarded = SelectedIndent.IsForwarded;
                                    BizAction.objIndent.IsChangeAndApprove = true; //}
                                    BizAction.objIndent.IsApproved = SelectedIndent.IsApproved; ///}
                                    BizAction.objIndent.IndentUnitID = SelectedIndent.IndentUnitID;
                                    BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                    //   BizAction.objIndent.IsModify = true;
                                    BizAction.objIndent.IndentDetailsList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();
                                    BizAction.objIndent.DeletedIndentDetailsList = (List<clsIndentDetailVO>)DeleteIndentAddedItems.ToList<clsIndentDetailVO>();
                                    //By Anjali.................................
                                    // BizAction.objIndent.IsIndent = true;
                                    BizAction.objIndent.IsIndent = 1;
                                    //............................................
                                    BizAction.objIndent.InventoryIndentType = InventoryIndentType.Indent;

                                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                    client.ProcessCompleted += (s, arg) =>
                                    {
                                        ClickedFlag1 = 0;
                                        if (arg.Error == null && ((clsUpdateIndentBizActionVO)arg.Result).objIndent != null)
                                        {
                                            if (arg.Result != null)
                                            {
                                                CmdPrint.IsEnabled = true;
                                                CmdNew.IsEnabled = true;
                                                CmdSave.IsEnabled = false;
                                                CmdCancel.IsEnabled = false;
                                                CmdModify.IsEnabled = false;
                                                CmdForward.IsEnabled = true;
                                                CmdApprove.IsEnabled = true;

                                                chkFreezIndent.IsEnabled = true;
                                                grdStoreIndent.IsEnabled = true;
                                                CmdConvertToPR.IsEnabled = true;
                                                lnkAddItems.IsEnabled = true;

                                                SelectedCommand = "Modify";
                                                objAnimation.Invoke(RotationType.Backward);
                                                FillIndentList();
                                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                mgbox.Show();
                                            }
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while updating indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                            mgbox.Show();
                                        }
                                    };
                                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                }
                                else
                                    ClickedFlag1 = 0;
                            };
                            mgBox.Show();
                        }
                    }
                    else
                        ClickedFlag1 = 0;
                }
            }
            catch (Exception ex)
            {
                ClickedFlag1 = 0;
                throw;
            }
        }

        private void CmdForward_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null)
                {
                    if (SelectedIndent.IsApproved)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent is already approved. \n This Indent can not be forward.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else
                    {
                        if (SelectedIndent.IsFreezed)
                        {
                            this.DataContext = new clsIndentDetailVO();
                            IndentAddedItems.Clear();
                            foreach (var item in SelectedIndent.IndentDetailsList)
                            {
                                IndentAddedItems.Add(item);
                            }

                            //var results = from r in ((List<clsStoreVO>)cmbAddFromStoreName.ItemsSource)
                            //              where r.StoreId == this.SelectedIndent.FromStoreID
                            //              select r;

                            //foreach (clsStoreVO item in results)
                            //{
                            //    cmbAddFromStoreName.SelectedItem = item;
                            //}

                            var results1 = from r in ((List<clsStoreVO>)cmbAddToStoreName.ItemsSource)
                                           where r.StoreId == this.SelectedIndent.ToStoreID
                                           select r;

                            foreach (clsStoreVO item in results1)
                            {
                                cmbAddFromStoreName.SelectedItem = item;
                                //cmbAddToStoreName.SelectedItem = item;
                            }
                            //this.cmbAddFromStoreName.SelectedValue = SelectedIndent.FromStoreID;
                            //this.cmbAddToStoreName.SelectedValue = SelectedIndent.ToStoreID;
                            this.dtpAddIndentDate.SelectedDate = SelectedIndent.Date;
                            this.dtpAddDueDate.SelectedDate = SelectedIndent.DueDate;
                            //this.chkIsAuthorized.IsChecked = SelectedIndent.IsAuthorized;
                            //this.dtpAuthorizationDate.SelectedDate = SelectedIndent.AuthorizationDate;
                            this.IndentNumber.Text = SelectedIndent.IndentNumber;
                            this.txtRemark.Text = SelectedIndent.Remark;
                            this.chkFreezIndent.IsChecked = SelectedIndent.IsFreezed;

                            CmdPrint.IsEnabled = false;
                            CmdNew.IsEnabled = false;
                            CmdSave.IsEnabled = false;
                            CmdCancel.IsEnabled = true;
                            CmdModify.IsEnabled = true;
                            CmdForward.IsEnabled = false;
                            CmdApprove.IsEnabled = false;
                            //Disable for Forward
                            grdStoreIndent.IsEnabled = false;
                            chkFreezIndent.IsEnabled = false;
                            lnkAddItems.IsEnabled = false;
                            this.cmbAddFromStoreName.IsEnabled = false;
                            this.cmbAddToStoreName.IsEnabled = true;
                            this.dtpAddIndentDate.IsEnabled = false;
                            this.dtpAddDueDate.IsEnabled = false;
                            //this.chkIsAuthorized.IsEnabled = false;
                            //this.dtpAuthorizationDate.IsEnabled = false;
                            this.txtRemark.IsEnabled = false;
                            this.chkFreezIndent.IsEnabled = false;

                            this.IsConvertToPR = false;

                            objAnimation.Invoke(RotationType.Forward);
                            SelectedCommand = "Forward";
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent is not Freezed \n Please Freeze Indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbox.Show();
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No Indent is Selected \n Please select a Indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null)
                {
                    if (SelectedIndent.IsApproved)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent is already approved .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else
                    {
                        if (SelectedIndent.IsFreezed)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Approve indent ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                                    BizAction.objIndent = SelectedIndent;
                                    if (BizAction.objIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Can't approve indent it is forcefully closed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        mgbox.Show();
                                        return;
                                    }
                                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)


                                        BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                    else
                                        BizAction.objIndent.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                                    BizAction.objIndent.IsApproved = true;
                                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                    client.ProcessCompleted += (s, arg) =>
                                    {
                                        if (arg.Error == null)
                                        {
                                            if (arg.Result != null)
                                            {
                                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent Approved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                mgbox.Show();
                                                FillIndentList();
                                            }
                                        }
                                    };
                                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                }
                            };
                            mgBox.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent is not Freezed \n Please Freeze Indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbox.Show();
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No Indent is Selected \n Please select a Indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (chkFreezIndent.IsChecked == true)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Indent is freezed you can't delete the item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
                return;
            }

            if (grdStoreIndent.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to delete the item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        IndentAddedItems.RemoveAt(grdStoreIndent.SelectedIndex);
                        grdStoreIndent.Focus();
                        grdStoreIndent.UpdateLayout();
                        grdStoreIndent.SelectedIndex = IndentAddedItems.Count - 1;

                    }
                };
                msgWD.Show();
            }
        }

        private void cmdCancelIndent_Click(object sender, RoutedEventArgs e)
        {

            if (((clsIndentMasterVO)dgIndentList.SelectedItem) != null)
            {

                if (((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus == InventoryIndentStatus.Cancelled)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD1 =
                 new MessageBoxControl.MessageBoxChildWindow("", "Cannot cancel the Indent, The Indent is Cancelled", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD1.Show();
                }
                else if (((clsIndentMasterVO)dgIndentList.SelectedItem).IsApproved == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD1 =
                 new MessageBoxControl.MessageBoxChildWindow("", "Cannot cancel the Indent, The Indent is Approved", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD1.Show();
                }
                else if (((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus == InventoryIndentStatus.Rejected)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD1 =
                 new MessageBoxControl.MessageBoxChildWindow("", "Cannot cancel the Indent, The Indent is Rejected", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD1.Show();
                }
                else
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to cancel the Indent?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            POCancellationWindow Win = new POCancellationWindow();
                            Win.Title = "Indent Cancellation";
                            Win.tblkCAncellationReason.Text = "Indent Cancellation";
                            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                            Win.Show();
                        }
                    };
                    msgWD.Show();
                }

            }

        }

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
            POCancellationWindow Win = (POCancellationWindow)sender;

            clsUpdateRemarkForIndentCancellationBizActionVO objBizActionVO = new clsUpdateRemarkForIndentCancellationBizActionVO();
            objBizActionVO.IndentMaster = new clsIndentMasterVO();
            objBizActionVO.IndentMaster.ID = SelectedIndent.ID;
            objBizActionVO.IndentMaster.UnitID = SelectedIndent.UnitID;
            objBizActionVO.CancellationRemark = Win.txtAppReason.Text;
            try
            {

                objBizActionVO.CancellationRemark = ((POCancellationWindow)sender).txtAppReason.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Indent cancelled successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
                        DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                        DataListPageSize = 5;
                        FillIndentList();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        # endregion

        #region Item Search Control
        ItemSearch _ItemSearchRowControl = null;
        private void AttachItemSearchControl(long StoreID, long SupplierID)
        {
            BdrItemSearch.Visibility = Visibility.Visible;
            ItemSearchStackPanel.Children.Clear();
            _ItemSearchRowControl = new ItemSearch(StoreID, SupplierID);
            _ItemSearchRowControl.IsFromIndent = true;
            _ItemSearchRowControl.OnQuantityEnter_Click += new RoutedEventHandler(_ItemSearchRowControl_OnQuantityEnter_Click);
            ItemSearchStackPanel.Children.Add(_ItemSearchRowControl);
        }

        void _ItemSearchRowControl_OnQuantityEnter_Click(object sender, RoutedEventArgs e)
        {
            String strItemIDs = string.Empty;//Only For PO
            ItemSearch _ItemSearchRowControl = (ItemSearch)sender;
            if (IndentAddedItems != null)
            {
                if (IndentAddedItems.Count.Equals(0))
                {
                    IndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
                }
            }
            else
            {
                IndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
            }
            _ItemSearchRowControl.SelectedItems[0].RowID = IndentAddedItems.Count + 1;
            foreach (var item in _ItemSearchRowControl.SelectedItems)
            {
                if (IndentAddedItems.Where(indentItems => indentItems.ItemID == item.ItemID).Any() == false)
                {
                    clsIndentDetailVO itemObj = new clsIndentDetailVO();
                    itemObj.ItemID = item.ItemID;
                    itemObj.ItemName = item.ItemName;
                    itemObj.SingleQuantity = Convert.ToSingle(item.RequiredQuantity);
                    itemObj.ConversionFactor = Convert.ToSingle(item.ConversionFactor);
                    //itemObj.UOMList = UOMLIstNew;
                    //itemObj.SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.PUM));
                    itemObj.UOM = item.PUOM;
                    itemObj.SUOM = item.SUOM;
                    itemObj.SUOMID = item.SUM;

                    //itemObj.MRP = (float)item.MRP;
                    //itemObj.MainMRP = (float)item.MRP;
                    itemObj.PUOMID = item.PUM;
                    itemObj.SUOMID = item.SUM;
                    itemObj.BaseUOMID = item.BaseUM;
                    itemObj.BaseUOM = item.BaseUMString;
                    itemObj.SellingUOMID = item.SellingUM;
                    itemObj.SellingUOM = item.SellingUMString;

                    if (itemObj.SelectedUOM.Description != itemObj.SUOM)
                    {
                        itemObj.RequiredQuantity = itemObj.SingleQuantity * itemObj.ConversionFactor;
                    }
                    else
                    {
                        itemObj.RequiredQuantity = itemObj.SingleQuantity;
                    }

                    IndentAddedItems.Add(itemObj);

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Item is alredy added!", "Please add another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

                strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemID) + ",");

            }

            grdStoreIndent.ItemsSource = null;
            grdStoreIndent.ItemsSource = IndentAddedItems;
            grdStoreIndent.UpdateLayout();

            _ItemSearchRowControl.SetFocus();
            _ItemSearchRowControl = null;
            _ItemSearchRowControl = new ItemSearch();
            // _ItemSearchRowControl.cmbItemName.Focus();
        }

        #endregion

        #region SelectionChanged/ KeyUp/ Other Events
        private void cmbAddToStoreName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (IndentAddedItems != null)
                {
                    if (IndentAddedItems.Count > 0)
                    {
                        if (SelectedIndent != null)
                        {
                            if (!SelectedIndent.IsFreezed)
                                if (cmbAddToStoreName.SelectedItem != null && ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId != 0)
                                {

                                    if (SelectedIndent.ToStoreID != ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId)
                                        IndentAddedItems.Clear();
                                }
                        }
                    }

                    long StoreID = 0;

                    if (cmbAddToStoreName.SelectedItem != null)
                        StoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;

                    //if (StoreID > 0)
                    //{
                    //    BdrItemSearch.Visibility = System.Windows.Visibility.Visible;
                    //    AttachItemSearchControl(StoreID, 0);
                    //}
                    //else
                    //{
                    //    BdrItemSearch.Visibility = System.Windows.Visibility.Collapsed;
                    //}

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void txtIndentNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillIndentList();
            }
            //else if (e.Key == Key.Tab)
            //{
            //    FillIndentList();
            //}
        }

        private void cmbFromStoreName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbFromStoreName.SelectedItem != null)
            //{
            //    FillIndentList();
            //}
        }

        private void cmbToStoreName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbToStoreName.SelectedItem != null)
            //{
            //    FillIndentList();
            //}
        }

        private void cmbFromStoreName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (cmbFromStoreName.SelectedItem != null)
                {
                    FillIndentList();
                }
            }
        }

        private void cmbToStoreName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (cmbToStoreName.SelectedItem != null)
                {
                    FillIndentList();
                }
            }
        }

        //private void grdStoreIndent_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    if (grdStoreIndent.ItemsSource != null)
        //    {
        //        if (((clsIndentDetailVO)e.Row.DataContext).IsItemBlock)
        //            e.Row.IsEnabled = false;
        //        else
        //            e.Row.IsEnabled = true;
        //    }
        //}
        #endregion

        #region Conversion Factor
        private void cmbUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdStoreIndent.SelectedItem != null)
            {
                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity = 0;
                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = 0;
                ////CalculateOpeningBalanceSummary();

                clsConversionsVO objConversion = new clsConversionsVO();
                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity = 0;
                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = 0;

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);

                    if (cmbConversions.SelectedItem != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList.Count > 0)
                        CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
                }
                else
                {

                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor = 0;
                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseConversionFactor = 0;

                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).MRP = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).MainMRP;
                    //(((clsIndentDetailVO)grdStoreIndent.SelectedItem).Rate = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).MainRate;
                }
            }
        }

        private void cmbUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList == null || ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ItemID;
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
                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];

                        //List<clsConversionsVO> ConvertLst = new List<clsConversionsVO>();
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.ToUOM).Select(y => y.First()).ToList());

                        //List<clsConversionsVO> MainConvertLst = new List<clsConversionsVO>();
                        //MainConvertLst = UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList().DeepCopy();

                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());



                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        //UOMConversionLIst.Add(new MasterListItem(0, "-- Select --"));
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (grdStoreIndent.SelectedItem != null)
                        {
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
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

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();
            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (grdStoreIndent.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList;

                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    //objConversionVO.MainMRP = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).MainMRP;
                    //objConversionVO.MainRate = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity;

                    long BaseUOMID = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).Rate = objConversionVO.Rate;
                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).MRP = objConversionVO.MRP;

                    if (objConversionVO.BaseQuantity > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = objConversionVO.BaseQuantity; //objConversionVO.Quantity;
                    if (objConversionVO.Quantity > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockingQuantity = objConversionVO.Quantity;

                    if (objConversionVO.BaseRate > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    if (objConversionVO.BaseMRP > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    if (objConversionVO.ConversionFactor > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockCF = objConversionVO.ConversionFactor;
                    if (objConversionVO.BaseConversionFactor > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor = objConversionVO.BaseConversionFactor; //objConversionVO.ConversionFactor;


                }
            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (grdStoreIndent.SelectedItem != null)
            {
                Conversion win = new Conversion();
                win.FillUOMConversions(Convert.ToInt64(((clsIndentDetailVO)grdStoreIndent.SelectedItem).ItemID), ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;

            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity > 0)
            {
                if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID == ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseUOMID && (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity % 1) != 0)
                {
                    ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity = 0;
                    string msgText = "Quantity Cannot Be In Fraction for Base UOM";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);


        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void cmdAddPatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PatientSearch Win = new PatientSearch();
                Win.isfromCouterSale = true;
                Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
                Win.Show();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                txtMRNo.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                txtPatientName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
            }
        }

        private void chkAgainstPatient_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true)
            {
                cmdAddPatient.IsEnabled = true;
                IsAgainstPatient = true;
            }
            else
            {
                cmdAddPatient.IsEnabled = false;
                IsAgainstPatient = false;
                txtMRNo.Text = "";
                txtPatientName.Text = "";
            }
        }

        private void txtMrno_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_Click(sender, e);

            }
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtName.Text = txtName.Text.ToTitleCase();
        }

        int selectionStart = 0;
        int selectionLength = 0;
        string textBefore = null;
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

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

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
                        clsUserRightsVO objUser;
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;                       

                        if (objUser.IsDirectIndent)
                        {
                            this.IsDirectIndent = true;
                        }
                        else
                        {
                            this.IsDirectIndent = false;
                        }

                        if (objUser.IsInterClinicIndent)
                        {
                            this.IsInterClinicIndent = true;
                        }
                        else
                        {
                            this.IsInterClinicIndent = false;
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
            finally
            {

            }
        }



    }
}


