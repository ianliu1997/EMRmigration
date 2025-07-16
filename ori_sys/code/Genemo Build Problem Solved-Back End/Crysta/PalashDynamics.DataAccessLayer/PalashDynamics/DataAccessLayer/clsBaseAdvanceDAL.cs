namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseAdvanceDAL
    {
        private static clsBaseAdvanceDAL _instance;

        protected clsBaseAdvanceDAL()
        {
        }

        public abstract IValueObject AddAdvance(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddAdvance(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject AddAdvanceWithPackageBill(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject DeleteAdvance(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAdvance(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAdvanceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAdvanceListForRequestApproval(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseAdvanceDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsAdvanceDAL";
                    _instance = (clsBaseAdvanceDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientAdvanceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateAdvance(IValueObject valueObject, clsUserVO UserVo);
    }
}

