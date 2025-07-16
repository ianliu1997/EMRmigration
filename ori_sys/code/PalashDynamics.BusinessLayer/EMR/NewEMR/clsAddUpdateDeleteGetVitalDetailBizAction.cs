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
using PalashDynamics.ValueObjects.EMR.NewEMR;

namespace PalashDynamics.BusinessLayer
{
     public class clsAddUpdateDeleteVitalDetailsBizAction : BizAction
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
        private clsAddUpdateDeleteVitalDetailsBizAction()
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
        private static clsAddUpdateDeleteVitalDetailsBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateDeleteVitalDetailsBizAction();

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
            clsAddUpdateDeleteVitalDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUpdateDeleteVitalDetailsBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    obj = (clsAddUpdateDeleteVitalDetailsBizActionVO)objBaseDAL.AddUpdateDeleteVitalDetails(obj, objUserVO);
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

     public class clsGetVitalListDetailsBizAction : BizAction
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
        private clsGetVitalListDetailsBizAction()
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
        private static clsGetVitalListDetailsBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetVitalListDetailsBizAction();

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
            clsGetVitalListDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetVitalListDetailsBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    obj = (clsGetVitalListDetailsBizActionVO)objBaseDAL.GetVitalListDetails(obj, objUserVO);
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


     class clsGetPatientVitalChartBizAction : BizAction
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
         private clsGetPatientVitalChartBizAction()
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
         private static clsGetPatientVitalChartBizAction _Instance = null;

         ///Method Input OPDPatient: none
         ///Name:GetInstance       
         ///Type:static
         ///Direction:None
         ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsGetPatientVitalChartBizAction();

             return _Instance;
         }

         ///Method Input OPDPatient: valueObject
         ///Name                   :ProcessRequest    
         ///Type                   :IValueObject
         ///Direction              :input-IvalueObject output-IvalueObject
         ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             // bool CurrentMethodExecutionStatus = true;
             clsGetPatientVitalChartBizActionVO obj = null;
             //int ResultStatus = 0;
             try
             {
                 obj = (clsGetPatientVitalChartBizActionVO)valueObject;
                 //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                 //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                 //Typecast the "valueObject" to "clsInputOutputVO"
                 if (obj != null)
                 {
                     clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                     obj = (clsGetPatientVitalChartBizActionVO)objBaseDAL.GetPatientPatientVitalChartList(obj, objUserVO);
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
