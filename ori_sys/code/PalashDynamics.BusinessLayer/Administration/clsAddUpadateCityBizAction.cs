using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.Administration
{
    class clsAddUpadateCityBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddUpadateCityBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpadateCityBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddUpadateCityBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddUpadateCityBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsAddUpadateCityBizActionVO)objBaseDAL.AddUpdateCityDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            return obj;
        }
    }


    class clsGetCityDetailsBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetCityDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCityDetailsBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetCityDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetCityDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsGetCityDetailsBizActionVO)objBaseDAL.GetCityDetailsList(obj, objUserVO);
                }

            }
            catch (HmsApplicationException)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            return obj;
        }
    }
}
