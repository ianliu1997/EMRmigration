using System;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.EMR
{
    class clsEMRTemplateFileLinkBizActionVO
    {
    }

    class clsUploadPatientImageBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsUploadPatientImageBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsUploadPatientImageBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUploadPatientImageBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUploadPatientImageBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUploadPatientImageBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    obj = (clsUploadPatientImageBizActionVO)objBaseDAL.UploadPatientImage(obj, objUserVO);
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

    class clsGetUploadPatientImageBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetUploadPatientImageBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetUploadPatientImageBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUploadPatientImageBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetUploadPatientImageBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetUploadPatientImageBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    obj = (clsGetUploadPatientImageBizActionVO)objBaseDAL.GetUploadPatientImage(obj, objUserVO);
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

    class clsUpdateUploadPatientImageBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private clsUpdateUploadPatientImageBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsUpdateUploadPatientImageBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateUploadPatientImageBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUpdateUploadPatientImageBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateUploadPatientImageBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    obj = (clsUpdateUploadPatientImageBizActionVO)objBaseDAL.UpdateUploadPatientImage(obj, objUserVO);
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



    class clsUploadPatientHystoLapBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsUploadPatientHystoLapBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsUploadPatientHystoLapBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUploadPatientHystoLapBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsUploadPatientHystoLapBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUploadPatientHystoLapBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    obj = (clsUploadPatientHystoLapBizActionVO)objBaseDAL.UploadPatientImageFromHystroScopy(obj, objUserVO);
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


    class clsdeleteUploadPatientHystoLapBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsdeleteUploadPatientHystoLapBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsdeleteUploadPatientHystoLapBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsdeleteUploadPatientHystoLapBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsDeleteUploadPatientHystoLapBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsDeleteUploadPatientHystoLapBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    obj = (clsDeleteUploadPatientHystoLapBizActionVO)objBaseDAL.deleteUploadPatientImageFromHystroScopy(obj, objUserVO);
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


    class clsGetPatientUploadedImagetHystoLapBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetPatientUploadedImagetHystoLapBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsGetPatientUploadedImagetHystoLapBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientUploadedImagetHystoLapBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientUploadedImagetHystoLapBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientUploadedImagetHystoLapBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseEMRDAL objBaseDAL = clsBaseEMRDAL.GetInstance();
                    obj = (clsGetPatientUploadedImagetHystoLapBizActionVO)objBaseDAL.GetUploadPatientImageFromHystroScopy(obj, objUserVO);
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

}
