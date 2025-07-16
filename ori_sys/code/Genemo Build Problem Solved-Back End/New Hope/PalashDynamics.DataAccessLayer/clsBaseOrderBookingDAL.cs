namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseOrderBookingDAL
    {
        private static clsBaseOrderBookingDAL _instance;

        protected clsBaseOrderBookingDAL()
        {
        }

        public abstract IValueObject AddHistory(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddMachineToSubTest(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddMachineToTest(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddPathologistToTemp(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddPathoPathoProfileMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPathOrderBooking(IValueObject valueObject, clsUserVO UserVo, DbTransaction myTrans, DbConnection myCon);
        public abstract IValueObject AddPathoTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPathoTestMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPathPatientReport(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPathPatientReportToGetEmail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateParameterLinking(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePathoMachineParameter(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ApprovePathPatientReport(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ChangePathoTemplateStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ChangePathoTestAgency(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject DeletePathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillPathoProfileService(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject FillPathoProfileTestByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillTemplateComboBoxInPathoResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAgencyApplicableUnitList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAssignedAgency(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetBatchCode(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetBillingStatus(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetDeltaCheckDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetDispachReceiveDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetFinalizedParameter(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetHelpValuesFroResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseOrderBookingDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsOrderBookingDAL";
                    _instance = (clsBaseOrderBookingDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetMachineParameterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMachineToSubTestList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetMachineToTestList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetParameterLinkingList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetParameterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetParameterListForTest(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetPathoGender(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetPathologistToTempList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetPathoOutSourcedTestList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetPathoParameterSampleAndItemDetailsByTestID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoParameterUnitsByParamID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoProfileDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoProfileServiceIDForPathoTestMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathOrderBookingList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathOrderBookingReportDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoResultEntryPrintDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoSubTestMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestForresultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestForResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestItemList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestParameterAndSubTesrForResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoTestResultEntryDateWise(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPathoUnAssignedAgencyTestList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetPreviousRecordDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetReflexTestingDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetResultOnParameterSelection(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServerDateTime(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetTemplateListForTest(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetTestDetailsByTestID(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetTestList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTestListWithDetailsID(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject UpdateEmailDeliveryStatusinPathDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePathOrderBookindDetail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePathOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusMachineParameterMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusParameterLinking(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UploadReport(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ViewPathoTemplate(IValueObject valueObject, clsUserVO UserVo);
    }
}

