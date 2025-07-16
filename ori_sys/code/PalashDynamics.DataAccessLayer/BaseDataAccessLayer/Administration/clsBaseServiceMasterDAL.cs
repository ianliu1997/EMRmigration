using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBaseServiceMasterDAL
    {
        static private clsBaseServiceMasterDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>

        public static clsBaseServiceMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsServiceMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseServiceMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;

        }

        public abstract IValueObject GetServiceList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddServiceMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddServiceTariff(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddTariffService(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateTariffService(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAllServiceList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAllTariffApplicableList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAllServiceClassRateDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetTariffServiceMasterID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject CheckForTariffExistanceInServiceTariffMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetServiceTariff(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetTariffService(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject DeletetariffServiceAndServiceTariffMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject DeleteTariffServiceClassRateDetail(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetTariffServiceList(IValueObject valueObject, clsUserVO objUserVO);
        //added by pallavi
        public abstract IValueObject GetTariffServiceMasterList(IValueObject valueObject, clsUserVO objUserVO);
        //
        public abstract IValueObject GetServiceBySpecialization(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateServiceMasterStatus(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetTariffServiceClassRate(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject ChangeTariffServiceStatus(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetServiceMasterDetailsForId(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateServiceTariff(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetServiceTariffClassList(IValueObject valueObject, clsUserVO objUserVO);
        #region Added by Shikha
        public abstract IValueObject AddUpdateSpecialization(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetSpecializationDetails(IValueObject valueObject, clsUserVO objUserVO);


        public abstract IValueObject AddUpdateSubSpecialization(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetSubSpecializationDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddUpdateCompanyDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCompanyDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddUpdateCompanyAssociate(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetCompanyAssociateDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddUpdateTariff(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetTariffDetails(IValueObject valueObject, clsUserVO objUserVO);


        public abstract IValueObject AddUpdateDepartmentMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDepartmentMasterDetails(IValueObject valueObject, clsUserVO objUserVO);


        #endregion

        public abstract IValueObject GetBankBranchDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUpdateBankBranchDetails(IValueObject valueObject, clsUserVO objUserVO);

        //Added Only For IPD by CDS

        public abstract IValueObject AddUpdateServiceClassRates(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject ModifyServiceClassRates(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetServiceClassRateList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetTariffServiceClassRateNew(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetAdmissionTypeTariffServiceList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddUpdateDoctorServiceRateCategory(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetFrontPannelDataGridList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetFrontPannelDataGridByID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetServiceListForDocSerRateCat(IValueObject valueObject, clsUserVO objUserVO);


        public abstract IValueObject GetUnSelectedRecordForCategoryComboBox(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetUnSelectedRecordForClinicComboBox(IValueObject valueObject, clsUserVO objUserVO);

        // By Anjali............................
        public abstract IValueObject AddApplyLevelToService(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetApplyLevelToService(IValueObject valueObject, clsUserVO objUserVO);

        //........................................

        public abstract IValueObject GetTariffServiceListForPathology(IValueObject valueObject, clsUserVO objUserVO);


        public abstract IValueObject GetServiceListForPathology(IValueObject valueObject, clsUserVO objUserVO);

        #region GST Details added by Ashish Z. on dated 24062017
        public abstract IValueObject AddUpdateSeviceTaxDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetServiceTaxDetails(IValueObject valueObject, clsUserVO objUserVO);
        #endregion
    }
}
