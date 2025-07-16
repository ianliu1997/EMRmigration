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
using CIMS.Forms;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.EmailServiceReference;
using System.Globalization;
using System.Windows.Data;
using MessageBoxControl;
using PalashDynamics;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;

namespace OPDModule.Forms
{
    public partial class frmReschedulePatientAppointment : ChildWindow, IInitiateCIMS
    {
        int ClickedFlag = 0;
        #region Variable Declaration
        public event RoutedEventHandler OnSaveButton_Click;
        public string Email { get; set; }
        public double AppointmentTimeSlots;


        clsAppointmentVO Item;
        public bool Status { get; set; }
        public bool TimeStatus { get; set; }

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        PatientVisitDetails win = new PatientVisitDetails();

        bool IsPatientExist = false;

        public clsGetPatientBizActionVO OBJPatient { get; set; }

        bool UseApplicationID = true;
        bool UseApplicationDoctorID = true;

        public List<clsAppointmentVO> ChkFromTimeList { get; set; }


        DateTime? DocSchedule1FromTime { get; set; }
        DateTime? DocSchedule1ToTime { get; set; }

        DateTime? DocSchedule2FromTime { get; set; }
        DateTime? DocSchedule2ToTime { get; set; }

        bool Schedule1Flag = false;
        bool Schedule2Flag = false;
        #endregion


