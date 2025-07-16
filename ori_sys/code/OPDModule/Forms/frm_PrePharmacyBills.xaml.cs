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
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using System.Windows.Browser;

namespace OPDModule.Forms
{
    public partial class frm_PrePharmacyBills : ChildWindow
    {
        public frm_PrePharmacyBills()
        {
            InitializeComponent();

            //============================================================================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 6;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;

        }

        #region Paging

        public PagedSortableCollectionView<clsBillVO> DataList { get; private set; }
        clsBillVO SelectedBill { get; set; }
        public event RoutedEventHandler OnCancelButton_Click;
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
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// Calls the function FillPharmacyBillSearchList
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillPharmacyBillSearchList();
        }

        //public string MrNo = string.Empty;
        public long PatientID = 0;
        public long PatientUnitID = 0;

        /// <summary>
        /// Function is for fetching Previous bill list.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        private void FillPharmacyBillSearchList()
        {

            clsPreviousPatientBillsBizActionVO BizAction1 = new clsPreviousPatientBillsBizActionVO();
            
            BizAction1.PatientID = PatientID;
            BizAction1.PatientUnitID = PatientUnitID;

            BizAction1.IsPagingEnabled = true;
            BizAction1.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction1.MaximumRows = DataList.PageSize;

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsPreviousPatientBillsBizActionVO result = arg.Result as clsPreviousPatientBillsBizActionVO;

                        DataList.Clear();
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.List != null)
                        {

                            foreach (var item in result.List)
                            {
                                DataList.Add(item);
                            }

                        }
                        dgBillList.ItemsSource = null;
                        dgBillList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction1.MaximumRows;
                        dgDataPager.Source = DataList;
                    }
                }
                else
                {
                    //HtmlPage.Window.Alert("Error occured while adding patient.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();
            //clsPreviousBillListBizActionVO objBizAction = new clsPreviousBillListBizActionVO();

            //objBizAction.MRNO = MrNo;
            //objBizAction.UnitID = UnitID;
            //objBizAction.IsPagingEnabled = true;
            //objBizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            //objBizAction.MaximumRows = DataList.PageSize;

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null && arg.Result != null)
            //        {
            //            clsPreviousBillListBizActionVO res = arg.Result as clsPreviousBillListBizActionVO;
            //        }
            //    };
            //Client.ProcessAsync(objBizAction, new clsUserVO());
            //Client.CloseAsync();

        }

        #endregion

        private void frm_PrePharmacyBills_Loaded(object sender, RoutedEventArgs e)
        {
            FillPharmacyBillSearchList();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            //if (OnCancelButton_Click != null)
            //    OnCancelButton_Click(this, new RoutedEventArgs());
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)dgBillList.SelectedItem;

                //  BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                if (SelectedBill.IsFreezed == true)
                {
                    PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        /// <summary>
        /// Method is for printing the Pharmacy bill.
        /// </summary>
        /// <param name="iBillId"></param>
        /// <param name="iUnitID"></param>
        private void PrintPharmacyBill(long iBillId, long iUnitID)
        {
            if (iBillId > 0)
            {
                string LoginUserName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UserName;
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/PharmacyBillCash.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&LoginUserName=" + LoginUserName;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

    }
}

