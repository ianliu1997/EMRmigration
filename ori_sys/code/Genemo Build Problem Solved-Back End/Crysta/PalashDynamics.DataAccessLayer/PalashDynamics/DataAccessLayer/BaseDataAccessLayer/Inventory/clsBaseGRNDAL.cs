namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseGRNDAL
    {
        private static clsBaseGRNDAL _instance;

        protected clsBaseGRNDAL()
        {
        }

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteGRNItems(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGRNForGRNReturn(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGRNInvoiceFile(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGRNItemsForGRNReturnQSSearch(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGRNItemsForQS(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseGRNDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsGRNDAL";
                    _instance = (clsBaseGRNDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemDetailsByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateGRNForApproval(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateGRNForApproval(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        public abstract IValueObject UpdateGRNForBarcode(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateGRNForRejection(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateGRNForRejection(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
    }
}

