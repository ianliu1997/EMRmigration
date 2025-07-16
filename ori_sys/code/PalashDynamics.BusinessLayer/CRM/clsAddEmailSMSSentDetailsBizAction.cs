using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.CRM
{
   internal class clsAddEmailSMSSentDetailsBizAction :BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsAddEmailSMSSentDetailsBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAddEmailSMSSentDetailsBizAction _Instance = null;


        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddEmailSMSSentDetailsBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddEmailSMSSentListVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddEmailSMSSentListVO)valueObject;
                if (obj != null)
                {
                    clsBaseCampMasterDAL objBaseCampMasterDAL = clsBaseCampMasterDAL.GetInstance();
                    obj = (clsAddEmailSMSSentListVO)objBaseCampMasterDAL.AddEmailSMSSentDetails(obj, objUserVO);
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
