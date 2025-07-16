namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Radiology;
    using System;
    using System.Data.Common;

    public abstract class clsBaseRadiologyDAL
    {
        private static clsBaseRadiologyDAL _instance;

        protected clsBaseRadiologyDAL()
        {
        }

        public abstract IValueObject AddOrderBooking(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddOrderBooking(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject AddRadDetilsForEmail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddRadiologistToTempList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddTestMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillTemplateComboBox(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillTestComboBox(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseRadiologyDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsRadiologyDAL";
                    _instance = (clsBaseRadiologyDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOrderDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOrderDetailsForWorkOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOrderList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPACSTestList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPACSTestSeriesList(IValueObject valueObject, clsUserVO UserVo);
        public abstract clsPACSTestSeriesVO GetPACSTestSeriesListaspx(string MRNO, string PATIENTNAME, string MODALITY, string STUDYDATE, string STUDYTIME, string STUDYDESC);
        public abstract clsPACSTestSeriesVO GetPACSTestseriesListSeriesWise(bool IsForShowSeriesPACS, bool IsForShowPACS, string SERIESNUMBER, string MRNO, bool IsVisitWise, string MODALITY, string STUDYTIME, string STUDYDESC, string STUDYDATE);
        public abstract IValueObject GetRadioGender(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetRadiologistbySpecia(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRadiologistGenderByitsID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRadiologistResultEntry(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetRadiologistResultEntryDefined(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetRadiologistToTempList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetRadologyPringHeaderFooterImage(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetResultEntryPrintDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServiceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTechnicianEntryList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTemplateAndItemList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTemplateDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTemplateList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTestList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTestListWithDetailsID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetUploadReport(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetVitalDetailsForRadiology(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject UpdateEmailDelivredIntoList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateRadOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateReportDelivery(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateTechnicianEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UploadReport(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject ViewTemplate(IValueObject valueObject, clsUserVO UserVo);
    }
}

