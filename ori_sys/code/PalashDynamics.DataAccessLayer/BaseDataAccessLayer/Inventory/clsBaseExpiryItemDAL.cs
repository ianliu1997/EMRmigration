using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    public abstract class clsBaseExpiryItemDAL
    {
        static private clsBaseExpiryItemDAL _instance = null;

        public static clsBaseExpiryItemDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsExpiryItemDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseExpiryItemDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetExpiryItemList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddExpiryItemReturn(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetExpiryItemReturnForDashBoard(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetExpiryList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetExpiryListForExpiredReturn(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetExpiredReturnDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateExpiryForApproveOrReject(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateExpiryForApproveOrReject(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);

        public abstract IValueObject ApproveExpiryItemReturn(IValueObject valueObject, clsUserVO UserVo);

        

    }
}
