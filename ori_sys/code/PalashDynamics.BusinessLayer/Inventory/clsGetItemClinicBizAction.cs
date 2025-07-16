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
 internal   class clsGetItemClinicBizAction:BizAction
    {
          protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetItemClinicBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetItemClinicBizActionVO)valueObject;

                if (obj != null)
                {
                    //if (obj.CheckForTaxExistatnce == true)
                    //{
                    //    clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                    //    obj = (clsGetItemSupplierBizActionVO)objBaseDAL.CheckForSupplierExistance(obj, objUserVO);
                    //}
                    //else if (obj.CheckForTaxExistatnce == false)
                    //{
                        //clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                        //obj = (clsGetItemClinicBizActionVO)objBaseDAL.GetItemClinicList(obj, objUserVO);
                    //}
                    if(obj.ISGteMultipleStoreList == true) //***//19
                    {
                         clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                         obj = (clsGetItemClinicBizActionVO)objBaseDAL.GetItemMapStoreList(obj, objUserVO);
                    }
                    else
                    {
                         clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                         obj = (clsGetItemClinicBizActionVO)objBaseDAL.GetItemClinicList(obj, objUserVO);
                    }
                    
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
        private clsGetItemClinicBizAction()
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
        private static clsGetItemClinicBizAction _Instance = null;

       
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetItemClinicBizAction();

            return _Instance;
        } 
    }
}
