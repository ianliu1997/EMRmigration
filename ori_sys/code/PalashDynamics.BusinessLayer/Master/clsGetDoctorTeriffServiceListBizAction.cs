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
    internal class clsGetDoctorTeriffServiceListBizAction : BizAction
    {
        private clsGetDoctorTeriffServiceListBizAction()
     {
     }
        private static clsGetDoctorTeriffServiceListBizAction _Instance = null;
     public static BizAction GetInstance()
     {
         if (_Instance == null)
             _Instance = new clsGetDoctorTeriffServiceListBizAction();

         return _Instance;
     }

     protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
     {
         bool CurrentMethodExecutionStatus = true;

         clsGetDoctorTariffServiceListBizActionVO obj = null;
         int ResultStatus = 0;
         try
         {
             obj = (clsGetDoctorTariffServiceListBizActionVO)valueObject;
             if (obj != null)
             {
                 clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                 obj = (clsGetDoctorTariffServiceListBizActionVO)objBaseDoctorDAL.GetDoctorTariffServiceDetailList(obj, objUserVO);
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
