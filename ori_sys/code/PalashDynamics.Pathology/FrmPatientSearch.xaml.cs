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

namespace PalashDynamics.Pathology
{
    public partial class FrmPatientSearch : ChildWindow
    {

        public event RoutedEventHandler OnSaveButton_Click;
        bool isLoaded = false;
        public bool VisitWise = false;



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

        public FrmPatientSearch()
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
            GetData();
            FillFrom();
          

        }
        List<MasterListItem> RegTypeList = new List<MasterListItem>();

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
            dataGrid2.SelectedIndex = 0;
        }

        public void FillFrom()
        {
            RegTypeList.Add(new MasterListItem(10, "-Select-"));
            RegTypeList.Add(new MasterListItem(0, "OPD"));
            RegTypeList.Add(new MasterListItem(1, "IPD"));
            RegTypeList.Add(new MasterListItem(2, "Pharmacy"));
            RegTypeList.Add(new MasterListItem(5, "Pathology"));
            cmbRegFrom.ItemsSource = RegTypeList;
            cmbRegFrom.SelectedItem = RegTypeList[0];
        }
        public void GetData()
        {
            clsGetPatientGeneralDetailsListBizActionVO BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();

            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();

            BizActionObject.VisitWise = VisitWise;
            if (VisitWise)
            {
                BizActionObject.VisitFromDate = DateTime.Now.Date;
                BizActionObject.VisitToDate = DateTime.Now.Date;
            }

              BizActionObject.ISDonorSerch = true;
             


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
            if (((MasterListItem)cmbRegFrom.SelectedItem) != null && ((MasterListItem)cmbRegFrom.SelectedItem).ID != 10)
            {
                // if (((MasterListItem)cmbRegFrom.SelectedItem).ID != 10)
                BizActionObject.RegistrationTypeID = ((MasterListItem)cmbRegFrom.SelectedItem).ID;
            }
            else
            {
                BizActionObject.RegistrationTypeID = 10;
            }
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
                        clsGetPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetPatientGeneralDetailsListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                        {

                            DataList.Add(person);
                        }

                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = DataList;

                        peopleDataPager.Source = null;
                        peopleDataPager.PageSize = BizActionObject.MaximumRows;
                        peopleDataPager.Source = DataList;
                    }
                    //  dataGrid2.SelectedIndex = 0;
                }

            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null && ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID != 0)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                {
                    OnSaveButton_Click(this, new RoutedEventArgs());
                    ////added by rohini dated 3.1.16
                    //((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
                    ////
                    this.Close();
                }
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
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void txtMrno_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
                peopleDataPager.PageIndex = 0;
            }

        }

        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
                //   dataGrid2.SelectedIndex = 0;
                peopleDataPager.PageIndex = 0;
            }

        }

        private void txtLastName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
                peopleDataPager.PageIndex = 0;
            }

        }

        private void txtContactNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
                peopleDataPager.PageIndex = 0;
            }
        }

        private void cmbLoyaltyProgram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetData();
        }

        private void SearchButton_Checked(object sender, RoutedEventArgs e)
        {

        }
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtAgeFrom_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() || Convert.ToInt64(((TextBox)sender).Text) <= 0 && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtAgeFrom_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
    }
}

