using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.NursingStation;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.NursingStation;

namespace PalashDynamics.BusinessLayer.NursingStation
{
    internal class clsDrugAdministrationChartBizAction : BizAction
    {
         #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsDrugAdministrationChartBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if(logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsDrugAdministrationChartBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if(_Instance == null)
                _Instance = new clsDrugAdministrationChartBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsDrugAdministrationChartBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsDrugAdministrationChartBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if(obj != null)
                {
                    clsBaseDrugAdministrationChartDAL objBaseDAL = clsBaseDrugAdministrationChartDAL.GetInstance();
                    obj = (clsDrugAdministrationChartBizActionVO)objBaseDAL.GetCurrentPrescriptionList(obj, objUserVO);
                }
            }
            catch(HmsApplicationException HEx)
            {
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsGetDrugListForDrugChartBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsGetDrugListForDrugChartBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetDrugListForDrugChartBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDrugListForDrugChartBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDrugListForDrugChartBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDrugListForDrugChartBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDrugAdministrationChartDAL objBaseDAL = clsBaseDrugAdministrationChartDAL.GetInstance();
                    obj = (clsGetDrugListForDrugChartBizActionVO)objBaseDAL.GetDrugListForDrugChart(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
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

    internal class clsSaveDrugFeedingDetailsBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsSaveDrugFeedingDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsSaveDrugFeedingDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsSaveDrugFeedingDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsSaveDrugFeedingDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsSaveDrugFeedingDetailsBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDrugAdministrationChartDAL objBaseDAL = clsBaseDrugAdministrationChartDAL.GetInstance();
                    obj = (clsSaveDrugFeedingDetailsBizActionVO)objBaseDAL.SaveDrugFeedingDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
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

    internal class clsGetFeedingDetailsBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsGetFeedingDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetFeedingDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetFeedingDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetFeedingDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetFeedingDetailsBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDrugAdministrationChartDAL objBaseDAL = clsBaseDrugAdministrationChartDAL.GetInstance();
                    obj = (clsGetFeedingDetailsBizActionVO)objBaseDAL.GetDrugFeedingDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
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
   
    internal class clsUpdateFeedingDetailsIsFreezeBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsUpdateFeedingDetailsIsFreezeBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsUpdateFeedingDetailsIsFreezeBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateFeedingDetailsIsFreezeBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsUpdateFeedingDetailsIsFreezeBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsUpdateFeedingDetailsIsFreezeBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDrugAdministrationChartDAL objBaseDAL = clsBaseDrugAdministrationChartDAL.GetInstance();
                    obj = (clsUpdateFeedingDetailsIsFreezeBizActionVO)objBaseDAL.UpdateFeedingDetailsFreeze(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
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
