using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.Master
{
  public class clsGetDoctorScheduleBizAction:BizAction
  {
      //This Region Contains Variables Which are Used At Form Level
      #region Variables Declaration

      LogManager logManager = null;

      //Declare the BaseServAppointmentMasterDAL object
      //Declare the Variable of UserId

      long lngUserId = 0;
      #endregion

      //Constructor For LogError Info
      public clsGetDoctorScheduleBizAction()
      {
          //Create Instance of the LogManager object 
          #region Logging Code

          if (logManager == null)
          {
              logManager = LogManager.GetInstance();

          }
          #endregion
      }


      private static clsGetDoctorScheduleBizAction _Insatnce = null;

      public static BizAction GetInstance()
      {
          if (_Insatnce == null)
          _Insatnce = new clsGetDoctorScheduleBizAction();
          return _Insatnce;
      }


      ///Method Input Appointments: valueObject
      ///Name                   :ProcessRequest    
      ///Type                   :IValueObject
      ///Direction              :input-IvalueObject output-IvalueObject
      ///Method Purpose    :Now Override the ProcessRequest Method of the BusinessAction class 
      
      protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
      {
          bool CurrentMethodExecutionStatus = true;
          clsGetDoctorScheduleBizActionVO obj = null;
          int ResultStatus = 0;
          try
          {
              obj = (clsGetDoctorScheduleBizActionVO)valueObject;
              if (obj != null)
              {
                  clsBaseDoctorDAL objBaseDoctorDAL = clsBaseDoctorDAL.GetInstance();
                  obj = (clsGetDoctorScheduleBizActionVO)objBaseDoctorDAL.GetDoctorSchedule(obj, objUserVO);
              }
          }

          catch (HmsApplicationException HEx)
          {
              CurrentMethodExecutionStatus = false;
          }
          catch (Exception ex)
          {
              CurrentMethodExecutionStatus = false;
              throw;
          }
          finally
          {

          }
          return obj;
          
      }


  }
}
