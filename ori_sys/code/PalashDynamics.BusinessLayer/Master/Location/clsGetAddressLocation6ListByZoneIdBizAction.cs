using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using PalashDynamics.ValueObjects.Master.Location;

namespace PalashDynamics.BusinessLayer.Master.Location
{
    internal class clsGetAddressLocation6ListByZoneIdBizAction : BizAction
    {
        private clsGetAddressLocation6ListByZoneIdBizAction()
        {

        }
        private static clsGetAddressLocation6ListByZoneIdBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAddressLocation6ListByZoneIdBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetAddressLocation6ListByZoneIdBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetAddressLocation6ListByZoneIdBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseLocationDAL objBaseDoctorDAL = clsLocationDAL.GetInstance();
                    obj = (clsGetAddressLocation6ListByZoneIdBizActionVO)objBaseDoctorDAL.GetAddressLocation6ListByZoneId(obj, objUserVO);
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
