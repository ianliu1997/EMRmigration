using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;



namespace PalashDynamics.BusinessLayer.Administration.StaffMaster
{
    internal class clsAddStaffBankInfoBizAction : BizAction
    {
         #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsAddStaffBankInfoBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAddStaffBankInfoBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsAddStaffBankInfoBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddStaffBankInfoBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddStaffBankInfoBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseStaffMasterDAL objBaseDoctorDAL = clsStaffMasterDAL.GetInstance();
                    obj = (clsAddStaffBankInfoBizActionVO)objBaseDoctorDAL.AddStaffBankInfo(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {

            }
            return obj;

        }
    }

    internal class clsGetStaffBankInfoByIdBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetStaffBankInfoByIdBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsGetStaffBankInfoByIdBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsGetStaffBankInfoByIdBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetStaffBankInfoByIdVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetStaffBankInfoByIdVO)valueObject;
                if (obj != null)
                {
                    clsBaseStaffMasterDAL objBaseDoctorDAL = clsStaffMasterDAL.GetInstance();
                    obj = (clsGetStaffBankInfoByIdVO)objBaseDoctorDAL.GetStaffBankInfoById(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return obj;
        }
      
    }

    internal class clsGetStaffBankInfoBizAction : BizAction
    {
        private static clsGetStaffBankInfoBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetStaffBankInfoBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetStaffBankInfoBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetStaffBankInfoBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseStaffMasterDAL objBaseDoctorDAL = clsStaffMasterDAL.GetInstance();
                    obj = (clsGetStaffBankInfoBizActionVO)objBaseDoctorDAL.GetStaffBankInfo(obj, objUserVO);
                }
            }
            //catch (HmsApplicationException HEx)
            //{
            //    CurrentMethodExecutionStatus = false;
            //    throw;
            //}
            catch (Exception ex)
            {
                //CurrentMethodExecutionStatus = false;
                //log error  

            }
            finally
            {
            }

            return valueObject;

        }
    }

    internal class clsUpdateStaffBankInfoBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsUpdateStaffBankInfoBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsUpdateStaffBankInfoBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsUpdateStaffBankInfoBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsUpdateStaffBankInfoVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateStaffBankInfoVO)valueObject;
                if (obj != null)
                {
                    clsBaseStaffMasterDAL objBaseDoctorDAL = clsStaffMasterDAL.GetInstance();
                    obj = (clsUpdateStaffBankInfoVO)objBaseDoctorDAL.UpdateStaffBankInfo(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return obj;

        }
    }
}
