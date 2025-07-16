using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Master;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;



namespace PalashDynamics.BusinessLayer.Master
{
    //class clsGetBdMasterListBizAction
    //{
        internal class clsGetBdMasterListBizAction : BizAction
        {
            //This Region Contains Variables Which are Used At Form Level
            #region Variables Declaration
            //Declare the LogManager object
            LogManager logManager = null;
            //Declare the BaseRoleMasterDAL object
            //Declare the Variable of UserId
            long lngUserId = 0;
            #endregion

            //constructor For Log Error Info
            public clsGetBdMasterListBizAction()
            {
                //Create Instance of the LogManager object 
                #region Logging Code
                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }

                #endregion

            }

            private static clsGetBdMasterListBizAction _Instance = null;
            /// <summary>
            /// To create singleton instance of the class and  This will Give Unique Instance
            /// </summary>
            /// <returns></returns>
            public static BizAction GetInstance()
            {
                if (_Instance == null)
                    _Instance = new clsGetBdMasterListBizAction();

                return _Instance;
            }
            ///Method Input Roles: valueObject
            ///Name                   :ProcessRequest    
            ///Type                   :IValueObject
            ///Direction              :input-IvalueObject output-IvalueObject
            ///Method Purpose         :Now Override the ProcessRequest Method of the BusinessAction class 

            protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
            {
                bool CurrentMethodExecutionStatus = true;

                clsGetBdMasterBizActionVO obj = null;
                int ResultStatus = 0;
                try
                {
                    obj = (clsGetBdMasterBizActionVO)valueObject;
                    //Typecast the "valueObject" to "clsInputOutputVO"
                    if (obj != null)
                    {
                        clsBaseMasterDAL objBaseDAL = clsBaseMasterDAL.GetInstance();
                        obj = (clsGetBdMasterBizActionVO)objBaseDAL.GetBdMasterList(obj, objUserVO);

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
                finally
                {
                }
                return obj;

            }

        }
   // }
}
