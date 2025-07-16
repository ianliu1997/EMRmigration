using System.Text;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using System;

namespace PalashDynamics.BusinessLayer.Administration
{
   internal class clsGetServiceMasterListBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetServiceMasterListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>



        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServiceMasterListBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceMasterListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetServiceMasterListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                    if (obj.GetAllServiceListDetails==true)
                    {
                        obj = (clsGetServiceMasterListBizActionVO)objBaseDAL.GetAllServiceList(obj, objUserVO);  
                    }
                    else if(obj.GetAllTariffIDDetails==true)
                    {
                        obj = (clsGetServiceMasterListBizActionVO)objBaseDAL.GetAllTariffApplicableList(obj, objUserVO);  
                    }
                    else if (obj.GetAllServiceClassRateDetails == true)
                    {
                        obj = (clsGetServiceMasterListBizActionVO)objBaseDAL.GetAllServiceClassRateDetails(obj, objUserVO);
                    }
                    else if (obj.GetTariffServiceMasterID == true)
                    {
                        obj = (clsGetServiceMasterListBizActionVO)objBaseDAL.GetTariffServiceMasterID(obj, objUserVO);
                    }
                        //Added By Pallavi
                    else if (obj.GetAllServiceMasterDetailsForID == true)
                    {
                        obj = (clsGetServiceMasterListBizActionVO)objBaseDAL.GetServiceMasterDetailsForId(obj, objUserVO);
                    }
                    else if (obj.IsFromDocSerRateCat == true)
                    {
                        obj = (clsGetServiceMasterListBizActionVO)objBaseDAL.GetServiceListForDocSerRateCat(obj, objUserVO);
                    }
                    else if (obj.GetServicesForPathology == true)
                    {
                        obj = (clsGetServiceMasterListBizActionVO)objBaseDAL.GetServiceListForPathology(obj, objUserVO);
                    }
                    else 
                    {
                        obj = (clsGetServiceMasterListBizActionVO)objBaseDAL.GetServiceList(obj, objUserVO);

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
   
    // Added Only For IPD by CDS

   internal class clsGetServiceWiseClassRateBizAction : BizAction
   {
       //This Region Contains Variables Which are Used At Form Level
       #region Variables Declaration
       //Declare the LogManager object
       LogManager logManager = null;
       //Declare the BaseRoleMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
       #endregion

       private static clsGetServiceWiseClassRateBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>



       public static BizAction GetInstance()
       {
           if (_Instance == null)
               _Instance = new clsGetServiceWiseClassRateBizAction();

           return _Instance;
       }

       protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
       {
           clsGetServiceWiseClassRateBizActionVO obj = null;
           int ResultStatus = 0;

           try
           {
               obj = (clsGetServiceWiseClassRateBizActionVO)valueObject;
               //Typecast the "valueObject" to "clsInputOutputVO"
               if (obj != null)
               {
                   clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();
                   obj = (clsGetServiceWiseClassRateBizActionVO)objBaseDAL.GetServiceClassRateList(obj, objUserVO);

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

}
