using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseMasterDAL
    {
        static private clsBaseMasterDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsMasterDAL";//HMSConfigurationManager.GetDataAccesslayerClassName("ServPathPatientDALClassName");
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        /// <summary>
        /// Get the Master List
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVO"></param>
        /// <returns></returns>
        public abstract IValueObject GetMasterList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetCodeMasterList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GETSupplierList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetRefDoctor(IValueObject valueObject, clsUserVO UserVO);


      //  public abstract IValueObject GetVisitTypeDetails(IValueObject valueObject, clsUserVO UserVO);

        


        public abstract IValueObject GetMasterSearchList(IValueObject valueObject, clsUserVO UserVO);


        public abstract IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO UserVO);
        // Added By Harish
        // Date 1 Aug 2011
        // For Dynamic get Master list from Any Master Table with Dynamic Column Names
        public abstract IValueObject GetMasterListByTableNameAndColumnName(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetColumnListByTableName(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject AddUserRole(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject GetRoleGeneralDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetSelectedRoleMenuId(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetRoleList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetAutoCompleteList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAutoCompleteList_2colums(IValueObject valueObject, clsUserVO UserVo); // By BHUSHAN
        public abstract IValueObject GetDashBoardList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientMasterList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetDoctorMasterList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateStatus(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject GetUserMasterList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetSMSTemplateList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddSMSTemplate(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetSMSTemplate(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateSMSTemplateStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetEmailTemplateList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetEmailTemplateDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddEmailTemplate(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateEmailTemplateStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateMasterList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateStausMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetMasterListDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorListBySpecializationID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetRadiologist(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddUpdateParameter(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateParameterStatus(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetParameterByID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetParametersForList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPathologist(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetEmbryologist(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetAnesthetist(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPathoUsers(IValueObject valueObject, clsUserVO objUserVO);
        // BY BHUSHAN . . . . . . .
        public abstract IValueObject GetPathoParameter(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetCurrencyMasterListDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddUpdateCurrencyMasterList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetOtherThanReferralDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);
        #region For IPD Module

        //By Anjali On 03/04/2014
        public abstract IValueObject GetMasterListForConsent(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject DeptFromSubSpecilization(IValueObject valueObject, clsUserVO objUserVO);

        #endregion

        #region For Pathology Additions

        public abstract IValueObject GetUnitContactNo(IValueObject valueObject, clsUserVO objUserVO);

        #endregion

        //By Rohinee On 05/08/2015
       // public abstract IValueObject GetAuthorisedUserMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMasterNames(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDataToPrint(IValueObject valueObject, clsUserVO objUserVO);


        //rohinee
        public abstract IValueObject AddUpdateSurrogactAgencyDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetSurrogactAgencyDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStatusSurrogactAgencyDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPathoFasting(IValueObject valueObject, clsUserVO objUserVO);
     //   public abstract IValueObject AddServicesToParameter(IValueObject valueObject, clsUserVO userVO); //FOR parameter details
     //   public abstract IValueObject GetServicesToParameterList(IValueObject valueObject, clsUserVO userVO);  //FOR parameter details
        // By Umesh
        //By Rohinee On 28/12/2015 for all doctors 
        public abstract IValueObject GetAllDoctorList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetServicesByID(IValueObject valueObject, clsUserVO objUserVO);
        //By Anjali.................................................................
        public abstract IValueObject GetStoreForComboBox(IValueObject valueObject, clsUserVO objUserVO);
        
        //..........................................................................

        public abstract IValueObject CheckDuplicasy(IValueObject valueObject, clsUserVO objUserVO);  // By Umesh

        //by neena
        public abstract IValueObject AddUpdateCleavageGradeDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetBdMasterList(IValueObject valueObject, clsUserVO UserVo);//Added By YK

        //***//
        public abstract IValueObject GetMarketingExecutivesList(IValueObject valueObject, clsUserVO UserVo);

    }
}
