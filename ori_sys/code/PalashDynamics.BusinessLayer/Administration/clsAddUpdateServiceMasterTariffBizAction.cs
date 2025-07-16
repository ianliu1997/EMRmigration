using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
namespace PalashDynamics.BusinessLayer.Administration
{
    class clsAddUpdateServiceMasterTariffBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private static clsAddUpdateServiceMasterTariffBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateServiceMasterTariffBizAction();

            return _Instance;
        }
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddUpdateServiceMasterTariffBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddUpdateServiceMasterTariffBizActionVO)valueObject;
                clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                if (obj != null)
                {
                    if (obj.IsModifyTariffClassRates == true)
                    {
                        obj = (clsAddUpdateServiceMasterTariffBizActionVO)objBaseDAL.ModifyServiceClassRates(obj, objUserVO);
                    }
                    else
                    {
                        obj = (clsAddUpdateServiceMasterTariffBizActionVO)objBaseDAL.AddUpdateServiceClassRates(obj, objUserVO);
                    }
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }

    class clsAddUpdateDoctorServiceRateCategoryBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private static clsAddUpdateDoctorServiceRateCategoryBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateDoctorServiceRateCategoryBizAction();

            return _Instance;
        }
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddUpdateDoctorServiceRateCategoryBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddUpdateDoctorServiceRateCategoryBizActionVO)valueObject;
                clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                if (obj != null)
                {
                    obj = (clsAddUpdateDoctorServiceRateCategoryBizActionVO)objBaseDAL.AddUpdateDoctorServiceRateCategory(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }

    class clsGetFrontPannelDataGridListBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private static clsGetFrontPannelDataGridListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetFrontPannelDataGridListBizAction();

            return _Instance;
        }
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetFrontPannelDataGridListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetFrontPannelDataGridListBizActionVO)valueObject;
                clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                if (obj != null)
                {
                    if (obj.CategoryID == 0)
                        obj = (clsGetFrontPannelDataGridListBizActionVO)objBaseDAL.GetFrontPannelDataGridList(obj, objUserVO);
                    else
                        obj = (clsGetFrontPannelDataGridListBizActionVO)objBaseDAL.GetFrontPannelDataGridByID(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }

    class clsGetUnSelectedRecordForCategoryComboBoxBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private static clsGetUnSelectedRecordForCategoryComboBoxBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUnSelectedRecordForCategoryComboBoxBizAction();

            return _Instance;
        }
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetUnSelectedRecordForCategoryComboBoxBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetUnSelectedRecordForCategoryComboBoxBizActionVO)valueObject;
                clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                if (obj != null)
                {
                    if (obj.IsFromDocSerLinling)
                    {
                        obj = (clsGetUnSelectedRecordForCategoryComboBoxBizActionVO)objBaseDAL.GetUnSelectedRecordForClinicComboBox(obj, objUserVO);
                    }
                    else
                    {

                        obj = (clsGetUnSelectedRecordForCategoryComboBoxBizActionVO)objBaseDAL.GetUnSelectedRecordForCategoryComboBox(obj, objUserVO);
                    }
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }

    #region GST Details added by Ashish Z. on dated 24062017
    class clsAddUpdateServiceTaxBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private static clsAddUpdateServiceTaxBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateServiceTaxBizAction();

            return _Instance;
        }
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddUpdateServiceTaxBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddUpdateServiceTaxBizActionVO)valueObject;
                clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                if (obj != null)
                {
                    obj = (clsAddUpdateServiceTaxBizActionVO)objBaseDAL.AddUpdateSeviceTaxDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }

    class clsGetServiceTaxDetailsBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private static clsGetServiceTaxDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServiceTaxDetailsBizAction();

            return _Instance;
        }
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetServiceTaxDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetServiceTaxDetailsBizActionVO)valueObject;
                clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                if (obj != null)
                {
                    obj = (clsGetServiceTaxDetailsBizActionVO)objBaseDAL.GetServiceTaxDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }
    #endregion

}
