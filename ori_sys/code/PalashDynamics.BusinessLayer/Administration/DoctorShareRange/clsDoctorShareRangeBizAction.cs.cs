using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.DoctorShareRange;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Master.DoctorPayment;

namespace PalashDynamics.BusinessLayer.Administration.DoctorShareRange
{
    internal class clsAddDoctorShareRangeBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;

            clsAddDoctorShareRangeBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddDoctorShareRangeBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseBillDAL objBaseDAL = clsBaseBillDAL.GetInstance();
                    if (obj.IsStatusChanged == true)
                    {
                        obj = (clsAddDoctorShareRangeBizActionVO)objBaseDAL.ChangeStatus(obj, objUserVO);
                    }
                    else
                    {
                        obj = (clsAddDoctorShareRangeBizActionVO)objBaseDAL.AddDoctorShareRange(obj, objUserVO);
                    }
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
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private clsAddDoctorShareRangeBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAddDoctorShareRangeBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddDoctorShareRangeBizAction();

            return _Instance;
        }
    }

    internal class clsGetDoctorShareRangeListBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            //clsGetDoctorShareRangeListBizActionVO obj = null;
            clsGetDoctorShareRangeList obj = null;

            int ResultStatus = 0;
            try
            {
                //obj = (clsGetDoctorShareRangeListBizActionVO)valueObject;
                obj = (clsGetDoctorShareRangeList)valueObject;
                
                if (obj != null)
                {
                    clsBaseBillDAL objBaseDAL = clsBaseBillDAL.GetInstance();
                    //obj = (clsGetDoctorShareRangeListBizActionVO)objBaseDAL.GetDoctorShareRangeList(obj, objUserVO);
                    obj = (clsGetDoctorShareRangeList)objBaseDAL.GetDoctorShareRangeList(obj, objUserVO);
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
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private clsGetDoctorShareRangeListBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetDoctorShareRangeListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorShareRangeListBizAction();

            return _Instance;
        }
    }
}