using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.DataAccessLayer;

namespace PalashDynamics.BusinessLayer.Master
{
    internal class clsCheckDuplicasyBizAction : BizAction
    {
         #region Variable Declaration
        //Declare the LogManager object
        LogManager logmanager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        public clsCheckDuplicasyBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logmanager == null)
            {
                logmanager = LogManager.GetInstance();
            }
            #endregion
        }

        private static clsCheckDuplicasyBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new clsCheckDuplicasyBizAction();
            }

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsCheckDuplicasyBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsCheckDuplicasyBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                    obj = (clsCheckDuplicasyBizActionVO)objBaseDAL.CheckDuplicasy(obj, objUserVO);
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
