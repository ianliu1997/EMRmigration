using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;

namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsDeleteDoctorShareForOverRideExistingShare : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsDeleteDoctorShareForOverRideExistingShare()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsDeleteDoctorShareForOverRideExistingShare _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsDeleteDoctorShareForOverRideExistingShare();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsDeleteDoctorShareForOverRideExistingShareVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsDeleteDoctorShareForOverRideExistingShareVO)valueObject;
                if (obj != null)
                {
                    clsBaseDoctorDAL objBaseDoctorDAL = clsDoctorDAL.GetInstance();
                    obj = (clsDeleteDoctorShareForOverRideExistingShareVO)objBaseDoctorDAL.DeleteExistingDoctorShareInfo(obj, objUserVO);
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
