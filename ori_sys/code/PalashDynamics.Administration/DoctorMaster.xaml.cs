using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using CIMS;
using CIMS.Forms;
using PalashDynamics.Collections;
using System.Reflection;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using System.Windows.Media.Imaging;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;



namespace PalashDynamics.Administration
{
    public partial class DoctorMaster : UserControl, IInitiateCIMS
    {
        #region Pagging

        public PagedSortableCollectionView<clsDoctorVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsDoctorBankInfoVO> DoctorBankDataList { get; private set; }
        public PagedSortableCollectionView<clsDoctorAddressInfoVO> DoctorAddressDataList { get; private set; }
        // public PagedSortableCollectionView<clsDoctorBankInfoVO> BankDataList { get; private set; }

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
        public int DoctorBankDataListPageSize
        {
            get
            {
                return DoctorBankDataList.PageSize;
            }
            set
            {
                if (value == DoctorBankDataList.PageSize) return;
                DoctorBankDataList.PageSize = value;
            }
        }
        public int DoctorAddressDataListPageSize
        {
            get
            {
                return DoctorAddressDataList.PageSize;
            }
            set
            {
                if (value == DoctorAddressDataList.PageSize) return;
                DoctorAddressDataList.PageSize = value;
            }
        }

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        void DoctorBankDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchBankData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
        }

        void DoctorAddressDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchDoctorAddressData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
        #endregion

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "PRO":


