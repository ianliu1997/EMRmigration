using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.Reflection;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;
using CIMS;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmDoctorOrder : ChildWindow
    {
        #region Variables
        public event RoutedEventHandler OnOKClick;

        public clsPatientProcedureScheduleVO objScheduleVO = null;
        string msgText = "";
        public PagedSortableCollectionView<clsPatientProcedureScheduleVO> DataList { get; private set; }
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                DataList.PageSize = 10;
            }
        }
        #endregion

        #region Constructor
        public frmDoctorOrder()
        {
            InitializeComponent();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            DataList = new PagedSortableCollectionView<clsPatientProcedureScheduleVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 10;
            FillPriorityCombo();
            GetDoctorOrder();
        }
        #endregion
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetDoctorOrder();
        }

        private void FillPriorityCombo()
        {
            List<MasterListItem> PriorityLst = new List<MasterListItem>();
            Type PriorityType = typeof(PriorityType);
            FieldInfo[] arrPriorityType = PriorityType.GetFields(BindingFlags.Public | BindingFlags.Static);

            PriorityLst.Add(new MasterListItem(0, "--Select--"));

            foreach (var test in arrPriorityType)
            {
                PriorityType TT = (PriorityType)Enum.Parse(typeof(PriorityType), Convert.ToString(test.GetValue(null)), true);
                PriorityLst.Add(new MasterListItem((int)TT, test.GetValue(null).ToString()));
            }
            cmbPriority.ItemsSource = null;
            cmbPriority.ItemsSource = PriorityLst;
            cmbPriority.SelectedItem = PriorityLst[0];
        }

        private void GetDoctorOrder()
        {
            clsGetDoctorForDoctorTypeBizActionVO BizAction = new clsGetDoctorForDoctorTypeBizActionVO();
            BizAction.DoctorOrderList = new List<clsPatientProcedureScheduleVO>();
            BizAction.IsCalledForDoctorOrder = true;
            BizAction.FromDate = dtpFromDate.SelectedDate;
            BizAction.ToDate = dtpToDate.SelectedDate;
            if ((bool)rbtnIPD.IsChecked)
                BizAction.Opd_Ipd = 1;
            else if ((bool)rbtnOPD.IsChecked)
                BizAction.Opd_Ipd = 0;
            else
                BizAction.Opd_Ipd = 2;

            BizAction.PriorityID = ((MasterListItem)cmbPriority.SelectedItem).ID;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    DataList.Clear();
                    BizAction.DoctorOrderList = ((clsGetDoctorForDoctorTypeBizActionVO)arg.Result).DoctorOrderList;
                    DataList.TotalItemCount = ((clsGetDoctorForDoctorTypeBizActionVO)arg.Result).TotalRows;
                    foreach (var item in BizAction.DoctorOrderList)
                    {
                        DataList.Add(item);
                    }

                    dgDoctorOrderList.ItemsSource = null;
                    dgDoctorOrderList.ItemsSource = DataList;
                    dgDataPager.Source = null;
                    dgDataPager.Source = DataList;
                }
                else
                {
                    msgText = "Error occured while processing.";
                    MessageBoxChildWindow win = new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    win.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (dgDoctorOrderList.SelectedItem != null)
            {
                objScheduleVO = new clsPatientProcedureScheduleVO();
                objScheduleVO = (clsPatientProcedureScheduleVO)dgDoctorOrderList.SelectedItem;
                if (OnOKClick != null)
                {
                    OnOKClick(this, e);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            GetDoctorOrder();
        }
    }
}

