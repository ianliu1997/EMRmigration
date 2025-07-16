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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.EMR;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.ValueObjects.Administration;
using System.Reflection;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace PalashDynamics.MIS.Sales
{
    public partial class PatientWiseDoctorReport : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;

        public PatientWiseDoctorReport()
        {
            InitializeComponent();
        }

        #region FillCombobox
       


      
        #endregion

        private void SearchPatient_Loaded(object sender, RoutedEventArgs e)
        {

            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            
            dtpFromDate.Focus();
        }

      
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            
            Nullable<DateTime> dtFT = null;
            Nullable<DateTime> dtTT = null;
            Nullable<DateTime> dtTP = null;

            string MRNO ="";
            string FirstName = "";
            string MiddleName = "";
            string LastName = "";

            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
          
            bool chkToDate = true;
            string msgTitle = "";
            if (dtpFromDate.SelectedDate != null)
            {
                dtFT = dtpFromDate.SelectedDate.Value.Date.Date;
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtTT = dtpToDate.SelectedDate.Value.Date.Date;
                if (dtFT.Value > dtTT.Value)
                {
                    dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                    dtTP = dtFT;
                    chkToDate = false;
                }
                else
                {
                    dtTP = dtTT;
                    //dtTT = dtTP.Value.Date.AddDays(1);
                    dtTT = dtTP.Value.AddDays(1);
                }

            }

            if (dtTT != null)
            {
                if (dtFT != null)
                {
                    dtFT = dtpFromDate.SelectedDate.Value.Date.Date;

                    //if (dtpF.Equals(dtpT))
                    //    dtpT = dtpF.Value.Date.AddDays(1);
                }
            }

            if (txtMrno.Text != "")
            {
                MRNO = txtMrno.Text;
            }
          
            if (txtFirstName.Text != null)
            {
                FirstName = txtFirstName.Text;
            }
            if (txtMiddleName.Text != null)
            {

                MiddleName = txtMiddleName.Text;
            }
            if (txtLastName.Text != null)
            {
                LastName = txtLastName.Text;
            }

            if (chkToDate == true)
            {
                string URL;
                if (dtFT != null && dtTT != null && dtTP != null)
                {
                    URL = "../Reports/Sales/DoctorWisePatientDetail.aspx?FromDate=" + dtFT.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtTT.Value.ToString("dd/MMM/yyyy") + "&UnitID=" + UnitID + "&MRNO=" + MRNO + "&FirstName=" + FirstName + "&MiddleName=" + MiddleName + "&LastName=" + LastName + "&ToDatePrint=" + dtTP.Value.ToString("dd/MMM/yyyy") + "&rptid=" + 2;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
               
            }
            else
            {
                string msgText = "Incorrect Date Range. /n From Date Cannot Be Greater Than To Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.SalesReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {

        }


    }
}
