using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseOrderBookingDAL
    {
        static private clsBaseOrderBookingDAL _instance = null;

        //public DbTransaction myTrans = null;
        // public DbConnection myCon = null;



        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseOrderBookingDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsOrderBookingDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseOrderBookingDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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


        public abstract IValueObject AddPathOrderBooking(IValueObject valueObject, clsUserVO UserVo, DbTransaction myTrans, DbConnection myCon);

        public abstract IValueObject GetPathOrderBookingList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPathOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdatePathOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPathPatientReport(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdatePathOrderBookindDetail(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Adds Patholgy Test Master Details
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject AddPathoTestMaster(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Pathology Test Master List
        /// </summary>
        /// <param name="valueObject">clsGetPathoTestDetailsBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsGetPathoTestDetailsBizActionVO object</returns>
        public abstract IValueObject GetPathoTestMasterDetails(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        ///  Fills Parameter,Sample & Items related to test id
        /// </summary>
        /// <param name="valueObject">clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO object</returns>
        public abstract IValueObject GetPathoParameterSampleAndItemDetailsByTestID(IValueObject valueObject, clsUserVO UserVo);
        /// <summary>
        /// Adds patho profile master
        /// </summary>
        /// <param name="valueObject">AddPathoProfileMasterBizActionVO</param>
        /// <param name="UserVo">UserVO</param>
        /// <returns>AddPathoProfileMasterBizActionVO</returns>
        public abstract IValueObject GetPathoParameterUnitsByParamID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPathoPathoProfileMaster(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets patho profile master
        /// </summary>
        /// <param name="valueObject">clsGetPathoProfileDetailsBizActionVO</param>
        /// <param name="UserVo">UserVO</param>
        /// <returns>clsGetPathoProfileDetailsBizActionVO</returns>
        public abstract IValueObject GetPathoProfileDetails(IValueObject valueObject, clsUserVO UserVo);


        /// <summary>
        /// Gets patho profile master
        /// </summary>
        /// <param name="valueObject">clsGetPathoProfileDetailsBizActionVO</param>
        /// <param name="UserVo">UserVO</param>
        /// <returns>clsGetPathoProfileDetailsBizActionVO</returns>
        public abstract IValueObject FillPathoProfileTestByID(IValueObject valueObject, clsUserVO UserVo);

        //Added By CDS Changes by Bhushanp 17012017
        public abstract IValueObject GetTestListWithDetailsID(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        
        //Added by priyanka
        public abstract IValueObject GetTestList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestItemList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestForResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestParameterAndSubTesrForResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UploadReport(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetHelpValuesFroResultEntry(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetPathoSubTestMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoProfileServiceIDForPathoTestMaster(IValueObject valueObject, clsUserVO UserVo);

        //Added By SailyP
        public abstract IValueObject AddPathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeletePathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPathoTestForresultEntry(IValueObject valueObject, clsUserVO UserVo);

        // By BHUSHAN 
        public abstract IValueObject GetResultOnParameterSelection(IValueObject valueObject, clsUserVO UserVo);

        //By Anjali.....
        public abstract IValueObject GetPathoTestResultEntryDateWise(IValueObject valueObject, clsUserVO UserVo);

        #region For Pathology Additions

        public abstract IValueObject GetAgencyApplicableUnitList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathOrderBookingReportDetailList(IValueObject valueObject, clsUserVO UserVo);

        //Added by Rajshree
        public abstract IValueObject UpdateEmailDeliveryStatusinPathDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPathPatientReportToGetEmail(IValueObject valueObject, clsUserVO UserVo);

        #endregion


         #region NEwly Added 

        public abstract IValueObject FillTemplateComboBoxInPathoResultEntry(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetFinalizedParameter(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject ViewPathoTemplate(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject ApprovePathPatientReport(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPathoResultEntryPrintDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPathoTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ChangePathoTemplateStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPathoTemplate(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPathoUnAssignedAgencyTestList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetPathoOutSourcedTestList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject ChangePathoTestAgency(IValueObject valueObject, clsUserVO UserVO);
          #endregion


        #region Machine Parameters
        public abstract IValueObject AddUpdatePathoMachineParameter(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusMachineParameterMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetParameterLinkingList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateParameterLinking(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusParameterLinking(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMachineParameterList(IValueObject valueObject, clsUserVO UserVo);
        #endregion

        //by rohini dater 18.1.2016
        public abstract IValueObject GetParameterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddMachineToTest(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetMachineToTestList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetParameterListForTest(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetTemplateListForTest(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetTestDetailsByTestID(IValueObject valueObject, clsUserVO userVO);  //FOR TEST DETAILS

        public abstract IValueObject AddMachineToSubTest(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetMachineToSubTestList(IValueObject valueObject, clsUserVO userVO);


        public abstract IValueObject AddPathologistToTemp(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetPathologistToTempList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject FillPathoProfileService(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject GetPathoGender(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetServerDateTime(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetDispachReceiveDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetBatchCode(IValueObject valueObject, clsUserVO userVO);


        //maintain result  entry histry change after authorization 
        public abstract IValueObject AddHistory(IValueObject valueObject, clsUserVO userVO);      
        //

        //By Anumani Dated 9/02/2016

        public abstract IValueObject GetPreviousRecordDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetDeltaCheckDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetReflexTestingDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetAssignedAgency(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetBillingStatus(IValueObject valueObject, clsUserVO userVO);


    }
}
