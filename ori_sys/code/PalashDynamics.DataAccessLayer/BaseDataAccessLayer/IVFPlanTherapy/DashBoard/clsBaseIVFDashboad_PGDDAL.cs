using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
  
    public abstract class clsBaseIVFDashboad_PGDDAL
    {
        static private clsBaseIVFDashboad_PGDDAL _instance = null;

        public static clsBaseIVFDashboad_PGDDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.clsIVFDashboad_PGDDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFDashboad_PGDDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        public abstract IValueObject AddUpdatePGDHistoryDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPGDHistoryDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePGDGeneralDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPGDGeneralDetails(IValueObject valueObject, clsUserVO UserVo);
        
        
       
    }
}
