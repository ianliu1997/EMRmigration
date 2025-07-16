using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer
{
    public abstract class clsBasePatientDAL
    {
        static private clsBasePatientDAL _instance = null;

        public static clsBasePatientDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsPatientDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBasePatientDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        public abstract IValueObject AddPatient(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject AddPatientForPathology(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientForPathology(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
    
        // Added By CDS
        public abstract IValueObject AddPatientOPDWithTransaction(IValueObject valueObject, clsUserVO UserVo);
        //By Anjali............................
        public abstract IValueObject AddPatientOPDWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        //...................................

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetPatientGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetOTPatientGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetOTPatientPackageList(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetPatient(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetPatientForLoyaltyCardIssue(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject UpdatePatientForLoyaltyCardIssue(IValueObject valueObject, clsUserVO UserVo);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetPatientDetailsForCRM(IValueObject valueObject, clsUserVO UserVo);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetPatientDetailsForCounterSale(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientTariffs(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientFamilyDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject CheckPatientFamilyRegisterd(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetTariffAndRelationFromApplication(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientPenPusherDetailsList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject clsGetPatientPenPusherDetailByID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPrintedPatientConscent(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddPatientDietPlan(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientDietPlan(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientDietPlanDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetDietPlanMaster(IValueObject valueObject, clsUserVO UserVo);
        
        public abstract IValueObject GetPrintedPatientConscent(IValueObject valueObject, clsUserVO UserVo);

        //Added By Saily P

        /// <summary>
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public abstract IValueObject AddNewCouple(IValueObject valueObject, clsUserVO UserVo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public abstract IValueObject GetPatientList(IValueObject valueObject, clsUserVO UserVo);


        //By Anjali.............
        public abstract IValueObject AddPatientLinkFile(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientLinkFile(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCoupleGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo);


        // BY BHUSHAN . . . . . .  . 
        public abstract IValueObject ADDPatientSignConsent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientSignConsent(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeletePatientSignConsent(IValueObject valueObject, clsUserVO UserVo);

        #region IPDPatientDetails
        public abstract IValueObject GetIPDPatient(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject GetIPDPatientList(IValueObject valueObject, clsUserVO UserVO);

        public abstract IValueObject AddPatientIPDWithTransaction(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject CheckPatientDuplicacy(IValueObject valueObject, clsUserVO UserVo);

        #endregion

        #region For Pathology Additions

        //Added By Rohit Tatiya 
        public abstract IValueObject GetPatientLabUploadReportData(IValueObject valueObject, clsUserVO UserVo);

        #endregion


        //For Surrogacy
        public abstract IValueObject AddSurrogate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSurrogate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientGeneralDetailsListForSurrogacy(IValueObject valueObject, clsUserVO UserVo);

        //.......................

        //EMR
        public abstract IValueObject GetPatientHeaderDetailsForConsole(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject SavePatientPhoto(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEMRAdmVisitListByPatientID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetEMRAdmVisitListByPatientIDForConsol(IValueObject valueObject, clsUserVO objUserVO);


        public abstract IValueObject GetPatientLinkFileViewDetals(IValueObject valueObject, clsUserVO UserVo);



        //By Anjali...............................
        public abstract IValueObject GetPatientScanDoc(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddPatientScanDoc(IValueObject valueObject, clsUserVO UserVo);
        //..........................................
        //added By akshays On 18/11/2015
        public abstract IValueObject AddBarcodeImageTODB(IValueObject valueObject, clsUserVO UserVo);
        //Closed By akshays On 18/11/2015

        // Added By CDS
        public abstract IValueObject GetPatientBillBalanceAmount(IValueObject valueObject, clsUserVO UserVo);
        //


        public abstract IValueObject GetPatientDetailsForPathology(IValueObject valueObject, clsUserVO UserVo);

        //added by neena
        public abstract IValueObject GetPatientMRNoList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject MovePatientPhotoToServer(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddDonarCode(IValueObject valueObject, clsUserVO UserVo);
        //

        //***//------------------
        public abstract IValueObject GetPatientCoupleList(IValueObject valueObject, clsUserVO UserVo);
    }
}
