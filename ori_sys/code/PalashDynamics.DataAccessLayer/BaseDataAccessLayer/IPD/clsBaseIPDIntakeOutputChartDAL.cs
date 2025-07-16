using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBaseIPDIntakeOutputChartDAL
    {
        static private clsBaseIPDIntakeOutputChartDAL _instance = null;

        public static clsBaseIPDIntakeOutputChartDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsIPDIntakeOutputChartDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIPDIntakeOutputChartDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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



        #region
        //Added By Kiran for

        public abstract IValueObject AddUpdateIntakeOutputChart(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetIntakeOutputChartDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetIntakeOutputChartDetailsByPatientID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateStatusIntakeOutputChart(IValueObject valueObject, clsUserVO UserVo);

        //------Added by Ashutosh 21/11/2013--------------//
        public abstract IValueObject UpdateIsFreezedStatus(IValueObject valueObject, clsUserVO UserVo);

        #endregion
    }
}
