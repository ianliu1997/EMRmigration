using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    public abstract class clsBaseIVFDashboad_PlanTherapyDAL
    {
        static private clsBaseIVFDashboad_PlanTherapyDAL _instance = null;

        public static clsBaseIVFDashboad_PlanTherapyDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.clsIVFDashboad_PlanTherapyDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFDashboad_PlanTherapyDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateIVFDashBoardPlanTherapy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDiagnosis(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTherapyDetailListForIVFDashboard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFollicularMonitoringSizeList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateFollicularMonitoring(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateTherapyExecution(IValueObject valueObject, clsUserVO UserVo);
        #region LutealPhase
        public abstract IValueObject AddUpdateLutealPhaseDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLutealPhaseDetails(IValueObject valueObject, clsUserVO UserVo);
        #endregion
        #region Outcome
        public abstract IValueObject AddUpdateOutcomeDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOutcomeDetails(IValueObject valueObject, clsUserVO UserVo);
        #endregion
        #region BirthDetails
        public abstract IValueObject AddUpdateBirthDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBirthDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteBirthDetails(IValueObject valueObject, clsUserVO UserVo);

        #endregion
        #region Document
        public abstract IValueObject AddDocument(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDocument(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteDocument(IValueObject valueObject, clsUserVO UserVo);

        #endregion
        #region for cycle code
        public abstract IValueObject GetCycleCodeForCombobox(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTherapyDetailsFromCycleCode(IValueObject valueObject, clsUserVO UserVo);
        #endregion
        #region PatientList
        public abstract IValueObject GetPatientListForDashboard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSearchKeywordforPatient(IValueObject valueObject, clsUserVO UserVo);

        #endregion

        //Dashboard previous diagnosis
        public abstract IValueObject GetIVFDashboardPreviousDiagnosis(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashboardcurrentDiagnosis(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoardPlanTherapyadditionmeasure(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashBoardPlanTherapyadditionmeasure(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashBoardPlanTherapyCPOEServices(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoardPlanTherapyCPOEServices(IValueObject valueObject, clsUserVO UserVo);

        //added by neena
        public abstract IValueObject GetFolliculeLRSum(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetManagementVisible(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBirthDetailsMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBirthDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateBirthDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVisitForARTPrescription(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashboardPrescriptionDiagnosis(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashboardInvestigation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPACVisible(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetHalfBilled(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSurrogatePatientListForDashboard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSurrogatePatientListForTransfer(IValueObject valueObject, clsUserVO UserVo);    
        
    }
}
