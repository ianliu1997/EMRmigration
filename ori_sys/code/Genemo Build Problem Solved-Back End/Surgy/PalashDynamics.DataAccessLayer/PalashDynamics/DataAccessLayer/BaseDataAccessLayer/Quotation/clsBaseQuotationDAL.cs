namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Quotation
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseQuotationDAL
    {
        private static clsBaseQuotationDAL _instance;

        protected clsBaseQuotationDAL()
        {
        }

        public abstract IValueObject AddQuotation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddQuotationAttachments(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteQuotationAttachments(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseQuotationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Inventory.clsQuotationDAL";
                    _instance = (clsBaseQuotationDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetQuotation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetQuotationAttachments(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetQuotationDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

