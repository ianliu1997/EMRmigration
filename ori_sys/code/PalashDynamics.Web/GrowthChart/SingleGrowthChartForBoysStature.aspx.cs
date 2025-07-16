using System;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace PalashDynamics.Web.GrowthChart
{
    public partial class SingleGrowthChartForBoysStature : System.Web.UI.Page
    {
     
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PatientName = string.Empty;
                string Record = string.Empty;
                DateTime VisitDate = new DateTime();
                double age = 0.0;
                double Weight = 0.0;
                double Stature = 0.0;
                double BMI = 0.0;
                string Comments = string.Empty;

                if (Request.QueryString["PatName"] != null)
                {
                    PatientName = Convert.ToString(Request.QueryString["PatName"]);
                }

                if (Request.QueryString["MrNo"] != null)
                {
                    Record = Convert.ToString(Request.QueryString["MrNo"]);
                }

                if (Request.QueryString["VisitDate"] != null)
                {
                    VisitDate = Convert.ToDateTime(Request.QueryString["VisitDate"]);
                }


                if (Request.QueryString["Age"] != null)
                {
                    age = Convert.ToDouble(Request.QueryString["Age"]);
                }
                if (Request.QueryString["Stature"] != null)
                {
                    Stature = Convert.ToDouble(Request.QueryString["Stature"]);
                }
                if (Request.QueryString["Weight"] != null)
                {
                    Weight = Convert.ToDouble(Request.QueryString["Weight"]);
                }
                if (Request.QueryString["BMI"] != null)
                {
                    BMI = Convert.ToDouble(Request.QueryString["BMI"]);
                }

                if (Request.QueryString["Comments"] != null)
                {
                    Comments = Convert.ToString(Request.QueryString["Comments"]);
                }


                Bitmap bmp = new Bitmap(720, 950);
                Graphics g = Graphics.FromImage(bmp);

                String path1 = Server.MapPath("~/GrowthChart/Image/StatureAndWeight2To20Boys.bmp");
                System.Drawing.Image img = System.Drawing.Image.FromFile(path1);

                g.Clear(Color.White);

                g.DrawImage(img, 10, 10);



                Pen drawingPen = new Pen(Color.FromArgb(180, Color.Black), 3);
                Pen drawingPen1 = new Pen(Color.FromArgb(180, Color.Red), 3);

                Rectangle rect = new Rectangle(107 + 28, 878 - 28, 1, 1);

                g.DrawString(PatientName, new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                            new Rectangle(450, 46, 500, 100));
                g.DrawString(Record, new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                           new Rectangle(565, 64, 500, 100));




                g.DrawString(Convert.ToString(VisitDate.ToShortDateString()), new Font(new FontFamily("Arial"), 6, FontStyle.Bold), Brushes.Black,
                          new Rectangle(55, 132 + 12, 80, 50));
                g.DrawString(Convert.ToString(age), new Font(new FontFamily("Arial"), 6, FontStyle.Bold), Brushes.Black,
                          new Rectangle(130, 132 + 12, 80, 50));
                g.DrawString(Convert.ToString(Weight), new Font(new FontFamily("Arial"), 6, FontStyle.Bold), Brushes.Black,
                          new Rectangle(188, 132 + 12, 80, 50));
                g.DrawString(Convert.ToString(Stature), new Font(new FontFamily("Arial"), 6, FontStyle.Bold), Brushes.Red,
                          new Rectangle(260, 132 + 12, 80, 50));
                g.DrawString(Convert.ToString(BMI), new Font(new FontFamily("Arial"), 6, FontStyle.Bold), Brushes.Black,
                          new Rectangle(315, 132 + 12, 80, 50));










                int x = 0;
                int y_Stature = 0;
                int y_Weight = 0;

                # region x axis
                if (age <= 2)
                {
                    x = 0;
                }
                else if (age > 2 && age <= 20)
                {
                    x = Convert.ToInt32(age * 26 - 52);
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

                # region y- for Stature axis
                if (Stature < 75)
                {
                    y_Stature = 0;
                }
                else
                {
                    int q = 0;
                    q = Convert.ToInt32(Stature) - 75;
                    if (Stature >= 90 && Stature < 100)
                    {
                        y_Stature = 5 * q - 1;
                    }
                    else
                        if (Stature >= 100 && Stature < 110)
                        {
                            y_Stature = 5 * q - 2;
                        }
                        else
                            if (Stature >= 110 && Stature < 120)
                            {
                                y_Stature = 5 * q - 3;
                            }
                            else
                                if (Stature >= 120 && Stature < 130)
                                {
                                    y_Stature = 5 * q - 4;
                                }
                                else
                                    if (Stature >= 130 && Stature < 140)
                                    {
                                        y_Stature = 5 * q - 5;
                                    }
                                    else
                                        if (Stature >= 140 && Stature < 150)
                                        {
                                            y_Stature = 5 * q - 6;
                                        }
                                        else
                                            if (Stature >= 150 && Stature < 160)
                                            {
                                                y_Stature = 5 * q - 7;
                                            }
                                            else
                                                if (Stature >= 160 && Stature < 170)
                                                {
                                                    y_Stature = 5 * q - 8;
                                                }
                                                else
                                                    if (Stature >= 170 && Stature < 180)
                                                    {
                                                        y_Stature = 5 * q - 9;
                                                    }
                                                    else
                                                        if (Stature >= 180 && Stature < 190)
                                                        {
                                                            y_Stature = 5 * q - 10;
                                                        }
                                                        else
                                                            if (Stature >= 190 && Stature < 200)
                                                            {
                                                                y_Stature = 5 * q - 11;
                                                            }
                                                            else
                                                                if (Stature >= 200 && Stature < 210)
                                                                {
                                                                    y_Stature = 5 * q - 12;
                                                                }
                                                                else
                                                                    y_Stature = 5 * q;


                }
                #endregion

                # region y - for Weight axis
                if (Weight < 6)
                {
                    y_Weight = 0;
                }
                else
                {
                    int q1 = 0;
                    q1 = Convert.ToInt32(Weight) - 6;
                    if (Weight > 6 && Weight < 29)
                    {
                        y_Weight = 5 * q1 - 1;
                    }
                    else if (Weight >= 30 && Weight < 40)
                    {
                        y_Weight = 5 * q1 - 2;
                    }
                    else if (Weight >= 40 && Weight < 50)
                    {
                        y_Weight = 5 * q1 - 3;
                    }
                    else if (Weight >= 50 && Weight < 60)
                    {
                        y_Weight = 5 * q1 - 4;
                    }
                    else if (Weight >= 60 && Weight < 70)
                    {
                        y_Weight = 5 * q1 - 5;
                    }
                    else if (Weight >= 70 && Weight < 80)
                    {
                        y_Weight = 5 * q1 - 6;
                    }
                    else if (Weight >= 80 && Weight < 90)
                    {
                        y_Weight = 5 * q1 - 7;
                    }
                    else if (Weight >= 90 && Weight < 100)
                    {
                        y_Weight = 5 * q1 - 8;
                    }
                    else if (Weight >= 100 && Weight < 110)
                    {
                        y_Weight = 5 * q1 - 9;
                    }
                    else if (Weight >= 40)
                    {
                        y_Weight = 5 * q1 - 3;
                    }
                    else if (Weight >= 40)
                    {
                        y_Weight = 5 * q1 - 3;
                    }
                    else if (Weight >= 40)
                    {
                        y_Weight = 5 * q1 - 3;
                    }
                    else if (Weight >= 40)
                    {
                        y_Weight = 5 * q1 - 3;
                    }
                    else
                        y_Stature = 5 * q1;

                }
                #endregion

                g.DrawString("Note : ", new Font(new FontFamily("Verdana"), 8, FontStyle.Bold), Brushes.Black,
                        new Rectangle(8, 905, 700, 100));

                if (Weight > 0 || Weight <= 110)
                {
                    g.DrawString("Patient weight is not within limit", new Font(new FontFamily("Verdana"), 8, FontStyle.Bold), Brushes.Black,
                      new Rectangle(20, 915, 700, 100));
                  
                }
                else
                {
                    Rectangle rect2 = new Rectangle(103 + x, 875 - y_Weight, 3, 3);
                    g.DrawEllipse(drawingPen, rect2);
                    
                }
                if (Stature >= 75 || Stature <= 197)
                {
                    g.DrawString("Patient Stature is not within limit", new Font(new FontFamily("Verdana"), 8, FontStyle.Bold), Brushes.Black,
                     new Rectangle(20, 925, 700, 100));
                  
                }
                else
                {
                    Rectangle rect3 = new Rectangle(103 + x, 709 - y_Stature, 3, 3);
                    g.DrawEllipse(drawingPen1, rect3);
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