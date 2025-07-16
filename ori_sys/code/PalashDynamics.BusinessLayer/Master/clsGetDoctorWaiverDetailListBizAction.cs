using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;

namespace PalashDynamics.BusinessLayer
{
    internal class clsGetDoctorWaiverDetailListBizAction : BizAction
    {
        private clsGetDoctorWaiverDetailListBizAction()
     {
     }
        private static clsGetDoctorWaiverDetailListBizAction _Instance = null;
     public static BizAction GetInstance()
     {
         if (_Instance == null)
             _Instance = new clsGetDoctorWaiverDetailListBizAction();

         return _Instance;
     }

     protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
     {
         bool CurrentMethodExecutionStatus = true;

         clsGetDoctorWaiverDetailListBizActionVO obj = null;
         int ResultStatus = 0;
         try
         {
             obj = (clsGetDoctorWaiverDetailListBizActionVO)valueObject;
             if (obj != null)
             {
                 clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                 obj = (clsGetDoctorWaiverDetailListBizActionVO)objBaseDoctorDAL.GetDoctorWaiverDetailList(obj, objUserVO);
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
