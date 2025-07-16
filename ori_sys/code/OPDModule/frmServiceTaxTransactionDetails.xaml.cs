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
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects;
using System.Reflection;

namespace OPDModule
{
    public partial class frmServiceTaxTransactionDetails : ChildWindow
    {
        public clsChargeVO objChargeVO = new clsChargeVO();

        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        public bool IsTaxReadOnly = true;
        public bool IsBillFreezed = false;

        public frmServiceTaxTransactionDetails()
        {
            InitializeComponent();
            FillTaxType();
            dgServiceTaxList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgServiceTaxList_CellEditEnded);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsTaxReadOnly || IsBillFreezed)        //check for Tax Isreadonly or not?
                dgServiceTaxList.IsReadOnly = true;

            if (objChargeVO.ServiceName != null)
                lblServiceName.Text = ": " + objChargeVO.ServiceName.ToString();
            FetchData();
        }

        void dgServiceTaxList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("Tax Amt."))
            {
                if (dgServiceTaxList.SelectedItem != null)
                {
                    if (((clsChargeTaxDetailsVO)dgServiceTaxList.SelectedItem).ServiceTaxAmount > ((clsChargeTaxDetailsVO)dgServiceTaxList.SelectedItem).TotalAmount)
                    {
                        ((clsChargeTaxDetailsVO)dgServiceTaxList.SelectedItem).ServiceTaxAmount = ((clsChargeTaxDetailsVO)dgServiceTaxList.SelectedItem).TotalAmount;
                        string msgText = "Tax Amount Should Not Be Greater Than Total Amount " + ((clsChargeTaxDetailsVO)dgServiceTaxList.SelectedItem).TotalAmount;
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                    }
                }
            }
            CalculateTotalTaxAmount();
            if (objChargeVO != null)
                objChargeVO.IsEditTax = true;

        }

        List<MasterListItem> TaxTypeList = new List<MasterListItem>();
        private void FillTaxType()
        {
            MasterListItem Default = new MasterListItem(0, "-- Select --");
            TaxTypeList.Insert(0, Default);
            EnumToList(typeof(TaxType), TaxTypeList);
        }

        private static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                if (Value > 0)
                {
                    string Display = Enum.GetName(EnumType, Value);
                    MasterListItem Item = new MasterListItem(Value, Display);
                    TheMasterList.Add(Item);
                }
            }
        }

        private static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }

        private void FetchData()
        {
            if (objChargeVO.ChargeTaxDetailsList != null)
            {
                foreach (var item in objChargeVO.ChargeTaxDetailsList.ToList())
                {
                    item.TaxTypeName = TaxTypeList.SingleOrDefault(z => z.ID == item.TaxType).Description.ToString();
                    item.Quantity = objChargeVO.Quantity;
                    item.TotalAmount = objChargeVO.TotalAmount;
                    item.Concession = objChargeVO.Concession;
                    if (objChargeVO.IsEditTax == false)
                        item.ServiceTaxPercent = Convert.ToDouble(item.Percentage);
                }
                dgServiceTaxList.ItemsSource = null;
                dgServiceTaxList.ItemsSource = objChargeVO.ChargeTaxDetailsList;
                CalculateTotalTaxAmount();
            }
        }

        private void CalculateTotalTaxAmount()
        {
            double TotalTaxAmount;

            TotalTaxAmount = 0;

            for (int i = 0; i < objChargeVO.ChargeTaxDetailsList.Count; i++)
            {
                TotalTaxAmount += (objChargeVO.ChargeTaxDetailsList[i].ServiceTaxAmount);
            }
            objChargeVO.TotalServiceTaxAmount = TotalTaxAmount;
            txtTotalTaxAmt.Text = String.Format("{0:0.00}", TotalTaxAmount);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (objChargeVO != null)
                objChargeVO = new clsChargeVO();

            if (objChargeVO.ChargeTaxDetailsList != null)
                objChargeVO.ChargeTaxDetailsList = new List<clsChargeTaxDetailsVO>();

            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);

            //if (OnCancelButton_Click != null)
            //{
            //    this.DialogResult = false;
            //    OnCancelButton_Click(this, new RoutedEventArgs());
            //    this.Close();
            //}

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButton_Click != null)
            {
                this.DialogResult = true;
                OnCancelButton_Click(this, new RoutedEventArgs());
                this.Close();
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            }
            
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}

