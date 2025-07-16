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
    class clsAddUpadateRegionBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddUpadateRegionBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpadateRegionBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddUpadateRegionBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddUpadateRegionBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsAddUpadateRegionBizActionVO)objBaseDAL.AddUpdateRegionDetails(obj, objUserVO);
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


    class clsGetRegionDetailsBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetRegionDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRegionDetailsBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetRegionDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetRegionDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsGetRegionDetailsBizActionVO)objBaseDAL.GetRegionDetailsList(obj, objUserVO);
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

    //
}
