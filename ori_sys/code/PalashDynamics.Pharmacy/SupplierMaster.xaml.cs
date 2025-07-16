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
using CIMS;

using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.ComponentModel;
using PalashDynamics.Collections;
using System.Xml.Linq;
using System.IO;
using System.Windows.Resources;
using System.Reflection;
using PalashDynamics.ValueObjects.Master.Location;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration;




namespace PalashDynamics.Pharmacy
{
    public partial class SupplierMaster : UserControl, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Validation

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCode.Text = txtCode.Text.ToTitleCase();
            if (!string.IsNullOrEmpty(txtCode.Text))
            {

                txtCode.ClearValidationError();
            }
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtName.Text = txtName.Text.ToTitleCase();
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                txtName.ClearValidationError();
            }
        }

        private void txtContactperson2Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtContactperson2Email.Text.Length > 0)
            {
                if (txtContactperson2Email.Text.IsEmailValid())
                    txtContactperson2Email.ClearValidationError();
                else
                {
                    txtContactperson2Email.SetValidation("Please Enter valid Email Adderess");
                    txtContactperson2Email.RaiseValidationError();
                }
            }
        }

        private void txtContactperson1Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtContactperson1Email.Text.Length > 0)
            {
                if (txtContactperson1Email.Text.IsEmailValid())
                    txtContactperson1Email.ClearValidationError();
                else
                {
                    txtContactperson1Email.SetValidation("Please Enter valid Email Adderess");
                    txtContactperson1Email.RaiseValidationError();
                }
            }
        }


        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtContactperson1MobileNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {


                if (!((TextBox)sender).Text.IsNumberValid())
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((TextBox)sender).Text.Length > 15)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtContactperson1MobileNo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtContactperson2MobileNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {


                if (!((TextBox)sender).Text.IsNumberValid())
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((TextBox)sender).Text.Length > 15)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtContactperson2MobileNo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtPhoneNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            //{


            //    if (!((TextBox)sender).Text.IsNumberValid())
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = "";
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }

            //}
        }

        private void txtPhoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            //textBefore = ((TextBox)sender).Text;
            //selectionStart = ((TextBox)sender).SelectionStart;
            //selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtFax_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            //{


            //    if (!((TextBox)sender).Text.IsNumberValid())
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = "";
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }

            //}
        }

        private void txtFax_KeyDown(object sender, KeyEventArgs e)
        {

        }

        //private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    txtCountry.Text = txtCountry.Text.ToTitleCase();
        //}

        //private void txtState_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    txtState.Text = txtState.Text.ToTitleCase();
        //}

        //private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    txtCity.Text = txtCity.Text.ToTitleCase();
        //}

        private void txtAddressLine1_LostFocus(object sender, RoutedEventArgs e)
        {
            txtAddressLine1.Text = txtAddressLine1.Text.ToTitleCase();

        }

        private void txtAddressLine2_LostFocus(object sender, RoutedEventArgs e)
        {
            txtAddressLine2.Text = txtAddressLine2.Text.ToTitleCase();

        }

        private void txtAddressLine3_LostFocus(object sender, RoutedEventArgs e)
        {
            txtAddressLine3.Text = txtAddressLine3.Text.ToTitleCase();
        }

        private void txtContactperson1Name_LostFocus(object sender, RoutedEventArgs e)
        {
            txtContactperson1Name.Text = txtContactperson1Name.Text.ToTitleCase();

        }

        private void txtContactperson2Name_LostFocus(object sender, RoutedEventArgs e)
        {
            txtContactperson2Name.Text = txtContactperson2Name.Text.ToTitleCase();
        }

        public bool ChkValidation()
        {
            bool result = true;
            if (txtCode.Text.Trim() == "")
            {
                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                result= false;

            }
            else
            {
                txtCode.ClearValidationError();
            }
            if (txtName.Text.Trim() == "")
            {
                txtName.SetValidation("Please Enter Description");
                txtName.RaiseValidationError();
                txtName.Focus();
                result= false;
            }
            else
            {
                txtName.ClearValidationError();
            }

            if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsFertilityPoint == false) //***//
            {
                if (txtPANNumber.Text.Trim() == "")
                {
                    txtPANNumber.SetValidation("Please Enter PAN NO.");
                    txtPANNumber.RaiseValidationError();
                    txtPANNumber.Focus();
                    result = false;
                }
                else
                {
                    txtName.ClearValidationError();
                }
            }

            if (txtContactperson1MobileNo.Text.Trim() != "")
            {
                if (Convert.ToInt64(txtContactperson1MobileNo.Text.Trim().Length) < 9)
                {
                    txtContactperson1MobileNo.SetValidation("Please enter correct mobile number");
                    txtContactperson1MobileNo.RaiseValidationError();
                    txtContactperson1MobileNo.Focus();
                    result= false;
                }
            }
            else
                txtContactperson1MobileNo.ClearValidationError();

            if (txtContactperson2MobileNo.Text.Trim() != "")
            {
                if (Convert.ToInt64(txtContactperson2MobileNo.Text.Trim().Length) < 9)
                {
                    txtContactperson2MobileNo.SetValidation("Please enter correct mobile number");
                    txtContactperson2MobileNo.RaiseValidationError();
                    txtContactperson2MobileNo.Focus();
                    result= false;
                }
            }
            else
                txtContactperson2MobileNo.ClearValidationError();

            if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsFertilityPoint == false) //***//
            {
                if (string.IsNullOrEmpty(txtGSTINNumber.Text.Trim()))
                {
                    txtGSTINNumber.SetValidation("Please enter GSTIN number");
                    txtGSTINNumber.RaiseValidationError();
                    txtGSTINNumber.Focus();
                    result = false;
                }
                else if (Convert.ToInt64(txtGSTINNumber.Text.Trim().Length) < 15 || Convert.ToInt64(txtGSTINNumber.Text.Trim().Length) > 15)
                {
                    txtGSTINNumber.SetValidation("Please enter valid GSTIN number");
                    txtGSTINNumber.RaiseValidationError();
                    txtGSTINNumber.Focus();
                    result = false;
                }
                else
                    txtGSTINNumber.ClearValidationError();
            }

            //if (((MasterListItem)cmbCity.SelectedItem) == null)
            //{
            //    cmbCity.TextBox.SetValidation("Please select City");
            //    cmbCity.TextBox.RaiseValidationError();
            //    cmbCity.Focus();
            //    result = false;
            //}
            //if (((MasterListItem)cmbCity.SelectedItem).ID == 0)
            //{
            //    cmbCity.TextBox.SetValidation("Please select City");
            //    cmbCity.TextBox.RaiseValidationError();
            //    cmbCity.Focus();
            //    result = false;
            //}
            //else
            //{
            //    cmbCity.TextBox.ClearValidationError();
            //}
            return result;
        }

        #endregion

        #region  Variables
        private SwivelAnimation objAnimation;
        private long SupplierId;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        public PagedSortableCollectionView<SupplierVO> MasterList { get; private set; }
        ObservableCollection<SupplierVO> lstSupplier = new ObservableCollection<SupplierVO>();
        bool IsCancel = true;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public long SelectedState{get;set;}
        public long SelectedCity { get; set; }
        public long SelectedArea { get; set; }
        #endregion

        #region Properties
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                OnPropertyChanged("PageSize");
            }
        }
        #endregion

        #region Constructor
        public SupplierMaster()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SupplierMaster_Loaded);
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            FillModeofPayment();
            FillTermsofPayment();
            FillTaxNature();
            FillCurrence();
            SupplierId = 0;
            //ChkValidation();

            MasterList = new PagedSortableCollectionView<SupplierVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;

            this.dataGrid2Pager.DataContext = MasterList;
            this.grdSupplier.DataContext = MasterList;

            SetupPage();



        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            SetupPage();
        }
        #endregion

        #region Loaded Event
        void SupplierMaster_Loaded(object sender, RoutedEventArgs e)
        {

            //cmdModify.IsEnabled = false;
            //cmdSave.IsEnabled = false;

            FillCountry();
            //FillState(0, cmbState);

            //FillDistrict(0, cmbDistrict);
            //FillCity(0, cmbCity);
            //FillArea(0, cmbArea);
            //FillAddressLocation6(0, cmbAddressLocation6);
            FillSupplierCategory();

        }
        #endregion Loaded Event

        #region Public Methods
        /// <summary>
        /// This Method Is Use For Two Purpose It Fill DataGrid (All Supplier Details) and 
        /// When We click On View Hyperlink Button Then It Will Get Details of Supplier on Which we Click  
        /// </summary>
        PagedCollectionView SupplierView;
        public void SetupPage()
        {

            ClsGetSupplierDetailsBizActionVO bizActionVO = new ClsGetSupplierDetailsBizActionVO();
            bizActionVO.SearchExpression = txtSearch.Text;
            bizActionVO.PagingEnabled = true;
            bizActionVO.MaximumRows = MasterList.PageSize;
            bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;


            SupplierVO getsupplierinfo = new SupplierVO();
            bizActionVO.ItemMatserDetails = new List<SupplierVO>();
            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {


                        bizActionVO.ItemMatserDetails = (((ClsGetSupplierDetailsBizActionVO)args.Result).ItemMatserDetails);

                        ///Setup Page Fill DataGrid
                        if (SupplierId == 0 && bizActionVO.ItemMatserDetails.Count > 0)
                        {
                            MasterList.Clear();
                            //lstSupplier.Clear();
                            MasterList.TotalItemCount = (int)(((ClsGetSupplierDetailsBizActionVO)args.Result).TotalRows);
                            foreach (SupplierVO item in bizActionVO.ItemMatserDetails)
                            {
                                MasterList.Add(item);
                                //lstSupplier.Add(item);
                            }
                        }

                        //SupplierView = new PagedCollectionView(lstSupplier);

                        //grdSupplier.ItemsSource = null;
                        //grdSupplier.ItemsSource = SupplierView;

                        //dataGrid2Pager.Source = null;
                        //dataGrid2Pager.PageSize = (int)bizActionVO.MaximumRows; //result.TotalRows;
                        //dataGrid2Pager.Source = MasterList;

                        #region Commented
                        ////When Click on View Button
                        //if (SupplierId > 0 && bizActionVO.ItemMatserDetails.Count >0)
                        //{
                        //    getsupplierinfo = bizActionVO.ItemMatserDetails[0];
                        //    SupplierId = getsupplierinfo.SupplierId;
                        //    txtCode.Text = getsupplierinfo.Code;
                        //    txtName.Text = getsupplierinfo.SupplierName;
                        //    txtAddressLine1.Text = getsupplierinfo.Address1;
                        //    txtAddressLine2.Text = getsupplierinfo.Address2;
                        //    txtAddressLine3.Text = getsupplierinfo.Address3;
                        //    txtCity.Text = getsupplierinfo.City;
                        //    txtState.Text = getsupplierinfo.State;
                        //    txtCountry.Text = getsupplierinfo.Country;
                        //    txtPincode.Text = getsupplierinfo.Pincode;
                        //    txtContactperson1Name.Text = getsupplierinfo.ContactPerson1Name;
                        //    txtContactperson1MobileNo.Text = getsupplierinfo.ContactPerson1MobileNo;
                        //    txtContactperson1Email.Text = getsupplierinfo.ContactPerson1Email;
                        //    txtContactperson2Name.Text = getsupplierinfo.ContactPerson2Name;
                        //    txtContactperson2MobileNo.Text = getsupplierinfo.ContactPerson2MobileNo;
                        //    txtContactperson2Email.Text = getsupplierinfo.ContactPerson2Email;
                        //    txtPhoneNo.Text = getsupplierinfo.PhoneNo;
                        //    txtFax.Text = getsupplierinfo.Fax;
                        //    cmbModeofPayment.SelectedValue = getsupplierinfo.ModeOfPayment;
                        //    cmbCurrency.SelectedValue = getsupplierinfo.Currency;
                        //    cmbTaxNature.SelectedValue = getsupplierinfo.TaxNature;
                        //    cmbTermsofPayment.SelectedValue = getsupplierinfo.TermofPayment;
                        //    txtMSTNumber.Text = getsupplierinfo.MSTNumber;
                        //    txtCSTNumber.Text = getsupplierinfo.CSTNumber;
                        //    txtVATNumber.Text = getsupplierinfo.VAT;
                        //    txtDrugLicenceNumber.Text = getsupplierinfo.DRUGLicence;
                        //    txtServiceTaxNumber.Text = getsupplierinfo.ServiceTaxNumber;
                        //    txtPANNumber.Text = getsupplierinfo.PANNumber;

                        //}
                        #endregion

                    }

                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// When We Click On Add Button All UI Must Empty
        /// </summary>
        public void ClearUI()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtAddressLine1.Text = "";
            txtAddressLine2.Text = "";
            txtAddressLine3.Text = "";
            //txtCity.Text = "";
            //txtState.Text = "";

            //txtCountry.Text = "";
            //txtPincode.Text = "";

            txtPOAutoCancelDays.Text = string.Empty;                // To Add PO Closing duration

            txtContactperson1Name.Text = "";
            txtContactperson1MobileNo.Text = "";
            txtContactperson1Email.Text = "";
            txtContactperson2Name.Text = "";
            txtContactperson2MobileNo.Text = "";
            txtContactperson2Email.Text = "";
            txtPhoneNo.Text = "";
            txtFax.Text = "";


            cmbCountry.SelectedValue = (long)0;
          //  cmbState.SelectedValue = (long)0;
        //    cmbDistrict.SelectedValue = (long)0;
          //  cmbCity.SelectedValue = (long)0;
           // cmbArea.SelectedValue = (long)0;
         //   cmbAddressLocation6.SelectedValue = (long)0;
            txtPinCode.Text = "";


            MasterListItem Defaultc = ((List<MasterListItem>)cmbCurrency.ItemsSource).FirstOrDefault(s => s.ID == 0);
            cmbCurrency.SelectedItem = Defaultc;
            Defaultc = ((List<MasterListItem>)cmbModeofPayment.ItemsSource).FirstOrDefault(s => s.ID == 0);
            cmbModeofPayment.SelectedItem = Defaultc;
            if (cmbTaxNature.ItemsSource != null)
            {
                Defaultc = ((List<MasterListItem>)cmbTaxNature.ItemsSource).FirstOrDefault(s => s.ID == 0);

                cmbTaxNature.SelectedItem = Defaultc;
            }
            if (cmbTermsofPayment.ItemsSource != null)
            {
                Defaultc = ((List<MasterListItem>)cmbTermsofPayment.ItemsSource).FirstOrDefault(s => s.ID == 0);
                cmbTermsofPayment.SelectedItem = Defaultc;
            }
            txtMSTNumber.Text = "";
            txtCSTNumber.Text = "";
            txtVATNumber.Text = "";
            txtDrugLicenceNumber.Text = "";
            txtServiceTaxNumber.Text = "";
            txtPANNumber.Text = "";

            cmbSupplierCategory.SelectedValue = (long)0;
            txtDepreciation.Text = string.Empty;
            txtRatingSystem.Text = string.Empty;

        }
        #endregion Public Methods

        #region Methods For Fill All Comboboxes
        public void FillModeofPayment()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ModeOfPayment;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();


                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbModeofPayment.ItemsSource = null;
                    cmbModeofPayment.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillTermsofPayment()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TermsofPayment;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();


                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbTermsofPayment.ItemsSource = null;
                    cmbTermsofPayment.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();

        }
        public void FillTaxNature()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TaxNature;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();


                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbTaxNature.ItemsSource = null;
                    cmbTaxNature.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCurrence()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Currency;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();


                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbCurrency.ItemsSource = null;
                    cmbCurrency.ItemsSource = objList;

                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion


        #region Button Click Events

        /// <summary>
        /// This Event is Call When We click on Add Button, and show Back Panel Which Have Supplier Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            ChkValidation();
            SetCommandButtonState("New");
            ClearUI();

            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This Event is Call When We click on Cancel Button, and show Front Panel On Which DataGrid Which
        /// Have All Supplier List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            SupplierId = 0;

            txtSearch.Text = string.Empty;

            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {

                ModuleName = "PalashDynamics.Administration";
                Action = "PalashDynamics.Administration.frmInventoryConfiguration";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Inventory Configuration";
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                IsCancel = true;
                SetupPage();

            }
        }
        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);




            }
            catch (Exception ex)
            {
                throw;
            }



        }


        /// <summary>
        /// This Event is Call When We click on Modify Button and Update Supplier Details
        /// (For Add and Modify Supplier Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidation())
            {

                string msgTitle = "";
                string msgText = "Are you sure you want to Update ?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW.Show();
            }
            else
            { ChkValidation(); }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();
        }

        private void Modify()
        {
            clsAddUpdateSupplierBizActionVO bizactionVO = new clsAddUpdateSupplierBizActionVO();
            SupplierVO addNewSupplierVO = new SupplierVO();
            try
            {
                addNewSupplierVO.SupplierId = SupplierId;
                addNewSupplierVO.Code = txtCode.Text;
                addNewSupplierVO.SupplierName = txtName.Text;

/* -x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x Begin : Below Conditional Statment is to check if the object is empty of null and assign the AutoCancelDays variable with the object value x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x- */

                if (!string.IsNullOrEmpty(txtPOAutoCancelDays.Text))
                    addNewSupplierVO.POAutoCloseDays = Convert.ToInt32(txtPOAutoCancelDays.Text);
                else
                    addNewSupplierVO.POAutoCloseDays = 0;

/* -x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x End : Below Conditional Statment is to check if the object is empty of null and assign the AutoCancelDays variable with the object value x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x- */

                addNewSupplierVO.Address1 = txtAddressLine1.Text;
                addNewSupplierVO.Address2 = txtAddressLine2.Text;
                addNewSupplierVO.Address3 = txtAddressLine3.Text;

                if (cmbCountry.SelectedItem != null)
                    addNewSupplierVO.Country = ((MasterListItem)cmbCountry.SelectedItem).ID;
                if (cmbState.SelectedItem != null)
                    addNewSupplierVO.State = ((MasterListItem)cmbState.SelectedItem).ID;

                //if (cmbDistrict.SelectedItem != null)
                //    addNewSupplierVO.District = ((MasterListItem)cmbDistrict.SelectedItem).ID;
                if (cmbCity.SelectedItem != null)
                    addNewSupplierVO.City = ((MasterListItem)cmbCity.SelectedItem).ID;
                if (cmbArea.SelectedItem != null)
                    addNewSupplierVO.Area = ((MasterListItem)cmbArea.SelectedItem).ID;


                //if (cmbAddressLocation6.SelectedItem != null)
                //    addNewSupplierVO.AddressLocation6ID = ((clsAddressLocation6VO)cmbAddressLocation6.SelectedItem).ID;






                //addNewSupplierVO.City = txtCity.Text;
                //addNewSupplierVO.State = txtState.Text;
                //addNewSupplierVO.Country = txtCountry.Text;
                //addNewSupplierVO.Pincode = txtPincode.Text;
                addNewSupplierVO.ContactPerson1Name = txtContactperson1Name.Text;
                addNewSupplierVO.ContactPerson1MobileNo = txtContactperson1MobileNo.Text;
                addNewSupplierVO.ContactPerson1Email = txtContactperson1Email.Text;
                addNewSupplierVO.ContactPerson2Name = txtContactperson2Name.Text;
                addNewSupplierVO.ContactPerson2MobileNo = txtContactperson2MobileNo.Text;
                addNewSupplierVO.ContactPerson2Email = txtContactperson2Email.Text;
                addNewSupplierVO.PhoneNo = txtPhoneNo.Text;
                addNewSupplierVO.Fax = txtFax.Text;
                addNewSupplierVO.Pincode = txtPinCode.Text;

                //Added by MMBABU
                if (cmbSupplierCategory.SelectedItem != null)
                    addNewSupplierVO.SupplierCategoryId = ((MasterListItem)cmbSupplierCategory.SelectedItem).ID;

                addNewSupplierVO.Depreciation = txtDepreciation.Text;
                addNewSupplierVO.RatingSystem = txtRatingSystem.Text;

                //END

                //addNewSupplierVO.ModeOfPayment = Convert.ToInt64(cmbModeofPayment.SelectedValue);
                //addNewSupplierVO.Currency = Convert.ToInt64(cmbCurrency.SelectedValue);
                //addNewSupplierVO.TaxNature = Convert.ToInt64(cmbTaxNature.SelectedValue);
                //addNewSupplierVO.TermofPayment = Convert.ToInt64(cmbTermsofPayment.SelectedValue);

                //Added by Priyanka
                if (cmbModeofPayment.SelectedItem != null && ((MasterListItem)cmbModeofPayment.SelectedItem).ID != 0)
                {
                    addNewSupplierVO.ModeOfPayment = ((MasterListItem)cmbModeofPayment.SelectedItem).ID;
                }
                if (cmbCurrency.SelectedItem != null && ((MasterListItem)cmbCurrency.SelectedItem).ID != 0)
                {
                    addNewSupplierVO.Currency = ((MasterListItem)cmbCurrency.SelectedItem).ID;
                }
                if (cmbTaxNature.SelectedItem != null && ((MasterListItem)cmbTaxNature.SelectedItem).ID != 0)
                {
                    addNewSupplierVO.TaxNature = ((MasterListItem)cmbTaxNature.SelectedItem).ID;
                }
                if (cmbTermsofPayment.SelectedItem != null && ((MasterListItem)cmbTermsofPayment.SelectedItem).ID != 0)
                {
                    addNewSupplierVO.TermofPayment = ((MasterListItem)cmbTermsofPayment.SelectedItem).ID;
                }
                //Added by Pallavi
                addNewSupplierVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                addNewSupplierVO.MSTNumber = txtMSTNumber.Text;
                addNewSupplierVO.CSTNumber = txtCSTNumber.Text;
                addNewSupplierVO.VAT = txtVATNumber.Text;
                addNewSupplierVO.DRUGLicence = txtDrugLicenceNumber.Text;
                addNewSupplierVO.ServiceTaxNumber = txtServiceTaxNumber.Text;
                addNewSupplierVO.PANNumber = txtPANNumber.Text;
                addNewSupplierVO.MFlag = false;//I dont Know Which Value I assign it Sarang didnt tell  me
                addNewSupplierVO.GSTINNo = Convert.ToString(txtGSTINNumber.Text.Trim());
                addNewSupplierVO.Status = ((SupplierVO)grdSupplier.SelectedItem).Status;
                addNewSupplierVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                addNewSupplierVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                addNewSupplierVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                addNewSupplierVO.UpdatedDateTime = System.DateTime.Now;
                addNewSupplierVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                addNewSupplierVO.IsFertilityPoint = (((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsFertilityPoint; //***//19

                bizactionVO.ItemMatserDetails.Add(addNewSupplierVO);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {

                        if (((clsAddUpdateSupplierBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            SupplierId = 0;
                            SetupPage();
                            msgText = "Record Updated Successfully!";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();

                            //After Updation Back to BackPanel and Setup Page
                            objAnimation.Invoke(RotationType.Backward);
                            SetCommandButtonState("Modify");
                            //cmdAdd.IsEnabled = true;
                            //cmdModify.IsEnabled = false;

                        }
                        else if (((clsAddUpdateSupplierBizActionVO)args.Result).SuccessStatus == 2)
                        {
                            msgText = "Record cannot be update because CODE already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (((clsAddUpdateSupplierBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            msgText = "Record cannot be update because DESCRIPTION already exist with same PAN No.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }

                    }

                };
                client.ProcessAsync(bizactionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }

        #region Commented
        //txtCode.Text;
        //   txtName.Text;
        //   txtAddressLine1.Text;
        //   txtAddressLine2.Text;
        //   txtAddressLine3.Text;
        //   txtCity.Text;
        //   txtState.Text;

        //   txtCountry.Text;
        //   txtPincode.Text;
        //   txtContactperson1Name.Text;
        //   txtContactperson1MobileNo.Text;
        //   txtContactperson1Email.Text;
        //   txtContactperson2Name.Text;
        //   txtContactperson2MobileNo.Text;
        //   txtContactperson2Email.Text;
        //   txtPhoneNo.Text;
        //   txtFax.Text;
        //   cmbModeofPayment.SelectedValue;
        //   cmbCurrency.SelectedValue;
        //   cmbTaxNature.SelectedValue;
        //   cmbTermsofPayment.SelectedValue;

        //   txtMSTNumber.Text;
        //   txtCSTNumber.Text;
        //   txtVATNumber.Text;
        //   txtDrugLicenceNumber.Text;
        //   txtServiceTaxNumber.Text;
        //   txtPANNumber.Text;
        #endregion

        /// <summary>
        /// This Event is Call When We click on Save Button and Save Supplier Details
        /// (For Add and Modify Supplier Details, only One VO 
        /// and Procedure Which have Both Functionality)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

            bool SaveSupplier = true;
            SaveSupplier = ChkValidation();
            if (((MasterListItem)cmbCity.SelectedItem) == null)
            {
                cmbCity.TextBox.SetValidation("Please select City");
                cmbCity.TextBox.RaiseValidationError();
                cmbCity.Focus();
                SaveSupplier = false;
            }
       
            else if (((MasterListItem)cmbCity.SelectedItem).ID == 0)
            {
                cmbCity.TextBox.SetValidation("Please select City");
                cmbCity.TextBox.RaiseValidationError();
                cmbCity.Focus();
                SaveSupplier = false;
            }
            else
            {
                cmbCity.TextBox.ClearValidationError();
            }

            if (SaveSupplier == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Save ?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
            else
            { ChkValidation(); }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

        public void Save()
        {
            //cmdSave.IsEnabled = false;
            clsAddUpdateSupplierBizActionVO bizactionVO = new clsAddUpdateSupplierBizActionVO();
            SupplierVO addNewSupplierVO = new SupplierVO();
            try
            {
                addNewSupplierVO.SupplierId = 0;
                addNewSupplierVO.Code = txtCode.Text;
                addNewSupplierVO.SupplierName = txtName.Text;


/* -x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x Begin : Below Conditional Statment is to check if the object is empty of null and assign the AutoCancelDays variable with the object value x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x- */
                
                if (!string.IsNullOrEmpty(txtPOAutoCancelDays.Text))
                    addNewSupplierVO.POAutoCloseDays = Convert.ToInt32(txtPOAutoCancelDays.Text);
                else
                    addNewSupplierVO.POAutoCloseDays = 0;

/* -x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x End : Below Conditional Statment is to check if the object is empty of null and assign the AutoCancelDays variable with the object value x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x- */


                addNewSupplierVO.Address1 = txtAddressLine1.Text;
                addNewSupplierVO.Address2 = txtAddressLine2.Text;
                addNewSupplierVO.Address3 = txtAddressLine3.Text;
                //addNewSupplierVO.City = txtCity.Text;
                //addNewSupplierVO.State = txtState.Text;
                //addNewSupplierVO.Country = txtCountry.Text;
                //addNewSupplierVO.Pincode = txtPincode.Text;

                if (cmbCountry.SelectedItem != null)
                    addNewSupplierVO.Country = ((MasterListItem)cmbCountry.SelectedItem).ID;
                if (cmbState.SelectedItem != null)
                    addNewSupplierVO.State = ((MasterListItem)cmbState.SelectedItem).ID;  //(PalashDynamics.ValueObjects.Administration.clsStateVO)

                //if (cmbDistrict.SelectedItem != null)
                //    addNewSupplierVO.District = ((clsDistVO)cmbDistrict.SelectedItem).DistID;
                if (cmbCity.SelectedItem != null)
                    addNewSupplierVO.City = ((MasterListItem)cmbCity.SelectedItem).ID;  //(PalashDynamics.ValueObjects.Administration.clsCityVO)
                if (cmbArea.SelectedItem != null)
                    addNewSupplierVO.Area = ((MasterListItem)cmbArea.SelectedItem).ID;  //clsAreaVO


                //if (cmbAddressLocation6.SelectedItem != null)
                //    addNewSupplierVO.AddressLocation6ID = ((clsAddressLocation6VO)cmbAddressLocation6.SelectedItem).ID;

                if (txtPinCode.Text != "")
                    addNewSupplierVO.Pincode = txtPinCode.Text;

                addNewSupplierVO.ContactPerson1Name = txtContactperson1Name.Text;
                addNewSupplierVO.ContactPerson1MobileNo = txtContactperson1MobileNo.Text;
                addNewSupplierVO.ContactPerson1Email = txtContactperson1Email.Text;
                addNewSupplierVO.ContactPerson2Name = txtContactperson2Name.Text;
                addNewSupplierVO.ContactPerson2MobileNo = txtContactperson2MobileNo.Text;
                addNewSupplierVO.ContactPerson2Email = txtContactperson2Email.Text;
                addNewSupplierVO.PhoneNo = txtPhoneNo.Text;
                addNewSupplierVO.Fax = txtFax.Text;

                //addNewSupplierVO.ModeOfPayment = Convert.ToInt64(cmbModeofPayment.SelectedValue);
                //addNewSupplierVO.Currency = Convert.ToInt64(cmbCurrency.SelectedValue);                
                //addNewSupplierVO.TaxNature = Convert.ToInt64(cmbTaxNature.SelectedValue);
                //addNewSupplierVO.TermofPayment = Convert.ToInt64(cmbTermsofPayment.SelectedValue);

                //Added by MMBABU
                if (cmbSupplierCategory.SelectedItem != null)
                    addNewSupplierVO.SupplierCategoryId = ((MasterListItem)cmbSupplierCategory.SelectedItem).ID;

                addNewSupplierVO.Depreciation = txtDepreciation.Text;
                addNewSupplierVO.RatingSystem = txtRatingSystem.Text;

                //END

                //Added by Priyanka
                addNewSupplierVO.ModeOfPayment = ((MasterListItem)cmbModeofPayment.SelectedItem).ID;
                addNewSupplierVO.Currency = ((MasterListItem)cmbCurrency.SelectedItem).ID;
                addNewSupplierVO.TaxNature = ((MasterListItem)cmbTaxNature.SelectedItem).ID;
                addNewSupplierVO.TermofPayment = ((MasterListItem)cmbTermsofPayment.SelectedItem).ID;

                addNewSupplierVO.MSTNumber = txtMSTNumber.Text;
                addNewSupplierVO.CSTNumber = txtCSTNumber.Text;
                addNewSupplierVO.VAT = txtVATNumber.Text;
                addNewSupplierVO.DRUGLicence = txtDrugLicenceNumber.Text;
                addNewSupplierVO.ServiceTaxNumber = txtServiceTaxNumber.Text;
                addNewSupplierVO.PANNumber = txtPANNumber.Text;
                addNewSupplierVO.MFlag = false;//I dont Know Which Value I assign it Sarang didnt tell  me
                addNewSupplierVO.Status = true;
                addNewSupplierVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                addNewSupplierVO.GSTINNo = Convert.ToString(txtGSTINNumber.Text.Trim());
                addNewSupplierVO.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                addNewSupplierVO.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                addNewSupplierVO.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                addNewSupplierVO.AddedDateTime = System.DateTime.Now;
                addNewSupplierVO.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                addNewSupplierVO.IsFertilityPoint = (((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsFertilityPoint; //***//19
                bizactionVO.ItemMatserDetails.Add(addNewSupplierVO);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsAddUpdateSupplierBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            SupplierId = 0;
                            SetupPage();
                            msgText = "Record Added Successfully!";

                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            //After Insertion Back to BackPanel and Setup Page
                            objAnimation.Invoke(RotationType.Backward);
                            SetCommandButtonState("Save");
                            //SetCommandButtonState("New");

                        }
                        else if (((clsAddUpdateSupplierBizActionVO)args.Result).SuccessStatus == 2)
                        {
                            msgText = "Record cannot be save because CODE already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }
                        else if (((clsAddUpdateSupplierBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            msgText = "Record cannot be save because DESCRIPTION with same city and same PAN No. is already exist!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindow.Show();
                        }

                    }

                };
                client.ProcessAsync(bizactionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// This Event is call When we Click On Hyperlink Button which is Present in DataGid 
        /// and Show Specific Supplier Details Which we Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        bool ViewSupplier = false;
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
          //  FillState(((SupplierVO)grdSupplier.SelectedItem).Country);
         //   FillCity(((SupplierVO)grdSupplier.SelectedItem).State);
         //   FillRegion(((SupplierVO)grdSupplier.SelectedItem).City);
           // ChkValidation();
            

            ViewSupplier = true;
            SetCommandButtonState("View");
            if (((SupplierVO)grdSupplier.SelectedItem).Status == false)
            {
                cmdModify.IsEnabled = false;
            }
            SupplierId = ((SupplierVO)grdSupplier.SelectedItem).SupplierId;

            txtCode.Text = ((SupplierVO)grdSupplier.SelectedItem).Code;
            txtName.Text = ((SupplierVO)grdSupplier.SelectedItem).SupplierName;
            txtPOAutoCancelDays.Text = ((SupplierVO)grdSupplier.SelectedItem).POAutoCloseDays.ToString();          // Set to show POAutoCancelDay on to the UI Object.
            txtAddressLine1.Text = ((SupplierVO)grdSupplier.SelectedItem).Address1;
            txtAddressLine2.Text = ((SupplierVO)grdSupplier.SelectedItem).Address2;
            txtAddressLine3.Text = ((SupplierVO)grdSupplier.SelectedItem).Address3;
            //txtCity.Text = ((SupplierVO)grdSupplier.SelectedItem).City;
            //txtState.Text = ((SupplierVO)grdSupplier.SelectedItem).State;
            //txtCountry.Text = ((SupplierVO)grdSupplier.SelectedItem).Country;
            //txtPincode.Text = ((SupplierVO)grdSupplier.SelectedItem).Pincode;



            cmbCountry.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).Country;
            SelectedState = ((SupplierVO)grdSupplier.SelectedItem).State;
            SelectedCity = ((SupplierVO)grdSupplier.SelectedItem).City;
            SelectedArea = ((SupplierVO)grdSupplier.SelectedItem).Area;
          //  cmbState.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).State;
            //cmbDistrict.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).District;
          //  cmbCity.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).City;
         //   cmbArea.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).Zone;
            //cmbAddressLocation6.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).AddressLocation6ID;

            txtPinCode.Text = ((SupplierVO)grdSupplier.SelectedItem).Pincode;

            txtContactperson1Name.Text = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson1Name;
            txtContactperson1MobileNo.Text = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson1MobileNo;
            txtContactperson1Email.Text = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson1Email;
            txtContactperson2Name.Text = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson2Name;
            txtContactperson2MobileNo.Text = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson2MobileNo;
            txtContactperson2Email.Text = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson2Email;
            txtPhoneNo.Text = ((SupplierVO)grdSupplier.SelectedItem).PhoneNo;
            txtFax.Text = ((SupplierVO)grdSupplier.SelectedItem).Fax;
            cmbModeofPayment.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).ModeOfPayment;
            cmbCurrency.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).Currency;
            cmbTaxNature.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).TaxNature;
            cmbTermsofPayment.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).TermofPayment;
            txtMSTNumber.Text = ((SupplierVO)grdSupplier.SelectedItem).MSTNumber;
            txtCSTNumber.Text = ((SupplierVO)grdSupplier.SelectedItem).CSTNumber;
            txtVATNumber.Text = ((SupplierVO)grdSupplier.SelectedItem).VAT;
            txtDrugLicenceNumber.Text = ((SupplierVO)grdSupplier.SelectedItem).DRUGLicence;
            txtServiceTaxNumber.Text = ((SupplierVO)grdSupplier.SelectedItem).ServiceTaxNumber;
            txtPANNumber.Text = ((SupplierVO)grdSupplier.SelectedItem).PANNumber;
            txtGSTINNumber.Text = Convert.ToString(((SupplierVO)grdSupplier.SelectedItem).GSTINNo);//Added By Bhushanp For GST 19062017
            //Added by MMBABU
            cmbSupplierCategory.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).SupplierCategoryId;
            txtDepreciation.Text = ((SupplierVO)grdSupplier.SelectedItem).Depreciation;
            txtRatingSystem.Text = ((SupplierVO)grdSupplier.SelectedItem).RatingSystem;

            //END

            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (grdSupplier.SelectedItem != null)
            {
                #region Commented
                //string msgTitle = "";
                //string msgText = "Are you sure you want to Update Status?";

                //MessageBoxControl.MessageBoxChildWindow msgWinStatus =
                //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                //msgWinStatus.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinStatus_OnMessageBoxClosed);

                //msgWinStatus.Show();
                //((SupplierVO)grdSupplier.SelectedItem).Status=Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                #endregion

                try
                {
                    clsAddUpdateSupplierBizActionVO bizactionVO = new clsAddUpdateSupplierBizActionVO();
                    SupplierVO addNewSupplierVO = new SupplierVO();
                    addNewSupplierVO.SupplierId = ((SupplierVO)grdSupplier.SelectedItem).SupplierId;
                    addNewSupplierVO.Code = ((SupplierVO)grdSupplier.SelectedItem).Code;
                    addNewSupplierVO.SupplierName = ((SupplierVO)grdSupplier.SelectedItem).SupplierName;
                    addNewSupplierVO.Address1 = ((SupplierVO)grdSupplier.SelectedItem).Address1;
                    addNewSupplierVO.Address2 = ((SupplierVO)grdSupplier.SelectedItem).Address2;
                    addNewSupplierVO.Address3 = ((SupplierVO)grdSupplier.SelectedItem).Address3;
                    addNewSupplierVO.City = ((SupplierVO)grdSupplier.SelectedItem).City;
                    addNewSupplierVO.State = ((SupplierVO)grdSupplier.SelectedItem).State;
                    addNewSupplierVO.Country = ((SupplierVO)grdSupplier.SelectedItem).Country;
                    addNewSupplierVO.Pincode = ((SupplierVO)grdSupplier.SelectedItem).Pincode;
                    addNewSupplierVO.ContactPerson1Name = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson1Name;
                    addNewSupplierVO.ContactPerson1MobileNo = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson1MobileNo;
                    addNewSupplierVO.ContactPerson1Email = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson1Email;
                    addNewSupplierVO.ContactPerson2Name = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson2Name;
                    addNewSupplierVO.ContactPerson2MobileNo = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson2MobileNo;
                    addNewSupplierVO.ContactPerson2Email = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson2Email;
                    addNewSupplierVO.PhoneNo = ((SupplierVO)grdSupplier.SelectedItem).PhoneNo;
                    addNewSupplierVO.Fax = ((SupplierVO)grdSupplier.SelectedItem).Fax;
                    addNewSupplierVO.ModeOfPayment = Convert.ToInt64(((SupplierVO)grdSupplier.SelectedItem).ModeOfPayment);
                    addNewSupplierVO.Currency = Convert.ToInt64(((SupplierVO)grdSupplier.SelectedItem).Currency);
                    addNewSupplierVO.TaxNature = Convert.ToInt64(((SupplierVO)grdSupplier.SelectedItem).TaxNature);
                    addNewSupplierVO.TermofPayment = Convert.ToInt64(((SupplierVO)grdSupplier.SelectedItem).TermofPayment);
                    addNewSupplierVO.MSTNumber = ((SupplierVO)grdSupplier.SelectedItem).MSTNumber;
                    addNewSupplierVO.CSTNumber = ((SupplierVO)grdSupplier.SelectedItem).CSTNumber;
                    addNewSupplierVO.VAT = ((SupplierVO)grdSupplier.SelectedItem).VAT;
                    addNewSupplierVO.DRUGLicence = ((SupplierVO)grdSupplier.SelectedItem).DRUGLicence;
                    addNewSupplierVO.ServiceTaxNumber = ((SupplierVO)grdSupplier.SelectedItem).ServiceTaxNumber;
                    addNewSupplierVO.PANNumber = ((SupplierVO)grdSupplier.SelectedItem).PANNumber;
                    addNewSupplierVO.MFlag = false;//I dont Know Which Value I assign it Sarang didnt tell  me
                    addNewSupplierVO.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                    addNewSupplierVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewSupplierVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewSupplierVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewSupplierVO.UpdatedDateTime = System.DateTime.Now;
                    addNewSupplierVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewSupplierVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateSupplierBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                SupplierId = 0;
                                SetupPage();
                                msgText = "Status Updated Successfully!";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                //cmdAdd.IsEnabled = true;
                                //cmdModify.IsEnabled = false;
                                SetCommandButtonState("Modify");

                            }

                        }

                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }



        }

        void msgWinStatus_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddUpdateSupplierBizActionVO bizactionVO = new clsAddUpdateSupplierBizActionVO();
                SupplierVO addNewSupplierVO = new SupplierVO();
                try
                {
                    addNewSupplierVO.SupplierId = ((SupplierVO)grdSupplier.SelectedItem).SupplierId;
                    addNewSupplierVO.Code = ((SupplierVO)grdSupplier.SelectedItem).Code;
                    addNewSupplierVO.SupplierName = ((SupplierVO)grdSupplier.SelectedItem).SupplierName;
                    addNewSupplierVO.Address1 = ((SupplierVO)grdSupplier.SelectedItem).Address1;
                    addNewSupplierVO.Address2 = ((SupplierVO)grdSupplier.SelectedItem).Address2;
                    addNewSupplierVO.Address3 = ((SupplierVO)grdSupplier.SelectedItem).Address3;
                    addNewSupplierVO.City = ((SupplierVO)grdSupplier.SelectedItem).City;
          //          addNewSupplierVO.Area = ((SupplierVO)grdSupplier.SelectedItem).Area;
                    addNewSupplierVO.State = ((SupplierVO)grdSupplier.SelectedItem).State;
                    addNewSupplierVO.Country = ((SupplierVO)grdSupplier.SelectedItem).Country;
                    addNewSupplierVO.Pincode = ((SupplierVO)grdSupplier.SelectedItem).Pincode;
                    addNewSupplierVO.ContactPerson1Name = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson1Name;
                    addNewSupplierVO.ContactPerson1MobileNo = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson1MobileNo;
                    addNewSupplierVO.ContactPerson1Email = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson1Email;
                    addNewSupplierVO.ContactPerson2Name = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson2Name;
                    addNewSupplierVO.ContactPerson2MobileNo = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson2MobileNo;
                    addNewSupplierVO.ContactPerson2Email = ((SupplierVO)grdSupplier.SelectedItem).ContactPerson2Email;
                    addNewSupplierVO.PhoneNo = ((SupplierVO)grdSupplier.SelectedItem).PhoneNo;
                    addNewSupplierVO.Fax = ((SupplierVO)grdSupplier.SelectedItem).Fax;
                    addNewSupplierVO.ModeOfPayment = Convert.ToInt64(((SupplierVO)grdSupplier.SelectedItem).ModeOfPayment);
                    addNewSupplierVO.Currency = Convert.ToInt64(((SupplierVO)grdSupplier.SelectedItem).Currency);
                    addNewSupplierVO.TaxNature = Convert.ToInt64(((SupplierVO)grdSupplier.SelectedItem).TaxNature);
                    addNewSupplierVO.TermofPayment = Convert.ToInt64(((SupplierVO)grdSupplier.SelectedItem).TermofPayment);
                    addNewSupplierVO.MSTNumber = ((SupplierVO)grdSupplier.SelectedItem).MSTNumber;
                    addNewSupplierVO.CSTNumber = ((SupplierVO)grdSupplier.SelectedItem).CSTNumber;
                    addNewSupplierVO.VAT = ((SupplierVO)grdSupplier.SelectedItem).VAT;
                    addNewSupplierVO.DRUGLicence = ((SupplierVO)grdSupplier.SelectedItem).DRUGLicence;
                    addNewSupplierVO.ServiceTaxNumber = ((SupplierVO)grdSupplier.SelectedItem).ServiceTaxNumber;
                    addNewSupplierVO.PANNumber = ((SupplierVO)grdSupplier.SelectedItem).PANNumber;
                    addNewSupplierVO.MFlag = false;//I dont Know Which Value I assign it Sarang didnt tell  me
                    addNewSupplierVO.Status = ((SupplierVO)grdSupplier.SelectedItem).Status;
                    addNewSupplierVO.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    addNewSupplierVO.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    addNewSupplierVO.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    addNewSupplierVO.UpdatedDateTime = System.DateTime.Now;
                    addNewSupplierVO.UpdateWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    bizactionVO.ItemMatserDetails.Add(addNewSupplierVO);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {

                            if (((clsAddUpdateSupplierBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                SupplierId = 0;
                                msgText = "Status Updated Successfully";

                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();

                                //After Updation Back to BackPanel and Setup Page
                                objAnimation.Invoke(RotationType.Backward);
                                SetCommandButtonState("Modify");

                            }

                        }

                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
            MasterList = new PagedSortableCollectionView<SupplierVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdSupplier.DataContext = MasterList;
            SetupPage();
        }

        #endregion

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<SupplierVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdSupplier.DataContext = MasterList;
            SetupPage();
        }

        private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbCountry.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbCountry.SelectedItem).ID > 0)
            //        FillState(((MasterListItem)cmbCountry.SelectedItem).ID, cmbState);
            //}
            //else
            //{
            //    FillState(0, cmbState);
            //}

            if (cmbCountry.SelectedItem != null && cmbCountry.SelectedValue != null)
            {
                //if (((MasterListItem)cmbCountry.SelectedItem).ID > 0)
                //{
                //  ((SupplierVO)this.DataContext).Country = ((MasterListItem)cmbCountry.SelectedItem).ID;
                List<MasterListItem> objList = new List<MasterListItem>();
                MasterListItem objM = new MasterListItem(0, "-- Select --");
                objList.Add(objM);
                cmbState.ItemsSource = objList;
                cmbState.SelectedItem = objM;
                cmbCity.ItemsSource = objList;
                cmbCity.SelectedItem = objM;
                cmbArea.ItemsSource = objList;
                cmbArea.SelectedItem = objM;
                FillState(((MasterListItem)cmbCountry.SelectedItem).ID);
                //        }
            }
        }

        private void FillCountry()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    MasterListItem Default = new MasterListItem(0, "- Select -");
                    objList.Add(Default);
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbCountry.ItemsSource = null;
                    cmbCountry.ItemsSource = objList.DeepCopy();


                    cmbCountry.SelectedItem = Default;




                    if (this.DataContext != null)
                    {
                        //cmbCountry.SelectedItem = ((List<MasterListItem>)cmbCountry.ItemsSource).Select(ss => ss.Description == ((clsPatientVO)this.DataContext).Country); //    ((clsPatientVO)this.DataContext).CountryId;
                        //cmbOfficeCountry.SelectedItem = ((List<MasterListItem>)cmbOfficeCountry.ItemsSource).Select(ss => ss.Description == ((clsPatientVO)this.DataContext).Country); //    ((clsPatientVO)this.DataContext).CountryId;
                        if (cmbCountry.SelectedItem != null)
                        {
                            cmbCountry.SelectedValue = ((SupplierVO)this.DataContext).Country;
                        }

                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();


        }

        # region //OldCode

        //private void FillState(long? CountryId, AutoCompleteComboBox objControl)
        //{

        //    clsGetCountryListBizActionVO BizAction = new clsGetCountryListBizActionVO();

        //    BizAction.CountryID = Convert.ToInt64(CountryId);
        //    BizAction.StateList = new List<clsStateVO>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {

        //            List<clsStateVO> objList = new List<clsStateVO>();

        //            clsStateVO Default = new clsStateVO { StateID = 0, StateName = "--Select--" };
        //            objList.Add(Default);
        //            objList.AddRange(((clsGetCountryListBizActionVO)e.Result).StateList);

        //            if (objControl.Name == "cmbState")
        //            {
        //                objControl.ItemsSource = null;
        //                objControl.ItemsSource = objList.DeepCopy();
        //            }

        //            //if (this.DataContext == null)
        //            //{
        //            //    cmbOfficeState.SelectedItem = Default;
        //            //}

        //            if (ViewSupplier == true && (SupplierVO)grdSupplier.SelectedItem != null)
        //            {
        //                cmbState.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).State;
        //            }
        //            else
        //            {
        //                objControl.SelectedItem = Default;
        //            }

        //            if (this.DataContext != null)
        //            {
        //                if (objControl.Name == "cmbState")
        //                {
        //                    objControl.SelectedValue = ((SupplierVO)this.DataContext).State;
        //                    //FillDistrict(((clsPatientVO)this.DataContext).StateID, cmbDistrict);

        //                }


        //            }
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();

        //}


        //private void FillDistrict(long? StateId, AutoCompleteComboBox objControl)
        //{

        //    clsGetStateListBizActionVO BizAction = new clsGetStateListBizActionVO();

        //    BizAction.StateID = Convert.ToInt64(StateId);
        //    BizAction.DistrictList = new List<clsDistVO>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {

        //            List<clsDistVO> objList = new List<clsDistVO>();

        //            clsDistVO Default = new clsDistVO { DistID = 0, DistName = "--Select--" };
        //            objList.Add(Default);
        //            objList.AddRange(((clsGetStateListBizActionVO)e.Result).DistrictList);

        //            if (objControl.Name == "cmbDistrict")
        //            {
        //                objControl.ItemsSource = null;
        //                objControl.ItemsSource = objList.DeepCopy();
        //            }






        //            if (ViewSupplier == true && (SupplierVO)grdSupplier.SelectedItem != null)
        //            {
        //                cmbDistrict.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).District;
        //            }
        //            else
        //            {
        //                objControl.SelectedItem = Default;
        //            }

        //            if (this.DataContext != null)
        //            {
        //                if (objControl.Name == "cmbDistrict")
        //                {
        //                    objControl.SelectedValue = ((SupplierVO)this.DataContext).District;
        //                    //FillCity(((clsPatientVO)this.DataContext).DistrictID, cmbCity);


        //                }

        //            }
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();

        //}

        //private void FillCity(long? DistId, AutoCompleteComboBox objControl)
        //{

        //    clsGetDistListBizActionVO BizAction = new clsGetDistListBizActionVO();

        //    BizAction.DistID = Convert.ToInt64(DistId);
        //    BizAction.CityList = new List<clsCityVO>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {

        //            List<clsCityVO> objList = new List<clsCityVO>();

        //            clsCityVO Default = new clsCityVO { CityID = 0, CityName = "--Select--" };
        //            objList.Add(Default);
        //            objList.AddRange(((clsGetDistListBizActionVO)e.Result).CityList);

        //            if (objControl.Name == "cmbCity")
        //            {
        //                objControl.ItemsSource = null;
        //                objControl.ItemsSource = objList.DeepCopy();
        //            }





        //            if (ViewSupplier == true && (SupplierVO)grdSupplier.SelectedItem != null)
        //            {
        //                cmbCity.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).City;
        //            }
        //            else
        //            {
        //                objControl.SelectedItem = Default;
        //            }

        //            if (this.DataContext != null)
        //            {
        //                if (objControl.Name == "cmbCity")
        //                {
        //                    objControl.SelectedValue = ((SupplierVO)this.DataContext).City;
        //                    //FillArea(((clsPatientVO)this.DataContext).CityID, cmbArea);.


        //                }

        //            }
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();

        //}

        //private void FillArea(long? CityId, AutoCompleteComboBox objControl)
        //{

        //    clsGetCityListBizActionVO BizAction = new clsGetCityListBizActionVO();

        //    BizAction.CityID = Convert.ToInt64(CityId);
        //    BizAction.AreaList = new List<clsAreaVO>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {

        //            List<clsAreaVO> objList = new List<clsAreaVO>();

        //            clsAreaVO Default = new clsAreaVO { AreaID = 0, AreaName = "--Select--" };
        //            objList.Add(Default);
        //            objList.AddRange(((clsGetCityListBizActionVO)e.Result).AreaList);


        //            if (objControl.Name == "cmbArea")
        //            {
        //                objControl.ItemsSource = null;
        //                objControl.ItemsSource = objList.DeepCopy();
        //            }

        //            if (ViewSupplier == true && (SupplierVO)grdSupplier.SelectedItem != null)
        //            {
        //                cmbArea.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).Zone;
        //            }
        //            else
        //            {
        //                objControl.SelectedItem = Default;
        //            }


        //            if (this.DataContext != null)
        //            {
        //                if (objControl.Name == "cmbArea")
        //                {
        //                    objControl.SelectedValue = ((SupplierVO)this.DataContext).Zone;

        //                    //  txtPinCode.Text = ((clsPatientVO)this.DataContext).Pincode;
        //                }


        //            }
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();

        //}


        //private void FillAddressLocation6(long? ZoneId, AutoCompleteComboBox objControl)
        //{

        //    clsGetAddressLocation6ListByZoneIdBizActionVO BizAction = new clsGetAddressLocation6ListByZoneIdBizActionVO();

        //    BizAction.ipZoneID = Convert.ToInt64(ZoneId);
        //    BizAction.objAddressLocation6List = new List<clsAddressLocation6VO>();

        //    List<clsAddressLocation6VO> objList = new List<clsAddressLocation6VO>();

        //    clsAddressLocation6VO Default = new clsAddressLocation6VO { ID = 0, AddressLocation6Name = "--Select--", PinCode = "" };
        //    objList.Add(Default);

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {


        //            objList.AddRange(((clsGetAddressLocation6ListByZoneIdBizActionVO)e.Result).objAddressLocation6List);


        //            if (objControl.Name == "cmbAddressLocation6")
        //            {
        //                objControl.ItemsSource = null;
        //                objControl.ItemsSource = objList.DeepCopy();
        //            }

        //            if (ViewSupplier == true && (SupplierVO)grdSupplier.SelectedItem != null)
        //            {
        //                cmbAddressLocation6.SelectedValue = ((SupplierVO)grdSupplier.SelectedItem).Area;
        //            }
        //            else
        //            {
        //                objControl.SelectedItem = Default;
        //            }

        //            if (this.DataContext != null)
        //            {
        //                if (objControl.Name == "cmbAddressLocation6")
        //                {
        //                    objControl.SelectedValue = ((SupplierVO)this.DataContext).Area;

        //                    // txtPinCode.Text = ((clsPatientVO)this.DataContext).Pincode;
        //                }


        //            }
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();

        //}

        # endregion //End Old Code

        # region //NewCode
        //public void FillState(long CountryID, long StateID, long CityID, long RegionID)
        //{
        //    clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
        //    BizAction.CountryId = CountryID;
        //    BizAction.ListStateDetails = new List<PalashDynamics.ValueObjects.Administration.clsStateVO>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
        //            {
        //                if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
        //                {
        //                    foreach (PalashDynamics.ValueObjects.Administration.clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
        //                    {
        //                        MasterListItem obj = new MasterListItem();
        //                        obj.ID = item.Id;
        //                        obj.Description = item.Description;
        //                        objList.Add(obj);
        //                    }
        //                }
        //            }
        //            cmbState.ItemsSource = null;
        //            cmbState.ItemsSource = objList.DeepCopy();

        //            //txtSpouseState.ItemsSource = null;
        //            //txtSpouseState.ItemsSource = objList.DeepCopy();

        //            //if (this.DataContext != null)
        //            //{
        //            //    cmbState.SelectedValue = ((SupplierVO)this.DataContext).State;
        //            //    //  txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
        //            //}
        //            FillCity(StateID, CityID, RegionID);
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //public void FillCity(long StateID, long CityID, long RegionID)
        //{
        //    clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
        //    BizAction.StateId = StateID;
        //    BizAction.ListCityDetails = new List<PalashDynamics.ValueObjects.Administration.clsCityVO>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {

        //        if (args.Error == null && args.Result != null)
        //        {

        //            BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            MasterListItem objM = new MasterListItem(0, "-- Select --");
        //            objList.Add(objM);
        //            if (BizAction.ListCityDetails != null)
        //            {
        //                if (BizAction.ListCityDetails.Count > 0)
        //                {
        //                    foreach (PalashDynamics.ValueObjects.Administration.clsCityVO item in BizAction.ListCityDetails)
        //                    {
        //                        MasterListItem obj = new MasterListItem();
        //                        obj.ID = item.Id;
        //                        obj.Description = item.Description;
        //                        objList.Add(obj);
        //                    }
        //                }
        //            }
        //            cmbCity.ItemsSource = null;
        //            cmbCity.ItemsSource = objList.DeepCopy();

        //            //txtSpouseCity.ItemsSource = null;
        //            //txtSpouseCity.ItemsSource = objList.DeepCopy();

        //            //if (this.DataContext != null)
        //            //{
        //            //    cmbCity.SelectedValue = ((SupplierVO)this.DataContext).City;
        //            //    //  txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
        //            //}
        //            FillRegion(CityID);
        //        }
        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        public void FillState(long CountryID)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<PalashDynamics.ValueObjects.Administration.clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (PalashDynamics.ValueObjects.Administration.clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }

                    cmbState.ItemsSource = null;
                    cmbState.ItemsSource = objList.DeepCopy();
                    cmbState.SelectedItem = objList[0];

                    foreach (var item in objList)  //By UmeshF for showing selected value in combobox while modify supplier
                    {
                        if (item.ID == SelectedState)
                            cmbState.SelectedItem = item;
                    }
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillCity(long StateID)
        {
            clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
            BizAction.StateId = StateID;
            BizAction.ListCityDetails = new List<PalashDynamics.ValueObjects.Administration.clsCityVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListCityDetails != null)
                    {
                        if (BizAction.ListCityDetails.Count > 0)
                        {
                            foreach (PalashDynamics.ValueObjects.Administration.clsCityVO item in BizAction.ListCityDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    cmbCity.ItemsSource = null;
                    cmbCity.ItemsSource = objList.DeepCopy();
                    foreach (var item in objList) //By UmeshF for showing selected value in combobox while modify supplier
                    {
                        if (item.ID == SelectedCity)
                            cmbCity.SelectedItem = item;
                    }
                    

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillRegion(long CityID)
        {
            clsGetRegionDetailsByCityIDBizActionVO BizAction = new clsGetRegionDetailsByCityIDBizActionVO();
            BizAction.CityId = CityID;
            BizAction.ListRegionDetails = new List<clsRegionVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDBizActionVO)args.Result).ListRegionDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListRegionDetails != null)
                    {
                        if (BizAction.ListRegionDetails.Count > 0)
                        {
                            foreach (clsRegionVO item in BizAction.ListRegionDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    cmbArea.ItemsSource = null;
                    cmbArea.ItemsSource = objList.DeepCopy();

                    foreach (var item in objList)   //By UmeshF for showing selected value in combobox while modify supplier
                    {
                        if (item.ID == SelectedArea)
                            cmbArea.SelectedItem = item;
                    }                   
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        # endregion  //End Code




        //Added by MMBABU to fill Supplier Category
        private void FillSupplierCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SupplierCategory;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    //MasterListItem objList = new MasterListItem(0, "- Select -");
                    //objList.Add(Default);
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbSupplierCategory.ItemsSource = null;
                    cmbSupplierCategory.ItemsSource = objList;

                    if (this.DataContext != null)
                    {
                        cmbSupplierCategory.SelectedValue = ((SupplierVO)this.DataContext).SupplierCategoryId;
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        //END

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbCity.SelectedItem != null)
            //{
            //    if (((clsCityVO)cmbCity.SelectedItem).CityID > 0)
            //        FillArea(((clsCityVO)cmbCity.SelectedItem).CityID, cmbArea);
            //    //else
            //    //    FillArea(0, cmbArea);
            //}
            //else
            //    FillArea(0, cmbArea);

         //   if (cmbCity.SelectedItem != null && cmbCity.SelectedValue != null)
            if (cmbCity.SelectedItem != null )
                if (((MasterListItem)cmbCity.SelectedItem).ID > 0)
                {
                   // if (((MasterListItem)cmbCity.SelectedItem).ID > 0)
                      //  ((SupplierVO)this.DataContext).City = ((MasterListItem)cmbCity.SelectedItem).ID;

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    cmbArea.ItemsSource = null;
                    cmbArea.SelectedItem = objM;
                    FillRegion(((MasterListItem)cmbCity.SelectedItem).ID);
                }

        }

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbState.SelectedItem != null)
            //{
            //    if (((clsStateVO)cmbState.SelectedItem).StateID > 0)
            //        FillDistrict(((clsStateVO)cmbState.SelectedItem).StateID, cmbDistrict);
            //    else
            //        FillDistrict(0, cmbDistrict);
            //}
            //else
            //{
            //    FillDistrict(0, cmbDistrict);
            //}
          //  if (cmbState.SelectedItem != null && cmbState.SelectedValue != null)
            if (cmbState.SelectedItem != null )
                if (((MasterListItem)cmbState.SelectedItem).ID > 0)
                {
                 //   ((SupplierVO)this.DataContext).State = ((MasterListItem)cmbState.SelectedItem).ID;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbCity.ItemsSource = objList;
                    cmbCity.SelectedItem = objM;
                    cmbArea.ItemsSource = objList;
                    cmbArea.SelectedItem = objM;
                    FillCity(((MasterListItem)cmbState.SelectedItem).ID);
                }


        }

        private void cmbDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbDistrict.SelectedItem != null)
            //{
            //    if (((clsDistVO)cmbDistrict.SelectedItem).DistID > 0)
            //        FillCity(((clsDistVO)cmbDistrict.SelectedItem).DistID, cmbCity);
            //    else
            //        FillCity(0, cmbCity);
            //}
            //else
            //{
            //    FillCity(0, cmbCity);
            //}




        }

        private void cmbArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbArea.SelectedItem != null)
            //{
            //    if (((clsAreaVO)cmbArea.SelectedItem).AreaID > 0)
            //        FillAddressLocation6(((clsAreaVO)cmbArea.SelectedItem).AreaID, cmbAddressLocation6);
            //    else
            //        FillAddressLocation6(0, cmbAddressLocation6);
            //}
            //else
            //    FillAddressLocation6(0, cmbAddressLocation6);

            //if (((MasterListItem)cmbArea.SelectedItem).ID > 0)
            //    ((SupplierVO)this.DataContext).Area = ((MasterListItem)cmbArea.SelectedItem).ID;


        }


        private void cmbAddressLocation6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAddressLocation6.SelectedItem != null)
                txtPinCode.Text = ((clsAddressLocation6VO)cmbAddressLocation6.SelectedItem).PinCode;
            else
                txtPinCode.Text = "";
        }

        private void txtPinCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtPinCode.Text != "")
            {

            }
        }

        private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsNumberValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    //((TextBox)sender).SelectionStart = selectionStart;
                    //((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

            if (e.Key == Key.Enter)
            {

                SetupPage();
            }
        }

        private void txtPOAutoCancelDays_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtPOAutoCancelDays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPOAutoCancelDays.Text) && !txtPOAutoCancelDays.Text.IsItNumber())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
        }

    }
}
