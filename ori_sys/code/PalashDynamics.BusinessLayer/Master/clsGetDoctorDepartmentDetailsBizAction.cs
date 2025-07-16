using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer;


namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsGetDoctorDepartmentDetailsBizAction : BizAction
    {

        private static clsGetDoctorDepartmentDetailsBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorDepartmentDetailsBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetDoctorDepartmentDetailsBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDoctorDepartmentDetailsBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO

                    if(obj.IsNonReferralDoctor==true)
                        obj = (clsGetDoctorDepartmentDetailsBizActionVO)objBaseMasterDAL.GetOtherThanReferralDoctorDepartmentDetails(obj, objUserVO);
                    else
                      obj = (clsGetDoctorDepartmentDetailsBizActionVO)objBaseMasterDAL.GetDoctorDepartmentDetails(obj, objUserVO);
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
    //added by rohini dated 28/12//2015 for all doctors from doctor master
    internal class clsGetDoctorListForComboBizAction : BizAction
    {

        private static clsGetDoctorListForComboBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDoctorListForComboBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetDoctorListForComboBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDoctorListForComboBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO


                    obj = (clsGetDoctorListForComboBizActionVO)objBaseMasterDAL.GetAllDoctorList(obj, objUserVO);
                  
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
    internal class clsGetRadiologistBizAction : BizAction
    {

        private static clsGetRadiologistBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetRadiologistBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetRadiologistBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetRadiologistBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetRadiologistBizActionVO)objBaseMasterDAL.GetRadiologist(obj, objUserVO);
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
    

  
    internal class clsGetPathologistBizAction : BizAction
    {

        private static clsGetPathologistBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPathologistBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPathologistBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPathologistBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetPathologistBizActionVO)objBaseMasterDAL.GetPathologist(obj, objUserVO);
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

    internal class clsGetAnesthetistBizAction : BizAction
    {

        private static clsGetAnesthetistBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAnesthetistBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetAnesthetistBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetAnesthetistBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetAnesthetistBizActionVO)objBaseMasterDAL.GetAnesthetist(obj, objUserVO);
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

    internal class clsGetEmbryologistBizAction : BizAction
    {

        private static clsGetEmbryologistBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetEmbryologistBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetEmbryologistBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetEmbryologistBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetEmbryologistBizActionVO)objBaseMasterDAL.GetEmbryologist(obj, objUserVO);
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

    internal class clsGetPathoUsersBizAction : BizAction  //by rohini
    {

        private static clsGetPathoUsersBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPathoUsersBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPathoUsersBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPathoUsersBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetPathoUsersBizActionVO)objBaseMasterDAL.GetPathoUsers(obj, objUserVO);
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

    // BY BHUSHAN . . . . . . .
    internal class clsGetPathoParameterUnitBizAction : BizAction
    {
        private static clsGetPathoParameterUnitBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPathoParameterUnitBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPathoParameterUnitBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPathoParameterUnitBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsGetPathoParameterUnitBizActionVO)objBaseMasterDAL.GetPathoParameter(obj, objUserVO);
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

    #region For IPD Module

    internal class clsFillDepartmentBizAction : BizAction
    {

        private static clsFillDepartmentBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsFillDepartmentBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsFillDepartmentBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsFillDepartmentBizActionVO)valueObject;
                if (obj != null)
                {
                    //Create the Instance of clsBasePatientDAL
                    clsBaseMasterDAL objBaseMasterDAL = clsBaseMasterDAL.GetInstance();
                    //Now Call the Insert method of the Patient DAO
                    obj = (clsFillDepartmentBizActionVO)objBaseMasterDAL.DeptFromSubSpecilization(obj, objUserVO);
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

    #endregion

}
