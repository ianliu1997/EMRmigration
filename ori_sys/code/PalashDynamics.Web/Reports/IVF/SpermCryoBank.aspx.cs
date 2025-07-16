using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.IVF
{
    public partial class SpermCryoBank : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strPath, strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            String strReportName = "User List Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";

            long CoupleUnitID = 0;
            long UnitID = 0;
            string MRNo = "";
            string FName = "";
            string MName = "";
            string Lname = "";
            string FamilyName = "";
            
            string AppPath = Request.ApplicationPath;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptSpermCryoBank.rpt"));         
          
            if (Request.QueryString["CoupleUnitID"] != null)
            {
                CoupleUnitID = Convert.ToInt64(Request.QueryString["CoupleUnitID"]);
            }
            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["MRNo"] != null)
            {
                MRNo = Convert.ToString(Request.QueryString["MRNo"]);
            }
            if (Request.QueryString["FName"] != null)
            {
                FName = Convert.ToString(Request.QueryString["FName"]);
            }
            if (Request.QueryString["MName"] != null)
            {
                MName = Convert.ToString(Request.QueryString["MName"]);
            }          
            if (Request.QueryString["FamilyName"] != null)
            {
                FamilyName = Convert.ToString(Request.QueryString["FamilyName"]);
            }
            if (Request.QueryString["LName"] != null)
            {
                Lname = Convert.ToString(Request.QueryString["LName"]);
            }
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@CoupleUnitID", CoupleUnitID);
            myDoc.SetParameterValue("@MRNo", MRNo);
            myDoc.SetParameterValue("@FName", FName);
            myDoc.SetParameterValue("@MName", MName);
            myDoc.SetParameterValue("@LName", Lname);
            myDoc.SetParameterValue("@FamilyName", FamilyName);
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
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