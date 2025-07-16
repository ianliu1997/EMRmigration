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
    class clsGetAppointmentByMrNoBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level

        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //Constructor for  Log Error Info
        public clsGetAppointmentByMrNoBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();

            }
            #endregion
        }

        private static clsGetAppointmentByMrNoBizAction _Instance = null;

        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAppointmentByMrNoBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetAppointmentByMrNoBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetAppointmentByMrNoBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseAppointmentDAL objBaseAppointmentDAL = clsBaseAppointmentDAL.GetInstance();
                    obj = (clsGetAppointmentByMrNoBizActionVO)objBaseAppointmentDAL.GetAppointmentBYMrNo(obj, objUserVO);
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
