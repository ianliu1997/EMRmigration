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
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master.CompanyPayment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Animations;
using PalashDynamics.Administration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Administration;
using System.Text;
using PalashDynamics.ValueObjects.Master.DoctorPayment;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.Windows.Data;

namespace PalashDynamics.Administration
{
    public partial class ChildDoctorPayment : ChildWindow
    {
        public PagedSortableCollectionView<clsDoctorPaymentVO> DataList { get; private set; }

        public List<clsDoctorPaymentVO> DataList1 { get; private set; }
        ////public List<clsDoctorPaymentVO> DeepCopyDataList { get; private set; }
        public List<clsDoctorPaymentVO> DeepCopyDataList = new List<clsDoctorPaymentVO>();
        public long DocID;
        public string BillNumber;
        public long Unitid;
        public long TariffServiceid;
        public long Serviceid;
        public double docrate = 0;
        double docsharepercent;
        double docshareamt;
        string title = "";


        public PagedCollectionView pcv = null;

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

        public ChildDoctorPayment(long DoctorID, long TariffServiceID, string BillNo, DateTime BillDate, long UnitID, String Patientname)
        {

            DocID = DoctorID;
            BillNumber = BillNo;
            Unitid = UnitID;

            TariffServiceid = TariffServiceID;
            FillDoctorPayment(DocID, BillNumber, Unitid, TariffServiceid);


            title = "Patient: " + Patientname;



            ServiceContent.Content = title;
            title = "";

            title = "Bill No: " + BillNo;
            ServiceContent3.Text = title;

            title = "Bill Date: " + BillDate.ToShortDateString();
            ServiceContent2.Text = title;

            dgDoctorPaymentList.UpdateLayout();
            dgDoctorPaymentList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgDoctorPaymentList_CellEditEnded);
            dgDoctorPaymentList.CellEditEnding += new EventHandler<DataGridCellEditEndingEventArgs>(dgDoctorPaymentList_CellEditEnding);



        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        void FillDoctorPayment(long DoctorID, string BillNo, long UnitID, long TariffServiceid)
        {

            clsGetDoctorPaymentChildBizActionVO BizActionVO = new clsGetDoctorPaymentChildBizActionVO();
            DataList = new PagedSortableCollectionView<clsDoctorPaymentVO>();
            DataListPageSize = 10;
            DataList1 = new List<clsDoctorPaymentVO>();
            InitializeComponent();
            BizActionVO.DoctorInfo = new clsDoctorPaymentVO();

            BizActionVO.DoctorId = DoctorID;
            BizActionVO.DoctorInfo.BillNo = BillNo;
            BizActionVO.UnitID = UnitID;
            BizActionVO.DoctorInfo.TariffServiceID = TariffServiceid;
            BizActionVO.IsPagingEnabled = true;
            BizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizActionVO.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    clsGetDoctorPaymentChildBizActionVO result = args.Result as clsGetDoctorPaymentChildBizActionVO;
                    if (result.DoctorInfoList != null)
                    {
                        DataList1.Clear();
                        DataList1 = result.DoctorInfoList;

                        if (DataList1.Count > 0)
                        {
                            if (DeepCopyDataList != null)
                            {
                                foreach (var item in DeepCopyDataList)
                                {
                                    foreach (var item1 in DataList1)
                                    {
                                        if (item1.ServiceID == item.ServiceID && item1.TariffServiceID == item.TariffServiceID && item.ID == item.ID)
                                        {
                                            item1.DoctorShareAmount = item.DoctorShareAmount;
                                            item1.IsPaymentChanged = item.IsPaymentChanged;
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var item in FinalServiceListForDoctorShareExistingList)
                        {
                            foreach (var item1 in DataList1)
                            {
                                if (item.TariffServiceID == item1.TariffServiceID)
                                {
                                    item1.DoctorSharePercentage = item.DoctorSharePercentage;
                                    item1.DoctorShareAmount = item.DoctorShareAmount;
                                    if (item1.DoctorShareAmount == 0)
                                    {
                                        item1.IsEnable = true;
                                    }
                                    else
                                    {
                                        if (item.IsPaymentChanged == true)
                                        {
                                            item.IsEnable = false;
                                        }
                                        else
                                        {
                                            item.IsEnable = true;
                                        }
                                    }
                                }
                            }
                        }
                        //pcv = new PagedCollectionView(DataList1);

                        if (DataList1 != null)
                        {

                            foreach (var item in DataList1)
                            {
                                if (item.DoctorShareAmount == 0)
                                {
                                    item.IsEnable = false;
                                }
                                else
                                {
                                    if (item.IsPaymentChanged == true)
                                    {
                                        item.IsEnable = false;
                                    }
                                    else
                                    {
                                        item.IsEnable = true;
                                    }
                                }
                            }
                            dgDoctorPaymentList.ItemsSource = null;

                            dgDoctorPaymentList.ItemsSource = DataList1;
                            dgDoctorPaymentList.UpdateLayout();
                        }

                    }

                }


            };

            client.ProcessAsync(BizActionVO, new clsUserVO());
            client.CloseAsync();
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;


            string msgTitle = "Palash";
            string msgText = "Are you sure you want to save Doctor Share Details ?";

            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

            msgW.Show();

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {

            if (result == MessageBoxResult.Yes)
            {

                Save();


            }
        }
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButtonClick;

        private void Save()
        {

            clsUpdateDoctorShareBizActionVO BizActionVO = new clsUpdateDoctorShareBizActionVO();
            
            foreach (var item in DataList1)
            {
                item.TariffServiceID = TariffServiceid;
            }
            BizActionVO.objDoctorShareList = DataList1;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Share Details Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();

                }
            };
            client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();



            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());



        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonClick != null)
            {
                //OnCancelButtonClick((this.DataContext), e);
                OnCancelButtonClick(this, new RoutedEventArgs());
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            }
            this.DialogResult = false;
        }

