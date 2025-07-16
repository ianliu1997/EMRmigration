using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;


namespace PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction
{
    class clsIVFDashboard_AddUpdateVitrificationBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_AddUpdateVitrificationBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIVFDashboard_AddUpdateVitrificationBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_AddUpdateVitrificationBizAction();

            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_AddUpdateVitrificationBizActionVO)valueObject;
                if (obj != null)
                {
                    if (obj.IsRenewal == true)
                    {
                        clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                        obj = (clsIVFDashboard_AddUpdateVitrificationBizActionVO)objBaseDAL.AddUpdateIVFDashBoard_RenewalDate(obj, objUserVO);
                    }
                    else if (obj.VitrificationMain.SaveForSingleEntry == true )
                    {
                        clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                        obj = (clsIVFDashboard_AddUpdateVitrificationBizActionVO)objBaseDAL.AddUpdateIVFDashBoard_VitrificationSingle(obj, objUserVO);
                    }
                    else
                    {
                        clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                        obj = (clsIVFDashboard_AddUpdateVitrificationBizActionVO)objBaseDAL.AddUpdateIVFDashBoard_Vitrification(obj, objUserVO);
                    }
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
     class clsIVFDashboard_GetVitrificationBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_GetVitrificationBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIVFDashboard_GetVitrificationBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_GetVitrificationBizAction();

            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetVitrificationBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_GetVitrificationBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                    obj = (clsIVFDashboard_GetVitrificationBizActionVO)objBaseDAL.GetIVFDashBoard_Vitrification(obj, objUserVO);
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
    class clsIVFDashboard_GetPreviousVitrificationBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_GetPreviousVitrificationBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIVFDashboard_GetPreviousVitrificationBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_GetPreviousVitrificationBizAction();

            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetPreviousVitrificationBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_GetPreviousVitrificationBizActionVO)valueObject;
                if (obj != null)
                {
                    //if (obj.VitrificationMain.IsOnlyForEmbryoVitrification == true)
                    //{
                        clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                        obj = (clsIVFDashboard_GetPreviousVitrificationBizActionVO)objBaseDAL.GetIVFDashBoard_PreviousEmbFromVitrification(obj, objUserVO);
                    //}
                    //else if (obj.VitrificationMainForOocyte.IsOnlyForEmbryoVitrification == true)
                    //{
                    //    clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                    //    obj = (clsIVFDashboard_GetPreviousVitrificationBizActionVO)objBaseDAL.GetIVFDashBoard_PreviousOocyteFromVitrification(obj, objUserVO);
                    //}
                }

                //obj = (clsIVFDashboard_GetPreviousVitrificationBizActionVO)valueObject;
                //if (obj != null)
                //{
                //    clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                //    obj = (clsIVFDashboard_GetPreviousVitrificationBizActionVO)objBaseDAL.GetIVFDashBoard_PreviousEmbFromVitrification(obj, objUserVO);
                //}
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
    
     class clsIVFDashboard_UpdateVitrificationDetailsBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_UpdateVitrificationDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIVFDashboard_UpdateVitrificationDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_UpdateVitrificationDetailsBizAction();

            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_UpdateVitrificationDetailsBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_UpdateVitrificationDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                    obj = (clsIVFDashboard_UpdateVitrificationDetailsBizActionVO)objBaseDAL.UpdateVitrificationDetails(obj, objUserVO);
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
    
      class clsIVFDashboard_GetUsedEmbryoDetailsBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_GetUsedEmbryoDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIVFDashboard_GetUsedEmbryoDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_GetUsedEmbryoDetailsBizAction();

            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                    obj = (clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO)objBaseDAL.GetUsedEmbryoDetails(obj, objUserVO);
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
