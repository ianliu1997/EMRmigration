using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using com.seedhealthcare.hms.Web.Logging;


namespace PalashDynamics.BusinessLayer.Administration.PatientSourceMaster
{
    class clsGetPatientSourceListByPatientCategoryIdBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion


        private static clsGetPatientSourceListByPatientCategoryIdBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientSourceListByPatientCategoryIdBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetPatientSourceListByPatientCategoryIdBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetPatientSourceListByPatientCategoryIdBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePatientSourceMasterMasterDAL objBaseDAL = clsBasePatientSourceMasterMasterDAL.GetInstance();
                    obj = (clsGetPatientSourceListByPatientCategoryIdBizActionVO)objBaseDAL.GetPatientSourceListByPatientCategoryId(obj, objUserVO);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }
}
