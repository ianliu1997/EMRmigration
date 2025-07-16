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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.ComponentModel;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.Agency;
using CIMS;
using System.Reflection;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Administration;
using System.Collections.ObjectModel;
using System.Text;
using PalashDynamics.UserControls;


namespace PalashDynamics.Administration.Agency
{
    public partial class frmAgencyServiceLinking : UserControl
    {
        #region Variable And List Declaration
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public PagedSortableCollectionView<clsAgencyClinicLinkVO> MasterList { get; private set; }
        private List<clsAgencyMasterVO> AgencyList;
        private List<clsAgencyMasterVO> SelectedAgencyList;
        private List<clsAgencyMasterVO> ModifyAgencyList;
        private List<clsSpecializationVO> SelectedSpecializationList;
        private SwivelAnimation objAnimation;
        PagedCollectionView collection;
        private bool IsCancel = true;
        private string msgText = String.Empty;
        private string msgTitle = String.Empty;
        private bool IsModify = false;
        private long SelectedAgencyID = 0;
        private long SelectedClinicID = 0;
        private bool SelectedAgencyDefaultStatus = false;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private long ServiceID = 0;
        private decimal iRate = 0;
        private bool DefaultList;
        private string ServiceIDList = String.Empty;
        private string RateList = String.Empty;
        private string IsDefaultList = String.Empty;
        StringBuilder ServiceIDBuilder;
        StringBuilder ServiceRateBuilder;
        StringBuilder IsDefaultBuilder;
        List<clsSpecializationVO> list = new List<clsSpecializationVO>();
        private List<clsServiceMasterVO> SelectedServiceList;
        #endregion

