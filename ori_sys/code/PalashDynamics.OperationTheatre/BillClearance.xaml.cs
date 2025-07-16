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
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using System.Windows.Browser;

namespace PalashDynamics.OperationTheatre
{
    public partial class BillClearance : ChildWindow
    {

        WaitIndicator indicator = new WaitIndicator();
        public PagedSortableCollectionView<clsBillVO> DataList { get; private set; }

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

        public PagedSortableCollectionView<clsBillVO> BillDataList { get; private set; }

        public int BillDataListPageSize
        {
            get
            {
                return BillDataList.PageSize;
            }
            set
            {
                if (value == BillDataList.PageSize) return;
                BillDataList.PageSize = value;
            }
        }

        public List<clsBillVO> ClearanceList = new List<clsBillVO>();

        public event RoutedEventHandler OnCancelButton_Click;
        public event RoutedEventHandler OnSaveButton_Click;
        public long ScheduleID;
        public long ScheduleUnitID;
        public string MRNO;
        public long BillClearanceID;
        public long BillClearanceUnitID;
        public bool BillClearanceIsFreezed;


        private ObservableCollection<clsBillVO> _BillSelected;
        public ObservableCollection<clsBillVO> SelectedBill { get { return _BillSelected; } }

        private ObservableCollection<clsBillVO> _SelectedBillList;
        public ObservableCollection<clsBillVO> SelectedBillList { get { return _SelectedBillList; } }

        public BillClearance()
        {
            InitializeComponent();

            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;



            BillDataList = new PagedSortableCollectionView<clsBillVO>();
            BillDataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            BillDataListPageSize = 15;
            dgDataPager.PageSize = BillDataListPageSize;
            dgDataPager.Source = BillDataList;

        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (BillClearanceIsFreezed == true)
            {
                OkSave.IsEnabled = false;
                chkFreezeStatus.IsEnabled = false;
                chkFreezeStatus.IsChecked = true;
            }
            FillBillClearanceList();
            FillBillTypeClPhList();
        }


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //FillBillClearanceList();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = false;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            indicator.Show();
            // if (SelectedBillList != null && SelectedBillList.Count > 0)
            if (BillDataList != null && BillDataList.Count > 0 || ClearanceList != null && ClearanceList.Count > 0)
            {
            //    OkSave.IsEnabled = false;
            //}
             //   if (ClearanceList != null && ClearanceList.Count > 0)
           // {
                clsGetBillClearanceBizActionVO BizAction = new clsGetBillClearanceBizActionVO();
                BizAction.List = new List<clsBillVO>();
                if (ClearanceList != null)
                {
                    foreach (var item in ClearanceList)
                    {
                        if (item.Status == false)
                        {
                            BillDataList.Add(item);
                        }
                    }
                }

                BizAction.List = BillDataList.ToList();
                BizAction.IsFreeze = chkFreezeStatus.IsChecked;
                BizAction.ScheduleID = ScheduleID;
                BizAction.ScheduleUnitID = ScheduleUnitID;
                BizAction.IsSaveBillClearance = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetBillClearanceBizActionVO obj = arg.Result as clsGetBillClearanceBizActionVO;
                        if (obj.SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill Clearance Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();

                            this.DialogResult = true;
                            if (OnSaveButton_Click != null)
                                OnSaveButton_Click(this, new RoutedEventArgs());

                        }
                        if (obj.SuccessStatus == -1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                    }
                    indicator.Close();
                };


                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }

