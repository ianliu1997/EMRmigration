using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBasePatientEMRDataDAL
    {
        static private clsBasePatientEMRDataDAL _instance = null;

        public static clsBasePatientEMRDataDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsPatientEMRDataDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBasePatientEMRDataDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        public abstract IValueObject AddPatientEMRData(IValueObject valueObject, clsUserVO UserVo);

        //// To save Template Data FieldWise in Table-T_PatientEMRDetails
        // Author- Harish Kirnani , Date:22July2011
        public abstract IValueObject AddUpdatePatientEMRDetail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientEMRUploadedFiles(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientEMRUploadedFiles(IValueObject valueObject, clsUserVO UserVo);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject UpdatePatientEMRData(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetPatientEMRData(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPatientFeedback(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePatientFeedback(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientFeedback(IValueObject valueObject, clsUserVO UserVo);

        //Added By Saily For IVF EMR Summary Load

        public abstract IValueObject GetPatientIVFEMR(IValueObject valueObject, clsUserVO UserVo);
        
        // Added by Saily for EMR FieldValues

        public abstract IValueObject GetEMRFieldValue(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateEMRFieldValue(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateStatusEMRFieldValue(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetUsedForValue(IValueObject valueObject, clsUserVO UserVo);
        

        //emr
        public abstract IValueObject GetPatientPrescriptionAndVisitDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPrescriptionAndVisitDetails_IPD(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientCurrentMedicationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPastMedicationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientCurrentMedicationDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientCurrentMedicationDetail(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientEMRDataDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPhysicalExamDetails(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject AddUpdateCompoundMedicationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientCompoundPrescription(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientCPOEDetail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientCPOEDetail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientCPOEPrescriptionDetail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemsNServiceBySelectedDiagnosis(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteCPOEService(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteCPOEMedicine(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateFollowUpDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientFollowUpDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPastChiefComplaints(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServicesCPOEDetail(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdatePatientHistoryData(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientHistoryDataAndDetail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientForIPDLAPAndHistro(IValueObject valueObject, clsUserVO UserVo);
        
        public abstract IValueObject GetPatientHistoryData(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdatePatientPhysicalExamDataAndDetail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientVisitSummaryDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetVisitAdmission(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetImage(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientAllVisit(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePatientIVFHistoryDataAndDetail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientIvfID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientEMRIvfHistoryDataDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientEMRPhysicalExamDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPastFollowUPNotes(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientPastcostNotes(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdatePatientOTDetail(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientFollowUpList(IValueObject valueObject, clsUserVO UserVo);     // To Get FollowUpList on Dashboard 08032017
    }
}
