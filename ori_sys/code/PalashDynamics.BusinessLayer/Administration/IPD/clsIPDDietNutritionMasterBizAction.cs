using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    public class clsIPDGetDietNutritionMasterBizAction:BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDGetDietNutritionMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDGetDietNutritionMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDGetDietNutritionMasterBizAction();

            return _Instance;
        }
        
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDGetDietNutritionBizActionVO obj = null;
            try
            {
                obj = (clsIPDGetDietNutritionBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDGetDietNutritionBizActionVO)objBaseItem.GetFoodItemMaster(obj, objUserVO);
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

    public class clsIPDAddUpdateDietNutritionMasterBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDAddUpdateDietNutritionMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDAddUpdateDietNutritionMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDAddUpdateDietNutritionMasterBizAction();            
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDAddUpdateDietNutritionBizActionVO obj = null;
            try
            {
                obj = (clsIPDAddUpdateDietNutritionBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDAddUpdateDietNutritionBizActionVO)objBaseItem.AddUpdateFoodItemMaster(obj, objUserVO);
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

    public class clsIPDUpdateDietNutritionMasterStatusBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsIPDUpdateDietNutritionMasterStatusBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }
        #endregion

        private static clsIPDUpdateDietNutritionMasterStatusBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsIPDUpdateDietNutritionMasterStatusBizAction();
            }
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateDietNutritionStatusBizActionVO obj = null;
            int ResultSet = 0;

            try
            {
                obj = (clsIPDUpdateDietNutritionStatusBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDUpdateDietNutritionStatusBizActionVO)objBaseItem.UpdateStatusFoodItemMaster(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return obj;
        }
    }

    public class clsIPDGetDietPlanMasterBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDGetDietPlanMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDGetDietPlanMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDGetDietPlanMasterBizAction();

            return _Instance;
        }
        
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDGetDietPlanBizActionVO obj = null;
            try
            {
                obj = (clsIPDGetDietPlanBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDGetDietPlanBizActionVO)objBaseItem.GetDietPlanMaster(obj, objUserVO);
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

    public class clsIPDAddUpdateDietPlanMasterBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsIPDAddUpdateDietPlanMasterBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsIPDAddUpdateDietPlanMasterBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsIPDAddUpdateDietPlanMasterBizAction();            
            return _Instance;
        }
            
        protected override ValueObjects.IValueObject  ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsIPDAddUpdateDietPlanBizactionVO obj = null;
            try
            {
                obj = (clsIPDAddUpdateDietPlanBizactionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDAddUpdateDietPlanBizactionVO)objBaseItem.AddUpdateDietPlanMaster(obj, objUserVO);
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

    public class clsIPDUpdateDietPlanMasterBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        #region constructor For Log Error Info
        public clsIPDUpdateDietPlanMasterBizAction()
        {           
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }            
        }
        #endregion

        private static clsIPDUpdateDietPlanMasterBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsIPDUpdateDietPlanMasterBizAction();
            }
            return _Instance;
        }
    
        protected override ValueObjects.IValueObject  ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsIPDUpdateDietPlanStatusBizActionVO obj = null;
            int ResultSet = 0;

            try
            {
                obj = (clsIPDUpdateDietPlanStatusBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseIPDConfigDAL objBaseItem = clsBaseIPDConfigDAL.GetInstance();
                    obj = (clsIPDUpdateDietPlanStatusBizActionVO)objBaseItem.UpdateDietPlanMasterStatus(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return obj;
        }
    }
}
