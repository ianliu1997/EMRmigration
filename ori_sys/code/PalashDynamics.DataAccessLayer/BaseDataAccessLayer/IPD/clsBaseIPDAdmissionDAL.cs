using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;


namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBaseIPDAdmissionDAL
    {
        static private clsBaseIPDAdmissionDAL _instance = null;

        public static clsBaseIPDAdmissionDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IPD.clsIPDAdmissionDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIPDAdmissionDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        /// <summary>
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public abstract IValueObject Save(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Get(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAdmissionList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDischargeApprovalList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMedicoLegalCase(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetConsentByTempleteID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetActiveAdmissionOfRegisteredPatient(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject UpdateAdmissionType(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRefEntityTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddRefEntityTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddAdviseDischargeList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAdvisedDischargeByAdmIdAndAdmUnitID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddDischargeApprovalByDepartment(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateDischargeApproval(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAdviseDischargeListForApproval(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddIPDAdmissionDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction); // to Save IPD Admission Details with transaction

        //emr
        public abstract IValueObject SaveRoundTrip(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckIPDRoundExitsOrNot(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject CancelIPDAdmission(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateCancelIPDAdmission(IValueObject valueObject, clsUserVO UserVo);
        //GetDoctorDetails
        public abstract IValueObject GetDoctorDetails(IValueObject valueObject, clsUserVO UserVo);
        
    }
}
