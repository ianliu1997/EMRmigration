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
using System.Data.SqlClient;
using System.Data;
using System.Reflection;


namespace PalashDynamics.Web.Reports.CRM
{
    public partial class LoyaltyProgram : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strPath, strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            String strReportName = "Loyalty Program Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";
           
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DateTime? ToDatePrint = null;
            long LoyaltyId = 0;
            long ClinicID = 0;

            bool IsExporttoExcel = false;
            string AppPath = Request.ApplicationPath;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("LoyaltyProgramReport.rpt"));

            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["LoyaltyID"] != null)
            {
                LoyaltyId = Convert.ToInt64(Request.QueryString["LoyaltyID"]);
            }
            if (Request.QueryString["ClinicID"] != null)
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["PrintDate"] != null && Request.QueryString["PrintDate"] != "")
            {
                ToDatePrint = Convert.ToDateTime(Request.QueryString["PrintDate"]);
            }
            if (Request.QueryString["IsExporttoExcel"] != null)
            {
                IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);
            }

            if (!IsExporttoExcel)
            {
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@UnitID", ClinicID);
                myDoc.SetParameterValue("@LoyaltyID", LoyaltyId);
                myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
                myDoc.SetParameterValue("@UnitID", ClinicID, "rptUnitLogo.rpt");//Added by ajit jadhav date 19/9/2016 Get Unit LOGO
                myDoc.SetParameterValue("@UnitID", ClinicID, "rptCommonReportHeader.rpt");
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
                cmd.CommandText = "CIMS_rptLoyaltyCardIssue";
                SqlParameter param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = FromDate;
                param.ParameterName = "Fromdate";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = ToDate;
                param.ParameterName = "ToDate";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = ToDatePrint;
                param.ParameterName = "ToDatePrint";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = ClinicID;
                param.ParameterName = "UnitID";
                cmd.Parameters.Add(param);


                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = LoyaltyId;
                param.ParameterName = "LoyaltyID";
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
                                      MRNo = "",
                                      PatientName = "", //c.IsNull("PatientName") ? string.Empty : c.Field<string>("PatientName"),
                                      LoyaltyCard = c.IsNull("LoyaltyCard") ? string.Empty : c.Field<string>("LoyaltyCard"),
                                      LoyaltyCardIssueDate = "",
                                      EffectiveDate = "",
                                      ExpiryDate = "",
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
                            if (dtReturn.Columns[i].ColumnName.Equals("Sr o"))
                            {
                                dtReturn.Columns[i].ColumnName = "Sr. No";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("unit"))
                            {
                                dtReturn.Columns[i].ColumnName = "Clinic Name";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("MRNo"))
                            {
                                dtReturn.Columns[i].ColumnName = "Mr. No";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("PatientName"))
                            {
                                dtReturn.Columns[i].ColumnName = "Patient Name";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("LoyaltyCard"))
                            {
                                dtReturn.Columns[i].ColumnName = "Loyalty Card";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("LoyaltyCardIssueDate"))
                            {
                                dtReturn.Columns[i].ColumnName = "Issue Date";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("EffectiveDate"))
                            {
                                dtReturn.Columns[i].ColumnName = "Effective Date";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("ExpiryDate"))
                            {
                                dtReturn.Columns[i].ColumnName = "Expiry Date";
                            }
                        }
                        //this is use for fields which are calculated by Formula and Sr no. 
                        for (int i = 0; i < dtReturn.Rows.Count; i++)
                        {
                            dtReturn.Rows[i]["Sr. No"] = i + 1;
                            dtReturn.Rows[i]["Mr. No"] = dt.Rows[i]["MRNo"];
                            //string FName = (string)(dt.Rows[i]["FirstName"]);
                            //string MName = (string)(dt.Rows[i]["MiddleName"]);
                            //string LName = (string)(dt.Rows[i]["LastName"]);
                            //string PName = FName + ' ' + MName + ' ' + LName;
                            dtReturn.Rows[i]["Patient Name"] = (string)(dt.Rows[i]["FirstName"]) + ' ' + (string)(dt.Rows[i]["MiddleName"]) + ' ' + (string)(dt.Rows[i]["LastName"]);
                            dtReturn.Rows[i]["Issue Date"] = ((DateTime)dt.Rows[i]["LoyaltyCardIssueDate"]).ToString("dd-MM-yyyy");
                            dtReturn.Rows[i]["Effective Date"] = ((DateTime)dt.Rows[i]["EffectiveDate"]).ToString("dd-MM-yyyy");
                            dtReturn.Rows[i]["Expiry Date"] = ((DateTime)dt.Rows[i]["ExpiryDate"]).ToString("dd-MM-yyyy");
                        }
                        objExcel.ExportToExcel("", strReportName, strPath, dtReturn);
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Sr. No.");
                        dt.Columns.Add("Clinic Name");
                        dt.Columns.Add("Mr. No.");
                        dt.Columns.Add("Patient Name");
                        dt.Columns.Add("Loyalty Card");
                        dt.Columns.Add("Issue Date");
                        dt.Columns.Add("Effective Date");
                        dt.Columns.Add("Expiry Date");
                        objExcel.ExportToExcel("", strReportName, strPath, dt);
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("Sr. No.");
                    dt.Columns.Add("Clinic Name");
                    dt.Columns.Add("Mr. No.");
                    dt.Columns.Add("Patient Name");
                    dt.Columns.Add("Loyalty Card");
                    dt.Columns.Add("Issue Date");
                    dt.Columns.Add("Effective Date");
                    dt.Columns.Add("Expiry Date");
                    objExcel.ExportToExcel("", strReportName, strPath, dt);
                }
                //Response.Redirect(AppPath + "\\Reports\\CRM\\" + strReportName + ".xls");
                // Response.Redirect(strReportName + ".xls");
                string URL = "/" + System.Configuration.ConfigurationSettings.AppSettings["VirtualDir"].ToString() + "/Reports/CRM/" + strReportName + ".xls";
                Response.Write("<script language='javascript'>window.open('" + URL + "','_self');</script>");

                //}

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