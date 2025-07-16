using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.User
{
   public class ForgotPasswordBizAction
    {
     
            //The Private declaration
       private static ForgotPasswordBizAction _Instance = null;
            /// <summary>
            /// To create singleton instance of the class and  This will Give Unique Instance
            /// </summary>
            /// <returns></returns>
       public static ForgotPasswordBizAction GetInstance()
            {
                if (_Instance == null)
                    _Instance = new ForgotPasswordBizAction();

                return _Instance;
            }
            /// <summary>
            /// This method is override from BizAction Class. 
            /// It takes string LoginName, string Password as input Parameter and return clsUserVO.This method creates instance of UMDAL class and call the AuthenticateUser() method of UMDAL Class which execute and return the resultset.
            /// </summary>
            /// <param name="LoginName"></param>
            /// <param name="Password"></param>
            /// <returns></returns>

       public clsUserVO ForgotPassword(string LoginName, long SecretQtnId, string SecretA)
            {
                clsBaseUserManagementDAL objBaseUMDAL = clsBaseUserManagementDAL.GetInstance();
                //  return (clsUserVO)objBaseUMDAL.AuthenticateUser(LoginName, Password);
                return (clsUserVO)objBaseUMDAL.ForgotPassword(LoginName, SecretQtnId, SecretA);
            }

        }
   public class UpdateForgotPasswordBizAction
   {
       //The Private declaration
       private static UpdateForgotPasswordBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>
       public static UpdateForgotPasswordBizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new UpdateForgotPasswordBizAction();

           return _Instance;
       }
       /// <summary>
       /// This method is override from BizAction Class. 
       /// It takes string LoginName, string Password as input Parameter and return clsUserVO.This method creates instance of UMDAL class and call the AuthenticateUser() method of UMDAL Class which execute and return the resultset.
       /// </summary>
       /// <param name="LoginName"></param>
       /// <param name="Password"></param>
       /// <returns></returns>
       public clsUserVO UpdateForgotPassword(long UserID, string NewPassword)
       {
           clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
           return (clsUserVO)objBaseDAL.UpdateForgotPassword(UserID, NewPassword );
       }
   }

    
}
