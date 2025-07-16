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

namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsGetDoctorWaiverDetailListByIDBizAction: BizAction
    {
        private clsGetDoctorWaiverDetailListByIDBizAction()
        {
        }
        private static clsGetDoctorWaiverDetailListByIDBizAction _Instance = null;
     public static BizAction GetInstance()
     {
         if (_Instance == null)
             _Instance = new clsGetDoctorWaiverDetailListByIDBizAction();

         return _Instance;
     }

     protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
     {
         bool CurrentMethodExecutionStatus = true;

         clsGetDoctorWaiverDetailListByIDBizActionVO obj = null;
         int ResultStatus = 0;
         try
         {
             obj = (clsGetDoctorWaiverDetailListByIDBizActionVO)valueObject;
             if (obj != null)
             {
                 clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                 obj = (clsGetDoctorWaiverDetailListByIDBizActionVO)objBaseDoctorDAL.GetDoctorWaiverDetailListByID(obj, objUserVO);
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
