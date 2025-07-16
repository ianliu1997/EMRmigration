using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.Pathology.PathologyMaster
{
    class clsGetPathoParameterBizAction : BizAction
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
        public clsGetPathoParameterBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static clsGetPathoParameterBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPathoParameterBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetPathoParameterMasterBizActionVO obj = null;
            int Resultset = 0;

            try
            {
                obj = (clsGetPathoParameterMasterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetPathoParameterMasterBizActionVO)objBaseDAL.GetParametersForList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return obj;
        }
    }

    class clsGetPathoParameterByIDBizAction : BizAction
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
        public clsGetPathoParameterByIDBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsGetPathoParameterByIDBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPathoParameterByIDBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoParameterByIDBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPathoParameterByIDBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetPathoParameterByIDBizActionVO)objBaseDAL.GetParameterByID(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }

    class clsGetPathoProfileServiceIDForPathoTestMasterBizAction : BizAction
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
        public clsGetPathoProfileServiceIDForPathoTestMasterBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsGetPathoProfileServiceIDForPathoTestMasterBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPathoProfileServiceIDForPathoTestMasterBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoProfileServiceIDForPathoTestMasterBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPathoProfileServiceIDForPathoTestMasterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsGetPathoProfileServiceIDForPathoTestMasterBizActionVO)objBaseDAL.GetPathoProfileServiceIDForPathoTestMaster(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }

    //added by rohini dated 19.2.16

    class clsGetPathoServicesByIDBizAction : BizAction
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
        public clsGetPathoServicesByIDBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsGetPathoServicesByIDBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPathoServicesByIDBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoServicesByIDBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPathoServicesByIDBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetPathoServicesByIDBizActionVO)objBaseDAL.GetServicesByID(obj, objUserVO);
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
