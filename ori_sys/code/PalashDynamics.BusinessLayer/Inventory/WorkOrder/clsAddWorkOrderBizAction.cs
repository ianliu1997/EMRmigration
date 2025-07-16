using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Inventory.WorkOrder;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
namespace PalashDynamics.BusinessLayer.Inventory.WorkOrder
{
    internal class clsAddWorkOrderBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddWorkOrderBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddWorkOrderBizActionVO)valueObject;

                if (obj != null)
                {

                    clsBaseWorkOrderDAL objBaseDAL = clsBaseWorkOrderDAL.GetInstance();
                    obj = (clsAddWorkOrderBizActionVO)objBaseDAL.AddWorkOrder(obj, objUserVO);

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

        private clsAddWorkOrderBizAction()
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
        private static clsAddWorkOrderBizAction _Instance = null;


        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddWorkOrderBizAction();

            return _Instance;
        }
    }





    internal class clsCancelWorkOrderBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsCancelWorkOrderBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsCancelWorkOrderBizActionVO)valueObject;

                if (obj != null)
                {

                    clsBaseWorkOrderDAL objBaseDAL = clsBaseWorkOrderDAL.GetInstance();
                    obj = (clsCancelWorkOrderBizActionVO)objBaseDAL.CancelWorkOrder(obj, objUserVO);

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

        private clsCancelWorkOrderBizAction()
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
        private static clsCancelWorkOrderBizAction _Instance = null;


        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsCancelWorkOrderBizAction();

            return _Instance;
        }
    }


    //internal class clsAddRateContractBizAction : BizAction
    //{
    //    #region Variables Declaration
    //    //Declare the LogManager object
    //    LogManager logManager = null;
    //    //Declare the BaseOPDPatientMasterDAL object
    //    //Declare the Variable of UserId
    //    long lngUserId = 0;
    //    #endregion
    //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
    //    {
    //        bool CurrentMethodExecutionStatus = true;
    //        clsAddRateContractBizActionVO obj = null;
    //        int ResultStatus = 0;
    //        try
    //        {
    //            obj = (clsAddRateContractBizActionVO)valueObject;

    //            if (obj != null)
    //            {

    //                clsBasePurchaseOrderDAL objBaseDAL = clsBasePurchaseOrderDAL.GetInstance();
    //                obj = (clsAddRateContractBizActionVO)objBaseDAL.AddRateContract(obj, objUserVO);

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
    //            //log error  
    //            //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            // Guid a = new Guid();
    //            //logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
    //        }
    //        finally
    //        {
    //            //log error  
    //            // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
    //        }
    //        return obj;
    //    }

    //    private clsAddRateContractBizAction()
    //    {
    //        //Create Instance of the LogManager object 
    //        #region Logging Code
    //        if (logManager == null)
    //        {
    //            logManager = LogManager.GetInstance();
    //        }
    //        #endregion
    //    }

    //    //The Private declaration
    //    private static clsAddRateContractBizAction _Instance = null;


    //    public static BizAction GetInstance()
    //    {
    //        if (_Instance == null)
    //            _Instance = new clsAddRateContractBizAction();

    //        return _Instance;
    //    }
    //}


    // internal class clsGetRateContractBizAction : BizAction
    //{
    //    #region Variables Declaration
    //    //Declare the LogManager object
    //    LogManager logManager = null;
    //    //Declare the BaseOPDPatientMasterDAL object
    //    //Declare the Variable of UserId
    //    long lngUserId = 0;
    //    #endregion
    //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
    //    {
    //        bool CurrentMethodExecutionStatus = true;
    //        clsGetRateContractBizActionVO obj = null;
    //        int ResultStatus = 0;
    //        try
    //        {
    //            obj = (clsGetRateContractBizActionVO)valueObject;

    //            if (obj != null)
    //            {

    //                clsBasePurchaseOrderDAL objBaseDAL = clsBasePurchaseOrderDAL.GetInstance();
    //                obj = (clsGetRateContractBizActionVO)objBaseDAL.GetRateContract(obj, objUserVO);

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
    //            //log error  
    //            //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            // Guid a = new Guid();
    //            //logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
    //        }
    //        finally
    //        {
    //            //log error  
    //            // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
    //        }
    //        return obj;
    //    }

    //    private clsGetRateContractBizAction()
    //    {
    //        //Create Instance of the LogManager object 
    //        #region Logging Code
    //        if (logManager == null)
    //        {
    //            logManager = LogManager.GetInstance();
    //        }
    //        #endregion
    //    }

    //    //The Private declaration
    //    private static clsGetRateContractBizAction _Instance = null;


    //    public static BizAction GetInstance()
    //    {
    //        if (_Instance == null)
    //            _Instance = new clsGetRateContractBizAction();

    //        return _Instance;
    //    }
    //}


    //internal class clsCheckContractBizAction : BizAction
    //{
    //    #region Variables Declaration
    //    //Declare the LogManager object
    //    LogManager logManager = null;
    //    //Declare the BaseOPDPatientMasterDAL object
    //    //Declare the Variable of UserId
    //    long lngUserId = 0;
    //    #endregion
    //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
    //    {
    //        bool CurrentMethodExecutionStatus = true;
    //        clsCheckContractBizActionVO obj = null;
    //        int ResultStatus = 0;
    //        try
    //        {
    //            obj = (clsCheckContractBizActionVO)valueObject;

    //            if (obj != null)
    //            {

    //                clsBasePurchaseOrderDAL objBaseDAL = clsBasePurchaseOrderDAL.GetInstance();
    //                obj = (clsCheckContractBizActionVO)objBaseDAL.CheckContractValidity(obj, objUserVO);

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
    //            //log error  
    //            //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            // Guid a = new Guid();
    //            //logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
    //        }
    //        finally
    //        {
    //            //log error  
    //            // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
    //        }
    //        return obj;
    //    }

    //    private clsCheckContractBizAction()
    //    {
    //        //Create Instance of the LogManager object 
    //        #region Logging Code
    //        if (logManager == null)
    //        {
    //            logManager = LogManager.GetInstance();
    //        }
    //        #endregion
    //    }

    //    //The Private declaration
    //    private static clsCheckContractBizAction _Instance = null;


    //    public static BizAction GetInstance()
    //    {
    //        if (_Instance == null)
    //            _Instance = new clsCheckContractBizAction();

    //        return _Instance;
    //    }
    //}

    //internal class clsGetRateContractItemDetailsBizAction : BizAction
    //{
    //    #region Variables Declaration
    //    //Declare the LogManager object
    //    LogManager logManager = null;
    //    //Declare the BaseOPDPatientMasterDAL object
    //    //Declare the Variable of UserId
    //    long lngUserId = 0;
    //    #endregion
    //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
    //    {
    //        bool CurrentMethodExecutionStatus = true;
    //        clsGetRateContractItemDetailsBizActionVO obj = null;
    //        int ResultStatus = 0;
    //        try
    //        {
    //            obj = (clsGetRateContractItemDetailsBizActionVO)valueObject;

    //            if (obj != null)
    //            {

    //                clsBasePurchaseOrderDAL objBaseDAL = clsBasePurchaseOrderDAL.GetInstance();
    //                obj = (clsGetRateContractItemDetailsBizActionVO)objBaseDAL.GetRateContractItemDetail(obj, objUserVO);

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
    //            //log error  
    //            //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            // Guid a = new Guid();
    //            //logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
    //            throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
    //        }
    //        finally
    //        {
    //            //log error  
    //            // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
    //        }
    //        return obj;
    //    }

    //    private clsGetRateContractItemDetailsBizAction()
    //    {
    //        //Create Instance of the LogManager object 
    //        #region Logging Code
    //        if (logManager == null)
    //        {
    //            logManager = LogManager.GetInstance();
    //        }
    //        #endregion
    //    }

    //    //The Private declaration
    //    private static clsGetRateContractItemDetailsBizAction _Instance = null;


    //    public static BizAction GetInstance()
    //    {
    //        if (_Instance == null)
    //            _Instance = new clsGetRateContractItemDetailsBizAction();

    //        return _Instance;
    //    }
    //}

}

