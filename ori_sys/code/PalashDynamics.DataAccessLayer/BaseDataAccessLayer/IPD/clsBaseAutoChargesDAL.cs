using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBaseAutoChargesDAL
    {
        static private clsBaseAutoChargesDAL _instance = null;

        public static clsBaseAutoChargesDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "IPD.clsAutoChargesDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseAutoChargesDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        public abstract IValueObject AddAutoChargesServiceList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetAutoChargesServiceList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject DeleteService(IValueObject valueObject, clsUserVO UserVo);


    }
}
