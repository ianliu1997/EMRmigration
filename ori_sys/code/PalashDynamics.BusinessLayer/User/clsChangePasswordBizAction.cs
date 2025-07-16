using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.BusinessLayer;
using PalashDynamics.ValueObjects.Patient;
//using PalashDynamics.CustomExceptions;
//using PalashDynamics.ValueObjects.User;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;


namespace PalashDynamics.BusinessLayer
{
    internal class clsChangePasswordBizAction : BizAction
    {
        //The Private declaration
        private static clsChangePasswordBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsChangePasswordBizAction();

            return _Instance;
        }
        /// <summary>
        /// This method is override from BizAction Class. It takes IValueObject, clsUserVO as input Parameter and return IValueObject.
        /// This method creates instance of UserManagementDAL class and call the ChangePassword() method of UserManagementDAL Class which execute and return the resultset.
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="objUserVO"></param>
        /// <returns></returns>
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            //The Private declaration
            clsChangePasswordBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                //Typecast the "valueObject" to "clsChangePasswordBizActionVO"
                obj = (clsChangePasswordBizActionVO)valueObject;
       
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseUserManagementDAL objBaseUserManagementDAL = clsBaseUserManagementDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsChangePasswordBizActionVO)objBaseUserManagementDAL.ChangePassword(obj, objUserVO);
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

            }
            return obj;
        }



   
   
   
   }
}
