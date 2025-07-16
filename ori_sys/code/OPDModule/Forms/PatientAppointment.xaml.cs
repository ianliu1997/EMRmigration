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
using OPDModule;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using System.Windows.Controls.Data;
using CIMS;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using System.ComponentModel;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using System.Windows.Browser;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.EmailServiceReference;
using System.Globalization;
using System.Windows.Data;
using MessageBoxControl;
using PalashDynamics;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Administration.UserRights;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects.IPD;

namespace CIMS.Forms
{
    public partial class PatientAppointment : ChildWindow, IInitiateCIMS
    {
        int ClickedFlag = 0;
      

        #region Variable Declaration
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCloseButton_Click;
        public string Email { get; set; }
        public double AppointmentTimeSlots;
        public long GUnitID = 0;
        clsAppointmentVO Item;
        clsUserRightsVO objUser;
        public bool Status { get; set; }
        public bool TimeStatus { get; set; }

        public bool IsFromEMR = false; //Added By YK 08042017

        public string MrNo;
        public string LastName;

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        PatientVisitDetails win = new PatientVisitDetails();


        bool IsPatientExist = false;
        public clsGetPatientBizActionVO OBJPatient { get; set; }

        bool UseApplicationID = true;
        bool UseApplicationDoctorID = true;

        public List<clsAppointmentVO> ChkFromTimeList { get; set; }
        List<clsSchedule> listSchedule = new List<clsSchedule>();

        DateTime? DocSchedule1FromTime { get; set; }
        DateTime? DocSchedule1ToTime { get; set; }

        DateTime? DocSchedule2FromTime { get; set; }
        DateTime? DocSchedule2ToTime { get; set; }

        bool Schedule1Flag = false;
        bool Schedule2Flag = false;
        public long AppointmentID = 0;
        public long AppointmentUnitID = 0;
        public bool IsFromAppointment = false;
        public Boolean IsFromFollowUp { get; set; }
        public string AppointmentMode = null;
        public DateTime FollowUpDate;
        public long FollowUpAppoinment;
        public long FollowUpDoctorID;
        public long GenderID;
        public long FollowUPDepartment;
        public string FollowUPRemark;
        bool MesFlag = true;
        int count = 0;
        #endregion

        #region IInitiateCIMS Members
        private bool EditMode = false;
        DateTime? HomeFollowUpDate;
        public void Initiate(string Mode)
        {
           
            AppointmentMode = Mode;

            if (Mode == "NEW")
            {
                IsFromAppointment = false;
                HomeFollowUpDate = DateTime.Now.Date.Date;
            }
            else if (Mode == "HomeFollowUp")
            {
                IsFromAppointment = false;
                HomeFollowUpDate = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FollowUpDate;
                dtpAppointmentDate.IsEnabled = false;
            }
            else
            {
                IsFromAppointment = true;
                ((IApplicationConfiguration)App.Current).SelectedPatient = null;                
            }
            if (Mode == "FollowUp")
            {
                this.IsFromFollowUp = true;
            }              

            while (true)
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                {
                    this.DataContext = new clsAppointmentVO()
                    {
                        UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                        DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                        DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                        AppointmentDate = DateTime.Now.Date.Date,

                    };
                    //btnPatientVisit.Visibility = System.Windows.Visibility.Collapsed;
                    btnPatientVisit.IsEnabled = false;
                    this.Title = "Appointment- New Patient";
                    break;
                }
                if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                {
                    this.DataContext = new clsAppointmentVO()
                    {
                        UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                        DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                        DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                        AppointmentDate = DateTime.Now.Date.Date
                    };
                    //  btnPatientVisit.Visibility = System.Windows.Visibility.Collapsed;
                    btnPatientVisit.IsEnabled = false;
                    break;

                }               

                if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0) //&& ((IApplicationConfiguration)App.Current).SelectedPatient.IsDischarged != false By Bhushanp 19012017
                {                   
                    this.Title = "Appointment-" + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                      " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    this.DataContext = new clsAppointmentVO
                    {
                        MrNo = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo,
                        PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        FirstName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName,
                        MiddleName = ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName,
                        LastName = ((IApplicationConfiguration)App.Current).SelectedPatient.LastName,
                        // DOB = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth,                 
                        UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID,
                        DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                        DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                        AppointmentDate = HomeFollowUpDate,                       
                        //AppointmentDate = DateTime.Now.Date.Date,                     
                        //UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitID                       

                    };
                    ((clsAppointmentVO)this.DataContext).MrNo = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    ((clsAppointmentVO)this.DataContext).FirstName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName;
                    ((clsAppointmentVO)this.DataContext).MiddleName = ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName;
                    ((clsAppointmentVO)this.DataContext).LastName = ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    //------------vikrant-------------------
                    if (IsFromFollowUp)
                    {
                        ((clsAppointmentVO)this.DataContext).DoctorId = FollowUpDoctorID;
                        ((clsAppointmentVO)this.DataContext).DepartmentId = FollowUPDepartment;
                        ((clsAppointmentVO)this.DataContext).AppointmentDate = FollowUpDate;
                        ((clsAppointmentVO)this.DataContext).Remark = FollowUPRemark;

                        //((clsAppointmentVO)this.DataContext).GenderId =;//



                    }
                    //------------vikrant-------------------


                    // ((clsAppointmentVO)this.DataContext).DOB = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                    //((clsAppointmentVO)this.DataContext).IsAge = ((IApplicationConfiguration)App.Current).SelectedPatient.IsAge;
                    //if (((IApplicationConfiguration)App.Current).SelectedPatient.IsAge == false)
                    //{
                    //    ((clsAppointmentVO)this.DataContext).DOB = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                    //}
                    //else
                    //{
                    //    DOB = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge;
                    //}


                    clsGetPatientBizActionVO BizAction1 = new clsGetPatientBizActionVO();
                    BizAction1.PatientDetails = new clsPatientVO();
                    BizAction1.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    BizAction1.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;


                    //if (((IApplicationConfiguration)App.Current).SelectedPatient != null || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    //{
                    txtContactNo1.IsEnabled = false;
                    txtFirstName.IsEnabled = false;
                    txtMiddleName.IsEnabled = false;
                    txtLastName.IsEnabled = false;
                    cmbBloodGroup.IsEnabled = false;
                    cmbGender.IsEnabled = false;
                    dtpDOB1.IsEnabled = false;
                    cmbMaritalStatus.IsEnabled = false;
                    txtFamilyName.IsEnabled = false;
                    txtMobileCountryCode.IsEnabled = false;
                    txtResiSTD.IsEnabled = false;
                    txtResiCountryCode.IsEnabled = false;
                    //    txtFaxNo.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                    txtContactNo2.IsEnabled = false;
                    txtYY.IsEnabled = false;
                    txtMM.IsEnabled = false;
                    txtDD.IsEnabled = false;
                    cmbNationality.IsEnabled = false; //***//
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsDocAttached != true)
                    {
                        DocLabel.Visibility = Visibility.Visible;
                    }
                    //////}
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    Client1.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                OBJPatient = new clsGetPatientBizActionVO();
                                OBJPatient = (clsGetPatientBizActionVO)arg.Result;

