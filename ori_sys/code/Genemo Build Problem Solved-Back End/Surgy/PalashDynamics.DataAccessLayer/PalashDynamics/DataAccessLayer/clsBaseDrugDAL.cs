namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseDrugDAL
    {
        private static clsBaseDrugDAL _instance;

        protected clsBaseDrugDAL()
        {
        }

        public abstract IValueObject AddBPControlDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddCaseReferral(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddDoctorSuggestedServices(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddGPControlDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientPrescription(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientPrescriptionResason(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPCR(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddVisionControlDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDrugList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFrequencyList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseDrugDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsDrugDAL";
                    _instance = (clsBaseDrugDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemMoleculeNameList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPrescription(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPrescriptionDetailByVisitID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPrescriptionDetailByVisitIDForPrint(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPrescriptionID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPrescriptionReason(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientVital(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateCaseReferral(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateDoctorSuggestedServices(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePatientPrescription(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePCR(IValueObject valueObject, clsUserVO UserVo);
    }
}

