namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseMasterEntryDAL
    {
        private static clsBaseMasterEntryDAL _instance;

        protected clsBaseMasterEntryDAL()
        {
        }

        public abstract IValueObject AddUpdateCashCounterDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateCityDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateCountryDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePriffixMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateRegionDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateStateDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAllItemListByMoluculeID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBdMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCashCounterDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCashCounterDetailsListByClinicID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCityDetailsByStateIDList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCityDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCountryDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseMasterEntryDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Administration.clsMasterEntryDAL";
                    _instance = (clsBaseMasterEntryDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPriffixMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRegionDetailsByCityIDList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRegionDetailsByCityIDListForReg(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRegionDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetStateDetailsByCountryIDList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetStateDetailsList(IValueObject valueObject, clsUserVO UserVo);
    }
}

