using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects.Administration.StaffMaster;

namespace PalashDynamics.BusinessLayer.Administration.StaffMaster
{
   internal class clsAddStaffMasterBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddStaffMasterBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddStaffMasterBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {


            clsAddStaffMasterBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsAddStaffMasterBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseStaffMasterDAL objBaseDAL = clsBaseStaffMasterDAL.GetInstance();
                    obj = (clsAddStaffMasterBizActionVO)objBaseDAL.AddStaffMaster(obj, objUserVO);


                }

            }
            catch (HmsApplicationException HEx)
            {
                
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
