using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
   public abstract class clsBaseIVFDashboad_OPUDAL
    {
       static private clsBaseIVFDashboad_OPUDAL _instance = null;

       public static clsBaseIVFDashboad_OPUDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.clsIVFDashboad_OPUDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFDashboad_OPUDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
       public abstract IValueObject AddUpdateOPUDetails(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject GetOPUDetails(IValueObject valueObject, clsUserVO UserVo);

       //added by neena dated 28/7/16
       public abstract IValueObject AddUpdateOocyteNumber(IValueObject valueObject, clsUserVO UserVo);

       public abstract IValueObject GetEmbryologySummary(IValueObject valueObject, clsUserVO UserVo);
       //

    }
   
    
}
