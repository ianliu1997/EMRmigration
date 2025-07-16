namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseGRNReturnDAL
    {
        private static clsBaseGRNReturnDAL _instance;

        protected clsBaseGRNReturnDAL()
        {
        }

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseGRNReturnDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsGRNReturnDAL";
                    _instance = (clsBaseGRNReturnDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GRNReturnApprove(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GRNReturnReject(IValueObject valueObject, clsUserVO UserVo);
    }
}

