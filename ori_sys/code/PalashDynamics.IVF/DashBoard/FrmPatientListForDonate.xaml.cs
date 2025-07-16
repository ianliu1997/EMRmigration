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
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Reflection;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmPatientListForDonate : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        bool isLoaded = false;
        public long PatientCategoryID { get; set; }
        public string Mrno { get; set; }
        public string PatientName { get; set; }
        public string SearchKeyword { get; set; }
        public long PatientID;
        public long PatientUnitID;
        public bool IsOocyteList = false;
        public bool IsEmbList = false;
        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }

        #region Properties
        /// <summary>
        /// Gets or sets the data list.
        /// </summary>
        /// <value>The data list to data bind ItemsSource.</value>
        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                //RaisePropertyChanged("PageSize");
            }
        }
        #endregion

        public FrmPatientListForDonate()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(PatientSearch_Loaded);
            this.DataContext = new clsPatientGeneralVO();

            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;

            OocyteList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            OocyteList.OnRefresh += new EventHandler<RefreshEventArgs>(OocyteList_OnRefresh);
            OocyteListPageSize = 15;
            OocyteList.PageSize = OocyteListPageSize;
            OocyteDataPager.DataContext = OocyteList;

            EmbryoList = new PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO>();
            EmbryoList.OnRefresh += new EventHandler<RefreshEventArgs>(EmbryoList_OnRefresh);
            EmbryoListPageSize = 15;
            EmbryoList.PageSize = EmbryoListPageSize;
            EmbryoDataPager.DataContext = EmbryoList;

        }

        void OocyteList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOocyteCryoBank();
        }

        void EmbryoList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillEmbryoCryoBank();
        }

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

        public int EmbryoListPageSize
        {
            get
            {
                return EmbryoList.PageSize;
            }
            set
            {
                if (value == EmbryoList.PageSize) return;
                EmbryoList.PageSize = value;
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }

        void PatientSearch_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            txtFirstName.Focus();
            if (PatientName != null)
                txtFirstName.Text = PatientName;
            if (Mrno != null)
                txtLastName.Text = Mrno;
            GetData();
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
        public PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO> OocyteList { get; private set; }
        public PagedSortableCollectionView<clsIVFDashBoard_VitrificationDetailsVO> EmbryoList { get; private set; }

        private void FillOocyteCryoBank()   // For IVF ADM Changes
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

                                item.PlanList = PlanList;
                                if (Convert.ToInt64(item.PlanId) > 0)
                                {
                                    item.SelectedPlanId = PlanList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PlanId));
                                }
                                else
                                {
                                    item.SelectedPlanId = PlanList.FirstOrDefault(p => p.ID == 0);
                                }
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

        private void FillEmbryoCryoBank()
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

                BizAction.IsFreezeOocytes = false;
                BizAction.PatientID = PatientID;
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
                                item.PlanList = PlanList;
                                if (Convert.ToInt64(item.PlanId) > 0)
                                {
                                    item.SelectedPlanId = PlanList.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PlanId));
                                }
                                else
                                {
                                    item.SelectedPlanId = PlanList.FirstOrDefault(p => p.ID == 0);
                                }
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

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
        }

        #region Validation
        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        string textBefore = null;

        private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsNumberValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;

                }
            }
        }

        #endregion

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        public void GetData()
        {
            GetPatientListForDashboardBizActionVO BizActionObject = new GetPatientListForDashboardBizActionVO();

            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();

            BizActionObject.VisitWise = false;

            // BizActionObject.PatientCategoryID = PatientCategoryID;

            if (txtFirstName.Text != "")
                BizActionObject.FirstName = txtFirstName.Text;
            if (txtMiddleName.Text != "")
                BizActionObject.MiddleName = txtMiddleName.Text;
            if (txtLastName.Text != "")
                BizActionObject.LastName = txtLastName.Text;
            if (txtFamilyName.Text != "")
                BizActionObject.FamilyName = txtFamilyName.Text;

            if (txtMrno.Text != "")
                BizActionObject.MRNo = txtMrno.Text;

            if (txtOPDNo.Text != "")
                BizActionObject.OPDNo = txtOPDNo.Text;

            if (txtContactNo.Text != "")
                BizActionObject.ContactNo = txtContactNo.Text;

            if (txtCivilID.Text != "")
                BizActionObject.CivilID = txtCivilID.Text;
            if (SearchKeyword != null)
                BizActionObject.SearchKeyword = SearchKeyword;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizActionObject.UnitID = 0;
            }
            else
            {
                BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizActionObject.IsPagingEnabled = true;
            BizActionObject.MaximumRows = DataList.PageSize; ;
            BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        GetPatientListForDashboardBizActionVO result = ea.Result as GetPatientListForDashboardBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                        {
                            DataList.Add(person);
                        }
                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = DataList;

                        DataPager.Source = null;
                        DataPager.PageSize = BizActionObject.MaximumRows;
                        DataPager.Source = DataList;
                    }
                }
            };
            client.ProcessAsync(BizActionObject, new clsUserVO());
            client.CloseAsync();
        }
        //List<MasterListItem> PlanList = null;
        private List<MasterListItem> _PlanList = new List<MasterListItem>();
        public List<MasterListItem> PlanList
        {
            get
            {
                return _PlanList;
            }
            set
            {
                _PlanList = value;
            }
        }
        private void FillPlanList()
        {
            PlanList = new List<MasterListItem>();
            PlanList.Add(new MasterListItem(0, "--Select--"));
            PlanList.Add(new MasterListItem(1, "Discard"));
            PlanList.Add(new MasterListItem(2, "Donate"));
        }

        //clsPatientGeneralVO SelectedPatient = new clsPatientGeneralVO();
        WaitIndicator wait = new WaitIndicator();
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null && ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID != 0)
            {
                try
                {
                    //SelectedPatient = ((clsPatientGeneralVO)dataGrid2.SelectedItem);
                    if (wait == null)
                    {
                        wait = new WaitIndicator();
                        wait.Show();
                    }

                    clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
                    BizAction.PatientID = ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID;
                    BizAction.PatientUnitID = ((clsPatientGeneralVO)dataGrid2.SelectedItem).UnitId;
                    BizAction.CoupleDetails = new clsCoupleVO();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                            {
                                BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                                BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                                CoupleDetails.MalePatient = new clsPatientGeneralVO();
                                CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                                CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                                ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = CoupleDetails;


                                if (BizAction.CoupleDetails.MalePatient == null || BizAction.CoupleDetails.FemalePatient == null || BizAction.CoupleDetails.FemalePatient.PatientID == 0 || BizAction.CoupleDetails.MalePatient.PatientID == 0)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Only Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    wait.Close();
                                }
                                else if ((PatientID == CoupleDetails.FemalePatient.PatientID) && (PatientUnitID == CoupleDetails.FemalePatient.UnitId))
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Can not donate to same patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    wait.Close();
                                }
                                else
                                {
                                    txtSelectedPatientName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                                    txtSelectedMrNO.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                                    //this.DialogResult = true;
                                    //if (OnSaveButton_Click != null)
                                    //{
                                    //    OnSaveButton_Click(this, new RoutedEventArgs());
                                    //    this.Close();
                                    //}
                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Only Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                wait.Close();
                            }
                            wait.Close();
                        }
                        else
                        {
                            wait.Close();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    wait.Close();
                    throw ex;
                }

                //this.DialogResult = true;
                //if (OnSaveButton_Click != null)
                //{  
                //    OnSaveButton_Click(this, new RoutedEventArgs());
                //    this.Close();
                //}
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
            }
        }

        private bool ValidateSaving()
        {
            bool result = true;

            if (IsOocyteList)
            {
                var item = OocyteList.Where(x => x.SelectedPlanId.ID == 2).ToList();
                var item1 = OocyteList.All(x => x.SelectedPlanId.ID == 0);
                if (item1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Please Select Embryo.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    result = false;
                }
                else if (item.Count > 0)
                {
                    if (txtSelectedPatientName.Text.Trim() == string.Empty)
                    {
                        txtSelectedPatientName.SetValidation("Please Select Recepient ");
                        txtSelectedPatientName.RaiseValidationError();
                        txtSelectedPatientName.Focus();
                        result = false;
                    }
                    else
                        txtSelectedPatientName.ClearValidationError();

                    if (txtSelectedMrNO.Text.Trim() == string.Empty)
                    {
                        txtSelectedMrNO.SetValidation("Please Select Recepient ");
                        txtSelectedMrNO.RaiseValidationError();
                        txtSelectedMrNO.Focus();
                        result = false;
                    }
                    else
                        txtSelectedMrNO.ClearValidationError();
                }
            }
            else if (IsEmbList)
            {
                var item = EmbryoList.Where(x => x.SelectedPlanId.ID == 2).ToList();
                var item1 = EmbryoList.All(x => x.SelectedPlanId.ID == 0);
                if (item1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Please Select Embryo.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    result = false;
                }
                else if (item.Count > 0)
                {
                    if (txtSelectedPatientName.Text.Trim() == string.Empty)
                    {
                        txtSelectedPatientName.SetValidation("Please Select Recepient ");
                        txtSelectedPatientName.RaiseValidationError();
                        txtSelectedPatientName.Focus();
                        result = false;
                    }
                    else
                        txtSelectedPatientName.ClearValidationError();

                    if (txtSelectedMrNO.Text.Trim() == string.Empty)
                    {
                        txtSelectedMrNO.SetValidation("Please Select Recepient ");
                        txtSelectedMrNO.RaiseValidationError();
                        txtSelectedMrNO.Focus();
                        result = false;
                    }
                    else
                        txtSelectedMrNO.ClearValidationError();
                }
            }

            return result;
        }

        private void cmdSaveOocyte_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateSaving())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsOocyteList)
                {
                    cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank BizAction = new cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank();
                    BizAction.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
                    BizAction.Vitrification.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
                    BizAction.IsOocyte = true;
                    //BizAction.VitrificationDetails = new clsIVFDashBoard_VitrificationDetailsVO();
                    //BizAction.VitrificationDetails = (clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem;
                    //BizAction.VitrificationDetails.PlanId = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).SelectedPlanId.ID;
                    //BizAction.VitrificationDetails.DonorPatientID = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).PatientID;
                    //BizAction.VitrificationDetails.DonorPatientUnitID = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).UnitID;
                    //BizAction.VitrificationDetails.RecepientPatientID = SelectedPatient.PatientID;
                    //BizAction.VitrificationDetails.RecepientPatientUnitID = SelectedPatient.UnitId;

                    for (int i = 0; i < OocyteList.Count; i++)
                    {
                        if (OocyteList[i].IsDiscard == true)
                            OocyteList[i].PlanId = 1;
                        //OocyteList[i].PlanId = OocyteList[i].SelectedPlanId.ID;
                        OocyteList[i].DonorPatientID = OocyteList[i].PatientID;
                        OocyteList[i].DonorPatientUnitID = OocyteList[i].UnitID;
                        if (OocyteList[i].PlanId == 2)
                        {
                            OocyteList[i].RecepientPatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                            OocyteList[i].RecepientPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;

                            //OocyteList[i].RecepientPatientID = SelectedPatient.PatientID;
                            //OocyteList[i].RecepientPatientUnitID = SelectedPatient.UnitId;
                        }
                    }
                    BizAction.Vitrification.VitrificationDetailsList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)OocyteList.ToList());


                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                else if (IsEmbList)
                {
                    cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank BizAction = new cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank();
                    BizAction.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
                    BizAction.Vitrification.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
                    BizAction.IsEmb = true;
                    //BizAction.VitrificationDetails = new clsIVFDashBoard_VitrificationDetailsVO();
                    //BizAction.VitrificationDetails = (clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem;
                    //BizAction.VitrificationDetails.PlanId = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).SelectedPlanId.ID;
                    //BizAction.VitrificationDetails.DonorPatientID = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).PatientID;
                    //BizAction.VitrificationDetails.DonorPatientUnitID = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem).UnitID;
                    //BizAction.VitrificationDetails.RecepientPatientID = SelectedPatient.PatientID;
                    //BizAction.VitrificationDetails.RecepientPatientUnitID = SelectedPatient.UnitId;

                    for (int i = 0; i < EmbryoList.Count; i++)
                    {

                        if (EmbryoList[i].IsDiscard == true)
                            EmbryoList[i].PlanId = 1;
                        //EmbryoList[i].PlanId = EmbryoList[i].SelectedPlanId.ID;
                        EmbryoList[i].DonorPatientID = EmbryoList[i].PatientID;
                        EmbryoList[i].DonorPatientUnitID = EmbryoList[i].UnitID;
                        if (EmbryoList[i].PlanId == 2)
                        {
                            EmbryoList[i].RecepientPatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                            EmbryoList[i].RecepientPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                            //EmbryoList[i].RecepientPatientID = SelectedPatient.PatientID;
                            //EmbryoList[i].RecepientPatientUnitID = SelectedPatient.UnitId;
                        }
                    }
                    BizAction.Vitrification.VitrificationDetailsList = ((List<clsIVFDashBoard_VitrificationDetailsVO>)EmbryoList.ToList());


                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                            msgW1.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
        }

        private void CmdCloseOocyte_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DonateDiscard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillPlanList();
            if (IsOocyteList)
            {
                OocyteGrid.Visibility = Visibility.Visible;
                ConTitle.Content = "Oocyte Details";
                FillOocyteCryoBank();
            }
            else if (IsEmbList)
            {
                EmbGrid.Visibility = Visibility.Visible;
                ConTitle.Content = "Embryo Details";
                FillEmbryoCryoBank();
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            clsIVFDashBoard_VitrificationDetailsVO obj;
            CheckBox chk = sender as CheckBox;
            for (int i = 0; i < OocyteList.Count; i++)
            {
                if (i == OocyteDataGrid.SelectedIndex)
                {
                    obj = ((clsIVFDashBoard_VitrificationDetailsVO)OocyteDataGrid.SelectedItem);
                    if (chk.IsChecked == true)
                    {
                        obj.IsDiscard = true;
                        obj.SelectedPlanId.ID = 1;
                    }
                    else if (chk.IsChecked == false)
                    {
                        obj.IsDiscard = false;
                        obj.SelectedPlanId.ID = 0;
                    }
                }

            }
        }

        private void CheckBoxEmb_Click(object sender, RoutedEventArgs e)
        {
            clsIVFDashBoard_VitrificationDetailsVO obj;
            CheckBox chk = sender as CheckBox;
            for (int i = 0; i < EmbryoList.Count; i++)
            {
                if (i == EmbryoDataGrid.SelectedIndex)
                {
                    obj = ((clsIVFDashBoard_VitrificationDetailsVO)EmbryoDataGrid.SelectedItem);
                    if (chk.IsChecked == true)
                    {
                        obj.IsDiscard = true;
                        obj.SelectedPlanId.ID = 1;
                    }
                    else if (chk.IsChecked == false)
                    {
                        obj.IsDiscard = false;
                        obj.SelectedPlanId.ID = 0;
                    }
                }

            }
        }
    }
}

