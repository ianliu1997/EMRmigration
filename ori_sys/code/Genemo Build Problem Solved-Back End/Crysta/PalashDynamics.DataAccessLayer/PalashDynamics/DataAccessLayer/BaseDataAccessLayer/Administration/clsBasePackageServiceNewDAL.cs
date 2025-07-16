namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePackageServiceNewDAL
    {
        private static clsBasePackageServiceNewDAL _instance;

        protected clsBasePackageServiceNewDAL()
        {
        }

        public abstract IValueObject AddPackageConsentLink(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPackagePharmacyItemsNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPackageServiceMaster(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddPackageServicesNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPackageSourceTariffCompanyLinking(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePackageRateClinicWise(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeletePackageItemsDetilsNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeletePackageServicesDetilsNew(IValueObject valueObject, clsUserVO UserVo);
        public static clsBasePackageServiceNewDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPackageServiceNewDAL";
                    _instance = (clsBasePackageServiceNewDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPackageConditionalServiceListNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageConsentLink(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackagePharmacyItemListNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageRateClinicWise(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageRelationsList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPackageRelationsListForPackageOnly(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPackageServiceDetailListNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageServiceList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPackageServiceMasterDetailsForId(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPackageServiceRelationsListNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageSourceTariffCompanyLinking(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePackageApplicableToAllPharmacyItemsNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePackageFreezeStatusNew(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePackageServiceMasterStatus(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdatePackgeApproveStatusNew(IValueObject valueObject, clsUserVO UserVo);
    }
}

