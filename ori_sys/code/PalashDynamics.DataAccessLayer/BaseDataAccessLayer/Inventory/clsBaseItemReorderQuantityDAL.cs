using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
   public abstract class clsBaseItemReorderQuantityDAL
    {
       static private clsBaseItemReorderQuantityDAL _instance = null;

       public static clsBaseItemReorderQuantityDAL GetInstance()
       {
           try
           {
               if (_instance == null)
               {
                   //Get the full name of data access layer class from xml file which stores the list of classess.
                   string _DerivedClassName = "Inventory.clsItemReorderQuantityDAL";
                   string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                   //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                   _instance = (clsBaseItemReorderQuantityDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
       public abstract IValueObject GetItemReorderQuantity(IValueObject valueObject, clsUserVO UserVo);

       
    }
}
