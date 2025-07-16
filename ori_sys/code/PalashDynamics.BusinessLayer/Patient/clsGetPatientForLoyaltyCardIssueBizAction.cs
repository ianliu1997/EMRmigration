using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;

namespace PalashDynamics.BusinessLayer
{
   internal class clsGetPatientForLoyaltyCardIssueBizAction:BizAction
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
        private clsGetPatientForLoyaltyCardIssueBizAction()
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
        private static clsGetPatientForLoyaltyCardIssueBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and This will Give Unique Instance
       public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientForLoyaltyCardIssueBizAction();

            return _Instance;
        }

       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)

        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientForLoyaltyCardIssueBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientForLoyaltyCardIssueBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientDAL objBaseDAL = clsBasePatientDAL.GetInstance();
                    obj = (clsGetPatientForLoyaltyCardIssueBizActionVO)objBaseDAL.GetPatientForLoyaltyCardIssue(obj, objUserVO);
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
