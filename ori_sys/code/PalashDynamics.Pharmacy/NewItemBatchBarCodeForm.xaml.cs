using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Pharmacy.ViewModels;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using MessageBoxControl;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.ComponentModel;

namespace PalashDynamics.Pharmacy
{
    public partial class NewItemBatchBarCodeForm : UserControl
    {

        #region Properties
        /// <summary>
        /// Gets or sets the data list.
        /// </summary>
        /// <value>The data list to data bind ItemsSource.</value>
        public PagedSortableCollectionView<clsItemMasterVO> ItemList1 { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return ItemList1.PageSize;
            }
            set
            {
                if (value == ItemList1.PageSize) return;
                ItemList1.PageSize = value;
                //RaisePropertyChanged("PageSize");
            }
        }
        #endregion

  
        //public ObservableCollection<clsItemMasterVO> ItemList { get; set; }
        //public PagedSortableCollectionView<clsItemMasterVO> ItemList1 { get; private set; }
       
        
      
        public int ItemPageSize { get; set; }
        public bool ShowItems { get; set; }

        private ObservableCollection<clsItemMasterVO> _ItemSelected;
        public ObservableCollection<clsItemMasterVO> SelectedItems { get { return _ItemSelected; } }
           
        public NewItemBatchBarCodeForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PrintItemBarCode_Loaded);
            this.DataContext = new clsItemMasterVO();

            ItemList1 = new PagedSortableCollectionView<clsItemMasterVO>();
            ItemList1.OnRefresh += new EventHandler<RefreshEventArgs>(ItemList1_OnRefresh);
            PageSize = 15;
            ShowItems = true;
        }
        void PrintItemBarCode_Loaded(object sender, RoutedEventArgs e)
        {
            FillItemList();
        }
        void ItemList1_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillItemList();
        }


        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShowItems == true)
            {
                if (_ItemSelected == null)
                    _ItemSelected = new ObservableCollection<clsItemMasterVO>();

                _ItemSelected.Clear();
                _ItemSelected.Add((clsItemMasterVO)dataGrid2.SelectedItem);
            }
               
          
        }


        private void CmdAssign_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

                if (_ItemSelected == null)
                    isValid = false;
                else if (_ItemSelected.Count == 0)
                    isValid = false;


            if (isValid)
            {
                assignBarcode();
               
            }
            else
            {
                string msgText = "";
                msgText = "Please select Item.";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
            }

          
 
        }

        void Assign_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmAssignBarcode)sender).DialogResult == true)
            {
                ((clsItemMasterVO)dataGrid2.SelectedItem).BarCode = ((frmAssignBarcode)sender).txtBarCode.Text;
            }
        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {

            if (dataGrid2.SelectedItem != null)
            {
               //// BarcodeForm win = new BarcodeForm();
               ////// BarcodeForm_N win = new BarcodeForm_N();  //Using Code128
               //// string date,MRP;
               //// date = ((clsItemMasterVO)dataGrid2.SelectedItem).ExpiryDate.ToShortDateString();
               
               ////  //MRP=((clsItemMasterVO)dataGrid2.SelectedItem).MRP.ToString();
               ////  MRP = String.Format("{0:0.000}", ((clsItemMasterVO)dataGrid2.SelectedItem).MRP);
               //// String strBarCode = ((clsItemMasterVO)dataGrid2.SelectedItem).BarCode;
               //// if (!String.IsNullOrEmpty(strBarCode))
               //// {
               ////     win.PrintData = strBarCode.ToUpper();
               ////     win.PrintItem = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemName;
               ////     win.PrintDate = date;
               ////     win.printMRP = MRP;
               ////     win.Show();
               //// }
               //// else
               //// {
               ////     MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please enter barcode for the item : " + ((clsItemMasterVO)dataGrid2.SelectedItem).ItemName, MessageBoxButtons.Ok, MessageBoxIcon.Information);
               ////     msgbox.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgbox_OnMessageBoxClosed);
               ////     msgbox.Show();
               //// }
                if (!String.IsNullOrEmpty(((clsItemMasterVO)dataGrid2.SelectedItem).BarCode))
                {
                    frmNewBarcodeCode128Form win = new frmNewBarcodeCode128Form();
                    win.PrintData = ((clsItemMasterVO)dataGrid2.SelectedItem).BarCode;
                    win.PrintItem = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemName;
                    win.PrintDate = ((clsItemMasterVO)dataGrid2.SelectedItem).ExpiryDate.ToShortDateString();
                    win.PrintBarcode = ((clsItemMasterVO)dataGrid2.SelectedItem).BarCode;
                    win.printMRP = String.Format("{0:0.000}", ((clsItemMasterVO)dataGrid2.SelectedItem).MRP);
                   
                    if (win.PrintData.Length > 20)
                        win.PrintData = win.PrintData.Substring(0, 20);
                    win.OnCancelButton_Click += new RoutedEventHandler(BarcodeWin_OnCancelButton_Click);// added by hrishikesh 11 Oct 2014 - 15 to 20
                    win.Show();
                }
                else
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please enter barcode for the item : " + ((clsItemMasterVO)dataGrid2.SelectedItem).ItemName, MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgbox_OnMessageBoxClosed);
                    msgbox.Show();
                }
                //win.Loaded += new RoutedEventHandler(win_Loaded);
                //win.Closed += new EventHandler(win_Closed);
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select Item.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
            }
        }
        void BarcodeWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ((frmNewBarcodeCode128Form)sender).DialogResult = true;
            FillItemList();
        }


        void msgbox_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                assignBarcode();
            }
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement MyData = new InventoryDashBoard();
            ((IApplicationConfiguration)App.Current).OpenMainContent(MyData);
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;
            if (res)
            {
                FillItemList();
            }
        }

        private void txtItemName_LostFocus(object sender, RoutedEventArgs e)
        {

        }
      
        private void FillItemList()
        {
            WaitIndicator IndicatiorGet = new WaitIndicator();
            try
            {
                IndicatiorGet.Show();
                clsGetItemListForNewItemBatchMasterBizActionVO BizAction = new clsGetItemListForNewItemBatchMasterBizActionVO();
                BizAction.IsPagingEnabled = true;
                BizAction.MaximumRows = ItemList1.PageSize;
                BizAction.StartIndex = ItemList1.PageIndex*ItemList1.PageSize;
                BizAction.ItemList = new List<clsItemMasterVO>();
             

                if (txtItemName.Text != "" )
                {
                    BizAction.ItemName = txtItemName.Text;  
                }
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
               
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    IndicatiorGet.Close();

                    if ((clsGetItemListForNewItemBatchMasterBizActionVO)arg.Result != null)
                    {

                       
                        ItemList1.TotalItemCount = (int)(((clsGetItemListForNewItemBatchMasterBizActionVO)arg.Result).TotalRows);
                        ItemList1.Clear();
                        if (((clsGetItemListForNewItemBatchMasterBizActionVO)arg.Result).ItemList != null)
                        {
                            foreach (var item in ((clsGetItemListForNewItemBatchMasterBizActionVO)arg.Result).ItemList)
                            {
                                ItemList1.Add(item);
                            }
                        }
                     
                       
                        //dataGrid2.UpdateLayout();

                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = ItemList1;
                        dataGrid2.SelectedIndex = -1; 

                        dataGrid2Pager.Source = null;
                        dataGrid2Pager.PageSize = BizAction.MaximumRows;
                        dataGrid2Pager.Source = ItemList1;

                        IndicatiorGet.Close();
                    }
                    else
                    {
                        IndicatiorGet.Close();
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palash", "Error while processing.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                IndicatiorGet.Close();
            }
        }

        private void assignBarcode()
        {
            frmAssignBarcode win = new frmAssignBarcode();
            win.ItemName = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemName;
            win.BatchCode = ((clsItemMasterVO)dataGrid2.SelectedItem).BatchCode;
            win.BarCode = ((clsItemMasterVO)dataGrid2.SelectedItem).BarCode;
            win.MRP = String.Format("{0:0.000}", ((clsItemMasterVO)dataGrid2.SelectedItem).MRP);
            win.date = ((clsItemMasterVO)dataGrid2.SelectedItem).ExpiryDate.ToShortDateString(); 
            win.OnAddButton_Click += new RoutedEventHandler(Assign_OnAddButton_Click);
            win.Show();
        }

    }
}
