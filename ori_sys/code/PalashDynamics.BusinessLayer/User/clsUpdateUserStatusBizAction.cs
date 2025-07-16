using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.User;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using System.Reflection;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.ValueObjects.User;


namespace PalashDynamics.BusinessLayer.User
{
    internal class clsUpdateUserLockedStatusBizAction : BizAction   
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsUpdateUserLockedStatusBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsUpdateUserLockedStatusBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsUpdateUserLockedStatusBizAction();
            }
            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateUserLockedStatusBizActionVO obj = null;
            int ResultSet = 0;
            try
            {
                obj = (clsUpdateUserLockedStatusBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsUpdateUserLockedStatusBizActionVO)objBaseDAL.UpdateUserLockedStatus(obj, objUserVO);
                }
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

    public  class clsUpdateUserAuditTrailBizAction:BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsUpdateUserAuditTrailBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsUpdateUserAuditTrailBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsUpdateUserAuditTrailBizAction();
            }
            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateAuditTrailBizActionVO obj = null;
            int ResultSet = 0;
            try
            {
                 obj = (clsUpdateAuditTrailBizActionVO)valueObject;
                 if (obj != null)
                 { 
                     clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                     obj = (clsUpdateAuditTrailBizActionVO)objBaseDAL.UpdateUserAuditTrail(obj, objUserVO);
                 }
            }
            catch (Exception ex)
            {                
             //   throw;
            }
            return obj;
        }
    }
    
    internal class clsUpdateUserStatusBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsUpdateUserStatusBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsUpdateUserStatusBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsUpdateUserStatusBizAction();
            }
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateUserStatusBizActionVO obj = null;
            int ResultSet = 0;
            try
            {
                obj = (clsUpdateUserStatusBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsUpdateUserStatusBizActionVO)objBaseDAL.UpdateUserStatus(obj, objUserVO);

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

    internal class clsResetPasswordBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsResetPasswordBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsResetPasswordBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsResetPasswordBizAction();
            }
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsResetPasswordBizActionVO obj = null;
            int ResultSet = 0;
            try
            {
                obj = (clsResetPasswordBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsResetPasswordBizActionVO)objBaseDAL.ResetPassword(obj, objUserVO);

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

    public class UpdateUserOnClose
    {
        //The Private declaration
        private static UpdateUserOnClose _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static UpdateUserOnClose GetInstance()
        {
            if (_Instance == null)
                _Instance = new UpdateUserOnClose();

            return _Instance;
        }
        /// <summary>
        /// This method is override from BizAction Class. 
        /// It takes string LoginName, string Password as input Parameter and return clsUserVO.This method creates instance of UMDAL class and call the AuthenticateUser() method of UMDAL Class which execute and return the resultset.
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public clsUserVO UpdateUser(long AuditId)
        {
            clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
            return (clsUserVO)objBaseDAL.UpdateAuditOnClose(AuditId);
        }

    }
}
