using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.UserRights;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.CustomExceptions;
namespace PalashDynamics.BusinessLayer.Administration.UserRights
{
    internal class clsSetCreditLimitBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager lgmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsSetCreditLimitBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsSetCreditLimitBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsSetCreditLimitBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsSetCreditLimitBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsSetCreditLimitBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUserRightsDAL objBaseDAL = clsBaseUserRightsDAL.GetInstance();
                    obj = (clsSetCreditLimitBizActionVO)objBaseDAL.AddCreditLimit(obj, objUserVO);
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

    internal class clsGetUserRightsBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetUserRightsBizAction()
        {
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
        }

        private static clsGetUserRightsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUserRightsBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetUserRightsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetUserRightsBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUserRightsDAL objBaseDAL = clsBaseUserRightsDAL.GetInstance();
                    obj = (clsGetUserRightsBizActionVO)objBaseDAL.GetUserRights(obj, objUserVO);
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

    // Added By CDS 4/01/2016
    internal class clsGetUserGRNCountWithRightsAndFrequencyBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetUserGRNCountWithRightsAndFrequencyBizAction()
        {
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
        }

        private static clsGetUserGRNCountWithRightsAndFrequencyBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUserGRNCountWithRightsAndFrequencyBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetUserGRNCountWithRightsAndFrequencyBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetUserGRNCountWithRightsAndFrequencyBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUserRightsDAL objBaseDAL = clsBaseUserRightsDAL.GetInstance();
                    obj = (clsGetUserGRNCountWithRightsAndFrequencyBizActionVO)objBaseDAL.GRNCountWithRightsAndFrequency(obj, objUserVO);
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

}
