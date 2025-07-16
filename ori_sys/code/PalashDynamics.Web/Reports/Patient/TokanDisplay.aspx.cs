using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using PalashDynamics.ValueObjects.TokenDisplay;
using PalashDynamics.ValueObjects;
//using CIMS;
using PalashDynamics.BusinessLayer.TokenDisplay;
using PalashDynamics.BusinessLayer.OutPatientDepartment.QueueManagement;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services;
namespace PalashDynamics.Web.Reports.Patient
{
    public partial class TokanDisplay : System.Web.UI.Page
    {
        string conStr = string.Empty;
        SqlConnection con = null;
        SqlDataAdapter daTokenDisplay = null;
        DataSet dsToken = null;
        SqlCommand cmd = null;
        long UnitID { get; set; }
        string dformat;
        protected void Page_Load(object sender, EventArgs e)
        {
            conStr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            con = new SqlConnection(conStr);
            dsToken = new DataSet();
            if (Request.QueryString["UnitID"] != null)
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            if (Request.QueryString["dformat"] != null)
                dformat = Convert.ToString(Request.QueryString["dformat"]);
            BindGrid();
        }
        private void BindGrid()
        {
            try
            {
                DateTime VisitDate = DateTime.Now;
                dateshow.Text = DateTime.Now.ToString(dformat);
                cmd = new SqlCommand("CIMS_GetTokenDisplayDetails", con);//Cabin
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UnitID", UnitID); 
                cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
                daTokenDisplay = new SqlDataAdapter(cmd);
                daTokenDisplay.Fill(dsToken);
                grdTokenPatientlist.DataSource = null;
                if (dsToken != null && dsToken.Tables[0].Rows.Count > 0)
                {
                    dsToken.Tables[0].Rows[0][1] = dsToken.Tables[0].Rows[0][1];
                    grdTokenPatientlist.DataSource = dsToken;
                    grdTokenPatientlist.DataBind();

                    lblMessage.Visible = false;
                    grdTokenPatientlist.Visible = true;
                    HospitalName.Text = Convert.ToString(dsToken.Tables[0].Rows[0][10]);
                   
                }
                else
                {
                    lblMessage.Text = "No record found.";
                    lblMessage.Visible = true;
                    grdTokenPatientlist.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { }
        }
        protected void grdTokenPatientlist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.ID = "Row_" + e.Row.RowIndex.ToString();
                e.Row.Attributes.Add("blinkingRow", "Y");
            }

        }    
       
      }    
}