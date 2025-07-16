using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.RateContract;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.RateContract;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.BusinessLayer.RateContract
{
   public class clsGetRateContractAgainstSupplierAndItemBizAction :BizAction
    {
          LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsGetRateContractAgainstSupplierAndItemBizAction()
     
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }

        }

        private static clsGetRateContractAgainstSupplierAndItemBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsGetRateContractAgainstSupplierAndItemBizAction();
           
            return _Instance;
        }
       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsGetRateContractAgainstSupplierAndItemBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetRateContractAgainstSupplierAndItemBizActionVO)valueObject;

               if (obj != null)
               {

                   clsBaseRateContractDAL objBaseDAL = clsBaseRateContractDAL.GetInstance();
                   obj = (clsGetRateContractAgainstSupplierAndItemBizActionVO)objBaseDAL.GetRateContractAgainstSupplierAndItem(valueObject, objUserVO);
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
           finally
           {
           }
           return obj;
       }
    }
 
}
