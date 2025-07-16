using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.EMR.GrowthChart;

namespace EMR
{
    public partial class frmIPDViewGrowthChart : UserControl
    {
        clsGrowthChartVO ChartDetails;
        bool chartType0to2 = false;
        bool viewAll = false;
        long PatientID = 0;
        long VisitID = 0;
        string PatientName = string.Empty;
        string MrNo = string.Empty;
        int GenderID = 0;
        long GenderFor = 0;
        long Age = 0;
        long AgeInMonth = 0;
        long unitid = 0;
        bool IsOPDIPD = false;

        public frmIPDViewGrowthChart()
        {
            InitializeComponent();
            rdo220BMI.Visibility = Visibility.Visible;
            rdo220StratureWeight.Visibility = Visibility.Visible;
        }

         public frmIPDViewGrowthChart(bool chartType0to2)
        {
            InitializeComponent();
            viewAll = false;
            if (chartType0to2 == true)
            {
                rdo02LenghtWeight.Visibility = Visibility.Visible;
                rdo02HCWeight.Visibility = Visibility.Visible;
                rdo220BMI.Visibility = Visibility.Collapsed;
                rdo220StratureWeight.Visibility = Visibility.Collapsed;
            }
            else
            {
                rdo220BMI.Visibility = Visibility.Visible;
                rdo220StratureWeight.Visibility = Visibility.Visible;
                rdo02LenghtWeight.Visibility = Visibility.Collapsed;
                rdo02HCWeight.Visibility = Visibility.Collapsed;

            }

        }

        public frmIPDViewGrowthChart(bool chartType0to2, clsGrowthChartVO ChartDetails)
        {
            InitializeComponent();
            this.chartType0to2 = chartType0to2;
            viewAll = false;
            this.PatientID = ChartDetails.Id;
            this.VisitID = ChartDetails.DrID;
            this.MrNo = ChartDetails.MRNo;
            this.PatientName = ChartDetails.PatientName;
            this.GenderFor = ChartDetails.GenderID;
            this.Age = ChartDetails.Age;
            this.AgeInMonth = ChartDetails.AgeInMonth;


            if (chartType0to2 == true)
            {
                rdo02LenghtWeight.Visibility = Visibility.Visible;
                rdo02HCWeight.Visibility = Visibility.Visible;
                rdo220BMI.Visibility = Visibility.Collapsed;
                rdo220StratureWeight.Visibility = Visibility.Collapsed;
            }
            else
            {
                rdo220BMI.Visibility = Visibility.Visible;
                rdo220StratureWeight.Visibility = Visibility.Visible;
                rdo02LenghtWeight.Visibility = Visibility.Collapsed;
                rdo02HCWeight.Visibility = Visibility.Collapsed;

            }
            this.ChartDetails = ChartDetails;
        }

        public frmIPDViewGrowthChart(bool chartType0to2, long PatientID, long lVisitId, string PatientName, string MrNo, int GenderID,long unitid1,bool IsOPDIPD1)
        {
            InitializeComponent();
            viewAll = true;
            this.PatientID = PatientID;
            this.VisitID = lVisitId;
            this.MrNo = MrNo;
            this.PatientName = PatientName;
            this.GenderID = GenderID;
            this.chartType0to2 = chartType0to2;
            unitid = unitid1;
            IsOPDIPD = IsOPDIPD1;
            if (chartType0to2 == true)
            {
                rdo02LenghtWeight.Visibility = Visibility.Visible;
                rdo02HCWeight.Visibility = Visibility.Visible;
                rdo220BMI.Visibility = Visibility.Collapsed;
                rdo220StratureWeight.Visibility = Visibility.Collapsed;
            }
            else
            {
                rdo220BMI.Visibility = Visibility.Visible;
                rdo220StratureWeight.Visibility = Visibility.Visible;
                rdo02LenghtWeight.Visibility = Visibility.Collapsed;
                rdo02HCWeight.Visibility = Visibility.Collapsed;
            }
        }

