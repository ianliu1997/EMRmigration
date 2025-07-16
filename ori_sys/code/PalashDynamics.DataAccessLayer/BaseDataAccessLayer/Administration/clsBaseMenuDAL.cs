using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer
{
   public abstract class clsBaseMenuDAL
    {
        static private clsBaseMenuDAL _instance = null;

        public static clsBaseMenuDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsMenuDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseMenuDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetMenuList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetUserMenu(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetMenuGeneralDetails(IValueObject valueObject, clsUserVO UserVo);

   //     public abstract IValueObject GetPassConfig(IValueObject valueObject, clsUserVO UserVo);

    } 
}
