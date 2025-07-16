using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    internal class clsIPDGetAdmissionTypeServiceListBizAction : BizAction
    {
        private static clsIPDGetAdmissionTypeServiceListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsIPDGetAdmissionTypeServiceListBizAction();

            return _Instance;
        }


        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;


            clsIPDGetAdmissionTypeServiceListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsIPDGetAdmissionTypeServiceListBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseIPDConfigDAL objBaseMasterDAL = clsBaseIPDConfigDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsIPDGetAdmissionTypeServiceListBizActionVO)objBaseMasterDAL.GetAdmisionTypeServiceList(obj, objUserVO);
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

            return valueObject;
        }
    }
}
