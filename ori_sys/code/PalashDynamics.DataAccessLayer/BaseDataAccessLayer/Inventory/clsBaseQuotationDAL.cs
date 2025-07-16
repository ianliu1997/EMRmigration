using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Quotation
{
  public abstract  class clsBaseQuotationDAL
    {
      static private clsBaseQuotationDAL _instance = null;

      public static clsBaseQuotationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "Inventory.clsQuotationDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseQuotationDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddQuotation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetQuotation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetQuotationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddQuotationAttachments(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetQuotationAttachments(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteQuotationAttachments(IValueObject valueObject, clsUserVO UserVo);
    }
    
}
