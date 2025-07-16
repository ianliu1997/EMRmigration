namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseReceivedItemAgainstReturnDAL
    {
        private static clsBaseReceivedItemAgainstReturnDAL _instance;

        protected clsBaseReceivedItemAgainstReturnDAL()
        {
        }

        public abstract IValueObject AddReceivedItemAgainstReturn(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseReceivedItemAgainstReturnDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsReceivedItemAgainstReturnDAL";
                    _instance = (clsBaseReceivedItemAgainstReturnDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemListByReturnReceivedId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetReceivedListAgainstReturn(IValueObject valueObject, clsUserVO UserVo);
    }
}

