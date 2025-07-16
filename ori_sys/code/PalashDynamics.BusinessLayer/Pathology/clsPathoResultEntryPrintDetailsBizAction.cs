using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.CustomExceptions;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using System.Reflection;
using PalashDynamics.DataAccessLayer;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.Pathology;

namespace PalashDynamics.BusinessLayer.Pathology
{
    class clsPathoResultEntryPrintDetailsBizAction : BizAction
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
        private clsPathoResultEntryPrintDetailsBizAction()
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
        private static clsPathoResultEntryPrintDetailsBizAction _Instance = null;

        ///Method Input OPDPatient: none
        ///Name:GetInstance       
        ///Type:static
        ///Direction:None
        ///Method Purpose: To create singleton instance of the class and  This will Give Unique Instance
        public static BizAction GetInstance()
        {
            if (_Instance == null)
                _Instance = new clsPathoResultEntryPrintDetailsBizAction();

            return _Instance;
        }

        protected override IValueObject ProcessRequest(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsPathoResultEntryPrintDetailsBizActionVO obj = null;
            int ResultStatus = 0;
            try
            {
                obj = (clsPathoResultEntryPrintDetailsBizActionVO)valueObject;
                //obj.OPDPatientDetails.AddedBy = objUserVO.ID;
                //obj.OPDPatientDetails.UpdatedBy = objUserVO.ID;
                //Typecast the "valueObject" to "clsInputOutputVO"
                if (obj != null)
                {
                    clsBaseOrderBookingDAL objBaseDAL = clsBaseOrderBookingDAL.GetInstance();
                    obj = (clsPathoResultEntryPrintDetailsBizActionVO)objBaseDAL.GetPathoResultEntryPrintDetails(obj, objUserVO);
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
                //log error  
                logManager.LogError(objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                // logManager.LogError(0, objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(obj.GetBizActionGuid(), objUserVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            /*
             * Comment on 12.05.2016 for the purpose of testing
             */ 
            //obj.ResultDetails.PatientInfoHTML = ReadPatientInfo();
            //obj.ResultDetails.DoctorInfoHTML = ReadDoctorInfo();

            obj.ResultDetails.PatientInfoHTML = ReadPatientInfo();
            obj.ResultDetails.DoctorInfoHTML = ReadDoctorInfo();

            return obj;
        }


        private String ReadPatientInfo()
        {
            String returnValue = "";
            try
            {
                


                string f = AppDomain.CurrentDomain.BaseDirectory.ToString();
                //"D:\\Pranav\\BSR\\BSR\\PalashDynamics.Web\\"

                //f = f + "PatientInfoPathoForC1.htm";

                f = f + "PatientPathoTemp.htm";

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
            }
            catch (Exception ex)
            {
                return returnValue;
            }
            return returnValue;

        }


        private String ReadDoctorInfo()
        {

            String returnValue = "";


            string f = AppDomain.CurrentDomain.BaseDirectory.ToString();
            //"D:\\Pranav\\BSR\\BSR\\PalashDynamics.Web\\"

            f = f + "PatientPathoFooterTemp.htm";

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
}
