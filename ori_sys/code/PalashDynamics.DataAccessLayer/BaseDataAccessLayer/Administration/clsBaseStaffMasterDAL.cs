using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBaseStaffMasterDAL
    {
        static private clsBaseStaffMasterDAL _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        public static clsBaseStaffMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "clsStaffMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseStaffMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }

        public abstract IValueObject AddStaffMaster(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetStaffMasterList(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetStaffMasterByID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetStaffByUnitIDandID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetStaffByUnitID(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject GetUserSearchList(IValueObject valueObject, clsUserVO objUserVO);


        //added by rohinee
        public abstract IValueObject AddStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO);
             
        public abstract IValueObject GetStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffBankInfoById(IValueObject valueObject, clsUserVO objUserVO);

        public abstract IValueObject AddStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffAddressInfoById(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO);
    }
}
