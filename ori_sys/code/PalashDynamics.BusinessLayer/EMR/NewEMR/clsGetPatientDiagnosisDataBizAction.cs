using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.ValueObjects.EMR;
namespace PalashDynamics.BusinessLayer
{
    public class clsGetPatientDiagnosisDataBizAction : BizAction
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
        private clsGetPatientDiagnosisDataBizAction()
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
        private static clsGetPatientDiagnosisDataBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientDiagnosisDataBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientDiagnosisDataBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientDiagnosisDataBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    if (obj.IsICDXhistory)
                    {
                        obj = (clsGetPatientDiagnosisDataBizActionVO)objBaseDAL.GetPatientEMRICDXDiagnosisList(obj, objUserVO);
                    }
                    else if (!obj.Ishistory)
                        obj = (clsGetPatientDiagnosisDataBizActionVO)objBaseDAL.GetPatientNewEMRDiagnosisList(obj, objUserVO);
                    else
                        obj = (clsGetPatientDiagnosisDataBizActionVO)objBaseDAL.GetPatientNewEMRDiagnosisListHistory(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
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

    public class clsGetPatientProcedureDataBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private clsGetPatientProcedureDataBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetPatientProcedureDataBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientProcedureDataBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientProcedureDataBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientProcedureDataBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();

                    obj = (clsGetPatientProcedureDataBizActionVO)objBaseDAL.GetPatientProcedureDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
          
            return obj;
        }
    }

}
