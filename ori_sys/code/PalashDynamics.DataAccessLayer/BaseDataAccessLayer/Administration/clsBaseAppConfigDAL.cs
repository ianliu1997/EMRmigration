using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;


namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseAppConfigDAL
    {
        static private clsBaseAppConfigDAL _instance = null;

        public static clsBaseAppConfigDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsAppConfigDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseAppConfigDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetAppConfig(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateAppConfig(IValueObject valueObject, clsUserVO UserVo);
        
        public abstract IValueObject GetAutoEmailConfig(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateAutoEmailConfig(IValueObject valueObject, clsUserVO UserVo);

        // Added by BHUSHAN
        public abstract IValueObject GetAutoEmailCCTOConfig(IValueObject valueObject, clsUserVO UserVo);
        // Added by BHUSHAN    
        public abstract IValueObject SetStatusAutoEmailCCTO(IValueObject valueObject, clsUserVO UserVo);
        // Added by BHUSHAN
        public abstract IValueObject AddEmailIDCCTo(IValueObject valueObject, clsUserVO UserVo);


        //public abstract IValueObject GetLoginUnit(IValueObject valueObject, clsUserVO UserVo);
    } 
}
