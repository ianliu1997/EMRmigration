using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    public abstract class clsBaseBillDAL
    {
        static private clsBaseBillDAL _instance = null;

        public static clsBaseBillDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsBillDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseBillDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        /// <summary>
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        //By Anjali................................
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        //........................................

        public abstract IValueObject GetCompanyCreditDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetFreezedList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetFreezedSearchList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdatePaymentDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject MaintainPaymentLog(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetTotalBillAccountsLedgers(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateBillPaymentDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetDailyCollection(IValueObject valueObject, clsUserVO UserVo);

        // BY BHUSHAN . . . . . . . . . . 
        public abstract IValueObject GetBillSearch_IVF_List_DashBoard(IValueObject valueObject, clsUserVO UserVo);
        // BY BHUSHAN . . . . . . . . . . 
        public abstract IValueObject GetBillSearch_USG_List_DashBoard(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject AddDoctorShareRange(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject GetDoctorShareRangeList(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject ChangeStatus(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject GetPharmacyBillSearchList(IValueObject valueObject, clsUserVO UserVo);

   //     public abstract IValueObject GetDoctorShareRangeListNew(IValueObject valueObject, clsUserVO UserVo); // Added By Umesh for Doctor Payment

        // Added By CDS
        public abstract IValueObject UpdateBillPaymentDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbTransaction pTransaction, DbConnection pConnection);

        public abstract IValueObject DeleteIsTempCharges(IValueObject valueObject, clsUserVO UserVo);

        #region For IPD Module

        public abstract IValueObject FillGrossDiscountReason(IValueObject valueObject, clsUserVO UserVo);

        #endregion

        #region Added By CDS For PackageServiceSearchFor Package
        public abstract IValueObject GetTariffTypeDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPackageServiceDetails(IValueObject valueObject, clsUserVO UserVo);
        #endregion


        #region Added By CDS For Package Discount
        public abstract IValueObject ApplyPackageDiscountRateOnService(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ApplyPackageDiscountRateOnItems(IValueObject valueObject, clsUserVO UserVo);
        #endregion


        //By Anjali.....................
        public abstract IValueObject GetBillListForRequestApproval(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPharmacyBill(IValueObject valueObject, clsUserVO UserVo);
        //Added by Bhushanp
        public abstract IValueObject GenerateXML(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject DeleteFiles(IValueObject valueObject, clsUserVO UserVo);
        //.............................

        //added by rohini 
        public abstract IValueObject AddPathologyBill(IValueObject valueObject, clsUserVO UserVo);
        //

        //***//----------
        public abstract IValueObject GetBillClearanceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddBillClearanceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSaveBillClearanceList(IValueObject valueObject, clsUserVO UserVo);
        //---------------

        public abstract IValueObject CheckPackageAdvanceBilling(IValueObject valueObject, clsUserVO UserVo);
    
    }
}
