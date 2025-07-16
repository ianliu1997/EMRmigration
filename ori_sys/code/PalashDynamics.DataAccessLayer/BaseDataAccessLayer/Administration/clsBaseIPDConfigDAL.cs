using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBaseIPDConfigDAL
    {
        static private clsBaseIPDConfigDAL _instance = null;
        public static clsBaseIPDConfigDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsIPDConfigDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIPDConfigDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateBlockMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetBlockMasterDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetBlockMasterList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateBlockMasterStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetWardMasterList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateWardMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateWardMasterStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetBedMasterList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateBedMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateBedMasterStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetRoomMasterList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetRoomMasterDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateRoomMasterList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateRoomMasterStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetClassMasterDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateClassMasterDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateClassMasterStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetBedListByDifferentSearch(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetFoodItemMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateFoodItemMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateStatusFoodItemMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetDietPlanMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateDietPlanMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateDietPlanMasterStatus(IValueObject valueObject, clsUserVO UserVo);

        #region Floor Master
        public abstract IValueObject GetFloorMasterList(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject AddUpdateFloorMaster(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject UpdateFloorMasterStatus(IValueObject valueObject, clsUserVO UserVO);

        #endregion

        #region Admission Type  Master

        public abstract IValueObject GetAdmissionTypeMasterList(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject AddUpdateAdmissionTypeMaster(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject UpdateAdmissionTypeMasterStatus(IValueObject valueObject, clsUserVO UserVO);

        #endregion

        #region Admission Type  Service Linked List

        public abstract IValueObject GetAdmissionTypeServiceLinkedList(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject AddAdmissionTypeServiceList(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject GetAdmissionTypeDetailListForAdmissionTypeMasterByAdmissionTypeID(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject GetAdmisionTypeServiceList(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject AddUpdateAdmissionTypeServiceList(IValueObject valueObject, clsUserVO UserVO);

        #endregion

        public abstract IValueObject GetBedCensusAndNonCensusList(IValueObject valueObject, clsUserVO UserVo);

    }
}
