using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsAddUpdateSurrogactAgencyMasterBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsAddUpdateSurrogactAgencyMasterBizAction()
        { 
           //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

       }

        private static clsAddUpdateSurrogactAgencyMasterBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
           {
               _Instance = new clsAddUpdateSurrogactAgencyMasterBizAction();
           }

           return _Instance;
       }
       
       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsAddUpdateSurrogactAgencyMasterBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUpdateSurrogactAgencyMasterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDoctorDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsAddUpdateSurrogactAgencyMasterBizActionVO)objBaseDoctorDAL.AddUpdateSurrogactAgencyDetails(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;
        }

    }

    internal class clsAddUpdateCleavageGradeMasterBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsAddUpdateCleavageGradeMasterBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

        }

        private static clsAddUpdateCleavageGradeMasterBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsAddUpdateCleavageGradeMasterBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsAddUpdateCleavageGradeMasterBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUpdateCleavageGradeMasterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDoctorDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsAddUpdateCleavageGradeMasterBizActionVO)objBaseDoctorDAL.AddUpdateCleavageGradeDetails(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;
        }

    }



        internal class clsGetSurrogactAgencyMasterBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetSurrogactAgencyMasterBizAction()
        { 
           //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

       }

        private static clsGetSurrogactAgencyMasterBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
           {
               _Instance = new clsGetSurrogactAgencyMasterBizAction();
           }

           return _Instance;
       }
       
       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetSurrogactAgencyMasterBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetSurrogactAgencyMasterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDoctorDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetSurrogactAgencyMasterBizActionVO)objBaseDoctorDAL.GetSurrogactAgencyDetails(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;
        }

    }
            internal class clsUpdateStatusSurrogactAgencyMasterBizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsUpdateStatusSurrogactAgencyMasterBizAction()
        { 
           //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion

       }

        private static clsUpdateStatusSurrogactAgencyMasterBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
           {
               _Instance = new clsUpdateStatusSurrogactAgencyMasterBizAction();
           }

           return _Instance;
       }
       
       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsUpdateStatusSurrogactAgencyMasterBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsUpdateStatusSurrogactAgencyMasterBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDoctorDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsUpdateStatusSurrogactAgencyMasterBizActionVO)objBaseDoctorDAL.UpdateStatusSurrogactAgencyDetails(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return obj;
        }

    }
    
}
