using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
   public abstract class clsBaseMISConfigDAL
    {
       static private clsBaseMISConfigDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>

       public static clsBaseMISConfigDAL GetInstance()
       {
           try
           {
               if (_instance == null)
               {
                   //Get the full name of data access layer class from xml file which stores the list of classess.
                   string _DerivedClassName = "clsMISConfigDAL";//HMSConfigurationManager.GetDataAccesslayerClassName("ServPathPatientDALClassName");
                   string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                   //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                   _instance = (clsBaseMISConfigDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
               }
           }
           catch (Exception ex)
           {
               throw;
           }
           return _instance;
       }

        /// <summary>
        /// Get the Master List
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVO"></param>
        /// <returns></returns>

       public abstract IValueObject AddMISConfig(IValueObject valueObject, clsUserVO UserVO);

       public abstract IValueObject GetMISConfig(IValueObject valueObject, clsUserVO UserVO);

       public abstract IValueObject GetMISReportType(IValueObject valueObject, clsUserVO UserVO);

       public abstract IValueObject GetStaff(IValueObject valueObject, clsUserVO UserVO);

       public abstract IValueObject AddUpdateMISConfig(IValueObject valueObject, clsUserVO UserVO);

       public abstract IValueObject GetMISConfigList(IValueObject valueObject, clsUserVO UserVO);
       
       public abstract IValueObject GetAutoEmailForMIS(IValueObject valueObject, clsUserVO UserVO);

       public abstract IValueObject GetMISDetailsFromCriteria(IValueObject valueObject, clsUserVO UserVO);

       public abstract IValueObject GetMISReportDetails(IValueObject valueObject, clsUserVO UserVO);


    }
}
