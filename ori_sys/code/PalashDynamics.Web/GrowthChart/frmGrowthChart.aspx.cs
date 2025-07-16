using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Text;

namespace PalashDynamics.Web.GrowthChart
{
    public partial class frmGrowthChart : System.Web.UI.Page
    {
        //public string strImage = "..\\images\\gcimage1.bmp";
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
            int AgeInMonth = 0;
           string MrNo = string.Empty;
           string Comments = string.Empty;
            
            DateTime visitDate = DateTime.Now;
            Double HC, Height, Weight;
            HC = Height = Weight = 0;
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

            if (Request.QueryString["AgeInMonth"] != null)
            {
                AgeInMonth = Convert.ToInt32(Request.QueryString["AgeInMonth"]);
            }
            if (Request.QueryString["VisitDate"] != null)
            {
                visitDate = Convert.ToDateTime(Request.QueryString["VisitDate"]);
            }

            if (Request.QueryString["Height"] != null)
            {
                Height = Convert.ToDouble(Request.QueryString["Height"]);
            }
            if (Request.QueryString["Weight"] != null)
            {
                Weight = Convert.ToDouble(Request.QueryString["Weight"]);
            }
            if (Request.QueryString["HC"] != null)
            {
                HC = Convert.ToDouble(Request.QueryString["HC"]);
            }

            if (Request.QueryString["MrNo"] != null)
            {
                MrNo =  Convert.ToString(Request.QueryString["MrNo"]);
            }
            if (Request.QueryString["Comments"] != null)
            {
                Comments = Convert.ToString(Request.QueryString["Comments"]);
            }           

            //this.Title = "21";
            ////////////////////////////GROWTHCHARTLib.GrowthChart myChart = new GROWTHCHARTLib.GrowthChart();
            ////////////////////////////myChart.GrowthChartType = chartType;
            ////////////////////////////myChart.FirstName = PatientName;
            ////////////////////////////myChart.LastName = "";
            ////////////////////////////myChart.Gender = GenderID;
            //////////////////////////////if (RecordID > 0)
            //////////////////////////////    myChart.RecordID = RecordID;

            ////////////////////////////myChart.RecordIDStr = MrNo;

            ////////////////////////////int index = myChart.AddNewData();
            ////////////////////////////string str = Comments;
            ////////////////////////////myChart.SetAge(index, AgeInMonth);
            ////////////////////////////myChart.SetWeight(index, (float)Weight);
            ////////////////////////////myChart.SetHeight(index, (float)Height);
            ////////////////////////////myChart.SetHeadCir(index, (float)HC);
            ////////////////////////////myChart.SetTestDate(index, visitDate);
            ////////////////////////////myChart.SetComments(index, str);

            //index = myChart.AddNewData();
            //str = "Test Data 2";
            //myChart.SetAge(index, 36);
            //myChart.SetWeight(index, (float)16.34);
            //myChart.SetHeight(index, (float)99.06);
            //myChart.SetTestDate(index, new DateTime(2001, 1, 1));
            //myChart.SetComments(index, str);
            //float fPerct = myChart.GetWeightPercentileForAge(index);
            //strWeightPerct = "Weight Percentile for Test Data 2 = " + fPerct.ToString("0.00");
            //fPerct = myChart.GetHeightPercentileForAge(index);
            //strHeightPerct = "Height Percentile for Test Data 2 = " + fPerct.ToString("0.00");
            //fPerct = myChart.GetBMIPercentileForAge(index);
            //strBMIPerct = "BMI Percentile for Test Data 2 = " + fPerct.ToString("0.00");
            ////weight for len
            //fPerct = myChart.GetWeightPercentileForHeight(index);
            //strW_FOR_L = "Weight for length Percentile for Test Data 2 = " + fPerct.ToString("0.00");

            //index = myChart.AddNewData();
            //str = "Test Data 3";
            //myChart.SetAge(index, 48);
            //myChart.SetWeight(index, (float)17.25);
            //myChart.SetHeight(index, (float)105.41);
            //myChart.SetTestDate(index, new DateTime(2002, 1, 1));
            //myChart.SetComments(index, str);

            //index = myChart.AddNewData();
            //str = "Test Data 4";
            //myChart.SetAge(index, 60);
            //myChart.SetWeight(index, (float)20.88);
            //myChart.SetHeight(index, (float)111.76);
            //myChart.SetTestDate(index, new DateTime(2003, 1, 1));
            //myChart.SetComments(index, str);

            //index = myChart.AddNewData();
            //str = "Test Data 5";
            //myChart.SetAge(index, 72);
            //myChart.SetWeight(index, (float)23.61);
            //myChart.SetHeight(index, (float)119.38);
            //myChart.SetTestDate(index, new DateTime(2004, 1, 1));
            //myChart.SetComments(index, str);

