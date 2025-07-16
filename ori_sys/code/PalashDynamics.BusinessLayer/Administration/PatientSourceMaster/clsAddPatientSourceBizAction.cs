using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.PatientSourceMaster
{
   internal class clsAddPatientSourceBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion


        private static clsAddPatientSourceBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddPatientSourceBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {


            clsAddPatientSourceBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddPatientSourceBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientSourceMasterMasterDAL objBaseDAL = clsBasePatientSourceMasterMasterDAL.GetInstance();
                    obj = (clsAddPatientSourceBizActionVO)objBaseDAL.AddPatientSourceMaster(obj, objUserVO);


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


   internal class clsAddRegistrationChargesBizAction : BizAction
   {
       //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseRoleMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
       #endregion


       private static clsAddRegistrationChargesBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsAddRegistrationChargesBizAction();

           return _Instance;
       }

       protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
       {


           clsAddRegistrationChargesBizActionVO obj = null;
           int ResultStatus = 0;

           try
           {
               obj = (clsAddRegistrationChargesBizActionVO)valueObject;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBaseRegistrationChargesMasterMasterDAL objBaseDAL = clsBaseRegistrationChargesMasterMasterDAL.GetInstance();
                   obj = (clsAddRegistrationChargesBizActionVO)objBaseDAL.AddRegistartionChargesMaster(obj, objUserVO);


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
