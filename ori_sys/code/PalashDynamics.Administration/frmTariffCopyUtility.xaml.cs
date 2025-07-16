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
using CIMS;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.Administration
{
    public partial class frmTariffCopyUtility : ChildWindow
    {
        #region Variable Declaration
        public PagedSortableCollectionView<clsTariffMasterBizActionVO> TariffList { get; private set; }
        public List<clsSubSpecializationVO> SelectedSubSpecializationList = new List<clsSubSpecializationVO>();
        long lTariffId { get; set; }
        long lSelectedSpecializationId { get; set; }
        FrameworkElement element;
        DataGridRow row;
        TextBox TxtDoctorShare;

        public int TariffListPageSize
        {
            get
            {
                return TariffList.PageSize;
            }
            set
            {
                if (value == TariffList.PageSize) return;
                TariffList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        #endregion

        #region Constructor and Loaded
        public frmTariffCopyUtility()
        {
            InitializeComponent();
            //======================================================
            TariffList = new PagedSortableCollectionView<clsTariffMasterBizActionVO>();
            SelectedSubSpecializationList = new List<clsSubSpecializationVO>();
            TariffList.OnRefresh += new EventHandler<RefreshEventArgs>(TariffList_OnRefresh);
            TariffListPageSize = 15;
            dgDataPager.PageSize = TariffListPageSize;
            dgDataPager.Source = TariffList;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillTariffComboBox();
        }
        #endregion

        void TariffList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetSpecializationsByTariffId(this.lTariffId);
        }

        #region Button Click Event
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void chkApplyToSubSp_Click(object sender, RoutedEventArgs e)
        {

        }

        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj, String TextBoxName)
        where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    if (Child is TextBox)
                    {
                        if (((TextBox)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else if (Child is DataGrid)
                    {
                        if (((DataGrid)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else
                    {
                        ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);
                        if (ChildOfChild != null)
                        {
                            return ChildOfChild;
                        }
                    }
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }

        private void chkSubSpecialization_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedSubSpecializationList.SelectedItems != null)
            {
                element = dgSelectedSubSpecializationList.Columns.Last().GetCellContent(dgSelectedSubSpecializationList.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtDoctorShare = FindVisualChild<TextBox>(row, "txtRate");

                List<clsSubSpecializationVO> CancelSubSp = new List<clsSubSpecializationVO>();
                CancelSubSp = ((List<clsSubSpecializationVO>)dgSelectedSubSpecializationList.ItemsSource).ToList();

                var item = from r in CancelSubSp
                           where r.Status == true
                           select r;

                if (item != null && item.ToList().Count > 0)
                {
                    cmdAdd.IsEnabled = true;
                }
                else
                {
                    cmdAdd.IsEnabled = false;
                }
                if (((CheckBox)sender).IsChecked == true)
                {
                    TxtDoctorShare.IsReadOnly = false;
                    clsSubSpecializationVO Obj = new clsSubSpecializationVO();
                    Obj.SpecializationId = lSelectedSpecializationId;
                    Obj.SubSpecializationId = ((clsSubSpecializationVO)dgSelectedSubSpecializationList.SelectedItem).SubSpecializationId;
                    Obj.SharePercentage = 0;
                    SelectedSubSpecializationList.Add(Obj);
                }
                else if (chkApplyToSubSp.IsChecked == true && txtAmountToAllSubSp.Text != null)
                {
                    TxtDoctorShare.Text = "0.0";
                    TxtDoctorShare.IsReadOnly = true;
                    clsSubSpecializationVO obj = (clsSubSpecializationVO)dgSelectedSubSpecializationList.SelectedItem;
                    obj = SelectedSubSpecializationList.Where(z => z.SpecializationId == obj.SpecializationId && z.SubSpecializationId == obj.SubSpecializationId).FirstOrDefault();
                    SelectedSubSpecializationList.Remove(obj);
                }
                else
                {
                    TxtDoctorShare.Text = "0.0";
                    TxtDoctorShare.IsReadOnly = true;
                    clsSubSpecializationVO obj = (clsSubSpecializationVO)dgSelectedSubSpecializationList.SelectedItem;
                    obj = SelectedSubSpecializationList.Where(z => z.SpecializationId == obj.SpecializationId && z.SubSpecializationId == obj.SubSpecializationId).FirstOrDefault();
                    SelectedSubSpecializationList.Remove(obj);
                }
            }
        }

        private void rdbRateInPercent_Click(object sender, RoutedEventArgs e)
        {
            if (rdbRateInPercent.IsChecked == true)
            {
                txtAmountToAllSubSp.IsEnabled = true;
                txtAmountToAllSubSp.Focus();
                lblRate.Name = "Rate in %";
                dgSelectedSubSpecializationList.Columns[2].Header = "Rate in %";
            }
        }

        private void rdbRateInAmount_Click(object sender, RoutedEventArgs e)
        {
            if (rdbRateInAmount.IsChecked == true)
            {
                txtAmountToAllSubSp.IsEnabled = true;
                txtAmountToAllSubSp.Focus();
                lblRate.Name = "Rate";
                dgSelectedSubSpecializationList.Columns[2].Header = "Rate";
            }
        }

        #endregion

        #region Other Control Events(SelectionChanged,TextChanged,)
        private void cmbTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbTariff.SelectedItem as MasterListItem).ID != 0)
            {
                dgDataPager.PageIndex = 0;
                this.lTariffId = (cmbTariff.SelectedItem as MasterListItem).ID;
                GetSpecializationsByTariffId((cmbTariff.SelectedItem as MasterListItem).ID);
            }
        }

        private void txtAmountToAllSubSp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmountToAllSubSp.Text))
                chkApplyToSubSp.IsEnabled = true;
            else
                chkApplyToSubSp.IsEnabled = false;
        }

        private void txtRate_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dgSpecializationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSpecializationList.SelectedItem != null)
            {
                lSelectedSpecializationId = (dgSpecializationList.SelectedItem as clsTariffMasterBizActionVO).GroupID;
                FillSubSpecialization(Convert.ToString((dgSpecializationList.SelectedItem as clsTariffMasterBizActionVO).GroupID));
            }
        }

        private void dgSelectedSubSpecializationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion

        #region Private Methods
        private void FillTariffComboBox()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TariffMaster;
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
                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetSpecializationsByTariffId(long TariffId)
        {
            clsGetSpecializationsByTariffIdBizActionVO BizAction = new clsGetSpecializationsByTariffIdBizActionVO();
            BizAction.TariffID = TariffId;

            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = TariffList.PageSize;
            BizAction.StartRowIndex = TariffList.PageIndex * TariffList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    TariffList.Clear();
                    TariffList.TotalItemCount = (int)((clsGetSpecializationsByTariffIdBizActionVO)e.Result).TotalRows;
                    clsGetSpecializationsByTariffIdBizActionVO result = e.Result as clsGetSpecializationsByTariffIdBizActionVO;
                    if (result.TariffList != null && result.TariffList.Count > 0)
                    {
                        foreach (var item in result.TariffList)
                        {
                            TariffList.Add(item);
                        }
                        dgSpecializationList.ItemsSource = null;
                        dgSpecializationList.ItemsSource = TariffList;
                        dgSpecializationList.SelectedIndex = -1;
                        dgDataPager.Source = null;
                        dgDataPager.PageSize = TariffList.PageSize;
                        dgDataPager.Source = TariffList;
                        dgSpecializationList.UpdateLayout();
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSubSpecialization(string fkSpecializationID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            if (fkSpecializationID != null)
            {
                BizAction.Parent = new KeyValue { Key = fkSpecializationID, Value = "fkSpecializationID" };
            }
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    List<clsSubSpecializationVO> SusSpList = new List<clsSubSpecializationVO>();

                    foreach (var item in objList)
                    {
                        clsSubSpecializationVO obj = new clsSubSpecializationVO();
                        obj.SubSpecializationId = item.ID;
                        obj.SubSpecializationName = item.Description;
                        SusSpList.Add(obj);
                    }
                    
                    dgSelectedSubSpecializationList.ItemsSource = null;
                    dgSelectedSubSpecializationList.ItemsSource = SusSpList;
                    dgSelectedSubSpecializationList.UpdateLayout();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        #endregion



    }
}

