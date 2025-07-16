using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;

namespace PalashDynamics.DataAccessLayer
{

    public abstract class clsBasePackageServiceDAL
    {
        static private clsBasePackageServiceDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBasePackageServiceDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsPackageServiceDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBasePackageServiceDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        /// <summary>
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>

        public abstract IValueObject GetAllPackageServices(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPackageServices(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPackageServiceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageServiceDetailList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPackageServiceDetailListbyServiceID(IValueObject valueObject, clsUserVO UserVo);

        #region For IPD Module

        public abstract IValueObject GetPackageServiceForBill(IValueObject valueObject, clsUserVO UserVo);

        #endregion


    }
}
