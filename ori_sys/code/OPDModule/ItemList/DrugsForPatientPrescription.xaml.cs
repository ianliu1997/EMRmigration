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
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Pharmacy.ViewModels;
using OPDModule;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Pharmacy
{
    public partial class DrugsForPatientPrescription : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public DrugsForPatientPrescription()
        {
            InitializeComponent();
            this.DataContext = new ItemSearchViewModel();
            this.Loaded += new RoutedEventHandler(DrugsForPatientPrescription_Loaded);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }


        void DrugsForPatientPrescription_Loaded(object sender, RoutedEventArgs e)
        {
            FillStores(ClinicID);
            if(VisitID!=0)
            { 
                clsGetPatientPrescriptionDetailByVisitIDBizActionVO BizAction=new clsGetPatientPrescriptionDetailByVisitIDBizActionVO();
                BizAction.VisitID = this.VisitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        dataGrid2.ItemsSource = ((clsGetPatientPrescriptionDetailByVisitIDBizActionVO)args.Result).PatientPrescriptionDetail;
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
        }


        public clsUserVO loggedinUser { get; set; }

        public long VisitID { get; set; }
        public long StoreID { get; set; }

        public long ClinicID
        {
            get
            {
                return ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
        }

        public clsPatientPrescriptionDetailVO SelectedDrug { get; set; }

        private ObservableCollection<clsItemStockVO> _BatchSelected;
        public ObservableCollection<clsItemStockVO> SelectedBatches { get { return _BatchSelected; } }

        private void FillStores(long pClinicID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
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

                    if (StoreID > 0)
                    {
                        cmbStore.SelectedValue = StoreID;
                    }
                    else
                    {
                        if (objList.Count > 1)
                            cmbStore.SelectedItem = objList[1];
                        else
                            cmbStore.SelectedItem = objList[0];
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDrug = null;
            _BatchSelected = null;

            this.DialogResult = false;
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (_BatchSelected != null)
                _BatchSelected.Clear();

            if (dataGrid2.SelectedItem != null)
            {
                SelectedDrug = (clsPatientPrescriptionDetailVO)dataGrid2.SelectedItem;
                if (SelectedDrug.DrugID == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is not available ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    // ((clsServiceMasterVO)dgPrescriptionServiceList.SelectedItem).SelectService = false;
                    SelectedDrug = new clsPatientPrescriptionDetailVO();

                }
                else
                {



                    ((ItemSearchViewModel)this.DataContext).SelectedItemID = ((clsPatientPrescriptionDetailVO)dataGrid2.SelectedItem).DrugID;
                    ((ItemSearchViewModel)this.DataContext).StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    ((ItemSearchViewModel)this.DataContext).loggedinUser = loggedinUser;
                    ((ItemSearchViewModel)this.DataContext).GetBatches();
                }
            }

            
            }            
        

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemBatches.SelectedItem != null)
            {
                if (_BatchSelected == null)
                    _BatchSelected = new ObservableCollection<clsItemStockVO>();

                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                    _BatchSelected.Add((clsItemStockVO)dgItemBatches.SelectedItem);
                else
                    _BatchSelected.Remove((clsItemStockVO)dgItemBatches.SelectedItem);

            }
        }

        private void dgItemBatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

