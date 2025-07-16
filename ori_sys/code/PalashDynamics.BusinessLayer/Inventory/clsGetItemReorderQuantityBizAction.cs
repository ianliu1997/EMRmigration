using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.Inventory
{
   internal class clsGetItemReorderQuantityBizAction : BizAction
    {
       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsGetItemReorderQuantityBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetItemReorderQuantityBizActionVO)valueObject;

               if (obj != null)
               {

                   clsBaseItemReorderQuantityDAL objBaseDAL = clsBaseItemReorderQuantityDAL.GetInstance();
                   obj = (clsGetItemReorderQuantityBizActionVO)objBaseDAL.GetItemReorderQuantity(obj, objUserVO);
                   
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
       private static clsGetItemReorderQuantityBizAction _Instance = null;
       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetItemReorderQuantityBizAction();

           return _Instance;
       } 
    }
}
