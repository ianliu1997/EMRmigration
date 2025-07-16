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
    internal class clsGetDoctorAddressInfoBizAction : BizAction
    {
        private static clsGetDoctorAddressInfoBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorAddressInfoBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;


            clsGetDoctorAddressInfoBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDoctorAddressInfoBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseDoctorDAL objBaseMasterDAL = clsBaseDoctorDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetDoctorAddressInfoBizActionVO)objBaseMasterDAL.GetDoctorAddressInfo(obj, objUserVO);
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
