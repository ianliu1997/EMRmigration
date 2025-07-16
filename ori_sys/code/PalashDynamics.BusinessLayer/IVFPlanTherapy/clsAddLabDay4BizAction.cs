using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;

namespace PalashDynamics.BusinessLayer.IVFPlanTherapy
{
    internal class  clsAddLabDay4BizAction:BizAction
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
        private clsAddLabDay4BizAction()
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
        private static clsAddLabDay4BizAction _Instance = null;
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddLabDay4BizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddLabDay4BizActionVO obj = null;

            try
            {
                obj = (clsAddLabDay4BizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                   obj = (clsAddLabDay4BizActionVO)objBaseDAL.AddLabDay4(obj, objUserVO);
                    
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

    internal class  clsGetLabDay3ForLabDay4BizAction:BizAction
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
        private clsGetLabDay3ForLabDay4BizAction()
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
        private static clsGetLabDay3ForLabDay4BizAction _Instance = null;
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetLabDay3ForLabDay4BizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetLabDay3ForLabDay4BizActionVO obj = null;

            try
            {
                obj = (clsGetLabDay3ForLabDay4BizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                    obj = (clsGetLabDay3ForLabDay4BizActionVO)objBaseDAL.GetLabDay3ForDay4(obj, objUserVO);
                    
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

     internal class  clsGetDay4DetailsBizAction:BizAction
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
        private clsGetDay4DetailsBizAction()
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
        private static clsGetDay4DetailsBizAction _Instance = null;
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDay4DetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDay4DetailsBizActionVO obj = null;

            try
            {
                obj = (clsGetDay4DetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                    obj = (clsGetDay4DetailsBizActionVO)objBaseDAL.GetFemaleLabDay4(obj, objUserVO);
                    
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

     internal class clsGetDay4MediaAndCalcDetailsBizAction : BizAction
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
         private clsGetDay4MediaAndCalcDetailsBizAction()
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
         private static clsGetDay4MediaAndCalcDetailsBizAction _Instance = null;


         ///Name:GetInstance       
         ///Type:static
         ///Direction:None
         ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsGetDay4MediaAndCalcDetailsBizAction();

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {

             clsGetDay4MediaAndCalcDetailsBizActionVO obj = null;

             try
             {
                 obj = (clsGetDay4MediaAndCalcDetailsBizActionVO)valueObject;
                 if (obj != null)
                 {
                     clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                     obj = (clsGetDay4MediaAndCalcDetailsBizActionVO)objBaseDAL.GetFemaleLabDay4MediaAndCalDetails(obj, objUserVO);
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

    internal class clsGetDay3ScoreBizAction : BizAction
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
         private clsGetDay3ScoreBizAction()
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
         private static clsGetDay3ScoreBizAction _Instance = null;


         ///Name:GetInstance       
         ///Type:static
         ///Direction:None
         ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsGetDay3ScoreBizAction();

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {

             clsGetDay3ScoreBizActionVO obj = null;

             try
             {
                 obj = (clsGetDay3ScoreBizActionVO)valueObject;
                 if (obj != null)
                 {
                     if (obj.Day == 2)
                     {
                         clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                         obj = (clsGetDay3ScoreBizActionVO)objBaseDAL.GetLabDay3Score(obj, objUserVO);
                     }
                     if (obj.Day == 3)
                     {
                         clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                         obj = (clsGetDay3ScoreBizActionVO)objBaseDAL.GetLabDay2Score(obj, objUserVO);
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


    
}
