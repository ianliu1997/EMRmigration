using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;

namespace PalashDynamics.Web.Reports.IVF
{
    public partial class ARTAndLabDaysReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            

            long CoupleID = 0;
            long CoupleUnitID = 0;
            long PlanTherapyId = 0;

            if (Request.QueryString["CoupleID"] != null)
            {
                CoupleID = Convert.ToInt64(Request.QueryString["CoupleID"]);
            }
            if (Request.QueryString["CoupleUnitID"] != null)
            {
                CoupleUnitID = Convert.ToInt64(Request.QueryString["CoupleUnitID"]);
            }
            //if (Request.QueryString["PlanTherapyId"] != null)
            //{
            //    PlanTherapyId = Convert.ToInt64(Request.QueryString["PlanTherapyId"]);
            //}


            ReportDocument myDoc0 = new ReportDocument();
            myDoc0.Load(Server.MapPath("rpt_ARTAndLabDaysReport.rpt"));
            myDoc0.SetParameterValue("@CoupleID", CoupleID);
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID);
            myDoc0.SetParameterValue("@CoupleID", CoupleID,"RptLabDay1");
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID, "RptLabDay1");
            myDoc0.SetParameterValue("@CoupleID", CoupleID, "RptLabDay2");
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID, "RptLabDay2");
            myDoc0.SetParameterValue("@CoupleID", CoupleID, "RptLabDay3");
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID, "RptLabDay3");
            myDoc0.SetParameterValue("@CoupleID", CoupleID, "RptLabDay4");
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID, "RptLabDay4");
            myDoc0.SetParameterValue("@CoupleID", CoupleID, "RptLabDay5");
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID, "RptLabDay5");
            myDoc0.SetParameterValue("@CoupleID", CoupleID, "RptLabDay6");
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID, "RptLabDay6");
            myDoc0.SetParameterValue("@CoupleID", CoupleID, "RptVitrification");
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID, "RptVitrification");
            myDoc0.SetParameterValue("@CoupleID", CoupleID, "RptThawing");
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID, "RptThawing");
            myDoc0.SetParameterValue("@CoupleID", CoupleID, "RptET");
            myDoc0.SetParameterValue("@CoupleUnitID", CoupleUnitID, "RptET");

         //   myDoc0.SetParameterValue("@PlanTherapyId", PlanTherapyId);
            myDoc0.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc0);
            CrystalReportViewer0.ReportSource = myDoc0;
            CrystalReportViewer0.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer0.DataBind();


         //   ReportDocument myDoc1 = new ReportDocument();
         //   myDoc1.Load(Server.MapPath("rpt_ARTAndLabDay1Report.rpt"));
         //   myDoc1.SetParameterValue("@CoupleID", CoupleID);
         //   myDoc1.SetParameterValue("@CoupleUnitID", CoupleUnitID);
         //   //myDoc1.SetParameterValue("@PlanTherapyId", PlanTherapyId);
         //   myDoc1.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
         //   CReportAuthentication.Impersonate(myDoc1);
         //   CrystalReportViewer1.ReportSource = myDoc1;
         //   CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
         //   CrystalReportViewer1.DataBind();


         //   ReportDocument myDoc2 = new ReportDocument();
         //   myDoc2.Load(Server.MapPath("rpt_ARTAndLabDay2Report.rpt"));
         //   myDoc2.SetParameterValue("@CoupleID", CoupleID);
         //   myDoc2.SetParameterValue("@CoupleUnitID", CoupleUnitID);
         //  // myDoc2.SetParameterValue("@PlanTherapyId", PlanTherapyId);
         //   myDoc2.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
         //   CReportAuthentication.Impersonate(myDoc2);
         //   CrystalReportViewer2.ReportSource = myDoc2;
         //   CrystalReportViewer2.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
         //   CrystalReportViewer2.DataBind();


         //   ReportDocument myDoc3 = new ReportDocument();
         //   myDoc3.Load(Server.MapPath("rpt_ARTAndLabDay3Report.rpt"));
         //   myDoc3.SetParameterValue("@CoupleID", CoupleID);
         //   myDoc3.SetParameterValue("@CoupleUnitID", CoupleUnitID);
         //   //myDoc3.SetParameterValue("@PlanTherapyId", PlanTherapyId);
         //   myDoc3.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
         //   CReportAuthentication.Impersonate(myDoc3);
         //   CrystalReportViewer3.ReportSource = myDoc3;
         //   CrystalReportViewer3.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
         //   CrystalReportViewer3.DataBind();

         //   ReportDocument myDoc4 = new ReportDocument();
         //   myDoc4.Load(Server.MapPath("rpt_ARTAndLabDay4Report.rpt"));
         //   myDoc4.SetParameterValue("@CoupleID", CoupleID);
         //   myDoc4.SetParameterValue("@CoupleUnitID", CoupleUnitID);
         //  // myDoc4.SetParameterValue("@PlanTherapyId", PlanTherapyId);
         //   myDoc4.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
         //   CReportAuthentication.Impersonate(myDoc4);
         //   CrystalReportViewer4.ReportSource = myDoc4;
         //   CrystalReportViewer4.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
         //   CrystalReportViewer4.DataBind();

         //   ReportDocument myDoc5 = new ReportDocument();
         //   myDoc5.Load(Server.MapPath("rpt_ARTAndLabDay5Report.rpt"));
         //   myDoc5.SetParameterValue("@CoupleID", CoupleID);
         //   myDoc5.SetParameterValue("@CoupleUnitID", CoupleUnitID);
         // //  myDoc5.SetParameterValue("@PlanTherapyId", PlanTherapyId);
         //   myDoc5.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
         //   CReportAuthentication.Impersonate(myDoc5);
         //   CrystalReportViewer5.ReportSource = myDoc5;
         //   CrystalReportViewer5.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
         //   CrystalReportViewer5.DataBind();

         //   ReportDocument myDoc6 = new ReportDocument();
         //   myDoc6.Load(Server.MapPath("rpt_ARTAndLabDay6Report.rpt"));
         //   myDoc6.SetParameterValue("@CoupleID", CoupleID);
         //   myDoc6.SetParameterValue("@CoupleUnitID", CoupleUnitID);
         ////   myDoc6.SetParameterValue("@PlanTherapyId", PlanTherapyId);
         //   myDoc6.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
         //   CReportAuthentication.Impersonate(myDoc6);
         //   CrystalReportViewer6.ReportSource = myDoc6;
         //   CrystalReportViewer6.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
         //   CrystalReportViewer6.DataBind();

        }
    }
}