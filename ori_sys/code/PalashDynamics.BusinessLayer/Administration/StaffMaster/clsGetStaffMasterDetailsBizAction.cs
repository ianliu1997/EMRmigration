using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects.Administration.StaffMaster;

namespace PalashDynamics.BusinessLayer.Administration.StaffMaster
{
   internal class clsGetStaffMasterDetailsBizAction:BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetStaffMasterDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetStaffMasterDetailsBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {


            clsGetStaffMasterDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetStaffMasterDetailsBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseStaffMasterDAL objBaseDAL = clsBaseStaffMasterDAL.GetInstance();
                    obj = (clsGetStaffMasterDetailsBizActionVO)objBaseDAL.GetStaffMasterList(obj, objUserVO);


                }

            }
            catch (HmsApplicationException HEx)
            {

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

   internal class clsGetStaffMasterByUnitIDBizAction : BizAction
   {

       //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseRoleMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
       #endregion

       private static clsGetStaffMasterByUnitIDBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetStaffMasterByUnitIDBizAction();

           return _Instance;
       }

       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           clsGetStaffMasterByUnitIDBizActionVO obj = null;
           int ResultStatus = 0;

           try
           {
               obj = (clsGetStaffMasterByUnitIDBizActionVO)valueObject;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj.IsStaffPatient)
               {
                   if (obj != null)
                   {
                       clsBaseStaffMasterDAL objBaseDAL = clsBaseStaffMasterDAL.GetInstance();
                       obj = (clsGetStaffMasterByUnitIDBizActionVO)objBaseDAL.GetStaffByUnitIDandID(obj, objUserVO);


                   }

               }
               else
               {
                   if (obj != null)
                   {
                       clsBaseStaffMasterDAL objBaseDAL = clsBaseStaffMasterDAL.GetInstance();
                       obj = (clsGetStaffMasterByUnitIDBizActionVO)objBaseDAL.GetStaffByUnitID(obj, objUserVO);


                   }
               }


           }
           catch (HmsApplicationException HEx)
           {

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

   internal class clsGetUserSearchBizAction : BizAction
   {
       private static clsGetUserSearchBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetUserSearchBizAction();

           return _Instance;
       }

       protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
       {
           clsGetUserSearchBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetUserSearchBizActionVO)valueObject;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBaseStaffMasterDAL objBaseDAL = clsBaseStaffMasterDAL.GetInstance();
                   obj = (clsGetUserSearchBizActionVO)objBaseDAL.GetUserSearchList(obj, objUserVO);
               }
           }
           catch (HmsApplicationException HEx)
           {
               throw HEx;
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           {

           }
           return obj;
       }
   }
}
