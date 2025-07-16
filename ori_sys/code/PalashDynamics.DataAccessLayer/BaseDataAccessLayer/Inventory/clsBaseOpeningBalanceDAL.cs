using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    public abstract class clsBaseOpeningBalanceDAL
    {
        static private clsBaseOpeningBalanceDAL _instance = null;

        public static clsBaseOpeningBalanceDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "Inventory.clsOpeningBalanceDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseOpeningBalanceDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddOpeningBalance(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetStockDetailsForOpeningBalance(IValueObject valueObject, clsUserVO UserVo);

      //  public abstract IValueObject CheckDuplicasy(IValueObject valueObject, clsUserVO UserVo);
       
    }
}
