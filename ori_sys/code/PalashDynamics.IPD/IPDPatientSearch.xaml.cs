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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Collections;
using System.ComponentModel;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.IPD
{
    public partial class IPDPatientSearch : ChildWindow
    {
        #region variable Declaration
        public event RoutedEventHandler OnSaveButton_Click;
        bool isLoaded = false;
        public bool IsFromDischarge = false;
        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }
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

        public IPDPatientSearch()
        {
            InitializeComponent();
            this.DataContext = new clsPatientGeneralVO();
            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;
        }
        #region TextLostFocusEvents
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

        private void IPDPatientSearch_Loaded(object sender, RoutedEventArgs e)
        {
            FetchData();
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }
        #region Selection Changed Events
        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
        }
        #endregion
        #region Button Click Events
        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        #region Private Methods
        private void FetchData()
        {
            clsGetIPDPatientListBizActionVO BizActionObject = new clsGetIPDPatientListBizActionVO();
            BizActionObject.IPDPatientList = new List<clsPatientGeneralVO>();
            BizActionObject.GeneralDetails = new clsPatientGeneralVO();

            BizActionObject.IsFromDischarge = IsFromDischarge;

            if (!String.IsNullOrEmpty(txtFirstName.Text))
                BizActionObject.GeneralDetails.FirstName = txtFirstName.Text;
            else
                BizActionObject.GeneralDetails.FirstName = null;

            if (!String.IsNullOrEmpty(txtMiddleName.Text))
                BizActionObject.GeneralDetails.MiddleName = txtMiddleName.Text;
            else
                BizActionObject.GeneralDetails.MiddleName = null;

            if (!String.IsNullOrEmpty(txtLastName.Text))
                BizActionObject.GeneralDetails.LastName = txtLastName.Text;
            else
                BizActionObject.GeneralDetails.LastName = null;
            if (!String.IsNullOrEmpty(txtFamilyName.Text))
                BizActionObject.GeneralDetails.FamilyName = txtFamilyName.Text;
            else
                BizActionObject.GeneralDetails.FamilyName = null;
            if (!String.IsNullOrEmpty(txtMrno.Text))
                BizActionObject.GeneralDetails.MRNo = txtMrno.Text;
            else
                BizActionObject.GeneralDetails.MRNo = null;
            if (!String.IsNullOrEmpty(txtIPDAdmissionNo.Text))
                BizActionObject.GeneralDetails.IPDAdmissionNo = txtIPDAdmissionNo.Text;
            else
                BizActionObject.GeneralDetails.IPDAdmissionNo = null;

            BizActionObject.GeneralDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;

            BizActionObject.IsPagingEnabled = true;
            BizActionObject.MaximumRows = DataList.PageSize; ;
            BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    clsGetIPDPatientListBizActionVO result = ea.Result as clsGetIPDPatientListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;
                    DataList.Clear();
                    foreach (clsPatientGeneralVO person in result.IPDPatientList)
                    {
                        DataList.Add(person);
                    }
                    dataGrid2.ItemsSource = null;
                    dataGrid2.ItemsSource = DataList;
                    peopleDataPager.Source = null;
                    peopleDataPager.PageSize = BizActionObject.MaximumRows;
                    peopleDataPager.Source = DataList;
                }
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion
        #region KeyUP Event
        private void txtBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Back || e.Key == Key.Back)
            {
                FetchData();
            }
        }
        #endregion
    }
}

