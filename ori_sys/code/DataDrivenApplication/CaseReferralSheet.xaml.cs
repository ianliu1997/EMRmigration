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

namespace DataDrivenApplication
{
    public partial class CaseReferralSheet : UserControl
    {
        public CaseReferralSheet()
        {
            InitializeComponent();
        }

        public void SetParticulars(string Name, string Age, string Gender, string Add, string Occupation, string Phone, string Date, string ClinicRefNo)
        {            
            TxtName.Text = Name!=null ? Name : "";
            TxtAge.Text = Age!=null ? Age : "";
            TxtGender.Text = Gender!=null ? Gender : "";
            TxtAdd.Text = Add!=null ? Add : "";
            TxtOccupation.Text = Occupation!=null ? Occupation : "";
            TxtPhone.Text = Phone!=null? Phone : "";
            TxtDate.Text = Date!=null? Date : "";
            TxtClinicRefNo.Text = ClinicRefNo!=null? ClinicRefNo:"";
        }

        public void SetReferralDetails(string ReferredByDoctor, string ReferredToDoctor, string ClinicNo1, string ClinicNo2, string MobileNo1, string MobileNo2)
        {
            TxtReferredBy.Text = ReferredByDoctor != null ? ReferredByDoctor : ""; ;
            TxtReferredTo.Text = ReferredToDoctor != null ? ReferredToDoctor : "";
            TxtClinicNo1.Text = ClinicNo1 != null ? ClinicNo1 : "";
            TxtClinicNo2.Text = ClinicNo2 != null ? ClinicNo2 : "";
            TxtMobile1.Text = MobileNo1 != null ? MobileNo1 : "";
            TxtMobile2.Text = MobileNo2 != null ? MobileNo2 : "";
        }

        public void SetProDiagChiefComplaint(string ProDiag, string ChiefComplaint)
        {
            TxtProDiag.Text = ProDiag != null ? ProDiag : "";
            TxtChiefComp.Text = ChiefComplaint != null ? ChiefComplaint : "";
        }

        public void SetPatientDetails(string Summary)
        {
            TxtSummary.Text = Summary != null ? Summary : "";
        }

        public void SetRefRemarks(string Remarks)
        {
            TxtObservation.Text = Remarks != null ? Remarks : "";
        }
    }
}
