using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{


    public abstract class clsBaseItemStockDAL
    {
        static private clsBaseItemStockDAL _instance = null;

        public static clsBaseItemStockDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsItemStockDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseItemStockDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        /// <summary>
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);

        public abstract IValueObject GetItemBatchwiseStock(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetItemCurrentStockList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetAvailableStockList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddFree(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);  // Add Stock For Free Items

        public abstract IValueObject GetItemBatchwiseStockFree(IValueObject valueObject, clsUserVO UserVo);  // Use to show only Free Item Batches

        public abstract IValueObject GetGRNItemBatchwiseStockForQS(IValueObject valueObject, clsUserVO UserVo);  // Use to show only GRN Items Batches

        
    }
}
