using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    public abstract class clsBaseIVFDashboard_SemenDAL
    {
        static private clsBaseIVFDashboard_SemenDAL _instance = null;

        public static clsBaseIVFDashboard_SemenDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.clsIVFDashboard_SemenDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFDashboard_SemenDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        public abstract IValueObject AddUpdateSemenExaminationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenExaminationDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateSemenWashDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenWashDetails(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetNewIUIDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateSemenUsedDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenUsedDetails(IValueObject valueObject, clsUserVO UserVo);

    }
}
