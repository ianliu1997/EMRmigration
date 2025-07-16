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

namespace PalashDynamics.IVF.TherpyExecution
{
    public partial class AddDrug : ChildWindow
    {
        #region Variables
        public event RoutedEventHandler OnSaveButton_Click;
        #endregion

        #region Properites
        public Boolean IsEdit { get; set; }
        public long DrugId { get; set; }
        public long TherpayExeId { get; set; }
        public DateTime? TherapyStartDate { get; set; }
        public bool IsSurrogateDrug { get; set; }
        public bool IsSurrogateCalendar { get; set; }
        #endregion

        #region Constructor
        public AddDrug()
        {
            InitializeComponent();
            fillDrugComboBox();
         
            this.Loaded += new RoutedEventHandler(AddDrug_Loaded);
        }

        void AddDrug_Loaded(object sender, RoutedEventArgs e)
        {
            if (TherapyStartDate != null)
            {
                dtthreapystartdate.DisplayDateStart = TherapyStartDate.Value;
                dtthreapystartdate.DisplayDateEnd = TherapyStartDate.Value.AddDays(59);
            }
            if (IsEdit == true)
            {
                CmbDrug.IsEnabled = false;
            }
            else
            {
                CmbDrug.IsEnabled = true;
            }

        }
        #endregion

        #region ComboBox Fill Methods
        public void fillDrugComboBox()
        {
        
            clsGetItemListBizActionVO BizAction = new clsGetItemListBizActionVO();
            BizAction.ItemDetails = new clsItemMasterVO();
            BizAction.ItemDetails.RetrieveDataFlag = false;
            

            //BizAction.FilterClinicId =0;
            BizAction.FilterCriteria = 1;
            //BizAction.FilterICatId=0;
            //    BizAction.FilterIDispensingId=0;
                BizAction.FilterIGroupID=1;
               // BizAction.FilterIMoleculeNameId=0;
               // BizAction.FilterITherClassId = 0;
           // BizAction.FilterStoreId=0;
            BizAction.ForReportFilter = true;

           
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {
                    MasterListItem item=new MasterListItem();
                    item.ID=0;
                    item.Description="--Select--";
                    ((clsGetItemListBizActionVO)args.Result).MasterList.Insert(0,item);
                    CmbDrug.ItemsSource = ((clsGetItemListBizActionVO)args.Result).MasterList;
                    if(DrugId==0)
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

        #region OK /Cancel Event
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (CmbDrug.SelectedItem == null )
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
            else if (dtthreapystartdate.SelectedDate == null)
            {
                dtthreapystartdate.SetValidation("Please Enter Start Date");
                dtthreapystartdate.RaiseValidationError();
                dtthreapystartdate.Focus();
            }
            else if (string.IsNullOrEmpty(txtForDays.Text) || txtForDays.Text.Equals("0"))
            {
                CmbDrug.ClearValidationError();
                dtthreapystartdate.ClearValidationError();
                txtForDays.SetValidation("For Days Cannot Be Empty or 0");
                txtForDays.RaiseValidationError();
                txtForDays.Focus();
            }
            else if (string.IsNullOrEmpty(txtDosage.Text) || txtDosage.Text.Equals("0"))
            {
                CmbDrug.ClearValidationError();
                dtthreapystartdate.ClearValidationError();
                txtForDays.ClearValidationError();
                txtDosage.SetValidation("Dosage Cannot Be Empty or 0");
                txtDosage.RaiseValidationError();
                txtDosage.Focus();
            }
            else if (txtTime.Value == null)
            {
                CmbDrug.ClearValidationError();
                dtthreapystartdate.ClearValidationError();
                txtForDays.ClearValidationError();
                txtDosage.ClearValidationError();
                txtTime.SetValidation("Please Enter Time");
                txtTime.RaiseValidationError();
                txtTime.Focus();
            }
            else
            {
                CmbDrug.ClearValidationError();
                dtthreapystartdate.ClearValidationError();
                txtForDays.ClearValidationError();
                txtDosage.ClearValidationError();
                txtTime.ClearValidationError();
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
           
            
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }


        #endregion
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtForDays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtForDays_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

    }
}

