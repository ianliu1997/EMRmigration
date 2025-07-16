using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.BusinessLayer.Inventory
{
    public class clsGetIssueListBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetIssueListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetIssueListBizActionVO)valueObject;

                if (obj != null)
                {

                    clsBaseIssueItemDAL objBaseDAL = clsBaseIssueItemDAL.GetInstance();

                    if (obj.IsQSOnly == true)
                        obj = (clsGetIssueListBizActionVO)objBaseDAL.GetIssueListQS(obj, objUserVO);  // Use to get already saved Issued Items to Quarantine Stores Only
                    else if (obj.IsForGRNQS == true)
                        obj = (clsGetIssueListBizActionVO)objBaseDAL.GetGRNToQSIssueList(obj, objUserVO);  //  set on ReceiveGRNToQS form while getting Records for Issue.
                    else
                        obj = (clsGetIssueListBizActionVO)objBaseDAL.GetIssueList(obj, objUserVO);

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
        private clsGetIssueListBizAction()
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
        private static clsGetIssueListBizAction _Instance = null;


        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIssueListBizAction();

            return _Instance;
        }
    }
}
