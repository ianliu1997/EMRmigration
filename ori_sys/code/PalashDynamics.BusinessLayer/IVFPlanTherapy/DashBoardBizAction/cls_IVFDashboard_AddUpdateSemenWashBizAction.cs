using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction
{
    class cls_IVFDashboard_AddUpdateSemenWashBizAction: BizAction
    {
         #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private cls_IVFDashboard_AddUpdateSemenWashBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static cls_IVFDashboard_AddUpdateSemenWashBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_IVFDashboard_AddUpdateSemenWashBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_IVFDashboard_AddUpdateSemenWashBizActionVO obj = null;

            try
            {
                obj = (cls_IVFDashboard_AddUpdateSemenWashBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_SemenDAL objBaseDAL = clsBaseIVFDashboard_SemenDAL.GetInstance();
                    obj = (cls_IVFDashboard_AddUpdateSemenWashBizActionVO)objBaseDAL.AddUpdateSemenWashDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    class cls_IVFDashboard_AddUpdateSemenUsedBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private cls_IVFDashboard_AddUpdateSemenUsedBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static cls_IVFDashboard_AddUpdateSemenUsedBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_IVFDashboard_AddUpdateSemenUsedBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_IVFDashboard_AddUpdateSemenUsedBizActionVO obj = null;

            try
            {
                obj = (cls_IVFDashboard_AddUpdateSemenUsedBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_SemenDAL objBaseDAL = clsBaseIVFDashboard_SemenDAL.GetInstance();
                    obj = (cls_IVFDashboard_AddUpdateSemenUsedBizActionVO)objBaseDAL.AddUpdateSemenUsedDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    class cls_GetIVFDashboard_SemenWashBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private cls_GetIVFDashboard_SemenWashBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static cls_GetIVFDashboard_SemenWashBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_GetIVFDashboard_SemenWashBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_GetIVFDashboard_SemenWashBizActionVO obj = null;

            try
            {
                obj = (cls_GetIVFDashboard_SemenWashBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_SemenDAL objBaseDAL = clsBaseIVFDashboard_SemenDAL.GetInstance();
                    obj = (cls_GetIVFDashboard_SemenWashBizActionVO)objBaseDAL.GetSemenWashDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

   
    class cls_GetIVFDashboard_SemenDetailsBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private cls_GetIVFDashboard_SemenDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static cls_GetIVFDashboard_SemenDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_GetIVFDashboard_SemenDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_GetIVFDashboard_SemenDetailsBizActionVO obj = null;

            try
            {
                obj = (cls_GetIVFDashboard_SemenDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_SemenDAL objBaseDAL = clsBaseIVFDashboard_SemenDAL.GetInstance();
                    obj = (cls_GetIVFDashboard_SemenDetailsBizActionVO)objBaseDAL.GetSemenDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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


    class cls_GetIVFDashboard_SemenUsedBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private cls_GetIVFDashboard_SemenUsedBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static cls_GetIVFDashboard_SemenUsedBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_GetIVFDashboard_SemenUsedBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_GetIVFDashboard_SemenUsedBizActionVO obj = null;

            try
            {
                obj = (cls_GetIVFDashboard_SemenUsedBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_SemenDAL objBaseDAL = clsBaseIVFDashboard_SemenDAL.GetInstance();
                    obj = (cls_GetIVFDashboard_SemenUsedBizActionVO)objBaseDAL.GetSemenUsedDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
    
     class cls_GetIVFDashboard_NewIUIDetailsBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private cls_GetIVFDashboard_NewIUIDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static cls_GetIVFDashboard_NewIUIDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_GetIVFDashboard_NewIUIDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_GetIVFDashboard_NewIUIDetailsBizActionVO obj = null;

            try
            {
                obj = (cls_GetIVFDashboard_NewIUIDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_SemenDAL objBaseDAL = clsBaseIVFDashboard_SemenDAL.GetInstance();
                    obj = (cls_GetIVFDashboard_NewIUIDetailsBizActionVO)objBaseDAL.GetNewIUIDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
