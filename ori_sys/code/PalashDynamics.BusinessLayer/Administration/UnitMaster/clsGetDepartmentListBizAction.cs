using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;

namespace PalashDynamics.BusinessLayer.Administration.UnitMaster
{
   internal class clsGetDepartmentListBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetDepartmentListBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDepartmentListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
             clsGetDepartmentListBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetDepartmentListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseUnitMasterDAL objBaseDAL = clsBaseUnitMasterDAL.GetInstance();
                    obj = (clsGetDepartmentListBizActionVO)objBaseDAL.GetDepartmentList(obj, objUserVO);


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
