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
using CIMS.Forms;
using PalashDynamics.CRM;
using CIMS;
using System.Collections.ObjectModel;

using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Administration;
using System.Windows.Data;
using PalashDynamics.Collections;
using System.Reflection;

namespace OPDModule.Forms
{
    public partial class frmPackageServices : UserControl
    {
        public frmPackageServices()
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            dgServiceList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgServiceList_CellEditEnded);
            //Paging
            DataList = new PagedSortableCollectionView<clsPackageServiceVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        #region Variable Declaration
        SwivelAnimation objAnimation;
        bool IsNew = false;
        bool IsCancel = true;
        ObservableCollection<clsPackageServiceDetailsVO> ServiceList { get; set; }
        #endregion

       

        void dgServiceList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.DisplayIndex == 3)
            {
                if (((clsPackageServiceDetailsVO)dgServiceList.SelectedItem).ConcessionPercentage > 100)
                {

                    ((clsPackageServiceDetailsVO)dgServiceList.SelectedItem).ConcessionPercentage = 100;
                    string msgTitle = "";
                    string msgText = "Percentage should not be greater than 100";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");
            this.DataContext = new clsPackageServiceVO();
            ServiceList = new ObservableCollection<clsPackageServiceDetailsVO>();
            FillService();
            FillFrontPanelService();
            FillDepartmentList(((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID);
            FetchData();

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            IsNew = true;
            SetCommandButtonState("New");
            ClearData();


            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            if (IsCancel == true)
            {

                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Billing Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
            objAnimation.Invoke(RotationType.Backward);
        }

        #region Paging

        public PagedSortableCollectionView<clsPackageServiceVO> DataList { get; private set; }


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


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }
        #endregion
     
        #region FillCombobox
        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];
                }

            };


            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillService()
        {

            clsGetPackageServiceBizActionVO BizAction = new clsGetPackageServiceBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetPackageServiceBizActionVO)e.Result).MasterList);
                    cmbService.ItemsSource = null;
                    cmbService.ItemsSource = objList;
                    cmbService.SelectedItem = objList[0];
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void FillFrontPanelService()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ServiceMaster;
            BizAction.IsActive = true;
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
                    cmbService1.ItemsSource = null;
                    cmbService1.ItemsSource = objList;
                    cmbService1.SelectedItem = objList[0];
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #endregion

        #region Save Data

        private void Save()
        {
            clsAddPackageServiceBizActionVO BizAction = new clsAddPackageServiceBizActionVO();
            try
            {
                BizAction.Details = (clsPackageServiceVO)this.DataContext;

                if (cmbService.SelectedItem != null)
                    BizAction.Details.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;

                BizAction.Details.PackageDetails = ServiceList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("Save");
                    if (arg.Error == null)
                    {
                        if (((clsAddPackageServiceBizActionVO)arg.Result).Details != null)
                        {
                            ClearData();
                            FetchData();
                            objAnimation.Invoke(RotationType.Backward);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Package Services added successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }


        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Package Services ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                if (CheckDuplicasy())
                {
                    Save();
                }
        }

        #endregion

        #region View Data
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            SetCommandButtonState("View");
            if (dgPackage.SelectedItem != null)
            {
                IsNew = false;
                this.DataContext = ((clsPackageServiceVO)dgPackage.SelectedItem);
                cmbService.SelectedValue = ((clsPackageServiceVO)this.DataContext).ServiceID;
                GetPackageDetails(((clsPackageServiceVO)dgPackage.SelectedItem).ID);
                objAnimation.Invoke(RotationType.Forward);   
            }
        }

        private void GetPackageDetails(long packageid)
        {
            clsGetPackageServiceDetailsListBizActionVO BizAction = new clsGetPackageServiceDetailsListBizActionVO();
            try
            {
                BizAction.PackageDetailList = new List<clsPackageServiceDetailsVO>();
                BizAction.PackageID = packageid;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPackageServiceDetailsListBizActionVO)arg.Result).PackageDetailList != null)
                        {
                            clsGetPackageServiceDetailsListBizActionVO DetailsVO = new clsGetPackageServiceDetailsListBizActionVO();
                            DetailsVO = (clsGetPackageServiceDetailsListBizActionVO)arg.Result;
                           
                            List<clsPackageServiceDetailsVO> ObjItem;
                            ObjItem = DetailsVO.PackageDetailList;
                            foreach (var item4 in ObjItem)
                            {
                                ServiceList.Add(item4);
                            }
                            PagedCollectionView collection = new PagedCollectionView(ServiceList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("Department"));
                            dgServiceList.ItemsSource = collection;
                            dgServiceList.UpdateLayout();
                            dgServiceList.Focus();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw; 
            }

        }

