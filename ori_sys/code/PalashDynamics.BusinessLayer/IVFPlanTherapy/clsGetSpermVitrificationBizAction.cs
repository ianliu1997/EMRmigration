using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.IVFPlanTherapy
{
    class clsGetSpermVitrificationBizAction:BizAction
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
        private clsGetSpermVitrificationBizAction()
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
        private static clsGetSpermVitrificationBizAction _Instance = null;
        
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetSpermVitrificationBizAction();
            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetSpermVitrificationBizActionVO obj = null;
            try
            {
                obj = (clsGetSpermVitrificationBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFPlanTherapyDAL objBaseDAL = clsBaseIVFPlanTherapyDAL.GetInstance();
                    obj = (clsGetSpermVitrificationBizActionVO)objBaseDAL.GetSpermVitrificationDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    }

    public class clsGetSpermThawingBizAction : BizAction
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
        private clsGetSpermThawingBizAction()
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
        private static clsGetSpermThawingBizAction _Instance = null;
        
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetSpermThawingBizAction();
            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetSpermThawingBizActionVO obj = null;
            try
            {
                obj = (clsGetSpermThawingBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFPlanTherapyDAL objBaseDAL = clsBaseIVFPlanTherapyDAL.GetInstance();
                    obj = (clsGetSpermThawingBizActionVO)objBaseDAL.GetSpermVitrificationDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    }

    public class clsAddUpdateSpermThawingBizAction : BizAction
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
        private clsAddUpdateSpermThawingBizAction()
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
        private static clsAddUpdateSpermThawingBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateSpermThawingBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateSpermThawingBizActionVO obj = null;

            try
            {
                obj = (clsAddUpdateSpermThawingBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIVFPlanTherapyDAL objBaseDAL = clsBaseIVFPlanTherapyDAL.GetInstance();
                    obj = (clsAddUpdateSpermThawingBizActionVO)objBaseDAL.AddUpdateSpermThawingDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
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
