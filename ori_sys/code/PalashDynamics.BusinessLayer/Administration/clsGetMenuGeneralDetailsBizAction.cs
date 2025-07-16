using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.Menu;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer
{
    internal class clsGetMenuGeneralDetailsBizAction:BizAction 
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServUserMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info
        private clsGetMenuGeneralDetailsBizAction()
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
        private static clsGetMenuGeneralDetailsBizAction _Instance = null;
        ///Method Input Users: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetMenuGeneralDetailsBizAction();

            return _Instance;
        }

        ///Method Input Users: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetMenuGeneralDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetMenuGeneralDetailsBizActionVO)valueObject;

                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseMenuDAL objBaseMenuDAL = clsBaseMenuDAL.GetInstance();
                    obj = (clsGetMenuGeneralDetailsBizActionVO)objBaseMenuDAL.GetMenuGeneralDetails(obj, objUserVO);
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
                //logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }
    
    }
}
