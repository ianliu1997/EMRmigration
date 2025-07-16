using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.BusinessLayer.IVFPlanTherapy
{
    class clsAddUpdateDonorBizAction : BizAction
    {
      
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion


       
        private clsAddUpdateDonorBizAction()
        {
          
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsAddUpdateDonorBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateDonorBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateDonorBizActionVO obj = null;

            try
            {
                obj = (clsAddUpdateDonorBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                    obj = (clsAddUpdateDonorBizActionVO)objBaseDAL.AddUpdateDonorRegistrationDetails(obj, objUserVO);
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

    class clsGetDonorDetailsForIUIBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion



        private clsGetDonorDetailsForIUIBizAction()
        {

            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetDonorDetailsForIUIBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDonorDetailsForIUIBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDonorDetailsForIUIBizActionVO obj = null;

            try
            {
                obj = (clsGetDonorDetailsForIUIBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                    obj = (clsGetDonorDetailsForIUIBizActionVO)objBaseDAL.GetDonorDetailsForIUI(obj, objUserVO);
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
    
    class clsGetDonorDetailsAgainstSearchBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion



        private clsGetDonorDetailsAgainstSearchBizAction()
        {

            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetDonorDetailsAgainstSearchBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDonorDetailsAgainstSearchBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDonorDetailsAgainstSearchBizActionVO obj = null;

            try
            {
                obj = (clsGetDonorDetailsAgainstSearchBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                    obj = (clsGetDonorDetailsAgainstSearchBizActionVO)objBaseDAL.GetDonorDetailsAgainstSearch(obj, objUserVO);
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
    

     class clsGetDonorDetailsBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion



        private clsGetDonorDetailsBizAction()
        {

            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetDonorDetailsBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDonorDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDonorDetailsBizActionVO obj = null;

            try
            {
                obj = (clsGetDonorDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                    obj = (clsGetDonorDetailsBizActionVO)objBaseDAL.GetDonorDetailsToModify(obj, objUserVO);
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
    
     class clsGetDonorBatchDetailsBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion



        private clsGetDonorBatchDetailsBizAction()
        {

            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetDonorBatchDetailsBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDonorBatchDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDonorBatchDetailsBizActionVO obj = null;

            try
            {
                obj = (clsGetDonorBatchDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                    obj = (clsGetDonorBatchDetailsBizActionVO)objBaseDAL.GetDonorBatchDetails(obj, objUserVO);
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
    
     class clsCheckDuplicasyDonorCodeAndBLabBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion



        private clsCheckDuplicasyDonorCodeAndBLabBizAction()
        {

            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsCheckDuplicasyDonorCodeAndBLabBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsCheckDuplicasyDonorCodeAndBLabBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsCheckDuplicasyDonorCodeAndBLabBizActionVO obj = null;

            try
            {
                obj = (clsCheckDuplicasyDonorCodeAndBLabBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                    obj = (clsCheckDuplicasyDonorCodeAndBLabBizActionVO)objBaseDAL.CheckDuplicasyDonorCodeAndBLab(obj, objUserVO);
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
     class clsGetDonorListBizAction : BizAction
     {

         #region Variables Declaration
         LogManager logManager = null;
         long lngUserId = 0;
         #endregion



         private clsGetDonorListBizAction()
         {

             #region Logging Code
             if (logManager == null)
             {
                 logManager = LogManager.GetInstance();
             }
             #endregion
         }

         //The Private declaration
         private static clsGetDonorListBizAction _Instance = null;


         ///Name:GetInstance       
         ///Type:static
         ///Direction:None
         ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsGetDonorListBizAction();

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             clsGetDonorListBizActionVO obj = null;

             try
             {
                 obj = (clsGetDonorListBizActionVO)valueObject;
                 if (obj != null)
                 {
                     clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                     obj = (clsGetDonorListBizActionVO)objBaseDAL.GetDonorList(obj, objUserVO);
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
