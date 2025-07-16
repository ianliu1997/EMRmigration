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
    internal class clsGetAddressLocation6BizAction : BizAction
    {
        private clsGetAddressLocation6BizAction()
        {

        }
        private static clsGetAddressLocation6BizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAddressLocation6BizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetAddressLocation6BizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetAddressLocation6BizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseLocationDAL objBaseDoctorDAL = clsLocationDAL.GetInstance();
                    obj = (clsGetAddressLocation6BizActionVO)objBaseDoctorDAL.GetAddressLocation6List(obj, objUserVO);
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
