using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBasePatientSposorDAL
    {
        static private clsBasePatientSposorDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBasePatientSposorDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsPatientSposorDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBasePatientSposorDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddPatientSponsor(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientSponsor(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientSponsorList(IValueObject valueObject, clsUserVO UserVo);

        // Added By CDS For Other Service Search Window
        public abstract IValueObject GetPatientSponsorCompanyList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientSponsorTariffList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPackageInfoList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetSelectedPackageInfoList(IValueObject valueObject, clsUserVO UserVo);        // For Package New Changes Added on 19062018

        // END

        public abstract IValueObject GetPatientSponsorGroupList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientSponsorServiceList(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetPatientSponsorCardList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject DeletePatientSponsor(IValueObject valueObject, clsUserVO UserVo);

        // Added By CDS For FollowUp 
        public abstract IValueObject GetFollowUpPatient(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddFollowUpPatientNew(IValueObject valueObject, clsUserVO UserVo);
        
        // END

        # region For IPD

        public abstract IValueObject AddPatientSponsorDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);

        # endregion

        public abstract IValueObject AddPatientSponsorForPathology(IValueObject valueObject, clsUserVO UserVo);
        

    }
}
