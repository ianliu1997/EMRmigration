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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Inventory.Quotation;
using System.Collections.ObjectModel;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Inventory;
using System.Reflection;
using System.Windows.Browser;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class QuotationComparison : ChildWindow
    {
        public ObservableCollection<clsQuotaionVO> QuotationItems { get; set; }
        WaitIndicator Indicatior;
        public PagedSortableCollectionView<clsQuotaionVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsItemMasterVO> ItemList { get; private set; }
        public QuotationComparison()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsQuotaionVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 10;
            if (QuotationItems == null)
                QuotationItems = new ObservableCollection<clsQuotaionVO>();
        }

        #region 'Paging'
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
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillQuotationDataGrid();
        }

        #endregion

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillStores(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
            FillSupplier();
        }

        public void FillSupplier()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Supplier;
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
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    lstboxSupplier.ItemsSource = null;
                    lstboxSupplier.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                    objList.Insert(0, new MasterListItem(0, "-- Select --"));
                    lstboxSupplier.ItemsSource = null;
                    lstboxSupplier.ItemsSource = objList;
                    lstboxSupplier.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                FillQuotationDataGrid();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillQuotationDataGrid()
        {
            //    try
            //    {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            DataList.Clear();
            clsGetQuotationBizActionVO objBizActionVO = new clsGetQuotationBizActionVO();
            // objBizActionVO.Quotation = new clsQuotaionVO();
            //if (cmbSearchStore.SelectedItem != null)
            //{
            //    objBizActionVO.SearchStoreID = ((clsStoreVO)cmbSearchStore.SelectedItem).StoreId;
            //}
            objBizActionVO.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            if (((MasterListItem)lstboxSupplier.SelectedItem) != null && ((MasterListItem)lstboxSupplier.SelectedItem).ID != 0)
            {
                string SupplierIDs = "";
                foreach (MasterListItem items in lstboxSupplier.SelectedItems)
                {
                    SupplierIDs = SupplierIDs + items.ID;
                    SupplierIDs = SupplierIDs + ",";
                }
                if (SupplierIDs.EndsWith(","))
                    objBizActionVO.SupplierIDs = SupplierIDs.Remove(SupplierIDs.Length - 1, 1);
            }

            else
            {
                objBizActionVO.SupplierIDs = "";
            }

            if (((clsItemMasterVO)lstboxItems.SelectedItem) != null && ((clsItemMasterVO)lstboxItems.SelectedItem).ID != 0)
            {
                string ItemIDs = "";
                foreach (clsItemMasterVO items in lstboxItems.SelectedItems)
                {
                    ItemIDs = ItemIDs + items.ID;
                    ItemIDs = ItemIDs + ",";
                }
                if (ItemIDs.EndsWith(","))
                    objBizActionVO.ItemIDs = ItemIDs.Remove(ItemIDs.Length - 1, 1);
            }
            else
            {
                objBizActionVO.ItemIDs = "";
            }
            //if (dtpFromDate.SelectedDate != null)
            //    objBizActionVO.SearchFromDate = dtpFromDate.SelectedDate.Value;
            //if (dtpToDate.SelectedDate != null)
            //    objBizActionVO.SearchToDate = dtpToDate.SelectedDate.Value.AddDays(1);
            objBizActionVO.IsPagingEnabled = false;
            objBizActionVO.StartIndex = DataList.PageIndex * DataList.PageSize;
            objBizActionVO.MaxRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        clsGetQuotationBizActionVO obj = ((clsGetQuotationBizActionVO)arg.Result);

                        if (obj.QuotaionList != null)
                        {
                            DataList.TotalItemCount = obj.TotalRowCount;
                            DataList.Clear();
                            foreach (var item in obj.QuotaionList)
                            {
                                DataList.Add(item);
                            }

                            dgQuotationList.ItemsSource = null;
                            dgQuotationList.ItemsSource = DataList;

                            datapager.Source = null;
                            datapager.PageSize = objBizActionVO.MaxRows;
                            datapager.Source = DataList;
                        }
                        //dgQuotationList.ItemsSource = null;
                        //dgQuotationList.ItemsSource = obj.QuotaionList;
                        Indicatior.Close();
                    }
                    lstboxItems.SelectedIndex=-1;
                    lstboxSupplier.SelectedIndex = -1;
                    objBizActionVO.ItemIDs = "";
                    objBizActionVO.SupplierIDs = "";
                }
                else
                {
                    //Indicatior.Close();
                    //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Opening Balance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                Indicatior.Close();
            };

            Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillQuotationDataGrid();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgQuotationList.SelectedItem != null)
            {

                //if (QuotationItems == null)
                //    QuotationItems = new ObservableCollection<clsQuotaionVO>();
                
                    CheckBox chk = (CheckBox)sender;
                    if (chk.IsChecked == true)
                        if (QuotationItems.Count != 3)
                        {
                            QuotationItems.Add((clsQuotaionVO)dgQuotationList.SelectedItem);
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "You Can Select Only Three Quotation For Comparision.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            chk.IsChecked = false;
                            msgW1.Show();
                        }
                       else
                            QuotationItems.Remove((clsQuotaionVO)dgQuotationList.SelectedItem);
              //  }

               
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //base.OnClosed(e);
            //Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        public long StoreID { get; set; }
        private void FillStores(long pClinicID)
        {
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.MasterList = new List<MasterListItem>();

            //if (pClinicID > 0)
            //{
            //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
            //}


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem(0, "- Select -", true));
            //        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
            //        cmbStore.ItemsSource = null;
            //        cmbStore.ItemsSource = objList;
            //        //if (StoreID > 0)
            //        //{
            //        //    cmbStore.SelectedValue = StoreID;
            //        //}
            //        //else
            //        //{
            //        //    if (objList.Count > 1)
            //        //    {
            //        //        cmbStore.SelectedItem = objList[1];   
            //        //    }
            //        //    else
            //        //    {
            //                cmbStore.SelectedItem = objList[0];
            //        //    }
            //        //}
            //    }
                
            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();

            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    clsStoreVO select = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == pClinicID && item.Status == true//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cmbStore.ItemsSource = result.ToList();
                            cmbStore.SelectedItem = result.ToList()[0];
                        }
                    }
                    else
                    {
                        if (BizActionObj.ToStoreList.ToList().Count > 0)
                        {
                            cmbStore.ItemsSource = BizActionObj.ToStoreList.ToList();
                            cmbStore.SelectedItem = BizActionObj.ToStoreList.ToList()[0];
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillItems(long StorteID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ItemMaster;
            BizAction.MasterList = new List<MasterListItem>();

            if (StorteID > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = StorteID.ToString() };
            }


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -", true));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    lstboxItems.ItemsSource = null;
                    lstboxItems.ItemsSource = objList;
                    if (StoreID > 0)
                    {
                        cmbStore.SelectedValue = StoreID;
                    }
                    else
                    {
                        if (objList.Count > 1)
                        {
                            cmbStore.SelectedItem = objList[1];
                        }
                        else
                        {
                            cmbStore.SelectedItem = objList[0];
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((clsStoreVO)cmbStore.SelectedItem).StoreId != 0)
            {
                // FillItems(((MasterListItem)cmbStore.SelectedItem).ID);
                GetData();
            }
            else
            {
                lstboxItems.ItemsSource = null;
            }

        }

        public clsGetItemListForSearchBizActionVO BizActionObject { get; set; }
        public void GetData()
        {
            BizActionObject = new clsGetItemListForSearchBizActionVO();
            Indicatior.Show();
            BizActionObject.ItemList = new List<clsItemMasterVO>();
            ItemList = new PagedSortableCollectionView<clsItemMasterVO>();
            BizActionObject.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetItemListForSearchBizActionVO result = ea.Result as clsGetItemListForSearchBizActionVO;
                    ItemList.Clear();              
                    foreach (clsItemMasterVO item in result.ItemList)
                    {
                        ItemList.Add(item);
                    }
                    ItemList.Insert(0, new clsItemMasterVO {ItemID=0,ItemName="--Select--" });                    
                        lstboxItems.ItemsSource = null;
                        lstboxItems.ItemsSource = ItemList;
                        lstboxItems.SelectedValue = ItemList[0];
                }
                Indicatior.Close();
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long clinic = 0;
            clinic = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                

                //string ItemIDs = "";
                //if (((clsItemMasterVO)lstboxItems.SelectedItem) != null && ((clsItemMasterVO)lstboxItems.SelectedItem).ID != 0)
                //{
                //    foreach (clsItemMasterVO items in lstboxItems.SelectedItems)
                //    {
                //        ItemIDs = ItemIDs + items.ID;
                //        ItemIDs = ItemIDs + ",";
                //    }
                //    if (ItemIDs.EndsWith(","))
                //        ItemIDs = ItemIDs.Remove(ItemIDs.Length - 1, 1);
                //}
                //else
                //{
                //    ItemIDs = "";
                //}
                             
                //string SupplierIDs ="";
                //if (((MasterListItem)lstboxSupplier.SelectedItem) != null && ((MasterListItem)lstboxSupplier.SelectedItem).ID != 0)
                //{
                //    foreach (MasterListItem items in lstboxSupplier.SelectedItems)
                //    {
                //        SupplierIDs = SupplierIDs + items.ID;
                //        SupplierIDs = SupplierIDs + ",";
                //    }
                //    if (SupplierIDs.EndsWith(","))
                //        SupplierIDs = SupplierIDs.Remove(SupplierIDs.Length - 1, 1);
                //}

                //else
                //{
                //    SupplierIDs = "";
                //}

            if (QuotationItems.Count >= 2)
            {
                string QIDs = "";
                if (QuotationItems.Count > 0)
                {
                    foreach (var item in QuotationItems)
                    {
                        QIDs = QIDs + item.ID;
                        QIDs = QIDs + ",";
                    }
                    if (QIDs.EndsWith(","))
                        QIDs = QIDs.Remove(QIDs.Length - 1, 1);
                }

                string URL = "../Reports/InventoryPharmacy/rptQuotationComparison.aspx?ClinicID=" + clinic + "&QuotationIDs=" + QIDs;// +"&ItemIDs=" + ItemIDs;
                QuotationItems.Clear();
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                dgQuotationList.ItemsSource = null;
                dgQuotationList.ItemsSource = DataList;
                dgQuotationList.UpdateLayout();
            }

            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Minimum Two Quotations For Comparison", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        //private void dgQuotationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (dgQuotationList.SelectedItem != null)
        //    {

        //    }
        //}
    }
}

