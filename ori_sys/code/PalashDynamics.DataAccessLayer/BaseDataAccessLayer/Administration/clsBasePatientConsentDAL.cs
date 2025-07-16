using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
   public abstract class clsBasePatientConsentDAL
    {

       static private clsBasePatientConsentDAL _instance = null;


        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
       public static clsBasePatientConsentDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsPatientConsentDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBasePatientConsentDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }


        public abstract IValueObject AddPatientConsent(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPatientConsent(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPatientConsentList(IValueObject valueObject, clsUserVO UserVo);

       //added by neena
        public abstract IValueObject GetIVFPackegeConsentList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFPackegeConsentList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFSavedPackegeConsentList(IValueObject valueObject, clsUserVO UserVo);
       //
    }
}
