using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
   public  class clsAddAppointmentBizActionVO:IBizActionValueObject
    {
       private clsAppointmentVO objAppointment = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains ChangePassword Details Which is Added.
        /// </summary>

       public clsAppointmentVO AppointmentDetails
       {
           get { return objAppointment; }
           set { objAppointment = value; }
       }
       private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>  

       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }


       #region IBizAction Members
       /// <summary>
       /// Retuns the bizAction Class Name.
       /// </summary>
       /// <returns></returns>
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsAddAppointmentBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           throw new NotImplementedException();
       }

       #endregion
    }

   public class clsCheckPatientDuplicacyAppointmentBizActionVO : IBizActionValueObject
   {
       #region IBizActionValueObject Members
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsCheckPatientDuplicacyAppointmentBizAction";
       }

       #endregion
       #region IValueObject Members
       public string ToXml()
       {
           return this.ToString();
       }
       #endregion
       public bool ResultStatus { get; set; }
       private int _SuccessStatus;
       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private clsPatientVO objPatient = null;
       public clsPatientVO PatientDetails
       {
           get { return objPatient; }
           set { objPatient = value; }
       }

       private List<clsPatientGeneralVO> Obj = null;
       public List<clsPatientGeneralVO> PatientList
       {
           get { return Obj; }
           set { Obj = value; }
       }

       public bool PatientEditMode { get; set; }

       private clsAppointmentVO objAppointment = null;
       public clsAppointmentVO AppointmentDetails
       {
           get { return objAppointment; }
           set { objAppointment = value; }
       }

   }
}
