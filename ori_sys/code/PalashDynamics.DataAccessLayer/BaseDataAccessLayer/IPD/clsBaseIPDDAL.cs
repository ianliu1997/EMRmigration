using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBaseIPDDAL
    {
        static private clsBaseIPDDAL _instance = null;

        public static clsBaseIPDDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IPD.clsIPDDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIPDDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        
        public abstract IValueObject GetIPDPatientDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddIPDBedTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIPDBedTransfer(IValueObject valueObject, clsUserVO UserVo);



        public abstract IValueObject AddIPDDischarge(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetIPDDischargeDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddMultipleBed(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetDischargeStatusDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}
