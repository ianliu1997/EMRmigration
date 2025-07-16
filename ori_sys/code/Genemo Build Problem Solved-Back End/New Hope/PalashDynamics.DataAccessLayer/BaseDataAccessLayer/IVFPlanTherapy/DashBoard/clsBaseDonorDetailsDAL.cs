namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseDonorDetailsDAL
    {
        private static clsBaseDonorDetailsDAL _instance;

        protected clsBaseDonorDetailsDAL()
        {
        }

        public abstract IValueObject AddUpdateDonorBatch(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDonorDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDonorRegistrationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateRecievOocytesDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckDuplicasyDonorCodeAndBLab(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDetailsOfReceivedOocyte(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDetailsOfReceivedOocyteEmbryo(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDetailsOfReceivedOocyteEmbryoFromDonorCycle(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorBatchDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorDetailsAgainstSearch(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorDetailsForIUI(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorDetailsToModify(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseDonorDetailsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsDonorDetailsDAL";
                    _instance = (clsBaseDonorDetailsDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetOPUDate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenBatchAndSpermiogram(IValueObject valueObject, clsUserVO UserVo);
    }
}

