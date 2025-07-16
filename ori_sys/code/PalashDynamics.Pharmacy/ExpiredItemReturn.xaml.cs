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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Patient;


namespace PalashDynamics.Pharmacy
{
    public partial class ExpiredItemReturn : UserControl
    {
        public ExpiredItemReturn()
        {
            InitializeComponent();
        }

        bool IsPageLoaded = false;
        private long _ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        public long ClinicID
        {
            get
            {
                return _ClinicID;
            }
            set
            {
                _ClinicID = value;
            }
        }

        public ObservableCollection<clsExpiredItemReturnDetailVO> ExpiryAddedItems { get; set; }

        public clsExpiredItemReturnVO ExpiryItemReturnMaster { get; set; }

        /// <summary>
        /// Get Expired Items button click
        /// </summary>
        private void txtGetItems_Click(object sender, RoutedEventArgs e)
        {
            if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID != 0 && cmbSupplier.SelectedItem != null && ((MasterListItem)cmbSupplier.SelectedItem).ID != 0)
            {
                ExpiryItemReturnMaster = new clsExpiredItemReturnVO();
                ExpiredItemSearch win = new ExpiredItemSearch();
                win.StoreID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                win.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Store and Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
            }
        }
       
        /// <summary>
        /// Expired Item Window Ok button click
        /// </summary>
      
        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ExpiredItemSearch Itemswin = (ExpiredItemSearch)sender;
            if (Itemswin.SelectedItems != null)
            {                
                foreach (var item in Itemswin.SelectedItems)
                {
                    IEnumerator<clsExpiredItemReturnDetailVO> list = (IEnumerator<clsExpiredItemReturnDetailVO>)ExpiryAddedItems.GetEnumerator();
                    bool IsExist = false;

                    while (list.MoveNext())
                    {
                        if (item.ItemID == ((clsExpiredItemReturnDetailVO)list.Current).ItemID)
                        {
                            if (item.BatchID == ((clsExpiredItemReturnDetailVO)list.Current).BatchID)
                            {
                                IsExist = true;
                                break;
                            }
                        }
                    }
                    if (IsExist == false)
                        ExpiryAddedItems.Add(new clsExpiredItemReturnDetailVO()
                        { 
                            ItemID = item.ItemID, ItemName = item.ItemName, ItemCode = item.ItemCode, AvailableStock = item.AvailableStock, 
                            BatchCode = item.BatchCode, BatchID = item.BatchID, BatchExpiryDate = item.BatchExpiryDate, UnitID = item.UnitID, PurchaseRate = item.PurchaseRate, 
                            ReturnQty = item.ReturnQty, VATPercentage = item.VATPercentage, Conversion = item.Conversion, TaxPercentage = item.TaxPercentage
                        }
                            );
                }

                dgexpItem.Focus();
                dgexpItem.UpdateLayout();

                dgexpItem.SelectedIndex = ExpiryAddedItems.Count - 1;

                ExpiryItemReturnMaster.StoreID = Itemswin.StoreID;
                ExpiryItemReturnMaster.SupplierID = Itemswin.SupplierID;
                if (ExpiryAddedItems.Count != 0)
                {
                    CmdSave.IsEnabled = true;
                }
                else
                {
                    CmdSave.IsEnabled = false;
                }
                CalculateGRNSummary();
            }
        }

        /// <summary>
        /// User control loaded
        /// </summary>
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {
                dpreturnDt.SelectedDate = DateTime.Now;

                ExpiryItemReturnMaster = new clsExpiredItemReturnVO();

                this.txtNetAmt.DataContext =  txtRemark.DataContext = txtTAmt.DataContext =  txtTTaxAmt.DataContext = txtTVAT.DataContext = ExpiryItemReturnMaster;

                ExpiryAddedItems = new ObservableCollection<clsExpiredItemReturnDetailVO>();
                dgexpItem.ItemsSource = ExpiryAddedItems;

                //FillClinic();
                FillStores(ClinicID);
                FillSupplier();
                IsPageLoaded = true;
            }
        }

        //private void FillClinic()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_Store;
        //    BizAction.Parent = new KeyValue();
        //    BizAction.Parent.Key = "1";
        //    BizAction.Parent.Value = "Status";

        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
        //    client1.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            cmbClinic.ItemsSource = null;
        //            BizAction.MasterList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
        //            MasterListItem Default = new MasterListItem { ID = 0, Description = "--Select--" };
        //            BizAction.MasterList.Insert(0, Default);
        //            cmbClinic.ItemsSource = BizAction.MasterList;
        //            cmbClinic.SelectedItem = Default;
                    
        //        }
        //    };
        //    client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client1.CloseAsync();
        //}

        /// <summary>
        /// Fills stores according clinic
        /// </summary>
          private void FillStores(long pClinicID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.IsActive = true;
            BizAction.MasterList = new List<MasterListItem>();

            if (pClinicID > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
            }

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client1.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    cmbClinic.ItemsSource = null;
                    BizAction.MasterList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                    MasterListItem Default = new MasterListItem { ID = 0, Description = "--Select--",Status = true };
                    BizAction.MasterList.Insert(0, Default);

                   
                    cmbClinic.ItemsSource = BizAction.MasterList;
                    cmbClinic.SelectedItem = BizAction.MasterList[0];
                }
            };
            client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client1.CloseAsync();
        }

          /// <summary>
          /// Fills supplier
          /// </summary>
        private void FillSupplier()
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
                    
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    
                    cmbSupplier.ItemsSource = null;
                    cmbSupplier.ItemsSource = objList;                    

                    cmbSupplier.SelectedItem = objList[0];                    
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        /// <summary>
        /// Expire item grid edited
        /// </summary>
        private void dgexpItem_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {            
            if (dgexpItem.SelectedItem != null)
            {
                if (e.Column.DisplayIndex == 6 && ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).AvailableStock < ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be greater than Current Stock.\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = 1;                   
                }
                else if (e.Column.DisplayIndex == 6 && ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty < 1)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Return Quantity cannot be less than 1 .\n Please Enter proper Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                    ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem).ReturnQty = 1;
                }
                else if (e.Column.DisplayIndex == 6 || e.Column.DisplayIndex == 7 || e.Column.DisplayIndex == 9)
                    CalculateGRNSummary();
            }
        }

        /// <summary>
        /// Calculates summary
        /// </summary>
        private void CalculateGRNSummary()
        {            
            ExpiryItemReturnMaster.NetAmount = ExpiryItemReturnMaster.OtherDeducution = ExpiryItemReturnMaster.TotalAmount = ExpiryItemReturnMaster.TotalOctriAmount = ExpiryItemReturnMaster.TotalTaxAmount = ExpiryItemReturnMaster.TotalVATAmount = 0;

            foreach (var item in ExpiryAddedItems)
            {
                ExpiryItemReturnMaster.TotalAmount += item.Amount;
                ExpiryItemReturnMaster.TotalVATAmount += item.VATAmount;
                ExpiryItemReturnMaster.TotalTaxAmount += item.TaxAmount;
                ExpiryItemReturnMaster.NetAmount += item.NetAmount;
                
            }

            this.txtNetAmt.DataContext =  txtRemark.DataContext = txtTAmt.DataContext = txtTTaxAmt.DataContext = txtTVAT.DataContext = ExpiryItemReturnMaster;
        }

        int ClickedFlag1 = 0;
        /// <summary>
        /// Save button click
        /// </summary>
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            PalashDynamics.UserControls.WaitIndicator wt = new UserControls.WaitIndicator();
            bool flagIsValid = true;
            ClickedFlag1 = ClickedFlag1 + 1;
            if (ClickedFlag1 == 1)
            {
              

                List<clsExpiredItemReturnDetailVO> objList = ExpiryAddedItems.ToList<clsExpiredItemReturnDetailVO>();
                clsAddExpiryItemReturnBizActionVO BizAction = new clsAddExpiryItemReturnBizActionVO();
                BizAction.objExpiryItem = new clsExpiredItemReturnVO();
                BizAction.objExpiryItem.Time = DateTime.Now;
                 BizAction.objExpiryItem = this.ExpiryItemReturnMaster;

                //User Related Values specified in DAL           
                BizAction.objExpiryItem.Status = true;

                if (dpreturnDt.SelectedDate.Value != null)
                {
                    BizAction.objExpiryItem.Date = dpreturnDt.SelectedDate.Value;
                }
                else
                {
                    BizAction.objExpiryItem.Date = DateTime.Now;
                }

                if (BizAction.objExpiryItem.Date.Value.Date > DateTime.Now.Date)
                {
                    dpreturnDt.SetValidation("Return Date should not be greater than Today's Date");
                    dpreturnDt.RaiseValidationError();
                    flagIsValid = false; ;
                    ClickedFlag1 = 0;
                    return;
                }
                else
                {
                    dpreturnDt.ClearValidationError();
                }
                if (cmbClinic.SelectedItem == null)
                    {
                        cmbClinic.TextBox.SetValidation("Store can not be blank.");
                        cmbClinic.TextBox.RaiseValidationError();
                        cmbClinic.Focus();
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Store can not be blank", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                        flagIsValid = false; ;
                        ClickedFlag1 = 0;
                        return;
                    }

                else if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                {
                    cmbClinic.TextBox.SetValidation("Store can not be blank.");
                    cmbClinic.TextBox.RaiseValidationError();
                    cmbClinic.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Store can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    flagIsValid = false;
                    ClickedFlag1 = 0;
                    return;

                }
                else
                {
                    cmbClinic.TextBox.ClearValidationError();
                }

                if (cmbSupplier.SelectedItem == null)
                {
                    cmbSupplier.TextBox.SetValidation("Supplier can not be blank.");
                    cmbSupplier.TextBox.RaiseValidationError();
                    cmbSupplier.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Supplier can not be blank", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    flagIsValid = false; ;
                    ClickedFlag1 = 0;
                    return;
                }

                else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
                {
                    cmbSupplier.TextBox.SetValidation("Supplier can not be blank.");
                    cmbSupplier.TextBox.RaiseValidationError();
                    cmbSupplier.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Supplier can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    flagIsValid = false;
                    ClickedFlag1 = 0;
                    return;
                }
                else
                {
                    cmbSupplier.TextBox.ClearValidationError();
                }
             
               
                if (objList != null && objList.Count > 0)
                {
                    foreach (var item in objList)
                    {
                        if (item.ReturnQty == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Zero Quantity.", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            flagIsValid = false;
                            ClickedFlag1 = 0;
                            return;


                        }
                    }

                }
                if (objList.Count == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                           new MessageBoxControl.MessageBoxChildWindow("","You can not save without Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    flagIsValid = false;
                    ClickedFlag1 = 0;
                    return;
                }


                if (flagIsValid)
                {
                    BizAction.objExpiryItemDetailList = (List<clsExpiredItemReturnDetailVO>)ExpiryAddedItems.ToList<clsExpiredItemReturnDetailVO>();
                    wt.Show();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, args) =>
                    {
                        ClickedFlag1 = 0;
                        FillStores(ClinicID);
                        FillSupplier();
                        //cmbClinic.SelectedValue = 0;
                        //cmbSupplier.SelectedValue = 0;
                        if (args.Error == null && args.Result != null)
                        {
                            ExpiryAddedItems.Clear();
                            ExpiryItemReturnMaster.NetAmount = ExpiryItemReturnMaster.OtherDeducution = ExpiryItemReturnMaster.TotalAmount = ExpiryItemReturnMaster.TotalOctriAmount = ExpiryItemReturnMaster.TotalTaxAmount = ExpiryItemReturnMaster.TotalVATAmount = 0;
                            ExpiryItemReturnMaster = null;
                            ExpiryItemReturnMaster = new clsExpiredItemReturnVO();
                            txtRemark.Text = string.Empty;
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Expiry Items Return Saved Successfully.", "Expiry Items Return Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbx.Show();
                            CmdSave.IsEnabled = false;
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Expiry Items Return Not Saved.", "Expiry Items Return Not Saved. \n Some Error Occured while saving information.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbx.Show();
                        }
                        wt.Close();
                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                else
                
                    ClickedFlag1 = 0;
            }
        }

        /// <summary>
        /// Close button click
        /// </summary>
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent(new InventoryDashBoard());
        }

        /// <summary>
        /// deletes expired item
        /// </summary>
        private void cmdDeleteexpItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgexpItem.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ExpiryAddedItems.RemoveAt(dgexpItem.SelectedIndex);
                        CalculateGRNSummary();

                    }
                };

                msgWD.Show();
            }
        }

        /// <summary>
        /// Clinic selection change event
        /// </summary>
        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null)
                {
                    if (ExpiryAddedItems != null)
                        ExpiryAddedItems.Clear();
                }

            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        

       
      
    }
}
