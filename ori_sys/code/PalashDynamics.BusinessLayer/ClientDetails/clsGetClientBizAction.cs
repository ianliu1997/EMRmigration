using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.ClientDetails;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ClientDetails;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.ClientDetails
{
    class clsGetClientBizAction : BizAction
    {

        #region Variable Declaration
             LogManager logManager = null;
        private static clsGetClientBizAction _instance = null;
        #endregion


             private clsGetClientBizAction()
             {
                 if (logManager == null) logManager = LogManager.GetInstance();


             }

             public static BizAction GetInstance()
             {
                 if (_instance == null) _instance = new clsGetClientBizAction();

                 return _instance;
             }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
           // throw new NotImplementedException();


            clsGetClientBizActionVO obj = null;

            try
            {
                obj = (clsGetClientBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseClientDetailsDAL objBaseDAL = clsBaseClientDetailsDAL.GetInstance();

                    obj =(clsGetClientBizActionVO)objBaseDAL.GetClient(valueObject, objUserVO);

                }
            }
            catch (Exception ex)
            {

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }

            return obj;
        }
    }
}
