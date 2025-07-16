using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ANC
{
    public abstract class clsBaseANCDAL
    {
        static private clsBaseANCDAL _instance = null;

        public static clsBaseANCDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "ANC.clsANCDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseANCDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        #region General Details
        public abstract IValueObject AddUpdateANCGeneralDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetANCGeneralDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetANCGeneralDetailsList(IValueObject valueObject, clsUserVO UserVO);
        #endregion

        #region History
        public abstract IValueObject AddUpdateANCHistory(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetANCHistory(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateObestricHistory(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetObestricHistoryList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteObestericHistory(IValueObject valueObject, clsUserVO UserVo);
        
        #endregion

        #region investigation Details
        public abstract IValueObject AddUpdateInvestigationDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetANCInvestigationList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateUSGDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetANCUSGList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteUSG(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteInvestigation(IValueObject valueObject, clsUserVO UserVo);
        
        
        #endregion

        #region Examination
        public abstract IValueObject AddUpdateExaminationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetANCExaminationList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject DeleteExamination(IValueObject valueObject, clsUserVO UserVo);
        
        #endregion
        #region Documents
        public abstract IValueObject AddUpdateANCDocuments(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetANCDocumentList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteDocument(IValueObject valueObject, clsUserVO UserVo);
        
        #endregion
        #region Suggestion
        public abstract IValueObject AddUpdateSuggestion(IValueObject valueObject, clsUserVO UserVo);
       
             public abstract IValueObject GetANCSuggestion(IValueObject valueObject, clsUserVO UserVo);
        #endregion

    }
}
