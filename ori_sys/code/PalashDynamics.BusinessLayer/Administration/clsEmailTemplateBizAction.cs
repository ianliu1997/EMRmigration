using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.Administration
{
    class clsEmailTemplateBizAction
    {
    }

    class clsEmailTemplateListBizAction:BizAction
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
        public clsEmailTemplateListBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsEmailTemplateListBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsEmailTemplateListBizAction();

            return _Instance;
        }
        
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetEmailTemplateListBizActionVO obj = null;
            int Resultset = 0;

            try
            {
                obj = (clsGetEmailTemplateListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetEmailTemplateListBizActionVO)objDAL.GetEmailTemplateList(obj, objUserVO);
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

    class clsAddEmailTemplateBizAction :BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsAddEmailTemplateBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAddEmailTemplateBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsAddEmailTemplateBizAction();
            }

            return _Instance;

        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddEmailTemplateBizActionVO obj = null;
            int Resultset = 0;

            try
            {
                obj = (clsAddEmailTemplateBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                    obj=((clsAddEmailTemplateBizActionVO)objBaseDAL.AddEmailTemplate(obj, objUserVO));
                }
            }
             catch (HmsApplicationException Hex)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //log error  
                logmanager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }
    }

    class clsGetEmailTemplateDetailsBizAction:BizAction 
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
        public clsGetEmailTemplateDetailsBizAction()
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
        private static clsGetEmailTemplateDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetEmailTemplateDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetEmailTemplateBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetEmailTemplateBizActionVO)valueObject;
                if (obj != null)
                {
                     clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                    obj=((clsGetEmailTemplateBizActionVO)objBaseDAL.GetEmailTemplateDetails(obj, objUserVO));
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

    class clsUpdateEmailTemplateStatusBizAction : BizAction 
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsUpdateEmailTemplateStatusBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsUpdateEmailTemplateStatusBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsUpdateEmailTemplateStatusBizAction();
            }
            return _Instance;
        }



        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateEmailTemplateStatusBizActionVO obj = null;
            int ResultSet = 0;

            try
            {
                obj = (clsUpdateEmailTemplateStatusBizActionVO)valueObject;
                if (obj != null)
                {
                     clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                     obj = (clsUpdateEmailTemplateStatusBizActionVO)objBaseDAL.UpdateEmailTemplateStatus(obj, objUserVO);
                }

            }
            catch (HmsApplicationException Hex)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //log error  
                logmanager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }
    }
}
