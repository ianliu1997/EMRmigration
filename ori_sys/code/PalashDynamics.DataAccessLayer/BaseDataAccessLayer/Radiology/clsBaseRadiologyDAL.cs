using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using PalashDynamics.ValueObjects.Radiology;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseRadiologyDAL
    {
        static private clsBaseRadiologyDAL _instance = null;

        public static clsBaseRadiologyDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsRadiologyDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseRadiologyDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        public abstract IValueObject AddTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTemplateList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTemplateDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServiceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemList(IValueObject valueObject, clsUserVO UserVo);

        //Added By CDS
        public abstract IValueObject GetTestListWithDetailsID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddTestMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTestList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTemplateAndItemList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ViewTemplate(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateTechnicianEntry(IValueObject valueObject, clsUserVO UserVo);



        public abstract IValueObject GetOrderDetailsForWorkOrder(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddOrderBooking(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddOrderBooking(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject GetOrderList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOrderDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillTestComboBox(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateReportDelivery(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillTemplateComboBox(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTechnicianEntryList(IValueObject valueObject, clsUserVO UserVo);

        

        #region For Radiology Additions

        public abstract IValueObject UpdateRadOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRadologyPringHeaderFooterImage(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetResultEntryPrintDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetRadiologistbySpecia(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRadiologistGenderByitsID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddRadDetilsForEmail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateEmailDelivredIntoList(IValueObject valueObject, clsUserVO UserVo);

        #endregion



        //PACS
        public abstract IValueObject GetPACSTestList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPACSTestSeriesList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetVitalDetailsForRadiology(IValueObject valueObject, clsUserVO UserVO);

        public abstract clsPACSTestSeriesVO GetPACSTestSeriesListaspx(string MRNO, string PATIENTNAME, string MODALITY, string STUDYDATE, string STUDYTIME, string STUDYDESC);

        public abstract clsPACSTestSeriesVO GetPACSTestseriesListSeriesWise(bool IsForShowSeriesPACS, bool IsForShowPACS, string SERIESNUMBER, string MRNO, bool IsVisitWise, string MODALITY, string STUDYTIME, string STUDYDESC, string STUDYDATE);

        //  public abstract IValueObject GetRadiologistToTempList(IValueObject valueObject, clsUserVO UserVo);AddRadiologistToTempList
        public abstract IValueObject AddRadiologistToTempList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetRadioGender(IValueObject valueObject, clsUserVO userVO);



        public abstract IValueObject GetRadiologistToTempList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetRadiologistResultEntry(IValueObject valueObject, clsUserVO userVO);//Added By Yogesh K 2 6 2016

        public abstract IValueObject GetRadiologistResultEntryDefined(IValueObject valueObject, clsUserVO userVO);//Added By Yogesh K 2 6 2016


      
      public abstract IValueObject UploadReport(IValueObject valueObject, clsUserVO userVO);//Added By Yogesh K 3 6 2016

      public abstract IValueObject GetUploadReport(IValueObject valueObject, clsUserVO userVO);//Added By Yogesh K 3 6 2016




    }
}
