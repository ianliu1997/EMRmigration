//Created Date:26/July/2013
//Created By: Nilesh Raut
//Specification: BizAction For Get Patient Prescription and Visit Details

//Review By:
//Review Date:

//Modified By:
//Modified Date: 

using System;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.BusinessLayer.EMR
{
    class clsGetPatientPrescriptionandVisitDetailsBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info
        private clsGetPatientPrescriptionandVisitDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetPatientPrescriptionandVisitDetailsBizAction _Instance = null;
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientPrescriptionandVisitDetailsBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            //bool CurrentMethodExecutionStatus = true;
            clsGetPatientPrescriptionandVisitDetailsBizActionVO obj = null;
            //int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientPrescriptionandVisitDetailsBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseDAL = clsBasePatientEMRDataDAL.GetInstance();
                    if (obj.ISForPrint == true)
                    {
                        obj = (clsGetPatientPrescriptionandVisitDetailsBizActionVO)objBaseDAL.GetPatientPrescriptionAndVisitDetails_IPD(obj, objUserVO);
                    }
                    else
                    {
                        obj = (clsGetPatientPrescriptionandVisitDetailsBizActionVO)objBaseDAL.GetPatientPrescriptionAndVisitDetails(obj, objUserVO);
                    }
                }
            }
            catch (HmsApplicationException)
            {
                //CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception)
            {
                //CurrentMethodExecutionStatus = false;
                //log error  
                //logManager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    }

    class clsGetPatientCurrentMedicationDetailsBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info
        private clsGetPatientCurrentMedicationDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetPatientCurrentMedicationDetailsBizAction _Instance = null;
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientCurrentMedicationDetailsBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            //bool CurrentMethodExecutionStatus = true;
            clsGetPatientCurrentMedicationDetailsBizActionVO obj = null;
            //int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientCurrentMedicationDetailsBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseDAL = clsBasePatientEMRDataDAL.GetInstance();
                    obj = (clsGetPatientCurrentMedicationDetailsBizActionVO)objBaseDAL.GetPatientCurrentMedicationDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException)
            {
                //CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception)
            {
                //CurrentMethodExecutionStatus = false;
                //log error  
                //logManager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    }

    public class clsGetPatientPastMedicationDetailsBizAction : BizAction
    {
        private static clsGetPatientPastMedicationDetailsBizAction _Instance = null;

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private clsGetPatientPastMedicationDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientPastMedicationDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientPastMedicationDetailsBizActionVO obj = null;
            try
            {
                obj = (clsGetPatientPastMedicationDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseDAL = clsBasePatientEMRDataDAL.GetInstance();
                    obj = (clsGetPatientPastMedicationDetailsBizActionVO)objBaseDAL.GetPatientPastMedicationDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return obj;
        }
    }

    class clsGetPatientCurrentMedicationDetailListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info
        private clsGetPatientCurrentMedicationDetailListBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetPatientCurrentMedicationDetailListBizAction _Instance = null;
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientCurrentMedicationDetailListBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            //bool CurrentMethodExecutionStatus = true;
            clsGetPatientCurrentMedicationDetailListBizActionVO obj = null;
            //int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientCurrentMedicationDetailListBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseDAL = clsBasePatientEMRDataDAL.GetInstance();
                    obj = (clsGetPatientCurrentMedicationDetailListBizActionVO)objBaseDAL.GetPatientCurrentMedicationDetailList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException)
            {
                //CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception)
            {
                //CurrentMethodExecutionStatus = false;
                //log error  
                //logManager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    }
}
