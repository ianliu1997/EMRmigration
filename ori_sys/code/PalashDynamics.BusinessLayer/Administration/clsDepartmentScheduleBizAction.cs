using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Administration.DepartmentScheduleMaster;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration
{
    internal class clsAddDepartmentScheduleMasterBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddDepartmentScheduleMasterBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddDepartmentScheduleMasterBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddDepartmentScheduleMasterBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddDepartmentScheduleMasterBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDepartmentScheduleDAL objBaseDAL = clsBaseDepartmentScheduleDAL.GetInstance();
                    obj = (clsAddDepartmentScheduleMasterBizActionVO)objBaseDAL.AddDepartmentScheduleMaster(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }

    internal class clsCheckTimeForScheduleExistanceDepartmentBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion


        private static clsCheckTimeForScheduleExistanceDepartmentBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsCheckTimeForScheduleExistanceDepartmentBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsCheckTimeForScheduleExistanceDepartmentBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsCheckTimeForScheduleExistanceDepartmentBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDepartmentScheduleDAL objBaseDAL = clsBaseDepartmentScheduleDAL.GetInstance();
                    obj = (clsCheckTimeForScheduleExistanceDepartmentBizActionVO)objBaseDAL.CheckScheduleTime(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsGetDepartmentScheduleTimeBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetDepartmentScheduleTimeBizAction()
        {

            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetDepartmentScheduleTimeBizAction _Instance = null;

        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDepartmentScheduleTimeBizAction();
            return _Instance;
        }

        ///Method Input Appointments: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose    :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDepartmentScheduleTimeVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDepartmentScheduleTimeVO)valueObject;
                if (obj != null)
                {
                    clsBaseDepartmentScheduleDAL objBaseDepartmentScheduleDAL = clsBaseDepartmentScheduleDAL.GetInstance();
                    obj = (clsGetDepartmentScheduleTimeVO)objBaseDepartmentScheduleDAL.GetDepartmentScheduleTime(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsGetDepartmentScheduleMasterListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetDepartmentScheduleMasterListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDepartmentScheduleMasterListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDepartmentScheduleMasterListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDepartmentScheduleMasterListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDepartmentScheduleDAL objBaseDAL = clsBaseDepartmentScheduleDAL.GetInstance();
                    obj = (clsGetDepartmentScheduleMasterListBizActionVO)objBaseDAL.GetDepartmentScheduleList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsGetDepartmentScheduleListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetDepartmentScheduleListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDepartmentScheduleListBizAction();

            return _Instance;
        }
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDepartmentScheduleListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDepartmentScheduleListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDepartmentScheduleDAL objBaseDAL = clsBaseDepartmentScheduleDAL.GetInstance();
                    obj = (clsGetDepartmentScheduleListBizActionVO)objBaseDAL.GetDepartmentScheduleDetailsList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsGetDepartmentDepartmentDetailsBizAction : BizAction
    {
        private static clsGetDepartmentDepartmentDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDepartmentDepartmentDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetDepartmentDepartmentDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDepartmentDepartmentDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseDepartmentScheduleDAL objBaseMasterDAL = clsBaseDepartmentScheduleDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetDepartmentDepartmentDetailsBizActionVO)objBaseMasterDAL.GetDepartmentDepartmentDetails(obj, objUserVO);
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

            }

            return valueObject;

        }
    }
}
