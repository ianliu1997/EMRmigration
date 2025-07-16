using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects;


namespace PalashDynamics.BusinessLayer.Administration
{
    class clsGetPassConfigBizAction: BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetPassConfigBizAction()
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
        private static clsGetPassConfigBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        { 
            if (_Instance == null)
            {
                _Instance = new clsGetPassConfigBizAction();
            }
                return _Instance;

        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
    
            clsGetPassConfigBizActionVO obj = null;
         
            try
            {
                obj = (clsGetPassConfigBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBasePassConfigDAL objBaseDAL = clsBasePassConfigDAL.GetInstance();
                    obj = (clsGetPassConfigBizActionVO)objBaseDAL.GetPassConfig(obj, objUserVO);

                }
            }
            catch(HmsApplicationException ex)
            {
            
                //log error  
                //logManager.LogError(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
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
