using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;

namespace PalashDynamics.BusinessLayer.IPD
{
    # region  DAL Added By Kiran for ADD,Update,GetIntakeOutputChartDetails,GetIntakeOutputChartDetailsByPatientID And UpdateStatusIntakeOutputChart
   
    internal class clsAddUpdateIntakeOutputChartAndDetailsBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsAddUpdateIntakeOutputChartAndDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsAddUpdateIntakeOutputChartAndDetailsBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateIntakeOutputChartAndDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddUpdateIntakeOutputChartAndDetailsBizActionVO obj = null;

            try
            {
                obj = (clsAddUpdateIntakeOutputChartAndDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDIntakeOutputChartDAL objBaseDAL = clsBaseIPDIntakeOutputChartDAL.GetInstance();
                    obj = (clsAddUpdateIntakeOutputChartAndDetailsBizActionVO)objBaseDAL.AddUpdateIntakeOutputChart(obj, objUserVO);

                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }
    }

    internal class clsGetIntakeOutputChartDetailsBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsGetIntakeOutputChartDetailsBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetIntakeOutputChartDetailsBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIntakeOutputChartDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetIntakeOutputChartDetailsBizActionVO obj = null;

            try
            {
                obj = (clsGetIntakeOutputChartDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDIntakeOutputChartDAL objBaseDAL = clsBaseIPDIntakeOutputChartDAL.GetInstance();
                    obj = (clsGetIntakeOutputChartDetailsBizActionVO)objBaseDAL.GetIntakeOutputChartDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }




    }

    internal class clsGetIntakeOutputChartDetailsByPatientIDBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsGetIntakeOutputChartDetailsByPatientIDBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetIntakeOutputChartDetailsByPatientIDBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIntakeOutputChartDetailsByPatientIDBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetIntakeOutputChartDetailsByPatientIDBizActionVO obj = null;

            try
            {
                obj = (clsGetIntakeOutputChartDetailsByPatientIDBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDIntakeOutputChartDAL objBaseDAL = clsBaseIPDIntakeOutputChartDAL.GetInstance();
                    obj = (clsGetIntakeOutputChartDetailsByPatientIDBizActionVO)objBaseDAL.GetIntakeOutputChartDetailsByPatientID(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }




    }

    internal class clsUpdateStatusIntakeOutputChartBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsUpdateStatusIntakeOutputChartBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsUpdateStatusIntakeOutputChartBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateStatusIntakeOutputChartBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsUpdateStatusIntakeOutputChartBizActionVO obj = null;

            try
            {
                obj = (clsUpdateStatusIntakeOutputChartBizActionVO)valueObject;
                if (obj != null)
                {
                    if (obj.IsCalledForFreeze == true)
                    {
                        clsBaseIPDIntakeOutputChartDAL objBaseDAL = clsBaseIPDIntakeOutputChartDAL.GetInstance();
                        obj = (clsUpdateStatusIntakeOutputChartBizActionVO)objBaseDAL.UpdateIsFreezedStatus(obj, objUserVO);
                    }
                    else
                    {
                        clsBaseIPDIntakeOutputChartDAL objBaseDAL = clsBaseIPDIntakeOutputChartDAL.GetInstance();
                        obj = (clsUpdateStatusIntakeOutputChartBizActionVO)objBaseDAL.UpdateStatusIntakeOutputChart(obj, objUserVO);
                    }
                    
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }




    }

    #endregion
}
