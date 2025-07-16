using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IPD;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using System.Reflection;

namespace PalashDynamics.BusinessLayer.IPD
{
   
  internal class clsGetUnitWiseEmpBizAction : BizAction
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
        private clsGetUnitWiseEmpBizAction()
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
        private static clsGetUnitWiseEmpBizAction _Instance = null;


        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetUnitWiseEmpBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsGetUnitWiseEmpBizActionVO obj = null;

            try
            {
                obj = (clsGetUnitWiseEmpBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseIPDVitalSDetailsDAL objBaseDAL = clsBaseIPDVitalSDetailsDAL.GetInstance();
                    obj = (clsGetUnitWiseEmpBizActionVO)objBaseDAL.GetUnitWiseEmpDetails(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return obj;
        }

    }

  internal class clsAddVitalSDetailsBizAction : BizAction
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
      private clsAddVitalSDetailsBizAction()
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
      private static clsAddVitalSDetailsBizAction _Instance = null;


      ///Name:GetInstance       
      ///Type:static
      ///Direction:None
      ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
      public static BizAction GetInstance()
      {
          if (_Instance == null)
              _Instance = new clsAddVitalSDetailsBizAction();

          return _Instance;
      }

      protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
      {

          clsAddVitalSDetailsBizActionVO obj = null;

          try
          {
              obj = (clsAddVitalSDetailsBizActionVO)valueObject;
              if (obj != null)
              {
                  clsBaseIPDVitalSDetailsDAL objBaseDAL = clsBaseIPDVitalSDetailsDAL.GetInstance();
                  obj = (clsAddVitalSDetailsBizActionVO)objBaseDAL.AddVitalSDetails(obj, objUserVO);
              }
          }
          catch (HmsApplicationException HEx)
          {
              throw;
          }
          catch (Exception ex)
          {
              logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
              throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
          }
          finally
          {

          }
          return obj;
      }

  }

  internal class clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizAction : BizAction
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
      private clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizAction()
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
      private static clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizAction _Instance = null;


      ///Name:GetInstance       
      ///Type:static
      ///Direction:None
      ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
      public static BizAction GetInstance()
      {
          if (_Instance == null)
              _Instance = new clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizAction();

          return _Instance;
      }

      protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
      {

          clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizActionVO obj = null;

          try
          {
              obj = (clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizActionVO)valueObject;
              if (obj != null)
              {
                  clsBaseIPDVitalSDetailsDAL objBaseDAL = clsBaseIPDVitalSDetailsDAL.GetInstance();
                  obj = (clsGetVitalSDetailsListByAdmIDAdmUnitIDDateBizActionVO)objBaseDAL.GetTPRDetailsListByAdmIDAdmUnitID(obj, objUserVO);
              }
          }
          catch (HmsApplicationException HEx)
          {
              throw;
          }
          catch (Exception ex)
          {
              logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
              throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
          }
          finally
          {

          }
          return obj;
      }

  }

  internal class clsGetVitalSDetailsListBizAction : BizAction
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
      private clsGetVitalSDetailsListBizAction()
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
      private static clsGetVitalSDetailsListBizAction _Instance = null;


      ///Name:GetInstance       
      ///Type:static
      ///Direction:None
      ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
      public static BizAction GetInstance()
      {
          if (_Instance == null)
              _Instance = new clsGetVitalSDetailsListBizAction();

          return _Instance;
      }

      protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
      {

          clsGetVitalSDetailsListBizActionVO obj = null;

          try
          {
              obj = (clsGetVitalSDetailsListBizActionVO)valueObject;
              if (obj != null)
              {
                  clsBaseIPDVitalSDetailsDAL objBaseDAL = clsBaseIPDVitalSDetailsDAL.GetInstance();
                  if (obj.IsListOfVitalDetails.Equals(true))
                  {
                      obj = (clsGetVitalSDetailsListBizActionVO)objBaseDAL.GetListofVitalDetails(obj, objUserVO);
                  }
                  else
                  {
                      obj = (clsGetVitalSDetailsListBizActionVO)objBaseDAL.GetVitalsDetailsList(obj, objUserVO);
                  }
              }
          }
          catch (HmsApplicationException HEx)
          {
              throw;
          }
          catch (Exception ex)
          {
              logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
              throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
          }
          finally
          {

          }
          return obj;
      }

  }

  internal class clsUpdateStatusVitalDetailsBizAction : BizAction
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
      private clsUpdateStatusVitalDetailsBizAction()
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
      private static clsUpdateStatusVitalDetailsBizAction _Instance = null;


      ///Name:GetInstance       
      ///Type:static
      ///Direction:None
      ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
      public static BizAction GetInstance()
      {
          if (_Instance == null)
              _Instance = new clsUpdateStatusVitalDetailsBizAction();

          return _Instance;
      }

      protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
      {

          clsUpdateStatusVitalDetailsBizActionVO obj = null;

          try
          {
              obj = (clsUpdateStatusVitalDetailsBizActionVO)valueObject;
              if (obj != null)
              {
                  clsBaseIPDVitalSDetailsDAL objBaseDAL = clsBaseIPDVitalSDetailsDAL.GetInstance();
                  obj = (clsUpdateStatusVitalDetailsBizActionVO)objBaseDAL.UpdateStatusVitalDetails(obj, objUserVO);
                
              }
          }
          catch (HmsApplicationException HEx)
          {
              throw;
          }
          catch (Exception ex)
          {
              logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
              throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
          }
          finally
          {

          }
          return obj;
      }

  }

  internal class clsGetGraphDetailsBizAction : BizAction
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
      private clsGetGraphDetailsBizAction()
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
      private static clsGetGraphDetailsBizAction _Instance = null;


      ///Name:GetInstance       
      ///Type:static
      ///Direction:None
      ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
      public static BizAction GetInstance()
      {
          if (_Instance == null)
              _Instance = new clsGetGraphDetailsBizAction();

          return _Instance;
      }

      protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
      {

          clsGetGraphDetailsBizActionVO obj = null;

          try
          {
              obj = (clsGetGraphDetailsBizActionVO)valueObject;
              if (obj != null)
              {
                  clsBaseIPDVitalSDetailsDAL objBaseDAL = clsBaseIPDVitalSDetailsDAL.GetInstance();
                  obj = (clsGetGraphDetailsBizActionVO)objBaseDAL.GetGraphDetails(obj, objUserVO);

              }
          }
          catch (HmsApplicationException HEx)
          {
              throw;
          }
          catch (Exception ex)
          {
              logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
              throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
          }
          finally
          {

          }
          return obj;
      }

  }
}
