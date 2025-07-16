using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;


namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBasePassConfigDAL
    {
        static private clsBasePassConfigDAL _Instance = null;

        public static clsBasePassConfigDAL GetInstance()
        {
            try
            {
                if (_Instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsPassConfigDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create Instance of Database dependent DataAccessLayer class and typecast its baseclass
                    _Instance = (clsBasePassConfigDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);  
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return _Instance;
        }

        public abstract IValueObject GetPassConfig(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePasswordConfig(IValueObject valueObject, clsUserVO UserVo);

    }
}
