using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.NursingStation
{
    public abstract class clsBaseDrugAdministrationChartDAL
    {
        static private clsBaseDrugAdministrationChartDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseDrugAdministrationChartDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsDrugAdministrationChartDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseDrugAdministrationChartDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetCurrentPrescriptionList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDrugListForDrugChart(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject SaveDrugFeedingDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDrugFeedingDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateFeedingDetailsFreeze(IValueObject valueObject, clsUserVO objUserVO);
    }
}
