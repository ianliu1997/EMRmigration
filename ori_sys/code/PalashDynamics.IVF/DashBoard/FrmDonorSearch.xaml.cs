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
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Collections;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmDonorSearch : ChildWindow
    {
        public FrmDonorSearch()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DonorSearch_Loaded);
            this.DataContext = new clsDonorGeneralDetailsVO();

            DataList = new PagedSortableCollectionView<clsDonorGeneralDetailsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;
        }
        void DonorSearch_Loaded(object sender, RoutedEventArgs e)
        {
          
            txtDonorCode.Focus();
            GetData();
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }

        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;

        #region fillcombox
        private void FillLab()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_LaboratoryMaster;
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
                    cmbLab.ItemsSource = null;
                    cmbLab.ItemsSource = objList.DeepCopy();
                    cmbLab.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {

                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion
        public PagedSortableCollectionView<clsDonorGeneralDetailsVO> DataList { get; private set; }
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                //RaisePropertyChanged("PageSize");
            }
        }
        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }
        private void GetData() 
        {
            clsGetDonorDetailsAgainstSearchBizActionVO BizActionVo = new clsGetDonorDetailsAgainstSearchBizActionVO();
            BizActionVo.DonorGeneralDetails = new clsDonorGeneralDetailsVO();
            BizActionVo.DonorGeneralDetailsList = new List<clsDonorGeneralDetailsVO>();

            BizActionVo.DonorGeneralDetails.DonorCode = txtDonorCode.Text;
            //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            //{
            //    BizActionVo.UnitID = 0;
            //}
            //else
            //{
            BizActionVo.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}

            BizActionVo.IsPagingEnabled = true;
            BizActionVo.MaximumRows = DataList.PageSize; ;
            BizActionVo.StartIndex = DataList.PageIndex * DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        clsGetDonorDetailsAgainstSearchBizActionVO result = ea.Result as clsGetDonorDetailsAgainstSearchBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        foreach (clsDonorGeneralDetailsVO person in result.DonorGeneralDetailsList)
                        {
                            DataList.Add(person);
                        }
                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = DataList;

                        DataPager.Source = null;
                        DataPager.PageSize = BizActionVo.MaximumRows;
                        DataPager.Source = DataList;
                    }
                }
            };
            client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        public event RoutedEventHandler OnSaveButton_Click;
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null && ((clsDonorGeneralDetailsVO)dataGrid2.SelectedItem).DonorID != 0)
            {
                this.DialogResult = true;
                //if (OnSaveButton_Click != null)
                //{
                //    OnSaveButton_Click(this, new RoutedEventArgs());
                //    this.Close();
                //}
                frmDonorRegistration win = new frmDonorRegistration();
                
                //win.PatientID=((clsDonorGeneralDetailsVO)dataGrid2.SelectedItem).DonorID;
                //win.PatientUnitID=((clsDonorGeneralDetailsVO)dataGrid2.SelectedItem).DonorUnitID;
                //win.IsFromDonorSearch=true;
                //win.OnSaveButton_Click += new RoutedEventHandler(Win_DonorFromregistration);
                //win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Donor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }

        }
        private void Win_DonorFromregistration(object sender, RoutedEventArgs e)
        {
            frmDonorRegistration win = (frmDonorRegistration)sender;
            //BatchID = win.BatchID;
            //BatchUnitID = win.BatchUnitID;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }
        public long PatientID;
        public long PatientUnitID;
        public long BatchID;
        public long BatchUnitID;
        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PatientID = ((clsDonorGeneralDetailsVO)dataGrid2.SelectedItem).DonorID;
            PatientUnitID = ((clsDonorGeneralDetailsVO)dataGrid2.SelectedItem).DonorUnitID;
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

     
    }
}
