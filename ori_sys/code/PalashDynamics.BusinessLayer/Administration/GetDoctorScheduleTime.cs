using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration
{
   internal class GetDoctorScheduleTime:BizAction
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


        public GetDoctorScheduleTime()
        {

            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static GetDoctorScheduleTime _Instance = null;
        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new GetDoctorScheduleTime();
            return _Instance;
        }

        ///Method Input Appointments: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose    :Now Override the ProcessRequest Method of the BusinessAction class 


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            GetDoctorScheduleTimeVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (GetDoctorScheduleTimeVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorScheduleDAL objBaseDoctorScheduleDAL = clsBaseDoctorScheduleDAL.GetInstance();
                    obj = (GetDoctorScheduleTimeVO)objBaseDoctorScheduleDAL.GetDoctorScheduleTime(obj, objUserVO);
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
