using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.BusinessLayer.User
{
    internal class clsAddUserCategoryLinkBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsAddUserCategoryLinkBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUserCategoryLinkBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddUserCategoryLinkBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUserCategoryLinkBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsAddUserCategoryLinkBizActionVO)objBaseDAL.AddUserCategoryLink(obj, objUserVO);
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

    internal class clsGetCategoryListBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsGetCategoryListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCategoryListBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetCategoryListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetCategoryListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetCategoryListBizActionVO)objBaseDAL.GetCategoryList(obj, objUserVO);
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

    internal class clsGetExistingCategoryListBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsGetExistingCategoryListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetExistingCategoryListBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetExistingCategoryListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetExistingCategoryListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetExistingCategoryListBizActionVO)objBaseDAL.GetExistingCategoryListForUser(obj, objUserVO);
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
