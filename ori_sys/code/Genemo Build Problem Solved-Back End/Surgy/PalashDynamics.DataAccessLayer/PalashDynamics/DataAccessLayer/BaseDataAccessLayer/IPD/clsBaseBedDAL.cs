namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseBedDAL
    {
        private static clsBaseBedDAL _instance;

        protected clsBaseBedDAL()
        {
        }

        public abstract IValueObject AddBedUnderMaintenance(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateReleaseBedUnderMaintenance(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckFinalBillbyPatientID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBillAndBedByAdmIDAndAdmUnitID(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseBedDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsBedDAL";
                    _instance = (clsBaseBedDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIPDBedTransferDetailsForSelectedPatient(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetIPDBedTransferList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIPDWardByClassID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetReleaseBedUnderMaintenanceList(IValueObject valueObject, clsUserVO UserVo);
    }
}

