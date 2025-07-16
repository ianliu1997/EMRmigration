using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer
{

    internal class clsGetPackageServiceBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;

            clsGetPackageServiceBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPackageServiceBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceDAL objBaseDAL = clsBasePackageServiceDAL.GetInstance();
                    obj = (clsGetPackageServiceBizActionVO)objBaseDAL.GetAllPackageServices(obj, objUserVO);
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



        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsGetPackageServiceBizAction()
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
        private static clsGetPackageServiceBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageServiceBizAction();

            return _Instance;
        }
    }


    internal class clsAddPackageServiceBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {



            clsAddPackageServiceBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddPackageServiceBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceDAL objBaseDAL = clsBasePackageServiceDAL.GetInstance();
                    obj = (clsAddPackageServiceBizActionVO)objBaseDAL.AddPackageServices(obj, objUserVO);
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
        private clsAddPackageServiceBizAction()
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
        private static clsAddPackageServiceBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddPackageServiceBizAction();

            return _Instance;
        }
    }

    internal class clsGetPackageServiceListBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {



            clsGetPackageServiceListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPackageServiceListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceDAL objBaseDAL = clsBasePackageServiceDAL.GetInstance();
                    obj = (clsGetPackageServiceListBizActionVO)objBaseDAL.GetPackageServiceList(obj, objUserVO);
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
        private clsGetPackageServiceListBizAction()
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
        private static clsGetPackageServiceListBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageServiceListBizAction();

            return _Instance;
        }
    }

    internal class clsGetPackageServiceDetailsListBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {



            clsGetPackageServiceDetailsListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPackageServiceDetailsListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceDAL objBaseDAL = clsBasePackageServiceDAL.GetInstance();
                    obj = (clsGetPackageServiceDetailsListBizActionVO)objBaseDAL.GetPackageServiceDetailList(obj, objUserVO);
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
        private clsGetPackageServiceDetailsListBizAction()
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
        private static clsGetPackageServiceDetailsListBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageServiceDetailsListBizAction();

            return _Instance;
        }
    }

    internal class clsGetPackageServiceFromServiceIDBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {



            clsGetPackageServiceFromServiceIDBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPackageServiceFromServiceIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceDAL objBaseDAL = clsBasePackageServiceDAL.GetInstance();
                    obj = (clsGetPackageServiceFromServiceIDBizActionVO)objBaseDAL.GetPackageServiceDetailListbyServiceID(obj, objUserVO);
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
        private clsGetPackageServiceFromServiceIDBizAction()
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
        private static clsGetPackageServiceFromServiceIDBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageServiceFromServiceIDBizAction();

            return _Instance;
        }
    }

    #region For IPD Module

    internal class clsGetPackageServiceForBillBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {



            clsGetPackageServiceForBillBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPackageServiceForBillBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceDAL objBaseDAL = clsBasePackageServiceDAL.GetInstance();
                    obj = (clsGetPackageServiceForBillBizActionVO)objBaseDAL.GetPackageServiceForBill(obj, objUserVO);
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
        private clsGetPackageServiceForBillBizAction()
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
        private static clsGetPackageServiceForBillBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageServiceForBillBizAction();

            return _Instance;
        }
    }

    #endregion

}
