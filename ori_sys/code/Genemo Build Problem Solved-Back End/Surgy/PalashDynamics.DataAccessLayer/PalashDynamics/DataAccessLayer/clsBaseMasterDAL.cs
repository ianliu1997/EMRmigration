namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseMasterDAL
    {
        private static clsBaseMasterDAL _instance;

        protected clsBaseMasterDAL()
        {
        }

        public abstract IValueObject AddEmailTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddSMSTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateCleavageGradeDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUpdateCurrencyMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUpdateMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUpdateParameter(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUpdateSurrogactAgencyDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUserRole(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject CheckDuplicasy(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject DeptFromSubSpecilization(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAllDoctorList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAnesthetist(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAutoCompleteList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAutoCompleteList_2colums(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBdMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCodeMasterList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetColumnListByTableName(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetCurrencyMasterListDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDashBoardList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDataToPrint(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorListBySpecializationID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEmailTemplateDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEmailTemplateList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEmbryologist(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsMasterDAL";
                    _instance = (clsBaseMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetMarketingExecutivesList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMasterList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetMasterListByTableNameAndColumnName(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetMasterListDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetMasterListForConsent(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetMasterNames(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetMasterSearchList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetOtherThanReferralDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetParameterByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetParametersForList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPathoFasting(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPathologist(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPathoParameter(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPathoUsers(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPatientMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRadiologist(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetRefDoctor(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetRoleGeneralDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRoleList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSelectedRoleMenuId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServicesByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetSMSTemplate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSMSTemplateList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetStoreForComboBox(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GETSupplierList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetSurrogactAgencyDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUnitContactNo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUserMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateEmailTemplateStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateParameterStatus(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateSMSTemplateStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatus(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject UpdateStatusSurrogactAgencyDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStausMaster(IValueObject valueObject, clsUserVO objUserVO);
    }
}

