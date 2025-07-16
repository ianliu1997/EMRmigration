using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer;
using System;

namespace PalashDynamics.BusinessLayer.OutPatientDepartment.QueueManagement
{
  public  class clsUpdatePatientSortOrderBizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of AppointmentId

        long lngAppointmentId = 0;
        #endregion

        public clsUpdatePatientSortOrderBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();

            }
            #endregion
        }

        //The Private declaration

        private static clsUpdatePatientSortOrderBizAction _Instance = null;

        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdatePatientSortOrderBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsUpdatePatientSortOrderBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdatePatientSortOrderBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseQueueDAL objBaseQueueDAL = clsBaseQueueDAL.GetInstance();
                    obj = (clsUpdatePatientSortOrderBizActionVO)objBaseQueueDAL.UpdateQueueSortOrder(obj, objUserVO);

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


    public  class clsUpdateDoctorInQueueBizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of AppointmentId

        long lngAppointmentId = 0;
        #endregion

        public clsUpdateDoctorInQueueBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();

            }
            #endregion
        }

        //The Private declaration

        private static clsUpdateDoctorInQueueBizAction _Instance = null;

        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateDoctorInQueueBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsUpdateDoctorInQueueBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateDoctorInQueueBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseQueueDAL objBaseQueueDAL = clsBaseQueueDAL.GetInstance();
                    obj = (clsUpdateDoctorInQueueBizActionVO)objBaseQueueDAL.UpdateDoctorInQueue(obj, objUserVO);

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
