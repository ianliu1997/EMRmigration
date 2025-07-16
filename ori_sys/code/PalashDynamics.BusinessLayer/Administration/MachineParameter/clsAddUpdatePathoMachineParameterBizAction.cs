using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.MachineParameter;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.Administration.MachineParameter
{
    public class clsAddUpdatePathoMachineParameterBizAction : BizAction
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
        public clsAddUpdatePathoMachineParameterBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsAddUpdatePathoMachineParameterBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdatePathoMachineParameterBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatePathoMachineParameterBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddUpdatePathoMachineParameterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsAddUpdatePathoMachineParameterBizActionVO)objBaseDAL.AddUpdatePathoMachineParameter(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }

    public class clsUpdateStatusMachineParameterBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //Constructor For Long Error Info
        public clsUpdateStatusMachineParameterBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsUpdateStatusMachineParameterBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateStatusMachineParameterBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusMachineParameterBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsUpdateStatusMachineParameterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsUpdateStatusMachineParameterBizActionVO)objBaseDAL.UpdateStatusMachineParameterMaster(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }

    public class clsGetPathoMachineParameterBizAction : BizAction
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
        public clsGetPathoMachineParameterBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsGetPathoMachineParameterBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPathoMachineParameterBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPathoMachineParameterBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPathoMachineParameterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsGetPathoMachineParameterBizActionVO)objBaseDAL.GetMachineParameterList(obj, objUserVO);
                    //if (obj.GetList == true)
                    //{
                    //    // call for List
                    //    obj = (clsGetPathoMachineParameterBizActionVO)objBaseDAL.GetMachineParameterList(obj, objUserVO);
                    //}
                    //else
                    //{
                    //    //Call for details
                    //    obj = (clsGetPathoMachineParameterBizActionVO)objBaseDAL.GetMachineParameterDetails(obj, objUserVO);
                    //}

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }

    public class clsGetParameterLinkingBizAction : BizAction
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
        public clsGetParameterLinkingBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsGetParameterLinkingBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetParameterLinkingBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetParameterLinkingBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetParameterLinkingBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsGetParameterLinkingBizActionVO)objBaseDAL.GetParameterLinkingList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }

    public class clsAddUpdateParameterLinkingBizAction : BizAction
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
        public clsAddUpdateParameterLinkingBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsAddUpdateParameterLinkingBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateParameterLinkingBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateParameterLinkingBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddUpdateParameterLinkingBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsAddUpdateParameterLinkingBizActionVO)objBaseDAL.AddUpdateParameterLinking(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }

    public class clsUpdateStatusParameterLinkingBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //Constructor For Long Error Info
        public clsUpdateStatusParameterLinkingBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsUpdateStatusParameterLinkingBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateStatusParameterLinkingBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusParameterLinkingBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsUpdateStatusParameterLinkingBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsUpdateStatusParameterLinkingBizActionVO)objBaseDAL.UpdateStatusParameterLinking(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }
    //by rohini dated 18.1.2016
    public class clsGetParaByParaAndMachineBizAction : BizAction
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
        public clsGetParaByParaAndMachineBizAction()
        {
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private Declaration
        private static clsGetParaByParaAndMachineBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetParaByParaAndMachineBizAction();

            return _Instance;
        }

        ///Method Input OPDPatient: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetParaByParaAndMachineBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetParaByParaAndMachineBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsGetParaByParaAndMachineBizActionVO)objBaseDAL.GetParameterList(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }
    //
}
