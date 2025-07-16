namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFPlanTherapyDAL
    {
        private static clsBaseIVFPlanTherapyDAL _instance;

        protected clsBaseIVFPlanTherapyDAL()
        {
        }

        public abstract IValueObject AddDiscardForSpermCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateETDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePlanTherapy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateSpermThawingDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateThawingDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateVitrificationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCoupleDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCoupleHeightAndWeight(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject getETDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFollicularModified(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFollicularMonitoringSizeList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFPlanTherapyDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsIVFPlanTherapyDAL";
                    _instance = (clsBaseIVFPlanTherapyDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientEMRDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSpermThawingDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSpermVitrificationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject getThawingDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTherapyDashBoard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTherapyDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTherapyDrugDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject getVitrificationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVitrificationForSpermCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateFollicularMonitoring(IValueObject valueObject, clsUserVO UserVo);
    }
}

