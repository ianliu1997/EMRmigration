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
namespace PalashDynamics.BusinessLayer.Administration
{
  internal  class clsGetServiceBySpecializationBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetServiceBySpecializationBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>



        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServiceBySpecializationBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServiceBySpecializationBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetServiceBySpecializationBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseServiceMasterDAL objBaseDAL = clsBaseServiceMasterDAL.GetInstance();

                    obj = (clsGetServiceBySpecializationBizActionVO)objBaseDAL.GetServiceBySpecialization(obj, objUserVO);

                 



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
