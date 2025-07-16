using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashboardMIS;

namespace PalashDynamics.Web.Reports.Dashboard
{
    public partial class ReferralReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack == false)
                FillUnits();


        }
        private void FillUnits()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            try
            {
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                BizAction = (clsGetMasterListBizActionVO)service.Process(BizAction, new clsUserVO());
                ddlUnits.DataSource = null;
                if (BizAction.MasterList != null)
                {
                    ddlUnits.DataSource = BizAction.MasterList;
                    ddlUnits.DataTextField = "Description";
                    ddlUnits.DataValueField = "ID";
                    ddlUnits.DataBind();

                    //if (FillUnitFlag == false)
                    //{
                    //    //ddlUnits.SelectedValue = BizAction.MasterList[0].Description;
                    //    var result = from r in ((List<MasterListItem>)ddlUnits.DataSource)
                    //                 where r.Description == defaultUnit
                    //                 select r;

                    //    if (result != null)
                    //    {
                    //        ddlUnits.SelectedValue = ((MasterListItem)result.First()).ID.ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    var result = from r in ((List<MasterListItem>)ddlUnits.DataSource)
                    //                 where r.Description == defaultUnit
                    //                 select r;

                    //    if (result != null)
                    //    {
                    //        ddlUnits.SelectedValue = ((MasterListItem)result.First()).ID.ToString();

                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Response.Write(BizAction.Error);
                // throw ex;                            
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            GetReport();
        }

        protected void ddlUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetReport();
        }

        private void GetReport()
        {

            cls_DashboardMIS_ReferralReportBizActionVO BizAction = new cls_DashboardMIS_ReferralReportBizActionVO();

            try
            {
               
                BizAction.Details.UnitID = Convert.ToInt64(ddlUnits.SelectedValue);
                BizAction.ReferralReportList = new List<cls_DashboardMIS_ReferralReportListVO>();
                //BizAction.Details
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                BizAction = (cls_DashboardMIS_ReferralReportBizActionVO)service.Process(BizAction, new clsUserVO());
                if (BizAction.ReferralReportList != null)
                {                   
                    grdView.DataSource = BizAction.ReferralReportList;
                    grdView.DataBind();
                }
            }
            catch (Exception ex)
            {
               // Response.Write(BizAction.Error);
                // throw ex;                            
            }
        }

    }
}