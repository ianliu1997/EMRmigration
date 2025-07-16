using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy
{
    public abstract class  clsBaseIVFPlanTherapyDAL
    {
        static private clsBaseIVFPlanTherapyDAL _instance = null;

        public static clsBaseIVFPlanTherapyDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsIVFPlanTherapyDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFPlanTherapyDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        #region Therapy Execution Calendar
        public abstract IValueObject AddUpdatePlanTherapy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTherapyDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTherapyDrugDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateFollicularMonitoring(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFollicularMonitoringSizeList(IValueObject valueObject, clsUserVO UserVo);
        
        //Added by Saily P
        public abstract IValueObject GetFollicularModified(IValueObject valueObject, clsUserVO UserVo);

        #endregion

        #region Couple Details

        public abstract IValueObject GetPatientEMRDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCoupleDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCoupleHeightAndWeight(IValueObject valueObject, clsUserVO UserVo);

        #endregion

        #region Vitrification
        public abstract IValueObject getVitrificationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateVitrificationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVitrificationForSpermCryoBank(IValueObject valueObject, clsUserVO UserVo);
        //Sperm Vitrification
        public abstract IValueObject GetSpermVitrificationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSpermThawingDetails(IValueObject valueObject, clsUserVO UserVo);
        #endregion

        #region Thawing
        public abstract IValueObject getThawingDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateThawingDetails(IValueObject valueObject, clsUserVO UserVo);
        //Added by Saily P
        public abstract IValueObject AddUpdateSpermThawingDetails(IValueObject valueObject, clsUserVO UserVo);
        #endregion

        #region ET
        public abstract IValueObject getETDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateETDetails(IValueObject valueObject, clsUserVO UserVo);

        #endregion

        #region ET and OPU DashBoard
        public abstract IValueObject GetTherapyDashBoard(IValueObject valueObject, clsUserVO UserVo);
        #endregion
        public abstract IValueObject AddDiscardForSpermCryoBank(IValueObject valueObject, clsUserVO UserVo);

    }
}
