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

namespace PalashDynamics.CRM
{
    public partial class ServiceDetails : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();

        public List<clsServiceMasterVO> ServiceItemSource { get; set; }

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public long TariffID { get; set; }

        #endregion

        public ServiceDetails()
        {
            InitializeComponent();
        }

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
            FillDataGrid();
            FillSpecialization();
            SetComboboxValue();
            txtServiceName.Focus();

        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (OnAddButton_Click != null)
            {
                this.DialogResult = true;
                OnAddButton_Click(this, new RoutedEventArgs());

                this.Close();
            }
        }

        /// <summary>
        /// Purpose:Getting list of Service.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
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
                        ServiceItemSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;

                        for (int i = 0; i < ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList.Count; i++)
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

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                        {
                          
                            dgServiceList.ItemsSource = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;
                            ServiceItemSource = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;

                            for (int i = 0; i < ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList.Count; i++)
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


        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialization.SelectedItem != null) ;
            FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID);
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
    }


}
