namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseLoyaltyProgramDAL
    {
        private static clsBaseLoyaltyProgramDAL _instance;

        protected clsBaseLoyaltyProgramDAL()
        {
        }

        public abstract IValueObject AddLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillCardTypeCombo(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillFamilyTariffUsingRelationID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAttachmentDetailsByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCategoryList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetClinicList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFamilyDetailsByID(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseLoyaltyProgramDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsLoyaltyProgramDAL";
                    _instance = (clsBaseLoyaltyProgramDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLoyaltyProgramByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLoyaltyProgramTariffByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRelationMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo);
    }
}

