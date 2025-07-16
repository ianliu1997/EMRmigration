using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;


namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBaseDischargeTemplateMasterDAL
    {
        static private clsBaseDischargeTemplateMasterDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseDischargeTemplateMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "Administration.clsDischargeTemplateMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseDischargeTemplateMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateDischargeTemplateMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDischargeTemplateMasterList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject CheckCountDischargeSummaryByID(IValueObject valueObject, clsUserVO objUserVO);
    }
}
