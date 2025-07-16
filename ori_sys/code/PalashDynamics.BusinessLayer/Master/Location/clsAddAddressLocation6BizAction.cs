using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;
using PalashDynamics.ValueObjects.Master.Location;

namespace PalashDynamics.BusinessLayer.Master.Location
{
    internal class clsAddAddressLocation6BizAction : BizAction
    {
        #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;

        #endregion

        //constructor For Log Error Info
        public clsAddAddressLocation6BizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsAddAddressLocation6BizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsAddAddressLocation6BizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddAddressLocation6BizActionVO obj = null;

            int ResultStatus = 0;
            
            try
            {
                obj = (clsAddAddressLocation6BizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseLocationDAL objBaseLocationDAL = clsBaseLocationDAL.GetInstance();
                    obj = (clsAddAddressLocation6BizActionVO)objBaseLocationDAL.AddAddressLocation6BizActionInfo(obj, objUserVO);
                } 
            }
            catch (Exception ex)
            {
                
            }
            finally
            {

            }
            return obj;
        }

    }

}
