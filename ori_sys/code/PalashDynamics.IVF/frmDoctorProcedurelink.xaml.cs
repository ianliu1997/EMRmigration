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
using PalashDynamics.Pharmacy;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS.Forms;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.UserControls;
using PalashDynamics;
using System.Windows.Browser;
using System.Text;
using PalashDynamics.Pharmacy.ItemSearch;
using PalashDynamics.IVF.PatientList;
using OPDModule;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using PalashDynamics.Collections;
//using OPDModule.Forms;

namespace PalashDynamics.IVF
{
    public partial class frmDoctorProcedurelink : UserControl
    {
        public long VisitId { get; set; }
        public long PatientId { get; set; }
        public bool IsFreez { get; set; }
  
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
        public PagedSortableCollectionView<clsDoctorProcedureLinkVO> DataList { get; set; }
        WaitIndicator Indicatior = null;

        public long PatientUnitId { get; set; }
        int ClickedFlag = 0;

        public frmDoctorProcedurelink()
        {
            InitializeComponent();
        }

        private void frmDoctorPatient_Loaded(object sender, RoutedEventArgs e)
        {
            CmdAdd.Content = "ADD";
            ProcDate.SelectedDate = DateTime.Now.Date;
            ProcTime.Value = DateTime.Now;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {                   
                    CmdSave.IsEnabled = false;
                } 
                DataList = new PagedSortableCollectionView<clsDoctorProcedureLinkVO>();
            }

            InitialiseForm();
            ChkValidation();          
            FillGender();
            FillProcedureperformed();
            FillUnitList();
            FillNurse();
            cmbdoctor.IsEnabled = true;

            DataList = new PagedSortableCollectionView<clsDoctorProcedureLinkVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dataGridPager.PageSize = DataListPageSize;
            dataGridPager.Source = DataList;

            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                txtMRNo.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                VisitId = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                GetPatient();
                //SetUpPage();
            }
            txtFirstName.Focus();
          
