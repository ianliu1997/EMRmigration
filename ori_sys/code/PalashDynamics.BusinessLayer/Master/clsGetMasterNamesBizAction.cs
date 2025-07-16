using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;
using System.Data.Common;

namespace PalashDynamics.BusinessLayer.Master
{
    class clsGetMasterNamesBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetMasterNamesBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetMasterNamesBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsGetMasterNamesBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetMasterNamesBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetMasterNamesBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDoctorDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetMasterNamesBizActionVO)objBaseDoctorDAL.GetMasterNames(obj, objUserVO);
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

    class clsGetDatatoPrintMasterBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetDatatoPrintMasterBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetDatatoPrintMasterBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsGetDatatoPrintMasterBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDatatoPrintMasterBizActionVO obj = null;

            int ResultStatus = 0;
            try
            {
                obj = (clsGetDatatoPrintMasterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDoctorDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetDatatoPrintMasterBizActionVO)objBaseDoctorDAL.GetDataToPrint(obj, objUserVO);
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
