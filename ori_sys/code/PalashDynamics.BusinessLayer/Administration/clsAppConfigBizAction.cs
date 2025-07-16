using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer
{
   
    internal class clsGetAppConfigBizAction : BizAction 
    {
         //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServUserMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info
        private clsGetAppConfigBizAction()
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
        private static clsGetAppConfigBizAction _Instance = null;
        ///Method Input Users: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAppConfigBizAction();

            return _Instance;
        }

        ///Method Input Users: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
          clsGetAppConfigBizActionVO obj = null;
      
            try
            {
                obj = (clsGetAppConfigBizActionVO)valueObject;
             
                if (obj != null)
                {
                    clsBaseAppConfigDAL objBaseMenuDAL = clsBaseAppConfigDAL.GetInstance();
                    obj = (clsGetAppConfigBizActionVO)objBaseMenuDAL.GetAppConfig(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
   
                throw;
            }
            catch (Exception ex)
            {
           
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {
                //log error  
                //logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    
    }

    internal class clsUpdateAppConfigBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServUserMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info
        private clsUpdateAppConfigBizAction()
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
        private static clsUpdateAppConfigBizAction _Instance = null;
        ///Method Input Users: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateAppConfigBizAction();

            return _Instance;
        }

        ///Method Input Users: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateAppConfigBizActionVO obj = null;

            try
            {
                obj = (clsUpdateAppConfigBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseAppConfigDAL objBaseDAL = clsBaseAppConfigDAL.GetInstance();
                    obj = (clsUpdateAppConfigBizActionVO)objBaseDAL.UpdateAppConfig(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {
                //log error  
                //logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

    }


    internal class clsGetAutoEmailConfigBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServUserMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info
        private clsGetAutoEmailConfigBizAction()
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
        private static clsGetAutoEmailConfigBizAction _Instance = null;
        ///Method Input Users: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAutoEmailConfigBizAction();

            return _Instance;
        }

        ///Method Input Users: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetAutoEmailConfigBizActionVO obj = null;

            try
            {
                obj = (clsGetAutoEmailConfigBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseAppConfigDAL objBaseMenuDAL = clsBaseAppConfigDAL.GetInstance();
                    obj = (clsGetAutoEmailConfigBizActionVO)objBaseMenuDAL.GetAutoEmailConfig(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {
                //log error  
                //logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

    }

    internal class clsUpdateAutoEmailConfigBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServUserMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info
        private clsUpdateAutoEmailConfigBizAction()
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
        private static clsUpdateAutoEmailConfigBizAction _Instance = null;
        ///Method Input Users: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateAutoEmailConfigBizAction();

            return _Instance;
        }

        ///Method Input Users: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateAutoEmailConfigBizActionVO obj = null;

            try
            {
                obj = (clsUpdateAutoEmailConfigBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseAppConfigDAL objBaseDAL = clsBaseAppConfigDAL.GetInstance();
                    obj = (clsUpdateAutoEmailConfigBizActionVO)objBaseDAL.UpdateAutoEmailConfig(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {
                //log error  
                //logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

    }

    // Biz Action for Email CCTo......By BHUSHAN
    internal class clsAppEmailCCToBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServUserMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsAppEmailCCToBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAppEmailCCToBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAppEmailCCToBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAppEmailCCToBizActionVo obj = null;

            try
            {
                obj = (clsAppEmailCCToBizActionVo)valueObject;

                if (obj != null)
                {
                    clsBaseAppConfigDAL objBaseDAL = clsBaseAppConfigDAL.GetInstance();
                    obj = (clsAppEmailCCToBizActionVo)objBaseDAL.GetAutoEmailCCTOConfig(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {
                //log error  
                //logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    }

    // BizAction For Status.... By BHUSHAN
    internal class clsStatusEmailCCToBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServUserMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsStatusEmailCCToBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsStatusEmailCCToBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsStatusEmailCCToBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsStatusEmailCCToBizActionVo obj = null;

            try
            {
                obj = (clsStatusEmailCCToBizActionVo)valueObject;

                if (obj != null)
                {
                    clsBaseAppConfigDAL objBaseDAL = clsBaseAppConfigDAL.GetInstance();
                    obj = (clsStatusEmailCCToBizActionVo)objBaseDAL.SetStatusAutoEmailCCTO(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {
                //log error  
                //logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    }

    // BizAction For ADD Email-ID...... By BHUSHAN
    internal class clsAddEmailIDCCToBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServUserMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsAddEmailIDCCToBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAddEmailIDCCToBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddEmailIDCCToBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddEmailIDCCToBizActionVo obj = null;

            try
            {
                obj = (clsAddEmailIDCCToBizActionVo)valueObject;

                if (obj != null)
                {
                    clsBaseAppConfigDAL objBaseDAL = clsBaseAppConfigDAL.GetInstance();
                    obj = (clsAddEmailIDCCToBizActionVo)objBaseDAL.AddEmailIDCCTo(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {

            }
            return obj;
        }

    }
}
