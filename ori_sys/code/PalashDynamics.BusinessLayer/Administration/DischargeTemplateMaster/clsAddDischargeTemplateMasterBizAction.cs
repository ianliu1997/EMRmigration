using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.DischargeTemplateMaster;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.DischargeTemplateMaster
{
    internal class clsAddDischargeTemplateMasterBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddDischargeTemplateMasterBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddDischargeTemplateMasterBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddDischargeTemplateMasterBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddDischargeTemplateMasterBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDischargeTemplateMasterDAL objBaseDAL = clsBaseDischargeTemplateMasterDAL.GetInstance();
                    obj = (clsAddDischargeTemplateMasterBizActionVO)objBaseDAL.AddUpdateDischargeTemplateMaster(obj, objUserVO);
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

    internal class clsGetDischargeTemplateMasterListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetDischargeTemplateMasterListBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDischargeTemplateMasterListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDischargeTemplateMasterListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDischargeTemplateMasterListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDischargeTemplateMasterDAL objBaseDAL = clsBaseDischargeTemplateMasterDAL.GetInstance();
                    if (obj.IsViewClick)
                    {
                        obj = (clsGetDischargeTemplateMasterListBizActionVO)objBaseDAL.CheckCountDischargeSummaryByID(obj, objUserVO);
                    }
                    else
                    {
                        obj = (clsGetDischargeTemplateMasterListBizActionVO)objBaseDAL.GetDischargeTemplateMasterList(obj, objUserVO);
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
}
