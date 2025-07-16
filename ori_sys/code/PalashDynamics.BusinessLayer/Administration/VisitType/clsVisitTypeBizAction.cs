using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.VisitType;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;

namespace PalashDynamics.BusinessLayer.Administration.VisitType
{


    internal class clsCheckVisitTypeMappedWithPackageServiceBizAction : BizAction
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsCheckVisitTypeMappedWithPackageServiceBizAction _Instance = null;
        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsCheckVisitTypeMappedWithPackageServiceBizAction();

            return _Instance;
        }


        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

         
            clsCheckVisitTypeMappedWithPackageServiceBizActionVO obj = null;
          
            try
            {
                obj = (clsCheckVisitTypeMappedWithPackageServiceBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseVisitTypeDAL objBaseDAL = clsBaseVisitTypeDAL.GetInstance();
                    obj = (clsCheckVisitTypeMappedWithPackageServiceBizActionVO)objBaseDAL.CheckVisitTypeMappedWithPackageService(obj, objUserVO);


                }

            }
            catch (HmsApplicationException HEx)
            {
               // CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
               // CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }


}
