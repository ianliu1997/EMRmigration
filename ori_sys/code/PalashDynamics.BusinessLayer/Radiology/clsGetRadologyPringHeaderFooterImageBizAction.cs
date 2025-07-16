using System;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.BusinessLayer
{
    class clsGetRadologyPringHeaderFooterImageBizAction:BizAction
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
        private clsGetRadologyPringHeaderFooterImageBizAction()
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
        private static clsGetRadologyPringHeaderFooterImageBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRadologyPringHeaderFooterImageBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            //bool CurrentMethodExecutionStatus = true;
            clsGetRadologyPringHeaderFooterImageBizActionVO obj = null;
            //int ResultStatus = 0;
            try
            {
                obj = (clsGetRadologyPringHeaderFooterImageBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                    obj = (clsGetRadologyPringHeaderFooterImageBizActionVO)objBaseDAL.GetRadologyPringHeaderFooterImage(obj, objUserVO);
                }
            }
            catch (HmsApplicationException )
            {
               // CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
               // CurrentMethodExecutionStatus = false;
                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
