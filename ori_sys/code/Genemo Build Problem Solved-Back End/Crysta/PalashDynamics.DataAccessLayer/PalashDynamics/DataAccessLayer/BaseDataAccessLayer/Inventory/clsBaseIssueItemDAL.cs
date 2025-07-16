namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIssueItemDAL
    {
        private static clsBaseIssueItemDAL _instance;

        protected clsBaseIssueItemDAL()
        {
        }

        public abstract IValueObject AddIssueItemToStore(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGRNToQSIssueList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIndentListBySupplier(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIssueItemDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsIssueItemDAL";
                    _instance = (clsBaseIssueItemDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIssueList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIssueListQS(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemListByIndentIdForIsueItem(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemListByIndentIdSrch(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemListByIssueId(IValueObject valueObject, clsUserVO UserVo);
    }
}

