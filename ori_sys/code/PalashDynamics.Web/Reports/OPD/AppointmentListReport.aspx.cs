using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.OPD
{
    public partial class AppointmentListReport : System.Web.UI.Page
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



                long clinicID = 0;
                long DeptID = 0;
                long DocID = 0;
                long Stat = 0;
                long Type = 0;
                long SpRegistration = 0;


                myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("crptAppointmentListReport.rpt"));



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

                if (Request.QueryString["AppointStatus"] != null)
                {
                    Stat = Convert.ToInt64(Request.QueryString["AppointStatus"]);
                }

                if (Request.QueryString["VisitMark"] != null)
                {
                    Type = Convert.ToInt64(Request.QueryString["VisitMark"]);
                }

                if (Request.QueryString["SpecialRegistrationId"] != null)
                {
                    SpRegistration = Convert.ToInt64(Request.QueryString["SpecialRegistrationId"]);
                }

                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@UnitID", clinicID);
                myDoc.SetParameterValue("@DepartmentID", DeptID);
                myDoc.SetParameterValue("@DoctorID", DocID);
                myDoc.SetParameterValue("@AppointStatus", Stat);
                myDoc.SetParameterValue("@VisitMark", Type);
                myDoc.SetParameterValue("@SpecialRegistrationId", SpRegistration);

              //  myDoc.SetParameterValue("@Status", true);


                myDoc.SetParameterValue("@UnitID", clinicID,"Header");
                myDoc.SetParameterValue("@UnitID", clinicID, "rptUnitLogo.rpt");
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);

                CrystalReportViewer1.ReportSource = myDoc;

                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                CrystalReportViewer1.DataBind();
                //MemoryStream oStream = new MemoryStream();

                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";

                //Response.BinaryWrite(oStream.ToArray());
                //Response.End();
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