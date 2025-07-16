using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.OutPatientDepartment;

namespace CIMS
{
    public interface IApplicationConfiguration
    {
        clsPatientGeneralVO SelectedPatient { get; set; }
        clsCoupleVO SelectedCoupleDetails { get; set; }
        clsIPDAdmissionVO SelectedIPDPatient { get; set; }
        clsUserVO CurrentUser { get; set; }

        clsPatientGeneralVO SelectedPatientForPediatric { get; set; }   // Use to select Female Patient on search window for Pediatric flow

        //string UserMachineName { get; set; }

        void FillMenu(string Parent);

        void OpenMainContent(string Action);

        clsAppConfigVO ApplicationConfigurations { get;  }

        void OpenMainContent(UIElement UIelement);

      

        clsPathOrderBookingVO SelectedPathologyWorkOrder { get; set; }
        clsRadOrderBookingVO SelectedRadiologyWorkOrder { get; set; }
        clsPatientProcedureScheduleVO SelectedOTBooking { get; set; }
        LocalizationManager LocalizedManager { get; set; }
    }

    public interface IInitiateCIMS
    {
        void Initiate(string Mode);
        
    }

    public interface GetVisitData
    {
        void Test(clsVisitVO CurrentVisit);
    }

    public interface IPassData
    {
        void PassDataToForm(clsBillVO value, bool fromForm);
    }

    public interface IPreInitiateCIMS
    {
        void PreInitiate(clsMenuVO _MenuDetails);
    }

    //By Anjali.................
    public interface IInitiateCIMSIVF
    {
        void Initiate(clsMenuVO Item);
    }
    //..........................
 	//Added by AJ Date 5/1/2017
    public interface IInitiateMaterialConsumption
    {
        void InitiateMaterialConsumption(clsIPDAdmissionVO objAdmission);
    }
    //***//-----------------------

    //by neena
    public interface IInitiateIVFDashBoard
    {
        void InitiateDashBoard(long TherapyId, long TherapyUnitId, long PatientID, long PatientUnitId,long GridIndex);
    }
    //
    public class Comman
    {
        //For Round Off Logic By CDS
        public static readonly float MinMaxRoundOff = 5;

        public static clsAppConfigVO ApplicationConfigurations { get; set; }

        //private constructor 
        private Comman()
        {

        }

        public static void SetDefaultHeader(clsMenuVO _SelfMenu)
        {
            if (_SelfMenu != null)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + _SelfMenu.Title;
            }
        }

        public static void SetPatientDetailHeader(clsPatientVO _Patient)
        {
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

            mElement.Text = " : " + _Patient.FirstName + " " + _Patient.MiddleName + " " + _Patient.LastName;

            mElement.Text += " - " + _Patient.GeneralDetails.MRNo + " : " + _Patient.Gender;
        }

        public interface IPreInitiateCIMS
        {
            void PreInitiate(clsMenuVO _MenuDetails);
        }   

        //public static void SetItemDetailHeader(clsPreventiveMaintenanceVO _Item)
        //{
        //    UserControl rootPage = Application.Current.RootVisual as UserControl;
        //    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

        //    mElement.Text = " : " + _Item.ItemName + " : " + _Item.ID + " : " + _Item.ItemCode;


        //}

