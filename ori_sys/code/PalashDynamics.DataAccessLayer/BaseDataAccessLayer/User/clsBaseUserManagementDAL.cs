using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using PalashDynamics.ConfigurationManager;
//using IVF.Morpheus.Web.ValueObjects.User;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseUserManagementDAL
    {
        static private clsBaseUserManagementDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseUserManagementDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsUserManagementDAL";//HMSConfigurationManager.GetDataAccesslayerClassName("ServPathPatientDALClassName");
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseUserManagementDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public abstract clsUserVO AuthenticateUser(string LoginName, string Password);

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objUserVO"></param>
        /// <returns></returns>
        public abstract IValueObject ChangePassword(IValueObject obj, clsUserVO objUserVO);
        
        public abstract IValueObject AddUser(IValueObject valueObject, clsUserVO objUserVO);
        
        public abstract IValueObject GetUserList(IValueObject valueObject, clsUserVO objUserVO);
        
        public abstract IValueObject GetUser(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateUserStatus(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject ResetPassword(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject CheckUserLoginExists(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject CheckUserExists(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject ChangeFirstPassword(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetLoginNamePassword(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetUserDashBoard(IValueObject valueObj, clsUserVO objUserVO);

        public abstract IValueObject ForgotPassword(string Login, long SecretQtnId, string SecretA);

        //public abstract IValueObject ForgotPassword(IValueObject valueObj, clsUserVO objUserVO);

        public abstract IValueObject UpdateForgotPassword(long UserId, string NewPassword);

        public abstract IValueObject LockUser(long UserID);

        public abstract IValueObject AuthenticateLoginName(string LoginName,string Password,long UnitId);

        public abstract IValueObject UpdateUserDashBoard(IValueObject valueObj, clsUserVO objUserVO);

        public abstract IValueObject UpdateUserLockedStatus(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AutoUnLockUser(long UserID);

        public abstract IValueObject GetUnitStoreList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetUnitStoreStatusList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateUserAuditTrail(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateAuditOnClose(long AuditId);

        public abstract IValueObject AddLicenseTo(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetLicenseDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddtoDatabase(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddtoStaticDB(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetSecretQtn(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateLicenseTo(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AssignUserEMRTemplate(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetUserEMRTemplateList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetEMRMenu(IValueObject valueObject, clsUserVO objUserVO);



        public abstract IValueObject AddUserCategoryLink(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetCategoryList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetExistingCategoryListForUser(IValueObject valueObject, clsUserVO objUserVO);
    }
}
