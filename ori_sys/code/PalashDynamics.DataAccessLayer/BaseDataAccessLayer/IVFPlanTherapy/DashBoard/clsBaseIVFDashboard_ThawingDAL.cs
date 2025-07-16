using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    public abstract class clsBaseIVFDashboard_ThawingDAL
    {

        static private clsBaseIVFDashboard_ThawingDAL _instance = null;

        public static clsBaseIVFDashboard_ThawingDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.clsIVFDashboard_ThawingDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFDashboard_ThawingDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateIVFDashBoard_Thawing(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject AddUpdateIVFDashBoard_ThawingForOocyte(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashBoard_Thawing(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject GetIVFDashBoard_ThawingForOocyte(IValueObject valueObject, clsUserVO UserVo);

        //added by neena
        public abstract IValueObject AddUpdateIVFDashBoard_ThawingWOCryo(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoard_ThawingOocyte(IValueObject valueObject, clsUserVO UserVo);    
        //
    }
}