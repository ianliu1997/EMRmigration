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
using PalashDynamics.ValueObjects;
using System.Reflection;
using System.Text;


namespace PalashDynamics.IVF.DashBoard
{
    public partial class CryoBankForDashboard : ChildWindow
    {
        public CryoBankForDashboard()
        {
            InitializeComponent();

            // Added By neena For Oocyte 
            OocyteList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            OocyteList.OnRefresh += new EventHandler<RefreshEventArgs>(OocyteList_OnRefresh);
            OocytePageSize = 15;
            OocyteList.PageSize = OocytePageSize;
            OocyteDataPager.DataContext = OocyteList;
            //END 

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

        void OocyteList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOocyteCryoBank();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        public PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO> OocyteList { get; private set; }
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

        public int OocytePageSize
        {
            get
            {
                return OocyteList.PageSize;
            }
            set
            {
                if (value == OocyteList.PageSize) return;
                OocyteList.PageSize = value;
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
                            EmbryoDataGrid.DataContext = null;
                            //EmbryoDataPager.DataContext = null;
                            BizAction.Vitrification = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification);
                            BizAction.Vitrification.VitrificationDetailsList = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification.VitrificationDetailsList);

                            if (BizAction.Vitrification.VitrificationDetailsList.Count > 0)
                            {
                                EmbryoList.Clear();
                                EmbryoList.TotalItemCount = Convert.ToInt16(((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).TotalRows);
                                foreach (clsIVFDashBoard_VitrificationDetailsVO item in BizAction.Vitrification.VitrificationDetailsList)
                                {
                                    if (item.ExpiryDate != null && item.ExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                    {
                                        item.ExpiryDate = item.ExpiryDate;
                                        //if (item.ExpiryTime != null)
                                        //    item.ExpiryDate = item.ExpiryDate.Value.Add(item.ExpiryTime.Value.TimeOfDay);
                                    }
                                    else
                                        item.ExpiryDate = null;
                                    if (item.ShortTerm == true)
                                        item.Type = "Short Term";
                                    if (item.LongTerm == true)
                                        item.Type = "Long Term";
                                    if (item.IsFreshEmbryoPGDPGS || item.IsFrozenEmbryoPGDPGS)
                                        item.PGDPGS = "PGDPGS";
                                    EmbryoList.Add(item);                                   
                                }                            
                                EmbryoDataGrid.ItemsSource = EmbryoList;
                                EmbryoDataGrid.DataContext = EmbryoList;
                                //EmbryoDataPager.DataContext = EmbryoList;
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

        //Added By neena For Oocyte Cryo Bank 
        //private void FillOocyteCryoBank()
        //{
        //    if (!string.IsNullOrEmpty(txtDonorCode.Text.Trim()))
        //    {
        //        OocyteDataGrid.ItemsSource = null;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank BizAction = new cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank();

        //            // BizAction.SearchExpression = txtSearch.Text;
        //            BizAction.PagingEnabled = true;
        //            BizAction.MaximumRows = OocyteList.PageSize;
        //            BizAction.StartRowIndex = OocyteList.PageIndex * OocyteList.PageSize;
        //            BizAction.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
        //            // BizAction.Vitrification.PatientDetails = new List<clsPatientGeneralVO>();
        //            //BizAction.CoupleUintID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //            //BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //            //Search
        //            if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
        //                BizAction.FName = txtFirstName.Text.Trim();
        //            if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
        //                BizAction.MName = txtMiddleName.Text.Trim();
        //            if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
        //                BizAction.LName = txtLastName.Text.Trim();
        //            if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
        //                BizAction.FamilyName = txtFamilyName.Text.Trim();
        //            if (!string.IsNullOrEmpty(txtMrno.Text.Trim()))
        //                BizAction.MRNo = txtMrno.Text.Trim();


        //            //if (dtpFromDate.SelectedDate.Value != null)
        //            //    BizAction.FromDate = dtpFromDate.SelectedDate.Value;
        //            //if (dtpToDate.SelectedDate.Value != null)
        //            //    BizAction.ToDate = dtpToDate.SelectedDate.Value;

        //            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //            client.ProcessCompleted += (s, arg) =>
        //            {
        //                if (arg.Result != null && arg.Error == null)
        //                {
        //                    OocyteDataGrid.ItemsSource = null;
        //                    OocyteDataGrid.DataContext = null;
        //                    BizAction.Vitrification = (((cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank)arg.Result).Vitrification);
        //                    BizAction.Vitrification.VitrificationDetailsForOocyteList = (((cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank)arg.Result).Vitrification.VitrificationDetailsForOocyteList);

        //                    if (BizAction.Vitrification.VitrificationDetailsForOocyteList.Count > 0)
        //                    {
        //                        OocyteList.Clear();
        //                        OocyteList.TotalItemCount = Convert.ToInt16(((cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank)arg.Result).TotalRows);
        //                        foreach (clsIVFDashBoard_VitrificationDetailsVO item in BizAction.Vitrification.VitrificationDetailsForOocyteList)
        //                        {
        //                            OocyteList.Add(item);
        //                        }
        //                        OocyteDataGrid.ItemsSource = OocyteList;
        //                        OocyteDataPager.DataContext = OocyteList;
        //                    }
        //                }
        //            };
        //            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //            client.CloseAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
        //}

        List<MasterListItem> mlSourceOfSperm = null;
        private void fillOocyteMaturity()
        {
            mlSourceOfSperm = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlSourceOfSperm.Insert(0, Default);
            EnumToList(typeof(OocyteMaturity), mlSourceOfSperm);
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {

                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);
                TheMasterList.Add(Item);
            }
        }

        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }

        private void FillOocyteCryoBank()   // For IVF ADM Changes
        {
            if (!string.IsNullOrEmpty(txtDonorCode.Text.Trim()))
            {
                OocyteDataGrid.ItemsSource = null;
            }
            else
            {
                try
                {
                    fillOocyteMaturity();
                    cls_IVFDashboard_GetVitrificationDetailsForCryoBank BizAction = new cls_IVFDashboard_GetVitrificationDetailsForCryoBank();
                    // BizAction.SearchExpression = txtSearch.Text;
                    BizAction.PagingEnabled = true;
                    BizAction.MaximumRows = OocyteList.PageSize;
                    BizAction.StartRowIndex = OocyteList.PageIndex * OocyteList.PageSize;
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
                    //if (cmbCane.SelectedItem != null)
                    //    BizAction.Cane = ((MasterListItem)cmbCane.SelectedItem).ID;//Commented By Yogesh K

                    //if (dtpFromDate.SelectedDate.Value != null)
                    //    BizAction.FromDate = dtpFromDate.SelectedDate.Value;
                    //if (dtpToDate.SelectedDate.Value != null)
                    //    BizAction.ToDate = dtpToDate.SelectedDate.Value;

                    BizAction.IsFreezeOocytes = true;   // For IVF ADM Changes

                    //if (cmbStraw.SelectedItem != null && ((MasterListItem)cmbStraw.SelectedItem).ID > 0)    // For IVF ADM Changes
                    //    BizAction.StrawId = ((MasterListItem)cmbStraw.SelectedItem).ID;

                    //if (cmbGobletColor.SelectedItem != null && ((MasterListItem)cmbGobletColor.SelectedItem).ID > 0)    // For IVF ADM Changes
                    //    BizAction.GobletColorId = ((MasterListItem)cmbGobletColor.SelectedItem).ID;

                    //if (!string.IsNullOrEmpty(txtCryoCode.Text.Trim()))     // For IVF ADM Changes
                    //    BizAction.CryoCode = txtCryoCode.Text.Trim();

                    //if (cmbTabColor.SelectedItem != null && ((MasterListItem)cmbTabColor.SelectedItem).ID > 0)  // For IVF ADM Changes
                    //    BizAction.TabColorId = ((MasterListItem)cmbTabColor.SelectedItem).ID;

                    //if (cmbCanisterNo.SelectedItem != null && ((MasterListItem)cmbCanisterNo.SelectedItem).ID > 0)  // For IVF ADM Changes
                    //    BizAction.CanisterId = ((MasterListItem)cmbCanisterNo.SelectedItem).ID;

                    //if (cmbTank.SelectedItem != null && ((MasterListItem)cmbTank.SelectedItem).ID > 0)  // For IVF ADM Changes
                    //    BizAction.TankId = ((MasterListItem)cmbTank.SelectedItem).ID;



                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Result != null && arg.Error == null)
                        {
                            OocyteDataGrid.ItemsSource = null;
                            OocyteDataGrid.DataContext = null;
                            BizAction.Vitrification = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification);
                            BizAction.Vitrification.VitrificationDetailsList = (((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).Vitrification.VitrificationDetailsList);

                            if (BizAction.Vitrification.VitrificationDetailsList.Count > 0)
                            {
                                OocyteList.Clear();
                                OocyteList.TotalItemCount = Convert.ToInt16(((cls_IVFDashboard_GetVitrificationDetailsForCryoBank)arg.Result).TotalRows);
                                foreach (clsIVFDashBoard_VitrificationDetailsVO item in BizAction.Vitrification.VitrificationDetailsList)
                                {
                                    //oocyte grade  

                                    if ((item.GradeID) > 0)
                                    {
                                        item.Grade = mlSourceOfSperm.FirstOrDefault(p => p.ID == item.GradeID).Description;
                                    }
                                    //
                                    if (item.ExpiryDate != null && item.ExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                    {
                                        item.ExpiryDate = item.ExpiryDate;
                                    }
                                    else
                                        item.ExpiryDate = null;
                                    if (item.ShortTerm == true)
                                        item.Type = "Short Term";
                                    if (item.LongTerm == true)
                                        item.Type = "Long Term";
                                    OocyteList.Add(item); 
                                }
                                OocyteDataGrid.ItemsSource = OocyteList;
                                OocyteDataGrid.DataContext = OocyteList;
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
                        SpermsDataGrid.DataContext = null;
                        //SpermsDataPager.DataContext = null;
                        BizAction.Vitrification = (((clsGetSpremFreezingDetailsBizActionVO)arg.Result).Vitrification);
                        if (BizAction.Vitrification.Count > 0)
                        {
                            SpermCollectionList.Clear();
                            SpermCollectionList.TotalItemCount = Convert.ToInt16(((clsGetSpremFreezingDetailsBizActionVO)arg.Result).TotalRows);
                            foreach (clsSpermFreezingVO item in BizAction.Vitrification)
                            {
                                if (item.ExpiryDate != null && item.ExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                {
                                    item.ExpiryDate = item.ExpiryDate;
                                }
                                else
                                    item.ExpiryDate = null;
                                if (item.ShortTerm == true)
                                    item.Type = "Short Term";
                                if (item.LongTerm == true)
                                    item.Type = "Long Term";
                                SpermCollectionList.Add(item);                                
                            }                        
                            SpermsDataGrid.ItemsSource = SpermCollectionList;
                            SpermsDataGrid.DataContext = SpermCollectionList;
                            //SpermsDataPager.DataContext = SpermCollectionList;
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

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (EmbryoDataGrid.SelectedItem != null)
            {
                //if (((clsGetVitrificationDetailsVO)EmbryoDataGrid.SelectedItem).IsSampleCollected)
                //{
                BarcodeForm win = new BarcodeForm();

                long VitrificationNo = ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).EmbSerialNumber;

                win.PrintData = "EM" + VitrificationNo.ToString();
                win.Show();
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select embryo.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
            //}


        }

        private void CmdPrintSpermBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (SpermsDataGrid.SelectedItem != null)
            {
                //if (((clsGetVitrificationDetailsVO)EmbryoDataGrid.SelectedItem).IsSampleCollected)
                //{
                BarcodeForm win = new BarcodeForm();

                long VitrificationNo = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem).SpremNo;

                win.PrintData = "SP" + VitrificationNo.ToString();
                win.Show();
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select sperm.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
            //}

        }

        Color myRgbColor;
        Color myRgbColor1;
        //Color myRgbColor3;

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillOocyteCryoBank();
            FillEmbryoCryoBank();
            FillSpermCollectionCryoBank();
            Donor.Visibility = Visibility.Visible;
            txtDonorCode.Visibility = Visibility.Visible;


            //myRgbColor = new Color();
            //myRgbColor.A = Convert.ToByte(100);
            //myRgbColor.R = Convert.ToByte(255);
            //myRgbColor.G = Convert.ToByte(0);
            //myRgbColor.B = Convert.ToByte(0);

            myRgbColor1 = new Color();
            myRgbColor1.A = Convert.ToByte(120);
            myRgbColor1.R = Convert.ToByte(240);
            myRgbColor1.G = Convert.ToByte(230);
            myRgbColor1.B = Convert.ToByte(140);

            myRgbColor = new Color();
            myRgbColor.A = Convert.ToByte(120);
            myRgbColor.R = Convert.ToByte(255);
            myRgbColor.G = Convert.ToByte(153);
            myRgbColor.B = Convert.ToByte(51);

            SolidColorBrush brush = new SolidColorBrush( myRgbColor1 );
            SolidColorBrush brush1 = new SolidColorBrush( myRgbColor );

            lblExpired.Background = brush1;
            lblNearingExpired.Background = brush;

            //myRgbColor3 = new Color();
            //myRgbColor3.A = Convert.ToByte(120);
            //myRgbColor3.R = Convert.ToByte(65);
            //myRgbColor3.G = Convert.ToByte(105);
            //myRgbColor3.B = Convert.ToByte(225);
           
        }

        private void CryoBankInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DonateDiscard_Click(object sender, RoutedEventArgs e)
        {
            FrmPatientListForDonate WinDonate = new FrmPatientListForDonate();
            WinDonate.IsOocyteList = true;
            WinDonate.OnSaveButton_Click += new RoutedEventHandler(WinDonate_OnSaveButton_Click);
            WinDonate.PatientID = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).PatientID;
            WinDonate.PatientUnitID = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).UnitID;
            WinDonate.Show();
        }

        void WinDonate_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FillOocyteCryoBank();
        }

        private void DonateDiscardEmbryo_Click(object sender, RoutedEventArgs e)
        {
            FrmPatientListForDonate WinDonate = new FrmPatientListForDonate();
            WinDonate.IsEmbList = true;
            WinDonate.OnSaveButton_Click += new RoutedEventHandler(WinDonateEmb_OnSaveButton_Click);
            WinDonate.PatientID = ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).PatientID;
            WinDonate.PatientUnitID = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).UnitID;
            WinDonate.Show();
        }

        void WinDonateEmb_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FillEmbryoCryoBank();
        }

