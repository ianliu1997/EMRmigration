using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.TokenDisplay;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.TokenDisplay;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using PalashDynamics.ValueObjects.TokenDisplay;

namespace PalashDynamics.BusinessLayer.TokenDisplay
{
    class clsAddUpdateTokenDisplayBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddUpdateTokenDisplayBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddUpdateTokenDisplayBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            //bool CurrentMethodExecutionStatus = true;
            clsAddUpdateTokenDisplayBizActionVO obj = null;
            //int ResultStatus = 0;
            try
            {
                obj = (clsAddUpdateTokenDisplayBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseTokenDisplayDAL objBaseDAL = clsBaseTokenDisplayDAL.GetInstance();
                    obj = (clsAddUpdateTokenDisplayBizActionVO)objBaseDAL.AddUpdateTokenDisplayDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException)
            {
                //CurrentMethodExecutionStatus = false;
            }
            catch (Exception)
            {
                //CurrentMethodExecutionStatus = false;
                throw;
            }
            return obj;
        }
    }

     public class clsGetTokenDisplayBizAction 
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetTokenDisplayBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static clsGetTokenDisplayBizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetTokenDisplayBizAction();

            return _Instance;
        }
        public clsGetQueueListBizActionVO GetQueueToken(clsGetQueueListBizActionVO valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsBaseTokenDisplayDAL objBaseDAL = clsBaseTokenDisplayDAL.GetInstance();
            return (clsGetQueueListBizActionVO)objBaseDAL.GetTokenDisplayDetailsList(valueObject, UserVo);
        }
        public clsGetTokenDisplayBizActionVO GetMasterList(clsGetTokenDisplayBizActionVO valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsBaseTokenDisplayDAL objBaseDAL = clsBaseTokenDisplayDAL.GetInstance();
            return (clsGetTokenDisplayBizActionVO)objBaseDAL.GetTokenDisplayDetailsList(valueObject, UserVo);
        }

        //protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        //{

        //    //bool CurrentMethodExecutionStatus = true;
        //    clsGetTokenDisplayBizActionVO obj = null;
        //    //int ResultStatus = 0;
        //    try
        //    {
        //        obj = (clsGetTokenDisplayBizActionVO)valueObject;
        //        if (obj != null)
        //        {
        //            clsBaseTokenDisplayDAL objBaseDAL = clsBaseTokenDisplayDAL.GetInstance();
        //            obj = (clsGetTokenDisplayBizActionVO)objBaseDAL.GetTokenDisplayDetailsList(obj, objUserVO);
        //        }
        //    }
        //    catch (HmsApplicationException)
        //    {
        //        //CurrentMethodExecutionStatus = false;
        //    }
        //    catch (Exception)
        //    {
        //        //CurrentMethodExecutionStatus = false;
        //        throw;
        //    }
        //    return obj;
        //}
        
    }


    class clsUpdateTokenDisplayStatusBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsUpdateTokenDisplayStatusBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsUpdateTokenDisplayStatusBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            //bool CurrentMethodExecutionStatus = true;
            clsUpdateTokenDisplayStatusBizActionVO obj = null;
            //int ResultStatus = 0;
            try
            {
                obj = (clsUpdateTokenDisplayStatusBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseTokenDisplayDAL objBaseDAL = clsBaseTokenDisplayDAL.GetInstance();
                    obj = (clsUpdateTokenDisplayStatusBizActionVO)objBaseDAL.UpdateStatusTokenDisplay(obj, objUserVO);
                }
            }
            catch (HmsApplicationException)
            {
                //CurrentMethodExecutionStatus = false;
            }
            catch (Exception)
            {
                //CurrentMethodExecutionStatus = false;
                throw;
            }
            return obj;
        }
    }
}
