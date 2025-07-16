namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseIVFLabDaysSummaryDAL
    {
        private static clsBaseIVFLabDaysSummaryDAL _instance;

        protected clsBaseIVFLabDaysSummaryDAL()
        {
        }

        public abstract IValueObject AddUpdateLabDaysSummary(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateLabDaysSummary(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject GetArtCycleSummary(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFLabDaysSummaryDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsIVFLabDaysSummaryDAL";
                    _instance = (clsBaseIVFLabDaysSummaryDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetLabDaysSummary(IValueObject valueObject, clsUserVO UserVo);
    }
}

