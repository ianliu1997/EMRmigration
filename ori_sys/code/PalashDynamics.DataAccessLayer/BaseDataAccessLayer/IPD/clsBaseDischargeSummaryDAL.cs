using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBaseDischargeSummaryDAL
    {
        static private clsBaseDischargeSummaryDAL _instance = null;

        public static clsBaseDischargeSummaryDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "IPD.clsDischargeSummaryDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseDischargeSummaryDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        public abstract IValueObject GetIPDDischargeSummaryList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDischargeSummary(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientsDischargeSummaryInfoInHTML(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FillDataGridDischargeSummaryList(IValueObject valueObject, clsUserVO UserVo);
        

        
    }
}
