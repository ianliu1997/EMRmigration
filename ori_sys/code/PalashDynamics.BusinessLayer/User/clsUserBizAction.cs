using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.User;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer
{
    class clsAddUserBizAction : BizAction
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
        private clsAddUserBizAction()
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
        private static clsAddUserBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUserBizAction();

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
            clsAddUserBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUserBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsAddUserBizActionVO)objBaseDAL.AddUser(obj, objUserVO);
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

    class clsSecretQtnBizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsSecretQtnBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsSecretQtnBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsSecretQtnBizAction();

            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsSecretQtnBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsSecretQtnBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsSecretQtnBizActionVO)objBaseDAL.GetSecretQtn(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {                
             //   throw;
            }
            return obj;
        }
    }

    class clsGetUnitStoreStatusListBizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetUnitStoreStatusListBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetUnitStoreStatusListBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUnitStoreStatusListBizAction();

            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetUnitStoreStatusBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetUnitStoreStatusBizActionVO)valueObject;
                 if (obj != null)
                 {
                     clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                     obj = (clsGetUnitStoreStatusBizActionVO)objBaseDAL.GetUnitStoreStatusList(obj, objUserVO);
                     //obj = (clsGetUnitStoreBizActionVO)objBaseDAL.GetUnitStoreList(obj, objUserVO);
                 }
            }
            catch (Exception ex)
            {                
             //   throw;
            }

            return obj;
        }
    }

    class clsGetUnitStoreListBizAction:BizAction
    {       
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetUnitStoreListBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetUnitStoreListBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUnitStoreListBizAction();

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
            clsGetUnitStoreBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetUnitStoreBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetUnitStoreBizActionVO)objBaseDAL.GetUnitStoreList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {                
                //throw;
            }

            return obj;
        }
    }

    class clsGetUserListBizAction : BizAction
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
        private clsGetUserListBizAction()
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
        private static clsGetUserListBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUserListBizAction();

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
            clsGetUserListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetUserListBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetUserListBizActionVO)objBaseDAL.GetUserList(obj, objUserVO);
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

    class clsGetLoginNameBizAction:BizAction 
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
        public clsGetLoginNameBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration.
        private static clsGetLoginNameBizAction _Instance = null;
        
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetLoginNameBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetLoginNameBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetLoginNameBizActionVO)valueObject;
                if (obj !=null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetLoginNameBizActionVO)objBaseDAL.CheckUserLoginExists(obj, objUserVO);                
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
    class clsGetExistingUserNameBizAction:BizAction
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
        public clsGetExistingUserNameBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        
        // The Private Declaration.
        private static clsGetExistingUserNameBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetExistingUserNameBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetExistingUserNameBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetExistingUserNameBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetExistingUserNameBizActionVO)objBaseDAL.CheckUserExists(obj, objUserVO);                
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

    //class clsGetSecretQuestionBizAction : BizAction
    //{
    //    //This Region Contains Variables Which are Used At Form Level
    //    #region Variables Declaration
    //    //Declare the LogManager object
    //    LogManager logManager = null;
    //    //Declare the BaseOPDPatientMasterDAL object
    //    //Declare the Variable of UserId
    //    long lngUserId = 0;
    //    #endregion

    //    public clsGetSecretQuestionBizAction()
    //    {
    //        //Create Instance of the LogManager object 
    //        #region Logging Code
    //        if (logManager == null)
    //        {
    //            logManager = LogManager.GetInstance();
    //        }
    //        #endregion
    //    }

    //    //The Private declaration
    //    private static clsGetSecretQuestionBizAction _Instance = null;

    //    public static BizAction GetInstance()
    //    {
    //        if (_Instance == null)
    //            _Instance = new clsGetSecretQuestionBizAction();

    //        return _Instance;
    //    }


    //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
    //    {
    //        bool CurrentMethodExecutionStatus = true;
    //        clsGetSecretQuestionBizActionVO obj = null;
    //        int ResultStatus = 0;
    //        try
    //        {
    //             obj = (clsGetSecretQuestionBizActionVO)valueObject;
    //             //Typecast the "valueObject" to "clsInputOutputVO"
    //             if (obj != null)
    //             {
    //                  clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
    //                  obj = (clsGetSecretQuestionBizActionVO)objBaseDAL.ForgotPassword(obj, objUserVO);
    //             }

    //        }
    //        catch (HmsApplicationException HEx)
    //        {
    //            CurrentMethodExecutionStatus = false;
    //            throw;
    //        }
    //        catch (Exception ex)
    //        {
    //            CurrentMethodExecutionStatus = false;
    //            //log error  
    //            logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
    //        }
    //        finally
    //        {
    //            //log error  
    //            // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
    //        }
    //        return obj;
    //    }
    //}

    class clsGetUserBizAction : BizAction
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
        private clsGetUserBizAction()
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
        private static clsGetUserBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUserBizAction();

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
            clsGetUserBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetUserBizActionVO)valueObject;
                 //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetUserBizActionVO)objBaseDAL.GetUser(obj, objUserVO);
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

    class clsAssignUserEMRTemplatesBizAction : BizAction
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
        private clsAssignUserEMRTemplatesBizAction()
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
        private static clsAssignUserEMRTemplatesBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAssignUserEMRTemplatesBizAction();

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
            clsAssignUserEMRTemplatesBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAssignUserEMRTemplatesBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsAssignUserEMRTemplatesBizActionVO)objBaseDAL.AssignUserEMRTemplate(obj, objUserVO);
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

    class clsGetUserEMRTemplateListBizAction : BizAction
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
        private clsGetUserEMRTemplateListBizAction()
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
        private static clsGetUserEMRTemplateListBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUserEMRTemplateListBizAction();

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
            clsGetUserEMRTemplateListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetUserEMRTemplateListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetUserEMRTemplateListBizActionVO)objBaseDAL.GetUserEMRTemplateList(obj, objUserVO);
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
