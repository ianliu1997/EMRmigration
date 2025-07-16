using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{


    public abstract class clsBaseOtConfigDAL
    {
        static private clsBaseOtConfigDAL _instance = null;

        public static clsBaseOtConfigDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsOtConfigDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseOtConfigDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        /// <summary>
        /// Adds Ot Table Details
        /// </summary>
        /// <param name="valueObject">clsAddUpdateOTTableDetailsBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns></returns>
        public abstract IValueObject AddUpdateOtTableDetails(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Ot Table List
        /// </summary>
        /// <param name="valueObject">clsGetOTTableDetailsBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns></returns>
        public abstract IValueObject GetOtTableDetails(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Patient Related Fields
        /// </summary>
        /// <param name="valueObject">clsGetPatientConfigFieldsBizActionVO </param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsGetPatientConfigFieldsBizActionVO object</returns>
        public abstract IValueObject GetConfig_Patient_Fields(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Adds or Updates Consent Master Details
        /// </summary>
        /// <param name="valueObject">clsAddUpdateConsentMasterBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsAddUpdateConsentMasterBizActionVO object</returns>
        public abstract IValueObject AddUpdateConsentMaster(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Fills Consent Master List
        /// </summary>
        /// <param name="valueObject">clsGetConsentMasterBizActionVO</param>
        /// <param name="UserVo">clsUserVO</param>
        /// <returns>clsGetConsentMasterBizActionVO object</returns>
        public abstract IValueObject GetConsentMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        /// <summary>
        /// Gets Instruction Details
        /// </summary>
        public abstract IValueObject GetInstructionDetails(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// AddUpdateInstructionDetails
        /// </summary>

        public abstract IValueObject AddUpdateInstructionDetails(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Updates Instruction Status
        /// </summary>
        public abstract IValueObject UpdateInstructionStatus(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Instruction details by instruction ID
        /// </summary>
        public abstract IValueObject GetInstructionDetailsByID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Add Update Procedure Master
        /// </summary>
        public abstract IValueObject AddProcedureMaster(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Procedure Master
        /// </summary>
        public abstract IValueObject GetProcedureMaster(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// GetsProcedureDetails by procedure id
        /// </summary>
        public abstract IValueObject GetProcDetailsByProcID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets OT for selected procedures
        /// </summary>
        public abstract IValueObject GetOTForProcedure(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Doctor for Doctor Classification
        /// </summary>
        public abstract IValueObject GetDoctorForDocType(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Doctor for Doctor Specialization
        /// </summary>
        public abstract IValueObject GetDoctorListBySpecialization(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets staff by designation
        /// </summary>
        public abstract IValueObject GetStaffByDesignation(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Add Update Procedure Schedule Details
        /// </summary>
        public abstract IValueObject AddupdatePatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Patient Procedure Schedule 
        /// </summary>
        public abstract IValueObject GetPatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Patient Procedure Schedule 
        /// </summary>
        public abstract IValueObject GetProcScheduleDetailsByProcScheduleID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Check OT Schedule existance
        /// </summary>
        public abstract IValueObject CheckOTScheduleExistance(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Adds OT ScheduleMaster
        /// </summary>
        public abstract IValueObject AddOTScheduleMaster(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets OT ScheduleMaster
        /// </summary>
        public abstract IValueObject GetOTScheduleMasterList(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets OT ScheduleDetailMaster
        /// </summary>
        public abstract IValueObject GetOTScheduleDetailList(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets OT Schedule Time
        /// </summary>
        public abstract IValueObject GetOTScheduleTime(IValueObject valueObject, clsUserVO UserVo);

        //Get OT schedule irrespective of patinet procedure schedule id
        public abstract IValueObject GetProcOTSchedule(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets checklist by procedure id
        /// </summary>
        public abstract IValueObject GetCheckListByProcedureID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets checklist by scehduleid
        /// </summary>
        public abstract IValueObject GetCheckListByScheduleID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Doc schedule list
        /// </summary>
        public abstract IValueObject GetDocScheduleList(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Cancelles OT Booking
        /// </summary>
        public abstract IValueObject CancelOTBooking(IValueObject valueObject, clsUserVO UserVo);


        /// <summary>
        /// Gets staff List
        /// </summary>
        public abstract IValueObject GetStaffForOTScheduling(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets procedures for OT booking id
        /// </summary>
        public abstract IValueObject GetProceduresForOTBookingID(IValueObject valueObject, clsUserVO UserVo);


        /// <summary>
        /// Gets procedures for OT booking id
        /// </summary>
        public abstract IValueObject GetConsentForProcedureID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServicesForProcedure(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusProcedureCheckList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProcedureCheckList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateProcedureCheckList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusProcedureSubCategory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetProcedureSubCategoryList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateProcedureSubCategory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ProcedureUpdtStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject SaveConsentDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientDetailsForConsent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOTBookingByDateID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetConsentByConsentType(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientConsents(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorOrderForOTScheduling(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDeleteConsents(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientConsentsDetailsInHTML(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetEMRTemplateByProcID(IValueObject valueObject, clsUserVO UserVo);


        //static private clsBaseOtConfigDAL _instance = null;

        //public static clsBaseOtConfigDAL GetInstance()
        //{
        //    try
        //    {
        //        if (_instance == null)
        //        {
        //            //Get the full name of data access layer class from xml file which stores the list of classess.
        //            string _DerivedClassName = "clsOtConfigDAL";
        //            string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
        //            //Create instance of Database dependent dataaccesslayer class and type cast it base class.
        //            _instance = (clsBaseOtConfigDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return _instance;
        //}

        ///// <summary>
        ///// Adds Ot Table Details
        ///// </summary>
        ///// <param name="valueObject">clsAddUpdateOTTableDetailsBizActionVO</param>
        ///// <param name="UserVo">clsUserVO</param>
        ///// <returns></returns>
        //public abstract IValueObject AddUpdateOtTableDetails(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets Ot Table List
        ///// </summary>
        ///// <param name="valueObject">clsGetOTTableDetailsBizActionVO</param>
        ///// <param name="UserVo">clsUserVO</param>
        ///// <returns></returns>
        //public abstract IValueObject GetOtTableDetails(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets Patient Related Fields
        ///// </summary>
        ///// <param name="valueObject">clsGetPatientConfigFieldsBizActionVO </param>
        ///// <param name="UserVo">clsUserVO</param>
        ///// <returns>clsGetPatientConfigFieldsBizActionVO object</returns>
        //public abstract IValueObject GetConfig_Patient_Fields(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Adds or Updates Consent Master Details
        ///// </summary>
        ///// <param name="valueObject">clsAddUpdateConsentMasterBizActionVO</param>
        ///// <param name="UserVo">clsUserVO</param>
        ///// <returns>clsAddUpdateConsentMasterBizActionVO object</returns>
        //public abstract IValueObject AddUpdateConsentMaster(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Fills Consent Master List
        ///// </summary>
        ///// <param name="valueObject">clsGetConsentMasterBizActionVO</param>
        ///// <param name="UserVo">clsUserVO</param>
        ///// <returns>clsGetConsentMasterBizActionVO object</returns>
        //public abstract IValueObject GetConsentMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        ///// <summary>
        ///// Gets Instruction Details
        ///// </summary>
        //public abstract IValueObject GetInstructionDetails(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// AddUpdateInstructionDetails
        ///// </summary>

        //public abstract IValueObject AddUpdateInstructionDetails(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Updates Instruction Status
        ///// </summary>
        //public abstract IValueObject UpdateInstructionStatus(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets Instruction details by instruction ID
        ///// </summary>
        //public abstract IValueObject GetInstructionDetailsByID(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Add Update Procedure Master
        ///// </summary>
        //public abstract IValueObject AddProcedureMaster(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets Procedure Master
        ///// </summary>
        //public abstract IValueObject GetProcedureMaster(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// GetsProcedureDetails by procedure id
        ///// </summary>
        //public abstract IValueObject GetProcDetailsByProcID(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets OT for selected procedures
        ///// </summary>
        //public abstract IValueObject GetOTForProcedure(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets Doctor for Doctor Classification
        ///// </summary>
        //public abstract IValueObject GetDoctorForDocType(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets staff by designation
        ///// </summary>
        //public abstract IValueObject GetStaffByDesignation(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Add Update Procedure Schedule Details
        ///// </summary>
        //public abstract IValueObject AddupdatePatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets Patient Procedure Schedule 
        ///// </summary>
        //public abstract IValueObject GetPatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets Patient Procedure Schedule 
        ///// </summary>
        //public abstract IValueObject GetProcScheduleDetailsByProcScheduleID(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Check OT Schedule existance
        ///// </summary>
        //public abstract IValueObject CheckOTScheduleExistance(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Adds OT ScheduleMaster
        ///// </summary>
        //public abstract IValueObject AddOTScheduleMaster(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets OT ScheduleMaster
        ///// </summary>
        //public abstract IValueObject GetOTScheduleMasterList(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets OT ScheduleDetailMaster
        ///// </summary>
        //public abstract IValueObject GetOTScheduleDetailList(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Updates OT Schedule
        /// </summary>
        public abstract IValueObject UpdateOTSchedule(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets OT Schedule Time
        ///// </summary>
        //public abstract IValueObject GetOTScheduleTime(IValueObject valueObject, clsUserVO UserVo);

        ////Get OT schedule irrespective of patinet procedure schedule id
        //public abstract IValueObject GetProcOTSchedule(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets checklist by procedure id
        ///// </summary>
        //public abstract IValueObject GetCheckListByProcedureID(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets checklist by scehduleid
        ///// </summary>
        //public abstract IValueObject GetCheckListByScheduleID(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets Doc schedule list
        ///// </summary>
        //public abstract IValueObject GetDocScheduleList(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Cancelles OT Booking
        ///// </summary>
        //public abstract IValueObject CancelOTBooking(IValueObject valueObject, clsUserVO UserVo);


        ///// <summary>
        ///// Gets staff List
        ///// </summary>
        //public abstract IValueObject GetStaffForOTScheduling(IValueObject valueObject, clsUserVO UserVo);

        ///// <summary>
        ///// Gets procedures for OT booking id
        ///// </summary>
        //public abstract IValueObject GetProceduresForOTBookingID(IValueObject valueObject, clsUserVO UserVo);


        ///// <summary>
        ///// Gets procedures for OT booking id
        ///// </summary>
        //public abstract IValueObject GetConsentForProcedureID(IValueObject valueObject, clsUserVO UserVo);

        //# region For IPD Module

        //public abstract IValueObject GetPatientDetailsForConsent(IValueObject valueObject, clsUserVO UserVo);

        //public abstract IValueObject GetPatientConsents(IValueObject valueObject, clsUserVO UserVo);

        //public abstract IValueObject SaveConsentDetails(IValueObject valueObject, clsUserVO UserVo);

        //public abstract IValueObject GetConsentByConsentType(IValueObject valueObject, clsUserVO UserVo);

        //public abstract IValueObject GetPatientConsentsDetailsInHTML(IValueObject valueObject, clsUserVO UserVo);


        

        //#endregion

    }
}
