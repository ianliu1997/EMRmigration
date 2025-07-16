using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;


namespace PalashDynamics.BusinessLayer
{
    //class clsGetAddUpdateRadiologyNewBizAction
    //{
    //} 

    internal class clsUpdateRadOrderBookingDetailDeliveryStatusBizAction : BizAction
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
        private clsUpdateRadOrderBookingDetailDeliveryStatusBizAction()
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
        private static clsUpdateRadOrderBookingDetailDeliveryStatusBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateRadOrderBookingDetailDeliveryStatusBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsUpdateRadOrderBookingDetailDeliveryStatusBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateRadOrderBookingDetailDeliveryStatusBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                    obj = (clsUpdateRadOrderBookingDetailDeliveryStatusBizActionVO)objBaseDAL.UpdateEmailDelivredIntoList(obj, objUserVO);
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

    internal class clsGetRadiologistBySpecializationBizAction : BizAction
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
         private clsGetRadiologistBySpecializationBizAction()
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
         private static clsGetRadiologistBySpecializationBizAction _Instance = null;

         ///Method Input OPDPatient: none
         ///Name:GetInstance       
         ///Type:static
         ///Direction:None
         ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsGetRadiologistBySpecializationBizAction();

             return _Instance;
         }


         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             bool CurrentMethodExecutionStatus = true;
             clsGetRadiologistBySpecializationBizActionVO obj = null;
             int ResultStatus = 0;
             try
             {
                 obj = (clsGetRadiologistBySpecializationBizActionVO)valueObject;
                 //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                 //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                 //Typecast the "valueObject" to "clsInputOutputVO"
                 if (obj != null)
                 {
                     clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                     obj = (clsGetRadiologistBySpecializationBizActionVO)objBaseDAL.GetRadiologistbySpecia(obj, objUserVO);
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

      internal class clsGetRadiologistGenderByIDBizAction : BizAction
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
         private clsGetRadiologistGenderByIDBizAction()
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
         private static clsGetRadiologistGenderByIDBizAction _Instance = null;

         ///Method Input OPDPatient: none
         ///Name:GetInstance       
         ///Type:static
         ///Direction:None
         ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsGetRadiologistGenderByIDBizAction();

             return _Instance;
         }


         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             bool CurrentMethodExecutionStatus = true;
             clsGetRadiologistGenderByIDBizActionVO obj = null;
             int ResultStatus = 0;
             try
             {
                 obj = (clsGetRadiologistGenderByIDBizActionVO)valueObject;
                 //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                 //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                 //Typecast the "valueObject" to "clsInputOutputVO"
                 if (obj != null)
                 {
                     clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                     obj = (clsGetRadiologistGenderByIDBizActionVO)objBaseDAL.GetRadiologistGenderByitsID(obj, objUserVO);
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

      internal class clsRadPathPatientReportDetailsForEmailSendingBizAction : BizAction
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
         private clsRadPathPatientReportDetailsForEmailSendingBizAction()
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
         private static clsRadPathPatientReportDetailsForEmailSendingBizAction _Instance = null;

         ///Method Input OPDPatient: none
         ///Name:GetInstance       
         ///Type:static
         ///Direction:None
         ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsRadPathPatientReportDetailsForEmailSendingBizAction();

             return _Instance;
         }


         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             bool CurrentMethodExecutionStatus = true;
             clsRadPathPatientReportDetailsForEmailSendingBizActionVO obj = null;
             int ResultStatus = 0;
             try
             {
                 obj = (clsRadPathPatientReportDetailsForEmailSendingBizActionVO)valueObject;
                 //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                 //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                 //Typecast the "valueObject" to "clsInputOutputVO"
                 if (obj != null)
                 {
                     clsBaseRadiologyDAL objBaseDAL = clsBaseRadiologyDAL.GetInstance();
                     obj = (clsRadPathPatientReportDetailsForEmailSendingBizActionVO)objBaseDAL.AddRadDetilsForEmail(obj, objUserVO);
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


      internal class clsGetRadiologistResultEntryBizAction : BizAction
      {

          private static clsGetRadiologistResultEntryBizAction _Instance = null;
          /// <summary>
          /// To create singleton instance of the class and  This will Give Unique Instance
          /// </summary>
          /// <returns></returns>
          public static BizAction GetInstance()
          {
              if (_Instance == null)
                  _Instance = new clsGetRadiologistResultEntryBizAction();

              return _Instance;
          }


          protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
          {
              bool CurrentMethodExecutionStatus = true;
              clsGetRadiologistResultEntryBizActionVO obj = null;
              int ResultStatus = 0;

              try
              {
                  obj = (clsGetRadiologistResultEntryBizActionVO)valueObject;
                  if (obj != null)
                  {
                      //Create the Instance of clsBasePatientDAL
                      clsBaseRadiologyDAL objBaseMasterDAL = clsBaseRadiologyDAL.GetInstance();
                      //Now Call the Insert method of the Patient DAO
                      obj = (clsGetRadiologistResultEntryBizActionVO)objBaseMasterDAL.GetRadiologistResultEntryDefined(obj, objUserVO);
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

   

    



    
    

}
