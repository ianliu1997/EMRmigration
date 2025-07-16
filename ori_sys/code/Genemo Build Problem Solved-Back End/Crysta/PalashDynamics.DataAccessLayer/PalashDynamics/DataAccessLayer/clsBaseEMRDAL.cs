namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseEMRDAL
    {
        private static clsBaseEMRDAL _instance;

        protected clsBaseEMRDAL()
        {
        }

        public abstract IValueObject AddEMRTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateCostNote(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDeleteDiagnosisDetailsBizAction(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDeleteVitalDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateFollowupNote(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientAllergies(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientChiefComplaints(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientInvestigations(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientMedicationFromCPOE(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateReferralDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject deleteUploadPatientImageFromHystroScopy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DoctorlistonReferal(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject getDoctorlistonreferralasperService(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMR_CaseReferral_FieldList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMR_PCR_FieldList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMRTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMRTemplateList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMRTemplateListForOT(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMRTemplateListForOTProcedure(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseEMRDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsEMRDAL";
                    _instance = (clsBaseEMRDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientAllergies(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientChiefComplaints(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientCostNotes(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDiagnosisEMRDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDrugAllergies(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDrugAllergiesList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientEMRICDXDiagnosisList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientFollowUpNotes(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientGrowthChartMonthlyVisitList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientGrowthChartVisitList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientGrowthChartYearlyVisitList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientNewEMRDiagnosisList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientNewEMRDiagnosisListHistory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPastHistroScopyAndLaproscopy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPastPhysicalExamDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPatientVitalChartList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPreviousVisitServices(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPatientProcedureDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientReferraldetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientReferraldetailsListHistory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientReferreddetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetUploadPatientImage(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetUploadPatientImageFromHystroScopy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVitalListDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateEMRTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateUploadPatientImage(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UploadPatientImage(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UploadPatientImageFromHystroScopy(IValueObject valueObject, clsUserVO UserVo);
    }
}

