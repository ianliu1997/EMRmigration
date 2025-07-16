using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Patient
{
    public abstract class clsBaseHSGDAL
    {
        static private clsBaseHSGDAL _instance = null;

        public static clsBaseHSGDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    
                    string _DerivedClassName = "Patient.clsHSGDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseHSGDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateHSG(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetHSGDetails(IValueObject valueObject, clsUserVO UserVo);

    }
}
