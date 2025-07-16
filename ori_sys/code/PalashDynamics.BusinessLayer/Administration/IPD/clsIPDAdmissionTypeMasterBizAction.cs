using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    public class clsIPDGetAdmissionTypeMasterBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDGetAdmissionTypeMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDGetAdmissionTypeMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDGetAdmissionTypeMasterBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDGetAdmissionTypeMasterBizActionVO obj = null;
            try
            {
                obj = (clsIPDGetAdmissionTypeMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDGetAdmissionTypeMasterBizActionVO)objBaseItem.GetAdmissionTypeMasterList(obj, objUserVO);
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

    public class clsIPDAddUpdateAdmissionTypeMasterBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDAddUpdateAdmissionTypeMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDAddUpdateAdmissionTypeMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDAddUpdateAdmissionTypeMasterBizAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDAddUpdateAdmissionTypeMasterBizActionVO obj = null;
            try
            {
                obj = (clsIPDAddUpdateAdmissionTypeMasterBizActionVO)valueObject;
                clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                if (obj.IsStatus == true)
                {
                    obj = (clsIPDAddUpdateAdmissionTypeMasterBizActionVO)objBaseItem.UpdateAdmissionTypeMasterStatus(obj, objUserVO);
                }
                else if (obj != null)
                {
                    obj = (clsIPDAddUpdateAdmissionTypeMasterBizActionVO)objBaseItem.AddUpdateAdmissionTypeMaster(obj, objUserVO);
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
