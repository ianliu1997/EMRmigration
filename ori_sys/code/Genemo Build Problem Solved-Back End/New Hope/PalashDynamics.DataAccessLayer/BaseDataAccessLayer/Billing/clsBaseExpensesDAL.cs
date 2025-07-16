namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseExpensesDAL
    {
        private static clsBaseExpensesDAL _instance;

        protected clsBaseExpensesDAL()
        {
        }

        public abstract IValueObject AddExpenses(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetExpenses(IValueObject valueObject, clsUserVO Uservo);
        public static clsBaseExpensesDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsExpensesDAL";
                    _instance = (clsBaseExpensesDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

