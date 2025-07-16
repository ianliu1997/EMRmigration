using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer;


namespace PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment
{
  internal  class clsGetDoctorTimeBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info

        public clsGetDoctorTimeBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetDoctorTimeBizAction _Instance = null;



        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorTimeBizAction();
            return _Instance;
        }

        ///Method Input Appointments: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose    :Now Override the ProcessRequest Method of the BusinessAction class 

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetDoctorTimeBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorTimeBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseAppointmentDAL objBaseAppointmentDAL = clsBaseAppointmentDAL.GetInstance();
                    obj = (clsGetDoctorTimeBizActionVO)objBaseAppointmentDAL.GetDoctorTime(obj, objUserVO);
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
            }
            return obj;
        
        }
     

    }
}
