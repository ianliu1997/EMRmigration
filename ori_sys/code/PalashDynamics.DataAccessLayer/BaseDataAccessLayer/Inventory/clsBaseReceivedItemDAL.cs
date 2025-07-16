using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer
{
    
     public abstract class clsBaseReceivedItemDAL
    {
        static private clsBaseReceivedItemDAL _instance = null;

        public static clsBaseReceivedItemDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsReceivedItemDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseReceivedItemDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetReceivedList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemListByIssueReceivedId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddReceivedItem(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetReceivedListQS(IValueObject valueObject, clsUserVO UserVo);  //For Quarantine Stores    

        public abstract IValueObject GetReceivedListForGRNToQS(IValueObject valueObject, clsUserVO UserVo);  //For Quarantine Stores
    }
}
