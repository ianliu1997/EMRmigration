using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR.GrowthChart;

namespace PalashDynamics.Web.GrowthChart
{
    public partial class GrowthChartForBoysBMI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PatientName = string.Empty;
                int GenderID = 0;
                int PatientID = 0;
                int UnitID = 0;
                int PatientUnitID = 0;
                int DrID = 0;
                int CurrentVisit = 0;
                Boolean IsopdIpd = false;

                string MrNo = string.Empty;
                string Comments = string.Empty;

                if (Request.QueryString["PatName"] != null)
                {
                    PatientName = Convert.ToString(Request.QueryString["PatName"]);
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

                if (Request.QueryString["CurrentVisit"] != null)
                {
                    CurrentVisit = Convert.ToInt32(Request.QueryString["CurrentVisit"]);
                }

                if (Request.QueryString["Comments"] != null)
                {
                    Comments = Convert.ToString(Request.QueryString["Comments"]);
                }
                if (Request.QueryString["IsopdIpd"] == "1")
                {
                    IsopdIpd = true;
                }
                else
                {
                    IsopdIpd = false;
                }
                clsGetPatientGrowthChartYearlyBizActionVO objGrowthChartYearly = new clsGetPatientGrowthChartYearlyBizActionVO();
                objGrowthChartYearly.GrowthChartDetailList = new List<clsGrowthChartVO>();
                objGrowthChartYearly.PatientID = PatientID;
                objGrowthChartYearly.UnitID = UnitID;
                objGrowthChartYearly.PatientUnitID = PatientUnitID;
                objGrowthChartYearly.DrID = DrID;
                objGrowthChartYearly.IsopdIPd = IsopdIpd;

                double BMI = 0.0; double age = 0.0;
                Bitmap bmp = new Bitmap(720, 950);
                Graphics g = Graphics.FromImage(bmp);

                String path1 = Server.MapPath("~/GrowthChart/Image/BMI2TO20Boys.bmp");
                System.Drawing.Image img = System.Drawing.Image.FromFile(path1);

                g.Clear(Color.White);

                g.DrawImage(img, 10, 10);

                Pen drawingPen = new Pen(Color.FromArgb(180, Color.Black), 3);

                Rectangle rect = new Rectangle(107 + 28, 878 - 28, 1, 1);

                g.DrawString(PatientName, new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                                new Rectangle(477, 94, 500, 100));
                g.DrawString(MrNo, new Font(new FontFamily("Verdana"), 6, FontStyle.Bold), Brushes.Black,
                           new Rectangle(600, 113, 500, 100));

                int x = 0;
                int y = 0;

                int n1 = 151;
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                objGrowthChartYearly = (clsGetPatientGrowthChartYearlyBizActionVO)service.Process(objGrowthChartYearly, new clsUserVO());
                {
                    foreach (clsGrowthChartVO item in objGrowthChartYearly.GrowthChartDetailList)
                    {
                        n1 = n1 + 12;
                        g.DrawString(Convert.ToString(item.VisitDate.ToShortDateString()), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(65, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.Age), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(130, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.Weight), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(169, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.Height), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(220, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.BMI), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(285, n1, 80, 50));
                        g.DrawString(Comments, new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(316, n1, 140, 50));
                    }

                    foreach (clsGrowthChartVO item in objGrowthChartYearly.GrowthChartDetailList)
                    {
                        BMI = Convert.ToDouble(item.BMI);
                        age = Convert.ToDouble(item.Age);

                        # region x axis
                        if (age <= 2)
                        {
                            x = -2;
                        }
                        else if (age > 2 && age <= 5)
                        {
                            x = Convert.ToInt32(age * 28 - 56);
                        }
                        else if (age >= 5.1 && age <= 7.4)
                        {
                            x = Convert.ToInt32((age * 27.7) - 51);
                        }
                        else if (age >= 7.5 && age <= 8.9)
                        {
                            x = Convert.ToInt32((age * 27.9) - 50);
                        }
                        else if (age >= 9 && age <= 11.4)
                        {
                            x = Convert.ToInt32((age * 28.3) - 51);
                        }
                        else if (age >= 11.5 && age <= 12.9)
                        {
                            x = Convert.ToInt32((age * 28.3) - 50);
                        }
                        else if (age >= 13 && age < 14.9)
                        {
                            x = Convert.ToInt32((age * 28.4) - 49);
                        }
                        else if (age >= 14.9 && age < 15.9)
                        {
                            x = Convert.ToInt32((age * 28.5) - 50);
                        }
                        else if (age >= 15.9 && age < 17.9)
                        {
                            x = Convert.ToInt32((age * 28.5) - 49);
                        }
                        else if (age >= 17.9 && age < 20)
                        {
                            x = Convert.ToInt32((age * 28.5) - 48);
                        }
                        else
                        {
                            x = Convert.ToInt32((age * 28.6) - 47);
                        }
                        #endregion

                        # region y axis
                        if (BMI < 10)
                        {
                            y = 0;
                        }
                        else
                        {
                            int u = 0;
                            int v = 0;
                            string a = BMI.ToString();
                            string[] b = a.Split('.');
                            int firstValue = int.Parse(b[0]);
                            Int64 secondValue = 0;
                            if (a.Contains('.') == true)
                            {
                                secondValue = Int64.Parse(b[1]);
                            }
                            else
                                secondValue = 0;
                            string t1;
                            t1 = 0 + "." + secondValue;
                            double d1 = 0;
                            d1 = Convert.ToDouble(t1);
                            u = Convert.ToInt32(firstValue % 10);
                            v = Convert.ToInt32(firstValue / 10);
                            if (v == 1)
                            {
                                y = Convert.ToInt32((u * 27.5 + d1 * 27.5));
                            }
                            else if (v == 2)
                            {
                                y = Convert.ToInt32(((u + 10) * 27.5 + d1 * 27.5) - 2);
                            }
                            else if (firstValue > 35)
                            {
                                y = Convert.ToInt32(((u + 20) * 27.5 + d1 * 27.5) - 6);
                            }
                            else
                            {
                                y = Convert.ToInt32(((u + 20) * 27.5 + d1 * 27.5) - 4);
                            }
                        }
                        #endregion

                        if (BMI <= 9.9 || BMI > 37)
                        {
                            //g.DrawString("Given BMI is not matching graph required range", new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                            //  new Rectangle(130, 907, 600, 650));
                        }
                        else
                        {
                            Rectangle rect2 = new Rectangle(107 + 1 + x, 878 - y, 3, 3);
                            g.DrawEllipse(drawingPen, rect2);
                        }
                    }

                    string imageName = "DrawTransparentEllipse" + DateTime.Now.ToString("dd_MM_yy") + DateTime.Now.ToString("hh_mm_ss");
                    //String path = Server.MapPath("~/Image/DrawTransparentEllipse.jpg");
                    String path = Server.MapPath("~/GrowthChart/Image/Temp/" + imageName + ".jpg");
                    bmp.Save(path, ImageFormat.Jpeg);
                    //Image1.ImageUrl = "~/Image/DrawTransparentEllipse.jpg";
                    Image1.ImageUrl = "~/GrowthChart/Image/Temp/" + imageName + ".jpg";
                    g.Dispose();
                    bmp.Dispose();
                }
            }
            catch(Exception)
            {
               
            }
        }
     

     
       
        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                string[] filesPath = Directory.GetFiles(Server.MapPath("~/GrowthChart/Image/Temp/"));
                foreach (string path in filesPath)
                {
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                    {
                        try
                        {
                            fi.Delete();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

       

    }
}