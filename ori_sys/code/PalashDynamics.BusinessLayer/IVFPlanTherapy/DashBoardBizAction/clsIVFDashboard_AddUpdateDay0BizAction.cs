using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;

namespace PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction
{
    class clsIVFDashboard_AddUpdateDay0BizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_AddUpdateDay0BizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIVFDashboard_AddUpdateDay0BizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_AddUpdateDay0BizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_AddUpdateDay0BizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_AddUpdateDay0BizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                    obj = (clsIVFDashboard_AddUpdateDay0BizActionVO)objBaseDAL.AddUpdateDay0Details(obj, objUserVO);
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

    class clsIVFDashboard_GetDay0DetailsBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_GetDay0DetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIVFDashboard_GetDay0DetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_GetDay0DetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetDay0DetailsBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_GetDay0DetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    if (obj.IsGetDate == true)
                    {
                        clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                        obj = (clsIVFDashboard_GetDay0DetailsBizActionVO)objBaseDAL.GetDate(obj, objUserVO);
                    }
                    else if (obj.IsSemenSample == true)
                    {
                        clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                        obj = (clsIVFDashboard_GetDay0DetailsBizActionVO)objBaseDAL.GetSemenSampleList(obj, objUserVO);
                    }
                    else
                    {
                        clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                        obj = (clsIVFDashboard_GetDay0DetailsBizActionVO)objBaseDAL.GetDay0Details(obj, objUserVO);
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

    
    
    class clsIVFDashboard_AddDay0OocyteListBizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_AddDay0OocyteListBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIVFDashboard_AddDay0OocyteListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_AddDay0OocyteListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_AddDay0OocyteListBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_AddDay0OocyteListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                    obj = (clsIVFDashboard_AddDay0OocyteListBizActionVO)objBaseDAL.AddDay0OocList(obj, objUserVO);
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

     class clsIVFDashboard_GetDay0OocyteListBizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private clsIVFDashboard_GetDay0OocyteListBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIVFDashboard_GetDay0OocyteListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIVFDashboard_GetDay0OocyteListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetDay0OocyteListBizActionVO obj = null;

            try
            {
                obj = (clsIVFDashboard_GetDay0OocyteListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                    obj = (clsIVFDashboard_GetDay0OocyteListBizActionVO)objBaseDAL.GetDay0OocList(obj, objUserVO);
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

     class clsIVFDashboard_GetDay0OocyteDetailsBizAction : BizAction
     {
         #region Variables Declaration
         //Declare the LogManager object
         LogManager logManager = null;
         //Declare the BaseOPDPatientMasterDAL object
         //Declare the Variable of UserId
         long lngUserId = 0;
         #endregion

         private clsIVFDashboard_GetDay0OocyteDetailsBizAction()
         {
             //Create Instance of the LogManager object 
             #region Logging Code
             if (logManager == null)
             {
                 logManager = LogManager.GetInstance();
             }
             #endregion
         }
         private static clsIVFDashboard_GetDay0OocyteDetailsBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsIVFDashboard_GetDay0OocyteDetailsBizAction();

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             clsIVFDashboard_GetDay0OocyteDetailsBizActionVO obj = null;

             try
             {
                 obj = (clsIVFDashboard_GetDay0OocyteDetailsBizActionVO)valueObject;
                 if (obj != null)
                 {
                     if (obj.IsOocyteRecipient)
                     {
                         clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                         obj = (clsIVFDashboard_GetDay0OocyteDetailsBizActionVO)objBaseDAL.GetDay0OocyteDetailsOocyteRecipient(obj, objUserVO);
                     }
                     else
                     {
                         clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                         obj = (clsIVFDashboard_GetDay0OocyteDetailsBizActionVO)objBaseDAL.GetDay0OocyteDetails(obj, objUserVO);
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

    //added by neena
     class clsIVFDashboard_AddUpdateFertCheckBizAction : BizAction
     {
         #region Variables Declaration
         //Declare the LogManager object
         LogManager logManager = null;
         //Declare the BaseOPDPatientMasterDAL object
         //Declare the Variable of UserId
         long lngUserId = 0;
         #endregion

         private clsIVFDashboard_AddUpdateFertCheckBizAction()
         {
             //Create Instance of the LogManager object 
             #region Logging Code
             if (logManager == null)
             {
                 logManager = LogManager.GetInstance();
             }
             #endregion
         }
         private static clsIVFDashboard_AddUpdateFertCheckBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsIVFDashboard_AddUpdateFertCheckBizAction();

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             clsIVFDashboard_AddUpdateFertCheckBizActionVO obj = null;

             try
             {
                 obj = (clsIVFDashboard_AddUpdateFertCheckBizActionVO)valueObject;
                 if (obj != null)
                 {
                     clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                     obj = (clsIVFDashboard_AddUpdateFertCheckBizActionVO)objBaseDAL.AddUpdateFertCheckDetails(obj, objUserVO);
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

     class clsIVFDashboard_GetFertCheckBizAction : BizAction
     {
         #region Variables Declaration
         //Declare the LogManager object
         LogManager logManager = null;
         //Declare the BaseOPDPatientMasterDAL object
         //Declare the Variable of UserId
         long lngUserId = 0;
         #endregion

         private clsIVFDashboard_GetFertCheckBizAction()
         {
             //Create Instance of the LogManager object 
             #region Logging Code
             if (logManager == null)
             {
                 logManager = LogManager.GetInstance();
             }
             #endregion
         }
         private static clsIVFDashboard_GetFertCheckBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsIVFDashboard_GetFertCheckBizAction();

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             clsIVFDashboard_GetFertCheckBizActionVO obj = null;

             try
             {
                 obj = (clsIVFDashboard_GetFertCheckBizActionVO)valueObject;
                 if (obj != null)
                 {
                     if (obj.IsGetDate)
                     {
                         clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                         obj = (clsIVFDashboard_GetFertCheckBizActionVO)objBaseDAL.GetFertCheckDate(obj, objUserVO);
                     }
                     else  if (obj.IsApply)
                     {
                         clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                         obj = (clsIVFDashboard_GetFertCheckBizActionVO)objBaseDAL.GetIVFICSIPlannedOocyteDetails(obj, objUserVO);
                     }
                     else
                     {
                         clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                         obj = (clsIVFDashboard_GetFertCheckBizActionVO)objBaseDAL.GetFertCheckDetails(obj, objUserVO);
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

     class clsIVFDashboard_DeleteAndGetLabDayImagesBizAction : BizAction
     {
         #region Variables Declaration
         //Declare the LogManager object
         LogManager logManager = null;
         //Declare the BaseOPDPatientMasterDAL object
         //Declare the Variable of UserId
         long lngUserId = 0;
         #endregion

         private clsIVFDashboard_DeleteAndGetLabDayImagesBizAction()
         {
             //Create Instance of the LogManager object 
             #region Logging Code
             if (logManager == null)
             {
                 logManager = LogManager.GetInstance();
             }
             #endregion
         }
         private static clsIVFDashboard_DeleteAndGetLabDayImagesBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsIVFDashboard_DeleteAndGetLabDayImagesBizAction();

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO obj = null;

             try
             {
                 obj = (clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)valueObject;
                 if (obj != null)
                 {
                     clsBaseIVFDashboad_LabDaysDAL objBaseDAL = clsBaseIVFDashboad_LabDaysDAL.GetInstance();
                     obj = (clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO)objBaseDAL.UpdateAndGetImageListDetails(obj, objUserVO);
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