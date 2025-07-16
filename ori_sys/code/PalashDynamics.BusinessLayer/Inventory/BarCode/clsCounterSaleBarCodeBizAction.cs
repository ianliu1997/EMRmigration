using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using PalashDynamics.ValueObjects.Inventory.BarCode;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;

namespace PalashDynamics.BusinessLayer.Inventory.BarCode
{
    class clsCounterSaleBarCodeBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsCounterSaleBarCodeBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsCounterSaleBarCodeBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                    obj = (clsCounterSaleBarCodeBizActionVO)objBaseDAL.GetBarCodeCounterSale(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw HEx;
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

        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsCounterSaleBarCodeBizAction()
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
        private static clsCounterSaleBarCodeBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsCounterSaleBarCodeBizAction();

            return _Instance;
        }
    }
}
