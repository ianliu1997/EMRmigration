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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using CIMS;

namespace OPDModule.Forms
{
    public partial class DonorCoupleLinkedList : ChildWindow
    {
        public string DonorID;
        public long DonorUnitID;
        public string DonorName;
        WaitIndicator indicator = new WaitIndicator();

        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;

        public PagedSortableCollectionView<clsPatientGeneralVO> CoupleList { get; private set; }

        public clsPatientGeneralVO DonorLink { get; set; }

        public int CoupleListPageSize
        {
            get
            {
                return CoupleList.PageSize;
            }
            set
            {
                if (value == CoupleList.PageSize) return;
                CoupleList.PageSize = value;
            }
        }

        public DonorCoupleLinkedList()
        {
            InitializeComponent();
            CoupleList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            CoupleList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            CoupleListPageSize = 15;
            dgDataPager.PageSize = CoupleListPageSize;
            dgDataPager.Source = CoupleList;

        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblDonor.Text = DonorName;
            FillCoupleList(DonorID, DonorUnitID);
        }


        public void FillCoupleList(string DonorID, long DonorUnitID)
        {
            indicator.Show();
            clsGetCoupleGeneralDetailsListBizActionVO BizAction = new clsGetCoupleGeneralDetailsListBizActionVO();
            BizAction.DonorID = DonorID;
            BizAction.DonorUnitID = DonorUnitID;
            BizAction.IsDonorLinkCouple = true;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = CoupleList.PageIndex * CoupleList.PageSize;
            BizAction.MaximumRows = CoupleList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    clsGetCoupleGeneralDetailsListBizActionVO result = e.Result as clsGetCoupleGeneralDetailsListBizActionVO;


                    CoupleList.Clear();
                    CoupleList.TotalItemCount = result.TotalRows;
                    if (result.PatientDetailsList != null)
                    {
                        foreach (var item in result.PatientDetailsList)
                        {
                            CoupleList.Add(item);
                        }
                    }


                    dataGrid1.ItemsSource = null;
                    dataGrid1.ItemsSource = CoupleList;
                    dataGrid1.SelectedIndex = -1;

                    dgDataPager.Source = null;
                    dgDataPager.PageSize = BizAction.MaximumRows;
                    dgDataPager.Source = CoupleList;

                    if (CoupleList.Count > 0)
                    {
                        chkAgainstStatus.IsChecked = true;
                        chkSelfStatus.IsChecked = false;
                    }
                    else
                    {
                        chkSelfStatus.IsChecked = true;
                        chkAgainstStatus.IsChecked = false;
                    }

                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();


        }



        private void AddBill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool IsValid = true;

            if (chkAgainstStatus.IsChecked == true  && ((clsPatientGeneralVO)dataGrid1.SelectedItem).IsPatientChecked == false)
            {
                IsValid = false;
                string msgText = "Please Select The Couple Patient";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);

                msgWD.Show();
                return;
            }

            DonorLink = new clsPatientGeneralVO();

            if (dataGrid1.ItemsSource != null && ((clsPatientGeneralVO)dataGrid1.SelectedItem) != null)
            {
                if (((clsPatientGeneralVO)dataGrid1.SelectedItem).IsPatientChecked == true)
                {
                    DonorLink.IsAgainstDonor = true;
                    DonorLink.PatientID = ((clsPatientGeneralVO)dataGrid1.SelectedItem).PatientID;
                    DonorLink.PatientUnitID = ((clsPatientGeneralVO)dataGrid1.SelectedItem).PatientUnitID;
                    DonorLink.CompanyID = ((clsPatientGeneralVO)dataGrid1.SelectedItem).CompanyID;
                    DonorLink.TariffID = ((clsPatientGeneralVO)dataGrid1.SelectedItem).TariffID;
                    DonorLink.PatientSourceID = ((clsPatientGeneralVO)dataGrid1.SelectedItem).PatientSourceID;
                }  
            }
 
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());
        }

        private void chkRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (chkSelfStatus.IsChecked == true)
            {
                if (((clsPatientGeneralVO)dataGrid1.SelectedItem) != null)
                {
                    ((clsPatientGeneralVO)dataGrid1.SelectedItem).IsPatientChecked = false;
                }
                dataGrid1.IsEnabled = false;
            }
            else
            {
                dataGrid1.IsEnabled = true;
            }
        }

        private void SelectCouple_Click(object sender, RoutedEventArgs e)
        {
            SelectCouple(sender, true);
        }

        private void SelectCouple(object sender, bool EventBy)
        {

            if (dataGrid1.ItemsSource == null)
                dataGrid1.ItemsSource = this.CoupleList;

            CheckBox chk = null;

            if (EventBy == false)
            {
                chk = (CheckBox)dataGrid1.Columns[0].GetCellContent(dataGrid1.SelectedItem);
                if (chk.IsChecked == false)
                    chk.IsChecked = true;
                else if (chk.IsChecked == true)
                    chk.IsChecked = false;
            }
            else
                chk = (CheckBox)sender;


            if (chk.IsChecked == true)
            {
                foreach (var Couple in CoupleList.Where(x => x.PatientID != ((clsPatientGeneralVO)dataGrid1.SelectedItem).PatientID))
                {
                    Couple.IsPatientChecked = false;
                }


                foreach (var Couple in CoupleList.Where(x => x.PatientID == ((clsPatientGeneralVO)dataGrid1.SelectedItem).PatientID))
                {
                    Couple.IsPatientChecked = true;
                }
            }
            else
            {
                long ID = ((clsPatientGeneralVO)dataGrid1.SelectedItem).ID;
                foreach (var BillClearance in CoupleList.Where(x => x.ID == ID))
                {

                }

            }

        }


    }
}


