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
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.Collections;
using System.Reflection;
using System.ComponentModel;
using PalashDynamics.UserControls;
using System.Windows.Media.Imaging;
using PalashDynamics.Collections;
using System.Reflection;
using System.IO;
using System.Globalization;


namespace PalashDynamics.Administration
{
    public partial class StaffMaster : UserControl
    {

        #region Validation

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        //rohini for date validation
        private void SetDateValidation()
        {
            dtpDOB1.DisplayDate = DateTime.Now.Date;
            dtpDOB1.DisplayDateEnd = DateTime.Now.Date;

            dtpDOB1.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(1), DateTime.MaxValue));
        }
        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {



            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                txtFirstName.ClearValidationError();
            }
            //if (string.IsNullOrEmpty(txtFirstName.Text))
            //{
            //    txtFirstName.SetValidation("Please Enter First Name");
            //    txtFirstName.RaiseValidationError();
            //    txtFirstName.Focus();
            //}
            //else {
            //    txtFirstName.Text = txtFirstName.Text.ToTitleCase();
            //    txtFirstName.ClearValidationError();
            //}
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();

        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();

            if (!string.IsNullOrEmpty(txtLastName.Text))
            {
                txtLastName.ClearValidationError();
            }

            //if (string.IsNullOrEmpty(txtLastName.Text))
            //{
            //    txtLastName.SetValidation("Please Enter Last Name");
            //    txtLastName.RaiseValidationError();
            //    txtLastName.Focus();
            //}
            //else 
            //{
            //    txtLastName.Text = txtLastName.Text.ToTitleCase();
            //    txtLastName.ClearValidationError();
            //}
        }

        private void dtpDOB1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((clsStaffMasterVO)grdstaff.DataContext).DOB == null)
            {
                dtpDOB1.SetValidation("Date Birth is required");
                dtpDOB1.RaiseValidationError();
            }
            else if (((clsStaffMasterVO)grdstaff.DataContext).DOB.Value.Date >= DateTime.Now.Date)
            {
                dtpDOB1.SetValidation("Date of Birth should be less than Todays Date");
                dtpDOB1.RaiseValidationError();
            }
            else
                dtpDOB1.ClearValidationError();

        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;


            if (e.Key == Key.Enter)
            {
                FetchData();
            }


        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }

        }

        private void txtFirstName1_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName1.Text = txtFirstName1.Text.ToTitleCase();
        }

        private void txtLastName2_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName2.Text = txtLastName2.Text.ToTitleCase();
        }

        private void txtEmailID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmailID.Text))
            {
                if (Extensions.IsEmailValid(txtEmailID.Text))
                {
                    txtEmailID.ClearValidationError();
                }
                else
                {
                    txtEmailID.SetValidation("Please Enter Valid Email Address");
                    txtEmailID.RaiseValidationError();
                    txtEmailID.Focus();
                }
            }
            else
            {
                txtEmailID.ClearValidationError();
            }
        }

        private void cmbDesignation1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbDesignation1.SelectedItem != null && ((MasterListItem)cmbDesignation1.SelectedItem).ID > 0)
            {
                cmbDesignation1.TextBox.ClearValidationError();
            }
        }

        private bool CheckValidations()
        {
            bool ErrorStatus = true;

            if (!string.IsNullOrEmpty(txtEmailID.Text))
            {
                if (Extensions.IsEmailValid(txtEmailID.Text))
                {
                    txtEmailID.ClearValidationError();
                }
                else
                {
                    txtEmailID.SetValidation("Invalid EmailId");
                    txtEmailID.RaiseValidationError();
                    txtEmailID.Focus();
                    ErrorStatus = false;
                }
            }


            if ((MasterListItem)cmbDesignation1.SelectedItem == null)
            {
                cmbDesignation1.TextBox.SetValidation("Please Select Designation");
                cmbDesignation1.TextBox.RaiseValidationError();
                cmbDesignation1.Focus();
                ErrorStatus = false;
            }

            else
            {
                if (cmbDesignation1.SelectedItem != null)
                {
                    if (((MasterListItem)cmbDesignation1.SelectedItem).ID == 0)
                    {
                        cmbDesignation1.TextBox.SetValidation("Please Select Designation");
                        cmbDesignation1.TextBox.RaiseValidationError();
                        cmbDesignation1.Focus();
                        ErrorStatus = false;
                    }
                }
            }

            if (((clsStaffMasterVO)grdstaff.DataContext).DOB == null)
            {
                dtpDOB1.SetValidation("Please Enter Date of Birth");
                dtpDOB1.RaiseValidationError();
                dtpDOB1.Focus();
                ErrorStatus = false;
            }
            //if (dtpDOB1.SelectedDate != null)            
            else
            {
                if (dtpDOB1.SelectedDate.Value.Date >= DateTime.Now.Date)
                {
                    dtpDOB1.SetValidation("Date of Birth should be less than Todays Date");
                    dtpDOB1.RaiseValidationError();
                    dtpDOB1.Focus();
                    ErrorStatus = false;
                }
                else
                    dtpDOB1.ClearValidationError();
            }

            if (((clsStaffMasterVO)grdstaff.DataContext).DateofJoining == null)
            {
                dtpDOJ.SetValidation("Please Select Joining Date ");
                dtpDOJ.RaiseValidationError();
                dtpDOJ.Focus();
                ErrorStatus = false;
            }

            else
                dtpDOJ.ClearValidationError();
            //rohini
            if (((clsStaffMasterVO)grdstaff.DataContext).DateofJoining != null && ((clsStaffMasterVO)grdstaff.DataContext).DOB != null)
            {
                if (dtpDOB1.SelectedDate == dtpDOJ.SelectedDate || dtpDOB1.SelectedDate > dtpDOJ.SelectedDate)
                {
                    dtpDOJ.SetValidation("Date of Joining should be greater than Date of Birth");
                    dtpDOJ.RaiseValidationError();
                    dtpDOJ.Focus();
                    ErrorStatus = false;
                }
                else
                    dtpDOJ.ClearValidationError();
            }
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                txtLastName.SetValidation("Please Enter Last Name");
                txtLastName.RaiseValidationError();
                txtLastName.Focus();
                ErrorStatus = false;
            }
            else
                txtLastName.ClearValidationError();

            if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                txtFirstName.SetValidation("Please Enter First Name");
                txtFirstName.RaiseValidationError();
                txtFirstName.Focus();
                ErrorStatus = false;
            }
            else
                txtFirstName.ClearValidationError();

            if (ErrorStatus == true)
            {
                cmbDesignation1.TextBox.ClearValidationError();
                dtpDOB1.ClearValidationError();
                txtFirstName.ClearValidationError();
                txtLastName.ClearValidationError();
                txtEmailID.ClearValidationError();
                ErrorStatus = true;
            }
            //Added by AJ Date 10/11/2016
            if ((ChkDischargeApprove).IsChecked == true)
            {
            if ((MasterListItem)cmbDepartment.SelectedItem == null)
            {
                cmbDepartment.TextBox.SetValidation("Please Select Department");
                cmbDepartment.TextBox.RaiseValidationError();
                cmbDepartment.Focus();
                ErrorStatus = false;              
            }
            else if (((MasterListItem)cmbDepartment.SelectedItem).ID == 0)
            {
                cmbDepartment.TextBox.SetValidation("Please Select Department");
                cmbDepartment.TextBox.RaiseValidationError();
                cmbDepartment.Focus();
                ErrorStatus = false;               

            }
            else
                cmbDepartment.TextBox.ClearValidationError();
            }
            //***//-----------------------------
            return ErrorStatus;
        }

        #endregion

        #region Variable Declaration
        private long IUnitId { get; set; }
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        clsStaffMasterVO objStaffMaster = new clsStaffMasterVO();
        //public clsGetPatientBizActionVO OBJPatient { get; set; }
        private SwivelAnimation objAnimation;
        bool IsCancel = true;
        bool isSelectedDID = false;
        public PagedSortableCollectionView<clsStaffMasterVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsStaffBankInfoVO> StaffBankDataList { get; private set; }
        public PagedSortableCollectionView<clsStaffAddressInfoVO> StaffAddressDataList { get; private set; }

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
        public int StaffBankDataListPageSize
        {
            get
            {
                return StaffBankDataList.PageSize;
            }
            set
            {
                if (value == StaffBankDataList.PageSize) return;
                StaffBankDataList.PageSize = value;
            }
        }
        public int DoctorAddressDataListPageSize
        {
            get
            {
                return StaffAddressDataList.PageSize;
            }
            set
            {
                if (value == StaffAddressDataList.PageSize) return;
                StaffAddressDataList.PageSize = value;
            }
        }
        #endregion



        #region Constructor

        public StaffMaster()
        {
            InitializeComponent();

            //Paging
            DataList = new PagedSortableCollectionView<clsStaffMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            this.DataPager.DataContext = DataList;
            this.dgStaffList.DataContext = DataList;

            FetchData();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //  Indicatior = new WaitIndicator();
            //   Indicatior.Show();
            this.DataContext = new clsStaffMasterVO();

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                cmbFindClinic.IsEnabled = true;
            }
            else
            {
                cmbFindClinic.IsEnabled = false;
            }


            StaffAddressDataList = new PagedSortableCollectionView<clsStaffAddressInfoVO>();
            StaffAddressDataList.OnRefresh += new EventHandler<RefreshEventArgs>(StaffAddressDataList_OnRefresh);
            DoctorAddressDataListPageSize = 3;
            this.dgAddressDetails.DataContext = StaffAddressDataList;
            this.dgDataPager2.DataContext = StaffAddressDataList;

            StaffBankDataList = new PagedSortableCollectionView<clsStaffBankInfoVO>();
            StaffBankDataList.OnRefresh += new EventHandler<RefreshEventArgs>(StaffBankDataList_OnRefresh);
            StaffBankDataListPageSize = 3;

            this.dgbankDetails.DataContext = StaffBankDataList;
            this.dgDataPager1.DataContext = StaffBankDataList;


            // SetCommandButtonState("Load");            
            // FillDesignation();
            // FillDesignationList();
            // FillUnit();
            // FillGender();
            // FillMaritalStatus();
            // FillAddressType();
            // FillBankDetail();
            // FetchData();

            //Indicatior.Close();          


        }

        #endregion

        #region OnRefresh Event

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }
        void StaffBankDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchBankData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
        }

        void StaffAddressDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchStaffAddressData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
        }

        #endregion

        #region Loaded Event
        private void StaffMaster_Loaded(object sender, RoutedEventArgs e)
        {
            Indicatior = new WaitIndicator();
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.DataContext = new clsStaffMasterVO();
                SetCommandButtonState("Load");
                CheckValidations();
                FetchData();

                FillDesignation();
                FillDesignationList();
                FillUnit();
                FillGender();
                FillMaritalStatus();
                FillAddressType();
                FillBankDetail();
                txtFirstName1.Focus();
                txtFirstName.Focus();
                Indicatior.Close();

            }
            IsPageLoded = true;
            txtFirstName.Focus();
            txtFirstName.UpdateLayout();
            txtFirstName1.Focus();
            txtFirstName1.UpdateLayout();

        }
        #endregion

        #region FillCombobox

        private void FillDesignation()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DesignationMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbDesignation.ItemsSource = null;
                    cmbDesignation.ItemsSource = objList;
                    MasterListItem MasterItem = ((List<MasterListItem>)cmbDesignation.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    cmbDesignation.SelectedItem = MasterItem;


                }

                //if (this.DataContext != null)
                //{
                //    cmbDesignation.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
                //}



            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillDesignationList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DesignationMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbDesignation1.ItemsSource = null;
                    cmbDesignation1.ItemsSource = objList;



                }

                #region Commented by shikha
                if (this.DataContext != null)
                {
                    cmbDesignation1.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
                }
                #endregion

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillUnit()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    cmbFindClinic.ItemsSource = null;
                    cmbFindClinic.ItemsSource = objList;
                    MasterListItem MasterItem = ((List<MasterListItem>)cmbFindClinic.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    cmbFindClinic.SelectedItem = MasterItem;

                }
                #region Commented by Shikha
                if (this.DataContext != null)
                {
                    cmbClinic.SelectedValue = ((clsStaffMasterVO)this.DataContext).UnitID;
                }


                //if ((clsStaffMasterVO)dgStaffList.SelectedItem != null)
                //{
                //    cmbClinic.SelectedValue = objStaffMaster.UnitID;
                //    cmbClinic.UpdateLayout();

                //}
                #endregion
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
        clsStaffBankInfoVO objStaffBankInfo = new clsStaffBankInfoVO();
        private void FillBankDetail()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_BankMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbBankName.ItemsSource = null;
                    cmbBankName.ItemsSource = objList;

                }


                if (this.DataContext != null)
                {
                    cmbBankName.SelectedValue = ((clsStaffMasterVO)this.DataContext).BankId;
                }

                if ((clsStaffBankInfoVO)dgbankDetails.SelectedItem != null)
                {
                    cmbBankName.SelectedValue = objStaffBankInfo.BankId;
                    cmbBranchName.UpdateLayout();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;


                }
                #region Commented by shikha
                if (this.DataContext != null)
                {
                    cmbGender.SelectedValue = ((clsStaffMasterVO)this.DataContext).GenderID;
                }


                //if ((clsStaffMasterVO)dgStaffList.SelectedItem != null)
                //{
                //    cmbGender.SelectedValue = objStaffMaster.GenderID;
                //    cmbGender.UpdateLayout();

                //}
                #endregion
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }



        #endregion

        #region Commented by shikha
        //private void SetComboboxValue()
        //{
        //    cmbGender.SelectedValue = ((clsStaffMasterVO)this.DataContext).GenderID;
        //    cmbDesignation.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
        //    cmbDesignation1.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
        //    cmbClinic.SelectedValue = ((clsStaffMasterVO)this.DataContext).UnitID;

        //}
        #endregion

        #region Click Event(Save/Modify/View)

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            // this.SetCommandButtonState("New");
            //// tbStaffInformation.SelectedItem = tbIGeneralInfo;


            // CheckValidations();
            // UserControl rootPage = Application.Current.RootVisual as UserControl;
            // TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            // mElement.Text = " : " + "New Staff Details";
            //  ClearControl();
            //      txtFirstName.Focus();
            // txtFirstName.UpdateLayout();
            //objAnimation.Invoke(RotationType.Forward);
            
            //AJ Date 12/11/2016
            cmbDepartment.Visibility = Visibility.Collapsed;
            lblDepartment.Visibility = Visibility.Collapsed;
            ChkDischargeApprove.IsChecked = false;
            objStaff.IsDischargeApprove = false;
            FillDepartmentList(((MasterListItem)cmbClinic.SelectedItem).ID);
            //***/-------------
            this.SetCommandButtonState("New");
            tbStaffInformation.SelectedIndex = 0;
          
            ClearControl();
            txtFirstName.Focus();
            this.DataContext = new clsStaffMasterVO();
            clsStaffMasterVO DVO = new clsStaffMasterVO();


            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Employee Details";
            SetDateValidation();
            // CheckValidations();
            objAnimation.Invoke(RotationType.Forward);
            SaveBank.IsEnabled = true;
            ModifyBank.IsEnabled = false;
            //DateTime dt = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //dtpDOJ.DisplayDateStart = dt;
            //dtpDOJ.SelectedDate = DateTime.Today;
            dtpDOJ.DisplayDate = DateTime.Today;
            dtpDOB1.DisplayDate = DateTime.Today;

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //SetCommandButtonState("Cancel");
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            //mElement.Text = "";

            //objAnimation.Invoke(RotationType.Backward);

            //if (IsCancel == true)
            //{
            //    mElement = (TextBlock)rootPage.FindName("SampleHeader");
            //    mElement.Text = "Clinic Configuration";

            //    UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmClinicConfiguration") as UIElement;
            //    ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            //}
            //else
            //{
            //    IsCancel = true;
            //}



            SetCommandButtonState("Cancel");
            this.DataContext = null;
            //StaffSignature = new byte[0];           
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Clinic Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmClinicConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                FetchData();
            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveStaffDetails = true;
            SaveStaffDetails = CheckValidations();


            if (SaveStaffDetails == true)
            {

                string msgTitle = "";
                string msgText = "Are You Sure You Want To Save The Employee Master?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
                FetchData();

            }
            else
            {

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Save The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }



        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveStaffMaster();

            }


        }

        void msgW11_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //tbIBankInfo.Visibility = Visibility.Visible;
                //tbIAddressInfo.Visibility = Visibility.Visible;
                //  FetchData();
                SetCommandButtonState("View");
                FillData(DID);
                isSelectedDID = false;
                //CheckBankValidation();
                // CheckAddressValidation();
                //  SaveDoctorMaster();
            }
            else
            {
                //SaveDoctorMaster();
                FetchData();
                ClearControl();
                objAnimation.Invoke(RotationType.Backward);
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Employee Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                SetCommandButtonState("Load");
                msgW1.Show();
                isSelectedDID = false;
            }


        }
        private void SaveStaffMaster()
        {
            clsAddStaffMasterBizActionVO BizAction = new clsAddStaffMasterBizActionVO();
            BizAction.StaffDetails = (clsStaffMasterVO)grdstaff.DataContext;

            BizAction.StaffDetails.FirstName = txtFirstName.Text;
            BizAction.StaffDetails.MiddleName = txtMiddleName.Text;
            BizAction.StaffDetails.LastName = txtLastName.Text;
            BizAction.StaffDetails.DOB = dtpDOB1.SelectedDate;
            BizAction.StaffDetails.EmailId = txtEmailID.Text.Trim();

            BizAction.StaffDetails.PANNumber = txtPAN.Text.Trim();
            BizAction.StaffDetails.PFNumber = txtprovidentFound.Text.Trim();

            BizAction.StaffDetails.AccessCardNumber = txtACN.Text.Trim();
            BizAction.StaffDetails.DateofJoining = dtpDOJ.SelectedDate;
            BizAction.StaffDetails.EmployeeNumber = txtEmployeeNumber.Text.Trim();

            //
            //WriteableBitmap bmp1 = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);
            //bmp1.Render(SignatuerImage, new MatrixTransform());
            //bmp1.Invalidate();

            //int[] p1 = bmp1.Pixels;
            //int len1 = p1.Length * 4;
            //byte[] result1 = new byte[len1];
            //Buffer.BlockCopy(p1, 0, result1, 0, len1);
            //BizAction.StaffDetails.Photo = result1;
            //
            //if (StaffSignature != null)
            //{
            //    BizAction.StaffDetails.Photo = StaffSignature;
            //}
            //else
            //{
            //    BizAction.StaffDetails.Photo = null;
            //}
            if (((clsStaffMasterVO)this.DataContext).Photo != null)
                    BizAction.StaffDetails.Photo = ((clsStaffMasterVO)this.DataContext).Photo;
            BizAction.StaffDetails.Experience = txtExperience.Text;
            BizAction.StaffDetails.Education = txtEducation.Text;

            if (cmbDesignation1.SelectedItem != null)
                BizAction.StaffDetails.DesignationID = ((MasterListItem)cmbDesignation1.SelectedItem).ID;
            if (cmbGender.SelectedItem != null)
                BizAction.StaffDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

            if (cmbClinic.SelectedItem != null)
                BizAction.StaffDetails.ClinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
            if (cmbMaritalStatus.SelectedItem != null)
                BizAction.StaffDetails.MaritalStatusId = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;
            //Added by AJ Date 10/11/2016
            if ((ChkDischargeApprove).IsChecked == true)
            {
                BizAction.StaffDetails.IsDischargeApprove = true;
                BizAction.StaffDetails.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            }
            //***//-------------------------------

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("Save");

                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddStaffMasterBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        FetchData();
                        Indicatior.Close();

                        clsAddStaffMasterBizActionVO result = arg.Result as clsAddStaffMasterBizActionVO;

                        if (result.StaffDetails != null)
                        {
                            DID = result.StaffDetails.ID;
                        }

                        string msgTitle = "Palash";
                        string msgText = "Are You Want To Save The Employee Bank Details And Employee Address Details?";

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW11_OnMessageBoxClosed);

                        msgW1.Show();


                    }
                    else if (((clsAddStaffMasterBizActionVO)arg.Result).SuccessStatus == 1)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Can Not Be Saved Because Employee Details Already Exist.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        Indicatior.Close();
                        SetCommandButtonState("New");

                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Adding Employee Master.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    SetCommandButtonState("New");
                    Indicatior.Close();
                }
                isSelectedDID = false;


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyStaffDetails = true;
            ModifyStaffDetails = CheckValidations();
            if (ModifyStaffDetails == true)
            {
                string msgTitle = "";
                string msgText = "Are You Sure You Want To Update The Employee Master?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();

            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                ModifyStaffMaster();
        }

        private void ModifyStaffMaster()
        {
            clsAddStaffMasterBizActionVO BizAction = new clsAddStaffMasterBizActionVO();
            BizAction.StaffDetails = new clsStaffMasterVO();

            if (dgStaffList.SelectedItem != null && isSelectedDID == true)
            {

                BizAction.StaffDetails.ID = ((clsStaffMasterVO)dgStaffList.SelectedItem).ID;
            }
            else if (DID != null)
                BizAction.StaffDetails.ID = DID;
            BizAction.StaffDetails.FirstName = txtFirstName.Text;
            BizAction.StaffDetails.MiddleName = txtMiddleName.Text;
            BizAction.StaffDetails.LastName = txtLastName.Text;
            BizAction.StaffDetails.DOB = dtpDOB1.SelectedDate;
            BizAction.StaffDetails.EmailId = txtEmailID.Text.Trim();

            BizAction.StaffDetails.PANNumber = txtPAN.Text.Trim();
            BizAction.StaffDetails.PFNumber = txtprovidentFound.Text.Trim();

            BizAction.StaffDetails.AccessCardNumber = txtACN.Text.Trim();
            BizAction.StaffDetails.DateofJoining = dtpDOJ.SelectedDate;
            BizAction.StaffDetails.EmployeeNumber = txtEmployeeNumber.Text.Trim();

            //WriteableBitmap bmp = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);
            //bmp.Render(SignatuerImage, new MatrixTransform());
            //bmp.Invalidate();

            //int[] p = bmp.Pixels;
            //int len = p.Length * 4;
            //byte[] result = new byte[len]; // ARGB
            //Buffer.BlockCopy(p, 0, result, 0, len);
            //BizAction.StaffDetails.Photo = result;
            //if (StaffSignature != null)
            //{
            //    BizAction.StaffDetails.Photo = StaffSignature;
            //}
            //else
            //{
            //    BizAction.StaffDetails.Photo = null;

            //}
            if (((clsStaffMasterVO)this.DataContext).Photo != null)
                BizAction.StaffDetails.Photo = ((clsStaffMasterVO)this.DataContext).Photo;

            BizAction.StaffDetails.Experience = txtExperience.Text;
            BizAction.StaffDetails.Education = txtEducation.Text;

            if (cmbDesignation1.SelectedItem != null)
                BizAction.StaffDetails.DesignationID = ((MasterListItem)cmbDesignation1.SelectedItem).ID;
            if (cmbGender.SelectedItem != null)
                BizAction.StaffDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

            if (cmbClinic.SelectedItem != null)
                BizAction.StaffDetails.ClinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
            if (cmbMaritalStatus.SelectedItem != null)
                BizAction.StaffDetails.MaritalStatusId = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;

            //Added by AJ Date 10/11/2016
            if ((ChkDischargeApprove).IsChecked == true)
            {
                BizAction.StaffDetails.IsDischargeApprove = true;
                BizAction.StaffDetails.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            }
            //***//-------------------------------

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {


                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddStaffMasterBizActionVO)arg.Result).SuccessStatus == 0)
                    {

                        FetchData();
                        objAnimation.Invoke(RotationType.Backward);
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Employee Master Updated Sucessfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        isSelectedDID = false;
                        SetCommandButtonState("Load");
                        msgW1.Show();
                    }
                    else if (((clsAddStaffMasterBizActionVO)arg.Result).SuccessStatus == 1)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Record can not be Modify because Employee Details already Exist.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        Indicatior.Close();
                        SetCommandButtonState("View");


                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Updating Employee Master.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    Indicatior.Close();
                    SetCommandButtonState("View");
                }

                //  SetCommandButtonState("Modify");

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }


        clsStaffMasterVO objStaff = new clsStaffMasterVO();

        private void FillData(long iStaffID)
        {
            //  DID = iStaffID;
            clsGetStaffMasterDetailsByIDBizActionVO BizAction = new clsGetStaffMasterDetailsByIDBizActionVO();
            BizAction.StaffId = iStaffID;
            cmbDepartment.Visibility = Visibility.Visible;
            lblDepartment.Visibility = Visibility.Visible;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    SetCommandButtonState("View");
                    if (dgStaffList.SelectedItem != null)
                        objAnimation.Invoke(RotationType.Forward);

                    objStaff = ((clsGetStaffMasterDetailsByIDBizActionVO)ea.Result).StaffMasterList;
                    //if (objStaff.Photo != null)
                    //{
                    //    byte[] imageBytes1 = objStaff.Photo;

                    //    //BitmapImage img = new BitmapImage();
                    //    //img.SetSource(new MemoryStream(imageBytes, false));
                    //    //HeaderImage.Source = img;
                    //    WriteableBitmap bmp1 = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                    //    bmp1.FromByteArray(imageBytes1);
                    //    SignatuerImage.Source = bmp1;
                    //}
                    //else
                    //{
                    //    SignatuerImage.Source = null;
                    //}

                    //rohini
                    if (objStaff.Photo != null)
                    {
                        byte[] imageBytes = objStaff.Photo;
                        BitmapImage img = new BitmapImage();
                        img.SetSource(new MemoryStream(imageBytes, false));
                        SignatuerImage.Source = img;


                        //WriteableBitmap bmp1 = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                        //bmp1.SetSource(new MemoryStream(objStaff.Photo, false));                  
                        //SignatuerImage.Source = bmp1;
                    }
                    else
                    {
                        SignatuerImage.Source = null;
                    }
                    //

                    //if (objStaff.Photo != null)
                    //{
                    //    byte[] imageBytes = objStaff.Photo;
                    //    BitmapImage img = new BitmapImage();
                    //    img.SetSource(new MemoryStream(imageBytes, false));
                    //    SignatuerImage.Source = img;
                    //}
                    //else
                    //{
                    //    SignatuerImage.Source = null;
                    //}

                    if (objStaff.DateofJoining != null)
                        dtpDOJ.SelectedDate = objStaff.DateofJoining;

                    if (objStaff.DOB != null)
                        dtpDOB1.SelectedDate = objStaff.DOB;

                    if (objStaff.MaritalStatusId != null)
                    {
                        cmbMaritalStatus.SelectedValue = objStaff.MaritalStatusId;
                    }


                    this.DataContext = objStaff;
                    FillGender();
                    if (dgStaffList.SelectedItem != null && isSelectedDID == true)
                    {
                        BankDetailPaging(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
                        StaffAddressDetailsPaging(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
                    }
                    else if (DID != null)
                    {
                        BankDetailPaging(DID);
                        StaffAddressDetailsPaging(DID);
                    }

                    if (objStaff.AccessCardNumber != "" && objStaff.AccessCardNumber != null)
                    {
                        txtACN.Text = objStaff.AccessCardNumber;
                    }
                    if (objStaff.PANNumber != "" && objStaff.PANNumber != null)
                    {
                        txtPAN.Text = objStaff.PANNumber;
                    }
                    if (objStaff.PFNumber != "" && objStaff.PFNumber != null)
                    {
                        txtprovidentFound.Text = objStaff.PFNumber;
                    }

                    if (objStaff.FirstName != "" && objStaff.FirstName != null)
                    {
                        txtFirstName.Text = objStaff.FirstName;
                    }
                    if (objStaff.MiddleName != "" && objStaff.MiddleName != null)
                    {
                        txtMiddleName.Text = objStaff.MiddleName;
                    }
                    if (objStaff.LastName != "" && objStaff.LastName != null)
                    {
                        txtLastName.Text = objStaff.LastName;
                    }

                    if (objStaff.DesignationID != null)
                    {
                        cmbDesignation1.SelectedValue = objStaff.DesignationID;
                    }
                    if (objStaff.ClinicId != null)
                    {
                        cmbClinic.SelectedValue = objStaff.ClinicId;
                    }

                    if (objStaff.PFNumber != "" && objStaff.PFNumber != null)
                    {
                        txtprovidentFound.Text = objStaff.PFNumber;
                    }
                    if (objStaff.PANNumber != "" && objStaff.PANNumber != null)
                    {
                        txtPAN.Text = objStaff.PANNumber;
                    }
                    if (objStaff.AccessCardNumber != "" && objStaff.AccessCardNumber != null)
                    {
                        txtACN.Text = objStaff.AccessCardNumber;
                    }
                    if (objStaff.EmailId != "" && objStaff.EmailId != null)
                    {
                        txtEmailID.Text = objStaff.EmailId;
                    }

                    //Added by AJ Date 10/11/2016
                    if (objStaff.IsDischargeApprove == true)
                    {
                        ChkDischargeApprove.IsChecked = true;
                        if (objStaff.DepartmentID != null)
                        {
                            FillDepartmentList(((MasterListItem)cmbClinic.SelectedItem).ID);
                        }
                    }
                    else
                    {
                        ChkDischargeApprove.IsChecked = false;
                        cmbDepartment.Visibility = Visibility.Collapsed;
                        lblDepartment.Visibility = Visibility.Collapsed;
                        //FillDepartmentList(((MasterListItem)cmbClinic.SelectedItem).ID);
                    }
                    //***//-------------------
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        void BankDetailPaging(long iStaffID)
        {
            StaffBankDataList = new PagedSortableCollectionView<clsStaffBankInfoVO>();
            StaffBankDataList.OnRefresh += new EventHandler<RefreshEventArgs>(StaffBankDataList_OnRefresh);
            StaffBankDataListPageSize = 3;
            this.dgDataPager1.DataContext = StaffBankDataList;
            this.dgbankDetails.DataContext = StaffBankDataList;
            FetchBankData(iStaffID);
            ClearBankControl();
        }

        void StaffAddressDetailsPaging(long iStaffID)
        {

            StaffAddressDataList = new PagedSortableCollectionView<clsStaffAddressInfoVO>();
            StaffAddressDataList.OnRefresh += new EventHandler<RefreshEventArgs>(StaffAddressDataList_OnRefresh);
            DoctorAddressDataListPageSize = 3;
            this.dgDataPager2.DataContext = StaffAddressDataList;
            this.dgAddressDetails.DataContext = StaffAddressDataList;
            FetchStaffAddressData(iStaffID);
        }

        #endregion

        #region Public Method

        private void FetchData()
        {
            clsGetStaffMasterDetailsBizActionVO BizAction = new clsGetStaffMasterDetailsBizActionVO();
            BizAction.StaffMasterList = new List<clsStaffMasterVO>();
            if (cmbDesignation.SelectedItem != null)
                BizAction.DesignationID = ((MasterListItem)cmbDesignation.SelectedItem).ID;

            if (txtFirstName1.Text != null)
                BizAction.FirstName = txtFirstName1.Text;

            if (txtLastName2.Text != null)
                BizAction.LastName = txtLastName2.Text;

            if (cmbFindClinic.SelectedItem != null && cmbFindClinic.SelectedItem != "-- Select --")
            {
                BizAction.ClinicId = ((MasterListItem)cmbFindClinic.SelectedItem).ID;
            }

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = 0;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    clsGetStaffMasterDetailsBizActionVO result = arg.Result as clsGetStaffMasterDetailsBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;


                    DataList.Clear();
                    foreach (var item in result.StaffMasterList)
                    {
                        DataList.Add(item);
                    }

                    dgStaffList.ItemsSource = null;
                    dgStaffList.ItemsSource = DataList;

                    DataPager.Source = null;
                    DataPager.PageSize = BizAction.MaximumRows;
                    DataPager.Source = DataList;

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

        #endregion


        #region Reset All Controls

        private void ClearControl()
        {
            grdstaff.DataContext = new clsStaffMasterVO();
            //dtpDOB1.SelectedDate = null;
            ((clsStaffMasterVO)grdstaff.DataContext).DOB = null;
            dtpDOB1.SelectedDate = null;
            cmbClinic.SelectedValue = (long)0;
            cmbDesignation1.SelectedValue = (long)0;
            cmbGender.SelectedValue = (long)0;

            //rohinee
            txtEducation.Text = string.Empty;
            txtExperience.Text = string.Empty;

            txtACN.Text = string.Empty;
            txtPAN.Text = string.Empty;
            txtDoctorAddress.Text = string.Empty;
            txtContact1.Text = string.Empty;
            txtContact2.Text = string.Empty;
            txtprovidentFound.Text = string.Empty;
            cmbMaritalStatus.SelectedValue = (long)0;
            cmbAddressType.SelectedValue = (long)0;
            txtAccountNumber.Text = string.Empty;
            txtMICRNumber.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            cmbBankName.SelectedValue = (long)0;
            cmbBranchName.SelectedValue = (long)0;
            RadioSaving.IsChecked = false;
            RadioCurrent.IsChecked = false;
            txtEmployeeNumber.Text = string.Empty;

            dtpDOJ.SelectedDate = null;

            SignatuerImage.Source = null;

            DID = 0;
        }

        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;

                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    tbIBankInfo.Visibility = Visibility.Collapsed;
                    tbIAddressInfo.Visibility = Visibility.Collapsed;
                    cmdCancel.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    SaveBank.IsEnabled = true;
                    ModifyBank.IsEnabled = false;
                    SaveAddress.IsEnabled = true;
                    ModifyAddress.IsEnabled = false;
                    IsCancel = false;


                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "ModifyBankInfo":
                    cmdNew.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    SaveBank.IsEnabled = false;
                    ModifyBank.IsEnabled = true;
                    SaveAddress.IsEnabled = true;
                    ModifyAddress.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "ModifyAddressInfo":
                    cmdNew.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    SaveAddress.IsEnabled = false;
                    ModifyAddress.IsEnabled = true;
                    SaveBank.IsEnabled = false;
                    ModifyBank.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //ClearBankControl();
                    //ClearAddressControl();
                    //ClearControl();
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    tbIBankInfo.Visibility = Visibility.Visible;
                    tbIAddressInfo.Visibility = Visibility.Visible;
                    SaveBank.IsEnabled = true;
                    ModifyBank.IsEnabled = false;
                    SaveAddress.IsEnabled = true;
                    ModifyAddress.IsEnabled = false;
                    IsCancel = false;
                    break;

                case "ViewBankInfo":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    SaveBank.IsEnabled = false;
                    ModifyBank.IsEnabled = true;
                    SaveAddress.IsEnabled = true;
                    ModifyAddress.IsEnabled = false;
                    break;
                case "ViewAddressInfo":

                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    SaveAddress.IsEnabled = false;
                    ModifyAddress.IsEnabled = true;
                    SaveBank.IsEnabled = true;
                    ModifyBank.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        //byte[] StaffSignature;
        //string StaffSignature = string.Empty;
        byte[] data;
        FileInfo fi;
        private void btnAddSignature_Click(object sender, RoutedEventArgs e)
        {

            //try
            //{
            //    OpenFileDialog openDialog1 = new OpenFileDialog();
            //    openDialog1.Multiselect = false;
            //    openDialog1.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            //    if (openDialog1.ShowDialog() == true)
            //    {
            //        //AttachedFileName = openDialog.File.Name;
            //        //txtFileName.Text = openDialog.File.Name;
            //        try
            //        {
            //            //using (Stream stream = openDialog.File.OpenRead())
            //            //{
            //            BitmapImage imageSource1 = new BitmapImage();
            //            imageSource1.SetSource(openDialog1.File.OpenRead());
            //            SignatuerImage.Source = null;
            //            SignatuerImage.Source = imageSource1;

            //        }
            //        catch (Exception ex)
            //        {
            //            string msgText = "Error while reading file.";

            //            MessageBoxControl.MessageBoxChildWindow msgWindow =
            //                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgWindow.Show();
            //        }
            //    }
            //}

            //catch (Exception)
            //{
            //    throw;
            //}

            OpenFileDialog OpenFile = new OpenFileDialog();

            OpenFile.Multiselect = false;
            OpenFile.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            OpenFile.FilterIndex = 1;
            if (OpenFile.ShowDialog() == true)
            {

                WriteableBitmap imageSource = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);
                try
                {
                    imageSource.SetSource(OpenFile.File.OpenRead());
                    SignatuerImage.Source = imageSource;
                    // MyPhoto = imageToByteArray(OpenFile.File);


                    WriteableBitmap bmp = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);
                    bmp.Render(SignatuerImage, new MatrixTransform());
                    bmp.Invalidate();                  
                    using (Stream stream = OpenFile.File.OpenRead())
                    {
                        data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);
                        fi = OpenFile.File;
                    }

                    //((clsPatientVO)this.DataContext).Photo = result;
                    ((clsStaffMasterVO)this.DataContext).Photo = data;
                }
                catch (Exception)
                {
                    HtmlPage.Window.Alert("Error Loading File");
                }

            }
        }
        private void hlbViewDoctorAddressInfo_Click(object sender, RoutedEventArgs e)
        {
            if (dgStaffList.SelectedItem != null && isSelectedDID == true)
            {
                FillStaffAddressData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
            }
            else if (DID != 0)
            {
                FillStaffAddressData(DID);
            }

            SaveBank.IsEnabled = true;
            ModifyBank.IsEnabled = false;
            SaveAddress.IsEnabled = false;
            ModifyAddress.IsEnabled = true;

        }
        private void hlbViewDoctorBankInfo_Click(object sender, RoutedEventArgs e)
        {
            if (dgStaffList.SelectedItem != null && isSelectedDID == true)
            {
                FillStaffBankData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
            }
            else if (DID != 0)
            {
                FillStaffBankData(DID);
            }
            SaveBank.IsEnabled = false;
            ModifyBank.IsEnabled = true;
            SaveAddress.IsEnabled = true;
            ModifyAddress.IsEnabled = false;


        }
        private void hlbViewStaffMaster_Click(object sender, RoutedEventArgs e)
        {

            SetCommandButtonState("View");
            //rohini  to set selected tab
            if (tbStaffInformation.SelectedItem != tbIGeneralInfo)
            {
                tbStaffInformation.SelectedItem = tbIGeneralInfo;
            } 
            FillData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsStaffMasterVO)dgStaffList.SelectedItem).StaffName;

            SetDateValidation();
            SaveBank.IsEnabled = true;
            ModifyBank.IsEnabled = false;
            SaveAddress.IsEnabled = true;
            ModifyAddress.IsEnabled = false;
            isSelectedDID = true;
            //new

            //SetCommandButtonState("View");
            //Save.IsEnabled = true;
            //SaveAddress.IsEnabled = true;
            //if (dgStaffList.SelectedItem != null)
            //{
            //    grdstaff.DataContext = ((clsStaffMasterVO)dgStaffList.SelectedItem);
            //    cmbGender.SelectedValue = ((clsStaffMasterVO)dgStaffList.SelectedItem).GenderID;
            //    cmbDesignation1.SelectedValue = ((clsStaffMasterVO)dgStaffList.SelectedItem).DesignationID;
            //    cmbClinic.SelectedValue = ((clsStaffMasterVO)dgStaffList.SelectedItem).UnitID;
            //    dtpDOB1.SelectedDate = ((clsStaffMasterVO)dgStaffList.SelectedItem).DOB;
            //  //  dtpDOJ.SelectedDate = ((clsStaffMasterVO)dgStaffList.SelectedItem).DateofJoining;
            //    //cmbMaritalStatus.SelectedValue = ((clsStaffMasterVO)dgStaffList.SelectedItem).MaritalStatusId;
            //                  objAnimation.Invoke(RotationType.Forward);

            //    FillData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
            //}

            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            //mElement.Text = " : " + ((clsStaffMasterVO)dgStaffList.SelectedItem).StaffName;




        }

        private void FillStaffBankData(long iStaffID)
        {
            clsGetStaffBankInfoByIdVO BizAction = new clsGetStaffBankInfoByIdVO();
            BizAction.StaffID = iStaffID;
            BizAction.ID = ((clsStaffBankInfoVO)dgbankDetails.SelectedItem).ID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    SetCommandButtonState("ViewBankInfo");
                    objStaffBankInfo = ((clsGetStaffBankInfoByIdVO)ea.Result).objStaffBankDetail;

                    if (objStaffBankInfo != null)
                    {
                        txtAccountNumber.Text = objStaffBankInfo.AccountNumber;

                    }
                    if (objStaffBankInfo != null)
                    {
                        if (objStaffBankInfo.AccountTypeName == "Saving")
                        {

                            RadioSaving.IsChecked = true;
                        }

                        else
                            RadioCurrent.IsChecked = true;

                    }

                    if (objStaffBankInfo != null)
                    {
                        txtMICRNumber.Text = Convert.ToString(objStaffBankInfo.MICRNumber);

                    }
                    if (objStaffBankInfo != null)
                    {
                        txtAddress.Text = Convert.ToString(objStaffBankInfo.BranchAddress);

                    }


                    FillBankDetail();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        private void cmdDoctorBankInfoSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveStaff = true;

            SaveStaff = CheckBankValidation();

            if (SaveStaff == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure You Want To Save The Employee Bank Details?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);

                msgW.Show();
            }
        }
        void msgW2_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveStaffBankInformation();
            else if (result == MessageBoxResult.No)
            {
                ClearBankControl();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Employee Bank Details adding Cancelled.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
            }

        }

        long DID;
        private void SaveStaffBankInformation()
        {
            Indicatior.Show();
            clsAddStaffBankInfoBizActionVO BizAction = new clsAddStaffBankInfoBizActionVO();
            BizAction.objStaffBankDetail = new clsStaffBankInfoVO();

            if (SaveBank.IsEnabled == false && isSelectedDID == true)  //if (dgStaffList.SelectedItem != null)
            {
                BizAction.objStaffBankDetail.StaffId = ((clsStaffMasterVO)dgStaffList.SelectedItem).ID;
            }
            else if (DID != 0)
                BizAction.objStaffBankDetail.StaffId = DID;

            if (cmbBankName.SelectedItem != null)
                BizAction.objStaffBankDetail.BankId = ((MasterListItem)cmbBankName.SelectedItem).ID;

            if (cmbBranchName.SelectedItem != null)
                BizAction.objStaffBankDetail.BranchId = ((MasterListItem)cmbBranchName.SelectedItem).ID;

            if (txtAccountNumber.Text != null)
                BizAction.objStaffBankDetail.AccountNumber = Convert.ToString(txtAccountNumber.Text);

            if (txtMICRNumber.Text != "")
            {
                BizAction.objStaffBankDetail.MICRNumber = txtMICRNumber.Text;
            }

            if (RadioSaving.IsChecked == true)
            {
                BizAction.objStaffBankDetail.AccountType = true;
            }

            if (RadioCurrent.IsChecked == true)
            {
                BizAction.objStaffBankDetail.AccountType = false;
            }
            if (txtAddress.Text != null)
                BizAction.objStaffBankDetail.BranchAddress = txtAddress.Text;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {


                if (arg.Error == null)
                {
                    if (dgStaffList.SelectedItem != null && isSelectedDID == true)
                    {
                        FetchBankData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
                    }
                    else if (DID != 0)
                    {
                        FetchBankData(DID);
                    }
                    ClearBankControl();

                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Employee Bank Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Save Employee Bank Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }
        private void FetchBankData(long iStaffID)
        {
            clsGetStaffBankInfoBizActionVO BizAction = new clsGetStaffBankInfoBizActionVO();
            BizAction.StaffBankDetailList = new List<clsStaffBankInfoVO>();

            BizAction.StaffID = iStaffID;

            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((PalashDynamics.ValueObjects.Administration.StaffMaster.clsGetStaffBankInfoBizActionVO)arg.Result).StaffBankDetailList != null)
                    {
                        PalashDynamics.ValueObjects.Administration.StaffMaster.clsGetStaffBankInfoBizActionVO result = arg.Result as PalashDynamics.ValueObjects.Administration.StaffMaster.clsGetStaffBankInfoBizActionVO;

                        if (result.StaffBankDetailList != null)
                        {
                            StaffBankDataList.Clear();
                            StaffBankDataList.TotalItemCount = ((PalashDynamics.ValueObjects.Administration.StaffMaster.clsGetStaffBankInfoBizActionVO)arg.Result).TotalRows;
                            foreach (clsStaffBankInfoVO item in result.StaffBankDetailList)
                            {
                                StaffBankDataList.Add(item);

                            }
                        }

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FetchStaffAddressData(long iStaffID)
        {

            clsGetStaffAddressInfoBizActionVO BizAction = new clsGetStaffAddressInfoBizActionVO();
            BizAction.StaffAddressDetailList = new List<clsStaffAddressInfoVO>();
            BizAction.StaffId = iStaffID;
            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = StaffAddressDataList.PageIndex * StaffAddressDataList.PageSize;
            BizAction.MaximumRows = StaffAddressDataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetStaffAddressInfoBizActionVO)arg.Result).StaffAddressDetailList != null)
                    {
                        clsGetStaffAddressInfoBizActionVO result = arg.Result as clsGetStaffAddressInfoBizActionVO;

                        if (result.StaffAddressDetailList != null)
                        {
                            StaffAddressDataList.Clear();
                            StaffAddressDataList.TotalItemCount = ((clsGetStaffAddressInfoBizActionVO)arg.Result).TotalRows;
                            foreach (clsStaffAddressInfoVO item in result.StaffAddressDetailList)
                            {
                                StaffAddressDataList.Add(item);
                            }
                        }

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private bool CheckBankValidation()
        {
            bool result = true;
            if ((MasterListItem)cmbBankName.SelectedItem == null)
            {
                cmbBankName.TextBox.SetValidation("Please Select Bank Name");
                cmbBankName.TextBox.RaiseValidationError();
                cmbBankName.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbBankName.SelectedItem).ID == 0)
            {
                cmbBankName.TextBox.SetValidation("Please Select Bank Name");
                cmbBankName.TextBox.RaiseValidationError();
                cmbBankName.Focus();
                result = false;
            }
            else
                cmbBankName.TextBox.ClearValidationError();


            if ((MasterListItem)cmbBranchName.SelectedItem == null)
            {
                cmbBranchName.TextBox.SetValidation("Please Select Branch Name");
                cmbBranchName.TextBox.RaiseValidationError();
                cmbBranchName.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbBranchName.SelectedItem).ID == 0)
            {
                cmbBranchName.TextBox.SetValidation("Please Select Branch Name");
                cmbBranchName.TextBox.RaiseValidationError();
                cmbBranchName.Focus();
                result = false;
            }
            else
                cmbBranchName.TextBox.ClearValidationError();



            if (txtAccountNumber.Text == "")
            {
                txtAccountNumber.SetValidation("Please Enter Account Number");
                txtAccountNumber.RaiseValidationError();
                txtAccountNumber.Focus();
                result = false;

            }
            else
                txtAccountNumber.ClearValidationError();
            return result;
        }
        private void FillMaritalStatus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_MaritalStatusMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbMaritalStatus.ItemsSource = null;
                    cmbMaritalStatus.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    cmbMaritalStatus.SelectedValue = ((clsStaffMasterVO)this.DataContext).MaritalStatusId;
                }


            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        void FillAddressType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            // BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DoctorAddressMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbAddressType.ItemsSource = null;
                    cmbAddressType.ItemsSource = objList;

                }


                if (this.DataContext != null)
                {
                    cmbAddressType.SelectedValue = ((clsStaffMasterVO)this.DataContext).StaffAddressInformation.AddressTypeID;
                }

                if ((clsStaffMasterVO)dgStaffList.SelectedItem != null)
                {
                    cmbAddressType.SelectedValue = objStaffAddressInfo.AddressTypeID;
                    cmbAddressType.UpdateLayout();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void cmdDoctorBankInfoModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyDoctor = true;
            ModifyDoctor = CheckBankValidation();
            if (ModifyDoctor == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure You Want To Update The Employee Bank Details?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW4_OnMessageBoxClosed);

                msgW1.Show();
            }
        }
        void msgW4_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                UpdateStaffBankInformation();
            else if (result == MessageBoxResult.No)
            {
                ClearBankControl();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Employee Bank Details Updating Cancelled.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                SaveBank.IsEnabled = true;
                ModifyBank.IsEnabled = false;
            }

        }

        private void UpdateStaffBankInformation()
        {

            clsUpdateStaffBankInfoVO BizAction = new clsUpdateStaffBankInfoVO();
            BizAction.objStaffBankDetail = new clsStaffBankInfoVO();

            if (dgStaffList.SelectedItem != null && isSelectedDID == true)
            {
                BizAction.objStaffBankDetail.StaffId = ((clsStaffMasterVO)dgStaffList.SelectedItem).ID;
            }
            else if (DID != 0)
                BizAction.objStaffBankDetail.StaffId = DID;

            BizAction.objStaffBankDetail.ID = ((clsStaffBankInfoVO)dgbankDetails.SelectedItem).ID;

            BizAction.objStaffBankDetail.AccountNumber = txtAccountNumber.Text;

            BizAction.objStaffBankDetail.BranchAddress = txtAddress.Text;
            BizAction.objStaffBankDetail.MICRNumber = txtMICRNumber.Text;
            if (RadioSaving.IsChecked == true)
            {
                BizAction.objStaffBankDetail.AccountType = true;
            }

            if (RadioCurrent.IsChecked == true)
            {
                BizAction.objStaffBankDetail.AccountType = false;
            }
            if (cmbBankName.SelectedItem != null)
                BizAction.objStaffBankDetail.BankId = ((MasterListItem)cmbBankName.SelectedItem).ID;

            if (cmbBranchName.SelectedItem != null)
                BizAction.objStaffBankDetail.BranchId = ((MasterListItem)cmbBranchName.SelectedItem).ID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("ModifyBankInfo");
                if (arg.Error == null)
                {
                    if (dgStaffList.SelectedItem != null && isSelectedDID == true)
                    {
                        FetchBankData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
                    }
                    else if (DID != 0)
                    {
                        FetchBankData(DID);
                    }

                    ClearBankControl();
                    // objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Employee Bank Details Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //Save.IsEnabled = true; ;
                    //modify.IsEnabled = false;
                    SaveBank.IsEnabled = true;
                    ModifyBank.IsEnabled = false;
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Updating Employee Bank Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    SaveBank.IsEnabled = false;
                    ModifyBank.IsEnabled = true;
                    msgW1.Show();
                    //SaveBank.IsEnabled = false;
                    //ModifyAddress.IsEnabled = true;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        bool isAddressView = false;

        clsStaffAddressInfoVO objStaffAddressInfo = new clsStaffAddressInfoVO();

        private void FillStaffAddressData(long iStaffID)
        {

            clsGetStaffAddressInfoByIdVO BizAction = new clsGetStaffAddressInfoByIdVO();

            BizAction.StaffId = iStaffID;
            BizAction.AddressTypeId = ((clsStaffAddressInfoVO)dgAddressDetails.SelectedItem).AddressTypeID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    SetCommandButtonState("ViewAddressInfo");



                    objStaffAddressInfo = ((clsGetStaffAddressInfoByIdVO)ea.Result).objStaffAddressDetail;

                    if (objStaffAddressInfo != null)
                    {

                        cmbAddressType.SelectedValue = objStaffAddressInfo.AddressTypeID;

                    }

                    if (objStaffAddressInfo != null)
                    {
                        txtName.Text = Convert.ToString(objStaffAddressInfo.Name);

                    }
                    if (objStaffAddressInfo != null)
                    {
                        txtDoctorAddress.Text = Convert.ToString(objStaffAddressInfo.Address);

                    }
                    if (objStaffAddressInfo != null)
                    {
                        txtContact1.Text = Convert.ToString(objStaffAddressInfo.Contact1);

                    }
                    if (objStaffAddressInfo != null)
                    {
                        txtContact2.Text = Convert.ToString(objStaffAddressInfo.Contact2);

                    }



                    FillBankDetail();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }


        private void cmbBankName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbBankName.SelectedItem != null)
            {
                FillBankBranch(((MasterListItem)cmbBankName.SelectedItem).ID);
            }

        }
        private void FillBankBranch(long BankId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_BankBranchMaster;
            if (BankId > 0)
                BizAction.Parent = new KeyValue { Key = BankId, Value = "BankId" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbBranchName.ItemsSource = null;
                    cmbBranchName.ItemsSource = objList;


                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbBranchName.SelectedItem = objList[0];

                    if ((clsStaffBankInfoVO)dgbankDetails.SelectedItem != null)
                    {
                        cmbBranchName.SelectedValue = objStaffBankInfo.BranchId;
                        cmbBranchName.UpdateLayout();

                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdDoctorAddressInfoSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveStaff = true;

            SaveStaff = CheckAddressValidation();

            if (SaveStaff == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure You Want To Save The Employee Address Details?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW3_OnMessageBoxClosed);

                msgW.Show();
            }
        }
        void msgW3_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveStaffAddressInformation();
            else if (result == MessageBoxResult.No)
            {
                ClearAddressControl();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Employee address Details adding Cancelled.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                SaveAddress.IsEnabled = true;
                ModifyAddress.IsEnabled = false;
                msgW1.Show();
            }
        }
        private bool CheckAddressValidation()
        {
            bool result = true;
            if ((MasterListItem)cmbAddressType.SelectedItem == null)
            {
                cmbAddressType.TextBox.SetValidation("Please Select Address Type");
                cmbAddressType.TextBox.RaiseValidationError();
                cmbAddressType.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbAddressType.SelectedItem).ID == 0)
            {
                cmbAddressType.TextBox.SetValidation("Please Select Address Type");
                cmbAddressType.TextBox.RaiseValidationError();
                cmbAddressType.Focus();
                result = false;
            }
            else
                cmbAddressType.TextBox.ClearValidationError();
            if (txtDoctorAddress.Text == "")
            {
                txtDoctorAddress.SetValidation("Please Enter Address");
                txtDoctorAddress.RaiseValidationError();
                txtDoctorAddress.Focus();
                result = false;
            }
            else
                txtDoctorAddress.ClearValidationError();

            if (txtContact1.Text == "")
            {
                txtContact1.SetValidation("Please Enter Phone Number");
                txtContact1.RaiseValidationError();
                txtContact1.Focus();
                result = false;
            }
            else if (txtContact1.Text.IsItNumber() == false)
            {
                txtContact1.SetValidation("Enter Number Only");
                txtContact1.RaiseValidationError();
                txtContact1.Focus();
                result = false;
            }
            else
                txtContact1.ClearValidationError();

            //COMMENTED BY ROHINI
            //else if (txtContact1.Text.Length < 10)
            //{
            //    txtContact1.SetValidation("Enter Atleast 10 Digit Number");
            //    txtContact1.RaiseValidationError();
            //    txtContact1.Focus();
            //    result = false;
            //}
         
            //COMMENTED BY ROHINI
            //if (txtContact2.Text != "" && txtContact2.Text != null)
            //{
            //    if (txtContact2.Text.Length < 10)
            //    {
            //        txtContact2.SetValidation("Enter Atleast 10 Digit Number");
            //        txtContact2.RaiseValidationError();
            //        txtContact2.Focus();
            //        result = false;
            //    }
            //    else
            //        txtContact2.ClearValidationError();
            //}

            //if (txtContact2.Text == "")
            //{
            //    txtContact2.SetValidation("Please Enter Phone Number");
            //    txtContact2.RaiseValidationError();
            //    txtContact2.Focus();
            //    result = false;
            //}
            //else if (txtContact2.Text.IsItNumber() == false)
            //{
            //    txtContact2.SetValidation("Enter Number Only");
            //    txtContact2.RaiseValidationError();
            //    txtContact2.Focus();
            //    result = false;
            //}
            //else
            //    txtContact2.ClearValidationError();


            return result;
        }
        private void SaveStaffAddressInformation()
        {
            Indicatior.Show();
            clsAddStaffAddressInfoBizActionVO BizAction = new clsAddStaffAddressInfoBizActionVO();
            BizAction.objStaffBankDetail = new clsStaffAddressInfoVO();

            //  if (Save.IsEnabled == false && isSelectedDID==true)  //if (dgStaffList.SelectedItem != null)
            if (isSelectedDID == true && dgStaffList.SelectedItem != null)
            {

                BizAction.objStaffBankDetail.StaffId = ((clsStaffMasterVO)dgStaffList.SelectedItem).ID;
            }
            else if (DID != 0)
                BizAction.objStaffBankDetail.StaffId = DID;

            if (cmbAddressType.SelectedItem != null)
                BizAction.objStaffBankDetail.AddressTypeID = ((MasterListItem)cmbAddressType.SelectedItem).ID;

            if (txtName.Text != null)
                BizAction.objStaffBankDetail.Name = Convert.ToString(txtName.Text);

            if (txtContact1.Text != null)
            {
                BizAction.objStaffBankDetail.Contact1 = txtContact1.Text;
            }
            if (txtContact2.Text != null)
            {
                BizAction.objStaffBankDetail.Contact2 = txtContact2.Text;
            }

            if (txtDoctorAddress.Text != null)
                BizAction.objStaffBankDetail.Address = txtDoctorAddress.Text;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                //SetCommandButtonState("Save");

                if (arg.Error == null && ((clsAddStaffAddressInfoBizActionVO)arg.Result).SuccessStatus == 0)
                {
                    if (dgStaffList.SelectedItem != null && isSelectedDID == true)
                    {
                        FetchStaffAddressData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
                        //FetchStaffAddressData(BizAction.st);

                    }
                    else if (DID != 0)
                        FetchStaffAddressData(DID);
                    ClearAddressControl();
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Employee Address Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    SaveAddress.IsEnabled = true; ;
                    ModifyAddress.IsEnabled = false;
                    msgW1.Show();
                }
                else if (arg.Error == null && ((clsAddStaffAddressInfoBizActionVO)arg.Result).SuccessStatus == 1 )
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Contact Number Already Exist.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                        Indicatior.Close();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Save Employee Address Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    SaveAddress.IsEnabled = true; ;
                    ModifyAddress.IsEnabled = false;
                    msgW1.Show();
                    Indicatior.Close();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void cmdDoctorAddressInfoModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyDoctor = true;
            ModifyDoctor = CheckAddressValidation();
            if (ModifyDoctor == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure You Want To Update The Employee Address Details?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW5_OnMessageBoxClosed);

                msgW1.Show();
            }
        }
        void msgW5_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                UpdateStaffAddressInformation();
        }
        private void UpdateStaffAddressInformation()
        {

            clsUpdateStaffAddressInfoVO BizAction = new clsUpdateStaffAddressInfoVO();
            BizAction.objStaffAddressDetail = new clsStaffAddressInfoVO();
            if (dgStaffList.SelectedItem != null)
            {
                BizAction.objStaffAddressDetail.StaffId = ((clsStaffAddressInfoVO)dgAddressDetails.SelectedItem).StaffId;
            }
            if (dgAddressDetails.SelectedItem != null)
            {
                BizAction.objStaffAddressDetail.ID = ((clsStaffAddressInfoVO)dgAddressDetails.SelectedItem).ID;
            }
            if (cmbAddressType.SelectedItem != null)
            {
                BizAction.objStaffAddressDetail.AddressTypeID = ((MasterListItem)cmbAddressType.SelectedItem).ID;
            }
            BizAction.objStaffAddressDetail.Address = txtDoctorAddress.Text;
            BizAction.objStaffAddressDetail.Name = txtName.Text;
            BizAction.objStaffAddressDetail.Contact1 = txtContact1.Text;
            BizAction.objStaffAddressDetail.Contact2 = txtContact2.Text;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("ModifyAddressInfo");
                if (arg.Error == null && ((clsUpdateStaffAddressInfoVO)arg.Result).SuccessStatus == 0)
                {
                    if (dgStaffList.SelectedItem != null && isSelectedDID == true)
                    {
                        FetchStaffAddressData(((clsStaffMasterVO)dgStaffList.SelectedItem).ID);
                    }
                    else if (DID != 0)
                    {
                        FetchStaffAddressData(DID);
                    }
                    ClearAddressControl();
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Employee Address Details Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    SaveAddress.IsEnabled = true; ;
                    ModifyAddress.IsEnabled = false;
                    //  Save.IsEnabled = true; ;
                    // modify.IsEnabled = false;

                    msgW1.Show();
                }
                else if (arg.Error == null && ((clsUpdateStaffAddressInfoVO)arg.Result).SuccessStatus == 1)
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Contact Number Already Exist.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    SaveAddress.IsEnabled = false; ;
                    ModifyAddress.IsEnabled = true;
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Updating Employee Address Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    SaveAddress.IsEnabled = false; ;
                    ModifyAddress.IsEnabled = true;
                    // Save.IsEnabled = false;
                    // modify.IsEnabled = true;
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ClearBankControl()
        {
            try
            {
                dgbankDetails.SelectedItem = null;

                RadioSaving.IsChecked = true;
                RadioCurrent.IsChecked = false;
                txtAccountNumber.Text = string.Empty;
                txtMICRNumber.Text = string.Empty;
                txtAddress.Text = string.Empty;

                cmbBankName.SelectedValue = (long)0;
                cmbBranchName.SelectedValue = (long)0;


            }
            catch (Exception)
            {
                throw;
            }
        }
        private void ClearAddressControl()
        {
            try
            {
                dgAddressDetails.SelectedItem = null;

                txtName.Text = string.Empty;
                txtDoctorAddress.Text = string.Empty;
                txtContact1.Text = string.Empty;
                txtContact2.Text = string.Empty;
                cmbAddressType.SelectedValue = (long)0;
                txtAddress.Text = string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtContact1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }


        private void txtMICRNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void tbStaffInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (tbIBankInfo != null)
            //{
            //    if (tbIBankInfo.IsSelected == true)
            //    {
            //        CheckBankValidation();


            //    }
            //}
            //if (tbIBankInfo != null)
            //{
            //    if (tbIAddressInfo.IsSelected == true)
            //    {
            //        CheckAddressValidation();
            //    }
            //}

        }

        private void dtpDOJ_LostFocus(object sender, RoutedEventArgs e)
        {

            // if (((clsStaffMasterVO)this.DataContext).DOB.Value.Date >= DateTime.Now.Date)
            // {
            //   dtpDOB1.SetValidation("Birth Date can not be greater than Todays Date");
            //   dtpDOB1.RaiseValidationError();
            // }
            // else if (((clsStaffMasterVO)this.DataContext).DOB == null)
            // {
            //     dtpDOB1.SetValidation("Birth Date is required");
            //     dtpDOB1.RaiseValidationError();
            // }    
            // else if (((clsStaffMasterVO)this.DataContext).DOB.Value.Date >= dtpDOJ.SelectedDate)
            //{
            //     dtpDOJ.SetValidation("Date of Joining should be greater than Date of Birth");
            //     dtpDOJ.RaiseValidationError();
            //}

            //else
            //{
            //    dtpDOJ.ClearValidationError();
            //    dtpDOB1.ClearValidationError();
            // }


            if (dtpDOB1.SelectedDate >= DateTime.Now.Date)
            {
                dtpDOB1.SetValidation("Date of Birth should be less than Todays Date");
                dtpDOB1.RaiseValidationError();
            }
            else if (dtpDOB1.SelectedDate == null)
            {
                dtpDOB1.SetValidation("Date of Birth is required");
                dtpDOB1.RaiseValidationError();
            }
            else if (dtpDOB1.SelectedDate >= dtpDOJ.SelectedDate)
            {
                dtpDOJ.SetValidation("Date of Joining should be greater than Date of Birth");
                dtpDOJ.RaiseValidationError();
            }

            else
            {
                dtpDOJ.ClearValidationError();
                dtpDOB1.ClearValidationError();
            }
        }

        private void txtLastName2_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

            if (e.Key == Key.Enter)
            {
                FetchData();
            }

        }
        //Added by AJ Date 10/11/2016
        private void ChkDischargeApprove_Click(object sender, RoutedEventArgs e)
        {

            if (((CheckBox)sender).IsChecked == true)
            {
                cmbDepartment.Visibility = Visibility.Visible;
                lblDepartment.Visibility = Visibility.Visible;
                FillDepartmentList(((MasterListItem)cmbClinic.SelectedItem).ID);
            }
            else
            {
                cmbDepartment.Visibility = Visibility.Collapsed;
                lblDepartment.Visibility = Visibility.Collapsed;
            }
        }


        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                    {

                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }


                    cmbDepartment.ItemsSource = null;

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbDepartment.ItemsSource = objList;

                    if (objStaff.IsDischargeApprove == true)
                    {                       
                        if (objStaff.DepartmentID != null)
                        {
                            cmbDepartment.SelectedValue = objStaff.DepartmentID;
                        }
                    }
                    else
                    {
                        cmbDepartment.SelectedValue = objList[0].ID;
                        cmbDepartment.SelectedItem = objList[0];
                    }
                }
            };


            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {

               
            }
          
        }
        //***//---------------------------------
    }
}


