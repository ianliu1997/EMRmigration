using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
   public abstract class clsBaseWOGRNDAL
    {
       static private clsBaseWOGRNDAL _instance = null;

       public static clsBaseWOGRNDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsWOGRNDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseWOGRNDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        public abstract IValueObject WOAdd(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject WOGetSearchList(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject WOGetItemDetailsList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject WOGetGRNForGRNReturn(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject CheckItemSupplier(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject WODeleteGRNItems(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject WOUpdateGRNForBarcode(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject WOGetItemDetailsByID(IValueObject valueObject, clsUserVO UserVo);
    }
}
