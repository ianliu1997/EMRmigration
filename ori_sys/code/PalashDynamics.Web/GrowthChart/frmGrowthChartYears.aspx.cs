using System;

namespace PalashDynamics.Web.GrowthChart
{
    public partial class frmGrowthChartYears : System.Web.UI.Page
    {
        public string strImage = "~/GrowthChart/Image/gcimage1.gif";

        public string strWeightPerct = "89.89";
        public string strHeightPerct = "0.0";
        public string strBMIPerct = "0.0";
        public string strW_FOR_L = "0.0";

        public string strWeightPerct2 = "89.89";
        public string strHeightPerct2 = "0.0";
        public string strBMIPerct2 = "0.0";
        public string strHCPerct = "0.0";
        public string strW_FOR_L2 = "0.0";

        protected void Page_Load(object sender, EventArgs e)
        {
            strImage = Server.MapPath("~/GrowthChart/Image/gcimage1.bmp");
            int chartType = 0;
            string PatientName = string.Empty;
            int GenderID = 0;

            int PatientID = 0;
            int UnitID = 0;
            int PatientUnitID = 0;
            int DrID = 0;

            string MrNo = string.Empty;
            string Comments = string.Empty;

            if (Request.QueryString["PatName"] != null)
            {
                PatientName = Convert.ToString(Request.QueryString["PatName"]);
            }
            if (Request.QueryString["chartType"] != null)
            {
                chartType = Convert.ToInt32(Request.QueryString["chartType"]);
            }
            if (Request.QueryString["GenderID"] != null)
            {
                GenderID = Convert.ToInt32(Request.QueryString["GenderID"]);
            }
            if (Request.QueryString["MrNo"] != null)
            {
                MrNo = Convert.ToString(Request.QueryString["MrNo"]);
            }
            if (Request.QueryString["PatientID"] != null)
            {
                PatientID = Convert.ToInt32(Request.QueryString["PatientID"]);
            }
            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt32(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["PatientUnitID"] != null)
            {
                PatientUnitID = Convert.ToInt32(Request.QueryString["PatientUnitID"]);
            }
            if (Request.QueryString["DrID"] != null)
            {
                DrID = Convert.ToInt32(Request.QueryString["DrID"]);
            }


            //GROWTHCHARTLib.GrowthChart myChart = new GROWTHCHARTLib.GrowthChart();
            //myChart.GrowthChartType = chartType;
            //myChart.FirstName = PatientName;
            //myChart.LastName = "";
            //myChart.Gender = GenderID;
            //myChart.RecordIDStr = MrNo;

            //clsGetPatientGrowthChartYearlyBizActionVO objGrowthChartYearly = new clsGetPatientGrowthChartYearlyBizActionVO();
            //objGrowthChartYearly.GrowthChartDetailList = new List<clsGrowthChartVO>();
            //objGrowthChartYearly.PatientID = PatientID;
            //objGrowthChartYearly.UnitID = UnitID;
            //objGrowthChartYearly.PatientUnitID = PatientUnitID;
            //objGrowthChartYearly.DrID = DrID;

            //PalashDynamicsWeb service = new PalashDynamicsWeb();
            //objGrowthChartYearly = (clsGetPatientGrowthChartYearlyBizActionVO)service.Process(objGrowthChartYearly, new clsUserVO());
            //{
            //    int index = 0;
            //    string str = Comments;
            //    foreach (clsGrowthChartVO item in objGrowthChartYearly.GrowthChartDetailList)
            //    {
            //        index = myChart.AddNewData();
            //        myChart.SetAge(index, item.AgeInMonth);
            //        myChart.SetWeight(index, (float)item.Weight);
            //        myChart.SetHeight(index, (float)item.Height);
            //        myChart.SetHeadCir(index, (float)item.HC);
            //        myChart.SetTestDate(index, item.VisitDate);
            //        myChart.SetComments(index, str);
            //    }
            //}

            //string strFilename;
            //DateTime dtCurr = DateTime.Now;
            //strFilename = dtCurr.ToString("MMddyyyyhhmmss") + ".bmp";
            //string szFile = HttpContext.Current.Request.PhysicalApplicationPath + "GrowthChart\\Image\\" + strFilename;
            //strImage = "..\\GrowthChart\\Image\\" + strFilename;
            //myChart.SaveChartToBMP(szFile);
        }
    }
}