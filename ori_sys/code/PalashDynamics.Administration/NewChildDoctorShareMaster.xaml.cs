using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using System.Windows.Data;

namespace PalashDynamics.Administration
{
    public partial class NewChildDoctorShareMaster : ChildWindow
    {
        public NewChildDoctorShareMaster()
        {
            InitializeComponent();
            FrontPanelDataList = new PagedSortableCollectionView<clsDoctorShareServicesDetailsVO>();
            FrontPanelDataList.OnRefresh += new EventHandler<RefreshEventArgs>(FrontPanelDataList_OnRefresh);

            DataListPageSizeSer = 15;
            // FetchData();
        }
        public PagedSortableCollectionView<clsDoctorShareServicesDetailsVO> FrontPanelDataList { get; private set; }
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;


        public int DataListPageSizeSer
        {
            get
            {
                return FrontPanelDataList.PageSize;
            }
            set
            {
                if (value == FrontPanelDataList.PageSize) return;
                FrontPanelDataList.PageSize = value;
            }
        }

        void FrontPanelDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }
        public bool FromDoctorShareChildWindow = false;

        PagedCollectionView collection;
        PagedSortableCollectionView<clsDoctorShareServicesDetailsVO> ServiceDetailsList = new PagedSortableCollectionView<clsDoctorShareServicesDetailsVO>();
        void FetchData()
        {
            WaitIndicator Pageload = new WaitIndicator();
            Pageload.Show();
            clsGetDoctorShare1DetailsBizActionVO BizActionVO = new clsGetDoctorShare1DetailsBizActionVO();
            BizActionVO.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            {
                BizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizActionVO.UnitID = 0;
            }
            BizActionVO.FromDoctorShareChildWindow = true;
            if (txtServiceName.Text != "")
            {
                BizActionVO.ServiceName = txtServiceName.Text;
            }
            else
            {
                BizActionVO.ServiceName = null;
            }
            BizActionVO.DoctorId = DoctorShareList[0].DoctorId;
            BizActionVO.SpecID = DoctorShareList[0].SpecializationID;
            BizActionVO.SubSpecID = DoctorShareList[0].SubSpecializationId;
            BizActionVO.TariffID = DoctorShareList[0].TariffID;
            BizActionVO.ModalityID = DoctorShareList[0].ModalityID;


            BizActionVO.IsPagingEnabled = true;
            BizActionVO.StartRowIndex = FrontPanelDataList.PageIndex * FrontPanelDataList.PageSize;
            BizActionVO.MaximumRows = FrontPanelDataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {
                    clsGetDoctorShare1DetailsBizActionVO result = args.Result as clsGetDoctorShare1DetailsBizActionVO;

                    if (result.DoctorShareInfoGetList != null)
                    {

                        FrontPanelDataList.TotalItemCount = result.TotalRows;
                        FrontPanelDataList.Clear();

                        foreach (var item in result.DoctorShareInfoGetList)
                        {
                            //item.DoctorName = DoctorShareList[0].DoctorName;
                            //item.SpecializationName = DoctorShareList[0].SpecializationName;
                            //item.SubSpecializationName = DoctorShareList[0].SubSpecializationName;
                            //item.TariffName = DoctorShareList[0].TariffName;
                            //item.Modality = DoctorShareList[0].Modality;
                            FrontPanelDataList.Add(item);
                        }

                        foreach (var item in FinalServiceListForDoctorShare)
                        {
                            foreach (var item1 in FrontPanelDataList)
                            {
                                if (item.ServiceId == item1.ServiceId)
                                {
                                    item1.DoctorSharePercentage = item.DoctorSharePercentage;
                                    item1.DoctorShareAmount = item.DoctorShareAmount;
                                }
                            }
                        }

                        dgDoctorShareList.ItemsSource = null;
                        collection = new PagedCollectionView(FrontPanelDataList);
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("DoctorName"));
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("SubSpecializationName"));
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("TariffName"));
                    //    collection.GroupDescriptions.Add(new PropertyGroupDescription("Modality"));
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("Services"));
                        dgDoctorShareList.ItemsSource = collection;
                        ServiceDetailsList = FrontPanelDataList;
                        //grdDocShare.ItemsSource = null;
                        //grdDocShare.ItemsSource = FrontPanelDataList;

                        DataPagerDoc.Source = null;
                        DataPagerDoc.PageSize = BizActionVO.MaximumRows;
                        DataPagerDoc.Source = FrontPanelDataList;
                    }
                }
                Pageload.Close();
            };

            client.ProcessAsync(BizActionVO, new clsUserVO());
            client.CloseAsync();
        }

        public List<clsDoctorShareServicesDetailsVO> DoctorShareList { get; set; }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem != null && FinalServiceListForDoctorShare.Count > 0)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update Service?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
            else
            {
                
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Record.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();

            }


        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                PalashDynamics.ValueObjects.Master.DoctorMaster.clsUpdateDoctorShareServiceBizActionVO BizAction = new PalashDynamics.ValueObjects.Master.DoctorMaster.clsUpdateDoctorShareServiceBizActionVO();

                //if ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem != null)
                //{
                //    BizAction.objServiceDetail = (clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem;
                //}

                if (FinalServiceListForDoctorShare.Count > 0)
                {
                    BizAction.objServiceList = FinalServiceListForDoctorShare;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Service Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        this.DialogResult = true;
                        if (OnSaveButton_Click != null)
                            OnSaveButton_Click(this, new RoutedEventArgs());


                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Doctor Share Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dgDoctorShareList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgDoctorShareList_CellEditEnded);
            FetchData();
        }
        FrameworkElement element;
        DataGridRow row;
        TextBox TxtDoctorShare;
        List<clsDoctorShareServicesDetailsVO> FinalServiceListForDoctorShare = new List<clsDoctorShareServicesDetailsVO>();

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
        private void chkSelectShare_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {

                //element = dgDoctorShareList.Columns.Last().GetCellContent(dgDoctorShareList.SelectedItem);
                //row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                //TxtDoctorShare = FindVisualChild<TextBox>(row, "txtDoctorSharePercentage");
                //TxtDoctorShare.IsEnabled = true;
                //string Rate = ((TextBox)sender).Text;


                //double ShareAmountInfo = Convert.ToDouble(TxtDoctorShare.Text);
                //double SharePercentageInfo = Convert.ToDouble(TxtDoctorShare.Text);

                //foreach (var item in ServiceDetailsList)
                //{
                //    if (item.ServiceId == ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).ServiceId)
                //    {
                //         item.DoctorShareAmount = ShareAmountInfo;
                //        item.DoctorSharePercentage = SharePercentageInfo;
                //        FinalServiceListForDoctorShare.Add(item);
                //    }

                //}

                //dgSelectedServiceList.ItemsSource = null;
                //dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                //dgSelectedServiceList.UpdateLayout();
                //dgSelectedServiceList.Focus();


                //FinalServiceListForDoctorShare.Add((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem)

            }
            else
            {
                TxtDoctorShare.IsEnabled = false;
                //ServiceList = (PagedSortableCollectionView<clsServiceMasterVO>)dgServiceList.ItemsSource;
                //var OBJ = from r in DataList
                //          where r.ID == ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ID
                //          select r;

                //decimal PreviousRate = 0;
                //foreach (var item in OBJ)
                //{
                //    PreviousRate = item.Rate;
                //}
                //foreach (var item in _OtherServiceSelected)
                //{
                //    if (item.ID == ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ID)
                //    {
                //        item.Rate = PreviousRate;
                //    }
                //}
                //TxtDoctorShare.Text = Convert.ToString(PreviousRate);

            }
        }


        void dgDoctorShareList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            element = dgDoctorShareList.Columns.Last().GetCellContent(dgDoctorShareList.SelectedItem);
            row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
            TxtDoctorShare = FindVisualChild<TextBox>(row, "txtDoctorShareAmount");
            //TxtDoctorShare.IsEnabled = false;

            double ServiceRate = ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).ServiceRate;
            double SharePercentage = ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).DoctorSharePercentage;
            double ShareAmount = ((ServiceRate * SharePercentage) / 100);

            if (FinalServiceListForDoctorShare.Count == 0)
            {
                //  ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                FinalServiceListForDoctorShare.Add((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem);
                TxtDoctorShare.Text = Convert.ToString(ShareAmount);
            }
            else
            {
                //FinalServiceListForDoctorShare.Add((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem);
                var Item = from r in FinalServiceListForDoctorShare
                           where r.ServiceId == ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).ServiceId && r.DoctorId == ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).DoctorId
                           select r;
                if (Item.ToList().Count > 0)
                {
                    foreach (var item in FinalServiceListForDoctorShare)
                    {

                        if (item.ServiceId == ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).ServiceId)
                        {
                            item.DoctorSharePercentage = ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).DoctorSharePercentage;
                            TxtDoctorShare.Text = Convert.ToString(item.DoctorShareAmount);
                        }

                    }
                }
                else
                {
                    FinalServiceListForDoctorShare.Add((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem);
                    TxtDoctorShare.Text = Convert.ToString(ShareAmount);
                }
            }




            dgDoctorShareList.UpdateLayout();
            dgDoctorShareList.Focus();
        }

        //private void txtServiceName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    FetchData();
        //}

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }
    }
}

