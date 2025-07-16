using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Windows.Browser;
using MessageBoxControl;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Data;
using PalashDynamics.Pharmacy.ItemSearch;
using System.Windows.Input;

namespace PalashDynamics.Pharmacy
{
    public partial class frmNewWOGRN : UserControl
    {
        public frmNewWOGRN()
        {
            //Added
            try
            {
                InitializeComponent();
                this.DataContext = new clsWOGRNVO();
                _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                dgAddGRNItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgAddGRNItems_CellEditEnded);
                dtpFromDate.SelectedDate = DateTime.Now.Date;
                dtpToDate.SelectedDate = DateTime.Now.Date;

                //======================================================
                //Paging
                DataList = new PagedSortableCollectionView<clsWOGRNVO>();
                DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                DataListPageSize = 15;
                dgDataPager.PageSize = DataListPageSize;
                dgDataPager.Source = DataList;
                //======================================================

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region 'Paging'

        public PagedSortableCollectionView<clsWOGRNVO> DataList { get; private set; }
        public Boolean blnDelete;
        public Boolean blnAdd;
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillGRNSearchList();

        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 

        MasterListItem SelectedItem = new MasterListItem();
        private void FillGRNSearchList()
        {
            indicator.Show();
            dgGRNList.ItemsSource = null;
            dgGRNItems.ItemsSource = null;

            clsGetWOGRNListBizActionVO BizAction = new clsGetWOGRNListBizActionVO();
            //BizAction.IsActive = true;
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
            //if (txtGRNNo.SelectedItem != null)
            //    BizAction.GRNNo = txtGRNNo.;
            BizAction.GRNNo = txtGRNNo.Text;
            if (cmbSearchSupplier.SelectedItem != null)
                BizAction.SupplierID = ((MasterListItem)cmbSearchSupplier.SelectedItem).ID;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetWOGRNListBizActionVO result = e.Result as clsGetWOGRNListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;

                    if (result.List != null)
                    {
                        DataList.Clear();
                        foreach (var item in result.List)
                        {
                            DataList.Add(item);


                        }

                        dgGRNList.ItemsSource = null;
                        dgGRNList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList;

                    }

                }
                indicator.Close();

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void txtAutocompleteText_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteText_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsPersonNameValid())
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

        #endregion

        PalashDynamics.Animations.SwivelAnimation _flip = null;
        bool IsPageLoaded = false;
        public event RoutedEventHandler OnBatchSelection_Click;
        private List<clsWOGRNDetailsVO> lstGRNDetailsDeepCopy = new List<clsWOGRNDetailsVO>();
        string LoggedinUserName = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
        int iCount, iItemsCount = 0;

        public List<clsWOGRNDetailsVO> GRNAddedItems { get; set; }
        public ObservableCollection<clsWOGRNDetailsVO> GRNPOAddedItems { get; set; }
        public PagedCollectionView PCVData;
        double orgVatPer = 0;
        double orgTax = 0;

        public Boolean ISEditMode = false;

        byte[] File;
        string fileName;
        bool IsFileAttached = false;
        public string PreviousBatchValue = "";
        /// <summary>
        /// Update the Pending quantity on Quantity Change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellEditChanges(object sender, DataGridCellEditEndedEventArgs e, double lngPOPendingQuantity)
        {

            clsWOGRNDetailsVO objSelectedGRNItem = (clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem;

            double itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID).ToList().Sum(x => x.Quantity);

            double Pendingqty = 0;
            if (objSelectedGRNItem.GRNID != 0 && chkIsFinalized.IsChecked == false)
            {
                Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID).First().PendingQuantity;
            }
            else
                Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID).First().PendingQuantity;

