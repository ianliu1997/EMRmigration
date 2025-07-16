using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.OpeartionTheatre;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer
{
   
    public class clsGetPatientUnitIDForOtDetailsBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetPatientUnitIDForOtDetailsBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetPatientUnitIDForOtDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetPatientUnitIDForOtDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetPatientUnitIDForOtDetailsBizActionVO obj = null;
            try
            {
                obj = (clsGetPatientUnitIDForOtDetailsBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetPatientUnitIDForOtDetailsBizActionVO)objBaseOTDetailsDAL.GetPatientUnitIDForOtDetails(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    public class clsGetProceduresForServiceIdBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetProceduresForServiceIdBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetProceduresForServiceIdBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetProceduresForServiceIdBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetProceduresForServiceIdBizActionVO obj = null;
            try
            {
                obj = (clsGetProceduresForServiceIdBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetProceduresForServiceIdBizActionVO)objBaseOTDetailsDAL.GetProceduresForServiceID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsGetServicesForProcedureIDBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetServicesForProcedureIDBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetServicesForProcedureIDBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetServicesForProcedureIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetServicesForProcedureIDBizActionVO obj = null;
            try
            {
                obj = (clsGetServicesForProcedureIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetServicesForProcedureIDBizActionVO)objBaseOTDetailsDAL.GetServicesForProcedureID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    public class clsGetDoctorForOTDetailsBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetDoctorForOTDetailsBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetDoctorForOTDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetDoctorForOTDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetDoctorForOTDetailsBizActionVO obj = null;
            try
            {
                obj = (clsGetDoctorForOTDetailsBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetDoctorForOTDetailsBizActionVO)objBaseOTDetailsDAL.GetDoctorForOTDetails(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsAddupdatOtDetailsBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsAddupdatOtDetailsBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsAddupdatOtDetailsBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsAddupdatOtDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsAddupdatOtDetailsBizActionVO obj = null;
            try
            {
                obj = (clsAddupdatOtDetailsBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsAddupdatOtDetailsBizActionVO)objBaseOTDetailsDAL.AddupdatOtDetails(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsAddUpdatOtSurgeryDetailsBizAction : BizAction
     {
         LogManager lgmanager = null;
         long LogUserID = 0;
         bool CurrentMethodExecutionStatus = true;
         private clsAddUpdatOtSurgeryDetailsBizAction()
         {
             if (lgmanager == null)
             {
                 lgmanager = LogManager.GetInstance();
             }
         }


         private static clsAddUpdatOtSurgeryDetailsBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)

                 _Instance = new clsAddUpdatOtSurgeryDetailsBizAction();

             return _Instance;
         }

         protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
         {

             clsAddUpdatOtSurgeryDetailsBizActionVO obj = null;
             try
             {
                 obj = (clsAddUpdatOtSurgeryDetailsBizActionVO)valueObject;

                 if (obj != null)
                 {
                     clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                     obj = (clsAddUpdatOtSurgeryDetailsBizActionVO)objBaseOTDetailsDAL.AddUpdateOtSurgeryDetails(obj, objUserVO);
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
                 //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                 // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsAddUpdatOtItemDetailsBizAction : BizAction
     {
         LogManager lgmanager = null;
         long LogUserID = 0;
         bool CurrentMethodExecutionStatus = true;
         private clsAddUpdatOtItemDetailsBizAction()
         {
             if (lgmanager == null)
             {
                 lgmanager = LogManager.GetInstance();
             }
         }


         private static clsAddUpdatOtItemDetailsBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)

                 _Instance = new clsAddUpdatOtItemDetailsBizAction();

             return _Instance;
         }

         protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
         {

             clsAddUpdateOtItemDetailsBizActionVO obj = null;
             try
             {
                 obj = (clsAddUpdateOtItemDetailsBizActionVO)valueObject;

                 if (obj != null)
                 {
                     clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                     obj = (clsAddUpdateOtItemDetailsBizActionVO)objBaseOTDetailsDAL.AddUpdateOtItemDetails(obj, objUserVO);
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
                 //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                 // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsAddUpdatOtDocEmpDetailsBizAction : BizAction
     {
         LogManager lgmanager = null;
         long LogUserID = 0;
         bool CurrentMethodExecutionStatus = true;
         private clsAddUpdatOtDocEmpDetailsBizAction()
         {
             if (lgmanager == null)
             {
                 lgmanager = LogManager.GetInstance();
             }
         }


         private static clsAddUpdatOtDocEmpDetailsBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)

                 _Instance = new clsAddUpdatOtDocEmpDetailsBizAction();

             return _Instance;
         }

         protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
         {

             clsAddUpdatOtDocEmpDetailsBizActionVO obj = null;
             try
             {
                 obj = (clsAddUpdatOtDocEmpDetailsBizActionVO)valueObject;

                 if (obj != null)
                 {
                     clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                     obj = (clsAddUpdatOtDocEmpDetailsBizActionVO)objBaseOTDetailsDAL.AddUpdateOtDocEmpDetails(obj, objUserVO);
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
                 //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                 // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsAddUpdatOtServicesDetailsBizAction : BizAction
     {
         LogManager lgmanager = null;
         long LogUserID = 0;
         bool CurrentMethodExecutionStatus = true;
         private clsAddUpdatOtServicesDetailsBizAction()
         {
             if (lgmanager == null)
             {
                 lgmanager = LogManager.GetInstance();
             }
         }


         private static clsAddUpdatOtServicesDetailsBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)

                 _Instance = new clsAddUpdatOtServicesDetailsBizAction();

             return _Instance;
         }

         protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
         {

             clsAddUpdatOtServicesDetailsBizActionVO obj = null;
             try
             {
                 obj = (clsAddUpdatOtServicesDetailsBizActionVO)valueObject;

                 if (obj != null)
                 {
                     clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                     obj = (clsAddUpdatOtServicesDetailsBizActionVO)objBaseOTDetailsDAL.AddUpdateOtServicesDetails(obj, objUserVO);
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
                 //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                 // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsAddUpdatOtNotesDetailsBizAction : BizAction
     {
         LogManager lgmanager = null;
         long LogUserID = 0;
         bool CurrentMethodExecutionStatus = true;
         private clsAddUpdatOtNotesDetailsBizAction()
         {
             if (lgmanager == null)
             {
                 lgmanager = LogManager.GetInstance();
             }
         }


         private static clsAddUpdatOtNotesDetailsBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)

                 _Instance = new clsAddUpdatOtNotesDetailsBizAction();

             return _Instance;
         }

         protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
         {

             clsAddUpdatOtNotesDetailsBizActionVO obj = null;
             try
             {
                 obj = (clsAddUpdatOtNotesDetailsBizActionVO)valueObject;

                 if (obj != null)
                 {
                     clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                     obj = (clsAddUpdatOtNotesDetailsBizActionVO)objBaseOTDetailsDAL.AddUpdateOtNotesDetails(obj, objUserVO);
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
                 //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                 // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsAddUpdatOtPostInstructionDetailsBizAction : BizAction
     {
         LogManager lgmanager = null;
         long LogUserID = 0;
         bool CurrentMethodExecutionStatus = true;
         private clsAddUpdatOtPostInstructionDetailsBizAction()
         {
             if (lgmanager == null)
             {
                 lgmanager = LogManager.GetInstance();
             }
         }


         private static clsAddUpdatOtPostInstructionDetailsBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)

                 _Instance = new clsAddUpdatOtPostInstructionDetailsBizAction();

             return _Instance;
         }

         protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
         {

             clsAddUpdatOtPostInstructionDetailsBizActionVO obj = null;
             try
             {
                 obj = (clsAddUpdatOtPostInstructionDetailsBizActionVO)valueObject;

                 if (obj != null)
                 {
                     clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                     obj = (clsAddUpdatOtPostInstructionDetailsBizActionVO)objBaseOTDetailsDAL.AddUpdatePostInstructionDetails(obj, objUserVO);
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
                 //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                 // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsAddUpdateOTDoctorNotesDetailsBizAction : BizAction
     {
         LogManager lgmanager = null;
         long LogUserID = 0;
         bool CurrentMethodExecutionStatus = true;
         private clsAddUpdateOTDoctorNotesDetailsBizAction()
         {
             if (lgmanager == null)
             {
                 lgmanager = LogManager.GetInstance();
             }
         }


         private static clsAddUpdateOTDoctorNotesDetailsBizAction _Instance = null;

         public static BizAction GetInstance()
         {
             if (_Instance == null)

                 _Instance = new clsAddUpdateOTDoctorNotesDetailsBizAction();

             return _Instance;
         }

         protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
         {
             clsAddUpdateOTDoctorNotesDetailsBizActionVO obj = null;
             try
             {
                 obj = (clsAddUpdateOTDoctorNotesDetailsBizActionVO)valueObject;

                 if (obj != null)
                 {
                     clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                     obj = (clsAddUpdateOTDoctorNotesDetailsBizActionVO)objBaseOTDetailsDAL.AddUpdateDoctorNotesDetails(obj, objUserVO);
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
                 //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                 // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsGetOTDetailsListizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetOTDetailsListizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetOTDetailsListizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetOTDetailsListizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetOTDetailsListizActionVO obj = null;
            try
            {
                obj = (clsGetOTDetailsListizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetOTDetailsListizActionVO)objBaseOTDetailsDAL.GetOTDetailsList(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
    
    public class clsGetDetailTablesOfOTDetailsByOTDetailIDBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetDetailTablesOfOTDetailsByOTDetailIDBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetDetailTablesOfOTDetailsByOTDetailIDBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetDetailTablesOfOTDetailsByOTDetailIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO obj = null;
            try
            {
                obj = (clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO)objBaseOTDetailsDAL.GetDetailTablesOfOTDetailsByOTDetailID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    public class clsGetSurgeryDetailsByOTDetailsIDBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;

        private clsGetSurgeryDetailsByOTDetailsIDBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }

        private static clsGetSurgeryDetailsByOTDetailsIDBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetSurgeryDetailsByOTDetailsIDBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetSurgeryDetailsByOTDetailsIDBizActionVO obj = null;
            try
            {
                obj = (clsGetSurgeryDetailsByOTDetailsIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetSurgeryDetailsByOTDetailsIDBizActionVO)objBaseOTDetailsDAL.GetSurgeryDetailsByOTDetailsID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    public class clsGetItemDetailsByOTDetailsIDBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetItemDetailsByOTDetailsIDBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetItemDetailsByOTDetailsIDBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetItemDetailsByOTDetailsIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetItemDetailsByOTDetailsIDBizActionVO obj = null;
            try
            {
                obj = (clsGetItemDetailsByOTDetailsIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetItemDetailsByOTDetailsIDBizActionVO)objBaseOTDetailsDAL.GetItemDetailsByOTDetailsID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    public class clsGetDocEmpDetailsByOTDetailsIDBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetDocEmpDetailsByOTDetailsIDBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetDocEmpDetailsByOTDetailsIDBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetDocEmpDetailsByOTDetailsIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetDocEmpDetailsByOTDetailsIDBizActionVO obj = null;
            try
            {
                obj = (clsGetDocEmpDetailsByOTDetailsIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetDocEmpDetailsByOTDetailsIDBizActionVO)objBaseOTDetailsDAL.GetDocEmpDetailsByOTDetailsID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    public class clsGetServicesByOTDetailsIDBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetServicesByOTDetailsIDBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetServicesByOTDetailsIDBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetServicesByOTDetailsIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetServicesByOTDetailsIDBizActionVO obj = null;
            try
            {
                obj = (clsGetServicesByOTDetailsIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetServicesByOTDetailsIDBizActionVO)objBaseOTDetailsDAL.GetServicesByOTDetailsID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    public class clsGetDoctorNotesByOTDetailsIDBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetDoctorNotesByOTDetailsIDBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetDoctorNotesByOTDetailsIDBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetDoctorNotesByOTDetailsIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetDoctorNotesByOTDetailsIDBizActionVO obj = null;
            try
            {
                obj = (clsGetDoctorNotesByOTDetailsIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetDoctorNotesByOTDetailsIDBizActionVO)objBaseOTDetailsDAL.GetDoctorNotesByOTDetailsID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    public class clsGetOTNotesByOTDetailsIDBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetOTNotesByOTDetailsIDBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }
        private static clsGetOTNotesByOTDetailsIDBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetOTNotesByOTDetailsIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetOTNotesByOTDetailsIDBizActionVO obj = null;
            try
            {
                obj = (clsGetOTNotesByOTDetailsIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetOTNotesByOTDetailsIDBizActionVO)objBaseOTDetailsDAL.GetOTNotesByOTDetailsID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

     public class clsGetConsetDetailsForConsentIDBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetConsetDetailsForConsentIDBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsGetConsetDetailsForConsentIDBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetConsetDetailsForConsentIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsGetConsetDetailsForConsentIDBizActionVO obj = null;
            try
            {
                obj = (clsGetConsetDetailsForConsentIDBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsGetConsetDetailsForConsentIDBizActionVO)objBaseOTDetailsDAL.GetConsetDetailsForConsentID(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    public class clsAddPatientWiseConsentPrintingBizAction : BizAction
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsAddPatientWiseConsentPrintingBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsAddPatientWiseConsentPrintingBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsAddPatientWiseConsentPrintingBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsAddPatientWiseConsentPrintingBizActionVO obj = null;
            try
            {
                obj = (clsAddPatientWiseConsentPrintingBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseOTDetailsDAL objBaseOTDetailsDAL = clsBaseOTDetailsDAL.GetInstance();
                    obj = (clsAddPatientWiseConsentPrintingBizActionVO)objBaseOTDetailsDAL.AddPatientWiseConsentPrinting(obj, objUserVO);
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
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

    
    
    

    
    
    
    
}
