using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    public class clsIPDGetClassMasterBizAction:BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDGetClassMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDGetClassMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDGetClassMasterBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {            
            clsIPDGetClassMasterBizActionVO obj = null;
            try
            {
                obj = (clsIPDGetClassMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDGetClassMasterBizActionVO)objBaseItem.GetClassMasterDetails(obj, objUserVO);
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

    public class clsIPDAddUpdateClassMasterBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDAddUpdateClassMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDAddUpdateClassMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDAddUpdateClassMasterBizAction();            
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDAddUpdateClassMasterBizActionVO obj = null;
            try
            {
                obj = (clsIPDAddUpdateClassMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDAddUpdateClassMasterBizActionVO)objBaseItem.AddUpdateClassMasterDetails(obj, objUserVO);
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

    public class clsIPDUpdateClassMasterStatusBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsIPDUpdateClassMasterStatusBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsIPDUpdateClassMasterStatusBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsIPDUpdateClassMasterStatusBizAction();
            }
            return _Instance;
        }

    
        protected override ValueObjects.IValueObject  ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateClassMasterStatusBizActionVO obj = null;
            int ResultSet = 0;

            try
            {
                obj = (clsIPDUpdateClassMasterStatusBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDUpdateClassMasterStatusBizActionVO)objBaseItem.UpdateClassMasterStatus(obj, objUserVO);
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