        #region IInitiateCIMS Members
        private bool EditMode = false;
        public void Initiate(string Mode)
        {
            while (true)
            {

                if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                {


                    this.DataContext = new clsAppointmentVO()
                    {
                        UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                        DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                        DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                        AppointmentDate = DateTime.Now.Date.Date

                    };

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

                    break;
                }

                if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    string Significant = string.Empty;
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSignificant == true)
                        Significant = " ***  ";
                    this.Title = "Patient Appointment- " + Significant + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                      " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    this.DataContext = new clsAppointmentVO
                    {
                        MrNo = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo,
                        PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        FirstName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName,
                        MiddleName = ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName,
                        LastName = ((IApplicationConfiguration)App.Current).SelectedPatient.LastName,
                        DOB = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth,


                        UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID,
                        DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                        DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                        AppointmentDate = DateTime.Now.Date.Date


                    };
                    ((clsAppointmentVO)this.DataContext).MrNo = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    ((clsAppointmentVO)this.DataContext).FirstName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName;
                    ((clsAppointmentVO)this.DataContext).MiddleName = ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName;
                    ((clsAppointmentVO)this.DataContext).LastName = ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    ((clsAppointmentVO)this.DataContext).DOB = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;


                    clsGetPatientBizActionVO BizAction1 = new clsGetPatientBizActionVO();
                    BizAction1.PatientDetails = new clsPatientVO();
                    BizAction1.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    BizAction1.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;


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
                                    txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                                    txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                                    txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");
                                    txtContactNo1.Text = OBJPatient.PatientDetails.ContactNo1;
                                    txtContactNo2.Text = OBJPatient.PatientDetails.ContactNo2;
                                    txtFaxNo.Text = OBJPatient.PatientDetails.FaxNo;
                                    txtMobileCountryCode.Text = OBJPatient.PatientDetails.MobileCountryCode.ToString();

                                    txtResiCountryCode.Text = OBJPatient.PatientDetails.ResiNoCountryCode.ToString();
                                    txtResiSTD.Text = OBJPatient.PatientDetails.ResiSTDCode.ToString();
                                    txtEmail.Text = OBJPatient.PatientDetails.Email;
                                    txtFamilyName.Text = OBJPatient.PatientDetails.FamilyName;

                                }
                            }
                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
        public frmReschedulePatientAppointment()
        {
            InitializeComponent();
        }
        public frmReschedulePatientAppointment(clsAppointmentVO Item)
        {
            InitializeComponent();
            this.Item = new clsAppointmentVO();
            this.Item = Item;
            this.DataContext = Item;
        }
        private void frmReschedulePatientAppointment_Loaded(object sender, RoutedEventArgs e)
        {


            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

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
                chkValidations();
                FillGender();
                FillMaritalStatus();
                FillBloodGroup();
                FillReasonList();
                FillUnitList();
                FillSpecialRegistration();
                txtMRNoPatientInformation.Focus();

                ShowBackColor();

                Indicatior.Close();
            }
            IsPageLoded = true;
        }

        private void ShowBackColor()
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            GradientStop gradientStop1 = new GradientStop();
            gradientStop1.Offset = 0;
            gradientStop1.Color = Color.FromArgb(255, 255, 255, 211);
            brush.GradientStops.Add(gradientStop1);
            //this.cmbAppointmentType.BackGroundColor = brush;
            this.cmbDoctorAddNewAppointment.BackGroundColor = brush;
            this.cmbDepartment.BackGroundColor = brush;
            this.txtContactNo1.Background = brush;
            this.cmbGender.Background = brush;
            this.cmbGender.BackGroundColor = brush;
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
        }

        #endregion

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #region FillCombobox

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
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

        private void FillMaritalStatus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MaritalStatusMaster;
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
            BizAction.MasterTable = MasterTableNameList.M_BloodGroupMaster;
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
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillUnitList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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
                    cmbAUnit.ItemsSource = null;
                    cmbAUnit.ItemsSource = objList;
                    ShowBackColor();
                }
                if (this.DataContext != null)
                {
                    cmbAUnit.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
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
                    if (((MasterListItem)cmbAUnit.SelectedItem).ID == 0)
                    {
                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }
                    cmbDepartment.ItemsSource = null;
                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbDepartment.ItemsSource = objList;
                    if (this.DataContext != null)
                    {
                        if (UseApplicationID == true)
                        {
                            cmbDepartment.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;
                            UseApplicationID = false;
                        }
                        else
                            cmbDepartment.SelectedValue = objList[0].ID;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }
        private void FillDoctor(long IUnitId, long iDeptId)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            if ((MasterListItem)cmbAUnit.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId;
            }
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptId;
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
                    if (this.DataContext != null)
                    {
                        if (UseApplicationDoctorID == true)
                        {
                            cmbDoctorAddNewAppointment.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;
                            UseApplicationDoctorID = false;
                        }
                        else
                            cmbDoctorAddNewAppointment.SelectedValue = objList[0].ID;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmbAUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbAUnit.SelectedItem != null)
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

        private void CmdPatientAppSearch_Click(object sender, RoutedEventArgs e)
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
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Registration not found against this MRNO.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW8.Show();
                        }
                        else
                        {
                            SearchAppointmentByMrNo(txtMRNoPatientInformation.Text);
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
        }

        public void SearchAppointmentByMrNo(string MrNo)
        {
            clsGetAppointmentByMrNoBizActionVO BizAction = new clsGetAppointmentByMrNoBizActionVO();
            BizAction.AppointmentDetails = (clsAppointmentVO)this.DataContext;
            BizAction.MRNo = MrNo;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    ((clsAppointmentVO)this.DataContext).FirstName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FirstName;
                    ((clsAppointmentVO)this.DataContext).MiddleName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.MiddleName;
                    ((clsAppointmentVO)this.DataContext).LastName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.LastName;
                    ((clsAppointmentVO)this.DataContext).FamilyName = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FamilyName;
                    ((clsAppointmentVO)this.DataContext).DOB = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.DOB;
                    ((clsAppointmentVO)this.DataContext).CivilId = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.CivilId;
                    ((clsAppointmentVO)this.DataContext).ContactNo1 = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo1;
                    ((clsAppointmentVO)this.DataContext).ContactNo2 = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo2;
                    ((clsAppointmentVO)this.DataContext).FaxNo = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FaxNo;
                    ((clsAppointmentVO)this.DataContext).Email = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.Email;
                    txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                    txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                    txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");
                    txtContactNo1.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo1;
                    txtContactNo2.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ContactNo2;
                    txtFaxNo.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.FaxNo;
                    txtMobileCountryCode.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.MobileCountryCode.ToString();
                    txtResiCountryCode.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ResiNoCountryCode.ToString();
                    txtResiSTD.Text = ((clsGetAppointmentByMrNoBizActionVO)arg.Result).AppointmentDetails.ResiSTDCode.ToString();
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
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #region  Validation
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Length > 0)
            {
                if (txtEmail.Text.IsEmailValid())
                    txtEmail.ClearValidationError();
                else
                {
                    txtEmail.SetValidation("Please Enter valid Email Address.");
                    txtEmail.RaiseValidationError();
                }
            }
        }

        private void txtContactNo1_LostFocus(object sender, RoutedEventArgs e)
        {

            if (txtContactNo1.Text.Length > 0)
            {
                if (txtContactNo1.Text.IsNumberValid())
                    txtContactNo1.ClearValidationError();
                else
                {
                    txtContactNo1.SetValidation("Please Enter Number in  Correct Format");
                    txtContactNo1.RaiseValidationError();
                }
                txtContactNo1.ClearValidationError();
            }
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
            if (((clsAppointmentVO)this.DataContext).AppointmentDate < DateTime.Today)
            {
                dtpAppointmentDate.SetValidation("Appointment Date  can not be less than Today");
                dtpAppointmentDate.RaiseValidationError();
            }
            else if (((clsAppointmentVO)this.DataContext).AppointmentDate == null)
            {
                dtpAppointmentDate.SetValidation("Please select the Appointment Date");
                dtpAppointmentDate.RaiseValidationError();
            }
            else
                dtpAppointmentDate.ClearValidationError();
        }

        private void dtpDOB1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((clsAppointmentVO)this.DataContext).DOB > DateTime.Now)
            {
                dtpDOB1.SetValidation("Date of birth can not be greater than Today");
                dtpDOB1.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else if (((clsAppointmentVO)this.DataContext).DOB == null)
            {
                dtpDOB1.SetValidation("Please select the Date of birth");
                dtpDOB1.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else
            {
                dtpDOB1.ClearValidationError();
                txtYY.Text = ConvertDate(dtpDOB1.SelectedDate, "YY");
                txtMM.Text = ConvertDate(dtpDOB1.SelectedDate, "MM");
                txtDD.Text = ConvertDate(dtpDOB1.SelectedDate, "DD");
            }
            txtYY.SelectAll();
        }
        private void txtContactNo1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItNumber(txtContactNo1.Text) == false)
            {
                txtContactNo1.SetValidation("Please enter Only Number");
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
                    if (!((WaterMarkTextbox)sender).Text.IsNumberValid() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 10)
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
                txtMiddleName.SetValidation("Please enter Only character");
                txtMiddleName.RaiseValidationError();
            }
            else
                txtMiddleName.ClearValidationError();
        }

        private void txtLastName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItCharacter(txtLastName.Text) == false)
            {
                txtLastName.SetValidation("Please enter Only character");
                txtLastName.RaiseValidationError();

            }
            else
                txtLastName.ClearValidationError();
        }

        private void txtFamilyName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItCharacter(txtFamilyName.Text) == false)
            {
                txtFamilyName.SetValidation("Please enter only character");
                txtFamilyName.RaiseValidationError();
            }
            else
                txtFamilyName.ClearValidationError();
        }

        private void txtContactNo2_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItNumber(txtContactNo2.Text) == false)
            {
                txtContactNo2.SetValidation("Please enter only Number");
                txtContactNo2.RaiseValidationError();
            }
            else
                txtContactNo2.ClearValidationError();
        }

        private void txtFaxNo_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Extensions.IsItNumber(txtFaxNo.Text) == false)
            {
                txtFaxNo.SetValidation("Please enter only Number");
                txtFaxNo.RaiseValidationError();
            }
            else
                txtFaxNo.ClearValidationError();
        }

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

        private void txtYY_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            {
                int val = int.Parse(txtYY.Text.Trim());
                if (val > 0)
                {
                    dtpDOB1.SelectedDate = ConvertDateBack("YY", val, dtpDOB1.SelectedDate);
                }
            }

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            {
                int val = int.Parse(txtMM.Text.Trim());
                if (val > 0)
                {
                    dtpDOB1.SelectedDate = ConvertDateBack("MM", val, dtpDOB1.SelectedDate);
                }
            }

            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            {
                int val = int.Parse(txtDD.Text.Trim());
                if (val > 0)
                    dtpDOB1.SelectedDate = ConvertDateBack("DD", val, dtpDOB1.SelectedDate);
            }
            txtMM.SelectAll();
        }

        private void txtMM_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            {
                int val = int.Parse(txtYY.Text.Trim());
                if (val > 0)
                {
                    dtpDOB1.SelectedDate = ConvertDateBack("YY", val, dtpDOB1.SelectedDate);
                }
            }

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            {
                int val = int.Parse(txtMM.Text.Trim());
                if (val > 0)
                {
                    dtpDOB1.SelectedDate = ConvertDateBack("MM", val, dtpDOB1.SelectedDate);

                }
            }

            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            {
                int val = int.Parse(txtDD.Text.Trim());
                if (val > 0)
                    dtpDOB1.SelectedDate = ConvertDateBack("DD", val, dtpDOB1.SelectedDate);
            }

            txtDD.SelectAll();
        }

        private void txtDD_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            {
                int val = int.Parse(txtYY.Text.Trim());
                if (val > 0)
                {
                    dtpDOB1.SelectedDate = ConvertDateBack("YY", val, dtpDOB1.SelectedDate);

                }
            }

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            {
                int val = int.Parse(txtMM.Text.Trim());
                if (val > 0)
                {
                    dtpDOB1.SelectedDate = ConvertDateBack("MM", val, dtpDOB1.SelectedDate);

                }
            }

            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            {
                int val = int.Parse(txtDD.Text.Trim());
                if (val > 0)
                    dtpDOB1.SelectedDate = ConvertDateBack("DD", val, dtpDOB1.SelectedDate);
            }

        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPositiveNumber())
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
                result = false;
                ClickedFlag = 0;
            }
            else
                txtFirstName.ClearValidationError();

            if (txtLastName.Text == "")
            {
                txtLastName.SetValidation("Please Enter Last Name");
                txtLastName.RaiseValidationError();
                if (result == true)
                    txtLastName.Focus();
                result = false;
                ClickedFlag = 0;
            }
            else
                txtLastName.ClearValidationError();

            if (txtContactNo1.Text == "")
            {
                txtContactNo1.SetValidation("Please Enter Mobile No.");
                txtContactNo1.RaiseValidationError();
                if (result == true)
                    txtContactNo1.Focus();
                result = false;
                ClickedFlag = 0;
            }
            else
                txtContactNo1.ClearValidationError();

            if (txtEmail.Text.Trim() != "")
            {
                if (txtEmail.Text.Length > 0)
                {
                    if (txtEmail.Text.IsEmailValid())
                        txtEmail.ClearValidationError();
                    else
                    {
                        txtEmail.SetValidation("Please Enter valid Email Address");
                        txtEmail.RaiseValidationError();
                        txtEmail.Focus();
                        result = false;
                        ClickedFlag = 0;
                    }

                }
            }
            if (((clsAppointmentVO)this.DataContext).AppointmentDate == null)
            {
                dtpAppointmentDate.SetValidation("Please select the Appointment Date");
                dtpAppointmentDate.RaiseValidationError();
                if (result == true)
                    dtpAppointmentDate.Focus();
                result = false;
                ClickedFlag = 0;
            }
            else if (((clsAppointmentVO)this.DataContext).AppointmentDate < DateTime.Today)
            {

                dtpAppointmentDate.SetValidation("Please select current date or greater than current date");
                dtpAppointmentDate.RaiseValidationError();
                if (result == true)
                    dtpAppointmentDate.Focus();
                result = false;
                ClickedFlag = 0;
            }
            else
                dtpAppointmentDate.ClearValidationError();

            if (IsPageLoded)
            {
                if ((MasterListItem)cmbGender.SelectedItem == null)
                {
                    cmbGender.TextBox.SetValidation("Gender is required");
                    cmbGender.TextBox.RaiseValidationError();
                    if (result == true)
                        cmbGender.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else if (((MasterListItem)cmbGender.SelectedItem).ID == 0)
                {
                    cmbGender.TextBox.SetValidation("Gender is required");
                    cmbGender.TextBox.RaiseValidationError();
                    if (result == true)
                        cmbGender.Focus();
                    result = false;
                    ClickedFlag = 0;

                }
                else
                    cmbGender.TextBox.ClearValidationError();

                if (tpFromTime.Value != null)
                {
                    if (dtpAppointmentDate.SelectedDate != null)
                    {
                        string time = tpFromTime.Value.Value.ToShortTimeString();
                        String mm = time.Substring(time.IndexOf(":") + 1, time.IndexOf(" ") - 2);
                        string hh = time.Substring(0, time.IndexOf(":"));
                        for (int k = 0; k < time.Length; k++)
                        {
                            if (time[k].Equals('P'))
                            {
                                if (Convert.ToInt32(hh) < 12)
                                {
                                    hh = (Convert.ToInt32(hh) + 12).ToString();
                                }
                            }
                        }
                        DateTime AppTime = new DateTime(dtpAppointmentDate.SelectedDate.Value.Year, dtpAppointmentDate.SelectedDate.Value.Month, dtpAppointmentDate.SelectedDate.Value.Day, Convert.ToInt16(hh), Convert.ToInt16(mm), 0);
                        if (DateTime.Now > AppTime)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW11 =
                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Time slot is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW11.Show();
                            tpFromTime.Focus();
                            ClickedFlag = 0;
                            result = false;
                            return result;
                        }
                    }
                }

                //if (((MasterListItem)(cmbAppointmentType.SelectedItem)).ID == 0)
                //{

                //    cmbAppointmentType.TextBox.SetValidation("Please Select Appointment Reason");
                //    cmbAppointmentType.TextBox.RaiseValidationError();
                //    if (result == true)
                //        cmbAppointmentType.Focus();
                //    result = false;
                //    ClickedFlag = 0;

                //}
                //else
                //    cmbAppointmentType.TextBox.ClearValidationError();

                if ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem == null)
                {
                    cmbDoctorAddNewAppointment.TextBox.SetValidation("Please select doctor");
                    cmbDoctorAddNewAppointment.TextBox.RaiseValidationError();
                    if (result == true)
                        cmbDoctorAddNewAppointment.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else if (((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID == 0)
                {
                    cmbDoctorAddNewAppointment.TextBox.SetValidation("Please select doctor");
                    cmbDoctorAddNewAppointment.TextBox.RaiseValidationError();
                    if (result == true)
                        cmbDoctorAddNewAppointment.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    cmbDoctorAddNewAppointment.TextBox.ClearValidationError();

                if ((MasterListItem)cmbDepartment.SelectedItem == null)
                {
                    cmbDepartment.TextBox.SetValidation("Please select department");
                    cmbDepartment.TextBox.RaiseValidationError();
                    if (result == true)
                        cmbDepartment.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else if (((MasterListItem)cmbDepartment.SelectedItem).ID == 0)
                {
                    cmbDepartment.TextBox.SetValidation("Please select department");
                    cmbDepartment.TextBox.RaiseValidationError();
                    if (result == true)
                        cmbDepartment.Focus();
                    result = false;
                    ClickedFlag = 0;

                }
                else
                    cmbDepartment.TextBox.ClearValidationError();

                if ((MasterListItem)cmbAUnit.SelectedItem == null)
                {
                    cmbAUnit.TextBox.SetValidation("Please select unit");
                    cmbAUnit.TextBox.RaiseValidationError();
                    if (result == true)
                        cmbAUnit.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else if (((MasterListItem)cmbAUnit.SelectedItem).ID == 0)
                {
                    cmbAUnit.TextBox.SetValidation("Please select unit");
                    cmbAUnit.TextBox.RaiseValidationError();
                    if (result == true)
                        cmbAUnit.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    cmbAUnit.TextBox.ClearValidationError();

                if (((clsAppointmentVO)this.DataContext).AppointmentDate < DateTime.Today)
                {

                    dtpAppointmentDate.SetValidation("Appointment Date  can not be less than Today");
                    dtpAppointmentDate.RaiseValidationError();
                    result = false;
                    ClickedFlag = 0;
                }
                else if (dtpAppointmentDate.SelectedDate == null)
                {
                    dtpAppointmentDate.SetValidation("Please select the Appointment Date");
                    dtpAppointmentDate.RaiseValidationError();
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    dtpAppointmentDate.ClearValidationError();


                if (txtEmail.Text.Trim() != "")
                {
                    if (txtEmail.Text.Length > 0)
                    {
                        if (txtEmail.Text.IsEmailValid())
                            txtEmail.ClearValidationError();
                        else
                        {
                            txtEmail.SetValidation("Please Enter valid Email Address");
                            txtEmail.RaiseValidationError();
                            txtEmail.Focus();
                            result = false;
                            ClickedFlag = 0;
                        }

                    }
                }

                if (((clsAppointmentVO)this.DataContext).DOB > DateTime.Now)
                {

                    dtpDOB1.SetValidation("Date of birth can not be greater than Today");
                    dtpDOB1.RaiseValidationError();
                    dtpDOB1.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    dtpDOB1.ClearValidationError();

                if (txtYY.Text != "")
                {
                    if (Convert.ToInt16(txtYY.Text) > 120)
                    {
                        txtYY.SetValidation("Age can not be greater than 121");
                        txtYY.RaiseValidationError();
                        txtYY.Focus();
                        result = false;
                        ClickedFlag = 0;
                    }
                    else
                        txtYY.ClearValidationError();
                }
                if (txtLastName.Text.Trim() == "")
                {
                    txtLastName.SetValidation("Please Enter Last Name");
                    txtLastName.RaiseValidationError();
                    txtLastName.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtLastName.ClearValidationError();

                if (txtFirstName.Text.Trim() == "")
                {
                    txtFirstName.SetValidation("Please Enter First Name");
                    txtFirstName.RaiseValidationError();
                    txtFirstName.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtFirstName.ClearValidationError();


                if (dgTimeSlots.ItemsSource == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW12 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select doctor schedule.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW12.Show();
                    ClickedFlag = 0;
                    result = false;
                    return result;

                }

                if (tpFromTime.Value == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW11 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter From Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter To Time.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
        }

        private void FetchPatientPastVisit(string MrNo)
        {
            clsGetPatientPastVisitBizActionVO BizAction = new clsGetPatientPastVisitBizActionVO();
            BizAction.MRNO = MrNo;
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
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        #region Save data
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
                        string msgTitle = "";
                        string msgText = "Are you sure you want to save the Appointment?";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                        msgW.Show();
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
                BizAction.AppointmentDetails = (clsAppointmentVO)this.DataContext;
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
                BizAction.AppointmentDetails.ContactNo1 = txtContactNo1.Text.Trim();
                BizAction.AppointmentDetails.ContactNo2 = txtContactNo2.Text.Trim();
                if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                    BizAction.AppointmentDetails.MobileCountryCode =txtMobileCountryCode.Text.Trim();
                if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                    BizAction.AppointmentDetails.ResiNoCountryCode = long.Parse(txtResiCountryCode.Text.Trim());
                if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                    BizAction.AppointmentDetails.ResiSTDCode = long.Parse(txtResiSTD.Text.Trim());
                BizAction.AppointmentDetails.AppointmentDate = dtpAppointmentDate.SelectedDate.Value;
                BizAction.AppointmentDetails.FromTime = Convert.ToDateTime(tpFromTime.Value);
                BizAction.AppointmentDetails.ToTime = Convert.ToDateTime(tpToTime.Value);
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
                                //EmailServiceClient EmailClient = new EmailServiceClient();
                                //EmailClient.SendEmailCompleted += (sa, arg1) =>
                                //{
                                //    if (arg1.Error == null)
                                //    {

                                //    }
                                //    else
                                //    {
                                //        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Email sending fail", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                //        msgW2.Show();
                                //    }
                                //};
                                //string Doctor = "Dr." + ((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).Description;
                                //string Date = dtpAppointmentDate.SelectedDate.Value.ToShortDateString();
                                //String FT = Convert.ToDateTime(tpFromTime.Value).ToShortTimeString(); ;
                                //TimeSpan TT = Convert.ToDateTime(tpFromTime.Value).TimeOfDay;
                                //string Subject = "Your appointment has been confirmed with " + Doctor + " On " + Date + " at " + FT;
                                //EmailClient.SendEmailAsync(Email, txtEmail.Text, " Appointment", Subject, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, null);
                                //EmailClient.CloseAsync();
                            }
                        }
                        Indicatior.Show();
                        this.DialogResult = true;
                        if (OnSaveButton_Click != null)
                            OnSaveButton_Click(this, new RoutedEventArgs());
                        ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        UIElement MyData = new AppointmentList();
                        ((IApplicationConfiguration)App.Current).OpenMainContent(MyData);
                        Indicatior.Close();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding Appointment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
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
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Time slots is Already Booked.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                                ClickedFlag = 0;
                            }
                            else
                            {
                                CheckDoctorAvailableOrNot();
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding Appointment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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

        private void CheckDoctorAvailableOrNot()
        {
            if (tpFromTime.Value != null)
            {
                if (Schedule1Flag == true && Schedule2Flag == true)
                {
                    if (tpFromTime.Value.Value.TimeOfDay < DocSchedule1FromTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW20.Show();
                        ClickedFlag = 0;
                    }
                    else if (tpFromTime.Value.Value.TimeOfDay >= DocSchedule1ToTime.Value.TimeOfDay && tpFromTime.Value.Value.TimeOfDay < DocSchedule2FromTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW21 =
                                                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW21.Show();
                        ClickedFlag = 0;
                    }
                    else if (tpFromTime.Value.Value.TimeOfDay >= DocSchedule2ToTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW22 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
                    if (tpFromTime.Value.Value.TimeOfDay < DocSchedule1FromTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW20.Show();
                        ClickedFlag = 0;
                    }
                    else if (tpFromTime.Value.Value.TimeOfDay >= DocSchedule1ToTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW22 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

                    if (tpFromTime.Value.Value.TimeOfDay < DocSchedule2FromTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW20 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW20.Show();
                        ClickedFlag = 0;
                    }
                    else if (tpFromTime.Value.Value.TimeOfDay >= DocSchedule2ToTime.Value.TimeOfDay)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW22 =
                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            Indicatior.Show();
            this.DialogResult = false;
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            UIElement MyData = new AppointmentList();
            ((IApplicationConfiguration)App.Current).OpenMainContent(MyData);
            Indicatior.Close();
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
                    cmbSpRegistration.SelectedItem = objList[0];


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
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
        void FillAppointmentSloat(long logDoctorID)
        {
            clsGetDoctorAppointmentSlotVO BizAction = new clsGetDoctorAppointmentSlotVO();
            BizAction.DoctorID = logDoctorID;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    if (((clsGetDoctorAppointmentSlotVO)e.Result).SuccessStatus == 1)
                    {
                        if (((clsGetDoctorAppointmentSlotVO)e.Result).AppointmentSlot > 0)
                            AppointmentTimeSlots = Convert.ToDouble(((clsGetDoctorAppointmentSlotVO)e.Result).AppointmentSlot);
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void cmbDoctorAddNewAppointment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTimeSlots.ItemsSource = null;
            tpFromTime.Value = null;
            tpToTime.Value = null;
            if (((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID > 0)
                FillAppointmentSloat(Convert.ToInt64(((MasterListItem)cmbDoctorAddNewAppointment.SelectedItem).ID));
        }

        private void dtpAppointmentDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTimeSlots.ItemsSource = null;
        }
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
                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Time slot is already booked.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW10.Show();
                    }
                    else if (objSchedule.apColor.Color == Colors.Gray)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW11 =
                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Time slot is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW11.Show();
                    }
                    else
                    {
                        tpFromTime.Value = Convert.ToDateTime(objSchedule.ScheduleFromTime);
                        tpToTime.Value = Convert.ToDateTime(objSchedule.ScheduleToTime);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region  Reset All Controls
        private void ClearControl()
        {
            tpFromTime.Value = null;
            tpToTime.Value = null;
            cmbBloodGroup.SelectedValue = ((clsAppointmentVO)this.DataContext).BloodId; ;
            cmbMaritalStatus.SelectedValue = ((clsAppointmentVO)this.DataContext).MaritalStatusId;
            cmbGender.SelectedValue = ((clsAppointmentVO)this.DataContext).GenderId;
            cmbAppointmentType.SelectedValue = ((clsAppointmentVO)this.DataContext).AppointmentReasonId;
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
        }
        #endregion

        #region Calculate age
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
                        break;
                    case "DD":
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
                    DateTime age = DateTime.MinValue + difference;
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
                            result = (age.Day - 1).ToString();
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
                                                            apColor = new SolidColorBrush(Colors.Green),
                                                            foreColor = new SolidColorBrush(Colors.White)
                                                        });
                                                        Dt = Dt.Value.AddMinutes(AppointmentTimeSlots);
                                                    }
                                                    for (int Schedulecount = 0; Schedulecount < Schedule2Hrs; Schedulecount++)
                                                    {

                                                        listSchedule.Add(new clsSchedule()
                                                        {
                                                            Day = "Schedule2",
                                                            ScheduleFromTime = Dt.Value,
                                                            ScheduleToTime = (Dt.Value.AddMinutes(AppointmentTimeSlots)),
                                                            apColor = new SolidColorBrush(Colors.Green),
                                                            foreColor = new SolidColorBrush(Colors.White)
                                                        });
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
                                                                foreach (var item4 in listSchedule)
                                                                {
                                                                    int hh = item4.ScheduleFromTime.Hour;
                                                                    int mm = item4.ScheduleFromTime.Minute;
                                                                    DateTime AppTime = new DateTime(dtpAppointmentDate.SelectedDate.Value.Year, dtpAppointmentDate.SelectedDate.Value.Month, dtpAppointmentDate.SelectedDate.Value.Day, hh, mm, 0);
                                                                    if (DateTime.Now > AppTime)
                                                                    {
                                                                        item4.apColor = new SolidColorBrush(Colors.Gray);
                                                                        item4.foreColor = new SolidColorBrush(Colors.White);
                                                                    }

                                                                }
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
                                                                                listSchedule[ListCount].PatientName = a.PatientName2;
                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                if (this.Item.PatientId == a.PatientId && this.Item.ID == a.AppointmentID) // || this.Item.PatientName.ToString().Replace(' ',' ') == a.PatientName2.ToString().Replace(' ',' '))
                                                                                {
                                                                                    listSchedule[ListCount].apColor = new SolidColorBrush(Colors.Yellow);
                                                                                    listSchedule[ListCount].foreColor = new SolidColorBrush(Colors.Black);
                                                                                }
                                                                                else
                                                                                {
                                                                                    listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                    listSchedule[ListCount].foreColor = new SolidColorBrush(Colors.White);
                                                                                }
                                                                            }
                                                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay >= a.FromTime.Value.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay <= a.ToTime.Value.TimeOfDay)
                                                                            {
                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;
                                                                                listSchedule[ListCount].PatientName = a.PatientName2;
                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                if (this.Item.PatientId == a.PatientId && this.Item.ID == a.AppointmentID)
                                                                                {
                                                                                    listSchedule[ListCount].apColor = new SolidColorBrush(Colors.Yellow);
                                                                                    listSchedule[ListCount].foreColor = new SolidColorBrush(Colors.Black);
                                                                                }
                                                                                else
                                                                                {
                                                                                    listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                    listSchedule[ListCount].foreColor = new SolidColorBrush(Colors.White);
                                                                                }
                                                                            }
                                                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.FromTime.Value.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay > a.FromTime.Value.TimeOfDay)
                                                                            {
                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;
                                                                                listSchedule[ListCount].PatientName = a.PatientName2;
                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                if (this.Item.PatientId == a.PatientId && this.Item.ID == a.AppointmentID)
                                                                                {
                                                                                    listSchedule[ListCount].apColor = new SolidColorBrush(Colors.Yellow);
                                                                                    listSchedule[ListCount].foreColor = new SolidColorBrush(Colors.Black);
                                                                                }
                                                                                else
                                                                                {
                                                                                    listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                    listSchedule[ListCount].foreColor = new SolidColorBrush(Colors.White);
                                                                                }
                                                                            }
                                                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay > a.FromTime.Value.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.ToTime.Value.TimeOfDay)
                                                                            {
                                                                                Color C = new Color();
                                                                                C.R = 198;
                                                                                C.B = 24;
                                                                                C.G = 15;
                                                                                C.A = 255;
                                                                                listSchedule[ListCount].PatientName = a.PatientName2;
                                                                                listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                listSchedule[ListCount].foreColor = new SolidColorBrush(Colors.White);
                                                                                if (this.Item.PatientId == a.PatientId && this.Item.AppointmentID == a.AppointmentID)
                                                                                {
                                                                                    listSchedule[ListCount].apColor = new SolidColorBrush(Colors.Yellow);
                                                                                    listSchedule[ListCount].foreColor = new SolidColorBrush(Colors.Black);
                                                                                }
                                                                                else
                                                                                {
                                                                                    listSchedule[ListCount].apColor = new SolidColorBrush(C);
                                                                                    listSchedule[ListCount].foreColor = new SolidColorBrush(Colors.White);
                                                                                }
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
                                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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
                                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW6.Show();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW6 =
                                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Schedule is not available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW6.Show();


                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW10 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Doctor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW10.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW12 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Appointment date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

        private void cmdSave_Checked(object sender, RoutedEventArgs e)
        {
            Console.Write("OK");
        }

      
    }
}
