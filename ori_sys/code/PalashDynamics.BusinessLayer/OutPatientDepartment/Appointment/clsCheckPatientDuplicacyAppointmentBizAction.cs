using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment
{
    internal class clsCheckPatientDuplicacyAppointmentBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseServAppointmentMasterDAL object
       //Declare the Variable of AppointmentId

       long lngAppointmentId = 0;
       #endregion
       
       //constructor For Log Error Info
       private clsCheckPatientDuplicacyAppointmentBizAction()
       {
           //Create Instance of the LogManager object 

           #region Logging Code
           if (logManager == null)
           {
               logManager = LogManager.GetInstance();

           }
           #endregion
       }

       //The Private declaration


       private static clsCheckPatientDuplicacyAppointmentBizAction _Instance = null;

       ///Method Input Appointments: none
       ///Name:GetInstance       
       ///Type:static
       ///Direction:None
       ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsCheckPatientDuplicacyAppointmentBizAction();
           return _Instance;
       }




       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsCheckPatientDuplicacyAppointmentBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsCheckPatientDuplicacyAppointmentBizActionVO)valueObject;

               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBaseAppointmentDAL objBaseAppointmentDAL = clsBaseAppointmentDAL.GetInstance();
                   obj = (clsCheckPatientDuplicacyAppointmentBizActionVO)objBaseAppointmentDAL.CheckAppointmentPatientDuplicacy(obj, objUserVO);
               }

           }

           catch (HmsApplicationException HEx)
           {
               CurrentMethodExecutionStatus = false;
               throw;
           }

           catch (Exception ex)
           {
               CurrentMethodExecutionStatus = false;
               throw;
           }
           finally
           {
               //log error  
               //logManager.LogInfo(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
           }

           return obj;
       }
    }
}
