using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.StaffMaster
{
     internal  class clsAddStaffAddressInfoBizAction : BizAction
    {
   
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsAddStaffAddressInfoBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAddStaffAddressInfoBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsAddStaffAddressInfoBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsAddStaffAddressInfoBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddStaffAddressInfoBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseStaffMasterDAL objBaseDoctorDAL = clsStaffMasterDAL.GetInstance();
                    obj = (clsAddStaffAddressInfoBizActionVO)objBaseDoctorDAL.AddStaffAddressInfo(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return obj;

        }
    }
     internal class clsGetStaffAddressInfoBizAction : BizAction
     {
         private static clsGetStaffAddressInfoBizAction _Instance = null;
         /// <summary>
         /// To create singleton instance of the class and  This will Give Unique Instance
         /// </summary>
         /// <returns></returns>
         public static BizAction GetInstance()
         {
             if (_Instance == null)
                 _Instance = new clsGetStaffAddressInfoBizAction();

             return _Instance;
         }


         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {
             bool CurrentMethodExecutionStatus = true;


             clsGetStaffAddressInfoBizActionVO obj = null;
             int ResultStatus = 0;

             try
             {
                 obj = (clsGetStaffAddressInfoBizActionVO)valueObject;
                 if (obj != null)
                 {
                     clsBaseStaffMasterDAL objBaseDoctorDAL = clsStaffMasterDAL.GetInstance();
                     obj = (clsGetStaffAddressInfoBizActionVO)objBaseDoctorDAL.GetStaffAddressInfo(obj, objUserVO);
                 }
             }
             //catch (HmsApplicationException HEx)
             //{
             //    CurrentMethodExecutionStatus = false;
             //    throw;
             //}
             catch (Exception ex)
             {
                 //CurrentMethodExecutionStatus = false;
                 ////log error  

             }

             return valueObject;
         }
     }

     internal class clsGetStaffAddressInfoByIdBizAction : BizAction
     {
         #region Variable Declaration
         //Declare the LogManager object
         LogManager logmanager = null;
         //Declare the BaseRoleMasterDAL object
         //Declare the Variable of UserId
         long lngUserId = 0;
         #endregion

         //constructor For Log Error Info
         public clsGetStaffAddressInfoByIdBizAction()
         {
             //Create Instance of the LogManager object 
             #region Logging Code
             if (logmanager == null)
             {
                 logmanager = LogManager.GetInstance();
             }
             #endregion

         }

         private static clsGetStaffAddressInfoByIdBizAction _Instance = null;
         /// <summary>
         /// To create singleton instance of the class and  This will Give Unique Instance
         /// </summary>
         /// <returns></returns>

         public static BizAction GetInstance()
         {
             if (_Instance == null)
             {
                 _Instance = new clsGetStaffAddressInfoByIdBizAction();
             }

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {

             clsGetStaffAddressInfoByIdVO obj = null;
             int ResultStatus = 0;
             try
             {
                 obj = (clsGetStaffAddressInfoByIdVO)valueObject;
                 if (obj != null)
                 {
                     clsBaseStaffMasterDAL objBaseDoctorDAL = clsStaffMasterDAL.GetInstance();
                     obj = (clsGetStaffAddressInfoByIdVO)objBaseDoctorDAL.GetStaffAddressInfoById(obj, objUserVO);
                 }
             }
             catch (Exception ex)
             {

             }
             finally
             {

             }
             return obj;
         }
     }

     internal class clsUpdateStaffAddressInfoBizAction : BizAction
     {
         #region Variable Declaration
         //Declare the LogManager object
         LogManager logmanager = null;
         //Declare the BaseRoleMasterDAL object
         //Declare the Variable of UserId
         long lngUserId = 0;
         #endregion

         //constructor For Log Error Info
         public clsUpdateStaffAddressInfoBizAction()
         {
             //Create Instance of the LogManager object 
             #region Logging Code
             if (logmanager == null)
             {
                 logmanager = LogManager.GetInstance();
             }
             #endregion
         }

         private static clsUpdateStaffAddressInfoBizAction _Instance = null;
         /// <summary>
         /// To create singleton instance of the class and  This will Give Unique Instance
         /// </summary>
         /// <returns></returns>

         public static BizAction GetInstance()
         {
             if (_Instance == null)
             {
                 _Instance = new clsUpdateStaffAddressInfoBizAction();
             }

             return _Instance;
         }

         protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
         {

             clsUpdateStaffAddressInfoVO obj = null;
             int ResultStatus = 0;
             try
             {
                 obj = (clsUpdateStaffAddressInfoVO)valueObject;
                 if (obj != null)
                 {
                     clsBaseStaffMasterDAL objBaseDoctorDAL = clsStaffMasterDAL.GetInstance();
                     obj = (clsUpdateStaffAddressInfoVO)objBaseDoctorDAL.UpdateStaffAddressInfo(obj, objUserVO);
                 }
             }
             catch (Exception ex)
             {

             }
             finally
             {

             }
             return obj;

         }
     }
    
}
