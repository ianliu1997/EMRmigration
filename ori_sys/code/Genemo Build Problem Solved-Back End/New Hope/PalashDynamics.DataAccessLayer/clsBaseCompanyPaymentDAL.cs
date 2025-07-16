namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseCompanyPaymentDAL
    {
        private static clsBaseCompanyPaymentDAL _instance;

        protected clsBaseCompanyPaymentDAL()
        {
        }

        public abstract IValueObject AddCompanyPaymentDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddInvoice(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject DeleteInvoiceBill(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCompanyInvoice(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCompanyInvoiceDetail(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCompanyPaymentDetail(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseCompanyPaymentDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsCompanyDAL";
                    _instance = (clsBaseCompanyPaymentDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetInvoiceSearchList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject ModifyInvoice(IValueObject valueObject, clsUserVO objUserVO);

    }
}

