namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseDischargeSummaryDAL
    {
        private static clsBaseDischargeSummaryDAL _instance;

        protected clsBaseDischargeSummaryDAL()
        {
        }

        public abstract IValueObject AddUpdateDischargeSummary(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillDataGridDischargeSummaryList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseDischargeSummaryDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsDischargeSummaryDAL";
                    _instance = (clsBaseDischargeSummaryDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIPDDischargeSummaryList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientsDischargeSummaryInfoInHTML(IValueObject valueObject, clsUserVO UserVo);
    }
}

