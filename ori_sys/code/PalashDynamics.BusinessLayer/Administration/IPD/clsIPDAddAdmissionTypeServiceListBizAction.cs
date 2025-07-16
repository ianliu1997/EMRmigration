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
    internal class clsIPDAddAdmissionTypeServiceListBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsIPDAddAdmissionTypeServiceListBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsIPDAddAdmissionTypeServiceListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsIPDAddAdmissionTypeServiceListBizAction();
            }

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDAddAdmissionTypeServiceListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsIPDAddAdmissionTypeServiceListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseDoctorDAL = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDAddAdmissionTypeServiceListBizActionVO)objBaseDoctorDAL.AddAdmissionTypeServiceList(obj, objUserVO);
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

    internal class clsIPDAddUpdateAdmissionTypeServiceListBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsIPDAddUpdateAdmissionTypeServiceListBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsIPDAddUpdateAdmissionTypeServiceListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsIPDAddUpdateAdmissionTypeServiceListBizAction();
            }

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDAddUpdateAdmissionTypeServiceListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsIPDAddUpdateAdmissionTypeServiceListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseDoctorDAL = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDAddUpdateAdmissionTypeServiceListBizActionVO)objBaseDoctorDAL.AddUpdateAdmissionTypeServiceList(obj, objUserVO);
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
