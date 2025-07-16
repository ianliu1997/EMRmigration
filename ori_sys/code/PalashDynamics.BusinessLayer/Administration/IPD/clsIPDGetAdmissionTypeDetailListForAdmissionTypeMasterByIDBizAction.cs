using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{    
    internal class clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizAction : BizAction
    {

        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizAction();
            }

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseDoctorDAL = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO)objBaseDoctorDAL.GetAdmissionTypeDetailListForAdmissionTypeMasterByAdmissionTypeID(obj, objUserVO);
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
