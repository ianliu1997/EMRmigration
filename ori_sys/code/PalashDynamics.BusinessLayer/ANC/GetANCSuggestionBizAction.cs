using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.ValueObjects.ANC;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ANC;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.BusinessLayer.ANC
{
    class GetANCSuggestionBizAction:BizAction
    {
    #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        #region Constructor
        private GetANCSuggestionBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        //The Private declaration
        private static GetANCSuggestionBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new GetANCSuggestionBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            GetANCSuggestionBizActionVO obj = null;

            try
            {
                obj = (GetANCSuggestionBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseANCDAL objBaseDAL = clsBaseANCDAL.GetInstance();
                    obj = (GetANCSuggestionBizActionVO)objBaseDAL.GetANCSuggestion(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            { 
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
