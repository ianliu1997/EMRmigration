namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseIPDAdmissionDAL
    {
        private static clsBaseIPDAdmissionDAL _instance;

        protected clsBaseIPDAdmissionDAL()
        {
        }

        public abstract IValueObject AddAdviseDischargeList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddDischargeApprovalByDepartment(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddIPDAdmissionDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject AddRefEntityTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CancelIPDAdmission(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckIPDRoundExitsOrNot(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Get(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetActiveAdmissionOfRegisteredPatient(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetAdmissionList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAdvisedDischargeByAdmIdAndAdmUnitID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAdviseDischargeListForApproval(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetConsentByTempleteID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIPDAdmissionDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsIPDAdmissionDAL";
                    _instance = (clsBaseIPDAdmissionDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetMedicoLegalCase(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDischargeApprovalList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRefEntityTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Save(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject SaveRoundTrip(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateAdmissionType(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateCancelIPDAdmission(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateDischargeApproval(IValueObject valueObject, clsUserVO UserVo);
    }
}

