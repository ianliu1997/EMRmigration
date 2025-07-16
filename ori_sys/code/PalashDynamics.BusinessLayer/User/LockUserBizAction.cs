using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.User
{
   public class LockUserBizAction
    {
        //The Private declaration
       private static LockUserBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>
       public static LockUserBizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new LockUserBizAction();

           return _Instance;
       }

        /// <summary>
        /// This method is override from BizAction Class. 
        /// It takes string LoginName, string Password as input Parameter and return clsUserVO.This method creates instance of UMDAL class and call the AuthenticateUser() method of UMDAL Class which execute and return the resultset.
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>

       public clsUserVO LockUser(long UserID)
       {
           clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
           return (clsUserVO)objBaseDAL.LockUser(UserID);
       }

       public clsUserVO AutoUnLockUser(long UserID)
       {
           clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
           return (clsUserVO)objBaseDAL.AutoUnLockUser(UserID);
       }

    }
}
