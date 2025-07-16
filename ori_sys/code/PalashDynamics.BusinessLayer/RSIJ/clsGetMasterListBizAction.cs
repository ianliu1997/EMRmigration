using System;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.RSIJ;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Administration;
using System.Collections.Generic;
//using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.DMS;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.RSIJ.Master
{
    public class clsGetRSIJMasterListBizAction : BizAction
    {

        private static clsGetRSIJMasterListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRSIJMasterListBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRSIJMasterListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetRSIJMasterListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseRSIJMasterDAL objBaseMasterDAL = clsBaseRSIJMasterDAL.GetInstance();
                    obj = (clsGetRSIJMasterListBizActionVO)objBaseMasterDAL.GetMasterList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            return valueObject;

        }
    }

    internal class clsGetRSIJDoctorDepartmentDetailsBizAction : BizAction
    {

        private static clsGetRSIJDoctorDepartmentDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRSIJDoctorDepartmentDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRSIJDoctorDepartmentDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetRSIJDoctorDepartmentDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseRSIJMasterDAL objBaseMasterDAL = clsBaseRSIJMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetRSIJDoctorDepartmentDetailsBizActionVO)objBaseMasterDAL.GetDoctorDepartmentDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //log error  

            }

            return valueObject;

        }
    }

    internal class clsGetRSIJQueueListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetRSIJQueueListBizAction()
        {

            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsGetRSIJQueueListBizAction _Instance = null;

        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRSIJQueueListBizAction();

            return _Instance;
        }

        ///Method Input Appointments: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose    :Now Override the ProcessRequest Method of the BusinessAction class 

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetRSIJQueueListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetRSIJQueueListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseRSIJMasterDAL objBaseRSIJDAL = clsBaseRSIJMasterDAL.GetInstance();
                    obj = (clsGetRSIJQueueListBizActionVO)objBaseRSIJDAL.GetOPDQueueList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }

            catch (Exception ex)
            {
                throw;
            }
            return obj;

        }
    }
    #region for the diagnosis
    internal class clsGetRSIJDiagnosisMasterBizaction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private clsGetRSIJDiagnosisMasterBizaction()
        {
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
        }

        //The Private declaration
        private static clsGetRSIJDiagnosisMasterBizaction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRSIJDiagnosisMasterBizaction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRSIJDiagnosisMasterBizactionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetRSIJDiagnosisMasterBizactionVO)valueObject;
                if (obj != null)
                {
                    clsBaseRSIJMasterDAL objBaseRSIJDAL = clsBaseRSIJMasterDAL.GetInstance();
                    obj = (clsGetRSIJDiagnosisMasterBizactionVO)objBaseRSIJDAL.GetDiagnosisList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return obj;
        }

    }
    #endregion

    /// <summary>
    /// To get the Drug details for the add drug in EMR.
    /// </summary>
    internal class clsGetRSIJItemListBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRSIJItemListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetRSIJItemListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseRSIJMasterDAL objBaseDAL = clsBaseRSIJMasterDAL.GetInstance();
                    obj = (clsGetRSIJItemListBizActionVO)objBaseDAL.GetItemList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return obj;
        }

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion

        private clsGetRSIJItemListBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetRSIJItemListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRSIJItemListBizAction();
            return _Instance;
        }
    }

    //
    public class clsGetRSIJLaboratoryServiceBizAction : BizAction
    {

        public static clsGetRSIJLaboratoryServiceBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRSIJLaboratoryServiceBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRSIJLaboratoryServiceBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetRSIJLaboratoryServiceBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseRSIJMasterDAL objBaseDAL = clsBaseRSIJMasterDAL.GetInstance();
                    obj = (clsGetRSIJLaboratoryServiceBizActionVO)objBaseDAL.GetMasterListByTableName(valueObject, objUserVO);
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


    internal class clsGetRSIJDoctorScheduleTime : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        //constructor For Log Error Info


        public clsGetRSIJDoctorScheduleTime()
        {

            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetRSIJDoctorScheduleTime _Instance = null;
        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRSIJDoctorScheduleTime();
            return _Instance;
        }

        ///Method Input Appointments: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose    :Now Override the ProcessRequest Method of the BusinessAction class 


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            GetDoctorScheduleTimeVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (GetDoctorScheduleTimeVO)valueObject;
                if (obj != null)
                {
                    clsBaseRSIJMasterDAL objBaseRSIJDAL = clsBaseRSIJMasterDAL.GetInstance();
                    obj = (GetDoctorScheduleTimeVO)objBaseRSIJDAL.GetDoctorScheduleTime(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
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

    /// <summary>
    /// Methos is used to give a call to the SQL Server.
    /// </summary>
    internal class ClsGetVisitAdmissionBizAction : BizAction
    {
        LogManager logManager = null;
        long lngUserId = 0;

        private static ClsGetVisitAdmissionBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new ClsGetVisitAdmissionBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            ClsGetVisitAdmissionBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (ClsGetVisitAdmissionBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseDAL = clsBasePatientEMRDataDAL.GetInstance();
                    obj = (ClsGetVisitAdmissionBizActionVO)objBaseDAL.GetVisitAdmission(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }

    /// <summary>
    /// Method is used to get the Image from the DMS Server.
    /// </summary>
    internal class ClsGetImageBizAction : BizAction
    {
        LogManager logManager = null;
        long lngUserId = 0;

        private static ClsGetImageBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new ClsGetImageBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            ClsGetImageBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (ClsGetImageBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBasePatientEMRDataDAL objBaseDAL = clsBasePatientEMRDataDAL.GetInstance();
                    obj = (ClsGetImageBizActionVO)objBaseDAL.GetImage(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }
}
