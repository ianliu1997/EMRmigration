using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.EMR.EMR_Field_Values;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.EMR.EMR_Field_Values
{
    class clsGetEMRFieldValueMasterBizAction:BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetEMRFieldValueMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsGetEMRFieldValueMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetEMRFieldValueMasterBizAction();

            return _Instance;
        }
        

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetFieldValueMasterBizActionVO obj = null;
            try
            {
                obj = (clsGetFieldValueMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseItem = clsBasePatientEMRDataDAL.GetInstance();
                    obj = (clsGetFieldValueMasterBizActionVO)objBaseItem.GetEMRFieldValue(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }

    class clsAddUpdateEMRFieldValueMasterBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsAddUpdateEMRFieldValueMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsAddUpdateEMRFieldValueMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsAddUpdateEMRFieldValueMasterBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddUpdateFieldValueMasterBizActionVO obj = null;
            try
            {
                obj = (clsAddUpdateFieldValueMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseItem = clsBasePatientEMRDataDAL.GetInstance();
                    obj = (clsAddUpdateFieldValueMasterBizActionVO)objBaseItem.AddUpdateEMRFieldValue(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }

    class clsUpdateStatusEMRFieldValueMasterBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsUpdateStatusEMRFieldValueMasterBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsUpdateStatusEMRFieldValueMasterBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsUpdateStatusEMRFieldValueMasterBizAction();
            }
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateStatusFieldValueMasterBizActionVO obj = null;
            int ResultSet = 0;

            try
            {
                obj = (clsUpdateStatusFieldValueMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseItem = clsBasePatientEMRDataDAL.GetInstance();
                    obj = (clsUpdateStatusFieldValueMasterBizActionVO)objBaseItem.UpdateStatusEMRFieldValue(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return obj;
        }
    }

    class clsGetUsedForMasterBizAction : BizAction 
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetUsedForMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsGetUsedForMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetUsedForMasterBizAction();

            return _Instance;
        }
        

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetUsedForMasterBizActionVO obj = null;
            try
            {
                obj = (clsGetUsedForMasterBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseItem = clsBasePatientEMRDataDAL.GetInstance();
                    obj = (clsGetUsedForMasterBizActionVO)objBaseItem.GetUsedForValue(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return obj;
        }
    }

}
