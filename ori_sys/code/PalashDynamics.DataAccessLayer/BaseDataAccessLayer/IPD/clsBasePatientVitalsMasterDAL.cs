using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using PalashDynamics.ValueObjects;
namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBasePatientVitalsMasterDAL
    {
        static private clsBasePatientVitalsMasterDAL _instance = null;

        public static clsBasePatientVitalsMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IPD.clsPatientVitalsMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBasePatientVitalsMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        public abstract IValueObject AddUpdatePatientVitalMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetPatientVitalMasterList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject UpdateStatusPatientVitalMaster(IValueObject valueObject, clsUserVO objUserVO);
    }
}
