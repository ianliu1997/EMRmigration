using System.Text;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using System;

namespace PalashDynamics.BusinessLayer.Administration
{
    internal class clsGetDoctorScheduleMasterListBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetDoctorScheduleMasterListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorScheduleMasterListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDoctorScheduleMasterListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDoctorScheduleMasterListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDoctorScheduleDAL objBaseDAL = clsBaseDoctorScheduleDAL.GetInstance();

                    if (obj.IsNewSchedule == false)
                    {
                        obj = (clsGetDoctorScheduleMasterListBizActionVO)objBaseDAL.GetDoctorScheduleList(obj, objUserVO);
                    }
                    else if (obj.IsNewSchedule == true)
                    {
                        obj = (clsGetDoctorScheduleMasterListBizActionVO)objBaseDAL.GetDoctorScheduleListNew(obj, objUserVO);   // added on 13032018 for New Doctor Schedule
                    }

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


    internal class clsGetDoctorScheduleListBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetDoctorScheduleListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorScheduleListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDoctorScheduleListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDoctorScheduleListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDoctorScheduleDAL objBaseDAL = clsBaseDoctorScheduleDAL.GetInstance();

                    if (obj.IsNewSchedule == true)
                    {
                        obj = (clsGetDoctorScheduleListBizActionVO)objBaseDAL.GetDoctorScheduleDetailsListNew(obj, objUserVO);// added on 21032018 for New Doctor Schedule
                    }
                    else
                    {
                        obj = (clsGetDoctorScheduleListBizActionVO)objBaseDAL.GetDoctorScheduleDetailsList(obj, objUserVO);
                    }

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


    //rohinee
    internal class clsGetDoctorScheduleListByIDBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetDoctorScheduleListByIDBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorScheduleListByIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDoctorScheduleListByIDBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDoctorScheduleListByIDBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDoctorScheduleDAL objBaseDAL = clsBaseDoctorScheduleDAL.GetInstance();
                    obj = (clsGetDoctorScheduleListByIDBizActionVO)objBaseDAL.GetDoctorScheduleDetailsListByID(obj, objUserVO);


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

    internal class clsGetDoctorDepartmentUnitListBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetDoctorDepartmentUnitListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorDepartmentUnitListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDoctorDepartmentUnitListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDoctorDepartmentUnitListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDoctorScheduleDAL objBaseDAL = clsBaseDoctorScheduleDAL.GetInstance();
                    obj = (clsGetDoctorDepartmentUnitListBizActionVO)objBaseDAL.GetDoctorDepartmentUnitList(obj, objUserVO);


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

}
