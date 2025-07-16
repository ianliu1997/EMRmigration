using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer
{
    internal class clsAddPatientConsentBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddPatientConsentBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddPatientConsentBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientConsentDAL objBaseDAL = clsBasePatientConsentDAL.GetInstance();
                    obj = (clsAddPatientConsentBizActionVO)objBaseDAL.AddPatientConsent(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

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



        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsAddPatientConsentBizAction()
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
        private static clsAddPatientConsentBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddPatientConsentBizAction();

            return _Instance;
        }
    }

    internal class clsGetPatientConsentMasterBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetPatientConsentMasterBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientConsentMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientConsentDAL objBaseDAL = clsBasePatientConsentDAL.GetInstance();
                    obj = (clsGetPatientConsentMasterBizActionVO)objBaseDAL.GetPatientConsent(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

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



        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsGetPatientConsentMasterBizAction()
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
        private static clsGetPatientConsentMasterBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientConsentMasterBizAction();

            return _Instance;
        }
    }

    //public class clsGetMasterListConsentBizAction : BizAction
    //{

    //    private static clsGetMasterListConsentBizAction _Instance = null;
    //    /// <summary>
    //    /// To create singleton instance of the class and  This will Give Unique Instance
    //    /// </summary>
    //    /// <returns></returns>
    //    public static BizAction GetInstance()
    //    {
    //        if (_Instance == null)
    //            _Instance = new clsGetMasterListConsentBizAction();

    //        return _Instance;
    //    }


    //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
    //    {
    //        bool CurrentMethodExecutionStatus = true;
    //        clsGetMasterListConsentBizActionVO obj = null;
    //        int ResultStatus = 0;
    //        try
    //        {
    //            obj = (clsGetMasterListConsentBizActionVO)valueObject;
    //            if (obj != null)
    //            {
    //                clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
    //                obj = (clsGetMasterListConsentBizActionVO)objBaseMasterDAL.GetMasterListForConsent(obj, objUserVO);
    //            }
    //        }
    //        catch (HmsApplicationException HEx)
    //        {
    //            CurrentMethodExecutionStatus = false;
    //            throw;
    //        }
    //        catch (Exception ex)
    //        {
    //            CurrentMethodExecutionStatus = false;
    //        }
    //        return valueObject;
    //    }
    //}

}
