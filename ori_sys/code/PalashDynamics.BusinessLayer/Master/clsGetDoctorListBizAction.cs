using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;

namespace  PalashDynamics.BusinessLayer.Master
{
 internal  class clsGetDoctorListBizAction:BizAction
    {
     private clsGetDoctorListBizAction()
     {
     }
     private static clsGetDoctorListBizAction  _Instance = null;
     public static BizAction GetInstance()
     {
         if (_Instance == null)
             _Instance = new clsGetDoctorListBizAction();

         return _Instance;
     }

     protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
     {
         bool CurrentMethodExecutionStatus = true;
         clsGetDoctorListBizActionVO obj = null;
         int ResultStatus = 0;
         try
         {
             obj = (clsGetDoctorListBizActionVO)valueObject;
             if (obj != null)
             {
                 clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                 if (obj.IsComboBoxFill == true)
                     obj = (clsGetDoctorListBizActionVO)objBaseDoctorDAL.FillDoctorCombo(obj, objUserVO);
                 else
                 obj = (clsGetDoctorListBizActionVO)objBaseDoctorDAL.GetDoctorWaiverDetailList(obj, objUserVO);
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
