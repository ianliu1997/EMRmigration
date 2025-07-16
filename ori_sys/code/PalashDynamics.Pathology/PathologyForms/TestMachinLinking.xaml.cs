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
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class TestMachinLinking : ChildWindow
    {
        #region Variable

        public clsPathOrderBookingDetailVO SelectedTest = new clsPathOrderBookingDetailVO();
        public PagedSortableCollectionView<clsMasterListVO> MasterList { get; private set; }
        public List<clsMasterListVO> SelectedMachineList { get; private set; }
        public int PageSize
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
        public event RoutedEventHandler OnAddButton_Click;
        #endregion

        #region Load and Constructor
        public TestMachinLinking()
        {
            InitializeComponent();
            MasterList = new PagedSortableCollectionView<clsMasterListVO>();
            PageSize = 10;
            this.dataGrid2Pager.DataContext = MasterList;
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillMachine();
        }

        #endregion

        #region Onclick Events

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (MasterList.Where(z => z.IsChecked = true).Any())
            {
                SaveMachinLinking();
                this.DialogResult = false;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                string strMsg = "Please Select Machine For Linking";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public CheckBox previousCheckBox = null;

        private void chkMachine_Click(object sender, RoutedEventArgs e)
        {
            if (previousCheckBox != null)
            {
                foreach (var item in MasterList)
                    item.IsChecked = false;
                previousCheckBox.IsChecked = false;
            }
            CheckBox currentCheckBox = sender as CheckBox;
            previousCheckBox = currentCheckBox;
            SelectedMachineList = new List<clsMasterListVO>();
            if (currentCheckBox.IsChecked == true)
            {
                foreach (var item in MasterList)
                {
                    if (item.Id == ((clsMasterListVO)dgMachineList.SelectedItem).Id)
                        item.IsChecked = true;
                }
            }
            dgMachineList.ItemsSource = null;
            dgMachineList.ItemsSource = MasterList;
            dgMachineList.UpdateLayout();

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillMachine();
        }

        #endregion

        #region keyup

        private void txtServiceName_KeyUp(object sender, KeyEventArgs e)
        {
            FillMachine();
        }
        private void txtServiceCode_KeyUp(object sender, KeyEventArgs e)
        {
            FillMachine();
        }

        #endregion

        #region Private Methods

        private void FillMachine()
        {
            try
            {
                clsGetMasterListDetailsBizActionVO bizActionVO = new clsGetMasterListDetailsBizActionVO();
                bizActionVO.SearchExperssion = txtMachineName.Text.Trim();
                bizActionVO.PagingEnabled = true;
                bizActionVO.MasterTableName = "M_MachineMaster";
                
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                clsMasterListVO getMasterListinfo = new clsMasterListVO();
                bizActionVO.ItemMatserDetails = new List<clsMasterListVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.ItemMatserDetails = (((clsGetMasterListDetailsBizActionVO)args.Result).ItemMatserDetails);
                        if (bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            if (previousCheckBox == null)
                                previousCheckBox = new CheckBox();
                            foreach (clsMasterListVO item in bizActionVO.ItemMatserDetails)
                            {
                                if (item.Id == SelectedTest.MachineID)
                                    item.IsChecked = true;
                                else
                                    item.IsChecked = false;
                                previousCheckBox.IsChecked = item.IsChecked;
                                MasterList.Add(item);

                            }
                            dgMachineList.ItemsSource = null;
                            dgMachineList.ItemsSource = MasterList;
                        }

                    }
                    else
                    {
                        dgMachineList.ItemsSource = null;
                        dataGrid2Pager.Source = null;
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }
        private void SaveMachinLinking()
        {
            try
            {
                clsUpdatePathOrderBookingDetailListBizActionVO BizAction = new clsUpdatePathOrderBookingDetailListBizActionVO();
                BizAction.IsForMachinLinking = true;
                BizAction.OrderBookingDetaildetails = SelectedTest;
                BizAction.MachineID = Convert.ToInt64(((clsMasterListVO)dgMachineList.SelectedItem).Id);
                BizAction.MachineUnitID = Convert.ToInt64(((clsMasterListVO)dgMachineList.SelectedItem).UnitId);
                BizAction.MachineName = Convert.ToString(((clsMasterListVO)dgMachineList.SelectedItem).Description);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Machine Linking saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("NT7", "Error occured while adding Bill details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                throw;
            }
        }

        #endregion
    }
}

