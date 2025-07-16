using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master.DoctorPayment;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;

namespace PalashDynamics.BusinessLayer.Master.DoctorPayment
{
    internal class clsGetDoctorPaymentListBizAction : BizAction
    {
        private clsGetDoctorPaymentListBizAction()
        {
        }

        private static clsGetDoctorPaymentListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorPaymentListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorPaymentListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorPaymentListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsGetDoctorPaymentListBizActionVO)objBaseDoctorDAL.GetDoctorPaymentBillsDetailList(obj, objUserVO);
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
