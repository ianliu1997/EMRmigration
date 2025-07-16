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
    class clsAddCancelAppReasonBizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of AppointmentId
        long lngAppointmentId = 0;
        #endregion

        public clsAddCancelAppReasonBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();

            }
            #endregion
        }

        //The Private declaration

        private static clsAddCancelAppReasonBizAction _Instance = null;

        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddCancelAppReasonBizAction();
            return _Instance;
        }
        
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddCancelAppReasonBizActionVO  obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddCancelAppReasonBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseAppointmentDAL objBaseAppointmentDAL = clsBaseAppointmentDAL.GetInstance();
                    obj = (clsAddCancelAppReasonBizActionVO)objBaseAppointmentDAL.AddCancelAppReason(obj, objUserVO);

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
