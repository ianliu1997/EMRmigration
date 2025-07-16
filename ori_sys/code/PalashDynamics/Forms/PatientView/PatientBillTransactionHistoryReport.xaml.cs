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
using CIMS;
using System.Windows.Browser;

namespace PalashDynamics.Forms.PatientView
{
    public partial class PatientBillTransactionHistoryReport : ChildWindow
    {
        public string MrNo = string.Empty;

        public PatientBillTransactionHistoryReport()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            string BillType = string.Empty;
            string PaymentBilltype = string.Empty;
            if (cmbBillType.SelectedItem != null && (cmbBillType.SelectedItem as MasterListItem).ID > 0)
            {
                BillType = (cmbBillType.SelectedItem as MasterListItem).Description;
            }

            if (cmbPaymentBilltype.SelectedItem != null && (cmbPaymentBilltype.SelectedItem as MasterListItem).ID > 0)
            {
                PaymentBilltype = (cmbPaymentBilltype.SelectedItem as MasterListItem).Description;
            }

            string URL = "../Reports/Patient/PatientBillTransactionHistoryReport.aspx?MrNo=" + MrNo
                + "&TransactionDate=" + (dtpTransactionDate.SelectedDate == null ? null : dtpTransactionDate.SelectedDate)
                + "&BillType=" + BillType
                + "&PaymentBilltype=" + PaymentBilltype
                + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region ComboBox Fill
            List<MasterListItem> BilltypeList = new List<MasterListItem>();
            BilltypeList.Add(new MasterListItem(0, "--Select--"));
            BilltypeList.Add(new MasterListItem(1, "Clinical"));
            BilltypeList.Add(new MasterListItem(2, "Pharmacy"));
            BilltypeList.Add(new MasterListItem(3, "Refund"));
            BilltypeList.Add(new MasterListItem(4, "Advance"));

            List<MasterListItem> PaymentBillTypeList = new List<MasterListItem>();
            PaymentBillTypeList.Add(new MasterListItem(0, "--Select--"));
            PaymentBillTypeList.Add(new MasterListItem(1, "Advance Received"));
            PaymentBillTypeList.Add(new MasterListItem(2, "Bill Received"));
            PaymentBillTypeList.Add(new MasterListItem(3, "Refund Against Advance"));
            PaymentBillTypeList.Add(new MasterListItem(4, "Refund Against Bill"));
            PaymentBillTypeList.Add(new MasterListItem(5, "Refund Sale Return"));

            cmbBillType.ItemsSource = BilltypeList;
            cmbBillType.SelectedItem = BilltypeList[0];

            cmbPaymentBilltype.ItemsSource = PaymentBillTypeList;
            cmbPaymentBilltype.SelectedItem = PaymentBillTypeList[0];
            #endregion
        }
    }
}

