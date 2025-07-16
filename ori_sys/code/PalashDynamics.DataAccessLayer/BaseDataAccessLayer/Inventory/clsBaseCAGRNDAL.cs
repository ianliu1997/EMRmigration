using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
   public abstract class clsBaseCAGRNDAL
    {
        static private clsBaseCAGRNDAL _instance = null;

        public static clsBaseCAGRNDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "Inventory.clsCAGRNDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseCAGRNDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
       

        public abstract IValueObject GetCAGRNSearchList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCAGRNItemDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddCAGRNItem(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCAList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCAItemDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCAItemDetailListById(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteCAItemById(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateCAGRNItem(IValueObject valueObject, clsUserVO UserVo);

       
    }
}
