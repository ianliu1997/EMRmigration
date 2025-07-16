using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;
using System.Windows.Browser;

namespace PalashDynamics.MIS.Sales
{
    public partial class RevenueTrandeReport : UserControl
    {
        public RevenueTrandeReport()
        {
        
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            FillUnitList();
            YearList();
            
        }

        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        private void FillUnitList()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                        cmbClinic.ItemsSource = null;
                        cmbClinic.ItemsSource = objList;

                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {
                            var res = from r in objList
                                      where r.ID == User.UserLoginInfo.UnitId
                                      select r;
                            cmbClinic.SelectedItem = ((MasterListItem)res.First());
                            cmbClinic.IsEnabled = false;
                        }
                        else
                            cmbClinic.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }





        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.SalesReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }





        //private void cmdPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    Nullable<DateTime> dtpF = null;
        //    Nullable<DateTime> dtpT = null;
        //    Nullable<DateTime> dtpP = null;

        //    bool IsExporttoExcel = false;
        //    bool chkToDate = true;
        //    string msgTitle = "";


        //    if (dtpFromDate.SelectedDate != null)
        //    {
        //        dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
        //    }

        //    if (dtpToDate.SelectedDate != null)
        //    {
        //        dtpT = dtpToDate.SelectedDate.Value.Date.Date;
        //        if (dtpF.Value > dtpT.Value)
        //        {
        //            dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
        //            dtpT = dtpF;
        //            chkToDate = false;
        //        }
        //        else
        //        {
        //            dtpP = dtpT;
        //            dtpT = dtpT.Value.AddDays(1);
        //        }
        //    }

        //    if (dtpT != null)
        //    {
        //        if (dtpF != null)
        //        {
        //            dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

        //        }
        //    }

        //    long UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;

        //    if (chkToDate == true)
        //    {
        //        string URL;
        //        if (dtpF != null && dtpT != null && dtpP != null)
        //        {
        //            URL = "../Reports/Sales/RevenueTradeReport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&UnitID=" + UnitID + "&Excel=" + chkExporttoExcel.IsChecked; // + "&ReferenceDoctor=" + ReferenceDoctor;
        //            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //        }
        //        else
        //        {

        //            URL = "../Reports/Sales/RevenueTradeReport.aspx?&UnitID=" + UnitID; // + "&ReferenceDoctor=" + ReferenceDoctor;
        //            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //        }


        //    }
        //    else
        //    {
        //        string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
        //        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgWindow.Show();
        //    }
        //}

        private List<int> m_Year;

        private void YearList()
        {      

            int YearStart = 2016;
            int YearEnd = (DateTime.Today.Year);
            m_Year = new List<int>();
            while (YearStart <= YearEnd)
            {
                m_Year.Add(YearStart);
                YearStart = YearStart + 1;
            }                    

            cmbYear.ItemsSource = null;
            cmbYear.ItemsSource = m_Year;
            cmbYear.SelectedItem = m_Year.Max();          
        }


        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {            
            bool IsExporttoExcel = false;
            bool chkToDate = true;
            string msgTitle = "";
            string SelectedYear = null;     
       
            int Year = 0000;         
            DateTime? FromSelectedYearDate = null;
            DateTime? ToSelectedYearDate = null;
          
            if (cmbYear.ItemsSource != null)
            {              
                SelectedYear = cmbYear.SelectedItem.ToString();
                Year =   Convert.ToInt16(SelectedYear);

                FromSelectedYearDate = new DateTime(Year, 04, 01, 00, 00, 01);
                ToSelectedYearDate = new DateTime(Year + 1, 03, 31, 00, 00, 01);


            }
            
                long UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                string URL;

                if (SelectedYear != null)
                {
                    URL = "../Reports/Sales/RevenueTradeReport.aspx?FromYear=" + FromSelectedYearDate.Value.ToString("dd/MMM/yyyy") + "&ToYear=" + ToSelectedYearDate.Value.ToString("dd/MMM/yyyy") + "&UnitID=" + UnitID + "&Excel=" + chkExporttoExcel.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
           
        }

    }
}
