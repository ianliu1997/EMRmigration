using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.EMR.NewEMR;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.EMR.NewEMR
{
    class clsGetServicesBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private clsGetServicesBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetServicesBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServicesBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServicesBizActionVO obj = null;
            try
            {
                obj = (clsGetServicesBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    obj = (clsGetServicesBizActionVO)objBaseDAL.GetPatientPreviousVisitServices(obj, objUserVO);
                }
            }
            catch (HmsApplicationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }
}
