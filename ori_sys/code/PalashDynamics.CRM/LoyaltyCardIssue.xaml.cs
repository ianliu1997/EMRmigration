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
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Billing;
using CIMS;
using PalashDynamics.Collections;


namespace PalashDynamics.CRM
{
    public partial class LoyaltyCardIssue : UserControl
    {
        #region Paging

        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }

        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }



        #endregion
       
        public LoyaltyCardIssue()
        {
            InitializeComponent();

            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }
       int ClickedFlag = 0;
        #region Variable Declaration
        bool IsPageLoded = false;
        public bool UnIssue;

        WaitIndicator Indicatior = null;

        #endregion

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }


        private void LoyaltyCardIssue_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                      && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                    {
                        //do  nothing
                    }
                    else
                        CmdIssue.IsEnabled = false;
                }
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.DataContext = new clsPatientVO();
                FillUnitList();
                SetComboboxValue();
                FetchData();
                dtpFromDate.Focus();
                Indicatior.Close();

            }
            IsPageLoded = true;
            dtpFromDate.Focus();
            dtpFromDate.UpdateLayout();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
           
        }
       
        #region FillCombobox
        private void FillUnitList()
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
                    cmbClinic.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbClinic.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }
        #endregion
       
        private void SetComboboxValue()
        {
            cmbClinic.SelectedValue = (clsPatientVO)this.DataContext;
        }
       
        /// <summary>
        /// Purpose:Send Patient for issue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdIssue_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientList.ItemsSource != null)
            {
                if ((clsPatientGeneralVO)dgPatientList.SelectedItem != null)
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                    {

                       

                        LoyaltyCardIssueTemplate win = new LoyaltyCardIssueTemplate();
                        win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                        win.Show();
                    }
                }

                else
                {
                    string msgTitle = "";
                    string msgText = "Please Select Patient";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();

                    
                }
            }
           
            
        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                if (((PalashDynamics.CRM.LoyaltyCardIssueTemplate)sender).DialogResult == true)
                {
                    Indicatior = new WaitIndicator();
                    Indicatior.Show();
                    try
                    {
                        clsUpdatePatientForIssueBizActionVO BizAction = new clsUpdatePatientForIssueBizActionVO();

                        BizAction.PatientDetails = (clsPatientVO)this.DataContext;
                        BizAction.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        BizAction.PatientDetails.LoyaltyCardNo = (((LoyaltyCardIssueTemplate)sender).txtLoyaltyCardNumber.Text);
                        BizAction.PatientDetails.PreferNameonLoyaltyCard = (((LoyaltyCardIssueTemplate)sender).txtPreferName.Text);

                        BizAction.PatientDetails.IssueDate = (((LoyaltyCardIssueTemplate)sender).dtpIssueDate.SelectedDate);
                        BizAction.PatientDetails.EffectiveDate = (((LoyaltyCardIssueTemplate)sender).dtpEffectiveDate.SelectedDate);
                        BizAction.PatientDetails.ExpiryDate = (((LoyaltyCardIssueTemplate)sender).dtpExpiryDate.SelectedDate);

                        if (((LoyaltyCardIssueTemplate)sender).cmbCardType.SelectedItem != null)
                            BizAction.PatientDetails.LoyaltyCardID = ((MasterListItem)(((LoyaltyCardIssueTemplate)sender).cmbCardType.SelectedItem)).ID;


                        if (((LoyaltyCardIssueTemplate)sender).cmbTariff.SelectedItem != null)
                            BizAction.PatientDetails.TariffID = (((LoyaltyCardIssueTemplate)sender).LTariffID);

                        BizAction.PatientDetails.Remark = (((LoyaltyCardIssueTemplate)sender).txtRemarks.Text);

                        BizAction.PatientDetails.FamilyDetails = (((LoyaltyCardIssueTemplate)sender).FamilyList);

                        BizAction.PatientDetails.OtherDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        BizAction.PatientDetails.OtherDetails.Question1 = ((LoyaltyCardIssueTemplate)sender).Question1.IsChecked;
                        BizAction.PatientDetails.OtherDetails.Question2 = ((LoyaltyCardIssueTemplate)sender).Question2.IsChecked;
                        BizAction.PatientDetails.OtherDetails.Question3 = ((LoyaltyCardIssueTemplate)sender).Question3.IsChecked;
                        BizAction.PatientDetails.OtherDetails.Question4 = ((LoyaltyCardIssueTemplate)sender).Question4.IsChecked;
                        BizAction.PatientDetails.OtherDetails.Question4Details = ((LoyaltyCardIssueTemplate)sender).txtSpecify.Text;
                        BizAction.PatientDetails.OtherDetails.Question5A = ((LoyaltyCardIssueTemplate)sender).Question5A.IsChecked;
                        BizAction.PatientDetails.OtherDetails.Question5B = ((LoyaltyCardIssueTemplate)sender).Question5B.IsChecked;
                        BizAction.PatientDetails.OtherDetails.Question5C = ((LoyaltyCardIssueTemplate)sender).Question5C.IsChecked;
                        BizAction.PatientDetails.OtherDetails.PatientUnitID = ((LoyaltyCardIssueTemplate)sender).PatientUnitID;

                        if (((LoyaltyCardIssueTemplate)sender).ServiceList.Count != 0)
                        {
                            BizAction.PatientDetails.ServiceDetails = (((LoyaltyCardIssueTemplate)sender).ServiceList);
                        }


                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            ClickedFlag = 0;
                            if (arg.Error == null)
                            {
                               
                                FetchData();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Loyalty card issued successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }

                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding  patient for issue .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();
                            }

                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();

                    }
                    catch (Exception ex)
                    {
                        throw;

                    }
                    finally
                    {
                        Indicatior.Close();
                    }

                }

            }

            
           
            
        }

        /// <summary>
        /// Purpose:Getting list of patient.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (dtpToDate.SelectedDate < dtpFromDate.SelectedDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "To date can not be less than From date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();

            }
            else
            {
                FetchData();
            }
           
        }

        private void FetchData()
        {
            clsGetPatientForLoyaltyCardIssueBizActionVO BizAction = new clsGetPatientForLoyaltyCardIssueBizActionVO();
            BizAction.PatientDetails = new List<clsPatientGeneralVO>();

            BizAction.FromDate = dtpFromDate.SelectedDate;
            if (dtpToDate.SelectedDate != null)
            {
                BizAction.ToDate = dtpToDate.SelectedDate.Value.AddDays(1);
            }
            
            if (txtMrno.Text != null)
                BizAction.MrNo = txtMrno.Text;

            if (txtOPDNo.Text != null)
                BizAction.OPDNo = txtOPDNo.Text;

            if (txtFirstName.Text != null)
                BizAction.FirstName = txtFirstName.Text;

            if (txtMiddleName.Text != null)
                BizAction.MiddleName = txtMiddleName.Text;

            if (txtLastName.Text != null)
                BizAction.LastName = txtLastName.Text;

            BizAction.IsLoyaltymember = UnIssue;

            if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID!=0)
            {
                BizAction.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

          //dgPatientList.ItemsSource = null;

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Result != null && arg.Error == null)
                {

                    if (((clsGetPatientForLoyaltyCardIssueBizActionVO)arg.Result).PatientDetails != null)
                    {
                        //dgPatientList.ItemsSource = ((clsGetPatientForLoyaltyCardIssueBizActionVO)arg.Result).PatientDetails;

                        clsGetPatientForLoyaltyCardIssueBizActionVO result = arg.Result as clsGetPatientForLoyaltyCardIssueBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.PatientDetails != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.PatientDetails)
                            {
                                DataList.Add(item);
                            }

                            dgPatientList.ItemsSource = null;
                            dgPatientList.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
                            dgPatientList.SelectedIndex = -1;

                        }

                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void dgPatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           ((IApplicationConfiguration)App.Current).SelectedPatient = ((clsPatientGeneralVO)dgPatientList.SelectedItem); 
        }

        #region Validation

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }
        #endregion

        
        private void CmdDeActive_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new clsPatientVO();
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            dtpFromDate.SelectedDate = null;
            dtpToDate.SelectedDate = null;
            txtOPDNo.Text = string.Empty;
            txtMrno.Text = string.Empty;
            cmbClinic.SelectedValue = (long)0;


            dgDataPager.PageIndex = 0;
            FetchData();
        }


       



    }
}

