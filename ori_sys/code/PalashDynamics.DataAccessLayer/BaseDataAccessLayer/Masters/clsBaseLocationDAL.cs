using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters
{
    public abstract class clsBaseLocationDAL
    {

        static private clsBaseLocationDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseLocationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsLocationDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseLocationDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }

        public abstract IValueObject AddCountryInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddStateInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddDistInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddCityInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddAreaInfo(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetStateList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCountryList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDistList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCityList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAreaList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateCountryInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStateInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateDistInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateCityInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateAreaInfo(IValueObject valueObject, clsUserVO objUserVO);
        
        public abstract IValueObject AddAddressLocation6BizActionInfo(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetAddressLocation6List(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateAddressLocation6Info(IValueObject valueObject, clsUserVO objUserVO);
        
        public abstract IValueObject GetAddressLocation6ListByZoneId(IValueObject valueObject, clsUserVO objUserVO);

    
    }
}
