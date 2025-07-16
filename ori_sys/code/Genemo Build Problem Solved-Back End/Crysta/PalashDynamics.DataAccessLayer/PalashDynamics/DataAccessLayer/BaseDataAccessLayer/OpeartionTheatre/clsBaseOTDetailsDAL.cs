namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.OpeartionTheatre
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseOTDetailsDAL
    {
        private static clsBaseOTDetailsDAL _instance;

        protected clsBaseOTDetailsDAL()
        {
        }

        public abstract IValueObject AddPatientWiseConsentPrinting(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDoctorNotesDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateOtDocEmpDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateOtItemDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateOtNotesDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateOtServicesDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateOtSurgeryDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePostInstructionDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddupdatOtDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetConsetDetailsForConsentID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDetailTablesOfOTDetailsByOTDetailID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDocEmpDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorForOTDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorNotesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseOTDetailsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsOTDetailsDAL";
                    _instance = (clsBaseOTDetailsDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTNotesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTSheetDetailsByOTID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientUnitIDForOtDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProceduresForServiceID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServicesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServicesForProcedureID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSurgeryDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateProcedureScheduleStatus(IValueObject valueObject, clsUserVO UserVo);
    }
}

