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
   internal class clsUpdateLoyaltyProgramBizAction:BizAction
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
        private clsUpdateLoyaltyProgramBizAction()
        {
            //Create Instance of the LogManager object 

            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsUpdateLoyaltyProgramBizAction _Instance = null;


        ///Method Input Appointments: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance



        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateLoyaltyProgramBizAction();
            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateLoyaltyProgramBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateLoyaltyProgramBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseLoyaltyProgramDAL objBaseLoyaltyProgramDAL = clsBaseLoyaltyProgramDAL.GetInstance();
                    obj = (clsUpdateLoyaltyProgramBizActionVO)objBaseLoyaltyProgramDAL.UpdateLoyaltyProgram(obj, objUserVO);
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
