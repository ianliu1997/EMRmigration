using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBasePackageServiceNewDAL
    {
        static private clsBasePackageServiceNewDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>

        public static clsBasePackageServiceNewDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsPackageServiceNewDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBasePackageServiceNewDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;

        }

        public abstract IValueObject AddPackageServiceMaster(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject GetPackageServiceList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdatePackageServiceMasterStatus(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPackageServiceMasterDetailsForId(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPackageRelationsList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddPackageServicesNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPackageServiceDetailListNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPackageServiceRelationsListNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPackagePharmacyItemsNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPackagePharmacyItemListNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject DeletePackageServicesDetilsNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPackageSourceTariffCompanyLinking(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdatePackageFreezeStatusNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdatePackgeApproveStatusNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPackageConditionalServiceListNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPackageSourceTariffCompanyLinking(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdatePackageRateClinicWise(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPackageRateClinicWise(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPackageRelationsListForPackageOnly(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject DeletePackageItemsDetilsNew(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdatePackageApplicableToAllPharmacyItemsNew(IValueObject valueObject, clsUserVO UserVo);

        //added by neena
        public abstract IValueObject AddPackageConsentLink(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageConsentLink(IValueObject valueObject, clsUserVO UserVo);
        //

    }
}
