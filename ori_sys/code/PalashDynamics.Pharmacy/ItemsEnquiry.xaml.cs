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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using System.ComponentModel;
using PalashDynamics.Collections;


namespace PalashDynamics.Pharmacy
{
    public partial class ItemsEnquiry : UserControl, INotifyPropertyChanged
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
        
        #region Variables

        private SwivelAnimation objAnimation = null;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        Int64 EnquiryId = 0;
        String FilterExpr="";

        public PagedSortableCollectionView<clsItemEnquiryVO> MasterList { get; private set; }
        public ObservableCollection<clsEnquiryDetailsVO> ScarpsAddedItems { get; set; }
        public ObservableCollection<clsItemsEnquiryTermConditionVO> termcondition { get; set; }
        #endregion Variables

        #region Validation
        public bool ValidationBackPanel()
        {
            if(dtEnquiryDate.SelectedDate==null)
            {
                dtEnquiryDate.SetValidation("Please Enter Item Enquiry Date");
                dtEnquiryDate.RaiseValidationError();
                dtEnquiryDate.Focus();
                return false;
            }
            else if (cboStore.SelectedItem == null)
            {
                msgText = "Please Select Store!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                cboStore.Focus();
                msgWindow.Show();
                return false;

            }
            else if (((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
            {
                msgText = "Please Select Store!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                cboStore.Focus();
                msgWindow.Show();
                return false;

            }
            //else if (cboStore.)
            //{
            //    msgText = "Please Select Store!";
            //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    cboStore.Focus();
            //    msgWindow.Show();
            //    return false;

            //}
            else if (lstboxSupplier.SelectedItems.Count == 0)
            {
                msgText = "Please Select Supplier!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                lstboxSupplier.Focus();
                msgWindow.Show();
                return false;
            }
            else if (dgEnquiryItems.ItemsSource == null)
            {
                msgText = "Item Equiry Grid Cannot be Blank!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgWindow.Show();
                return false;
            }
            else if (ScarpsAddedItems.Count == 0 || ScarpsAddedItems == null)
            {
                msgText = "Item Equiry Grid Cannot be Blank!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgWindow.Show();
                return false;
            }




            else if (ScarpsAddedItems.Count > 0 && ScarpsAddedItems != null)
            {
                List<clsEnquiryDetailsVO> objList = ScarpsAddedItems.ToList<clsEnquiryDetailsVO>();
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                if (objList != null && objList.Count > 0)
                {
                    foreach (var item in objList)
                    {
                        if (item.Quantity == 0)
                        {
                            mgbx = new MessageBoxControl.MessageBoxChildWindow("Enquiry Quantity Zero!.", "Enquiry Quantity In The List Can't Be Zero. Please Enter Enquiry Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            return false;
                        }

                        item.Quantity = Convert.ToSingle(System.Math.Round(item.Quantity, 1));
                        if (((int)item.Quantity).ToString().Length > 5)
                        {
                           // MessageBoxControl.MessageBoxChildWindow mgbx = null;
                            mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Length Should Not Be Greater Than 5 Digits.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            item.Quantity = 1;
                            return false;


                        }

                    }
                    //return true;
                }

                return true;
            }

            else
            {
                dtEnquiryDate.ClearValidationError();
                return true;
            }

        }


        public bool ValidationFrontPanel()
        {
            if (dtpFromDate.SelectedDate == null && dtpToDate.SelectedDate!=null )
            {
                dtpFromDate.SetValidation("Please Enter From Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                return false;
            }
            else if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Please Enter To Date");
                dtpToDate.RaiseValidationError();
                dtpToDate.Focus();
                return false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
                dtpToDate.ClearValidationError();
                return true;
            }

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


        public ItemsEnquiry()
        {
            InitializeComponent();
            InitializeComponent();
            objAnimation = new PalashDynamics.Animations.SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(ItemsEnquiry_Loaded);
           
            SetCommandButtonState("New");
            FillStore();
            FillSupplier();
            //setupPage();
            txtHeader.Text = "Dear Sir, \r \t \t  \t  Please quote your lowest rates for the following as per the \r \t \t \t Terms & Conditions mentioned here below";
            ScarpsAddedItems = new ObservableCollection<clsEnquiryDetailsVO>();

            MasterList = new PagedSortableCollectionView<clsItemEnquiryVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 10;
            this.dataGrid2Pager.DataContext = MasterList;
            this.dgEnquiryList.DataContext = MasterList;
           

        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            setupPage();
        }

        void ItemsEnquiry_Loaded(object sender, RoutedEventArgs e)
        {
            this.dtpFromDate.SelectedDate = DateTime.Now.Date;
            this.dtpToDate.SelectedDate = DateTime.Now.Date;
            setupPage();
        }

        #region Public Methods

        public void setupPage()
        {
            clsGetItemEnquiryBizActionVO BizAction = new clsGetItemEnquiryBizActionVO();
            BizAction.ItemMatserDetail = new List<clsItemEnquiryVO>();
            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

            if (cmbSupplier.SelectedItem != null)
            {
                BizAction.SupplierId = ((MasterListItem)cmbSupplier.SelectedItem).ID;
            }
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value;
            //change by harish 17 apr
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitId = 0;
            }
            else
            {
                BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {


                    BizAction = (clsGetItemEnquiryBizActionVO)args.Result;
                    //dgEnquiryList.ItemsSource = null;
                    MasterList.Clear();
                    MasterList.TotalItemCount = (int)(((clsGetItemEnquiryBizActionVO)args.Result).TotalRows);
                    foreach (clsItemEnquiryVO item in BizAction.ItemMatserDetail)
                    {
                        MasterList.Add(item);
                    }
                    //dgEnquiryList.ItemsSource = null;
                    //dgEnquiryList.ItemsSource = BizAction.ItemMatserDetail;


                }

              };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void setupEnquiryItemsGrid()
        {
            clsGetItemEnquiryDetailsBizActionVO BizAction = new clsGetItemEnquiryDetailsBizActionVO();
            BizAction.ItemMatserDetail = new List<clsEnquiryDetailsVO>();
            BizAction.ItemEnquiryId = EnquiryId;
            BizAction.UnitID = ((clsItemEnquiryVO)dgEnquiryList.SelectedItem).UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {


                    BizAction = (clsGetItemEnquiryDetailsBizActionVO)args.Result;

                    dgEnquiryItemsList.ItemsSource = null;
                    dgEnquiryItemsList.ItemsSource = BizAction.ItemMatserDetail;


                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void ClearUI()
        {
            txtHeader.Text = "";
            txtNotes.Text = "";
            cmbSupplier.SelectedValue = (long)0;
            cboStore.SelectedValue=(long)0 ;
           
            //lstboxSupplier.SelectedItems = null;
            lstboxSupplier.SelectedItem = null;
           // dtEnquiryDate.SelectedDate = null;
            dgEnquiryItems.ItemsSource = null;
            txtHeader.Text = "Dear Sir, \r \t \t  \t  Please quote your lowest rates for the following as per the \r \t \t \tTerms & Conditions mentioned here below";
            ScarpsAddedItems = new ObservableCollection<clsEnquiryDetailsVO>();

        }


        #endregion Public Methods

        #region FillTermsAndConditionGrid

        public void FillTermsAndCondition()
        {
            
            termcondition = new ObservableCollection<clsItemsEnquiryTermConditionVO>();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TermAndCondition;
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
                    
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    foreach (var item in objList)
                    {
                        clsItemsEnquiryTermConditionVO TermConditem = new clsItemsEnquiryTermConditionVO();
                        TermConditem.TermsConditionID = item.ID;
                        TermConditem.TermsCondition = item.Description;
                        termcondition.Add(TermConditem);

                    }
                    
                    dggrdTermsandCondition.ItemsSource = null;
                    dggrdTermsandCondition.ItemsSource = termcondition;


                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        #endregion

        #region Fill ComboBoxes

        //public void FillStore()
        //{


        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_Store;
        //    BizAction.Parent = new KeyValue();
        //    BizAction.Parent.Key = "1";
        //    BizAction.Parent.Value = "Status";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {

        //        if (args.Error == null && args.Result != null)
        //        {

        //            List<MasterListItem> objList = new List<MasterListItem>();



        //            objList.Add(new MasterListItem(0, "-- Select --"));

        //            objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);



        //            cboStore.ItemsSource = null;
        //            cboStore.ItemsSource = objList;


        //        }



        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //Added By Pallavi
        private void FillStore()
        {

            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            List<clsStoreVO> objList = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                   // BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                 
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                 select item;
                    objList = (List<clsStoreVO>)result.ToList();
                    objList.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);

                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, Default);
                    

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cboStore.ItemsSource = result.ToList();
                            cboStore.SelectedItem = result.ToList()[0];
                        }
                    }
                    else
                    {
                        if (BizActionObj.ToStoreList.ToList().Count > 0)
                        {
                            cboStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cboStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                        }
                    }

                }

            };

            client.CloseAsync();

        }
        public void FillSupplier()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Supplier;
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
                   
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    lstboxSupplier.ItemsSource = null;

                    //Change by harish 17 apr
                    //lstboxSupplier.ItemsSource = objList;
                    lstboxSupplier.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList;

                    objList.Insert(0,new MasterListItem(0, "-- Select --"));
                    cmbSupplier.ItemsSource = null;
                    cmbSupplier.ItemsSource = objList;
                    cmbSupplier.SelectedItem = objList[0];

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }




        #endregion

        #region Set Command Button State New/Save
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;

                case "Save":

                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;

                    cmdCancel.IsEnabled = false;
                    break;

                case "Modify":

                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;

                    cmdCancel.IsEnabled = true;
                    break;

                case "ClickNew":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;

                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Click Events
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("ClickNew");
            dtEnquiryDate.SelectedDate = DateTime.Now.Date;

            FillTermsAndCondition();
            ClearUI();
            objAnimation.Invoke(RotationType.Forward);
        }

