namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBasePatientSposorDAL
    {
        private static clsBasePatientSposorDAL _instance;

        protected clsBasePatientSposorDAL()
        {
        }

        public abstract IValueObject AddFollowUpPatientNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientSponsor(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientSponsorDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject AddPatientSponsorForPathology(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeletePatientSponsor(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFollowUpPatient(IValueObject valueObject, clsUserVO UserVo);
        public static clsBasePatientSposorDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPatientSposorDAL";
                    _instance = (clsBasePatientSposorDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientPackageInfoList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientSponsor(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientSponsorCardList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientSponsorCompanyList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientSponsorGroupList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientSponsorList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientSponsorServiceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientSponsorTariffList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSelectedPackageInfoList(IValueObject valueObject, clsUserVO UserVo);
    }
}

