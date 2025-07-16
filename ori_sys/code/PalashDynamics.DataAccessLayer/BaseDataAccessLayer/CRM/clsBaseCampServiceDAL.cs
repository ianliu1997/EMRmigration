using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;


namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM
{
    public abstract class clsBaseCampServiceDAL
    {
        static private clsBaseCampServiceDAL _instance = null;
       
        public static clsBaseCampServiceDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsCampServiceDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseCampServiceDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }


            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }

        public abstract IValueObject AddCampService(IValueObject valueObject, clsUserVO UserVo);


    }
}
