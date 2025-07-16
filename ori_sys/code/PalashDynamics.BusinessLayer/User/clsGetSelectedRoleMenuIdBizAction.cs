using System;
using System.Collections.Generic;
using System.Text;
using PalashDynamics.BusinessLayer;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Administration.RoleMaster;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.BusinessLayer
{
    internal class clsGetSelectedRoleMenuIdBizAction : BizAction
    {
          //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the GetSelectedRoleMenuIdDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info
        private clsGetSelectedRoleMenuIdBizAction()
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
        private static clsGetSelectedRoleMenuIdBizAction _Instance = null;
        ///Method Input GetSelectedRoleMenuId: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetSelectedRoleMenuIdBizAction();

            return _Instance;
        }

        ///Method Input Tests: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetSelectedRoleMenuIdBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetSelectedRoleMenuIdBizActionVO)valueObject;

                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseGetSelectedRoleMenuIdDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetSelectedRoleMenuIdBizActionVO)objBaseGetSelectedRoleMenuIdDAL.GetSelectedRoleMenuId(obj, objUserVO);
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
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));

            }
            finally
            {
                //log error  
              //  logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    }
}
