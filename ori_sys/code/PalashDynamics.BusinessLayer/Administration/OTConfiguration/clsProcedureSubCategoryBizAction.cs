using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.OTConfiguration
{
    internal class clsAddUpdateProcedureSubCategoryBizAction: BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private static clsAddUpdateProcedureSubCategoryBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateProcedureSubCategoryBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddUpdateProcedureSubCategoryBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUpdateProcedureSubCategoryBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseOtConfigDAL objBaseDAL = clsBaseOtConfigDAL.GetInstance();
                    obj = (clsAddUpdateProcedureSubCategoryBizActionVO)objBaseDAL.AddUpdateProcedureSubCategory(obj, objUserVO);
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

    internal class clsGetProcedureSubCategoryListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private static clsGetProcedureSubCategoryListBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetProcedureSubCategoryListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetProcedureSubCategoryListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetProcedureSubCategoryListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseOtConfigDAL objBaseDAL = clsBaseOtConfigDAL.GetInstance();
                   obj = (clsGetProcedureSubCategoryListBizActionVO)objBaseDAL.GetProcedureSubCategoryList(obj, objUserVO);
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

    internal class clsUpdateStatusProcedureSubCategoryBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private static clsUpdateStatusProcedureSubCategoryBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateStatusProcedureSubCategoryBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsUpdateStatusProcedureSubCategoryBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateStatusProcedureSubCategoryBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseOtConfigDAL objBaseDAL = clsBaseOtConfigDAL.GetInstance();
                  obj = (clsUpdateStatusProcedureSubCategoryBizActionVO)objBaseDAL.UpdateStatusProcedureSubCategory(obj, objUserVO);
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
