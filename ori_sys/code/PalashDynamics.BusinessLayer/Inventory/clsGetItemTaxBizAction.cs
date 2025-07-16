using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;

using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
namespace PalashDynamics.BusinessLayer.Inventory
{
   internal class clsGetItemTaxBizAction:BizAction   
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetItemTaxBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetItemTaxBizActionVO)valueObject;
              
                if (obj != null)
                {
                    clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                    obj = (clsGetItemTaxBizActionVO)objBaseDAL.GetItemTaxList(obj, objUserVO);
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
        private clsGetItemTaxBizAction()
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
        private static clsGetItemTaxBizAction _Instance = null;


        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetItemTaxBizAction();

            return _Instance;
        } 
    }

   internal class clsGetAllItemTaxDetailBizAction : BizAction
   {

       #region Variables Declaration

       LogManager logManager = null;

       long lngUserId = 0;
       #endregion

       //constructor For Log Error Info
       private clsGetAllItemTaxDetailBizAction()
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
       private static clsGetAllItemTaxDetailBizAction _Instance = null;


       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetAllItemTaxDetailBizAction();

           return _Instance;
       }

       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {

           clsGetAllItemTaxDetailBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetAllItemTaxDetailBizActionVO)valueObject;

               if (obj != null)
               {
                   clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                   obj = (clsGetAllItemTaxDetailBizActionVO)objBaseDAL.GetAllItemTaxDetail(obj, objUserVO);
               }
           }
           catch (HmsApplicationException HEx)
           {

           }
           catch (Exception ex)
           {

           }
           finally
           {

           }
           return obj;
       }



   }

   internal class clsGetItemClinicDetailBizAction : BizAction
   {

       #region Variables Declaration

       LogManager logManager = null;

       long lngUserId = 0;
       #endregion
       private clsGetItemClinicDetailBizAction()
       {
           if (logManager == null)
           {
               logManager = LogManager.GetInstance();
           }
       }
       private static clsGetItemClinicDetailBizAction _Instance = null;
       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetItemClinicDetailBizAction();

           return _Instance;
       }
       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {

           clsGetItemClinicDetailBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetItemClinicDetailBizActionVO)valueObject;

               if (obj != null)
               {
                   clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                   obj = (clsGetItemClinicDetailBizActionVO)objBaseDAL.GetItemClinicDetailList(obj, objUserVO);
               }
           }
           catch (HmsApplicationException HEx)
           {

           }
           catch (Exception ex)
           {

           }
           finally
           {

           }
           return obj;
       }
   }
}
