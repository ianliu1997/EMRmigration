using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBaseUnitMasterDAL
    {
        static private clsBaseUnitMasterDAL _instance = null;


        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseUnitMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsUnitMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseUnitMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }


        public abstract IValueObject AddUnitMaster(IValueObject valueObject, clsUserVO objUserVO);
        
        public abstract IValueObject GetDepartmentList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetUnitList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUnitDetailsByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUserWiseUnitList(IValueObject valueObject, clsUserVO objUserVO);
    }
}
