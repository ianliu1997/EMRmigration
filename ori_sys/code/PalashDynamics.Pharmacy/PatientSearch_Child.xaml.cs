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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Collections;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.Pharmacy
{
    public partial class PatientSearch_Child : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        bool isLoaded = false;
        public bool VisitWise = false;
        public bool isfromCouterSale = false;
        public bool isfromMaterialConsumpation = false; //Added by AJ Date 29/12/2016
        public long StoreID { get; set; } 

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

        public PatientSearch_Child()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PatientSearch_Child_Loaded);
            this.DataContext = new clsPatientGeneralVO();

            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }

        void PatientSearch_Child_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            txtFirstName.Focus();
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
            dataGrid2.SelectedIndex = 0;
        }

        public void GetData()
        {
            clsGetPatientGeneralDetailsListBizActionVO BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();

            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();


            if (isfromCouterSale == true)
                BizActionObject.RegistrationWise = true;
            else
            {
                BizActionObject.VisitWise = VisitWise;
                if (VisitWise)
                {
                    BizActionObject.VisitFromDate = DateTime.Now.Date;
                    BizActionObject.VisitToDate = DateTime.Now.Date;
                }
            }

            BizActionObject.isfromMaterialConsumpation = isfromMaterialConsumpation;     //Added by AJ Date 29/12/2016      
            BizActionObject.StoreID = this.StoreID;
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
    }
}

