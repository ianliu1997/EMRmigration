using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Master.CompanyPayment;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseCompanyPaymentDAL
    {

       static private clsBaseCompanyPaymentDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
       public static clsBaseCompanyPaymentDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsCompanyDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseCompanyPaymentDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception )
            {
                throw;
            }

            return _instance;
        }


        public abstract IValueObject GetCompanyInvoice(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCompanyPaymentDetail(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddCompanyPaymentDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddInvoice(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetInvoiceSearchList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetCompanyInvoiceDetail(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject ModifyInvoice(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject DeleteInvoiceBill(IValueObject valueObject, clsUserVO objUserVO);
        

    }
}
