using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CompoundDrug
{
    public abstract class clsBaseCompoundDrugDAL
    {
        static private clsBaseCompoundDrugDAL _instance = null;

        public static clsBaseCompoundDrugDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "CompoundDrug.clsCompoundDrugDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseCompoundDrugDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        public abstract IValueObject AddCompoundDrug(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCompoundDrug(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckCompoundDrug(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCompoundDrugDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateCompoundDrugByIDAndUnitID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetCompoundDrugAndDetailsByIDAndUnitID(IValueObject valueObject, clsUserVO UserVo);
        //For the PAtient Drug
        public abstract IValueObject AddPatientCompoundDrug(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPrescriptionCompoundDrugByVisitID(IValueObject valueObject, clsUserVO UserVo);
        
    }
}
