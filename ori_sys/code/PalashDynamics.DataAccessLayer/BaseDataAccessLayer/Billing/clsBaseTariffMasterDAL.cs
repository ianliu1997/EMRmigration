using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    public abstract class clsBaseTariffMasterDAL
    {

        static private clsBaseTariffMasterDAL _instance = null;

        public static clsBaseTariffMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsTariffMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseTariffMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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

        public abstract IValueObject AddTariff(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetServiceByTariffID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetServicesforIssue(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetTariffList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetSpecializationsByTariffId(IValueObject valueObject, clsUserVO UserVo);


    }
}
