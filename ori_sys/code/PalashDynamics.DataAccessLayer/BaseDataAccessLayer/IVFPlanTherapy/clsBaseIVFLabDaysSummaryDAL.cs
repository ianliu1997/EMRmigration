using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy
{
    public abstract class clsBaseIVFLabDaysSummaryDAL
    {
        static private clsBaseIVFLabDaysSummaryDAL _instance = null;

        public static clsBaseIVFLabDaysSummaryDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsIVFLabDaysSummaryDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFLabDaysSummaryDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateLabDaysSummary(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateLabDaysSummary(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);

        public abstract IValueObject GetLabDaysSummary(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetArtCycleSummary(IValueObject valueObject, clsUserVO UserVo);



        //public ValueObjects.IVFPlanTherapy.clsGetArtCycleSummaryBizActionVO GetArtCycleSummary(ValueObjects.IVFPlanTherapy.clsGetArtCycleSummaryBizActionVO obj, clsUserVO objUserVO)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
