using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using System.Reflection;


namespace PalashDynamics.BusinessLayer
{
  internal class clsUpdatePasswordConfigBizAction:BizAction
   {

       #region Variable Declaration
       //Declare the LogManager object
       LogManager logmanager = null;
       //Declare the BaseRoleMasterDAL object
       //Declare the Variable of UserId
       long lngUserId = 0;
        #endregion
      
      //constructor For Log Error Info

       public clsUpdatePasswordConfigBizAction()
       {
           //Create Instance of the LogManager object 
           #region Logging Code
           if (logmanager == null)
           {
               logmanager = LogManager.GetInstance();
           }
           #endregion
       }

       private static clsUpdatePasswordConfigBizAction _Instance = null;
       /// <summary>
       /// To create singleton instance of the class and  This will Give Unique Instance
       /// </summary>
       /// <returns></returns>

       public static BizAction GetInstance()
       { 
             if (_Instance==null)
              {   
                _Instance = new clsUpdatePasswordConfigBizAction();
              }
   
           return _Instance;

       }


       protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsAddPasswordConfigBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddPasswordConfigBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBasePassConfigDAL objBaseDAL = clsBasePassConfigDAL.GetInstance();
                    obj = (clsAddPasswordConfigBizActionVO)objBaseDAL.UpdatePasswordConfig(obj, objUserVO);

                }
            }
            catch (HmsApplicationException Hex)
            {
                CurrentMethodExecutionStatus = false;
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //log error  
                logmanager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            { 
            
            }
            return obj;

        }
    }
}
