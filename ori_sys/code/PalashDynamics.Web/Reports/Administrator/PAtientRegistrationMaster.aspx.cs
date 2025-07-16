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

namespace PalashDynamics.Web.Reports.Administrator
{
    public partial class PAtientRegistrationMaster : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportDocument myDoc;

            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("Rpt_patientregistration.rpt"));

            long a = 0, b = 0, c = 0, d = 0, f = 0, g = 0, h = 0, i = 0, j = 0, k = 0, l = 0, m = 0, n = 0, o = 0, p = 0, q = 0, r = 0, s = 0, t = 0, u = 0,v=0, uid = 0;
            long photoattached = 2, Documentttachd = 2;
            long Notphotoattachd = 0, NotDocumentattachd = 0;
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpDOB1 = null;
            Nullable<DateTime> dtpAnni = null;
            long ClinicID = 0;
            string FromAge = "";
            string ToAge = "";
            //added by rohini dated 24/12/2015
            string Mobile = "";
            string Mrno = "";
            //
            bool Excel = false;
            if (Request.QueryString["a"] != null)
            {
                a = Convert.ToInt64(Request.QueryString["a"]);
            }
            if (Request.QueryString["b"] != null)
            {
                b = Convert.ToInt64(Request.QueryString["b"]);
            }
            if (Request.QueryString["c"] != null)
            {
                c = Convert.ToInt64(Request.QueryString["c"]);
            }
            if (Request.QueryString["d"] != null)
            {
                d = Convert.ToInt64(Request.QueryString["d"]);
            }
            if (Request.QueryString["f"] != null)
            {
                f = Convert.ToInt64(Request.QueryString["f"]);
            }
            if (Request.QueryString["g"] != null)
            {
                g = Convert.ToInt64(Request.QueryString["g"]);
            }
            if (Request.QueryString["h"] != null)
            {
                h = Convert.ToInt64(Request.QueryString["h"]);
            }
            if (Request.QueryString["i"] != null)
            {
                i = Convert.ToInt64(Request.QueryString["i"]);
            }
            if (Request.QueryString["j"] != null)
            {
                j = Convert.ToInt64(Request.QueryString["j"]);
            }
            if (Request.QueryString["k"] != null)
            {
                k = Convert.ToInt64(Request.QueryString["k"]);
            }
            if (Request.QueryString["l"] != null)
            {
                l = Convert.ToInt64(Request.QueryString["l"]);
            }
            if (Request.QueryString["m"] != null)
            {
                m = Convert.ToInt64(Request.QueryString["m"]);
            }
            if (Request.QueryString["n"] != null)
            {
                n = Convert.ToInt64(Request.QueryString["n"]);
            }
            if (Request.QueryString["o"] != null)
            {
                o = Convert.ToInt64(Request.QueryString["o"]);
            }
            if (Request.QueryString["p"] != null)
            {
                p = Convert.ToInt64(Request.QueryString["p"]);
            }
            if (Request.QueryString["q"] != null)
            {
                q = Convert.ToInt64(Request.QueryString["q"]);
            }
            if (Request.QueryString["r"] != null)
            {
                r = Convert.ToInt64(Request.QueryString["r"]);
            }
            if (Request.QueryString["s"] != null)
            {
                s = Convert.ToInt64(Request.QueryString["s"]);
            }

            //added by rohini dated 24/12/2015
            if (Request.QueryString["t"] != null)
            {
                t = Convert.ToInt64(Request.QueryString["t"]);
            }
            //

            if (Request.QueryString["u"] != null)
            {
                u = Convert.ToInt64(Request.QueryString["u"]);
            }

            if (Request.QueryString["v"] != null)
            {
                v = Convert.ToInt64(Request.QueryString["v"]);
            }

            if (Request.QueryString["photoattached"] != null)
            {
                photoattached = Convert.ToInt64(Request.QueryString["photoattached"]);
            }
            if (Request.QueryString["Documentttachd"] != null)
            {
                Documentttachd = Convert.ToInt64(Request.QueryString["Documentttachd"]);
            }
            if (Request.QueryString["dtpF"] != null && Request.QueryString["dtpF"] != "")
            {
                dtpF = Convert.ToDateTime(Request.QueryString["dtpF"]);
            }
            if (Request.QueryString["dtpT"] != null && Request.QueryString["dtpT"] != "")
            {
                dtpT = Convert.ToDateTime(Request.QueryString["dtpT"]);
            }
            if (Request.QueryString["dtpDOB1"] != null && Request.QueryString["dtpDOB1"] != "")
            {
                dtpDOB1 = Convert.ToDateTime(Request.QueryString["dtpDOB1"]);
            }
            if (Request.QueryString["dtpAnni"] != null && Request.QueryString["dtpAnni"] != "")
            {
                dtpAnni = Convert.ToDateTime(Request.QueryString["dtpAnni"]);
            }
            uid = Convert.ToInt64(Request.QueryString["Uid"]);

            if (Request.QueryString["FromAge"] != null)
            {
                FromAge = Convert.ToString(Request.QueryString["FromAge"]);
            }
            if (Request.QueryString["ToAge"] != null)
            {
                ToAge = Convert.ToString(Request.QueryString["ToAge"]);
            }

            //added by rohini h dated 24/12/2015
            if (Request.QueryString["Mobile"] != null)
            {
                Mobile = Convert.ToString(Request.QueryString["Mobile"]);
            }
            if (Request.QueryString["Mrno"] != null)
            {
                Mrno = Convert.ToString(Request.QueryString["Mrno"]);
            }
            //
            if (Request.QueryString["ClinicID"] != null)
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);

            }
            if (Request.QueryString["Excel"] != null)
            {
                Excel = Convert.ToBoolean(Request.QueryString["Excel"]);
            }
            myDoc.SetParameterValue("@UnitID", uid, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@Fromdate", dtpF);
            myDoc.SetParameterValue("@Todate", dtpT);
            myDoc.SetParameterValue("@DOB", dtpDOB1);
            myDoc.SetParameterValue("@MaritalAnni", dtpAnni);
            myDoc.SetParameterValue("@RegistrationType", a);
            myDoc.SetParameterValue("@ReferenceFrom", b);
            myDoc.SetParameterValue("@ReferralDoctor", c);
            myDoc.SetParameterValue("@BloodGroup", d);
            myDoc.SetParameterValue("@MaritalStatus", f);
            myDoc.SetParameterValue("@Occupation", g);
            myDoc.SetParameterValue("@IDProof", h);
            myDoc.SetParameterValue("@Spcialreg", i);
            myDoc.SetParameterValue("@Religion", j);
            myDoc.SetParameterValue("@Gender", k);
            myDoc.SetParameterValue("@Perflanguage", l);
            myDoc.SetParameterValue("@Tretreqd", m);
            myDoc.SetParameterValue("@Nationlity", n);
            myDoc.SetParameterValue("@Countery", o);
            myDoc.SetParameterValue("@State", p);
            myDoc.SetParameterValue("@City", q);
            myDoc.SetParameterValue("@Area", r);
            myDoc.SetParameterValue("@Education", s);
            //added by rohini dated 24/12/2015
            myDoc.SetParameterValue("@CatID", t);
            //

            myDoc.SetParameterValue("@MarketingExecutiveID", u);
            myDoc.SetParameterValue("@CampID", v); 

            myDoc.SetParameterValue("@photoattachd", photoattached);
            myDoc.SetParameterValue("@Documentttachd", Documentttachd);
            myDoc.SetParameterValue("@Notphotoattachd", Notphotoattachd);
            myDoc.SetParameterValue("@NotDocumentattachd", NotDocumentattachd);
            myDoc.SetParameterValue("@FromAge", FromAge);
            myDoc.SetParameterValue("@ToAge", ToAge);
            //added by rohini dated 24/12/2015

            myDoc.SetParameterValue("@ContactNo", Mobile);
            myDoc.SetParameterValue("@MRNo", Mrno);
            //
            myDoc.SetParameterValue("@ClinicID", ClinicID);

            myDoc.SetParameterValue("@UnitID", uid, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            if (Excel == true)
            {
                //commented by rohini 28/12/2015
                //MemoryStream oStream = new MemoryStream();
                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                //Response.Clear();
                //Response.Buffer = true;

                // by rohini new code to reffer for all report 28/12/2015
                System.IO.Stream oStream = null;
                byte[] byteArray = null;
                oStream = myDoc.ExportToStream(ExportFormatType.ExcelWorkbook);
                byteArray = new byte[oStream.Length];
                oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";

                //Response.BinaryWrite(oStream.ToArray());


                //  var ieVersion = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("Version");
                //  string str = Convert.ToString(Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("Version")).Substring(0, 3);

                var version = Request.Browser.Version;

                if (Convert.ToDouble(version) == 11.0)
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }
                else if (Convert.ToDouble(version) == 10.0)
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }
                else if (Convert.ToDouble(version) == 9.0)
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }
                else if (Convert.ToDouble(version) == 8.0)
                {
                    Response.ContentType = "application/vnd.ms-excel";
                }
                else
                {
                    Response.ContentType = "application/vnd.ms-excel";
                }
                //commented by rohini 28/12/2015
                //Response.BinaryWrite(oStream.ToArray());
                //Response.End();
                // by rohini new code to reffer for all report 28/12/2015

                Response.BinaryWrite(byteArray);

                Response.End();


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