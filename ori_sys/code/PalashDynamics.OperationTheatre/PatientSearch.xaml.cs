using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.OperationTheatre
{
    public partial class PatientSearch : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        bool isLoaded = false;
        WaitIndicator objWaitIndicator = null;
        LocalizationManager objLocalizationManager;
        private PatientTypes OPD_IPD;
        private bool IsDefaultConstructorCall = false;
        public int SearchFor = 0;

        #region Properties

        public long PatientCategoryID { get; set; }

        /// <summary>
        /// Gets or sets the data list.
        /// </summary>
        /// <value>The data list to data bind ItemsSource.</value>
        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                //RaisePropertyChanged("PageSize");
            }
        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        public PatientSearch()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PatientSearch_Loaded);
            this.DataContext = new clsPatientGeneralVO();
            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            IsDefaultConstructorCall = true;
            PageSize = 15;
            objLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }

        void PatientSearch_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            txtPatientName.Focus();
            EnableControl();
            dtpAdmissionFromDate.IsEnabled = false;
            dtpAdmissionToDate.IsEnabled = false;
            dtpRegistrationFromDate.IsEnabled = false;
            dtpRegistrationToDate.IsEnabled = false;
            if (!this.OPD_IPD.Equals(PatientTypes.OPD))//Added by Ashish Thombre
            {
                dtpVisitFromDate.IsEnabled = false;
                dtpVisitToDate.IsEnabled = false;
            }

            //if (this.OPD_IPD.Equals(PatientTypes.IPD) || this.OPD_IPD.Equals(PatientTypes.IPDAll) || this.OPD_IPD.Equals(PatientTypes.All))
            //{
            //    chkCurrentAdmitted.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    chkCurrentAdmitted.Visibility = Visibility.Collapsed;
            //}


            if ((bool)rbOPD.IsChecked)
            {
                if (chkVisit != null && dtpVisitFromDate != null && dtpVisitToDate != null)
                {
                    chkVisit.IsChecked = true;
                    chkAdmission.IsEnabled = false;
                    //dtpVisitFromDate.IsEnabled = true;
                    //dtpVisitToDate.IsEnabled = true;
                    dtpVisitFromDate.SelectedDate = DateTime.Now;
                    dtpVisitToDate.SelectedDate = DateTime.Now;
                }
            }
            GetData();
        }

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "OPD":
                    OPD_IPD = PatientTypes.OPD;
                    break;
                case "IPD":
                    OPD_IPD = PatientTypes.IPD;
                    break;
                case "All":
                    OPD_IPD = PatientTypes.All;
                    break;
                case "OPD_IPD":
                    OPD_IPD = PatientTypes.OPD_IPD;
                    break;
                case "None":
                    OPD_IPD = PatientTypes.None;
                    break;
                case "IPDAll":
                    OPD_IPD = PatientTypes.IPDAll;
                    //IsCallForAdmAllPatient = true;
                    break;
                case "OTIPD":
                    OPD_IPD = PatientTypes.IPD;
                    //IsAdmittedDischarge = true;
                    break;
            }
        }

        #endregion

        private void EnableControl()
        {
            if (this.OPD_IPD.Equals(PatientTypes.IPDAll))
            {
                rbIPD.IsChecked = true;
                rbIPD.IsEnabled = true;
                rbOPD.IsEnabled = false;
                rbAll.IsEnabled = false;
                chkVisit.IsEnabled = false;
            }
            else if (this.OPD_IPD.Equals(PatientTypes.OPD))
            {

                //rbIPD.IsEnabled = true;
                rbIPD.IsEnabled = false;
                rbAll.IsEnabled = false;
                chkAdmission.IsEnabled = false;
                txtIPDNo.IsReadOnly = true;
                chkCurrentAdmitted.Visibility = Visibility.Collapsed;
                //Added by Ashish Thombre
                //if (IsPatientCall == true)
                //{
                //    rbOPD.IsChecked = false;
                //    chkVisit.IsEnabled = false;
                //    dtpVisitToDate.IsEnabled = false;
                //    dtpVisitFromDate.IsEnabled = false;
                //    chkVisit.IsEnabled = false;
                //}
                //else
                //{
                //    rbOPD.IsChecked = true;
                //    chkVisit.IsEnabled = true;
                //    dtpVisitToDate.IsEnabled = true;
                //    dtpVisitFromDate.IsEnabled = true;
                //    chkVisit.IsChecked = true;

                //    dtpVisitToDate.SelectedDate = DateTime.Now.Date;
                //    dtpVisitFromDate.SelectedDate = DateTime.Now.Date;
                //}
            }
            else if (this.OPD_IPD.Equals(PatientTypes.IPD))
            {
                rbIPD.IsChecked = true;
                rbIPD.IsEnabled = true;
                rbOPD.IsEnabled = false;
                rbAll.IsEnabled = false;
                chkVisit.IsEnabled = false;
                txtOPDNo.IsReadOnly = true;

            }
            else if (this.OPD_IPD.Equals(PatientTypes.All))
            {
                rbAll.IsChecked = true;
                rbAll.IsEnabled = true;
                rbOPD.IsEnabled = false;
                rbIPD.IsEnabled = false;
                txtIPDNo.IsReadOnly = true;
                txtOPDNo.IsReadOnly = true;
                chkCurrentAdmitted.Visibility = Visibility.Collapsed;
            }
            else if (this.OPD_IPD.Equals(PatientTypes.OPD_IPD))
            {
                rbOPD.IsChecked = true;
                rbAll.IsEnabled = false;
                rbOPD.IsEnabled = true;
                rbIPD.IsEnabled = true;
            }

        }


        private void dgPatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dgPatientList.SelectedItem;
        }

        #region Validation
        private void txtPatientName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPatientName.Text = txtPatientName.Text.ToTitleCase();
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        #endregion

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            DataPager.PageIndex = 0;
            GetData();
        }

        public void GetData()
        {
            objWaitIndicator = new WaitIndicator();
            objWaitIndicator.Show();

            clsGetOTPatientGeneralDetailsListBizActionVO BizActionObject = new clsGetOTPatientGeneralDetailsListBizActionVO();
            //clsGetPatientGeneralDetailsListBizActionVO BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();

            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();

            if (SearchFor.Equals(0) && IsDefaultConstructorCall)
            {
                BizActionObject.IsSearchForRegistration = true;
            }

            if (this.OPD_IPD.Equals(PatientTypes.IPD))
                BizActionObject.IsCurrentAdmitted = ((bool)chkCurrentAdmitted.IsChecked);
            else
                BizActionObject.IsCurrentAdmitted = false;


            if (chkRegistration.IsChecked == true && (bool)rbOPD.IsChecked)
            {
                BizActionObject.RegistrationWise = true;
            }

            if ((bool)rbOPD.IsChecked && chkRegistration.IsChecked != true)
            {
                BizActionObject.VisitWise = true;
            }

            if ((bool)rbIPD.IsChecked)
            {
                BizActionObject.AdmissionWise = true;
                //BizActionObject.IsBirthCertificate = false;
                BizActionObject.RegistrationWise = false;
                //BizActionObject.IsPatientAcceptanceCompulsory = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsPatientAcceptanceCompulsory;

            }

            if ((bool)rbAll.IsChecked)
            {
                BizActionObject.RegistrationWise = true;
            }

            if ((bool)chkVisit.IsChecked)
            {

                if (dtpVisitFromDate.SelectedDate != null)
                {
                    BizActionObject.VisitFromDate = dtpVisitFromDate.SelectedDate.Value.Date;
                }
                if (dtpVisitToDate.SelectedDate != null)
                {
                    BizActionObject.VisitToDate = dtpVisitToDate.SelectedDate.Value.Date;
                }
            }
            if ((bool)chkAdmission.IsChecked)
            {
                if (dtpAdmissionFromDate.SelectedDate != null)
                {
                    BizActionObject.AdmissionFromDate = dtpAdmissionFromDate.SelectedDate.Value.Date;
                }

                if (dtpAdmissionToDate.SelectedDate != null)
                {
                    BizActionObject.AdmissionToDate = dtpAdmissionToDate.SelectedDate.Value.Date;
                }
            }
            if ((bool)chkRegistration.IsChecked)
            {
                if (dtpRegistrationFromDate.SelectedDate != null)
                {                  
                    BizActionObject.FromDate = dtpRegistrationFromDate.SelectedDate.Value.Date;
                }
                if (dtpRegistrationToDate.SelectedDate != null)
                {
                    if (dtpRegistrationToDate.SelectedDate.Value.Date == dtpRegistrationFromDate.SelectedDate.Value.Date)
                    {
                        BizActionObject.ToDate = dtpRegistrationToDate.SelectedDate.Value.AddDays(1);
                    }
                    else
                    {
                        BizActionObject.ToDate = dtpRegistrationToDate.SelectedDate.Value.Date;
                    }
                }
            }


            if (!string.IsNullOrEmpty(txtPatientName.Text))
                BizActionObject.FirstName = txtPatientName.Text;

            if (!string.IsNullOrEmpty(txtLastName.Text))
                BizActionObject.LastName = txtLastName.Text;

            if (!String.IsNullOrEmpty(txtFamilyName.Text))
                BizActionObject.FamilyName = txtFamilyName.Text;

            if (!String.IsNullOrEmpty(txtMrno.Text))
                BizActionObject.MRNo = txtMrno.Text;

            if (!String.IsNullOrEmpty(txtContactNo.Text))
                BizActionObject.ContactNo = txtContactNo.Text;

            if (txtOPDNo.Text != null && txtOPDNo.Text.Length != 0)
                BizActionObject.OPDNo = txtOPDNo.Text;

            if (txtIPDNo.Text != null && txtIPDNo.Text.Length != 0)
                BizActionObject.IPDNo = txtIPDNo.Text;

            BizActionObject.IsPagingEnabled = true;
            BizActionObject.MaximumRows = DataList.PageSize; ;
            BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    clsGetOTPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetOTPatientGeneralDetailsListBizActionVO;
                    //clsGetPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetPatientGeneralDetailsListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;
                    DataList.Clear();
                    foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                    {
                        DataList.Add(person);
                    }
                    dgPatientList.ItemsSource = null;
                    dgPatientList.ItemsSource = DataList;
                    DataPager.Source = null;
                    DataPager.PageSize = BizActionObject.MaximumRows;
                    DataPager.Source = DataList;
                }
                else
                {
                    string msgText = "No Record found.";
                    //string msgText = objLocalizationManager.GetValue("NoRecordFound_Msg");
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
                objWaitIndicator.Close();
            };

            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientList.SelectedItem != null && ((clsPatientGeneralVO)dgPatientList.SelectedItem).PatientID != 0)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                {
                    OnSaveButton_Click(this, new RoutedEventArgs());
                    this.Close();
                }
            }
            else
            {
                string msgText = "Please select patient.";
                //string msgText = objLocalizationManager.GetValue("PatientValidation_Msg");
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void txtPatientName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PatientSearchButton_Click(sender, e);

        }

        private void rbOPD_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAdmission != null && chkVisit != null)
            {
                chkAdmission.IsChecked = false;
                chkVisit.IsEnabled = true;
                chkAdmission.IsEnabled = false;
            }
        }

        private void rbIPD_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAdmission != null && chkVisit != null)
            {
                chkVisit.IsChecked = false;
                chkVisit.IsEnabled = false;
                chkAdmission.IsEnabled = true;
            }
        }

        private void rbAll_Checked(object sender, RoutedEventArgs e)
        {
            if (chkAdmission != null && chkVisit != null)
            {
                chkVisit.IsChecked = false;
                chkAdmission.IsChecked = false;
                chkVisit.IsEnabled = false;
                chkAdmission.IsEnabled = false;
            }
        }

        public bool RegistrationWise = false;
        public bool VisitWise = false;
        public bool AdmissionWise = false;

        private void chkRegistration_Checked(object sender, RoutedEventArgs e)
        {
            RegistrationWise = (bool)chkRegistration.IsChecked;
            dtpRegistrationFromDate.IsEnabled = true;
            dtpRegistrationToDate.IsEnabled = true;
        }

        private void chkRegistration_Unchecked(object sender, RoutedEventArgs e)
        {
            RegistrationWise = (bool)chkRegistration.IsChecked;
            dtpRegistrationFromDate.IsEnabled = false;
            dtpRegistrationToDate.IsEnabled = false;
            dtpRegistrationFromDate.SelectedDate = null;
            dtpRegistrationToDate.SelectedDate = null;
        }

        private void dtpRegistrationFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpRegistrationFromDate.SelectedDate != null && dtpRegistrationToDate.SelectedDate != null)
            {
                if (dtpRegistrationToDate.SelectedDate < dtpRegistrationFromDate.SelectedDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Should be Less than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    dtpRegistrationFromDate.SelectedDate = null;
                }
            }
        }

        private void dtpRegistrationToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpRegistrationFromDate.SelectedDate != null && dtpRegistrationToDate.SelectedDate != null)
            {
                if (dtpRegistrationToDate.SelectedDate < dtpRegistrationFromDate.SelectedDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Should be Less than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    dtpRegistrationToDate.SelectedDate = null;
                }
            }
        }

        private void chkCurrentAdmitted_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)((CheckBox)sender).IsChecked)
            {

            }

            //if ((bool)((CheckBox)sender).IsChecked)
            //{
            //    cmbUnit.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}
            //else
            //{
            //    if (cmbUnit.ItemsSource != null)
            //        cmbUnit.SelectedItem = ((List<MasterListItem>)cmbUnit.ItemsSource)[0];
            //}
        }

        private void chkVisit_Checked(object sender, RoutedEventArgs e)
        {
            VisitWise = (bool)chkVisit.IsChecked;
            dtpVisitFromDate.IsEnabled = true;
            dtpVisitToDate.IsEnabled = true;
        }

        private void chkVisit_Unchecked(object sender, RoutedEventArgs e)
        {
            VisitWise = (bool)chkVisit.IsChecked;
            dtpVisitFromDate.IsEnabled = false;
            dtpVisitToDate.IsEnabled = false;
            dtpVisitFromDate.SelectedDate = null;
            dtpVisitToDate.SelectedDate = null;
        }

        private void dtpVisitFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpVisitFromDate.SelectedDate != null && dtpVisitToDate.SelectedDate != null)
            {
                if (dtpVisitToDate.SelectedDate < dtpVisitFromDate.SelectedDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Should be Less than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    dtpVisitFromDate.SelectedDate = null;
                }
            }
        }

        private void dtpVisitToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpRegistrationFromDate.SelectedDate != null && dtpVisitToDate.SelectedDate != null)
            {
                if (dtpVisitToDate.SelectedDate < dtpRegistrationFromDate.SelectedDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Should be Less than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    dtpVisitToDate.SelectedDate = null;
                }
            }
        }

        private void chkAdmission_Unchecked(object sender, RoutedEventArgs e)
        {
            AdmissionWise = (bool)chkAdmission.IsChecked;
            dtpAdmissionFromDate.IsEnabled = false;
            dtpAdmissionToDate.IsEnabled = false;
            dtpAdmissionFromDate.SelectedDate = null;
            dtpAdmissionToDate.SelectedDate = null;
        }

        private void chkAdmission_Checked(object sender, RoutedEventArgs e)
        {
            AdmissionWise = (bool)chkAdmission.IsChecked;
            dtpAdmissionFromDate.IsEnabled = true;
            dtpAdmissionToDate.IsEnabled = true;
        }

        private void dtpAdmissionFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpAdmissionFromDate.SelectedDate != null && dtpAdmissionToDate.SelectedDate != null)
            {
                if (dtpAdmissionToDate.SelectedDate < dtpAdmissionFromDate.SelectedDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Should be Less than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    dtpAdmissionFromDate.SelectedDate = null;
                }
            }
        }

        private void dtpAdmissionToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpAdmissionFromDate.SelectedDate != null && dtpAdmissionToDate.SelectedDate != null)
            {
                if (dtpAdmissionToDate.SelectedDate < dtpAdmissionFromDate.SelectedDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Should be Less than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    dtpAdmissionToDate.SelectedDate = null;
                }
            }
        }
    }
}

