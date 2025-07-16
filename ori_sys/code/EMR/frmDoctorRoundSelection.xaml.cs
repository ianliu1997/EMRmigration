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
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using CIMS;
using PalashDynamics.ValueObjects.IPD;
using System.Reflection;
using System.Windows.Controls.Primitives;
namespace EMR
{
    public partial class frmDoctorRoundSelection : ChildWindow
    {
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        public clsVisitVO CurrentVisit { get; set; }
        public Boolean GetNewRound;
        public RoutedEventHandler OnAddButton_Click;
        public frmDoctorRoundSelection()
        {
            InitializeComponent();
        }
        private void cmdAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NewRound.IsChecked == false && ExistingRound.IsChecked == false)
                {
                    ShowMessageBox("Please select Round", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    return;
                }

                if (NewRound.IsChecked == true)
                {
                    GetNewRound = true;
                }
                else
                {
                    GetNewRound = false;
                }
                this.DialogResult = false;
                this.OnAddButton_Click(GetNewRound, e);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion
        private void CheckAtleastOneRoundExistOrNot()
        {
            try
            {
                ClsIPDCheckRoundExistsBizactionVO Bizaction = new ClsIPDCheckRoundExistsBizactionVO();
                Bizaction.PatientID = CurrentVisit.PatientId;
                Bizaction.VisitId = CurrentVisit.ID;
                Bizaction.IsOpdIpd = CurrentVisit.OPDIPD;
                Bizaction.DoctorID =Convert.ToInt64(((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((ClsIPDCheckRoundExistsBizactionVO)args.Result).status == 0)
                        {
                            ExistingRound.Visibility = Visibility.Collapsed;
                        }
                    }
                };
                client.ProcessAsync(Bizaction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception pp)
            {
                throw;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CheckAtleastOneRoundExistOrNot();
        }

        private void cmdCancel_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("EMR.IPD_EMR.frmAdmissionList") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(mydata);
            ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
            PART_MaximizeToggle.IsChecked = false;
        }
    }
}
