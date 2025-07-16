using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer
{

    public abstract class clsBaseTariffDAL
    {
        static private clsBaseTariffDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseTariffDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsTariffDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseTariffDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }


        public abstract IValueObject GetTariffList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPatientCategoryList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetCompanyList(IValueObject valueObject, clsUserVO objUserVO);



    }
}