           // SetUpPage();

        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetUpPage();
        }


        private void InitialiseForm()
        {
            //PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
            //dgPharmacyItems.ItemsSource = PharmacyItems;
            //dgPharmacyItems.UpdateLayout();

         
        }

        private bool ChkValidation()
        {
            bool result = true;
        
            if (txtLastName.Text == "")
            {
                txtLastName.SetValidation("Please Enter Surname");
                txtLastName.RaiseValidationError();
                result = false;
                txtLastName.Focus();
            }
            else
                txtLastName.ClearValidationError();


            if (txtFirstName.Text == "")
            {
                txtFirstName.SetValidation("Please Enter First Name");
                txtFirstName.RaiseValidationError();
                result = false;
                txtFirstName.Focus();
            }
            else
                txtFirstName.ClearValidationError();

            if (txtYY.Text != "")
            {
                if (Convert.ToInt16(txtYY.Text) > 120)
                {
                    txtYY.SetValidation("Age Can Not Be Greater Than 121");
                    txtYY.RaiseValidationError();
                    txtYY.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtYY.ClearValidationError();
            }
            return result;
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
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetPatient()
        {
            clsGetPatientBizActionVO BizAction1 = new clsGetPatientBizActionVO();
            BizAction1.PatientDetails = new clsPatientVO();
            BizAction1.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction1.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

            BizAction1.PatientDetails.GeneralDetails.LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsGetPatientBizActionVO Patient = new clsGetPatientBizActionVO();
                        Patient = (clsGetPatientBizActionVO)arg.Result;

                        txtFirstName.Text = Patient.PatientDetails.GeneralDetails.FirstName;
                        if (Patient.PatientDetails.GeneralDetails.MiddleName != null)
                        {
                            txtMiddleName.Text = Patient.PatientDetails.GeneralDetails.MiddleName;
                        }
                        txtLastName.Text = Patient.PatientDetails.GeneralDetails.LastName;
                        dtpDOB.SelectedDate = Patient.PatientDetails.GeneralDetails.DateOfBirth;

                        txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                        txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                        txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                        cmbGender.SelectedValue = Patient.PatientDetails.GenderID;
                        txtMobileCountryCode.Text = Patient.PatientDetails.MobileCountryCode.ToString();
                        txtContactNo.Text = Patient.PatientDetails.ContactNo1.ToString();
                        SetUpPage();
                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("", "Error occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();
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
                    cmbAUnit.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                }             
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }
        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;

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
                    cmbSpecilization.ItemsSource = null;
                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbSpecilization.ItemsSource = objList;

                    cmbSpecilization.SelectedValue = (long)0;           
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
            if ((MasterListItem)cmbSpecilization.SelectedItem != null)
            {
                BizAction.SpecializationID = iDeptId;
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

                    cmbdoctor.ItemsSource = null;

                    if (((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbdoctor.ItemsSource = objList;

                    cmbdoctor.SelectedValue = (long)0;     
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillProcedureperformed()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ProcedurePerformed;
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
                    cmbProcedure.ItemsSource = null;
                    cmbProcedure.ItemsSource = objList;
                    cmbProcedure.SelectedValue = (long)0;
                }
                cmbProcedure.SelectedValue = (long)0;              
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillNurse()
        {
               clsGetStaffMasterDetailsBizActionVO BizAction = new clsGetStaffMasterDetailsBizActionVO();
                BizAction.ID = 0;
                BizAction.DesignationID = 4;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsGetStaffMasterDetailsBizActionVO objGetList = e.Result as clsGetStaffMasterDetailsBizActionVO;
                        autoUserName.ItemsSource = null;
                        autoUserName.ItemsSource = objGetList.StaffMasterList;
                        autoUserName.ItemFilter = (search, item) =>
                        {
                            clsStaffMasterVO fam = item as clsStaffMasterVO;
                            if (fam != null)
                            {
                                search = search.ToUpper();
                                return (fam.Value.ToUpper().StartsWith(search));
                            }
                            else
                            {
                                return false;
                            }
                        };
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();    
        }
    
        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.Show();
        }

        #region Calculate age from birthdate
        /// <summary>
        /// Purpose:Get Age from birthdate.
        /// </summary>
        /// <param name="Datevalue"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
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

        #endregion

        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    clsGetPatientDetailsForCounterSaleBizActionVO BizAction = new clsGetPatientDetailsForCounterSaleBizActionVO();
                    BizAction.PatientDetails = new clsPatientVO();
                    BizAction.MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    VisitId = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                clsGetPatientDetailsForCounterSaleBizActionVO ObjPatient = new clsGetPatientDetailsForCounterSaleBizActionVO();
                                ObjPatient = (clsGetPatientDetailsForCounterSaleBizActionVO)arg.Result;
                                PatientId = ObjPatient.PatientDetails.GeneralDetails.PatientID;

                                txtMRNo.Text = ObjPatient.PatientDetails.GeneralDetails.MRNo;
                                txtFirstName.Text = ObjPatient.PatientDetails.GeneralDetails.FirstName;
                                if (ObjPatient.PatientDetails.GeneralDetails.MiddleName != null)
                                {
                                    txtMiddleName.Text = ObjPatient.PatientDetails.GeneralDetails.MiddleName;
                                }
                                txtLastName.Text = ObjPatient.PatientDetails.GeneralDetails.LastName;
                                dtpDOB.SelectedDate = ObjPatient.PatientDetails.GeneralDetails.DateOfBirth;

                                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                                cmbGender.SelectedValue = ObjPatient.PatientDetails.GenderID;
                                txtMobileCountryCode.Text = ObjPatient.PatientDetails.MobileCountryCode.ToString();
                                txtContactNo.Text = ObjPatient.PatientDetails.ContactNo1.ToString();
                              
                                CheckVisit();
                                SetUpPage();
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured While Adding Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                        Indicatior.Close();
                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Indicatior.Close();
        }

        #region Get Visit details
        /// <summary>
        /// Purpose:Check patient visit.
        /// </summary>
        private void CheckVisit()
        {
            WaitIndicator ind = new WaitIndicator();
            ind.Show();

            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
            BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.Details.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.GetLatestVisit = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).Details != null && ((clsGetVisitBizActionVO)arg.Result).Details.ID > 0 && ((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
                    {
                        ind.Close();
                    }
                    else
                    {
                        ind.Close();
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //       new MessageBoxControl.MessageBoxChildWindow("", "Visit is not marked for the Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();
                        return;
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetVisitDetails()
        {
            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();

            BizAction.Details.ID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).Details != null)
                    {
                        //CheckVisitType(((clsGetVisitBizActionVO)arg.Result).Details.VisitTypeID);
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
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

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void WaterMarkTextbox_OnTextChanged(object sender, RoutedEventArgs e)
        {

        }

        private void txtContactNo1_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtYY_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            {
                int val = int.Parse(txtYY.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);
                }
            }

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            {
                int val = int.Parse(txtMM.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);
                }
            }

            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            {
                int val = int.Parse(txtDD.Text.Trim());
                if (val > 0)
                    dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
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
                    dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);
                }
            }

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            {
                int val = int.Parse(txtMM.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);
                }
            }

            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            {
                int val = int.Parse(txtDD.Text.Trim());
                if (val > 0)
                    dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
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
                    dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);
                }
            }

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            {
                int val = int.Parse(txtMM.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);
                }
            }

            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            {
                int val = int.Parse(txtDD.Text.Trim());
                if (val > 0)
                    dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
            }
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
            }
            else if (!((TextBox)sender).Text.IsNumberValid())
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

        private void dtpDOB_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            //((IApplicationConfiguration)App.Current).FillMenu("Billing");
        }

        private bool Checkvalidition()
        {
            bool result = true;
            if (txtbillno.Text == "")
            {
                txtbillno.SetValidation("Please select Bill.");
                txtbillno.RaiseValidationError();
                result = false;
                txtbillno.Focus();
            }
            else if ((MasterListItem)cmbdoctor.SelectedItem == null || ((MasterListItem)cmbdoctor.SelectedItem).ID == 0)
            {
                txtbillno.ClearValidationError();
                cmbdoctor.TextBox.SetValidation("Please Select Doctor");
                cmbdoctor.TextBox.RaiseValidationError();
                cmbdoctor.Focus();
                result = false;
            }
            else if ((MasterListItem)cmbSpecilization.SelectedItem == null || ((MasterListItem)cmbSpecilization.SelectedItem).ID == 0)
            {
                txtbillno.ClearValidationError();
                cmbdoctor.TextBox.ClearValidationError();
                cmbSpecilization.TextBox.SetValidation("Please Select Specialization");
                cmbSpecilization.TextBox.RaiseValidationError();
                cmbSpecilization.Focus();
                result = false;
            }
            else if ((MasterListItem)cmbProcedure.SelectedItem == null || ((MasterListItem)cmbProcedure.SelectedItem).ID == 0)
            {
                txtbillno.ClearValidationError();
                cmbdoctor.TextBox.ClearValidationError();
                cmbSpecilization.TextBox.ClearValidationError();
                cmbProcedure.TextBox.SetValidation("Please Select Procedure");
                cmbProcedure.TextBox.RaiseValidationError();
                cmbProcedure.Focus();
                result = false;
            }
            else
                result = true;

                
            return result;
 
        }

        private void CmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                if (Checkvalidition())
                {

                    string msgTitle = "Palash";
                    string msgText = "Are You Sure \n  You Want To Save the Details?";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin.OnMessageBoxClosed += (result) =>
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            Save();
                        }
                    };
                    msgWin.Show();

                }
            
            }
            else
            {
                msgText = "Please Select The Patient .";
                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgWindow.Show(); 
            }
        }

        private void cmdDeletePharmacyItems_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtBillnoCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                long patientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                long PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                frmBillListPatientWise serviceSearch = null;
                serviceSearch = new frmBillListPatientWise(patientID, PatientUnitID);
                serviceSearch.Show();
                serviceSearch.OnSaveButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
            }
            else
            {
                msgText = "Please Select The Patient";

                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgWindow.Show();
            }
        }

        string billno;
        long billID = 0;
        long billUnitID = 0;

        private void ClearControl() 
        {
            cmbSpecilization.SelectedValue = (long)0;
            cmbProcedure.SelectedValue = (long)0;
            cmbdoctor.SelectedValue = (long)0;
            autoUserName.SelectedItem =null;
           // txtbillno.Text = " ";
            ProcDate.SelectedDate = DateTime.Now; ;
            ProcTime.Value =DateTime.Now;
          //  BillDate.SelectedDate = null;
        }
        
        void serviceSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            billID = ((OPDModule.frmBillListPatientWise)(sender)).BillID;
            billUnitID = ((OPDModule.frmBillListPatientWise)(sender)).BillUnitID;
            DateTime billDate = ((OPDModule.frmBillListPatientWise)(sender)).BillDate;
            billno = ((OPDModule.frmBillListPatientWise)(sender)).BillNo;

            txtbillno.Text = Convert.ToString( billno);
            BillDate.SelectedDate = billDate;
        }
       
        private void SetUpPage() 
        {
            clsGetDoctorProcedureLinkBizActionVO BizAction = new clsGetDoctorProcedureLinkBizActionVO();
            BizAction.LinkDetails = new clsDoctorProcedureLinkVO();
            BizAction.LinkDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.LinkDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = 0;
            BizAction.MaximumRows = 15;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {               
                    if (((clsGetDoctorProcedureLinkBizActionVO)args.Result).LinkDetailsList!= null)
                    {
                        clsGetDoctorProcedureLinkBizActionVO result = new clsGetDoctorProcedureLinkBizActionVO();
                         result = args.Result as clsGetDoctorProcedureLinkBizActionVO;
                                                 
                         DataList.TotalItemCount = result.TotalRows;
                         DataList.Clear();
                         if (result.LinkDetailsList != null)
                         {
                             foreach (var item in result.LinkDetailsList)
                             {
                                 DataList.Add(item);
                             }
                             dgProcedureDoctor.ItemsSource = null;
                             dgProcedureDoctor.ItemsSource = DataList;
                             dgProcedureDoctor.SelectedIndex = -1;
                             dataGridPager.Source = null;
                             dataGridPager.PageSize = BizAction.MaximumRows;
                             dataGridPager.Source = DataList;
                            
                         }                       
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("PALASH", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void Save()
        {
            clsAddDoctorProcedureLinkBizActionVO bizAction = new clsAddDoctorProcedureLinkBizActionVO();

            bizAction.LinkDetails = new clsDoctorProcedureLinkVO();
            if (IsModify == true)
                bizAction.LinkDetails.ID = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).ID;

            bizAction.LinkDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            bizAction.LinkDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            bizAction.LinkDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

            bizAction.LinkDetails.BillID = billID;
            bizAction.LinkDetails.BillUnitID = billUnitID;

            bizAction.LinkDetails.Date = ProcDate.SelectedDate;
            bizAction.LinkDetails.Time = ProcTime.Value;
            bizAction.LinkDetails.BillDate = BillDate.SelectedDate;
            bizAction.LinkDetails.BillNo = txtbillno.Text;
            if (cmbProcedure.SelectedItem != null)
            bizAction.LinkDetails.ProcedureID = ((MasterListItem)cmbProcedure.SelectedItem).ID;
            if (cmbdoctor.SelectedItem != null)
            bizAction.LinkDetails.DoctorID = ((MasterListItem)cmbdoctor.SelectedItem).ID;
            if (autoUserName.SelectedItem != null && ((clsStaffMasterVO)autoUserName.SelectedItem).ID!=0)
                bizAction.LinkDetails.NurseID = ((clsStaffMasterVO)autoUserName.SelectedItem).ID;

            if (cmbSpecilization.SelectedItem != null)
                bizAction.LinkDetails.SpecilazationID = ((MasterListItem)cmbSpecilization.SelectedItem).ID;
            bizAction.LinkDetails.Status = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsAddDoctorProcedureLinkBizActionVO)args.Result).SuccessStatus == 1)
                    {
                        IsModify = false;
                        ClearControl();
                        string msgTitle = "Palash";
                        string msgText = "Details Added Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                SetUpPage();
                            }
                        };
                        msgWin.Show();
                    }
                    else if (((clsAddDoctorProcedureLinkBizActionVO)args.Result).SuccessStatus == 2)
                    {
                        IsModify = false;
                        ClearControl();
                        string msgTitle = "Palash";
                        string msgText = "Details Modified Successfully.";
                        CmdAdd.Content = "ADD";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                SetUpPage();
                            }
                        };
                        msgWin.Show();
                    }
        
                  
                }
            };
            client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
             }
     

        private void cmbAUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbAUnit.SelectedItem != null)
            {
                FillDepartmentList(((MasterListItem)cmbAUnit.SelectedItem).ID);
            }
        }

        string msgTitle = " PALASH ";
        string msgText = "";
        private void autoUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            clsStaffMasterVO BizAction = new clsStaffMasterVO();
            BizAction = (clsStaffMasterVO)autoUserName.SelectedItem;
            if (BizAction != null)
            {
                if (BizAction.ID == 0)
                {
                    autoUserName.SetValidation("Employee Does not Exists");
                    autoUserName.RaiseValidationError();
                    autoUserName.Text = "";
                    autoUserName.Focus();
                }
                else
                {
                    autoUserName.ClearValidationError();
               
                }
             
            }
            else if (BizAction == null && !string.IsNullOrEmpty(autoUserName.Text))
            {
                msgText = "User Name Does not Exist.";

                MessageBoxControl.MessageBoxChildWindow msgWindow =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgWindow.Show();
            }
        }

        private void cmbSpecilization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProcedureDoctor.SelectedItem != null)
            {
                if (((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).SpecilazationID == null || ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).SpecilazationID == (long)0)
                {
                    if ((MasterListItem)cmbSpecilization.SelectedItem != null)
                    {

                        FillDoctor(((MasterListItem)cmbAUnit.SelectedItem).ID, ((MasterListItem)cmbSpecilization.SelectedItem).ID);
                    }
                }
                else
                {
                    cmbdoctor.SelectedValue = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).DoctorID;
                }
            }
            else
            {
                if ((MasterListItem)cmbSpecilization.SelectedItem != null)
                {

                    FillDoctor(((MasterListItem)cmbAUnit.SelectedItem).ID, ((MasterListItem)cmbSpecilization.SelectedItem).ID);
                }
            }
 
        }
        bool IsModify = false;
        private void btnmodify_Click(object sender, RoutedEventArgs e)
        {
            if (dgProcedureDoctor.SelectedItem != null) 
            {
                IsModify = true;
                CmdAdd.Content = "Modify";

                cmbSpecilization.SelectedValue = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).SpecilazationID;
                cmbProcedure.SelectedValue = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).ProcedureID;
                cmbdoctor.SelectedValue=((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).DoctorID;
                autoUserName.SelectedItem = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).Nurse;
               // autoUserName.sel
                txtbillno.Text = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).BillNo;
                ProcDate.SelectedDate = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).Date;
                ProcTime.Value = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).Time;
                BillDate.SelectedDate = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).BillDate;


            }
         
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
         
                clsDeleteDoctorProcedureLinkBizActionVO bizAction = new clsDeleteDoctorProcedureLinkBizActionVO();
                bizAction.LinkDetails = new clsDoctorProcedureLinkVO();
                bizAction.LinkDetails.ID = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).ID;
                bizAction.LinkDetails.UnitID = ((clsDoctorProcedureLinkVO)dgProcedureDoctor.SelectedItem).UnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {

                        string msgTitle = "Palash";
                        string msgText = "Details Deleated Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += (result) =>
                        {
                            if (result == MessageBoxResult.OK)
                            {
                                SetUpPage();
                            }
                        };
                        msgWin.Show();
                    }
                 };
                client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

        }

    }
}
