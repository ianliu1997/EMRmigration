namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseVisitTypeDAL
    {
        private static clsBaseVisitTypeDAL _instance;

        protected clsBaseVisitTypeDAL()
        {
        }

        public abstract IValueObject AddVisitType(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject CheckVisitTypeMappedWithPackageService(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseVisitTypeDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsVisitTypeDAL";
                    _instance = (clsBaseVisitTypeDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetVisitTypeMaster(IValueObject valueObject, clsUserVO UserVO);
    }
}

