using System;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace PalashDynamics.Web.GrowthChart
{
    public partial class SingleGrowthChartForBoysBMI : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string PatientName = string.Empty;
                string Record = string.Empty;
                DateTime VisitDate = new DateTime();
                double age = 0.0;
                double weight = 0.0;
                double Stature = 0.0;
                double BMI = 0.0;
                string Comments = string.Empty;

                if (Request.QueryString["PatName"] != null)
                {
                    PatientName = Convert.ToString(Request.QueryString["PatName"]);
                }

                if (Request.QueryString["VisitDate"] != null)
                {
                    VisitDate = Convert.ToDateTime(Request.QueryString["VisitDate"]);
                }

                if (Request.QueryString["MrNo"] != null)
                {
                    Record = Convert.ToString(Request.QueryString["MrNo"]);
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
                    weight = Convert.ToDouble(Request.QueryString["Weight"]);
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

                String path1 = Server.MapPath("~/GrowthChart/Image/BMI2TO20Boys.bmp");
                System.Drawing.Image img = System.Drawing.Image.FromFile(path1);

                g.Clear(Color.White);

                g.DrawImage(img, 10, 10);



                Pen drawingPen = new Pen(Color.FromArgb(180, Color.Black), 3);

                Rectangle rect = new Rectangle(107 + 28, 878 - 28, 1, 1);

                g.DrawString(PatientName, new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                                new Rectangle(477, 94, 500, 100));
                g.DrawString(Record, new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                           new Rectangle(600, 113, 500, 100));



                int x = 0;
                int y = 0;

                g.DrawString(Convert.ToString(VisitDate.ToShortDateString()), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(65, 151 + 12, 80, 50));
                g.DrawString(Convert.ToString(age), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(130, 151 + 12, 80, 50));
                g.DrawString(Convert.ToString(weight), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(169, 151 + 12, 80, 50));
                g.DrawString(Convert.ToString(Stature), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(220, 151 + 12, 80, 50));
                g.DrawString(Convert.ToString(BMI), new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(285, 151 + 12, 80, 50));
                g.DrawString(Comments, new Font(new FontFamily("Verdana"), 6), Brushes.Black,
                                  new Rectangle(316, 151 + 12, 140, 50));






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
                    int secondValue = 0;
                    if (a.Contains('.') == true)
                    {
                        secondValue = int.Parse(b[1]);
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
                    g.DrawString("Given BMI is not matching graph required range", new Font(new FontFamily("Verdana"), 10, FontStyle.Bold), Brushes.Black,
                      new Rectangle(130, 907, 600, 650));
                }
                else
                {
                    Rectangle rect2 = new Rectangle(107 + 1 + x, 878 - y, 3, 3);
                    g.DrawEllipse(drawingPen, rect2);
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