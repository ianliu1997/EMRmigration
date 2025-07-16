using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.IPD
{
    internal class clsIPDGetBedCensusAndNonCensusListBizAction: BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDGetBedCensusAndNonCensusListBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDGetBedCensusAndNonCensusListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDGetBedCensusAndNonCensusListBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDGetBedCensusAndNonCensusListBizActionVO obj = null;
            try
            {
                obj = (clsIPDGetBedCensusAndNonCensusListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDGetBedCensusAndNonCensusListBizActionVO)objBaseItem.GetBedCensusAndNonCensusList(obj, objUserVO);
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
