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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.UserControls;
using PalashDynamics;
using System.Windows.Browser;
using System.Text;
using PalashDynamics.Pharmacy.ItemSearch;

namespace OPDModule.Forms
{
    public partial class ClinicalNotes : UserControl
    {
        public ClinicalNotes()
        {
            InitializeComponent();
        }
        public long VisitId { get; set; }
        public long PatientId { get; set; }
        string FileName;
        byte[] data;
        clsGetPatientBizActionVO Patient = new clsGetPatientBizActionVO();

        private void ClinicalNotes_Loaded(object sender, RoutedEventArgs e)
        {
            FillGender();
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                txtMRNo.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                VisitId = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                GetPatient();

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

                        dgPatientAttachmentItems.ItemsSource = Patient.PatientAttachmentDetailList;
                      //  txtReferenceDoctor.Text = ObjPatient.PatientDetails.Doctor.ToString();



                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();
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
        
        private void hlbViewAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientAttachmentItems.SelectedItem != null)
            {
                for (int i = 0; i < Patient.PatientAttachmentDetailList.Count; i++)
                {
                    FileName = Patient.PatientAttachmentDetailList[i].AttachedFileName;
                    data = Patient.PatientAttachmentDetailList[i].Attachment;
                    HtmlPage.Window.Invoke("openAttachment", new string[] { FileName });
                }
            }
            
        }
    }
}