        bool CheckEmpty()
        {
            bool retval = false;
            if (chartType0to2 == true)
            {
                if (rdo02LenghtWeight.IsChecked == false && rdo02HCWeight.IsChecked == false)
                    retval = true;
            }
            else
            {
                if (rdo220BMI.IsChecked == false && rdo220StratureWeight.IsChecked == false)
                    retval = true;
            }
            return retval;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (CheckEmpty() == false)
            {
                if (viewAll == false)
                {
                    string address = string.Empty;
                    string URL = string.Empty;
                    clsGrowthChartVO obj = new clsGrowthChartVO();

                    if (GenderFor == 2)
                    {
                        if (rdo220BMI.IsChecked == true)
                        {
                            address = "../GrowthChart/SingleGrowthChartForGirlsBMI.aspx?PatName=";
                        }
                        else if (rdo220StratureWeight.IsChecked == true)
                        {

                            address = "../GrowthChart/SingleGrowthChartForGirlsStature.aspx?PatName=";
                        }
                        else if (rdo02LenghtWeight.IsChecked == true)
                        {

                            address = "../GrowthChart/SingleGrowthChartForGirlsLengthAndWeight.aspx?PatName=";
                        }
                        else if (rdo02HCWeight.IsChecked == true)
                        {

                            address = "../GrowthChart/SingleGrowthChartForGirlsHCAndWeight.aspx?PatName=";
                        }
                    }
                    else
                    {
                        if (rdo220BMI.IsChecked == true)
                        {

                            address = "../GrowthChart/SingleGrowthChartForBoysBMI.aspx?PatName=";
                        }
                        else if (rdo220StratureWeight.IsChecked == true)
                        {

                            address = "../GrowthChart/SingleGrowthChartForBoysStature.aspx?PatName=";
                        }
                        else if (rdo02LenghtWeight.IsChecked == true)
                        {

                            address = "../GrowthChart/SingleGrowthChartForBoysLengthAndWeight.aspx?PatName=";
                        }
                        else if (rdo02HCWeight.IsChecked == true)
                        {

                            address = "../GrowthChart/SingleGrowthChartForBoysHCAndWeight.aspx?PatName=";
                        }
                    }
                    if (rdo220BMI.IsChecked == true || rdo220StratureWeight.IsChecked == true)
                    {
                        URL = address + PatientName + "&MrNo=" + ChartDetails.MRNo + "&VisitDate=" + ChartDetails.VisitDate.ToShortDateString() + "&GenderID=" + ChartDetails.GenderID + "&Age=" + Age + "&Stature=" + ChartDetails.Height + "&Weight=" + ChartDetails.Weight + "&BMI=" + ChartDetails.BMI + "&Comments=" + txtComments.Text;
                    }
                    else if (rdo02LenghtWeight.IsChecked == true || rdo02HCWeight.IsChecked == true)
                    {
                        URL = address + PatientName + "&VisitDate=" + ChartDetails.VisitDate.ToShortDateString() + "&GenderID=" + ChartDetails.GenderID + "&AgeInMonth=" + AgeInMonth + "&Length=" + ChartDetails.Height + "&Weight=" + ChartDetails.Weight + "&HC=" + ChartDetails.HC + "&MrNo=" + ChartDetails.MRNo + "&Comments=" + txtComments.Text;
                    }

                    var option = new HtmlPopupWindowOptions { Left = 0, Top = 0, Width = 720, Height = 1000 };
                    //   var option = new HtmlPopupWindowOptions { Left = 0, Top = 0, Width = 799, Height = 1000 };

                    if (HtmlPage.IsPopupWindowAllowed)
                        HtmlPage.PopupWindow(new Uri(Application.Current.Host.Source, URL), "", option);
                    else
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                    //int chartType = 0;
                    //if (rdo220BMI.IsChecked == true)
                    //    chartType = 3;
                    //else if (rdo220StratureWeight.IsChecked == true)
                    //    chartType = 4;
                    //else if (rdo02LenghtWeight.IsChecked == true)
                    //    chartType = 20;
                    //else if (rdo02HCWeight.IsChecked == true)
                    //    chartType = 21;
                    //string URL = "../GrowthChart/frmGrowthChart.aspx?PatName=" + ChartDetails.PatientName + "&VisitDate=" + ChartDetails.VisitDate.ToString("dd/MMM/yyyy") + "&GenderID=" + ChartDetails.GenderID + "&chartType=" + chartType + "&AgeInMonth=" + ChartDetails.AgeInMonth + "&Height=" + ChartDetails.Height + "&Weight=" + ChartDetails.Weight + "&HC=" + ChartDetails.HC + "&MrNo=" + ChartDetails.MRNo + "&Comments=" + txtComments.Text;
                    ////frmWebBrowser viewGrowth = new frmWebBrowser(URL);   
                    //var option =new HtmlPopupWindowOptions { Left=0,Top=0,Width=1000,Height=1200};

                    //if (HtmlPage.IsPopupWindowAllowed)
                    //    HtmlPage.PopupWindow(new Uri(Application.Current.Host.Source, URL), "new", option);
                    //else
                    //    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    string address = string.Empty;
                    //     int chartType = 0;

                    clsGrowthChartVO obj = new clsGrowthChartVO();


                    if (GenderID == 2)
                    {
                        if (rdo220BMI.IsChecked == true)
                        {
                            //           chartType = 3;
                            //    address = "../GrowthChart/frmGrowthChartYears.aspx?PatName=";
                            address = "../GrowthChart/GrowthChartForGirlsBMI.aspx?PatName=";
                        }
                        else if (rdo220StratureWeight.IsChecked == true)
                        {
                            //         chartType = 4;
                            address = "../GrowthChart/GrowthChartForGirlsStature.aspx?PatName=";
                        }
                        else if (rdo02LenghtWeight.IsChecked == true)
                        {
                            //         chartType = 20;
                            address = "../GrowthChart/GrowthChartForGirlsLengthAndWeight.aspx?PatName=";
                        }
                        else if (rdo02HCWeight.IsChecked == true)
                        {
                            //        chartType = 21;
                            address = "../GrowthChart/GrowthChartForGirlsHCAndWeight.aspx?PatName=";
                        }
                    }
                    else
                    {
                        if (rdo220BMI.IsChecked == true)
                        {

                            address = "../GrowthChart/GrowthChartForBoysBMI.aspx?PatName=";
                        }
                        else if (rdo220StratureWeight.IsChecked == true)
                        {

                            address = "../GrowthChart/GrowthChartForBoysStature.aspx?PatName=";
                        }
                        else if (rdo02LenghtWeight.IsChecked == true)
                        {

                            address = "../GrowthChart/GrowthChartForBoysLengthAndWeight.aspx?PatName=";
                        }
                        else if (rdo02HCWeight.IsChecked == true)
                        {

                            address = "../GrowthChart/GrowthChartForBoysHCAndWeight.aspx?PatName=";
                        }
                    }

                    string URL = address + PatientName + "&GenderID=" + GenderID + "&MrNo=" + MrNo + "&PatientID=" + PatientID + "&DrID=" + VisitID + "&Comments=" + txtComments.Text + "&UnitID=" + unitid + "&Isopdipd=" + IsOPDIPD;

                    var option = new HtmlPopupWindowOptions { Left = 0, Top = 0, Width = 720, Height = 1000 };
                    //   var option = new HtmlPopupWindowOptions { Left = 0, Top = 0, Width = 799, Height = 1000 };

                    if (HtmlPage.IsPopupWindowAllowed)
                        HtmlPage.PopupWindow(new Uri(Application.Current.Host.Source, URL), "", option);
                    else
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                }

                //this.DialogResult = true;
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Select the Chart.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgbx.Show();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
        }

        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
        //    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        //}

        private void rdo220StratureWeight_Checked(object sender, RoutedEventArgs e)
        {
            tbComments.Visibility = Visibility.Collapsed;
            txtComments.Visibility = Visibility.Collapsed;
        }

        private void rdo_Checked(object sender, RoutedEventArgs e)
        {
            if (viewAll == false)
            {
                tbComments.Visibility = Visibility.Visible;
                txtComments.Visibility = Visibility.Visible;
            }
        }
    }
}
