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
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.BusinessLayer.Master
{
   internal class clsGetDoctorMasterListBizAction:BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsGetDoctorMasterListBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }

            #endregion


        }



        private static clsGetDoctorMasterListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorMasterListBizAction();

            return _Instance;
        }
        ///Method Input Roles: valueObject
        ///Name                   :ProcessRequest    
        ///Type                   :IValueObject
        ///Direction              :input-IvalueObject output-IvalueObject
        ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetDoctorMasterListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetDoctorMasterListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsGetDoctorMasterListBizActionVO)objBaseDAL.GetDoctorMasterList(obj, objUserVO);


                }

            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //log error  

            }
            finally
            {
            }
            return obj;



            
        }



    }

   internal class clsGetUserMasterListBizAction : BizAction
   {

       //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseRoleMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
       #endregion

       //constructor For Log Error Info
       public clsGetUserMasterListBizAction()
       {
           //Create Instance of the LogManager object 
           #region Logging Code
           if (logManager == null)
           {
               logManager = LogManager.GetInstance();
           }

           #endregion


       }



       private static clsGetUserMasterListBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>
       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetUserMasterListBizAction();

           return _Instance;
       }
       ///Method Input Roles: valueObject
       ///Name                   :ProcessRequest    
       ///Type                   :IValueObject
       ///Direction              :input-IvalueObject output-IvalueObject
       ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 

       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           bool CurrentMethodExecutionStatus = true;
           clsGetUserMasterListBizActionVO obj = null;
           int ResultStatus = 0;
           try
           {
               obj = (clsGetUserMasterListBizActionVO)valueObject;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                   obj = (clsGetUserMasterListBizActionVO)objBaseDAL.GetUserMasterList(obj, objUserVO);


               }

           }
           catch (HmsApplicationException HEx)
           {
               CurrentMethodExecutionStatus = false;
               throw;
           }
           catch (Exception ex)
           {
               CurrentMethodExecutionStatus = false;
               //log error  

           }
           finally
           {
           }
           return obj;




       }



   }
}
