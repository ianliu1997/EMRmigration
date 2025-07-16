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
    internal class clsAddDoctorPaymentDetailsBizAction : BizAction
    {
        private clsAddDoctorPaymentDetailsBizAction()
        {
        }

        private static clsAddDoctorPaymentDetailsBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddDoctorPaymentDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsAddDoctorPaymentDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddDoctorPaymentDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsAddDoctorPaymentDetailsBizActionVO)objBaseDoctorDAL.AddDoctorPaymentDetailList(obj, objUserVO);
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