#endregion

        #region Service Details
        private void btnService_Click(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem != null && ((MasterListItem)cmbDepartment.SelectedItem).ID> 0)
            {
                ServiceDetails objservice = new ServiceDetails();
                objservice.CmdAddFreeService.Visibility = System.Windows.Visibility.Collapsed;
                objservice.CmdconcessionService.Visibility = System.Windows.Visibility.Collapsed;
                objservice.TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;
                objservice.OnAddButton_Click += new RoutedEventHandler(objservice_OnAddButton_Click);
                objservice.Show();
            }
        }

        void objservice_OnAddButton_Click(object sender, RoutedEventArgs e)
        {

            if (((ServiceDetails)sender).DialogResult == true)
            {
                for (int i = 0; i < ((ServiceDetails)sender).check.Count; i++)
                {
                    if (((ServiceDetails)sender).check[i] == true)
                    {
                        var item1 = from r in ServiceList
                                    where (r.ServiceID == (((ServiceDetails)sender).ServiceItemSource[i]).ID
                                    && r.DepartmentID == ((MasterListItem)cmbDepartment.SelectedItem).ID)
                                    select new clsPackageServiceDetailsVO
                                    {
                                        ServiceID = r.ServiceID,
                                        ServiceCode = r.ServiceCode,
                                        Status = r.Status

                                    };
                        if (item1.ToList().Count == 0)
                        {

                            clsPackageServiceDetailsVO ObjTemp = new clsPackageServiceDetailsVO();

                            ObjTemp.ServiceID = (((ServiceDetails)sender).ServiceItemSource[i]).ID;
                            ObjTemp.ServiceName = (((ServiceDetails)sender).ServiceItemSource[i]).ServiceName;
                            ObjTemp.ServiceCode = (((ServiceDetails)sender).ServiceItemSource[i]).ServiceCode;
                            ObjTemp.Rate = (double)((ServiceDetails)sender).ServiceItemSource[i].Rate;
                            ObjTemp.Quantity = 1;
                            ObjTemp.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
                            ObjTemp.Department = ((MasterListItem)cmbDepartment.SelectedItem).Description;
                            ObjTemp.IsActive = true;
                            ServiceList.Add(ObjTemp);
                        }
                    }
                }
                PagedCollectionView collection = new PagedCollectionView(ServiceList);
                collection.GroupDescriptions.Add(new PropertyGroupDescription("Department"));
                dgServiceList.ItemsSource = collection;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();
            }




        }

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceList.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ServiceList.RemoveAt(dgServiceList.SelectedIndex);
                        dgServiceList.Focus();
                        dgServiceList.UpdateLayout();
                        dgServiceList.SelectedIndex = ServiceList.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }

        private double CalculateAmount()
        {
            double Total = 0;
            for (int i = 0; i < ServiceList.Count; i++)
            {
                Total += (ServiceList[i].NetAmount);
            }
            return Total;   
        }
        #endregion

        #region Get Package Amount
        private void GetAmount(long serviceID)
        {
            try
            {
                clsGetTariffServiceListBizActionVO BizAction = new clsGetTariffServiceListBizActionVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();
                BizAction.TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                        {
                            List<clsServiceMasterVO> ObjList = new List<clsServiceMasterVO>();
                            ObjList = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;

                            var item1 = from r in ObjList
                                        where (r.ID == serviceID)
                                        select new clsServiceMasterVO
                                        {
                                            Rate = r.Rate
                                        };
                            if (item1.ToList().Count > 0)
                            {
                                ObjList = item1.ToList();
                                foreach (var item in ObjList)
                                {
                                    txtAmount.Text = item.Rate.ToString();
                                }

                            }
                        }
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void cmbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbService.SelectedItem != null)
            {
                if (((MasterListItem)cmbService.SelectedItem).ID > 0)
                {
                    GetAmount(((MasterListItem)cmbService.SelectedItem).ID);
                }
            }
        }
        #endregion

        #region FetchData
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void FetchData()
        {
            clsGetPackageServiceListBizActionVO BizAction = new clsGetPackageServiceListBizActionVO();
            try
            {
                BizAction.PackageList = new List<clsPackageServiceVO>();
                BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                if(cmbService1.SelectedItem!=null)
                    BizAction.ServiceID = ((MasterListItem)cmbService1.SelectedItem).ID;

                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPackageServiceListBizActionVO)arg.Result).PackageList != null)
                        {
                            clsGetPackageServiceListBizActionVO result = arg.Result as clsGetPackageServiceListBizActionVO;

                            DataList.TotalItemCount = result.TotalRows;

                            if (result.PackageList != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.PackageList)
                                {
                                    DataList.Add(item);
                                }

                                dgPackage.ItemsSource = null;
                                dgPackage.ItemsSource = DataList;

                                dataGrid2Pager.Source = null;
                                dataGrid2Pager.PageSize = BizAction.MaximumRows;
                                dataGrid2Pager.Source = DataList;

                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        #endregion

        #region Update Data
        private void Update()
        {
            clsAddPackageServiceBizActionVO BizAction = new clsAddPackageServiceBizActionVO();
            try
            {
                BizAction.Details = (clsPackageServiceVO)this.DataContext;

                if (cmbService.SelectedItem != null)
                    BizAction.Details.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;

                BizAction.Details.PackageDetails = ServiceList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("Modify");
                    if (arg.Error == null)
                    {
                        if (((clsAddPackageServiceBizActionVO)arg.Result).Details != null)
                        {
                            ClearData();
                            FetchData();
                            objAnimation.Invoke(RotationType.Backward);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Package Services updated successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidation())
            {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to update the Package Services ?";

            MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

            msgW1.Show();
            }
        }
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    Update();
                }
            }
               
        }

        #endregion

        #region Clear UI
        private void ClearData()
        {
            this.DataContext = new clsPackageServiceVO();
            ServiceList=new ObservableCollection<clsPackageServiceDetailsVO>();
            dgServiceList.ItemsSource = null;
            cmbDepartment.SelectedValue=(long)0;
            cmbService.SelectedValue = (long)0;

        }

        #endregion

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

        #region Validation

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItNumber())
            {
                if (textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        private bool ChkValidation()
        {
            bool result = true;

            if ((MasterListItem)cmbService.SelectedItem == null)
            {

                cmbService.TextBox.SetValidation("Please Select Service");
                cmbService.TextBox.RaiseValidationError();
                cmbService.Focus();
                result = false;


            }
            else if (((MasterListItem)cmbService.SelectedItem).ID == 0)
            {
                cmbService.TextBox.SetValidation("Please Select Service");
                cmbService.TextBox.RaiseValidationError();
                cmbService.Focus();
                result = false;

            }
            else
                cmbService.TextBox.ClearValidationError();

            if (txtValidity.Text == "")
            {
                txtValidity.SetValidation("Please enter Validity");
                txtValidity.RaiseValidationError();
                txtValidity.Focus();
                result = false;

            }
            else
                txtValidity.ClearValidationError();
            

            

            if (dgServiceList.ItemsSource == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "You Can not save without service list.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                dgServiceList.Focus();
                result = false;
                return result;

            }

           

            return result;
        }

        private bool CheckDuplicasy()
        {
            clsPackageServiceVO Item;
            if (IsNew)
            {
                Item = ((PagedSortableCollectionView<clsPackageServiceVO>)dgPackage.ItemsSource).FirstOrDefault(p => p.ServiceID.Equals(((MasterListItem)cmbService.SelectedItem).ID));
               
            }
            else
            {
                Item = ((PagedSortableCollectionView<clsPackageServiceVO>)dgPackage.ItemsSource).FirstOrDefault(p => p.ServiceID.Equals(((MasterListItem)cmbService.SelectedItem).ID) && p.ID != ((clsPackageServiceVO)dgPackage.SelectedItem).ID);
            }

            if (Item != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Service already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }          
            else
            {
                return true;
            }
        }
        #endregion

       

        
    }
    
}
