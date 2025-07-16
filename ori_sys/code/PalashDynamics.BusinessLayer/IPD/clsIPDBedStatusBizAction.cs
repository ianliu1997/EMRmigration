/* Added  By SUDHIR PATIL on 03/March/2014 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.BusinessLayer.IPD
{
    internal class clsIPDBedStatusBizAction : BizAction
    {
        LogManager logManager = null;
        private clsIPDBedStatusBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsIPDBedStatusBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIPDBedStatusBizAction();
            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIPDBedStatusBizActionVO obj = null;
            try
            {
                obj = (clsIPDBedStatusBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseBedStatusDAL objBaseDAL = clsBaseBedStatusDAL.GetInstance();
                    obj = (clsIPDBedStatusBizActionVO)objBaseDAL.ViewBedStatus(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsGetWardByFloorBizAction : BizAction
    {
        LogManager logManager = null;
        private clsGetWardByFloorBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsGetWardByFloorBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetWardByFloorBizAction();
            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetWardByFloorBizActionVO obj = null;
            try
            {
                obj = (clsGetWardByFloorBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseBedStatusDAL objBaseDAL = clsBaseBedStatusDAL.GetInstance();
                    obj = (clsGetWardByFloorBizActionVO)objBaseDAL.GetWardByFloor(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }
}
