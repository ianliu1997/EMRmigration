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
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Windows.Browser;
using CIMS;

namespace EMR
{
    public partial class FrmHistorySelection : ChildWindow
    {
        int HistoryFlag;
        public clsVisitVO CurrentVisit { get; set; }
        public int TemplateID { get; set; }

        public FrmHistorySelection()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion
        private void cmdAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (History.IsChecked == false && nmHistory.IsChecked == false && Couple.IsChecked == false && Hystroscopy.IsChecked == false && Laproscopy.IsChecked == false)
                {
                    ShowMessageBox("Please select at least One Option", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    return;
                }
                if (History.IsChecked == true)
                {
                    HistoryFlag = 1;//select history
                }
                if (nmHistory.IsChecked == true)
                {
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/NewPatientHystroscopyandLaproscopyHistory.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID + "&TemplateID="+ TemplateID + "&EmrID=0"), "_blank");
                    this.DialogResult = false;
                    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                    return;
                }
                if (Couple.IsChecked == true)
                {
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientCoupleHistory.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID), "_blank");
                    this.DialogResult = false;
                    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                    return;
                }

                if (Hystroscopy.IsChecked == true)
                {
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/NewPatientHystroscopyandLaproscopyHistory.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID + "&TemplateID=23" + "&EmrID=0"), "_blank");
                    this.DialogResult = false;
                    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                    return;
                }
                if (Laproscopy.IsChecked == true)
                {
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/NewPatientHystroscopyandLaproscopyHistory.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID + "&TemplateID=24" + "&EmrID=0"), "_blank");
                    this.DialogResult = false;
                    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                    return;
                }

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientClinicalSummarynew.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID + "&HistoryFlag=2&TemplateID=" + TemplateID), "_blank");
                this.DialogResult = false;
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void cmdCancel_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

