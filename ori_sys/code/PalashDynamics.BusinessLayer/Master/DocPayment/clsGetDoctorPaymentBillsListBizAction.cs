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
    internal class clsGetDoctorPaymentBillsListBizAction:BizAction
    {
        private clsGetDoctorPaymentBillsListBizAction()
        {
        }

        private static clsGetDoctorPaymentBillsListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorPaymentBillsListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorPaymentBillsListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorPaymentBillsListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsGetDoctorPaymentBillsListBizActionVO)objBaseDoctorDAL.GetDoctorPaymentBillsDetailList(obj, objUserVO);
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