            else
            {
                string msgText = "";
                msgText = "Bill not Selected";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
                indicator.Close();
            }

        }

        private void AddBill_Click(object sender, RoutedEventArgs e)
        {
            SelectBill(sender, true);
        }

        private void SelectBill(object sender, bool EventBy)
        {
            if (dgBillList.SelectedItem != null)
            {
                if (_BillSelected == null)
                    _BillSelected = new ObservableCollection<clsBillVO>();

                CheckBox chk = null;

                if (EventBy == false)
                {
                    chk = (CheckBox)dgBillList.Columns[0].GetCellContent(dgBillList.SelectedItem);
                    if (chk.IsChecked == false)
                        chk.IsChecked = true;
                    else if (chk.IsChecked == true)
                        chk.IsChecked = false;
                }
                else
                    chk = (CheckBox)sender;


                if (chk.IsChecked == true)
                    _BillSelected.Add((clsBillVO)dgBillList.SelectedItem);
                else
                    _BillSelected.Remove((clsBillVO)dgBillList.SelectedItem);

                if (_SelectedBillList == null)
                    _SelectedBillList = new ObservableCollection<clsBillVO>();

                clsBillVO objItem = new clsBillVO();

                if (dgBillList.SelectedItem != null)
                {
                    objItem.ID = ((clsBillVO)(dgBillList.SelectedItem)).ID;
                    objItem.UnitID = ((clsBillVO)(dgBillList.SelectedItem)).UnitID;
                    objItem.FirstName = ((clsBillVO)(dgBillList.SelectedItem)).FirstName;
                    objItem.MiddleName = ((clsBillVO)(dgBillList.SelectedItem)).MiddleName;
                    objItem.LastName = ((clsBillVO)(dgBillList.SelectedItem)).LastName;
                    objItem.Date = ((clsBillVO)(dgBillList.SelectedItem)).Date;
                    objItem.BillNo = ((clsBillVO)(dgBillList.SelectedItem)).BillNo;
                    objItem.MRNO = ((clsBillVO)(dgBillList.SelectedItem)).MRNO;
                    objItem.Opd_Ipd_External_No = ((clsBillVO)(dgBillList.SelectedItem)).Opd_Ipd_External_No;
                    objItem.TotalBillAmount = ((clsBillVO)(dgBillList.SelectedItem)).TotalBillAmount;
                    objItem.TotalConcessionAmount = ((clsBillVO)(dgBillList.SelectedItem)).TotalConcessionAmount;
                    objItem.NetBillAmount = ((clsBillVO)(dgBillList.SelectedItem)).NetBillAmount;
                    objItem.PaidAmount = ((clsBillVO)(dgBillList.SelectedItem)).PaidAmountSelf;
                    objItem.BalanceAmountSelf = ((clsBillVO)(dgBillList.SelectedItem)).BalanceAmountSelf;
                    objItem.PatientAdvance = ((clsBillVO)(dgBillList.SelectedItem)).PatientAdvance;
                    objItem.PackageAdvance = ((clsBillVO)(dgBillList.SelectedItem)).PackageAdvance;
                    objItem.BalanceAdvance = ((clsBillVO)(dgBillList.SelectedItem)).BalanceAdvance;
                    objItem.PacBilledCount = ((clsBillVO)(dgBillList.SelectedItem)).PacBilledCount;
                    objItem.SemenfreezingCount = ((clsBillVO)(dgBillList.SelectedItem)).SemenfreezingCount;

                }

                var item2 = from r in _SelectedBillList
                            where r.ID == ((clsBillVO)(dgBillList.SelectedItem)).ID
                            select r;


                if (item2.ToList().Count == 0)
                {
                    if (chk.IsChecked == true)
                        _SelectedBillList.Add(objItem);
                }
                else if (item2.ToList().Count > 0)
                {
                    foreach (var item in _SelectedBillList)
                    {
                        if (item.ID == objItem.ID)
                        {
                            _SelectedBillList.Remove(item);
                            BillDataList.Remove(item);
                            chk.IsChecked = false;
                            break;
                        }
                    }
                }

                foreach (var item in SelectedBillList)
                {
                    if (item.ID == objItem.ID)
                    {
                        BillDataList.Add(item);
                        break;
                    }

                }

                dgSelectedBill.ItemsSource = null;
                dgSelectedBill.ItemsSource = BillDataList;

            }
        }


        private void RemoveBill_Click(object sender, RoutedEventArgs e)
        {
            SelectBillClearance(sender, true);
        }

        private void SelectBillClearance(object sender, bool EventBy)
        {

            if (dgSelectedBill.ItemsSource == null)
                dgSelectedBill.ItemsSource = this.BillDataList;

            CheckBox chk = null;

            if (EventBy == false)
            {
                chk = (CheckBox)dgSelectedBill.Columns[0].GetCellContent(dgSelectedBill.SelectedItem);
                if (chk.IsChecked == false)
                    chk.IsChecked = true;
                else if (chk.IsChecked == true)
                    chk.IsChecked = false;
            }
            else
                chk = (CheckBox)sender;


            if (chk.IsChecked == true)
            {
                this.BillDataList.Add((clsBillVO)dgSelectedBill.SelectedItem);
                foreach (var BillClearance in BillDataList.Where(x => x.ID == ((clsBillVO)dgSelectedBill.SelectedItem).ID))
                {
                    BillClearance.Status = true;
                }
            }
            else
            {
                long ID = ((clsBillVO)dgSelectedBill.SelectedItem).ID;
                this.BillDataList.Remove((clsBillVO)dgSelectedBill.SelectedItem);
                foreach (var BillClearance in BillDataList.Where(x => x.ID == ID))
                {
                    BillClearance.Status = false;
                }

                foreach (var BillStatus in ClearanceList.Where(x => x.ID == ID))
                {
                    BillStatus.Status = false;
                }

                foreach (var item in DataList)
                {
                    if (item.ID == ID)
                    {                                         
                        _SelectedBillList.Remove(item);
                        item.Status = false;
                        break;
                    }
                }

            }

        }

        private void dgBillList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                // SelectBatch(sender, false);
            }
        }

        private void FillBillTypeClPhList()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "--All--"));
            objList.Add(new MasterListItem(1, "Clinical Bill"));
            objList.Add(new MasterListItem(2, "Pharmacy Bill"));

            cmbBillTypeClPh.ItemsSource = null;
            cmbBillTypeClPh.ItemsSource = objList;
            cmbBillTypeClPh.SelectedItem = objList[0];
        }

        private void txtBillNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Serach_Bill();
            }
        }

        private void ItemSearchButton_Click(object sender, RoutedEventArgs e)
        {
            Serach_Bill();
        }

        public void Serach_Bill()
        {
            bool res = true;
            ToolTip ToolTipControl = new ToolTip();


            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {

                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {

                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
                    string strMsg = "From Date should be less than To Date";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                    dtpFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Please Select To Date");
                dtpToDate.RaiseValidationError();
                string strMsg = "Please Select To Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate == null)
            {
                dtpFromDate.SetValidation("Plase Select From Date");
                dtpFromDate.RaiseValidationError();

                string strMsg = "Plase Select From Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (res)
            {
                dgDataPager.PageIndex = 0;
                FillBillSearchList();
            }
        }


        private void FillBillSearchList()
        {

            indicator.Show();

            clsGetBillClearanceBizActionVO BizAction = new clsGetBillClearanceBizActionVO();

            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
            if (!string.IsNullOrEmpty(txtBillNO.Text.Trim()))
            {
                BizAction.BillNO = txtBillNO.Text;
            }
            BizAction.MRNO = MRNO;
            if (cmbBillTypeClPh.SelectedItem != null)
            {
                BizAction.BillType = (BillTypes)((MasterListItem)cmbBillTypeClPh.SelectedItem).ID;
            }

            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    clsGetBillClearanceBizActionVO result = e.Result as clsGetBillClearanceBizActionVO;

                    DataList.Clear();
                    DataList.TotalItemCount = result.TotalRows;
                    if (result.List != null)
                    {

                        foreach (var item in result.List)
                        {
                            if (ClearanceList != null)
                            {

                                var List = (from a in ClearanceList
                                            where a.ID.ToString() == item.ID.ToString()
                                            select a).SingleOrDefault();

                                if (List == null)
                                {
                                    DataList.Add(item);
                                    txtBalanceAdvance.Text = Convert.ToString(item.BalanceAdvance);
                                }

                            }
                            else
                            {
                                DataList.Add(item);
                            }
                        }
                    }
                    dgBillList.ItemsSource = null;
                    dgBillList.ItemsSource = DataList;
                    dgBillList.SelectedIndex = -1;

                    dgDataPager.Source = null;
                    dgDataPager.PageSize = BizAction.MaximumRows;
                    dgDataPager.Source = DataList;
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }





        private void FillBillClearanceList()
        {

            indicator.Show();

            clsGetBillClearanceBizActionVO BizAction = new clsGetBillClearanceBizActionVO();
            BizAction.BillClearanceID = BillClearanceID;
            BizAction.BillClearanceUnitID = BillClearanceUnitID;
            BizAction.MRNO = MRNO;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.IsBillClearanceList = true;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = BillDataList.PageIndex * BillDataList.PageSize;
            BizAction.MaximumRows = BillDataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    clsGetBillClearanceBizActionVO result = e.Result as clsGetBillClearanceBizActionVO;


                    BillDataList.Clear();
                    BillDataList.TotalItemCount = result.TotalRows;
                    if (result.List != null)
                    {

                        foreach (var item in result.List)
                        {
                            item.Status = true;
                            BillDataList.Add(item);
                            ClearanceList.Add(item);
                        }
                    }
                    dgSelectedBill.ItemsSource = null;
                    dgSelectedBill.ItemsSource = BillDataList;
                    dgSelectedBill.SelectedIndex = -1;

                    SectedBillDataPager.Source = null;
                    SectedBillDataPager.PageSize = BizAction.MaximumRows;
                    SectedBillDataPager.Source = BillDataList;
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }






        private void dgBillList_GotFocus(object sender, RoutedEventArgs e)
        {
            //gbBillList.Height = 250;
            //gbBillList.Width = 850;
            //gbBillList.Background = new SolidColorBrush(Colors.LightGray);
        }

        private void dgBillList_LostFocus(object sender, RoutedEventArgs e)
        {
            //gbBillList.Height = 250;
            //gbBillList.Width = 560;
            //gbBillList.Background = new SolidColorBrush(Colors.White);
        }

        private void dgSelectedBill_GotFocus(object sender, RoutedEventArgs e)
        {
            //gbSelectedBill.Height = 250;
            //gbSelectedBill.Width = 850;           
            //gbSelectedBill.Background = new SolidColorBrush(Colors.LightGray);
        }

        private void dgSelectedBill_LostFocus(object sender, RoutedEventArgs e)
        {
            //gbSelectedBill.Height = 250;
            //gbSelectedBill.Width = 560;        
            //gbSelectedBill.Background = new SolidColorBrush(Colors.White);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

            if (dgSelectedBill.SelectedItem != null)
            {
                long BillId =  ((clsBillVO)dgSelectedBill.SelectedItem).ID;
                long UnitID = ((clsBillVO)dgSelectedBill.SelectedItem).UnitID ;
                string URL = "../Reports/OperationTheatre/ProceduralClearance.aspx?OTBillID=" + BillId + "&UnitID=" + UnitID ;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
    }
}