            //index = myChart.AddNewData();
            //str = "Test Data 6";
            //myChart.SetAge(index, 84);
            //myChart.SetWeight(index, (float)26.33);
            //myChart.SetHeight(index, (float)125.73);
            //myChart.SetTestDate(index, new DateTime(2005, 1, 1));
            //myChart.SetComments(index, str);

            //index = myChart.AddNewData();
            //str = "Test Data 7";
            //myChart.SetAge(index, 96);
            //myChart.SetWeight(index, (float)30.87);
            //myChart.SetHeight(index, (float)134.62);
            //myChart.SetTestDate(index, new DateTime(2006, 1, 1));
            //myChart.SetComments(index, str);

            //float fMonth = (float)83.50;
            //float fWeight = (float)28.34; //kg
            //float fHeight = (float)128.67; //cm
            //float fHC = (float)42.67; //cm
            //strWeightPerct2 = "Weight Percentile for " + fMonth.ToString("0.00") + " Months and " + fWeight.ToString("0.00")
            //    + "kg = " + myChart.GetWeightPercentileForAge2(fMonth, fWeight).ToString("0.00");
            //strHeightPerct2 = "Height Percentile for " + fMonth.ToString("0.00") + " Months and " + fHeight.ToString("0.00")
            //    + "cm Height  = " + myChart.GetHeightPercentileForAge2(fMonth, fHeight).ToString("0.00");
            //float fBMI = myChart.GetBMI(fWeight, fHeight);
            //strBMIPerct2 = "BMI Percentile for " + fMonth.ToString("0.00") + " Months and " + fBMI.ToString("0.00")
            //    + "kg/cm2  = " + myChart.GetBMIPercentileForAge2(fMonth, fBMI).ToString("0.00");

            //fMonth = (float)6.50;
            //fWeight = (float)9.34; //kg
            //fHeight = (float)72.67; //cm

            //strHCPerct = "Head Circumference Percentile for " + fMonth.ToString("0.00") + " Months and " + fHC.ToString("0.00")
            //    + "cm = " + myChart.GetHeadCirPercentileForAge2(fMonth, fHC).ToString("0.00");

            //strW_FOR_L2 = "Weight fro length Percentile for " + fMonth.ToString("0.00") + " Months and " + fWeight.ToString("0.00")
            //    + "kg, Height " + fHeight.ToString("0.00")
            //    + "cm Height  =" + myChart.GetWeightPercentileForHeight2(fMonth, fWeight, fHeight).ToString("0.00");
            ////////////////////string strFilename;
            ////////////////////DateTime dtCurr = DateTime.Now;
            ////////////////////strFilename = dtCurr.ToString("MMddyyyyhhmmss") + ".bmp";
            //////////////////////string szFile = HttpContext.Current.Request.PhysicalApplicationPath + "images\\" + strFilename;
            ////////////////////string szFile = HttpContext.Current.Request.PhysicalApplicationPath + "GrowthChart\\Image\\" + strFilename;
            //////////////////////strImage = Server.MapPath("~/GrowthChart/Image") + "\\" + strFilename;
            ////////////////////strImage = "..\\GrowthChart\\Image\\" + strFilename;
            ////////////////////myChart.SaveChartToBMP(szFile);

            #region Old Code
            ////Bitmap bmp = new Bitmap(895, 687);
            //Bitmap bmp = new Bitmap(895, 750);
            //Graphics g = Graphics.FromImage(bmp);

            //String path1 = Server.MapPath("~/GrowthChart/Image/boysbmi.gif");
            //System.Drawing.Image img = System.Drawing.Image.FromFile(path1);

            //g.Clear(Color.White);
            //g.DrawImage(img, 10, 10);

            //Pen drawingPen = new Pen(Color.FromArgb(70, Color.Red), 6);
            ////Rectangle rect = new Rectangle(100, 35, 300, 200);
            //Rectangle rect = new Rectangle(300, 35, 5, 5);
            //Rectangle rect2 = new Rectangle(300, 565, 5, 5);
            //Rectangle rect3 = new Rectangle(350, 575, 1, 0);
            ////RectangleF rect1 = new RectangleF(300, 575, 5, 5);
            ////RectangleF rect2= new RectangleF(300, 575, 0.5,0.5);
            ////g.DrawEllipse(drawingPen, rect);
            //g.DrawEllipse(drawingPen, rect2);
            //g.DrawEllipse(drawingPen, rect3);
            //String path = Server.MapPath("~/GrowthChart/Image/DrawTransparentEllipse.jpg");
            //bmp.Save(path, ImageFormat.Jpeg);
            //Image1.ImageUrl = "~/GrowthChart/Image/DrawTransparentEllipse.jpg";
            //g.Dispose();
            //bmp.Dispose();
            #endregion

        }
    }
}