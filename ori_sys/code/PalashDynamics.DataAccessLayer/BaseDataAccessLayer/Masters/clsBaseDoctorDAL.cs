using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseDoctorDAL
    {

        static private clsBaseDoctorDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseDoctorDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsDoctorDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseDoctorDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }


        public abstract IValueObject GetDoctorList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorSchedule(IValueObject valueObject, clsUserVO objUserVO);



        public abstract IValueObject AddDoctorMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDepartmentListForDoctorMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorDetailListForDoctorMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorDetailListForDoctorMasterByDoctorID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDepartmentListByUnitID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDepartmentListForDoctorMasterByDoctorID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject DeleteDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);

        //Added by Somnath
        public abstract IValueObject AddDoctorBankInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorBankInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateDoctorBankInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorBankInfoById(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddDoctorAddressInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorAddressInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorAddressInfoById(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateDoctorAddressInfo(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetClassificationListForDoctorMaster(IValueObject valueObject, clsUserVO objUserVO);


        //Added By Somanath For Doctor Waiver Detail
        public abstract IValueObject GetDoctorWaiverDetailList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorTariffServiceDetailList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddDoctorWaiverDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorWaiverDetailListByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateDoctorWaiverInfo(IValueObject valueObject, clsUserVO objUserVO);
        //End
        public abstract IValueObject GetPendingDoctorDetail(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetExistingDoctorList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateDoctorShareInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddDoctorShareDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorShares1DetailsList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject DeleteExistingDoctorShareInfo(IValueObject valueObject, clsUserVO objUserVO);

        /*Added by CDS Only For IPD */
        public abstract IValueObject FillDoctorCombo(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorServiceLinkingByCategoryId(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUpdateDoctorServiceLinkingByCategory(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorServiceLinkingByClinic(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddupdateBulkRateChangeDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetBulkRateChangeDetailsList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetBulkRateChangeDetailsListByID(IValueObject valueObject, clsUserVO objUserVO);

        /*Added by Umesh For Doctor Payment */
        public abstract IValueObject AddDoctorBillPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddDoctorPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorPaymentBillsDetailList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorPaymentList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorBillPaymentDetailListByBillID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorShareAmount(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPaidDoctorPaymentDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject SaveDoctorPaymentDetailList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorDetailList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorPaymentChildDetailsList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorPaymentDetailsList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorPaymentFrontDetailsList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorShareRangeList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject SaveDoctorSettlePaymentDetailList(IValueObject valueObject, clsUserVO objUserVO);
        //SaveDoctorSettlePaymentDetailList

        // Added BY CDS For Bill With Respect TO Doctor Service Rate 
        public abstract IValueObject GetDoctorServiceLinkingByDoctorId(IValueObject valueObject, clsUserVO objUserVO);






























    }
}
