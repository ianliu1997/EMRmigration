namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBasePatientDAL
    {
        private static clsBasePatientDAL _instance;

        protected clsBasePatientDAL()
        {
        }

        public abstract IValueObject AddBarcodeImageTODB(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddDonarCode(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddNewCouple(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatient(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientDietPlan(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientForPathology(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientForPathology(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        public abstract IValueObject AddPatientIPDWithTransaction(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientLinkFile(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientOPDWithTransaction(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientOPDWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        public abstract IValueObject AddPatientScanDoc(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ADDPatientSignConsent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPrintedPatientConscent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddSurrogate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckPatientDuplicacy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckPatientFamilyRegisterd(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject clsGetPatientPenPusherDetailByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeletePatientSignConsent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCoupleGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDietPlanMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMRAdmVisitListByPatientID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetEMRAdmVisitListByPatientIDForConsol(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBasePatientDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPatientDAL";
                    _instance = (clsBasePatientDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIPDPatient(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetIPDPatientList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetOTPatientGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTPatientPackageList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatient(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientBillBalanceAmount(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientCoupleList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDetailsForCounterSale(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDetailsForCRM(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDetailsForPathology(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDietPlan(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDietPlanDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientFamilyDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientForLoyaltyCardIssue(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientGeneralDetailsListForSurrogacy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientHeaderDetailsForConsole(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientLabUploadReportData(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientLinkFile(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientLinkFileViewDetals(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientMRNoList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPenPusherDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientScanDoc(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientSignConsent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientTariffs(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPrintedPatientConscent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSurrogate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTariffAndRelationFromApplication(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject MovePatientPhotoToServer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject SavePatientPhoto(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePatientForLoyaltyCardIssue(IValueObject valueObject, clsUserVO UserVo);
    }
}

