using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.BusinessLayer.Patient
{
   internal class clsGetPatientPenPusherDetailByIDBizAction:BizAction
    {
        
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetPatientPenPusherDetailByIDBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsGetPatientPenPusherDetailByIDBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
           {
               _Instance = new clsGetPatientPenPusherDetailByIDBizAction();
           }

           return _Instance;
       }

       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {

           clsGetPatientPenPusherDetailByIDBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetPatientPenPusherDetailByIDBizActionVO)valueObject;
               if (obj != null)
               {

                   clsBasePatientDAL objBaseDoctorDAL = clsPatientDAL.GetInstance();
                   obj = (clsGetPatientPenPusherDetailByIDBizActionVO)objBaseDoctorDAL.clsGetPatientPenPusherDetailByID(obj, objUserVO);
               }
           }
           catch (Exception ex)
           {

           }
           finally
           {

           }
           return obj;
       }
    }
}
