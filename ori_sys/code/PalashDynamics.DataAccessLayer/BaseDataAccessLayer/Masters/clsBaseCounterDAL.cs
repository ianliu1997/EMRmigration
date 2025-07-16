using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters
{
    
    public abstract class clsBaseCounterDAL
    {
        static private clsBaseCounterDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseCounterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsCounterDAL";//HMSConfigurationManager.GetDataAccesslayerClassName("ServPathPatientDALClassName");
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseCounterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        public abstract IValueObject GetCounterListByUnitId(IValueObject valueObject, clsUserVO UserVO);
 


    }
}
