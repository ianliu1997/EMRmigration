namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIndentDAL
    {
        private static clsBaseIndentDAL _instance;

        protected clsBaseIndentDAL()
        {
        }

        public abstract IValueObject AddIndent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ApproveDirect(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CloseIndentFromDashBoard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ClosePurchaseRequisitionFromDashBoard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIndentDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIndentList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIndentListByStoreId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIndentListForDashBoard(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIndentDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsIndentDAL";
                    _instance = (clsBaseIndentDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject UpdateIndent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateIndentForChangeAndApprove(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateIndentOnlyForFreeze(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateIndentRemarkandCancelIndent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateIndentRemarkandRejectIndent(IValueObject valueObject, clsUserVO UserVo);
    }
}

