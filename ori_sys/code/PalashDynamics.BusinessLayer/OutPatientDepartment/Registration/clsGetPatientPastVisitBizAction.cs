using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
namespace PalashDynamics.BusinessLayer
{
   internal class clsGetPatientPastVisitBizAction:BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsGetPatientPastVisitBizAction()
        {
            //Create Instance of the LogManager object
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }

            #endregion


        }

        private static clsGetPatientPastVisitBizAction _Instance = null;

        ///Method Input : none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientPastVisitBizAction();
            return _Instance;

        }

        ///Method Input : valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose    :Now Override the ProcessRequest Method of the BusinessAction class 

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetPatientPastVisitBizActionVO obj = null;
            try
            {
                obj = (clsGetPatientPastVisitBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseVisitDAL objBaseVisittDAL = clsBaseVisitDAL.GetInstance();
                    obj = (clsGetPatientPastVisitBizActionVO)objBaseVisittDAL.GetPatientPastVisit(obj, objUserVO);

                }
            }

            catch (HmsApplicationException HEx)
            {

                throw;
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

   internal class clsGetPendingVisitBizAction : BizAction
   { 
       #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsGetPendingVisitBizAction()
        {
            //Create Instance of the LogManager object
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }

            #endregion


        }

        private static clsGetPendingVisitBizAction _Instance = null;

        ///Method Input : none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPendingVisitBizAction();
            return _Instance;

        }

        ///Method Input : valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose    :Now Override the ProcessRequest Method of the BusinessAction class 

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetPendingVisitBizActioVO obj = null;
            try
            {
                obj = (clsGetPendingVisitBizActioVO)valueObject;
                if (obj != null)
                {
                    clsBaseVisitDAL objBaseVisittDAL = clsBaseVisitDAL.GetInstance();
                    obj = (clsGetPendingVisitBizActioVO)objBaseVisittDAL.GetAllPendingVisitCount(obj, objUserVO);

                }
            }

            catch (HmsApplicationException HEx)
            {

                throw;
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

   internal class clsClosePendingVisitBizAction : BizAction
   { 
       #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseServAppointmentMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        public clsClosePendingVisitBizAction()
        {
            //Create Instance of the LogManager object
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }

            #endregion


        }

        private static clsClosePendingVisitBizAction _Instance = null;

        ///Method Input : none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsClosePendingVisitBizAction();
            return _Instance;

        }

        ///Method Input : valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose    :Now Override the ProcessRequest Method of the BusinessAction class 

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsClosePendingVisitBizActioVO obj = null;
            try
            {
                obj = (clsClosePendingVisitBizActioVO)valueObject;
                if (obj != null)
                {
                    clsBaseVisitDAL objBaseVisittDAL = clsBaseVisitDAL.GetInstance();
                    obj = (clsClosePendingVisitBizActioVO)objBaseVisittDAL.ClosePendingVisit(obj, objUserVO);

                }
            }

            catch (HmsApplicationException HEx)
            {

                throw;
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
