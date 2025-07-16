using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.CompoundDrug;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CompoundDrug;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.BusinessLayer.CompoundDrug
{
   internal class clsGetCompoundDrugBizAction:BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetCompoundDrugBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetCompoundDrugBizActionVO)valueObject;

                if (obj != null)
                {

                    clsBaseCompoundDrugDAL objBaseDAL = clsBaseCompoundDrugDAL.GetInstance();
                        obj = (clsGetCompoundDrugBizActionVO)objBaseDAL.GetCompoundDrug(valueObject,objUserVO);
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
        private clsGetCompoundDrugBizAction()
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
        private static clsGetCompoundDrugBizAction _Instance = null;

       
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCompoundDrugBizAction();

            return _Instance;
        } 
    }



   internal class clsCheckCompoundDrugBizAction : BizAction
   {
       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsCheckCompoundDrugBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsCheckCompoundDrugBizActionVO)valueObject;

               if (obj != null)
               {

                   clsBaseCompoundDrugDAL objBaseDAL = clsBaseCompoundDrugDAL.GetInstance();
                   obj = (clsCheckCompoundDrugBizActionVO)objBaseDAL.CheckCompoundDrug(valueObject, objUserVO);
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
       private clsCheckCompoundDrugBizAction()
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
       private static clsCheckCompoundDrugBizAction _Instance = null;


       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsCheckCompoundDrugBizAction();

           return _Instance;
       }
   }
}
