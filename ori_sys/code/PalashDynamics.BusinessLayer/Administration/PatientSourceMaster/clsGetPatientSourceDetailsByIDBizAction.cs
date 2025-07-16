using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.PatientSourceMaster
{
   public class clsGetPatientSourceDetailsByIDBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion


        private static clsGetPatientSourceDetailsByIDBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientSourceDetailsByIDBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {


            clsGetPatientSourceDetailsByIDBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPatientSourceDetailsByIDBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientSourceMasterMasterDAL objBaseDAL = clsBasePatientSourceMasterMasterDAL.GetInstance();
                    obj = (clsGetPatientSourceDetailsByIDBizActionVO)objBaseDAL.GetPatientSourceByID(obj, objUserVO);


                }

            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }


   public class clsGetRegistrationChargesDetailsByIDBizAction : BizAction
   {
       //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseRoleMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
       #endregion


       private static clsGetRegistrationChargesDetailsByIDBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetRegistrationChargesDetailsByIDBizAction();

           return _Instance;
       }

       protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
       {


           clsGetRegistrationChargesDetailsByIDBizActionVO obj = null;
           int ResultStatus = 0;

           try
           {
               obj = (clsGetRegistrationChargesDetailsByIDBizActionVO)valueObject;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBaseRegistrationChargesMasterMasterDAL objBaseDAL = clsBaseRegistrationChargesMasterMasterDAL.GetInstance();
                   obj = (clsGetRegistrationChargesDetailsByIDBizActionVO)objBaseDAL.GetRegistrationChargesByID(obj, objUserVO);


               }

           }

           catch (Exception ex)
           {
               throw;
           }
           finally
           {

           }
           return obj;
       }
   }

   // Added By CDS 
   public class clsGetRegistrationChargesDetailsByPatientTypeIDBizAction : BizAction
   {
       //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseRoleMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
       #endregion


       private static clsGetRegistrationChargesDetailsByPatientTypeIDBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetRegistrationChargesDetailsByPatientTypeIDBizAction();

           return _Instance;
       }

       protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
       {


           clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO obj = null;
           int ResultStatus = 0;

           try
           {
               obj = (clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO)valueObject;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBaseRegistrationChargesMasterMasterDAL objBaseDAL = clsBaseRegistrationChargesMasterMasterDAL.GetInstance();
                   obj = (clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO)objBaseDAL.GetRegistrationChargesByPatientTypeID(obj, objUserVO);


               }

           }

           catch (Exception ex)
           {
               throw;
           }
           finally
           {

           }
           return obj;
       }
   }
}
