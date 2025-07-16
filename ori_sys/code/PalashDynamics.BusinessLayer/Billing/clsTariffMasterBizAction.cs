using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;

namespace PalashDynamics.BusinessLayer.Billing
{
    internal class clsAddTariffMasterBizAction : BizAction
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
        private clsAddTariffMasterBizAction()
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
        private static clsAddTariffMasterBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddTariffMasterBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddTariffMasterBizActionVO obj = null;

            try
            {
                obj = (clsAddTariffMasterBizActionVO)valueObject;
              
                if (obj != null)
                {
                    clsBaseTariffMasterDAL objBaseDAL = clsBaseTariffMasterDAL.GetInstance();
                    obj = (clsAddTariffMasterBizActionVO)objBaseDAL.AddTariff(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    internal class clsGetServiceByTariffIDBizAction : BizAction
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
        private clsGetServiceByTariffIDBizAction()
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
        private static clsGetServiceByTariffIDBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServiceByTariffIDBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetServiceByTariffIDBizActionVO obj = null;

            try
            {
                obj = (clsGetServiceByTariffIDBizActionVO)valueObject;
              
                if (obj != null)
                {
                    clsBaseTariffMasterDAL objBaseDAL = clsBaseTariffMasterDAL.GetInstance();
                    obj = (clsGetServiceByTariffIDBizActionVO)objBaseDAL.GetServiceByTariffID(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
    
    internal class clsGetServiceForIssueBizAction : BizAction
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
        private clsGetServiceForIssueBizAction()
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
        private static clsGetServiceForIssueBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServiceForIssueBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetServiceForIssueBizActionVO obj = null;

            try
            {
                obj = (clsGetServiceForIssueBizActionVO)valueObject;
              
                if (obj != null)
                {
                    clsBaseTariffMasterDAL objBaseDAL = clsBaseTariffMasterDAL.GetInstance();
                    obj = (clsGetServiceForIssueBizActionVO)objBaseDAL.GetServicesforIssue(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    internal class clsGetTariffListBizAction:BizAction
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
        private clsGetTariffListBizAction()
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
        private static clsGetTariffListBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetTariffListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetTariffListBizActionVO obj = null;

            try
            {
                obj = (clsGetTariffListBizActionVO)valueObject;
              
                if (obj != null)
                {
                    clsBaseTariffMasterDAL objBaseDAL = clsBaseTariffMasterDAL.GetInstance();
                    obj = (clsGetTariffListBizActionVO)objBaseDAL.GetTariffList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    internal class clsGetSpecializationsByTariffIdBizAction : BizAction
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
        private clsGetSpecializationsByTariffIdBizAction()
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
        private static clsGetSpecializationsByTariffIdBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetSpecializationsByTariffIdBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetSpecializationsByTariffIdBizActionVO obj = null;

            try
            {
                obj = (clsGetSpecializationsByTariffIdBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseTariffMasterDAL objBaseDAL = clsBaseTariffMasterDAL.GetInstance();
                    obj = (clsGetSpecializationsByTariffIdBizActionVO)objBaseDAL.GetSpecializationsByTariffId(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
