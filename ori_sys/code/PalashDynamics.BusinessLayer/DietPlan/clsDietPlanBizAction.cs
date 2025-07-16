using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer
{
   class clsAddPatientDietPlanBizAction : BizAction
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
       private clsAddPatientDietPlanBizAction()
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
       private static clsAddPatientDietPlanBizAction _Instance = null;


       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsAddPatientDietPlanBizAction();

           return _Instance;
       }


       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsAddPatientDietPlanBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsAddPatientDietPlanBizActionVO)valueObject;
               //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
               //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                   obj = (clsAddPatientDietPlanBizActionVO)objBaseDAL.AddPatientDietPlan(obj, objUserVO);
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
               logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

   class clsGetPatientDietPlanBizAction : BizAction
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
       private clsGetPatientDietPlanBizAction()
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
       private static clsGetPatientDietPlanBizAction _Instance = null;


       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetPatientDietPlanBizAction();

           return _Instance;
       }


       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsGetPatientDietPlanBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetPatientDietPlanBizActionVO)valueObject;
               //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
               //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                   obj = (clsGetPatientDietPlanBizActionVO)objBaseDAL.GetPatientDietPlan(obj, objUserVO);
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
               logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

   class clsGetPatientDietPlanDetailsBizAction : BizAction
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
       private clsGetPatientDietPlanDetailsBizAction()
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
       private static clsGetPatientDietPlanDetailsBizAction _Instance = null;


       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetPatientDietPlanDetailsBizAction();

           return _Instance;
       }


       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsGetPatientDietPlanDetailsBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetPatientDietPlanDetailsBizActionVO)valueObject;
               //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
               //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                   obj = (clsGetPatientDietPlanDetailsBizActionVO)objBaseDAL.GetPatientDietPlanDetails(obj, objUserVO);
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
               logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

   class clsGeDietPlanMasterBizAction : BizAction
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
       private clsGeDietPlanMasterBizAction()
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
       private static clsGeDietPlanMasterBizAction _Instance = null;


       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGeDietPlanMasterBizAction();

           return _Instance;
       }


       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsGeDietPlanMasterBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGeDietPlanMasterBizActionVO)valueObject;
               //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
               //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                   obj = (clsGeDietPlanMasterBizActionVO)objBaseDAL.GetDietPlanMaster(obj, objUserVO);
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
               logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
