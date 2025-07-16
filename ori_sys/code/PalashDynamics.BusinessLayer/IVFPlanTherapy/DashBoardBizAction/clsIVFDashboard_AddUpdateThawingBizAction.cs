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
using PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard;


namespace PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction
{
    class clsIVFDashboard_AddUpdateThawingBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_AddUpdateThawingBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsIVFDashboard_AddUpdateThawingBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_AddUpdateThawingBizAction();

            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_AddUpdateThawingBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_AddUpdateThawingBizActionVO)valueObject;
                if (obj != null)
                {
                    //if (obj.IsOnlyForEmbryoThawing == true)
                    if (obj.IsThawFreezeOocytes == true)
                    {
                        clsBaseIVFDashboard_ThawingDAL objBaseDAL = clsBaseIVFDashboard_ThawingDAL.GetInstance();
                        obj = (clsIVFDashboard_AddUpdateThawingBizActionVO)objBaseDAL.AddUpdateIVFDashBoard_ThawingOocyte(obj, objUserVO);
                    }
                    else
                    {
                        clsBaseIVFDashboard_ThawingDAL objBaseDAL = clsBaseIVFDashboard_ThawingDAL.GetInstance();
                        obj = (clsIVFDashboard_AddUpdateThawingBizActionVO)objBaseDAL.AddUpdateIVFDashBoard_Thawing(obj, objUserVO);
                    }
                    //else
                    //{
                    //    clsBaseIVFDashboard_ThawingDAL objBaseDAL = clsBaseIVFDashboard_ThawingDAL.GetInstance();
                    //    obj = (clsIVFDashboard_AddUpdateThawingBizActionVO)objBaseDAL.AddUpdateIVFDashBoard_ThawingForOocyte(obj, objUserVO);
                    //}
                }
                //if (obj != null)
                //{
                //    clsBaseIVFDashboard_ThawingDAL objBaseDAL = clsBaseIVFDashboard_ThawingDAL.GetInstance();
                //    obj = (clsIVFDashboard_AddUpdateThawingBizActionVO)objBaseDAL.AddUpdateIVFDashBoard_Thawing(obj, objUserVO);
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
     class clsIVFDashboard_GetThawingBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_GetThawingBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsIVFDashboard_GetThawingBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_GetThawingBizAction();

            return _Instance;
        }
        
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetThawingBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_GetThawingBizActionVO)valueObject;
                if (obj != null)
                {
                    //if (obj.IsOnlyForEmbryoThawing == true)
                    //{
                        clsBaseIVFDashboard_ThawingDAL objBaseDAL = clsBaseIVFDashboard_ThawingDAL.GetInstance();
                        obj = (clsIVFDashboard_GetThawingBizActionVO)objBaseDAL.GetIVFDashBoard_Thawing(obj, objUserVO);
                    //}
                    //else
                    //{

                    //    clsBaseIVFDashboard_ThawingDAL objBaseDAL = clsBaseIVFDashboard_ThawingDAL.GetInstance();
                    //    obj = (clsIVFDashboard_GetThawingBizActionVO)objBaseDAL.GetIVFDashBoard_ThawingForOocyte(obj, objUserVO);
                    //}
                }
                //if (obj != null)
                //{
                //    clsBaseIVFDashboard_ThawingDAL objBaseDAL = clsBaseIVFDashboard_ThawingDAL.GetInstance();
                //    obj = (clsIVFDashboard_GetThawingBizActionVO)objBaseDAL.GetIVFDashBoard_Thawing(obj, objUserVO);
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
    
    //added by neena
     class clsIVFDashboard_AddUpdateThawingWOCryoBizAction : BizAction
     {
         #region Variables Declaration
         //Declare the LogManager object
         LogManager logManager = null;
         //Declare the BaseOPDPatientMasterDAL object
         //Declare the Variable of UserId
         long lngUserId = 0;
         #endregion

         private clsIVFDashboard_AddUpdateThawingWOCryoBizAction()
         {
             //Create Instance of the LogManager object 
             #region Logging Code
             if (logManager == null)
             {
                 logManager = LogManager.GetInstance();
             }
             #endregion
         }

         private static clsIVFDashboard_AddUpdateThawingWOCryoBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsIVFDashboard_AddUpdateThawingWOCryoBizAction();

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO obj = null;

             try
             {
                 obj = (clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO)valueObject;
                 if (obj != null)
                 {
                     clsBaseIVFDashboard_ThawingDAL objBaseDAL = clsBaseIVFDashboard_ThawingDAL.GetInstance();
                     //if (obj.IsThawFreezeOocytes == true)
                     //{
                     //    obj = (clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO)objBaseDAL.AddUpdateIVFDashBoard_ThawingForFE_ICSI(obj, objUserVO);      //Flag use while save Freeze Oocytes under FE ICSI Cycle for Thaw 
                     //}
                     //else
                     //{
                     obj = (clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO)objBaseDAL.AddUpdateIVFDashBoard_ThawingWOCryo(obj, objUserVO);
                     //}
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
    //
}
