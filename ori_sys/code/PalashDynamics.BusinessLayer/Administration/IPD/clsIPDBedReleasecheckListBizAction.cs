using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    public class clsAddUpdateBedReleaseCheckListDetailsBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsAddUpdateBedReleaseCheckListDetailsBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsAddUpdateBedReleaseCheckListDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsAddUpdateBedReleaseCheckListDetailsBizAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddUpdateBedReleaseCheckListDetailsBizActionVO obj = null;
            try
            {
                obj = (clsAddUpdateBedReleaseCheckListDetailsBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseBedReleaseCheckListDAL objBaseItem = clsBaseBedReleaseCheckListDAL.GetInstance();
                    obj = (clsAddUpdateBedReleaseCheckListDetailsBizActionVO)objBaseItem.AddUpdateBedReleaseCheckListDetails(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }

    public class clsGetBedReleaseCheckListBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetBedReleaseCheckListBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsGetBedReleaseCheckListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetBedReleaseCheckListBizAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetBedReleaseCheckListBizActionVO obj = null;
            try
            {
                obj = (clsGetBedReleaseCheckListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseBedReleaseCheckListDAL objBaseItem = clsBaseBedReleaseCheckListDAL.GetInstance();
                    obj = (clsGetBedReleaseCheckListBizActionVO)objBaseItem.GetBedReleaseCheckList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }

    public class clsGetBedReleseListBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetBedReleseListBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsGetBedReleseListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetBedReleseListBizAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetBedReleseListBizActionVO obj = null;
            try
            {
                obj = (clsGetBedReleseListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseBedReleaseCheckListDAL objBaseItem = clsBaseBedReleaseCheckListDAL.GetInstance();
                    obj = (clsGetBedReleseListBizActionVO)objBaseItem.GetBedReleaseList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }
}
