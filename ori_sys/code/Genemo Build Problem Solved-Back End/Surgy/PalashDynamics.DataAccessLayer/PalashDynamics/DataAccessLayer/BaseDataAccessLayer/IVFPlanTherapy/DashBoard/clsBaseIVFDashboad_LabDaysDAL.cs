namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFDashboad_LabDaysDAL
    {
        private static clsBaseIVFDashboad_LabDaysDAL _instance;

        protected clsBaseIVFDashboad_LabDaysDAL()
        {
        }

        public abstract IValueObject AddDay0OocList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddLabDayDocuments(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddObervationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDay0Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDay1Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDay2Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDay3Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDay4Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDay5Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDay6Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDay7Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDecision(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateETDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateFertCheckDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateGraphicalRepList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateMediaDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePlanDecision(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay0Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay0OocList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay0OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay0OocyteDetailsOocyteRecipient(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay1Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay1OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay1OocyteObservations(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay2Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay2OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay3Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay3OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay4Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay4OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay5Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay5OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay6Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay6OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay7Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay7OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetETDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFertCheckDate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFertCheckDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGraphicalRepOocList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFDashboad_LabDaysDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFDashboad_LabDaysDAL";
                    _instance = (clsBaseIVFDashboad_LabDaysDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIVFICSIPlannedOocyteDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMediaDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetObservationDate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenSampleList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateAndGetImageListDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

