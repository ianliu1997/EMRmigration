using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.Reports.IPD
{
    public partial class BedTransferHistory : System.Web.UI.Page
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
                string FirstName = null;
                string MiddleName = null;
                string LastName = null;
                string FamilyName = null;
                string MRNo = null;
                string IPDNO = null;
                long UnitID = 0;


                myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("rptIPDBedtransferHistory.rpt"));

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
                    MiddleName = Convert.ToString(Request.QueryString["LastName"]);
                }
                if (Request.QueryString["FamilyName"] != null)
                {
                    FamilyName = Convert.ToString(Request.QueryString["FamilyName"]);
                }
                if (Request.QueryString["MRNo"] != null)
                {
                    MRNo = Convert.ToString(Request.QueryString["MRNo"]);
                }
                if (Request.QueryString["IPDNO"] != null)
                {
                    IPDNO = Convert.ToString(Request.QueryString["IPDNO"]);
                }
                if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"]!="")
                {
                    FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
                }
                if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"]!="")
                {
                    ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
                }
                if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "")
                {
                    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                }
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@FirstName", FirstName);
                myDoc.SetParameterValue("@MiddleName", MiddleName);
                myDoc.SetParameterValue("@LastName", LastName);
                myDoc.SetParameterValue("@FamilyName", FamilyName);
                myDoc.SetParameterValue("@MRNo", MRNo);
                myDoc.SetParameterValue("@IPDNO", IPDNO);
                myDoc.SetParameterValue("@IdColumnName", "Id");
                myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);

                CrystalReportViewer1.ReportSource = myDoc;

                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                CrystalReportViewer1.DataBind();

            }
            catch (Exception)
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