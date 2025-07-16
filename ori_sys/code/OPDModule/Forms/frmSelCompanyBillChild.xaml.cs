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
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master.CompanyPayment;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Billing;

namespace OPDModule.Forms
{
    public partial class frmSelCompanyBillChild : ChildWindow
    {
        public long CompanyID;
        public event RoutedEventHandler OnAddButton_Click;
        WaitIndicator Indicator = null;
        public long UnitID;

        private ObservableCollection<clsCompanyPaymentDetailsVO> _ItemSelected;
        public ObservableCollection<clsCompanyPaymentDetailsVO> SelectedItems { get { return _ItemSelected; } }

        private List<clsCompanyPaymentDetailsVO> lstCompanyPaymentDetails;
        List<clsCompanyPaymentDetailsVO> newlstCompanyPaymentDetails;
        private ObservableCollection<clsCompanyPaymentDetailsVO> _BillSelected;

        public frmSelCompanyBillChild()
        {
            InitializeComponent();
            lstCompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
            DataList = new PagedSortableCollectionView<clsCompanyPaymentDetailsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
          
            BillList = new ObservableCollection<clsCompanyPaymentDetailsVO>();
            CompanyDetails = new clsCompanyPaymentDetailsVO();
            Indicator = new WaitIndicator();
        }

        #region Paging

        public ObservableCollection<clsCompanyPaymentDetailsVO> BillList { get; set; }
       public clsCompanyPaymentDetailsVO CompanyDetails { get; set; }
        public PagedSortableCollectionView<clsCompanyPaymentDetailsVO> DataList { get; private set; }

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
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillData();

        }



        #endregion

        private void FillData()
        {
            WaitIndicator Indicator = new WaitIndicator();
            Indicator.Show();
            clsGetCompanyPaymentDetailsBizActionVO BizAction = new clsGetCompanyPaymentDetailsBizActionVO();
            BizAction.IsFromNewForm = true;
            if (UnitID > 0)
                BizAction.UnitID = UnitID;
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (CompanyID > 0)
            {
                BizAction.CompanyID = CompanyID.ToString();
            }
            else
            {
                BizAction.CompanyID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID.ToString();
            }

            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date.ToShortDateString();
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date.ToShortDateString();

            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    clsGetCompanyPaymentDetailsBizActionVO result = arg.Result as clsGetCompanyPaymentDetailsBizActionVO;
                    DataList.Clear();
                    DataList.TotalItemCount = result.TotalRows;
                    if (result.CompanyPaymentDetails != null)
                    {
                        foreach (var item in result.CompanyPaymentDetails)
                        {
                            item.IsEnabled = true;
                            DataList.Add(item);
                        }
                    }
                    dgCompPayList.ItemsSource = null;

                    dgCompPayList.ItemsSource = DataList;
                    dgDataPager.Source = null;
                    dgDataPager.PageSize = BizAction.MaximumRows;
                    dgDataPager.Source = DataList;

                    Indicator.Close();
                }
                else
                {
                    Indicator.Close();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
   

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
          
        }

        clsBillVO Objbill = new clsBillVO();
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
         
            if (dgCompPayList.SelectedItem != null)
            {
                if (_ItemSelected == null)
                    _ItemSelected = new ObservableCollection<clsCompanyPaymentDetailsVO>();

                CheckBox chk = (CheckBox)sender;
                if (chk.IsChecked == true)
                {
                    _ItemSelected.Add((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem);
                }
                else
                {
                    _ItemSelected.Remove((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem);
                }

                foreach (var item in _ItemSelected)
                {
                    item.IsSelected = true;
                    dgCompPayList.UpdateLayout();
                }

            }

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillData();
        }

        int ClickedFlag = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                bool isValid = true;
                if (_ItemSelected == null)
                {
                    isValid = false;
                }
                else if (_ItemSelected.Count <= 0)
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
                    string strMsg = "No Bill/s Selected for Adding";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                }
            }
            else
            {
                ClickedFlag = 0;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            _ItemSelected = null;
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillData();
        }

       
    }
}

