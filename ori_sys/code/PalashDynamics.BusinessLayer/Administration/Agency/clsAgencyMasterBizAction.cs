using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Administration.Agency;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration.Agency;

namespace PalashDynamics.BusinessLayer.Administration.Agency
{

    internal class clsAddAgencyMasterBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsAddAgencyMasterBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddAgencyMasterBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddAgencyMasterBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddAgencyMasterBizActionVO)valueObject;
                if (obj != null)
                {
                    if (obj.IsAgent)
                    {
                        clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                        obj = (clsAddAgencyMasterBizActionVO)objBaseDAL.AddAgentMaster(obj, objUserVO);
                    }
                    else if (obj.CheckStatus)
                    {
                        clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                        obj = (clsAddAgencyMasterBizActionVO)objBaseDAL.UpdateStatusAgent(obj, objUserVO);
                    }
                    else
                    {
                        clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                        obj = (clsAddAgencyMasterBizActionVO)objBaseDAL.AddAgencyMaster(obj, objUserVO);
                    }
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }

    internal class clsGetAgencyMasterListBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        
        private static clsGetAgencyMasterListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAgencyMasterListBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetAgencyMasterListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetAgencyMasterListBizActionVO)valueObject;
                if (obj != null)
                {
                    if (obj.IsAgent)
                    {
                        clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                        obj = (clsGetAgencyMasterListBizActionVO)objBaseDAL.GetAgentMasterList(obj, objUserVO);
                    }
                    else if (obj.GetAgentByID)
                    {
                        clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                        obj = (clsGetAgencyMasterListBizActionVO)objBaseDAL.GetAgentDetilsByID(obj, objUserVO);
                    }
                    else
                    {
                        clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                        obj = (clsGetAgencyMasterListBizActionVO)objBaseDAL.GetAgencyMasterList(obj, objUserVO);
                    }
                }
            }
            catch (HmsApplicationException HEx)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw;
            }
            finally
            {

            }
            return obj;
        }
    }
  
}
