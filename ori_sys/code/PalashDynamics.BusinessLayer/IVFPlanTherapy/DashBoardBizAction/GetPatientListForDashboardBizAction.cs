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
    class GetPatientListForDashboardBizAction: BizAction
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
        private GetPatientListForDashboardBizAction()
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
        private static GetPatientListForDashboardBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new GetPatientListForDashboardBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            GetPatientListForDashboardBizActionVO obj = null;

            try
            {
                obj = (GetPatientListForDashboardBizActionVO)valueObject;
                if (obj != null)
                {
                    if (obj.IsSurrogacy)
                    {
                        clsBaseIVFDashboad_PlanTherapyDAL objBaseDAL = clsBaseIVFDashboad_PlanTherapyDAL.GetInstance();
                        obj = (GetPatientListForDashboardBizActionVO)objBaseDAL.GetSurrogatePatientListForDashboard(obj, objUserVO);
                    }
                    else if (obj.IsSurrogacyForTransfer)
                    {
                        clsBaseIVFDashboad_PlanTherapyDAL objBaseDAL = clsBaseIVFDashboad_PlanTherapyDAL.GetInstance();
                        obj = (GetPatientListForDashboardBizActionVO)objBaseDAL.GetSurrogatePatientListForTransfer(obj, objUserVO);
                    }
                    else
                    {
                        clsBaseIVFDashboad_PlanTherapyDAL objBaseDAL = clsBaseIVFDashboad_PlanTherapyDAL.GetInstance();
                        obj = (GetPatientListForDashboardBizActionVO)objBaseDAL.GetPatientListForDashboard(obj, objUserVO);
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

    
    class GetSearchkeywordForPatientBizAction: BizAction
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
        private GetSearchkeywordForPatientBizAction()
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
        private static GetSearchkeywordForPatientBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new GetSearchkeywordForPatientBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            GetSearchkeywordForPatientBizActionVO obj = null;

            try
            {
                obj = (GetSearchkeywordForPatientBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboad_PlanTherapyDAL objBaseDAL = clsBaseIVFDashboad_PlanTherapyDAL.GetInstance();
                    obj = (GetSearchkeywordForPatientBizActionVO)objBaseDAL.GetSearchKeywordforPatient(obj, objUserVO);
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
    //class GetSurrogatePatientListForDashboardBizAction : BizAction
    //{

    //    //This Region Contains Variables Which are Used At Form Level
    //    #region Variables Declaration
    //    //Declare the LogManager object
    //    LogManager logManager = null;
    //    //Declare the BaseOPDPatientMasterDAL object
    //    //Declare the Variable of UserId
    //    long lngUserId = 0;
    //    #endregion


    //    //constructor For Log Error Info
    //    private GetSurrogatePatientListForDashboardBizAction()
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
    //    private static GetSurrogatePatientListForDashboardBizAction _Instance = null;


    //    ///Name:GetInstance       
    //    ///Type:static
    //    ///Direction:None
    //    ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
    //    public static BizAction GetInstance()
    //    {
    //        if (_Instance == null)
    //            _Instance = new GetSurrogatePatientListForDashboardBizAction();

    //        return _Instance;
    //    }

    //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
    //    {

    //        GetSurrogatePatientListForDashboardBizActionVO obj = null;

    //        try
    //        {
    //            obj = (GetSurrogatePatientListForDashboardBizActionVO)valueObject;
    //            if (obj != null)
    //            {
    //                clsBaseIVFDashboad_PlanTherapyDAL objBaseDAL = clsBaseIVFDashboad_PlanTherapyDAL.GetInstance();
    //                obj = (GetSurrogatePatientListForDashboardBizActionVO)objBaseDAL.GetSurrogatePatientListForDashboard(obj, objUserVO);
    //            }
    //        }
    //        catch (HmsApplicationException HEx)
    //        {
    //            throw;
    //        }
    //        catch (Exception ex)
    //        {

    //            //log error  
    //            logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
    //
}
