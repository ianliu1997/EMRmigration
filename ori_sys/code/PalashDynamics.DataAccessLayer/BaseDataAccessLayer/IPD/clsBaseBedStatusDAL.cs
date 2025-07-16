using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBaseBedStatusDAL
    {
        static private clsBaseBedStatusDAL _instance = null;

        public static clsBaseBedStatusDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "IPD.clsBedStatusDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseBedStatusDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject ViewBedStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetWardByFloor(IValueObject valueObject, clsUserVO UserVo);
    }
}