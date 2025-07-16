namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFDashboard_VitrificationDAL
    {
        private static clsBaseIVFDashboard_VitrificationDAL _instance;

        protected clsBaseIVFDashboard_VitrificationDAL()
        {
        }

        public abstract IValueObject AddUpdateDonateDiscardEmbryoForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDonateDiscardOocyteForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoard_RenewalDate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoard_Vitrification(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoard_VitrificationSingle(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFDashboard_VitrificationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFDashboard_VitrificationDAL";
                    _instance = (clsBaseIVFDashboard_VitrificationDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIVFDashBoard_PreviousEmbFromVitrification(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashBoard_PreviousOocyteFromVitrification(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashBoard_Vitrification(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOocyteVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetUsedEmbryoDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateVitrificationDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

