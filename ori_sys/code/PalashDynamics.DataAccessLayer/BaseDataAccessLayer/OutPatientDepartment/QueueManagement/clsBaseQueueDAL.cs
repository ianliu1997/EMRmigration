using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseQueueDAL
    {
        static private clsBaseQueueDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>

        public static clsBaseQueueDAL GetInstance()
        {
            try
            {

                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsQueueDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.

                    _instance = (clsBaseQueueDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>

        public abstract IValueObject GetQueueList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddQueueList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateQueueSortOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetQueueByID(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject UpdateDoctorInQueue(IValueObject valueObject, clsUserVO UserVo);





    }

}
