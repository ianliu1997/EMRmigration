using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.TokenDisplay
{
   
    public abstract class clsBaseTokenDisplayDAL
    {
        static private clsBaseTokenDisplayDAL _instance = null;

        public static clsBaseTokenDisplayDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "TokenDisplay.clsTokenDisplayDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseTokenDisplayDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateTokenDisplayDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateStatusTokenDisplay(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetTokenDisplayDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientTokenDisplayDetails(IValueObject valueObject, clsUserVO UserVo);
        
       

    }
}
