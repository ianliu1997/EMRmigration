using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;

namespace PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction
{
    class cls_IVFDashboard_GetVitrificationDetailsForCryoBankBizAction: BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private cls_IVFDashboard_GetVitrificationDetailsForCryoBankBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static cls_IVFDashboard_GetVitrificationDetailsForCryoBankBizAction _Instance = null;

        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_IVFDashboard_GetVitrificationDetailsForCryoBankBizAction();
            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_IVFDashboard_GetVitrificationDetailsForCryoBank obj = null;
            try
            {
                obj = (cls_IVFDashboard_GetVitrificationDetailsForCryoBank)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                    obj = (cls_IVFDashboard_GetVitrificationDetailsForCryoBank)objBaseDAL.GetVitrificationForCryoBank(obj, objUserVO);
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

    //Added By CDS For Oocyte Bank
    class cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBankBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBankBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBankBizAction _Instance = null;

        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBankBizAction();
            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank obj = null;
            try
            {
                obj = (cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank)valueObject;
                if (obj != null)
                {
                    clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                    obj = (cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank)objBaseDAL.GetOocyteVitrificationForCryoBank(obj, objUserVO);
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

    //added by neena for oocyte donate discard option
    class cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBankBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseOPDPatientMasterDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBankBizAction()
        {
            //Create Instance of the LogManager object 
            #region Logging Code
            if (logManager == null)
            {
                logManager = LogManager.GetInstance();
            }
            #endregion
        }

        //The Private declaration
        private static cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBankBizAction _Instance = null;

        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBankBizAction();
            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank obj = null;
            try
            {
                obj = (cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank)valueObject;
                if (obj != null)
                {
                    if (obj.IsOocyte)
                    {
                        clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                        obj = (cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank)objBaseDAL.AddUpdateDonateDiscardOocyteForCryoBank(obj, objUserVO);
                    }
                    else if (obj.IsEmb)
                    {
                        clsBaseIVFDashboard_VitrificationDAL objBaseDAL = clsBaseIVFDashboard_VitrificationDAL.GetInstance();
                        obj = (cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank)objBaseDAL.AddUpdateDonateDiscardEmbryoForCryoBank(obj, objUserVO);
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
