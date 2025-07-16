using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBaseDepartmentScheduleDAL
    {
        static private clsBaseDepartmentScheduleDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseDepartmentScheduleDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsDepartmentScheduleDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseDepartmentScheduleDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        public abstract IValueObject AddDepartmentScheduleMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDepartmentScheduleList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDepartmentScheduleTime(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject CheckScheduleTime(IValueObject valueObject, clsUserVO objUserVO);

        ///New DepartmentSchedule.        
        public abstract IValueObject GetDepartmentScheduleDetailsList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDepartmentDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);
    }
}
