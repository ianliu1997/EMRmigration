namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseIVFEmbryoTransferDAL
    {
        private static clsBaseIVFEmbryoTransferDAL _instance;

        protected clsBaseIVFEmbryoTransferDAL()
        {
        }

        public abstract IValueObject AddForwardedEmbryos(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject AddUpdateEmbryoTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEmbryoTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetForwardedEmbryos(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFEmbryoTransferDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsIVFEmbryoTransferDAL";
                    _instance = (clsBaseIVFEmbryoTransferDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }
    }
}

