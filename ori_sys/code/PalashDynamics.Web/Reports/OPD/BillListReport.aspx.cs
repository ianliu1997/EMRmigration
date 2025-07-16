using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.OPD
{
    public partial class BillListReport : System.Web.UI.Page
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
                long UnitID = 0;
                string MRNO = null;
                string BillNO = null;
                string OPDNO = null;
                string FirstName = null;
                string MiddleName = null;
                string LastName = null;
                long BillStatus = 0;
                long BillType = 0;
                long CostingDivisionID = 0;
                bool Excel = false;

                myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("rptBillListReport.rpt"));



                if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
                {
                    FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
                }
                if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
                {
                    ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
                }

                if (Request.QueryString["UnitID"] != null)
                {
                    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                }

                if (Request.QueryString["MRNO"] != null)
                {
                    MRNO = Convert.ToString(Request.QueryString["MRNO"]);
                }

                if (Request.QueryString["BillNO"] != null)
                {
                    BillNO = Convert.ToString(Request.QueryString["BillNO"]);
                }

                if (Request.QueryString["OPDNO"] != null)
                {
                    OPDNO = Convert.ToString(Request.QueryString["OPDNO"]);
                }

                if (Request.QueryString["FirstName"] != null)
                {
                    FirstName = Convert.ToString(Request.QueryString["FirstName"]);
                }

                if (Request.QueryString["MiddleName"] != null)
                {
                    MiddleName = Convert.ToString(Request.QueryString["MiddleName"]);
                }

                if (Request.QueryString["LastName"] != null)
                {
                    LastName = Convert.ToString(Request.QueryString["LastName"]);
                }

                if (Request.QueryString["BillStatus"] != null)
                {
                    BillStatus = Convert.ToInt64(Request.QueryString["BillStatus"]);
                }
                if (Request.QueryString["BillType"] != null)
                {
                    BillType = Convert.ToInt64(Request.QueryString["BillType"]);
                }
                if (Request.QueryString["CostingDivisionID"] != null)
                {
                    CostingDivisionID = Convert.ToInt64(Request.QueryString["CostingDivisionID"]);
                }

                if (Request.QueryString["Excel"] != null && Request.QueryString["Excel"] != "")
                {
                    Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
                }

                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@UnitID", UnitID);
                myDoc.SetParameterValue("@MRNO", MRNO);
                myDoc.SetParameterValue("@BillNO", BillNO);
                myDoc.SetParameterValue("@OPDNO", OPDNO);
                myDoc.SetParameterValue("@FirstName", FirstName);
                myDoc.SetParameterValue("@MiddleName", MiddleName);
                myDoc.SetParameterValue("@LastName", LastName);
                myDoc.SetParameterValue("@BillSettleType", BillStatus);
                myDoc.SetParameterValue("@BillType", BillType);
                myDoc.SetParameterValue("@CostingDivisionID", CostingDivisionID);



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