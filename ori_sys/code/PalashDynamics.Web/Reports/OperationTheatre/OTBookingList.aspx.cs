using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.OperationTheatre
{
    public partial class OTBookingList : System.Web.UI.Page
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




            DateTime? OTBookingDate = null;
            DateTime? OTToDate = null;
            long UnitID = 0;
            string MRNO = null;
            long OTID = 0;
            long OTTableID = 0;          
            string FirstName = null;           
            string LastName = null;
            bool IsEmergency = false;
            bool IsCancelled = false;           
            bool Excel = false;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptOTBookingList.rpt"));

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                OTBookingDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                OTToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }

            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }

            if (Request.QueryString["MRNO"] != null)
            {
                MRNO = Convert.ToString(Request.QueryString["MRNO"]);
            }

            if (Request.QueryString["OTID"] != null)
            {
                OTID = Convert.ToInt64(Request.QueryString["OTID"]);
            }

            if (Request.QueryString["OTTableID"] != null)
            {
                OTTableID = Convert.ToInt64(Request.QueryString["OTTableID"]);
            }

            if (Request.QueryString["FirstName"] != null)
            {
                FirstName = Convert.ToString(Request.QueryString["FirstName"]);
            }           

            if (Request.QueryString["LastName"] != null)
            {
                LastName = Convert.ToString(Request.QueryString["LastName"]);
            }

            if (Request.QueryString["IsEmergency"] != null)
            {
                IsEmergency = Convert.ToBoolean(Request.QueryString["IsEmergency"]);
            }
            if (Request.QueryString["IsCancelled"] != null)
            {
                IsCancelled = Convert.ToBoolean(Request.QueryString["IsCancelled"]);
            }            

            if (Request.QueryString["Excel"] != null && Request.QueryString["Excel"] != "")
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }


            myDoc.SetParameterValue("@OTBookingDate", OTBookingDate);
            myDoc.SetParameterValue("@OTToDate", OTToDate);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@MRNO", MRNO);
            myDoc.SetParameterValue("@OTID", OTID);
            myDoc.SetParameterValue("@OTTableID", OTTableID);
            myDoc.SetParameterValue("@FirstName", FirstName);           
            myDoc.SetParameterValue("@LastName", LastName);
            myDoc.SetParameterValue("@IsEmergency", IsEmergency);
            myDoc.SetParameterValue("@IsCancelled", IsCancelled);


            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;

            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            CrystalReportViewer1.DataBind();

            if (Excel)
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
                throw;
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