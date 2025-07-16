using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseAdvanceDAL
    {

        static private clsBaseAdvanceDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseAdvanceDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsAdvanceDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseAdvanceDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        public abstract IValueObject AddAdvance(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetAdvance(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateAdvance(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetAdvanceList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientAdvanceList(IValueObject valueObject, clsUserVO UserVo);
 
        public abstract IValueObject DeleteAdvance(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddAdvance(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);     // 20042017 Refund To Advance

        public abstract IValueObject GetAdvanceListForRequestApproval(IValueObject valueObject, clsUserVO UserVo);

        // For Package Advance & Bill Save in transaction : added on 16082018
        public abstract IValueObject AddAdvanceWithPackageBill(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);   
    }
}
