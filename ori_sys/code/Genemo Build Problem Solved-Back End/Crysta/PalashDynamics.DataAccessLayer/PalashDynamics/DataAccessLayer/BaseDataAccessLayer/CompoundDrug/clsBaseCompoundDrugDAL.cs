namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CompoundDrug
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseCompoundDrugDAL
    {
        private static clsBaseCompoundDrugDAL _instance;

        protected clsBaseCompoundDrugDAL()
        {
        }

        public abstract IValueObject AddCompoundDrug(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientCompoundDrug(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckCompoundDrug(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCompoundDrug(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCompoundDrugAndDetailsByIDAndUnitID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCompoundDrugDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseCompoundDrugDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "CompoundDrug.clsCompoundDrugDAL";
                    _instance = (clsBaseCompoundDrugDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientPrescriptionCompoundDrugByVisitID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateCompoundDrugByIDAndUnitID(IValueObject valueObject, clsUserVO UserVo);
    }
}