        public static void OpenReport(ReportType _Type)
        {
            if (_Type.Equals(ReportType.OPDReport))
            {
                string URL = "../Reports/OPD/OPDReport.aspx";
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else if (_Type.Equals(ReportType.EMRReport))
            {
                string URL = "../Reports/EMR/EMRSummaryReport.aspx";
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else if (_Type.Equals(ReportType.MISReport))
            {
                string URL = "../Reports/ReportConsole/PatientReports/frmPatientReports.aspx";
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else if (_Type.Equals(ReportType.AppointmentList))
            {
                string URL = "../Reports/OPD/AppointmentListReport.aspx";
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        static void webBrowserForPrinting_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        public static double ToRoundUp(double _Amount)
        {
            double RoundupAmount = Math.Round(_Amount, ApplicationConfigurations.RoundingDigit);
            return Convert.ToDouble(RoundupAmount.ToString(ApplicationConfigurations.RoundupDigitString));
        }

        public static decimal ToRoundUp(decimal _Amount)
        {
            double RoundupAmount = Math.Round(Convert.ToDouble(_Amount), ApplicationConfigurations.RoundingDigit);
            return Convert.ToDecimal(RoundupAmount.ToString(ApplicationConfigurations.RoundupDigitString));
        }

        public static string ToRoundUp(string _Amount)
        {
            if (!string.IsNullOrEmpty(_Amount))
            {
                double RoundupAmount = Math.Round(Convert.ToDouble(_Amount), ApplicationConfigurations.RoundingDigit);
                return RoundupAmount.ToString(ApplicationConfigurations.RoundupDigitString);
            }
            else
            {
                return "0";
            }
        }

        public static bool HandleAlphnumericAndSpecialChar(object sender, KeyEventArgs e, bool IsDescription)
        {
            bool result = false;
            TextBox txt = (TextBox)sender;

            if (string.IsNullOrEmpty(txt.Text))
            {
                if (e.PlatformKeyCode == 9)
                {
                    result = false;
                }
                else if (e.PlatformKeyCode == 32)
                {
                    result = true;
                }
                else if (IsDescription.Equals(true))
                {
                    if (Keyboard.Modifiers == ModifierKeys.Shift && e.PlatformKeyCode != 109 && e.PlatformKeyCode != 190 && e.PlatformKeyCode != 55 && ((e.Key < Key.A) || (e.Key > Key.Z)))
                        result = true;
                    else if (((e.PlatformKeyCode < 48) || (e.PlatformKeyCode > 57)) && ((e.Key < Key.NumPad0) || (e.Key > Key.NumPad9)) && ((e.Key < Key.A) || (e.Key > Key.Z)) && e.PlatformKeyCode != 109 && e.PlatformKeyCode != 32 && e.PlatformKeyCode != 189 && e.PlatformKeyCode != 190)
                    {
                        result = true;
                    }
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && e.PlatformKeyCode != 109 && ((e.Key < Key.A) || (e.Key > Key.Z)))
                    result = true;
                else if (((e.PlatformKeyCode < 48) || (e.PlatformKeyCode > 57)) && ((e.Key < Key.NumPad0) || (e.Key > Key.NumPad9)) && ((e.Key < Key.A) || (e.Key > Key.Z)) && e.PlatformKeyCode != 109 && e.PlatformKeyCode != 32 && e.PlatformKeyCode != 189)
                {
                    result = true;
                }
            }
            else
            {
                if (IsDescription.Equals(true))
                {
                    if (e.PlatformKeyCode == 9)
                    {
                        result = false;
                    }
                    else if (Keyboard.Modifiers == ModifierKeys.Shift && e.PlatformKeyCode != 109 && e.PlatformKeyCode != 55)
                        result = true;
                    else if (((e.PlatformKeyCode < 48) || (e.PlatformKeyCode > 57)) && ((e.Key < Key.NumPad0) || (e.Key > Key.NumPad9)) && ((e.Key < Key.A) || (e.Key > Key.Z)) && e.PlatformKeyCode != 109 && e.PlatformKeyCode != 32 && e.PlatformKeyCode != 189 && e.PlatformKeyCode != 190)
                    {
                        result = true;
                    }
                }
                else if (e.PlatformKeyCode == 9)
                {
                    result = false;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && e.PlatformKeyCode != 109)
                    result = true;
                else if (((e.PlatformKeyCode < 48) || (e.PlatformKeyCode > 57)) && ((e.Key < Key.NumPad0) || (e.Key > Key.NumPad9)) && ((e.Key < Key.A) || (e.Key > Key.Z)) && e.PlatformKeyCode != 109 && e.PlatformKeyCode != 32 && e.PlatformKeyCode != 189)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool HandleDecimal(object sender, KeyEventArgs e)
        {
            bool result = false;
            TextBox txt = (TextBox)sender;

            if (string.IsNullOrEmpty(txt.Text))
            {
                if (e.PlatformKeyCode == 9)
                {
                    e.Handled = false;
                }
                else if (e.PlatformKeyCode == 32)
                {
                    result = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && e.PlatformKeyCode != 109)
                    result = true;
                else if (((e.PlatformKeyCode < 48) || (e.PlatformKeyCode > 57)) && ((e.Key < Key.NumPad0) || (e.Key > Key.NumPad9)) && e.PlatformKeyCode != 109 && e.PlatformKeyCode != 190 && e.PlatformKeyCode != 110)
                {
                    result = true;
                }
            }
            else
            {
                if (e.PlatformKeyCode == 9)
                {
                    e.Handled = false;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && e.PlatformKeyCode != 109)
                    result = true;
                else if (((e.PlatformKeyCode < 48) || (e.PlatformKeyCode > 57)) && ((e.Key < Key.NumPad0) || (e.Key > Key.NumPad9)) && e.PlatformKeyCode != 109 && e.PlatformKeyCode != 190 && e.PlatformKeyCode != 110)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool HandleNumber(object sender, KeyEventArgs e)
        {
            bool result = false;
            TextBox txt = (TextBox)sender;

            if (string.IsNullOrEmpty(txt.Text))
            {
                if (e.PlatformKeyCode == 9)
                {
                    e.Handled = false;
                }
                else if (e.PlatformKeyCode == 32)
                {
                    result = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift)
                    result = true;
                else if (((e.PlatformKeyCode < 48) || (e.PlatformKeyCode > 57)) && ((e.Key < Key.NumPad0) || (e.Key > Key.NumPad9)) && e.PlatformKeyCode != 109)
                {
                    result = true;
                }
            }
            else
            {
                if (e.PlatformKeyCode == 9)
                {
                    e.Handled = false;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift)
                    result = true;
                else if (((e.PlatformKeyCode < 48) || (e.PlatformKeyCode > 57)) && ((e.Key < Key.NumPad0) || (e.Key > Key.NumPad9)) && e.PlatformKeyCode != 109)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool HandleCharacter(object sender, KeyEventArgs e)
        {
            bool result = false;
            TextBox txt = (TextBox)sender;

            if (string.IsNullOrEmpty(txt.Text))
            {
                if (e.PlatformKeyCode == 9)
                {
                    e.Handled = false;
                }
                else if (e.PlatformKeyCode == 32)
                {
                    result = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && e.PlatformKeyCode != 109 && ((e.Key < Key.A) || (e.Key > Key.Z)))
                    result = true;
                else if (((e.Key < Key.A) || (e.Key > Key.Z)))
                {
                    result = true;
                }
            }
            else
            {
                if (e.PlatformKeyCode == 9 || e.PlatformKeyCode == 32)
                {
                    e.Handled = false;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Shift && e.PlatformKeyCode != 109)
                    result = true;
                else if (((e.Key < Key.A) || (e.Key > Key.Z)))
                {
                    result = true;
                }
            }

            return result;
        }


    }

    public static class DateTimeExtensions
    {
        public static long TotalMonthDifference(this DateTime dtThis, DateTime dtOther)
        {
            int intReturn = 0;
            dtThis = dtThis.Date.AddDays(-(dtThis.Day - 1));
            dtOther = dtOther.Date.AddDays(-(dtOther.Day - 1));
            while (dtOther.Date > dtThis.Date)
            {
                intReturn++;
                dtThis = dtThis.AddMonths(1);
            }
            return intReturn;
        }

        /// <summary>
        /// Gets the total number of years between two dates, rounded to whole months.
        /// Examples: 
        /// 2011-12-14, 2012-12-15 returns 1.
        /// 2011-12-14, 2012-12-14 returns 1.
        /// 2011-12-14, 2012-12-13 returns 0,9167.
        /// </summary>
        /// <param name="start">
        /// Stardate of time period
        /// </param>
        /// <param name="end">
        /// Enddate of time period
        /// </param>
        /// <returns>
        /// Total Years between the two days
        /// </returns>
        public static double TotalYearsDifference(this DateTime start, DateTime end)
        {
            // Get difference in total months.
            int months = ((end.Year - start.Year) * 12) + (end.Month - start.Month);

            // substract 1 month if end month is not completed
            if (end.Day < start.Day)
            {
                months--;
            }
            double totalyears = months / 12d;
            return totalyears;
        }
    }

}
