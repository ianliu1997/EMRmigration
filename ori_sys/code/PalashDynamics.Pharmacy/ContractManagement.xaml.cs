using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Windows.Browser;
using MessageBoxControl;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Data;
using PalashDynamics.Pharmacy.GRNSearch;

namespace PalashDynamics.Pharmacy
{
    public partial class ContractManagement : UserControl
    {
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        public PagedCollectionView PCVData;
        public ObservableCollection<clsCAGRNDetailsVO> GRNAddedItems { get; set; }
        public ObservableCollection<clsCAGRNDetailsVO> GRNCAAddedItems { get; set; }
        public PagedSortableCollectionView<clsCAGRNVO> DataList { get; private set; }
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
        public ContractManagement()
        {
            InitializeComponent();
            _flip = new PalashDynamics.Animations.SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(ContractManagement_Loaded);

            //FillSupplier();
            //FillStore();
        }

        void ContractManagement_Loaded(object sender, RoutedEventArgs e)
        {
            

            long clinicId = 0; //= ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                clinicId = 0;
            }
            else
            {
                clinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            FillStore(clinicId);
            FillSupplier();
            GRNAddedItems = new ObservableCollection<clsCAGRNDetailsVO>();
            GRNCAAddedItems = new ObservableCollection<clsCAGRNDetailsVO>();
            DataList = new PagedSortableCollectionView<clsCAGRNVO>();
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            SetCommandButtonState("New");
            FillCAGRNSearchList();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
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
                FillCAGRNSearchList();
                //======================================================
            }
        }

        private void FillCAGRNSearchList()
        {
            dgGRNList.ItemsSource = null;
            dgGRNItems.ItemsSource = null;

            clsGetCAListBizActionVO BizAction = new clsGetCAListBizActionVO();
            //BizAction.IsActive = true;
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

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
                    clsGetCAListBizActionVO result = e.Result as clsGetCAListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;

                    if (result.List != null)
                    {
                        DataList.Clear();
                        foreach (var item in result.List)
                        {
                            DataList.Add(item);


                        }

                        dgGRNList.ItemsSource = null;
                        dgGRNList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList;

                    }

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            // SetCommandButtonState("ClickNew");
            
            SetCommandButtonState("Save");
            InitialiseForm();
            _flip.Invoke(RotationType.Forward);
            //  dpInvDt.SelectedDate = DateTime.Today;
            //  cmbReceivedBy.SelectedValue = 0;
        }

