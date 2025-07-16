using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.MISConfiguration;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.MISConfiguration
{
    internal class clsAddConfig_MISStaffBizAction:BizAction
    {
        #region Variable Declaration

        LogManager logmanager = null;

        long lngUserId = 0;
        #endregion


        public clsAddConfig_MISStaffBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAddConfig_MISStaffBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsAddConfig_MISStaffBizAction();
            }
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddConfig_MISStaffBizActionVO obj = null;

            int Resultset = 0;

            try
            {
                obj = (clsAddConfig_MISStaffBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMISConfigDAL objBaseDal = clsBaseMISConfigDAL.GetInstance();
                    obj = (clsAddConfig_MISStaffBizActionVO)objBaseDal.AddMISConfig(obj, objUserVO);
                }

            }
            catch (HmsApplicationException Hex)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //log error  
                logmanager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;

        }

      }

   
}
