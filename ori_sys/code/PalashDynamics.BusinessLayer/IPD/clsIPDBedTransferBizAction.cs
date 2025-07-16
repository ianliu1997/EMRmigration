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
    internal class clsGetIPDPatientDetailsBizAction : BizAction
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
        private clsGetIPDPatientDetailsBizAction()
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
        private static clsGetIPDPatientDetailsBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIPDPatientDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetIPDPatientDetailsBizActionVO obj = null;

            try
            {
                obj = (clsGetIPDPatientDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDDAL objBaseDAL = clsBaseIPDDAL.GetInstance();
                    obj = (clsGetIPDPatientDetailsBizActionVO)objBaseDAL.GetIPDPatientDetails(obj, objUserVO);
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

    internal class clsAddIPDBedTransferBizAction : BizAction
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
        private clsAddIPDBedTransferBizAction()
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
        private static clsAddIPDBedTransferBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddIPDBedTransferBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddIPDBedTransferBizActionVO obj = null;

            try
            {
                obj = (clsAddIPDBedTransferBizActionVO)valueObject;
                clsBaseIPDDAL objBaseDAL = clsBaseIPDDAL.GetInstance();
                if (obj.BedDetails.IsMultipleBed == true)
                {
                    obj = (clsAddIPDBedTransferBizActionVO)objBaseDAL.AddMultipleBed(obj, objUserVO);
                }
                else if (obj != null)
                {
                    obj = (clsAddIPDBedTransferBizActionVO)objBaseDAL.AddIPDBedTransfer(obj, objUserVO);
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
    internal class clsAddIPDDischargeBizAction : BizAction
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
        private clsAddIPDDischargeBizAction()
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
        private static clsAddIPDDischargeBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddIPDDischargeBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddIPDDischargeBizActionVO obj = null;

            try
            {
                obj = (clsAddIPDDischargeBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDDAL objBaseDAL = clsBaseIPDDAL.GetInstance();
                    obj = (clsAddIPDDischargeBizActionVO)objBaseDAL.AddIPDDischarge(obj, objUserVO);
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

    internal class clsGetIPDDischargeBizAction : BizAction
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
        private clsGetIPDDischargeBizAction()
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
        private static clsGetIPDDischargeBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIPDDischargeBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetIPDDischargeBizActionVO obj = null;

            try
            {
                obj = (clsGetIPDDischargeBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDDAL objBaseDAL = clsBaseIPDDAL.GetInstance();
                    obj = (clsGetIPDDischargeBizActionVO)objBaseDAL.GetIPDDischargeDetails(obj, objUserVO);
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

    internal class clsGetDischargeStatusBizAction : BizAction
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
        private clsGetDischargeStatusBizAction()
        {
            //Create Instance of the LogManager object 
            //#region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            //#endregion
        }

        //The Private declaration
        private static clsGetDischargeStatusBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDischargeStatusBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetDischargeStatusBizActionVO obj = null;

            try
            {
                obj = (clsGetDischargeStatusBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDDAL objBaseDAL = clsBaseIPDDAL.GetInstance();
                    obj = (clsGetDischargeStatusBizActionVO)objBaseDAL.GetDischargeStatusDetails(obj, objUserVO);
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



    #region Bed Transfer
    internal class clsGetIPDBedTransferBizAction : BizAction
    {
        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private clsGetIPDBedTransferBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsGetIPDBedTransferBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIPDBedTransferBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetIPDBedTransferBizActionVO obj = null;
            try
            {
                obj = (clsGetIPDBedTransferBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDDAL objBaseDAL = clsBaseIPDDAL.GetInstance();
                    obj = (clsGetIPDBedTransferBizActionVO)objBaseDAL.GetIPDBedTransfer(obj, objUserVO);
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
    internal class clsGetIPDWardByClassIDBizAction : BizAction
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
        private clsGetIPDWardByClassIDBizAction()
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
        private static clsGetIPDWardByClassIDBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIPDWardByClassIDBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetIPDWardByClassIDBizActionVO obj = null;
            try
            {
                obj = (clsGetIPDWardByClassIDBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseBedDAL objBaseDAL = clsBaseBedDAL.GetInstance();
                    obj = (clsGetIPDWardByClassIDBizActionVO)objBaseDAL.GetIPDWardByClassID(obj, objUserVO);
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

    internal class clsGetIPDBedTransferListBizAction : BizAction
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
        private clsGetIPDBedTransferListBizAction()
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
        private static clsGetIPDBedTransferListBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIPDBedTransferListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetIPDBedTransferListBizActionVO obj = null;
            try
            {
                obj = (clsGetIPDBedTransferListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseBedDAL objBaseDAL = clsBaseBedDAL.GetInstance();
                    if (obj.IsCheckFinalBill)
                    {
                        obj = (clsGetIPDBedTransferListBizActionVO)objBaseDAL.CheckFinalBillbyPatientID(obj, objUserVO);
                    }
                    else if (obj.IsSelectedPatient == true)
                    {
                        obj = (clsGetIPDBedTransferListBizActionVO)objBaseDAL.GetIPDBedTransferDetailsForSelectedPatient(obj, objUserVO);
                    }
                    else
                    {
                        obj = (clsGetIPDBedTransferListBizActionVO)objBaseDAL.GetIPDBedTransferList(obj, objUserVO);
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

    #region BedUnderMaintence
    internal class clsAddBedUnderMaintenanceBizAction : BizAction
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
        private clsAddBedUnderMaintenanceBizAction()
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
        private static clsAddBedUnderMaintenanceBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddBedUnderMaintenanceBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddBedUnderMaintenanceBizActionVO obj = null;

            try
            {
                obj = (clsAddBedUnderMaintenanceBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseBedDAL objBaseDAL = clsBaseBedDAL.GetInstance();
                    obj = (clsAddBedUnderMaintenanceBizActionVO)objBaseDAL.AddBedUnderMaintenance(obj, objUserVO);
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

    internal class clsAddUpdateReleaseBedUnderMaintenanceBizAction : BizAction
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
        private clsAddUpdateReleaseBedUnderMaintenanceBizAction()
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
        private static clsAddUpdateReleaseBedUnderMaintenanceBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateReleaseBedUnderMaintenanceBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddUpdateReleaseBedUnderMaintenanceBizActionVO obj = null;

            try
            {
                obj = (clsAddUpdateReleaseBedUnderMaintenanceBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseBedDAL objBaseDAL = clsBaseBedDAL.GetInstance();
                    obj = (clsAddUpdateReleaseBedUnderMaintenanceBizActionVO)objBaseDAL.AddUpdateReleaseBedUnderMaintenance(obj, objUserVO);
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

    internal class clsGetReleaseBedUnderMaintenanceListBizAction : BizAction
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
        private clsGetReleaseBedUnderMaintenanceListBizAction()
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
        private static clsGetReleaseBedUnderMaintenanceListBizAction _Instance = null;

        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetReleaseBedUnderMaintenanceListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetReleaseBedUnderMaintenanceListBizActionVO obj = null;

            try
            {
                obj = (clsGetReleaseBedUnderMaintenanceListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseBedDAL objBaseDAL = clsBaseBedDAL.GetInstance();
                    obj = (clsGetReleaseBedUnderMaintenanceListBizActionVO)objBaseDAL.GetReleaseBedUnderMaintenanceList(obj, objUserVO);
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

    #region For Nursing Station

    internal class clsGetBillAndBedByAdmIDAndAdmUnitIDBizAction : BizAction
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
        private clsGetBillAndBedByAdmIDAndAdmUnitIDBizAction()
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
        private static clsGetBillAndBedByAdmIDAndAdmUnitIDBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetBillAndBedByAdmIDAndAdmUnitIDBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO obj = null;

            try
            {
                obj = (clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseBedDAL objBaseDAL = clsBaseBedDAL.GetInstance();
                    obj = (clsGetBillAndBedByAdmIDAndAdmUnitIDBizActionVO)objBaseDAL.GetBillAndBedByAdmIDAndAdmUnitID(obj, objUserVO);
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
