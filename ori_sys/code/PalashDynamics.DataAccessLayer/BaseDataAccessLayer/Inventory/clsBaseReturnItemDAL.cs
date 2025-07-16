using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    public abstract class clsBaseReturnItemDAL
    {
        static private clsBaseReturnItemDAL _instance = null;

        public static clsBaseReturnItemDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsReturnItemDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseReturnItemDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetReturnList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemListByReturnId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddReturnItemToStore(IValueObject valueObject, clsUserVO UserVo);
              
    }
}
