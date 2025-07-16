using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer;
namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsGetDoctorBankInfoBizAction : BizAction
    {
        private static clsGetDoctorBankInfoBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorBankInfoBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetDoctorBankInfoBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDoctorBankInfoBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseDoctorDAL objBaseMasterDAL = clsBaseDoctorDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetDoctorBankInfoBizActionVO)objBaseMasterDAL.GetDoctorBankInfo(obj, objUserVO);
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
