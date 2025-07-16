using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBasePatientSourceMasterMasterDAL
    {
        static private clsBasePatientSourceMasterMasterDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBasePatientSourceMasterMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsPatientSourceMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBasePatientSourceMasterMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }

        public abstract IValueObject AddPatientSourceMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetTariffList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetTariffListForCompMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPatientSourceList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPatientSourceByID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPatientSourceListByPatientCategoryId(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetItemGroupMasterList(IValueObject valueObject, clsUserVO objUserVO);
        
    }

    //rohinee
    public abstract class clsBaseRegistrationChargesMasterMasterDAL
    {
        static private clsBaseRegistrationChargesMasterMasterDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseRegistrationChargesMasterMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsRegistrationChargesMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseRegistrationChargesMasterMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }

        public abstract IValueObject AddRegistartionChargesMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPatientSourceList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetRegistrationChargesByID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetRegistrationChargesByPatientTypeID(IValueObject valueObject, clsUserVO objUserVO);

     
        


    }
}
