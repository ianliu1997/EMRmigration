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
    class clsGetStateDetailsByCountyIDBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetStateDetailsByCountyIDBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetStateDetailsByCountyIDBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetStateDetailsByCountyIDBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetStateDetailsByCountyIDBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsGetStateDetailsByCountyIDBizActionVO)objBaseDAL.GetStateDetailsByCountryIDList(obj, objUserVO);

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

    class clsGetCityDetailsByStateIDBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetCityDetailsByStateIDBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetCityDetailsByStateIDBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetCityDetailsByStateIDBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetCityDetailsByStateIDBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsGetCityDetailsByStateIDBizActionVO)objBaseDAL.GetCityDetailsByStateIDList(obj, objUserVO);

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

    class clsGetRegionDetailsByCityIDBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetRegionDetailsByCityIDBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRegionDetailsByCityIDBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRegionDetailsByCityIDBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetRegionDetailsByCityIDBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsGetRegionDetailsByCityIDBizActionVO)objBaseDAL.GetRegionDetailsByCityIDList(obj, objUserVO);

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

    #region Defined to get optimised data in DAL (in terms of Data Size) 09012017

    class clsGetRegionDetailsByCityIDForRegBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetRegionDetailsByCityIDForRegBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRegionDetailsByCityIDForRegBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRegionDetailsByCityIDForRegBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetRegionDetailsByCityIDForRegBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsGetRegionDetailsByCityIDForRegBizActionVO)objBaseDAL.GetRegionDetailsByCityIDListForReg(obj, objUserVO); // Defined to get optimised data in DAL (in terms of Data Size) 09012017

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

    #endregion

}