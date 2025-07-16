namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration.Agency
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseAgencyMasterDAL
    {
        private static clsBaseAgencyMasterDAL _instance;

        protected clsBaseAgencyMasterDAL()
        {
        }

        public abstract IValueObject AddAgencyCliniLink(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddAgencyMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddAgentMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddServiceAgencyLink(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAgencyclinicLinkList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAgencyMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAgentDetilsByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetAgentMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetClinicMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseAgencyMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsAgencyMasterDAL";
                    _instance = (clsBaseAgencyMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetSelectedServiceList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetServiceList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetServiceToAgencyAssigned(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetServiceToAgencyAssignedCheck(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStatusAgent(IValueObject valueObject, clsUserVO objUserVO);
    }
}

