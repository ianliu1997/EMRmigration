using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using System.Windows.Controls.Data;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using System.Collections.Generic;
using PalashDynamics.Service.PalashTestServiceReference;
using System;
using CIMS;
using PalashDynamics.Collections;
using System.Linq;

namespace PalashDynamics.Administration
{
    public partial class PackageServices : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();
        public clsPackageServiceVO SelectedServicesVO = null;  // PackageDetails List
        public List<clsServiceMasterVO> ServiceItemSource { get; set; }
        public List<MasterListItem> SpecilizationItemSource { get; set; }

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public long TariffID { get; set; }

        public long ValidityDuration = 0;

        public long PackageServiceID = 0;

        #endregion
        public PackageServices()
        {
            InitializeComponent();

            //Paging
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            ServiceItemSource = new List<clsServiceMasterVO>();
            SpecilizationItemSource = new List<MasterListItem>();

        }

        #region Paging

        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }


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
            BindServiceListGrid();  // FillDataGrid();

        }
        #endregion

        #region FillCombobox
        private void FillSpecialization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
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

                    cmbSpecialization.ItemsSource = null;
                    //cmbUnitAppointmentSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                    cmbSpecialization.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).ID;

                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillSubSpecialization(long iSupId)
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

                    cmbSubSpecialization.ItemsSource = null;
                    cmbSubSpecialization.ItemsSource = objList;

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbSubSpecialization.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        #endregion

        private void SetComboboxValue()
        {
            cmbSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).Specialization;
            cmbSubSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ServiceDetails_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new clsServiceMasterVO();
            //FetchData();

            FillSpecialization();
            SetComboboxValue();
            txtServiceName.Focus();

            //rdbService.IsChecked = true;    //Package New Changes Added on 03052018

            if (rdbSpecilization.IsChecked == true)
            {
                DocSpecilization.Visibility = System.Windows.Visibility.Visible;
                DocService.Visibility = System.Windows.Visibility.Collapsed;
                FillSpecilizationGrid();
                cmbSpecialization.IsEnabled = false;
                cmbSubSpecialization.IsEnabled = false;
                chkAll.Visibility = Visibility.Visible;
            }
            else if (rdbService.IsChecked == true)
            {
                DocService.Visibility = System.Windows.Visibility.Visible;
                DocSpecilization.Visibility = System.Windows.Visibility.Collapsed;
                txtServiceName.IsReadOnly = false;
                cmbSpecialization.IsEnabled = true;
                cmbSubSpecialization.IsEnabled = true;
                BindServiceListGrid();  // FillDataGrid();
                chkAll.Visibility = Visibility.Collapsed;
            }

            

        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (OnAddButton_Click != null)
            {
                if (ServiceItemSource.Count == 0 && SpecilizationItemSource.Count == 0)  //chkAll.IsChecked == false &&
                {
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Select atleast one Specialization or Service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW2.Show();
                }
                else
                {
                    this.DialogResult = true;
                    OnAddButton_Click(this, new RoutedEventArgs());

                    this.Close();
                }
            }
        }

        /// <summary>
        /// Purpose:Getting list of Service.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (rdbService.IsChecked == true)
            {
                BindServiceListGrid();  // FillDataGrid();
            }
            else
            {
                FillSpecilizationGrid();
            }
        }

        private void FetchData()
        {
            clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
            BizAction.ServiceList = new List<clsServiceMasterVO>();

            if (txtServiceName.Text != null)
                BizAction.ServiceName = txtServiceName.Text;
            if (cmbSpecialization.SelectedItem != null)
                BizAction.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
            if (cmbSubSpecialization.SelectedItem != null)
                BizAction.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;



            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            dgServiceList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList != null)
                    {
                        dgServiceList.ItemsSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
                        //ServiceItemSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;

                        //for (int i = 0; i < ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList.Count; i++)
                        //{
                        //    bool b = false;
                        //    check.Add(b);
                        //}

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


        private void BindServiceListGrid()
        {
            try
            {
                WaitIndicator obj = new WaitIndicator();
                obj.Show();

                clsGetServiceMasterListBizActionVO BizActionObj = new clsGetServiceMasterListBizActionVO();

                BizActionObj.GetAllServiceListDetails = true;
                BizActionObj.ServiceList = new List<clsServiceMasterVO>();

                BizActionObj.IsPagingEnabled = true;
                BizActionObj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizActionObj.MaximumRows = DataList.PageSize;

                BizActionObj.IsStatus = true;
                BizActionObj.IsFromPackage = true;  // Only In Case From Package Master From Admin then Set to True 

                //BizActionObj.ServiceName = txtSearchCriteria.Text;

                if (txtServiceName.Text != null)
                    BizActionObj.ServiceName = txtServiceName.Text;

                if (cmbSpecialization.SelectedItem == null)
                {
                    BizActionObj.Specialization = 0;
                }
                else
                {
                    BizActionObj.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID == null ? 0 : ((MasterListItem)cmbSpecialization.SelectedItem).ID;     //(long) cboSpecialization.SelectedItem; //((clsServiceMasterVO)this.DataContext).Specialization;
                }

                if (cmbSubSpecialization.SelectedItem == null)
                {
                    BizActionObj.SubSpecialization = 0;
                }
                else
                {
                    BizActionObj.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID == null ? 0 : ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;     //(long) cboSpecialization.SelectedItem; //((clsServiceMasterVO)this.DataContext).Specialization;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetServiceMasterListBizActionVO result = args.Result as clsGetServiceMasterListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.ServiceList != null)
                        {
                            DataList.Clear();
                            //foreach (var item in result.ServiceList)
                            //{
                            //    DataList.Add(item);
                            //}
                            foreach (var item in result.ServiceList)
                            {
                                DataList.Add(item);
                            }

                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = DataList;

                            dataGrid2Pager.Source = null;
                            dataGrid2Pager.PageSize = BizActionObj.MaximumRows;
                            dataGrid2Pager.Source = DataList;
                        }

                        ////grdServices.ItemsSource = null;
                        ////grdServices.ItemsSource = ((clsGetServiceMasterListBizActionVO)args.Result).ServiceList;

                        //ServiceItemSource = result.ServiceList;

                        //for (int i = 0; i < result.ServiceList.Count; i++)
                        //{
                        //    bool b = false;
                        //    check.Add(b);
                        //}

                        obj.Close();
                    }
                };
                client.ProcessAsync(BizActionObj, new clsUserVO()); //User
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillDataGrid()
        {
            try
            {
                clsGetTariffServiceListBizActionVO BizAction = new clsGetTariffServiceListBizActionVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();
                if (txtServiceName.Text != null)
                    BizAction.ServiceName = txtServiceName.Text;
                if (cmbSpecialization.SelectedItem != null)
                    BizAction.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                if (cmbSubSpecialization.SelectedItem != null)
                    BizAction.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;
                BizAction.TariffID = TariffID;

                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                        {

                            clsGetTariffServiceListBizActionVO result = arg.Result as clsGetTariffServiceListBizActionVO;

                            DataList.TotalItemCount = result.TotalRows;

                            if (result.ServiceList != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.ServiceList)
                                {
                                    DataList.Add(item);
                                }

                                dgServiceList.ItemsSource = null;
                                dgServiceList.ItemsSource = DataList;

                                dataGrid2Pager.Source = null;
                                dataGrid2Pager.PageSize = BizAction.MaximumRows;
                                dataGrid2Pager.Source = DataList;

                            }

                            ////dgServiceList.ItemsSource = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;
                            //ServiceItemSource = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;

                            //for (int i = 0; i < ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList.Count; i++)
                            //{
                            //    bool b = false;
                            //    check.Add(b);
                            //}

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

        private void FillSpecilizationGrid()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    dgSpecilization.ItemsSource = null;
                    dgSpecilization.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;


                }

            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialization.SelectedItem != null) ;
            FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID);
        }

        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceList.SelectedItem != null)
            {
                if (ServiceItemSource == null)
                    ServiceItemSource = new List<clsServiceMasterVO>();

                if (((CheckBox)sender).IsChecked == true)
                {
                    var item1 = from r in SelectedServicesVO.PackageDetails.ToList()
                                where (r.ServiceID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID && r.DepartmentID == ((clsServiceMasterVO)dgServiceList.SelectedItem).Specialization
                                && r.IsSpecilizationGroup == false
                                && r.ProcessID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ProcessID
                                )
                                select new clsPackageServiceDetailsVO
                                {
                                    GroupID = r.GroupID,
                                    Status = r.Status

                                };

                    if (item1.ToList().Count == 0)
                    {
                        if (((clsServiceMasterVO)dgServiceList.SelectedItem).IsPackage == false)
                        {
                            ServiceItemSource.Add((clsServiceMasterVO)dgServiceList.SelectedItem); //check[dgServiceList.SelectedIndex] = true;
                        }
                        else if (((clsServiceMasterVO)dgServiceList.SelectedItem).IsPackage == true)
                        {
                            if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageID > 0 && PackageServiceID > 0 && ((clsServiceMasterVO)dgServiceList.SelectedItem).ID != PackageServiceID)
                            {
                                GetPackageServiceDetails(((clsServiceMasterVO)dgServiceList.SelectedItem).PackageID, ((CheckBox)sender));
                            }
                            else
                            {
                                if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageID == 0)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Service cannot be add, as its a Package Service with no Package Details!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                    msgW1.Show();
                                }
                                else if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageID > 0 && PackageServiceID > 0 && ((clsServiceMasterVO)dgServiceList.SelectedItem).ID == PackageServiceID)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Service cannot be add, as its a Package Service of same package!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                    msgW1.Show();
                                }

                                ((CheckBox)sender).IsChecked = false;
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "'" + ((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceName + "'" + " service is already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                }
                else
                {
                    if (((clsServiceMasterVO)dgServiceList.SelectedItem).IsPackage == false)
                    {
                        ServiceItemSource.Remove((clsServiceMasterVO)dgServiceList.SelectedItem); //check[dgServiceList.SelectedIndex] = false;
                    }
                    else if (((clsServiceMasterVO)dgServiceList.SelectedItem).IsPackage == true)
                    {
                        for (int i = 0; i < ServiceItemSource.Count; i++)
                        {
                            if (ServiceItemSource[i].PackageID == ((clsServiceMasterVO)dgServiceList.SelectedItem).PackageID)
                            {
                                //check[i] = false;

                                var listItems = from x in ServiceItemSource
                                                where x.PackageID != ((clsServiceMasterVO)dgServiceList.SelectedItem).PackageID
                                                select x;

                                foreach (clsServiceMasterVO item in listItems)
                                {
                                    ServiceItemSource.Remove(item);
                                }

                            }
                        }

                    }
                }

            }
        }

        private void GetPackageServiceDetails(long packageid, CheckBox chk)
        {
            clsGetPackageServiceDetailsListBizActionVO BizAction = new clsGetPackageServiceDetailsListBizActionVO();
            try
            {
                BizAction.PackageDetailList = new List<clsPackageServiceDetailsVO>();
                BizAction.PackageID = packageid;
                BizAction.UnitId = ((clsServiceMasterVO)dgServiceList.SelectedItem).UnitID;
                // ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId

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

                            if (Convert.ToInt64(DetailsVO.PackageDetailList[0].Validity) > ValidityDuration)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Package can not be added, its existing duration should be less than new one", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();

                                chk.IsChecked = false;
                                dgServiceList.UpdateLayout();

                            }
                            else
                            {

                                List<clsPackageServiceDetailsVO> ObjItem;
                                ObjItem = DetailsVO.PackageDetailList;

                                int Cnt = 0;
                                int Cnt1 = 0;
                                long SerId = 0;

                                clsServiceMasterVO ObjTemp = new clsServiceMasterVO();
                                MasterListItem ObjTemp1 = new MasterListItem();

                                foreach (var item4 in ObjItem)
                                {
                                    //ServiceList.Add(item4);


                                    if (item4.IsSpecilizationGroup == false)
                                    {
                                        var item1 = from r in ServiceItemSource
                                                    where (r.ServiceID == item4.ServiceID
                                                    )
                                                    select new clsServiceMasterVO
                                                    {
                                                        ServiceID = r.ServiceID,
                                                        ServiceCode = r.ServiceCode,
                                                        Status = r.Status

                                                    };

                                        if (item1.ToList().Count == 0)
                                        {
                                            if (SerId == 0 || SerId != item4.ServiceID)
                                                SerId = item4.ServiceID;

                                            //clsServiceMasterVO ObjTemp = new clsServiceMasterVO();

                                            if (SerId == item4.ServiceID)
                                            {
                                                if (Cnt1 == 0)
                                                    ObjTemp = new clsServiceMasterVO();

                                                ObjTemp.ID = item4.ServiceID;   //ObjTemp.ServiceID = item4.ID;
                                                ObjTemp.ServiceName = item4.ServiceName;
                                                ObjTemp.ServiceCode = item4.ServiceCode;
                                                ObjTemp.Rate = Convert.ToDecimal(item4.Rate);
                                                ObjTemp.Specialization = item4.DepartmentID;
                                                ObjTemp.SpecializationString = item4.Department;
                                                //ObjTemp.Quantity = 0;
                                                //ObjTemp.IsPackage = true;
                                                ObjTemp.PackageID = packageid;

                                                ObjTemp.FromPackage = true;

                                                ObjTemp.PackageInPackageItemList.Add(item4);
                                                Cnt1++;
                                            }
                                            else
                                            {
                                                SerId = item4.ServiceID;
                                                Cnt1 = 0;
                                            }

                                            Cnt = ObjItem.Count(c => c.ServiceID == item4.ServiceID && c.DepartmentID == item4.DepartmentID && c.IsSpecilizationGroup == false);

                                            if (Cnt1 == Convert.ToInt32(DetailsVO.PackageDetailList[0].Validity)) //if (Cnt1 == Cnt)
                                            {
                                                ServiceItemSource.Add(ObjTemp);
                                                Cnt1 = 0;
                                            }

                                            //if (ServiceItemSource.Count > 0)
                                            //{
                                            //    check[ServiceItemSource.Count - 1] = true;
                                            //}

                                        }

                                    }
                                    else if (item4.IsSpecilizationGroup == true)
                                    {
                                        var item1 = from r in SpecilizationItemSource
                                                    where (r.ID == item4.GroupID
                                                    )
                                                    select new MasterListItem
                                                 {
                                                     ID = r.ID,   //ID as GroupId
                                                     Description = r.Description

                                                 };

                                        if (item1.ToList().Count == 0)
                                        {
                                            if (SerId == 0 || SerId != item4.ServiceID)
                                                SerId = item4.ServiceID;

                                            //MasterListItem ObjTemp1 = new MasterListItem();

                                            if (SerId == item4.ServiceID)
                                            {
                                                if (Cnt1 == 0)
                                                    ObjTemp1 = new MasterListItem();

                                                ObjTemp1.ID = item4.ServiceID;   //item4.ID;
                                                ObjTemp1.Description = item4.Department;
                                                ObjTemp1.FromPackage = true;

                                                ObjTemp1.PackageInPackageItemList.Add(item4);
                                                Cnt1++;
                                            }
                                            else
                                            {
                                                SerId = item4.ServiceID;
                                                Cnt1 = 0;
                                            }

                                            Cnt = ObjItem.Count(c => c.ServiceID == item4.ServiceID && c.DepartmentID == item4.DepartmentID && c.IsSpecilizationGroup == true);


                                            if (Cnt1 == Convert.ToInt32(DetailsVO.PackageDetailList[0].Validity)) //if (Cnt1 == Cnt)
                                            {
                                                SpecilizationItemSource.Add(ObjTemp1);
                                                Cnt1 = 0;
                                            }

                                            //if (SpecilizationItemSource.Count > 0)
                                            //{
                                            //    check[SpecilizationItemSource.Count - 1] = true;
                                            //}

                                        }

                                    }


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
            catch (Exception)
            {
                throw;
            }

        }


        #region Reset All controls
        private void ClearControl()
        {
            this.DataContext = new clsServiceMasterVO();
            txtServiceName.Text = string.Empty;
            dgServiceList.SelectedItem = null;
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void chkSpecialization_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                var item1 = from r in SelectedServicesVO.PackageDetails.ToList()
                            where (r.ServiceID == ((MasterListItem)dgSpecilization.SelectedItem).ID && r.DepartmentID == (((MasterListItem)dgSpecilization.SelectedItem).ID)
                                && r.IsSpecilizationGroup == true
                                )
                                select new clsPackageServiceDetailsVO
                                {
                                    GroupID = r.GroupID,
                                    Status = r.Status

                                };

                if (item1.ToList().Count == 0)
                {
                    SpecilizationItemSource.Add((MasterListItem)dgSpecilization.SelectedItem); //check[dgSpecilization.SelectedIndex] = true;
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "'" + ((MasterListItem)dgSpecilization.SelectedItem).Description + "'" + " specialization is already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
            }
            else
            {
                SpecilizationItemSource.Remove((MasterListItem)dgSpecilization.SelectedItem); //check[dgSpecilization.SelectedIndex] = false;
                chkAll.IsChecked = false;
            }
        }

        private void rdbSpecilization_Click(object sender, RoutedEventArgs e)
        {
            if (rdbSpecilization.IsChecked == true)
            {
                chkAll.Visibility = Visibility.Visible;
                chkAll.IsChecked = false;
                FillSpecilizationGrid();
                dgServiceList.ItemsSource = null;
                DocSpecilization.Visibility = System.Windows.Visibility.Visible;
                DocService.Visibility = System.Windows.Visibility.Collapsed;
                cmbSpecialization.IsEnabled = false;
                cmbSubSpecialization.IsEnabled = false;
                txtServiceName.IsReadOnly = true;
            }
            else
            {
                dgSpecilization.ItemsSource = null;

            }
        }

        private void rdbService_Click(object sender, RoutedEventArgs e)
        {
            if (rdbService.IsChecked == true)
            {
                chkAll.Visibility = Visibility.Collapsed;
                chkAll.IsChecked = false;
                dgSpecilization.ItemsSource = null;
                BindServiceListGrid();  // FillDataGrid();
                DocService.Visibility = System.Windows.Visibility.Visible;
                DocSpecilization.Visibility = System.Windows.Visibility.Collapsed;
                txtServiceName.IsReadOnly = false;
                cmbSpecialization.IsEnabled = true;
                cmbSubSpecialization.IsEnabled = true;

            }
            else
            {
                dgServiceList.ItemsSource = null;

            }
        }

        private void chkSelectAllSpecialization_Click(object sender, RoutedEventArgs e)
        {
            //List<MasterListItem> ObjList = new List<MasterListItem>();

            //if (dgSpecilization.ItemsSource != null)
            //{
            //    ObjList = (List<MasterListItem>)dgSpecilization.ItemsSource;

            //    if (ObjList != null && ObjList.Count > 0)
            //    {
            //        if (chkSelectAllSpecialization.IsChecked == true)
            //        {
            //            foreach (var item in ObjList)
            //            {
            //                item.Status = true;

            //                //if (_ItemSelected == null)
            //                //    _ItemSelected = new ObservableCollection<MasterListItem>();

            //                SpecilizationItemSource.Add(item);

            //            }
            //        }
            //        else
            //        {
            //            foreach (var item in ObjList)
            //            {
            //                item.Status = false;

            //                //if (_ItemSelected != null)
            //                //    _ItemSelected.Remove(item);

            //                SpecilizationItemSource.Remove(item);
            //            }
            //        }

            //        dgSpecilization.ItemsSource = null;
            //        dgSpecilization.ItemsSource = SpecilizationItemSource;

            //    }
            //    else
            //        chkSelectAllSpecialization.IsChecked = false;
            //}
        }

        private void rdbSpecilization_Checked(object sender, RoutedEventArgs e)
        {
            //if (rdbSpecilization.IsChecked == true)
            //{
            //    chkAll.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    chkAll.Visibility = Visibility.Collapsed;
            //}
        }

        private void rdbService_Checked(object sender, RoutedEventArgs e)
        {
            //if (rdbSpecilization.IsChecked == true)
            //{
            //    chkAll.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    chkAll.Visibility = Visibility.Collapsed;
            //}
        }

        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAll.IsChecked == true)
            {
                if (((CheckBox)sender).IsChecked == true)
                {
                    List<MasterListItem> objSpecializationList = ((List<MasterListItem>)(dgSpecilization.ItemsSource)).ToList();
                    foreach (MasterListItem item in objSpecializationList)
                    {
                        item.isChecked = true;
                        SpecilizationItemSource.Add(item);
                    }

                    dgSpecilization.ItemsSource = null;
                    dgSpecilization.ItemsSource = objSpecializationList;
                }
            }
            else
            {
                if (((CheckBox)sender).IsChecked == false)
                {
                    List<MasterListItem> objSpecializationList = ((List<MasterListItem>)(dgSpecilization.ItemsSource)).ToList();
                    foreach (MasterListItem item in objSpecializationList)
                    {
                        item.isChecked = false;
                    }

                    SpecilizationItemSource.Clear();
                    dgSpecilization.ItemsSource = null;
                    dgSpecilization.ItemsSource = objSpecializationList;
                }
            }
        }




    }
}

