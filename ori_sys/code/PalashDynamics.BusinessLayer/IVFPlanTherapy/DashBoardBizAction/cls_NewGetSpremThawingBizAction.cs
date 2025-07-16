using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.DashBoardVO;

using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;

namespace PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction
{
    class cls_NewGetSpremThawingBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private cls_NewGetSpremThawingBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static cls_NewGetSpremThawingBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_NewGetSpremThawingBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_NewGetSpremThawingBizActionVO obj = null;

            try
            {
                obj = (cls_NewGetSpremThawingBizActionVO)valueObject;
                if (obj != null)
                {
                    cls_NewBaseSpremThawingDAL objBaseDAL = cls_NewBaseSpremThawingDAL.GetInstance();
                    if (obj.IsFromIUI == true)
                    {
                        obj = (cls_NewGetSpremThawingBizActionVO)objBaseDAL.GetThawingDetailsListForIUI(obj, objUserVO);
                    }
                    else if (obj.IsThawDetails == true)
                    {
                        obj = (cls_NewGetSpremThawingBizActionVO)objBaseDAL.GetThawingDetailsList(obj, objUserVO);
                    }
                    else
                    {
                        obj = (cls_NewGetSpremThawingBizActionVO)objBaseDAL.GetSpremFreezingforThawingNew(obj, objUserVO);
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
     class cls_GetSpremFreezingDetilsForThawingBizAction : BizAction
    {
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private cls_GetSpremFreezingDetilsForThawingBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }
        private static cls_GetSpremFreezingDetilsForThawingBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_GetSpremFreezingDetilsForThawingBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO obj = null;

            try
            {
                obj = (cls_GetSpremFreezingDetilsForThawingBizActionVO)valueObject;
                if (obj != null)
                {
                    cls_NewBaseSpremThawingDAL objBaseDAL = cls_NewBaseSpremThawingDAL.GetInstance();
                    if (obj.IsForTemplate == true)
                    {
                        obj = (cls_GetSpremFreezingDetilsForThawingBizActionVO)objBaseDAL.GetTesaPesaForCode(obj, objUserVO);
                    }
                    else if (obj.IsView == true)
                    {
                        obj = (cls_GetSpremFreezingDetilsForThawingBizActionVO)objBaseDAL.GetSpermFrezingDetailsForThawingView(obj, objUserVO);
                    }
                    else
                    {
                        obj = (cls_GetSpremFreezingDetilsForThawingBizActionVO)objBaseDAL.GetSpermFrezingDetailsForThawing(obj, objUserVO);
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
    
}
