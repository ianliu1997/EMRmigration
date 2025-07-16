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
    public partial class GrowthChartForBoysLengthAndWeight : System.Web.UI.Page
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
                if (Request.QueryString["IsopdIpd"] == "1")
                {
                    IsopdIpd = true;
                }
                else
                {
                    IsopdIpd = false;
                }

                if (Request.QueryString["Comments"] != null)
                {
                    Comments = Convert.ToString(Request.QueryString["Comments"]);
                }
                Bitmap bmp = new Bitmap(720, 950);
                Graphics g = Graphics.FromImage(bmp);

                String path1 = Server.MapPath("~/GrowthChart/Image/LengthandWeight0To24Boys.bmp");
                System.Drawing.Image img = System.Drawing.Image.FromFile(path1);

                g.Clear(Color.White);

                g.DrawImage(img, 10, 10);



                Pen drawingPen = new Pen(Color.FromArgb(180, Color.Black), 3);

                Pen drawingPen1 = new Pen(Color.FromArgb(180, Color.Brown), 3);

                g.DrawString(PatientName, new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                                   new Rectangle(478, 45, 500, 150));
                g.DrawString(MrNo, new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                           new Rectangle(590, 66, 500, 150));


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
                    int n1 = 782;

                    foreach (clsGrowthChartVO item in objGrowthChartMonthly.GrowthChartDetailList)
                    {
                        n1 = n1 + 12;
                        g.DrawString(Convert.ToString(item.VisitDate.ToShortDateString()), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                  new Rectangle(50 + 230, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.AgeInMonth), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                  new Rectangle(130 + 220, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.Weight), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                  new Rectangle(188 + 210, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.Height), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Brown,
                                  new Rectangle(260 + 190, n1, 80, 50));
                        g.DrawString(Convert.ToString(item.HC), new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                  new Rectangle(315 + 200, n1, 80, 50));
                        g.DrawString(Comments, new Font(new FontFamily("Verdana"), 5, FontStyle.Bold), Brushes.Black,
                                new Rectangle(315 + 237, n1, 300, 250));
                    }



                    double BMI = 0.0; double age = 0.0; double Stature = 0.0; double Weight = 0.0;
                    foreach (clsGrowthChartVO item in objGrowthChartMonthly.GrowthChartDetailList)
                    {
                        BMI = Convert.ToDouble(item.BMI);
                        age = Convert.ToDouble(item.AgeInMonth);
                        Stature = Convert.ToDouble(item.Height);
                        Weight = Convert.ToDouble(item.Weight);

                        //if (Request.QueryString["BMI"] != null)
                        //{
                        //    BMI = Convert.ToDouble(Request.QueryString["BMI"]);
                        //}
                        //if (Request.QueryString["Age"] != null)
                        //{
                        //    age = Convert.ToDouble(Request.QueryString["Age"]);
                        //}
                        //if (Request.QueryString["Stature"] != null)
                        //{
                        //    Stature = Convert.ToDouble(Request.QueryString["Stature"]);
                        //}
                        //if (Request.QueryString["Weight"] != null)
                        //{
                        //    Weight = Convert.ToDouble(Request.QueryString["Weight"]);
                        //}
                        // n means number of records

                        int x = 0;
                        int y_Stature = 0;
                        int y_Weight = 0;

                        # region x axis
                        if (age > 0 && age <= 2)
                        {
                            x = Convert.ToInt32(age * 19);
                        }
                        else if (age > 2 && age <= 8)
                        {
                            x = Convert.ToInt32(age * 19) + 1;
                        }
                        else if (age > 8 && age <= 15)
                        {
                            x = Convert.ToInt32(age * 19) + 2;
                        }
                        else if (age > 15)
                        {
                            x = Convert.ToInt32(age * 19) + 3;
                        }


                        #endregion

                        # region y- for Stature axis
                        if (Stature < 35)
                        {
                            y_Stature = 0;
                        }
                        else
                        {
                            int q = 0;
                            q = Convert.ToInt32(Stature) - 35;
                            if (Stature > 35 && Stature <= 39)
                            {
                                y_Stature = 7 * q;
                            }
                            else if (Stature > 39 && Stature <= 49)
                            {
                                y_Stature = 7 * q + 1;
                            }
                            else if (Stature > 49 && Stature <= 53)
                            {
                                y_Stature = 7 * q + 2;
                            }
                            else if (Stature > 53 && Stature <= 59)
                            {
                                y_Stature = 7 * q + 3;
                            }
                            else if (Stature > 59 && Stature <= 63)
                            {
                                y_Stature = 7 * q + 4;
                            }
                            else if (Stature > 63 && Stature <= 69)
                            {
                                y_Stature = 7 * q + 5;
                            }
                            else if (Stature > 69 && Stature <= 74)
                            {
                                y_Stature = 7 * q + 6;
                            }
                            else if (Stature > 74 && Stature <= 79)
                            {
                                y_Stature = 7 * q + 7;
                            }
                            else if (Stature > 79 && Stature <= 84)
                            {
                                y_Stature = 7 * q + 8;
                            }
                            else if (Stature > 84 && Stature <= 89)
                            {
                                y_Stature = 7 * q + 9;
                            }
                            else if (Stature > 89 && Stature <= 94)
                            {
                                y_Stature = 7 * q + 10;
                            }
                            else if (Stature > 94 && Stature <= 99)
                            {
                                y_Stature = 7 * q + 11;
                            }
                            else if (Stature > 99 && Stature <= 105)
                            {
                                y_Stature = 7 * q + 12;
                            }
                            else
                            {
                                y_Stature = 7 * q;
                            }

                        }

                        #endregion

                        # region y - for Weight  axis
                        if (Weight < 2)
                        {
                            if (Weight <= 1.4)
                            {
                                y_Weight = 0;
                            }
                            else if (Weight == 1.5)
                            {
                                y_Weight = 3;
                            }
                            else if (Weight == 1.6)
                            {
                                y_Weight = 7;
                            }
                            else if (Weight == 1.7)
                            {
                                y_Weight = 11;
                            }
                            else if (Weight == 1.8)
                            {
                                y_Weight = 14;
                            }
                            else if (Weight == 1.9)
                            {
                                y_Weight = 17;
                            }
                        }
                        else
                        {
                            int u = 0;
                            int v = 0;
                            string a = Weight.ToString();
                            string[] b = a.Split('.');
                            int firstValue = int.Parse(b[0]);
                            int secondValue = 0;
                            if (a.Contains('.') == true)
                            {
                                secondValue = int.Parse(b[1]);
                            }
                            else
                                secondValue = 0;
                            double d1 = 0;
                            if (secondValue == 1)
                            {
                                d1 = 3.5;
                            }
                            else if (secondValue == 2)
                            {
                                d1 = 7;
                            }
                            else if (secondValue == 3)
                            {
                                d1 = 10.5;
                            }
                            else if (secondValue == 4)
                            {
                                d1 = 14;
                            }
                            else if (secondValue == 5)
                            {
                                d1 = 17.5;
                            }
                            else if (secondValue == 6)
                            {
                                d1 = 21;
                            }
                            else if (secondValue == 7)
                            {
                                d1 = 24.5;
                            }
                            else if (secondValue == 8)
                            {
                                d1 = 28;
                            }
                            else if (secondValue == 9)
                            {
                                d1 = 31.5;
                            }
                            else
                            {
                                d1 = 0;
                            }

                            u = Convert.ToInt32(firstValue % 10) - 2;
                            v = Convert.ToInt32(firstValue / 10);

                            if (Weight > 2 && Weight <= 4.6)
                            {
                                y_Weight = Convert.ToInt32((u * 35 + d1));
                            }
                            else if (Weight > 4.6 && Weight <= 5.6)
                            {
                                y_Weight = Convert.ToInt32((u * 35 + d1)) + 2;
                            }
                            else if (Weight > 5.6 && Weight <= 7.4)
                            {
                                y_Weight = Convert.ToInt32((u * 35 + d1)) + 4;
                            }
                            else if (Weight > 7.4 && Weight <= 8.2)
                            {
                                y_Weight = Convert.ToInt32((u * 35 + d1)) + 5;
                            }
                            else if (Weight > 8.2 && Weight <= 9.4)
                            {
                                y_Weight = Convert.ToInt32((u * 35 + d1)) + 6;
                            }
                            else if (Weight > 9.4 && Weight <= 9.9)
                            {
                                y_Weight = Convert.ToInt32((u * 35 + d1)) + 7;
                            }
                            else
                                if (v == 1)
                                {
                                    if (Weight > 10 && Weight <= 10.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 7;
                                    }
                                    else if (Weight > 10.4 && Weight <= 11.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 8;
                                    }
                                    else if (Weight > 11.4 && Weight <= 12.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 9;
                                    }
                                    else if (Weight > 12.4 && Weight <= 13.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 10;
                                    }
                                    else if (Weight > 13.4 && Weight <= 14.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 11;
                                    }
                                    else if (Weight > 14.4 && Weight <= 15.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 12;
                                    }
                                    else if (Weight > 15.4 && Weight <= 16.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 13;
                                    }
                                    else if (Weight > 16.4 && Weight <= 17.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 14;
                                    }
                                    else if (Weight > 17.4 && Weight <= 18.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 15;
                                    }
                                    else if (Weight > 18.4 && Weight <= 19.4)
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 16;
                                    }
                                    else
                                    {
                                        y_Weight = Convert.ToInt32(((u + 10) * 35 + d1)) + 17;
                                    }
                                }


                        }
                        #endregion

                        if (Stature < 35 || Stature > 103)
                        {
                            //g.DrawString("Given Lenght is not ful filling graph requirment limit", new Font(new FontFamily("Verdana"), 9, FontStyle.Bold), Brushes.Black,
                            //     new Rectangle(130, 884, 600, 650));
                        }
                        else
                        {
                            Rectangle rect5 = new Rectangle(148 + x, 593 - 2 + 1 - y_Stature, 3, 3);
                            g.DrawEllipse(drawingPen1, rect5);
                        }

                        if (Weight < 1.4 || Weight > 18.4)
                        {
                            //g.DrawString("Given Weight is not ful filling graph requirment limit", new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                            //    new Rectangle(130, 907, 600, 650));
                        }
                        else
                        {
                            if (Weight < 2)
                            {
                                Rectangle rect3 = new Rectangle(148 + x, 866 - 2 + 1 - y_Weight, 3, 3);
                                g.DrawEllipse(drawingPen, rect3);
                            }
                            else
                            {
                                Rectangle rect4 = new Rectangle(148 + x, 866 - 2 + 1 - 21 - y_Weight, 3, 3);
                                g.DrawEllipse(drawingPen, rect4);
                            }
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
            catch (Exception)
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