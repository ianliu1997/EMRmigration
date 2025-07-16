using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsAddUpdateBulkRateChangeDetailsBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsAddUpdateBulkRateChangeDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsAddUpdateBulkRateChangeDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsAddUpdateBulkRateChangeDetailsBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsAddUpdateBulkRateChangeDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUpdateBulkRateChangeDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsAddUpdateBulkRateChangeDetailsBizActionVO)objBaseDoctorDAL.AddupdateBulkRateChangeDetails(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;
        }

    }

    internal class clsGetBulkRateChangeDetailsListBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetBulkRateChangeDetailsListBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsGetBulkRateChangeDetailsListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsGetBulkRateChangeDetailsListBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetBulkRateChangeDetailsListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetBulkRateChangeDetailsListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    if (!obj.IsFromViewClick)
                        obj = (clsGetBulkRateChangeDetailsListBizActionVO)objBaseDoctorDAL.GetBulkRateChangeDetailsList(obj, objUserVO);
                    else
                        obj = (clsGetBulkRateChangeDetailsListBizActionVO)objBaseDoctorDAL.GetBulkRateChangeDetailsListByID(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;
        }

    }
}
