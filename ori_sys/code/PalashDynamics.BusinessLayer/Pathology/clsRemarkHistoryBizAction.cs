using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Pathology;

namespace PalashDynamics.BusinessLayer.Pathology
{
       
    public class clsRemarkHistoryBizAction : BizAction
    {
        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsRemarkHistoryBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsRemarkHistoryBizActionVO)valueObject;              
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsRemarkHistoryBizActionVO)objBaseDAL.AddHistory(obj, objUserVO);
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

                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {               
            }
            return obj;
        }

        #region Variables Declaration

        LogManager logManager = null;

        long lngUserId = 0;
        #endregion

        //constructor For Log Error Info
        private clsRemarkHistoryBizAction()
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
        private static clsRemarkHistoryBizAction _Instance = null;

        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsRemarkHistoryBizAction();

            return _Instance;
        }
    }
    }
   