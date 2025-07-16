using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.Animations;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using OPDModule.Forms;
using PalashDynamics.Pharmacy.Inventory;
using PalashDynamics.Administration;
using PalashDynamics.ValueObjects.Administration.UserRights;

namespace PalashDynamics.Pharmacy
{
    public partial class RateContract : UserControl
    {
        private SwivelAnimation objAnimation;
        public ObservableCollection<clsRateContractDetailsVO> SelectedItems { get; set; }
        bool IsCancel = true;


        public string ModuleName { get; set; }
        public string Action { get; set; }
        public RateContract()
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            dgItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgItems_CellEditEnded);

            DataList = new PagedSortableCollectionView<clsRateContractVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        bool IsPageLoaded = false;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
                SetCommandButtonState("Load");
                this.DataContext = new clsRateContractVO();
                SelectedItems = new ObservableCollection<clsRateContractDetailsVO>();
                FillSupplier();
                FillConditions();
                FetchData();
                FillSpplierRepresentative();
                FillClinicRepresentative();
            }
            IsPageLoaded = true;
        }

        //Begin check user rights by Prashant Channe 16/10/2018

        clsUserRightsVO objUser = new clsUserRightsVO();
        //clsRateContractVO objRateContractVO = new clsRateContractVO(); // Added by Prashant Channe on 24/10/2018
        bool IsEditAfterFreeze { get; set; }    // Added by Prashant Channe on 24/10/2018

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
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;
                        //if (objUser.POApprovalLvlID > 0)
                        //{
                        //ApproveFlag += 1;
                        //if (ApproveFlag == 1)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                        //      new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve the PO with " + objUser.POApprovalLvl + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        //    msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                        //    msgWindowUpdate.Show();
                        //}
                        //}
                        //FillPurchaseOrderDataGrid();

                        //ButtonVisibility();

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
        }

        public void ButtonVisibility()
        {
            if ((objUser.IsRCEditOnFreeze && this.IsEditAfterFreeze == true) && chkFreeze.IsChecked == true) // modified by Prashant Channe on 24/10/2018, to check if already Freezeed when checking user rights
            {
                
                cmdModify.IsEnabled = true;
                //dgItems.Columns[dgItems.Columns.Count - 1].Visibility = Visibility.Collapsed; //added by Prashant Channe on 16/10/2018, to hide Delete column from user who has rights to modify already freezed rate contract 
                foreach (var item in dgItems.Columns)
                {
                    if (item.Header != null && item.Header.ToString() == "Reason For Edit")
                    {
                        item.Visibility = Visibility.Visible;
                    }

                    if (item.Header != null && item.Header.ToString() == "Delete")
                    {
                        item.Visibility = Visibility.Collapsed;
                    }
                }
                tbRCReasonForEdit.Visibility = Visibility.Visible;
                txtRCReasonForEdit.Visibility = Visibility.Visible;
                //Reason For Edit
            }
            else
            {
                cmdModify.IsEnabled = false;
                dgItems.Columns[dgItems.Columns.Count - 2].Visibility = Visibility.Collapsed; //added by Prashant Channe on 17/10/2018, to hide ReasonForEdit column from user who has no rights to modify already freezed rate contract 
            }

        }

        //End check user rights by Prashant Channe 16/10/2018

        private void SetDateValidation()
        {
            dtpFromDate.DisplayDateStart = DateTime.Now.Date;
            dtpToDate.DisplayDateStart = DateTime.Now.Date;
            dtpContractDate.DisplayDate = DateTime.Now.Date;
            dtpContractDate.DisplayDateEnd = DateTime.Now.Date;
            dtpFDate.DisplayDate = DateTime.Now.Date;
            dtpTDate.DisplayDateStart = dtpFDate.SelectedDate;

            dtpContractDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(1), DateTime.MaxValue));
        }

        void dgItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (dgItems.SelectedItem != null)
            {
                clsRateContractDetailsVO objRCDetails = new clsRateContractDetailsVO();
                objRCDetails = dgItems.SelectedItem as clsRateContractDetailsVO;

                if (e.Column.Header.ToString().Equals("Purchase Cost Price"))
                {
                    objRCDetails.MainRate = Convert.ToSingle(objRCDetails.Rate) / objRCDetails.BaseConversionFactor;
                    objRCDetails.BaseRate = Convert.ToSingle(objRCDetails.Rate) / objRCDetails.BaseConversionFactor;
                }

                if (e.Column.Header.ToString().Equals("M.R.P"))
                {
                    objRCDetails.MainMRP = Convert.ToSingle(objRCDetails.MRP) / objRCDetails.BaseConversionFactor;
                    objRCDetails.BaseMRP = Convert.ToSingle(objRCDetails.MRP) / objRCDetails.BaseConversionFactor;
                }
                Calculatesummary();
            }

        }

        #region Paging

        public PagedSortableCollectionView<clsRateContractVO> DataList { get; private set; }

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



        #endregion
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


                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cmbFSupplier.ItemsSource = null;
                        cmbFSupplier.ItemsSource = objList;
                        cmbFSupplier.SelectedItem = objList[0];

                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;
                        cmbSupplier.SelectedItem = objList[0];
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

        private void FillClinicRepresentative()
        {
            clsGetStaffMasterDetailsBizActionVO BizAction = new clsGetStaffMasterDetailsBizActionVO();
            BizAction.StaffMasterList = new List<clsStaffMasterVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    if (((clsGetStaffMasterDetailsBizActionVO)e.Result).StaffMasterList != null)
                    {
                        clsGetStaffMasterDetailsBizActionVO result = e.Result as clsGetStaffMasterDetailsBizActionVO;

                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        foreach (var item in result.StaffMasterList)
                        {
                            MasterListItem Obj = new MasterListItem();
                            Obj.ID = item.ID;
                            Obj.Description = (item.FirstName + " " + item.MiddleName + " " + item.LastName);
                            Obj.Status = item.Status;
                            objList.Add(Obj);
                        }


                        cmbClinicRepresentative.ItemsSource = null;
                        cmbClinicRepresentative.ItemsSource = objList;
                        cmbClinicRepresentative.SelectedItem = objList[0];
                    }
                }


            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        List<string> Representative = new List<string>();
        private void FillSpplierRepresentative()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            BizAction.TableName = "M_Supplier";
            BizAction.ColumnName = "ContactPerson1Name";

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    if (Representative == null)
                        Representative = new List<string>();

                    Representative = ((clsGetAutoCompleteListVO)e.Result).List;
                    FillSpplierRepresentative2();

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSpplierRepresentative2()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            BizAction.TableName = "M_Supplier";
            BizAction.ColumnName = "ContactPerson2Name";

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    if (Representative == null)
                        Representative = new List<string>();

                    if (((clsGetAutoCompleteListVO)e.Result).List != null && ((clsGetAutoCompleteListVO)e.Result).List.Count > 0)
                    {
                        foreach (var item in ((clsGetAutoCompleteListVO)e.Result).List)
                        {
                            Representative.Add(item);

                        }
                    }

                    cmbSupplierRepresentative.ItemsSource = null;
                    cmbSupplierRepresentative.ItemsSource = Representative;

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        List<MasterListItem> ConditionList = new List<MasterListItem>();
        public void FillConditions()
        {
            ConditionList = new List<MasterListItem>();
            ConditionList.Add(new MasterListItem(0, "-- Select --"));
            ConditionList.Add(new MasterListItem(1, "="));
            ConditionList.Add(new MasterListItem(2, "<"));
            ConditionList.Add(new MasterListItem(3, "Between"));
            ConditionList.Add(new MasterListItem(4, ">"));
            ConditionList.Add(new MasterListItem(5, "No Limit"));


        }
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            FreezeControls(true);
            SetCommandButtonState("New");
            SetDateValidation();
            dtpFromDate.SelectedDate = DateTime.Today;
            dtpToDate.DisplayDateStart = dtpFromDate.SelectedDate;
            chkFreeze.IsEnabled = true;
            chkFreeze.IsChecked = false;
            objAnimation.Invoke(RotationType.Forward);
            dgItems.Columns[dgItems.Columns.Count - 1].Visibility = Visibility.Visible; //added by Prashant Channe on 25/10/2018, to show Delete column from user who has rights to modify already freezed rate contract 
            dgItems.Columns[dgItems.Columns.Count - 2].Visibility = Visibility.Collapsed; //added by Prashant Channe on 25/10/2018, to hide ReasonForEdit column from user who has no rights to modify already freezed rate contract 
            tbRCReasonForEdit.Visibility = Visibility.Collapsed;
            txtRCReasonForEdit.Visibility = Visibility.Collapsed;
        }

        int ClickedFlag1 = 0;
        
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 = ClickedFlag1 + 1;
            if (ClickedFlag1 == 1)
            {
                if (Validation())
                {
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Save Rate Contract?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                    msgWin.Show();
                }
            }
        }

        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
            else if (result == MessageBoxResult.No)
                ClickedFlag1 = 0;
        }

        private void Save()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            clsAddRateContractBizActionVO BizAction = new clsAddRateContractBizActionVO();
            BizAction.RateContract = (clsRateContractVO)this.DataContext;

            if (cmbSupplier.SelectedItem != null)
                BizAction.RateContract.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;

            if (cmbClinicRepresentative.SelectedItem != null)
                BizAction.RateContract.ClinicRepresentativeID = ((MasterListItem)cmbClinicRepresentative.SelectedItem).ID;
            BizAction.RateContract.ContractDetails = SelectedItems.ToList();

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client.ProcessCompleted += (sObjPatientEMRData, argsObjPatientEMRData) =>
            {
                if (argsObjPatientEMRData.Error == null)
                {
                    if (((clsAddRateContractBizActionVO)argsObjPatientEMRData.Result).SuccessStatus == 1)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        objAnimation.Invoke(RotationType.Backward);
                        SetCommandButtonState("Save");
                        ClickedFlag1 = 0;
                        FetchData();
                    }
                    else if (((clsAddRateContractBizActionVO)argsObjPatientEMRData.Result).SuccessStatus == 2)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                        ClickedFlag1 = 0;
                    }
                    else if (((clsAddRateContractBizActionVO)argsObjPatientEMRData.Result).SuccessStatus == 3)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                        ClickedFlag1 = 0;
                    }
                    Indicatior.Close();
                }
                else
                {
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while saving record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        
        
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if ((dgItems.ItemsSource) != null)
            {
                ClickedFlag1 = ClickedFlag1 + 1;                              

                if (ClickedFlag1 == 1)
                {
                    if (Validation())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Modify Rate Contract?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin1_OnMessageBoxClosed);

                        msgWin.Show();
                    }
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Atleast One Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void msgWin1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Modify();
            }
            else if (result == MessageBoxResult.No)
                ClickedFlag1 = 0;
        }

        private void Modify()
        {

            WaitIndicator Indicatior = new WaitIndicator();
            clsAddRateContractBizActionVO BizAction = new clsAddRateContractBizActionVO();
            BizAction.RateContract = (clsRateContractVO)this.DataContext;

            if (cmbSupplier.SelectedItem != null)
                BizAction.RateContract.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;

            if (cmbClinicRepresentative.SelectedItem != null)
                BizAction.RateContract.ClinicRepresentativeID = ((MasterListItem)cmbClinicRepresentative.SelectedItem).ID;

            BizAction.RateContract.ContractDetails = SelectedItems.ToList();

            if (objUser.IsRCEditOnFreeze == true)   // Added by Prashant Channe on 24Oct2018 for Rate Contract Edit After Freeze functionality
                BizAction.RateContract.IsEditAfterFreeze = this.IsEditAfterFreeze;

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client.ProcessCompleted += (sObjPatientEMRData, argsObjPatientEMRData) =>
            {
                if (argsObjPatientEMRData.Error == null)
                {
                    if (((clsAddRateContractBizActionVO)argsObjPatientEMRData.Result).SuccessStatus == 1)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Rate Contract Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        objAnimation.Invoke(RotationType.Backward);
                        SetCommandButtonState("Modify");
                        ClickedFlag1 = 0;
                        FetchData();
                    }
                    else if (((clsAddRateContractBizActionVO)argsObjPatientEMRData.Result).SuccessStatus == 2)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Contract cannot be update because CODE already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                    else if (((clsAddRateContractBizActionVO)argsObjPatientEMRData.Result).SuccessStatus == 3)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Contract cannot be update because DESCRIPTION already exist!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                    }
                    Indicatior.Close();
                }
                else
                {
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FetchData()
        {
            clsGetRateContractBizActionVO BizAction = new clsGetRateContractBizActionVO();
            BizAction.RateContract = new List<clsRateContractVO>();
            BizAction.Code = txtFCode.Text;
            BizAction.Description = txtFDescription.Text;
            if (cmbFSupplier.SelectedItem != null)
                BizAction.SupplierID = ((MasterListItem)cmbFSupplier.SelectedItem).ID;

            if (dtpFDate.SelectedDate != null)
                BizAction.FromDate = dtpFDate.SelectedDate;


            if (dtpTDate.SelectedDate != null)
                BizAction.ToDate = dtpTDate.SelectedDate;

            BizAction.PagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetRateContractBizActionVO)arg.Result).RateContract != null)
                    {
                        clsGetRateContractBizActionVO result = arg.Result as clsGetRateContractBizActionVO;
                        DataList.TotalItemCount = result.TotalRowCount;

                        if (result.RateContract != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.RateContract)
                            {
                                DataList.Add(item);
                            }

                            dgContract.ItemsSource = null;
                            dgContract.ItemsSource = DataList;

                            datapager.Source = null;
                            datapager.PageSize = BizAction.MaximumRows;
                            datapager.Source = DataList;

                        }

                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {


            SetCommandButtonState("Cancel");

            objAnimation.Invoke(RotationType.Backward);

            if (FrontPanel.Visibility == Visibility.Visible)
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

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            ItemListWithCategory ItemsWin = new ItemListWithCategory();
            ItemsWin.StoreID = 0;  //((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathologyStoreID;
            ItemsWin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
            ItemsWin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            ItemsWin.ShowBatches = false;
            ItemsWin.cmbStore.IsEnabled = false;
            ItemsWin.ShowZeroStockBatches = false;

            ItemsWin.IsCategoryOn = false;
            ItemsWin.IsGroupOn = false;
            ItemsWin.rdbItemsCategory.IsEnabled = false;
            ItemsWin.rdbItemsGroup.IsEnabled = false;
            ItemsWin.brdItemFilter.Visibility = Visibility.Collapsed;
            ItemsWin.OnSaveButton_Click += new RoutedEventHandler(Itemwin_OnSaveButton_Click);
            ItemsWin.Show();

            //ItemList Itemwin = new ItemList();
            //Itemwin.ShowBatches = false;
            //Itemwin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
            //Itemwin.AllowStoreSelection = true;
            //Itemwin.StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyStoreID;
            //Itemwin.OnSaveButton_Click += new RoutedEventHandler(Itemwin_OnSaveButton_Click);
            //Itemwin.Show();
        }

        void Itemwin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemListWithCategory Itemswin = (ItemListWithCategory)sender; //ItemList Itemswin = (ItemList)sender;
            if (Itemswin.SelectedItems != null)
            {
                long lngContractID = 0;
                long lngContractUnitID = 0;
                if (dgContract != null && dgContract.SelectedItem != null)
                {
                    lngContractID = ((clsRateContractVO)dgContract.SelectedItem).ID;
                    lngContractUnitID = ((clsRateContractVO)dgContract.SelectedItem).UnitId;
                }
                foreach (var item in Itemswin.SelectedItems)
                {
                    clsRateContractDetailsVO objRC = new clsRateContractDetailsVO();
                    objRC.ItemID = item.ID;
                    objRC.ItemCode = item.ItemCode;
                    objRC.ItemName = item.ItemName;
                    objRC.HSNCode = item.HSNCodes;
                    objRC.Rate = item.PurchaseRate * Convert.ToDecimal(item.PurchaseToBaseCF);
                    objRC.MRP = item.MRP * Convert.ToDecimal(item.PurchaseToBaseCF);
                    objRC.BaseRate = Convert.ToSingle(item.PurchaseRate);
                    objRC.BaseMRP = Convert.ToSingle(item.MRP);
                    objRC.MainRate = Convert.ToSingle(item.PurchaseRate);
                    objRC.MainMRP = Convert.ToSingle(item.MRP);
                    objRC.SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM };
                    objRC.ConversionFactor = item.PurchaseToBaseCF / item.StockingToBaseCF;
                    objRC.SUOM = item.SUOM;
                    objRC.SUOMID = item.SUM;

                    objRC.BaseConversionFactor = item.PurchaseToBaseCF;
                    objRC.BaseUOMID = item.BaseUM;
                    objRC.BaseUOM = item.BaseUMString;

                    objRC.PUOM = item.PUOM;
                    //objRC.SUOM = item.SUOM;
                    objRC.InclusiveOfTax = item.InclusiveOfTax;
                    objRC.EnableInclusiveOfTax = item.InclusiveOfTax == false ? true : false;
                    objRC.VATPercent = item.VatPer;
                    //objRC.ConversionFactor = Convert.ToSingle(item.ConversionFactor);
                    objRC.ConditionList = ConditionList;
                    objRC.ContractID = lngContractID;
                    objRC.ContractUnitId = lngContractUnitID;
                    objRC.ManufactureCompany = item.Manufacturer;
                    SelectedItems.Add(objRC);
                }
                dgItems.ItemsSource = null;
                dgItems.ItemsSource = SelectedItems;
                Calculatesummary();
                ConditionWiseEnableDisable();
            }
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();

            if (dgContract.SelectedItem != null)
            {
                SetCommandButtonState("View");
                //dtpFromDate = null;
                //dtpFromDate = new DatePicker();
                //dtpToDate = null;
                //dtpToDate = new DatePicker();

                dtpFromDate.SelectedDate = null;
                dtpToDate.SelectedDate = null;

                this.DataContext = ((clsRateContractVO)dgContract.SelectedItem).DeepCopy();

                cmbSupplier.SelectedValue = ((clsRateContractVO)this.DataContext).SupplierID;
                cmbClinicRepresentative.SelectedValue = ((clsRateContractVO)this.DataContext).ClinicRepresentativeID;

                dtpToDate.SelectedDate = ((clsRateContractVO)this.DataContext).ToDate;
                dtpFromDate.SelectedDate = ((clsRateContractVO)this.DataContext).FromDate;

                if (((clsRateContractVO)this.DataContext).IsFreeze == true)
                {
                    cmdModify.IsEnabled = false;
                    chkFreeze.IsEnabled = false;

                    if (objUser.IsRCEditOnFreeze == true && (dgContract.SelectedItem != null && ((clsRateContractVO)dgContract.SelectedItem).IsFreeze == true))   //added by Prashant channe on 24/10/2018
                        this.IsEditAfterFreeze = true;

                    ButtonVisibility(); //To check user rights and accordinly allow Modification for already freezed Rate Contract, added by Prashant channe on 16/10/2018

                }
                else
                {
                    chkFreeze.IsEnabled = true;

                }
                FillItemDetail();
                Boolean blnFreeze = ((clsRateContractVO)dgContract.SelectedItem).IsFreeze;
                FreezeControls(!blnFreeze);
                
                objAnimation.Invoke(RotationType.Forward);
            }

        }
        private void FreezeControls(Boolean blnEnable)
        {
            dtpToDate.IsEnabled = blnEnable;            
            dtpFromDate.IsEnabled = blnEnable;
            dtpContractDate.IsEnabled = blnEnable;
            txtTotalAmt.IsEnabled = blnEnable;
            txtCode.IsEnabled = blnEnable;
            txtDescription.IsEnabled = blnEnable;
            cmbSupplier.IsEnabled = blnEnable;
            //dgItems.IsReadOnly = blnEnable;
            cmbSupplierRepresentative.IsEnabled = blnEnable;
            cmbClinicRepresentative.IsEnabled = blnEnable;
            txtRemarks.IsEnabled = blnEnable;
            lnkAddItems.IsEnabled = blnEnable;
        }

        private void FillItemDetail()
        {
            WaitIndicator Ind = new WaitIndicator();
            Ind.Show();
            clsGetRateContractItemDetailsBizActionVO BizAction = new clsGetRateContractItemDetailsBizActionVO();
            BizAction.ContractID = ((clsRateContractVO)dgContract.SelectedItem).ID;
            BizAction.ContractUnitId = ((clsRateContractVO)dgContract.SelectedItem).UnitId;
            dtpFromDate.DisplayDateStart = null;
            dtpToDate.DisplayDateStart = null;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetRateContractItemDetailsBizActionVO)arg.Result).RateContractList != null)
                    {
                        clsGetRateContractItemDetailsBizActionVO result = arg.Result as clsGetRateContractItemDetailsBizActionVO;
                        if (result.RateContractList != null && result.RateContractList.Count > 0)
                        {
                            foreach (var item in result.RateContractList)
                            {
                                item.ConditionList = ConditionList;
                                SelectedItems.Add(item);
                            }
                            ConditionWiseEnableDisable();
                        }

                        dgItems.ItemsSource = SelectedItems;
                        Ind.Close();

                    }
                    else
                    {
                        Ind.Close();
                    }
                }
                else
                {
                    Ind.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void ConditionWiseEnableDisable()
        {
            try
            {
                foreach (var rowItem in dgItems.ItemsSource)
                {
                    clsRateContractDetailsVO objRateContractDetails = ((clsRateContractDetailsVO)rowItem);
                    int iConditionID = Convert.ToInt32(objRateContractDetails.SelectedCondition.ID);
                    if (iConditionID == 3) // Condition Between.
                    {
                        FrameworkElement fe = dgItems.Columns[3].GetCellContent(rowItem);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                        if (result != null)
                        {
                            DataGridCell cell = (DataGridCell)result;
                            ((TextBox)cell.Content).Text = string.Empty;
                            cell.IsEnabled = false;
                        }

                        FrameworkElement fe1 = dgItems.Columns[4].GetCellContent(rowItem);
                        FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));
                        if (result1 != null)
                        {
                            DataGridCell cell = (DataGridCell)result1;
                            cell.IsEnabled = true;
                        }

                        FrameworkElement fe3 = dgItems.Columns[5].GetCellContent(rowItem);
                        FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

                        if (result3 != null)
                        {
                            DataGridCell cell = (DataGridCell)result3;
                            cell.IsEnabled = true;
                        }
                        objRateContractDetails.UnlimitedQuantity = false;

                        objRateContractDetails.Quantity = 0;
                    }

                    else if (iConditionID == 5)
                    {
                        objRateContractDetails.Quantity = 0;
                        objRateContractDetails.MaxQuantity = 0;
                        objRateContractDetails.MinQuantity = 0;


                        DataGridColumn column = dgItems.Columns[3];
                        FrameworkElement fe = column.GetCellContent(rowItem);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        if (result != null)
                        {
                            DataGridCell cell = (DataGridCell)result;
                            ((TextBox)cell.Content).Text = string.Empty;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column2 = dgItems.Columns[4];
                        FrameworkElement fe2 = column2.GetCellContent(rowItem);
                        FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                        if (result2 != null)
                        {
                            DataGridCell cell = (DataGridCell)result2;
                            ((TextBox)cell.Content).Text = string.Empty;
                            cell.IsEnabled = false;
                        }


                        DataGridColumn column3 = dgItems.Columns[5];
                        FrameworkElement fe3 = column3.GetCellContent(rowItem);
                        FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

                        if (result3 != null)
                        {
                            DataGridCell cell = (DataGridCell)result3;
                            ((TextBox)cell.Content).Text = string.Empty;
                            cell.IsEnabled = false;
                        }
                    }
                    else if (iConditionID != 3 && iConditionID != 0)
                    {
                        FrameworkElement fe4 = dgItems.Columns[3].GetCellContent(rowItem);
                        FrameworkElement result4 = GetParent(fe4, typeof(DataGridCell));
                        if (result4 != null)
                        {
                            DataGridCell cell = (DataGridCell)result4;
                            cell.IsEnabled = true;
                        }

                        FrameworkElement fe = dgItems.Columns[4].GetCellContent(rowItem);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        if (result != null)
                        {
                            DataGridCell cell = (DataGridCell)result;
                            ((TextBox)cell.Content).Text = string.Empty;
                            cell.IsEnabled = false;
                        }


                        FrameworkElement fe1 = dgItems.Columns[5].GetCellContent(rowItem);
                        FrameworkElement result2 = GetParent(fe1, typeof(DataGridCell));

                        if (result2 != null)
                        {
                            DataGridCell cell = (DataGridCell)result2;
                            ((TextBox)cell.Content).Text = string.Empty;
                            cell.IsEnabled = false;
                        }
                        objRateContractDetails.MinQuantity = 0;
                        objRateContractDetails.MaxQuantity = 0;
                        objRateContractDetails.UnlimitedQuantity = false;
                    }
                    else
                    {

                        FrameworkElement fe = dgItems.Columns[3].GetCellContent(rowItem);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                        if (result != null)
                        {
                            DataGridCell cell = (DataGridCell)result;
                            cell.IsEnabled = true;
                        }

                        FrameworkElement fe2 = dgItems.Columns[4].GetCellContent(rowItem);
                        FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                        if (result2 != null)
                        {
                            DataGridCell cell = (DataGridCell)result2;
                            cell.IsEnabled = true;
                        }

                        FrameworkElement fe3 = dgItems.Columns[5].GetCellContent(rowItem);
                        FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

                        if (result3 != null)
                        {
                            DataGridCell cell = (DataGridCell)result3;
                            cell.IsEnabled = true;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgItems.SelectedItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Delete the selected Item ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SelectedItems.RemoveAt(dgItems.SelectedIndex);
                    }
                };

                msgWD.Show();
            }

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsRateContractVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            FetchData();
        }

        private void dgContract_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgContract.SelectedItem != null)
            {
                clsGetRateContractItemDetailsBizActionVO BizAction = new clsGetRateContractItemDetailsBizActionVO();
                BizAction.ContractID = ((clsRateContractVO)dgContract.SelectedItem).ID;
                BizAction.ContractUnitId = ((clsRateContractVO)dgContract.SelectedItem).UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {

                        if (((clsGetRateContractItemDetailsBizActionVO)arg.Result).RateContractList != null)
                        {
                            clsGetRateContractItemDetailsBizActionVO result = arg.Result as clsGetRateContractItemDetailsBizActionVO;

                            if (result.RateContractList != null && result.RateContractList.Count > 0)
                            {
                                dgContractItem.ItemsSource = result.RateContractList;
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
                dgContractItem.ItemsSource = null;

        }

        private void chkMultipleQty_Click(object sender, RoutedEventArgs e)
        {
            if (((clsRateContractDetailsVO)dgItems.SelectedItem).UnlimitedQuantity == true)
            {
                ((clsRateContractDetailsVO)dgItems.SelectedItem).Quantity = 0;
                ((clsRateContractDetailsVO)dgItems.SelectedItem).MaxQuantity = 0;
                ((clsRateContractDetailsVO)dgItems.SelectedItem).MinQuantity = 0;
                ((clsRateContractDetailsVO)dgItems.SelectedItem).SelectedCondition = new MasterListItem { ID = 0, Description = "--Select--" };

                DataGridColumn column = dgItems.Columns[3];
                FrameworkElement fe = column.GetCellContent(dgItems.SelectedItem);
                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                if (result != null)
                {
                    DataGridCell cell = (DataGridCell)result;

                    cell.IsEnabled = false;
                }

                DataGridColumn column2 = dgItems.Columns[4];
                FrameworkElement fe2 = column2.GetCellContent(dgItems.SelectedItem);
                FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                if (result2 != null)
                {
                    DataGridCell cell = (DataGridCell)result2;
                    cell.IsEnabled = false;
                }


                DataGridColumn column3 = dgItems.Columns[5];
                FrameworkElement fe3 = column3.GetCellContent(dgItems.SelectedItem);
                FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

                if (result3 != null)
                {
                    DataGridCell cell = (DataGridCell)result3;
                    cell.IsEnabled = false;
                }


                DataGridColumn column4 = dgItems.Columns[6];
                FrameworkElement fe4 = column4.GetCellContent(dgItems.SelectedItem);
                FrameworkElement result4 = GetParent(fe4, typeof(DataGridCell));

                if (result4 != null)
                {
                    DataGridCell cell = (DataGridCell)result4;
                    cell.IsEnabled = false;
                }

            }
            else
            {
                DataGridColumn column = dgItems.Columns[3];
                FrameworkElement fe = column.GetCellContent(dgItems.SelectedItem);
                FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                if (result != null)
                {
                    DataGridCell cell = (DataGridCell)result;
                    cell.IsEnabled = true;
                }

                DataGridColumn column2 = dgItems.Columns[4];
                FrameworkElement fe2 = column2.GetCellContent(dgItems.SelectedItem);
                FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                if (result2 != null)
                {
                    DataGridCell cell = (DataGridCell)result2;
                    cell.IsEnabled = true;
                }


                DataGridColumn column3 = dgItems.Columns[5];
                FrameworkElement fe3 = column3.GetCellContent(dgItems.SelectedItem);
                FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

                if (result3 != null)
                {
                    DataGridCell cell = (DataGridCell)result3;
                    cell.IsEnabled = true;
                }


                DataGridColumn column4 = dgItems.Columns[6];
                FrameworkElement fe4 = column4.GetCellContent(dgItems.SelectedItem);
                FrameworkElement result4 = GetParent(fe4, typeof(DataGridCell));

                if (result4 != null)
                {
                    DataGridCell cell = (DataGridCell)result4;
                    cell.IsEnabled = true;
                }
            }
        }

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        void Calculatesummary()
        {
            decimal ContractValue = 0;
            foreach (var item in SelectedItems.ToList())
            {
                ContractValue += item.NetAmount;
            }
            txtTotalAmt.Text = ContractValue.ToString();
            (DataContext as clsRateContractVO).ContractValue = ContractValue;
        }

        private bool Validation()
        {
            bool Result = true;

            if (dtpToDate.SelectedDate == null)
            {
                    dtpToDate.SetValidation("Please select Contract End Date");
                    dtpToDate.RaiseValidationError();
                    dtpToDate.Focus();
                    Result = false;
                    ClickedFlag1 = 0;                
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpFromDate.SelectedDate == null)
            {
                    dtpFromDate.SetValidation("Please select Contract Start Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    Result = false;
                    ClickedFlag1 = 0;                
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }
            if (cmbSupplier.SelectedItem == null || ((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
            {             
                    cmbSupplier.TextBox.SetValidation("Please select Supplier");
                    cmbSupplier.TextBox.RaiseValidationError();
                    cmbSupplier.Focus();
                    Result = false;
                    ClickedFlag1 = 0;             
            }
            else
            {
                cmbSupplier.TextBox.ClearValidationError();
            }
            if (dtpContractDate.SelectedDate == null)
            {               
                dtpContractDate.SetValidation("Please select Contarct Date");
                dtpContractDate.RaiseValidationError();
                dtpContractDate.Focus();
                Result = false;
                ClickedFlag1 = 0;
            }
            else
            {
                dtpContractDate.ClearValidationError();
            }
            if (txtDescription.Text.Trim() == "")
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                Result = false;
                ClickedFlag1 = 0;
            }
            else if (txtDescription.Text == null)
            {
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                Result = false;
                ClickedFlag1 = 0;
            }
            else
            {
                txtDescription.ClearValidationError();
            }

            if (objUser.IsRCEditOnFreeze == true && this.IsEditAfterFreeze == true)// Added by Prashant Channe on 23/10/2018, to check if ReasonForEdit is not null or empty
            {
                if (String.IsNullOrEmpty(txtRCReasonForEdit.Text.Trim()))
                {
                    txtRCReasonForEdit.SetValidation("Please Enter Reason for Edit");
                    txtRCReasonForEdit.RaiseValidationError();
                    txtRCReasonForEdit.Focus();
                    Result = false;
                    ClickedFlag1 = 0;
                }
                else
                {
                    txtRCReasonForEdit.ClearValidationError();
                }
            }

            //if (txtCode.Text.Trim() == "")
            //{
            //    txtCode.SetValidation("Please Enter Code");
            //    txtCode.RaiseValidationError();
            //    txtCode.Focus();
            //    Result = false;
            //    ClickedFlag1 = 0;
            //}
            //else if (String.IsNullOrEmpty(txtCode.Text))
            //{
            //    txtCode.SetValidation("Please Enter Code");
            //    txtCode.RaiseValidationError();
            //    txtCode.Focus();
            //    Result = false;
            //    ClickedFlag1 = 0;
            //}
            //else
            //{
            //    txtCode.ClearValidationError();
            //}

            if (IsPageLoaded)
            {

                
                if (SelectedItems.Count <= 0)
                {
                    
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "You can not save the rate contract without Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWD.Show();
                        Result = false;
                        ClickedFlag1 = 0;
                    
                }

                if (objUser.IsRCEditOnFreeze == true && this.IsEditAfterFreeze == true) // Added by Prashant Channe on 28/10/2018, to skip the date validation for user with RCEditOnFreeze rights
                {
                    //skip date validation
                }
                else
                {
                    if (dtpContractDate.SelectedDate > dtpToDate.SelectedDate)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Contract Date must be less than Contract End Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWD.Show();
                        Result = false;
                        ClickedFlag1 = 0;
                        return Result;

                    }
                    if (dtpContractDate.SelectedDate > dtpFromDate.SelectedDate)
                    {


                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Contract  Date must be less than or Equal to Contract Start Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWD.Show();
                        Result = false;
                        ClickedFlag1 = 0;
                        return Result;

                    }
                    if (dtpFromDate.SelectedDate < DateTime.Today)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Contract  from date must be greater than or Equal to today's Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWD.Show();
                        Result = false;
                        ClickedFlag1 = 0;
                        return Result;
                    }
                    if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Contract Start Date must be less than Contract End Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWD.Show();
                        Result = false;
                        ClickedFlag1 = 0;
                        return Result;

                    }
                }

                //if (SelectedItems.Where(Items => Items.UnlimitedQuantity == false).Any() == true)
                //{
                //    if (SelectedItems.Where(Items => Items.SelectedCondition == null).Any() == true)
                //    {
                //        MessageBoxControl.MessageBoxChildWindow msgWD =
                //                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Condition", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                //        msgWD.Show();
                //        Result = false;
                //        ClickedFlag1 = 0;
                //        return Result;
                //    }

                //    if (SelectedItems.Where(Items => Items.SelectedCondition.Description.Equals("--Select--")).Any() == true)
                //    {
                //        MessageBoxControl.MessageBoxChildWindow msgWD =
                //                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Condition for : " + SelectedItems.Where(Items => Items.SelectedCondition.ID == 0).FirstOrDefault().ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                //        msgWD.Show();
                //        Result = false;
                //        ClickedFlag1 = 0;
                //        return Result;
                //    }

                //}

                long Flag = 0;
                clsRateContractDetailsVO objDetails = new clsRateContractDetailsVO();
                foreach (var item in SelectedItems)
                {
                    if (item.Rate < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Rate for : " + objDetails.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        Result = false;
                        ClickedFlag1 = 0;
                        return Result;
                    }

                    if (item.MRP < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter MRP for : " + objDetails.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD1.Show();
                        Result = false;
                        ClickedFlag1 = 0;
                        return Result;
                    }

                    //if (item.SelectedCondition.ID > 0 && item.SelectedCondition.ID == 3 && item.MinQuantity < 1)
                    //{
                    //    Flag = 1;
                    //    objDetails = item;
                    //    break;
                    //}
                    //if (item.SelectedCondition.ID > 0 && item.SelectedCondition.ID == 3 && item.MaxQuantity < 1)
                    //{
                    //    Flag = 2;
                    //    objDetails = item;
                    //    break;
                    //}

                    //if (item.SelectedCondition.ID > 0 && item.SelectedCondition.ID != 3 && item.SelectedCondition.ID != 5 && item.Quantity < 1)
                    //{
                    //    Flag = 3;
                    //    objDetails = item;
                    //    break;
                    //}
                    //if (item.SelectedCondition.ID > 0 && item.SelectedCondition.ID == 3 && item.MinQuantity > item.MaxQuantity)
                    //{
                    //    Flag = 4;
                    //    objDetails = item;
                    //    item.MinQuantity = 0;
                    //    break;
                    //}
                }
                //if (Flag == 1)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgWD =
                //              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Min Quantity for : " + objDetails.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgWD.Show();
                //    Result = false;
                //    ClickedFlag1 = 0;
                //    return Result;
                //}
                //if (Flag == 2)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgWD =
                //                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Max Quantity for :" + objDetails.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgWD.Show();
                //    Result = false;
                //    ClickedFlag1 = 0;
                //    return Result;
                //}

                //if (Flag == 3)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgWD =
                //                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Quantity for :" + objDetails.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                //    msgWD.Show();
                //    Result = false;
                //    ClickedFlag1 = 0;
                //    return Result;

                //}
                //if (Flag == 4)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgWD =
                //              new MessageBoxControl.MessageBoxChildWindow("Palash", "Max Quantity should be greater than the min quantity for :" + objDetails.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgWD.Show();
                //    Result = false;

                //    ClickedFlag1 = 0;
                //    return Result;
                //}

            }

            return Result;
        }

        private void ClearUI()
        {
            //    ClickedFlag1 = 0;
            //    this.DataContext = new clsRateContractVO();
            //    SelectedItems = new ObservableCollection<clsRateContractDetailsVO>();
            //    dgItems.ItemsSource = SelectedItems;
            //    cmbSupplier.SelectedValue = (long)0;
            //    cmbClinicRepresentative.SelectedValue = (long)0;
            //    chkFreeze.IsEnabled = true;


            this.DataContext = new clsRateContractVO();
            SelectedItems = new ObservableCollection<clsRateContractDetailsVO>();
            dgItems.ItemsSource = SelectedItems;
            cmbSupplier.SelectedValue = (long)0;
            cmbClinicRepresentative.SelectedValue = (long)0;

            //added by Prashant channe on 24/10/2018
            this.IsEditAfterFreeze = false;
        }

        private void cmbCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null && dgItems.SelectedItem != null)//&& ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID != 0
            {
                if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID == 3)
                {
                    DataGridColumn column = dgItems.Columns[3];
                    FrameworkElement fe = column.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                    if (result != null)
                    {
                        DataGridCell cell = (DataGridCell)result;
                        //cell.Content = (object)(String.Empty);
                        ((TextBox)cell.Content).Text = string.Empty;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column1 = dgItems.Columns[4];
                    FrameworkElement fe1 = column1.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

                    if (result1 != null)
                    {
                        DataGridCell cell = (DataGridCell)result1;
                        cell.IsEnabled = true;
                    }

                    DataGridColumn column3 = dgItems.Columns[5];
                    FrameworkElement fe3 = column3.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

                    if (result3 != null)
                    {
                        DataGridCell cell = (DataGridCell)result3;
                        cell.IsEnabled = true;
                    }
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).MinQuantity = 0;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).MaxQuantity = 0;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).UnlimitedQuantity = false;

                    ((clsRateContractDetailsVO)dgItems.SelectedItem).Quantity = 0;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).UnlimitedQuantity = false;
                }
                else if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID == 5)
                {
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).Quantity = 0;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).MaxQuantity = 0;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).MinQuantity = 0;


                    DataGridColumn column = dgItems.Columns[3];
                    FrameworkElement fe = column.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                    if (result != null)
                    {
                        DataGridCell cell = (DataGridCell)result;
                        ((TextBox)cell.Content).Text = string.Empty;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column2 = dgItems.Columns[4];
                    FrameworkElement fe2 = column2.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                    if (result2 != null)
                    {
                        DataGridCell cell = (DataGridCell)result2;
                        ((TextBox)cell.Content).Text = string.Empty;
                        cell.IsEnabled = false;
                    }


                    DataGridColumn column3 = dgItems.Columns[5];
                    FrameworkElement fe3 = column3.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

                    if (result3 != null)
                    {
                        DataGridCell cell = (DataGridCell)result3;
                        ((TextBox)cell.Content).Text = string.Empty;
                        cell.IsEnabled = false;
                    }
                }
                else if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID != 3 && ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID != 0)
                {
                    DataGridColumn column4 = dgItems.Columns[3];
                    FrameworkElement fe4 = column4.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result4 = GetParent(fe4, typeof(DataGridCell));

                    if (result4 != null)
                    {
                        DataGridCell cell = (DataGridCell)result4;
                        cell.IsEnabled = true;
                    }


                    DataGridColumn column = dgItems.Columns[4];
                    FrameworkElement fe = column.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                    if (result != null)
                    {
                        DataGridCell cell = (DataGridCell)result;
                        ((TextBox)cell.Content).Text = string.Empty;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column2 = dgItems.Columns[5];
                    FrameworkElement fe1 = column2.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result2 = GetParent(fe1, typeof(DataGridCell));

                    if (result2 != null)
                    {
                        DataGridCell cell = (DataGridCell)result2;
                        ((TextBox)cell.Content).Text = string.Empty;
                        cell.IsEnabled = false;
                    }
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).MinQuantity = 0;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).MaxQuantity = 0;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).UnlimitedQuantity = false;
                }
                else
                {
                    DataGridColumn column = dgItems.Columns[3];
                    FrameworkElement fe = column.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));
                    if (result != null)
                    {
                        DataGridCell cell = (DataGridCell)result;
                        cell.IsEnabled = true;
                    }

                    DataGridColumn column2 = dgItems.Columns[4];
                    FrameworkElement fe2 = column2.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                    if (result2 != null)
                    {
                        DataGridCell cell = (DataGridCell)result2;
                        cell.IsEnabled = true;
                    }


                    DataGridColumn column3 = dgItems.Columns[5];
                    FrameworkElement fe3 = column3.GetCellContent(dgItems.SelectedItem);
                    FrameworkElement result3 = GetParent(fe3, typeof(DataGridCell));

                    if (result3 != null)
                    {
                        DataGridCell cell = (DataGridCell)result3;
                        cell.IsEnabled = true;

                    }
                }

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


        private void Description_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = txtDescription.Text.ToTitleCase();

        }

        private void Code_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCode.Text = txtCode.Text.ToTitleCase();
        }

        private void txtFDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFDescription.Text = txtFDescription.Text.ToTitleCase();

        }
        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedItems != null)
                    SelectedItems.Clear();

                DataList = new PagedSortableCollectionView<clsRateContractVO>();
                DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                DataListPageSize = 5;
                FetchData();
            }
        }

        private void txtFCode_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFCode.Text = txtFCode.Text.ToTitleCase();
        }




        private void txtMaxQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Text != null && txt.Text != "")
            {
                if (Convert.ToDecimal(txt.Text) < ((clsRateContractDetailsVO)dgItems.SelectedItem).MinQuantity)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Max Quantity must be greater than Min Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).MinQuantity = 0;

                }
            }

            if (((clsRateContractDetailsVO)dgItems.SelectedItem).MaxQuantity < 0)
            {

                ((clsRateContractDetailsVO)dgItems.SelectedItem).MaxQuantity = 0;

            }
        }

        private void txtDiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Text != null && txt.Text != "")
            {
                if (Convert.ToDecimal(txt.Text) > 100)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Discount Percentage must be less than or equal 100%.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).DiscountPercent = 0;
                    ((TextBox)sender).Text = "0";
                }
            }

        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtAutocompleteText_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteText_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsPersonNameValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
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

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgItems.SelectedItem != null)
            {
                Conversion win = new Conversion();
                win.FillUOMConversions(Convert.ToInt64(((clsRateContractDetailsVO)dgItems.SelectedItem).ItemID), ((clsRateContractDetailsVO)dgItems.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;
            ((clsRateContractDetailsVO)dgItems.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsRateContractDetailsVO)dgItems.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsRateContractDetailsVO)dgItems.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;
            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsRateContractDetailsVO)dgItems.SelectedItem).SUOMID);
            Calculatesummary();
        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();
            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgItems.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsRateContractDetailsVO)dgItems.SelectedItem).UOMConversionList;
                    objConversionVO.UOMConvertLIst = UOMConvertLIst;
                    objConversionVO.MainMRP = ((clsRateContractDetailsVO)dgItems.SelectedItem).MainMRP;
                    objConversionVO.MainRate = ((clsRateContractDetailsVO)dgItems.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsRateContractDetailsVO)dgItems.SelectedItem).Quantity);
                    long BaseUOMID = ((clsRateContractDetailsVO)dgItems.SelectedItem).BaseUOMID;
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).Rate = Convert.ToDecimal(objConversionVO.Rate);
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).MRP = Convert.ToDecimal(objConversionVO.MRP);
                    //((clsRateContractDetailsVO)dgItems.SelectedItem).RequiredQuantity = objConversionVO.Quantity;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).StockingQuantity = objConversionVO.Quantity;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).BaseMRP = objConversionVO.BaseMRP;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsRateContractDetailsVO)dgItems.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;

                }
            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void txtReasonForEdit_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtRCReasonForEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            txtRCReasonForEdit.Text = (txtRCReasonForEdit.Text.Trim()).ToTitleCase();
        }

    }
}
