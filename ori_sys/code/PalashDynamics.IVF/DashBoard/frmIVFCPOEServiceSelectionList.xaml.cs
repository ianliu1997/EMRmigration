using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration;
using System.Text;
using System.Windows.Input;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects.RSIJ;
using System.Windows.Data;
using PalashDynamics.Administration;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmIVFCPOEServiceSelectionList : ChildWindow
    {
        public bool Refferal = false;
        #region Public Variable
        public event RoutedEventHandler OnCancelButtonClick;
        public event RoutedEventHandler OnAddButton_Click;
        List<MasterListItem> objClinicList = new List<MasterListItem>();
        List<MasterListItem> objSpecializationList = new List<MasterListItem>();
        public clsVisitVO CurrentVisit { get; set; }
        public PagedSortableCollectionView<clsServiceMasterVO> DataListService { get; private set; }

        //change by sagar for separate service
        public bool Pathology = false;
        public bool Radiology = false;
        public bool Procedure = false;
        public bool Admission = false;
        public bool IsOther = false;
        #endregion

        private void FillService()
        {
            try
            {
                clsGetMasterForServiceBizActionVO BizAction = new clsGetMasterForServiceBizActionVO();
                BizAction.ServiceDetails = new List<clsServiceMasterVO>();
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                {
                    BizAction.UnitID = 0;
                }
                if (txtServiceCode.Text != null)
                {
                    BizAction.ServiceCode = txtServiceCode.Text;
                }
                if (txtServiceName.Text != null)
                {
                    BizAction.Description = txtServiceName.Text;
                }
                if (cmbSpecilization.SelectedItem != null)
                    BizAction.SpecializationID = ((MasterListItem)cmbSpecilization.SelectedItem).ID;
                if (cmbSubSpecilization.SelectedItem != null)
                    BizAction.SubSpecializationID = ((MasterListItem)cmbSubSpecilization.SelectedItem).ID;
                BizAction.IsPagingEnabled = true;
                BizAction.StartRowIndex = DataListService.PageIndex * DataListService.PageSize;
                BizAction.MaximumRows = DataListService.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetMasterForServiceBizActionVO)arg.Result).ServiceDetails != null)
                        {
                            clsGetMasterForServiceBizActionVO result = arg.Result as clsGetMasterForServiceBizActionVO;
                            if (result.ServiceDetails != null)
                            {
                                DataListService.Clear();
                                DataListService.TotalItemCount = ((clsGetMasterForServiceBizActionVO)arg.Result).TotalRows;

                                foreach (clsServiceMasterVO item in result.ServiceDetails)
                                {
                                    item.SelectService = false;
                                    DataListService.Add(item);
                                }
                            }
                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = DataListService;

                            DataPagerDoc.Source = null;
                            DataPagerDoc.PageSize = BizAction.MaximumRows;
                            DataPagerDoc.Source = DataListService;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            { }
        }

        #region Paging
        public int DataListPageSizeSer
        {
            get
            {
                return DataListService.PageSize;
            }
            set
            {
                if (value == DataListService.PageSize) return;
                DataListService.PageSize = value;
            }
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillService();
        }
        void DataListService_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillService();
        }
        #endregion


        public frmIVFCPOEServiceSelectionList()
        {
            InitializeComponent();
            DataListService = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataListService.OnRefresh += new EventHandler<RefreshEventArgs>(DataListService_OnRefresh);
            DataListPageSizeSer = 15;
            this.Loaded += new RoutedEventHandler(frmServiceShareSelectionList_Loaded);
            this.Title = "Service List";// LocalizationManager.resourceManager.GetString("ttlServiceList");
        }

        void frmServiceShareSelectionList_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Service List";
            FillSpecialization();

        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            bool isValid = true;
            if (DataListService == null)
            {
                isValid = false;
            }
            else if (DataListService.Count <= 0)
            {
                isValid = false;
            }

            if (isValid)
            {
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                string strMsg = "No Service/s Selected for Adding";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick((this.DataContext), e);
            }
            this.DialogResult = false;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            DataPagerDoc.PageIndex = 0;
            FillService();
        }

        public ObservableCollection<clsServiceMasterVO> ServiceList { get; set; }

        private void chkMultipleServices_Click(object sender, RoutedEventArgs e)
        {
            bool IsValid = true;
            if (dgServiceList.SelectedItem != null)
            {
                try
                {
                    if (ServiceList == null)
                        ServiceList = new ObservableCollection<clsServiceMasterVO>();

                    CheckBox chk = (CheckBox)sender;
                    StringBuilder strError = new StringBuilder();

                    if (chk.IsChecked == true)
                    {
                        if (ServiceList.Count > 0)
                        {
                            var item = from r in ServiceList
                                       where r.ServiceCode == ((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceCode && r.ServiceName == ((clsServiceMasterVO)dgServiceList.SelectedItem).Description
                                       select new clsServiceMasterVO
                                       {
                                           ServiceCode = r.ServiceCode,
                                           ServiceName = r.ServiceName
                                       };
                            if (item.ToList().Count > 0)
                            {
                                if (strError.ToString().Length > 0)
                                    strError.Append(",");
                                strError.Append(((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceName);

                                if (!string.IsNullOrEmpty(strError.ToString()))
                                {
                                    string strMsg = "Services already Selected : " + strError.ToString();

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    ((clsServiceMasterVO)dgServiceList.SelectedItem).SelectService = false;
                                    IsValid = false;
                                }
                            }
                            else
                            {
                                ServiceList.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                            }
                        }
                        else
                        {
                            ServiceList.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                        }
                    }
                    else
                        ServiceList.Remove((clsServiceMasterVO)dgServiceList.SelectedItem);
                }
                catch (Exception)
                { }

            }
        }

        private void txtServiceName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataPagerDoc.PageIndex = 0;
                FillService();
            }
        }

        private void cmbSpecilization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecilization.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecilization.SelectedItem).ID);
            }
        }

        private void FillSpecialization()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Specialization;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                        cmbSpecilization.ItemsSource = null;
                        cmbSpecilization.ItemsSource = objList;
                        try
                        {
                            if (Pathology)
                                cmbSpecilization.SelectedItem = ((List<MasterListItem>)cmbSpecilization.ItemsSource).First(dd => dd.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID);
                            else if (Radiology)
                                cmbSpecilization.SelectedItem = ((List<MasterListItem>)cmbSpecilization.ItemsSource).First(dd => dd.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID);
                            else
                                cmbSpecilization.IsEnabled = true;
                            cmbSpecilization.IsEnabled = false;
                        }
                        catch (Exception)
                        {
                            cmbSpecilization.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
                        }
                    }

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception) { FillService(); }

        }

        private void FillSubSpecialization(long iSupId)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
                if (iSupId > 0)
                    BizAction.Parent = new KeyValue { Key = iSupId, Value = "fkSpecializationID" };
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

                        cmbSubSpecilization.ItemsSource = null;
                        cmbSubSpecilization.ItemsSource = objList;

                        if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                            cmbSubSpecilization.SelectedItem = objList[0];
                        else
                            FillService();

                        if (cmbSpecilization.SelectedItem != null)
                        {
                            FillService();
                        }
                    }
                    else
                    {
                        FillService();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                FillService();
            }
        }
    }
}


