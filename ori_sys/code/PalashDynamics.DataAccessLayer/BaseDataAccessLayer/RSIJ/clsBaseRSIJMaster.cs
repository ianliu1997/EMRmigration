using System;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.RSIJ
{
    public abstract class clsBaseRSIJMasterDAL
    {
        static private clsBaseRSIJMasterDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseRSIJMasterDAL GetInstance()
        {
            try
            {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "RSIJ.clsRSIJMasterDAL";//HMSConfigurationManager.GetDataAccesslayerClassName("ServPathPatientDALClassName");
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseRSIJMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        /// <summary>
        /// Get the Master List
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVO"></param>
        /// <returns></returns>
        public abstract IValueObject GetMasterList(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject GetDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetOPDQueueList(IValueObject valueObject, clsUserVO objUserVO);
        
        /// <summary>
        /// This Method is Used to Get all the Diagnosis Master Table Records.
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetDiagnosisList(IValueObject valueObject, clsUserVO UserVo);
        /// <summary>
        /// This Method is Used to Get all the Drug Master Table Records.
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetItemList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetDoctorScheduleTime(IValueObject valueObject, clsUserVO objUserVO);


    }
}
