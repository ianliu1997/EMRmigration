using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.OTConfiguration
{
    internal class clsAddUpdateProcedureCheckListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private static clsAddUpdateProcedureCheckListBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateProcedureCheckListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddUpdateProcedureCheckListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUpdateProcedureCheckListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseOtConfigDAL objBaseDAL = clsBaseOtConfigDAL.GetInstance();
                    obj = (clsAddUpdateProcedureCheckListBizActionVO)objBaseDAL.AddUpdateProcedureCheckList(obj, objUserVO);
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

    internal class clsGetProcedureCheckListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private static clsGetProcedureCheckListBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetProcedureCheckListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetProcedureCheckListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetProcedureCheckListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseOtConfigDAL objBaseDAL = clsBaseOtConfigDAL.GetInstance();
                    obj = (clsGetProcedureCheckListBizActionVO)objBaseDAL.GetProcedureCheckList(obj, objUserVO);
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

    internal class clsUpdateStatusProcedureCheckListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private static clsUpdateStatusProcedureCheckListBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateStatusProcedureCheckListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsUpdateStatusProcedureCheckListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateStatusProcedureCheckListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseOtConfigDAL objBaseDAL = clsBaseOtConfigDAL.GetInstance();
                    obj = (clsUpdateStatusProcedureCheckListBizActionVO)objBaseDAL.UpdateStatusProcedureCheckList(obj, objUserVO);
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
