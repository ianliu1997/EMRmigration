using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.Administration
{
    class clsUpdateServiceTariffBizAction : BizAction
    {
        ////This Region Contains Variables Which are Used At Form Level
        //#region Variables Declaration
        ////Declare the LogManager object
        //LogManager logManager = null;
        ////Declare the BaseRoleMasterDAL object
        ////Declare the Variable of UserId
        //long lngUserId = 0;
        //#endregion

        //private static clsUpdateServiceTariffBizAction _Instance = null;
        ///// <summary>
        ///// To create singleton instance of the class and  This will Give Unique Instance
        ///// </summary>
        ///// <returns></returns>

        //public static BizAction GetInstance()
        //{
        //    if (_Instance == null)
        //        _Instance = new clsUpdateServiceTariffBizAction();

        //    return _Instance;
        //}


        //protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        //{

        //    bool CurrentMethodExecutionStatus = true;
        //    clsUpdateServiceTariffMasterBizActionVO obj = null;
        //    int ResultStatus = 0;

        //    try
        //    {
        //        obj = (clsUpdateServiceTariffMasterBizActionVO)valueObject;
        //        //Typecast the "valueObject" to "clsInputOutputVO"
        //        if (obj != null)
        //        {
        //            clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
        //            obj = (clsUpdateServiceTariffMasterBizActionVO)objBaseDAL.UpdateServiceTariff(obj, objUserVO);


        //        }

        //    }
        //    catch (HmsApplicationException HEx)
        //    {
        //        CurrentMethodExecutionStatus = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        CurrentMethodExecutionStatus = false;
        //        throw;
        //    }
        //    finally
        //    {

        //    }
        //    return obj;
        //}
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsUpdateServiceTariffBizAction()
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
        private static clsUpdateServiceTariffBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateServiceTariffBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateServiceTariffMasterBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateServiceTariffMasterBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                    obj = (clsUpdateServiceTariffMasterBizActionVO)objBaseDAL.UpdateServiceTariff(obj, objUserVO);
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