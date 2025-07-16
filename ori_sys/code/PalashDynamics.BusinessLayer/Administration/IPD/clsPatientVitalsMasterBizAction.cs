using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    public class clsAddUpdateIPDPatientVitalsMasterBIzAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsAddUpdateIPDPatientVitalsMasterBIzAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsAddUpdateIPDPatientVitalsMasterBIzAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsAddUpdateIPDPatientVitalsMasterBIzAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddUpdateIPDPatientVitalsMasterBIzActionVO obj = null;
            try
            {
                obj = (clsAddUpdateIPDPatientVitalsMasterBIzActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientVitalsMasterDAL objBaseItem = clsBasePatientVitalsMasterDAL.GetInstance();
                    obj = (clsAddUpdateIPDPatientVitalsMasterBIzActionVO)objBaseItem.AddUpdatePatientVitalMaster(obj, objUserVO);
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

    public class clsGetIPDPatientVitalsMasterBIzAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetIPDPatientVitalsMasterBIzAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsGetIPDPatientVitalsMasterBIzAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetIPDPatientVitalsMasterBIzAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetIPDPatientVitalsMasterBIzActionVO obj = null;
            try
            {
                obj = (clsGetIPDPatientVitalsMasterBIzActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientVitalsMasterDAL objBaseItem = clsBasePatientVitalsMasterDAL.GetInstance();
                    obj = (clsGetIPDPatientVitalsMasterBIzActionVO)objBaseItem.GetPatientVitalMasterList(obj, objUserVO);
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

    public class clsUpdateStatusIPDPatientVitalsMasterBIzAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsUpdateStatusIPDPatientVitalsMasterBIzAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsUpdateStatusIPDPatientVitalsMasterBIzAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsUpdateStatusIPDPatientVitalsMasterBIzAction();
            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsUpdateStatusIPDPatientVitalsMasterBIzActionVO obj = null;
            try
            {
                obj = (clsUpdateStatusIPDPatientVitalsMasterBIzActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientVitalsMasterDAL objBaseItem = clsBasePatientVitalsMasterDAL.GetInstance();
                    obj = (clsUpdateStatusIPDPatientVitalsMasterBIzActionVO)objBaseItem.UpdateStatusPatientVitalMaster(obj, objUserVO);
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
