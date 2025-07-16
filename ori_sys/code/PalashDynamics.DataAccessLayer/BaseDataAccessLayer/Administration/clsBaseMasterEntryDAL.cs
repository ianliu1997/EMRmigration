using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    public abstract class clsBaseMasterEntryDAL
    {

        static private clsBaseMasterEntryDAL _instance = null;

        public static clsBaseMasterEntryDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "Administration.clsMasterEntryDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseMasterEntryDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateCityDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetCityDetailsList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetStateDetailsByCountryIDList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetCityDetailsByStateIDList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetRegionDetailsByCityIDList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateStateDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetStateDetailsList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateCountryDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetCountryDetailsList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateRegionDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetRegionDetailsList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetAllItemListByMoluculeID(IValueObject valueObject, clsUserVO UserVo);

        // Added by CDS

        public abstract IValueObject AddUpdateCashCounterDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetCashCounterDetailsList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetCashCounterDetailsListByClinicID(IValueObject valueObject, clsUserVO UserVo);


        // added By Akshays
        public abstract IValueObject AddUpdatePriffixMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPriffixMaster(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetBdMasterList(IValueObject valueObject, clsUserVO UserVo);

        #region Defined to get optimised data in DAL (in terms of Data Size) 09012017

        public abstract IValueObject GetRegionDetailsByCityIDListForReg(IValueObject valueObject, clsUserVO UserVo);

        #endregion

    }
}
