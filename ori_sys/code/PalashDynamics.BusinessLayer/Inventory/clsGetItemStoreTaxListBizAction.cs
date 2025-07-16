using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;

namespace PalashDynamics.BusinessLayer.Inventory
{
    internal class clsGetItemStoreTaxListBizAction : BizAction
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
        private clsGetItemStoreTaxListBizAction()
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
        private static clsGetItemStoreTaxListBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetItemStoreTaxListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetItemStoreTaxListBizActionVO obj = null;
            try
            {
                obj = (clsGetItemStoreTaxListBizActionVO)valueObject;
                if (obj != null && obj.ISGetAllStoreTax == false)
                {
                    clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                    obj = (clsGetItemStoreTaxListBizActionVO)objBaseDAL.GetStoreItemTaxList(obj, objUserVO);
                }

                else //***//19
                {
                    clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();
                    obj = (clsGetItemStoreTaxListBizActionVO)objBaseDAL.GetMultipleStoreItemTaxList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }
            catch (Exception ex)
            {
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                
            }
            return obj;
        }
    }
}
