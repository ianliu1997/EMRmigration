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
using PalashDynamics.Administration;
using System.Reflection;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Data;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects;
using CIMS.Forms;

namespace OPDModule.Forms
{
    public partial class DoctorMasterChildwindow : ChildWindow, IInitiateCIMS
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
           // FetchBankData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
        }

        void DoctorAddressDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //FetchDoctorAddressData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
        #endregion
        public event RoutedEventHandler OnSaveButton_Click;
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "PRO":

                    Flagref = true;
                    break;
            }

        }
        public long DoctorId=0;
        public long MarketingExecutivesID = 0;

        public DoctorMasterChildwindow()
        {
            InitializeComponent();
            clsDoctorVO DVO = new clsDoctorVO();
            
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsDoctorVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

            DataListPageSize = 15;
          

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
               
               
                FillDoctorType();
                FillSpecialization();
                FillMaritalStatus();
                FillGender();
                SetComboboxValue();
                ClearControl();
                txtFirstName.Focus();
                Indicatior.Close();
                FillMarketingExecutives();

            }
            IsPageLoded = true;
            txtFirstName.Focus();
            txtFirstName.UpdateLayout();
            cmbSpecialization.TextBox.BorderBrush = new SolidColorBrush(Colors.Red);

        }


        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            
            this.DataContext = new clsDoctorVO();
            clsDoctorVO DVO = new clsDoctorVO();
            ClearControl();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Doctor Details";
            //objAnimation.Invoke(RotationType.Forward);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
            //SetCommandButtonState("Cancel");
            //this.DataContext = null;

            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            //mElement.Text = "";

            //objAnimation.Invoke(RotationType.Backward);
            //// FetchData();

            //if (IsCancel == true)
            //{
            //    mElement = (TextBlock)rootPage.FindName("SampleHeader");
            //    mElement.Text = "Clinic Configuration";

            //    UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmClinicConfiguration") as UIElement;
            //    ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            //    FetchData();
            //}
            //else
            //{
            //    IsCancel = true;
            //}
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

                        cmbDoctorType.ItemsSource = null;
                        cmbDoctorType.ItemsSource = objList;

                        cmbDoctorType.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorTypeForReferral;
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

                //if ((clsDoctorVO)dgDoctorList.SelectedItem != null)
                //{
                //    cmbSpecialization.SelectedValue = objDoctor.Specialization;
                //    cmbSpecialization.UpdateLayout();

                //}

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

        private void SetComboboxValue()
        {
            cmbSpecialization.SelectedValue = ((clsDoctorVO)this.DataContext).Specialization;
            cmbSubSpecialization.SelectedValue = ((clsDoctorVO)this.DataContext).SubSpecialization;
            cmbDoctorType.SelectedValue = ((clsDoctorVO)this.DataContext).DoctorType;
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

            if (SaveDoctor == true )//&& IsDepartmentchk() && IsClassificationchk()
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Doctor Master?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
              
            }
            //else
            //{
            //   // txtContact1.ClearValidationError();
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required fields to save the Doctor Master", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //}
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {  
                SaveDoctorMaster();
                cmdSave.IsEnabled = false;
            }
           
        }
        private clsDoctorVO AddStatus()
        {
            clsDoctorVO objDoctorVO = (clsDoctorVO)this.DataContext;
           
            return objDoctorVO;
        }
        private void SaveDoctorMaster()
        {
            Indicatior.Show();
           
            clsDoctorVO objDoctor = AddStatus();

            clsAddDoctorMasterBizActionVO BizAction = new clsAddDoctorMasterBizActionVO();
            BizAction.DoctorDetails = (clsDoctorVO)this.DataContext;
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
            if (txtContact1.Text.Trim() != string.Empty)
            {
                BizAction.DoctorDetails.Contact1 = txtContact1.Text.Trim();
            }
            BizAction.DoctorDetails.AddressTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AddressTypeID;
            if (txtDoctorAddress.Text.Trim() != string.Empty)
            {
                BizAction.DoctorDetails.Address = txtDoctorAddress.Text.Trim();
            }
            //***//
            if (cmbMarketingExecutives.SelectedItem != null)
                BizAction.DoctorDetails.MarketingExecutivesID = ((MasterListItem)cmbMarketingExecutives.SelectedItem).ID;
            //---------
            BizAction.DoctorDetails.IsFromReferralDoctorChildWindow = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("Save");

                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsAddDoctorMasterBizActionVO)arg.Result).SuccessStatus != null && ((clsAddDoctorMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Aready exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            this.DialogResult = true;
                        }
                        else
                        {
                            Indicatior.Close();

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Master Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (re) =>
                                    {
                                        this.DialogResult = true;
                                        if (arg.Result != null)
                                            DoctorId = ((clsAddDoctorMasterBizActionVO)arg.Result).DoctorDetails.DoctorId;
                                            MarketingExecutivesID = ((MasterListItem)cmbMarketingExecutives.SelectedItem).ID; //***//
                                        if (OnSaveButton_Click != null)
                                            OnSaveButton_Click(this, new RoutedEventArgs());
                                    };
                            msgW1.Show();
                           
                        }
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


        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();

        }

        private void FetchData()
        {
            clsGetDoctorDetailListForDoctorMasterBizActionVO BizAction = new clsGetDoctorDetailListForDoctorMasterBizActionVO();
            BizAction.DoctorDetails = new List<clsDoctorVO>();


            //if (txtFirstName1.Text != null)
            //    BizAction.FirstName = txtFirstName1.Text;

            //if (txtLastName2.Text != null)
            //    BizAction.LastName = txtLastName2.Text;

           // else
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
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
            SetCommandButtonState("View");
            //ClearControl();
          //  FillData(((clsDoctorVO)dgDoctorList.SelectedItem).DoctorId);


            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            //mElement.Text = " : " + ((clsDoctorVO)dgDoctorList.SelectedItem).DoctorName;
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
                    //if (dgDoctorList.SelectedItem != null)
                    //    objAnimation.Invoke(RotationType.Forward);


                    objDoctor = ((clsGetDoctorDetailListForDoctorMasterByIDBizActionVO)ea.Result).DoctorDetails;
                    


                    if (objDoctor.MaritalStatusId != null)
                    {
                        cmbMaritalStatus.SelectedValue = objDoctor.MaritalStatusId;
                    }



                   
                    this.DataContext = objDoctor;
                    IUnitId = objDoctor.UnitID;
            

                    FillSpecialization();
                    FillDoctorType();
                    FillGender();
            
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
                //case "Load":
                //    cmdModify.IsEnabled = false;
                //    cmdSave.IsEnabled = true;
                //    cmdCancel.IsEnabled = true;
                //    cmdNew.IsEnabled = true;
                //    IsCancel = true;
                //    break;
                //case "New":
                //    cmdModify.IsEnabled = false;
                //    cmdSave.IsEnabled = true;
                //    cmdNew.IsEnabled = false;
                //    tbIBankInfo.Visibility = Visibility.Collapsed;
                //    tbIAddressInfo.Visibility = Visibility.Collapsed;
                //    //tbIBankInfo.IsEnabled = false;
                //    //tbIAddressInfo.IsEnabled = false;
                //    tbIClassificationInfo.IsEnabled = true;
                //    cmdCancel.IsEnabled = true;
                //    Save.IsEnabled = true;
                //    modify.IsEnabled = false;
                //    SaveAddress.IsEnabled = true;
                //    modifyAddress.IsEnabled = false;

                //    IsCancel = false;
                //    break;
                //case "Save":
                //    cmdNew.IsEnabled = true;
                //    cmdSave.IsEnabled = false;
                //    cmdCancel.IsEnabled = true;
                //    //tbIBankInfo.IsEnabled = false;
                //    IsCancel = true;
                //    break;
                //case "Modify":
                //    cmdNew.IsEnabled = true;
                //    cmdModify.IsEnabled = false;
                //    cmdCancel.IsEnabled = true;

                //    IsCancel = true;
                //    break;
                //case "ModifyBankInfo":
                //    cmdNew.IsEnabled = false;
                //    cmdModify.IsEnabled = true;
                //    cmdCancel.IsEnabled = true;
                //    Save.IsEnabled = true;
                //    IsCancel = true;
                //    break;
                //case "ModifyAddressInfo":
                //    cmdNew.IsEnabled = false;
                //    cmdModify.IsEnabled = true;
                //    cmdCancel.IsEnabled = true;
                //    SaveAddress.IsEnabled = true;
                //    IsCancel = true;
                //    break;
                //case "Cancel":
                //    cmdNew.IsEnabled = true;
                //    cmdModify.IsEnabled = false;
                //    cmdSave.IsEnabled = false;
                //    cmdCancel.IsEnabled = true;
                //    break;

                //case "View":
                //    cmdModify.IsEnabled = true;
                //    cmdSave.IsEnabled = false;
                //    cmdNew.IsEnabled = false;
                //    cmdCancel.IsEnabled = true;
                //    tbIBankInfo.Visibility = Visibility.Visible;
                //    tbIAddressInfo.Visibility = Visibility.Visible;
                //    tbIClassificationInfo.IsEnabled = true;
                //    Save.IsEnabled = true;
                //    modify.IsEnabled = false;
                //    SaveAddress.IsEnabled = true;
                //    modifyAddress.IsEnabled = false;
                //    IsCancel = false;
                //    break;
                //case "ViewBankInfo":
                //    cmdModify.IsEnabled = true;
                //    cmdSave.IsEnabled = false;
                //    cmdNew.IsEnabled = false;
                //    cmdCancel.IsEnabled = true;
                //    Save.IsEnabled = false;
                //    modify.IsEnabled = true;
                //    //SaveAddress.IsEnabled = false;
                //    //modifyAddress.IsEnabled = true;
                //    break;
                //case "ViewAddressInfo":

                //    cmdNew.IsEnabled = false;
                //    cmdCancel.IsEnabled = true;
                //    Save.IsEnabled = false;
                //    modify.IsEnabled = true;
                //    SaveAddress.IsEnabled = false;
                //    modifyAddress.IsEnabled = true;
                //    break;

                //default:
                //    break;
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
                
                //txtprovidentFound.Text = string.Empty;
                cmbSpecialization.SelectedValue = (long)0;
                cmbSubSpecialization.SelectedValue = (long)0;
                cmbDoctorType.SelectedValue = (long)0;
                cmbMaritalStatus.SelectedValue = (long)0;
                txtEmailID.Text = string.Empty;
                txtEmployeeNumber.Text = string.Empty;
                dvo.DateofJoining = null;
                dtpDOJ.SelectedDate = null;
                cmbDoctorGender.SelectedValue = (long)0;
                DoctorPhoto.Source = null;
                objDoctor.Photo = null;
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

            if (IsPageLoded)
            {
               

                //if ((MasterListItem)cmbSubSpecialization.SelectedItem == null)
                //{
                //    cmbSubSpecialization.TextBox.SetValidation("Please Select SubSpecialization");
                //    cmbSubSpecialization.TextBox.RaiseValidationError();
                //    cmbSubSpecialization.Focus();
                //    result = false;
                //}
                //else if (((MasterListItem)cmbSubSpecialization.SelectedItem).ID == 0)
                //{
                //    cmbSubSpecialization.TextBox.SetValidation("Please Select SubSpecialization");
                //    cmbSubSpecialization.TextBox.RaiseValidationError();
                //    cmbSubSpecialization.Focus();
                //    result = false;
                //}
                //else
                //    cmbSubSpecialization.TextBox.ClearValidationError();



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
                if (txtEmailID.Text.Length > 0)
                if (txtEmailID.Text.IsEmailValid() == true)
                {
                    txtEmailID.ClearValidationError();
                }
                else
                {
                    txtEmailID.SetValidation("Please Enter Valid Email Adderess");
                    txtEmailID.RaiseValidationError();
                    txtEmailID.Focus();
                    result = false;
                }
            }
            //if (txtEducation.Text == "")
            //{
            //    txtEducation.SetValidation("Please Enter Education");
            //    txtEducation.RaiseValidationError();
            //    txtEducation.Focus();
            //    result = false;
            //}
            //else
            //    txtEducation.ClearValidationError();

            if (((clsDoctorVO)this.DataContext).DOB > DateTime.Today)
            {

                dtpDOB1.SetValidation("Date of birth can not be greater than Today");
                dtpDOB1.RaiseValidationError();
                dtpDOB1.Focus();
                result = false;
            }
            //else if (((clsDoctorVO)this.DataContext).DOB == null)
            //{
            //    dtpDOB1.SetValidation("Please select the Date of birth");
            //    dtpDOB1.RaiseValidationError();
            //    dtpDOB1.Focus();
            //    result = false;
            //}
            else
                dtpDOB1.ClearValidationError();

 
           

            
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

            return result;
        }

     

      
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
            if (((clsDoctorVO)this.DataContext).DOB > DateTime.Today)
            {

                dtpDOB1.SetValidation("Date of birth can not be greater than Today");
                dtpDOB1.RaiseValidationError();
                //dtpDOB1.Focus();
            }
            //else 
            if (((clsDoctorVO)this.DataContext).DOB == null)
            {
                dtpDOB1.SetValidation("Please select the Date of birth");
                dtpDOB1.RaiseValidationError();
            }
            else
                dtpDOB1.ClearValidationError();


            //if (((clsCampMasterVO)this.DataContext).FromDate < DateTime.Today)
            //{

            //    dtpFromDate.SetValidation("From Date can not be less than Today");
            //    dtpFromDate.RaiseValidationError();
            //    dtpFromDate.Focus();
            //    result = false;
            //}
            //else if (dtpFromDate.SelectedDate == null)
            //{
            //    dtpFromDate.SetValidation("Please Select From Date ");
            //    dtpFromDate.RaiseValidationError();
            //    dtpFromDate.Focus();
            //    result = false;
            //}
            //else
            //    dtpFromDate.ClearValidationError();

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
            if (txtEmailID.Text.Length > 0)
            {
                if (txtEmailID.Text.IsEmailValid())
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
        }

        private void txtEmailID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            PhotoWindow phWin = new PhotoWindow();
            //if (this.DataContext != null)
            //    phWin.MyPhoto = ((clsDoctorVO)this.DataContext).Photo;
            if (objDoctor.Photo != null)
                phWin.MyPhoto = objDoctor.Photo;

            phWin.OnSaveButton_Click += new RoutedEventHandler(phWin_OnSaveButton_Click);
            phWin.Show();
        }

        void phWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CIMS.Forms.PhotoWindow)sender).DialogResult == true)
                //((clsDoctorVO)this.DataContext).Photo = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;
                objDoctor.Photo = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;

        }

        private void btnAddSignature_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtContact2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsMobileNumberValid() && textBefore != null)
              //  if (!((TextBox)sender).Text.IsMobileNumberValid() && textBefore != null)
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

                    cmbMarketingExecutives.SelectedItem = objList[0];
                }



            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        
    }
}

