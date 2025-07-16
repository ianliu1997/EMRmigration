using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Administration.Agency;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration.Agency;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.Administration.Agency
{
    internal class clsGetServiceListBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsGetServiceListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServiceListBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetServiceListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetServiceListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                    if (obj.IsAgencyServiceLinkView == true)
                    {
                        obj = (clsGetServiceListBizActionVO)objBaseDAL.GetSelectedServiceList(obj, objUserVO);
                    }
                    else
                    {
                        obj = (clsGetServiceListBizActionVO)objBaseDAL.GetServiceList(obj, objUserVO);
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

    internal class clsAddServiceAgencyLinkBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsAddServiceAgencyLinkBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddServiceAgencyLinkBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddServiceAgencyLinkBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddServiceAgencyLinkBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                    obj = (clsAddServiceAgencyLinkBizActionVO)objBaseDAL.AddServiceAgencyLink(obj, objUserVO);
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

    internal class clsGetServiceToAgencyBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsGetServiceToAgencyBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServiceToAgencyBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetServiceToAgencyBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetServiceToAgencyBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                    //if (obj.IsAgencyServiceLinkView == true)
                    //{
                        obj = (clsGetServiceToAgencyBizActionVO)objBaseDAL.GetServiceToAgencyAssigned(obj, objUserVO);
                    //}
                    //else
                    //{
                    //    obj = (clsGetServiceToAgencyBizActionVO)objBaseDAL.GetServiceList(obj, objUserVO);
                    //}
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
