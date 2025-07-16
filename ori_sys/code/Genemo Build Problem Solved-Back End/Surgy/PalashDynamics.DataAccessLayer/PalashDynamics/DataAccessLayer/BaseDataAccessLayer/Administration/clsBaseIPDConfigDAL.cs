namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIPDConfigDAL
    {
        private static clsBaseIPDConfigDAL _instance;

        protected clsBaseIPDConfigDAL()
        {
        }

        public abstract IValueObject AddAdmissionTypeServiceList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject AddUpdateAdmissionTypeMaster(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject AddUpdateAdmissionTypeServiceList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject AddUpdateBedMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateBlockMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateClassMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDietPlanMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateFloorMaster(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject AddUpdateFoodItemMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateRoomMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateWardMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAdmisionTypeServiceList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetAdmissionTypeDetailListForAdmissionTypeMasterByAdmissionTypeID(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetAdmissionTypeMasterList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetAdmissionTypeServiceLinkedList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetBedCensusAndNonCensusList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBedListByDifferentSearch(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBedMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBlockMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBlockMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetClassMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDietPlanMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFloorMasterList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetFoodItemMaster(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIPDConfigDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsIPDConfigDAL";
                    _instance = (clsBaseIPDConfigDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetRoomMasterDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRoomMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetWardMasterList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateAdmissionTypeMasterStatus(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject UpdateBedMasterStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateBlockMasterStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateClassMasterStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateDietPlanMasterStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateFloorMasterStatus(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject UpdateRoomMasterStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusFoodItemMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateWardMasterStatus(IValueObject valueObject, clsUserVO UserVo);
    }
}

