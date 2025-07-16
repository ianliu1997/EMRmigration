using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using PalashDynamics.ValueObjects.DashBoardVO;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using com.seedhealthcare.hms.Web.Logging;

namespace PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction
{

    class clsGetDetailsOfReceivedOocyteBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetDetailsOfReceivedOocyteBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsGetDetailsOfReceivedOocyteBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDetailsOfReceivedOocyteBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDetailsOfReceivedOocyteBizActionVO obj = null;

            try
            {
                obj = (clsGetDetailsOfReceivedOocyteBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                    obj = (clsGetDetailsOfReceivedOocyteBizActionVO)objBaseDAL.GetDetailsOfReceivedOocyte(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

    }

    //added by neena for getting donated oocyte and embryo
    class clsGetDetailsOfReceivedOocyteEmbryoBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion
        private clsGetDetailsOfReceivedOocyteEmbryoBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static clsGetDetailsOfReceivedOocyteEmbryoBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetDetailsOfReceivedOocyteEmbryoBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDetailsOfReceivedOocyteEmbryoBizActionVO obj = null;

            try
            {
                obj = (clsGetDetailsOfReceivedOocyteEmbryoBizActionVO)valueObject;
                if (obj != null)
                {
                    if (obj.Details.IsDonorCycle)
                    {
                        clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                        obj = (clsGetDetailsOfReceivedOocyteEmbryoBizActionVO)objBaseDAL.GetDetailsOfReceivedOocyteEmbryoFromDonorCycle(obj, objUserVO);
                    }
                    else
                    {
                        clsBaseDonorDetailsDAL objBaseDAL = clsBaseDonorDetailsDAL.GetInstance();
                        obj = (clsGetDetailsOfReceivedOocyteEmbryoBizActionVO)objBaseDAL.GetDetailsOfReceivedOocyteEmbryo(obj, objUserVO);
                    }
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //  logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return obj;
        }

    }
    //
}
