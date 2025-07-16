using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration.Agency
{
    public abstract class clsBaseAgencyMasterDAL
    {
        static private clsBaseAgencyMasterDAL _instance = null;
        public static clsBaseAgencyMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsAgencyMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseAgencyMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }

        public abstract IValueObject AddAgencyMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetAgencyMasterList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddAgencyCliniLink(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetAgencyclinicLinkList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetClinicMasterList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetServiceList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetSelectedServiceList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddServiceAgencyLink(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetServiceToAgencyAssigned(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetServiceToAgencyAssignedCheck(IValueObject valueObject, clsUserVO objUserVO); //for service assigned to agency check before delete

        //added by neena
        public abstract IValueObject AddAgentMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAgentMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAgentDetilsByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStatusAgent(IValueObject valueObject, clsUserVO objUserVO);
        //

    }
    
   
}
