namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseOtConfigDAL
    {
        private static clsBaseOtConfigDAL _instance;

        protected clsBaseOtConfigDAL()
        {
        }

        public abstract IValueObject AddOTScheduleMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddProcedureMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateConsentMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateInstructionDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateOtTableDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddupdatePatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateProcedureCheckList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateProcedureSubCategory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CancelOTBooking(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckOTScheduleExistance(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCheckListByProcedureID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCheckListByScheduleID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetConfig_Patient_Fields(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetConsentByConsentType(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetConsentForProcedureID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetConsentMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDeleteConsents(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDocScheduleList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorForDocType(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorListBySpecialization(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorOrderForOTScheduling(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMRTemplateByProcID(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseOtConfigDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsOtConfigDAL";
                    _instance = (clsBaseOtConfigDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetInstructionDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetInstructionDetailsByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTBookingByDateID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTForProcedure(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTScheduleDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTScheduleMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTScheduleTime(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOtTableDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientConsents(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientConsentsDetailsInHTML(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDetailsForConsent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProcDetailsByProcID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProcedureCheckList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProcedureMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProceduresForOTBookingID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProcedureSubCategoryList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProcOTSchedule(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProcScheduleDetailsByProcScheduleID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServicesForProcedure(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetStaffByDesignation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetStaffForOTScheduling(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ProcedureUpdtStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject SaveConsentDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateInstructionStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateOTSchedule(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusProcedureCheckList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusProcedureSubCategory(IValueObject valueObject, clsUserVO UserVo);
    }
}