        private void InitialiseForm()
        {
            

            this.DataContext = new clsCAGRNVO();

            GRNAddedItems.Clear();
            GRNCAAddedItems.Clear();

            cmbSupplier.SelectedValue = ((clsCAGRNVO)this.DataContext).SupplierID;
            cmbSearchSupplier.SelectedValue = ((clsCAGRNVO)this.DataContext).SupplierID;
            cmbStore.SelectedValue = ((clsCAGRNVO)this.DataContext).StoreID;

            GRNAddedItems = new ObservableCollection<clsCAGRNDetailsVO>();
            GRNCAAddedItems = new ObservableCollection<clsCAGRNDetailsVO>();
            txtServiceAgent.Text = String.Empty;
            txtSerialNo.Text = String.Empty;
            dpContractExpiryDate.SelectedDate = null;
            dpEndDate.SelectedDate = null;
            dpStartDate.SelectedDate = null;
            gdtpFromDate.SelectedDate = null;
            gdtpToDate.SelectedDate = null;
            IsFinalised.IsChecked = false;

        }

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgGRNList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgGRNList.SelectedItem != null)
            {
                FillCADetailslList(((clsCAGRNVO)dgGRNList.SelectedItem).ID);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsCAGRNVO obj = (clsCAGRNVO)dgGRNList.SelectedItem;

                if (obj != null)
                {
                    this.DataContext = obj;
                    ((clsCAGRNVO)this.DataContext).GRNNO = obj.GRNNO;
                    ((clsCAGRNVO)this.DataContext).Date = obj.Date;

                    ((clsCAGRNVO)this.DataContext).StoreID = obj.StoreID;
                    ((clsCAGRNVO)this.DataContext).SupplierID = obj.SupplierID;

                    if (cmbStore.ItemsSource != null)
                    {
                        var results = from r in ((List<MasterListItem>)cmbStore.ItemsSource)
                                      where r.ID == obj.StoreID
                                      select r;

                        foreach (MasterListItem item in results)
                        {
                            cmbStore.SelectedItem = item;
                        }
                    }

                    if (cmbSupplier.ItemsSource != null)
                    {
                        var results1 = from r in ((List<MasterListItem>)cmbSupplier.ItemsSource)
                                       where r.ID == obj.SupplierID
                                       select r;

                        foreach (MasterListItem item in results1)
                        {
                            cmbSupplier.SelectedItem = item;
                        }
                    }

                    ((clsCAGRNVO)this.DataContext).ID = obj.ID;

                    ((clsCAGRNVO)this.DataContext).ServiceAgent = obj.ServiceAgent;
                    txtServiceAgent.Text = Convert.ToString(obj.ServiceAgent);

                    ((clsCAGRNVO)this.DataContext).ContractExpiryDate = obj.ContractExpiryDate;
                    dpContractExpiryDate.SelectedDate = obj.ContractExpiryDate;

                    ((clsCAGRNVO)this.DataContext).SerialNo = obj.SerialNo;
                    txtSerialNo.Text = Convert.ToString(obj.SerialNo);

                    ((clsCAGRNVO)this.DataContext).FromDate = obj.FromDate;
                    dpStartDate.SelectedDate = obj.FromDate;
                    gdtpFromDate.SelectedDate = obj.FromDate;

                    ((clsCAGRNVO)this.DataContext).EndDate = obj.EndDate;
                    dpEndDate.SelectedDate = obj.EndDate;
                    gdtpToDate.SelectedDate = obj.EndDate;

                    ((clsCAGRNVO)this.DataContext).ServiceAgent = obj.ServiceAgent;
                    txtServiceAgent.Text = Convert.ToString(obj.ServiceAgent);

                    ((clsCAGRNVO)this.DataContext).ServiceAgent = obj.ServiceAgent;
                    txtServiceAgent.Text = Convert.ToString(obj.ServiceAgent);

                    ((clsCAGRNVO)this.DataContext).ServiceAgent = obj.ServiceAgent;
                    txtServiceAgent.Text = Convert.ToString(obj.ServiceAgent);

                    ((clsCAGRNVO)this.DataContext).SupplierID = obj.SupplierID;
                    cmbSupplier.SelectedValue = obj.SupplierID;

                    ((clsCAGRNVO)this.DataContext).StoreID = obj.StoreID;
                    cmbStore.SelectedValue = obj.StoreID;

                    ((clsCAGRNVO)this.DataContext).Finalized = obj.Finalized;
                    IsFinalised.IsChecked = obj.Finalized;
                    if ((bool)IsFinalised.IsChecked)
                    {
                        IsFinalised.IsEnabled = false;
                        SetCommandButtonState("Modify");
                    }
                    else
                    {
                        IsFinalised.IsEnabled = true;
                        SetCommandButtonState("Save");
                    }

                   FillCAItems(((clsCAGRNVO)dgGRNList.SelectedItem).ID, ((clsCAGRNVO)dgGRNList.SelectedItem).UnitId);
                    dgCAGRNItems.UpdateLayout();
                    dgCAGRNItems.Focus();

                //    txtPONO.Text = String.Empty;
                    
                   // SetCommandButtonState("Modify");

                    
                    _flip.Invoke(RotationType.Forward);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ViewAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _flip.Invoke(RotationType.Backward);
        }

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdGetGRN_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStore.SelectedItem == null || ((MasterListItem)cmbStore.SelectedItem).ID == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0 || cmbSupplier.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else
            {
                GRNSearchForCA GRNCA = new GRNSearchForCA();
                GRNCA.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                GRNCA.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                GRNCA.OnSaveButton_Click += new RoutedEventHandler(GRNCA_OnSaveButton_Click);
                GRNCA.Show();
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Close");
            _flip.Invoke(RotationType.Backward);
        }

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

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //cmbBloodGroup.ItemsSource = null;
                    //cmbBloodGroup.ItemsSource = objList;
                    cmbSupplier.ItemsSource = null;
                    cmbSupplier.ItemsSource = objList;

                    cmbSearchSupplier.ItemsSource = null;
                    cmbSearchSupplier.ItemsSource = objList;


                    cmbSupplier.SelectedItem = objList[0];
                    cmbSearchSupplier.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbSupplier.SelectedValue = ((clsCAGRNVO)this.DataContext).SupplierID;
                        cmbSearchSupplier.SelectedValue = ((clsCAGRNVO)this.DataContext).SupplierID;

                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillStore(long pClinicID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Store;
                BizAction.MasterList = new List<MasterListItem>();

                if (pClinicID > 0)
                {
                    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
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

                        //if (objList.Count > 1)
                        //    cmbStore.SelectedItem = objList[1];
                        //else
                        cmbStore.SelectedItem = objList[0];
                        if (this.DataContext != null)
                        {
                            cmbStore.SelectedValue = ((clsCAGRNVO)this.DataContext).StoreID;


                        }
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
        int RowCount = 0;
        void GRNCA_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            GRNSearchForCA ItemsGRN = (GRNSearchForCA)sender;
            RowCount = 0;
            if ( ItemsGRN.ItemsSelected != null)
            {
               
                foreach (var item in ItemsGRN.ItemsSelected)
                {
                        clsCAGRNDetailsVO GRNItem = new clsCAGRNDetailsVO();
                        GRNItem.ItemName = item.ItemName;
                        GRNItem.ItemCode = item.ItemCode;
                        GRNItem.GRNUnitID = item.GRNUnitID;
                        GRNItem.ItemID = item.ItemID;
                        GRNItem.UnitId = item.UnitId;
                       // GRNItem.ItemCategory = item.ItemCategory;
                       // GRNItem.ItemGroup = item.ItemGroup;

                         var item5 = from r5 in GRNAddedItems
                                    where r5.ItemCode == item.ItemCode 
                                    select r5;

                        var item6 = from r6 in GRNCAAddedItems
                                    where r6.ItemCode == item.ItemCode 
                                    select r6;

                        // Check the list GRNAddedItem and GRNPOAdded Item contain any element.
                        if ((item5 != null && item5.ToList().Count == 0) || (item6 != null && item6.ToList().Count == 0))
                        {
                            if (GRNAddedItems.Count > 0)
                            {
                                var item2 = from r in GRNAddedItems
                                            where r.ItemCode == item.ItemCode
                                            select r;

                                if (item2 != null && item2.ToList().Count > 0)
                                {
                                    clsCAGRNDetailsVO GRNCopyItem = GRNAddedItems.Where(g => g.ItemCode == item.ItemCode).FirstOrDefault();

                                    if (GRNItem != null)
                                    {
                                        var item3 = from r1 in GRNCAAddedItems
                                                    where r1.ItemCode == item.ItemCode
                                                    select r1;

                                        if (item3 != null && item3.ToList().Count == 0)
                                        {
                                            GRNCAAddedItems.Add(GRNCopyItem.DeepCopy());
                                        }
                                    }
                                }
                                else
                                {
                                    GRNAddedItems.Add(GRNItem);
                                    var item4 = from r2 in GRNCAAddedItems
                                                where r2.ItemCode == item.ItemCode
                                                select r2;

                                    if (item4 != null && item4.ToList().Count == 0)
                                    {
                                        GRNCAAddedItems.Add(GRNItem.DeepCopy());
                                    }

                                }
                            }
                            else
                            {
                                GRNAddedItems.Add(GRNItem);
                                GRNCAAddedItems.Add(GRNItem.DeepCopy());
                            }
                            //GRNAddedItems.Add(GRNItem);
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Item combination already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgW1.Show();
                        }
                   
                }

                
                PCVData = new PagedCollectionView(GRNAddedItems);
                //PCVData.GroupDescriptions.Add(new PropertyGroupDescription("PONO"));
                ObservableCollection<clsCAGRNDetailsVO> ob = new ObservableCollection<clsCAGRNDetailsVO>();
                ob = (ObservableCollection<clsCAGRNDetailsVO>)(PCVData).SourceCollection;
                dgCAGRNItems.ItemsSource = PCVData;

                dgCAGRNItems.Focus();
                dgCAGRNItems.UpdateLayout();
                dgCAGRNItems.SelectedIndex = GRNAddedItems.Count - 1;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (GRNAddedItems == null || GRNAddedItems.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "At least one Item is compulsory for saving Contract Management", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return;
            }

            string msgTitle = "Palash";
            string msgText = "";
            
                msgText = "Are you sure you want to Save Contract Management ?";
            


            MessageBoxControl.MessageBoxChildWindow msgWin =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

            msgWin.Show();
        }

        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {

                SaveCAGRN();
            }
            
        }

        private void SaveCAGRN()
        {
            Boolean blnValidSave = false;

          //  Indicatior = new WaitIndicator();
           // Indicatior.Show();
            if (false)
            {
                try
                {

                    clsCAGRNVO GrnObj = new clsCAGRNVO();
                    GrnObj.ItemsCAGRN = GRNAddedItems.ToList();
                    clsAddCAGRNItemBizActionVO BizAction = new clsAddCAGRNItemBizActionVO();
                    BizAction.Details = new clsCAGRNVO();
                    BizAction.Details = GrnObj;
                    BizAction.ServiceAgent = txtServiceAgent.Text;
                    BizAction.ContracExpirytDate = dpContractExpiryDate.SelectedDate.Value.Date;
                    BizAction.SerialNumber = txtSerialNo.Text;
                    BizAction.StartDate = dpStartDate.SelectedDate.Value.Date;
                    BizAction.EndDate = dpEndDate.SelectedDate.Value.Date;
                    BizAction.Finalised = (bool)IsFinalised.IsChecked;
                    BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizAction.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    BizAction.StoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                    BizAction.GRNUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null && ((clsAddCAGRNItemBizActionVO)arg.Result).Details != null)
                        {
                            if (((clsAddCAGRNItemBizActionVO)arg.Result).Details != null)
                            {
                                //  GRNItemDetails = ((clsAddGRNBizActionVO)arg.Result).Details;

                                //  FillGRNSearchList();
                                string message = "Contract details saved successfully.";

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                // SetCommandButtonState("Save");
                                msgW1.Show();
                                _flip.Invoke(RotationType.Backward);
                            }
                            //Indicatior.Close();
                        }
                        else
                        {
                            // Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding GRN details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();

                }
                catch (Exception)
                {
                    // Indicatior.Close();
                }
            }

            else
            {
                try
                {

                    clsCAGRNVO GrnObj = new clsCAGRNVO();
                    GrnObj.ItemsCAGRN = GRNAddedItems.ToList();
                    clsUpdateCAGRNItemBizActionVO BizAction = new clsUpdateCAGRNItemBizActionVO();
                    BizAction.Details = new clsCAGRNVO();
                    BizAction.Details = GrnObj;
                    BizAction.ID = ((clsCAGRNVO)dgGRNList.SelectedItem).ID;
                    BizAction.ServiceAgent = txtServiceAgent.Text;
                    BizAction.ContracExpirytDate = dpContractExpiryDate.SelectedDate.Value.Date;
                    BizAction.SerialNumber = txtSerialNo.Text;
                    BizAction.StartDate = dpStartDate.SelectedDate.Value.Date;
                    BizAction.EndDate = dpEndDate.SelectedDate.Value.Date;
                    BizAction.Finalised = (bool)IsFinalised.IsChecked;
                    BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizAction.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    BizAction.StoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                    BizAction.GRNUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null && ((clsUpdateCAGRNItemBizActionVO)arg.Result).Details != null)
                        {
                            if (((clsUpdateCAGRNItemBizActionVO)arg.Result).Details != null)
                            {
                                //  GRNItemDetails = ((clsAddGRNBizActionVO)arg.Result).Details;

                                //  FillGRNSearchList();
                                string message = "Contract details saved successfully.";

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                // SetCommandButtonState("Save");
                                msgW1.Show();
                                _flip.Invoke(RotationType.Backward);
                            }
                            //Indicatior.Close();
                        }
                        else
                        {
                            // Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding GRN details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();

                }
                catch (Exception)
                {
                    // Indicatior.Close();
                }
            }
        }

        private void FillCADetailslList(long pContractManagementId)
        {
          

            clsGetCAItemDetailListBizActionVO BizAction = new clsGetCAItemDetailListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.ContractManagementId = pContractManagementId;
            BizAction.UnitId = ((clsCAGRNVO)dgGRNList.SelectedItem).UnitId;

            // if (IsSearchForGRNReturn == true)
            //     BizAction.Freezed = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsCAGRNDetailsVO> objList = new List<clsCAGRNDetailsVO>();

                    objList = ((clsGetCAItemDetailListBizActionVO)e.Result).List;

                    dgGRNItems.ItemsSource = null;
                    dgGRNItems.ItemsSource = objList;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillCAItems(long pGRNID, long UnitId)
        {
            clsGetCADetailsListByIDBizActionVO BizAction = new clsGetCADetailsListByIDBizActionVO();
            BizAction.ContractManagementId = pGRNID;
            BizAction.UnitId = UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsCAGRNDetailsVO> objList = new List<clsCAGRNDetailsVO>();
                    objList = new List<clsCAGRNDetailsVO>();
                    objList = ((clsGetCADetailsListByIDBizActionVO)e.Result).List;
                    GRNAddedItems.Clear();

                    foreach (var item in objList)
                    {

                        GRNAddedItems.Add(item);
                    }

                    dgCAGRNItems.ItemsSource = null;
                    dgCAGRNItems.ItemsSource = GRNAddedItems;
                   
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (IsFinalised.IsChecked == true)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Contract Agreement is finalized you can't delete the item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
                return;
            }

            if (dgCAGRNItems.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to delete the item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        DeleteCAItemDetail(((clsCAGRNDetailsVO)dgCAGRNItems.SelectedItem).ID, ((clsCAGRNVO)dgGRNList.SelectedItem).ID, ((clsCAGRNDetailsVO)dgCAGRNItems.SelectedItem).UnitId);
                        GRNAddedItems.RemoveAt(dgCAGRNItems.SelectedIndex);
                        dgCAGRNItems.Focus();
                        dgCAGRNItems.UpdateLayout();
                        dgCAGRNItems.SelectedIndex = GRNAddedItems.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }

        private void DeleteCAItemDetail(long Id, long ContractManagementId, long UnitId)
        {
            clsDeleteCAByIDBizActionVO BizAction = new clsDeleteCAByIDBizActionVO();
            BizAction.ContractManagementId = ContractManagementId;
            BizAction.ID = Id;
            BizAction.UnitId = UnitId;
            //((clsCAGRNVO)dgGRNList.SelectedItem).ID
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdClose.IsEnabled = false;

                    break;

                case "Save":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdClose.IsEnabled = true;

                    break;

                case "Modify":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false; //false;
                    cmdClose.IsEnabled = true;

                    break;

                case "Close":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdClose.IsEnabled = false;

                    break;

                default:
                    break;
            }
        }

    }
}
