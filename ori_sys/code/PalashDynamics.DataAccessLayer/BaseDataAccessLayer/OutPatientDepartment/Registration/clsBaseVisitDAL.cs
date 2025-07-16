using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseVisitDAL
    {
        static private clsBaseVisitDAL _instance = null;

        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseVisitDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsVisitDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseVisitDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        public abstract IValueObject AddVisit(IValueObject valueObject, clsUserVO UserVo);
        //By Anjali..........................
        public abstract IValueObject AddVisit(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        //......................................

        public abstract IValueObject GetVisit(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetVisitCount(IValueObject valueObject, clsUserVO UserVo);
         
        public abstract IValueObject GetVisitList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetSecondLastVisit(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPastVisit(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateCurrentVisitStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientEMRVisitList(IValueObject valueObject, clsUserVO UserVo);
        
        public abstract IValueObject GetAllPendingVisitCount(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject ClosePendingVisit(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetCurrentVisit(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetEMRVisit(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetEMRdignosisFillorNot(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject AddVisitForPathology(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);

      
        
      

        

    }
}
