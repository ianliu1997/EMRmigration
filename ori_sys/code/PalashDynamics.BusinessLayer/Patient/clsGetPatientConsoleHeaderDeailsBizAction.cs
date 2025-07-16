//Created Date:23/Oct/2013
//Created By: Nilesh Raut
//Specification: Biz action VO For Display  Patient Details on Console

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

namespace PalashDynamics.BusinessLayer
{
    class clsGetPatientConsoleHeaderDeailsBizAction : BizAction
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
        private clsGetPatientConsoleHeaderDeailsBizAction()
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
        private static clsGetPatientConsoleHeaderDeailsBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientConsoleHeaderDeailsBizAction();

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
            clsGetPatientConsoleHeaderDeailsBizActionVO obj = null;
            //int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientConsoleHeaderDeailsBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                    obj = (clsGetPatientConsoleHeaderDeailsBizActionVO)objBaseDAL.GetPatientHeaderDetailsForConsole(obj, objUserVO);
                }
            }
            catch (HmsApplicationException)
            {
                //CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception)
            {
                // CurrentMethodExecutionStatus = false;
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

    class clsSavePhotoBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private clsSavePhotoBizAction()
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
        private static clsSavePhotoBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsSavePhotoBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsSavePhotoBizActionVO obj = null;
            //int ResultStatus = 0;
            try
            {
                obj = (clsSavePhotoBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                    obj = (clsSavePhotoBizActionVO)objBaseDAL.SavePatientPhoto(obj, objUserVO);
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

    internal class clsGetEMRAdmVisitListByPatientIDBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        public clsGetEMRAdmVisitListByPatientIDBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetEMRAdmVisitListByPatientIDBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetEMRAdmVisitListByPatientIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetEMRAdmVisitListByPatientIDBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetEMRAdmVisitListByPatientIDBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                    if (obj.IsForNursingConsol == false)
                        obj = (clsGetEMRAdmVisitListByPatientIDBizActionVO)objBaseDAL.GetEMRAdmVisitListByPatientID(obj, objUserVO);
                    else
                        obj = (clsGetEMRAdmVisitListByPatientIDBizActionVO)objBaseDAL.GetEMRAdmVisitListByPatientIDForConsol(obj, objUserVO);

                }
            }
            catch (HmsApplicationException HEx)
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }
}

