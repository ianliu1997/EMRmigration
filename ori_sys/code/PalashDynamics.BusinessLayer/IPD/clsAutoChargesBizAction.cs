using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    #region Auto Charges
    //Added by kiran for Add Auto Charges Service List
    public class clsAddIPDAutoChargesServiceListBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsAddIPDAutoChargesServiceListBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsAddIPDAutoChargesServiceListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsAddIPDAutoChargesServiceListBizAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddIPDAutoChargesServiceListBizActionVO obj = null;
            try
            {
                obj = (clsAddIPDAutoChargesServiceListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseAutoChargesDAL objBaseItem = clsBaseAutoChargesDAL.GetInstance();
                    obj = (clsAddIPDAutoChargesServiceListBizActionVO)objBaseItem.AddAutoChargesServiceList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }

    //Added By kiran for Get Auto Charges Service List 
    public class clsGetIPDAutoChargesServiceListBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetIPDAutoChargesServiceListBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsGetIPDAutoChargesServiceListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetIPDAutoChargesServiceListBizAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetIPDAutoChargesServiceListBizActionVO obj = null;
            try
            {
                obj = (clsGetIPDAutoChargesServiceListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseAutoChargesDAL objBaseItem = clsBaseAutoChargesDAL.GetInstance();
                    obj = (clsGetIPDAutoChargesServiceListBizActionVO)objBaseItem.GetAutoChargesServiceList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }

    //Added By kiran for Delete Service 
    public class clsDeleteServiceBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsDeleteServiceBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsDeleteServiceBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsDeleteServiceBizAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsDeleteServiceBizActionVO obj = null;
            try
            {
                obj = (clsDeleteServiceBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseAutoChargesDAL objBaseItem = clsBaseAutoChargesDAL.GetInstance();
                    obj = (clsDeleteServiceBizActionVO)objBaseItem.DeleteService(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }
    #endregion
}
