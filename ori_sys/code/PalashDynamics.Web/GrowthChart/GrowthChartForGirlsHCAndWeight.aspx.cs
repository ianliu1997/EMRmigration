using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR.GrowthChart;

namespace PalashDynamics.Web.GrowthChart
{
    public partial class GrowthChartForGirlsHCAndWeight : System.Web.UI.Page
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
                bool IsopdIpd = false;

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

                Bitmap bmp = new Bitmap(720, 950);
                Graphics g = Graphics.FromImage(bmp);

                String path1 = Server.MapPath("~/GrowthChart/Image/HCandWeight0TO24MonthsGirls.bmp");
                System.Drawing.Image img = System.Drawing.Image.FromFile(path1);

                g.Clear(Color.White);

                g.DrawImage(img, 10, 10);



                Pen drawingPen = new Pen(Color.FromArgb(180, Color.Black), 3);

                Pen drawingPen1 = new Pen(Color.FromArgb(180, Color.Blue), 3);

                Rectangle rect = new Rectangle(107 + 28, 878 - 28, 1, 1);


                g.DrawString(PatientName, new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                            new Rectangle(478 - 4, 45 - 2 + 5 + 2 + 1, 500, 150));
                g.DrawString(MrNo, new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                           new Rectangle(590 - 4, 66 - 2 + 5 + 1, 500, 150));




                clsGetPatientGrowthChartMonthlyBizActionVO objGrowthChartMonthly = new clsGetPatientGrowthChartMonthlyBizActionVO();
                objGrowthChartMonthly.GrowthChartDetailList = new List<clsGrowthChartVO>();
                objGrowthChartMonthly.PatientID = PatientID;
                objGrowthChartMonthly.UnitID = UnitID;
                objGrowthChartMonthly.PatientUnitID = PatientUnitID;
                objGrowthChartMonthly.DrID = DrID;
                objGrowthChartMonthly.IsOPDIPD = IsopdIpd;
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                objGrowthChartMonthly = (clsGetPatientGrowthChartMonthlyBizActionVO)service.Process(objGrowthChartMonthly, new clsUserVO());
                {
                    int n1 = 782 - 2 + 22;
                    foreach (clsGrowthChartVO item in objGrowthChartMonthly.GrowthChartDetailList)
                    {
                        n1 = n1 + 12;
                        g.DrawString(Convert.ToString(item.VisitDate.ToShortDateString()), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                  new Rectangle(50 + 230 - 5, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.AgeInMonth), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                  new Rectangle(130 + 220 - 10, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.Weight), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                  new Rectangle(188 + 210 - 10, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.Height), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Blue,
                                  new Rectangle(260 + 190 - 10, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.HC), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                  new Rectangle(315 + 200 - 10, n1, 80, 50));
                        g.DrawString(Comments, new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                new Rectangle(315 + 237 - 10 - 6, n1, 300, 250));

                    }


                    double HC = 0.0; double age = 0.0; double Length = 0.0; double Weight = 0.0;

                    foreach (clsGrowthChartVO item in objGrowthChartMonthly.GrowthChartDetailList)
                    {
                        HC = Convert.ToDouble(item.HC);
                        age = Convert.ToDouble(item.AgeInMonth);
                        Length = Convert.ToDouble(item.Height);
                        Weight = Convert.ToDouble(item.Weight);


                        int x_Length = 0;
                        int y_Weight = 0;

                        int x_Age = 0;
                        int y_HC = 0;

                        # region x - Length axis
                        if (Length >= 46)
                        {
                            x_Length = Convert.ToInt32((Length - 46) * 7) + 7;
                        }
                        # endregion

                        # region Y - Weight axis
                        if (Weight > 0)
                        {
                            y_Weight = Convert.ToInt32(Weight * 21);
                        }
                        #endregion


                        if (Weight < 0 || Weight > 24 || Length < 45.5 || Length > 110)
                        {
                            //g.DrawString("Given Weight or Length is not ful-filling graph required range", new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                            //    new Rectangle(130, 907, 600, 650));
                        }
                        else
                        {
                            Rectangle rect2 = new Rectangle(138 - 1 - 1 + x_Length, 836 - y_Weight, 2, 2);
                            g.DrawEllipse(drawingPen1, rect2);
                        }

                        # region x - Age aixs
                        if (age >= 0 && age <= 6)
                        {
                            x_Age = Convert.ToInt32(age * 18) + Convert.ToInt32(age) - 1;
                        }
                        else if (age > 6 && age <= 10)
                        {
                            x_Age = Convert.ToInt32(age * 18) + Convert.ToInt32(age) - 2;
                        }
                        else if (age > 10 && age <= 16)
                        {
                            x_Age = Convert.ToInt32(age * 18) + Convert.ToInt32(age) - 3;
                        }
                        else if (age > 16 && age <= 20)
                        {
                            x_Age = Convert.ToInt32(age * 18) + Convert.ToInt32(age) - 4;
                        }
                        else
                        {
                            x_Age = Convert.ToInt32(age * 18) + Convert.ToInt32(age) - 5;
                        }
                        # endregion

                        # region Y- HC axis
                        if (HC >= 29 && HC <= 30)
                        {
                            y_HC = Convert.ToInt32((HC - 29) * 16) - 1;
                        }
                        else if (HC == 31)
                        {
                            y_HC = Convert.ToInt32((HC - 29) * 16);
                        }
                        else if (HC >= 32)
                        {
                            y_HC = Convert.ToInt32((HC - 29) * 16) + 2;
                        }
                        # endregion


                        if (HC < 29 || HC > 54)
                        {
                            //g.DrawString("Given HC is not ful-filling graph required range", new Font(new FontFamily("Verdana"), 9, FontStyle.Bold), Brushes.Black,
                            //    new Rectangle(130, 884, 600, 650));
                        }
                        else
                        {
                            Rectangle rect3 = new Rectangle(139 - 1 + x_Age, 549 - y_HC, 2, 2);
                            g.DrawEllipse(drawingPen, rect3);
                        }


                    }
                }

                string imageName = "DrawTransparentEllipse" + DateTime.Now.ToString("dd_MM_yy") + DateTime.Now.ToString("hh_mm_ss");
                String path = Server.MapPath("~/GrowthChart/Image/Temp/" + imageName + ".jpg");
                bmp.Save(path, ImageFormat.Jpeg);
                Image1.ImageUrl = "~/GrowthChart/Image/Temp/" + imageName + ".jpg";
                g.Dispose();
                bmp.Dispose();
            }
            catch(Exception )
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