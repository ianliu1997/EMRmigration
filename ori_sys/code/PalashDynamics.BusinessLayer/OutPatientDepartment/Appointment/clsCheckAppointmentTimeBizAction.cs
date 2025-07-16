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
   internal class clsCheckAppointmentTimeBizAction:BizAction
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

        public clsCheckAppointmentTimeBizAction()
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

        private static clsCheckAppointmentTimeBizAction _Instance = null;

        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsCheckAppointmentTimeBizAction();
            return _Instance;
        }




        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsCheckAppointmentTimeBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsCheckAppointmentTimeBizActionVO)valueObject;

                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseAppointmentDAL objBaseAppointmentDAL = clsBaseAppointmentDAL.GetInstance();
                    obj = (clsCheckAppointmentTimeBizActionVO)objBaseAppointmentDAL.CheckAppointmentTime(obj, objUserVO);
                }

            }

            catch (HmsApplicationException HEx)
            {

                throw;
            }

            catch (Exception ex)
            {

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
