using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters
{
    public abstract class clsBaseVisitTypeDAL
    {

        static private clsBaseVisitTypeDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseVisitTypeDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsVisitTypeDAL";//HMSConfigurationManager.GetDataAccesslayerClassName("ServPathPatientDALClassName");
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseVisitTypeDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        /// <summary>
        /// Get the VisitTpe List
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVO"></param>
        /// <returns></returns>
        public abstract IValueObject GetList(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject GetVisitTypeMaster(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject AddVisitType(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject CheckVisitTypeMappedWithPackageService(IValueObject valueObject, clsUserVO UserVo);
    }
}
