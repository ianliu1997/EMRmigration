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
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PalashDynamics.Web.Reports.CRM
{
    public partial class ClinicWiseInvestigation : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strPath;
                string strConnection;
                String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
                String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
                String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
                String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

                String strReportName = "Clinic Wise Investigation Report";
                strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
                strPath = Server.MapPath(".");
                strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";
                string FromDate = null;
                string ToDate = null;
                string ToDatePrint = null;

                DateTime? FrDt = null;
                DateTime? ToDt = null;
                DateTime? ToDtP = null;
                long clinicID = 0, counterID=0;
                bool IsExporttoExcel = false;
                string AppPath = Request.ApplicationPath;

                FromDate = Convert.ToString(Request.QueryString["FromDate"]);
                ToDate = Convert.ToString(Request.QueryString["ToDate"]);
                ToDatePrint = Convert.ToString(Request.QueryString["ToDatePrint"]);
                IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);
                if (!string.IsNullOrEmpty(FromDate))
                {
                    FrDt = Convert.ToDateTime(FromDate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    ToDt = Convert.ToDateTime(ToDate);
                }
                if (!string.IsNullOrEmpty(ToDatePrint))
                {
                    ToDtP = Convert.ToDateTime(ToDatePrint);
                }
                if (Request.QueryString["ClinicID"] != null)
                {
                    clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
                }
                if (Request.QueryString["counterID"] != null)
                {
                    counterID = Convert.ToInt64(Request.QueryString["counterID"]);
                }
                if (!IsExporttoExcel)
                {
                    myDoc = new ReportDocument();
                    myDoc.Load(Server.MapPath("crClinicWiseInvestigationReport1.rpt"));
                    myDoc.SetParameterValue("@FromDate", FrDt);
                    myDoc.SetParameterValue("@ToDate", ToDt);
                    myDoc.SetParameterValue("@ToDatePrint", ToDtP);
                    myDoc.SetParameterValue("@ClinicID", clinicID);
                    myDoc.SetParameterValue("@counterID", counterID);
                    myDoc.SetParameterValue("@UnitID", clinicID, "rptCommonReportHeader.rpt");
                    myDoc.SetParameterValue("@UnitID", clinicID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
                    myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                    CReportAuthentication.Impersonate(myDoc);
                    CrystalReportViewer1.ReportSource = myDoc;
                    CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                    CrystalReportViewer1.DataBind();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand();
                    DataTable dt = new DataTable();
                    SqlConnection sqlConnection1 = new SqlConnection(strConnection);
                    sqlConnection1.Open();
                    cmd.Connection = sqlConnection1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "CIMS_rptClinicWiseInvestigationReport";
                    SqlParameter param = new SqlParameter();
                    param.DbType = DbType.DateTime;
                    param.Value = FrDt;
                    param.ParameterName = "FromDate";
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.DbType = DbType.DateTime;
                    param.Value = ToDt;
                    param.ParameterName = "ToDate";
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.DbType = DbType.DateTime;
                    param.Value = ToDt;
                    param.ParameterName = "ToDatePrint";
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.DbType = DbType.Int64;
                    param.Value = clinicID;
                    param.ParameterName = "ClinicID";
                    cmd.Parameters.Add(param);

                    param = new SqlParameter();
                    param.DbType = DbType.Int64;
                    param.Value = counterID;
                    param.ParameterName = "counterID";
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();
                    
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);

                    ExportToExcel.CExcelExport objExcel = new ExportToExcel.CExcelExport();
                    DataTable dtReturn = new DataTable();
                    objExcel.ConnectionString = strConnection;
                    if (dt.Rows.Count > 0)
                    {
                        //Change Order ad define in Report
                        var varlist = from c in dt.AsEnumerable()

                                      select new
                                      {
                                          SrNo = 0,
                                          Date = "",
                                          ClinicName = c.IsNull("ClinicName") ? string.Empty : c.Field<string>("ClinicName"),
                                          TestName = c.IsNull("TestName") ? string.Empty : c.Field<string>("TestName"),
                                          Tot = c.IsNull("Tot") ? 0.0 : c.Field<System.Double>("Tot"),
                                          TotalCount = 0
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
                    if (dtReturn != null)
                    {
                        if (dtReturn.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtReturn.Columns.Count; i++)
                            {
                                if (dtReturn.Columns[i].ColumnName.Equals("Sr No"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Sr. No";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("ClinicName"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Clinic Name";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("TestName"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Test Name";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("Tot"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Total Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("TotalCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Total Count";
                                }
                            }
                            //this is use for fields which are calculated by Formula and Sr no. 
                            for (int i = 0; i < dtReturn.Rows.Count; i++)
                            {
                                dtReturn.Rows[i]["Sr. No"] = i + 1;
                                dtReturn.Rows[i]["Total Count"] = dt.Rows[i]["TotalCount"];
                                //DateTime newdate = (DateTime)dt.Rows[i]["Date"];
                                dtReturn.Rows[i]["Date"] = ((DateTime)dt.Rows[i]["Date"]).ToString("dd-MM-yyyy");
                            }
                            objExcel.ExportToExcel("", strReportName, strPath, dtReturn);
                        }
                        else
                        {
                            dt = new DataTable();
                            dt.Columns.Add("Sr. No");
                            dt.Columns.Add("Date");
                            dt.Columns.Add("Clinic Name");
                            dt.Columns.Add("Test Name");
                            dt.Columns.Add("Total Revenue Count");
                            objExcel.ExportToExcel("", strReportName, strPath, dt);
                        }
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Sr. No");
                        dt.Columns.Add("Date");
                        dt.Columns.Add("Clinic Name");
                        dt.Columns.Add("Test Name");
                        dt.Columns.Add("Total Revenue Count");
                        objExcel.ExportToExcel("", strReportName, strPath, dt);
                    }
                    //Response.Redirect(AppPath+"\\Reports\\CRM\\" + strReportName + ".xls");
                    // Response.Redirect(strReportName + ".xls");
                    //// string URL = "/" + System.Configuration.ConfigurationSettings.AppSettings["VirtualDir"].ToString() + "/Reports/CRM/" + strReportName + ".xlsx";
                    ////Response.Write("<script language='javascript'>window.open('" + URL + "','_self');</script>");

                    string URL = "/" + System.Configuration.ConfigurationSettings.AppSettings["VirtualDir"].ToString() + "/Reports/CRM/" + strReportName + ".xls";
                    Response.Write("<script language='javascript'>window.open('" + URL + "','_self');</script>");
                    //  }                          
                    sqlConnection1.Close();
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