using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Administration.IPD;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    class clsIPDGetBedMasterBizAction:BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDGetBedMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDGetBedMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDGetBedMasterBizAction();

            return _Instance;
        }
        

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDGetBedMasterBizActionVO obj = null;
            try
            {
                obj = (clsIPDGetBedMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDGetBedMasterBizActionVO)objBaseItem.GetBedMasterList(obj, objUserVO);
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

    class clsAddUpdateBedMasterBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsAddUpdateBedMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsAddUpdateBedMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsAddUpdateBedMasterBizAction();            
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDAddUpdateBedMasterBizActionVO obj = null;
            try
            {
                obj = (clsIPDAddUpdateBedMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDAddUpdateBedMasterBizActionVO)objBaseItem.AddUpdateBedMaster(obj, objUserVO);
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

    class clsGetIPDBedListBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetIPDBedListBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsGetIPDBedListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetIPDBedListBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetIPDBedListBizActionVO obj = null;
            try
            {
                obj = (clsGetIPDBedListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsGetIPDBedListBizActionVO)objBaseItem.GetBedListByDifferentSearch(obj, objUserVO);
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

    class clsIPDUpdateBedMasterStatusBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsIPDUpdateBedMasterStatusBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsIPDUpdateBedMasterStatusBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsIPDUpdateBedMasterStatusBizAction();
            }
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateBedMasterStatusBizActionVO obj = null;
            int ResultSet = 0;

            try
            {
                obj = (clsIPDUpdateBedMasterStatusBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDUpdateBedMasterStatusBizActionVO)objBaseItem.UpdateBedMasterStatus(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return obj;
        }
    }

}
