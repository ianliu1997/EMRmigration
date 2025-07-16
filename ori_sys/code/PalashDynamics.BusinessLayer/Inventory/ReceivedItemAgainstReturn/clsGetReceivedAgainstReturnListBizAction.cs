using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.BusinessLayer.Inventory
{
    
   public class clsGetReceivedAgainstReturnListBizAction :BizAction
    {
          protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetReceivedListAgainstReturnBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetReceivedListAgainstReturnBizActionVO)valueObject;

                if (obj != null)
                {
                  
                        clsBaseReceivedItemAgainstReturnDAL objBaseDAL = clsBaseReceivedItemAgainstReturnDAL.GetInstance();
                        obj = (clsGetReceivedListAgainstReturnBizActionVO)objBaseDAL.GetReceivedListAgainstReturn(obj, objUserVO);
                  
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
                //logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

           #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsGetReceivedAgainstReturnListBizAction()
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
        private static clsGetReceivedAgainstReturnListBizAction _Instance = null;

       
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetReceivedAgainstReturnListBizAction();

            return _Instance;
        } 
    }
}

