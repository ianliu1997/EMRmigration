namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIPDIntakeOutputChartDAL
    {
        private static clsBaseIPDIntakeOutputChartDAL _instance;

        protected clsBaseIPDIntakeOutputChartDAL()
        {
        }

        public abstract IValueObject AddUpdateIntakeOutputChart(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIPDIntakeOutputChartDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsIPDIntakeOutputChartDAL";
                    _instance = (clsBaseIPDIntakeOutputChartDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIntakeOutputChartDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIntakeOutputChartDetailsByPatientID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateIsFreezedStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusIntakeOutputChart(IValueObject valueObject, clsUserVO UserVo);
    }
}

