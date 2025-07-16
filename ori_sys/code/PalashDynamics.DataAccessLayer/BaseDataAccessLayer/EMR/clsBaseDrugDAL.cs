using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBaseDrugDAL
    {
        static private clsBaseDrugDAL _instance = null;

        public static clsBaseDrugDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsDrugDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseDrugDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        /// <summary>
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public abstract IValueObject GetDrugList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPatientPrescription(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPrescription(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdatePatientPrescription(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPrescriptionDetailByVisitID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPCR(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdatePCR(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddCaseReferral(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateCaseReferral(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddDoctorSuggestedServices(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateDoctorSuggestedServices(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetFrequencyList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddBPControlDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddVisionControlDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddGPControlDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientVital(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetItemMoleculeNameList(IValueObject valueObject, clsUserVO UserVo);

        //public abstract IValueObject GetDoctorSuggestedServiceDetailByVisitID(IValueObject valueObject, clsUserVO UserVo);

        //added by neena
        public abstract IValueObject GetPatientPrescriptionDetailByVisitIDForPrint(IValueObject valueObject, clsUserVO UserVo); 

        public abstract IValueObject AddPatientPrescriptionResason(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPrescriptionReason(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPrescriptionID(IValueObject valueObject, clsUserVO UserVo);

        //
    }
}
