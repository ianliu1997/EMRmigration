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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;


namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmLutualPhaseNew : ChildWindow
    {
        public frmLutualPhaseNew()
        {
            InitializeComponent();
        }

        #region Variables
        public event RoutedEventHandler OnSaveButton_Click;
        #endregion

        #region Properites
        public Boolean IsEdit { get; set; }
        public long DrugId { get; set; }
        public long TherpayExeId { get; set; }
        public DateTime? TherapyStartDate { get; set; }
        #endregion

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fillDrugComboBox();           
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (CmbDrug.SelectedItem == null)
            {
                if (((MasterListItem)CmbDrug.SelectedItem).ID == 0)
                {
                    CmbDrug.TextBox.SetValidation("Please Select Drug");
                    CmbDrug.TextBox.RaiseValidationError();
                    CmbDrug.Focus();
                }
            }
            if (((MasterListItem)CmbDrug.SelectedItem).ID == 0)
            {
                CmbDrug.TextBox.SetValidation("Please Select Drug");
                CmbDrug.TextBox.RaiseValidationError();
                CmbDrug.Focus();
            }
            else if (dtpdrugstartdate.SelectedDate == null)
            {
                dtpdrugstartdate.SetValidation("Please Enter Start Date");
                dtpdrugstartdate.RaiseValidationError();
                dtpdrugstartdate.Focus();
            }
            else if (string.IsNullOrEmpty(txtForDays.Text) || txtForDays.Text.Equals("0"))
            {
                CmbDrug.ClearValidationError();
                dtpdrugstartdate.ClearValidationError();
                txtForDays.SetValidation("For Days Cannot Be Empty or 0");
                txtForDays.RaiseValidationError();
                txtForDays.Focus();
            }
            else if (string.IsNullOrEmpty(txtDosage.Text) || txtDosage.Text.Equals("0"))
            {
                CmbDrug.ClearValidationError();
                dtpdrugstartdate.ClearValidationError();
                txtForDays.ClearValidationError();
                txtDosage.SetValidation("Dosage Cannot Be Empty or 0");
                txtDosage.RaiseValidationError();
                txtDosage.Focus();
            }
            else if (txtTime.Value == null)
            {
                CmbDrug.ClearValidationError();
                dtpdrugstartdate.ClearValidationError();
                txtForDays.ClearValidationError();
                txtDosage.ClearValidationError();
                txtTime.SetValidation("Please Enter Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
            }
            else
            {
                CmbDrug.ClearValidationError();
                dtpdrugstartdate.ClearValidationError();
                txtForDays.ClearValidationError();
                txtDosage.ClearValidationError();
                txtTime.ClearValidationError();
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
        }

        #region ComboBox Fill Methods
        public void fillDrugComboBox()
        {
            clsGetItemListBizActionVO BizAction = new clsGetItemListBizActionVO();
            BizAction.ItemDetails = new clsItemMasterVO();
            BizAction.ItemDetails.RetrieveDataFlag = false;
            BizAction.FilterCriteria = 1;
            BizAction.FilterIGroupID = 1;
            BizAction.ForReportFilter = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {
                    MasterListItem item = new MasterListItem();
                    item.ID = 0;
                    item.Description = "--Select--";
                    ((clsGetItemListBizActionVO)args.Result).MasterList.Insert(0, item);
                    CmbDrug.ItemsSource = ((clsGetItemListBizActionVO)args.Result).MasterList;
                    if (DrugId == 0)
                        CmbDrug.SelectedValue = (long)0;
                    else
                        CmbDrug.SelectedValue = (long)DrugId;
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
           
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdSaveGeneral_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdGenerateCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

       
    }
}

