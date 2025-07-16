using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using PalashDynamics.ValueObjects.Master.DoctorPayment;

namespace PalashDynamics.BusinessLayer.Master.DoctorPayment
{
    internal class clsGetDoctorShareAmountBizAction : BizAction
    {
        private clsGetDoctorShareAmountBizAction()
        {
        }

        private static clsGetDoctorShareAmountBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorShareAmountBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorShareAmountBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorShareAmountBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsGetDoctorShareAmountBizActionVO)objBaseDoctorDAL.GetDoctorShareAmount(obj, objUserVO);
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