                                if (OBJPatient != null)
                                {
                                    cmbMaritalStatus.SelectedValue = OBJPatient.PatientDetails.MaritalStatusID;
                                    cmbBloodGroup.SelectedValue = OBJPatient.PatientDetails.BloodGroupID;
                                    cmbGender.SelectedValue = OBJPatient.PatientDetails.GenderID;
                                    cmbSpRegistration.SelectedValue = OBJPatient.PatientDetails.SpecialRegistrationID;
                                    //txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                                    //txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                                    //txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");

                                    //Added by Ajit jadhav
                                    //* Added Date - 6/10/2016
                                    cmbNationality.SelectedValue = OBJPatient.PatientDetails.NationalityID;
                                    //***//---------
                                    if (OBJPatient.PatientDetails.GeneralDetails.IsAge == false)
                                    {
                                        dtpDOB1.SelectedDate = OBJPatient.PatientDetails.GeneralDetails.DateOfBirth;
                                        txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                                        txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                                        txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");
                                    }
                                    else
                                    {
                                        DOB = OBJPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge;
                                        txtYY.Text = ConvertDate(OBJPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "YY");
                                        txtMM.Text = ConvertDate(OBJPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "MM");
                                        txtDD.Text = ConvertDate(OBJPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "DD");
                                    }

                                    //if (((IApplicationConfiguration)App.Current).SelectedPatient.IsAge == false)
                                    //{
                                    //    dtpDOB1.SelectedDate = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                                    //    txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                                    //    txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                                    //    txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");
                                    //}
                                    //else
                                    //{
                                    //    txtYY.Text = ConvertDate(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge, "YY");
                                    //    txtMM.Text = ConvertDate(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge, "MM");
                                    //    txtDD.Text = ConvertDate(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge, "DD");
                                    //}
                                    txtContactNo1.Text = OBJPatient.PatientDetails.ContactNo1;
                                    txtContactNo2.Text = OBJPatient.PatientDetails.ContactNo2;
                                    //txtFaxNo.Text = OBJPatient.PatientDetails.FaxNo;
                                    txtMobileCountryCode.Text = OBJPatient.PatientDetails.MobileCountryCode.ToString();

                                    txtResiCountryCode.Text = OBJPatient.PatientDetails.ResiNoCountryCode.ToString();
                                    txtResiSTD.Text = OBJPatient.PatientDetails.ResiSTDCode.ToString();
                                    txtEmail.Text = OBJPatient.PatientDetails.Email;
                                    txtFamilyName.Text = OBJPatient.PatientDetails.FamilyName;
                                    if (txtResiCountryCode.Text.Equals("0") || txtResiSTD.Text.Equals("0"))
                                    {
                                        txtResiCountryCode.Text = " ";
                                        txtResiSTD.Text = " ";
                                    }
                                }
                            }
                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Adding Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };
                    Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client1.CloseAsync();
                }
                break;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PatientAppointment()
        {
            InitializeComponent();
            Item = new clsAppointmentVO();
        }
        public PatientAppointment(clsAppointmentVO Item1)
        {
            InitializeComponent();
            //  this.Item = new clsAppointmentVO();
            this.Item = Item1;
            AppointmentID = Item1.AppointmentID;
            this.DataContext = Item1;
        }

        private void frmAppointment_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {

                Indicatior = new WaitIndicator();
                Indicatior.Show();

                tpFromTime.IsEnabled = false;
                tpToTime.IsEnabled = false;


                Email = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Email;
                AppointmentTimeSlots = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AppointmentSlot;

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                      && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                    {
                        //do  nothing
                    }
                    else
                        cmdSave.IsEnabled = false;
                }

                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);

                FillGender();
                FillMaritalStatus();
                FillBloodGroup();
                FillReasonList();
                FillUnitList();
                FillNationalityMaster();//***//
                //   FillDoctor(Item.UnitId, Item.DoctorId);
                //FillDepartmentList(Item.UnitId);
                FillSpecializationList();
                FillSpecialRegistration();
               

                txtMRNoPatientInformation.Focus();
                Indicatior.Close();


                if (Item.IsReschedule == true)
                {
                    txtContactNo1.IsEnabled = false;
                    txtFirstName.IsEnabled = false;
                    txtMiddleName.IsEnabled = false;
                    txtLastName.IsEnabled = false;
                    cmbBloodGroup.IsEnabled = false;
                    cmbGender.IsEnabled = false;
                    dtpDOB1.IsEnabled = false;
                    cmbMaritalStatus.IsEnabled = false;
                    txtFamilyName.IsEnabled = false;
                    txtMobileCountryCode.IsEnabled = false;
                    txtResiSTD.IsEnabled = false;
                    txtResiCountryCode.IsEnabled = false;
                    // txtFaxNo.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                    txtContactNo2.IsEnabled = false;
                    txtYY.IsEnabled = false;
                    txtMM.IsEnabled = false;
                    txtDD.IsEnabled = false;
                    Reasontxt.Visibility = Visibility.Visible;
                    txtReasonReschedule.Visibility = Visibility.Visible;
                    DateTime? Dob1 = Item.DOB;
                    //Added by Ajit date 17/10/2016
                    cmbAUnit.IsEnabled = false;
                    //***//

                    if (Item.IsAge)
                    {
                        Dob1 = Item.DateOfBirthFromAge;
                    }
                    else
                    {
                        dtpDOB1.SelectedDate = Item.DOB;
                    }

                    if (Item.DOB != null)  //* Added by - Ajit Jadhav  //* Added Date - 24/9/2016   //* Comments - if DOB is null then age save null     
                    {
                        if (Item.IsAge)
                        {
                            //  dtpDOB1.SelectedDate = null;
                            txtYY.Text = ConvertDate(Dob1, "YY");
                            txtMM.Text = ConvertDate(Dob1, "MM");
                            txtDD.Text = ConvertDate(Dob1, "DD");
                        }
                        else
                        {
                            txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                            txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                            txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");

                        }
                    }

                    if (Item.FromTime != null)
                    {
                        //tpFromTime.Value.Value.TimeOfDay = Item.FromTime;
                        tpFromTime.Value = Item.FromTime;
                    }
                    if (Item.ToTime != null)
                    {
                        tpToTime.Value = Item.ToTime;
                    }

                    txtMobileCountryCode.Text = Item.MobileCountryCode;
                    txtContactNo1.Text = Item.ContactNo1;
                    txtResiCountryCode.Text = Item.ResiNoCountryCode.ToString();
                    txtResiSTD.Text = Item.ResiSTDCode.ToString();
                    txtContactNo2.Text = Item.ContactNo2;
                    txtEmail.Text = Item.Email;
                    txtMRNoPatientInformation.IsEnabled = false;
                    CmdPatientAppSearch.IsEnabled = false;
                    this.Title = "Patient Appointment- " + Item.FirstName + " " + Item.LastName;
                    IsFromAppointment = true;
                    btnPatientVisit.IsEnabled = false;

                    if (txtResiCountryCode.Text.Equals("0") || txtResiSTD.Text.Equals("0"))
                    {
                        txtResiCountryCode.Text = " ";
                        txtResiSTD.Text = " ";
                    }
                    // CmdPatientAppSearch.Visibility = System.Windows.Visibility.Collapsed;
                    //txtFaxNo.Text = Item.FaxNo;
                }
                else

                    chkValidations();

              
            }
            IsPageLoded = true;
            //Commented by Bhushanp 19012017
            //if(((IApplicationConfiguration)App.Current).SelectedPatient.IsDischarged == false)
            //{

            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient is already admitted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //}
        }

        #region Selected Index Changed

        private void cmbUnitAppointmentSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmbDoctorAppointmentSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmbDepartmentAppointmentSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtpAppointmentSummary_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }



        private void txtMrno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtMRNoPatientInformation.Text == "")
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Patient MRNO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                }
                else
                {
                    clsCheckMRnoBizActionVO BizActionObj = new clsCheckMRnoBizActionVO();
                    BizActionObj.Details = new clsAppointmentVO();
                    BizActionObj.MRNO = txtMRNoPatientInformation.Text;


                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {

                            Status = ((clsCheckMRnoBizActionVO)arg.Result).SuccessStatus;

                            if (Status == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW8 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Registration Not Found Against This MRNO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW8.Show();
                            }
                            else
                            {
                                GetActiveAdmissionOfPatient(txtMRNoPatientInformation.Text.ToString());
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();

                }
            }
        }

        #endregion

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #region FillCombobox
        ///Method Purpose: Filled combobox from MasterList.

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

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;


                }

                if (this.DataContext != null)
                {
                    cmbGender.SelectedValue = ((clsAppointmentVO)this.DataContext).GenderId;
                }

                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    if (OBJPatient != null)
                    {
                        if (OBJPatient.PatientDetails != null)
                        {
                            if (OBJPatient.PatientDetails.GenderID != 0)
                            {
                                cmbGender.SelectedValue = OBJPatient.PatientDetails.GenderID;
                            }
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillSpecialRegistration()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SpecialRegistrationMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbSpRegistration.ItemsSource = null;
                    cmbSpRegistration.ItemsSource = objList;
                    //                    cmbSpRegistration.SelectedItem = objList[0];




                }

                if (this.DataContext != null)
                {
                    cmbSpRegistration.SelectedValue = ((clsAppointmentVO)this.DataContext).SpecialRegistrationID;
                }

                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    if (OBJPatient != null)
                    {
                        if (OBJPatient.PatientDetails != null)
                        {
                            if (OBJPatient.PatientDetails.SpecialRegistrationID != 0)
                            {
                                cmbSpRegistration.SelectedValue = OBJPatient.PatientDetails.SpecialRegistrationID;
                            }
                        }
                    }
                }

                if (IsFromEMR == false)         //Added By YK 08042017 for get date from EMR from 
                {
                    dtpAppointmentDate.DisplayDateStart = DateTime.Today;
                }
                else
                {
                    dtpAppointmentDate.DisplayDateStart = FollowUpDate;//Added By YK 08042017

                    //  SearchAppointmentByMrNo(MrNo);
                    getDetailsFromEMRFollowup(MrNo);
                    //dtpAppointmentDate.SelectedDate = FollowUpDate; 
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

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
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbMaritalStatus.ItemsSource = null;
                    cmbMaritalStatus.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    cmbMaritalStatus.SelectedValue = ((clsAppointmentVO)this.DataContext).MaritalStatusId;
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

        private void FillBloodGroup()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_BloodGroupMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbBloodGroup.ItemsSource = null;
                    cmbBloodGroup.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    cmbBloodGroup.SelectedValue = ((clsAppointmentVO)this.DataContext).BloodId;
                }

                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    if (OBJPatient != null)
                    {
                        if (OBJPatient.PatientDetails != null)
                        {
                            if (OBJPatient.PatientDetails.BloodGroupID != 0)
                            {
                                cmbBloodGroup.SelectedValue = OBJPatient.PatientDetails.BloodGroupID;
                            }
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();


        }

        private void FillReasonList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_AppointmentReasonMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbAppointmentType.ItemsSource = null;
                    cmbAppointmentType.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbAppointmentType.SelectedValue = ((clsAppointmentVO)this.DataContext).AppointmentReasonId;
                }
                if (IsFromFollowUp == true)
                {
                    cmbAppointmentType.SelectedValue = FollowUpAppoinment;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillUnitList()
        {
            clsGetUnitDetailsByIDBizActionVO BizAction = new clsGetUnitDetailsByIDBizActionVO();
            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizAction.IsUserWise = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUnitDetailsByIDBizActionVO)ea.Result).ObjMasterList);
                    cmbAUnit.ItemsSource = null;
                    cmbAUnit.ItemsSource = objList;
                    cmbAUnit.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbAUnit.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();



            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem(0, "- Select -"));
            //        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
            //        cmbAUnit.ItemsSource = null;
            //        cmbAUnit.ItemsSource = objList;
            //        cmbAUnit.SelectedItem = objList[0];

            //    }
            //    if (this.DataContext != null)
            //    {
            //        cmbAUnit.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);    
        }

        //* Added by - Ajit Jadhav
        //* Added Date - 9/8/2016
        //* Comments - Get Nationalitymaster Data
        private void FillNationalityMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_NationalityMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbNationality.ItemsSource = null;
                        cmbNationality.ItemsSource = objList;
                    }
                    if (this.DataContext != null)
                    {
                        cmbNationality.SelectedValue = ((clsAppointmentVO)this.DataContext).NationalityId;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    {
                        if (OBJPatient != null)
                        {
                            if (OBJPatient.PatientDetails != null)
                            {
                                if (OBJPatient.PatientDetails.NationalityID != 0)
                                {
                                    cmbNationality.SelectedValue = OBJPatient.PatientDetails.NationalityID;
                                }
                            }
                        }
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //***//----------------------------------
        private void FillDepartmentList(long iUnitId)
        {
            clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO BizActionVo = new clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO();
            BizActionVo.IsUnitWise = true;
            BizActionVo.IsClinical = true;  // flag use to Show/not Clinical Departments  02032017
            BizActionVo.UnitID = iUnitId;   // fill Unitwise Departments  02032017

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    if (((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem != null)
                    {
                        objList.AddRange(((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem);
                    }
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbDepartment.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;
                    }
                }
            };
            client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);



            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            //if (iUnitId > 0)
            //    BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem(0, "- Select -"));
            //        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
            //        if (((MasterListItem)cmbAUnit.SelectedItem).ID == 0)
            //        {
            //            var results = from a in objList
            //                          group a by a.ID into grouped
            //                          select grouped.First();
            //            objList = results.ToList();
            //        }
            //        else
            //        {

            //            var results = from a in objList
            //                          group a by a.ID into grouped
            //                          select grouped.First();
            //            objList = results.ToList();
            //        }
            //        cmbDepartment.ItemsSource = null;

            //        if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
            //            cmbDepartment.ItemsSource = objList;

            //        if (this.DataContext != null)
            //        {
            //            if (UseApplicationID == true)
            //            {
            //                cmbDepartment.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;
            //                UseApplicationID = false;
            //            }
            //        }
            //        else
            //        {
            //            cmbDepartment.SelectedValue = objList[0].ID;
            //            cmbDepartment.SelectedItem = objList[0];
            //        }
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillDoctor(long IUnitId, long iDeptId)
        {
            Indicatior.Show();
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            if ((MasterListItem)cmbAUnit.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId;
            }
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbDoctorAddNewAppointment.ItemsSource = null;

                    if (((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbDoctorAddNewAppointment.ItemsSource = objList;

                    // COmmented BY CDS On 30/11/20165
                    //if((MasterListItem)cmbDepartment.SelectedItem != null && ((MasterListItem)cmbDepartment.SelectedItem).ID>0)
                    //{
                    //    if (this.DataContext != null)
                    //    {
                    //        if (UseApplicationDoctorID == true)
                    //        {

                    //            cmbDoctorAddNewAppointment.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;
                    //            UseApplicationDoctorID = false;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    cmbDoctorAddNewAppointment.SelectedValue = objList[0].ID;
                    //    cmbDoctorAddNewAppointment.SelectedItem = objList[0];
                    //}
                    //END



                    if (this.DataContext != null)
                    {
                        if (UseApplicationDoctorID == true)
                        {

                            cmbDoctorAddNewAppointment.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;
                            UseApplicationDoctorID = false;
                        }
                    }
                    else
                    {
                        cmbDoctorAddNewAppointment.SelectedValue = objList[0].ID;
                        cmbDoctorAddNewAppointment.SelectedItem = objList[0];
                    }

                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            Indicatior.Close();
        }

        //****************************************************************************************************************
        private void FillDoctorScheduleWise()
        {
            GetDoctorScheduleWiseVO BizAction = new GetDoctorScheduleWiseVO();
            BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();

            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (cmbDepartment.SelectedItem != null)
                BizAction.DepartmentId = ((MasterListItem)cmbDepartment.SelectedItem).ID;

            BizAction.Day = Convert.ToInt64(DateTime.Now.DayOfWeek) + 1;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsDoctorScheduleDetailsVO> ObjScheduleList = new List<clsDoctorScheduleDetailsVO>();
                    ObjScheduleList = ((GetDoctorScheduleWiseVO)e.Result).DoctorScheduleDetailsList;

                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    foreach (var item in ObjScheduleList)
                    {
                        DateTime ST = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.StartTime.Value.Hour, item.StartTime.Value.Minute, 0);
                        DateTime ET = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.EndTime.Value.Hour, item.EndTime.Value.Minute, 0);




                        if (DateTime.Now >= ST && DateTime.Now <= ET)
                        // if (dtpAppointmentDate.SelectedDate.Value. >= ST && DocSchedule1ToTime <= ET)
                        {

                            foreach (MasterListItem item1 in ((List<MasterListItem>)(cmbDoctorAddNewAppointment.ItemsSource)))
                            {
                                if (item1.ID == item.DoctorID)
                                {
                                    objList.Add(item1);
                                }
                            }
                        }

                    }



                    if (objList.Count > 0)
                    {
                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }




                    cmbDoctorAddNewAppointment.ItemsSource = null;
                    cmbDoctorAddNewAppointment.ItemsSource = objList;

                    //if (this.DataContext != null)
                    //{
                    //    if (UsePrevDoctorID == true)
                    //    {
                    //        cmbDoctor.SelectedValue = ((clsVisitVO)this.DataContext).DoctorID;
                    //        // UsePrevDoctorID = false;

                    //        if (((clsVisitVO)this.DataContext).DoctorID == 0)
                    //        {
                    //            cmbDoctorAddNewAppointment.TextBox.SetValidation("Please select the Doctor");
                    //            cmbDoctorAddNewAppointment.TextBox.RaiseValidationError();
                    //        }
                    //        // UsePrevDoctorID = false;
                    //    }
                    //else
                    //{
                    if (objList.Count > 0)
                    {

                        cmbDoctorAddNewAppointment.SelectedValue = objList[0].ID;

                        if (cmbDoctorAddNewAppointment.SelectedItem != null && ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID == 0)
                        {
                            cmbDoctorAddNewAppointment.TextBox.SetValidation("Please select the Doctor");
                            cmbDoctorAddNewAppointment.TextBox.RaiseValidationError();
                        }
                    }
                    //}
                    //}

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //*******************************************************************************************************************************************

        private void cmbAUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbAUnit.SelectedItem != null) && (((MasterListItem)cmbAUnit.SelectedItem).ID > 0))
            {
                FillDepartmentList(((MasterListItem)cmbAUnit.SelectedItem).ID);
            }



        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {

                FillDoctor(((MasterListItem)cmbAUnit.SelectedItem).ID, ((MasterListItem)cmbDepartment.SelectedItem).ID);
            }
            dgTimeSlots.ItemsSource = null;
            tpFromTime.Value = null;
            tpToTime.Value = null;
        }

        #endregion

        private void SetComboboxValue()
        {

            //cmbAppointmentType.SelectedValue = ((clsAppointmentVO)this.DataContext).AppointmentReasonId;
        }

        /// <summary>
        /// Purpose:Check entered mrno is valid or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdPatientAppSearch_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            //By Anjali............
            Win.isfromCouterSale = true;
            Win.IsClinicVisible = true;
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.Show();

        }

        private void getDetailsFromEMRFollowup(string MrNumber)
        { 
          

            clsGetAppointmentByMrNoBizActionVO BizAction = new clsGetAppointmentByMrNoBizActionVO();
            BizAction.AppointmentDetails = (clsAppointmentVO)this.DataContext;
            if (BizAction.AppointmentDetails == null)
            {
                BizAction.AppointmentDetails = new clsAppointmentVO();
            }
            BizAction.MRNo = MrNumber;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else if (GUnitID > 0)
            {
                BizAction.UnitID = GUnitID;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            //if (((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId != null)
            //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    ((clsAppointmentVO)this.DataContext).PatientId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.PatientId;
                    ((clsAppointmentVO)this.DataContext).UnitId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.UnitId;
                    ((clsAppointmentVO)this.DataContext).PatientUnitId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.PatientUnitId;

                    ((clsAppointmentVO)this.DataContext).FirstName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FirstName;
                    ((clsAppointmentVO)this.DataContext).MiddleName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.MiddleName;
                    ((clsAppointmentVO)this.DataContext).LastName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.LastName;
                    ((clsAppointmentVO)this.DataContext).FamilyName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FamilyName != null ? ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FamilyName : "";

                    if (((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.IsAge == true)
                    {
                        DOB = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.DOB;
                    }
                    else
                    {

                        ((clsAppointmentVO)this.DataContext).DOB = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.DOB;
                    }
                    if (((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.IsAge == true)
                    {

                        txtYY.Text = ConvertDate(DOB, "YY");
                        txtMM.Text = ConvertDate(DOB, "MM");
                        txtDD.Text = ConvertDate(DOB, "DD");
                    }
                    else
                    {

                        txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                        txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                        txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");
                    }

                    ((clsAppointmentVO)this.DataContext).CivilId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.CivilId;
                    ((clsAppointmentVO)this.DataContext).ContactNo1 = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo1;
                    ((clsAppointmentVO)this.DataContext).ContactNo2 = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo2;
                    ((clsAppointmentVO)this.DataContext).FaxNo = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FaxNo;
                    ((clsAppointmentVO)this.DataContext).Email = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.Email;

                    ((clsAppointmentVO)this.DataContext).MrNo = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.MrNo;

                    ((clsAppointmentVO)this.DataContext).GenderId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.GenderId;


                    txtMRNoPatientInformation.Text = MrNo;
                    cmbGender.SelectedValue = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.GenderId;
                    txtContactNo1.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo1;
                    txtContactNo2.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo2;
                    //  txtFaxNo.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FaxNo;
                    txtMobileCountryCode.Text = Convert.ToString(((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.MobileCountryCode);
                    txtResiCountryCode.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ResiNoCountryCode.ToString();
                    txtResiSTD.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ResiSTDCode.ToString();

                  //  cmbGender.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.Gender;

                    dtpAppointmentDate.SelectedDate = FollowUpDate.Date;

                    if (txtResiCountryCode.Text.Equals("0") || txtResiSTD.Text.Equals("0"))
                    {
                        txtResiCountryCode.Text = " ";
                        txtResiSTD.Text = " ";
                    }
                    ////////////////Start//////////////////
                    //IEnumerator<MasterListItem> list = (IEnumerator<MasterListItem>)cmbGender.ItemsSource.GetEnumerator();
                    //while (list.MoveNext())
                    //{
                    //    MasterListItem objMsterListItem = list.Current;
                    //    if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.GenderId)
                    //    {
                    //        cmbGender.SelectedItem = objMsterListItem;
                    //        break;
                    //    }

                    //}

                    //list = (IEnumerator<MasterListItem>)cmbBloodGroup.ItemsSource.GetEnumerator();
                    //while (list.MoveNext())
                    //{
                    //    MasterListItem objMsterListItem = list.Current;
                    //    if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.BloodId)
                    //    {
                    //        cmbBloodGroup.SelectedItem = objMsterListItem;
                    //        break;
                    //    }
                    //}
                    //list = (IEnumerator<MasterListItem>)cmbSpRegistration.ItemsSource.GetEnumerator();
                    //while (list.MoveNext())
                    //{
                    //    MasterListItem objMsterListItem = list.Current;
                    //    if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.SpecialRegistrationID)
                    //    {
                    //        cmbSpRegistration.SelectedItem = objMsterListItem;
                    //        break;
                    //    }
                    //}


                    //list = (IEnumerator<MasterListItem>)cmbMaritalStatus.ItemsSource.GetEnumerator();
                    //while (list.MoveNext())
                    //{
                    //    MasterListItem objMsterListItem = list.Current;
                    //    if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.MaritalStatusId)
                    //    {
                    //        cmbMaritalStatus.SelectedItem = objMsterListItem;
                    //        break;
                    //    }
                    //}


                    //list = (IEnumerator<MasterListItem>)cmbNationality.ItemsSource.GetEnumerator();
                    //while (list.MoveNext())
                    //{
                    //    MasterListItem objMsterListItem = list.Current;
                    //    if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.NationalityId)
                    //    {
                    //        cmbNationality.SelectedItem = objMsterListItem;
                    //        break;
                    //    }
                    //}

                    ////////////////End.///////////////

                    #region FreezeControls
                    txtContactNo1.IsEnabled = false;
                    txtFirstName.IsEnabled = false;
                    txtMiddleName.IsEnabled = false;
                    txtLastName.IsEnabled = false;
                    cmbBloodGroup.IsEnabled = false;
                    cmbGender.IsEnabled = false;
                    dtpDOB1.IsEnabled = false;
                    cmbMaritalStatus.IsEnabled = false;
                    txtFamilyName.IsEnabled = false;
                    txtMobileCountryCode.IsEnabled = false;
                    txtResiSTD.IsEnabled = false;
                    txtResiCountryCode.IsEnabled = false;
                    //txtFaxNo.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                    txtContactNo2.IsEnabled = false;
                    txtYY.IsEnabled = false;
                    txtMM.IsEnabled = false;
                    txtDD.IsEnabled = false;
                    cmbNationality.IsEnabled = false;
                    this.Title = "Patient Appointment- " + Item.FirstName + " " + Item.LastName;
                    #endregion

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




        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                    {
                        txtMRNoPatientInformation.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                        GUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                        if (txtMRNoPatientInformation.Text == "")
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Patient MRNO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            ClearControl();
                        }
                        else
                        {
                            clsCheckMRnoBizActionVO BizActionObj = new clsCheckMRnoBizActionVO();
                            BizActionObj.Details = new clsAppointmentVO();
                            BizActionObj.MRNO = txtMRNoPatientInformation.Text.Trim();
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, arg) =>
                            {
                                if (arg.Error == null)
                                {
                                    Status = ((clsCheckMRnoBizActionVO)arg.Result).SuccessStatus;
                                    if (Status == false)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW8 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Registration Not Found Against This MRNO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                        msgW8.Show();
                                    }
                                    else
                                    {
                                        //SearchAppointmentByMrNo(txtMRNoPatientInformation.Text); Commented By Bhushanp 10/01/2010
                                        GetActiveAdmissionOfPatient(txtMRNoPatientInformation.Text.ToString());
                                    }
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                    msgW1.Show();
                                }
                                Indicatior.Close();
                            };
                            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Patient MRNO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ClearControl();
                    }
                }
                Indicatior.Close();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw ex;
            }

        }
        /// <summary>
        /// Purpose:Get appointment using MRNO.
        /// </summary>
        /// <param name="MrNo"></param>

        public void SearchAppointmentByMrNo(string MrNo)
        {

            clsGetAppointmentByMrNoBizActionVO BizAction = new clsGetAppointmentByMrNoBizActionVO();
            BizAction.AppointmentDetails = (clsAppointmentVO)this.DataContext;
            if (BizAction.AppointmentDetails == null)
            {
                BizAction.AppointmentDetails = new clsAppointmentVO();
            }
            BizAction.MRNo = MrNo;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else if (GUnitID > 0)
            {
                BizAction.UnitID = GUnitID;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            //if (((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId != null)
            //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    ((clsAppointmentVO)this.DataContext).PatientId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.PatientId;
                    ((clsAppointmentVO)this.DataContext).UnitId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.UnitId;
                    ((clsAppointmentVO)this.DataContext).PatientUnitId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.PatientUnitId;

                    ((clsAppointmentVO)this.DataContext).FirstName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FirstName;
                    ((clsAppointmentVO)this.DataContext).MiddleName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.MiddleName;
                    ((clsAppointmentVO)this.DataContext).LastName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.LastName;
                    ((clsAppointmentVO)this.DataContext).FamilyName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FamilyName != null ? ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FamilyName : "";

                    if (((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.IsAge == true)
                    {
                        DOB = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.DOB;
                    }
                    else
                    {

                        ((clsAppointmentVO)this.DataContext).DOB = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.DOB;
                    }
                    if (((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.IsAge == true)
                    {

                        txtYY.Text = ConvertDate(DOB, "YY");
                        txtMM.Text = ConvertDate(DOB, "MM");
                        txtDD.Text = ConvertDate(DOB, "DD");
                    }
                    else
                    {

                        txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                        txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                        txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");
                    }

                    ((clsAppointmentVO)this.DataContext).CivilId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.CivilId;
                    ((clsAppointmentVO)this.DataContext).ContactNo1 = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo1;
                    ((clsAppointmentVO)this.DataContext).ContactNo2 = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo2;
                    ((clsAppointmentVO)this.DataContext).FaxNo = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FaxNo;
                    ((clsAppointmentVO)this.DataContext).Email = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.Email;
                    txtContactNo1.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo1;
                    txtContactNo2.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo2;
                    //  txtFaxNo.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FaxNo;
                    txtMobileCountryCode.Text = Convert.ToString(((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.MobileCountryCode);
                    txtResiCountryCode.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ResiNoCountryCode.ToString();
                    txtResiSTD.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ResiSTDCode.ToString();
                    if (txtResiCountryCode.Text.Equals("0") || txtResiSTD.Text.Equals("0"))
                    {
                        txtResiCountryCode.Text = " ";
                        txtResiSTD.Text = " ";
                    }
                    IEnumerator<MasterListItem> list = (IEnumerator<MasterListItem>)cmbGender.ItemsSource.GetEnumerator();
                    while (list.MoveNext())
                    {
                        MasterListItem objMsterListItem = list.Current;
                        if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.GenderId)
                        {
                            cmbGender.SelectedItem = objMsterListItem;
                            break;
                        }

                    }

                    list = (IEnumerator<MasterListItem>)cmbBloodGroup.ItemsSource.GetEnumerator();
                    while (list.MoveNext())
                    {
                        MasterListItem objMsterListItem = list.Current;
                        if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.BloodId)
                        {
                            cmbBloodGroup.SelectedItem = objMsterListItem;
                            break;
                        }
                    }
                    list = (IEnumerator<MasterListItem>)cmbSpRegistration.ItemsSource.GetEnumerator();
                    while (list.MoveNext())
                    {
                        MasterListItem objMsterListItem = list.Current;
                        if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.SpecialRegistrationID)
                        {
                            cmbSpRegistration.SelectedItem = objMsterListItem;
                            break;
                        }
                    }


                    list = (IEnumerator<MasterListItem>)cmbMaritalStatus.ItemsSource.GetEnumerator();
                    while (list.MoveNext())
                    {
                        MasterListItem objMsterListItem = list.Current;
                        if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.MaritalStatusId)
                        {
                            cmbMaritalStatus.SelectedItem = objMsterListItem;
                            break;
                        }
                    }


                    list = (IEnumerator<MasterListItem>)cmbNationality.ItemsSource.GetEnumerator();
                    while (list.MoveNext())
                    {
                        MasterListItem objMsterListItem = list.Current;
                        if (objMsterListItem.ID == (Int64)((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.NationalityId)
                        {
                            cmbNationality.SelectedItem = objMsterListItem;
                            break;
                        }
                    }

                    #region FreezeControls
                    txtContactNo1.IsEnabled = false;
                    txtFirstName.IsEnabled = false;
                    txtMiddleName.IsEnabled = false;
                    txtLastName.IsEnabled = false;
                    cmbBloodGroup.IsEnabled = false;
                    cmbGender.IsEnabled = false;
                    dtpDOB1.IsEnabled = false;
                    cmbMaritalStatus.IsEnabled = false;
                    txtFamilyName.IsEnabled = false;
                    txtMobileCountryCode.IsEnabled = false;
                    txtResiSTD.IsEnabled = false;
                    txtResiCountryCode.IsEnabled = false;
                    //txtFaxNo.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                    txtContactNo2.IsEnabled = false;
                    txtYY.IsEnabled = false;
                    txtMM.IsEnabled = false;
                    txtDD.IsEnabled = false;
                    cmbNationality.IsEnabled = false;
                    this.Title = "Patient Appointment- " + Item.FirstName + " " + Item.LastName;
                    #endregion

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

        #region  Validation

        /// <summary>
        /// Checks & assigns validations for the controls.
        /// </summary>
        /// <returns></returns>

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {

            //if (Extensions.IsEmailValid(txtEmail.Text) == false)
            //{
            //    txtEmail.SetValidation("Please Enter valid Email Adderess");
            //    txtEmail.RaiseValidationError();

            //}
            //else
            //    txtEmail.ClearValidationError();


            if (txtEmail.Text.Length > 0)
            {
                if (txtEmail.Text.IsEmailValid())
                    txtEmail.ClearValidationError();
                else
                {
                    txtEmail.SetValidation("Please Enter Valid Email Address");
                    txtEmail.RaiseValidationError();
                }
            }
        }

        private void txtContactNo1_LostFocus(object sender, RoutedEventArgs e)
        {

            if (txtContactNo1.Text.Length > 0)
            {
                if (txtContactNo1.Text.IsNumberValid())
                {
                    txtContactNo1.ClearValidationError();
                }
                else
                {

                    txtContactNo1.SetValidation("Please Enter Number In  Correct Format");
                    txtContactNo1.RaiseValidationError();
                }
                txtContactNo1.ClearValidationError();
            }
            //if (Extensions.IsPhoneNumberValid(txtContactNo1.Text) == false)
            //{
            //    txtContactNo1.Textbox.SetValidation("Please Enter Number in  Correct Format");
            //    txtContactNo1.Textbox.RaiseValidationError();

            //}
            //else
            //    txtContactNo1.Textbox.ClearValidationError();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();

        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
            //if (txtLastName.Text == "")
            //{
            //    txtLastName.SetValidation("Please Enter Last Name");
            //    txtLastName.RaiseValidationError();

            //}
            //else
            //    txtLastName.ClearValidationError();
            txtFamilyName.Text = txtLastName.Text;
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

        private void dtpAppointmentDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((clsAppointmentVO)this.DataContext) != null)
            {
                if (((clsAppointmentVO)this.DataContext).AppointmentDate < DateTime.Today)
                {

                    dtpAppointmentDate.SetValidation("Appointment Date  Cannot Be Less Than Today");
                    dtpAppointmentDate.RaiseValidationError();
                    dtpAppointmentDate.Focus();
                    dtpAppointmentDate.Text = " ";
                    dtpAppointmentDate.Text = DateTime.Today.ToString();
                }
                else if (((clsAppointmentVO)this.DataContext).AppointmentDate == null)
                {
                    dtpAppointmentDate.SetValidation("Please Select The Appointment Date");
                    dtpAppointmentDate.RaiseValidationError();
                    dtpAppointmentDate.Text = " ";
                    dtpAppointmentDate.Text = DateTime.Today.ToString();
                }
                else
                    dtpAppointmentDate.ClearValidationError();
            }

        }


        //* Commented by - Ajit Jadhav
        //* Added Date - 9/8/2016
        //* Comments - For New Mandatory Fields
        private void dtpDOB1_LostFocus(object sender, RoutedEventArgs e)
        {

            //if (((clsAppointmentVO)this.DataContext).DOB > DateTime.Now)
            //{
            //    dtpDOB1.SetValidation("Date of Birth Cannot Be Greater Than Today");
            //    dtpDOB1.RaiseValidationError();
            //    txtYY.Text = "0";
            //    txtMM.Text = "0";
            //    txtDD.Text = "0";
            //}
            //else if (((clsAppointmentVO)this.DataContext).DOB == null)
            //{
            //    dtpDOB1.SetValidation("Please Select The Date of Birth");
            //    dtpDOB1.RaiseValidationError();
            //    txtYY.Text = "0";
            //    txtMM.Text = "0";
            //    txtDD.Text = "0";
            //}
            //else
            //{
            //    dtpDOB1.ClearValidationError();
            //    txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
            //    txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
            //    txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");
            //}

            //txtYY.SelectAll();

            if (dtpDOB1.SelectedDate != null)
            {
                dtpDOB1.ClearValidationError();
                txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");
            }
            txtYY.SelectAll();

        }

        //private void txtFirstName_SelectionChanged(object sender, RoutedEventArgs e)
        //{
        //    if (Extensions.IsItCharacter(txtFirstName.Text) == false)
        //    {
        //        txtFirstName.SetValidation("Please enter Character Only");
        //        txtFirstName.RaiseValidationError();
        //    }
        //    else
        //        txtFirstName.ClearValidationError();
        //}

        private void txtContactNo1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItNumber(txtContactNo1.Text) == false)
            {
                txtContactNo1.SetValidation("Please Enter Only Number");
                txtContactNo1.RaiseValidationError();
            }
            else

                txtContactNo1.ClearValidationError();


        }

        private void WaterMarkTextbox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {
                    if (((WaterMarkTextbox)sender).Text.Length == 1)
                    {
                        if (!((WaterMarkTextbox)sender).Text.IsValueNotZero())
                        {
                            ((WaterMarkTextbox)sender).Text = textBefore;
                            ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                            ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                            textBefore = "";
                            selectionStart = 0;
                            selectionLength = 0;
                        }
                    }
                    else if (!((WaterMarkTextbox)sender).Text.IsItNumber()) //if (!((WaterMarkTextbox)sender).Text.IsNumberValid() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 14)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }

        }
        private void txtMobileCountryCode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }
        private void txtMobileCountryCode_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsValidCountryCode() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 4)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }
        }
        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        private void txtMiddleName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItCharacter(txtMiddleName.Text) == false)
            {
                txtMiddleName.SetValidation("Please Enter Only Character");
                txtMiddleName.RaiseValidationError();
            }
            else
                txtMiddleName.ClearValidationError();
        }

        private void txtLastName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItCharacter(txtLastName.Text) == false)
            {
                txtLastName.SetValidation("Please Enter Only Character");
                txtLastName.RaiseValidationError();

            }
            else
                txtLastName.ClearValidationError();
        }

        private void txtFamilyName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItCharacter(txtFamilyName.Text) == false)
            {
                txtFamilyName.SetValidation("Please Enter Only Character");
                txtFamilyName.RaiseValidationError();
            }
            else
                txtFamilyName.ClearValidationError();



        }

        private void txtContactNo2_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItNumber(txtContactNo2.Text) == false)
            {
                txtContactNo2.SetValidation("Please Enter Only Number");
                txtContactNo2.RaiseValidationError();
            }
            else
                txtContactNo2.ClearValidationError();
        }

        //private void txtFaxNo_SelectionChanged(object sender, RoutedEventArgs e)
        //{
        //    if (Extensions.IsItNumber(txtFaxNo.Text) == false)
        //    {
        //        txtFaxNo.SetValidation("Please enter only Number");
        //        txtFaxNo.RaiseValidationError();
        //    }
        //    else
        //        txtFaxNo.ClearValidationError();

        //}

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

        private void txtFaxNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItNumber())
            {
                if (textBefore != null)
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


        DateTime? DOB = null;

        void CalculateBirthDate()
        {

            //int Yearval = 0;
            //int Monthval = 0;
            //int DayVal = 0;

            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //    Yearval = int.Parse(txtYY.Text.Trim());

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //    Monthval = int.Parse(txtMM.Text.Trim());


            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //    DayVal = int.Parse(txtDD.Text.Trim());

            //if (Yearval > 0 || Monthval > 0 || DayVal > 0)
            //{
            //    dtpDOB.SelectedDate = CalculateDateFromAge(Yearval, Monthval, DayVal);
            //    dtpDOB.ClearValidationError();
            //    txtYY.ClearValidationError();
            //    txtMM.ClearValidationError();
            //    txtDD.ClearValidationError();
            //}
            //else
            //{

            //    txtYY.SetValidation("Age is Required");
            //    txtYY.RaiseValidationError();
            //    txtMM.SetValidation("Age is Required");
            //    txtMM.RaiseValidationError();
            //    txtDD.SetValidation("Age is Required");
            //    txtDD.RaiseValidationError();

            //    dtpDOB.SetValidation("Please select the Date of birth");
            //    dtpDOB.RaiseValidationError();
            //    txtYY.Text = "0";
            //    txtMM.Text = "0";
            //    txtDD.Text = "0";


            //}

            int Yearval = 0;
            int Monthval = 0;
            int DayVal = 0;

            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
                Yearval = int.Parse(txtYY.Text.Trim());

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
                Monthval = int.Parse(txtMM.Text.Trim());


            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
                DayVal = int.Parse(txtDD.Text.Trim());

            if (Yearval > 0 || Monthval > 0 || DayVal > 0)
            {
                DOB = CalculateDateFromAge(Yearval, Monthval, DayVal);

            }
            else
            {




            }
        }

        private DateTime? CalculateDateFromAge(int Year, int Month, int Days)
        {
            try
            {
                DateTime BirthDate;
                //if (DateTobeConvert != null && parameter.ToString().ToUpper() != "YY")
                //    BirthDate = DateTobeConvert.Value;
                //else
                BirthDate = DateTime.Now;
                if (Year > 0)
                {
                    BirthDate = BirthDate.AddYears(-Year);
                }

                if (Month > 0)
                {
                    BirthDate = BirthDate.AddMonths(-Month);
                }

                if (Days > 0)
                {
                    BirthDate = BirthDate.AddDays(-Days);
                }
                //result = (age.Day - 1).ToString();
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }
        }


        private void txtYY_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //{
            //    int val = int.Parse(txtYY.Text.Trim());
            //    if (val > 0)
            //    {
            //     //  dtpDOB1.SelectedDate = ConvertDateBack("YY", val, dtpDOB1.SelectedDate);
            //       DOB = ConvertDateBack("YY", val, DOB);
            //    }
            //}

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //{
            //    int val = int.Parse(txtMM.Text.Trim());
            //    if (val > 0)
            //    {
            //  //    dtpDOB1.SelectedDate = ConvertDateBack("MM", val, dtpDOB1.SelectedDate);
            //      DOB = ConvertDateBack("YY", val, DOB);
            //    }
            //}

            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //{
            //    int val = int.Parse(txtDD.Text.Trim());
            //    if (val > 0)
            //    {
            //          // dtpDOB1.SelectedDate = ConvertDateBack("DD", val, dtpDOB1.SelectedDate);
            //           DOB = ConvertDateBack("YY", val, DOB);
            //    }
            //}

            CalculateBirthDate();
            txtMM.SelectAll();
        }

        private void txtMM_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //{
            //    int val = int.Parse(txtYY.Text.Trim());
            //    if (val > 0)
            //    {
            //      // dtpDOB1.SelectedDate = ConvertDateBack("YY", val, dtpDOB1.SelectedDate);
            //       DOB = ConvertDateBack("YY", val, DOB);

            //    }
            //}

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //{
            //    int val = int.Parse(txtMM.Text.Trim());
            //    if (val > 0)
            //    {
            // //   dtpDOB1.SelectedDate = ConvertDateBack("MM", val, dtpDOB1.SelectedDate);
            //    DOB = ConvertDateBack("YY", val, DOB);

            //    }
            //}

            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //{
            //    int val = int.Parse(txtDD.Text.Trim());
            //    if (val > 0)
            //    {
            //      //  dtpDOB1.SelectedDate = ConvertDateBack("DD", val, dtpDOB1.SelectedDate);
            //        DOB = ConvertDateBack("YY", val, DOB);
            //    }
            //}

            CalculateBirthDate();
            txtDD.SelectAll();
        }

        private void txtDD_LostFocus(object sender, RoutedEventArgs e)
        {

            CalculateBirthDate();
            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //{
            //    int val = int.Parse(txtYY.Text.Trim());
            //    if (val > 0)
            //    {
            //     //   dtpDOB1.SelectedDate = ConvertDateBack("YY", val, dtpDOB1.SelectedDate);
            //        DOB = ConvertDateBack("YY", val, DOB);

            //    }
            //}

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //{
            //    int val = int.Parse(txtMM.Text.Trim());
            //    if (val > 0)
            //    {
            //     //  dtpDOB1.SelectedDate = ConvertDateBack("MM", val, dtpDOB1.SelectedDate);
            //       DOB = ConvertDateBack("YY", val, DOB);

            //    }
            //}

            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //{
            //    int val = int.Parse(txtDD.Text.Trim());
            //    if (val > 0)
            //    {
            //     //  dtpDOB1.SelectedDate = ConvertDateBack("DD", val, dtpDOB1.SelectedDate);
            //       DOB = ConvertDateBack("YY", val, DOB);
            //    }
            //}

        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {

            }
            else if (!((TextBox)sender).Text.IsPositiveNumber())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private bool chkValidations()
        {
            bool result = true;


            if (txtFirstName.Text == "")
            {

                txtFirstName.SetValidation("Please Enter First Name");
                txtFirstName.RaiseValidationError();
                txtFirstName.Focus();
                MesFlag = false;
                result = false;
                ClickedFlag = 0;
            }
            else
                txtFirstName.ClearValidationError();

            //* Added by - Ajit Jadhav
            //* Added Date - 24/9/2016
            //* Comments - New Mandatory Fields

            if (Item.IsReschedule == true)
            {
                if (txtReasonReschedule.Text.Trim() == "")
                {
                    txtReasonReschedule.SetValidation("Rescheduling Appointment Is Mandatory");
                    txtReasonReschedule.RaiseValidationError();
                    txtReasonReschedule.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtReasonReschedule.ClearValidationError();



            }
            //***//--------------------------------------------------

            // Commented by Ajit jadhav date 6/10/2016
            //if (txtLastName.Text == "")
            //{
            //    txtLastName.SetValidation("Please Enter Last Name");
            //    txtLastName.RaiseValidationError();
            //    txtLastName.Focus();
            //    MesFlag = false;
            //    result = false;
            //    ClickedFlag = 0;
            //}
            //else
            //    txtLastName.ClearValidationError();

            //if (txtContactNo1.Text == "" && txtContactNo2.Text == "")
            //{

            //    txtContactNo1.Textbox.SetValidation("Please Enter Contact Details ");
            //    txtContactNo1.Textbox.RaiseValidationError();
            //    txtContactNo1.Textbox.Focus();
            //    txtContactNo1.Textbox.Focus();
            //    MesFlag = false;
            //    result = false;
            //    ClickedFlag = 0;
            //}
            //else
            //    txtContactNo1.ClearValidationError();

            //if (txtMobileCountryCode.Text == " ")
            //{
            //    txtMobileCountryCode.Textbox.SetValidation("Please Enter Country Code ");
            //    txtMobileCountryCode.Textbox.RaiseValidationError();
            //    txtMobileCountryCode.Textbox.Focus();
            //    txtMobileCountryCode.Textbox.Focus();
            //    MesFlag = false;
            //    result = false;
            //    ClickedFlag = 0;
            //}
            //else
            //    txtMobileCountryCode.ClearValidationError();

            if (txtContactNo1.Text.Trim() != "")
            {
                if (txtContactNo1.Text.Trim().Length > 14)
                {
                    txtContactNo1.Textbox.SetValidation("Please Enter Number In  Correct Format");
                    txtContactNo1.Textbox.RaiseValidationError();
                    txtContactNo1.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }
            }
            else
                  txtContactNo1.ClearValidationError();


            if (((clsAppointmentVO)this.DataContext)!= null && ((clsAppointmentVO)this.DataContext).AppointmentDate == null)
            {
                dtpAppointmentDate.SetValidation("Please Select The Appointment Date");
                dtpAppointmentDate.RaiseValidationError();
                dtpAppointmentDate.Focus();
                MesFlag = false;
                result = false;
                ClickedFlag = 0;
            }
            else if (((clsAppointmentVO)this.DataContext) != null && ((clsAppointmentVO)this.DataContext).AppointmentDate < DateTime.Today)
            {

                dtpAppointmentDate.SetValidation("Please Select Current Date Or Greater Than Current Date");
                dtpAppointmentDate.RaiseValidationError();
                dtpAppointmentDate.Focus();
                MesFlag = false;
                 result = false;
                ClickedFlag = 0;
            }
            else
                dtpAppointmentDate.ClearValidationError();


            //if (string.IsNullOrEmpty(txtYY.Text) && string.IsNullOrEmpty(txtMM.Text) && string.IsNullOrEmpty(txtDD.Text))   //Commented by ajit jadhav Date 23/9/2016 for User Requirement
            //{

            //    txtYY.SetValidation("Please Enter Age");
            //    txtYY.RaiseValidationError();
            //    txtMM.SetValidation("Please Enter Age");
            //    txtMM.RaiseValidationError();
            //    txtDD.SetValidation("Please Enter Age");
            //    txtDD.RaiseValidationError();
            //    MesFlag = false;
            //    result = false;
            //    txtYY.Focus();
            //    dtpDOB1.ClearValidationError();
            //}
            //else
            //{
            //    txtYY.ClearValidationError();
            //    txtMM.ClearValidationError();
            //    txtDD.ClearValidationError();
            //}
            if (txtYY.Text != "")
            {
                if (Convert.ToInt16(txtYY.Text) > 120)
                {
                    txtYY.SetValidation("Age Cannot Be Greater Than 120");
                    txtYY.RaiseValidationError();
                    txtYY.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }

                else
                    txtYY.ClearValidationError();

                //if (Convert.ToInt16(txtYY.Text) < 18)
                //{
                //    txtYY.SetValidation("Age Cannot Be Less Than 18");
                //    txtYY.RaiseValidationError();
                //    txtYY.Focus();
                //    MesFlag = false;
                //    result = false;
                //    ClickedFlag = 0;

                //}
                //else
                //    txtYY.ClearValidationError();
            }

            if (txtMM.Text != "")
            {
                if (Convert.ToInt16(txtMM.Text) > 11)
                {
                    txtMM.SetValidation("Months Cannot Be Greater Than 12");
                    txtMM.RaiseValidationError();
                    txtMM.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtMM.ClearValidationError();
            }

            if (txtDD.Text != "")
            {
                if (Convert.ToInt16(txtDD.Text) > 30) //Changes made by AniketK on 31Jan2019 // if (Convert.ToInt16(txtDD.Text) > 29)
                {
                    txtDD.SetValidation("Days Cannot Be Greater than 30"); //Changes made by AniketK on 31Jan2019  // txtDD.SetValidation("Days Cannot Be Greater than 29");
                    txtDD.RaiseValidationError();
                    txtDD.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtDD.ClearValidationError();
            }

            //***************************************************************************************************************************** 
            if (IsPageLoded)
            {
                if (txtFirstName.Text == "")
                {
                    txtFirstName.SetValidation("Please Enter First Name");
                    txtFirstName.RaiseValidationError();
                    txtFirstName.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtFirstName.ClearValidationError();

                //if (txtLastName.Text == "")
                //{
                //    txtLastName.SetValidation("Please Enter Last Name");
                //    txtLastName.RaiseValidationError();
                //    txtLastName.Focus();
                //    MesFlag = false;
                //    result = false;
                //    ClickedFlag = 0;
                //}
                //else
                //    txtLastName.ClearValidationError();

                if ((MasterListItem)cmbGender.SelectedItem == null)
                {
                    cmbGender.TextBox.SetValidation("Gender Is Required");
                    cmbGender.TextBox.RaiseValidationError();
                    cmbGender.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }

                else if (((MasterListItem)cmbGender.SelectedItem).ID == 0)
                {
                    cmbGender.TextBox.SetValidation("Gender Is Required");
                    cmbGender.TextBox.RaiseValidationError();
                    cmbGender.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;

                }
                else
                    cmbGender.TextBox.ClearValidationError();

                if (((clsAppointmentVO)this.DataContext).DOB > DateTime.Now)
                {

                    dtpDOB1.SetValidation("Date of Birth Cannot Be Greater Than Today");
                    dtpDOB1.RaiseValidationError();
                    dtpDOB1.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }
                //else if ((string.IsNullOrEmpty(txtYY.Text) && string.IsNullOrEmpty(txtMM.Text) && string.IsNullOrEmpty(txtDD.Text)) && (((clsAppointmentVO)this.DataContext).DOB == null))  //Commented by ajit jadhav Date 23/9/2016 for User Requirement
                //{
                //    dtpDOB1.SetValidation("Please Select The Date of Birth");
                //    dtpDOB1.RaiseValidationError();
                //    dtpDOB1.Focus();
                //    MesFlag = false;
                //    result = false;
                //    ClickedFlag = 0;
                //}
                else
                    dtpDOB1.ClearValidationError();

                if (txtYY.Text != "")
                {
                    if (Convert.ToInt16(txtYY.Text) > 120)
                    {
                        txtYY.SetValidation("Age Cannot Be Greater Than 120");
                        txtYY.RaiseValidationError();
                        txtYY.Focus();
                        MesFlag = false;
                        result = false;
                        ClickedFlag = 0;
                    }

                    else
                        txtYY.ClearValidationError();

                    //if (Convert.ToInt16(txtYY.Text) < 18)
                    //{
                    //    txtYY.SetValidation("Age Cannot Be Less Than 18");
                    //    txtYY.RaiseValidationError();
                    //    txtYY.Focus();
                    //    MesFlag = false;
                    //    result = false;
                    //    ClickedFlag = 0;

                    //}
                    //else
                    //    txtYY.ClearValidationError();
                }

                if (txtMM.Text != "")
                {
                    if (Convert.ToInt16(txtMM.Text) > 11)
                    {
                        txtMM.SetValidation("Months Cannot Be Greater Than 12");
                        txtMM.RaiseValidationError();
                        txtMM.Focus();
                        MesFlag = false;
                        result = false;
                        ClickedFlag = 0;
                    }
                    else
                        txtMM.ClearValidationError();
                }

                if (txtDD.Text != "")
                {
                    if (Convert.ToInt16(txtDD.Text) > 30)  //Changes made by AniketK on 31Jan2019 // if (Convert.ToInt16(txtDD.Text) > 29)
                    {
                        txtDD.SetValidation("Days Cannot Be Greater than 365");
                        txtDD.RaiseValidationError();
                        txtDD.Focus();
                        MesFlag = false;
                        result = false;
                        ClickedFlag = 0;
                    }
                    else
                        txtDD.ClearValidationError();
                }
                if (txtContactNo1.Text == "" && txtContactNo2.Text == "")
                {
                    txtContactNo1.SetValidation("Please Enter Contact Details ");
                    txtContactNo1.RaiseValidationError();
                    txtContactNo1.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtContactNo1.ClearValidationError();

                if (txtMobileCountryCode.Text == " ")
                {
                    txtMobileCountryCode.Textbox.SetValidation("Please Enter Country Code ");
                    txtMobileCountryCode.Textbox.RaiseValidationError();
                    txtMobileCountryCode.Textbox.Focus();
                    txtMobileCountryCode.Textbox.Focus();
                    MesFlag = false;
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtMobileCountryCode.ClearValidationError();


                if (tpFromTime.Value != null)
                {
                    if (dtpAppointmentDate.SelectedDate != null)
                    {
                        string time = tpFromTime.Value.Value.ToShortTimeString();
                        String mm = time.Substring(time.IndexOf(":") + 1, time.IndexOf(" ") - 2);
                        string hh = time.Substring(0, time.IndexOf(":"));
                        for (int k = 0; k < time.Length; k++)
                        {

                            if (time[k].Equals('A'))
                            {

                            }

                            if (time[k].Equals('P'))
                            {
                                if (Convert.ToInt32(hh) < 12)
                                {
                                    hh = (Convert.ToInt32(hh) + 12).ToString();
                                }
                            }
                            if (((clsAppointmentVO)this.DataContext).AppointmentDate < DateTime.Today)
                            {

                                dtpAppointmentDate.SetValidation("Appointment Date  Cannot Be Less Than Today");
                                dtpAppointmentDate.RaiseValidationError();
                                MesFlag = false;
                                result = false;
                                ClickedFlag = 0;
                            }
                            else if (dtpAppointmentDate.SelectedDate == null)
                            {
                                dtpAppointmentDate.SetValidation("Please Select The Appointment Date");
                                dtpAppointmentDate.RaiseValidationError();
                                MesFlag = false;
                                result = false;
                                ClickedFlag = 0;
                            }
                            else
                                dtpAppointmentDate.ClearValidationError();

                        }

                        if (txtEmail.Text.Length > 0)
                        {
                            if (txtEmail.Text.IsEmailValid())
                                txtEmail.ClearValidationError();
                            else
                            {
                                txtEmail.SetValidation("Please Enter Valid Email Address");
                                txtEmail.RaiseValidationError();
                                txtEmail.Focus();
                                MesFlag = false;
                                result = false;
                                ClickedFlag = 0;
                            }

                        }



                        if ((MasterListItem)cmbAUnit.SelectedItem == null)
                        {
                            cmbAUnit.TextBox.SetValidation("Please Select Unit");
                            cmbAUnit.TextBox.RaiseValidationError();
                            cmbAUnit.Focus();
                            MesFlag = false;
                            result = false;
                            ClickedFlag = 0;
                        }
                        else if (((MasterListItem)cmbAUnit.SelectedItem).ID == 0)
                        {
                            cmbAUnit.TextBox.SetValidation("Please Select Unit");
                            cmbAUnit.TextBox.RaiseValidationError();
                            cmbAUnit.Focus();
                            MesFlag = false;
                            result = false;
                            ClickedFlag = 0;

                        }
                        else
                            cmbAUnit.TextBox.ClearValidationError();


                        if ((MasterListItem)cmbDepartment.SelectedItem == null)
                        {
                            cmbDepartment.TextBox.SetValidation("Please Select Department");
                            cmbDepartment.TextBox.RaiseValidationError();
                            cmbDepartment.Focus();
                            MesFlag = false;
                            result = false;
                            ClickedFlag = 0;
                        }
                        else if (((MasterListItem)cmbDepartment.SelectedItem).ID == 0)
                        {
                            cmbDepartment.TextBox.SetValidation("Please Select Department");
                            cmbDepartment.TextBox.RaiseValidationError();
                            cmbDepartment.Focus();
                            MesFlag = false;
                            result = false;
                            ClickedFlag = 0;

                        }
                        else
                            cmbDepartment.TextBox.ClearValidationError();


                        if ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem == null)
                        {
                            cmbDoctorAddNewAppointment.TextBox.SetValidation("Please Select Doctor");
                            cmbDoctorAddNewAppointment.TextBox.RaiseValidationError();
                            cmbDoctorAddNewAppointment.Focus();
                            MesFlag = false;
                            result = false;
                            ClickedFlag = 0;
                        }
                        else if (((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID == 0)
                        {
                            cmbDoctorAddNewAppointment.TextBox.SetValidation("Please Select Doctor");
                            cmbDoctorAddNewAppointment.TextBox.RaiseValidationError();
                            cmbDoctorAddNewAppointment.Focus();
                            MesFlag = false;
                            result = false;
                            ClickedFlag = 0;

                        }
                        else
                            cmbDoctorAddNewAppointment.TextBox.ClearValidationError();

                        //* Added by - Ajit Jadhav
                        //* Added Date - 9/8/2016
                        //* Comments - Create New AutoCompleteComboBox and check Mendatory Fields
                        //if ((MasterListItem)cmbNationality.SelectedItem == null)
                        //{
                        //    cmbNationality.TextBox.SetValidation("Nationality Is Required");      //Commented by ajit jadhav Date 23/9/2016 for User Requirement
                        //    cmbNationality.TextBox.RaiseValidationError();
                        //    cmbNationality.Focus();
                        //    MesFlag = false;
                        //    result = false;
                        //    ClickedFlag = 0;
                        //}

                        //else if (((MasterListItem)cmbNationality.SelectedItem).ID == 0)
                        //{
                        //    cmbNationality.TextBox.SetValidation("Nationality Is Required");
                        //    cmbNationality.TextBox.RaiseValidationError();
                        //    cmbNationality.Focus();
                        //    MesFlag = false;
                        //    result = false;
                        //    ClickedFlag = 0;

                        //}
                        //else
                        //    cmbNationality.TextBox.ClearValidationError();

                        //***//---------------------------------------

                        DateTime AppTime = new DateTime(dtpAppointmentDate.SelectedDate.Value.Year, dtpAppointmentDate.SelectedDate.Value.Month, dtpAppointmentDate.SelectedDate.Value.Day, Convert.ToInt16(hh), Convert.ToInt16(mm), 0);
                        if (DateTime.Now > AppTime)
                        {
                            //MessageBox.Show("Time slot is not available");
                            MessageBoxControl.MessageBoxChildWindow msgW11 =
                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Time Slot Is Not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW11.Show();

                            tpFromTime.Focus();
                            ClickedFlag = 0;
                            result = false;
                            MesFlag = false;
                            return result;

                        }
                        if (tpFromTime.Value.Value.TimeOfDay == tpToTime.Value.Value.TimeOfDay)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW11 =
                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "To Time Cannot Be Same As From Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW11.Show();
                            tpToTime.Focus();
                            ClickedFlag = 0;
                            result = false;
                            MesFlag = false;
                            return result;
                        }
                        if (tpFromTime.Value.Value.TimeOfDay > tpToTime.Value.Value.TimeOfDay)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW11 =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "From Time Cannot Be After To Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW11.Show();
                            tpToTime.Focus();
                            ClickedFlag = 0;
                            result = false;
                            MesFlag = false;
                            return result;
                        }

                        foreach (var a in listSchedule)
                        {
                            if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay > a.ScheduleToTime.TimeOfDay)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW20.Show();
                                ClickedFlag = 0;
                                break;
                            }
                            else if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay < a.ScheduleToTime.TimeOfDay)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW20.Show();
                                ClickedFlag = 0;
                                break;
                            }
                            else
                            {
                            }
                        }
                    }
                }



                if (((MasterListItem)(cmbAppointmentType.SelectedItem)).ID == 0)
                {

                    cmbAppointmentType.TextBox.SetValidation("Please Select Appointment Reason");
                    cmbAppointmentType.TextBox.RaiseValidationError();
                    cmbAppointmentType.Focus();
                    result = false;
                    ClickedFlag = 0;

                }
                else
                    cmbAppointmentType.TextBox.ClearValidationError();














                if (dgTimeSlots.ItemsSource == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW12 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Doctor Schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW12.Show();
                    ClickedFlag = 0;
                    result = false;
                    return result;

                }

                if (tpFromTime.Value == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW11 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter From Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW11.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            tpFromTime.Focus();
                        }
                    };
                    msgW11.Show();
                    ClickedFlag = 0;
                    result = false;
                    return result;


                }
                if (tpToTime.Value == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW12 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter To Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW12.OnMessageBoxClosed += (MessageBoxResult res1) =>
                    {
                        if (res1 == MessageBoxResult.OK)
                        {
                            tpToTime.Focus();

                        }
                    };

                    msgW12.Show();
                    ClickedFlag = 0;

                    result = false;
                    return result;

                }


            }
            return result;

        }

        #endregion

        #region Get Past Visit details
        /// <summary>
        /// Purpose:Display Patient Past Appointment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void PastVisits_Click(object sender, RoutedEventArgs e)
        {
            if (txtMRNoPatientInformation.Text == "")
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Patient MRNO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW3.Show();
            }
            else
            {

                clsCheckMRnoBizActionVO BizActionObj = new clsCheckMRnoBizActionVO();
                BizActionObj.Details = new clsAppointmentVO();
                BizActionObj.MRNO = txtMRNoPatientInformation.Text;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {

                        Status = ((clsCheckMRnoBizActionVO)arg.Result).SuccessStatus;

                        if (Status == false)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW7 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Valid MRNO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW7.Show();


                        }
                        else
                        {
                            win.Show();
                            FetchPatientPastVisit(txtMRNoPatientInformation.Text);
                            FetchPastAppointment(txtMRNoPatientInformation.Text);
                            FetchFutureAppointment(txtMRNoPatientInformation.Text);
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();



            }
        }

        /// <summary>
        /// Method Purpose:For fetching patient past visit.
        /// </summary>
        /// <param name="MrNo"></param>

        private void FetchPatientPastVisit(string MrNo)
        {
            clsGetPatientPastVisitBizActionVO BizAction = new clsGetPatientPastVisitBizActionVO();
            BizAction.MRNO = MrNo;
            //BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;




            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            win.dgPastVisitList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList != null)
                    {

                        win.dgPastVisitList.ItemsSource = ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList;
                        //win.Show();
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

        private void FetchPastAppointment(string MrNo)
        {
            clsGetPastAppointmentBizActionVO BizAction = new clsGetPastAppointmentBizActionVO();

            BizAction.MRNO = MrNo;


            //BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;




            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            win.dgPastAppointmentList.ItemsSource = null;

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetPastAppointmentBizActionVO)arg.Result).AppointmentList != null)
                    {
                        win.dgPastAppointmentList.ItemsSource = ((clsGetPastAppointmentBizActionVO)arg.Result).AppointmentList;


                        //win.Show();
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

        private void FetchFutureAppointment(string MrNo)
        {
            clsGetFutureAppointmentBizActionVO BizAction = new clsGetFutureAppointmentBizActionVO();

            BizAction.MRNO = MrNo;


            //BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;




            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            win.dgFutureAppointmentList.ItemsSource = null;

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetFutureAppointmentBizActionVO)arg.Result).AppointmentList != null)
                    {
                        win.dgFutureAppointmentList.ItemsSource = ((clsGetFutureAppointmentBizActionVO)arg.Result).AppointmentList;


                        //win.Show();
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



        #endregion

        #region Save data
        /// <summary>
        /// Purpose:Add new appointment of patient.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                try
                {
                    bool SaveApp = true;

                    SaveApp = chkValidations();

                    if (SaveApp == true)
                    {
                        if (txtMRNoPatientInformation.Text == "")
                        {
                            checkDuplication();
                        }
                        else
                        {
                            string msgTitle = "";
                            string msgText = "Are You Sure You Want To Save The Appointment?";

                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }
                    else
                    {
                        if (MesFlag)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Mandatory Fields Are Compulsory.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();

                        }
                        ClickedFlag = 0;
                    }

                }
                catch (Exception ex)
                {
                    throw;

                }
            }
        }

        private void SaveAppointment()
        {
            if (ClickedFlag == 1)
            {
                clsAddAppointmentBizActionVO BizAction = new clsAddAppointmentBizActionVO();
                ((clsAppointmentVO)this.DataContext).AppointmentID = 0;
                BizAction.AppointmentDetails = (clsAppointmentVO)this.DataContext;
                BizAction.AppointmentDetails.AppointmentID = 0;

                if (cmbGender.SelectedItem != null)
                    BizAction.AppointmentDetails.GenderId = ((MasterListItem)cmbGender.SelectedItem).ID;

                if (cmbBloodGroup.SelectedItem != null)
                    BizAction.AppointmentDetails.BloodId = ((MasterListItem)cmbBloodGroup.SelectedItem).ID;

                if (cmbMaritalStatus.SelectedItem != null)
                    BizAction.AppointmentDetails.MaritalStatusId = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;

                if (cmbAUnit.SelectedItem != null)
                    BizAction.AppointmentDetails.UnitId = ((MasterListItem)cmbAUnit.SelectedItem).ID;

                if (cmbDepartment.SelectedItem != null)
                    BizAction.AppointmentDetails.DepartmentId = ((MasterListItem)cmbDepartment.SelectedItem).ID;

                if (cmbDoctorAddNewAppointment.SelectedItem != null)
                    BizAction.AppointmentDetails.DoctorId = ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID;

                if (cmbAppointmentType.SelectedItem != null)
                    BizAction.AppointmentDetails.AppointmentReasonId = ((MasterListItem)cmbAppointmentType.SelectedItem).ID;

                if (cmbSpRegistration.SelectedItem != null)
                    BizAction.AppointmentDetails.SpecialRegistrationID = ((MasterListItem)cmbSpRegistration.SelectedItem).ID;

                BizAction.AppointmentDetails.ContactNo1 = txtContactNo1.Text.Trim();
                BizAction.AppointmentDetails.ContactNo2 = txtContactNo2.Text.Trim();

                if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                    BizAction.AppointmentDetails.MobileCountryCode = txtMobileCountryCode.Text.Trim();
                if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                    BizAction.AppointmentDetails.ResiNoCountryCode = long.Parse(txtResiCountryCode.Text.Trim());
                if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                    BizAction.AppointmentDetails.ResiSTDCode = long.Parse(txtResiSTD.Text.Trim());
                if (Item.IsReschedule == true)
                {
                    BizAction.AppointmentDetails.AppointmentStatus = 2;
                    BizAction.AppointmentDetails.ParentAppointmentID = AppointmentID;
                    BizAction.AppointmentDetails.ParentAppointmentUnitID = Item.UnitId;
                }
                else
                {
                    BizAction.AppointmentDetails.AppointmentStatus = 0;
                }


                if (dtpDOB1.SelectedDate == null)
                {
                    if (Item.IsReschedule == true && dtpDOB1.SelectedDate != null)
                    {
                        BizAction.AppointmentDetails.IsAge = true;
                        BizAction.AppointmentDetails.DOB = Item.DateOfBirthFromAge;
                    }
                    else
                    {
                        BizAction.AppointmentDetails.IsAge = true;
                        BizAction.AppointmentDetails.DOB = DOB;
                    }
                }
                else
                {
                    BizAction.AppointmentDetails.IsAge = false;
                    BizAction.AppointmentDetails.DOB = dtpDOB1.SelectedDate;
                }

                BizAction.AppointmentDetails.FromTime = Convert.ToDateTime(tpFromTime.Value);
                BizAction.AppointmentDetails.ToTime = Convert.ToDateTime(tpToTime.Value);

                //* Added by - Ajit Jadhav
                //* Added Date - 9/8/2016
                //* Comments - Add New Combobox Natinality
                if (cmbNationality.SelectedItem != null)
                    BizAction.AppointmentDetails.NationalityId = ((MasterListItem)cmbNationality.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        ClickedFlag = 0;
                        if (chkEmail.IsChecked == true)
                        {
                            if (txtEmail.Text != "")
                            {
                                EmailServiceClient EmailClient = new EmailServiceClient();
                                EmailClient.SendEmailCompleted += (sa, arg1) =>
                                {
                                    if (arg1.Error == null)
                                    {
                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Email Sending Fail", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW2.Show();
                                    }
                                };
                                string Doctor = "Dr." + ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).Description;

                                string Date = dtpAppointmentDate.SelectedDate.Value.ToShortDateString();
                                String FT = Convert.ToDateTime(tpFromTime.Value).ToShortTimeString(); ;
                                TimeSpan TT = Convert.ToDateTime(tpFromTime.Value).TimeOfDay;
                                string Subject = "Your appointment has been confirmed with " + Doctor + " On " + Date + " at " + FT;
                                EmailClient.SendEmailAsync(Email, txtEmail.Text, " Appointment", Subject);
                                EmailClient.CloseAsync();
                            }
                        }
                        this.Title = "Patient Appointment -  " + "Patient Name";

                        if (Item.IsReschedule == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Rescheduled Appointment Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            ClickedFlag = 0;
                            //Added by ajit date - 3/10/2016  
                            this.DialogResult = false;
                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                            UIElement MyData = new AppointmentList();
                            ((IApplicationConfiguration)App.Current).OpenMainContent(MyData);
                            //***//------------
                        }
                        else
                        {

                            if (((clsAddAppointmentBizActionVO)arg.Result).AppointmentDetails.ResultStatus == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Different Appointment With Same Time Slot Could Not Be Saved.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                            else
                            {
                                if (IsFromFollowUp == true)
                                {
                                    this.Content = null;
                                    this.DialogResult = false;
                                    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                                    this.DataContext = null;
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Appointment Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    //Added by ajit date - 3/10/2016  
                                    this.DialogResult = false;
                                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                    UIElement MyData = new AppointmentList();
                                    ((IApplicationConfiguration)App.Current).OpenMainContent(MyData);
                                }
                                ClickedFlag = 0;

                                txtContactNo1.IsEnabled = true;
                                txtFirstName.IsEnabled = true;
                                txtMiddleName.IsEnabled = true;
                                txtLastName.IsEnabled = true;
                                cmbBloodGroup.IsEnabled = true;
                                cmbGender.IsEnabled = true;
                                dtpDOB1.IsEnabled = true;
                                cmbMaritalStatus.IsEnabled = true;
                                txtFamilyName.IsEnabled = true;
                                txtMobileCountryCode.IsEnabled = true;
                                txtResiSTD.IsEnabled = true;
                                txtResiCountryCode.IsEnabled = true;
                                // txtFaxNo.IsEnabled = false;
                                txtEmail.IsEnabled = true;
                                txtContactNo2.IsEnabled = true;
                                txtYY.IsEnabled = true;
                                txtMM.IsEnabled = true;
                                txtDD.IsEnabled = true;
                                cmbNationality.IsEnabled = true;
                            }
                        }
                        this.DataContext = new clsAppointmentVO
                        {
                            UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                            DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                            DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                            AppointmentDate = DateTime.Now.Date.Date
                        };
                        ClearControl();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Adding Appointment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }

        /// <summary>
        /// Purpose:Check entered doctor time slots r available or not.
        /// </summary>
        /// <param name="result"></param>
        /// 

        int ResultStatus = 0;

        void checkDuplication()
        {
            clsCheckPatientDuplicacyAppointmentBizActionVO BizAction = new clsCheckPatientDuplicacyAppointmentBizActionVO();
            BizAction.AppointmentDetails = ((clsAppointmentVO)this.DataContext);
            if ((MasterListItem)cmbGender.SelectedItem != null)
                BizAction.AppointmentDetails.GenderId = ((MasterListItem)cmbGender.SelectedItem).ID;
            if (dtpDOB1.SelectedDate != null)
            {
                BizAction.AppointmentDetails.DOB = dtpDOB1.SelectedDate.Value.Date;
            }

            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text))
            {
                BizAction.AppointmentDetails.MobileCountryCode = txtMobileCountryCode.Text;
            }

            if (!string.IsNullOrEmpty(txtContactNo1.Text))
            {
                BizAction.AppointmentDetails.ContactNo1 = txtContactNo1.Text;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    if (((clsCheckPatientDuplicacyAppointmentBizActionVO)ea.Result).SuccessStatus != 0)
                    {
                        ClickedFlag = 0;
                        string strDuplicateMsg = "";
                        if (((clsCheckPatientDuplicacyAppointmentBizActionVO)ea.Result).SuccessStatus == 3)
                        {
                            strDuplicateMsg = "Mobile Number already exists, Are you sure you want to save the Patient Details ?";
                            MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", strDuplicateMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW1.Show();
                            ClickedFlag = 1;
                        }
                        else if (((clsCheckPatientDuplicacyAppointmentBizActionVO)ea.Result).SuccessStatus == 2)
                        {
                            strDuplicateMsg = "Patient already exists with same Name and Gender, Are you sure you want to save the Patient Details ?";

                            MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", strDuplicateMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW1.Show();
                            ClickedFlag = 1;
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient already exists.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            ResultStatus = 1;
                            ClickedFlag = 0;
                        }
                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "Are You Sure You Want To Save The Appointment?";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                        msgW.Show();
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            bool Flag = true;
            if (result == MessageBoxResult.Yes)
            {
                clsGetAppointmentByDoctorAndAppointmentDateBizActionVO BizActionobj = new clsGetAppointmentByDoctorAndAppointmentDateBizActionVO();
                BizActionobj.DoctorId = ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID;
                BizActionobj.DepartmentId = ((MasterListItem)cmbDepartment.SelectedItem).ID;
                BizActionobj.UnitId = ((MasterListItem)cmbAUnit.SelectedItem).ID;
                BizActionobj.AppointmentDate = dtpAppointmentDate.SelectedDate;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (sa, arg1) =>
                {
                    if (arg1.Error == null)
                    {
                        if (((clsGetAppointmentByDoctorAndAppointmentDateBizActionVO)arg1.Result).AppointmentDetailsList != null)
                        {
                            ChkFromTimeList = ((clsGetAppointmentByDoctorAndAppointmentDateBizActionVO)arg1.Result).AppointmentDetailsList;
                            for (int i = 0; i < ChkFromTimeList.Count; i++)
                            {
                                DateTime AlreadyFromDatetime = new DateTime(dtpAppointmentDate.SelectedDate.Value.Year, dtpAppointmentDate.SelectedDate.Value.Month, dtpAppointmentDate.SelectedDate.Value.Day, Convert.ToInt16(ChkFromTimeList[i].FromTime.Value.Hour), Convert.ToInt16(ChkFromTimeList[i].FromTime.Value.Minute), 0);
                                DateTime AlreadyToDatetime = new DateTime(dtpAppointmentDate.SelectedDate.Value.Year, dtpAppointmentDate.SelectedDate.Value.Month, dtpAppointmentDate.SelectedDate.Value.Day, Convert.ToInt16(ChkFromTimeList[i].ToTime.Value.Hour), Convert.ToInt16(ChkFromTimeList[i].ToTime.Value.Minute), 0);

                                DateTime FromDatetime = new DateTime(dtpAppointmentDate.SelectedDate.Value.Year, dtpAppointmentDate.SelectedDate.Value.Month, dtpAppointmentDate.SelectedDate.Value.Day, Convert.ToInt16(tpFromTime.Value.Value.Hour), Convert.ToInt16(tpFromTime.Value.Value.Minute), 0);
                                DateTime ToDatetime = new DateTime(dtpAppointmentDate.SelectedDate.Value.Year, dtpAppointmentDate.SelectedDate.Value.Month, dtpAppointmentDate.SelectedDate.Value.Day, Convert.ToInt16(tpToTime.Value.Value.Hour), Convert.ToInt16(tpToTime.Value.Value.Minute), 0);
                                if (FromDatetime == AlreadyFromDatetime)
                                {
                                    Flag = false;
                                }
                                else if (ToDatetime == AlreadyToDatetime)
                                {
                                    Flag = false;
                                }
                                else if (FromDatetime < AlreadyFromDatetime && ToDatetime > AlreadyToDatetime)
                                {
                                    Flag = false;
                                }
                                else
                                {
                                    while (ToDatetime >= AlreadyFromDatetime && ToDatetime <= AlreadyToDatetime)
                                    {
                                        if (ToDatetime > AlreadyFromDatetime && ToDatetime < AlreadyToDatetime)
                                        {
                                            Flag = false;
                                            break;
                                        }
                                        ToDatetime = ToDatetime.AddMinutes(AppointmentTimeSlots);
                                    }
                                    while (FromDatetime >= AlreadyFromDatetime && FromDatetime <= AlreadyToDatetime)
                                    {
                                        if (FromDatetime > AlreadyFromDatetime && FromDatetime < AlreadyToDatetime)
                                        {
                                            Flag = false;
                                            break;
                                        }
                                        FromDatetime = FromDatetime.AddMinutes(AppointmentTimeSlots);
                                    }
                                }
                            }
                            if (Flag == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Time slot is Already Booked.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                                ClickedFlag = 0;
                            }
                            else
                            {
                                CheckDoctorAvailableOrNot();
                                //SaveAppointment();
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Adding Appointment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionobj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                ClickedFlag = 0;
            }
        }

        /// <summary>
        /// Purpose:Check selected doctor time is available or not
        /// </summary>
        private void CheckDoctorAvailableOrNot()
        {
            if (count > 1)
            {
                foreach (var a in listSchedule)
                {
                    if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay == a.ScheduleToTime.TimeOfDay)
                    {
                        SaveAppointment();
                        count = 0;
                        break;
                    }
                    else if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay > a.ScheduleToTime.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW20.Show();
                        ClickedFlag = 0;
                        break;
                    }
                    else if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay < a.ScheduleToTime.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW20.Show();
                        ClickedFlag = 0;
                        break;
                    }
                }
            }
            else if (tpFromTime.Value != null)
            {
                if (Schedule1Flag == true && Schedule2Flag == true)
                {

                    if (tpFromTime.Value.Value.TimeOfDay < DocSchedule1FromTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW20.Show();
                        ClickedFlag = 0;
                    }
                    else if (tpFromTime.Value.Value.TimeOfDay >= DocSchedule1ToTime.Value.TimeOfDay && tpFromTime.Value.Value.TimeOfDay < DocSchedule2FromTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW21 =
                                                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW21.Show();
                        ClickedFlag = 0;
                    }
                    else if (tpFromTime.Value.Value.TimeOfDay >= DocSchedule2ToTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW22 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW22.Show();
                        ClickedFlag = 0;
                    }
                    else
                    {
                        SaveAppointment();
                    }
                }
                else if (Schedule1Flag == true)
                {
                    if (count > 1)
                    {
                        foreach (var a in listSchedule)
                        {
                            if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay == a.ScheduleToTime.TimeOfDay)
                            {
                                SaveAppointment();
                                count = 0;
                                break;
                            }
                            else if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay > a.ScheduleToTime.TimeOfDay)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW20.Show();
                                ClickedFlag = 0;
                                break;
                            }
                            else if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay < a.ScheduleToTime.TimeOfDay)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW20.Show();
                                ClickedFlag = 0;
                                break;
                            }
                            else
                            {

                            }
                        }
                    }

                    else if (tpFromTime.Value.Value.TimeOfDay < DocSchedule1FromTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW20.Show();
                        ClickedFlag = 0;
                    }
                    else if (tpFromTime.Value.Value.TimeOfDay >= DocSchedule1ToTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW22 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW22.Show();
                        ClickedFlag = 0;
                    }
                    else
                    {
                        SaveAppointment();
                    }
                }
                else if (Schedule2Flag == true)
                {
                    if (count > 1)
                    {
                        foreach (var a in listSchedule)
                        {
                            if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay == a.ScheduleToTime.TimeOfDay)
                            {
                                SaveAppointment();
                                count = 0;
                                break;
                            }
                            else if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay > a.ScheduleToTime.TimeOfDay)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW20.Show();
                                ClickedFlag = 0;
                                break;
                            }
                            else if ((tpFromTime.Value.Value.TimeOfDay == a.ScheduleFromTime.TimeOfDay) && tpToTime.Value.Value.TimeOfDay < a.ScheduleToTime.TimeOfDay)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW20.Show();
                                ClickedFlag = 0;
                                break;
                            }
                        }
                    }

                    else if (tpFromTime.Value.Value.TimeOfDay < DocSchedule2FromTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW20.Show();
                        ClickedFlag = 0;
                    }
                    else if (tpFromTime.Value.Value.TimeOfDay >= DocSchedule2ToTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW22 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW22.Show();
                        ClickedFlag = 0;
                    }
                    else
                    {
                        SaveAppointment();
                    }
                }
            }
        }

        #endregion
        HyperlinkButton Selectedhb;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        public bool IsGetError = false;

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            IsGetError = true;
            Indicatior.Show();
            this.DialogResult = false;
            if (IsFromFollowUp == true)
            {
                this.Content = null;
                this.DialogResult = false;
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                this.DataContext = null;
                IsFromAppointment = false;
                Indicatior.Close();
                return;
            }
            if (IsFromAppointment == true)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                UIElement MyData = new AppointmentList();
                ((IApplicationConfiguration)App.Current).OpenMainContent(MyData);
                this.DataContext = null;
            }

            else
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                this.DataContext = null;

            }
            Indicatior.Close();
        }

        /// <summary>
        /// Purpose:Display doctor schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void cmbDoctorAddNewAppointment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTimeSlots.ItemsSource = null;
            tpFromTime.Value = null;
            tpToTime.Value = null;
            if (cmbDoctorAddNewAppointment.SelectedItem != null)
            {
                if (((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID != 0)
                {
                    dgTimeSlots.ItemsSource = null;
                    GetSchedule();
                }
            }
            tpFromTime.IsEnabled = false;
            tpToTime.IsEnabled = false;
        }

        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;

                        if (objUser.IsCrossAppointment && Item.IsReschedule != true)
                        {
                            cmbAUnit.IsEnabled = true;
                        }
                        else
                        {
                            cmbAUnit.IsEnabled = false;

                        }
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void dtpAppointmentDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsGetError == false)
            {
                dgTimeSlots.ItemsSource = null;
                if (cmbDoctorAddNewAppointment.ItemsSource != null && cmbDepartment.ItemsSource != null)
                {
                    if (((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID != 0 && ((MasterListItem)cmbAUnit.SelectedItem).ID != 0 && ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID != 0)
                    {
                        dgTimeSlots.ItemsSource = null;
                        GetSchedule();
                    }
                }
            }
        }

        /// <summary>
        /// Purpose:Get selected time of doctor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgTimeSlots_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clsSchedule objSchedule = new clsSchedule();
            try
            {
                if ((clsSchedule)dgTimeSlots.SelectedItem != null)
                {

                    objSchedule = (clsSchedule)dgTimeSlots.SelectedItem;
                    Color C = new Color();
                    C.R = 198;
                    C.B = 24;
                    C.G = 15;
                    C.A = 255;
                    SolidColorBrush ABC = new SolidColorBrush(C);

                    if (objSchedule.apColor.Color == ABC.Color)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW10 =
                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Time Slot Is Already Booked.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW10.Show();
                    }
                    else if (objSchedule.apColor.Color == Colors.Gray)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW11 =
                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Time Slot Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW11.Show();
                    }
                    else
                    {


                        tpFromTime.Value = Convert.ToDateTime(objSchedule.ScheduleFromTime);
                        tpToTime.Value = Convert.ToDateTime(objSchedule.ScheduleToTime);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #region  Reset All Controls
        /// <summary>
        /// Purpose:Clear All Controls.
        /// </summary>
        private void ClearControl()
        {
            txtMRNoPatientInformation.Text = string.Empty;
            tpFromTime.Value = null;
            tpToTime.Value = null;
            cmbBloodGroup.SelectedValue = ((clsAppointmentVO)this.DataContext).BloodId; ;
            cmbMaritalStatus.SelectedValue = ((clsAppointmentVO)this.DataContext).MaritalStatusId;
            cmbGender.SelectedValue = ((clsAppointmentVO)this.DataContext).GenderId;
            cmbAppointmentType.SelectedValue = ((clsAppointmentVO)this.DataContext).AppointmentReasonId;
            cmbSpRegistration.SelectedValue = ((clsAppointmentVO)this.DataContext).SpecialRegistrationID;
            dtpDOB1.Text = string.Empty;
            dgTimeSlots.ItemsSource = null;
            txtDD.Text = string.Empty;
            txtYY.Text = string.Empty;
            txtMM.Text = string.Empty;
            txtResiSTD.Text = string.Empty;
            txtResiCountryCode.Text = string.Empty;
            txtMobileCountryCode.Text = string.Empty;
            txtContactNo1.Text = string.Empty;
            txtContactNo2.Text = string.Empty;
            chkEmail.IsChecked = false;
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
        }

        #endregion

        #region Calculate age
        /// <summary>
        /// Purpose:Calculete age from birthdate
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <param name="DateTobeConvert"></param>
        /// <returns></returns>
        private DateTime? ConvertDateBack(string parameter, int value, DateTime? DateTobeConvert)
        {
            try
            {
                DateTime BirthDate;
                if (DateTobeConvert != null && parameter.ToString().ToUpper() != "YY")
                    BirthDate = DateTobeConvert.Value;
                else
                    BirthDate = DateTime.Now;


                int mValue = Int32.Parse(value.ToString());

                switch (parameter.ToString().ToUpper())
                {
                    case "YY":
                        BirthDate = BirthDate.AddYears(-mValue);

                        break;
                    case "MM":
                        BirthDate = BirthDate.AddMonths(-mValue);
                        // result = (age.Month - 1).ToString();
                        break;
                    case "DD":
                        //result = (age.Day - 1).ToString();
                        BirthDate = BirthDate.AddDays(-mValue);
                        break;
                    default:
                        BirthDate = BirthDate.AddYears(-mValue);
                        break;
                }
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }

        }

        private string ConvertDate(object Datevalue, string parameter)
        {
            if (Datevalue != null)
            {
                try
                {
                    DateTime BirthDate = (DateTime)Datevalue;
                    TimeSpan difference = DateTime.Now.Subtract(BirthDate);

                    //return date.ToString(parameter.ToString());
                    // This is to convert the timespan to datetime object
                    DateTime age = DateTime.MinValue + difference;

                    // Min value is 01/01/0001
                    // Actual age is say 24 yrs, 9 months and 3 days represented as timespan
                    // Min Valye + actual age = 25 yrs , 10 months and 4 days.
                    // subtract our addition or 1 on all components to get the actual date.
                    string result = "";
                    switch (parameter.ToString().ToUpper())
                    {
                        case "YY":
                            result = (age.Year - 1).ToString();
                            break;
                        case "MM":
                            result = (age.Month - 1).ToString();
                            break;
                        case "DD":
                            int day = BirthDate.Day;
                            int curday = DateTime.Now.Day;
                            int dif = 0;
                            if (day > curday)
                                dif = (curday + 30) - day;
                            else
                                dif = curday - day;
                            result = dif.ToString();
                            //result = (age.Day - 1).ToString();
                            break;
                        default:
                            result = (age.Year - 1).ToString();
                            break;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        #endregion

        #region Get Schedule
        /// <summary>
        /// Purpose:Get selected doctor schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 




        private void GetSchedule()
        {
            dgTimeSlots.ItemsSource = null;
            listSchedule.Clear();
            tpFromTime.IsEnabled = false;
            tpToTime.IsEnabled = false;

            try
            {
                if (((clsAppointmentVO)this.DataContext).AppointmentDate != null)
                {
                    if (cmbDoctorAddNewAppointment.SelectedItem != null && ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID != 0)
                    {
                        GetDoctorScheduleTimeVO BizAction = new GetDoctorScheduleTimeVO();
                        BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();

                        if (cmbAUnit.SelectedItem != null)
                            BizAction.UnitId = ((MasterListItem)cmbAUnit.SelectedItem).ID;

                        if (cmbDepartment.SelectedItem != null)
                            BizAction.DepartmentId = ((MasterListItem)cmbDepartment.SelectedItem).ID;

                        if (cmbDoctorAddNewAppointment.SelectedItem != null)
                            BizAction.DoctorId = ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID;

                        string objDay = null;
                        if (dtpAppointmentDate.SelectedDate != null)
                        {

                            objDay = dtpAppointmentDate.SelectedDate.Value.DayOfWeek.ToString();
                        }

                        if (dtpAppointmentDate.SelectedDate != null)    //For New Doctor Schedule Changes modified on 29052018 
                            BizAction.Date = dtpAppointmentDate.SelectedDate.Value.Date;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        dgTimeSlots.ItemsSource = null;
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (((GetDoctorScheduleTimeVO)arg.Result).DoctorScheduleDetailsList != null && ((GetDoctorScheduleTimeVO)arg.Result).DoctorScheduleDetailsList.Count != 0)
                                {

                                    GetDoctorScheduleTimeVO DetailsVO = new GetDoctorScheduleTimeVO();
                                    DetailsVO = (GetDoctorScheduleTimeVO)arg.Result;


                                    if (DetailsVO.DoctorScheduleDetailsList != null)
                                    {

                                        List<clsDoctorScheduleDetailsVO> ObjItem;
                                        ObjItem = DetailsVO.DoctorScheduleDetailsList;
                                        bool DayFlag = false;
                                        foreach (var item in ObjItem)
                                        {
                                            if (BizAction.DoctorId == item.DoctorID)
                                            {
                                                double Schedule1Hrs = 0;
                                                double Schedule2Hrs = 0;

                                                if (objDay == item.Day)
                                                {
                                                    count++;
                                                    DayFlag = true;
                                                    if (item.StartTime != null && item.EndTime != null)
                                                    {
                                                        if (item.ScheduleID == 1)
                                                        {
                                                            Schedule1Flag = true;
                                                            DocSchedule1FromTime = item.StartTime;
                                                            DocSchedule1ToTime = item.EndTime;
                                                            TimeSpan tm1;

                                                            tm1 = ((DateTime)(item.EndTime)).Subtract((DateTime)(item.StartTime));
                                                            Schedule1Hrs = tm1.TotalMinutes / AppointmentTimeSlots;
                                                        }
                                                        else
                                                        {
                                                            Schedule2Flag = true;
                                                            DocSchedule2FromTime = item.StartTime;
                                                            DocSchedule2ToTime = item.EndTime;

                                                            TimeSpan tm;

                                                            tm = ((DateTime)(item.EndTime)).Subtract((DateTime)(item.StartTime));
                                                            Schedule2Hrs = tm.TotalMinutes / AppointmentTimeSlots;
                                                        }
                                                    }

                                                    DateTime? Dt = null;
                                                    if (item.StartTime != null)
                                                        Dt = item.StartTime;

                                                    for (int Schedulecount = 0; Schedulecount < Schedule1Hrs; Schedulecount++)
                                                    {

                                                        listSchedule.Add(new clsSchedule()
                                                        {
                                                            Day = "Schedule1",
                                                            ScheduleFromTime = Dt.Value,
                                                            ScheduleToTime = (Dt.Value.AddMinutes(AppointmentTimeSlots)),
                                                            apColor = new SolidColorBrush(Colors.Green)

                                                        }

                                                        );
                                                        Dt = Dt.Value.AddMinutes(AppointmentTimeSlots);
                                                    }
                                                    for (int Schedulecount = 0; Schedulecount < Schedule2Hrs; Schedulecount++)
                                                    {

                                                        listSchedule.Add(new clsSchedule()
                                                        {
                                                            Day = "Schedule2",
                                                            ScheduleFromTime = Dt.Value,
                                                            ScheduleToTime = (Dt.Value.AddMinutes(AppointmentTimeSlots)),
                                                            apColor = new SolidColorBrush(Colors.Green)
                                                        }
                                                        );

                                                        Dt = Dt.Value.AddMinutes(AppointmentTimeSlots);

                                                    }

                                                    PagedCollectionView collection = new PagedCollectionView(listSchedule);
                                                    collection.GroupDescriptions.Add(new PropertyGroupDescription("Day"));
                                                    dgTimeSlots.ItemsSource = collection;
                                                    dgTimeSlots.SelectedIndex = -1;


                                                    clsGetAppointmentByDoctorAndAppointmentDateBizActionVO BizActionobj = new clsGetAppointmentByDoctorAndAppointmentDateBizActionVO();
                                                    BizActionobj.DoctorId = BizAction.DoctorId;
                                                    BizActionobj.DepartmentId = BizAction.DepartmentId;
                                                    BizActionobj.UnitId = BizAction.UnitId;
                                                    BizActionobj.AppointmentDate = dtpAppointmentDate.SelectedDate;

                                                    client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                                    client.ProcessCompleted += (sa, arg1) =>
                                                    {
                                                        if (arg1.Error == null)
                                                        {
                                                            if (((clsGetAppointmentByDoctorAndAppointmentDateBizActionVO)arg1.Result).AppointmentDetailsList != null)
                                                            {

                                                                List<clsAppointmentVO> AppointmentList = ((clsGetAppointmentByDoctorAndAppointmentDateBizActionVO)arg1.Result).AppointmentDetailsList;



                                                                //For already booked time
                                                                foreach (clsAppointmentVO a in AppointmentList)
                                                                {

                                                                    for (int ListCount = 0; ListCount < listSchedule.Count; ListCount++)
                                                                    {
                                                                        if (((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay <= a.ToTime.Value.TimeOfDay || ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.ToTime.Value.TimeOfDay)
                                                                        {
                                                                            if (a.FromTime.Value.TimeOfDay == ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay)
                                                                            {
                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;

                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                //   listSchedule[ListCount].apColor = new SolidColorBrush(Colors.Gray);
                                                                                //break;
                                                                            }
                                                                            //--------------Added by harish
                                                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay >= a.FromTime.Value.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay <= a.ToTime.Value.TimeOfDay)
                                                                            {

                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;

                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                //break;
                                                                            }
                                                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.FromTime.Value.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay > a.FromTime.Value.TimeOfDay)
                                                                            {
                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;

                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                //break;

                                                                            }
                                                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay > a.FromTime.Value.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.ToTime.Value.TimeOfDay)
                                                                            {

                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;

                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                //break;


                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            break;
                                                                        }
                                                                    }
                                                                }

                                                                //For Past Time
                                                                foreach (var item4 in listSchedule)
                                                                {
                                                                    int hh = item4.ScheduleFromTime.Hour;
                                                                    int mm = item4.ScheduleFromTime.Minute;
                                                                    if (listSchedule.Count > 1)
                                                                    {

                                                                    }
                                                                    DateTime AppTime = new DateTime(dtpAppointmentDate.SelectedDate.Value.Year, dtpAppointmentDate.SelectedDate.Value.Month, dtpAppointmentDate.SelectedDate.Value.Day, hh, mm, 0);
                                                                    if (DateTime.Now > AppTime)
                                                                    {
                                                                        item4.apColor = new SolidColorBrush(Colors.Gray);
                                                                    }

                                                                }


                                                                dgTimeSlots.ItemsSource = collection;
                                                                dgTimeSlots.SelectedIndex = -1;
                                                            }


                                                        }
                                                        else
                                                        {
                                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                                            msgW1.Show();
                                                        }


                                                    };

                                                    client.ProcessAsync(BizActionobj, ((IApplicationConfiguration)App.Current).CurrentUser);
                                                    client.CloseAsync();


                                                }

                                            }
                                        }
                                        if (DayFlag == false)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW6 =
                                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                            msgW6.Show();

                                            tpFromTime.IsEnabled = false;
                                            tpToTime.IsEnabled = false;


                                        }
                                    }
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW6 =
                                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                    msgW6.Show();

                                    tpFromTime.IsEnabled = false;
                                    tpToTime.IsEnabled = false;
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
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW10 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Doctor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW10.Show();
                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW12 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Appointment Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW12.Show();

                    tpFromTime.IsEnabled = false;
                    tpToTime.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void hlSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((clsAppointmentVO)this.DataContext).AppointmentDate != null)
                {
                    if (cmbDoctorAddNewAppointment.SelectedItem != null && ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID != 0)
                    {
                        GetDoctorScheduleTimeVO BizAction = new GetDoctorScheduleTimeVO();
                        BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();

                        if (cmbAUnit.SelectedItem != null)
                            BizAction.UnitId = ((MasterListItem)cmbAUnit.SelectedItem).ID;

                        if (cmbDepartment.SelectedItem != null)
                            BizAction.DepartmentId = ((MasterListItem)cmbDepartment.SelectedItem).ID;

                        if (cmbDoctorAddNewAppointment.SelectedItem != null)
                            BizAction.DoctorId = ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID;

                        string objDay = null;
                        if (dtpAppointmentDate.SelectedDate != null)
                        {

                            objDay = dtpAppointmentDate.SelectedDate.Value.DayOfWeek.ToString();
                        }

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        dgTimeSlots.ItemsSource = null;
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (((GetDoctorScheduleTimeVO)arg.Result).DoctorScheduleDetailsList != null && ((GetDoctorScheduleTimeVO)arg.Result).DoctorScheduleDetailsList.Count != 0)
                                {

                                    GetDoctorScheduleTimeVO DetailsVO = new GetDoctorScheduleTimeVO();
                                    DetailsVO = (GetDoctorScheduleTimeVO)arg.Result;
                                    List<clsSchedule> listSchedule = new List<clsSchedule>();

                                    if (DetailsVO.DoctorScheduleDetailsList != null)
                                    {

                                        List<clsDoctorScheduleDetailsVO> ObjItem;
                                        ObjItem = DetailsVO.DoctorScheduleDetailsList;
                                        bool DayFlag = false;
                                        foreach (var item in ObjItem)
                                        {
                                            if (BizAction.DoctorId == item.DoctorID)
                                            {
                                                double Schedule1Hrs = 0;
                                                double Schedule2Hrs = 0;

                                                if (objDay == item.Day)
                                                {
                                                    DayFlag = true;
                                                    if (item.StartTime != null && item.EndTime != null)
                                                    {
                                                        if (item.ScheduleID == 1)
                                                        {
                                                            Schedule1Flag = true;
                                                            DocSchedule1FromTime = item.StartTime;
                                                            DocSchedule1ToTime = item.EndTime;
                                                            TimeSpan tm1;

                                                            tm1 = ((DateTime)(item.EndTime)).Subtract((DateTime)(item.StartTime));
                                                            Schedule1Hrs = tm1.TotalMinutes / AppointmentTimeSlots;
                                                        }
                                                        else
                                                        {
                                                            Schedule2Flag = true;
                                                            DocSchedule2FromTime = item.StartTime;
                                                            DocSchedule2ToTime = item.EndTime;

                                                            TimeSpan tm;

                                                            tm = ((DateTime)(item.EndTime)).Subtract((DateTime)(item.StartTime));
                                                            Schedule2Hrs = tm.TotalMinutes / AppointmentTimeSlots;
                                                        }
                                                    }

                                                    DateTime? Dt = null;
                                                    if (item.StartTime != null)
                                                        Dt = item.StartTime;

                                                    for (int Schedulecount = 0; Schedulecount < Schedule1Hrs; Schedulecount++)
                                                    {

                                                        listSchedule.Add(new clsSchedule()
                                                        {
                                                            Day = "Schedule1",
                                                            ScheduleFromTime = Dt.Value,
                                                            ScheduleToTime = (Dt.Value.AddMinutes(AppointmentTimeSlots)),
                                                            apColor = new SolidColorBrush(Colors.Green)

                                                        }

                                                        );
                                                        Dt = Dt.Value.AddMinutes(AppointmentTimeSlots);
                                                    }
                                                    for (int Schedulecount = 0; Schedulecount < Schedule2Hrs; Schedulecount++)
                                                    {

                                                        listSchedule.Add(new clsSchedule()
                                                        {
                                                            Day = "Schedule2",
                                                            ScheduleFromTime = Dt.Value,
                                                            ScheduleToTime = (Dt.Value.AddMinutes(AppointmentTimeSlots)),
                                                            apColor = new SolidColorBrush(Colors.Green)
                                                        }
                                                        );

                                                        Dt = Dt.Value.AddMinutes(AppointmentTimeSlots);

                                                    }

                                                    PagedCollectionView collection = new PagedCollectionView(listSchedule);
                                                    collection.GroupDescriptions.Add(new PropertyGroupDescription("Day"));
                                                    dgTimeSlots.ItemsSource = collection;
                                                    dgTimeSlots.SelectedIndex = -1;


                                                    clsGetAppointmentByDoctorAndAppointmentDateBizActionVO BizActionobj = new clsGetAppointmentByDoctorAndAppointmentDateBizActionVO();
                                                    BizActionobj.DoctorId = BizAction.DoctorId;
                                                    BizActionobj.DepartmentId = BizAction.DepartmentId;
                                                    BizActionobj.UnitId = BizAction.UnitId;
                                                    BizActionobj.AppointmentDate = dtpAppointmentDate.SelectedDate;

                                                    client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                                    client.ProcessCompleted += (sa, arg1) =>
                                                    {
                                                        if (arg1.Error == null)
                                                        {
                                                            if (((clsGetAppointmentByDoctorAndAppointmentDateBizActionVO)arg1.Result).AppointmentDetailsList != null)
                                                            {

                                                                List<clsAppointmentVO> AppointmentList = ((clsGetAppointmentByDoctorAndAppointmentDateBizActionVO)arg1.Result).AppointmentDetailsList;

                                                                //For Past Time
                                                                foreach (var item4 in listSchedule)
                                                                {
                                                                    int hh = item4.ScheduleFromTime.Hour;
                                                                    int mm = item4.ScheduleFromTime.Minute;
                                                                    DateTime AppTime = new DateTime(dtpAppointmentDate.SelectedDate.Value.Year, dtpAppointmentDate.SelectedDate.Value.Month, dtpAppointmentDate.SelectedDate.Value.Day, hh, mm, 0);
                                                                    if (DateTime.Now > AppTime)
                                                                    {
                                                                        item4.apColor = new SolidColorBrush(Colors.Gray);
                                                                    }

                                                                }

                                                                //For already booked time
                                                                foreach (clsAppointmentVO a in AppointmentList)
                                                                {

                                                                    for (int ListCount = 0; ListCount < listSchedule.Count; ListCount++)
                                                                    {
                                                                        if (((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay <= a.ToTime.Value.TimeOfDay || ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.ToTime.Value.TimeOfDay)
                                                                        {
                                                                            if (a.FromTime.Value.TimeOfDay == ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay)
                                                                            {
                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;

                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                //break;
                                                                            }
                                                                            //--------------Added by harish
                                                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay >= a.FromTime.Value.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay <= a.ToTime.Value.TimeOfDay)
                                                                            {

                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;

                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                //break;
                                                                            }
                                                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.FromTime.Value.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay > a.FromTime.Value.TimeOfDay)
                                                                            {
                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;

                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                //break;

                                                                            }
                                                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay > a.FromTime.Value.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.ToTime.Value.TimeOfDay)
                                                                            {

                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;

                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                //break;


                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            break;
                                                                        }
                                                                    }
                                                                }


                                                                dgTimeSlots.ItemsSource = collection;
                                                                dgTimeSlots.SelectedIndex = -1;
                                                            }


                                                        }
                                                        else
                                                        {
                                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                                            msgW1.Show();
                                                        }


                                                    };

                                                    client.ProcessAsync(BizActionobj, ((IApplicationConfiguration)App.Current).CurrentUser);
                                                    client.CloseAsync();


                                                }

                                            }
                                        }
                                        if (DayFlag == false)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW6 =
                                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                            msgW6.Show();

                                        }
                                    }
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW6 =
                                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule Is Not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                    msgW6.Show();


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
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW10 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Doctor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW10.Show();
                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW12 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Appointment Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW12.Show();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private void tpFromTime_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void chkEmail_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtContactNo1_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtContactNo1_KeyDown_1(object sender, KeyEventArgs e)
        {

        }

        private void txtContactNo1_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

		private void GetActiveAdmissionOfPatient(string MRNO)
        {
            clsGetActiveAdmissionBizActionVO BizObject = new clsGetActiveAdmissionBizActionVO();
            BizObject.PatientID = 0;
            BizObject.PatientUnitID = 0;
            BizObject.MRNo = MRNO;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsGetActiveAdmissionBizActionVO)arg.Result).Details != null && ((clsGetActiveAdmissionBizActionVO)arg.Result).Details.AdmID > 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient is already admitted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            SearchAppointmentByMrNo(txtMRNoPatientInformation.Text);
                        }
                    }
                }
            };
            Client.ProcessAsync(BizObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbSpecilization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if ((MasterListItem)cmbSpecilization.SelectedItem != null)
            //{

            //    FillDoctor(((MasterListItem)cmbAUnit.SelectedItem).ID, ((MasterListItem)cmbSpecilization.SelectedItem).ID);
            //}
            //dgTimeSlots.ItemsSource = null;
            //tpFromTime.Value = null;
            //tpToTime.Value = null;
        }

        #region Commented Code
        //string time = ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.ToShortTimeString();
        //String mm = time.Substring(time.IndexOf(":") + 1, time.IndexOf(" ") - 2);
        //string hh = time.Substring(0, time.IndexOf(":"));
        //for (int k = 0; k < time.Length; k++)
        //{

        //    if (time[k].Equals('A'))
        //    {

        //    }

        //    if (time[k].Equals('P'))
        //    {
        //        if (Convert.ToInt32(hh) < 12)
        //        {
        //            hh = (Convert.ToInt32(hh) + 12).ToString();
        //        }
        //    }


        //}
        #endregion



        private void FillSpecializationList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbSpecilization.ItemsSource = null;
                    cmbSpecilization.ItemsSource = objList.ToList();
                    cmbSpecilization.SelectedItem = objList[0];
                }

                //if (this.DataContext != null)
                //{
                //    cmbSpecilization.SelectedValue = ((clsAppointmentVO)this.DataContext).AppointmentReasonId;
                //}
                //if (IsFromFollowUp == true)
                //{
                //    cmbSpecilization.SelectedValue = FollowUpAppoinment;
                //}
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }



    }

    public class clsSchedule : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _ScheduleFromTime;
        public DateTime ScheduleFromTime
        {
            get
            {
                return _ScheduleFromTime;
            }
            set
            {
                _ScheduleFromTime = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ScheduleFromTime"));
                }

            }
        }


        private DateTime _ScheduleToTime;
        public DateTime ScheduleToTime
        {
            get
            {
                return _ScheduleToTime;
            }
            set
            {
                _ScheduleToTime = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ScheduleToTime"));
                }
                //PropertyChangedHandler("PlayerName");
            }
        }

        private string _Day;
        public string Day
        {
            get
            {
                return _Day;
            }
            set
            {
                _Day = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Day"));
                }
                //PropertyChangedHandler("PlayerName");
            }
        }


        private SolidColorBrush _apColor;
        public SolidColorBrush apColor
        {
            get
            {
                return _apColor;
            }
            set
            {
                _apColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("apColor"));
                }
                //PropertyChangedHandler("PlayerName");
            }
        }
        private SolidColorBrush _foreColor;
        public SolidColorBrush foreColor
        {
            get
            {
                return _foreColor;
            }
            set
            {
                _foreColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("foreColor"));
                }
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get
            {
                return _PatientName;
            }
            set
            {
                _PatientName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PatientName"));
                }

            }
        }
    }
}
