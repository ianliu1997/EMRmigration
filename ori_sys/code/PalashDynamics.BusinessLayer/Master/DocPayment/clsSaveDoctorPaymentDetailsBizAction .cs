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
    class clsSaveDoctorPaymentDetailsBizAction : BizAction
    {

        private clsSaveDoctorPaymentDetailsBizAction()
        {
        }

        private static clsSaveDoctorPaymentDetailsBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsSaveDoctorPaymentDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsSaveDoctorPaymentDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsSaveDoctorPaymentDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsSaveDoctorPaymentDetailsBizActionVO)objBaseDoctorDAL.SaveDoctorPaymentDetailList(obj, objUserVO);
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

    class clsSaveDoctorSettlePaymentDetailsBizAction : BizAction
    {

        private clsSaveDoctorSettlePaymentDetailsBizAction()
        {
        }

        private static clsSaveDoctorSettlePaymentDetailsBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsSaveDoctorSettlePaymentDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsSaveDoctorSettlePaymentDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsSaveDoctorSettlePaymentDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsBaseDoctorDAL.GetInstance();
                    obj = (clsSaveDoctorSettlePaymentDetailsBizActionVO)objBaseDoctorDAL.SaveDoctorSettlePaymentDetailList(obj, objUserVO);
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
