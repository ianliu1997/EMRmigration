using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.BusinessLayer.Administration.UnitMaster
{
    internal class clsdbDetailsBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsdbDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsdbDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsdbDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsdbDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsdbDetailsBizActionVO)objBaseDAL.AddtoDatabase(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return obj;
        }
    }

    internal class clsGetLicenseDetailsBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetLicenseDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetLicenseDetailsBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetLicenseDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetLicenseDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsGetLicenseDetailsBizActionVO)objBaseDAL.GetLicenseDetails(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {                
             //   throw;
            }
            return obj;
           
        }
    }

    internal class clsUpdateLicenseToBizAction : BizAction 
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsUpdateLicenseToBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateLicenseToBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateLicenseToBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateLicenseToBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                    obj = (clsUpdateLicenseToBizActionVO)objBaseDAL.UpdateLicenseTo(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {                
             //   throw;
            }

            return obj;
        }
    }

    internal class clsAddLicenseToBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddLicenseToBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddLicenseToBizAction();

            return _Instance;
        }
        
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddLicenseToBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                 obj = (clsAddLicenseToBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                 if (obj != null)
                 {
                     clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
                     obj = (clsAddLicenseToBizActionVO)objBaseDAL.AddLicenseTo(obj, objUserVO);
                 }
            }
            catch (Exception ex)
            {               
             //   throw;
            }

            return obj;
        }
    }

   internal class clsAddUnitMasterBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddUnitMasterBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUnitMasterBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddUnitMasterBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddUnitMasterBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUnitMasterDAL objBaseDAL = clsBaseUnitMasterDAL.GetInstance();
                    obj = (clsAddUnitMasterBizActionVO)objBaseDAL.AddUnitMaster(obj, objUserVO);
                }

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
