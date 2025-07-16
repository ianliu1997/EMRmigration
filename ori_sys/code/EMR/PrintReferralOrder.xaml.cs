using System;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.UserControls;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.EMR;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using PalashDynamic.Localization;

namespace EMR
{
    public partial class PrintReferralOrder : ChildWindow
    {
        #region Variable Declaration
        public clsVisitVO CurrentVisit;
        public bool IsEnabledControl { get; set; }
        public Patient SelectedPatient { get; set; }
        WaitIndicator Indicatior = new WaitIndicator();
        clsDoctorSuggestedServiceDetailVO SelectedItem = new clsDoctorSuggestedServiceDetailVO();

        public ObservableCollection<clsDoctorSuggestedServiceDetailVO> DoctorList { get; set; }
        public long VisitId = 0;
        public bool IsfromVisitWin; 

        #endregion

        #region constructor
        /// <summary>
        /// Defined Parameterless constructor.
        /// </summary>
        public PrintReferralOrder()
        {
            InitializeComponent();
            this.Title = LocalizationManager.resourceManager.GetString("ttlReferralOrderList");
            DoctorList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();
            this.Loaded += new RoutedEventHandler(PrintReferralOrder_Loaded);
        }
        #endregion

        #region Loaded
        /// <summary>
        /// Handles the Loaded Event of the Page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PrintReferralOrder_Loaded(object sender, RoutedEventArgs e)
        {
            GetReferralList();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// MEthod Fetches the current visit Referrals.
        /// </summary>
        private void GetReferralList()
        {
            WaitIndicator IndicatiorGet = new WaitIndicator();
            try
            {
                IndicatiorGet.Show();
                clsGetReferralDetailsBizActionVO BizAction = new clsGetReferralDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    if (IsfromVisitWin == true)
                        BizAction.VisitID = VisitId;
                    else
                        BizAction.VisitID = CurrentVisit.ID;
                    BizAction.PatientID = CurrentVisit.PatientId;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    IndicatiorGet.Close();

                    if (arg.Error == null)
                    {
                        if (arg.Result != null && ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail != null)
                        {
                            foreach (var item in ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail)
                            {
                                DoctorList.Add(item);
                            }
                        }
                        dgReferralList.ItemsSource = null;
                        dgReferralList.ItemsSource = DoctorList;
                        dgReferralList.UpdateLayout();
                        IndicatiorGet.Close();
                    }
                    else
                    {
                        IndicatiorGet.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                IndicatiorGet.Close();
            }
        }
        #endregion

        #region Click Event
        /// <summary>
        /// Print button click Event Gets Handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgReferralList.SelectedIndex != -1)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/AllReferralLetterPrint.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PrescriptionID=" + ((clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem).PrescriptionID + "&ID=" + ((clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem).ID), "_blank");
            }
        }

        /// <summary>
        /// Cancel Button Click Event Get Handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion
    }
}

