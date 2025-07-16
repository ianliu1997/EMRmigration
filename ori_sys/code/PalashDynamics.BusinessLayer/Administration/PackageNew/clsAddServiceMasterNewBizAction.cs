using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.Administration.PackageNew
{
    internal class clsAddServiceMasterNewBizAction : BizAction
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
        private clsAddServiceMasterNewBizAction()
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
        private static clsAddServiceMasterNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddServiceMasterNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddServiceMasterNewBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddServiceMasterNewBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();

                    if (obj.UpdateServiceMasterStatus == true)
                    {
                        obj = (clsAddServiceMasterNewBizActionVO)objBaseDAL.UpdatePackageServiceMasterStatus(obj, objUserVO);
                    }
                    else
                    {
                        obj = (clsAddServiceMasterNewBizActionVO)objBaseDAL.AddPackageServiceMaster(obj, objUserVO);
                    }

                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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

    internal class clsGetServiceMasterListNewBizAction : BizAction
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
        private clsGetServiceMasterListNewBizAction()
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
        private static clsGetServiceMasterListNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServiceMasterListNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetServiceMasterListNewBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetServiceMasterListNewBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();

                    if (obj.GetAllServiceListDetails == true)
                    {
                        obj = (clsGetServiceMasterListNewBizActionVO)objBaseDAL.GetPackageServiceList(obj, objUserVO);
                    }
                    else if (obj.GetAllServiceMasterDetailsForID == true)
                    {
                        obj = (clsGetServiceMasterListNewBizActionVO)objBaseDAL.GetPackageServiceMasterDetailsForId(obj, objUserVO);
                    }
                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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

    internal class clsGetPackageRelationsBizAction : BizAction
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
        private clsGetPackageRelationsBizAction()
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
        private static clsGetPackageRelationsBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageRelationsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPackageRelationsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPackageRelationsBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();


                    obj = (clsGetPackageRelationsBizActionVO)objBaseDAL.GetPackageRelationsList(obj, objUserVO);

                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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

    internal class clsAddPackageServiceNewBizAction : BizAction
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
        private clsAddPackageServiceNewBizAction()
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
        private static clsAddPackageServiceNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddPackageServiceNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddPackageServiceNewBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddPackageServiceNewBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {


                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();


                    if (obj.UpdatePackageFreezeStatus == true)
                    {
                        obj = (clsAddPackageServiceNewBizActionVO)objBaseDAL.UpdatePackageFreezeStatusNew(obj, objUserVO);
                    }
                    else if (obj.UpdatePackgeApproveStatus == true)
                    {
                        obj = (clsAddPackageServiceNewBizActionVO)objBaseDAL.UpdatePackgeApproveStatusNew(obj, objUserVO);
                    }
                    else
                    {
                        obj = (clsAddPackageServiceNewBizActionVO)objBaseDAL.AddPackageServicesNew(obj, objUserVO);
                    }

                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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

    internal class clsGetPackageServiceDetailsListNewBizAction : BizAction
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
        private clsGetPackageServiceDetailsListNewBizAction()
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
        private static clsGetPackageServiceDetailsListNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageServiceDetailsListNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPackageServiceDetailsListNewBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPackageServiceDetailsListNewBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsGetPackageServiceDetailsListNewBizActionVO)objBaseDAL.GetPackageServiceDetailListNew(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  
                //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

    }

    internal class clsGetPackageServicesAndRelationsNewBizAction : BizAction
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
        private clsGetPackageServicesAndRelationsNewBizAction()
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
        private static clsGetPackageServicesAndRelationsNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageServicesAndRelationsNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPackageServicesAndRelationsNewBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPackageServicesAndRelationsNewBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsGetPackageServicesAndRelationsNewBizActionVO)objBaseDAL.GetPackageServiceRelationsListNew(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  
                //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

    }

    internal class clsAddPackagePharmacyItemsNewBizAction : BizAction
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
        private clsAddPackagePharmacyItemsNewBizAction()
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
        private static clsAddPackagePharmacyItemsNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddPackagePharmacyItemsNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddPackagePharmacyItemsNewBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddPackagePharmacyItemsNewBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();

                    if (obj.ApplicableToAll == false)
                        obj = (clsAddPackagePharmacyItemsNewBizActionVO)objBaseDAL.AddPackagePharmacyItemsNew(obj, objUserVO);
                    else if (obj.ApplicableToAll == true)
                        obj = (clsAddPackagePharmacyItemsNewBizActionVO)objBaseDAL.UpdatePackageApplicableToAllPharmacyItemsNew(obj, objUserVO);

                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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

    internal class clsGetPackagePharmacyItemListNewBizAction : BizAction
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
        private clsGetPackagePharmacyItemListNewBizAction()
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
        private static clsGetPackagePharmacyItemListNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackagePharmacyItemListNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPackagePharmacyItemListNewBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPackagePharmacyItemListNewBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsGetPackagePharmacyItemListNewBizActionVO)objBaseDAL.GetPackagePharmacyItemListNew(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {

                //log error  
                //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

    }

    internal class clsDeletePackageServiceDetilsListNewBizAction : BizAction
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
        private clsDeletePackageServiceDetilsListNewBizAction()
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
        private static clsDeletePackageServiceDetilsListNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsDeletePackageServiceDetilsListNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsDeletePackageServiceDetilsListNewBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsDeletePackageServiceDetilsListNewBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();


                    obj = (clsDeletePackageServiceDetilsListNewBizActionVO)objBaseDAL.DeletePackageServicesDetilsNew(obj, objUserVO);

                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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

    internal class clsGetPackageConditionalServicesNewBizAction : BizAction
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
        private clsGetPackageConditionalServicesNewBizAction()
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
        private static clsGetPackageConditionalServicesNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageConditionalServicesNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPackageConditionalServicesNewBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPackageConditionalServicesNewBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsGetPackageConditionalServicesNewBizActionVO)objBaseDAL.GetPackageConditionalServiceListNew(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {

                //log error  
                //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

    }

    internal class clsGetPackageRelationListBizAction : BizAction
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
        private clsGetPackageRelationListBizAction()
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
        private static clsGetPackageRelationListBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageRelationListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPackageRelationListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPackageRelationListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();


                    obj = (clsGetPackageRelationListBizActionVO)objBaseDAL.GetPackageRelationsListForPackageOnly(obj, objUserVO);

                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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

    internal class clsDeletePackageItemDetilsListNewBizAction : BizAction
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
        private clsDeletePackageItemDetilsListNewBizAction()
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
        private static clsDeletePackageItemDetilsListNewBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsDeletePackageItemDetilsListNewBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsDeletePackageItemDetilsListNewBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsDeletePackageItemDetilsListNewBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();


                    obj = (clsDeletePackageItemDetilsListNewBizActionVO)objBaseDAL.DeletePackageItemsDetilsNew(obj, objUserVO);

                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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

    //added by neena
    internal class clsAddPackageConsentLinkBizAction : BizAction
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
        private clsAddPackageConsentLinkBizAction()
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
        private static clsAddPackageConsentLinkBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddPackageConsentLinkBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddPackageConsentLinkBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddPackageConsentLinkBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsAddPackageConsentLinkBizActionVO)objBaseDAL.AddPackageConsentLink(obj, objUserVO);



                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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

    internal class clsGetPackageConsentLinkBizAction : BizAction
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
        private clsGetPackageConsentLinkBizAction()
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
        private static clsGetPackageConsentLinkBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPackageConsentLinkBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPackageConsentLinkBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPackageConsentLinkBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePackageServiceNewDAL objBaseDAL = clsBasePackageServiceNewDAL.GetInstance();
                    obj = (clsGetPackageConsentLinkBizActionVO)objBaseDAL.GetPackageConsentLink(obj, objUserVO);



                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
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
    //

}