        private void txtDoctorShareAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            //double docshareamt=0;


        }

        TextBox tb1 = new TextBox();
        TextBox tb2 = new TextBox();
        TextBox txtdocpercent = new TextBox();
        TextBox txtdocamt = new TextBox();
        TextBox txtdocrate = new TextBox();
        TextBox txtdocConcessionAmount = new TextBox();
        private void txtDoctorSharePercentage_TextChanged(object sender, RoutedEventArgs e)
        {
            tb1 = (TextBox)sender;


            var selectedrow = dgDoctorPaymentList.SelectedItem as clsDoctorPaymentVO;

            if (selectedrow != null)
            {
                txtdocpercent = dgDoctorPaymentList.Columns[5].GetCellContent(selectedrow) as TextBox;



                if (txtdocpercent.Text != "")
                {

                    docsharepercent = Convert.ToDouble(txtdocpercent.Text);
                    // txtdocamt = FindVisualChild<TextBox>(row, "txtDoctorShareAmount");
                    txtdocamt = dgDoctorPaymentList.Columns[6].GetCellContent(selectedrow) as TextBox;
                    txtdocrate = dgDoctorPaymentList.Columns[3].GetCellContent(selectedrow) as TextBox;
                    txtdocConcessionAmount = dgDoctorPaymentList.Columns[4].GetCellContent(selectedrow) as TextBox;
                    docrate = Convert.ToDouble(txtdocrate.Text);
                    if (docsharepercent >= 0)
                    {
                        //docshareamt = Math.Round((docrate * docsharepercent) / 100, 2);

                        docshareamt = (Math.Round((docrate * docsharepercent) / 100, 2)) - (Convert.ToDouble(txtdocConcessionAmount.Text) / 2);
                        if (docshareamt < 0)
                        {
                            docshareamt = 0;
                        }

                        tb2.Text = docshareamt.ToString();
                        foreach (var item in DataList1)
                        {
                            if (item.ServiceID == selectedrow.ServiceID)
                            {
                                item.DoctorShareAmount = docshareamt;
                                item.DoctorSharePercentage = docsharepercent;
                                if (docshareamt == 0)
                                {
                                    item.IsEnable = true;
                                }
                                else
                                {
                                    item.IsEnable = false;
                                }
                            }

                        }

                        if (docshareamt == 0 && Convert.ToDouble(txtdocConcessionAmount.Text) > 0)
                        {
                            txtdocamt.IsReadOnly = false;
                            txtdocamt.IsEnabled = true;
                        }
                        else
                        {
                            txtdocamt.IsReadOnly = true;
                            txtdocamt.IsEnabled = true;
                        }

                        txtdocamt.Text = Convert.ToString(docshareamt);
                        dgDoctorPaymentList.ItemsSource = null;

                        dgDoctorPaymentList.ItemsSource = DataList1;
                        dgDoctorPaymentList.UpdateLayout();



                    }



                }
            }


            //element = dgDoctorPaymentList.Columns.Last().GetCellContent(dgDoctorPaymentList.SelectedItem);
            //row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
            //TxtDoctorShare = FindVisualChild<TextBox>(row, "txtDoctorSharePercentage");
            ////TxtDoctorShare.IsEnabled = false;

            //double ServiceRate = ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).Rate;
            //double SharePercentage = ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).DoctorSharePercentage;
            //double ShareAmount = ((ServiceRate * SharePercentage) / 100);

            //if (FinalServiceListForDoctorShare.Count == 0)
            //{
            //    //  ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //    FinalServiceListForDoctorShare.Add((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem);
            //    TxtDoctorShare.Text = Convert.ToString(ShareAmount);
            //}
            //else
            //{
            //    //FinalServiceListForDoctorShare.Add((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem);
            //    var Item = from r in FinalServiceListForDoctorShare
            //               where r.ServiceID == ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).ServiceID
            //               select r;
            //    if (Item.ToList().Count > 0)
            //    {
            //        foreach (var item in FinalServiceListForDoctorShare)
            //        {

            //            if (item.ServiceID == ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).ServiceID)
            //            {
            //                item.DoctorSharePercentage = ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).DoctorSharePercentage;
            //                TxtDoctorShare.Text = Convert.ToString(item.DoctorShareAmount);
            //            }

            //        }
            //    }
            //    else
            //    {
            //        FinalServiceListForDoctorShare.Add((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem);
            //        TxtDoctorShare.Text = Convert.ToString(ShareAmount);
            //    }
            //}




            dgDoctorPaymentList.UpdateLayout();
            dgDoctorPaymentList.Focus();



        }



        ////private void txtDoctorShareAmount_SelectionChanged(object sender, RoutedEventArgs e)
        ////{
        ////    tb2 = (TextBox)sender;

        ////    var selectedrow = dgDoctorPaymentList.SelectedItem as clsDoctorPaymentVO;

        ////    if (selectedrow != null)
        ////    {
        ////        txtdocamt = dgDoctorPaymentList.Columns[5].GetCellContent(selectedrow) as TextBox;

        ////        TextBox t1 = new TextBox();
        ////        t1 = dgDoctorPaymentList.Columns[2].GetCellContent(selectedrow) as TextBox;


        ////        if (txtdocamt.Text != "")
        ////        {

        ////            docshareamt = Convert.ToDouble(txtdocamt.Text);

        ////            if (docshareamt >= 0)
        ////            {
        ////                docsharepercent = Math.Round((docshareamt * 100) / docrate, 2);

        ////                tb1.Text = docsharepercent.ToString();
        ////                foreach (var item in DataList1)
        ////                {
        ////                    if (item.ServiceID == selectedrow.ServiceID)
        ////                    {
        ////                        item.DoctorShareAmount = docshareamt;
        ////                        item.DoctorSharePercentage = docsharepercent;
        ////                    }

        ////                }
        ////                dgDoctorPaymentList.ItemsSource = null;

        ////                dgDoctorPaymentList.ItemsSource = DataList1;
        ////                dgDoctorPaymentList.UpdateLayout();

        ////            }
        ////        }
        ////    }
        ////}

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

        FrameworkElement element;
        DataGridRow row;
        TextBox TxtDoctorShare;
        public List<clsDoctorPaymentVO> FinalServiceListForDoctorShareExistingList { get; set; }
        List<clsDoctorPaymentVO> FinalServiceListForDoctorShare = new List<clsDoctorPaymentVO>();
        void dgDoctorPaymentList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            try
            {
                element = dgDoctorPaymentList.Columns.Last().GetCellContent(dgDoctorPaymentList.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtDoctorShare = FindVisualChild<TextBox>(row, "txtDoctorShareAmount");
                //TxtDoctorShare.IsEnabled = false;

                double ServiceRate = ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).Rate;
                double SharePercentage = ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).DoctorSharePercentage;
                double ShareAmount = ((ServiceRate * SharePercentage) / 100);

                if (FinalServiceListForDoctorShare.Count == 0)
                {
                    //  ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    FinalServiceListForDoctorShare.Add((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem);
                    TxtDoctorShare.Text = Convert.ToString(ShareAmount);
                }
                else
                {
                    //FinalServiceListForDoctorShare.Add((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem);
                    var Item = from r in FinalServiceListForDoctorShare
                               where r.ServiceID == ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).ServiceID
                               select r;
                    if (Item.ToList().Count > 0)
                    {
                        foreach (var item in FinalServiceListForDoctorShare)
                        {

                            if (item.ServiceID == ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).ServiceID)
                            {
                                item.DoctorSharePercentage = ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).DoctorSharePercentage;
                                TxtDoctorShare.Text = Convert.ToString(ShareAmount);
                            }

                        }
                    }
                    else
                    {
                        FinalServiceListForDoctorShare.Add((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem);
                        TxtDoctorShare.Text = Convert.ToString(ShareAmount);
                    }
                }

                FinalServiceListForDoctorShareExistingList = FinalServiceListForDoctorShare.DeepCopy();

            }
            catch (Exception ex)
            {

            }
            dgDoctorPaymentList.UpdateLayout();
            dgDoctorPaymentList.Focus();

        }
        TextBox txtdocShareAmount = new TextBox();
        private void txtDoctorShareAmount_SelectionChanged(object sender, RoutedEventArgs e)
        {

            // commented on 14062018
            //MessageBoxControl.MessageBoxChildWindow msgWD =
            //            new MessageBoxControl.MessageBoxChildWindow("Palash", "Do You want to save the changes ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            //msgWD.OnMessageBoxClosed += (arg) =>
            //{
            //    if (arg == MessageBoxResult.Yes)
            //    {
            //        tb1 = (TextBox)sender;


            //        var selectedrow = dgDoctorPaymentList.SelectedItem as clsDoctorPaymentVO;

            //        if (selectedrow != null)
            //        {
            //            txtdocShareAmount = dgDoctorPaymentList.Columns[6].GetCellContent(selectedrow) as TextBox;



            //            if (txtdocShareAmount.Text != "")
            //            {

            //                docshareamt = Convert.ToDouble(txtdocShareAmount.Text);
            //                foreach (var item in DataList1)
            //                {
            //                    if (item.ServiceID == selectedrow.ServiceID)
            //                    {
            //                        //if (item.DoctorShareAmount != docshareamt)
            //                        //{
            //                        item.IsPaymentChanged = true;
            //                        DeepCopyDataList.Add(item);

            //                        //}
            //                        item.DoctorShareAmount = docshareamt;
            //                        item.DoctorSharePercentage = docsharepercent;

            //                    }

            //                }
            //            }
            //            //     DeepCopyDataList=   DeepCopyDataList.DeepCopy();
            //            dgDoctorPaymentList.ItemsSource = null;

            //            dgDoctorPaymentList.ItemsSource = DataList1;
            //            dgDoctorPaymentList.UpdateLayout();
            //        }
            //    }
            //};
            //msgWD.Show();




        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dgDoctorPaymentList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgDoctorPaymentList_CellEditEnded);
            dgDoctorPaymentList.CellEditEnding += new EventHandler<DataGridCellEditEndingEventArgs>(dgDoctorPaymentList_CellEditEnding);

        }



        private void dgDoctorPaymentList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                element = dgDoctorPaymentList.Columns.Last().GetCellContent(dgDoctorPaymentList.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtDoctorShare = FindVisualChild<TextBox>(row, "txtDoctorShareAmount");
                //TxtDoctorShare.IsEnabled = false;

                double ServiceRate = ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).Rate;
                double SharePercentage = ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).DoctorSharePercentage;
                double ShareAmount = ((ServiceRate * SharePercentage) / 100);

                if (FinalServiceListForDoctorShare.Count == 0)
                {
                    //  ((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem).UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    FinalServiceListForDoctorShare.Add((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem);
                    TxtDoctorShare.Text = Convert.ToString(ShareAmount);
                }
                else
                {
                    //FinalServiceListForDoctorShare.Add((clsDoctorShareServicesDetailsVO)dgDoctorShareList.SelectedItem);
                    var Item = from r in FinalServiceListForDoctorShare
                               where r.ServiceID == ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).ServiceID
                               select r;
                    if (Item.ToList().Count > 0)
                    {
                        foreach (var item in FinalServiceListForDoctorShare)
                        {

                            if (item.ServiceID == ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).ServiceID)
                            {
                                item.DoctorSharePercentage = ((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem).DoctorSharePercentage;
                                TxtDoctorShare.Text = Convert.ToString(ShareAmount);
                            }

                        }
                    }
                    else
                    {
                        FinalServiceListForDoctorShare.Add((clsDoctorPaymentVO)dgDoctorPaymentList.SelectedItem);
                        TxtDoctorShare.Text = Convert.ToString(ShareAmount);
                    }
                }

                FinalServiceListForDoctorShareExistingList = FinalServiceListForDoctorShare.DeepCopy();

            }
            catch (Exception ex)
            {

            }
            dgDoctorPaymentList.UpdateLayout();
            dgDoctorPaymentList.Focus();

        }
    }
}