        StringBuilder OocyteNo = new StringBuilder();
        StringBuilder SpermNo = new StringBuilder();
        private void hlOocyteRenewal_Click(object sender, RoutedEventArgs e)
        {
            OocyteNo = new StringBuilder();
            FrmCryoRenewal CryoDate = new FrmCryoRenewal();
            CryoDate.IsOocyteFreezed = true;
            CryoDate.OnCloseButton_Click += new RoutedEventHandler(CryoDate_OnCloseButton_Click);
            CryoDate.VitificationID = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).VitrivicationID;
            CryoDate.VitificationUnitID = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).VitrificationUnitID;
            CryoDate.PriviousExpiryDate = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).ExpiryDate;
            var oocytelist = OocyteList.Where(x => x.VitrivicationID == CryoDate.VitificationID).ToList();
            foreach (var item in oocytelist)
            {
                if (OocyteNo.ToString().Length > 0)
                    OocyteNo.Append(",");
                OocyteNo.Append(item.EmbNumber.ToString());
            }
            CryoDate.Title = "Renewal Date For Oocyte : " + OocyteNo;
            CryoDate.Show();
        }

        void CryoDate_OnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            FillOocyteCryoBank();
        }

        private void hlEmbryoRenewal_Click(object sender, RoutedEventArgs e)
        {
            FrmCryoRenewal CryoDate = new FrmCryoRenewal();
            CryoDate.IsOocyteFreezed = false;
            CryoDate.OnCloseButton_Click += new RoutedEventHandler(EmbryoCryoDate_OnCloseButton_Click);
            CryoDate.VitificationDetailsID = ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).ID;
            CryoDate.VitificationDetailsUnitID = ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).UnitID;
            CryoDate.PriviousExpiryDate = ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).ExpiryDate;
            CryoDate.Title = "Renewal Date For Embryo : " + ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).EmbNumber;
            CryoDate.Show();
        }

        void EmbryoCryoDate_OnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            FillEmbryoCryoBank();
        }

        private void hlSpermRenewal_Click(object sender, RoutedEventArgs e)
        {
            SpermNo = new StringBuilder();
            FrmCryoRenewal CryoDate = new FrmCryoRenewal();
            CryoDate.IsSprem = true;
            CryoDate.OnCloseButton_Click += new RoutedEventHandler(SpermCryoDate_OnCloseButton_Click);
            CryoDate.SpremFreezingID = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem).SpremFreezingID;
            CryoDate.SpremFreezingUnitID = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem).SpremFreezingUnitID;
            CryoDate.PriviousExpiryDate = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem).ExpiryDate;
            var spermlist = SpermCollectionList.Where(x => x.SpremFreezingID == CryoDate.SpremFreezingID).ToList();
            foreach (var item in spermlist)
            {
                if (SpermNo.ToString().Length > 0)
                    SpermNo.Append(",");
                SpermNo.Append(item.SpremNo.ToString());
            }
            CryoDate.Title = "Renewal Date For Sperm : " + SpermNo;
            CryoDate.Show();
        }

        void SpermCryoDate_OnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            FillSpermCollectionCryoBank();
        }

        private void OocyteDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsIVFDashBoard_VitrificationDetailsVO item = (clsIVFDashBoard_VitrificationDetailsVO)e.Row.DataContext;
            DateTime currentDate = DateTime.Now;
            DateTime NextOneMonthDate = currentDate.AddMonths(1);
            if (item.ExpiryDate != null)
            {
                DateTime NextOneMonthDateExpiry = item.ExpiryDate.Value.AddMonths(1);

                if (item.ExpiryDate >= currentDate && item.ExpiryDate <= NextOneMonthDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor1);

                if(item.ExpiryDate<currentDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor);
            }

            
        }

        private void EmbryoDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsIVFDashBoard_VitrificationDetailsVO item = (clsIVFDashBoard_VitrificationDetailsVO)e.Row.DataContext;
            DateTime currentDate = DateTime.Now;
            DateTime NextOneMonthDate = currentDate.AddMonths(1);
            if (item.ExpiryDate != null)
            {
                //item.ExpiryDate = item.ExpiryDate.Value.Add(item.ExpiryTime.Value.TimeOfDay);
                DateTime NextOneMonthDateExpiry = item.ExpiryDate.Value.AddMonths(1);

                if (item.ExpiryDate >= currentDate && item.ExpiryDate <= NextOneMonthDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor1);

                if (item.ExpiryDate < currentDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor);
            }

        }

        private void SpermsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsSpermFreezingVO item = (clsSpermFreezingVO)e.Row.DataContext;
            DateTime currentDate = DateTime.Now;
            DateTime NextOneMonthDate = currentDate.AddMonths(1);
            if (item.ExpiryDate != null)
            {
                DateTime NextOneMonthDateExpiry = item.ExpiryDate.Value.AddMonths(1);

                if (item.ExpiryDate >= currentDate && item.ExpiryDate <= NextOneMonthDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor1);

                if (item.ExpiryDate < currentDate)
                    e.Row.Background = new SolidColorBrush(myRgbColor);
            }
        }

        
        private void DiscardSperm_Click(object sender, RoutedEventArgs e)
        {
            FrmPatientSpermDiscard WinDonate = new FrmPatientSpermDiscard(); 
            WinDonate.IsSperm = true;
            WinDonate.OnSaveButton_Click += new RoutedEventHandler(WinDonateSperm_OnSaveButton_Click);
            WinDonate.PatientID = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem).PatientID;
            WinDonate.PatientUnitID = ((clsSpermFreezingVO)SpermsDataGrid.SelectedItem).PatientUnitID;
            WinDonate.Show();
        }

        void WinDonateSperm_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FillSpermCollectionCryoBank();
        }

    }
}

