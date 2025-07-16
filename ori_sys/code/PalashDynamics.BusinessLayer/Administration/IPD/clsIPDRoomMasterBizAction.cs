using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    class clsIPDGetRoomMasterBizAction:BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDGetRoomMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDGetRoomMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDGetRoomMasterBizAction();

            return _Instance;
        }
        

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDGetRoomMasterBizActionVO obj = null;
            try
            {
                obj = (clsIPDGetRoomMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDGetRoomMasterBizActionVO)objBaseItem.GetRoomMasterList(obj, objUserVO);
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

    class clsIPDAddUpdateRoomMasterBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDAddUpdateRoomMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDAddUpdateRoomMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDAddUpdateRoomMasterBizAction();            
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDAddUpdateRoomMasterBizActionVO obj = null;
            try
            {
                obj = (clsIPDAddUpdateRoomMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDAddUpdateRoomMasterBizActionVO)objBaseItem.AddUpdateRoomMasterList(obj, objUserVO);
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

    class clsIPDUpdateRoomMasterStatusBizAction : BizAction
    {
         #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsIPDUpdateRoomMasterStatusBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsIPDUpdateRoomMasterStatusBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsIPDUpdateRoomMasterStatusBizAction();
            }
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateRoomStatusBizActionVO obj = null;
            int ResultSet = 0;

            try
            {
                obj = (clsIPDUpdateRoomStatusBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDUpdateRoomStatusBizActionVO)objBaseItem.UpdateRoomMasterStatus(obj, objUserVO);
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
