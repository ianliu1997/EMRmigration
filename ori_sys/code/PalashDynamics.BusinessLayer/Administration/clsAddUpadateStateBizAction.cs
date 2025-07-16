using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects.Administration;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.Administration
{
   public class clsAddUpadateStateBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddUpadateStateBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpadateStateBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddUpadateStateBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddUpadateStateBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsAddUpadateStateBizActionVO)objBaseDAL.AddUpdateStateDetails(obj, objUserVO);
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


    class clsGetStateDetailsBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetStateDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetStateDetailsBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetStateDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetStateDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterEntryDAL objBaseDAL = clsBaseMasterEntryDAL.GetInstance();
                    obj = (clsGetStateDetailsBizActionVO)objBaseDAL.GetStateDetailsList(obj, objUserVO);
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
