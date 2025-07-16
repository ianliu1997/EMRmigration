using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using PalashDynamics.ValueObjects.Master.DoctorMaster;

namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsGetDoctorServiceLinkingByCategoryBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetDoctorServiceLinkingByCategoryBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsGetDoctorServiceLinkingByCategoryBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsGetDoctorServiceLinkingByCategoryBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorServiceLinkingByCategoryBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorServiceLinkingByCategoryBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    if (!obj.IsForClinic)
                        obj = (clsGetDoctorServiceLinkingByCategoryBizActionVO)objBaseDoctorDAL.GetDoctorServiceLinkingByCategoryId(obj, objUserVO);
                    else
                        obj = (clsGetDoctorServiceLinkingByCategoryBizActionVO)objBaseDoctorDAL.GetDoctorServiceLinkingByClinic(obj, objUserVO);  
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

    internal class clsAddUpdateDoctorServiceLinkingByCategoryBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsAddUpdateDoctorServiceLinkingByCategoryBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsAddUpdateDoctorServiceLinkingByCategoryBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsAddUpdateDoctorServiceLinkingByCategoryBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsAddUpdateDoctorServiceLinkingByCategoryBizActionVO)objBaseDoctorDAL.AddUpdateDoctorServiceLinkingByCategory(obj, objUserVO);
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

    //Added BY CDS clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO
    internal class clsGetDoctorServiceLinkingByCategoryAndServiceBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetDoctorServiceLinkingByCategoryAndServiceBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsGetDoctorServiceLinkingByCategoryAndServiceBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsGetDoctorServiceLinkingByCategoryAndServiceBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();

                    obj = (clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO)objBaseDoctorDAL.GetDoctorServiceLinkingByDoctorId(obj, objUserVO);
                  
                        //obj = (clsGetDoctorServiceLinkingByCategoryAndServiceBizActionVO)objBaseDoctorDAL.GetDoctorServiceLinkingByClinic(obj, objUserVO);
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
