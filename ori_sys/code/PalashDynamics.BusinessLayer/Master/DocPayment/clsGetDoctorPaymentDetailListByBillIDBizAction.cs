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
    internal class clsGetDoctorPaymentDetailListByBillIDBizAction : BizAction
    {
        private clsGetDoctorPaymentDetailListByBillIDBizAction()
        {
        }
        private static clsGetDoctorPaymentDetailListByBillIDBizAction _Instance = null;
     public static BizAction GetInstance()
     {
         if (_Instance == null)
             _Instance = new clsGetDoctorPaymentDetailListByBillIDBizAction();

         return _Instance;
     }

     protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
     {
         bool CurrentMethodExecutionStatus = true;

         clsGetDoctorPaymentDetailListByBillIDBizActionVO obj = null;
         int ResultStatus = 0;
         try
         {
             obj = (clsGetDoctorPaymentDetailListByBillIDBizActionVO)valueObject;
             if (obj != null)
             {
                 clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                 obj = (clsGetDoctorPaymentDetailListByBillIDBizActionVO)objBaseDoctorDAL.GetDoctorBillPaymentDetailListByBillID(obj, objUserVO);
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