        int ClickedFlag1 = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 = ClickedFlag1 + 1;
            if (ClickedFlag1 == 1)
            {
                if (ValidationBackPanel())
                {
                    clsAddItemsEnquiryBizActionVO bizActionVO = new clsAddItemsEnquiryBizActionVO();
                    clsItemEnquiryVO EnquiryVO = new clsItemEnquiryVO();
                    try
                    {

                        EnquiryVO.Date = dtEnquiryDate.SelectedDate;
                        EnquiryVO.EnquiryNO = null;
                        EnquiryVO.Header = txtHeader.Text;
                        EnquiryVO.Notes = txtNotes.Text;
                        //Commented By Pallavi
                        //EnquiryVO.StoreID = ((MasterListItem)cboStore.SelectedItem).ID;
                        EnquiryVO.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                        EnquiryVO.Time = System.DateTime.Now;
                        EnquiryVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        EnquiryVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        EnquiryVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        EnquiryVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                        EnquiryVO.DateTime = System.DateTime.Now;
                        EnquiryVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                        bizActionVO.ItemMatserDetail = new clsItemEnquiryVO();
                        bizActionVO.ItemMatserDetail = EnquiryVO;

                        bizActionVO.ItemsDetail = (List<clsEnquiryDetailsVO>)ScarpsAddedItems.ToList<clsEnquiryDetailsVO>();
                        //bizActionVO.ItemsTermsConditionDetail = (List<clsItemsEnquiryTermConditionVO>)termcondition.ToList<clsItemsEnquiryTermConditionVO>();

                        foreach (clsItemsEnquiryTermConditionVO items in termcondition)
                        {
                            clsItemsEnquiryTermConditionVO EnquiryTermsCondition = new clsItemsEnquiryTermConditionVO();
                            if (items.IsCheckedStatus == true)
                            {
                                EnquiryTermsCondition.TermsConditionID = items.TermsConditionID;
                                EnquiryTermsCondition.Remarks = items.Remarks;
                                EnquiryTermsCondition.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                bizActionVO.ItemsTermsConditionDetail.Add(EnquiryTermsCondition);
                            }

                        }

                        foreach (MasterListItem items in lstboxSupplier.SelectedItems)
                        {
                            clsItemsEnquirySupplierVO EnquirySupplier = new clsItemsEnquirySupplierVO();
                            EnquirySupplier.SupplierID = items.ID;
                            EnquirySupplier.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                            bizActionVO.ItemsSupplierDetail.Add(EnquirySupplier);
                        }



                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            ClickedFlag1 = 0;
                            if (args.Error == null && args.Result != null)
                            {
                                if (((clsAddItemsEnquiryBizActionVO)args.Result).SuccessStatus == 1)
                                {
                                    msgText = "Record is successfully submitted with enquiry no." + ((clsAddItemsEnquiryBizActionVO)args.Result).ItemMatserDetail.EnquiryNO;
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();
                                    //After Insertion Back to BackPanel and Setup Page
                                    SetCommandButtonState("Save");
                                    setupPage();
                                    objAnimation.Invoke(RotationType.Backward);
                                }

                            }

                        };
                        client.ProcessAsync(bizActionVO, new clsUserVO());
                        client.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        ClickedFlag1 = 0;
                        throw;
                    }

                }
                else
                    ClickedFlag1 = 0;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //ClearUI();
            SetCommandButtonState("New");
            objAnimation.Invoke(RotationType.Backward);
        }

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            if (cboStore.SelectedItem == null)
            {
                msgText = "Please Select Store!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                cboStore.Focus();
                msgWindow.Show();
               

            }
            else if (((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
            {
                msgText = "Please Select Store!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                cboStore.Focus();
                msgWindow.Show();
               
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

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;
           
            List<clsEnquiryDetailsVO> EnquiryItemsIndatagrid = new List<clsEnquiryDetailsVO>();
             EnquiryItemsIndatagrid = (List<clsEnquiryDetailsVO>)ScarpsAddedItems.ToList<clsEnquiryDetailsVO>();

            if (Itemswin.SelectedItems != null )
            {
                    foreach (var Selitem in Itemswin.SelectedItems)
                    {
                            var item =
                            from p in ScarpsAddedItems
                            where p.ItemId == Selitem.ID
                            select p;
                            if (((List<clsEnquiryDetailsVO>)item.ToList<clsEnquiryDetailsVO>()).Count > 0)
                            {
                                msgText = "Item is Already Added!";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWindow.Show();
                            }
                            else
                            {
                                ScarpsAddedItems.Add(new clsEnquiryDetailsVO() { ItemCode = Selitem.ItemCode, ItemId = (Selitem.ID), ItemName = Selitem.ItemName, UOM = Selitem.PUOM });
                            }
                              
                           
                    }
                   
                }




                dgEnquiryItems.ItemsSource = null;
                dgEnquiryItems.ItemsSource = ScarpsAddedItems;
                
            }
      
        
        #endregion Click Events

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(dggrdTermsandCondition.SelectedItems!=null)
            {
                foreach (var item in dggrdTermsandCondition.SelectedItems)
            	{
		            
	            }
            }
            
        }

        private void dggrdTermsandCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            #region Commented By Shikha
            //FilterExpr = "";
            //if (ValidationFrontPanel())
            //{
            //    if (cmbSupplier.SelectedItem != null)
            //    {
            //        FilterExpr = "SupplierID='" + ((MasterListItem)cmbSupplier.SelectedItem).ID + "' OR";
            //    }
            //    if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            //    {
            //        FilterExpr = FilterExpr + "  Date = '" + dtpFromDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'";
            //    }
            //    if (dtpFromDate.SelectedDate == null && dtpToDate.SelectedDate != null)
            //    {
            //        FilterExpr = FilterExpr + "Date ='" + dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'";
            //    }
            //    if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            //    {
            //        FilterExpr = FilterExpr + "  Date >= '" + dtpFromDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'";
            //    }
            //    if (dtpFromDate.SelectedDate == null && cmbSupplier.SelectedItem != null)
            //    {
            //        FilterExpr = FilterExpr.Substring(0, FilterExpr.Length - 3);
            //    }
            //    if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate != null)
            //    {
            //        FilterExpr = FilterExpr + " and Date <='" + dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'";
            //    }
            //    setupPage();
            //}
            #endregion

            MasterList = new PagedSortableCollectionView<clsItemEnquiryVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 10;
            this.dataGrid2Pager.DataContext = MasterList;
            dgEnquiryList.DataContext = null;
            this.dgEnquiryList.DataContext = MasterList;
            setupPage();
        }

        #region Selection Changed Event

        private void dgEnquiryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgEnquiryItemsList.ItemsSource = null;
            if (dgEnquiryList.SelectedItem != null)
            {
                clsItemEnquiryVO Item = ((clsItemEnquiryVO)dgEnquiryList.SelectedItem).DeepCopy<clsItemEnquiryVO>();
                EnquiryId = Item.EnquiryId;
                setupEnquiryItemsGrid();
            }
        }

        #endregion Selection Changed Event

        private void cmdDeleteEnquiryItems_Click(object sender, RoutedEventArgs e)
        {
            if (dgEnquiryItems.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ScarpsAddedItems.RemoveAt(dgEnquiryItems.SelectedIndex);
                        //CalculateOpeningBalanceSummary();
                    }
                };

                msgWD.Show();
            }
        }

        private void cboStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboStore.SelectedItem != null)
                {
                    if (ScarpsAddedItems != null)
                        ScarpsAddedItems.Clear();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }
    }
}
