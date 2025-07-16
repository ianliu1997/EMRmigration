using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.OpeartionTheatre
{
    public abstract class clsBaseOTDetailsDAL
    {
        static private clsBaseOTDetailsDAL _instance = null;

        public static clsBaseOTDetailsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsOTDetailsDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseOTDetailsDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        /// <summary>
        /// Gets PatientUnitID for patientID
        /// </summary>
        public abstract IValueObject GetPatientUnitIDForOtDetails(IValueObject valueObject, clsUserVO UserVo);


        /// <summary>
        /// Gets Procedures For Service ID
        /// </summary>
        public abstract IValueObject GetProceduresForServiceID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Add update OT Surgery details
        /// </summary>
        public abstract IValueObject AddUpdateOtSurgeryDetails(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Services For Procedure ID
        /// </summary>
        public abstract IValueObject GetServicesForProcedureID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Gets Doctor for OTDetails
        /// </summary>
        public abstract IValueObject GetDoctorForOTDetails(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Get OT Sheet details
        /// </summary>
        public abstract IValueObject GetOTSheetDetailsByOTID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// Add update OT details
        /// </summary>
        public abstract IValueObject AddupdatOtDetails(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        ///Gets OT Details
        /// </summary>
        public abstract IValueObject GetOTDetailsList(IValueObject valueObject, clsUserVO UserVo);

          /// <summary>
        ///Gets OT Details Tables by OT Details ID
        /// </summary>
        public abstract IValueObject GetDetailTablesOfOTDetailsByOTDetailID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        ///Gets conset details for consent id
        /// </summary>
        public abstract IValueObject GetConsetDetailsForConsentID(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        ///Add Patient Wise Consent Printing
        /// </summary>
        public abstract IValueObject AddPatientWiseConsentPrinting(IValueObject valueObject, clsUserVO UserVo);



        public abstract IValueObject UpdateProcedureScheduleStatus(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateOtDocEmpDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateOtItemDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateOtServicesDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateOtNotesDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdatePostInstructionDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateDoctorNotesDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetSurgeryDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetDocEmpDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetItemDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetServicesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetDoctorNotesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetOTNotesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo);
        







    }
}
