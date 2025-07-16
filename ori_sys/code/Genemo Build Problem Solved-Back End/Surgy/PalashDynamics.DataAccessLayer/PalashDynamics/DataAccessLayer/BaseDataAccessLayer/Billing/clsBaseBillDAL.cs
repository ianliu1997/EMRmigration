namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseBillDAL
    {
        private static clsBaseBillDAL _instance;

        protected clsBaseBillDAL()
        {
        }

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        public abstract IValueObject AddBillClearanceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddDoctorShareRange(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject AddPathologyBill(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPharmacyBill(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ApplyPackageDiscountRateOnItems(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ApplyPackageDiscountRateOnService(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ChangeStatus(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject CheckPackageAdvanceBilling(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteFiles(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteIsTempCharges(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillGrossDiscountReason(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GenerateXML(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBillClearanceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBillListForRequestApproval(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBillSearch_IVF_List_DashBoard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBillSearch_USG_List_DashBoard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCompanyCreditDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDailyCollection(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorShareRangeList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetFreezedList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFreezedSearchList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseBillDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsBillDAL";
                    _instance = (clsBaseBillDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientPackageServiceDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPharmacyBillSearchList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSaveBillClearanceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTariffTypeDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTotalBillAccountsLedgers(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject MaintainPaymentLog(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateBillPaymentDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateBillPaymentDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbTransaction pTransaction, DbConnection pConnection);
        public abstract IValueObject UpdatePaymentDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