                    Flagref = true;
                    break;
            }

        }
        //Rohini for birth date validation
        private void SetDateValidation()
        {
            dtpDOB1.DisplayDate = DateTime.Now.Date;
            dtpDOB1.DisplayDateEnd = DateTime.Now.Date;
            dtpDOB1.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(1), DateTime.MaxValue));
        }
        public DoctorMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsDoctorVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

            DataListPageSize = 15;
            this.dgDataPager.DataContext = DataList;
            this.dgDoctorList.DataContext = DataList;

           // FetchData();

            //======================================================
        }

        #region Variable Declaration
        private long IUnitId { get; set; }
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public List<bool> check = new List<bool>();
        public bool CheckStatus { get; set; }
        bool IsCancel = true;
        public clsGetPatientBizActionVO OBJPatient { get; set; }
        bool Flagref = false;
        //public clsGetBankDetailsVO OBJBank { get; set; }
        #endregion

        private void DoctorMaster_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.DataContext = new clsDoctorVO();
                SetCommandButtonState("Load");
                CheckValidation();
                //dtpDOJ.SelectedDate = DateTime.Now;
                //dtpDOB1.SelectedDate = DateTime.Now;               
                FetchData();
                FillDoctorType();
                FillSpecialization();
                FillUnitDepartmentList();
                FillUnitClassificationList();
                FillMaritalStatus();
                FillAddressType();
                FillBankDetail();
                FillGender();
                FillDoctorCategory();
                FillMarketingExecutives();
                //BindDepartmentGrid();
                SetComboboxValue();
                txtFirstName1.Focus();
                txtFirstName.Focus();
                Indicatior.Close();
               
            }
            IsPageLoded = true;
            txtFirstName.Focus();
            txtFirstName.UpdateLayout();
            txtFirstName1.Focus();
            txtFirstName1.UpdateLayout();
            _captureSource = new CaptureSource();
            _SpousecaptureSource = new CaptureSource();
        }

        void BankDetailPaging(long iDoctorID)
        {
            DoctorBankDataList = new PagedSortableCollectionView<clsDoctorBankInfoVO>();
            DoctorBankDataList.OnRefresh += new EventHandler<RefreshEventArgs>(DoctorBankDataList_OnRefresh);
            DoctorBankDataListPageSize = 3;
            this.dgDataPager1.DataContext = DoctorBankDataList;
            this.dgbankDetails.DataContext = DoctorBankDataList;
            FetchBankData(iDoctorID);
            ClearBankControl();
        }

        void DoctorAddressDetailsPaging(long iDoctorID)
        {

            DoctorAddressDataList = new PagedSortableCollectionView<clsDoctorAddressInfoVO>();
            DoctorAddressDataList.OnRefresh += new EventHandler<RefreshEventArgs>(DoctorAddressDataList_OnRefresh);
            DoctorAddressDataListPageSize = 3;
            this.dgDataPager2.DataContext = DoctorAddressDataList;
            this.dgAddressDetails.DataContext = DoctorAddressDataList;
            FetchDoctorAddressData(iDoctorID);
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            cmbDoctorCategory.IsEnabled = true;
            this.SetCommandButtonState("New");
            tbDoctorInformation.SelectedIndex = 0;
            txtFirstName.Focus();
            ClearControl();

            this.DataContext = new clsDoctorVO();
            clsDoctorVO DVO = new clsDoctorVO();
            FillUnitDepartmentList();

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Doctor Details";
            //dtpDOB1.SelectedDate = DateTime.Now;
            //dtpDOJ.SelectedDate = DateTime.Now;
            SetDateValidation();
            cmbSubSpecialization.SelectedValue = (long)0;

            List<MasterListItem> objList = new List<MasterListItem>();
            MasterListItem objM = new MasterListItem(0, "-- Select --");
            objList.Add(objM);
            //  cmbSpecialization.ItemsSource = objList;
            if (((clsDoctorVO)this.DataContext).Photo != null)
            {
                ((clsDoctorVO)this.DataContext).Photo = null;
                objDoctor.Photo = null;
            }
            imgPhoto.Source = null;
            cmbSubSpecialization.ItemsSource = objList;
            cmbSubSpecialization.SelectedItem = objM;

            dtpDOJ.DisplayDate = DateTime.Today;
            dtpDOB1.DisplayDate = DateTime.Today;

            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            cmbDoctorCategory.IsEnabled = true;
            //  this.DataContext = new clsDoctorVO();
            SetCommandButtonState("Cancel");
            this.DataContext = null;
            //ROHINEE
            DoctorSignature = new byte[0];
            //
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);
            // FetchData();

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
            //rohinee
            ClearControl();
            ClearBankControl();
            ClearAddressControl();
        }

        #region FillCombobox

        PagedCollectionView collection;

        //public void BindDepartmentGrid(List<clsUnitDepartmentsDetailsVO> lstDepartment)
        //{
        //    try
        //    {
        //        dgDepartmentList.ItemsSource = null;
        //        collection = new PagedCollectionView(lstDepartment);
        //        collection.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
        //        dgDepartmentList.ItemsSource = collection;
        //    }
        //    catch (Exception ex)
        //    {

        //        // throw;
        //    }
        //}

        clsGetDepartmentListForDoctorMasterBizActionVO BizActionObj = new clsGetDepartmentListForDoctorMasterBizActionVO();

        private void FillUnitDepartmentList()
        {

            BizActionObj.DoctorDetails = new List<clsDoctorVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            dgDepartmentList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDepartmentListForDoctorMasterBizActionVO)arg.Result).DoctorDetails != null)
                    {
                        BizActionObj.DoctorDetails = ((clsGetDepartmentListForDoctorMasterBizActionVO)arg.Result).DoctorDetails;
                        List<clsUnitDepartmentsDetailsVO> userDepartmentList = new List<clsUnitDepartmentsDetailsVO>();

                        foreach (var item in BizActionObj.DoctorDetails)
                        {
                            userDepartmentList.Add(new clsUnitDepartmentsDetailsVO() { DepartmentID = item.DepartmentID, UnitName = item.UnitName, UnitID = item.UnitID, DepartmentName = item.DepartmentName, UnitDepartName = item.UnitDepartName, Status = false, IsDefault = false });
                        }

                        collection = new PagedCollectionView(userDepartmentList);
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                        dgDepartmentList.ItemsSource = collection;
                        // dgDepartmentList.ItemsSource = userDepartmentList;

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }


            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillUnitClassificationList()
        {
            clsGetClassificationListForDoctorMasterBizActionVO BizActionObj = new clsGetClassificationListForDoctorMasterBizActionVO();
            BizActionObj.DoctorDetails = new List<clsDoctorVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            dgClassificationList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetClassificationListForDoctorMasterBizActionVO)arg.Result).DoctorDetails != null)
                    {
                        BizActionObj.DoctorDetails = ((clsGetClassificationListForDoctorMasterBizActionVO)arg.Result).DoctorDetails;
                        List<clsUnitClassificationsDetailsVO> userClassificationList = new List<clsUnitClassificationsDetailsVO>();

                        foreach (var item in BizActionObj.DoctorDetails)
                        {
                            userClassificationList.Add(new clsUnitClassificationsDetailsVO() { ClassificationID = item.ClassificationID, UnitClassificationName = item.UnitClassificationName, IsAvailable = false, IsDefault = false });
                        }

                        dgClassificationList.ItemsSource = userClassificationList;

                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDepartments()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_DepartmentMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        List<MasterListItem> uList = new List<MasterListItem>();
                        uList = ((clsGetMasterListBizActionVO)ea.Result).MasterList;

                        dgDepartmentList.ItemsSource = null;

                        List<clsDepartmentsDetailsVO> userDepartmentList = new List<clsDepartmentsDetailsVO>();

                        foreach (var item in uList)
                        {
                            userDepartmentList.Add(new clsDepartmentsDetailsVO() { DepartmentID = item.ID, DepartmentName = item.Description, Status = false, IsDefault = false });
                        }

                        dgDepartmentList.ItemsSource = userDepartmentList;



                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;

            }
            catch (Exception ex)
            {

            }
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

                    cmbDoctorGender.ItemsSource = null;
                    cmbDoctorGender.ItemsSource = objList;

                }

                if (objDoctor != null)
                {
                    cmbDoctorGender.SelectedValue = objDoctor.GenderId;
                }

                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    if (OBJPatient != null)
                    {
                        if (OBJPatient.PatientDetails != null)
                        {
                            if (OBJPatient.PatientDetails.GenderID != 0)
                            {
                                cmbDoctorGender.SelectedValue = OBJPatient.PatientDetails.GenderID;
                            }
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillDoctorType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DoctorTypeMaster;
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

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 4

                                      select r;
                        cmbDoctorType.ItemsSource = null;
                        cmbDoctorType.ItemsSource = results.ToList();
                    }
                    else
                    {
                        cmbDoctorType.ItemsSource = null;
                        cmbDoctorType.ItemsSource = objList;
                    }

                }

                if (this.DataContext != null)
                {
                    cmbDoctorType.SelectedValue = ((clsDoctorVO)this.DataContext).DoctorId;
                }


                if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                {
                    cmbDoctorType.SelectedValue = objDoctor.DoctorType;
                    cmbDoctorType.UpdateLayout();

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSpecialization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
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
                    cmbSpecialization.ItemsSource = null;
                    cmbSpecialization.ItemsSource = objList;


                }

                if (this.DataContext != null)
                {
                    cmbSpecialization.SelectedValue = ((clsDoctorVO)this.DataContext).Specialization;
                }

                if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                {
                    cmbSpecialization.SelectedValue = objDoctor.Specialization;
                    cmbSpecialization.UpdateLayout();

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSubSpecialization(long iSupId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            if (iSupId > 0)
                BizAction.Parent = new KeyValue { Key = iSupId, Value = "fkSpecializationID" };
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

                    cmbSubSpecialization.ItemsSource = null;
                    cmbSubSpecialization.ItemsSource = objList;

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbSubSpecialization.SelectedItem = objList[0];

                    if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                    {
                        cmbSubSpecialization.SelectedValue = objDoctor.SubSpecialization;
                        cmbSubSpecialization.UpdateLayout();

                    }
                    else if (DID > 0)
                    {
                        cmbSubSpecialization.SelectedValue = objDoctor.SubSpecialization;
                        cmbSubSpecialization.UpdateLayout();
                    }
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDoctorCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DoctorCategoryMaster;
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
                    cmbDoctorCategory.ItemsSource = null;
                    cmbDoctorCategory.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbDoctorCategory.SelectedValue = ((clsDoctorVO)this.DataContext).DoctorCategoryId;
                }

                if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                {
                    cmbDoctorCategory.SelectedValue = objDoctor.DoctorCategoryId;
                    cmbDoctorCategory.UpdateLayout();

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        // Added By Somnath

        private void FillBankDetail()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_BankMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    //cmbMaritalStatus.ItemsSource = null;
                    //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    List<MasterListItem> objList = new List<MasterListItem>();

                    // objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbBankName.ItemsSource = null;
                    cmbBankName.ItemsSource = objList;

                }


                if (this.DataContext != null)
                {
                    cmbBankName.SelectedValue = ((clsDoctorVO)this.DataContext).BankId;
                }

                if ((clsDoctorBankInfoVO)dgbankDetails.SelectedItem != null)
                {
                    cmbBankName.SelectedValue = objDoctorBankInfo.BankId;
                    cmbBranchName.UpdateLayout();
                }
                //else if (DID > 0)
                //{
                //    cmbBankName.SelectedValue = objDoctorBankInfo.BankId;
                //    cmbBranchName.UpdateLayout();
                //}


            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
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

                    if ((clsDoctorBankInfoVO)dgbankDetails.SelectedItem != null)
                    {
                        cmbBranchName.SelectedValue = objDoctorBankInfo.BranchId;
                        cmbBranchName.UpdateLayout();

                    }
                    //else if (DID > 0)
                    //{
                    //    cmbBranchName.SelectedValue = objDoctorBankInfo.BranchId;
                    //    cmbBranchName.UpdateLayout();
                    //}

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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
                    //cmbMaritalStatus.ItemsSource = null;
                    //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    List<MasterListItem> objList = new List<MasterListItem>();

                    // objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbMaritalStatus.ItemsSource = null;
                    cmbMaritalStatus.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    cmbMaritalStatus.SelectedValue = ((clsDoctorVO)this.DataContext).MaritalStatusId;
                }

                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    if (OBJPatient != null)
                    {
                        if (OBJPatient.PatientDetails != null)
                        {
                            if (OBJPatient.PatientDetails.MaritalStatusID != 0)
                            {
                                cmbMaritalStatus.SelectedValue = OBJPatient.PatientDetails.MaritalStatusID;
                            }
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        void FillAddressType()
        {
            //Added by somnath(Not required)

            //List<MasterListItem> mlPaymode = new List<MasterListItem>();
            //MasterListItem Default = new MasterListItem(0, "- Select -");
            //mlPaymode.Insert(0, Default);
            //EnumToList(typeof(MasterAddressType), mlPaymode);
            //cmbAddressType.ItemsSource = mlPaymode;
            //cmbAddressType.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AddressTypeID;

            //End


            //Added by somnath(required)


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
                    //cmbMaritalStatus.ItemsSource = null;
                    //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    List<MasterListItem> objList = new List<MasterListItem>();

                    // objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbAddressType.ItemsSource = null;
                    cmbAddressType.ItemsSource = objList;

                }


                if (this.DataContext != null)
                {
                    cmbAddressType.SelectedValue = ((clsDoctorVO)this.DataContext).DoctorAddressInformation.AddressTypeID;
                }

                if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                {
                    cmbAddressType.SelectedValue = objDoctorAddressInfo.AddressTypeID;
                    cmbAddressType.UpdateLayout();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //Added By Somanath(Not required)
        //public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        //{
        //    Array Values = GetValues(EnumType);
        //    foreach (int Value in Values)
        //    {

        //        string Display = Enum.GetName(EnumType, Value);
        //        MasterListItem Item = new MasterListItem(Value, Display);
        //        TheMasterList.Add(Item);
        //    }
        //}
        //public static object[] GetValues(Type enumType)
        //{
        //    if (!enumType.IsEnum)
        //    {
        //        throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
        //    }

        //    List<object> values = new List<object>();

        //    var fields = from field in enumType.GetFields()
        //                 where field.IsLiteral
        //                 select field;

        //    foreach (FieldInfo field in fields)
        //    {
        //        object value = field.GetValue(enumType);
        //        values.Add(value);
        //    }

        //    return values.ToArray();
        //}


        //End
        #endregion

        /// <summary>
        /// Purpose:Getting list of selected items.
        /// </summary>
        /// <returns></returns>

        private clsDoctorVO CreateUserObjectFromFormData()
        {
            clsDoctorVO objDoctorVO = (clsDoctorVO)this.DataContext;
            objDoctorVO.DepartmentDetails = (List<clsDepartmentsDetailsVO>)dgDepartmentList.ItemsSource;
            objDoctorVO.UnitClassificationDetailsList = (List<clsUnitClassificationsDetailsVO>)dgClassificationList.ItemsSource;
            return objDoctorVO;
        }

        private clsDoctorVO AddStatus()
        {
            clsDoctorVO objDoctorVO = (clsDoctorVO)this.DataContext;
            List<clsUnitDepartmentsDetailsVO> dept = new List<clsUnitDepartmentsDetailsVO>();
            foreach (clsUnitDepartmentsDetailsVO item in dgDepartmentList.ItemsSource)
            {
                dept.Add(item);

            }
            objDoctorVO.UnitDepartmentDetails = dept;

            return objDoctorVO;
        }

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialization.SelectedItem != null)
            {
                //if (((MasterListItem)cmbSpecialization.SelectedItem).ID != 0)
                //{
                FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID);
                //}
            }
        }

        private void cmbBankName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbBankName.SelectedItem != null)
            {
                //if (((MasterListItem)cmbBankName.SelectedItem).ID != 0)
                //{
                //    FillBankBranch(((MasterListItem)cmbBankName.SelectedItem).ID);
                //}
                FillBankBranch(((MasterListItem)cmbBankName.SelectedItem).ID);
            }
        }

        private void SetComboboxValue()
        {
            cmbSpecialization.SelectedValue = ((clsDoctorVO)this.DataContext).Specialization;
            cmbSubSpecialization.SelectedValue = ((clsDoctorVO)this.DataContext).SubSpecialization;
            cmbDoctorType.SelectedValue = ((clsDoctorVO)this.DataContext).DoctorType;
            cmbDoctorCategory.SelectedValue = ((clsDoctorVO)this.DataContext).DoctorCategoryId;
            cmbMarketingExecutives.SelectedValue = ((clsDoctorVO)this.DataContext).MarketingExecutivesID; //***//
            //cmbBranchName.SelectedValue = ((clsDoctorVO)this.DataContext).BankBranchId;
        }

        /// <summary>
        /// Purpose:Save New Doctor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveDoctor = true;

            SaveDoctor = CheckValidation();

            if (SaveDoctor == true && IsDepartmentchk() && IsClassificationchk())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Doctor Master?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
                FetchData();
            }
            else
            {

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required fields to save the Doctor Master?", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveDoctorMaster();

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
                //  SaveDoctorMaster();
            }
            else
            {
                //SaveDoctorMaster();
                FetchData();
                ClearControl();
                objAnimation.Invoke(RotationType.Backward);
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }
        long DID;
        // bool isDuplicateSave = false;
        private void SaveDoctorMaster()
        {

            Indicatior.Show();
            //clsDoctorVO objDoctor = CreateUserObjectFromFormData();  
            clsDoctorVO objDoctor = AddStatus();
            //objDoctor.Photo=this.objDoctor.Photo;

            clsAddDoctorMasterBizActionVO BizAction = new clsAddDoctorMasterBizActionVO();
            BizAction.DoctorDetails = (clsDoctorVO)this.DataContext;
            if ((List<clsUnitClassificationsDetailsVO>)dgClassificationList.ItemsSource != null)
            {


                List<clsUnitClassificationsDetailsVO> ListItem = new List<clsUnitClassificationsDetailsVO>();
                ListItem = (List<clsUnitClassificationsDetailsVO>)dgClassificationList.ItemsSource;
                var expensiveInStockProducts =
                                                    from p in ListItem
                                                    where p.IsAvailable == true
                                                    select p;

                BizAction.DoctorDetails.UnitClassificationDetailsList = ((List<clsUnitClassificationsDetailsVO>)expensiveInStockProducts.ToList());

            }
            if (txtFirstName.Text != null)
                BizAction.DoctorDetails.FirstName = txtFirstName.Text;
            if (txtMiddleName.Text != null)
                BizAction.DoctorDetails.MiddleName = txtMiddleName.Text;
            if (txtLastName.Text != null)
                BizAction.DoctorDetails.LastName = txtLastName.Text;
            if (dtpDOB1.SelectedDate != null)
                BizAction.DoctorDetails.DOB = dtpDOB1.SelectedDate;
            if (txtEducation.Text != null)
                BizAction.DoctorDetails.Education = txtEducation.Text;
            if (txtExperience.Text != null)
                BizAction.DoctorDetails.Experience = txtExperience.Text;
            if (txtEmailID.Text != null)
                BizAction.DoctorDetails.EmailId = txtEmailID.Text;
            if (txtPAN.Text != null)
                BizAction.DoctorDetails.PermanentAccountNumber = txtPAN.Text;
            if (txtprovidentFound.Text != null)
                BizAction.DoctorDetails.ProvidentFund = txtprovidentFound.Text;
            if (txtRegistrationNumber.Text != null)
                BizAction.DoctorDetails.RegistrationNumber = txtRegistrationNumber.Text;
            if (txtACN.Text != null)
                BizAction.DoctorDetails.AccessCardNumber = txtACN.Text;
            if (dtpDOJ.SelectedDate != null)
                BizAction.DoctorDetails.DateofJoining = dtpDOJ.SelectedDate;
            if (txtEmployeeNumber.Text != null)
                BizAction.DoctorDetails.EmployeeNumber = txtEmployeeNumber.Text;

            if (((clsDoctorVO)this.DataContext).Photo != null)
                BizAction.DoctorDetails.Photo = ((clsDoctorVO)this.DataContext).Photo;
            //BizAction.DoctorDetails.Photo = objDoctor.Photo;
            if (cmbSpecialization.SelectedItem != null)
                BizAction.DoctorDetails.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID;

            if (cmbSubSpecialization.SelectedItem != null)
                BizAction.DoctorDetails.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;

            if (cmbDoctorGender.SelectedItem != null)
                BizAction.DoctorDetails.GenderId = ((MasterListItem)cmbDoctorGender.SelectedItem).ID;

            if (cmbDoctorType.SelectedItem != null)
                BizAction.DoctorDetails.DoctorType = ((MasterListItem)cmbDoctorType.SelectedItem).ID;

            if (cmbMaritalStatus.SelectedItem != null)
            {
                BizAction.DoctorDetails.MaritalStatusId = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;
            }

            //Added by Ashish Z.
            if (cmbDoctorCategory.SelectedItem != null)
            {
                BizAction.DoctorDetails.DoctorCategoryId = ((MasterListItem)cmbDoctorCategory.SelectedItem).ID;
            }
            //rohini
            if (((clsDoctorVO)this.DataContext).Signature != null)
                BizAction.DoctorDetails.Signature = ((clsDoctorVO)this.DataContext).Signature;
            else
                BizAction.DoctorDetails.Signature = null;

            //***//
            if (cmbMarketingExecutives.SelectedItem != null)
            {
                BizAction.DoctorDetails.MarketingExecutivesID = ((MasterListItem)cmbMarketingExecutives.SelectedItem).ID;
            }

            //WriteableBitmap bmp1 = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);
            //bmp1.Render(SignatuerImage, new MatrixTransform());
            //bmp1.Invalidate();

            //int[] p1 = bmp1.Pixels;
            //int len1 = p1.Length * 4;
            //byte[] result1 = new byte[len1];
            //Buffer.BlockCopy(p1, 0, result1, 0, len1);
            //BizAction.DoctorDetails.Signature = result1;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("Save");

                if (arg.Error == null && arg.Result != null)
                {
                    //rohinee
                    if (((clsAddDoctorMasterBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        FetchData();
                        Indicatior.Close();
                        //isDuplicateSave = false;
                        clsAddDoctorMasterBizActionVO result = arg.Result as clsAddDoctorMasterBizActionVO;
                        if (result.DoctorDetails != null)
                        {
                            DID = result.DoctorDetails.DoctorId;
                        }



                        string msgTitle = "Palash";
                        string msgText = "Are you sure you want to save the Doctor Bank Information And Doctor Address Information ?";

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW11_OnMessageBoxClosed);

                        msgW1.Show();
                    }

                    else if (((clsAddDoctorMasterBizActionVO)arg.Result).SuccessStatus == 1)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Record can not be Saved because Doctor record already Exist.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        Indicatior.Close();
                        SetCommandButtonState("New");

                    }

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Camp  Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }


        /// <summary>
        /// Purpose:Modify existing doctor details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyDoctor = true;
            ModifyDoctor = CheckValidation();
            if (ModifyDoctor == true && IsDepartmentchk() && IsClassificationchk())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Update the Doctor Master?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }

        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();
        }

        private void Modify()
        {
            clsAddDoctorMasterBizActionVO BizAction = new clsAddDoctorMasterBizActionVO();
            BizAction.DoctorDetails = new clsDoctorVO();


            if (dgDoctorList.SelectedItem != null)
            {
                BizAction.DoctorDetails.DoctorId = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId;
            }
            else
                BizAction.DoctorDetails.DoctorId = DID;
            if (txtFirstName.Text != null)
                BizAction.DoctorDetails.FirstName = txtFirstName.Text;
            if (txtMiddleName.Text != null)
                BizAction.DoctorDetails.MiddleName = txtMiddleName.Text;
            if (txtLastName.Text != null)
                BizAction.DoctorDetails.LastName = txtLastName.Text;
            if (dtpDOB1.SelectedDate != null)
                BizAction.DoctorDetails.DOB = dtpDOB1.SelectedDate;
            if (txtEducation.Text != null)
                BizAction.DoctorDetails.Education = txtEducation.Text;
            if (txtExperience.Text != null)
                BizAction.DoctorDetails.Experience = txtExperience.Text;
            if (txtEmailID.Text != null)
                BizAction.DoctorDetails.EmailId = txtEmailID.Text;
            if (txtPAN.Text != null)
                BizAction.DoctorDetails.PermanentAccountNumber = txtPAN.Text;
            if (txtprovidentFound.Text != null)
                BizAction.DoctorDetails.ProvidentFund = txtprovidentFound.Text;
            if (txtRegistrationNumber.Text != null)
                BizAction.DoctorDetails.RegistrationNumber = txtRegistrationNumber.Text;
            if (txtACN.Text != null)
                BizAction.DoctorDetails.AccessCardNumber = txtACN.Text;
            if (dtpDOJ.SelectedDate != null)
                BizAction.DoctorDetails.DateofJoining = dtpDOJ.SelectedDate;
            if (txtEmployeeNumber.Text != null)
                BizAction.DoctorDetails.EmployeeNumber = txtEmployeeNumber.Text;
            if (objDoctor.Photo != null)
                BizAction.DoctorDetails.Photo = objDoctor.Photo;
            if (cmbSpecialization.SelectedItem != null)
                BizAction.DoctorDetails.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID;

            if (cmbSubSpecialization.SelectedItem != null)
                BizAction.DoctorDetails.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;

            if (cmbDoctorGender.SelectedItem != null)
                BizAction.DoctorDetails.GenderId = ((MasterListItem)cmbDoctorGender.SelectedItem).ID;

            if (cmbDoctorType.SelectedItem != null)
                BizAction.DoctorDetails.DoctorType = ((MasterListItem)cmbDoctorType.SelectedItem).ID;
            if (cmbMaritalStatus.SelectedItem != null)
                BizAction.DoctorDetails.MaritalStatusId = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;

            if (cmbDoctorCategory.SelectedItem != null)
            {
                BizAction.DoctorDetails.DoctorCategoryId = ((MasterListItem)cmbDoctorCategory.SelectedItem).ID;
            }

            //ROHINEE
            if (((clsDoctorVO)this.DataContext).Signature != null)
                BizAction.DoctorDetails.Signature = ((clsDoctorVO)this.DataContext).Signature;

            //***//
            if (cmbMarketingExecutives.SelectedItem != null)
            {
                BizAction.DoctorDetails.MarketingExecutivesID = ((MasterListItem)cmbMarketingExecutives.SelectedItem).ID;
            }

            //WriteableBitmap bmp = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);
            //bmp.Render(SignatuerImage, new MatrixTransform());
            //bmp.Invalidate();

            //int[] p3 = bmp.Pixels;
            //int len = p3.Length * 4;
            //byte[] result = new byte[len]; // ARGB
            //Buffer.BlockCopy(p3, 0, result, 0, len);
            //BizAction.DoctorDetails.Signature = result;


            List<clsUnitDepartmentsDetailsVO> dept = new List<clsUnitDepartmentsDetailsVO>();// ((PagedCollectionView)dgDepartmentList.ItemsSource)).FirstOrDefault(p => p.Status == true);
            foreach (clsUnitDepartmentsDetailsVO item in dgDepartmentList.ItemsSource)
            {
                dept.Add(item);

            }
            BizAction.DoctorDetails.UnitDepartmentDetails = dept;

            //BizAction.DoctorDetails.DepartmentDetails = (List<clsDepartmentsDetailsVO>)dgDepartmentList.ItemsSource;
            // BizAction.DoctorDetails.UnitDepartmentDetails = (List<clsUnitDepartmentsDetailsVO>)dgDepartmentList.ItemsSource;
            // BizAction.DoctorDetails.UnitClassificationDetailsList = (List<clsUnitClassificationsDetailsVO>)dgClassificationList.ItemsSource;

            if ((List<clsUnitClassificationsDetailsVO>)dgClassificationList.ItemsSource != null)
            {


                List<clsUnitClassificationsDetailsVO> ListItem = new List<clsUnitClassificationsDetailsVO>();
                ListItem = (List<clsUnitClassificationsDetailsVO>)dgClassificationList.ItemsSource;
                var expensiveInStockProducts =
                                                    from p in ListItem
                                                    where p.IsAvailable == true
                                                    select p;

                BizAction.DoctorDetails.UnitClassificationDetailsList = ((List<clsUnitClassificationsDetailsVO>)expensiveInStockProducts.ToList());

            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("Modify");
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddDoctorMasterBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        FetchData();
                        objAnimation.Invoke(RotationType.Backward);
                        Indicatior.Close();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                    else if (((clsAddDoctorMasterBizActionVO)arg.Result).SuccessStatus == 1)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Record can not be Modify because Doctor Information already Exist.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        Indicatior.Close();
                        SetCommandButtonState("View");

                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while updating Doctor Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    SetCommandButtonState("View");
                    Indicatior.Close();

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        /// <summary>
        /// Purpose:Getting list of Doctor details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();

        }

        private void FetchData()
        {
            clsGetDoctorDetailListForDoctorMasterBizActionVO BizAction = new clsGetDoctorDetailListForDoctorMasterBizActionVO();
            BizAction.DoctorDetails = new List<clsDoctorVO>();


            if (txtFirstName1.Text != null)
                BizAction.FirstName = txtFirstName1.Text;

            if (txtLastName2.Text != null)
                BizAction.LastName = txtLastName2.Text;

            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = 0;
            }
            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //dgDoctorList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDoctorDetailListForDoctorMasterBizActionVO)arg.Result).DoctorDetails != null)
                    {
                        //dgDoctorList.ItemsSource = ((clsGetDoctorDetailListForDoctorMasterBizActionVO)arg.Result).DoctorDetails;
                        clsGetDoctorDetailListForDoctorMasterBizActionVO result = arg.Result as clsGetDoctorDetailListForDoctorMasterBizActionVO;

                        if (result.DoctorDetails != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetDoctorDetailListForDoctorMasterBizActionVO)arg.Result).TotalRows;
                            foreach (clsDoctorVO item in result.DoctorDetails)
                            {
                                if (item.IsDocAttached == true)
                                     item.AttachedImage = "/PalashDynamics;component/Icons/Attachment-milann-color.png";


                                DataList.Add(item);

                            }


                        }
                        //if (DataList != null)
                        //{
                        //    BankDetailPaging();
                        //    DoctorAddressDetailsPaging();
                        //}

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void chkDepartment_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Purpose:View Doctor details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>      
        /// 
        private void hlbViewDoctorMaster_Click(object sender, RoutedEventArgs e)
        {
            cmbDoctorCategory.IsEnabled = false;
            SetCommandButtonState("View");
            //rohini  to set selected tab
            //  txtFirstName.Focus();
            if (tbDoctorInformation.SelectedItem != tbIGeneralInfo)
            {
                tbDoctorInformation.SelectedItem = tbIGeneralInfo;
            }
            FillData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorName;
            SetDateValidation();
            SaveBank.IsEnabled = true;
            ModifyBank.IsEnabled = false;
            SaveAddress.IsEnabled = true;
            ModifyAddress.IsEnabled = false;

        }

        clsDoctorVO objDoctor = new clsDoctorVO();

        private void FillData(long iDoctorID)
        {
            clsGetDoctorDetailListForDoctorMasterByIDBizActionVO BizAction = new clsGetDoctorDetailListForDoctorMasterByIDBizActionVO();
            BizAction.DoctorID = iDoctorID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    SetCommandButtonState("View");
                    if (dgDoctorList.SelectedItem != null)
                        objAnimation.Invoke(RotationType.Forward);


                    objDoctor = ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)ea.Result).DoctorDetails;
                    this.DataContext = objDoctor;
                    //if (objDoctor.Signature != null)
                    //{
                    //   // byte[] imageBytes1 = objDoctor.Signature;
                    //    WriteableBitmap bmp1 = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                    //    bmp1.FromByteArray(objDoctor.Signature);
                    //    SignatuerImage.Source = bmp1;
                    //}
                    //else
                    //{
                    //    SignatuerImage.Source = null;
                    //}
                    //rohini



                    if (objDoctor.Signature != null)
                    {
                        //WriteableBitmap bmp1 = new WriteableBitmap((int)SignatuerImage.Width, (int)SignatuerImage.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                        //bmp1.SetSource(new MemoryStream(objDoctor.Signature, false));
                        ////  bmp1.FromByteArray(objStaff.Photo);
                        //SignatuerImage.Source = bmp1;

                        byte[] imageBytes = objDoctor.Signature;
                        BitmapImage img = new BitmapImage();
                        img.SetSource(new MemoryStream(imageBytes, false));
                        SignatuerImage.Source = img;
                    }
                    if (objDoctor.Photo != null)
                    {
                        //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                        //bmp.FromByteArray(objDoctor.Photo);
                        //imgPhoto.Source = bmp;

                        byte[] imageBytes = objDoctor.Photo;
                        BitmapImage img = new BitmapImage();
                        img.SetSource(new MemoryStream(imageBytes, false));
                        imgPhoto.Source = img;
                    }
                    else
                    {
                        imgPhoto.Source = null;
                    }
                    //
                    //txtFirstName.Text = objDoctor.FirstName;
                    //if (objDoctor.MiddleName != null)
                    //{
                    //    txtMiddleName.Text = objDoctor.MiddleName;
                    //}


                    if (objDoctor.DateofJoining != null)
                        dtpDOJ.SelectedDate = objDoctor.DateofJoining;
                    //if (objDoctor.EmailId != null)
                    //{
                    //    txtEmailID.Text = objDoctor.EmailId;
                    //}

                    //if (objDoctor.Education != null)
                    //{
                    //    txtEducation.Text = objDoctor.Education;
                    //}

                    //if (objDoctor.AccessCardNumber != null)
                    //{
                    //    txtACN.Text = objDoctor.AccessCardNumber;
                    //}
                    //if (objDoctor.ProvidentFund != null)
                    //{
                    //    txtprovidentFound.Text = objDoctor.ProvidentFund;
                    //}
                    //if (objDoctor.PermanentAccountNumber != null)
                    //{
                    //    txtPAN.Text = objDoctor.PermanentAccountNumber;
                    //}
                    //if (objDoctor.Experience != null)
                    //{
                    //    txtExperience.Text = objDoctor.Experience;
                    //}

                    //if (objDoctor.RegistrationNumber != null)
                    //{
                    //    txtRegistrationNumber.Text = objDoctor.RegistrationNumber;
                    //}

                    if (objDoctor.MaritalStatusId != null)
                    {
                        cmbMaritalStatus.SelectedValue = objDoctor.MaritalStatusId;
                    }

                    ////if (objDoctor.GenderId != null)
                    ////{
                    ////    cmbDoctorGender.SelectedValue = objDoctor.GenderId;

                    ////}
                    //if (objDoctor.EmployeeNumber != null)
                    //{
                    //    txtEmployeeNumber.Text = objDoctor.EmployeeNumber;
                    //}
                    //if (objDoctor.DateofJoining != null)
                    //   dtpDOJ.SelectedDate = objDoctor.DateofJoining;
                    //if (objDoctor.Experience != null)
                    //{
                    //    txtExperience.Text = objDoctor.Experience;
                    //}


                    IUnitId = objDoctor.UnitID;

                    //if (objDoctor.Photo != null)
                    //{
                    //    byte[] imageBytes = objDoctor.Photo;
                    //    //BitmapImage img = new BitmapImage();
                    //    //img.SetSource(new MemoryStream(imageBytes, false));
                    //    DoctorPhoto.Height = 300;
                    //    DoctorPhoto.Width = 300;
                    //    //this.DoctorPhoto.Source = img;
                    //    WriteableBitmap bmp = new WriteableBitmap((int)DoctorPhoto.Width, (int)DoctorPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                    //    bmp.FromByteArray(imageBytes);
                    //    DoctorPhoto.Source = bmp;


                    //}
                    //if (objDoctor.Photo == null)
                    //{
                    //    ImageSourceConverter c = new ImageSourceConverter();
                    //    //this.DoctorPhoto.Source = (ImageSource)c.ConvertFromString("/OPDModule/Icons/Add-Male-User1.png");
                    //    this.DoctorPhoto.Source = new BitmapImage(new Uri("/Add-Male-User1.png", UriKind.Relative));


                    //    //this.DoctorPhoto.Source = new BitmapImage(new Uri("~/Icons/Add-Male-User1.png", UriKind.RelativeOrAbsolute));
                    //    //this.DoctorPhoto.Source = "~/Add-Male-User1.png";
                    //}




                    if (objDoctor.UnitDepartmentDetails != null)
                    {
                        //dgDepartmentList.ItemsSource=new List<clsUnitDepartmentsDetailsVO> ();
                        //List<clsUnitDepartmentsDetailsVO> lstDepartment = (List<clsUnitDepartmentsDetailsVO>)dgDepartmentList.ItemsSource;
                        //foreach (var item1 in objDoctor.UnitDepartmentDetails)
                        //{
                        //    foreach (var item in lstDepartment)
                        //    {
                        //        if (item.UnitID == item1.UnitID && item.DepartmentID == item1.DepartmentID)
                        //        {
                        //            item.IsDefault = item1.IsDefault;
                        //            item.Status = item1.Status;
                        //            break;
                        //        }
                        //    }
                        //}


                        //for (int J = 0; J < objDoctor.UnitDepartmentDetails.Count; J++)
                        //{
                        //    for (int i = 0; i < lstDepartment.Count; i++)
                        //    {
                        //        if (lstDepartment[i].DepartmentID == objDoctor.UnitDepartmentDetails[J].DepartmentID)
                        //        {
                        //            lstDepartment[i].Status = objDoctor.UnitDepartmentDetails[J].Status;
                        //            break;
                        //        }
                        //    }
                        //}
                        //
                        //dgDepartmentList.ItemsSource = null;

                        collection = new PagedCollectionView(objDoctor.UnitDepartmentDetails);
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                        dgDepartmentList.ItemsSource = collection;

                        //dgDepartmentList.ItemsSource = null;
                        //dgDepartmentList.ItemsSource = objDoctor.UnitDepartmentDetails;
                        //BindDepartmentGrid(lstDepartment);
                    }

                    if (objDoctor.UnitClassificationDetailsList != null)
                    {
                        List<clsUnitClassificationsDetailsVO> lstClassification = (List<clsUnitClassificationsDetailsVO>)dgClassificationList.ItemsSource;
                        foreach (var item1 in objDoctor.UnitClassificationDetailsList)
                        {
                            foreach (var item in lstClassification)
                            {
                                if (item.ClassificationID == item1.ClassificationID)
                                {
                                    //item.IsDefault = item1.IsDefault;
                                    item.IsAvailable = item1.IsAvailable;
                                    break;
                                }
                            }
                        }


                        dgClassificationList.ItemsSource = null;
                        dgClassificationList.ItemsSource = lstClassification;
                    }


                    FillSpecialization();
                    FillDoctorType();
                    FillDoctorCategory();
                    FillGender();
                    FillMarketingExecutives();
                    if (dgDoctorList.SelectedItem != null)
                    {
                        BankDetailPaging(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
                        DoctorAddressDetailsPaging(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
                    }
                    else
                    {
                        BankDetailPaging(DID);
                        DoctorAddressDetailsPaging(DID);
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

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
                    cmdDoctorServiceLinking.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdDoctorServiceLinking.IsEnabled = false;
                    tbIBankInfo.Visibility = Visibility.Collapsed;
                    tbIAddressInfo.Visibility = Visibility.Collapsed;
                    //tbIBankInfo.IsEnabled = false;
                    //tbIAddressInfo.IsEnabled = false;
                    tbIClassificationInfo.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    SaveBank.IsEnabled = true;
                    ModifyBank.IsEnabled = false;
                    SaveAddress.IsEnabled = true;
                    ModifyAddress.IsEnabled = false;

                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdDoctorServiceLinking.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    //tbIBankInfo.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdDoctorServiceLinking.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;

                    IsCancel = true;
                    break;
                case "ModifyBankInfo":
                    cmdNew.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    SaveBank.IsEnabled = false;
                    ModifyBank.IsChecked = true;
                    SaveAddress.IsEnabled = true;
                    ModifyAddress.IsEnabled = false;
                    IsCancel = true;
                    break;
                case "ModifyAddressInfo":
                    cmdNew.IsEnabled = false;
                    cmdDoctorServiceLinking.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    SaveBank.IsEnabled = true;
                    ModifyBank.IsEnabled = false;
                    SaveAddress.IsEnabled = false;
                    ModifyAddress.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdDoctorServiceLinking.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdDoctorServiceLinking.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    tbIBankInfo.Visibility = Visibility.Visible;
                    tbIAddressInfo.Visibility = Visibility.Visible;
                    tbIClassificationInfo.IsEnabled = true;
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
                    cmdDoctorServiceLinking.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    SaveBank.IsEnabled = false;
                    ModifyBank.IsEnabled = true;
                    SaveAddress.IsEnabled = true;
                    ModifyAddress.IsEnabled = false;
                    break;
                case "ViewAddressInfo":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdDoctorServiceLinking.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    SaveAddress.IsEnabled = false;
                    ModifyAddress.IsEnabled = true;
                    SaveBank.IsEnabled = false;
                    ModifyBank.IsEnabled = true;
                    break;

                default:
                    break;
            }
        }


        #endregion

        #region Reset all controls.
        private void ClearControl()
        {
            try
            {
                clsDoctorVO dvo = new clsDoctorVO();
                txtFirstName.Text = string.Empty;
                txtMiddleName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                dtpDOB1.SelectedDate = null;
                txtEducation.Text = string.Empty;
                txtExperience.Text = string.Empty;
                txtprovidentFound.Text = string.Empty;
                txtRegistrationNumber.Text = string.Empty;
                txtACN.Text = string.Empty;
                txtPAN.Text = string.Empty;
                txtName.Text = string.Empty;
                txtDoctorAddress.Text = string.Empty;
                txtContact1.Text = string.Empty;
                txtContact2.Text = string.Empty;
                //txtprovidentFound.Text = string.Empty;
                cmbSpecialization.SelectedValue = (long)0;
                cmbSubSpecialization.SelectedValue = (long)0;
                cmbDoctorType.SelectedValue = (long)0;
                cmbDoctorCategory.SelectedValue = (long)0;
                cmbMaritalStatus.SelectedValue = (long)0;
                cmbAddressType.SelectedValue = (long)0;
                txtAccountNumber.Text = string.Empty;
                txtMICRNumber.Text = string.Empty;
                txtIFSCCode.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtEmailID.Text = string.Empty;
                cmbBankName.SelectedValue = (long)0;
                cmbBranchName.SelectedValue = (long)0;
                RadioSaving.IsChecked = false;
                RadioCurrent.IsChecked = false;
                txtEmployeeNumber.Text = string.Empty;
                //dvo.DateofJoining = null;
                dtpDOJ.SelectedDate = null;

                cmbDoctorGender.SelectedValue = (long)0;
                imgPhoto.Source = null;

                //rohini
                SignatuerImage.Source = null;
                //List<clsUnitDepartmentsDetailsVO> lList = (List<cls UnitDepartmentsDetailsVO>)dgDepartmentList.ItemsSource;
                //foreach (var item in lList)
                //{
                //    item.Status = false;
                //    item.IsDefault = false;
                //}

                //dgDepartmentList.ItemsSource = null;
                //dgDepartmentList.ItemsSource = lList;       

                List<clsUnitClassificationsDetailsVO> List = (List<clsUnitClassificationsDetailsVO>)dgClassificationList.ItemsSource;
                if (List != null)
                {
                    foreach (var item in List)
                    {
                        item.IsAvailable = false;
                        //item.IsDefault = false;
                    }
                }
                dgClassificationList.ItemsSource = null;
                dgClassificationList.ItemsSource = List;
            }
            catch (Exception)
            {
                throw;
            }


        }


        private void ClearBankControl()
        {
            try
            {
                //clsDoctorVO dvo = new clsDoctorVO();
                dgbankDetails.SelectedItem = null;
                RadioSaving.IsChecked = true;
                RadioCurrent.IsChecked = false;
                txtAccountNumber.Text = string.Empty;
                txtMICRNumber.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtIFSCCode.Text = string.Empty;

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

        #endregion

        # region Validation
        private bool CheckValidation()
        {
            bool result = true;

            if ((MasterListItem)cmbSpecialization.SelectedItem == null)
            {
                cmbSpecialization.TextBox.SetValidation("Please Select Specialization");
                cmbSpecialization.TextBox.RaiseValidationError();
                cmbSpecialization.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbSpecialization.SelectedItem).ID == 0)
            {
                cmbSpecialization.TextBox.SetValidation("Please Select Specialization");
                cmbSpecialization.TextBox.RaiseValidationError();
                cmbSpecialization.Focus();
                result = false;
            }
            else
                cmbSpecialization.TextBox.ClearValidationError();

            if ((MasterListItem)cmbSubSpecialization.SelectedItem == null)
            {
                cmbSubSpecialization.TextBox.SetValidation("Please Select SubSpecialization");
                cmbSubSpecialization.TextBox.RaiseValidationError();
                cmbSubSpecialization.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbSubSpecialization.SelectedItem).ID == 0)
            {
                cmbSubSpecialization.TextBox.SetValidation("Please Select SubSpecialization");
                cmbSubSpecialization.TextBox.RaiseValidationError();
                cmbSubSpecialization.Focus();
                result = false;
            }
            else
                cmbSubSpecialization.TextBox.ClearValidationError();



            if ((MasterListItem)cmbDoctorType.SelectedItem == null)
            {
                cmbDoctorType.TextBox.SetValidation("Please Select Doctor Type");
                cmbDoctorType.TextBox.RaiseValidationError();
                cmbDoctorType.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbDoctorType.SelectedItem).ID == 0)
            {
                cmbDoctorType.TextBox.SetValidation("Please Select Doctor Type");
                cmbDoctorType.TextBox.RaiseValidationError();
                cmbDoctorType.Focus();
                result = false;
            }
            else
                cmbDoctorType.TextBox.ClearValidationError();

            if (txtEmailID.Text.Length > 0 && txtEmailID.Text.IsEmailValid() == false)
            {
                txtEmailID.SetValidation("Please Enter Valid Email Adderess");
                txtEmailID.RaiseValidationError();
                txtEmailID.Focus();
                result = false;
            }
            else
                txtEmailID.ClearValidationError();


            if (txtEducation.Text == "")
            {
                txtEducation.SetValidation("Please Enter Education");
                txtEducation.RaiseValidationError();
                txtEducation.Focus();
                result = false;
            }
            else
                txtEducation.ClearValidationError();


            if (((clsDoctorVO)this.DataContext).DOB == null)
            {
                dtpDOB1.SetValidation("Please Enter Date of Birth");
                dtpDOB1.RaiseValidationError();
                dtpDOB1.Focus();
                result = false;
            }
            else
            {
                if (((clsDoctorVO)this.DataContext).DOB >= DateTime.Today)
                {
                    dtpDOB1.SetValidation("Date of Birth should be less than Todays Date");
                    dtpDOB1.RaiseValidationError();
                    dtpDOB1.Focus();
                    result = false;
                }
                else
                    dtpDOB1.ClearValidationError();
            }
            if (((clsDoctorVO)this.DataContext).DateofJoining == null)
            {
                dtpDOJ.SetValidation("Please Select Joining Date ");
                dtpDOJ.RaiseValidationError();
                dtpDOJ.Focus();
                result = false;
            }
            else
                dtpDOJ.ClearValidationError();
            //-----------by rohini----------
            //if (dtpDOB1.SelectedDate >= DateTime.Now.Date)
            //{
            //    dtpDOB1.SetValidation("Date of Birth should be less than Todays Date");
            //    dtpDOB1.RaiseValidationError();
            //}
            //else if (dtpDOB1.SelectedDate == null)
            //{
            //    dtpDOB1.SetValidation("Please select the Date of birth");
            //    dtpDOB1.RaiseValidationError();
            //}
            //else if (dtpDOB1.SelectedDate >= dtpDOJ.SelectedDate)
            //{
            //    dtpDOJ.SetValidation("Date of Joining should be greater than Date of Birth");
            //    dtpDOJ.RaiseValidationError();
            //}

            //else
            //{
            //    dtpDOJ.ClearValidationError();
            //    dtpDOB1.ClearValidationError();
            //}
            if (((clsDoctorVO)this.DataContext).DateofJoining != null && ((clsDoctorVO)this.DataContext).DOB != null)
            {

                if (dtpDOB1.SelectedDate == dtpDOJ.SelectedDate || dtpDOB1.SelectedDate > dtpDOJ.SelectedDate)
                {
                    dtpDOJ.SetValidation("Date of Joining should be greater than Date of Birth");
                    dtpDOJ.RaiseValidationError();
                    dtpDOJ.Focus();
                    result = false;
                }
                else
                    dtpDOJ.ClearValidationError();

            }
            //by rohini
            if (((clsDoctorVO)this.DataContext).DateofJoining != null && ((clsDoctorVO)this.DataContext).DOB != null)
            {
                if (dtpDOB1.SelectedDate < dtpDOJ.SelectedDate)
                {
                    //System.DateTime firstDate = (DateTime)dtpDOB1.SelectedDate;
                    //System.DateTime secondDate = (DateTime)dtpDOJ.SelectedDate;

                    //System.TimeSpan diff = secondDate.Subtract(firstDate);
                    //System.TimeSpan diff1 = secondDate - firstDate;
                    //int totYrs = secondDate.AddYears(-(firstDate.Year)).Year;
                    DateTime zeroTime = new DateTime(1, 1, 1);
                    TimeSpan span = dtpDOJ.SelectedDate.Value.Subtract(dtpDOB1.SelectedDate.Value);
                    int years = (zeroTime + span).Year - 1;

                    if (years < 18)
                    {
                        dtpDOJ.SetValidation("Minimun 18 Years difference Required Between Date of Birth And Date Of Joining");
                        dtpDOJ.RaiseValidationError();
                        dtpDOJ.Focus();
                        result = false;
                    }
                    else
                        dtpDOJ.ClearValidationError();
                }
            }




            if (txtLastName.Text == "")
            {
                txtLastName.SetValidation("Please Enter Last Name");
                txtLastName.RaiseValidationError();
                txtLastName.Focus();
                result = false;
            }
            else
                txtLastName.ClearValidationError();

            if (txtFirstName.Text == "")
            {
                txtFirstName.SetValidation("Please Enter First Name");
                txtFirstName.RaiseValidationError();
                txtFirstName.Focus();
                result = false;
            }
            else
                txtFirstName.ClearValidationError();

            //if (txtMiddleName.Text == "")
            //{
            //    txtMiddleName.SetValidation("Please Enter Middle Name");
            //    txtMiddleName.RaiseValidationError();
            //    txtMiddleName.Focus();
            //    result = false;
            //}
            //else
            //    txtMiddleName.ClearValidationError();

            //if (((clsDoctorVO)this.DataContext).DOB < DateTime.Today)
            //{

            //    dtpDOB1.SetValidation("Birth Date can not be less than Today");
            //    dtpDOB1.RaiseValidationError();
            //    dtpDOB1.Focus();
            //    result = false;
            //}
            //else 

            if ((MasterListItem)cmbDoctorCategory.SelectedItem == null)
            {
                cmbDoctorCategory.TextBox.SetValidation("Please Select Doctor Category");
                cmbDoctorCategory.TextBox.RaiseValidationError();
                cmbDoctorCategory.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbDoctorCategory.SelectedItem).ID == 0)
            {
                cmbDoctorCategory.TextBox.SetValidation("Please Select Doctor Category");
                cmbDoctorCategory.TextBox.RaiseValidationError();
                cmbDoctorCategory.Focus();
                result = false;
            }
            else
                cmbDoctorCategory.TextBox.ClearValidationError();

            //***//
            if (((MasterListItem)cmbDoctorType.SelectedItem) != null)
            {
                if (((MasterListItem)cmbDoctorType.SelectedItem).ID == 4 && ((MasterListItem)cmbMarketingExecutives.SelectedItem).ID == 0)
                {
                    cmbMarketingExecutives.TextBox.SetValidation("Please Select Marketing Executives");
                    cmbMarketingExecutives.TextBox.RaiseValidationError();
                    cmbMarketingExecutives.Focus();
                    result = false;
                }

                else
                {
                    cmbMarketingExecutives.TextBox.ClearValidationError();
                }
            }

            else
            {
                cmbMarketingExecutives.TextBox.ClearValidationError();
            }

            if (txtPAN.Text == "")
            {
                txtPAN.SetValidation("Please Enter PAN Card Number");
                txtPAN.RaiseValidationError();
                txtPAN.Focus();
                result = false;
            }
            else
                 txtPAN.ClearValidationError();
            


            return result;
        }

        private bool IsDepartmentchk()
        {
            //clsDoctorVO objDoctorVO = (clsDoctorVO)this.DataContext;
            //clsUnitDepartmentsDetailsVO dept = ((List<clsUnitDepartmentsDetailsVO>)dgDepartmentList.ItemsSource).FirstOrDefault(p => p.Status == true);

            clsUnitDepartmentsDetailsVO dept = null;// ((PagedCollectionView)dgDepartmentList.ItemsSource)).FirstOrDefault(p => p.Status == true);
            foreach (clsUnitDepartmentsDetailsVO item in dgDepartmentList.ItemsSource)
            {
                if (item.Status == true)
                {
                    dept = new clsUnitDepartmentsDetailsVO();
                    dept = item;
                    break;
                }
            }

            if (dept != null)
            {
                return true;
            }


            else
            {
                string msgTitle = "Palash";
                string msgText = "Please Select Department";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW.Show();

                return false;
            }

        }

        //Added By Somnath
        private bool IsClassificationchk()
        {
            clsUnitClassificationsDetailsVO Classification = ((List<clsUnitClassificationsDetailsVO>)dgClassificationList.ItemsSource).FirstOrDefault(p => p.IsAvailable == true);

            if (Classification != null)
            {
                return true;
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Please Select Classification";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW.Show();

                return false;
            }

        }
        //End
        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();

            if (txtFirstName.Text == "")
            {
                txtFirstName.SetValidation("Please Enter First Name");
                txtFirstName.RaiseValidationError();
            }
            else
                txtFirstName.ClearValidationError();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();



        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();

            if (txtLastName.Text == "")
            {
                txtLastName.SetValidation("Please Enter Last Name");
                txtLastName.RaiseValidationError();
            }
            else
                txtLastName.ClearValidationError();

        }

        private void dtpDOB1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((clsDoctorVO)this.DataContext).DOB >= DateTime.Today)
            {

                dtpDOB1.SetValidation("Date of Birth should be less than Todays Date");
                dtpDOB1.RaiseValidationError();
                //dtpDOB1.Focus();
            }
            else if (((clsDoctorVO)this.DataContext).DOB == null)
            {
                dtpDOB1.SetValidation("Please select the Date of birth");
                dtpDOB1.RaiseValidationError();
            }
            else
                dtpDOB1.ClearValidationError();

        }

        private void txtEducation_LostFocus(object sender, RoutedEventArgs e)
        {
            //txtEducation.Text = txtEducation.Text.ToTitleCase();

            if (txtEducation.Text == "")
            {
                txtEducation.SetValidation("Please Enter Education");
                txtEducation.RaiseValidationError();
            }
            else
                txtEducation.ClearValidationError();


        }

        private void cmbSpecialization_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

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

        #endregion

        private void txtEmailID_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtEmailID.Text.Length > 0)
            //{
            //    if (txtEmailID.Text.IsEmailValid())
            //    {
            //        txtEmailID.ClearValidationError();
            //    }
            //    else
            //    {
            //        txtEmailID.SetValidation("Please Enter Valid Email Address");
            //        txtEmailID.RaiseValidationError();
            //        txtEmailID.Focus();
            //    }
            //}

            if (txtEmailID.Text.Length > 0)
            {
                if (txtEmailID.Text.IsEmailValid())
                    txtEmailID.ClearValidationError();
                else
                {
                    txtEmailID.SetValidation("Please Enter Valid Email-ID");
                    txtEmailID.RaiseValidationError();
                }
            }

            //if (txtEmailID.Text.Length > 0)
            //{
            //    if (!txtEmailID.Text.IsEmailValid())
            //    {
            //        txtEmailID.SetValidation("Please Enter Valid Email Address");
            //        txtEmailID.RaiseValidationError();
            //        txtEmailID.Focus();
            //    }
            //    else
            //    {
            //        txtEmailID.ClearValidationError();

            //    }
            //}
        }




        //Added By Somnath


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

            if (txtMICRNumber.Text.IsItNumber() == false)
            {
                txtMICRNumber.SetValidation("Please Enter Only Number value");
                txtMICRNumber.RaiseValidationError();
                txtMICRNumber.Focus();
                result = false;
            }
            else
                txtMICRNumber.ClearValidationError();


            if (txtIFSCCode.Text == "")
            {
                txtIFSCCode.SetValidation("Please Enter IFSC Code");
                txtIFSCCode.RaiseValidationError();
                txtIFSCCode.Focus();
                result = false;

            }
            else
                txtIFSCCode.ClearValidationError();

            return result;
        }

        private void cmdDoctorBankInfoSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveDoctor = true;

            SaveDoctor = CheckBankValidation();

            if (SaveDoctor == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Doctor bank information?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);

                msgW.Show();
            }
        }

        void msgW2_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveDoctorBankInformation();

            else if (result == MessageBoxResult.No)
            {
                ClearBankControl();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Bank Details adding Cancelled.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
            }
        }

        private void SaveDoctorBankInformation()
        {
            Indicatior.Show();
            //clsDoctorVO objDoctor = CreateUserObjectFromFormData();  
            //clsDoctorVO objDoctor = AddStatus();
            clsAddDoctorBankInfoBizActionVO BizAction = new clsAddDoctorBankInfoBizActionVO();
            BizAction.objDoctorBankDetail = new clsDoctorBankInfoVO();
            if (dgDoctorList.SelectedItem != null)
            {
                BizAction.objDoctorBankDetail.DoctorId = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId;
            }
            else
                BizAction.objDoctorBankDetail.DoctorId = DID;

            if (cmbBankName.SelectedItem != null)
                BizAction.objDoctorBankDetail.BankId = ((MasterListItem)cmbBankName.SelectedItem).ID;

            if (cmbBranchName.SelectedItem != null)
                BizAction.objDoctorBankDetail.BranchId = ((MasterListItem)cmbBranchName.SelectedItem).ID;

            if (txtAccountNumber.Text != null)
                BizAction.objDoctorBankDetail.AccountNumber = Convert.ToString(txtAccountNumber.Text);

            if (txtMICRNumber.Text != "")
            {
                BizAction.objDoctorBankDetail.MICRNumber = Convert.ToInt64(txtMICRNumber.Text);
            }

            if (txtIFSCCode.Text != null)
            {
                BizAction.objDoctorBankDetail.IFSCCode = Convert.ToString(txtIFSCCode.Text);
            }

            if (RadioSaving.IsChecked == true)
            {
                BizAction.objDoctorBankDetail.AccountType = true;
            }

            if (RadioCurrent.IsChecked == true)
            {
                BizAction.objDoctorBankDetail.AccountType = false;
            }
            if (txtAddress.Text != null)
                BizAction.objDoctorBankDetail.BranchAddress = txtAddress.Text;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                //SetCommandButtonState("Save");

                if (arg.Error == null)
                {
                    if (dgDoctorList.SelectedItem != null)
                    {
                        FetchBankData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
                    }
                    else
                    {
                        FetchBankData(DID);
                    }
                    //ClearControl();
                    ClearBankControl();
                    //objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Bank Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Camp  Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void FetchBankData(long iDoctorID)
        {
            clsGetDoctorBankInfoBizActionVO BizAction = new clsGetDoctorBankInfoBizActionVO();
            BizAction.DoctorBankDetailList = new List<clsDoctorBankInfoVO>();

            BizAction.DoctorID = iDoctorID;

            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //dgDoctorList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDoctorBankInfoBizActionVO)arg.Result).DoctorBankDetailList != null)
                    {
                        //dgDoctorList.ItemsSource = ((clsGetDoctorDetailListForDoctorMasterBizActionVO)arg.Result).DoctorDetails;
                        clsGetDoctorBankInfoBizActionVO result = arg.Result as clsGetDoctorBankInfoBizActionVO;

                        if (result.DoctorBankDetailList != null)
                        {
                            DoctorBankDataList.Clear();
                            DoctorBankDataList.TotalItemCount = ((clsGetDoctorBankInfoBizActionVO)arg.Result).TotalRows;
                            foreach (clsDoctorBankInfoVO item in result.DoctorBankDetailList)
                            {
                                DoctorBankDataList.Add(item);

                            }
                        }

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdDoctorBankInfoModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyDoctor = true;
            ModifyDoctor = CheckBankValidation();
            if (ModifyDoctor == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Update the Doctor Bank details?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW4_OnMessageBoxClosed);

                msgW1.Show();
            }

        }

        void msgW4_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                UpdateDoctorBankInformation();
        }

        private void UpdateDoctorBankInformation()
        {

            clsUpdateDoctorBankInfoVO BizAction = new clsUpdateDoctorBankInfoVO();
            BizAction.objDoctorBankDetail = new clsDoctorBankInfoVO();

            if (dgDoctorList.SelectedItem != null)
            {
                BizAction.objDoctorBankDetail.DoctorId = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId;
            }
            else
                BizAction.objDoctorBankDetail.DoctorId = DID;

            BizAction.objDoctorBankDetail.ID = ((clsDoctorBankInfoVO)dgbankDetails.SelectedItem).ID;
            BizAction.objDoctorBankDetail.AccountNumber = txtAccountNumber.Text;
            BizAction.objDoctorBankDetail.BranchAddress = txtAddress.Text;
            BizAction.objDoctorBankDetail.MICRNumber = Convert.ToInt64(txtMICRNumber.Text);
            BizAction.objDoctorBankDetail.IFSCCode = txtIFSCCode.Text; //***//
            if (RadioSaving.IsChecked == true)
            {
                BizAction.objDoctorBankDetail.AccountType = true;
            }

            if (RadioCurrent.IsChecked == true)
            {
                BizAction.objDoctorBankDetail.AccountType = false;
            }


            if (cmbBankName.SelectedItem != null)
                BizAction.objDoctorBankDetail.BankId = ((MasterListItem)cmbBankName.SelectedItem).ID;

            if (cmbBranchName.SelectedItem != null)
                BizAction.objDoctorBankDetail.BranchId = ((MasterListItem)cmbBranchName.SelectedItem).ID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("ModifyBankInfo");
                if (arg.Error == null)
                {

                    FetchBankData(((clsDoctorBankInfoVO)dgbankDetails.SelectedItem).DoctorId);

                    ClearBankControl();
                    // objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Bank Details Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    SaveBank.IsEnabled = true;
                    SaveAddress.IsEnabled = true; ;
                    ModifyAddress.IsEnabled = false;
                    ModifyBank.IsEnabled = false;
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while updating Doctor Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                    SaveBank.IsEnabled = false;
                    ModifyBank.IsEnabled = true;
                    SaveAddress.IsEnabled = true; ;
                    ModifyAddress.IsEnabled = false;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void hlbViewDoctorBankInfo_Click(object sender, RoutedEventArgs e)
        {
            if (dgDoctorList.SelectedItem != null)
            {
                FillDoctorBankData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
            }
            else
                FillDoctorBankData(DID);

            SaveBank.IsEnabled = false;
            ModifyBank.IsEnabled = true;
            SaveAddress.IsEnabled = true;
            ModifyAddress.IsEnabled = false;

        }

        clsDoctorBankInfoVO objDoctorBankInfo = new clsDoctorBankInfoVO();

        private void FillDoctorBankData(long iDoctorID)
        {
            clsGetDoctorBankInfoByIdVO BizAction = new clsGetDoctorBankInfoByIdVO();
            //BizAction.objDoctorBankDetail = new clsDoctorBankInfoVO();
            BizAction.DoctorID = iDoctorID;
            BizAction.ID = ((clsDoctorBankInfoVO)dgbankDetails.SelectedItem).ID;
            // BizAction.BankId = ((clsDoctorBankInfoVO)dgbankDetails.SelectedItem).BankId;
            //BizAction.BankId = objDoctor.DoctorBankInformation.BankId; 
            //BizAction.objDoctorBankDetail.BankId = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorBankInformation.BankId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    SetCommandButtonState("ViewBankInfo");

                    // ClearControl();

                    objDoctorBankInfo = ((clsGetDoctorBankInfoByIdVO)ea.Result).objDoctorBankDetail;

                    if (objDoctorBankInfo != null)
                    {
                        txtAccountNumber.Text = objDoctorBankInfo.AccountNumber;

                    }
                    if (objDoctorBankInfo != null)
                    {
                        if (objDoctorBankInfo.AccountTypeName == "Saving")
                        {

                            RadioSaving.IsChecked = true;
                        }

                        else
                            RadioCurrent.IsChecked = true;

                    }

                    if (objDoctorBankInfo != null)
                    {
                        txtMICRNumber.Text = Convert.ToString(objDoctorBankInfo.MICRNumber);

                    }
                    if (objDoctorBankInfo != null)
                    {
                        txtAddress.Text = Convert.ToString(objDoctorBankInfo.BranchAddress);

                    }

                    if (objDoctorBankInfo != null) //***//
                    {
                        txtIFSCCode.Text = Convert.ToString(objDoctorBankInfo.IFSCCode);

                    }

                    FillBankDetail();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

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
            //else if (txtContact1.Text.Length < 10)
            //{
            //    txtContact1.SetValidation("Enter Atleast 10 Digit Number");
            //    txtContact1.RaiseValidationError();
            //    txtContact1.Focus();
            //    result = false;
            //}



            if (txtContact2.Text == "")
            {
                txtContact2.SetValidation("Please Enter Phone Number");
                txtContact2.RaiseValidationError();
                txtContact2.Focus();
                result = false;
            }
            else if (txtContact2.Text.IsItNumber() == false)
            {
                txtContact2.SetValidation("Enter Number Only");
                txtContact2.RaiseValidationError();
                txtContact2.Focus();
                result = false;
            }
            else
                txtContact2.ClearValidationError();

            //else if (txtContact2.Text.Length < 10)
            //{
            //    txtContact2.SetValidation("Enter Atleast 10 Digit Number");
            //    txtContact2.RaiseValidationError();
            //    txtContact2.Focus();
            //    result = false;
            //}


            return result;
        }

        private void cmdDoctorAddressInfoSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveDoctor = true;

            SaveDoctor = CheckAddressValidation();

            if (SaveDoctor == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Doctor Address information?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW3_OnMessageBoxClosed);

                msgW.Show();
            }
        }

        void msgW3_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveDoctorAddressInformation();
            else if (result == MessageBoxResult.No)
            {
                ClearAddressControl();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Address Details adding Cancelled.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
            }
        }

        private void SaveDoctorAddressInformation()
        {
            Indicatior.Show();
            //clsDoctorVO objDoctor = CreateUserObjectFromFormData();  
            //clsDoctorVO objDoctor = AddStatus();
            clsAddDoctorAddressInfoBizActionVO BizAction = new clsAddDoctorAddressInfoBizActionVO();
            BizAction.objDoctorBankDetail = new clsDoctorAddressInfoVO();

            if (dgDoctorList.SelectedItem != null)
            {
                BizAction.objDoctorBankDetail.DoctorId = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId;
            }
            else
                BizAction.objDoctorBankDetail.DoctorId = DID;

            if (cmbAddressType.SelectedItem != null)
                BizAction.objDoctorBankDetail.AddressTypeID = ((MasterListItem)cmbAddressType.SelectedItem).ID;

            if (txtName.Text != null)
                BizAction.objDoctorBankDetail.Name = Convert.ToString(txtName.Text);

            if (txtContact1.Text != null)
            {
                BizAction.objDoctorBankDetail.Contact1 = txtContact1.Text;
            }
            if (txtContact2.Text != null)
            {
                BizAction.objDoctorBankDetail.Contact2 = txtContact2.Text;
            }

            if (txtDoctorAddress.Text != null)
                BizAction.objDoctorBankDetail.Address = txtDoctorAddress.Text;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                //SetCommandButtonState("Save");

                if (arg.Error == null && ((clsAddDoctorAddressInfoBizActionVO)arg.Result).SuccessStatus == 0)
                {
                    if (dgDoctorList.SelectedItem != null)
                    {
                        FetchDoctorAddressData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
                    }
                    else
                        FetchDoctorAddressData(DID);

                    // ClearControl();
                    ClearAddressControl();
                    //objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Address Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }

                else if (arg.Error == null && ((clsAddDoctorAddressInfoBizActionVO)arg.Result).SuccessStatus == 1)
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Contact Number Already Exist.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    Indicatior.Close();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Camp  Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void FetchDoctorAddressData(long iDoctorID)
        {

            clsGetDoctorAddressInfoBizActionVO BizAction = new clsGetDoctorAddressInfoBizActionVO();
            BizAction.DoctorAddressDetailList = new List<clsDoctorAddressInfoVO>();
            BizAction.DoctorID = iDoctorID;
            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //dgDoctorList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDoctorAddressInfoBizActionVO)arg.Result).DoctorAddressDetailList != null)
                    {
                        //dgDoctorList.ItemsSource = ((clsGetDoctorDetailListForDoctorMasterBizActionVO)arg.Result).DoctorDetails;
                        clsGetDoctorAddressInfoBizActionVO result = arg.Result as clsGetDoctorAddressInfoBizActionVO;

                        if (result.DoctorAddressDetailList != null)
                        {
                            DoctorAddressDataList.Clear();
                            DoctorAddressDataList.TotalItemCount = ((clsGetDoctorAddressInfoBizActionVO)arg.Result).TotalRows;
                            foreach (clsDoctorAddressInfoVO item in result.DoctorAddressDetailList)
                            {
                                //.Add(item);
                                //item.AddressType = ((MasterListItem)cmbAddressType.SelectedItem).Description;
                                DoctorAddressDataList.Add(item);
                            }
                        }

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void hlbViewDoctorAddressInfo_Click(object sender, RoutedEventArgs e)
        {
            SaveBank.IsEnabled = true;
            ModifyBank.IsEnabled = false;
            SaveAddress.IsEnabled = false;
            ModifyAddress.IsEnabled = true;
            if (dgDoctorList.SelectedItem != null)
            {
                FillDoctorAddressData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
            }
            else
            {
                FillDoctorAddressData(DID);
            }


        }

        clsDoctorAddressInfoVO objDoctorAddressInfo = new clsDoctorAddressInfoVO();

        private void FillDoctorAddressData(long iDoctorID)
        {

            clsGetDoctorAddressInfoByIdVO BizAction = new clsGetDoctorAddressInfoByIdVO();

            BizAction.DoctorID = iDoctorID;
            BizAction.AddressTypeId = ((clsDoctorAddressInfoVO)dgAddressDetails.SelectedItem).AddressTypeID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    SetCommandButtonState("ViewAddressInfo");
                    objDoctorAddressInfo = ((clsGetDoctorAddressInfoByIdVO)ea.Result).objDoctorAddressDetail;
                    if (objDoctorAddressInfo != null)
                    {
                        cmbAddressType.SelectedValue = objDoctorAddressInfo.AddressTypeID;
                    }

                    if (objDoctorAddressInfo != null)
                    {
                        txtName.Text = Convert.ToString(objDoctorAddressInfo.Name);

                    }
                    if (objDoctorAddressInfo != null)
                    {
                        txtDoctorAddress.Text = Convert.ToString(objDoctorAddressInfo.Address);

                    }
                    if (objDoctorAddressInfo != null)
                    {
                        txtContact1.Text = Convert.ToString(objDoctorAddressInfo.Contact1);

                    }
                    if (objDoctorAddressInfo != null)
                    {
                        txtContact2.Text = Convert.ToString(objDoctorAddressInfo.Contact2);

                    }


                    FillAddressType();
                    // FillBankDetail();
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
                string msgText = "Are you sure you want to Update the Doctor Address details?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW5_OnMessageBoxClosed);

                msgW1.Show();
            }
        }

        void msgW5_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                UpdateDoctorAddressInformation();
        }

        private void UpdateDoctorAddressInformation()
        {

            clsUpdateDoctorAddressInfoVO BizAction = new clsUpdateDoctorAddressInfoVO();
            BizAction.objDoctorAddressDetail = new clsDoctorAddressInfoVO();
            //(clsDoctorVO)this.DataContext).DoctorAddressInformation


            BizAction.objDoctorAddressDetail.DoctorId = ((clsDoctorAddressInfoVO)dgAddressDetails.SelectedItem).DoctorId;
            // BizAction.objDoctorAddressDetail.DoctorId = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId;
            BizAction.objDoctorAddressDetail.ID = ((clsDoctorAddressInfoVO)dgAddressDetails.SelectedItem).ID;
            BizAction.objDoctorAddressDetail.AddressTypeID = ((MasterListItem)cmbAddressType.SelectedItem).ID;
            BizAction.objDoctorAddressDetail.Address = txtDoctorAddress.Text;
            BizAction.objDoctorAddressDetail.Name = txtName.Text;
            BizAction.objDoctorAddressDetail.Contact1 = txtContact1.Text;
            BizAction.objDoctorAddressDetail.Contact2 = txtContact2.Text;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("ModifyAddressInfo");
                if (arg.Error == null && ((clsUpdateDoctorAddressInfoVO)arg.Result).SuccessStatus == 0)
                {
                    if (dgDoctorList.SelectedItem != null && ((clsUpdateDoctorAddressInfoVO)arg.Result).SuccessStatus == 0)
                    {
                        FetchDoctorAddressData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
                    }
                    else
                        FetchDoctorAddressData(DID);
                    ClearAddressControl();
                    // objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Address Details Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    SaveBank.IsEnabled = true;
                    ModifyBank.IsEnabled = false;
                    SaveAddress.IsEnabled = true;
                    ModifyAddress.IsEnabled = false;


                }
                else if (arg.Error == null && ((clsUpdateDoctorAddressInfoVO)arg.Result).SuccessStatus == 1)
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Contact Number Already Exist.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    Indicatior.Close();
                    SaveBank.IsEnabled = true;
                    ModifyBank.IsEnabled = false;
                    SaveAddress.IsEnabled = false;
                    ModifyAddress.IsEnabled = true;
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while updating Doctor Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                    SaveBank.IsEnabled = true;
                    ModifyBank.IsEnabled = false;
                    SaveAddress.IsEnabled = false;
                    ModifyAddress.IsEnabled = true;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            //PhotoWindow phWin = new PhotoWindow();
            ////if (this.DataContext != null)
            ////    phWin.MyPhoto = ((clsDoctorVO)this.DataContext).Photo;
            //if (objDoctor.Photo != null)
            //    phWin.MyPhoto = objDoctor.Photo;

            //phWin.OnSaveButton_Click += new RoutedEventHandler(phWin_OnSaveButton_Click);
            //phWin.Show();


            PhotoWindow phWin = new PhotoWindow();
            if (this.DataContext != null)
                phWin.MyPhoto = ((clsDoctorVO)this.DataContext).Photo;
            phWin.OnSaveButton_Click += new RoutedEventHandler(phWin_OnSaveButton_Click);
            phWin.Show();
        }

        void phWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //if (((CIMS.Forms.PhotoWindow)sender).DialogResult == true)
            //    //((clsDoctorVO)this.DataContext).Photo = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;
            //    objDoctor.Photo = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;
            if (((CIMS.Forms.PhotoWindow)sender).DialogResult == true)
                ((clsDoctorVO)this.DataContext).Photo = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;
        }
        //ROHINEE
        public byte[] DoctorSignature { get; set; }
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

            // ((clsDoctorVO)this.DataContext).Signature = null;
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


                    ((clsDoctorVO)this.DataContext).Signature = data;
                }
                catch (Exception)
                {
                    HtmlPage.Window.Alert("Error Loading File");
                }

            }
        }

        //Added by Ashish Z.
        private void cmdDoctorServiceLinking_Click(object sender, RoutedEventArgs e)
        {
            if (dgDoctorList.SelectedItem != null)
            {
                frmDoctorServiceRateWiseLinkChildWindow Win = new frmDoctorServiceRateWiseLinkChildWindow();
                Win.DoctorID = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId;
                Win.DoctorName = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorName;
                Win.DocCategory = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorCategoryId;
                //Win.MaxHeight = this.ActualHeight * 0.8;
                //Win.MaxWidth = this.ActualWidth * 0.7;
                Win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Select the Doctor to link with Service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }


        private void dtpDOJ_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (((clsDoctorVO)this.DataContext).DOB == null)
            //{
            //    dtpDOB1.SetValidation("Birth Date is required");
            //    dtpDOB1.RaiseValidationError();
            //}
            //else if (((clsDoctorVO)this.DataContext).DOB.Value.Date >= DateTime.Now.Date)
            //{
            //    dtpDOB1.SetValidation("Birth Date can not be greater than Todays Date");
            //    dtpDOB1.RaiseValidationError();
            //}
            //else
            //    dtpDOB1.ClearValidationError();
            //if (dtpDOJ.SelectedDate != null)
            //{
            //if (((clsDoctorVO)this.DataContext).DOB.Value.Date >= dtpDOJ.SelectedDate)
            //{

            //    dtpDOJ.SetValidation("Date of Joining should be greater than Date of Birth");
            //    dtpDOJ.RaiseValidationError();
            //}
            //else
            //    dtpDOJ.ClearValidationError();
            //}


            //if (dtpDOB1.SelectedDate >= DateTime.Now.Date)
            //{
            //    dtpDOB1.SetValidation("Date of Birth should be less than Todays Date");
            //    dtpDOB1.RaiseValidationError();
            //}
            //else if (dtpDOB1.SelectedDate == null)
            //{
            //    dtpDOB1.SetValidation("Please select the Date of birth");
            //    dtpDOB1.RaiseValidationError();
            //}
            //else if (dtpDOB1.SelectedDate >= dtpDOJ.SelectedDate)
            //{
            //    dtpDOJ.SetValidation("Date of Joining should be greater than Date of Birth");
            //    dtpDOJ.RaiseValidationError();
            //}

            //else
            //{
            //    //dtpDOJ.ClearValidationError();
            //    dtpDOB1.ClearValidationError();
            //}
        }

        private void txtContact2_TextChanged(object sender, TextChangedEventArgs e)
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
        //
        //private void FillDoctorDetails(long iID)
        //{
        //    clsGetDoctorDetailListForDoctorMasterByIDBizActionVO BizAction = new clsGetDoctorDetailListForDoctorMasterByIDBizActionVO();
        //    BizAction.DoctorDetails = (clsDoctorVO)this.DataContext;

        //    BizAction.DoctorID = iID;
        //     Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            SetCommandButtonState("Modify");
        //            if (dgDoctorList.SelectedItem != null)
        //               // objAnimation.Invoke(RotationType.Forward);

        //            ((clsDoctorVO)this.DataContext).DoctorId = ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.DoctorId;
        //            txtFirstName.Text= ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.FirstName;
        //            txtMiddleName.Text = ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.MiddleName;
        //            txtLastName.Text= ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.LastName;
        //            dtpDOB1.SelectedDate= ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.DOB;
        //            ((clsDoctorVO)this.DataContext).SpecializationDis= ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.SpecializationDis;
        //            ((clsDoctorVO)this.DataContext).Specialization = ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.Specialization;
        //            ((clsDoctorVO)this.DataContext).SubSpecializationDis = ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.SubSpecializationDis;
        //            ((clsDoctorVO)this.DataContext).SubSpecialization = ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.SubSpecialization;
        //            ((clsDoctorVO)this.DataContext).DoctorTypeDis = ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.DoctorTypeDis;
        //            ((clsDoctorVO)this.DataContext).DoctorType = ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)arg.Result).DoctorDetails.DoctorType;

        //            FillSpecialization();
        //            FillDoctorType();

        //        }
        //        else
        //            HtmlPage.Window.Alert("Error occured while processing.");
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();


        //}

        //private void FillDepartmentList()
        //{
        //    if (dgDoctorList.SelectedItem != null)
        //    {

        //        clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO BizActionObj = new clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO();
        //        BizActionObj.DoctorDetails = new List<clsDoctorVO>();

        //        BizActionObj.DoctorID = ObjclsDoctorVO.DoctorId;



        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        dgDepartmentList.ItemsSource = null;
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null)
        //            {
        //                if (((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).DoctorDetails != null)
        //                {
        //                    dgDepartmentList.ItemsSource = ((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).DoctorDetails;

        //                }
        //            }
        //            else
        //                HtmlPage.Window.Alert("Error occured while processing.");

        //        };
        //        client.ProcessAsync(BizActionObj, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //}





        //SetCommandButtonState("Modify");


        //   if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
        //   {
        //       ObjclsDoctorVO = ((clsDoctorVO)dgDoctorList.SelectedItem);
        //       FillDoctorDetails(ObjclsDoctorVO.DoctorId);

        //   }

        //   clsGetDepartmentListForDoctorMasterBizActionVO BizActionObj = new clsGetDepartmentListForDoctorMasterBizActionVO();

        //   Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //   PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, ea) =>
        //        {
        //            if (ea.Error == null && ea.Result != null)
        //            {
        //                SetCommandButtonState("Modify");

        //                clsDoctorVO objDoctor = new clsDoctorVO();
        //                if (objDoctor.DepartmentDetails != null)
        //                {
        //                    List<clsDepartmentsDetailsVO> lstDepartment = (List<clsDepartmentsDetailsVO>)dgDepartmentList.ItemsSource;
        //                    foreach (var item1 in objDoctor.DepartmentDetails)
        //                    {

        //                        foreach (var item in lstDepartment)
        //                        {
        //                            if (item.DepartmentID == item1.DepartmentID)
        //                            {
        //                                item.IsDefault = item1.IsDefault;
        //                                item.Status = item1.Status;

        //                            }
        //                        }
        //                    }


        //                    dgDepartmentList.ItemsSource = null;
        //                    dgDepartmentList.ItemsSource = lstDepartment;
        //                }
        //            }
        //        };
        //    client.ProcessAsync(BizActionObj, new clsUserVO());
        //    client.CloseAsync();

        //   objAnimation.Invoke(RotationType.Forward);



        CaptureSource _captureSource = new CaptureSource();
        CaptureSource _SpousecaptureSource = new CaptureSource();
        WriteableBitmap _images = null;
        WriteableBitmap _Spouseimages = null;
        byte[] data;
        FileInfo fi;
        private void cmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            //((clsDoctorVO)this.DataContext).Photo = null;
            OpenFileDialog OpenFile = new OpenFileDialog();

            OpenFile.Multiselect = false;
            OpenFile.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            OpenFile.FilterIndex = 1;

            if (OpenFile.ShowDialog() == true)
            {

                WriteableBitmap imageSource = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                try
                {
                    imageSource.SetSource(OpenFile.File.OpenRead());
                    imgPhoto.Source = imageSource;
                    // MyPhoto = imageToByteArray(OpenFile.File);


                    WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                    bmp.Render(imgPhoto, new MatrixTransform());
                    bmp.Invalidate();

                    //int[] p = bmp.Pixels;
                    //int len = p.Length * 4;
                    //byte[] result = new byte[len]; // ARGB
                    //Buffer.BlockCopy(p, 0, result, 0, len);

                    using (Stream stream = OpenFile.File.OpenRead())
                    {
                        data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);
                        fi = OpenFile.File;
                    }

                    ((clsDoctorVO)this.DataContext).Photo = data;
                }
                catch (Exception)
                {
                    HtmlPage.Window.Alert("Error Loading File");
                }

            }
        }

        private void cmdStartCapture_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (_captureSource != null)
                {
                    _captureSource.Stop();
                    VideoBrush vidBrush = new VideoBrush();
                    vidBrush.SetSource(_captureSource);
                    WebcamCapture.Fill = vidBrush; // paint the brush on the rectangle

                    // request user permission and display the capture
                    if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
                    {
                        _captureSource.Start();

                    }
                    cmdStartCapture.Visibility = Visibility.Collapsed;
                    cmdCaptureImage.Visibility = Visibility.Visible;
                    borderimage.Visibility = System.Windows.Visibility.Collapsed;
                    borderwebcap.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void cmdStopCapture_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCaptureImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_captureSource != null)
                {
                    // capture the current frame and add it to our observable collection                
                    _captureSource.CaptureImageAsync();
                    //_SpousecaptureSource.CaptureImageCompleted += ((s, args) =>
                    //{
                    //    _Spouseimages = (args.Result);
                    //    imgSpousePhoto.Source = _Spouseimages;
                    //});

                    _captureSource.CaptureImageCompleted += ((s, args) =>
                    {
                        _images = (args.Result);
                        imgPhoto.Source = _images;

                        WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                        bmp.Render(imgPhoto, new MatrixTransform());
                        bmp.Invalidate();

                        int[] p = bmp.Pixels;
                        int len = p.Length * 4;
                        byte[] result = new byte[len]; // ARGB
                        Buffer.BlockCopy(p, 0, result, 0, len);

                        ((clsDoctorVO)this.DataContext).Photo = result;

                    });
                    borderimage.Visibility = System.Windows.Visibility.Visible;
                    borderwebcap.Visibility = System.Windows.Visibility.Collapsed;
                    cmdStartCapture.Visibility = Visibility.Visible;
                    cmdCaptureImage.Visibility = Visibility.Collapsed;
                    _captureSource.Stop();

                }
            }
            catch (System.Exception ex)
            {


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


        //***//--------------------------------------------

        private void FillMarketingExecutives()
        {
            clsGetMarketingExecutivesListVO BizAction = new clsGetMarketingExecutivesListVO();

            BizAction.IsMarketingExecutives = true;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMarketingExecutivesListVO)e.Result).MarketingExecutivesList);

                    cmbMarketingExecutives.ItemsSource = null;
                    cmbMarketingExecutives.ItemsSource = objList;
                    
                    if (cmbMarketingExecutives.IsEnabled == false)
                    {
                        cmbMarketingExecutives.SelectedItem = objList[0];
                    }
                }
                if (cmbMarketingExecutives.IsEnabled == true)
                {


                    if (this.DataContext != null)
                    {
                        cmbMarketingExecutives.SelectedValue = ((clsDoctorVO)this.DataContext).MarketingExecutivesID;
                    }

                    if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                    {
                        cmbMarketingExecutives.SelectedValue = objDoctor.MarketingExecutivesID;
                        cmbMarketingExecutives.UpdateLayout();

                    }
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbDoctorType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbDoctorType.SelectedItem).ID == 4)
            {
                cmbMarketingExecutives.IsEnabled = true;
            }

            else
            {
                cmbMarketingExecutives.IsEnabled = false;               
                FillMarketingExecutives();            
            }


        }

        private void txtIdentityNumber_KeyDown(object sender, KeyEventArgs e)
        {         
                textBefore = ((TextBox)sender).Text;
                selectionStart = ((TextBox)sender).SelectionStart;
                selectionLength = ((TextBox)sender).SelectionLength;
         
        }      

        private void txtIdentityNumber_TextChanged(object sender, TextChangedEventArgs e)
        {          
                if (((TextBox)sender).Text.Length > 10)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }           
        }

        //private void AttachDoc_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgDoctorList.SelectedItem != null)
        //    {
        //        frmPatientScanDocument win = new frmPatientScanDocument();
        //        win.Title = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
        //        win.Show();
        //    }
        //    else
        //    {
        //        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgW1.Show();
        //    }
        //}


        private void AttachDoc_Click(object sender, RoutedEventArgs e)
        {
            if (dgDoctorList.SelectedItem != null)
            {
                {
                    
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID = ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId; ;
                    

                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    WebClient c2 = new WebClient();
                    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                    c2.OpenReadAsync(new Uri("PalashDynamics" + ".xap", UriKind.Relative));
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Doctor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }            

        }


        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;

                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == "PalashDynamics.dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance("PalashDynamics.Forms.PatientView.frmPatientScanDocument") as UIElement;

                if (myData != null)
                {                   
                    ((ChildWindow)myData).Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void txtIFSC_KeyDown(object sender, KeyEventArgs e)
        {           
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        private void txtIFSC_TextChanged(object sender, TextChangedEventArgs e)
        {           

            if (!((TextBox)sender).Text.IsIFSCCodeValid())
            {               
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                
            }

        }

        private void txtIFSC_LostFocus(object sender, RoutedEventArgs e)
        {
            txtIFSCCode.Text = txtIFSCCode.Text.ToUpper();
        }

        

       
    }
}
