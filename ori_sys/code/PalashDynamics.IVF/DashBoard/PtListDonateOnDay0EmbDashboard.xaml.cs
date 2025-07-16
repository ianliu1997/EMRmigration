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
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class PtListDonateOnDay0EmbDashboard : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCloseButtonClick;
        bool isLoaded = false;
        public long PatientCategoryID { get; set; }
        public string Mrno { get; set; }
        public string PatientName { get; set; }
        public string SearchKeyword { get; set; }
        public clsCoupleVO CurrentCycleCoupleDetails;
        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }

        #region Properties
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

        public PtListDonateOnDay0EmbDashboard()
        {
            InitializeComponent();       
            this.Loaded += new RoutedEventHandler(PatientSearch_Loaded);
            this.DataContext = new clsPatientGeneralVO();

            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;
        }
        
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }

        void PatientSearch_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            txtFirstName.Focus();
            if (PatientName!=null)
            txtFirstName.Text = PatientName;
            if (Mrno != null)
            txtLastName.Text= Mrno;
            GetData();           
        }
        
        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
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

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        string textBefore = null;

        private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsNumberValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;


                }
            }
        }

        #endregion

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }
       
        public void GetData()
        {
            GetPatientListForDashboardBizActionVO BizActionObject = new GetPatientListForDashboardBizActionVO();
            
            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();
           
            BizActionObject.VisitWise = false;

           // BizActionObject.PatientCategoryID = PatientCategoryID;

            if (txtFirstName.Text != "")
                BizActionObject.FirstName = txtFirstName.Text;
            if (txtMiddleName.Text != "")
                BizActionObject.MiddleName = txtMiddleName.Text;
            if (txtLastName.Text != "")
                BizActionObject.LastName = txtLastName.Text;
            if (txtFamilyName.Text != "")
                BizActionObject.FamilyName = txtFamilyName.Text;

            if (txtMrno.Text != "")
                BizActionObject.MRNo = txtMrno.Text;

            if (txtOPDNo.Text != "")
                BizActionObject.OPDNo = txtOPDNo.Text;

            if (txtContactNo.Text != "")
                BizActionObject.ContactNo = txtContactNo.Text;

            if (txtCivilID.Text != "")
                BizActionObject.CivilID = txtCivilID.Text;
            if (SearchKeyword != null)
                BizActionObject.SearchKeyword = SearchKeyword;
          
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizActionObject.UnitID = 0;
            }
            else
            {
                BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizActionObject.IsPagingEnabled = true;
            BizActionObject.MaximumRows = DataList.PageSize; ;
            BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        GetPatientListForDashboardBizActionVO result = ea.Result as GetPatientListForDashboardBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                        {
                            DataList.Add(person);
                        }
                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = DataList;

                        DataPager.Source = null;
                        DataPager.PageSize = BizActionObject.MaximumRows;
                        DataPager.Source = DataList;
                    }                    
                }               
            };
            client.ProcessAsync(BizActionObject, new clsUserVO());
            client.CloseAsync();
        }

        WaitIndicator wait = new WaitIndicator();
        
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null && ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID != 0)
            {
                try
                {
                    //SelectedPatient = ((clsPatientGeneralVO)dataGrid2.SelectedItem);
                    if (wait == null)
                    {
                        wait = new WaitIndicator();
                        wait.Show();
                    }

                    clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
                    BizAction.PatientID = ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID;
                    BizAction.PatientUnitID = ((clsPatientGeneralVO)dataGrid2.SelectedItem).UnitId;
                    BizAction.CoupleDetails = new clsCoupleVO();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                            {
                                BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                                BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                                CoupleDetails.MalePatient = new clsPatientGeneralVO();
                                CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                                CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;                                
                                ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = CoupleDetails;
                              

                                if (BizAction.CoupleDetails.MalePatient == null || BizAction.CoupleDetails.FemalePatient == null || BizAction.CoupleDetails.FemalePatient.PatientID == 0 || BizAction.CoupleDetails.MalePatient.PatientID == 0)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Only Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    wait.Close();
                                }
                                else if ((CurrentCycleCoupleDetails.FemalePatient.PatientID == CoupleDetails.FemalePatient.PatientID) && (CurrentCycleCoupleDetails.FemalePatient.UnitId == CoupleDetails.FemalePatient.UnitId))
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Can not donate to same patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    wait.Close();
                                }
                                else
                                {
                                    this.DialogResult = true;
                                    if (OnSaveButton_Click != null)
                                    {
                                        OnSaveButton_Click(this, new RoutedEventArgs());
                                        this.Close();
                                    }
                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Only Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                wait.Close();
                            }
                            wait.Close();
                        }
                        else
                        {
                            wait.Close();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    wait.Close();
                    throw ex;
                }           

                //this.DialogResult = true;
                //if (OnSaveButton_Click != null)
                //{  
                //    OnSaveButton_Click(this, new RoutedEventArgs());
                //    this.Close();
                //}
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCloseButtonClick != null)
            {
                OnCloseButtonClick(this, new RoutedEventArgs());
                this.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
            }
        }
    }
}

