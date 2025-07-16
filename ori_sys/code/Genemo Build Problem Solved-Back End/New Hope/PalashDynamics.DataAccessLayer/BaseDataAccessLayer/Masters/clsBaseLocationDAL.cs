namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseLocationDAL
    {
        private static clsBaseLocationDAL _instance;

        protected clsBaseLocationDAL()
        {
        }

        public abstract IValueObject AddAddressLocation6BizActionInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddAreaInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddCityInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddCountryInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddDistInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddStateInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAddressLocation6List(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAddressLocation6ListByZoneId(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAreaList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCityList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCountryList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDistList(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseLocationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsLocationDAL";
                    _instance = (clsBaseLocationDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetStateList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateAddressLocation6Info(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateAreaInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateCityInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateCountryInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateDistInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStateInfo(IValueObject valueObject, clsUserVO objUserVO);
    }
}

