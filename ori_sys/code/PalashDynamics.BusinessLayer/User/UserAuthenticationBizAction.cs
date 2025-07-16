using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using IVF.Morpheus.Web.BusinessLayer;
//using IVF.Morpheus.Web.ValueObjects.Patient;
//using IVF.Morpheus.Web.CustomExceptions;
//using IVF.Morpheus.Web.ValueObjects.User;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.ValueObjects.Administration.UnitMaster;


namespace PalashDynamics.BusinessLayer
{
    public class UserAuthenticationBizAction
    {
        //The Private declaration
        private static UserAuthenticationBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static UserAuthenticationBizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new UserAuthenticationBizAction();

            return _Instance;
        }
        /// <summary>
        /// This method is override from BizAction Class. 
        /// It takes string LoginName, string Password as input Parameter and return clsUserVO.This method creates instance of UMDAL class and call the AuthenticateUser() method of UMDAL Class which execute and return the resultset.
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public clsUserVO AuthenticateUser(string LoginName, string Password)
        {
            clsBaseUserManagementDAL objBaseUMDAL = clsBaseUserManagementDAL.GetInstance();
            return  (clsUserVO)objBaseUMDAL.AuthenticateUser(LoginName,Password);
        }          
   }
              
    public class LoginAuthenticationBizAction
    {
        //The Private declaration
        private static LoginAuthenticationBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static LoginAuthenticationBizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new LoginAuthenticationBizAction();

            return _Instance;
        }
        /// <summary>
        /// This method is override from BizAction Class. 
        /// It takes string LoginName, string Password as input Parameter and return clsUserVO.This method creates instance of UMDAL class and call the AuthenticateUser() method of UMDAL Class which execute and return the resultset.
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public clsUserVO LoginAuthenticate(string LoginName, string Password, long UnitId)
        {
            clsBaseUserManagementDAL objBaseDAL = clsBaseUserManagementDAL.GetInstance();
            return (clsUserVO)objBaseDAL.AuthenticateLoginName(LoginName, Password, UnitId);
        }
 
    }
}
