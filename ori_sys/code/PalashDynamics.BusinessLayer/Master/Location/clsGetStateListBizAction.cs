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
    internal class clsGetStateListBizAction : BizAction
    {
        private clsGetStateListBizAction()
        {
        }
        private static clsGetStateListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetStateListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetStateListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetStateListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseLocationDAL objBaseDoctorDAL = clsLocationDAL.GetInstance();
                    obj = (clsGetStateListBizActionVO)objBaseDoctorDAL.GetStateList(obj, objUserVO);
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
