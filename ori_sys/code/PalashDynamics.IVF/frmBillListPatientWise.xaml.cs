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
using PalashDynamics.ValueObjects.Billing;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using DataDrivenApplication;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Browser;

namespace OPDModule
{
    public partial class frmBillListPatientWise : ChildWindow
    {
        WaitIndicator indicator = new WaitIndicator();
        long EPatientID = 0;
        long EPatientUnitID = 0;

        #region "Paging"
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
                // RaisePropertyChanged("DataListPageSize");
            }
        }


        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillBillSearchList();

        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
        private void FillBillSearchList()
        {
            indicator.Show();

            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.PatientID = EPatientID;
            BizAction.PatientUnitID = EPatientUnitID;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = BizAction.PatientUnitID.Value;//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;
                    DataList.Clear();
                    if (result.List != null)
                    {
                        foreach (var item in result.List)
                        {
                            DataList.Add(item);
                        }
                        dgBillList.ItemsSource = null;
                        dgBillList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList;   
                    }
                }
                indicator.Close();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #endregion

        public frmBillListPatientWise( long PatientID, long PatientUnitID)
        {
            InitializeComponent();
            EPatientID = PatientID;
            EPatientUnitID = PatientUnitID;

            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
        }

          public  DateTime BillDate;
          bool IsPatientExist = false;
          public long BillID = 0, BillUnitID=0;
          public string BillNo ;
          long UnitID = 0;

        public event RoutedEventHandler OnSaveButton_Click;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            if (dgBillList.ItemsSource != null)
            {
                if (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).ID != 0)
                {
                    this.DialogResult = true;
                    if (OnSaveButton_Click != null)
                    {
                        BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                        BillDate = ((clsBillVO)dgBillList.SelectedItem).Date;
                        BillNo = ((clsBillVO)dgBillList.SelectedItem).BillNo;
                        BillUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                        OnSaveButton_Click(this, new RoutedEventArgs());
                        this.Close();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Bill .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill Details not available", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillBillSearchList();
        }
                 
        private void dgBillList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        clsBillVO SelectedBill { get; set; }
        clsPatientVO myPatient { get; set; }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                //  BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                if (SelectedBill.IsFreezed == true)
                {
                    //if(SelectedBill.VisitTypeID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyVisitTypeID)
                    if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                    {
                        PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
                    }
                    else if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                    {
                        PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
                    }
                    else
                    {
                        PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
                        PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
                    }
                }
            }
        }

        private void PrintBill(long iBillId, long iUnitID)
        {
            if (iBillId > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void PrintPharmacyBill(long iBillId, long IUnitID)
        {
            if (iBillId > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = IUnitID;
                string URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2; ;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
    }
}