            if (itemQty <= Pendingqty)
            {
                foreach (clsWOGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID).ToList())
                {
                    item.WOPendingQuantity = Pendingqty - itemQty;
                }
            }
            else
            {

                objSelectedGRNItem.Quantity = 0;
                itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID).ToList().Sum(x => x.Quantity);
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW3.Show();
                foreach (clsWOGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID).ToList())
                {
                    item.WOPendingQuantity = Pendingqty - itemQty;
                }
            }
        }
        void dgAddGRNItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            DataGrid dgItems = (DataGrid)sender;
            clsWOGRNDetailsVO obj = (clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem;
            double lngPOPendingQuantity = 0;
            Boolean blnAddItem = false;
            if (dgAddGRNItems.SelectedItem != null)
            {
                if (e.Column.Header != null)
                {
                    if (e.Column.Header.ToString().Equals("Free Quantity") || e.Column.Header.ToString().Equals("Cost Price"))
                    {
                        if (((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP > 0 && ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP < ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "MRP should be greater than rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;
                            return;
                        }
                    }
                    if (e.Column.Header.ToString().Equals("MRP"))
                    {
                        if (obj.MRP > 0 && obj.ConversionFactor > 0)
                        {
                            obj.UnitMRP = obj.MRP;
                        }
                    }

                    if (e.Column.Header.ToString().Equals("Quantity"))
                    {
                        lngPOPendingQuantity = obj.WOPendingQuantity;
                        if (GRNAddedItems.Where(z => z.ItemID == obj.ItemID && z.UnitId == obj.UnitId).Count() > 1)
                        {
                            if (rdbDirectPur.IsChecked != true)
                            {
                                CellEditChanges(sender, e, lngPOPendingQuantity);
                                blnAddItem = true;
                            }
                        }
                        if (obj.Quantity == 0 && obj.FreeQuantity == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Quantity In The List Can't Be Zero. Please enter Quantity or free quantity greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            return;
                        }

                        if (!blnAddItem)
                        {
                            if (obj.ItemQuantity > 0)
                            {
                                if ((obj.ItemQuantity < obj.Quantity || obj.WOQuantity < obj.Quantity) && rdbDirectPur.IsChecked == false)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();
                                    obj.Quantity = obj.ItemQuantity;
                                    return;
                                }

                                else
                                    obj.WOPendingQuantity = obj.ItemQuantity - obj.Quantity;
                            }
                            else if ((obj.PendingQuantity < obj.Quantity || obj.WOQuantity < obj.Quantity) && rdbDirectPur.IsChecked == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                 new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                obj.Quantity = obj.WOPendingQuantity;
                                obj.WOPendingQuantity = obj.PendingQuantity - obj.Quantity;
                                return;
                            }
                            else
                            {
                                if (rdbDirectPur.IsChecked != true)
                                {
                                    obj.WOPendingQuantity = obj.PendingQuantity - obj.Quantity;
                                }

                            }
                        }

                    }
                    if (e.Column.Header.ToString().Equals("Batch Code"))
                    {
                        //PreviousBatchValue = obj.BatchCode;
                        if (PreviousBatchValue != obj.BatchCode)
                        {
                            obj.BarCode = obj.BatchCode;
                        }
                    }
                    if (e.Column.Header.ToString().Equals("C.Disc. %"))
                    {
                        if (obj.CDiscountPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Concession Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();

                            obj.CDiscountPercent = 0;
                            return;
                        }
                        if (obj.CDiscountPercent == 100)
                        {

                            if (obj.SchDiscountPercent == 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                          new MessageBoxControl.MessageBoxChildWindow("", "Can't Enter C. Discount Percentage! Sch.Discount Percent is 100% ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();

                                obj.CDiscountPercent = 0;
                                return;
                            }
                            else
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }
                        }
                        if (obj.CDiscountPercent >= 0 && obj.CDiscountPercent < 100)
                        {
                            if (obj.SchDiscountPercent == 100)
                            {
                                obj.CDiscountPercent = 0;
                            }
                            else
                            {
                                if (obj.VATPercent == 0)
                                {
                                    obj.VATPercent = orgVatPer;
                                }
                                if (obj.ItemTax == 0)
                                {
                                    obj.ItemTax = orgTax;
                                }
                            }

                        }
                    }
                    if (e.Column.Header.ToString().Equals("Sch.Disc. %"))
                    {
                        if (obj.SchDiscountPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Sch. Discount Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.SchDiscountPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100)
                        {
                            if (obj.CDiscountPercent == 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                          new MessageBoxControl.MessageBoxChildWindow("", "Can't Enter Sch. Discount Percentage! C.Discount Percent is 100% ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                obj.SchDiscountPercent = 0;
                                return;
                            }
                            else
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }

                        }
                        if (obj.SchDiscountPercent >= 0 && obj.SchDiscountPercent < 100)
                        {
                            if (obj.CDiscountPercent == 100)
                            {
                                obj.SchDiscountPercent = 0;
                            }
                            else
                            {
                                if (obj.VATPercent == 0)
                                {
                                    obj.VATPercent = orgVatPer;
                                }
                                if (obj.ItemTax == 0)
                                {
                                    obj.ItemTax = orgTax;
                                }
                            }
                        }
                    }
                    if (e.Column.Header.ToString().Equals("VAT%"))
                    {
                        if (obj.VATPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "VAT Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.VATPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.VATPercent > 0)
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }
                        }
                    }
                    if (e.Column.Header.ToString().Equals("Cost Price"))
                    {
                        if (obj != null)
                        {
                            if (obj.Rate == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                   new MessageBoxControl.MessageBoxChildWindow("Zero Cost Price.", "Cost Price In The List Can't Be Zero. Please Enter Rate Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                return;
                            }
                            if (obj.Rate < 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                   new MessageBoxControl.MessageBoxChildWindow("Negative Cost Price.", "Cost Price Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                obj.Rate = 0;
                                return;
                            }
                            else if (obj.Rate > 0 && obj.ConversionFactor > 0)
                            {
                                obj.UnitRate = obj.Rate / obj.ConversionFactor;
                            }
                        }
                    }

                    if (e.Column.Header.ToString().Equals("Batch Code") || e.Column.Header.ToString().Equals("Expiry Date"))
                    {
                        if (obj != null)
                        {
                            foreach (var item in GRNAddedItems)
                            {
                                if (item.ItemID == obj.ItemID)
                                {
                                    if (item.IsBatchRequired == true)
                                    {
                                        if (obj.BatchCode == string.Empty || obj.BatchCode == null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("Batch Required!.", "Please Enter Batch!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW3.Show();
                                            return;

                                        }
                                        else if (obj.ExpiryDate == null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!.", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW3.Show();
                                            return;

                                        }
                                        else if (obj.ExpiryDate < DateTime.Now.Date)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date must not be less than today's date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW3.Show();
                                            obj.ExpiryDate = DateTime.Now.Date;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (e.Column.Header.ToString().Equals("Conversion Factor"))
                    {
                        if (obj.ConversionFactor <= 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Conversion factor must be greater than the zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.ConversionFactor = 1;
                        }
                        else if (obj.ConversionFactor != null && obj.ConversionFactor > 0)
                        {
                            obj.MRP = obj.UnitMRP;
                            obj.Rate = obj.UnitRate;
                        }
                    }

                    if (e.Column.Header.ToString().Equals("PO Pending Quantity") || e.Column.Header.ToString().Equals("Quantity") || e.Column.Header.ToString().Equals("Free Quantity") || e.Column.Header.ToString().Equals("Cost Price") || e.Column.Header.ToString().Equals("Amount")
                       || e.Column.Header.ToString().Equals("C.Disc. Amount") || e.Column.Header.ToString().Equals("Sch.Disc. %") || e.Column.Header.ToString().Equals("C.Disc. %"))
                        CalculateGRNSummary();

                }
            }
        }

        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdDraft.IsEnabled = false;
                    cmdCancel.IsEnabled = false;

                    break;

                case "Save":
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdDraft.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = false;

                    break;

                case "Modify":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true; //false;
                    cmdSave.Content = "Save"; //"Modify";
                    cmdDraft.IsEnabled = true;  //false;
                    cmdCancel.IsEnabled = true;

                    break;

                case "ClickNew":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdSave.Content = "Save";
                    cmdDraft.IsEnabled = true;
                    cmdCancel.IsEnabled = true;

                    break;

                default:
                    break;
            }
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rdbDirectPur.IsChecked = true;
            //dgPO.Height = 90;
            //dgPO.Visibility = Visibility.Visible;
            if (!IsPageLoaded)
            {
                this.DataContext = new clsWOGRNVO();

                long clinicId = 0; //= ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    clinicId = 0;
                }
                else
                {
                    clinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                FillStores(clinicId);
                FillReceivedByList();
                FillSupplier();

                //======================================================
                //Paging
                GetGRNNumber();
                FillGRNSearchList();
                //======================================================

                SetCommandButtonState("New");
                _flip.Invoke(RotationType.Backward);
                GRNAddedItems = new List<clsWOGRNDetailsVO>();
                GRNPOAddedItems = new ObservableCollection<clsWOGRNDetailsVO>();
                //dgAddGRNItems.ItemsSource = GRNAddedItems;
                dgAddGRNItems.ItemsSource = new ObservableCollection<clsWOGRNDetailsVO>();
                IsPageLoaded = true;
                dpInvDt.SelectedDate = DateTime.Today;
                IsFileAttached = false;
                dpInvDt.SelectedDate = DateTime.Now.Date;
                //dpInvDt.IsEnabled = false;

            }

        }

        /// <summary>
        /// Fills store according to clinicid
        /// </summary>
        private void FillStores(long pClinicID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            if (pClinicID > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //cmbBloodGroup.ItemsSource = null;
                    //cmbBloodGroup.ItemsSource = objList;
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    //if (objList.Count > 1)
                    //    cmbStore.SelectedItem = objList[1];
                    //else
                    cmbStore.SelectedItem = objList[0];
                    if (this.DataContext != null)
                    {
                        cmbStore.SelectedValue = ((clsWOGRNVO)this.DataContext).StoreID;


                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
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

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //cmbBloodGroup.ItemsSource = null;
                    //cmbBloodGroup.ItemsSource = objList;
                    cmbSupplier.ItemsSource = null;
                    cmbSupplier.ItemsSource = objList;

                    cmbSearchSupplier.ItemsSource = null;
                    cmbSearchSupplier.ItemsSource = objList;


                    cmbSupplier.SelectedItem = objList[0];
                    cmbSearchSupplier.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbSupplier.SelectedValue = ((clsWOGRNVO)this.DataContext).SupplierID;
                        cmbSearchSupplier.SelectedValue = ((clsWOGRNVO)this.DataContext).SupplierID;

                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        ItemSearchRowForGRN _ItemSearchRowControl = null;
        private void AttachItemSearchControl(long StoreID, long SupplierID)
        {
            BdrItemSearch.Visibility = Visibility.Visible;
            ItemSearchStackPanel.Children.Clear();
            _ItemSearchRowControl = new ItemSearchRowForGRN(StoreID, SupplierID);
            _ItemSearchRowControl.OnQuantityEnter_Click += new RoutedEventHandler(_ItemSearchRowControl_OnQuantityEnter_Click);
            // _ItemSearchRowControl.SetFocus();
            ItemSearchStackPanel.Children.Add(_ItemSearchRowControl);
        }

        void _ItemSearchRowControl_OnQuantityEnter_Click(object sender, RoutedEventArgs e)
        {
            ItemSearchRowForGRN _ItemSearchRowControl = (ItemSearchRowForGRN)sender;
            if (GRNAddedItems != null)
            {
                if (GRNAddedItems.Count.Equals(0))
                {
                    GRNAddedItems = new List<clsWOGRNDetailsVO>();
                }
            }
            else
            {
                GRNAddedItems = new List<clsWOGRNDetailsVO>();
            }


            _ItemSearchRowControl.SelectedItems[0].RowID = GRNAddedItems.Count + 1;




            #region non Editable row
            //if (_ItemSearchRowControl.SelectedItems[0].IsBatchRequired == false)
            //{
            //    DataGridColumn column = dgAddGRNItems.Columns[2];

            //    FrameworkElement fe = column.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            //    FrameworkElement result = GetParent(fe, typeof(DataGridCell));

            //    if (result != null)
            //    {
            //        DataGridCell cell = (DataGridCell)result;
            //        cell.IsEnabled = false;
            //    }
            //}
            if (_ItemSearchRowControl.SelectedItems[0].IsBatchRequired == false)
            {
                _ItemSearchRowControl.SelectedItems[0].IsReadOnly = true;
            }
            else
            {
                _ItemSearchRowControl.SelectedItems[0].IsReadOnly = false;
            }
         

            DataGridColumn column2 = dgAddGRNItems.Columns[3];
            FrameworkElement fe1 = column2.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

            if (result1 != null)
            {
                DataGridCell cell = (DataGridCell)result1;
                cell.IsEnabled = false;
            }

            DataGridColumn column3 = dgAddGRNItems.Columns[8];
            FrameworkElement fe2 = column3.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

            if (result2 != null)
            {
                DataGridCell cell = (DataGridCell)result2;
                cell.IsEnabled = false;
            }

            DataGridColumn column4 = dgAddGRNItems.Columns[11];
            FrameworkElement fe4 = column4.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result3 = GetParent(fe4, typeof(DataGridCell));

            if (result3 != null)
            {
                DataGridCell cell = (DataGridCell)result3;
                cell.IsEnabled = false;
            }

            DataGridColumn column5 = dgAddGRNItems.Columns[12];
            FrameworkElement fe5 = column5.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result4 = GetParent(fe5, typeof(DataGridCell));

            if (result4 != null)
            {
                DataGridCell cell = (DataGridCell)result4;
                cell.IsEnabled = false;
            }



            #endregion
            bool IsItemAlreadyAdded = false;
            foreach (var item in GRNAddedItems)
            {
                if (item.ItemCode == _ItemSearchRowControl.SelectedItems[0].ItemCode && item.BatchCode == _ItemSearchRowControl.SelectedItems[0].BatchCode)
                    IsItemAlreadyAdded = true;
                else
                    IsItemAlreadyAdded = false;
                ////By Anjali..........
                //item.BarCode = item.BatchCode;
                ////...........................


            }
            if (IsItemAlreadyAdded == false)
            {
                //By Anjali...............
                //By Somnath...............
                //if (GRNAddedItems != null && GRNAddedItems.Count() > 0)
                //{
                //    foreach (var item in GRNAddedItems)
                //    {
                //        if (item.ItemID == _ItemSearchRowControl.SelectedItems[0].ItemID && item.BatchCode == _ItemSearchRowControl.SelectedItems[0].BatchCode)
                //        {
                //            _ItemSearchRowControl.SelectedItems[0].BarCode = _ItemSearchRowControl.SelectedItems[0].BatchCode;
                //            GRNAddedItems.Add(_ItemSearchRowControl.SelectedItems[0]);
                //        }
                //    }
                //}
                //else
                //{
                //    _ItemSearchRowControl.SelectedItems[0].BarCode = _ItemSearchRowControl.SelectedItems[0].BatchCode;
                //    GRNAddedItems.Add(_ItemSearchRowControl.SelectedItems[0]);
                //}
                //_ItemSearchRowControl.SelectedItems[0].BarCode = _ItemSearchRowControl.SelectedItems[0].BatchCode;


                //GRNAddedItems.Add(_ItemSearchRowControl.SelectedItems[0]);

                if (_ItemSearchRowControl.SelectedItems[0].BarCode == null)
                {
                    foreach (var item in GRNAddedItems)
                    {
                        if (item.ItemID == _ItemSearchRowControl.SelectedItems[0].ItemID && item.BatchCode == _ItemSearchRowControl.SelectedItems[0].BatchCode)
                        {
                            item.BarCode = _ItemSearchRowControl.SelectedItems[0].BatchCode;
                        }
                    }
                }


                //.......................

                if (rdbDirectPur.IsChecked == true)
                {
                    dgAddGRNItems.Columns[28].Visibility = Visibility.Collapsed;
                }
                else
                {
                    dgAddGRNItems.Columns[28].Visibility = Visibility.Visible;
                }

                dgAddGRNItems.ItemsSource = null;
                dgAddGRNItems.ItemsSource = GRNAddedItems;
                dgAddGRNItems.UpdateLayout();
                // CalculateTotal();
                CalculateGRNSummary();
                //_ItemSearchRowControl.SetFocus();
                //_ItemSearchRowControl.cmbItemName.Focus();
                if (_ItemSearchRowControl.SelectedItems[0].IsBatchRequired == false)
                {
                    set();
                }

                _ItemSearchRowControl.SetFocus();
                _ItemSearchRowControl = null;
                _ItemSearchRowControl = new ItemSearchRowForGRN();
                _ItemSearchRowControl.cmbItemName.Focus();

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Item with same batch already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                _ItemSearchRowControl.SetFocus();

            }
        }


        private void set()
        {

            DataGridColumn column = dgAddGRNItems.Columns[2];

            FrameworkElement fe = column.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result = GetParent(fe, typeof(DataGridCell));

            if (result != null)
            {
                DataGridCell cell = (DataGridCell)result;
                cell.IsEnabled = false;
            }
        }
        /// <summary>
        /// Fills Received by
        /// </summary>
        private void FillReceivedByList()
        {
            clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.IsDecode = true;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetUserMasterListBizActionVO)e.Result).MasterList);

                    var result = from item in objList
                                 where item.ID == ((IApplicationConfiguration)App.Current).CurrentUser.ID
                                 select item;


                    cmbReceivedBy.ItemsSource = null;
                    cmbReceivedBy.ItemsSource = objList;
                    //cmbReceivedBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    //((clsGetUserMasterListBizActionVO)(cmbReceivedBy.SelectedItem)).ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    cmbReceivedBy.SelectedItem = result.ToList()[0];
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        void win_OnOkButtonClick(object sender, RoutedEventArgs e)
        {


        }

        private void dgDirectPurchase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txttotcamt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Search button click
        /// </summary>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            bool res = true;



            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
                {
                    //dtpFromDate.SetValidation("From Date should be less than To Date");
                    //dtpFromDate.RaiseValidationError();
                    //dtpFromDate.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "From Date should be less than To Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    dtpFromDate.SelectedDate = dtpToDate.SelectedDate;
                    res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }



            else if (dtpFromDate.SelectedDate != null && dtpFromDate.SelectedDate.Value.Date > DateTime.Now.Date)
            {
                //dtpFromDate.SetValidation("From Date should not be greater than Today's Date");
                //dtpFromDate.RaiseValidationError();
                //dtpFromDate.Focus();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "From Date should not be greater than Today's Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                dtpFromDate.SelectedDate = DateTime.Now;
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Plase Select To Date");
                dtpToDate.RaiseValidationError();
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate == null)
            {
                dtpFromDate.SetValidation("Plase Select From Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (res)
            {
                //======================================================
                //Paging
                dgDataPager.PageIndex = 0;
                FillGRNSearchList();
                //======================================================
            }

        }


        /// <summary>
        /// Fills GRN Details List according to GRNID
        /// </summary>

        private void FillGRNDetailslList(long pGRNID)
        {
            clsGetWOGRNDetailsListBizActionVO BizAction = new clsGetWOGRNDetailsListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.GRNID = pGRNID;
            BizAction.UnitId = ((clsWOGRNVO)dgGRNList.SelectedItem).UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsWOGRNDetailsVO> objList = new List<clsWOGRNDetailsVO>();

                    objList = ((clsGetWOGRNDetailsListBizActionVO)e.Result).List;
                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;

                    dgGRNItems.ItemsSource = null;
                    dgGRNItems.ItemsSource = objList;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("ClickNew");
            InitialiseForm();
            //this.DataContext = new clsGRNVO();

            //long clinicId = 0; //= ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            //{
            //    clinicId = 0;
            //}
            //else
            //{
            //    clinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}

            //FillStores(clinicId);
            //FillSupplier();
            GRNAddedItems = new List<clsWOGRNDetailsVO>();
            dgAddGRNItems.ItemsSource = null;
            BdrItemSearch.Visibility = Visibility.Collapsed;
            _flip.Invoke(RotationType.Forward);
            dpInvDt.SelectedDate = DateTime.Today;
            cmbReceivedBy.SelectedValue = 0;
            //DataGridColumnVisibility();
        }

        int ClickedFlag1 = 0;
        public clsWOGRNVO GRNItemDetails;

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Boolean blnValid = false;
            iItemsCount = 0;

            iCount = GRNAddedItems.Count;
            //for (int iGrnItems = 0; iGrnItems < GRNAddedItems.Count; iGrnItems++)
            //{
            if (GRNAddedItems == null || GRNAddedItems.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "At least one item is compulsory for saving Goods Received Note", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return;
            }
            foreach (clsWOGRNDetailsVO item in GRNAddedItems)
            {

                bool b = ValidateOnSave(item);
                if (b)
                {
                    blnValid = true;

                    iItemsCount++;
                }
                if (!b)
                {
                    return;
                }
            }

            if (blnValid && iCount != 0 && iItemsCount == iCount)
            {
                string msgTitle = "Palash";
                string msgText = "";
                chkIsFinalized.IsChecked = true;
                if (chkConsignment.IsChecked == false)
                {
                    msgText = "Are you sure you want to Save GRN ?";
                }
                else
                {
                    msgText = "Are you sure you want to Save Consignment GRN ?";
                }


                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                msgWin.Show();
            }

        }

        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {

                SaveGRN(false);
            }
            else if (result == MessageBoxResult.No)
            {
                iItemsCount = 0;
                chkIsFinalized.IsChecked = false;
            }
        }

        private void ConfirmSave(bool IsDraft)
        {
            string msgTitle = "";
            string msgText = "Are you sure you want to save the GRN Details";

            if (IsDraft)
                msgText += " as Draft";

            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW.OnMessageBoxClosed += (res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    SaveGRN(IsDraft);
                }
                else
                    ClickedFlag1 = 0;
            };

            msgW.Show();
        }


        /// <summary>
        /// Validates form
        /// </summary>
        private bool ValidateForm()
        {
            bool isValid = true;

            if (cmbStore.SelectedItem == null)
            {
                cmbStore.TextBox.SetValidation("Please Select the Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
                isValid = false;
                return false;
            }
            else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
            {
                cmbStore.TextBox.SetValidation("Please Select the Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
                isValid = false;
                return false;
            }
            else
                cmbStore.TextBox.ClearValidationError();


            if (cmbSupplier.SelectedItem == null)
            {
                cmbSupplier.TextBox.SetValidation("Please Select The Supplier");
                cmbSupplier.TextBox.RaiseValidationError();
                cmbSupplier.Focus();
                isValid = false;
                return false;
            }
            else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
            {
                cmbSupplier.TextBox.SetValidation("Please Select The Supplier");
                cmbSupplier.TextBox.RaiseValidationError();
                cmbSupplier.Focus();
                isValid = false;
                return false;
            }
            else
                cmbSupplier.TextBox.ClearValidationError();

            if (GRNAddedItems == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "At least one item is compulsory for saving Goods Received Note", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                isValid = false;
                return false;
            }
            else if (GRNAddedItems.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                              new MessageBoxControl.MessageBoxChildWindow("", "At least one item is compulsory for saving Goods Received Note", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW2.Show();
                isValid = false;
                return false;
            }

            if (dpInvDt.SelectedDate == null)
            {
                dpInvDt.SetValidation("Please Select The Invoice Date");
                dpInvDt.RaiseValidationError();
                dpInvDt.Focus();
                isValid = false;
                return false;
            }
            //else if (dpInvDt.SelectedDate < Convert.ToDateTime(((clsGRNVO)this.DataContext).PODate) || dpInvDt.SelectedDate > dpgrDt.SelectedDate) //.ToShortDateString()
            //{
            //    dpInvDt.SetValidation("Invoice Date Must be between PO date and GRN date");
            //    dpInvDt.RaiseValidationError();
            //    dpInvDt.Focus();
            //    isValid = false;
            //    return false;
            //}
            else
            {
                dpInvDt.ClearValidationError();
            }

            List<clsWOGRNDetailsVO> objList = GRNAddedItems.ToList<clsWOGRNDetailsVO>();
            double TotalQuantity = 0;
            if (objList != null && objList.Count > 0)
            {
                foreach (var item in objList)
                {
                    TotalQuantity = 0;
                    if (item.IsBatchRequired == true)
                    {
                        if (item.BatchCode == "" || item.BatchCode == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msg =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Batch Code is Required for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msg.Show();
                            isValid = false;
                            break;
                        }
                        if (item.ExpiryDate == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                          new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            isValid = false;
                            break;
                        }
                    }
                    TotalQuantity = item.Quantity + item.FreeQuantity;
                    if (TotalQuantity == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                         new MessageBoxControl.MessageBoxChildWindow("Zero Quantity.", "Quantity In The List Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        isValid = false;
                        break;

                    }
                    if (item.Rate == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW4 =
                        new MessageBoxControl.MessageBoxChildWindow("Zero Rate.", "Rate In The List Can't Be Zero. Please Enter Rate.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW4.Show();
                        isValid = false;
                        break;
                    }
                    if (item.ItemGroup != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
                    {
                        if (item.MRP <= 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW4 =
                                                   new MessageBoxControl.MessageBoxChildWindow("Zero MRP.", "MRP In The List Can't Be Zero. Please Enter MRP Greater Than Rate.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW4.Show();
                            ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;
                            isValid = false;
                            break;
                        }
                    }
                    if (item.Quantity > item.WOPendingQuantity && rdbDirectPur.IsChecked == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW4 =
                                               new MessageBoxControl.MessageBoxChildWindow("Quantity.", "Quantity must be equal or less than the Purchase Order pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW4.Show();
                        isValid = false;
                        break;
                    }

                    if (isValid)
                    {
                        var chkitem = from r in GRNAddedItems
                                      where r.BatchCode == item.BatchCode && r.ExpiryDate == item.ExpiryDate
                                      && r.ItemID == item.ItemID && item.IsBatchRequired == true
                                      select new clsWOGRNDetailsVO
                                      {
                                          Status = r.Status,
                                          ID = r.ID
                                      };
                        if (chkitem.ToList().Count > 1)
                        {
                            isValid = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Same Batch appears more than once for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            break;
                        }


                        var chkitem1 = from r in GRNAddedItems
                                       where r.ItemID == item.ItemID && item.IsBatchRequired == false
                                       select new clsWOGRNDetailsVO
                                       {
                                           Status = r.Status,
                                           ID = r.ID
                                       };
                        if (chkitem1.ToList().Count > 1)
                        {
                            isValid = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Same Item appears more than once for NonBatchWise item " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            break;
                        }

                    }
                }
            }
            return isValid;
        }

        WaitIndicator Indicatior;
        public Boolean ValidateOnSave(clsWOGRNDetailsVO item)
        {
            double TotalQuantity = 0;
            Boolean isValid = true;
            if (cmbStore.SelectedItem == null || ((MasterListItem)cmbStore.SelectedItem).ID == 0)
            {
                cmbStore.TextBox.SetValidation("Please Select the Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
                isValid = false;
                return false;
            }

            else
                cmbStore.TextBox.ClearValidationError();
            if (cmbSupplier.SelectedItem == null || ((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
            {
                cmbSupplier.TextBox.SetValidation("Please Select The Supplier");
                cmbSupplier.TextBox.RaiseValidationError();
                cmbSupplier.Focus();
                isValid = false;
                return false;
            }

            else
                cmbSupplier.TextBox.ClearValidationError();
            if (dpInvDt.SelectedDate == null)
            {
                dpInvDt.SetValidation("Please Select The Invoice Date");
                dpInvDt.RaiseValidationError();
                dpInvDt.Focus();
                isValid = false;
                return false;
            }
            else
            {
                dpInvDt.ClearValidationError();
            }

            if ((item.IsBatchRequired == true) && (string.IsNullOrEmpty(item.BatchCode)))
            {
                MessageBoxControl.MessageBoxChildWindow msg =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Batch Code is Required for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msg.Show();
                isValid = false;
                return isValid;
            }
            if (item.IsBatchRequired == true && item.ExpiryDate == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
              new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW3.Show();
                isValid = false;
                return isValid;
            }

            TotalQuantity = item.Quantity + item.FreeQuantity;
            if (TotalQuantity == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                 new MessageBoxControl.MessageBoxChildWindow("Zero Quantity.", "Quantity In The List Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW3.Show();
                isValid = false;
                return isValid;

            }
            if (item.Rate == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                new MessageBoxControl.MessageBoxChildWindow("Zero Cost Price.", "Cost Price In The List Can't Be Zero. Please Enter Cost Price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW4.Show();
                isValid = false;
                return isValid;

            }
            if (item.ItemGroup != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
            {
                if (item.MRP <= 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW4 =
                                           new MessageBoxControl.MessageBoxChildWindow("MRP.", "MRP must be greater than the cost price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW4.Show();
                    ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;
                    isValid = false;
                    return isValid;
                }
            }

            if (item.MRP > 0 && item.MRP < item.Rate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                                       new MessageBoxControl.MessageBoxChildWindow("MRP.", "MRP must be greater than the cost price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW4.Show();
                ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;
                isValid = false;
                return isValid;
            }

            if (cmdSave.IsPressed == true && item.IsBatchRequired == true && string.IsNullOrEmpty(item.BarCode))
            {
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash.", "Please enter the BarCode.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW4.Show();
                isValid = false;
                return isValid;
            }
            var lst = GRNAddedItems.Where(itm => itm.ItemID.Equals(item.ItemID));

            if (lst != null && lst.Count() == 1 && this.ISEditMode == false)
            {
                if (item.ItemQuantity > 0)
                {
                    if (chkIsFinalized.IsChecked == false && (item.ItemQuantity < item.Quantity || item.WOQuantity < item.Quantity) && rdbDirectPur.IsChecked == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        isValid = false;
                        return isValid;
                    }
                    else
                        item.WOPendingQuantity = item.ItemQuantity - item.Quantity;
                }
                else if (chkIsFinalized.IsChecked == false && (item.PendingQuantity < item.Quantity || item.WOQuantity < item.Quantity) && rdbDirectPur.IsChecked == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    isValid = false;
                    return isValid;
                }
                else if (rdbDirectPur.IsChecked != true)
                {
                    item.WOPendingQuantity = item.PendingQuantity - item.Quantity;
                }
            }


            return isValid;
        }

        /// <summary>
        /// Saves GRN
        /// </summary>
        /// 
        private void SaveGRN(bool IsDraft)
        {
            Boolean blnValidSave = false;
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            //if (rdbPo.IsChecked == true)
            //{
                var POIDs = GRNAddedItems.GroupBy(i => i.WONO).Distinct().ToList();
                for (Int32 iPOID = 0; iPOID < POIDs.Count && !blnValidSave; iPOID++)
                {
                    var lstGRNItemIDWise = POIDs[iPOID].Where(z => z.ItemID == POIDs[iPOID].ToList()[0].ItemID).ToList();
                    List<clsWOGRNDetailsVO> lstTarget = lstGRNItemIDWise.ToList();

                    if (lstGRNItemIDWise.ToList().Count > 1)
                    {
                        if (lstTarget.Count > 1)
                            lstTarget.RemoveAt(0);
                        Int32 iQuantity = 0;
                        Int32 iPendingQuantity = 0;
                        foreach (clsWOGRNDetailsVO objGRNDetailsVO in lstTarget.ToList())
                        {
                            if (objGRNDetailsVO.ItemID == lstGRNItemIDWise.ToList()[0].ItemID && objGRNDetailsVO.WONO.Trim() == lstGRNItemIDWise.ToList()[0].WONO.Trim() && objGRNDetailsVO.BatchCode.Trim() == lstGRNItemIDWise.ToList()[0].BatchCode.Trim())
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "You cannot save the same Batch Code for the item : " + objGRNDetailsVO.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    return;
                                };
                                Indicatior.Close();
                                blnValidSave = true;
                                msgW1.Show();
                                break;
                            }
                            if (objGRNDetailsVO.ItemID == lstGRNItemIDWise.ToList()[0].ItemID && objGRNDetailsVO.WONO.Trim() == lstGRNItemIDWise.ToList()[0].WONO.Trim() && objGRNDetailsVO.BatchCode.Trim() == lstGRNItemIDWise.ToList()[0].BatchCode.Trim())
                            {
                                iQuantity += Convert.ToInt32(lstGRNItemIDWise.ToList()[0].Quantity) + Convert.ToInt32(objGRNDetailsVO.Quantity);
                                iPendingQuantity += Convert.ToInt32(lstGRNItemIDWise.ToList()[0].WOPendingQuantity) + Convert.ToInt32(objGRNDetailsVO.Quantity);
                                if (iQuantity > iPendingQuantity)
                                {
                                    blnValidSave = true;
                                    Indicatior.Close();
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                    {
                                        return;
                                    };
                                    msgW1.Show();
                                    break;
                                }
                            }
                        //}
                    }
                }
            }
            try
            {
                if (!blnValidSave)
                {
                    clsWOGRNVO GrnObj = new clsWOGRNVO();
                    GrnObj = (clsWOGRNVO)this.DataContext;

                    if (rdbPo.IsChecked == true)
                    {
                        GrnObj.GRNType = InventoryGRNType.AgainstPO;
                    }
                    else if (rdbDirectPur.IsChecked == true)
                    {
                        GrnObj.GRNType = InventoryGRNType.Direct;
                    }
                    if (cmbSupplier.SelectedItem != null)
                        GrnObj.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    if (cmbStore.SelectedItem != null)
                        GrnObj.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;

                    if (cmbReceivedBy.SelectedItem != null)
                        GrnObj.ReceivedByID = ((MasterListItem)cmbReceivedBy.SelectedItem).ID;

                    if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                    {
                        GrnObj.Items = new List<clsWOGRNDetailsVO>();
                        GrnObj.Items = GRNAddedItems.ToList();
                    }
                    foreach (clsWOGRNDetailsVO item33 in GRNPOAddedItems)
                    {
                        var item55 = from r55 in GRNAddedItems
                                     where r55.ItemID == item33.ItemID //&& r55.POID == item33.POID && r55.POUnitID == item33.POUnitID
                                     select r55;

                        if (item55 != null && item55.ToList().Count > 0)
                        {
                            item33.BatchCode = (item55.FirstOrDefault()).BatchCode;
                        }
                    }

                    if (GRNPOAddedItems != null && GRNPOAddedItems.Count > 0)
                    {
                        GrnObj.ItemsWOGRN = new List<clsWOGRNDetailsVO>();
                        GrnObj.ItemsWOGRN = GRNPOAddedItems.ToList();
                    }



                    clsAddWOGRNBizActionVO BizAction = new clsAddWOGRNBizActionVO();
                    BizAction.Details = new clsWOGRNVO();
                    BizAction.Details = GrnObj;
                    BizAction.IsDraft = IsDraft;

                    BizAction.IsEditMode = ISEditMode;

                    if (chkIsFinalized.IsChecked == true)
                    {
                        BizAction.Details.Freezed = true;
                    }
                    else
                    {
                        BizAction.Details.Freezed = false;
                    }

                    if (chkConsignment.IsChecked == true)
                    {
                        BizAction.Details.IsConsignment = true;
                    }
                    else
                        BizAction.Details.IsConsignment = false;

                    if (IsFileAttached)
                    {
                        BizAction.IsFileAttached = true;
                        BizAction.File = File;
                        BizAction.FileName = fileName;
                    }
                    else
                        BizAction.IsFileAttached = false;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        ClickedFlag1 = 0;
                        if (arg.Error == null && arg.Result != null && ((clsAddWOGRNBizActionVO)arg.Result).Details != null)
                        {
                            if (((clsAddWOGRNBizActionVO)arg.Result).Details != null)
                            {
                                GRNItemDetails = ((clsAddWOGRNBizActionVO)arg.Result).Details;

                                if (((clsAddWOGRNBizActionVO)arg.Result).IsDraft == false)
                                {
                                    PrintWOGRNItemBarCode barcodeWin = new PrintWOGRNItemBarCode();
                                    barcodeWin.WOGRNItemDetails = GRNItemDetails;
                                    barcodeWin.OnCancelButton_Click += new RoutedEventHandler(BarcodeWin_OnCancelButton_Click);
                                    barcodeWin.Show();
                                }

                                FillGRNSearchList();
                                string message = "GRN Details Saved Successfully With GRNNo " + ((clsAddGRNBizActionVO)arg.Result).Details.GRNNO;

                                if (((clsAddGRNBizActionVO)arg.Result).IsDraft)
                                    message += " as Draft";

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                SetCommandButtonState("Save");
                                msgW1.Show();
                                _flip.Invoke(RotationType.Backward);
                            }
                            Indicatior.Close();
                        }
                        else
                        {
                            Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Adding GRN Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                else
                    indicator.Close();
            }
            catch (Exception)
            {
                ClickedFlag1 = 0;
                Indicatior.Close();
            }
        }
        void BarcodeWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            FillGRNSearchList();
        }
        private static void MessgBoxQuantityAndPendingQuantity()
        {
            MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("", "Quantity Must Not Be Greater Than The Pending Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            msgW1.Show();
        }


        /// <summary>
        /// Cancel button click
        /// </summary>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            this.DataContext = new clsWOGRNVO();
            //if (PreviousGRNType!= null)
            //((clsGRNVO)this.DataContext).GRNType = PreviousGRNType;
            //InitialiseForm();
            _flip.Invoke(RotationType.Backward);

        }

        /// <summary>
        /// Initialize form
        /// </summary>
        private void InitialiseForm()
        {
            rdbCashMode.IsChecked = true;

            rdbDirectPur.IsChecked = true;

            this.DataContext = new clsWOGRNVO();

            GRNAddedItems.Clear();
            GRNPOAddedItems.Clear();
            cmbReceivedBy.SelectedValue = 0;

            cmbSupplier.SelectedValue = ((clsWOGRNVO)this.DataContext).SupplierID;
            cmbSearchSupplier.SelectedValue = ((clsWOGRNVO)this.DataContext).SupplierID;
            cmbStore.SelectedValue = ((clsWOGRNVO)this.DataContext).StoreID;

            GRNAddedItems = new List<clsWOGRNDetailsVO>();
            GRNPOAddedItems = new ObservableCollection<clsWOGRNDetailsVO>();
       //     txtPONO.Text = String.Empty;

            ISEditMode = false;
            chkIsFinalized.IsChecked = false;
            chkConsignment.IsChecked = false;
        }

        /// <summary>
        /// Add item button click
        /// </summary>
        #region Commented
        //private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        //{

        //    if (cmbStore.SelectedItem == null || ((MasterListItem)cmbStore.SelectedItem).ID == 0)
        //    {
        //        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                      new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgW1.Show();


        //    }
        //    else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0 || cmbSupplier.SelectedItem == null)
        //    {
        //        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                      new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgW1.Show();
        //    }
        //    else
        //    {
        //        //POSearchWindow
        //        if (this.ISEditMode == false && GRNAddedItems != null)
        //            GRNAddedItems.Clear();
        //        txtPONO.Text = String.Empty;
        //        PurchaseOrder_SearchWindow POWin = new PurchaseOrder_SearchWindow();
        //        POWin.cboStoreName.IsEnabled = false;
        //        POWin.cboSuppName.IsEnabled = false;
        //        POWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
        //        POWin.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;

        //        POWin.OnSaveButton_Click += new RoutedEventHandler(POWin_OnSaveButton_Click);
        //        POWin.Show();
        //    }

        //}
        #endregion Commented
        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {

          //  if (((clsWOGRNVO)this.DataContext).GRNType == ValueObjects.InventoryGRNType.Direct)
          //  {
          //      //ItemList Itemswin = new ItemList();
          //      //Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
          //      //Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
          //      //Itemswin.ShowBatches = false;
          //      //if (cmbStore.SelectedItem == null || ((MasterListItem)cmbStore.SelectedItem).ID == 0)
          //      //{
          //      //    MessageBoxControl.MessageBoxChildWindow msgW1 =
          //      //                  new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
          //      //    msgW1.Show();


          //      //}
          //      //else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0 || cmbSupplier.SelectedItem == null)
          //      //{
          //      //    MessageBoxControl.MessageBoxChildWindow msgW1 =
          //      //                  new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
          //      //    msgW1.Show();
          //      //}
          //      //else
          //      //{

          //      //    Itemswin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
          //      //    Itemswin.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
          //      //    Itemswin.cmbStore.IsEnabled = false;


          //      //    Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
          //      //    Itemswin.Show();
          //      //}
          //  }
          //  else
          //  //if (((clsGRNVO)this.DataContext).GRNType == ValueObjects.InventoryGRNType.AgainstPO)
          //  {
          ////      txtPONO.Text = String.Empty;
                if (cmbStore.SelectedItem == null || ((MasterListItem)cmbStore.SelectedItem).ID == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0 || cmbSupplier.SelectedItem == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    //POSearchWindow
                    if (this.ISEditMode == false && GRNAddedItems != null)
                        GRNAddedItems.Clear();
                    if (this.ISEditMode == false && GRNPOAddedItems != null)
                        GRNPOAddedItems.Clear();
            //        txtPONO.Text = String.Empty;
                    WorkOrder_SearchWindow POWin = new WorkOrder_SearchWindow();
                    POWin.cboStoreName.IsEnabled = false;
                    POWin.cboSuppName.IsEnabled = false;
                    POWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    POWin.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    POWin.OnSaveButton_Click += new RoutedEventHandler(POWin_OnSaveButton_Click);
                    POWin.Show();

               // }
            }
        }

        /// <summary>
        /// OK button on PO search window click
        /// </summary>
        void POWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrder_SearchWindow Itemswin = (PurchaseOrder_SearchWindow)sender;
            RowCount = 0;
            if (Itemswin.SelectedPO != null && Itemswin.ItemsSelected != null)
            {
                if (rdbDirectPur.IsChecked == true)
                {
                    dgAddGRNItems.Columns[28].Visibility = Visibility.Collapsed;
                }
                else
                {
                    dgAddGRNItems.Columns[28].Visibility = Visibility.Visible;
                }
                if (this.DataContext != null)
                {
                    ((clsWOGRNVO)this.DataContext).WOID = Itemswin.SelectedPO.ID;
                    ((clsWOGRNVO)this.DataContext).WONO = Itemswin.SelectedPO.PONO;
                    ((clsWOGRNVO)this.DataContext).WODate = Itemswin.SelectedPO.Date;
                }

                foreach (var item in Itemswin.ItemsSelected)
                {

                    if (item.PendingQuantity != 0)
                    {
                        clsWOGRNDetailsVO GINItem = new clsWOGRNDetailsVO();
                        GINItem.IsBatchRequired = item.BatchesRequired;
                        GINItem.ItemID = item.ItemID.Value;
                        GINItem.ItemName = item.ItemName;
                        GINItem.Quantity = 0;
                        GINItem.WOQuantity = Convert.ToDouble(item.Quantity);
                        GINItem.Rate = Convert.ToDouble(item.Rate);
                        GINItem.MRP = Convert.ToDouble(item.MRP);
                        GINItem.SchDiscountPercent = Convert.ToDouble(item.DiscPercent);
                        if (GINItem.SchDiscountPercent == 0) GINItem.SchDiscountAmount = Convert.ToDouble(item.DiscountAmount);
                        GINItem.VATPercent = Convert.ToDouble(item.VATPer);
                        if (GINItem.VATPercent == 0) GINItem.VATAmount = Convert.ToDouble(item.VATAmount);

                        GINItem.WOPendingQuantity = Convert.ToDouble(item.PendingQuantity);
                        GINItem.PendingQuantity = Convert.ToDouble(item.PendingQuantity);
                        GINItem.WoItemsID = item.PoItemsID;
                        GINItem.ItemTax = item.ItemTax;
                        GINItem.WODate = item.PODate;
                        GINItem.WOID = item.POID;
                        GINItem.WONO = item.PONO;
                        GINItem.WOUnitID = item.POUnitID;
                        GINItem.WODate = item.PODate;
                        GINItem.PurchaseUOM = item.PurchaseUOM;
                        GINItem.StockUOM = item.StockUOM;
                        GINItem.ConversionFactor = item.ConversionFactor;
                        GINItem.UnitRate = Convert.ToDouble(item.Rate);
                        GINItem.UnitMRP = Convert.ToDouble(item.MRP);
                        GINItem.BarCode = Convert.ToString(item.BarCode);
                        GINItem.ItemCategory = item.ItemCategory;
                        GINItem.ItemGroup = item.ItemGroup;

                        //if (!String.IsNullOrEmpty(txtPONO.Text) && !txtPONO.Text.Trim().Contains(item.PONO.Trim()))
                        //    txtPONO.Text = String.Format(txtPONO.Text + "," + item.PONO.Trim());

                        if (GINItem.IsBatchRequired == false)
                        {
                            GetAvailableQuantityForNonBatchItems(item.ItemID.Value);
                        }
                        GINItem.IndentID = Convert.ToInt64(item.IndentID);
                        GINItem.IndentUnitID = Convert.ToInt64(item.IndentUnitID);

                        var item5 = from r5 in GRNAddedItems
                                    where r5.ItemID == item.ItemID && r5.WOID == item.POID && r5.WOUnitID == item.POUnitID
                                    select r5;

                        var item6 = from r6 in GRNPOAddedItems
                                    where r6.ItemID == item.ItemID && r6.WOID == item.POID && r6.WOUnitID == item.POUnitID
                                    select r6;

                        // Check the list GRNAddedItem and GRNPOAdded Item contain any element.
                        if ((item5 != null && item5.ToList().Count == 0) && (item6 != null && item6.ToList().Count == 0))
                        {
                            if (GRNAddedItems.Count > 0)
                            {
                                var item2 = from r in GRNAddedItems
                                            where r.ItemID == item.ItemID
                                            select r;

                                if (item2 != null && item2.ToList().Count > 0)
                                {
                                    clsWOGRNDetailsVO GRNItem = GRNAddedItems.Where(g => g.ItemID == item.ItemID).FirstOrDefault();

                                    if (GRNItem != null)
                                    {
                                        var item3 = from r1 in GRNPOAddedItems
                                                    where r1.ItemID == item.ItemID && r1.WOID == item.POID && r1.WOUnitID == item.POUnitID
                                                    select r1;

                                        if (item3 != null && item3.ToList().Count == 0)
                                        {
                                            GRNPOAddedItems.Add(GINItem.DeepCopy());
                                        }

                                        GRNItem.WOQuantity += Convert.ToInt64(item.Quantity);
                                        GRNItem.WOPendingQuantity += Convert.ToInt64(item.PendingQuantity);
                                        GRNItem.PendingQuantity += Convert.ToInt64(item.PendingQuantity);

                                    }
                                }
                                else
                                {
                                    GRNAddedItems.Add(GINItem);
                                    var item4 = from r2 in GRNPOAddedItems
                                                where r2.ItemID == item.ItemID && r2.WOID == item.POID && r2.WOUnitID == item.POUnitID
                                                select r2;

                                    if (item4 != null && item4.ToList().Count == 0)
                                    {
                                        GRNPOAddedItems.Add(GINItem.DeepCopy());
                                    }

                                }
                            }
                            else
                            {
                                GRNAddedItems.Add(GINItem);
                                GRNPOAddedItems.Add(GINItem.DeepCopy());
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Item PO combination already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can Not Save Pending Quantity Value as Zero", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        //if (txtPONO.Text.Trim().Contains(item.PONO.Trim()))
                        //    txtPONO.Text = (txtPONO.Text.Replace(item.PONO, string.Empty)).Trim(',');
                    }
                }

                PCVData = new PagedCollectionView(GRNAddedItems);
                PCVData.GroupDescriptions.Add(new PropertyGroupDescription("WONO"));
                List<clsWOGRNDetailsVO> ob = new List<clsWOGRNDetailsVO>();
                ob = (List<clsWOGRNDetailsVO>)(PCVData).SourceCollection;
                dgAddGRNItems.ItemsSource = PCVData;

                dgAddGRNItems.Focus();
                dgAddGRNItems.UpdateLayout();
                CalculateGRNSummary();
                dgAddGRNItems.SelectedIndex = GRNAddedItems.Count - 1;
            }
        }

        MessageBoxControl.MessageBoxChildWindow msgW1 = null;

        /// <summary>
        /// Items Search window ok button click
        /// </summary>
        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;
            RowCount = 0;
            dgAddGRNItems.Columns[5].Visibility = Visibility.Collapsed;
            dgAddGRNItems.Columns[4].Visibility = Visibility.Visible;
            if (Itemswin.SelectedItems != null)
            {
                if (GRNAddedItems == null)
                    GRNAddedItems = new List<clsWOGRNDetailsVO>();

                if (GRNPOAddedItems == null)
                    GRNPOAddedItems = new ObservableCollection<clsWOGRNDetailsVO>();

                foreach (var item in Itemswin.SelectedItems)
                {
                    clsWOGRNDetailsVO grnObj = new clsWOGRNDetailsVO();
                    grnObj.IsBatchRequired = item.BatchesRequired;
                    grnObj.ItemID = item.ID;
                    grnObj.ItemTax = item.TotalPerchaseTaxPercent;
                    grnObj.ItemName = item.ItemName;
                    grnObj.VATPercent = Convert.ToDouble(item.VatPer);
                    grnObj.Rate = Convert.ToDouble(item.PurchaseRate);
                    grnObj.MRP = Convert.ToDouble(item.MRP);
                    grnObj.StockUOM = item.SUOM;
                    grnObj.PurchaseUOM = item.PUOM;
                    grnObj.ConversionFactor = Convert.ToDouble(item.ConversionFactor);
                    if (item.AssignSupplier == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Supplier is not assigned to item", "Do you want to assign supplier for the item:" + item.ItemName, MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Question);
                        msgWin.OnMessageBoxClosed += (re) =>
                        {
                            MessageBoxResult reply = msgWin.Result;

                            if (re == MessageBoxResult.OK)
                            {

                                grnObj.AssignSupplier = true;
                            }
                            else
                                grnObj.AssignSupplier = false;
                        };
                        msgWin.Show();
                    }
                    else
                        grnObj.AssignSupplier = item.AssignSupplier;

                    if (item.BatchesRequired == false)
                    {
                        GetAvailableQuantityForNonBatchItems(item.ID);
                    }
                    var item2 = from r in GRNAddedItems
                                where r.ItemID == item.ID && item.BatchesRequired == false
                                select r;

                    if (item2.ToList().Count > 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Palash", item.ItemName + " already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                    }
                    else
                    {
                        if (item.BatchesRequired == false)
                        {
                            GRNAddedItems.Add(grnObj);
                        }

                    }
                    if (item.BatchesRequired == true)
                    {
                        GRNAddedItems.Add(grnObj);
                    }

                }
                #region tempcommented
                //int grnAddedeItemCnt = GRNAddedItems.Count;
                //#region tempcommented
                //foreach (var item in GRNAddedItems)
                //{
                //    grnAddedeItemCnt++;
                //    clsCheckItemSupplierFromGRNBizActionVO BizAction = new clsCheckItemSupplierFromGRNBizActionVO();
                //    BizAction.ItemID = item.ID;
                //    if (cmbSupplier.SelectedItem != null && ((MasterListItem)cmbSupplier.SelectedItem).ID != 0)
                //        BizAction.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;


                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    Client.ProcessCompleted += (s, arg) =>
                //    {
                //        if (arg.Error == null && arg.Result != null)
                //        {
                //            if (((clsCheckItemSupplierFromGRNBizActionVO)arg.Result).checkSupplier == true)
                //            {
                //                item.AssignSupplier = false;
                //            }
                //            else
                //                item.AssignSupplier = true;



                //        }
                //        if (grnAddedeItemCnt - 1 == GRNAddedItems.Count)
                //        {
                //            dgAddGRNItems.Focus();
                //            dgAddGRNItems.UpdateLayout();
                //            dgAddGRNItems.ItemsSource = null;
                //            dgAddGRNItems.ItemsSource = GRNAddedItems;
                //            dgAddGRNItems.SelectedIndex = GRNAddedItems.Count - 1;
                //        }

                //    };

                //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //    Client.CloseAsync();

                //}
                #endregion

                dgAddGRNItems.ItemsSource = GRNAddedItems;
                dgAddGRNItems.Focus();
                dgAddGRNItems.UpdateLayout();

                dgAddGRNItems.SelectedIndex = GRNAddedItems.Count - 1;

            }
        }


        /// <summary>
        /// GRN Type radio button click
        /// </summary>
        /// 
        public InventoryGRNType PreviousGRNType { get; set; }
        private void rbGRNType_Checked(object sender, RoutedEventArgs e)
        {
            if (IsPageLoaded)
            {
                PreviousGRNType = ((clsWOGRNVO)this.DataContext).GRNType;
                if (rdbDirectPur.IsChecked == true)
                {
                    ((clsWOGRNVO)this.DataContext).GRNType = ValueObjects.InventoryGRNType.Direct;
                    
                     txtPONO.Text = "";
                    BdrItemSearch.Visibility = Visibility.Visible;
                    if (GRNAddedItems != null)
                    {
                        ((clsWOGRNVO)this.DataContext).WOID = 0;
                        ((clsWOGRNVO)this.DataContext).WONO = "";
                        GRNAddedItems.Clear();
                        GRNPOAddedItems.Clear();
                        CalculateGRNSummary();
                    }

                }
                else if (rdbPo.IsChecked == true)
                {
                    ((clsWOGRNVO)this.DataContext).GRNType = ValueObjects.InventoryGRNType.AgainstPO;
                    BdrItemSearch.Visibility = Visibility.Collapsed;
                    if (GRNAddedItems != null)
                    {
                        GRNAddedItems.Clear();
                        GRNPOAddedItems.Clear();
                        CalculateGRNSummary();
                    }
                }
            }

        }

        /// <summary>
        /// radio button payment mode click
        /// </summary>

        private void rbPayMode_Click(object sender, RoutedEventArgs e)
        {
            if (IsPageLoaded)
            {
                if (rdbCashMode.IsChecked == true)
                    ((clsWOGRNVO)this.DataContext).PaymentModeID = ValueObjects.MaterPayModeList.Cash;
                else if (rdbCreditMode.IsChecked == true)
                    ((clsWOGRNVO)this.DataContext).PaymentModeID = ValueObjects.MaterPayModeList.Credit;
            }
        }


        /// <summary>
        /// Calculates GRN summary
        /// </summary>
        private void CalculateGRNSummary()
        {

            double CDiscount, SchDiscount, VATAmount, Amount, NetAmount, ItemTaxAmount;

            CDiscount = SchDiscount = VATAmount = Amount = NetAmount = ItemTaxAmount = 0;

            foreach (var item in GRNAddedItems)
            {
                Amount += item.Amount;
                CDiscount += item.CDiscountAmount;
                SchDiscount += item.SchDiscountAmount;
                VATAmount += item.VATAmount;
                NetAmount += item.NetAmount;
                ItemTaxAmount += item.TaxAmount;



            }

            ((clsWOGRNVO)this.DataContext).TotalAmount = Amount;
            ((clsWOGRNVO)this.DataContext).TotalCDiscount = CDiscount;
            ((clsWOGRNVO)this.DataContext).TotalSchDiscount = SchDiscount;
            ((clsWOGRNVO)this.DataContext).TotalVAT = VATAmount;
            ((clsWOGRNVO)this.DataContext).NetAmount = NetAmount;

            ((clsWOGRNVO)this.DataContext).TotalTAxAmount = ItemTaxAmount;

            txttotcamt.Text = Amount.ToString();
            txtcdiscamt.Text = CDiscount.ToString();
            txthdiscamt.Text = SchDiscount.ToString();
            txttaxamount.Text = ItemTaxAmount.ToString();
            txtVatamt.Text = VATAmount.ToString();
            txtNetamt.Text = NetAmount.ToString();

            //dgAddGRNItems.ItemsSource = null;
            //dgAddGRNItems.ItemsSource = GRNAddedItems;

            dgGRNItems.Focus();
            dgAddGRNItems.UpdateLayout();

            //SetControlForExistingBatch();
            if (_ItemSearchRowControl != null)
                _ItemSearchRowControl.SetFocus();
        }

        private void SetControlForExistingBatch()
        {
            foreach (var item in GRNAddedItems)
            {
                if (((clsWOGRNDetailsVO)item).IsBatchAssign == true)
                {
                    clsWOGRNDetailsVO objRateContractDetails = ((clsWOGRNDetailsVO)item);

                    FrameworkElement fe = dgAddGRNItems.Columns[2].GetCellContent(item);
                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                    if (result != null)
                    {
                        DataGridCell cell = (DataGridCell)result;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column2 = dgAddGRNItems.Columns[3];
                    FrameworkElement fe1 = column2.GetCellContent(item);
                    FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

                    if (result1 != null)
                    {
                        DataGridCell cell = (DataGridCell)result1;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column3 = dgAddGRNItems.Columns[8];
                    FrameworkElement fe2 = column3.GetCellContent(item);
                    FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                    if (result2 != null)
                    {
                        DataGridCell cell = (DataGridCell)result2;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column4 = dgAddGRNItems.Columns[11];
                    FrameworkElement fe4 = column4.GetCellContent(item);
                    FrameworkElement result3 = GetParent(fe4, typeof(DataGridCell));

                    if (result3 != null)
                    {
                        DataGridCell cell = (DataGridCell)result3;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column5 = dgAddGRNItems.Columns[12];
                    FrameworkElement fe5 = column5.GetCellContent(item);
                    FrameworkElement result4 = GetParent(fe5, typeof(DataGridCell));

                    if (result4 != null)
                    {
                        DataGridCell cell = (DataGridCell)result4;
                        cell.IsEnabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// GRN list selection changed.
        /// </summary>
        private void dgGRNList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgGRNList.SelectedItem != null)
            {
                FillGRNDetailslList(((clsWOGRNVO)dgGRNList.SelectedItem).ID);
            }
        }

        /// <summary>
        /// print button click
        /// </summary>
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNList.SelectedItem != null)
            {
                //string Parameters = "";
                long GunitID = 0;
                long ID = 0;
                InventoryGRNType GRNType;
                ID = ((clsWOGRNVO)dgGRNList.SelectedItem).ID;
                GunitID = ((clsWOGRNVO)dgGRNList.SelectedItem).UnitId;
                GRNType = ((clsWOGRNVO)dgGRNList.SelectedItem).GRNType;
                // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

                string URL = "../Reports/InventoryPharmacy/GRNPrint.aspx?ID=" + ID + "&UnitID=" + GunitID + "&GRNType=" + GRNType;

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        /// <summary>
        /// Delete item click
        /// </summary>
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {




                clsWOGRNDetailsVO objSelectedGRNItem = (clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem;
                clsWOGRNDetailsVO objGRNDetails = new clsWOGRNDetailsVO();
                if (dgAddGRNItems.ItemsSource.GetType().Name.Equals("PagedCollectionView"))
                {
                    objGRNDetails = ((List<clsWOGRNDetailsVO>)((PagedCollectionView)(dgAddGRNItems.ItemsSource)).SourceCollection)[dgAddGRNItems.SelectedIndex];
                }
                else
                    objGRNDetails = ((List<clsWOGRNDetailsVO>)(dgAddGRNItems.ItemsSource))[dgAddGRNItems.SelectedIndex];
                double qty = ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity;
                long itemid = ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;

                foreach (clsWOGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID).ToList())
                {
                    item.WOPendingQuantity = item.WOQuantity - item.Quantity;
                }

                GRNAddedItems.RemoveAt(dgAddGRNItems.SelectedIndex);
                dgAddGRNItems.ItemsSource = null;

                foreach (var item in GRNAddedItems.Where(Z => Z.ItemID == itemid).ToList())
                {
                    if (objGRNDetails.GRNID > 0)
                    {
                        item.WOPendingQuantity = item.WOPendingQuantity + qty;
                        item.ItemQuantity += qty;
                    }

                }

                dgAddGRNItems.ItemsSource = GRNAddedItems;
                dgAddGRNItems.UpdateLayout();
                CalculateGRNSummary();
            //    txtPONO.Text = String.Empty;

                //if (rdbPo.IsChecked == true)
                //{
                //    foreach (clsWOGRNDetailsVO objDetail in GRNAddedItems)
                //    {
                //        if (!txtPONO.Text.Contains(objDetail.WONO))
                //            txtPONO.Text = String.Format(txtPONO.Text + "," + objDetail.WONO);
                //    }
                //    txtPONO.Text = txtPONO.Text.Trim(',');
                //}





                //dgAddGRNItems.ItemsSource = GRNAddedItems;
                //dgAddGRNItems.UpdateLayout();
                //CalculateGRNSummary();
                //txtPONO.Text = String.Empty;


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// Store selection changed
        /// </summary>
        /// 
        private long StoreID { get; set; }
        private long SupplierID { get; set; }
        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbStore.SelectedItem != null)
                {

                    if (GRNAddedItems != null)
                    {
                        //txtPONO.Text = String.Empty;
                        GRNAddedItems.Clear();
                        GRNPOAddedItems.Clear();
                        CalculateGRNSummary();
                    }

                    if (((MasterListItem)cmbStore.SelectedItem).ID > 0)
                    {
                        StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                        if (_ItemSearchRowControl != null)
                        {
                            _ItemSearchRowControl.StoreID = StoreID;
                        }
                    }

                    if (StoreID > 0 && SupplierID > 00 && rdbDirectPur.IsChecked == true)
                    {
                        AttachItemSearchControl(StoreID, SupplierID);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Supplier selection changed
        /// </summary>
        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbSupplier.SelectedItem != null)
                {
                    if (GRNAddedItems != null)
                    {
                      //  txtPONO.Text = "";
                        GRNAddedItems.Clear();
                        GRNPOAddedItems.Clear();
                        CalculateGRNSummary();
                    }
                    SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    if (StoreID > 0 && SupplierID > 0 &&
                        rdbDirectPur.IsChecked == true)
                    {
                        AttachItemSearchControl(StoreID, SupplierID);
                    }

                }
                else
                {
                    SupplierID = 0;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Assign Batch click
        /// </summary>

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkIsFinalized.IsChecked == false)
                {
                    if (((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID > 0 && ((MasterListItem)cmbStore.SelectedItem).ID > 0)
                    {
                        cmbStore.ClearValidationError();
                        PalashDynamics.Radiology.ItemSearch.AssignBatch BatchWin = new PalashDynamics.Radiology.ItemSearch.AssignBatch();
                        BatchWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                        BatchWin.SelectedItemID = ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;
                        BatchWin.ItemName = ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemName;

                        BatchWin.ShowZeroStockBatches = true;  // To show zero stock batches to add stock to it using GRN.

                        BatchWin.OnAddButton_Click += new RoutedEventHandler(OnAddBatchButton_Click);
                        BatchWin.Show();
                    }
                    else
                    {
                        if (cmbStore.SelectedItem == null)
                        {
                            cmbStore.TextBox.SetValidation("Please select the store");
                            cmbStore.TextBox.RaiseValidationError();

                        }
                        else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                        {
                            cmbStore.TextBox.SetValidation("Please select the store");
                            cmbStore.TextBox.RaiseValidationError();

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Assign batch ok button click
        /// </summary>
        /// 
        public bool IsBatchAssign = false;
        void OnAddBatchButton_Click(object sender, RoutedEventArgs e)
        {
            PalashDynamics.Radiology.ItemSearch.AssignBatch AssignBatchWin = (PalashDynamics.Radiology.ItemSearch.AssignBatch)sender;
            if (AssignBatchWin.DialogResult == true && AssignBatchWin.SelectedBatches != null)
            {
                foreach (var item in AssignBatchWin.SelectedBatches)
                {
                    if (dgAddGRNItems.ItemsSource != null)
                    {
                        List<clsWOGRNDetailsVO> obclnGRNDetails = new List<clsWOGRNDetailsVO>();

                        if (dgAddGRNItems.ItemsSource.GetType().Name.Equals("PagedCollectionView"))
                        {
                            obclnGRNDetails = (List<clsWOGRNDetailsVO>)((PagedCollectionView)(dgAddGRNItems.ItemsSource)).SourceCollection;
                        }
                        else
                            obclnGRNDetails = ((List<clsWOGRNDetailsVO>)(dgAddGRNItems.ItemsSource));
                        List<clsWOGRNDetailsVO> grnItemList = obclnGRNDetails;
                        foreach (var BatchItems in grnItemList.Where(x => x.ItemID == item.ItemID && x.WONO == ((clsWOGRNDetailsVO)(dgAddGRNItems.SelectedItem)).WONO))
                        {
                            if (BatchItems.BatchID == item.BatchID)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Item with same batch already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                return;
                            }
                        }
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).BatchID = item.BatchID;
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).BatchCode = item.BatchCode;
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).ExpiryDate = item.ExpiryDate;
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).AvailableQuantity = item.AvailableStock;
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = item.MRP;
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate = item.PurchaseRate;
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = item.BarCode;
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).ConversionFactor = item.ConversionFactor;
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment = item.IsConsignment;
                        ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).IsBatchAssign = true;


                        if (((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment == true)
                        {
                            ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment = true;
                            chkConsignment.IsChecked = true;

                        }
                        else
                        {
                            ((clsWOGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment = false;
                            chkConsignment.IsChecked = false;
                        }





                        DataGridColumn column = dgAddGRNItems.Columns[2];

                        FrameworkElement fe = column.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                        if (result != null)
                        {
                            DataGridCell cell = (DataGridCell)result;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column2 = dgAddGRNItems.Columns[3];
                        FrameworkElement fe1 = column2.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

                        if (result1 != null)
                        {
                            DataGridCell cell = (DataGridCell)result1;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column3 = dgAddGRNItems.Columns[9];
                        FrameworkElement fe2 = column3.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                        if (result2 != null)
                        {
                            DataGridCell cell = (DataGridCell)result2;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column4 = dgAddGRNItems.Columns[12];
                        FrameworkElement fe4 = column4.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result3 = GetParent(fe4, typeof(DataGridCell));

                        if (result3 != null)
                        {
                            DataGridCell cell = (DataGridCell)result3;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column5 = dgAddGRNItems.Columns[13];
                        FrameworkElement fe5 = column5.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result4 = GetParent(fe5, typeof(DataGridCell));

                        if (result4 != null)
                        {
                            DataGridCell cell = (DataGridCell)result4;
                            cell.IsEnabled = false;
                        }
                        dgAddGRNItems.UpdateLayout();
                        dgAddGRNItems.Focus();

                        CalculateGRNSummary();

                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = null;
            if (child != null)
            {
                parent = child.Parent;
            }
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;

        }
        private bool ChkBatchExistsOrNot()
        {
            bool result = true;
            List<clsWOGRNDetailsVO> objList = GRNAddedItems.ToList<clsWOGRNDetailsVO>();
            if (objList != null && objList.Count > 0)
            {
                foreach (var item in objList)
                {
                    var chkitem = from r in GRNAddedItems
                                  where r.BatchCode == item.BatchCode && r.ExpiryDate == item.ExpiryDate


                                  select new clsWOGRNDetailsVO
                                  {
                                      Status = r.Status,
                                      ID = r.ID
                                  };

                    if (chkitem.ToList().Count > 1)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Same Batch appears more than once for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        result = false;
                        return result;

                    }
                }
            }
            return result;
        }

        #region Added By Ashish Thombre

        private void cmdAddAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFileAttached)
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                bool? dialogResult = OpenFile.ShowDialog();
                FileInfo Attachment;

                if (dialogResult.Value)
                {
                    try
                    {
                        using (Stream stream = OpenFile.File.OpenRead())
                        {
                            // Only allows file less than 5mb.
                            if (stream.Length < 5120000)
                            {
                                File = new byte[stream.Length];
                                stream.Read(File, 0, (int)stream.Length);
                                Attachment = OpenFile.File;
                                fileName = OpenFile.File.Name;
                                IsFileAttached = true;
                            }
                            else
                            {
                                MessageBox.Show("File must be less than 5 MB");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                MessageBoxChildWindow cw;
                cw = new MessageBoxChildWindow("", "File already attached.", MessageBoxButtons.Ok, MessageBoxIcon.Information);

                cw.Show();
            }
        }

        private void ViewAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNList.SelectedItem != null)
            {
                clsWOGRNVO obj = (clsWOGRNVO)dgGRNList.SelectedItem;
                ViewAttachment(obj.FileName, obj.File);
            }
        }

        private void ViewAttachment(string fileName, byte[] data)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.UploadETemplateFileCompleted += (s, args) =>
            {
                if (args.Error == null)
                {
                    HtmlPage.Window.Invoke("openAttachment", new string[] { fileName });
                }
            };
            client.UploadETemplateFileAsync(fileName, data);
        }

        private void cmdDraft_Click(object sender, RoutedEventArgs e)
        {

            if (GRNAddedItems == null || GRNAddedItems.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "At least one item is compulsory for saving Goods Received Note", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return;
            }

            Boolean blnValid = false;
            int iCount, iItemsCount = 0;
            iCount = GRNAddedItems.Count;
            foreach (clsWOGRNDetailsVO item in GRNAddedItems)
            {
                bool b = ValidateOnSave(item);
                if (b)
                {
                    blnValid = true;
                    iItemsCount++;
                }
                if (!b)
                {
                    return;
                }
            }
            if (blnValid && iItemsCount == iCount)
            {
                chkIsFinalized.IsChecked = false;

                string msgTitle = "Palash";
                string msgText = "";
                if (chkConsignment.IsChecked == false)
                {
                    msgText = "Are you sure you want to Save GRN as Draft?";
                }
                else
                {
                    msgText = "Are you sure you want to Save Consignment GRN ?";
                }

                MessageBoxControl.MessageBoxChildWindow msgWin1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin1_OnMessageBoxClosed);

                msgWin1.Show();
            }

        }

        private void msgWin1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveGRN(true);
            }
            else if (result == MessageBoxResult.No)
                iItemsCount = 0;
        }

        #endregion

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsWOGRNVO obj = (clsWOGRNVO)dgGRNList.SelectedItem;

                if (obj != null)
                {
                    this.DataContext = obj;
                    ((clsWOGRNVO)this.DataContext).GRNNO = obj.GRNNO;
                    ((clsWOGRNVO)this.DataContext).Date = obj.Date;

                    ((clsWOGRNVO)this.DataContext).StoreID = obj.StoreID;
                    ((clsWOGRNVO)this.DataContext).SupplierID = obj.SupplierID;

                    if (cmbStore.ItemsSource != null)
                    {
                        var results = from r in ((List<MasterListItem>)cmbStore.ItemsSource)
                                      where r.ID == obj.StoreID
                                      select r;

                        foreach (MasterListItem item in results)
                        {
                            cmbStore.SelectedItem = item;
                        }
                    }

                    if (cmbSupplier.ItemsSource != null)
                    {
                        var results1 = from r in ((List<MasterListItem>)cmbSupplier.ItemsSource)
                                       where r.ID == obj.SupplierID
                                       select r;

                        foreach (MasterListItem item in results1)
                        {
                            cmbSupplier.SelectedItem = item;
                        }
                    }

                    ((clsWOGRNVO)this.DataContext).ID = obj.ID;
                    ((clsWOGRNVO)this.DataContext).WOID = obj.WOID;

                    ((clsWOGRNVO)this.DataContext).InvoiceNo = obj.InvoiceNo;
                    ((clsWOGRNVO)this.DataContext).InvoiceDate = obj.InvoiceDate;

                    ((clsWOGRNVO)this.DataContext).PaymentModeID = obj.PaymentModeID;

                    if (obj.PaymentModeID == MaterPayModeList.Cash)
                    {
                        rdbCashMode.IsChecked = true;
                    }
                    else if (obj.PaymentModeID == MaterPayModeList.Credit)
                    {
                        rdbCreditMode.IsChecked = true;
                    }

                    if (obj.GRNType == InventoryGRNType.Direct)
                    {
                        rdbDirectPur.IsChecked = true;
                    }
                    else if (obj.GRNType == InventoryGRNType.AgainstPO)
                    {
                        rdbPo.IsChecked = true;
                    }
                    if (obj.IsConsignment == true)
                    {
                        chkConsignment.IsChecked = true;
                        chkConsignment.IsEnabled = false;
                    }
                    else
                    {
                        chkConsignment.IsChecked = false;
                        chkConsignment.IsEnabled = true;
                    }

                    ((clsWOGRNVO)this.DataContext).GatePassNo = obj.GatePassNo;
                    txtGateEntryNo.Text = Convert.ToString(obj.GatePassNo);

                    ((clsWOGRNVO)this.DataContext).InvoiceDate = obj.InvoiceDate;
                    dpInvDt.SelectedDate = obj.InvoiceDate;

                    ((clsWOGRNVO)this.DataContext).InvoiceNo = obj.InvoiceNo;
                    txtinvno.Text = Convert.ToString(obj.InvoiceNo);

                    ((clsWOGRNVO)this.DataContext).TotalAmount = obj.TotalAmount;
                    txttotcamt.Text = obj.TotalAmount.ToString("0.000");

                    ((clsWOGRNVO)this.DataContext).TotalCDiscount = obj.TotalCDiscount;
                    txtcdiscamt.Text = obj.TotalCDiscount.ToString();

                    ((clsWOGRNVO)this.DataContext).TotalSchDiscount = obj.TotalSchDiscount;
                    txthdiscamt.Text = obj.TotalSchDiscount.ToString();

                    ((clsWOGRNVO)this.DataContext).TotalTAxAmount = obj.TotalTAxAmount;
                    txttaxamount.Text = obj.TotalTAxAmount.ToString();

                    ((clsWOGRNVO)this.DataContext).TotalVAT = obj.TotalVAT;
                    txtVatamt.Text = obj.TotalVAT.ToString();

                    ((clsWOGRNVO)this.DataContext).NetAmount = obj.NetAmount;
                    txtNetamt.Text = obj.NetAmount.ToString();

                    ((clsWOGRNVO)this.DataContext).Freezed = obj.Freezed;
                    chkIsFinalized.IsChecked = obj.Freezed;

                    ((clsWOGRNVO)this.DataContext).Remarks = obj.Remarks;
                    txtremarks.Text = Convert.ToString(obj.Remarks);

                    if (cmbReceivedBy.ItemsSource != null)
                    {
                        var results2 = from r in ((List<MasterListItem>)cmbReceivedBy.ItemsSource)
                                       where r.ID == obj.ReceivedByID
                                       select r;

                        ((MasterListItem)cmbReceivedBy.SelectedItem).ID = obj.ReceivedByID;
                    }
                    ISEditMode = true;

                    //FillGRNAddDetailslList(((clsGRNVO)dgGRNList.SelectedItem).ID, ((clsGRNVO)dgGRNList.SelectedItem).Freezed, ((clsGRNVO)dgGRNList.SelectedItem).UnitId);
                    FillGRNItems(((clsWOGRNVO)dgGRNList.SelectedItem).ID, ((clsWOGRNVO)dgGRNList.SelectedItem).UnitId);
                    dgAddGRNItems.UpdateLayout();
                    dgAddGRNItems.Focus();

                    chkIsFinalized.IsChecked = ((clsWOGRNVO)dgGRNList.SelectedItem).Freezed;
                  //  txtPONO.Text = String.Empty;
                    foreach (clsWOGRNDetailsVO item in ((List<clsWOGRNDetailsVO>)(dgGRNItems.ItemsSource)))
                    {
                        //if (!txtPONO.Text.Trim().Contains(item.WONO))
                        //    txtPONO.Text = String.Format((txtPONO.Text.Trim() + "," + item.WONO.Trim()).Trim(','));
                    }
                    SetCommandButtonState("Modify");

                    if (((clsWOGRNVO)dgGRNList.SelectedItem).Freezed == true)
                    {
                        cmdSave.IsEnabled = false;
                        cmdDraft.IsEnabled = false;
                    }
                    else
                    {
                        cmdSave.IsEnabled = true;
                        cmdDraft.IsEnabled = true;
                    }

                    _flip.Invoke(RotationType.Forward);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillGRNItems(long pGRNID, long UnitId)
        {
            clsGetWOGRNDetailsListByIDBizActionVO BizAction = new clsGetWOGRNDetailsListByIDBizActionVO();
            BizAction.GRNID = pGRNID;
            BizAction.UnitId = UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsWOGRNDetailsVO> objList = new List<clsWOGRNDetailsVO>();
                    objList = new List<clsWOGRNDetailsVO>();
                    objList = ((clsGetWOGRNDetailsListByIDBizActionVO)e.Result).List;
                    GRNAddedItems.Clear();

                    foreach (var item in objList)
                    {
                        item.ItemQuantity = item.WOPendingQuantity + item.Quantity;

                        GRNAddedItems.Add(item);
                    }

                    CalculateGRNSummary();
                    dgAddGRNItems.ItemsSource = null;
                    dgAddGRNItems.ItemsSource = GRNAddedItems;
                    dgAddGRNItems.UpdateLayout();
                    dgAddGRNItems.Focus();
                    foreach (clsWOGRNDetailsVO item in dgAddGRNItems.ItemsSource)
                    {
                        if (item.IsBatchRequired == false)
                        {
                            DataGridColumn column = dgAddGRNItems.Columns[2];
                            FrameworkElement fe = column.GetCellContent(item);
                            FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                            if (result != null)
                            {
                                DataGridCell cell = (DataGridCell)result;
                                cell.IsEnabled = false;
                            }
                        }
                    }


                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillGRNAddDetailslList(long pGRNID, bool Freezed, long UnitId)
        {
            clsGetWOGRNDetailsListBizActionVO BizAction = new clsGetWOGRNDetailsListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.GRNID = pGRNID;
            BizAction.UnitId = UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsWOGRNDetailsVO> objList = new List<clsWOGRNDetailsVO>();

                    objList = ((clsGetWOGRNDetailsListBizActionVO)e.Result).List;
                    GRNAddedItems.Clear();

                    int iGRN = 0;
                    double TotalQty = 0;

                    foreach (var item in objList)
                    {
                        iGRN = ((clsGetWOGRNDetailsListBizActionVO)e.Result).POGRNList.Where(z => z.ItemID == item.ItemID).Count();

                        if (iGRN != null && iGRN == 1 && item.PendingQuantity > 0)
                        {
                            item.ItemQuantity = item.WOPendingQuantity + item.Quantity;
                        }
                        else
                        {
                            TotalQty = ((clsGetWOGRNDetailsListBizActionVO)e.Result).POGRNList.Where(z => z.ItemID == item.ItemID).ToList().Select(z => z.PendingQuantity).Sum();
                            Double dGrnQty = objList.Where(z => z.ItemID == item.ItemID && z.WOID == item.WOID && z.WOUnitID == item.WOUnitID).ToList().Select(z => z.Quantity).Sum();
                            item.PendingQuantity = TotalQty;
                            item.WOPendingQuantity = TotalQty - dGrnQty;


                            item.ItemQuantity = item.WOPendingQuantity + item.Quantity;
                        }

                        GRNAddedItems.Add(item);
                    }

                    lstGRNDetailsDeepCopy = GRNAddedItems.ToList().DeepCopy();
                    GRNPOAddedItems.Clear();
                    GRNPOAddedItems = new ObservableCollection<clsWOGRNDetailsVO>();

                    List<clsWOGRNDetailsVO> objList2 = new List<clsWOGRNDetailsVO>();
                    objList2 = ((clsGetWOGRNDetailsListBizActionVO)e.Result).POGRNList;

                    foreach (var item in objList2)
                    {
                        GRNPOAddedItems.Add(item);
                    }

                    dgAddGRNItems.ItemsSource = null;
                    dgAddGRNItems.ItemsSource = GRNAddedItems;

                    CalculateGRNSummary();
                    foreach (var item2 in GRNAddedItems)
                    {
                        CheckBatch(item2);
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void CheckBatch(clsWOGRNDetailsVO item)
        {
            if (GRNAddedItems != null && GRNAddedItems.Count > 0)
            {

                WaitIndicator In = new WaitIndicator();
                In.Show();
                clsGetItemStockBizActionVO BizAction = new clsGetItemStockBizActionVO();
                BizAction.BatchList = new List<clsItemStockVO>();
                if (cmbStore.SelectedItem != null)
                    BizAction.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                BizAction.ItemID = item.ItemID;
                BizAction.ShowZeroStockBatches = true;

                BizAction.IsPagingEnabled = true;
                BizAction.MaximumRows = 100000;
                BizAction.StartIndex = DataList.PageIndex * 100000;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsGetItemStockBizActionVO result = ea.Result as clsGetItemStockBizActionVO;
                        foreach (var item2 in result.BatchList)
                        {
                            if (item2.BatchCode == item.BatchCode)
                            {
                                item.IsBatchAssign = true;
                                item.AvailableQuantity = item2.AvailableStock;
                                SetControlForExistingBatch();
                                In.Close();
                            }

                        }
                        In.Close();
                    }
                    else
                    {
                        In.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

        }

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNItems.SelectedItem != null)
            {
                WOBarcodeForm win = new WOBarcodeForm();
                string date;
                long ItemID = 0;
                string BatchID = null;
                string BatchCode = null;
                string ItemCode = null;
                // ItemID = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemID;
                string ItemName = ((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).ItemName;

                if (dgGRNItems.SelectedItem != null)
                {
                    if (((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate != null)
                    {
                        date = ((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate.Value.ToShortDateString();
                    }
                    else
                        date = null;
                }
                else
                {
                    date = null;
                }
                if (dgGRNItems.SelectedItem != null)
                {
                    ItemID = ((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).ItemID;
                    if (((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).BatchCode != null)
                    {
                        string str = ((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).BatchCode;
                        BatchID = Convert.ToString(((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).BatchID);
                        BatchCode = str.Substring(0, 3) + "/" + BatchID.ToString() + "B";
                    }
                    else
                    {
                        BatchID = ((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).BatchID + "BI";
                    }
                }

                if (BatchCode == null && BatchID == null)
                {
                    ItemID = ((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).ID;
                    string str1 = ((clsWOGRNDetailsVO)dgGRNItems.SelectedItem).ItemCode;
                    ItemCode = str1.Substring(0, 4) + "I";
                }


                if (BatchCode != null)
                    win.PrintData = "*" + ItemID.ToString() + "-" + BatchCode + "*";
                else
                {
                    if (BatchCode == null && BatchID != null)
                    {
                        win.PrintData = "*" + ItemID.ToString() + "-" + BatchID + "*";
                    }
                    else
                    {
                        win.PrintData = "*" + ItemID.ToString() + "-" + ItemCode + "*";
                    }
                }
                win.PrintItem = ItemName;
                win.PrintDate = date;

                win.PrintFrom = "GRN";
              win.WOGRNItemDetails = (clsWOGRNDetailsVO)dgGRNItems.SelectedItem;

                win.Show();
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select Item.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
            }
        }

        public PagedSortableCollectionView<clsItemStockVO> BatchList { get; private set; }
        int RowCount = 0;
        AutoCompleteBox txtBatchCode;

        private void DataGridColumnVisibility()
        {
            Boolean blnReadOnly = false;
            if (chkIsFinalized.IsChecked == true)
            {
                blnReadOnly = true;
            }
            dgAddGRNItems.Columns[2].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[3].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[4].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[6].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[7].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[8].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[12].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[13].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[15].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[26].IsReadOnly = blnReadOnly;
            if (txtBatchCode != null) txtBatchCode.IsEnabled = blnReadOnly;
        }

        private void GetBatches(long ItemID, AutoCompleteBox CmbBatchCode)
        {
            BatchList = new PagedSortableCollectionView<clsItemStockVO>();
            clsGetItemStockBizActionVO BizAction = new clsGetItemStockBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();
            BizAction.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
            BizAction.ItemID = ItemID;
            BizAction.ShowExpiredBatches = false;
            BizAction.ShowZeroStockBatches = true;
            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = DataList.PageSize;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetItemStockBizActionVO result = ea.Result as clsGetItemStockBizActionVO;

                    BatchList.Clear();
                    //BatchList.TotalItemCount = result.TotalRows;
                    List<MasterListItem> ObjList = new List<MasterListItem>();
                    foreach (clsItemStockVO item in result.BatchList)
                    {
                        ObjList.Add(new MasterListItem() { ID = item.BatchID, Description = item.BatchCode });
                        BatchList.Add(item);
                    }
                    if (ObjList.Count > 0)
                    {
                        CmbBatchCode.ItemsSource = null;
                        CmbBatchCode.ItemsSource = ObjList;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        void txtBatchCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //List<clsItemStockVO> ItemBatchDetailList = new List<clsItemStockVO>();
            //if (((AutoCompleteBox)sender).SelectedItem != null)
            //{
            //    string BatchCode = ((AutoCompleteBox)sender).SelectedItem.ToString();
            //    //List<clsItemStockVO> ItemBatchDetailList = new List<clsItemStockVO>();

            //    foreach (var item in BatchList)
            //    {
            //        if (dgAddGRNItems.SelectedItem != null && item.ItemID == ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID)
            //        {
            //            ItemBatchDetailList.Add(item);
            //        }
            //    }
            //    foreach (var batch in ItemBatchDetailList)
            //    {
            //        if (dgAddGRNItems.ItemsSource != null)
            //        {
            //            if (batch.ItemID == ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID && batch.BatchCode == BatchCode)
            //            {
            //                List<clsGRNDetailsVO> lstGRNItems = GRNAddedItems.ToList();
            //                int iDuplicateBatchCode = (from r in GRNAddedItems
            //                                           where r.BatchCode == batch.BatchCode
            //                                           && r.PONO != ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).PONO
            //                                           select new clsGRNDetailsVO
            //                                           {
            //                                               Status = r.Status,
            //                                               ID = r.ID
            //                                           }).ToList().Count();
            //                foreach (DataGridColumn dgColumn in dgAddGRNItems.Columns)
            //                {
            //                    dgColumn.IsReadOnly = true;
            //                }
            //                dgAddGRNItems.Columns[6].IsReadOnly = false;
            //                dgAddGRNItems.Columns[2].IsReadOnly = false;
            //                dgAddGRNItems.Columns[7].IsReadOnly = false;
            //                dgAddGRNItems.Columns[26].IsReadOnly = false;
            //                if (iDuplicateBatchCode > 1)
            //                {
            //                    (((AutoCompleteBox)sender).SelectedItem) = String.Empty;
            //                    MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                                                           new MessageBoxControl.MessageBoxChildWindow("", "Same batch already exists!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //                    msgW3.Show();
            //                    break;
            //                }
            //                else
            //                {
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BatchID = batch.BatchID;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BatchCode = batch.BatchCode;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ExpiryDate = batch.ExpiryDate;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).AvailableQuantity = batch.AvailableStock;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = batch.MRP;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate = batch.PurchaseRate;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = batch.BarCode;

            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                //foreach (DataGridColumn dgColumn in dgAddGRNItems.Columns)
            //                //{
            //                //    dgColumn.IsReadOnly = false;
            //                //}
            //                //dgAddGRNItems.Columns[4].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[5].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[9].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[10].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[13].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[18].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[19].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[22].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[24].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[25].IsReadOnly = true;

            //            }
            //        }
            //        else
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW1.Show();
            //        }
            //    }
            //}
        }

        clsWOGRNDetailsVO objBatchCode;
        private void GetAvailableQuantityForNonBatchItems(long itemID)
        {
            clsGetItemCurrentStockListBizActionVO BizAction = new clsGetItemCurrentStockListBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();
            BizAction.ItemID = itemID;
            if (cmbStore.SelectedItem != null && ((MasterListItem)cmbStore.SelectedItem).ID != 0)
            {
                BizAction.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetItemCurrentStockListBizActionVO)arg.Result != null)
                    {
                        clsGetItemCurrentStockListBizActionVO result = arg.Result as clsGetItemCurrentStockListBizActionVO;


                        foreach (var item in result.BatchList)
                        {
                            foreach (var item1 in GRNAddedItems)
                            {
                                if (item.ItemID == item1.ItemID)
                                {
                                    item1.AvailableQuantity = item.AvailableStock;
                                }
                            }
                        }

                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            clsWOGRNDetailsVO obj = (dgAddGRNItems.SelectedItem) as clsWOGRNDetailsVO;
            clsWOGRNDetailsVO objGRNDetails = obj.DeepCopy();
            string strMessage = String.Empty;
            if (obj != null && obj.IsBatchRequired == true)
            {
                if (GRNAddedItems.Where(z => z.ItemID == obj.ItemID && z.Quantity != null && z.Quantity == 0).Any())
                {
                    strMessage = "Please enter the Quantity first.";
                }
                else if (objGRNDetails != null && objGRNDetails.WOPendingQuantity <= 0)
                {
                    strMessage = "Cannot add item cause pending quantity is zero.";
                }
                else
                {
                    List<clsWOGRNDetailsVO> lstDetails = GRNAddedItems.Where(z => z.ItemID == obj.ItemID).ToList();
                    if (lstDetails.Count > 1)
                    {
                        double item = (from grndetail in lstDetails
                                       where grndetail.ItemID == obj.ItemID
                                       orderby grndetail.WOPendingQuantity ascending
                                       select grndetail.WOPendingQuantity).First();
                        if (item <= 0)
                        {
                            strMessage = "Cannot add item cause pending quantity is zero.";
                        }
                    }
                }
                if (String.IsNullOrEmpty(strMessage))
                {
                    List<clsWOGRNDetailsVO> lstDetails = GRNAddedItems.Where(z => z.ItemID == obj.ItemID).ToList();
                    if (lstDetails.Count > 1)
                    {
                        double item = (from grndetail in lstDetails
                                       where grndetail.ItemID == obj.ItemID
                                       orderby grndetail.WOPendingQuantity ascending
                                       select grndetail.WOPendingQuantity).First();
                        //objGRNDetails.PendingQuantity = item;
                        objGRNDetails.WOPendingQuantity = item;

                    }
                    else
                        objGRNDetails.PendingQuantity = obj.PendingQuantity;
                    objGRNDetails.Quantity = 0;
                    objGRNDetails.BatchID = 0;
                    objGRNDetails.BatchCode = string.Empty;
                    objGRNDetails.ExpiryDate = null;
                    objGRNDetails.IsBatchAssign = false;
                    objGRNDetails.FreeQuantity = 0;
                    objGRNDetails.AvailableQuantity = 0;
                    GRNAddedItems.Add(objGRNDetails);
                    //if (!GRNAddedItems.Where(z => z.ItemID == objGRNDetails.ItemID && z.POID == objGRNDetails.POID && z.POUnitID == objGRNDetails.POUnitID).Any())
                    //    GRNPOAddedItems.Add(objGRNDetails);
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.None);
                    msgWindow.Show();
                }

            }
        }

        private void chkConsignment_Click(object sender, RoutedEventArgs e)
        {
            if (chkConsignment.IsChecked == false)
            {
                if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                {
                    if (GRNAddedItems.Where(z => z.IsConsignment).Any() == true)
                    {
                        GRNAddedItems = new List<clsWOGRNDetailsVO>();
                        dgAddGRNItems.ItemsSource = GRNAddedItems;

                    }
                }
            }
            else
            {
                if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                {
                    if (GRNAddedItems.Where(z => z.IsBatchAssign).Any() == true)
                    {
                        if (GRNAddedItems.Where(z => z.IsConsignment).Any() == false)
                        {
                            GRNAddedItems = new List<clsWOGRNDetailsVO>();
                            dgAddGRNItems.ItemsSource = GRNAddedItems;

                        }
                    }
                }
            }
        }

        private void dtpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetGRNNumber();
        }

        private void GetGRNNumber()
        {
            clsGetAllGRNNumberBetweenDateRangeBizActionVO BizActionObject = new clsGetAllGRNNumberBetweenDateRangeBizActionVO();
            BizActionObject.ItemList = new List<clsItemMasterVO>();
            BizActionObject.MasterList = new List<MasterListItem>();
            if (cmbSearchSupplier.SelectedItem != null)
                BizActionObject.SupplierID = ((MasterListItem)cmbSearchSupplier.SelectedItem).ID;
            else
                BizActionObject.SupplierID = 0;
            BizActionObject.FromDate = dtpFromDate.SelectedDate;
            BizActionObject.ToDate = dtpToDate.SelectedDate;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetAllGRNNumberBetweenDateRangeBizActionVO result = ea.Result as clsGetAllGRNNumberBetweenDateRangeBizActionVO;

                    txtGRNNo.ItemsSource = null;
                    txtGRNNo.ItemsSource = result.MasterList;

                }

            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void dtpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetGRNNumber();
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        MasterListItem SelectedItem1 = new MasterListItem();
        private void cmbItemName_KeyUp(object sender, KeyEventArgs e)
        {
            SelectedItem1 = new MasterListItem();
            SelectedItem1 = ((MasterListItem)txtGRNNo.SelectedItem);

        }

        private void cmbSearchSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetGRNNumber();
        }

        private void dgAddGRNItems_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
            {
                PreviousBatchValue = ((clsWOGRNDetailsVO)((DataGrid)sender).SelectedItem).BatchCode;
            }
        }

        //private void txtBatchCode_TextChanged(object sender, RoutedEventArgs e)
        //{
        //    AutoCompleteBox tb = (AutoCompleteBox)sender;

        //    string BatchCode;
        //    BatchCode = tb.Text;
        //    //if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem) != null)
        //    //{
        //    //    //ItemID = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;

        //    //    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = BatchCode;
        //    //    if (BatchCode == "" || BatchCode == null)
        //    //        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = null;
        //    //}
        //    foreach (clsGRNDetailsVO item in (List<clsGRNDetailsVO>)dgAddGRNItems.ItemsSource)
        //    {
        //        if (item.BatchCode == BatchCode)
        //        {
        //            item.BarCode = BatchCode;
        //            break;
        //        }
        //    }
        //}

        private void txtBatchCode_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox tb = (AutoCompleteBox)sender;

            string BatchCode;
            BatchCode = tb.Text;
            //if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem) != null)
            //{
            //    //ItemID = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;

            //    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = BatchCode;
            //    if (BatchCode == "" || BatchCode == null)
            //        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = null;
            //}

            //if (PreviousBatchValue != BatchCode)
            //{
            //    foreach (clsGRNDetailsVO item in (List<clsGRNDetailsVO>)dgAddGRNItems.ItemsSource)
            //    {

            //        if (item.BatchCode == BatchCode)
            //        {
            //            item.BarCode = BatchCode;
            //            break;
            //        }
            //    }
            //}
        }



    }
}
