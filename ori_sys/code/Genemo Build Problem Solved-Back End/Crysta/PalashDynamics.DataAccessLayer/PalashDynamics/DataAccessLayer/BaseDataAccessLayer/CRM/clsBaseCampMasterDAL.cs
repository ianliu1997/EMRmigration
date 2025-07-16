namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseCampMasterDAL
    {
        private static clsBaseCampMasterDAL _instance;

        protected clsBaseCampMasterDAL()
        {
        }

        public abstract IValueObject AddCampDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddCampMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddCampService(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddEmailSMSSentDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPROPatient(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteCampService(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCampDetailsByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCampDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCampFreeAndConssService(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCampMasterByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCampMasterList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseCampMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsCampMasterDAL";
                    _instance = (clsBaseCampMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPROPatientList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateCampDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateCampMaster(IValueObject valueObject, clsUserVO UserVo);
    }
}

