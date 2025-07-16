using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer;


namespace PalashDynamics.BusinessLayer.OutPatientDepartment.QueueManagement
{
  internal class clsAddQueueListBizAction:BizAction
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
       private clsAddQueueListBizAction()
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

        private static clsAddQueueListBizAction _Instance = null;

        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddQueueListBizAction();
            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
           clsAddQueueListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddQueueListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseQueueDAL objBaseQueueDAL = clsBaseQueueDAL.GetInstance();
                    obj = (clsAddQueueListBizActionVO)objBaseQueueDAL.AddQueueList(obj, objUserVO);
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
               
            }

            return obj;
        }
        
    }
}
