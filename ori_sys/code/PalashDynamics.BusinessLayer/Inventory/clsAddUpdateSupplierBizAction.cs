using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
namespace PalashDynamics.BusinessLayer.Inventory
{
    class clsAddUpdateSupplierBizAction:BizAction
    {
         LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsAddUpdateSupplierBizAction()
     
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }

        }


        private static clsAddUpdateSupplierBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsAddUpdateSupplierBizAction();
           
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsAddUpdateSupplierBizActionVO obj = null;
            try
            {
                obj = (clsAddUpdateSupplierBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseItemMasterDAL objBaseItemDal = clsBaseItemMasterDAL.GetInstance();
                    obj = (clsAddUpdateSupplierBizActionVO)objBaseItemDal.AddUpdateSupplier(obj, objUserVO);
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
