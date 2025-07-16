namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.NursingStation
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseDrugAdministrationChartDAL
    {
        private static clsBaseDrugAdministrationChartDAL _instance;

        protected clsBaseDrugAdministrationChartDAL()
        {
        }

        public abstract IValueObject GetCurrentPrescriptionList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDrugFeedingDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDrugListForDrugChart(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseDrugAdministrationChartDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsDrugAdministrationChartDAL";
                    _instance = (clsBaseDrugAdministrationChartDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject SaveDrugFeedingDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateFeedingDetailsFreeze(IValueObject valueObject, clsUserVO objUserVO);
    }
}

