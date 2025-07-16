using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.Location;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;

namespace PalashDynamics.BusinessLayer.Master.Location
{
    internal class clsGetCountryListBizAction : BizAction
    {
        private clsGetCountryListBizAction()
        {
        }
        private static clsGetCountryListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCountryListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetCountryListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetCountryListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseLocationDAL objBaseDoctorDAL = clsLocationDAL.GetInstance();
                    obj = (clsGetCountryListBizActionVO)objBaseDoctorDAL.GetCountryList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;

        }
    }
}
