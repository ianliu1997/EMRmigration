using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    public abstract class clsBaseItemSalesDAL
    {
        static private clsBaseItemSalesDAL _instance = null;

        public static clsBaseItemSalesDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsItemSalesDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseItemSalesDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);

        #region Added by shikha
        public abstract IValueObject GetItemSale(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemSaleDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemSaleDetailsForCashSettlement(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddItemSaleReturn(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemSaleReturn(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemSaleReturnDetails(IValueObject valueObject, clsUserVO UserVo);
        #endregion

        public abstract IValueObject GetItemSaleComplete(IValueObject valueObject, clsUserVO UserVo);
    }
}
