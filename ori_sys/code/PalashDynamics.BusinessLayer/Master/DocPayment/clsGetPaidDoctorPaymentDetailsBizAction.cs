using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using PalashDynamics.ValueObjects.Master.DoctorPayment;


namespace PalashDynamics.BusinessLayer.Master.DocPayment
{
    internal class clsGetPaidDoctorPaymentDetailsBizAction : BizAction
    {

        private clsGetPaidDoctorPaymentDetailsBizAction()
        {
        }

        private static clsGetPaidDoctorPaymentDetailsBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPaidDoctorPaymentDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetPaidDoctorPaymentDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPaidDoctorPaymentDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsGetPaidDoctorPaymentDetailsBizActionVO)objBaseDoctorDAL.GetPaidDoctorPaymentDetails(obj, objUserVO);
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
