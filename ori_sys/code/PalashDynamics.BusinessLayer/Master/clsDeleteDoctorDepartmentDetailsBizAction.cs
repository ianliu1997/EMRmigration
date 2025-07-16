using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Master.DoctorMaster;

namespace PalashDynamics.BusinessLayer
{
   internal class clsDeleteDoctorDepartmentDetailsBizAction:BizAction
    {

        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

       public clsDeleteDoctorDepartmentDetailsBizAction()
       {
           //Create Instance of the LogManager object 
           #region Logging Code
           if (logmanager == null)
           {
               logmanager = LogManager.GetInstance();
           }
           #endregion
       }

       private static clsDeleteDoctorDepartmentDetailsBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
           {
               _Instance = new clsDeleteDoctorDepartmentDetailsBizAction();
           }

           return _Instance;
       }

       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           clsDeleteDoctorDepartmentDetailsBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsDeleteDoctorDepartmentDetailsBizActionVO)valueObject;
               if (obj != null)
               {
                   clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                   obj = (clsDeleteDoctorDepartmentDetailsBizActionVO)objBaseDoctorDAL.DeleteDoctorDepartmentDetails(obj, objUserVO);
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
