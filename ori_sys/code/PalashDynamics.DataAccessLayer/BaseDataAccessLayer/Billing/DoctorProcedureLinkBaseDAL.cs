using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;


namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer
{
    public abstract class DoctorProcedureLinkBaseDAL
    {
        static private DoctorProcedureLinkBaseDAL _instance = null;

        public static DoctorProcedureLinkBaseDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "DoctorProcedureLinkDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (DoctorProcedureLinkBaseDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        public abstract IValueObject AddDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject DeleteDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo);

    }
}
