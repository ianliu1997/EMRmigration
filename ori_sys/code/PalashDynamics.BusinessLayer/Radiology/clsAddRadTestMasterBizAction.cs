using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.BusinessLayer
{
   internal class clsAddRadTestMasterBizAction:BizAction
    {   //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsAddRadTestMasterBizAction()
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
        private static clsAddRadTestMasterBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddRadTestMasterBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddRadTestMasterBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddRadTestMasterBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                    obj = (clsAddRadTestMasterBizActionVO)objBaseDAL.AddTestMaster(obj, objUserVO);
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
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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


   internal class clsAddRadiologistToTempbizAction : BizAction
   {   //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseOPDPatientMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
       #endregion

       //constructor For Log Error Info
       private clsAddRadiologistToTempbizAction()
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
       private static clsAddRadiologistToTempbizAction _Instance = null;

       ///Method Input OPDPatient: none
       ///Name:GetInstance       
       ///Type:static
       ///Direction:None
       ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsAddRadiologistToTempbizAction();

           return _Instance;
       }


       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsAddRadiologistToTempbizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsAddRadiologistToTempbizActionVO)valueObject;
               //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
               //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                   obj = (clsAddRadiologistToTempbizActionVO)objBaseDAL.AddRadiologistToTempList(obj, objUserVO);
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
               logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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

   internal class clsGetRadioTemplateGenderBizAction : BizAction
   {   //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseOPDPatientMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
       #endregion

       //constructor For Log Error Info
       private clsGetRadioTemplateGenderBizAction()
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
       private static clsGetRadioTemplateGenderBizAction _Instance = null;

       ///Method Input OPDPatient: none
       ///Name:GetInstance       
       ///Type:static
       ///Direction:None
       ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetRadioTemplateGenderBizAction();

           return _Instance;
       }


       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsGetRadioTemplateGenderBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetRadioTemplateGenderBizActionVO)valueObject;
               //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
               //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {

                   clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                   obj = (clsGetRadioTemplateGenderBizActionVO)objBaseDAL.GetRadioGender(obj, objUserVO);
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
               logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
               throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
           }
           finally
           {
           }
           return obj;
       }
   }

      internal class clsGetRadiologistToTempBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRadiologistToTempBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetRadiologistToTempBizActionVO)valueObject;

                if (obj != null)
                {
                    //if (obj.CheckForTaxExistatnce == true)
                    //{
                    //    clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                    //    obj = (clsGetItemSupplierBizActionVO)objBaseDAL.CheckForSupplierExistance(obj, objUserVO);
                    //}
                    //else if (obj.CheckForTaxExistatnce == false)
                    //{
                    
                    clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                    obj = (clsGetRadiologistToTempBizActionVO)objBaseDAL.GetRadiologistToTempList(obj, objUserVO);
                    //}
                }

                //if(obj!=null && )
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
                //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // Guid a = new Guid();
                //logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsGetRadiologistToTempBizAction()
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
        private static clsGetRadiologistToTempBizAction _Instance = null;


        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRadiologistToTempBizAction();

            return _Instance;
        }
    }
   

   //internal class clsAddRadioTemplateMasterBizAction : BizAction
   //{   //This Region Contains Variables Which are Used At Form Level
   //    #region Variables Declaration
   //    //Declare the LogManager object
   //    LogManager logManager = null;
   //    //Declare the BaseOPDPatientMasterDAL object
   //    //Declare the Variable of UserId
   //    long lngUserId = 0;
   //    #endregion

   //    //constructor For Log Error Info
   //    private clsAddRadioTemplateMasterBizAction()
   //    {
   //        //Create Instance of the LogManager object 
   //        #region Logging Code
   //        if (logManager == null)
   //        {
   //            logManager = LogManager.GetInstance();
   //        }
   //        #endregion
   //    }

   //    //The Private declaration
   //    private static clsAddPathoTemplateMasterBizAction _Instance = null;

   //    ///Method Input OPDPatient: none
   //    ///Name:GetInstance       
   //    ///Type:static
   //    ///Direction:None
   //    ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
   //    public static BizAction GetInstance()
   //    {
   //        if (_Instance == null)
   //            _Instance = new clsAddRadioTemplateMasterBizAction();

   //        return _Instance;
   //    }


   //    protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
   //    {
   //        bool CurrentMethodExecutionStatus = true;
   //        clsAddRadioTemplateMasterBizAction obj = null;
   //        int ResultStatus = 0;
   //        try
   //        {
   //            obj = (clsAddRadioTemplateMasterBizAction)valueObject;
   //            //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
   //            //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
   //            //Typecast the "valueObject" to "clsInputOutputVO"
   //            if (obj != null)
   //            {
   //                clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
   //                if (obj.IsModifyStatus == true)
   //                    obj = (clsAddRadioTemplateMasterBizAction)objBaseDAL.ChangePathoTemplateStatus(obj, objUserVO);
   //                else
   //                    obj = (clsAddRadioTemplateMasterBizAction)objBaseDAL.AddPathoTemplate(obj, objUserVO);
   //            }
   //        }
   //        catch (HmsApplicationException HEx)
   //        {
   //            CurrentMethodExecutionStatus = false;
   //            throw;
   //        }
   //        catch (Exception ex)
   //        {
   //            CurrentMethodExecutionStatus = false;
   //            //log error  
   //            logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
   //            // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
   //            throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
   //        }
   //        finally
   //        {
   //            //log error  
   //            // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
   //        }
   //        return obj;
   //    }
   //}
}
