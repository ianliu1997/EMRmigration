namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFDashboad_PlanTherapyDAL
    {
        private static clsBaseIVFDashboad_PlanTherapyDAL _instance;

        protected clsBaseIVFDashboad_PlanTherapyDAL()
        {
        }

        public abstract IValueObject AddDocument(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateBirthDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateBirthDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDiagnosis(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoardPlanTherapy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoardPlanTherapyadditionmeasure(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoardPlanTherapyCPOEServices(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateLutealPhaseDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateOutcomeDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteBirthDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteDocument(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBirthDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBirthDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBirthDetailsMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCycleCodeForCombobox(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDocument(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFollicularMonitoringSizeList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFolliculeLRSum(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetHalfBilled(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFDashboad_PlanTherapyDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFDashboad_PlanTherapyDAL";
                    _instance = (clsBaseIVFDashboad_PlanTherapyDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIVFDashboardcurrentDiagnosis(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashboardInvestigation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashBoardPlanTherapyadditionmeasure(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashBoardPlanTherapyCPOEServices(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashboardPrescriptionDiagnosis(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashboardPreviousDiagnosis(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLutealPhaseDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetManagementVisible(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOutcomeDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPACVisible(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientListForDashboard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSearchKeywordforPatient(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSurrogatePatientListForDashboard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSurrogatePatientListForTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTherapyDetailListForIVFDashboard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTherapyDetailsFromCycleCode(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVisitForARTPrescription(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateFollicularMonitoring(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateTherapyExecution(IValueObject valueObject, clsUserVO UserVo);
    }
}

