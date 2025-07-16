using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using System;


namespace PalashDynamics.Web.Reports.Pathology
{
    public partial class BSRPathologyResultEntryReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            string SelectedTestID = null;//SelectedTestID as ResultID
            long UnitID = 0;
            int ISRferalDoctorSignature = 0;
            bool IsDuplicate=false;
            string BillNumber = null;
            long EmpID = 0;
            string SampleNo = null;
            string TestID = null;
            bool IsOutSource = false;


            myDoc = new ReportDocument();         
         
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != "0")
            {
                SelectedTestID = Convert.ToString(Request.QueryString["ID"]);
            }
            if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "0")
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["ISRferalDoctorSignature"] != null && Request.QueryString["ISRferalDoctorSignature"] != "0")
            {
                ISRferalDoctorSignature = Convert.ToInt32(Request.QueryString["ISRferalDoctorSignature"]);
            }
            if (Request.QueryString["IsDuplicate"] != null && Request.QueryString["IsDuplicate"] != "0")
            {
                IsDuplicate = Convert.ToBoolean(Request.QueryString["IsDuplicate"]);
            }
            if (!String.IsNullOrWhiteSpace(Request.QueryString["BillNumber"]) &&!String.IsNullOrWhiteSpace(Request.QueryString["BillNumber"]))
            {
                BillNumber = Convert.ToString(Request.QueryString["BillNumber"]);               
            }
            // Added by Anumani 
            if (Request.QueryString["UnitID"] != null && Request.QueryString["UnitID"] != "0")
            {
                SampleNo = Convert.ToString(Request.QueryString["SampleNo"]);
            }

            if (Request.QueryString["TestID"] != null && Request.QueryString["TestID"] != "0")
            {
                TestID = Convert.ToString(Request.QueryString["TestID"]);
            }   

            if (Request.QueryString["EmpID"] != null && Request.QueryString["EmpID"] != "0")
            {
                EmpID = Convert.ToInt64(Request.QueryString["EmpID"]);
            }
            //if (EmpID != 24)
            //{
            //    myDoc.Load(Server.MapPath("rptBSRPathoResultEntry_3.rpt"));
            //}
            //else
            //{
            //    myDoc.Load(Server.MapPath("rptBSRPathoResultEntryForCompare_2.rpt"));
            //}

            if (Request.QueryString["IsOutSource"] != null && Request.QueryString["IsOutSource"] != "0")
            {
                IsOutSource = Convert.ToBoolean(Request.QueryString["IsOutSource"]);
            }

            //Added By CDS 
            if (IsOutSource == false)
            {
                myDoc.Load(Server.MapPath("rptBSRPathoResultEntry_3.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("rptBSRPathologyOutSourceReport.rpt"));
            }

            myDoc.SetParameterValue("@BillID", SelectedTestID);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@ISRferalDoctorSignature", ISRferalDoctorSignature);
            myDoc.SetParameterValue("@IsDuplicate", IsDuplicate);
            myDoc.SetParameterValue("@BillNumber", BillNumber);
            myDoc.SetParameterValue("@SampleNo", SampleNo);
            myDoc.SetParameterValue("@TestID", TestID);
            myDoc.SetParameterValue("@IsDuplicate", IsDuplicate);

            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();


            MemoryStream oStream = new MemoryStream();
            oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(oStream.ToArray());
            Response.End();
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