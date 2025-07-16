namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseVisitDAL
    {
        private static clsBaseVisitDAL _instance;

        protected clsBaseVisitDAL()
        {
        }

        public abstract IValueObject AddVisit(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddVisit(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        public abstract IValueObject AddVisitForPathology(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject ClosePendingVisit(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAllPendingVisitCount(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCurrentVisit(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMRdignosisFillorNot(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMRVisit(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseVisitDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsVisitDAL";
                    _instance = (clsBaseVisitDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientEMRVisitList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPastVisit(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSecondLastVisit(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVisit(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVisitCount(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVisitList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateCurrentVisitStatus(IValueObject valueObject, clsUserVO UserVo);
    }
}

