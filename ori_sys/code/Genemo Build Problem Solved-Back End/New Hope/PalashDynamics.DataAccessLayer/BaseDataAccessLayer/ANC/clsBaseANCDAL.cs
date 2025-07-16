namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ANC
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseANCDAL
    {
        private static clsBaseANCDAL _instance;

        protected clsBaseANCDAL()
        {
        }

        public abstract IValueObject AddUpdateANCDocuments(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateANCGeneralDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateANCHistory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateExaminationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateInvestigationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateObestricHistory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateSuggestion(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateUSGDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteDocument(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteExamination(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteInvestigation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteObestericHistory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteUSG(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetANCDocumentList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetANCExaminationList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetANCGeneralDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetANCGeneralDetailsList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetANCHistory(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetANCInvestigationList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetANCSuggestion(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetANCUSGList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseANCDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "ANC.clsANCDAL";
                    _instance = (clsBaseANCDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetObestricHistoryList(IValueObject valueObject, clsUserVO UserVo);
    }
}

