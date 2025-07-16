using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Inventory;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.Inventory
{


    internal class clsFillStoreMasterListBizAction : BizAction
    {

        LogManager lgmanager = null;
        long LogUserID = 0;
        bool CurrentMethodExecutionStatus = true;
        private clsFillStoreMasterListBizAction()
        {
            if (lgmanager == null)
            {
                lgmanager = LogManager.GetInstance();
            }
        }


        private static clsFillStoreMasterListBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)

                _Instance = new clsFillStoreMasterListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            clsFillStoreMasterListBizActionVO obj = null;
            try
            {
                obj = (clsFillStoreMasterListBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBaseMasterDAL objBaseItemDal = clsBaseMasterDAL.GetInstance();
                    obj = (clsFillStoreMasterListBizActionVO)objBaseItemDal.GetStoreForComboBox(obj, objUserVO);
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
