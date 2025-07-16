namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFDAL
    {
        private static clsBaseIVFDAL _instance;

        protected clsBaseIVFDAL()
        {
        }

        public abstract IValueObject AddClinicalSummary(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddFemaleHistory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddGeneralExaminationForFemale(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddGeneralExaminationForMale(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddMaleHistory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetClinicalSummary(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleHistory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGeneralExaminationForFemale(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGeneralExaminationForMale(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsIVFDAL";
                    _instance = (clsBaseIVFDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetMaleHistory(IValueObject valueobject, clsUserVO UserVO);
        public abstract IValueObject UpdateFemaleHistory(IValueObject valueObject, clsUserVO UserVo);
    }
}

