using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer
{
    internal class clsAddPatientSponsorBizAction : BizAction 
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsAddPatientSponsorBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsAddPatientSponsorBizAction _Instance = null;


     
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddPatientSponsorBizAction();

            return _Instance;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="objUserVO"></param>
        /// <returns></returns>
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsAddPatientSponsorBizActionVO obj = null;

            int ResultStatus = 0;

            try
            {
                obj = (clsAddPatientSponsorBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientSposorDAL objBasePatientSposorDAL = clsBasePatientSposorDAL.GetInstance();
                    obj = (clsAddPatientSponsorBizActionVO)objBasePatientSposorDAL.AddPatientSponsor(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return obj;

        }
    }


   internal class clsDeletePatientSponsorBizAction : BizAction 
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsDeletePatientSponsorBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsDeletePatientSponsorBizAction _Instance = null;


     
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsDeletePatientSponsorBizAction();

            return _Instance;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueObject"></param>
        /// <param name="objUserVO"></param>
        /// <returns></returns>
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsDeletePatientSponsorBizActionVO obj = null;

            int ResultStatus = 0;

            try
            {
                obj = (clsDeletePatientSponsorBizActionVO)valueObject;

                if (obj != null)
                {
                    clsBasePatientSposorDAL objBasePatientSposorDAL = clsBasePatientSposorDAL.GetInstance();
                    obj = (clsDeletePatientSponsorBizActionVO)objBasePatientSposorDAL.DeletePatientSponsor(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return obj;

        }
    }


   internal class clsAddPatientSponsorForPathologyBizAction : BizAction
   {
       //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseOPDPatientMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
       #endregion

       //constructor For Log Error Info
       private clsAddPatientSponsorForPathologyBizAction()
       {
           //Create Instance of the LogManager object 
           #region Logging Code
           if (logManager == null)
           {
               logManager = LogManager.GetInstance();
           }
           #endregion
       }

       //The Private declaration
       private static clsAddPatientSponsorForPathologyBizAction _Instance = null;



       ///Name:GetInstance       
       ///Type:static
       ///Direction:None
       ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsAddPatientSponsorForPathologyBizAction();

           return _Instance;
       }



       /// <summary>
       /// 
       /// </summary>
       /// <param name="valueObject"></param>
       /// <param name="objUserVO"></param>
       /// <returns></returns>
       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;

           clsAddPatientSponsorForPathologyBizActionVO obj = null;

           int ResultStatus = 0;

           try
           {
               obj = (clsAddPatientSponsorForPathologyBizActionVO)valueObject;

               if (obj != null)
               {
                   clsBasePatientSposorDAL objBasePatientSposorDAL = clsBasePatientSposorDAL.GetInstance();
                   obj = (clsAddPatientSponsorForPathologyBizActionVO)objBasePatientSposorDAL.AddPatientSponsorForPathology(obj, objUserVO);
               }
           }
           catch (Exception ex)
           {

               throw;
           }

           return obj;

       }
   }
}
