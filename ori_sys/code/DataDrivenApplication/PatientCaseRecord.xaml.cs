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
    public partial class PatientCaseRecord : UserControl
    {
        public PatientCaseRecord()
        {
            InitializeComponent();
        }

        public void SetParticulars(string Name, string Age, string Gender, string Add, string Occupation, string Phone, string Date, string ClinicRefNo)
        {
            TxtName.Text = Name != null ? Name : "";
            TxtAge.Text = Age != null ? Age : "";
            TxtGender.Text = Gender != null ? Gender : "";
            TxtAdd.Text = Add != null ? Add : "";
            TxtOccupation.Text = Occupation != null ? Occupation : "";
            TxtPhone.Text = Phone != null ? Phone : "";
            TxtDate.Text = Date != null ? Date : "";
            TxtClinicRefNo.Text = ClinicRefNo != null ? ClinicRefNo : "";
        }
        public void SetIllnessSummary(string ComplaintReported, string ChiefComplaint, string PastHistory, string DrugHistory, string Allergies)
        {
            TxtComplaintReported.Text = ComplaintReported;
            TxtChiefComplaints.Text = ChiefComplaint;
            TxtPastHistory.Text = PastHistory;
            TxtDrugHistory.Text = DrugHistory;
            TxtAllergies.Text = Allergies;
        }
        public void SetObservation(string Investigations, string ProvisionalDiagnosis, string FinalDiagnosis)
        {
            TxtInvestigations.Text = Investigations;
            TxtProDiag.Text = ProvisionalDiagnosis;
            TxtFinalDiag.Text = FinalDiagnosis;
        }
        public void SetTherapy(Grid Therapy)
        {
            Therapy.HorizontalAlignment = HorizontalAlignment.Stretch;
            BorderMedication.Child = Therapy;
        }
        public void SetEducation(string AdvisoryAttached, string WhenToVisit, string SpecificInstructions)
        {
            TxtAdvisoryAttached.Text = AdvisoryAttached;
            TxtWhenToVisitHospital.Text = WhenToVisit;
            TxtSpecificInstructions.Text = SpecificInstructions;
        }
        public void SetOthers(string FollowUpDate, string FollowUpAt, string ReferralTo)
        {
            TxtFollowUpDate.Text = FollowUpDate;
            TxtFollowUpAt.Text = FollowUpAt;
            TxtReferralTo.Text = ReferralTo;
        }
    }
}
