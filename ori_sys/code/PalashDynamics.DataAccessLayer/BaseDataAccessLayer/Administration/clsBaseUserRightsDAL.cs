using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBaseUserRightsDAL
    {
        static private clsBaseUserRightsDAL _instance = null;


        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseUserRightsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "Administration.clsUserRightsDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseUserRightsDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }
        //public abstract IValueObject GetAppConfig1(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddCreditLimit(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetUserRights(IValueObject valueObject, clsUserVO objUserVO);

        //public abstract IValueObject GetUnitList(IValueObject valueObject, clsUserVO objUserVO);
        //public abstract IValueObject GetUnitDetailsByID(IValueObject valueObject, clsUserVO objUserVO);

        // Added  By CDS 04/01/2016

        public abstract IValueObject GRNCountWithRightsAndFrequency(IValueObject valueObject, clsUserVO objUserVO);
        
    }
}
