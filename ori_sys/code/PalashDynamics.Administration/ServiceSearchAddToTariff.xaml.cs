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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;

namespace PalashDynamics.Administration
{
    public partial class ServiceSearchAddToTariff : ChildWindow
    {
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();

        public List<clsServiceMasterVO> ServiceItemSource { get; set; }

        public List<clsServiceMasterVO> SelectedServices { get; set; }

        public long PatientSourceID { get; set; }

        public long ClassID { get; set; }
        public long PatientTariffID { get; set; }

        bool IsPageLoded = false;
        bool IsTariffFisrtFill = true;

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

        WaitIndicator Indicatior = null;

        public ServiceSearchAddToTariff()
        {
            InitializeComponent();
            ClassID = 1; // Default

            this.Loaded += new RoutedEventHandler(ServiceSearchAddToTariff_Loaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================

        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillService();
        }

        void ServiceSearchAddToTariff_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsServiceMasterVO();

                //PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                //FillPatientSponsorDetails();
                //// FillTariff(true);
                //FillSpecialization();
                //SetComboboxValue();

                FillService();

                txtServiceName.Focus();
            }
            else
                txtServiceName.Focus();

            txtServiceName.UpdateLayout();
            IsPageLoded = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            SelectedServices = new List<clsServiceMasterVO>();

            for (int i = 0; i < check.Count; i++)
            {
                if (check[i])
                {
                    SelectedServices.Add(ServiceItemSource[i]);
                    //    new clsServiceMasterVO()
                    //{
                    //    //ID = ServiceItemSource[i].ID,
                    //    //Description = ServiceItemSource[i].ServiceName,
                    //    //TariffServiceMasterID = ServiceItemSource[i].TariffServiceMasterID,
                    //    //TariffID = ServiceItemSource[i].TariffID,
                    //    //Specialization = ServiceItemSource[i].Specialization,
                    //    //Rate = ServiceItemSource[i].Rate, Code = ServiceItemSource[i].ServiceCode
                    //});
                }
            }

            if (SelectedServices.Count == 0)
            {
                string strMsg = "No Service/s Selected for Adding";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else
            {
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());

            }

        }


        private void FillService()
        {
            try
            {

                clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();
                BizAction.IsStatus = true;  // this flag is used for getting the services from M_ServiceMaster whose Status is True.

                if (txtServiceName.Text != null)
                    BizAction.ServiceName = txtServiceName.Text;

                BizAction.IsPagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList != null)
                        {
                            DataList.TotalItemCount = (int)(((clsGetServiceMasterListBizActionVO)arg.Result).TotalRows);   //result.TotalRows;
                            DataList.Clear();

                            //dgServiceList.ItemsSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
                            BizAction.ServiceList = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
                            List<clsServiceMasterVO> ObjServiceList = new List<clsServiceMasterVO>();

                            foreach (var item in BizAction.ServiceList)
                            {
                                ObjServiceList.Add(new clsServiceMasterVO()
                                {
                                    ServiceID = item.ID,
                                    ServiceCode = item.ServiceCode,
                                    ServiceName = item.ServiceName,
                                    Specialization = item.Specialization,
                                    SubSpecialization = item.SubSpecialization,
                                    Description = item.Description,
                                    ShortDescription = item.ShortDescription,
                                    LongDescription = item.LongDescription,
                                    Rate = item.Rate,
                                    SelectService = false,
                                });
                            }

                            //dgServiceList.ItemsSource = ObjServiceList;

                            foreach (var item in ObjServiceList)   //((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList)  
                            {
                                DataList.Add(item);
                            }

                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;


                            check.Clear();
                            //dgServiceList.ItemsSource = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;
                            ServiceItemSource = DataList.ToList();

                            for (int i = 0; i < DataList.Count; i++)
                            {
                                bool b = false;
                                check.Add(b);
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

        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {

                check[dgServiceList.SelectedIndex] = true;
            }
            else
            {
                check[dgServiceList.SelectedIndex] = false;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillService();
        }

        private void txtServiceName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillService();
            }
        }



    }
}
