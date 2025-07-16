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

    internal class clsAddAgencyClinicLinkBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsAddAgencyClinicLinkBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddAgencyClinicLinkBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsAddAgencyClinicLinkBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddAgencyClinicLinkBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                    obj = (clsAddAgencyClinicLinkBizActionVO)objBaseDAL.AddAgencyCliniLink(obj, objUserVO);
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

    internal class clsGetAgencyClinicLinkBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsGetAgencyClinicLinkBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetAgencyClinicLinkBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetAgencyClinicLinkBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetAgencyClinicLinkBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                    obj = (clsGetAgencyClinicLinkBizActionVO)objBaseDAL.GetAgencyclinicLinkList(obj, objUserVO);
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

    internal class clsGetClinicMasterListBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsGetClinicMasterListBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetClinicMasterListBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetClinicMasterListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetClinicMasterListBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                    obj = (clsGetClinicMasterListBizActionVO)objBaseDAL.GetClinicMasterList(obj, objUserVO);
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

    internal class clsGetServiceToAgencyAssignedBizAction : BizAction
    {

        #region Variables Declaration
        LogManager logManager = null;
        long lngUserId = 0;
        #endregion
        private static clsGetServiceToAgencyAssignedBizAction _Instance = null;
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetServiceToAgencyAssignedBizAction();
            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetServiceToAgencyAssignedBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetServiceToAgencyAssignedBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseAgencyMasterDAL objBaseDAL = clsBaseAgencyMasterDAL.GetInstance();
                    //if (obj.IsAgencyServiceLinkView == true)
                    //{
                    obj = (clsGetServiceToAgencyAssignedBizActionVO)objBaseDAL.GetServiceToAgencyAssignedCheck(obj, objUserVO);
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
