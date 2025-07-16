using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.CRM;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;

namespace PalashDynamics.BusinessLayer.CRM
{
  internal  class clsGetCampMasterListBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsGetCampMasterListBizAction()
        {
            //Create Instance of the LogManager object 

            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion


        }

        private static clsGetCampMasterListBizAction _Instance = null;


        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCampMasterListBizAction();
            return _Instance;
        }
       

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetCampMasterListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetCampMasterListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseCampMasterDAL objBaseCampMasterDAL = clsBaseCampMasterDAL.GetInstance();
                    obj = (clsGetCampMasterListBizActionVO)objBaseCampMasterDAL.GetCampMasterList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {

                throw;
            }

            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }

            return obj;

        }
    }
}