        #region Paging
        public PagedSortableCollectionView<clsServiceMasterVO> ServiceList { get; private set; }
        private List<clsServiceMasterVO> AllServiceList;
        private List<clsServiceMasterVO> _OtherServiceSelected;
        public List<clsServiceMasterVO> SelectedOtherServices { get { return _OtherServiceSelected; } }
        public int ServiceListPageSize
        {
            get
            {
                return ServiceList.PageSize;
            }
            set
            {
                if (value == ServiceList.PageSize) return;
                ServiceList.PageSize = value;
            }
        }
        public int MasterPageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
            }
        }
        public bool IsNewLink = false;
        #endregion

        public frmAgencyServiceLinking()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            FillClinic();
            _OtherServiceSelected = new List<clsServiceMasterVO>();
            MasterList = new PagedSortableCollectionView<clsAgencyClinicLinkVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            MasterPageSize = 15;
            this.dgDataPager.DataContext = MasterList;
            this.dgAgencyServiceList.DataContext = MasterList;

            ServiceList = new PagedSortableCollectionView<clsServiceMasterVO>();
            ServiceList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            ServiceListPageSize = 15;
            this.dataGrid2Pager.DataContext = ServiceList;
            this.dgServiceList.DataContext = ServiceList;

            AllServiceList = new List<clsServiceMasterVO>();
            AgencyList = new List<clsAgencyMasterVO>();
            SelectedAgencyList = new List<clsAgencyMasterVO>();
            ModifyAgencyList = new List<clsAgencyMasterVO>();
            ServiceIDBuilder = new StringBuilder();
            ServiceRateBuilder = new StringBuilder();
            IsDefaultBuilder = new StringBuilder();
            SelectedServiceList = new List<clsServiceMasterVO>();
            _OtherServiceSelected = new List<clsServiceMasterVO>();
        }
        #region Loaded Event And Refresh Events
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillAgencyClinicLinkList();
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchServiceList();
        }
        private void frmAgencyServiceLink_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedSpecializationList = new List<clsSpecializationVO>();
            FillALLAgencySpecializationList();
            FillAgencyClinicLinkList();
            //cmdSave.IsEnabled = true;

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            IsNewLink = true;
            try
            {
                if (dgAgencyServiceList.SelectedItem == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Select one Agency From List.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                else
                {
                    GetAgencyServiceLink();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Save Service Agency Linking?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        IsModify = true;
                        SaveAgencyServiceLink();
                    }
                };
                msgWindow.Show();
              
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Update Service Agency Linking?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveAgencyServiceLink();
                    }
                };
                msgWindow.Show();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");

            
            objAnimation.Invoke(RotationType.Backward);
            ClearUI();
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Clinic Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.Agency.frmAgencyConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

                //to display name as per pranav sir said
                UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");
                mElement1.Text = " ";
            }
            else
            {
                IsCancel = true;
                //to display name as per pranav sir said
                UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");
                mElement1.Text = " ";
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillAgencyClinicLinkList();
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillAgencyClinicLinkList();
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (dgAgencyServiceList.SelectedItem != null)
            {
                clsAgencyClinicLinkVO objVO = dgAgencyServiceList.SelectedItem as clsAgencyClinicLinkVO;
                objAnimation.Invoke(RotationType.Forward);
                SetCommandButtonState("View");
                SelectedAgencyID = objVO.AgencyID;
                SelectedClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                SelectedAgencyDefaultStatus = objVO.IsDefault;
                FetchServiceList();
                //FetchAgencySpecializationList(SelectedAgencyID);
                FillSpecialization();
                SelectedServiceList = new List<clsServiceMasterVO>();
                GetAgencyServiceLink(); 
                if (rdbApplyDiscountBaseRate.IsChecked == true)
                {
                    txtDiscount.Visibility = Visibility.Visible;
                }
                else
                {
                    txtDiscount.Visibility = Visibility.Collapsed;
                }

                //to display name as per pranav sir said
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + objVO.Description;
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Select one Agency From List.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
        }

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialization.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID);
            }
        }

        private void cmdBackPanelSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchServiceList();
        }

        private void txtServiceName_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter || e.Key == Key.Back || e.Key == Key.Delete)
            {
                FetchServiceList();
            }
        }

        private void rdbApplyDefaultAgency_Click(object sender, RoutedEventArgs e)
        {
            if (rdbApplyDefaultAgency.IsChecked == true)
            {
                ServiceList.ToList().ForEach(z => z.IsDefaultAgency = true);
                if (rdbCheckAll.IsChecked == true && AllServiceList != null && AllServiceList.Count > 0)
                {
                    AllServiceList.ToList().ForEach(x => x.IsDefaultAgency = true);
                }
                dgSelectedServiceList.ItemsSource = null;
                if (rdbCheckAll.IsChecked == true)
                    dgSelectedServiceList.ItemsSource = AllServiceList;
                //else
                //    dgSelectedServiceList.ItemsSource = null;
                dgSelectedServiceList.UpdateLayout();

                dgServiceList.ItemsSource = null;
                dgServiceList.ItemsSource = ServiceList;
                dgServiceList.UpdateLayout();
            }
            else
            {
                ServiceList.ToList().ForEach(z => z.IsDefaultAgency = false);
                if (rdbCheckAll.IsChecked == true && AllServiceList != null && AllServiceList.Count > 0)
                {
                    AllServiceList.ToList().ForEach(x => x.IsDefaultAgency = false);
                }
                dgSelectedServiceList.ItemsSource = null;
                if (rdbCheckAll.IsChecked == true)
                    dgSelectedServiceList.ItemsSource = AllServiceList;
                //else
                //    dgSelectedServiceList.ItemsSource = null;
                dgSelectedServiceList.UpdateLayout();

                dgServiceList.ItemsSource = null;
                dgServiceList.ItemsSource = ServiceList;
                dgServiceList.UpdateLayout();
            }
        }

        private void rdbCheckAll_Click(object sender, RoutedEventArgs e)
        {
            if (rdbCheckAll.IsChecked == true)
            {
                dgSelectedServiceList.ItemsSource = null;
                GetAllServicesForAgency();
            }
            else
            {
                if (AllServiceList != null && AllServiceList.Count > 0)
                {
                    ServiceList.ToList().ForEach(z => z.SelectService = false);
                    AllServiceList.ToList().ForEach(z => z.SelectService = false);
                    dgServiceList.ItemsSource = null;
                    dgServiceList.ItemsSource = ServiceList;
                    dgServiceList.UpdateLayout();
                    _OtherServiceSelected.Clear();
                    dgSelectedServiceList.ItemsSource = null;
                    dgSelectedServiceList.UpdateLayout();
                }
            }
        }

        private void rdbApplyDiscountBaseRate_Click(object sender, RoutedEventArgs e)
        {
            if (rdbApplyDiscountBaseRate.IsChecked == true)
            {
                txtDiscount.Visibility = Visibility.Visible;
            }
            else
            {
                txtDiscount.Visibility = Visibility.Collapsed;
            }
        }

        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtNo_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceList.SelectedItem != null)
            {


                //NEWLY ADDEDE BY ROHINI AS PER MILANNS REQUIREMENT

                long UnitID = 0;
                long AgencyID = 0;
                long ServiceID = 0;

               
                clsServiceMasterVO objVO = (clsServiceMasterVO)dgSelectedServiceList.SelectedItem;
                clsServiceMasterVO objServiceVO = dgServiceList.SelectedItem as clsServiceMasterVO;
                if (_OtherServiceSelected == null)
                    _OtherServiceSelected = new List<clsServiceMasterVO>();
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    if (_OtherServiceSelected.Count > 0)
                    {
                        var item = from r in _OtherServiceSelected
                                   where r.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceID
                                   select new clsServiceMasterVO
                                   {
                                       Status = r.Status,
                                       ID = r.ID,
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
                            }
                        }
                        else
                        {
                            _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                        }
                    }
                    else
                    {
                        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                    }
                }
                else
                {
                    clsServiceMasterVO obj;
                    if (objVO != null)
                    {
                        obj = _OtherServiceSelected.Where(z => z.ServiceID == objVO.ServiceID).FirstOrDefault();
                        _OtherServiceSelected.Remove(obj);
                        ServiceList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                        dgServiceList.ItemsSource = null;
                        dgServiceList.ItemsSource = ServiceList;
                        dataGrid2Pager.Source = null;
                        dataGrid2Pager.Source = ServiceList;
                        dgServiceList.UpdateLayout();
                    }
                    else if (objServiceVO != null)
                    {
                        obj = _OtherServiceSelected.Where(z => z.ServiceID == objServiceVO.ServiceID).FirstOrDefault();
                        _OtherServiceSelected.Remove(obj);
                        ServiceList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                        dgServiceList.ItemsSource = null;
                        dgServiceList.ItemsSource = ServiceList;
                        dataGrid2Pager.Source = null;
                        dataGrid2Pager.Source = ServiceList;
                        dgServiceList.UpdateLayout();
                    }
                }
                foreach (var item in _OtherServiceSelected)
                {
                    item.SelectService = true;
                }
                dgSelectedServiceList.ItemsSource = null;
                dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                dgSelectedServiceList.UpdateLayout();
                dgSelectedServiceList.Focus();
            }
        }

        private void chkIsDefault_Click(object sender, RoutedEventArgs e)
        {
            #region added by rohini for only one service to one agency
            WaitIndicator Indicatior = new WaitIndicator();
          
            try
            {
                Indicatior.Show();
                clsGetServiceToAgencyBizActionVO bizAction = new clsGetServiceToAgencyBizActionVO();
                bizAction.ServiceDetails = new clsServiceMasterVO();
                bizAction.ServiceList = new List<clsServiceMasterVO>();
                bizAction.IsAgencyServiceLinkView = true;

                 if (dgAgencyServiceList.SelectedItem != null)
                {
                    bizAction.UnitID = ((clsAgencyClinicLinkVO)dgAgencyServiceList.SelectedItem).ApplicableUnitID;

                bizAction.AgencyID = ((clsAgencyClinicLinkVO)dgAgencyServiceList.SelectedItem).AgencyID;
                bizAction.ServiceID = ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ServiceID;

                }
                //if (dgAgencyServiceList.SelectedItem != null)
                //{
                //    clsAgencyClinicLinkVO objVO = dgAgencyServiceList.SelectedItem as clsAgencyClinicLinkVO;
                //    bizAction.ServiceDetails.AgencyID = objVO.AgencyID;
                //    bizAction.ServiceDetails.UnitID = objVO.UnitID;
                //}
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    List<clsServiceMasterVO> result = ((clsGetServiceToAgencyBizActionVO)args.Result).ServiceList ;
                    //if (SelectedServiceList != null)
                    //    SelectedServiceList.Clear();
                    //if (_OtherServiceSelected != null)
                    //    _OtherServiceSelected.Clear();
                    if (((clsGetServiceToAgencyBizActionVO)args.Result).ServiceDetails.AssignedAgencyID != 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Default service is already assigned to other agency", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        CheckBox chk = sender as CheckBox;
                       
                        foreach (var item in _OtherServiceSelected)
                        {

                            
                            if (((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ServiceID == item.ServiceID && chk.IsChecked==true)
                                {
                                    item.IsDefaultAgency = false;
                                    chk.IsChecked = false;
                                }
                            
                        }
                        dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                        dgSelectedServiceList.UpdateLayout();
                        
                    }
                    else
                    {
                     
                    }
                };
                client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Indicatior.Close();
            }

            #endregion



            //
        }

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSearchService_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtServiceSearch.Text))
            {
                GetAllServicesForAgency();
            }

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtServiceSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Back || e.Key == Key.Delete)
            {
                //GetAllServicesForAgency();
            }
        }
        private void GetAllServicesForAgency()
        {
            WaitIndicator objWait = new WaitIndicator();
            objWait.Show();
            try
            {
                clsGetServiceListBizActionVO bizActionObj = new clsGetServiceListBizActionVO();
                bizActionObj.ServiceList = new List<clsServiceMasterVO>();
                bizActionObj.SpecializationID = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                if (dgAgencyServiceList.SelectedItem != null)
                {
                    bizActionObj.UnitID = ((clsAgencyClinicLinkVO)dgAgencyServiceList.SelectedItem).UnitID;
                    bizActionObj.AgencyID = ((clsAgencyClinicLinkVO)dgAgencyServiceList.SelectedItem).AgencyID;
                }
                bizActionObj.SubSpecializationID = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;
                if (!String.IsNullOrEmpty(txtServiceSearch.Text))
                    bizActionObj.ServiceName = txtServiceSearch.Text;
                else
                    bizActionObj.ServiceName = null;
                bizActionObj.PagingEnabled = false;
                bizActionObj.StartRowIndex = ServiceList.PageIndex * ServiceList.PageSize;
                bizActionObj.MaximumRows = ServiceList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null && ((clsGetServiceListBizActionVO)args.Result).ServiceList != null)
                    {
                        if (((clsGetServiceListBizActionVO)args.Result).ServiceList != null)
                        {
                            AllServiceList.Clear();
                            if (SelectedAgencyDefaultStatus == true)
                            {
                                foreach (var item in ((clsGetServiceListBizActionVO)args.Result).ServiceList)
                                {
                                    item.IsDefaultAgency = true;
                                    AllServiceList.Add(item);
                                }
                            }
                            else
                            {
                                foreach (var item in ((clsGetServiceListBizActionVO)args.Result).ServiceList)
                                {
                                    item.IsDefaultAgency = false;
                                    AllServiceList.Add(item);
                                }
                            }
                            if (AllServiceList != null && AllServiceList.Count > 0)
                            {
                                AllServiceList.ToList().ForEach(z => z.SelectService = true);
                                ServiceList.ToList().ForEach(z => z.SelectService = true);
                                _OtherServiceSelected.Clear();
                                _OtherServiceSelected = AllServiceList.ToList();
                            }
                            if (rdbApplyDefaultAgency.IsChecked == true)
                            {
                                AllServiceList.ToList().ForEach(z => z.IsDefaultAgency = true);
                                ServiceList.ToList().ForEach(x => x.IsDefaultAgency = true);
                                _OtherServiceSelected.Clear();
                                _OtherServiceSelected = AllServiceList.ToList();
                            }
                        }
                        dgServiceList.ItemsSource = null;
                        dgServiceList.ItemsSource = ServiceList;
                        dgServiceList.UpdateLayout();
                        dgSelectedServiceList.ItemsSource = null;
                        dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                        dgSelectedServiceList.UpdateLayout();
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                objWait.Close();
            }
            return;
        }
        private void GetAgencyServiceLink()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                Indicatior.Show();
                clsGetServiceListBizActionVO bizAction = new clsGetServiceListBizActionVO();
                bizAction.ServiceDetails = new clsServiceMasterVO();
                bizAction.ServiceList = new List<clsServiceMasterVO>();
                bizAction.IsAgencyServiceLinkView = true;
                if (dgAgencyServiceList.SelectedItem != null)
                {
                    clsAgencyClinicLinkVO objVO = dgAgencyServiceList.SelectedItem as clsAgencyClinicLinkVO;
                    bizAction.ServiceDetails.AgencyID = objVO.AgencyID;
                    bizAction.ServiceDetails.UnitID = objVO.ApplicableUnitID;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    clsGetServiceListBizActionVO result = args.Result as clsGetServiceListBizActionVO;
                    if (SelectedServiceList != null)
                        SelectedServiceList.Clear();
                    if (_OtherServiceSelected != null)
                        _OtherServiceSelected.Clear();
                    if (result.ServiceList != null)
                    {
                        foreach (clsServiceMasterVO item in result.ServiceList)
                        {
                            SelectedServiceList.Add(item);
                        }
                        _OtherServiceSelected = SelectedServiceList;
                        foreach (var item in _OtherServiceSelected)
                        {
                            item.SelectService = true;
                        }
                        dgSelectedServiceList.ItemsSource = null;
                        dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                        dgSelectedServiceList.UpdateLayout();
                        if (_OtherServiceSelected != null && _OtherServiceSelected.Count > 0 && IsNewLink == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Services Are Already Linked With The Selected Agency.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    objAnimation.Invoke(RotationType.Backward);
                                    SetCommandButtonState("Load");
                                    IsNewLink = false;
                                }
                            };
                            msgWindow.Show();
                        }
                        else
                        {
                            try
                            {
                                SelectedAgencyID = ((clsAgencyClinicLinkVO)dgAgencyServiceList.SelectedItem).AgencyID;
                                SelectedClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                                SelectedAgencyDefaultStatus = ((clsAgencyClinicLinkVO)dgAgencyServiceList.SelectedItem).IsDefault;
                                if (IsNewLink == true)
                                {
                                    SetCommandButtonState("New");
                                    ClearUI();
                                }
                                FetchServiceList();
                                FillSpecialization();
                                objAnimation.Invoke(RotationType.Forward);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }
                };
                client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Indicatior.Close();
            }
        }
        private void FetchAgencySpecializationList(long SelectedAgencyID)
        {
            list = new List<clsSpecializationVO>();
            list = SelectedSpecializationList.ToList().Where(z => z.AgencyID == SelectedAgencyID).ToList();

            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "-- Select --"));
            foreach (var item in list)
            {
                objList.Add(new MasterListItem { ID = item.SpecializationId, Description = item.SpecializationName });
            }
            cmbSpecialization.ItemsSource = null;
            cmbSpecialization.ItemsSource = objList;
            cmbSpecialization.SelectedItem = objList[0];
        }

        private void FillSpecialization()
        {
            clsGetSpecializationDetailsBizActionVO BizAction = new clsGetSpecializationDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.IsFromAgency = true;
            BizAction.AgencyID = SelectedAgencyID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetSpecializationDetailsBizActionVO)arg.Result).MasterList);
                    cmbSpecialization.ItemsSource = null;
                    cmbSpecialization.ItemsSource = objList;
                    cmbSpecialization.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
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
                    if (iSupId > 0)
                        cmbSubSpecialization.IsEnabled = true;
                    else
                        cmbSubSpecialization.IsEnabled = false;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillAgencyClinicLinkList()
        {
            try
            {
                WaitIndicator Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetAgencyClinicLinkBizActionVO bizActionObj = new clsGetAgencyClinicLinkBizActionVO();
                bizActionObj.AgencyClinicLinkDetails = new clsAgencyClinicLinkVO();
                if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID > 0)
                {
                    bizActionObj.AgencyClinicLinkDetails.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }
                else
                {
                    bizActionObj.AgencyClinicLinkDetails.UnitID = 0;
                }
                bizActionObj.PagingEnabled = true;
                bizActionObj.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                bizActionObj.MaximumRows = MasterList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetAgencyClinicLinkBizActionVO result = args.Result as clsGetAgencyClinicLinkBizActionVO;
                        MasterList.Clear();
                        SelectedAgencyList.Clear();
                        SelectedAgencyList = result.AgencyMasterList;
                        if (result.AgencyMasterList != null)
                        {
                            MasterList.TotalItemCount = (int)((clsGetAgencyClinicLinkBizActionVO)args.Result).TotalRows;
                            foreach (clsAgencyClinicLinkVO item in result.AgencyClinicLinkList)
                            {
                                MasterList.Add(item);
                            }
                            dgAgencyServiceList.ItemsSource = null;
                            collection = new PagedCollectionView(MasterList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("ClinicName"));
                            dgAgencyServiceList.ItemsSource = collection;
                            dgAgencyServiceList.SelectedIndex = -1;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = MasterList.PageSize;
                            dgDataPager.Source = MasterList;
                            Indicatior.Close();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        Indicatior.Close();
                    }
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            return;
        }

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
                    IsModify = false;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    IsModify = false;
                    txtDiscount.Visibility = Visibility.Collapsed;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    //new added
                    cmdSave.IsEnabled = true;
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
                    IsModify = false;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    IsModify = true;
                    txtDiscount.Visibility = Visibility.Collapsed;
                    break;
                case "View1":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }
        private void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;

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
                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    cmbClinic.SelectedItem = objList[0];

                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void FillALLAgencySpecializationList()
        {
            try
            {
                clsGetAgencyMasterListBizActionVO bizActionObj = new clsGetAgencyMasterListBizActionVO();
                bizActionObj.AgencyMasterDetails = new clsAgencyMasterVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetAgencyMasterListBizActionVO result = args.Result as clsGetAgencyMasterListBizActionVO;
                        ServiceList.Clear();
                        SelectedSpecializationList.Clear();
                        SelectedSpecializationList = result.AgencyMasterDetails.SelectedSpecializationList;

                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            return;
        }
        private void SaveAgencyServiceLink()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                Indicatior.Show();
                IsDefaultList = String.Empty;
                ServiceIDList = String.Empty;
                RateList = String.Empty;
                IsDefaultList = String.Empty;
                ServiceIDBuilder = new StringBuilder();
                ServiceRateBuilder = new StringBuilder();
                IsDefaultBuilder = new StringBuilder();
                /*================================================================================================ */
                if (_OtherServiceSelected.ToList() != null && _OtherServiceSelected.ToList().Count > 0)
                {
                    foreach (clsServiceMasterVO item in _OtherServiceSelected.ToList())
                    {
                        ServiceID = item.ServiceID;
                        iRate = item.Rate;
                        DefaultList = item.IsDefaultAgency;
                        ServiceIDBuilder.Append(ServiceID).Append(",");
                        ServiceRateBuilder.Append(iRate).Append(",");
                        IsDefaultBuilder.Append(DefaultList).Append(",");
                    }
                }
                ServiceIDList = ServiceIDBuilder.ToString();
                RateList = ServiceRateBuilder.ToString();
                IsDefaultList = IsDefaultBuilder.ToString();
                if (ServiceIDList != null && ServiceIDList.Length != 0)
                {
                    ServiceIDList = ServiceIDList.TrimEnd(',');
                }
                if (RateList != null && RateList.Length != 0)
                {
                    RateList = RateList.TrimEnd(',');
                }
                if (IsDefaultList != null && IsDefaultList.Length != 0)
                {
                    IsDefaultList = IsDefaultList.TrimEnd(',');
                }
                /* ================================================================================================ */

                clsAddServiceAgencyLinkBizActionVO BizAction = new clsAddServiceAgencyLinkBizActionVO();
                BizAction.objServiceAgencyDetails = new clsAgencyMasterVO();
                if (dgAgencyServiceList.SelectedItem != null)
                {
                    clsAgencyClinicLinkVO objAgency = (clsAgencyClinicLinkVO)dgAgencyServiceList.SelectedItem;
                    BizAction.objServiceAgencyDetails.UnitID = objAgency.UnitID;
                    BizAction.objServiceAgencyDetails.ID = objAgency.AgencyID;

                    BizAction.objServiceAgencyDetails.ApplicableUnitID = objAgency.ApplicableUnitID;

                }
                if (IsDefaultList != null && IsDefaultList.Length != 0)
                {
                    BizAction.objServiceAgencyDetails.IsDefaultList = IsDefaultList;
                }
                if (ServiceIDList != null && ServiceIDList.Length > 0)
                {
                    BizAction.objServiceAgencyDetails.ServiceIDList = ServiceIDList;
                }
                if (RateList != null && RateList.Length > 0)
                {
                    BizAction.objServiceAgencyDetails.RateList = RateList;
                }
                if (!String.IsNullOrEmpty(txtDiscount.Text))
                {
                    BizAction.objServiceAgencyDetails.Discount = Convert.ToDecimal(txtDiscount.Text);
                }
                if (IsModify == true)
                {
                    BizAction.IsModify = IsModify;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null && ((clsAddServiceAgencyLinkBizActionVO)args.Result).SuccessStatus == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1;
                        if (IsModify == true && BizAction.IsModify == true)
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Agency Service Linking Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        else
                            msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Agency Service Linking Added Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Load");
                                FillAgencyClinicLinkList();
                            }
                        };
                        msgW1.Show();
                        //new added
                        //IsModify = false;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Indicatior.Close();
            }
        }
        private bool Validation()
        {
            bool IsValidate = true;
            if (String.IsNullOrEmpty(txtDiscount.Text) && rdbApplyDiscountBaseRate.IsChecked == true)
            {
                txtDiscount.SetValidation("Please, Enter Discount Percentage.");
                txtDiscount.RaiseValidationError();
                txtDiscount.Focus();
                IsValidate = false;
            }
            if (!String.IsNullOrEmpty(txtDiscount.Text) && Extensions.IsItDecimal(txtDiscount.Text) == true && rdbApplyDiscountBaseRate.IsChecked == true)
            {

                txtDiscount.SetValidation(" Concession Amount should be number");
                txtDiscount.RaiseValidationError();
                txtDiscount.Focus();
                IsValidate = false;
            }
            if (_OtherServiceSelected != null && _OtherServiceSelected.ToList().Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please, Select Services To Link With AgencyY.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                IsValidate = false;
            }
            return IsValidate;
        }
        private void FetchServiceList()
        {
            WaitIndicator objWait = new WaitIndicator();
            objWait.Show();
            try
            {
                clsGetServiceListBizActionVO bizActionObj = new clsGetServiceListBizActionVO();
                bizActionObj.ServiceList = new List<clsServiceMasterVO>();
                if (cmbSpecialization.SelectedItem != null)
                    bizActionObj.SpecializationID = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                if (dgAgencyServiceList.SelectedItem != null)
                {
                    bizActionObj.UnitID = ((clsAgencyClinicLinkVO)dgAgencyServiceList.SelectedItem).UnitID;
                    bizActionObj.AgencyID = ((clsAgencyClinicLinkVO)dgAgencyServiceList.SelectedItem).AgencyID;
                }
                if (cmbSubSpecialization.SelectedItem != null)
                    bizActionObj.SubSpecializationID = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;
                if (txtServiceName.Text != "")
                    bizActionObj.ServiceName = txtServiceName.Text;
                else
                    bizActionObj.ServiceName = null;
                bizActionObj.PagingEnabled = true;
                bizActionObj.StartRowIndex = ServiceList.PageIndex * ServiceList.PageSize;
                bizActionObj.MaximumRows = ServiceList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null && ((clsGetServiceListBizActionVO)args.Result).ServiceList != null)
                    {
                        if (((clsGetServiceListBizActionVO)args.Result).ServiceList != null)
                        {
                            ServiceList.Clear();
                            ServiceList.TotalItemCount = (int)((clsGetServiceListBizActionVO)args.Result).TotalRows;
                            if (SelectedAgencyDefaultStatus == true)
                            {
                                foreach (var item in ((clsGetServiceListBizActionVO)args.Result).ServiceList)
                                {
                                    //coomented for temp pupose
                                    //item.IsDefaultAgency = true;
                                    ServiceList.Add(item);
                                }
                            }
                            else
                            {
                                foreach (var item in ((clsGetServiceListBizActionVO)args.Result).ServiceList)
                                {
                                   //item.IsDefaultAgency = false;
                                    ServiceList.Add(item);
                                }
                            }
                            if (_OtherServiceSelected != null && _OtherServiceSelected.Count > 0)
                            {
                                foreach (clsServiceMasterVO item in _OtherServiceSelected.ToList())
                                {
                                    ((clsGetServiceListBizActionVO)args.Result).ServiceList.ToList().ForEach(x =>
                                    {
                                        if (item.ServiceID == x.ServiceID)
                                        {
                                            x.SelectService = true;
                                        }
                                    });
                                }
                            }
                            if (rdbApplyDefaultAgency.IsChecked == true)
                            {
                                ServiceList.ToList().ForEach(z => z.IsDefaultAgency = true);
                            }
                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = ServiceList;

                            dataGrid2Pager.Source = null;
                            dataGrid2Pager.Source = ServiceList;
                            dgServiceList.UpdateLayout();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                objWait.Close();
            }
            return;
        }

        private void ClearUI()
        {
            if (_OtherServiceSelected != null)
            {
                _OtherServiceSelected.Clear();
                _OtherServiceSelected = new List<clsServiceMasterVO>();
            }
            if (SelectedServiceList != null)
            {
                SelectedServiceList.Clear();
                SelectedServiceList = new List<clsServiceMasterVO>();
            }
            dgSelectedServiceList.ItemsSource = null;
            IsModify = false;
            txtServiceSearch.Text = String.Empty;
            if (AllServiceList != null)
            {
                AllServiceList.Clear();
                AllServiceList = new List<clsServiceMasterVO>();
            }
            rdbCheckAll.IsChecked = false;
            rdbApplyDefaultAgency.IsChecked = false;
            rdbApplyDiscountBaseRate.IsChecked = false;
            txtDiscount.Text = String.Empty;
            txtServiceName.Text = String.Empty;
            txtServiceSearch.Text = String.Empty;
            txtDiscount.Visibility = Visibility.Collapsed;
        }

        #endregion


    }
}
