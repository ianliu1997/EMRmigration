using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
   public abstract class clsBaseIVFDashboard_IUIDAL
    {
        static private clsBaseIVFDashboard_IUIDAL _instance = null;

       public static clsBaseIVFDashboard_IUIDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.clsIVFDashboard_IUIDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFDashboard_IUIDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

       public abstract IValueObject AddUpdateIUIDetails(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject GetIUIDetails(IValueObject valueObject, clsUserVO UserVo);
       
    }

    }

