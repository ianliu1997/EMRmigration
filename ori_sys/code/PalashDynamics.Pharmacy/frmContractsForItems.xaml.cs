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
using PalashDynamics.ValueObjects.RateContract;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Controls;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.Pharmacy
{
    public partial class frmContractsForItems : ChildWindow
    {
        public frmContractsForItems()
        {
            InitializeComponent();
            lstItemWiseRateContract = new List<ItemWiseRateContracts>();
        }
        public List<clsRateContractDetailsVO> lstRateContractItemDetailsNew { get; set; }

        public List<clsRateContractItemDetailsVO> lstRateContractItemDetails { get; set; }
        public List<clsRateContractMasterVO> lstRateContract { get; set; }
        public event RoutedEventHandler OnOKButton_Click;
        public clsPurchaseOrderDetailVO POItem { get; set; }
        public MasterListItem SupplierID { get; set; }
        public ItemWiseRateContracts SelectedItem { get; set; }
        public clsRateContractDetailsVO SelectedItemNew { get; set; }
        List<ItemWiseRateContracts> lstItemWiseRateContract { get; set; }
        public bool blnIsValid = false;
        private decimal dblOrigionalDiscount { get; set; }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblitemName.Text = "Item Name: " + Convert.ToString(POItem.ItemName);
                dgRateContracts.ItemsSource = null;
                dgRateContracts.ItemsSource = lstRateContractItemDetailsNew;


                //for (int iCount = 0; iCount < lstRateContractItemDetails.Count; iCount++)
                //{

                //    lstItemWiseRateContract.Add(new ItemWiseRateContracts
                //    {
                //        ItemName = POItem.ItemName,
                //        ContractName = lstRateContract.Where(z => z.RateContractID == lstRateContractItemDetails[iCount].RateContractID && z.RateContractUnitID == lstRateContractItemDetails[iCount].RateContractUnitID).First().Description,
                //        DiscountPercent = lstRateContractItemDetails[iCount].DiscountPercent,
                //        ContractQuantity = lstRateContractItemDetails[iCount].Quantity,
                //        Condition = lstRateContractItemDetails[iCount].Condition,
                //        ContractDate = lstRateContract[iCount].ContractDate,
                //        ContractValue = lstRateContract[iCount].ContractValue,
                //        MinQuantity = lstRateContractItemDetails[iCount].MinQuantity,
                //        MaxQuantity = lstRateContractItemDetails[iCount].MaxQuantity,
                //        RateContractID=lstRateContractItemDetails[iCount].RateContractID,
                //        RateContractUnitID = lstRateContractItemDetails[iCount].RateContractUnitID,
                //        Rate = lstRateContractItemDetails[iCount].Rate,
                //        MRP = lstRateContractItemDetails[iCount].MRP
                //    });
                //}
                //dgRateContracts.ItemsSource = null;
                //txtIndentQty.Text = Convert.ToString(POItem.Quantity);
                //dgRateContracts.ItemsSource = lstItemWiseRateContract;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        private void cmdCancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (blnIsValid)
            {
                POItem.DiscountPercent = dblOrigionalDiscount;
            }
            this.DialogResult = true;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            //if (IsValid())
            //{
            if (dgRateContracts.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Contract.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
            else
            {
                this.DialogResult = true;
                if (OnOKButton_Click != null)
                    OnOKButton_Click(this, new RoutedEventArgs());
            }


            //}
        }
        private void dgRateContracts_SelectionChanged(object sender, RoutedEventArgs e)
        {
            dblOrigionalDiscount = POItem.DiscountPercent;
            SelectedItemNew = (clsRateContractDetailsVO)dgRateContracts.SelectedItem;
            //SelectedItem = (ItemWiseRateContracts)dgRateContracts.SelectedItem;
            //double lngActualQty = Convert.ToDouble(txtIndentQty.Text);

            //if (SelectedItem != null)
            //{
            //    switch (SelectedItem.Condition)
            //    {
            //        case "Between":
            //            if ((lngActualQty >= SelectedItem.MinQuantity && lngActualQty <= SelectedItem.MaxQuantity))
            //            {
            //                POItem.DiscountPercent = Convert.ToDecimal(SelectedItem.DiscountPercent);
            //                blnIsValid = true;
            //            }
            //            else
            //            {
            //                blnIsValid = false;
            //            }
            //            break;
            //        case ">":
            //            if (Convert.ToDouble(POItem.Quantity) > SelectedItem.ContractQuantity)
            //            {
            //                POItem.DiscountPercent = Convert.ToDecimal(SelectedItem.DiscountPercent);
            //                blnIsValid = true;
            //            }
            //            else
            //            {
            //                blnIsValid = false;
            //            }
            //            break;
            //        case "<":
            //            if (Convert.ToDouble(POItem.Quantity) < SelectedItem.ContractQuantity)
            //            {
            //                POItem.DiscountPercent = Convert.ToDecimal(SelectedItem.DiscountPercent);
            //                blnIsValid = true;
            //            }
            //            else
            //            {
            //                blnIsValid = false;
            //            }
            //            break;
            //        case "No Limit":
            //            POItem.DiscountPercent = Convert.ToDecimal(SelectedItem.DiscountPercent);
            //            blnIsValid = true;
            //            break;
            //        case "=":
            //            if (Convert.ToDouble(POItem.Quantity) == SelectedItem.ContractQuantity)
            //            {
            //                POItem.DiscountPercent = Convert.ToDecimal(SelectedItem.DiscountPercent);
            //                blnIsValid = true;
            //            }
            //            else
            //            {
            //                blnIsValid = false;
            //            }
            //            break;
            //    }
            //}
        }
        private Boolean IsValid()
        {
            if (dgRateContracts.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Contract.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
            if (!blnIsValid)
            {
                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Palash", "Codition does not satisfied", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWin.Show();
                return false;
            }
            else
            {
                return true;
            }
        }
        //rohini for supplier 
        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;


                        //cmbSupplier.SelectedItem = objList[0];


                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public partial class ItemWiseRateContracts
    {
        private long _RateContractID;
        public long RateContractID { get; set; }

        private long _RateContractUnitID;
        public long RateContractUnitID { get; set; }


        private string _ContractName;
        public string ContractName { get; set; }
        private double _DiscountPercent;
        public double DiscountPercent
        {
            get;
            set;
        }

        private string _ItemName;
        public string ItemName { get; set; }

        private long _ItemID;
        public long ItemID { get; set; }
        private double _ContractQuantity;
        public double ContractQuantity { get; set; }
        private string _Condition;
        public string Condition { get; set; }
        private double _ContractValue;
        public double ContractValue { get; set; }
        private double _MinQuantity;
        public double MinQuantity { get; set; }
        private double _MaxQauntity;
        public double MaxQuantity { get; set; }
        private DateTime _ContractDate;
        public DateTime ContractDate { get; set; }
        public double Rate { get; set; }
        public double MRP { get; set; }
    }
}

