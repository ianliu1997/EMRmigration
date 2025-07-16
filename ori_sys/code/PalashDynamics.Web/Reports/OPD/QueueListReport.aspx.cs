using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace PalashDynamics.Web.Reports.OPD
{
    public partial class QueueListReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
                String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
                String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
                String dbName = ConfigurationManager.AppSettings["DBName"].ToString();


                DateTime? FromDate = null;
                DateTime? ToDate = null;
                bool Excel = false;
                long clinicID = 0;
                long DeptID = 0;
                long DocID = 0;
                long CurrentVisit = 0;
                long SpRegistration = 0;
                string MRNO = null;
                
                //string TokenNo = null;

                //Begin:: Added by Aniket on 15/10/2018 to pass parameter to report for filtering. 
                string FirstName = null;
                string LastName = null;
                //End:: Added by Aniket on 15/10/2018 to pass parameter to report for filtering.


                //Begin:: Added by Aniket on 15/10/2018 to pass parameter to report for filtering.
                string ContactNo = null;
                string TokenNo = null;
                //End:: Added by Aniket on 15/10/2018 to pass parameter to report for filtering.

                myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("rptQueueListReport.rpt"));
               

                if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
                {
                    FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
                }
                if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
                {
                    ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
                }

                if (Request.QueryString["ClinicID"] != null)
                {
                    clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
                }

                if (Request.QueryString["DepartmentID"] != null)
                {
                    DeptID = Convert.ToInt64(Request.QueryString["DepartmentID"]);
                }

                if (Request.QueryString["DoctorID"] != null)
                {
                    DocID = Convert.ToInt64(Request.QueryString["DoctorID"]);
                }

                if (Request.QueryString["VisitStatus"] != null)
                {
                    CurrentVisit = Convert.ToInt64(Request.QueryString["VisitStatus"]);
                }

                if (Request.QueryString["SpecialRegistrationId"] != null)
                {
                   SpRegistration = Convert.ToInt64(Request.QueryString["SpecialRegistrationId"]);
                }

                if (Request.QueryString["MRNO"] != null)
                {
                    MRNO = Convert.ToString(Request.QueryString["MRNO"]);
                }

                if (Request.QueryString["FirstName"] != null)
                {
                    FirstName = Convert.ToString(Request.QueryString["FirstName"]);
                }

                if (Request.QueryString["LastName"] != null)
                {
                    LastName = Convert.ToString(Request.QueryString["LastName"]);
                }

                if (Request.QueryString["ContactNo"] != null)
                {
                    ContactNo = Convert.ToString(Request.QueryString["ContactNo"]);
                }

                if (Request.QueryString["TokenNo"] != null)
                {
                    TokenNo = Convert.ToString(Request.QueryString["TokenNo"]);
                }

                


                if (Request.QueryString["Excel"] != null)
                {
                    Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
                }
               
                
 
              
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@UnitID", clinicID);
                myDoc.SetParameterValue("@DepartmentID", DeptID);
                myDoc.SetParameterValue("@DoctorID", DocID);
                myDoc.SetParameterValue("@CurrentVisitStatus", CurrentVisit);
                myDoc.SetParameterValue("@SpecialRegID", SpRegistration);
                myDoc.SetParameterValue("@MRNO", MRNO);
                myDoc.SetParameterValue("@FirstName", FirstName);
                myDoc.SetParameterValue("@LastName", LastName);
                myDoc.SetParameterValue("@ContactNo", ContactNo);
                myDoc.SetParameterValue("@TokanNo", TokenNo);

                myDoc.SetParameterValue("@UnitID", clinicID, "rptCommonReportHeader.rpt"); //Adedd by AJ Date 31/5/2017

                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                
                CrystalReportViewer1.ReportSource = myDoc;
                
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                CrystalReportViewer1.DataBind();

                if (Excel == true)
                {
                    System.IO.Stream oStream = null;
                    byte[] byteArray = null;
                    oStream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                    byteArray = new byte[oStream.Length];
                    oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(byteArray);
                    Response.End();
                }

              
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
        {
            try
            {
                if (myDoc != null)
                    myDoc.Dispose();
                CrystalReportViewer1.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }    
    }
}