using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.BusinessLayer.Inventory
{
    class clsUpdateCentralStoreDetailsBizAction:BizAction 
    {
        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;

        #region Constructor
        public clsUpdateCentralStoreDetailsBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }
        #endregion

        private static clsUpdateCentralStoreDetailsBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsUpdateCentralStoreDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsUpdateCentralStoreDetailsBizActionVO obj = null;
            try
            {
                obj = (clsUpdateCentralStoreDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseItemMasterDAL objBaseItemDal = clsBaseItemMasterDAL.GetInstance();
                    obj = (clsUpdateCentralStoreDetailsBizActionVO)objBaseItemDal.clsUpdateCentralStoreDetails(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //log error  
                //lgmanager.LogError(new Guid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return obj;
        }
    }
}
