using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBaseDoctorScheduleDAL
    {
        static private clsBaseDoctorScheduleDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseDoctorScheduleDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsDoctorScheduleDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseDoctorScheduleDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }

        public abstract IValueObject AddDoctorScheduleMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetVisitTypeDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorScheduleList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorScheduleTime(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorSchedule(IValueObject valueObject, clsUserVO objUserVO);
        
        public abstract IValueObject CheckScheduleTime(IValueObject valueObject, clsUserVO objUserVO);



        ///New DoctorSchedule.
        ///Date:11-Aug-2011

        public abstract IValueObject GetDoctorScheduleDetailsList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorScheduleWise(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorScheduleDetailsListByID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorDepartmentUnitList(IValueObject valueObject, clsUserVO objUserVO);        // added on 13032018 for Doctor Schedule Change
        public abstract IValueObject GetDoctorScheduleListNew(IValueObject valueObject, clsUserVO objUserVO);           // added on 13032018 for Doctor Schedule Change
        public abstract IValueObject AddDoctorScheduleMasterNew(IValueObject valueObject, clsUserVO objUserVO);         // added on 14032018 for Doctor Schedule Change
        public abstract IValueObject GetDoctorScheduleDetailsListNew(IValueObject valueObject, clsUserVO objUserVO);    // added on 21032018 for New Doctor Schedule

    }
}
