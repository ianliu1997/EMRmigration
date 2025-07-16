using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.BusinessLayer.Administration
{
    class clsAddUpadateCountryBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddUpadateCountryBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpadateCountryBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddUpadateCountryBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddUpadateCountryBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsAddUpadateCountryBizActionVO)objBaseDAL.AddUpdateCountryDetails(obj, objUserVO);
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


    class clsGetCountryDetailsBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetCountryDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCountryDetailsBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetCountryDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetCountryDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsGetCountryDetailsBizActionVO)objBaseDAL.GetCountryDetailsList(obj, objUserVO);
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
