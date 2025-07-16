namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseUserManagementDAL
    {
        private static clsBaseUserManagementDAL _instance;

        protected clsBaseUserManagementDAL()
        {
        }

        public abstract IValueObject AddLicenseTo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddtoDatabase(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddtoStaticDB(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUser(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUserCategoryLink(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AssignUserEMRTemplate(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AuthenticateLoginName(string LoginName, string Password, long UnitId);
        public abstract clsUserVO AuthenticateUser(string LoginName, string Password);
        public abstract IValueObject AutoUnLockUser(long UserID);
        public abstract IValueObject ChangeFirstPassword(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject ChangePassword(IValueObject obj, clsUserVO objUserVO);
        public abstract IValueObject CheckUserExists(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject CheckUserLoginExists(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject ForgotPassword(string Login, long SecretQtnId, string SecretA);
        public abstract IValueObject GetCategoryList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetEMRMenu(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetExistingCategoryListForUser(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseUserManagementDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsUserManagementDAL";
                    _instance = (clsBaseUserManagementDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetLicenseDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetLoginNamePassword(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetSecretQtn(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUnitStoreList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUnitStoreStatusList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUser(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUserDashBoard(IValueObject valueObj, clsUserVO objUserVO);
        public abstract IValueObject GetUserEMRTemplateList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUserList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject LockUser(long UserID);
        public abstract IValueObject ResetPassword(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateAuditOnClose(long AuditId);
        public abstract IValueObject UpdateForgotPassword(long UserId, string NewPassword);
        public abstract IValueObject UpdateLicenseTo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateUserAuditTrail(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateUserDashBoard(IValueObject valueObj, clsUserVO objUserVO);
        public abstract IValueObject UpdateUserLockedStatus(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateUserStatus(IValueObject valueObject, clsUserVO objUserVO);
    }
}

