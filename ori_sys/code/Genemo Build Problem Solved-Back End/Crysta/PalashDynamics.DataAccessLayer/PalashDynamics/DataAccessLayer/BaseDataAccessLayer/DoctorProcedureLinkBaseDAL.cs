namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class DoctorProcedureLinkBaseDAL
    {
        private static DoctorProcedureLinkBaseDAL _instance;

        protected DoctorProcedureLinkBaseDAL()
        {
        }

        public abstract IValueObject AddDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo);
        public static DoctorProcedureLinkBaseDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "DoctorProcedureLinkDAL";
                    _instance = (DoctorProcedureLinkBaseDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }
    }
}

