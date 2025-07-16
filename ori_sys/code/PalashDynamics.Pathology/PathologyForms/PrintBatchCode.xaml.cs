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
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Reflection;
using PalashDynamics.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using PalashDynamics.UserControls;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Windows.Data;


namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class PrintBatchCode : ChildWindow
    {
        public PagedSortableCollectionView<clsPathOrderBookingDetailVO> DataList { get; private set; }
        public bool IsFromAccept;
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
        public PrintBatchCode()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsPathOrderBookingDetailVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;

            //DataPager.PageSize = DataListPageSize;
            //DataPager.Source = DataList;

            //this.DataPager.DataContext = DataList;
            //dgBatchList.DataContext = DataList;
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillBatchList();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        public event RoutedEventHandler OnCancelButtonClick;
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
            //Application.Current.RootVisu al.SetValue(Control.IsEnabledProperty, true);
            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), e);
            }
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void hlkPrint_Click(object sender, RoutedEventArgs e)
        {
            //long UID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            if (IsFromAccept == false)
            {
                if (dgBatchList.SelectedItem != null)
                {
                    string b = ((clsPathOrderBookingDetailVO)dgBatchList.SelectedItem).BatchCode;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Pathology/PrintDispatchBatchReport.aspx?UnitID=" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId + "&BatchNo=" + ((clsPathOrderBookingDetailVO)dgBatchList.SelectedItem).BatchCode), "_blank");
                }
            }
            else
            {
                if (dgBatchList.SelectedItem != null)
                {
                    string b = ((clsPathOrderBookingDetailVO)dgBatchList.SelectedItem).BatchCode;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Pathology/PrintReceiveBatchCode.aspx?UnitID=" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId + "&BatchNo=" + ((clsPathOrderBookingDetailVO)dgBatchList.SelectedItem).BatchCode), "_blank");
                }
            }
        }
        //by rohini dated 4/2/16 for dispatch
        private void FillDispatch()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "IsProcessingUnit";

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    CmbDispatchTo.ItemsSource = null;
                    CmbDispatchTo.ItemsSource = objList;
                    //CmbDispatchTo.SelectedValue = objList[0];
                    foreach (var item in objList)
                    {
                        if (item.ID == 0)
                        {
                            CmbDispatchTo.SelectedItem = item;
                        }
                    }
                    FillBatchList();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

           
            dtpFrom.SelectedDate = System.DateTime.Now;
            dtpTo.SelectedDate = System.DateTime.Now;
            if (IsFromAccept == false)
            {
                CmbDispatchTo.Visibility = Visibility.Visible;
                lblDispatchTo.Visibility = Visibility.Visible;
            }
            else
            {                
                CmbDispatchTo.Visibility = Visibility.Collapsed;
                lblDispatchTo.Visibility = Visibility.Collapsed;
            }
            if (IsFromAccept == false)
            {
                FillDispatch();
            }
            else
            {
                FillBatchList();
            }
            
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FillBatchList();
        }
        private List<clsPathOrderBookingDetailVO> objOrderBooking1 = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> OrderBookingList1
        {
            get { return objOrderBooking1; }
            set { objOrderBooking1 = value; }
        }
        private void FillBatchList()
        {
            try
            {
                clsGetBatchCodeBizActionVO BizAction = new clsGetBatchCodeBizActionVO();
                // BizAction.id = ((MasterListItem)CmbStatus.SelectedItem).ID;
                if (dtpFrom.SelectedDate != null)
                    BizAction.FromDate = ((DateTime)dtpFrom.SelectedDate).Date;
                else
                    BizAction.FromDate = null;
                if (dtpTo.SelectedDate != null)
                    BizAction.ToDate = ((DateTime)dtpTo.SelectedDate).Date.AddDays(1);
                else
                    BizAction.ToDate = null;
                if (txtBatch.Text != null && txtBatch.Text != string.Empty)
                {
                    BizAction.BatchCode = Convert.ToString(txtBatch.Text.Trim());
                }
                if (IsFromAccept == false)
                {
                    if((MasterListItem)CmbDispatchTo.SelectedItem!=null)
                    {
                        if (((MasterListItem)CmbDispatchTo.SelectedItem).ID > 0)
                        {
                            BizAction.DispatchTo = ((MasterListItem)CmbDispatchTo.SelectedItem).ID;
                        }
                    }
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = 0;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                if (IsFromAccept ==true)
                {
                    BizAction.IsFromAccept = true;
                }

                //BizAction.IsPagingEnabled = true;
                //BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                //BizAction.MaximumRows = DataList.PageSize;
                // BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        OrderBookingList1 = new List<clsPathOrderBookingDetailVO>();
                        OrderBookingList1 = ((clsGetBatchCodeBizActionVO)args.Result).OrderBookingList;
                        dgBatchList.ItemsSource = null;
                        dgBatchList.ItemsSource = OrderBookingList1;
                        dgBatchList.UpdateLayout();

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                //indicator.Close();
            }
        }

        private void txtBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillBatchList();
            }
        }

        private void txtBatch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

