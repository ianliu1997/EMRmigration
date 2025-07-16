using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Billing;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;

namespace PalashDynamics.BusinessLayer.Billing
{
    class clsPreviousBillListBizAction : BizAction
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
        private clsPreviousBillListBizAction()
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
        private static clsPreviousBillListBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsPreviousBillListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsPreviousPatientBillsBizActionVO obj = null;
            try
            {
                obj = (clsPreviousPatientBillsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseBillDAL objBaseDAL = clsBaseBillDAL.GetInstance();
                    obj = (clsPreviousPatientBillsBizActionVO)objBaseDAL.GetPharmacyBillSearchList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return obj;
        }

    }
}
