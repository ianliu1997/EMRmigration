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
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using CIMS;

using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration.IPD;

namespace PalashDynamics.Administration
{
    public partial class frmServiceMasterNew : UserControl
    {
        #region Variables and List Definitions
        bool IsCancel = true;
        bool Edit = false;
        bool IsNew = false;
        public bool IsValidate = true;
        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }
        private SwivelAnimation objAnimation;

        public List<long> lstTariffs = new List<long>();
        public long pkServiceID { get; set; }
        public Boolean isPageLoaded = false;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public bool isView = false;
        public string msgText;
        public string msgTitle;
        public bool EditMode { get; set; }
        public bool IsView { get; set; }
        WaitIndicator wait = new WaitIndicator();
        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsClassDataGrid> MasterClassList { get; private set; }
        public int DataListPageSize
        {
            get { return DataList.PageSize; }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }
        #endregion 

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    CmdModify.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    CmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "New":
                    CmdModify.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                case "Save":
                    CmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "Modify":
                    CmdNew.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "Cancel":
                    CmdNew.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    AssignButtons.Visibility = Visibility.Visible;
                    break;
                case "FrontPanel":
                    CmdNew.IsEnabled = true;
                    CmdModify.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    break;
                case "View":
                    CmdModify.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "View1":
                    CmdModify.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdNew.IsEnabled = false;
                    CmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                default:
                    break;
            }
        }

        #endregion
        public frmServiceMasterNew()
        {
            InitializeComponent();
            this.DataContext = new clsServiceMasterVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillServiceGrid();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillServiceGrid();
            SetCommandButtonState("Load");
            FillSpecialization();
            FillCodeType();
            FillSACCodes();
            ClassList = new List<clsServiceMasterVO>();
            FillClassGrid();

        }

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialization.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID.ToString());
                //cmdSearch_Click(sender, e);
                //FillServiceGrid();
            }
        }

        private void cmdAssignItems_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdAssignAgency_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdAssignTariff_Click(object sender, RoutedEventArgs e)
        {
            if (grdServices.SelectedItem != null)
            {
                clsServiceMasterVO objServiceVO = new clsServiceMasterVO();
                objServiceVO = (clsServiceMasterVO)grdServices.SelectedItem;
                //if (objServiceVO.IsPackage && objServiceVO != null && objServiceVO.IsFreezed == false || objServiceVO.IsApproved == false)//Commented By CDS  if (objServiceVO != null && objServiceVO.IsFreezed==true && objServiceVO.IsApproved==true)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //       new MessageBoxControl.MessageBoxChildWindow("Palash", "Package Should Freezed And Approved.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                //    msgW1.Show();

                //}
                //else
                //{
                if (!objServiceVO.IsPackage)
                {
                    AssignTariffPopUp Win = new AssignTariffPopUp();
                    Win.ServiceID = ((clsServiceMasterVO)grdServices.SelectedItem).ID;
                    Win.Show();
                    Win.GetSelectedServiceDetails(objServiceVO);
                }
                //}

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select the Service to link with Tariff.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }

        private void cmdAssignPackage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdDefineClassRate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbSpecialization1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSpecialization1.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecialization1.SelectedItem).ID.ToString());
                IsValidate = true;
            }
        }
        private void chkServiceTax_Click(object sender, RoutedEventArgs e)
        {
            if (chkServiceTax.IsChecked == true)
            {
                txtServiceTaxPercentage.IsEnabled = true;
                txtServiceTaxAmount.IsEnabled = true;
            }
            if (chkServiceTax.IsChecked == false)
            {
                txtServiceTaxPercentage.IsEnabled = false;
                txtServiceTaxAmount.IsEnabled = false;
                txtServiceTaxAmount.Text = "0.00";
                txtServiceTaxPercentage.Text = "0.00";
            }
        }

        private void txtServiceTaxPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtServiceTaxPercentage.Text.Equals("") && (txtServiceTaxPercentage.Text != "0"))
                {
                    if (Extensions.IsItDecimal(txtServiceTaxPercentage.Text) == false)
                    {
                        if (Convert.ToDecimal(txtServiceTaxPercentage.Text) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Service tax percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtServiceTaxPercentage.Text = "0.00";
                            txtServiceTaxAmount.Text = "0.00";
                            return;
                        }
                        String str1 = txtServiceTaxPercentage.Text.Substring(txtServiceTaxPercentage.Text.IndexOf(".") + 1);
                        if (Convert.ToDecimal(str1) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                                        new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtServiceTaxPercentage.Text = "0.00";
                            return;
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal ServiceRate = 0;
                        ServiceRate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal ServiceTaxPer = 0;
                        if (Extensions.IsPositiveNumber(txtServiceTaxPercentage.Text) == true)
                        {
                            if (Extensions.IsItDecimal(txtServiceTaxPercentage.Text) == false)
                            {
                                ServiceTaxPer = Convert.ToDecimal(txtServiceTaxPercentage.Text);
                            }
                        }
                        //else
                        //{
                        //    txtServiceTaxPercentage.SetValidation(" Service Tax should be in positive number");
                        //    txtServiceTaxPercentage.RaiseValidationError();
                        //    ServiceTaxPer = 0;
                        //}
                        decimal ServiceTaxAmount = 0;
                        ServiceTaxAmount = ((ServiceRate * ServiceTaxPer) / 100);
                        txtServiceTaxAmount.Text = ServiceTaxAmount.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtServiceTaxPercentage.Text = "0.00";
            }
        }

        private void txtServiceTaxAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtServiceTaxAmount.Text.Equals("") && (txtServiceTaxAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal ServiceRate = 0;
                        ServiceRate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal ServiceTaxAmount = 0;
                        if (Extensions.IsItDecimal(txtServiceTaxAmount.Text) == false)
                        {
                            ServiceTaxAmount = Convert.ToDecimal(txtServiceTaxAmount.Text);
                        }
                        else
                        {
                            txtServiceTaxAmount.SetValidation(" Service Tax Amount should be number");
                            txtServiceTaxAmount.RaiseValidationError();
                            ServiceTaxAmount = 0;
                        }
                        decimal ServiceTaxPer = 0;
                        ServiceTaxPer = (100 * ServiceTaxAmount) / ServiceRate;
                        if (ServiceTaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Service tax percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtServiceTaxPercentage.Text = "0.00";
                            txtServiceTaxAmount.Text = "0.00";
                            ServiceTaxPer = 0;
                            return;
                        }
                        txtServiceTaxPercentage.Text = ServiceTaxPer.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtServiceTaxAmount.Text = "0.00";
            }
        }

        private void chkLuxuryTax_Click(object sender, RoutedEventArgs e)
        {
            if (chkLuxuryTax.IsChecked == true)
            {
                txtLuxuryTaxPercentage.IsEnabled = true;
                txtLuxuryTaxAmount.IsEnabled = true;
            }
            if (chkLuxuryTax.IsChecked == false)
            {
                txtLuxuryTaxPercentage.IsEnabled = false;
                txtLuxuryTaxAmount.IsEnabled = false;
                txtLuxuryTaxAmount.Text = "0.00";
                txtLuxuryTaxPercentage.Text = "0.00";
            }
        }

        private void txtLuxuryTaxPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtLuxuryTaxPercentage.Text.Equals("") && (txtLuxuryTaxPercentage.Text != "0"))
                {
                    if (Extensions.IsItDecimal(txtLuxuryTaxPercentage.Text) == false)
                    {
                        if (Convert.ToDecimal(txtLuxuryTaxPercentage.Text) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Luxury tax percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtLuxuryTaxPercentage.Text = "0.00";
                            txtLuxuryTaxAmount.Text = "0.00";
                            return;
                        }
                        String str1 = txtLuxuryTaxPercentage.Text.Substring(txtLuxuryTaxPercentage.Text.IndexOf(".") + 1);
                        if (Convert.ToDecimal(str1) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                                        new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtLuxuryTaxPercentage.Text = "0.00";
                            return;
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal LuxuryRate = 0;
                        LuxuryRate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal LuxuryTaxPer = 0;
                        if (Extensions.IsItDecimal(txtLuxuryTaxPercentage.Text) == false)
                        {
                            LuxuryTaxPer = Convert.ToDecimal(txtLuxuryTaxPercentage.Text);
                        }
                        else
                        {
                            txtLuxuryTaxPercentage.SetValidation(" Luxury Tax should be number");
                            txtLuxuryTaxPercentage.RaiseValidationError();
                            LuxuryTaxPer = 0;
                        }
                        decimal LuxuryTaxAmount = 0;
                        LuxuryTaxAmount = ((LuxuryRate * LuxuryTaxPer) / 100);
                        txtLuxuryTaxAmount.Text = LuxuryTaxAmount.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                              new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtLuxuryTaxPercentage.Text = "0.00";
            }
        }

        private void txtLuxuryTaxAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtLuxuryTaxAmount.Text.Equals("") && (txtLuxuryTaxAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal LuxuryRate = 0;
                        LuxuryRate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal LuxuryTaxAmount = 0;
                        if (Extensions.IsItDecimal(txtLuxuryTaxAmount.Text) == false)
                        {
                            LuxuryTaxAmount = Convert.ToDecimal(txtLuxuryTaxAmount.Text);
                        }
                        else
                        {
                            txtLuxuryTaxAmount.SetValidation(" Luxury Tax Amount should be number");
                            txtLuxuryTaxAmount.RaiseValidationError();
                            LuxuryTaxAmount = 0;
                        }
                        decimal LuxuryTaxPer = 0;
                        LuxuryTaxPer = (100 * LuxuryTaxAmount) / LuxuryRate;
                        if (LuxuryTaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Luxury tax percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtLuxuryTaxPercentage.Text = "0.00";
                            txtLuxuryTaxAmount.Text = "0.00";
                            LuxuryTaxPer = 0;
                            return;
                        }
                        txtLuxuryTaxPercentage.Text = LuxuryTaxPer.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtLuxuryTaxAmount.Text = "0.00";
            }
        }

        private void txtStaffDiscountPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtStaffDiscountPercentage.Text.Equals("") && (txtStaffDiscountPercentage.Text != "0"))
                {
                    if (Extensions.IsItDecimal(txtStaffDiscountPercentage.Text) == false)
                    {
                        if (Convert.ToDecimal(txtStaffDiscountPercentage.Text) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "staff discount percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtStaffDiscountPercentage.Text = "0.00";
                            txtStaffDiscountAmount.Text = "0.00";
                            return;
                        }
                        String str1 = txtStaffDiscountPercentage.Text.Substring(txtStaffDiscountPercentage.Text.IndexOf(".") + 1);
                        if (Convert.ToDecimal(str1) > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                                        new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtStaffDiscountPercentage.Text = "0.00";
                            return;
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal Percent = 0;
                        if (Extensions.IsPositiveNumber(txtStaffDiscountPercentage.Text) == true)
                        {
                            if (Extensions.IsItDecimal(txtStaffDiscountPercentage.Text) == false)
                            {
                                Percent = Convert.ToDecimal(txtStaffDiscountPercentage.Text);
                            }

                        }
                        //else
                        //{
                        //    txtStaffDiscountPercentage.SetValidation(" Staff Discount Percent should be positive number");
                        //    txtStaffDiscountPercentage.RaiseValidationError();
                        //    Percent = 0;
                        //}
                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);
                        txtStaffDiscountAmount.Text = TaxAmount.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtStaffDiscountPercentage.Text = "0.00";
            }
        }

        private void txtStaffDiscountAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtStaffDiscountAmount.Text.Equals("") && (txtStaffDiscountAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);
                        decimal TaxAmount = 0;
                        if (Extensions.IsItDecimal(txtStaffDiscountAmount.Text) == false)
                        {
                            TaxAmount = Convert.ToDecimal(txtStaffDiscountAmount.Text);
                        }
                        else
                        {
                            txtStaffDiscountAmount.SetValidation("Staff Discount Amount should be number");
                            txtStaffDiscountAmount.RaiseValidationError();
                            TaxAmount = 0;
                        }
                        decimal TaxPer = 0;
                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "staff discount percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtStaffDiscountPercentage.Text = "0.00";
                            txtStaffDiscountAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }
                        txtStaffDiscountPercentage.Text = TaxPer.ToString("0.00");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtStaffDiscountAmount.Text = "0.00";
            }
        }

        private void txtStaffParentPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtStaffParentPercentage.Text.Equals("") && (txtStaffParentPercentage.Text != "0"))
                {

                    if (!txtStaffParentPercentage.Text.Equals("") && (txtStaffParentPercentage.Text != "0"))
                    {
                        if (Extensions.IsItDecimal(txtStaffParentPercentage.Text) == false)
                        {
                            if (Convert.ToDecimal(txtStaffParentPercentage.Text) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("", "staff parent percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtStaffParentPercentage.Text = "0.00";
                                txtStaffParentAmount.Text = "0.00";
                                return;
                            }
                            String str1 = txtStaffParentPercentage.Text.Substring(txtStaffParentPercentage.Text.IndexOf(".") + 1);
                            if (Convert.ToDecimal(str1) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtStaffParentPercentage.Text = "0.00";
                                return;
                            }
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);
                        decimal Percent = 0;
                        if (Extensions.IsPositiveNumber(txtStaffParentPercentage.Text) == true)
                        {
                            if (Extensions.IsItDecimal(txtStaffParentPercentage.Text) == false)
                            {
                                Percent = Convert.ToDecimal(txtStaffParentPercentage.Text);
                            }
                        }
                        //else
                        //{
                        //    txtStaffParentPercentage.SetValidation(" Staff Parent Percent should be positive number");
                        //    txtStaffParentPercentage.RaiseValidationError();
                        //    Percent = 0;
                        //}
                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);
                        txtStaffParentAmount.Text = TaxAmount.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtStaffParentPercentage.Text = "0.00";
            }
        }

        private void txtConcessionPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtConcessionPercentage.Text.Equals("") && (txtConcessionPercentage.Text != "0"))
                {
                    if (!txtConcessionPercentage.Text.Equals("") && (txtConcessionPercentage.Text != "0"))
                    {
                        if (Extensions.IsItDecimal(txtConcessionPercentage.Text) == false)
                        {
                            if (Convert.ToDecimal(txtConcessionPercentage.Text) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtConcessionPercentage.Text = "0.00";
                                txtConcessionAmount.Text = "0.00";
                                return;
                            }
                            String str1 = txtConcessionPercentage.Text.Substring(txtConcessionPercentage.Text.IndexOf(".") + 1);
                            if (Convert.ToDecimal(str1) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtConcessionPercentage.Text = "0.00";
                                return;
                            }
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal Percent = 0;
                        if (Extensions.IsPositiveNumber(txtConcessionPercentage.Text) == true)
                        {
                            if (Extensions.IsItDecimal(txtConcessionPercentage.Text) == false)
                            {
                                Percent = Convert.ToDecimal(txtConcessionPercentage.Text);
                            }
                        }
                        //else
                        //{
                        //    txtConcessionPercentage.SetValidation("Concession Percent should be positive number");
                        //    txtConcessionPercentage.RaiseValidationError();
                        //    Percent = 0;
                        //}
                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);
                        txtConcessionAmount.Text = TaxAmount.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtConcessionPercentage.Text = "0.00";
            }
        }

        private void txtStaffParentAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtStaffParentAmount.Text.Equals("") && (txtStaffParentAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal TaxAmount = 0;
                        if (Extensions.IsItDecimal(txtStaffParentAmount.Text) == false)
                        {
                            TaxAmount = Convert.ToDecimal(txtStaffParentAmount.Text);
                        }
                        else
                        {
                            txtStaffParentAmount.SetValidation(" Staff Parent Amount should be number");
                            txtStaffParentAmount.RaiseValidationError();
                            TaxAmount = 0;
                        }
                        decimal TaxPer = 0;
                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "staff parent percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtStaffParentPercentage.Text = "0.00";
                            txtStaffParentAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }
                        txtStaffParentPercentage.Text = TaxPer.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtStaffParentAmount.Text = "0.00";
            }
        }

        private void txtConcessionAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtConcessionAmount.Text.Equals("") && (txtConcessionAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal TaxAmount = 0;
                        if (Extensions.IsItDecimal(txtConcessionAmount.Text) == false)
                        {
                            TaxAmount = Convert.ToDecimal(txtConcessionAmount.Text);
                        }
                        else
                        {
                            txtConcessionAmount.SetValidation(" Concession Amount should be number");
                            txtConcessionAmount.RaiseValidationError();
                            TaxAmount = 0;
                        }
                        decimal TaxPer = 0;
                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtConcessionPercentage.Text = "0.00";
                            txtConcessionAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }
                        txtConcessionPercentage.Text = TaxPer.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtConcessionAmount.Text = "0.00";
            }
        }

        private void chkApplicableToAllDoctors_Click(object sender, RoutedEventArgs e)
        {
            if (chkApplicableToAllDoctors.IsChecked == true)
            {
                txtDoctorApplicableAmount.IsEnabled = true;
                txtDoctorApplicablePercent.IsEnabled = true;
            }
            else
            {
                txtDoctorApplicableAmount.IsEnabled = false;
                txtDoctorApplicablePercent.IsEnabled = false;

                txtDoctorApplicableAmount.Text = "0.00";
                txtDoctorApplicablePercent.Text = "0.00";
            }
        }

        private void txtDoctorApplicablePercent_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtDoctorApplicablePercent.Text.Equals("") && (txtDoctorApplicablePercent.Text != "0"))
                {
                    if (!txtDoctorApplicablePercent.Text.Equals("") && (txtDoctorApplicablePercent.Text != "0"))
                    {
                        if (Extensions.IsItDecimal(txtDoctorApplicablePercent.Text) == false)
                        {
                            if (Convert.ToDecimal(txtDoctorApplicablePercent.Text) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("", "Doctor applicable percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtDoctorApplicablePercent.Text = "0.00";
                                txtDoctorApplicableAmount.Text = "0.00";
                                return;
                            }
                            String str1 = txtDoctorApplicablePercent.Text.Substring(txtDoctorApplicablePercent.Text.IndexOf(".") + 1);
                            if (Convert.ToDecimal(str1) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtDoctorApplicablePercent.Text = "0.00";
                                return;
                            }
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);
                        decimal Percent = 0;
                        if (Extensions.IsPositiveNumber(txtDoctorApplicablePercent.Text) == true)
                        {
                            if (Extensions.IsItDecimal(txtDoctorApplicablePercent.Text) == false)
                            {
                                Percent = Convert.ToDecimal(txtDoctorApplicablePercent.Text);

                            }
                        }
                        //else
                        //{
                        //    txtDoctorApplicablePercent.SetValidation("Doctor Applicable Percent should be positive number");
                        //    txtDoctorApplicablePercent.RaiseValidationError();
                        //    Percent = 0;
                        //}
                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);
                        txtDoctorApplicableAmount.Text = TaxAmount.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtDoctorApplicablePercent.Text = "0.00";
            }
        }

        private void txtDoctorApplicableAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtDoctorApplicableAmount.Text.Equals("") && (txtDoctorApplicableAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal TaxAmount = 0;
                        if (Extensions.IsItDecimal(txtDoctorApplicableAmount.Text) == false)
                        {
                            TaxAmount = Convert.ToDecimal(txtDoctorApplicableAmount.Text);
                        }
                        else
                        {
                            txtDoctorApplicableAmount.SetValidation(" Doctor Applicable Amount should be number");
                            txtDoctorApplicableAmount.RaiseValidationError();
                            TaxAmount = 0;
                        }
                        decimal TaxPer = 0;
                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Doctor applicable percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtDoctorApplicablePercent.Text = "0.00";
                            txtDoctorApplicableAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }
                        txtDoctorApplicablePercent.Text = TaxPer.ToString("0.00");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtDoctorApplicableAmount.Text = "0.00";
            }
        }

        private void chkRateEditable_Click(object sender, RoutedEventArgs e)
        {
            if (chkRateEditable.IsChecked == true)
            {
                txtMaxRate.IsEnabled = true;
                txtMinRate.IsEnabled = true;
            }
            else
            {
                txtMaxRate.IsEnabled = false;
                txtMinRate.IsEnabled = false;
                txtMaxRate.Text = "0.00";
                txtMinRate.Text = "0.00";
            }
        }

        private void chkSeniorCitizen_Click(object sender, RoutedEventArgs e)
        {
            if (chkSeniorCitizen.IsChecked == true)
            {
                txtSeniorCitizenPer.IsEnabled = true;
                txtSeniorCitizenPerAmount.IsEnabled = true;
                txtSeniorCitizenAge.IsEnabled = true;
            }
            if (chkSeniorCitizen.IsChecked == false)
            {
                txtSeniorCitizenPer.IsEnabled = false;
                txtSeniorCitizenPerAmount.IsEnabled = false;
                txtSeniorCitizenPerAmount.Text = "0.00";
                txtSeniorCitizenPer.Text = "0.00";
                txtSeniorCitizenAge.IsEnabled = false;
                txtSeniorCitizenAge.Text = "0";
            }
        }

        private void txtSeniorCitizenPer_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtSeniorCitizenPer.Text.Equals("") && (txtSeniorCitizenPer.Text != "0"))
                {
                    if (!txtSeniorCitizenPer.Text.Equals("") && (txtSeniorCitizenPer.Text != "0"))
                    {
                        if (Extensions.IsItDecimal(txtSeniorCitizenPer.Text) == false)
                        {
                            if (Convert.ToDecimal(txtSeniorCitizenPer.Text) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtSeniorCitizenPer.Text = "0.00";
                                txtSeniorCitizenPerAmount.Text = "0.00";
                                return;
                            }
                            String str1 = txtSeniorCitizenPer.Text.Substring(txtSeniorCitizenPer.Text.IndexOf(".") + 1);
                            if (Convert.ToDecimal(str1) > 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW.Show();
                                txtSeniorCitizenPer.Text = "0.00";
                                return;
                            }
                        }
                    }
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal Percent = 0;
                        if (Extensions.IsItDecimal(txtSeniorCitizenPer.Text) == false)
                        {
                            Percent = Convert.ToDecimal(txtSeniorCitizenPer.Text);
                        }
                        else
                        {
                            txtSeniorCitizenPer.SetValidation("Concession Percent should be number");
                            txtSeniorCitizenPer.RaiseValidationError();
                            Percent = 0;
                        }
                        decimal TaxAmount = 0;
                        TaxAmount = ((Rate * Percent) / 100);
                        txtSeniorCitizenPerAmount.Text = TaxAmount.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtSeniorCitizenPer.Text = "0.00";
            }
        }

        private void txtSeniorCitizenPerAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!txtSeniorCitizenPerAmount.Text.Equals("") && (txtSeniorCitizenPerAmount.Text != "0"))
                {
                    if (!txtServiceRate.Text.Equals("") && (txtServiceRate.Text != "0"))
                    {
                        decimal Rate = 0;
                        Rate = Convert.ToDecimal(txtServiceRate.Text);

                        decimal TaxAmount = 0;
                        if (Extensions.IsPositiveNumber(txtServiceTaxPercentage.Text) == false)
                        {
                            if (Extensions.IsItDecimal(txtSeniorCitizenPerAmount.Text) == false)
                            {
                                TaxAmount = Convert.ToDecimal(txtSeniorCitizenPerAmount.Text);
                            }
                        }
                        //else
                        //{
                        //    txtSeniorCitizenPerAmount.SetValidation("Concession Amount should be positive number");
                        //    txtSeniorCitizenPerAmount.RaiseValidationError();
                        //    TaxAmount = 0;
                        //    txtSeniorCitizenPerAmount.Text = "0.00";
                        //}
                        decimal TaxPer = 0;
                        TaxPer = (100 * TaxAmount) / Rate;
                        if (TaxPer > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                  new MessageBoxControl.MessageBoxChildWindow("", "Concession percentage should not be greater than 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            txtSeniorCitizenPer.Text = "0.00";
                            txtSeniorCitizenPerAmount.Text = "0.00";
                            TaxPer = 0;
                            return;
                        }
                        txtSeniorCitizenPer.Text = TaxPer.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtSeniorCitizenPerAmount.Text = "0.00";
            }
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            objAnimation.Invoke(RotationType.Forward);
            AssignButtons.Visibility = Visibility.Collapsed;
            ServiceClassList = new List<clsServiceMasterVO>();
            grdServices.SelectedItem = null;
            this.DataContext = null;
            FillClassGrid();
            SetCommandButtonState("New");
            // cmbSpecialization1.TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            // cmbSubSpecialization1.TextBox.BorderBrush = new SolidColorBrush(Colors.Red);

        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            IsValidate = true;
            ValidateSaveMofify();
            if (IsValidate == true)
            {
                msgText = "Are you sure you want to Save the Record ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        if (CheckDuplicasy())
                        {
                            SaveServiceMaster();
                            SetCommandButtonState("Save");
                        }
                    }
                };
                msgWD.Show();
            }
        }
        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            FillServiceGrid();
            FillClassGrid();
            this.DataContext = null;
            ClassList = new List<clsServiceMasterVO>();
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Billing Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                IsCancel = true;
            }
        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            IsValidate = true;
            ValidateSaveMofify();
            if (IsValidate == true)
            {
                msgText = "Are you sure you want to Modify the Record ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        if (CheckDuplicasy())
                        {
                            if (objServiceVO != null)
                            {
                                ModifyServiceMaster();
                            }
                        }
                    }
                };
                msgWD.Show();
            }
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {

            //FillClassGrid();
            this.DataContext = new clsServiceMasterVO();

            AssignButtons.Visibility = Visibility.Collapsed;
            ViewService();
            //  cmbSpecialization1.TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            //cmbSubSpecialization1.TextBox.BorderBrush = new SolidColorBrush(Colors.Red);

        }

        private void grdServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsServiceMasterVO)grdServices.SelectedItem != null)
            {
                if (((clsServiceMasterVO)grdServices.SelectedItem).Status == false || ((clsServiceMasterVO)grdServices.SelectedItem).IsPackage == true)
                {
                    AssignButtons.Visibility = Visibility.Collapsed;
                    cmdAssignTariff.IsEnabled = false;
                }
                else
                {
                    AssignButtons.Visibility = Visibility.Visible;
                    cmdAssignTariff.IsEnabled = true;
                }
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            long ID = Convert.ToInt64(chk.Tag);
            clsAddServiceMasterBizActionVO objBizAction = new clsAddServiceMasterBizActionVO();
            objBizAction.UpdateServiceMasterStatus = true;
            objBizAction.ServiceMasterDetails = new clsServiceMasterVO();
            objBizAction.ServiceMasterDetails.ID = ID;
            objBizAction.ServiceMasterDetails.Status = (bool)chk.IsChecked;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Service Master", "Service Status Changed Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                    FillServiceGrid();

                }
            };
            client.ProcessAsync(objBizAction, User); //new clsUserVO());
            client.CloseAsync();
        }

        private void chkStaffDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (chkStaffDiscount.IsChecked == true)
            {
                txtStaffDiscountAmount.IsEnabled = true;
                txtStaffDiscountPercentage.IsEnabled = true;
            }
            if (chkStaffDiscount.IsChecked == false)
            {
                txtStaffDiscountAmount.IsEnabled = false;
                txtStaffDiscountPercentage.IsEnabled = false;
                txtStaffDiscountAmount.Text = "0.00";
                txtStaffDiscountPercentage.Text = "0.00";
            }
        }

        private void chkConcession_Click(object sender, RoutedEventArgs e)
        {
            if (chkConcession.IsChecked == true)
            {
                txtConcessionAmount.IsEnabled = true;
                txtConcessionPercentage.IsEnabled = true;
            }
            if (chkConcession.IsChecked == false)
            {
                txtConcessionAmount.IsEnabled = false;
                txtConcessionPercentage.IsEnabled = false;

                txtConcessionAmount.Text = "0.00";
                txtConcessionPercentage.Text = "0.00";
            }
        }

        private void chkStaffParentDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (chkStaffParentDiscount.IsChecked == true)
            {
                txtStaffParentAmount.IsEnabled = true;
                txtStaffParentPercentage.IsEnabled = true;
            }
            if (chkStaffParentDiscount.IsChecked == false)
            {
                txtStaffParentAmount.IsEnabled = false;
                txtStaffParentPercentage.IsEnabled = false;
                txtStaffParentAmount.Text = "0.00";
                txtStaffParentPercentage.Text = "0.00";
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillServiceGrid();
        }
        #region Fill ComboBoxes
        private void FillSpecialization()
        {
            wait.Show();
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Specialization;
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

                        cmbSpecialization.ItemsSource = null;
                        cmbSpecialization.ItemsSource = objList;
                        cmbSpecialization.SelectedItem = objList[0];

                        cmbSpecialization1.ItemsSource = null;
                        cmbSpecialization1.ItemsSource = objList;
                        cmbSpecialization1.SelectedItem = objList[0];
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                wait.Close();
            }
        }

        private void FillSubSpecialization(string fkSpecializationID)
        {
            //wait.Show();
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                if (fkSpecializationID != null)
                {
                    BizAction.Parent = new KeyValue { Key = fkSpecializationID, Value = "fkSpecializationID" };
                }
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
                        cmbSubSpecialization.ItemsSource = null;
                        cmbSubSpecialization.ItemsSource = objList;
                        cmbSubSpecialization.SelectedValue = objList[0].ID;

                        cmbSubSpecialization1.ItemsSource = null;
                        cmbSubSpecialization1.ItemsSource = objList;
                        cmbSubSpecialization1.SelectedValue = objList[0].ID;
                    }
                    if (this.DataContext != null)
                    {
                        cmbSubSpecialization1.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;
                    }
                    //wait.Close();
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                //wait.Close();
            }
        }

        private void FillCodeType()
        {
            wait.Show();
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CodeType;
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

                        cboCodeType.ItemsSource = null;
                        cboCodeType.ItemsSource = objList;
                        cboCodeType.SelectedValue = objList[0].ID;
                    }
                    if (this.DataContext != null)
                    {
                        cboCodeType.SelectedValue = ((clsServiceMasterVO)this.DataContext).CodeType;
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                wait.Close();
            }
        }

        private void FillSACCodes()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_SACCodes;
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
                        List<MasterListItem> objListCodes = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        foreach (var item in objList.ToList())
                        {
                            if (item.ID != 0)
                                item.Description = item.Code;
                        }
                        cmbSACCode.ItemsSource = null;
                        cmbSACCode.ItemsSource = objList;
                        cmbSACCode.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region private methods
        private void FillServiceGrid()
        {

            try
            {
                wait.Show();
                clsGetServiceMasterListBizActionVO BizActionObj = new clsGetServiceMasterListBizActionVO();
                BizActionObj.GetAllServiceListDetails = true;
                BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                BizActionObj.IsPagingEnabled = true;
                BizActionObj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizActionObj.MaximumRows = DataList.PageSize;
                if (txtServiceCode.Text != string.Empty && txtServiceCode.Text != null)
                    BizActionObj.ServiceCode = txtServiceCode.Text.Trim();
                BizActionObj.ServiceName = txtServiceName.Text;
                if (cmbSpecialization.SelectedItem == null)
                {
                    BizActionObj.Specialization = 0;
                }
                else
                {
                    BizActionObj.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID == null ? 0 : ((MasterListItem)cmbSpecialization.SelectedItem).ID;     //(long) cboSpecialization.SelectedItem; //((clsServiceMasterVO)this.DataContext).Specialization;
                }
                if (cmbSubSpecialization.SelectedItem == null)
                {
                    BizActionObj.SubSpecialization = 0;
                }
                else
                {
                    BizActionObj.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID == null ? 0 : ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetServiceMasterListBizActionVO result = args.Result as clsGetServiceMasterListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.ServiceList != null)
                        {
                            DataList.Clear();
                            foreach (var item in result.ServiceList)
                            {
                                DataList.Add(item);
                            }
                            grdServices.ItemsSource = null;
                            grdServices.ItemsSource = DataList;
                            //grdServices.SelectedIndex = -1;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizActionObj.MaximumRows;
                            dgDataPager.Source = DataList;
                        }
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizActionObj, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
                wait.Close();
            }
        }
        string previousServiceName = String.Empty;
        clsServiceMasterVO objServiceVO = new clsServiceMasterVO();
        private void ViewService()
        {
            objServiceVO = new clsServiceMasterVO();
            objServiceVO = (clsServiceMasterVO)grdServices.SelectedItem;
            clsGetServiceMasterListBizActionVO objVo = new clsGetServiceMasterListBizActionVO();
            objVo.GetAllServiceMasterDetailsForID = true;
            objVo.ServiceMaster = new clsServiceMasterVO();
            objVo.ServiceList = new List<clsServiceMasterVO>();
            objVo.ServiceMaster.ID = objServiceVO.ID;
            objVo.ServiceMaster.UnitID = objServiceVO.UnitID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    clsGetServiceMasterListBizActionVO obj = args.Result as clsGetServiceMasterListBizActionVO;
                    if (obj != null)
                    {
                        this.DataContext = obj.ServiceMaster;
                        previousServiceName = obj.ServiceMaster.Description;
                        ((clsServiceMasterVO)this.DataContext).EditMode = true;
                        if (objServiceVO != null)
                        {
                            objServiceVO.EditMode = true;
                            if (((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.CodeType != 0)
                            {
                                cboCodeType.SelectedValue = ((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.CodeType;
                            }
                            if (((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.Specialization != 0)
                            {
                                cmbSpecialization1.SelectedValue = ((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.Specialization;
                            }

                            //for GST 27062017
                            if (((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.SACCodeID != 0)
                            {
                                cmbSACCode.SelectedValue = ((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.SACCodeID;
                            }
                            //End

                            FillSubSpecialization(objServiceVO.Specialization.ToString());
                            if (((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.SubSpecialization != 0)
                            {
                                cmbSubSpecialization1.SelectedValue = ((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.SubSpecialization;
                            }
                            if (((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.Rate >= 0)
                            {
                                txtServiceRate.Text = Convert.ToString(((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.Rate);
                            }
                            if (!String.IsNullOrEmpty(objServiceVO.CodeDetails))
                                txtCode.Text = objServiceVO.CodeDetails;
                            if (obj.ServiceMaster.Concession == true)
                            {
                                txtConcessionAmount.IsEnabled = true;
                                txtConcessionPercentage.IsEnabled = true;
                            }
                            else
                            {
                                txtConcessionAmount.IsEnabled = false;
                                txtConcessionPercentage.IsEnabled = false;
                            }
                            if (obj.ServiceMaster.DoctorShare == true)
                            {
                                txtDoctorApplicableAmount.IsEnabled = true;
                                txtDoctorApplicablePercent.IsEnabled = true;
                            }
                            else
                            {
                                txtDoctorApplicableAmount.IsEnabled = false;
                                txtDoctorApplicablePercent.IsEnabled = false;
                            }
                            if (obj.ServiceMaster.RateEditable == true)
                            {
                                txtMinRate.IsEnabled = true;
                                txtMaxRate.IsEnabled = true;
                            }
                            else
                            {
                                txtMinRate.IsEnabled = false;
                                txtMaxRate.IsEnabled = false;
                            }

                            if (obj.ServiceMaster.ServiceTax == true)
                            {
                                txtServiceTaxAmount.IsEnabled = true;
                                txtServiceTaxPercentage.IsEnabled = true;
                            }
                            else
                            {
                                txtServiceTaxAmount.IsEnabled = false;
                                txtServiceTaxPercentage.IsEnabled = false;
                            }

                            if (obj.ServiceMaster.StaffDependantDiscount == true)
                            {
                                txtStaffParentAmount.IsEnabled = true;
                                txtStaffParentPercentage.IsEnabled = true;
                            }
                            else
                            {
                                txtStaffParentAmount.IsEnabled = false;
                                txtStaffParentPercentage.IsEnabled = false;
                            }
                            if (obj.ServiceMaster.StaffDiscount == true)
                            {
                                txtStaffDiscountAmount.IsEnabled = true;
                                txtStaffDiscountPercentage.IsEnabled = true;
                            }
                            else
                            {
                                txtStaffDiscountAmount.IsEnabled = false;
                                txtStaffDiscountPercentage.IsEnabled = false;
                            }


                            if (obj.ServiceMaster.SeniorCitizen == true)
                            {
                                txtSeniorCitizenAge.IsEnabled = true;
                                txtSeniorCitizenPerAmount.IsEnabled = true;
                                txtSeniorCitizenPer.IsEnabled = true;
                            }
                            else
                            {
                                txtSeniorCitizenAge.IsEnabled = false;
                                txtSeniorCitizenPerAmount.IsEnabled = false;
                                txtSeniorCitizenPer.IsEnabled = false;
                            }
                            if (((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.Rate != null)
                            {
                                txtServiceRate.Text = Convert.ToString(((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.Rate);
                            }
                            if (((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.IsFavourite == true)
                            {
                                IsFavourite.IsChecked = true;
                            }
                            else
                            {
                                IsFavourite.IsChecked = false;
                            }
                            if (((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.IsLinkWithInventory == true)
                            {
                                ChklnkwithInv.IsChecked = true;
                            }
                            else
                            {
                                ChklnkwithInv.IsChecked = false;
                            }
                            ServiceClassList = ClassList;
                            if (obj.ServiceList != null)
                            {
                                if (obj.ServiceList.Count > 0)
                                {
                                    foreach (var item in obj.ServiceList)
                                    {
                                        foreach (var item1 in ClassList)
                                        {
                                            if (item1.ClassID == item.ClassID && item.Rate > 0)
                                            {
                                                item1.Rate = item.Rate;
                                                item1.IsSelected = true;
                                                item1.IsEnabled = true;
                                                break;
                                            }
                                        }
                                        //ClassList.Where(z => z.ID == item.ID).ToList().ForEach((z => z.Rate = item.Rate) && (z=> z.IsSelected = item.IsSelected) && (z => z.IsEnabled = item.IsEnabled));
                                    }
                                }
                                else
                                {
                                    foreach (var item1 in ClassList)
                                    {
                                        item1.Rate = Convert.ToDecimal("0.0");
                                        item1.IsSelected = false;
                                        item1.IsEnabled = true;
                                    }
                                }
                                ServiceClassList = ClassList.DeepCopy();
                                var ItemCount = from r in ClassList
                                                where r.IsSelected == false
                                                select r;
                                if (ItemCount.ToList().Count() > 0)
                                {
                                    chkApplyToAllClass.IsChecked = false;
                                }
                                else
                                {
                                    foreach (var item in ServiceClassList)
                                    {
                                        if (item.Rate == Convert.ToDecimal(txtServiceRate.Text) && item.Rate != 0)
                                        {

                                            chkApplyToAllClass.IsChecked = true;
                                        }
                                        else
                                        {

                                            chkApplyToAllClass.IsChecked = false;
                                        }
                                        item.IsEnabled = true;
                                        if (item.Rate == Convert.ToDecimal(txtServiceRate.Text))
                                        {
                                            chkApplyToAllClass.IsChecked = true;
                                        }
                                        else
                                            chkApplyToAllClass.IsChecked = false;
                                    }
                                    grdClass.ItemsSource = null;
                                    grdClass.ItemsSource = ServiceClassList;
                                    grdClass.UpdateLayout();
                                    grdClass.Focus();
                                }
                            }
                            objAnimation.Invoke(RotationType.Forward);
                            if (((clsGetServiceMasterListBizActionVO)args.Result).ServiceMaster.Status == true)
                                SetCommandButtonState("View");
                            else
                                SetCommandButtonState("View1");
                        }
                    }
                }
            };
            client.ProcessAsync(objVo, new clsUserVO());
            client.CloseAsync();
        }
        public clsAddServiceMasterBizActionVO objBizActionVO;

        public string ServiceName1 = "";
        private void SaveServiceMaster()
        {
            try
            {
                objBizActionVO = new clsAddServiceMasterBizActionVO();
                objBizActionVO.IsModify = false;
                objBizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                objBizActionVO.ServiceClassList = new List<clsServiceMasterVO>();
                //objBizActionVO.ServiceClassList = ClassList;
                foreach (clsServiceMasterVO item in grdClass.ItemsSource)
                {
                    objBizActionVO.ServiceClassList.Add(item);
                }
                objBizActionVO.ServiceMasterDetails.EditMode = true;//((clsServiceMasterVO)this.DataContext).EditMode;
                objBizActionVO.ServiceMasterDetails.ServiceCode = txtServiceCode1.Text;
                objBizActionVO.ServiceMasterDetails.ID = pkServiceID == null ? 0 : pkServiceID;
                objBizActionVO.ServiceMasterDetails.CodeType = ((MasterListItem)cboCodeType.SelectedItem).ID;
                objBizActionVO.ServiceMasterDetails.CodeDetails = txtCode.Text == "" ? "" : txtCode.Text;
                ServiceName1 = txtServiceName1.Text == "" ? "" : txtServiceName1.Text;
                objBizActionVO.ServiceMasterDetails.Specialization = ((MasterListItem)cmbSpecialization1.SelectedItem).ID;
                objBizActionVO.ServiceMasterDetails.SubSpecialization = ((MasterListItem)cmbSubSpecialization1.SelectedItem).ID;
                objBizActionVO.ServiceMasterDetails.ServiceName = txtServiceName1.Text == "" ? "" : txtServiceName1.Text;
                objBizActionVO.ServiceMasterDetails.ShortDescription = txtServiceShortDescription.Text == "" ? "" : txtServiceShortDescription.Text;
                objBizActionVO.ServiceMasterDetails.LongDescription = txtServiceLongDescription.Text == "" ? "" : txtServiceLongDescription.Text;
                if (!String.IsNullOrEmpty(txtCode.Text))
                {
                    objBizActionVO.ServiceMasterDetails.CodeDetails = txtCode.Text;
                }
                if (IsFavourite.IsEnabled == true)
                    objBizActionVO.ServiceMasterDetails.IsFavourite = true;
                else
                    objBizActionVO.ServiceMasterDetails.IsFavourite = false;

                if (ChklnkwithInv.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.IsLinkWithInventory = true;
                else
                    objBizActionVO.ServiceMasterDetails.IsLinkWithInventory = false;
                if (chkStaffDiscount.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.StaffDiscount = true;//((clsServiceMasterVO)this.DataContext).StaffDiscount;
                else
                    objBizActionVO.ServiceMasterDetails.StaffDiscount = false;
                objBizActionVO.ServiceMasterDetails.StaffDiscountAmount = txtStaffDiscountAmount.Text == "" ? 0 : decimal.Parse(txtStaffDiscountAmount.Text);
                objBizActionVO.ServiceMasterDetails.StaffDiscountPercent = txtStaffDiscountPercentage.Text == "" ? 0 : decimal.Parse(txtStaffDiscountPercentage.Text);

                if (chkStaffParentDiscount.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.StaffDependantDiscount = true;
                else
                    objBizActionVO.ServiceMasterDetails.StaffDependantDiscount = false;
                objBizActionVO.ServiceMasterDetails.StaffDependantDiscountAmount = txtStaffParentAmount.Text == "" ? 0 : decimal.Parse(txtStaffParentAmount.Text);
                objBizActionVO.ServiceMasterDetails.StaffDependantDiscountPercent = txtStaffParentPercentage.Text == "" ? 0 : decimal.Parse(txtStaffParentPercentage.Text);

                if (chkConcession.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.Concession = true;
                else
                    objBizActionVO.ServiceMasterDetails.Concession = false;
                objBizActionVO.ServiceMasterDetails.ConcessionAmount = txtConcessionAmount.Text == "" ? 0 : decimal.Parse(txtConcessionAmount.Text);
                objBizActionVO.ServiceMasterDetails.ConcessionPercent = txtConcessionPercentage.Text == "" ? 0 : decimal.Parse(txtConcessionPercentage.Text);
                if (chkServiceTax.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.ServiceTax = true;
                else
                    objBizActionVO.ServiceMasterDetails.ServiceTax = false;
                objBizActionVO.ServiceMasterDetails.ServiceTaxAmount = txtServiceTaxAmount.Text == "" ? 0 : decimal.Parse(txtServiceTaxAmount.Text);
                objBizActionVO.ServiceMasterDetails.ServiceTaxPercent = txtServiceTaxPercentage.Text == "" ? 0 : decimal.Parse(txtServiceTaxPercentage.Text);

                if (chkLuxuryTax.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.ServiceTax = true;
                else
                    objBizActionVO.ServiceMasterDetails.ServiceTax = false;
                objBizActionVO.ServiceMasterDetails.LuxuryTaxAmount = txtLuxuryTaxAmount.Text == "" ? 0 : decimal.Parse(txtServiceTaxAmount.Text);
                objBizActionVO.ServiceMasterDetails.LuxuryTaxPercent = txtLuxuryTaxPercentage.Text == "" ? 0 : decimal.Parse(txtServiceTaxPercentage.Text);

                if (chkSeniorCitizen.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.SeniorCitizen = true;
                else
                    objBizActionVO.ServiceMasterDetails.SeniorCitizen = false;
                objBizActionVO.ServiceMasterDetails.SeniorCitizenConAmount = txtSeniorCitizenPerAmount.Text == "" ? 0 : decimal.Parse(txtSeniorCitizenPerAmount.Text);
                objBizActionVO.ServiceMasterDetails.SeniorCitizenConPercent = txtSeniorCitizenPer.Text == "" ? 0 : decimal.Parse(txtSeniorCitizenPer.Text);
                if (!String.IsNullOrEmpty(txtSeniorCitizenAge.Text))
                    objBizActionVO.ServiceMasterDetails.SeniorCitizenAge = Convert.ToInt32(txtSeniorCitizenAge.Text);
                if (chkInHouse.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.InHouse = true;
                else
                    objBizActionVO.ServiceMasterDetails.InHouse = false;
                if (chkApplicableToAllDoctors.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.DoctorShare = true;
                else
                    objBizActionVO.ServiceMasterDetails.DoctorShare = false;
                objBizActionVO.ServiceMasterDetails.DoctorShareAmount = txtDoctorApplicableAmount.Text == "" ? 0 : decimal.Parse(txtDoctorApplicableAmount.Text);
                objBizActionVO.ServiceMasterDetails.DoctorSharePercentage = txtDoctorApplicablePercent.Text == "" ? 0 : decimal.Parse(txtDoctorApplicablePercent.Text);

                if (chkRateEditable.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.RateEditable = true;
                else
                    objBizActionVO.ServiceMasterDetails.RateEditable = false;
                objBizActionVO.ServiceMasterDetails.MaxRate = txtMaxRate.Text == "" ? 0 : decimal.Parse(txtMaxRate.Text);
                objBizActionVO.ServiceMasterDetails.MinRate = txtMinRate.Text == "" ? 0 : decimal.Parse(txtMinRate.Text);

                objBizActionVO.ServiceMasterDetails.Rate = txtServiceRate.Text == "" ? 0 : decimal.Parse(txtServiceRate.Text);
                objBizActionVO.ServiceMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.ServiceMasterDetails.Status = true;
                objBizActionVO.ServiceMasterDetails.CreatedUnitID = 1;
                objBizActionVO.ServiceMasterDetails.UpdatedUnitID = 1;
                objBizActionVO.ServiceMasterDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                objBizActionVO.ServiceMasterDetails.AddedOn = DateTime.Now.DayOfWeek.ToString();
                objBizActionVO.ServiceMasterDetails.AddedDateTime = DateTime.Now;
                objBizActionVO.ServiceMasterDetails.TariffIDList = lstTariffs;

                if (IsPackage.IsChecked == true)
                {
                    objBizActionVO.ServiceMasterDetails.IsPackage = true;
                }

                if (cmbSACCode.SelectedItem != null)
                    objBizActionVO.ServiceMasterDetails.SACCodeID = ((MasterListItem)cmbSACCode.SelectedItem).ID;
                //**********************Inserting Service Master***********************************//

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddServiceMasterBizActionVO)arg.Result).SuccessStatus == 3)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because service name already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            SetCommandButtonState("New");
                        }
                        else if (((clsAddServiceMasterBizActionVO)arg.Result).SuccessStatus == 0)
                        {
                            string msgTitle = "";
                            string msgText = "Service Added Successfully";
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    objAnimation.Invoke(RotationType.Backward);
                                    FillServiceGrid();
                                    SetCommandButtonState("Load");
                                }
                            };
                            msgW.Show();
                        }
                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "Some error occurred while saving";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                        SetCommandButtonState("New");
                    }
                };
                client.ProcessAsync(objBizActionVO, new clsUserVO());
                client.CloseAsync();
                //}
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                SetCommandButtonState("New");
            }
        }

        private void ModifyServiceMaster()
        {
            try
            {
                objBizActionVO = new clsAddServiceMasterBizActionVO();
                objBizActionVO.IsModify = true;
                objBizActionVO.ServiceMasterDetails = new clsServiceMasterVO();
                objBizActionVO.ServiceClassList = new List<clsServiceMasterVO>();
                // objBizActionVO.ServiceClassList = ClassList;
                foreach (clsServiceMasterVO item in grdClass.ItemsSource)
                {

                    objBizActionVO.ServiceClassList.Add(item);
                }
                objBizActionVO.ServiceMasterDetails.EditMode = ((clsServiceMasterVO)this.DataContext).EditMode;
                objBizActionVO.ServiceMasterDetails.ServiceCode = ((clsServiceMasterVO)this.DataContext).ServiceCode;
                objBizActionVO.ServiceMasterDetails.ID = ((clsServiceMasterVO)this.DataContext).ID;
                objBizActionVO.ServiceMasterDetails.CodeType = ((MasterListItem)cboCodeType.SelectedItem).ID;
                objBizActionVO.ServiceMasterDetails.Code = txtServiceCode1.Text == "" ? "" : txtServiceCode1.Text;
                objBizActionVO.ServiceMasterDetails.Specialization = ((MasterListItem)cmbSpecialization1.SelectedItem).ID;
                objBizActionVO.ServiceMasterDetails.SubSpecialization = ((MasterListItem)cmbSubSpecialization1.SelectedItem).ID;
                objBizActionVO.ServiceMasterDetails.ServiceName = txtServiceName1.Text == "" ? "" : txtServiceName1.Text;
                objBizActionVO.ServiceMasterDetails.ShortDescription = txtServiceShortDescription.Text == "" ? "" : txtServiceShortDescription.Text;
                objBizActionVO.ServiceMasterDetails.LongDescription = txtServiceLongDescription.Text == "" ? "" : txtServiceLongDescription.Text;
                objBizActionVO.ServiceMasterDetails.CodeDetails = txtCode.Text == "" ? "" : txtCode.Text;
                objBizActionVO.ServiceMasterDetails.Rate = Convert.ToDecimal(txtServiceRate.Text == "" ? "" : txtServiceRate.Text);

                if (IsFavourite.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.IsFavourite = true;
                else
                    objBizActionVO.ServiceMasterDetails.IsFavourite = false;

                if (ChklnkwithInv.IsChecked == true)
                    objBizActionVO.ServiceMasterDetails.IsLinkWithInventory = true;
                else
                    objBizActionVO.ServiceMasterDetails.IsLinkWithInventory = false;

                objBizActionVO.ServiceMasterDetails.StaffDiscount = ((clsServiceMasterVO)this.DataContext).StaffDiscount;
                objBizActionVO.ServiceMasterDetails.StaffDiscountAmount = txtStaffDiscountAmount.Text == "" ? 0 : decimal.Parse(txtStaffDiscountAmount.Text);
                objBizActionVO.ServiceMasterDetails.StaffDiscountPercent = txtStaffDiscountPercentage.Text == "" ? 0 : decimal.Parse(txtStaffDiscountPercentage.Text);
                objBizActionVO.ServiceMasterDetails.StaffDependantDiscount = ((clsServiceMasterVO)this.DataContext).StaffDependantDiscount;
                objBizActionVO.ServiceMasterDetails.StaffDependantDiscountAmount = txtStaffParentAmount.Text == "" ? 0 : decimal.Parse(txtStaffParentAmount.Text);
                objBizActionVO.ServiceMasterDetails.StaffDependantDiscountPercent = txtStaffParentPercentage.Text == "" ? 0 : decimal.Parse(txtStaffParentPercentage.Text);
                objBizActionVO.ServiceMasterDetails.Concession = ((clsServiceMasterVO)this.DataContext).Concession;

                objBizActionVO.ServiceMasterDetails.ConcessionAmount = txtConcessionAmount.Text == "" ? 0 : decimal.Parse(txtConcessionAmount.Text);
                objBizActionVO.ServiceMasterDetails.ConcessionPercent = txtConcessionPercentage.Text == "" ? 0 : decimal.Parse(txtConcessionPercentage.Text);

                objBizActionVO.ServiceMasterDetails.ServiceTax = ((clsServiceMasterVO)this.DataContext).ServiceTax;
                objBizActionVO.ServiceMasterDetails.ServiceTaxAmount = txtServiceTaxAmount.Text == "" ? 0 : decimal.Parse(txtServiceTaxAmount.Text);
                objBizActionVO.ServiceMasterDetails.ServiceTaxPercent = txtServiceTaxPercentage.Text == "" ? 0 : decimal.Parse(txtServiceTaxPercentage.Text);
                objBizActionVO.ServiceMasterDetails.LuxuryTaxAmount = txtLuxuryTaxAmount.Text == "" ? 0 : decimal.Parse(txtLuxuryTaxAmount.Text);
                objBizActionVO.ServiceMasterDetails.LuxuryTaxPercent = txtLuxuryTaxPercentage.Text == "" ? 0 : decimal.Parse(txtLuxuryTaxPercentage.Text);


                objBizActionVO.ServiceMasterDetails.SeniorCitizen = ((clsServiceMasterVO)this.DataContext).SeniorCitizen;
                objBizActionVO.ServiceMasterDetails.SeniorCitizenConAmount = txtSeniorCitizenPerAmount.Text == "" ? 0 : decimal.Parse(txtSeniorCitizenPerAmount.Text);
                objBizActionVO.ServiceMasterDetails.SeniorCitizenConPercent = txtSeniorCitizenPer.Text == "" ? 0 : decimal.Parse(txtSeniorCitizenPer.Text);
                objBizActionVO.ServiceMasterDetails.SeniorCitizenAge = ((clsServiceMasterVO)this.DataContext).SeniorCitizenAge;
                objBizActionVO.ServiceMasterDetails.InHouse = ((clsServiceMasterVO)this.DataContext).InHouse;
                objBizActionVO.ServiceMasterDetails.DoctorShare = ((clsServiceMasterVO)this.DataContext).DoctorShare;
                objBizActionVO.ServiceMasterDetails.DoctorSharePercentage = ((clsServiceMasterVO)this.DataContext).DoctorSharePercentage;
                objBizActionVO.ServiceMasterDetails.DoctorShareAmount = txtDoctorApplicableAmount.Text == "" ? 0 : decimal.Parse(txtDoctorApplicableAmount.Text);
                objBizActionVO.ServiceMasterDetails.DoctorSharePercentage = txtDoctorApplicablePercent.Text == "" ? 0 : decimal.Parse(txtDoctorApplicablePercent.Text);
                objBizActionVO.ServiceMasterDetails.RateEditable = ((clsServiceMasterVO)this.DataContext).RateEditable;
                objBizActionVO.ServiceMasterDetails.MaxRate = txtMaxRate.Text == "" ? 0 : decimal.Parse(txtMaxRate.Text);
                objBizActionVO.ServiceMasterDetails.MinRate = txtMinRate.Text == "" ? 0 : decimal.Parse(txtMinRate.Text);
                objBizActionVO.ServiceMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.ServiceMasterDetails.Status = true;
                objBizActionVO.ServiceMasterDetails.CreatedUnitID = 1;
                objBizActionVO.ServiceMasterDetails.UpdatedUnitID = 1;
                objBizActionVO.ServiceMasterDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                objBizActionVO.ServiceMasterDetails.AddedOn = DateTime.Now.DayOfWeek.ToString();
                objBizActionVO.ServiceMasterDetails.AddedDateTime = DateTime.Now;
                objBizActionVO.ServiceMasterDetails.IsPackage = ((clsServiceMasterVO)this.DataContext).IsPackage;
                objBizActionVO.ServiceMasterDetails.TariffIDList = lstTariffs;

                if (cmbSACCode.SelectedItem != null)
                    objBizActionVO.ServiceMasterDetails.SACCodeID = ((MasterListItem)cmbSACCode.SelectedItem).ID;

                //**********************Inserting Service Master***********************************//

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddServiceMasterBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be Modify because service name already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                           {
                               txtServiceName1.Text = previousServiceName;
                           };
                            msgW1.Show();
                        }
                        else if (((clsAddServiceMasterBizActionVO)arg.Result).SuccessStatus == 0)
                        {
                            string msgTitle = "";
                            string msgText = "Service Modified Successfully";
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.Show();
                            objAnimation.Invoke(RotationType.Backward);
                            txtServiceCode.Text = "";
                            txtServiceName.Text = "";
                            FillSpecialization();
                            FillSACCodes();
                            FillServiceGrid();
                            FillClassGrid();
                            AssignButtons.Visibility = Visibility.Visible;
                            SetCommandButtonState("Load");
                        }
                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "Some error occurred while Modifying";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                        SetCommandButtonState("View");
                    }
                };
                client.ProcessAsync(objBizActionVO, new clsUserVO());
                client.CloseAsync();
                //}
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
            }
        }

        private bool CheckDuplicasy()
        {
            clsServiceMasterVO ServiceItem1;
            if (IsNew)
            {
                ServiceItem1 = ((PagedSortableCollectionView<clsServiceMasterVO>)grdServices.ItemsSource).FirstOrDefault(p => p.ServiceName.ToUpper().Trim().Equals(txtServiceName.Text.ToUpper()));
            }
            else
            {
                ServiceItem1 = ((PagedSortableCollectionView<clsServiceMasterVO>)grdServices.ItemsSource).FirstOrDefault(p => p.ServiceName.ToUpper().Trim().Equals(txtServiceName.Text.ToUpper()) && p.ID != ((clsServiceMasterVO)grdServices.SelectedItem).ID);
                Edit = false;
            }
            if (ServiceItem1 != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because service name already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ClearUI()
        {
            clsServiceMasterVO obj = new clsServiceMasterVO();
            txtServiceCode.Text = "";
            txtCode.Text = "";
            txtServiceLongDescription.Text = "";
            txtServiceName.Text = "";
            txtServiceName1.Text = "";
            txtServiceShortDescription.Text = "";
            txtMinRate.Text = "";
            txtMaxRate.Text = "";
            txtDoctorApplicableAmount.Text = "";
            txtDoctorApplicablePercent.Text = "";
            cboCodeType.SelectedValue = obj.CodeType;
            cmbSpecialization1.SelectedValue = obj.Specialization;
            cmbSubSpecialization1.SelectedValue = obj.SubSpecialization;



            chkApplicableToAllDoctors.IsChecked = obj.DoctorShare;
            chkConcession.IsChecked = obj.Concession;
            txtConcessionAmount.Text = "";
            chkApplyToAllClass.IsChecked = false;
            txtConcessionAmount.IsEnabled = false;
            txtConcessionPercentage.Text = "";
            txtConcessionPercentage.IsEnabled = false;
            chkInHouse.IsChecked = obj.InHouse;
            IsFavourite.IsChecked = obj.IsFavourite;
            ChklnkwithInv.IsChecked = obj.IsLinkWithInventory;
            chkRateEditable.IsChecked = obj.RateEditable;
            txtMinRate.IsEnabled = false;
            txtMaxRate.IsEnabled = false;
            chkServiceTax.IsChecked = obj.ServiceTax;
            txtServiceTaxAmount.Text = "";
            txtServiceTaxAmount.IsEnabled = false;
            txtServiceTaxPercentage.Text = "";
            txtServiceTaxPercentage.IsEnabled = false;
            chkLuxuryTax.IsChecked = obj.ServiceTax;
            txtLuxuryTaxAmount.Text = "";
            txtLuxuryTaxAmount.IsEnabled = false;
            txtLuxuryTaxPercentage.Text = "";
            txtLuxuryTaxPercentage.IsEnabled = false;
            chkStaffDiscount.IsChecked = obj.StaffDependantDiscount;
            txtStaffDiscountAmount.Text = "";
            txtStaffDiscountAmount.IsEnabled = false;
            txtStaffDiscountPercentage.Text = "";
            txtStaffDiscountPercentage.IsEnabled = false;
            chkStaffParentDiscount.IsChecked = obj.StaffDependantDiscount;
            txtStaffParentAmount.Text = "";
            txtStaffParentPercentage.IsEnabled = false;
            txtStaffParentPercentage.Text = "";
            txtStaffParentAmount.IsEnabled = false;
            txtDoctorApplicablePercent.IsEnabled = false;
            txtDoctorApplicableAmount.IsEnabled = false;
            IsNew = false;
            Edit = false;
            grdClass.ItemsSource = null;
            txtSeniorCitizenAge.IsEnabled = false;
            txtSeniorCitizenPerAmount.IsEnabled = false;
            txtSeniorCitizenPer.IsEnabled = false;
            IsPackage.IsChecked = obj.IsPackage;

            txtServiceRate.Text = "";//"0.00";
            cmbSACCode.SelectedValue = (long)0;
        }
        #endregion

        #region Validations
        private void validateSearch()
        {
            if (String.IsNullOrEmpty(txtServiceCode.Text))
            {
                txtServiceCode.SetValidation("Please, Enter Service Code");
                txtServiceCode.RaiseValidationError();
                txtServiceCode.Focus();
                IsValidate = false;
            }
            else
                txtServiceCode.ClearValidationError();
            if (String.IsNullOrEmpty(txtServiceName.Text))
            {
                txtServiceName.SetValidation("Please, Enter ServiceName");
                txtServiceName.RaiseValidationError();
                txtServiceName.Focus();
                IsValidate = false;
            }
            else
                txtServiceName.ClearValidationError();
        }

        private void ValidateSaveMofify()
        {
            try
            {
                if (String.IsNullOrEmpty(txtServiceName1.Text))
                {
                    txtServiceName1.SetValidation("Please, Enter Service Name");
                    txtServiceName1.RaiseValidationError();
                    txtServiceName1.Focus();
                    IsValidate = false;
                }
                else
                {
                    txtServiceName1.ClearValidationError();
                }
                //if (((MasterListItem)cboCodeType.SelectedItem).ID == 0)
                //{
                //    cboCodeType.TextBox.SetValidation("Please, Select Code Type");
                //    cboCodeType.TextBox.RaiseValidationError();
                //    cboCodeType.TextBox.Focus();
                //    IsValidate = false;
                //}
                //else
                //{
                //    cboCodeType.TextBox.ClearValidationError();
                //}
                if (((MasterListItem)cmbSpecialization1.SelectedItem).ID == 0)
                {
                    cmbSpecialization1.TextBox.SetValidation("Please, Select Specialization");
                    cmbSpecialization1.TextBox.RaiseValidationError();
                    cmbSpecialization1.TextBox.Focus();
                    IsValidate = false;
                }
                else
                {
                    cmbSpecialization1.TextBox.ClearValidationError();
                }
                if (((MasterListItem)cmbSubSpecialization1.SelectedItem).ID == 0)
                {
                    cmbSubSpecialization1.TextBox.SetValidation("Please, Select Sub Specialization");
                    cmbSubSpecialization1.TextBox.RaiseValidationError();
                    cmbSubSpecialization1.TextBox.Focus();
                    IsValidate = false;
                }
                else
                {
                    cmbSubSpecialization1.TextBox.ClearValidationError();
                }
                if (String.IsNullOrEmpty(txtServiceShortDescription.Text))
                {
                    txtServiceShortDescription.SetValidation("Please, Enter Short Description");
                    txtServiceShortDescription.RaiseValidationError();
                    txtServiceShortDescription.Focus();
                    IsValidate = false;
                }
                else
                {
                    txtServiceLongDescription.ClearValidationError();
                }
                if (String.IsNullOrEmpty(txtServiceLongDescription.Text))
                {
                    txtServiceLongDescription.SetValidation("Please, Enter Long Description");
                    txtServiceLongDescription.RaiseValidationError();
                    txtServiceLongDescription.Focus();
                    IsValidate = false;
                }
                else
                {
                    txtServiceLongDescription.ClearValidationError();
                }
                if (String.IsNullOrEmpty(txtServiceRate.Text))
                {
                    txtServiceRate.SetValidation("Please, Enter Service Rate");
                    txtServiceRate.RaiseValidationError();
                    txtServiceRate.Focus();
                    IsValidate = false;
                }
                else if (Convert.ToDecimal(txtServiceRate.Text) == 0)
                {
                    txtServiceRate.SetValidation("Service Rate Should Be Greater Than Zero");
                    txtServiceRate.RaiseValidationError();
                    txtServiceRate.Focus();
                    IsValidate = false;
                }
                else
                {
                    txtServiceRate.ClearValidationError();
                }
                if (txtStaffDiscountPercentage.Text != "")
                {
                    if (Extensions.IsItDecimal(txtStaffDiscountPercentage.Text) == true)
                    {
                        txtStaffDiscountPercentage.SetValidation(" Staff Discount Percentage should be number");
                        txtStaffDiscountPercentage.RaiseValidationError();
                        IsValidate = false;
                    }
                }
                else
                {
                    txtStaffDiscountPercentage.ClearValidationError();
                }
                if (txtStaffDiscountAmount.Text != "")
                {
                    if (Extensions.IsItDecimal(txtStaffDiscountAmount.Text) == true)
                    {
                        txtStaffDiscountAmount.SetValidation(" Staff Discount Amount should be number");
                        txtStaffDiscountAmount.RaiseValidationError();
                        IsValidate = false;
                    }
                }
                else
                {
                    txtStaffDiscountAmount.ClearValidationError();
                }
                if (txtStaffParentPercentage.Text != "")
                {
                    if (Extensions.IsItDecimal(txtStaffParentPercentage.Text) == true)
                    {
                        txtStaffParentPercentage.SetValidation(" Staff Dependant Discount Percentage should be number");
                        txtStaffParentPercentage.RaiseValidationError();
                        IsValidate = false;
                    }
                }
                else
                {
                    txtStaffParentPercentage.ClearValidationError();
                }
                if (txtStaffParentAmount.Text != "")
                {
                    if (Extensions.IsItDecimal(txtStaffParentAmount.Text) == true)
                    {
                        txtStaffParentAmount.SetValidation(" Staff Dependant Discount Amount should be number");
                        txtStaffParentAmount.RaiseValidationError();
                        IsValidate = false;
                    }
                }
                else
                {
                    txtStaffParentAmount.ClearValidationError();
                }

                if (txtConcessionPercentage.Text != "")
                {
                    if (Extensions.IsItDecimal(txtConcessionPercentage.Text) == true)
                    {
                        txtConcessionPercentage.SetValidation(" Concession Percentage should be number");
                        txtConcessionPercentage.RaiseValidationError();
                        IsValidate = false;
                    }
                }
                else
                {
                    txtConcessionPercentage.ClearValidationError();
                }
                if (txtConcessionAmount.Text != "")
                {
                    if (Extensions.IsItDecimal(txtConcessionAmount.Text) == true)
                    {
                        txtConcessionAmount.SetValidation(" Concession Amount should be number");
                        txtConcessionAmount.RaiseValidationError();
                        IsValidate = false;
                    }
                }
                else
                {
                    txtConcessionAmount.ClearValidationError();
                }

                if (txtDoctorApplicablePercent.Text != "")
                {
                    if (Extensions.IsItDecimal(txtDoctorApplicablePercent.Text) == true)
                    {

                        txtDoctorApplicablePercent.SetValidation(" Doctor Share Percentage should be number");
                        txtDoctorApplicablePercent.RaiseValidationError();
                        IsValidate = false;
                    }
                }
                else
                {
                    txtDoctorApplicablePercent.ClearValidationError();
                }
                if (txtDoctorApplicableAmount.Text != "")
                {
                    if (Extensions.IsItDecimal(txtDoctorApplicableAmount.Text) == true)
                    {

                        txtDoctorApplicableAmount.SetValidation(" Doctor Share Amount should be number");
                        txtDoctorApplicableAmount.RaiseValidationError();
                        IsValidate = false;
                    }
                }
                else
                {
                    txtDoctorApplicableAmount.ClearValidationError();
                }

                if (chkRateEditable.IsChecked == true)
                {
                    try
                    {
                        if (txtMinRate.Text != "")
                        {
                            if (Extensions.IsItDecimal(txtMinRate.Text) == true)
                            {
                                txtMinRate.SetValidation(" Min Rate Amount should be number Or Not Negative Number.");
                                txtMinRate.RaiseValidationError();
                                IsValidate = false;
                            }
                            else if (txtServiceRate.Text != "")
                            {
                                if (Convert.ToDouble(txtServiceRate.Text) < Convert.ToDouble(txtMinRate.Text))
                                {
                                    txtMinRate.SetValidation(" Min Rate should be less than or equal to Base Rate");
                                    txtMinRate.RaiseValidationError();
                                    IsValidate = false;
                                }
                            }
                            else if (txtMaxRate.Text != "")
                            {
                                if (Convert.ToDouble(txtMaxRate.Text) < Convert.ToDouble(txtMinRate.Text))
                                {
                                    txtMinRate.SetValidation(" Min Rate should be less than or equal to Max Rate");
                                    txtMinRate.RaiseValidationError();
                                    IsValidate = false;
                                }
                            }
                            else
                                txtMinRate.ClearValidationError();
                        }
                        else
                        {
                            txtMinRate.ClearValidationError();
                        }
                        if (txtMaxRate.Text != "")
                        {
                            if (Extensions.IsItDecimal(txtMaxRate.Text) == true)
                            {
                                txtMaxRate.SetValidation(" Max Rate Amount should be number Or Not Negative Number.");
                                txtMaxRate.RaiseValidationError();
                                IsValidate = false;
                            }
                            //else if (txtServiceRate.Text != "")
                            //{
                            //    if (Convert.ToDouble(txtServiceRate.Text) < Convert.ToDouble(txtMaxRate.Text))
                            //    {
                            //        txtMaxRate.SetValidation(" Max Rate should be less than or equal to Base Rate");
                            //        txtMaxRate.RaiseValidationError();
                            //        IsValidate = false;
                            //    }
                            //}
                            else if (txtMinRate.Text != "")
                            {
                                if (Convert.ToDouble(txtMaxRate.Text) < Convert.ToDouble(txtMinRate.Text))
                                {
                                    txtMaxRate.SetValidation(" Max Rate should be greater than or equal to Max Rate");
                                    txtMaxRate.RaiseValidationError();
                                    IsValidate = false;
                                }
                            }
                            else
                                txtMaxRate.ClearValidationError();
                        }
                        else
                        {
                            txtMaxRate.ClearValidationError();
                        }
                        //if (txtMinRate.Text != "" && txtMaxRate.Text != "" )
                        //{
                        //    if (Convert.ToDouble(txtMaxRate.Text) < Convert.ToDouble(txtServiceRate.Text) || Convert.ToDouble(txtServiceRate.Text) < Convert.ToDouble(txtMinRate.Text))
                        //    {
                        //        txtMaxRate.SetValidation("Please Enter Max. Rate Less Than Or Equal to Base Service Rate.");
                        //        txtMaxRate.RaiseValidationError();
                        //        txtMaxRate.Focus();
                        //        IsValidate = false;
                        //    }
                        //    else
                        //        txtMaxRate.ClearValidationError();
                        //}
                    }
                    catch (Exception Ex)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Min. Rate or max Rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW.Show();
                        txtMinRate.Text = "0.00";
                        txtMaxRate.Text = "0.00";
                        IsValidate = false;
                    }
                }
                if (chkServiceTax.IsChecked == true)
                {
                    if ((txtServiceTaxPercentage.Text.Equals("")) || (txtServiceTaxPercentage.Text == "0"))
                    {
                        if (txtServiceTaxAmount.Text.Equals("") || (txtServiceTaxAmount.Text == "0"))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Service Tax Percentage or Service Tax Amount Need To Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindowUpdate.Show();
                            IsValidate = false;
                        }
                    }
                }
                if (chkStaffDiscount.IsChecked == true)
                {
                    if (txtStaffDiscountPercentage.Text.Equals("") || (txtStaffDiscountPercentage.Text == "0"))
                    {
                        if (txtStaffDiscountAmount.Text.Equals("") || (txtStaffDiscountAmount.Text == "0"))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Staff Discount Percentage or Staff Discount Amount Need To Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindowUpdate.Show();
                            IsValidate = false;
                        }
                    }
                }

                if (chkStaffParentDiscount.IsChecked == true)
                {
                    if (txtStaffParentPercentage.Text.Equals("") || (txtStaffParentPercentage.Text == "0"))
                    {
                        if (txtStaffParentAmount.Text.Equals("") || (txtStaffParentAmount.Text == "0"))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Either Staff Parent Percentage or Staff Parent Amount Need To Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindowUpdate.Show();
                            IsValidate = false;
                        }
                    }
                }

                if (chkConcession.IsChecked == true)
                {
                    if (txtConcessionPercentage.Text.Equals("") || (txtConcessionPercentage.Text == "0"))
                    {
                        if (txtConcessionAmount.Text.Equals("") || (txtConcessionAmount.Text == "0"))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Either Concession Percentage or Concession Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindowUpdate.Show();
                            IsValidate = false;
                        }
                    }
                }
                if (chkSeniorCitizen.IsChecked == true)
                {

                    if (txtSeniorCitizenAge.Text.Equals("") || (txtSeniorCitizenAge.Text.Trim() == "0"))
                    {
                        txtSeniorCitizenAge.SetValidation("Senior Citizen Age is required");
                        txtSeniorCitizenAge.RaiseValidationError();
                        txtSeniorCitizenAge.Focus();
                        IsValidate = false;
                    }

                    if (txtSeniorCitizenPer.Text.Equals("") || (txtSeniorCitizenPer.Text.Trim() == "0"))
                    {
                        if (txtSeniorCitizenPerAmount.Text.Equals("") || (txtSeniorCitizenPerAmount.Text.Trim() == "0"))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Either Concession Percentage or Concession Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindowUpdate.Show();
                            IsValidate = false;
                        }
                    }
                    else if (txtSeniorCitizenPer.Text.Equals("") || (txtSeniorCitizenPer.Text.Trim() == "0.00"))
                    {
                        if (txtSeniorCitizenPerAmount.Text.Equals("") || (txtSeniorCitizenPerAmount.Text.Trim() == "0.00"))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Either Concession Percentage or Concession Amount Can Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindowUpdate.Show();
                            IsValidate = false;
                        }
                    }
                }
                if (chkApplicableToAllDoctors.IsChecked == true)
                {
                    if (txtDoctorApplicablePercent.Text.Equals("") || (txtDoctorApplicablePercent.Text == "0"))
                    {
                        if (txtDoctorApplicableAmount.Text.Equals("") || (txtDoctorApplicableAmount.Text == "0"))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Either Doctor Share Percentage or Doctor Share Amount Need To Be Added!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWindowUpdate.Show();
                            IsValidate = false;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindowUpdate.Show();
                IsCancel = false;
            }
        }
        #endregion

        private void txtServiceCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtServiceCode.Text) && e.Key == Key.Enter)
            {
                FillServiceGrid();
                dgDataPager.PageIndex = 0;
            }
            //if (!String.IsNullOrEmpty(txtServiceCode.Text))
            //{
            //    FillServiceGrid();
            //}
        }

        private void txtServiceName_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!String.IsNullOrEmpty(txtServiceName.Text))
            //{
            //    FillServiceGrid();
            //}
        }

        private void cmbSubSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (((MasterListItem)cmbSpecialization.SelectedItem).ID > 0)
            //{
            //    FillServiceGrid();
            //}
        }

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsValidate = true;
        }

        private void txtServiceName1_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsValidate = true;
        }

        private void txtServiceShortDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsValidate = true;
        }

        private void txtServiceLongDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsValidate = true;
        }

        private void txtServiceRate_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            {

                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;

            }
            IsValidate = true;

        }
        decimal PrevServiceRate = 0;
        private void txtServiceRate_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtServiceRate.Text != "")
                {
                    //  if (Extensions.IsPositiveNumber(txtServiceRate.Text) == false)
                    //  {
                    //      txtServiceRate.SetValidation("Rate should be positive number");
                    //      MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //          new MessageBoxControl.MessageBoxChildWindow("", "Rate should be positive number", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //      msgW1.Show();
                    //      txtServiceRate.Text = PrevServiceRate.ToString();
                    //      return;
                    //  }
                    //else  if (Extensions.IsItDecimal(txtServiceRate.Text) == true)
                    //  {
                    //      txtServiceRate.SetValidation("Rate should be Numeric");
                    //      MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //          new MessageBoxControl.MessageBoxChildWindow("", "Rate should be Numeric", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //      msgW1.Show();
                    //      txtServiceRate.Text = PrevServiceRate.ToString();
                    //      return;
                    //  }

                    //else 
                    if (chkRateEditable.IsChecked == true)
                    {
                        try
                        {
                            if (Convert.ToDecimal(txtServiceRate.Text) > Convert.ToDecimal(txtMaxRate.Text) || Convert.ToDecimal(txtServiceRate.Text) < Convert.ToDecimal(txtMinRate.Text))
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Error occurred while adding service rate", "Service rate must be in between min. rate & max. rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                txtServiceRate.Text = PrevServiceRate.ToString();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                            new MessageBoxControl.MessageBoxChildWindow("", "Incorrect min.rate or max. rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            txtMaxRate.Text = "0.00";
                            txtMinRate.Text = "0.00";
                            txtServiceRate.Text = "0.00";
                            return;
                        }
                    }

                    decimal ServiceRate = 0;
                    ServiceRate = Convert.ToDecimal(txtServiceRate.Text);
                    if (chkServiceTax.IsChecked == true)
                    {
                        decimal ServiceTaxPer = Convert.ToDecimal(txtServiceTaxPercentage.Text);
                        decimal ServiceTaxAmount = 0;
                        ServiceTaxAmount = ((ServiceRate * ServiceTaxPer) / 100);
                        txtServiceTaxAmount.Text = ServiceTaxAmount.ToString("0.00");
                    }
                    if (chkStaffDiscount.IsChecked == true)
                    {
                        decimal Percent = 0;
                        Percent = Convert.ToDecimal(txtStaffDiscountPercentage.Text);
                        decimal TaxAmount = 0;
                        TaxAmount = ((ServiceRate * Percent) / 100);
                        txtStaffDiscountAmount.Text = TaxAmount.ToString("0.00");
                    }
                    if (chkStaffParentDiscount.IsChecked == true)
                    {
                        decimal Percent = 0;
                        Percent = Convert.ToDecimal(txtStaffParentPercentage.Text);
                        decimal TaxAmount = 0;
                        TaxAmount = ((ServiceRate * Percent) / 100);
                        txtStaffParentAmount.Text = TaxAmount.ToString("0.00");
                    }
                    if (chkConcession.IsChecked == true)
                    {
                        decimal Percent = 0;
                        Percent = Convert.ToDecimal(txtConcessionPercentage.Text);
                        decimal TaxAmount = 0;
                        TaxAmount = ((ServiceRate * Percent) / 100);
                        txtConcessionAmount.Text = TaxAmount.ToString("0.00");
                    }
                    if (chkApplicableToAllDoctors.IsChecked == true)
                    {
                        decimal Percent = 0;
                        Percent = Convert.ToDecimal(txtDoctorApplicablePercent.Text);
                        decimal TaxAmount = 0;
                        TaxAmount = ((ServiceRate * Percent) / 100);
                        txtDoctorApplicableAmount.Text = TaxAmount.ToString("0.00");
                    }
                    if (chkApplyToAllClass.IsChecked == true && !String.IsNullOrEmpty(txtServiceRate.Text))
                    {
                        foreach (var item in ClassList)
                        {
                            item.Rate = Convert.ToDecimal(txtServiceRate.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("", "Incorrect Data", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
                txtServiceRate.Text = "0.00";
                return;
            }
        }

        private void cboCodeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsValidate = true;
        }

        private void cmbSubSpecialization1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsValidate = true;
        }

        List<clsServiceMasterVO> ClassList = new List<clsServiceMasterVO>();
        List<clsServiceMasterVO> ServiceClassList = new List<clsServiceMasterVO>();
        private void FillClassGrid()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                ClassList = new List<clsServiceMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        foreach (var item in objList)
                        {
                            ClassList.Add(new clsServiceMasterVO()
                            {
                                ClassID = item.ID,
                                ClassName = item.Description,
                                UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                                Rate = Convert.ToDecimal("0.00"),
                                Status = item.Status
                            });
                        }
                        grdClass.ItemsSource = null;
                        grdClass.ItemsSource = ClassList;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void chkApplyToAllClass_Click(object sender, RoutedEventArgs e)
        {
            if (chkApplyToAllClass.IsChecked == true)
            {
                foreach (var item in ClassList)
                {
                    if (!String.IsNullOrEmpty(txtServiceRate.Text))
                        item.Rate = Convert.ToDecimal(txtServiceRate.Text);
                    item.IsClassRateReadonly = true;
                    item.IsSelected = true;
                    item.IsEnabled = false;
                }
                grdClass.ItemsSource = null;
                grdClass.ItemsSource = ClassList.DeepCopy();
                grdClass.UpdateLayout();
                grdClass.Focus();

            }
            else
            {

                if (ServiceClassList.Count > 0)
                {
                    foreach (var item in ServiceClassList)
                    {
                        if (item.Rate > 0)
                        {
                            item.IsSelected = true;
                            grdClass.ItemsSource = null;
                            grdClass.ItemsSource = ServiceClassList.DeepCopy();
                            grdClass.UpdateLayout();
                            grdClass.Focus();
                            break;
                        }
                        else
                        {
                            item.IsSelected = false;
                        }
                    }

                    grdClass.ItemsSource = null;
                    grdClass.ItemsSource = ServiceClassList.DeepCopy();
                    grdClass.UpdateLayout();
                    grdClass.Focus();
                }
                else
                {
                    foreach (var item in ClassList)
                    {
                        item.Rate = Convert.ToDecimal("0.00");
                        item.IsClassRateReadonly = false;
                        item.IsSelected = false;
                        item.IsEnabled = true;
                    }
                    grdClass.ItemsSource = null;
                    grdClass.ItemsSource = ClassList.DeepCopy();
                    grdClass.UpdateLayout();
                    grdClass.Focus();
                }
            }
        }
        FrameworkElement element;
        DataGridRow row;
        TextBox TxtServiceClassRate;
        private void chkSelectClass_Click(object sender, RoutedEventArgs e)
        {
            var i = from r in ServiceClassList
                    where r.IsSelected == false
                    select r;

            if (chkApplyToAllClass.IsChecked == true)
            {
                if (i != null && i.ToList().Count > 0)
                {
                    chkApplyToAllClass.IsChecked = false;
                }
            }
            element = grdClass.Columns.Last().GetCellContent(grdClass.SelectedItem);
            row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
            TxtServiceClassRate = FindVisualChild<TextBox>(row, "txtServiceClassRate");

            if (((CheckBox)sender).IsChecked == true)
            {
                TxtServiceClassRate.IsEnabled = true;
                //foreach (var item in ClassList)
                //{
                //    if (item.ClassID == ((clsServiceMasterVO)grdClass.SelectedItem).ClassID && item.IsSelected==true)
                //    {
                //        item.Rate = Convert.ToDecimal("0.00");
                //        item.IsSelected = true;
                //    }

                //}
            }
            else if (((CheckBox)sender).IsChecked == false)
            {
                TxtServiceClassRate.IsEnabled = false;
                TxtServiceClassRate.Text = "0.00";
            }
            //else
            //{
            //    TxtServiceClassRate.IsEnabled = false;
            //    //foreach (var i in grdClass.ItemsSource)
            //    //{
            //        foreach (var item in ServiceClassList)
            //        {
            //            if (item.ClassID == ((clsServiceMasterVO)grdClass.SelectedItem).ClassID)
            //            {
            //                item.Rate = Convert.ToDecimal("0.00");
            //                item.IsSelected = false;
            //                break;
            //            }
            //            //else
            //            //{
            //            //    item.IsSelected = true;
            //            //}
            //        }
            //    //}
            //    grdClass.ItemsSource = null;
            //    grdClass.ItemsSource = ServiceClassList;
            //    grdClass.UpdateLayout();
            //    grdClass.Focus();
            //}
        }

        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj, String TextBoxName)
          where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    if (Child is TextBox)
                    {
                        if (((TextBox)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else if (Child is DataGrid)
                    {
                        if (((DataGrid)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else
                    {
                        ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                        if (ChildOfChild != null)
                        {
                            return ChildOfChild;
                        }
                    }
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }

        private void txtServiceClassRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            {

                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
            else if ((((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces()))
            {
                string TextValue = ((TextBox)sender).Text;
                foreach (var item in ClassList)
                {
                    if (item.ClassID == ((clsServiceMasterVO)grdClass.SelectedItem).ClassID)
                    {
                        if (TextValue != string.Empty)
                            item.Rate = Convert.ToDecimal(TextValue);
                        else
                            item.Rate = 0;
                    }
                }
            }
        }
        private void txtServiceClassRate_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtServiceCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItNumber() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
            //else if (((TextBox)sender).Text != string.Empty )
            //{
            //    if (Convert.ToInt32(txtSeniorCitizenAge.Text.Trim()) > 120)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW =
            //                          new MessageBoxControl.MessageBoxChildWindow("", "Age cannot be greater then 120 years", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgW.Show();
            //        txtSeniorCitizenAge.Text = "0";
            //    }
            //}
        }

        private void txtServiceCode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
            if (e.Key == Key.Enter)
            {
                FillServiceGrid();
                dgDataPager.PageIndex = 0;
            }
        }

        private void txtSeniorCitizenAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSeniorCitizenAge.Text != null && Convert.ToInt32(txtSeniorCitizenAge.Text.Trim()) > 0)
            {
                if (Convert.ToInt32(txtSeniorCitizenAge.Text.Trim()) < 60 || Convert.ToInt32(txtSeniorCitizenAge.Text.Trim()) > 120)
                {
                    txtSeniorCitizenAge.SetValidation("Please enter age Between 60 to 120 Years");
                    txtSeniorCitizenAge.RaiseValidationError();
                    txtSeniorCitizenAge.Focus();
                    //MessageBoxControl.MessageBoxChildWindow msgW =
                    //                  new MessageBoxControl.MessageBoxChildWindow("", "Please enter age Between 60 to 120 Years", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgW.Show();
                    txtSeniorCitizenAge.Text = "0";
                }
                else
                    txtSeniorCitizenAge.ClearValidationError();
            }
        }

        private void txtServiceName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillServiceGrid();
                dgDataPager.PageIndex = 0;
            }
        }

        private void txtServiceTaxPercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPositiveDoubleValid() && textBefore != null)
            {

                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtServiceTaxPercentage_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtMaxRate_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (chkRateEditable.IsChecked == true && (txtServiceRate.Text != null && txtServiceRate.Text != "0" && txtServiceRate.Text != ""))
            //{
            //    if (txtMaxRate.Text != null && txtMaxRate.Text != "0")
            //        if (Convert.ToDouble(txtMaxRate.Text) > Convert.ToDouble(txtServiceRate.Text))
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW =
            //                              new MessageBoxControl.MessageBoxChildWindow("", "Maximum Rate Should Be Less Than Base Rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW.Show();
            //            txtMaxRate.Text = txtServiceRate.Text;
            //        }

            //}
        }

        private void txtServiceRate_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtLuxuryTaxAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPositiveDoubleValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtLuxuryTaxAmount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtLuxuryTaxPercentage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPositiveDoubleValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtLuxuryTaxPercentage_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtSeniorCitizenAge_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtSeniorCitizenAge_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtServiceTaxAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            {

                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtServiceTaxAmount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmdApplyLevels_Click(object sender, RoutedEventArgs e)
        {
            if (grdServices.SelectedItem != null)
            {
                FrmApplyLevel win = new FrmApplyLevel();
                win.Title = "Service : " + ((clsServiceMasterVO)grdServices.SelectedItem).ServiceName + ")";
                win.ServiceID = ((clsServiceMasterVO)grdServices.SelectedItem).ID;
                win.ServiceUnitID = ((clsServiceMasterVO)grdServices.SelectedItem).UnitID;
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Please Select Service ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void cmdAssignConsent_Click(object sender, RoutedEventArgs e)
        {
            if (grdServices.SelectedItem != null)
            {
                clsServiceMasterVO objServiceVO = new clsServiceMasterVO();
                objServiceVO = (clsServiceMasterVO)grdServices.SelectedItem;

                AssignPackageConsent Win = new AssignPackageConsent();
                Win.ServiceID = ((clsServiceMasterVO)grdServices.SelectedItem).ID;
                Win.Show();
                //Win.GetSelectedServiceDetails(objServiceVO);

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select the Service to link with Consent.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }

        private void cmdApplyTax_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsServiceMasterVO objServiceMasterVO = new clsServiceMasterVO();
                objServiceMasterVO = (clsServiceMasterVO)grdServices.SelectedItem;
                if (objServiceMasterVO != null)
                {
                    if (objServiceMasterVO.Status == true)
                    {
                        frmServiceTaxLinking win = new frmServiceTaxLinking();
                        win.objServiceMasterVO.ServiceID = objServiceMasterVO.ID;
                        win.objServiceMasterVO.ServiceName = objServiceMasterVO.ServiceName;
                        win.objServiceMasterVO.ClassID = objServiceMasterVO.ClassID;
                        win.objServiceMasterVO.ClassName = objServiceMasterVO.ClassName;
                        win.objServiceMasterVO.TariffID = objServiceMasterVO.TariffID;
                        win.objServiceMasterVO.TariffName = objServiceMasterVO.TariffName;
                        win.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("", "Service is Deactivated!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgW.Show();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
