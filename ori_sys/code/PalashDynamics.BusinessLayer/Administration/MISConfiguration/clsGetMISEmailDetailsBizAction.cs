using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.MISConfiguration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.Administration.MISConfiguration
{
    internal class clsGetMISEmailDetailsBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsGetMISEmailDetailsBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }


        private static clsGetMISEmailDetailsBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsGetMISEmailDetailsBizAction();
            }
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetMISEmailDetailsBizActionVO obj = null;
            int Resultset = 0;

            try
            {
                obj = (clsGetMISEmailDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMISConfigDAL objBaseDAL = clsBaseMISConfigDAL.GetInstance();
                    obj = (clsGetMISEmailDetailsBizActionVO)objBaseDAL.GetAutoEmailForMIS(obj, objUserVO);
                }
            }
            catch (HmsApplicationException Hex)
            {
        
            }
            catch (Exception ex)
            {
             
                //log error  
                logmanager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }
    }

    internal class GetMISDetailsFromCriteriaBizAction : BizAction
    {
              #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public GetMISDetailsFromCriteriaBizAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }


        private static GetMISDetailsFromCriteriaBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new GetMISDetailsFromCriteriaBizAction();
            }
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            GetMISDetailsFromCriteriaBizActionVO obj = null;
            int Resultset = 0;

            try
            {
                obj = (GetMISDetailsFromCriteriaBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMISConfigDAL objBaseDAL = clsBaseMISConfigDAL.GetInstance();
                    obj = (GetMISDetailsFromCriteriaBizActionVO)objBaseDAL.GetMISDetailsFromCriteria(obj, objUserVO);
                }
            }
            catch (HmsApplicationException Hex)
            {
        
            }
            catch (Exception ex)
            {
             
                //log error  
                logmanager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }
    }


    internal class clsGetMISReportDetailsBiZAction : BizAction
    {
           #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsGetMISReportDetailsBiZAction()
        {
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }


        private static clsGetMISReportDetailsBiZAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsGetMISReportDetailsBiZAction();
            }
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetMISReportDetailsBiZActionVO obj = null;
            int Resultset = 0;

            try
            {
                obj = (clsGetMISReportDetailsBiZActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMISConfigDAL objBaseDAL = clsBaseMISConfigDAL.GetInstance();
                    obj = (clsGetMISReportDetailsBiZActionVO)objBaseDAL.GetMISReportDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException Hex)
            {
        
            }
            catch (Exception ex)
            {
             
                //log error  
                logmanager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }
    }
}
