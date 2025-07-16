using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.User
{
   internal class clsChangeFirstPasswordBizAction:BizAction
    {
       private static clsChangeFirstPasswordBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>
       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsChangeFirstPasswordBizAction();

           return _Instance;
       }
       /// <summary>
       /// This method is override from BizAction Class. It takes IValueObject, clsUserVO as input Parameter and return IValueObject.
       /// This method creates instance of UserManagementDAL class and call the ChangePassword() method of UserManagementDAL Class which execute and return the resultset.
       /// </summary>
       /// <param name="valueObject"></param>
       /// <param name="objUserVO"></param>
       /// <returns></returns>
        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            //The Private declaration
            clsChangePasswordFirstTimeBizActionVO obj = null;
            int ResultSet = 0;

            try
            {
                //Typecast the "valueObject" to "clsChangePasswordFirstBizActionVO"
                obj = (clsChangePasswordFirstTimeBizActionVO)valueObject;

                if (obj != null)
                { 
                    //Create the instance of clsBaseUserManagmentDal
                    clsBaseUserManagementDAL objUser = clsBaseUserManagementDAL.GetInstance();

                    obj = (clsChangePasswordFirstTimeBizActionVO)objUser.ChangeFirstPassword(obj, objUserVO);

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
