using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;

using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
namespace PalashDynamics.BusinessLayer.Inventory
{
    internal class clsGetStoreStatusBizAction:BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetStoreStatusBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>



        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetStoreStatusBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetStoreStatusBizActionVO obj = null;
            int ResultStatus = 0;

            try
            {
                obj = (clsGetStoreStatusBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseItemMasterDAL objBaseDAL = clsBaseItemMasterDAL.GetInstance();

                    obj = (clsGetStoreStatusBizActionVO)objBaseDAL.GetStoreStatus(obj, objUserVO);





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
