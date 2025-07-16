using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Patient;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Patient;

namespace PalashDynamics.BusinessLayer.Patient
{
  public  class clsAddUpdateHSGBizAction : BizAction 
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion


        private clsAddUpdateHSGBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAddUpdateHSGBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateHSGBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddUpdateHSGBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUpdateHSGBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseHSGDAL objBaseDAL = clsBaseHSGDAL.GetInstance();
                    obj = (clsAddUpdateHSGBizActionVO)objBaseDAL.AddUpdateHSG(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw HEx;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

  public class clsGetHSGBizAction : BizAction
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
        private clsGetHSGBizAction()
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
        private static clsGetHSGBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetHSGBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetHSGBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetHSGBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseHSGDAL objBaseDAL = clsBaseHSGDAL.GetInstance();
                    obj = (clsGetHSGBizActionVO)objBaseDAL.GetHSGDetails(obj, objUserVO);
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
                //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // Guid a = new Guid();
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
