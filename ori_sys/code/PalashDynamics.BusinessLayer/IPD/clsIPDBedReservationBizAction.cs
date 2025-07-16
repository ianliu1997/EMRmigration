using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.IPD
{
    internal class clsAddIPDBedReservationBizAction : BizAction
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
        private clsAddIPDBedReservationBizAction()
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
        private static clsAddIPDBedReservationBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddIPDBedReservationBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddIPDBedReservationBizActionVO obj = null;

            try
            {
                obj = (clsAddIPDBedReservationBizActionVO)valueObject;
                if (obj.BedDetails.IsFromReminderLog == true)
                {
                    clsBaseReservationDAL objBaseDAL = clsBaseReservationDAL.GetInstance();
                    obj = (clsAddIPDBedReservationBizActionVO)objBaseDAL.AddIPDPatientReminderLog(obj, objUserVO);
                }
                if (obj.BedDetails.IsFromReminderLogForGet == true)
                {
                    clsBaseReservationDAL objBaseDAL = clsBaseReservationDAL.GetInstance();
                    obj = (clsAddIPDBedReservationBizActionVO)objBaseDAL.GetIPDPatientReminderLog(obj, objUserVO);
                }
                else if (obj != null)
                {                    
                    clsBaseReservationDAL objBaseDAL = clsBaseReservationDAL.GetInstance();
                    obj = (clsAddIPDBedReservationBizActionVO)objBaseDAL.AddIPDBedReservation(obj, objUserVO);
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

    internal class clsGetIPDBedReservationStatusBizAction : BizAction
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
        private clsGetIPDBedReservationStatusBizAction()
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
        private static clsGetIPDBedReservationStatusBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIPDBedReservationStatusBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetIPDBedReservationStatusBizActionVO obj = null;

            try
            {
                obj = (clsGetIPDBedReservationStatusBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseReservationDAL objBaseDAL = clsBaseReservationDAL.GetInstance();
                    obj = (clsGetIPDBedReservationStatusBizActionVO)objBaseDAL.GetIPDBedReservationStatus(obj, objUserVO);
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

    internal class clsGetIPDBedReservationListBizAction : BizAction
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
        private clsGetIPDBedReservationListBizAction()
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
        private static clsGetIPDBedReservationListBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIPDBedReservationListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetIPDBedReservationListBizActionVO obj = null;

            try
            {
                obj = (clsGetIPDBedReservationListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseReservationDAL objBaseDAL = clsBaseReservationDAL.GetInstance();
                    obj = (clsGetIPDBedReservationListBizActionVO)objBaseDAL.GetIPDBedReservationList(obj, objUserVO);
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

    internal class clsAddIPDBedUnReservationBizAction : BizAction
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
        private clsAddIPDBedUnReservationBizAction()
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
        private static clsAddIPDBedUnReservationBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddIPDBedUnReservationBizAction();

            return _Instance;
        }
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddIPDBedUnReservationBizActionVO obj = null;

            try
            {
                obj = (clsAddIPDBedUnReservationBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseUnReservationDAL objBaseDAL = clsBaseUnReservationDAL.GetInstance();
                    obj = (clsAddIPDBedUnReservationBizActionVO)objBaseDAL.AddIPDBedUnReservation(obj, objUserVO);
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
}
