using System;
using System.Web;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Web.Configuration;

namespace PalashDynamics.Web.Reports.CRM
{
    public partial class CustomerDetailReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            string strPath;
            string strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            String strReportName = "Customer Details Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DateTime? ToDatePrint = null;

            //long CardID = 0;
            string Area = null;
            long clinicID = 0;
            //  long? visitCount=null;
            long visitCount = 0;
            bool IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);
            string AppPath = Request.ApplicationPath;

      

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);

            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            //if (Request.QueryString["CardID"] != null)
            //{
            //    CardID = Convert.ToInt64(Request.QueryString["CardID"]);
            //}
            if (Request.QueryString["ClinicID"] != null)
            {
                clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["Area"] != null && Request.QueryString["Area"] != "")
            {
                Area = Convert.ToString(Request.QueryString["Area"]);
            }
            if (Request.QueryString["VisitCount"] != null)
            {
                visitCount = Convert.ToInt64(Request.QueryString["VisitCount"]);
            }
            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }
            //if (Request.QueryString["IsExporttoExcel"] != null && Request.QueryString["IsExporttoExcel"] != "")
            //{
            //    IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);
            //}            
            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("rptCustomerDetailReportrpt1.rpt"));
            if (!IsExporttoExcel)
            {
                //myDoc.SetParameterValue("@CardID", CardID);
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                if (Area != null)
                { myDoc.SetParameterValue("@Area",Area); ; }
                else
                { myDoc.SetParameterValue("@Area", DBNull.Value); }


                myDoc.SetParameterValue("@ClinicID", clinicID);
                myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
                myDoc.SetParameterValue("@VisitCount", DBNull.Value);

                myDoc.SetParameterValue("@UnitID", clinicID, "rptUnitLogo.rpt");//Added by ajit jadhav date 19/9/2016 Get Unit LOGO
                myDoc.SetParameterValue("@UnitID", clinicID, "rptCommonReportHeader.rpt");
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);

                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();

                ////myDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "");

                //MemoryStream oStream = new MemoryStream();

                //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";

                //Response.BinaryWrite(oStream.ToArray());
                //Response.End();
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                SqlConnection sqlConnection1 = new SqlConnection(strConnection);
                sqlConnection1.Open();

                //using (SqlCommand cmd = new SqlCommand())
                //{
                cmd.Connection = sqlConnection1;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CIMS_rptCustomerDetailReport";
                SqlParameter param = new SqlParameter();


                //param.DbType = DbType.Int64;
                //param.Value = CardID;
                //param.ParameterName = "CardID";
                //cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = FromDate;
                param.ParameterName = "FromDate";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = ToDate;
                param.ParameterName = "ToDate";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.String;
                //param.Value = Area;
                if (Area != null)
                { param.Value = Area; }
                else
                { param.Value = DBNull.Value; }
                param.ParameterName = "Area";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = clinicID;
                param.ParameterName = "ClinicID";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                // param.Value = visitCount;
                param.Value = DBNull.Value;
                param.ParameterName = "VisitCount";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = ToDatePrint;
                param.ParameterName = "ToDatePrint";
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                ExportToExcel.CExcelExport objExcel = new ExportToExcel.CExcelExport();
                DataTable dtReturn = new DataTable();
                objExcel.ConnectionString = strConnection;

                if (dt.Rows.Count > 0)
                {
                    //Change Order as define in Report
                    var varlist = from c in dt.AsEnumerable()

                                  select new
                                  {
                                      SrNo = 0,
                                      unit = c.IsNull("unit") ? string.Empty : c.Field<string>("unit"),
                                      RegistrationDate = "",
                                      PatientName = c.IsNull("PatientName") ? string.Empty : c.Field<string>("PatientName"),
                                      Area = "",
                                      //LoyaltyCard = c.IsNull("LoyaltyCard") ? 0 : c.Field<long>("LoyaltyCard"),
                                      //LoyaltyCard = c.IsNull("LoyaltyCard") ? string.Empty : c.Field<string>("LoyaltyCard"),
                                      Visitcount = c.IsNull("Visitcount") ? 0 : c.Field<System.Int64>("Visitcount")
                                      //PharmacyAmt = c.IsNull("PharmacyAmt") ? 0.0 : c.Field<System.Double>("PharmacyAmt"),
                                      //BillAmount = c.IsNull("BillAmount") ? 0.0 : c.Field<System.Double>("Billamount")
                                  };
                    PropertyInfo[] oProps = null;
                    foreach (var rec in varlist)
                    {
                        if (oProps == null)
                        {
                            oProps = ((Type)rec.GetType()).GetProperties();
                            foreach (PropertyInfo pi in oProps)
                            {
                                Type colType = pi.PropertyType;
                                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                                == typeof(Nullable<>)))
                                {
                                    colType = colType.GetGenericArguments()[0];
                                }
                                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                            }
                        }
                        DataRow dr = dtReturn.NewRow();
                        foreach (PropertyInfo pi in oProps)
                        {
                            dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                            (rec, null);
                        }
                        dtReturn.Rows.Add(dr);
                    }
                }
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtReturn.Columns.Count; i++)
                        {
                            if (dtReturn.Columns[i].ColumnName.Equals("Sr No"))
                            {
                                dtReturn.Columns[i].ColumnName = "Sr. No";
                            }
                            //dtReturn.Rows[i]["SrNo"] = i + 1;
                            if (dtReturn.Columns[i].ColumnName.Equals("unit"))
                            {
                                dtReturn.Columns[i].ColumnName = "Clinic Name";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("RegistrationDate"))
                            {
                                dtReturn.Columns[i].ColumnName = "Registration Date";
                            }
                            //if (dtReturn.Columns[i].ColumnName.Equals("LoyaltyCard"))
                            //{
                            //    dtReturn.Columns[i].ColumnName = "Loyalty Card";
                            //}
                            if (dtReturn.Columns[i].ColumnName.Equals("Visitcount"))
                            {
                                dtReturn.Columns[i].ColumnName = "Visit Count";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("Area"))
                            {
                                dtReturn.Columns[i].ColumnName = "Area";
                            }
                        }

                        //this is use for fields which are calculated by Formula and Sr no. 
                        //dtReturn.NewRow();
                        int j = 0;
                        long tvsc = 0;
                        for (j = 0; j < dtReturn.Rows.Count; j++)
                        {
                            dtReturn.Rows[j]["Sr. No"] = j + 1;
                            dtReturn.Rows[j]["Registration Date"] = ((DateTime)dt.Rows[j]["RegistrationDate"]).ToString("dd-MM-yyyy");
                            //dtReturn.Rows[j]["PatientName"] = (string)(dt.Rows[j]["FirstName"]) + ' ' + (string)(dt.Rows[j]["MiddleName"]) + ' ' + (string)(dt.Rows[j]["LastName"]);
                            //dtReturn.Rows[j]["Visit Count"] = dt.Rows[j]["Visitcnt"]; //(Int64)(dt.Rows[i]["Visitcnt"]);
                            tvsc = Convert.ToInt64(dt.Rows[j]["Visitcount"]) + tvsc;
                            //c.IsNull("Visitcnt") ? 0 : c.Field<System.Int64>("Visitcnt")
                        }
                        DataRow dr = dtReturn.NewRow();

                        dtReturn.Rows.Add(dr);
                        //Label label = new Label();
                        //label.Text = "Total Visit Count";
                        //label.Font.Bold = true;

                        dtReturn.Rows[j]["Loyalty Card"] = "Total Visit Count";
                        dtReturn.Rows[j]["Visit Count"] = tvsc;

                        objExcel.ExportToExcel("", strReportName, strPath, dtReturn);
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Sr. No");
                        dt.Columns.Add("Clinic Name");
                        dt.Columns.Add("Registration Date");
                        dt.Columns.Add("Patient Name");
                        //dt.Columns.Add("Loyalt Card");
                        dt.Columns.Add("Visit Count");
                        dt.Columns.Add("Area");
                        objExcel.ExportToExcel("", strReportName, strPath, dt);
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("Sr. No");
                    dt.Columns.Add("Clinic Name");
                    dt.Columns.Add("Registration Date");
                    dt.Columns.Add("Patient Name");
                    //dt.Columns.Add("Loyalt Card");
                    dt.Columns.Add("Area");
                    dt.Columns.Add("Visit Count");
                    objExcel.ExportToExcel("", strReportName, strPath, dt);
                }
                //Response.Redirect(AppPath + "\\Reports\\CRM\\" + strReportName + ".xls");
                //Response.Redirect(strReportName + ".xls");
                //Response.Redirect("http://" + Server.MachineName + "/RAZIClinic.Web/Reports/CRM/" + strReportName + ".xls");

                string URL = "/" + System.Configuration.ConfigurationSettings.AppSettings["VirtualDir"].ToString() + "/Reports/CRM/" + strReportName + ".xls";
                Response.Write("<script language='javascript'>window.open('" + URL + "','_self');</script>");
                
                sqlConnection1.Close();
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