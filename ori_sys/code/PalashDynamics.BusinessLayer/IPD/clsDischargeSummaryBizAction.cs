using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using com.seedhealthcare.hms.CustomExceptions;

namespace PalashDynamics.BusinessLayer.Administration.IPD
{
    internal class clsGetIPDDischargeSummaryBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetIPDDischargeSummaryBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetIPDDischargeSummaryBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetIPDDischargeSummaryBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetIPDDischargeSummaryBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDischargeSummaryDAL objBaseDAL = clsBaseDischargeSummaryDAL.GetInstance();
                    obj = (clsGetIPDDischargeSummaryBizActionVO)objBaseDAL.GetIPDDischargeSummaryList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsAddIPDDischargeSummaryBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsAddIPDDischargeSummaryBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsAddIPDDischargeSummaryBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsAddIPDDischargeSummaryBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsAddIPDDischargeSummaryBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDischargeSummaryDAL objBaseDAL = clsBaseDischargeSummaryDAL.GetInstance();
                    obj = (clsAddIPDDischargeSummaryBizActionVO)objBaseDAL.AddUpdateDischargeSummary(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }

    internal class clsGetPatientsDischargeSummaryInfoInHTMLBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsGetPatientsDischargeSummaryInfoInHTMLBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsGetPatientsDischargeSummaryInfoInHTMLBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO)valueObject;
                if (obj != null)
                {
                    clsBaseDischargeSummaryDAL objBaseDAL = clsBaseDischargeSummaryDAL.GetInstance();
                    obj = (clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO)objBaseDAL.GetPatientsDischargeSummaryInfoInHTML(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            //Added by Bhushanp 10/01/2017 check null value
            if (obj.DischargeSummaryDetails != null)
            {
                obj.DischargeSummaryDetails.PatientInfoHTML = ReadPatientInfo();
                obj.DischargeSummaryDetails.PatientFooterInfoHTML = ReadPatientFooterInfo();
            }
            return obj;
        }

        private String ReadPatientInfo()
        {
            String returnValue = "";
            string f = AppDomain.CurrentDomain.BaseDirectory.ToString();   

            f = f + "PatientInfoForDischargeTemp.htm";
       
            List<string> lines = new List<string>();
            lines = System.IO.File.ReadAllLines(f).ToList();
            foreach (string s1 in lines)
            {
                returnValue = returnValue + s1;
            }
            return returnValue;
        }
        private String ReadPatientFooterInfo()
        {

            String returnValue = "";


            string f = AppDomain.CurrentDomain.BaseDirectory.ToString();
            //"D:\\Pranav\\BSR\\BSR\\PalashDynamics.Web\\"

            f = f + "DischargeSummeryFooter.htm";

            // 1
            // Declare new List.
            List<string> lines = new List<string>();



            lines = System.IO.File.ReadAllLines(f).ToList();


            // 5
            // Print out all the lines.
            foreach (string s1 in lines)
            {
                //Console.WriteLine(s);
                returnValue = returnValue + s1;
            }

            return returnValue;

        }
    }

    internal class clsFillDataGridDischargeSummaryListBizAction : BizAction
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the LogManager object
        LogManager logManager = null;
        //Declare the BaseRoleExpDetailsDAL object
        //Declare the Variable of UserId
        long lngUserId = 0;
        #endregion

        private static clsFillDataGridDischargeSummaryListBizAction _Instance = null;

        /// <summary>
        /// To create singleton instance of the class and  This will Give Unique Instance
        /// </summary>
        /// <returns></returns>
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsFillDataGridDischargeSummaryListBizAction();

            return _Instance;
        }

        protected override ValueObjects.IValueObject ProcessRequest(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO objUserVO)
        {
            clsFillDataGridDischargeSummaryListBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsFillDataGridDischargeSummaryListBizActionVO)valueObject;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseDischargeSummaryDAL objBaseDAL = clsBaseDischargeSummaryDAL.GetInstance();
                    obj = (clsFillDataGridDischargeSummaryListBizActionVO)objBaseDAL.FillDataGridDischargeSummaryList(obj, objUserVO);
                }
            }
            catch (HmsApplicationException HEx)
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return obj;
        }
    }
}
