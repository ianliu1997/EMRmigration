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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.Forms.Home
{
    public partial class HomeCryoBank : UserControl
    {
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        TextBlock mElement;
        public HomeCryoBank()
        {
            InitializeComponent();
            mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Cryo Bank List";
            EmbryoList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            EmbryoList.OnRefresh += new EventHandler<RefreshEventArgs>(EmbryoList_OnRefresh);
            PageSize = 15;
            EmbryoList.PageSize = PageSize;
            EmbryoDataPager.DataContext = EmbryoList;


            SpermCollectionList = new PagedSortableCollectionView<clsSpermFreezingVO>();
            SpermCollectionList.OnRefresh += new EventHandler<RefreshEventArgs>(SpermCollection_OnRefresh);
            SpermListPageSize = 15;
            SpermCollectionList.PageSize = SpermListPageSize;
            SpermsDataPager.DataContext = SpermCollectionList;
        }
        void SpermCollection_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillSpermCollectionCryoBank();
        }

        void EmbryoList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillEmbryoCryoBank();
        }

        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = false;
        //}

        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
        //    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        //}
        public PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO> EmbryoList { get; private set; }
        public PagedSortableCollectionView<clsSpermFreezingVO> SpermCollectionList { get; private set; }
        public int PageSize
        {
            get
            {
                return EmbryoList.PageSize;
            }
            set
            {
                if (value == EmbryoList.PageSize) return;
                EmbryoList.PageSize = value;
                //  OnPropertyChanged("PageSize");
            }
        }
        public int SpermListPageSize
        {
            get
            {
                return SpermCollectionList.PageSize;
            }
            set
            {
                if (value == SpermCollectionList.PageSize) return;
                SpermCollectionList.PageSize = value;
            }
        }
        private void FillEmbryoCryoBank()
        {
            if (!string.IsNullOrEmpty(txtDonorCode.Text.Trim()))
            {
                EmbryoDataGrid.ItemsSource = null;
            }
            else
            {
                try
                {
                    cls_IVFDashboard_GetVitrificationDetailsForCryoBank BizAction = new cls_IVFDashboard_GetVitrificationDetailsForCryoBank();
                    // BizAction.SearchExpression = txtSearch.Text;
                    BizAction.PagingEnabled = true;
                    BizAction.MaximumRows = EmbryoList.PageSize;
                    BizAction.StartRowIndex = EmbryoList.PageIndex * EmbryoList.PageSize;
                    BizAction.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
                    // BizAction.Vitrification.PatientDetails = new List<clsPatientGeneralVO>();
                    //BizAction.CoupleUintID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    //Search
                    if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                        BizAction.FName = txtFirstName.Text.Trim();
                    if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
                        BizAction.MName = txtMiddleName.Text.Trim();
                    if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
                        BizAction.LName = txtLastName.Text.Trim();
                    if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
                        BizAction.FamilyName = txtFamilyName.Text.Trim();
                    if (!string.IsNullOrEmpty(txtMrno.Text.Trim()))
                        BizAction.MRNo = txtMrno.Text.Trim();
                    //rohini
                    if (cmbCane.SelectedItem != null)
                        BizAction.Cane = ((MasterListItem)cmbCane.SelectedItem).ID;

                    //if (dtpFromDate.SelectedDate.Value != null)
                    //    BizAction.FromDate = dtpFromDate.SelectedDate.Value;
                    //if (dtpToDate.SelectedDate.Value != null)
                    //    BizAction.ToDate = dtpToDate.SelectedDate.Value;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Result != null && arg.Error == null)
                        {
                            EmbryoDataGrid.ItemsSource = null;
                            EmbryoDataPager.DataContext = null;
                            BizAction.Vitrification = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification);
                            BizAction.Vitrification.VitrificationDetailsList = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification.VitrificationDetailsList);

                            if (BizAction.Vitrification.VitrificationDetailsList.Count > 0)
                            {
                                EmbryoList.Clear();
                                EmbryoList.TotalItemCount = Convert.ToInt16(((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).TotalRows);
                                foreach (clsIVFDashBoard_VitrificationDetailsVO item in BizAction.Vitrification.VitrificationDetailsList)
                                {
                                    EmbryoList.Add(item);
                                }
                                EmbryoDataGrid.ItemsSource = EmbryoList;
                                EmbryoDataPager.DataContext = EmbryoList;
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void FillSpermCollectionCryoBank()
        {
            try
            {
                clsGetSpremFreezingDetailsBizActionVO BizAction = new clsGetSpremFreezingDetailsBizActionVO();
                // BizAction.SearchExpression = txtSearch.Text;
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = SpermCollectionList.PageSize;
                BizAction.StartRowIndex = SpermCollectionList.PageIndex * SpermCollectionList.PageSize;
                BizAction.Vitrification = new List<clsSpermFreezingVO>();

                if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                    BizAction.FName = txtFirstName.Text.Trim();
                if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
                    BizAction.MName = txtMiddleName.Text.Trim();
                if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
                    BizAction.LName = txtLastName.Text.Trim();
                if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
                    BizAction.FamilyName = txtFamilyName.Text.Trim();
                if (!string.IsNullOrEmpty(txtMrno.Text.Trim()))
                    BizAction.MRNo = txtMrno.Text.Trim();
                if (!string.IsNullOrEmpty(txtDonorCode.Text.Trim()))
                    BizAction.DonorCode = txtDonorCode.Text.Trim();
                //rohini
                if (cmbCane.SelectedItem != null)
                    BizAction.Cane = ((MasterListItem)cmbCane.SelectedItem).ID;
                // BizAction.Vitrification.PatientDetails = new List<clsPatientGeneralVO>();
                BizAction.CoupleUintID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Result != null && arg.Error == null)
                    {
                        SpermsDataGrid.ItemsSource = null;
                        SpermsDataPager.DataContext = null;
                        BizAction.Vitrification = (((clsGetSpremFreezingDetailsBizActionVO)arg.Result).Vitrification);
                        if (BizAction.Vitrification.Count > 0)
                        {
                            SpermCollectionList.Clear();
                            SpermCollectionList.TotalItemCount = Convert.ToInt16(((clsGetSpremFreezingDetailsBizActionVO)arg.Result).TotalRows);
                            foreach (clsSpermFreezingVO item in BizAction.Vitrification)
                            {
                                SpermCollectionList.Add(item);
                            }
                            SpermsDataGrid.ItemsSource = SpermCollectionList;
                            SpermsDataPager.DataContext = SpermCollectionList;
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ValidationsForSearch()
        {
            if (dtpFromDate.SelectedDate.Value != null && dtpToDate.SelectedDate.Value != null)
            {
                if (dtpFromDate.SelectedDate.Value > dtpToDate.SelectedDate.Value)
                {
                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    //res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }
            if (dtpFromDate.SelectedDate.Value != null && dtpFromDate.SelectedDate.Value.Date > DateTime.Now.Date)
            {
                dtpFromDate.SetValidation("From Date should not be greater than Today's Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                // res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }
        }

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {

            if (chkEmbryo.IsChecked == true)
                FillEmbryoCryoBank();
            else if (chkSperm.IsChecked == true)
                FillSpermCollectionCryoBank();
            else if (chkBoth.IsChecked == true)
            {
                FillEmbryoCryoBank();
                FillSpermCollectionCryoBank();
            }
        }
        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
                txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
                txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
                txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
                txtFamilyName.Text.ToTitleCase();
        }

        private void chkRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (chkBoth.IsChecked == true || chkSperm.IsChecked == true)
            {
                Donor.Visibility = Visibility.Visible;
                txtDonorCode.Visibility = Visibility.Visible;
            }
            else
            {
                Donor.Visibility = Visibility.Collapsed;
                txtDonorCode.Visibility = Visibility.Collapsed;
            }
           
        }
        string Fname, Mname, Lname, Familyname, MRNo, CtcNo, CoupleUnitID;
        public long UnitID { get; set; }
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {

            //string URL = "../Reports/IVF/CryoBank.aspx?BillID=" + iBillId + "&UnitID=" + UnitID;
            //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

            if (txtMrno.Text != null)
                MRNo = txtMrno.Text;

            if (txtFirstName.Text != null)
                Fname = txtFirstName.Text;

            if (txtLastName.Text != null)
                Lname = txtLastName.Text;

            if (txtFamilyName.Text != null)
                Familyname = txtFamilyName.Text;

            UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            string URL = "../Reports/IVF/CryoBank.aspx?UnitID=" + UnitID + "&MRNo=" + MRNo + "&FName=" + Fname + "&LName=" + Lname + "&FamilyName=" + Familyname;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

            //string URL = "../Reports/IVF/CryoBank.aspx?UnitID=" + UnitID + "&MRNo=" + MRNo + "&CoupleUnitID=" +CoupleUnitID  + "&FName=" + Fname + "&LName=" + Lname + "&FamilyName=" + Familyname;
            //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        //private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        //{
        //    if (EmbryoDataGrid.SelectedItem != null)
        //    {
        //        //if (((clsGetVitrificationDetailsVO)EmbryoDataGrid.SelectedItem).IsSampleCollected)
        //        //{
        //        BarcodeForm win = new BarcodeForm();

        //        long VitrificationNo = ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).EmbSerialNumber;

        //        win.PrintData = "EM" + VitrificationNo.ToString();
        //        win.Show();
        //    }
        //    else
        //    {
        //        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select embryo.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
        //        msgbox.Show();

        //    }
        //    //}


        //}

        //private void CmdPrintSpermBarcode_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SpermsDataGrid.SelectedItem != null)
        //    {
        //        //if (((clsGetVitrificationDetailsVO)EmbryoDataGrid.SelectedItem).IsSampleCollected)
        //        //{
        //        BarcodeForm win = new BarcodeForm();

        //        long VitrificationNo = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem).SpremNo;

        //        win.PrintData = "SP" + VitrificationNo.ToString();
        //        win.Show();
        //    }
        //    else
        //    {
        //        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select sperm.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
        //        msgbox.Show();

        //    }
        //    //}

        //}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtFirstName.Focus();           
            FillEmbryoCryoBank();
            FillCaneList();
            FillSpermCollectionCryoBank();
            Donor.Visibility = Visibility.Visible;
            txtDonorCode.Visibility = Visibility.Visible;
        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            if (chkEmbryo.IsChecked == true)
            {
                Embryo.Visibility = Visibility.Visible;
                Sperms.Visibility = Visibility.Collapsed;
               
                FillEmbryoCryoBank();
            }
            else if (chkSperm.IsChecked == true)
            {
                Embryo.Visibility = Visibility.Collapsed;
                Sperms.Visibility = Visibility.Visible;
                FillSpermCollectionCryoBank();
            }
            else if (chkBoth.IsChecked == true)
            {
                Embryo.Visibility = Visibility.Visible;
                Sperms.Visibility = Visibility.Visible;
                FillEmbryoCryoBank();
                FillSpermCollectionCryoBank();
            }
        }
        private void FillCaneList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster ;
            BizAction.MasterList = new List<MasterListItem>();
            //PalashServiceClient Client = null;
            //Client = new PalashServiceClient();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    
                    cmbCane.ItemsSource = null;
                    cmbCane.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                    cmbCane.SelectedValue = objList[0];
                }
               
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        private void EmbryoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtMiddleName_KeyDown(object sender, KeyEventArgs e)
        {
           
        }
    }
}
