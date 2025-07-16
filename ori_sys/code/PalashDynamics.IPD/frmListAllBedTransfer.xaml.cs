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
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.ComponentModel;
using System.Windows.Browser;
using CIMS;

namespace PalashDynamics.IPD
{
    public partial class frmListAllBedTransfer : ChildWindow
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Variable Declarations
        public PagedSortableCollectionView<clsIPDBedTransferVO> DataList { get; private set; }
        long PatientID = 0;
        bool IsSelected = false;
        public int PageSizeData
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                OnPropertyChanged("PageSizeData");
            }
        }
        #endregion

        #region Constructor and Loaded
        public frmListAllBedTransfer()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsIPDBedTransferVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSizeData = 15;
            dataPager.PageSize = PageSizeData;
            dataPager.Source = DataList;
            dgTransfer.ItemsSource = DataList;

        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillBedTransferHistoryGrid();
        }

        private void frmListAllBedTransfer_Loaded(object sender, RoutedEventArgs e)
        {
            FillBedTransferHistoryGrid();
        }


        #endregion

        #region Button Click Events
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }


        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            //Added By Bhushanp 10/01/2017
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidate())
            {
                //DataList.PageIndex = 0;
                FillBedTransferHistoryGrid();
                dataPager.PageIndex = 0;
            }
        }

        private void cmdAllPrint_Click(object sender, RoutedEventArgs e)
        {
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            string FirstName = null;
            string MiddleName = null;
            string LastName = null;
            string FamilyName = null;
            string MRNo = null;
            string IPDNO = null;
            if (!String.IsNullOrEmpty(txtFirstName.Text))
                FirstName = txtFirstName.Text;
            if (!String.IsNullOrEmpty(txtMiddleName.Text))
                MiddleName = txtMiddleName.Text;
            if (!String.IsNullOrEmpty(txtLastName.Text))
                LastName = txtLastName.Text;
            if (!String.IsNullOrEmpty(txtFamilyName.Text))
                FamilyName = txtFamilyName.Text;
            if (!String.IsNullOrEmpty(txtMrno.Text))
                MRNo = txtMrno.Text;
            if (!String.IsNullOrEmpty(txtIPDNO.Text))
                IPDNO = txtIPDNO.Text;
            if (dtpFromDate.SelectedDate != null)
                FromDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
            if (dtpToDate.SelectedDate != null)
                ToDate = Convert.ToDateTime(dtpToDate.SelectedDate);
            if (dgTransfer.ItemsSource != null)
            {
                if (DataList.Count > 0)
                {
                    if (IsSelected == true)
                    {
                        string URL = "../Reports/IPD/BedTransferHistoryForSelectedPatient.aspx?PatientID=" + PatientID + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID; ;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                    else
                    {
                        string URL = "../Reports/IPD/BedTransferHistory.aspx?FirstName=" + FirstName + "&MiddleName" + MiddleName + "@LastName" + LastName + "@FamilyName" + FamilyName + "&MRNo" + MRNo + "&IPDNO" + IPDNO + "&FromDate=" + FromDate + "&ToDate=" + ToDate  + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                }
                else
                    cmdAllPrint.IsEnabled = false;
            }
        }
        #endregion

        #region Private Methods
        private void FillBedTransferHistoryGrid()
        {
            try
            {
                clsGetIPDBedTransferListBizActionVO bizActionVO = new clsGetIPDBedTransferListBizActionVO();
                bizActionVO.BedDetails = new clsIPDBedTransferVO();
                if (!String.IsNullOrEmpty(txtFirstName.Text))
                    bizActionVO.BedDetails.FirstName = txtFirstName.Text;
                else
                    bizActionVO.BedDetails.FirstName = null;
                if (!String.IsNullOrEmpty(txtMiddleName.Text))
                    bizActionVO.BedDetails.MiddleName = txtMiddleName.Text;
                else
                    bizActionVO.BedDetails.MiddleName = null;
                if (!String.IsNullOrEmpty(txtLastName.Text))
                    bizActionVO.BedDetails.LastName = txtLastName.Text;
                else
                    bizActionVO.BedDetails.LastName = null;
                if (!String.IsNullOrEmpty(txtFamilyName.Text))
                    bizActionVO.BedDetails.FamilyName = txtFamilyName.Text;
                else
                    bizActionVO.BedDetails.FamilyName = null;
                if (!String.IsNullOrEmpty(txtMrno.Text))
                {
                    bizActionVO.BedDetails.MRNo = txtMrno.Text;
                }
                if (!String.IsNullOrEmpty(txtIPDNO.Text))
                {
                    bizActionVO.BedDetails.IPDNo = txtIPDNO.Text;
                }
                if (dtpFromDate.SelectedDate != null)
                {
                    bizActionVO.BedDetails.FromDate = dtpFromDate.SelectedDate;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    bizActionVO.BedDetails.ToDate = dtpToDate.SelectedDate;
                }
                //Added By Bhushanp 30062017
                bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = DataList.PageSize;
                bizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                bizActionVO.BedList = new List<clsIPDBedTransferVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.BedList = (((clsGetIPDBedTransferListBizActionVO)args.Result).BedList);
                        if (bizActionVO.BedList.Count > 0)
                        {
                            DataList.TotalItemCount = (int)(((clsGetIPDBedTransferListBizActionVO)args.Result).TotalRows);
                            DataList.Clear();
                            foreach (clsIPDBedTransferVO item in bizActionVO.BedList)
                            {
                                DataList.Add(item);
                            }
                            dgTransfer.ItemsSource = null;
                            dgTransfer.ItemsSource = DataList;
                            dgTransfer.SelectedIndex = -1;

                            dataPager.Source = null;
                            dataPager.PageSize = Convert.ToInt32(bizActionVO.MaximumRows);
                            dataPager.Source = DataList;
                        }
                        else
                        {
                            dgTransfer.ItemsSource = null;
                            dataPager.Source = null;
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw;
            }



        }

        private bool IsValidate()
        {
            bool isValid = true;
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msg =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "from Date Should Not Be Greater Than To Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    dtpFromDate.Focus();
                    msg.Show();
                    //dtpFromDate.SetValidation("from Date Should Not Be Greater Than To Date");
                    //dtpFromDate.RaiseValidationError();
                    //dtpFromDate.Focus();
                    isValid = false;
                }
            }
            return isValid;
        }
        #endregion

        #region Selection Changed and KeyUp Events
        private void dgTransfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsIPDBedTransferVO)dgTransfer.SelectedItem != null)
            {
                PatientID = ((clsIPDBedTransferVO)dgTransfer.SelectedItem).PatientID;
                IsSelected = true;
            }

        }

        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillBedTransferHistoryGrid();
            }
        }
        #endregion
    }
}

