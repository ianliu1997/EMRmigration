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
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmCryoBankForThawing : ChildWindow
    {
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        TextBlock mElement;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;

        public frmCryoBankForThawing()
        {
            InitializeComponent();
            mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Cryo Bank List";
            EmbryoList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            EmbryoList.OnRefresh += new EventHandler<RefreshEventArgs>(EmbryoList_OnRefresh);
            PageSize = 15;
            EmbryoList.PageSize = PageSize;
            EmbryoDataPager.DataContext = EmbryoList;


            //SpermCollectionList = new PagedSortableCollectionView<clsSpermFreezingVO>();
            //SpermCollectionList.OnRefresh += new EventHandler<RefreshEventArgs>(SpermCollection_OnRefresh);
            //SpermListPageSize = 15;
            //SpermCollectionList.PageSize = SpermListPageSize;
            //SpermsDataPager.DataContext = SpermCollectionList;

            OocyteList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            OocyteList.OnRefresh += new EventHandler<RefreshEventArgs>(OocyteList_OnRefresh);
            OocyteListPageSize = 15;
            OocyteList.PageSize = OocyteListPageSize;
            OocyteDataPager.DataContext = OocyteList;

            SaveCommandPanel.Visibility = Visibility.Collapsed;
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

        public PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO> OocyteList { get; private set; }     // For IVF ADM Changes

        // For IVF ADM Changes
        public int OocyteListPageSize
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

        #region For IVF ADM Changes

        public event RoutedEventHandler OnAddButton_Click;
        private ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> _SelectedOocytes;
        public ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO> SelectedOocytes { get { return _SelectedOocytes; } }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public bool IsForEmbryo { get; set; }  // flag use to show data for Embryo/Oocyte Bank

        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
                    //if (cmbCane.SelectedItem != null)
                    //    BizAction.Cane = ((MasterListItem)cmbCane.SelectedItem).ID;//Commented By Yogesh K

                    //if (dtpFromDate.SelectedDate.Value != null)
                    //    BizAction.FromDate = dtpFromDate.SelectedDate.Value;
                    //if (dtpToDate.SelectedDate.Value != null)
                    //    BizAction.ToDate = dtpToDate.SelectedDate.Value;

                    BizAction.IsFreezeOocytes = false;   // For IVF ADM Changes

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

                    BizAction.PatientID = PatientID;    // For IVF ADM Changes

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
                                    item.VitrificationDate = item.VitrificationDate.Value.Add(item.VitrificationTime.Value.TimeOfDay);
                                    if (item.ExpiryDate != null && item.ExpiryDate != Convert.ToDateTime("1/1/0001 12:00:00 AM"))
                                    {
                                        item.ExpiryDate = item.ExpiryDate;
                                        //if (item.ExpiryTime != null)
                                        //    item.ExpiryDate = item.ExpiryDate.Value.Add(item.ExpiryTime.Value.TimeOfDay);
                                    }
                                    else
                                        item.ExpiryDate = null;
                                    item.IsCheckRefreeze = true;
                                    if (item.ShortTerm == true)
                                        item.Type = "Short Term";
                                    if (item.LongTerm == true)
                                        item.Type = "Long Term";

                                    if (item.IsFreshEmbryoPGDPGS || item.IsFrozenEmbryoPGDPGS)
                                        item.PGDPGS = "PGDPGS";

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

                    BizAction.PatientID = PatientID;    // For IVF ADM Changes

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
                                    if (PlanTherapyID == item.PlanTherapyID && PlanTherapyUnitID == item.PlanTherapyUnitID)
                                    {
                                        if (item.IsRefreeze == true || item.IsRefreezeFromOtherCycle == true)
                                            item.IsCheckRefreeze = false;
                                        else
                                            item.IsCheckRefreeze = true;
                                    }
                                    else
                                        item.IsCheckRefreeze = true;
                                    //else
                                    //{
                                    //    if (item.IsRefreezeFromOtherCycle == true)
                                    //        item.IsCheckRefreeze = false;
                                    //    else
                                    //        item.IsCheckRefreeze = true;

                                    //}

                                    //if (item.IsRefreeze == true || item.IsRefreezeFromOtherCycle == true)
                                    //    item.IsCheckRefreeze = false;
                                    //else
                                    //    item.IsCheckRefreeze = true;

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
            //    try
            //    {
            //        clsGetSpremFreezingDetailsBizActionVO BizAction = new clsGetSpremFreezingDetailsBizActionVO();
            //        // BizAction.SearchExpression = txtSearch.Text;
            //        BizAction.PagingEnabled = true;
            //        BizAction.MaximumRows = SpermCollectionList.PageSize;
            //        BizAction.StartRowIndex = SpermCollectionList.PageIndex * SpermCollectionList.PageSize;
            //        BizAction.Vitrification = new List<clsSpermFreezingVO>();

            //        if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            //            BizAction.FName = txtFirstName.Text.Trim();
            //        if (!string.IsNullOrEmpty(txtMiddleName.Text.Trim()))
            //            BizAction.MName = txtMiddleName.Text.Trim();
            //        if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
            //            BizAction.LName = txtLastName.Text.Trim();
            //        if (!string.IsNullOrEmpty(txtFamilyName.Text.Trim()))
            //            BizAction.FamilyName = txtFamilyName.Text.Trim();
            //        if (!string.IsNullOrEmpty(txtMrno.Text.Trim()))
            //            BizAction.MRNo = txtMrno.Text.Trim();
            //        if (!string.IsNullOrEmpty(txtDonorCode.Text.Trim()))
            //            BizAction.DonorCode = txtDonorCode.Text.Trim();
            //        //rohini   
            //        if (cmbCane.SelectedItem != null)
            //            BizAction.Cane = ((MasterListItem)cmbCane.SelectedItem).ID;
            //        // BizAction.Vitrification.PatientDetails = new List<clsPatientGeneralVO>();
            //        BizAction.CoupleUintID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //        BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //        client.ProcessCompleted += (s, arg) =>
            //        {
            //            if (arg.Result != null && arg.Error == null)
            //            {
            //                SpermsDataGrid.ItemsSource = null;
            //                SpermsDataPager.DataContext = null;
            //                BizAction.Vitrification = (((clsGetSpremFreezingDetailsBizActionVO)arg.Result).Vitrification);
            //                if (BizAction.Vitrification.Count > 0)
            //                {
            //                    SpermCollectionList.Clear();
            //                    SpermCollectionList.TotalItemCount = Convert.ToInt16(((clsGetSpremFreezingDetailsBizActionVO)arg.Result).TotalRows);
            //                    foreach (clsSpermFreezingVO item in BizAction.Vitrification)
            //                    {
            //                        SpermCollectionList.Add(item);
            //                    }
            //                    SpermsDataGrid.ItemsSource = SpermCollectionList;
            //                    SpermsDataPager.DataContext = SpermCollectionList;
            //                }
            //            }
            //        };
            //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //        client.CloseAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
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
            if (chkOocyte.IsChecked == true)
                FillOocyteCryoBank();
            if (chkEmbryo.IsChecked == true)
                FillEmbryoCryoBank();
            else if (chkSperm.IsChecked == true)
                FillSpermCollectionCryoBank();
            //else if (chkBoth.IsChecked == true)
            //{
            //    FillEmbryoCryoBank();
            //    FillSpermCollectionCryoBank();
            //}
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

        Color myRgbColor;
        Color myRgbColor1;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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

            SolidColorBrush brush = new SolidColorBrush(myRgbColor1);
            SolidColorBrush brush1 = new SolidColorBrush(myRgbColor);

            lblExpired.Background = brush1;
            lblNearingExpired.Background = brush;

            txtFirstName.Focus();

            if (IsForEmbryo == true)  // flag use to show data for Embryo/Oocyte Bank
            {
                chkEmbryo.IsChecked = true;
                CryoBankInfo.SelectedIndex = 1;

                Embryotab.Visibility = Visibility.Visible;
                Oocytetab.Visibility = Visibility.Collapsed;

                FillEmbryoCryoBank();
            }
            else
            {
                chkOocyte.IsChecked = true;
                CryoBankInfo.SelectedIndex = 2;

                Embryotab.Visibility = Visibility.Collapsed;
                Oocytetab.Visibility = Visibility.Visible;

                FillOocyteCryoBank();
            }

            //FillSpermCollectionCryoBank();

            fillStraw();
            fillGobletColor();
            FillCanisterList();
            fillTank();

            //Donor.Visibility = Visibility.Visible;
            //txtDonorCode.Visibility = Visibility.Visible;
        }
        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            if (chkEmbryo.IsChecked == true)
            {
                //Embryo.Visibility = Visibility.Visible;
                //Sperms.Visibility = Visibility.Collapsed;

                //Embryotab.Visibility = Visibility.Visible;
                //Oocytetab.Visibility = Visibility.Collapsed;
                //Spermstab.Visibility = Visibility.Collapsed;

                FillEmbryoCryoBank();
            }
            else if (chkSperm.IsChecked == true)
            {
                //Embryo.Visibility = Visibility.Collapsed;
                //Sperms.Visibility = Visibility.Visible;

                //Embryotab.Visibility = Visibility.Collapsed;
                //Oocytetab.Visibility = Visibility.Collapsed;
                //Spermstab.Visibility = Visibility.Visible;

                FillSpermCollectionCryoBank();
            }
            else if (chkOocyte.IsChecked == true)
            {
                //Embryo.Visibility = Visibility.Collapsed;
                //Sperms.Visibility = Visibility.Visible;

                //Embryotab.Visibility = Visibility.Collapsed;
                //Oocytetab.Visibility = Visibility.Visible;
                //Spermstab.Visibility = Visibility.Collapsed;

                FillOocyteCryoBank();
            }
            else if (chkBoth.IsChecked == true)
            {
                //Embryo.Visibility = Visibility.Visible;
                //Sperms.Visibility = Visibility.Visible;

                //Embryotab.Visibility = Visibility.Visible;
                //Oocytetab.Visibility = Visibility.Visible;
                //Spermstab.Visibility = Visibility.Visible;

                FillEmbryoCryoBank();
                FillSpermCollectionCryoBank();
                FillOocyteCryoBank();
            }
        }

        //added by rohini dated 17/12/2015
        private void fillStraw()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFStrawMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbStraw.ItemsSource = null;
                    cmbStraw.ItemsSource = objList;
                    cmbStraw.SelectedItem = objList[0];
                }

                //if (this.DataContext != null)
                //{
                //    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                //}

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        //added by rohini dated 17/12/2015
        private void fillGobletColor()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFGobletColor;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbGobletColor.ItemsSource = null;
                    cmbGobletColor.ItemsSource = objList.DeepCopy();
                    cmbGobletColor.SelectedItem = objList[0].DeepCopy();

                    cmbTabColor.ItemsSource = null;
                    cmbTabColor.ItemsSource = objList.DeepCopy();
                    cmbTabColor.SelectedItem = objList[0].DeepCopy();
                }

                //if (this.DataContext != null)
                //{
                //    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                //}
                //fillStraw();

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillCanisterList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFCanisterMaster;   //MasterTableNameList.M_IVFCanMaster;
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

                    cmbCanisterNo.ItemsSource = null;
                    cmbCanisterNo.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                    cmbCanisterNo.SelectedItem = objList[0];
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        //added by rohini dated 17/12/2015
        private void fillTank()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFTankMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbTank.ItemsSource = null;
                    cmbTank.ItemsSource = objList;
                    cmbTank.SelectedItem = objList[0];
                }

                //if (this.DataContext != null)
                //{
                //    //cmbSrcTreatmentPlan.SelectedValue = ((clsVO)this.DataContext).;
                //}
                //fillGobletColor();

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }





        private void EmbryoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtMiddleName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void CryoBankInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chkOocyte_Click(object sender, RoutedEventArgs e)
        {
            if (OocyteDataGrid.SelectedItem != null)
            {
                if (_SelectedOocytes == null)
                    _SelectedOocytes = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    if (_SelectedOocytes.Count > 0)
                    {
                        var item = from r in _SelectedOocytes
                                   where r.ID == ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).ID
                                   select new clsIVFDashBoard_VitrificationDetailsVO
                                   {
                                       EmbStatus = r.EmbStatus,
                                       ID = r.ID,
                                       EmbNumber = r.EmbNumber
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).EmbNumber);

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "";

                                if (chkOocyte.IsChecked == true)
                                    strMsg = "Oocyte already Selected : " + strError.ToString();
                                else if (chkEmbryo.IsChecked == true)
                                    strMsg = "Embryo already Selected : " + strError.ToString();

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            _SelectedOocytes.Add((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem);
                        }
                    }
                    else
                    {
                        _SelectedOocytes.Add((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem);
                    }
                }
                else
                    _SelectedOocytes.Remove((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem);


                foreach (var item in _SelectedOocytes)
                {
                    item.SelectOocyte = true;
                }

            }
        }

        private void CmdThawOocytes_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (_SelectedOocytes == null)
            {
                isValid = false;
            }
            else if (_SelectedOocytes.Count <= 0)
            {
                isValid = false;
            }
            if (isValid)
            {
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                string strMsg = "";

                if (chkOocyte.IsChecked == true)
                    strMsg = "No Oocyte/s Selected for Adding";
                else if (chkEmbryo.IsChecked == true)
                    strMsg = "No Embryo/s Selected for Adding";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void chkEmbryo_Click(object sender, RoutedEventArgs e)
        {
            if (EmbryoDataGrid.SelectedItem != null)
            {
                if (_SelectedOocytes == null)
                    _SelectedOocytes = new ObservableCollection<clsIVFDashBoard_VitrificationDetailsVO>();
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    if (_SelectedOocytes.Count > 0)
                    {
                        var item = from r in _SelectedOocytes
                                   where r.ID == ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).ID
                                   select new clsIVFDashBoard_VitrificationDetailsVO
                                   {
                                       EmbStatus = r.EmbStatus,
                                       ID = r.ID,
                                       EmbNumber = r.EmbNumber
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem).EmbNumber);

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "";

                                if (chkOocyte.IsChecked == true)
                                    strMsg = "Oocyte already Selected : " + strError.ToString();
                                else if (chkEmbryo.IsChecked == true)
                                    strMsg = "Embryo already Selected : " + strError.ToString();

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            _SelectedOocytes.Add((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem);
                        }
                    }
                    else
                    {
                        _SelectedOocytes.Add((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem);
                    }
                }
                else
                    _SelectedOocytes.Remove((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem);


                foreach (var item in _SelectedOocytes)
                {
                    item.SelectOocyte = true;
                }

            }
        }

        private void chkRadioButton_Click(object sender, RoutedEventArgs e)
        {

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

                if (item.ExpiryDate < currentDate)
                {
                    e.Row.Background = new SolidColorBrush(myRgbColor);
                    item.IsCheckRefreeze = false;
                }
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
                {
                    e.Row.Background = new SolidColorBrush(myRgbColor);
                    item.IsCheckRefreeze = false;
                }
            }

        }

    }
}

