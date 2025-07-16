namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseReceivedItemDAL
    {
        private static clsBaseReceivedItemDAL _instance;

        protected clsBaseReceivedItemDAL()
        {
        }

        public abstract IValueObject AddReceivedItem(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseReceivedItemDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsReceivedItemDAL";
                    _instance = (clsBaseReceivedItemDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemListByIssueReceivedId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetReceivedList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetReceivedListForGRNToQS(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetReceivedListQS(IValueObject valueObject, clsUserVO UserVo);
    }
}

