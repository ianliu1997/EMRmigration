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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using System.Reflection;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class ItemLedgerReport : UserControl
    {
        List<long> SelectedItemsList = new List<long>();
        public ObservableCollection<clsItemMasterVO> ItemList { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public long StoreID { get; set; }
        public long ClinicID { get; set; }
        public long SelectionCount;
        public long clinicId;
        public long storeId;
        public bool IsOldReport { get; set; } //Added by Ashish z. as per Discussion with Dr. Gautham(Milann) and Mangesh on dated 28082016
        public ItemLedgerReport()
        {
            InitializeComponent();
        }

        private void GetItemMaster()
        {
            //try
            //{
            //    clsGetItemListBizActionVO BizAction = new clsGetItemListBizActionVO();
            //    BizAction.ItemList = new List<clsItemMasterVO>();
            //    BizAction.ItemDetails = new clsItemMasterVO();
            //    BizAction.ItemDetails.RetrieveDataFlag = false;
            //    BizAction.ForReportFilter = true;

            //    if (SelectionCount > 0)
            //    {
            //        BizAction.FilterCriteria = SelectionCount;

            //        //Added BY Pallavi
            //        BizAction.FilterClinicId = clinicId;
            //        if (cmbStore.SelectedItem != null)
            //        {
            //            BizAction.FilterStoreId = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            //        }
            //        //BizAction.FilterStoreId = storeId;

            //    }

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, args) =>
            //    {
            //        if (args.Error == null && args.Result != null)
            //        {

            //            if (((clsGetItemListBizActionVO)args.Result).MasterList != null && ((clsGetItemListBizActionVO)args.Result).MasterList.Count > 0)
            //            {

            //                List<MasterListItem> ObjList = new List<MasterListItem>();
            //                ObjList.Add(new MasterListItem { ID = 0, Description = "--Select Items--" });
            //                ObjList.AddRange(((clsGetItemListBizActionVO)args.Result).MasterList);
            //                cmbItem.ItemsSource = null;
            //                cmbItem.ItemsSource = ObjList;
            //                cmbItem.SelectedItem = ObjList[0];
            //            }


            //        }
            //    };

            //    client.ProcessAsync(BizAction, new clsUserVO());
            //    client.CloseAsync();
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        private void FillClinic()
        {
            try
            {

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                        cmbClinic.ItemsSource = null;
                        cmbClinic.ItemsSource = objList;

                        if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {
                            //for selecting unitid according to user login unit id
                            //for selecting unitid according to user login unit id
                            var res = from r in objList
                                      where r.ID == User.UserLoginInfo.UnitId
                                      select r;
                            cmbClinic.SelectedItem = ((MasterListItem)res.First());
                            cmbClinic.IsEnabled = false;
                        }
                        else
                            cmbClinic.SelectedItem = objList[0];

                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void FillStores(long clinicId)
        {
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            ////BizAction.IsActive = true;
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.MasterList = new List<MasterListItem>();

            //if (clinicId > 0)
            //{
            //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
            //}

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();


            //        objList.Add(new MasterListItem(0, "- Select -"));
            //        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

            //        cmbStore.ItemsSource = null;
            //        cmbStore.ItemsSource = objList;

            //        cmbStore.SelectedItem = objList[0];

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
                                 where item.ClinicId == clinicId && item.Status == true//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
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

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbClinic.SelectedItem).ID != 0)
            {
                List<clsItemMasterVO> objList = ItemList.ToList<clsItemMasterVO>();
                if (objList != null)
                {
                    foreach (var item in objList)
                    {
                        if (item.IsSelected == true)
                        {
                            SelectedItemsList.Add(item.ID);
                        }
                    }
                }

                string ItemIDs = "";

                if (cmbItem.SelectedItem!=null && ((MasterListItem)cmbItem.SelectedItem).ID == 0)
                {
                    ItemIDs = null;
                }
                else if (ItemList != null && ItemList.Count > 0)
                {
                    foreach (var item in ItemList)
                    {
                        ItemIDs = ItemIDs + item.ID;
                        ItemIDs = ItemIDs + ",";
                    }

                    if (ItemIDs.EndsWith(","))
                        ItemIDs = ItemIDs.Remove(ItemIDs.Length - 1, 1);

                }

                DateTime FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
                DateTime ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;
                long clinic = 0;
                long store = 0;
                string ItemName = string.Empty;

                if (!string.IsNullOrEmpty(txtItemName.Text))
                {
                    ItemName = txtItemName.Text.ToString();
                }

                if (cmbClinic.SelectedItem != null)
                {
                    clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }

                if (cmbStore.SelectedItem != null)
                {
                    store = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                }


                if (!this.IsOldReport)
                {
                    string URL = "../Reports/InventoryPharmacy/ItemLedgerMIS.aspx?ClinicID=" + clinic + "&StoreID=" + store + "&ItemIDs=" + ItemIDs + "&FromDate="
                        + FromDate + "&ToDate=" + ToDate + "&Excel=" + chkExcel.IsChecked + "&UserID=" + ((IApplicationConfiguration)App.Current).CurrentUser.ID + "&IsOldReport=" + false + "&ItemName=" + ItemName;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else if (this.IsOldReport) //Added by Ashish z. as per Discussion with Dr. Gautham(Milann) and Mangesh on dated 28082016
                {
                    string URL = "../Reports/InventoryPharmacy/ItemLedgerMIS.aspx?ClinicID=" + clinic + "&StoreID=" + store + "&ItemIDs=" + ItemIDs + "&FromDate="
                        + FromDate + "&ToDate=" + ToDate + "&Excel=" + chkExcel.IsChecked + "&UserID=" + ((IApplicationConfiguration)App.Current).CurrentUser.ID + "&IsOldReport=" + true + "&ItemName=" + ItemName; ;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Clinic", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsOldReport) contentName.Content = "ITEM LEDGER REPORT(Old)";
            else contentName.Content = "ITEM LEDGER REPORT";
            ItemList = new ObservableCollection<clsItemMasterVO>();
            lstItems.ItemsSource = null;
            lstItems.ItemsSource = ItemList;
            FillClinic();
            GetItemMaster();
            DateTime now = DateTime.Now;
            this.dtpFromDate.SelectedDate = new DateTime(now.Year, now.Month, 1);
            this.dtpToDate.SelectedDate = DateTime.Now;
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbClinic.SelectedItem != null)
            {
                clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
                SelectionCount = SelectionCount + 1;
                FillStores(clinicId);
                GetItemMaster();
            }
        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStore.SelectedItem != null && ((clsStoreVO)cmbStore.SelectedItem).StoreId != 0)
            {
                storeId = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                SelectionCount = SelectionCount + 1;
                GetItemMaster();
            }
        }

        private void chkAllStores_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                List<clsItemMasterVO> objItemStoreList = new List<clsItemMasterVO>();
                // objItemStoreList = (List<clsItemMasterVO>)lstItems.ItemsSource;
                //objItemStoreList = lstItems.ItemsSource;
                foreach (var item in lstItems.ItemsSource)
                {
                    objItemStoreList.Add((clsItemMasterVO)item);
                }
                if (objItemStoreList != null)
                {

                    foreach (var item in objItemStoreList)
                    {
                        item.Status = true;
                        item.IsSelected = true;
                    }



                }
                lstItems.ItemsSource = null;
                lstItems.ItemsSource = objItemStoreList;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void chkAllStores_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                List<clsItemMasterVO> objItemStoreList = new List<clsItemMasterVO>();
                //objItemStoreList = (List<clsItemMasterVO>)lstItems.ItemsSource;

                foreach (var item in lstItems.ItemsSource)
                {
                    objItemStoreList.Add((clsItemMasterVO)item);
                }
                if (objItemStoreList != null)
                {

                    foreach (var item in objItemStoreList)
                    {
                        item.Status = false;
                        item.IsSelected = false;
                    }
                }
                lstItems.ItemsSource = null;
                lstItems.ItemsSource = objItemStoreList;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void chkStores_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstItems.SelectedItem != null)
                {
                    ItemList.RemoveAt(lstItems.SelectedIndex);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbItem.SelectedItem != null && ((MasterListItem)cmbItem.SelectedItem).ID > 0)
            {
                clsItemMasterVO tempItem = new clsItemMasterVO();
                var item1 = from r in ItemList
                            where (r.ID == ((MasterListItem)cmbItem.SelectedItem).ID)
                            select new clsItemMasterVO
                            {
                                Status = r.Status,
                                ID = r.ID,
                                ItemName = r.ItemName
                            };

                if (item1.ToList().Count == 0)
                {

                    tempItem.ID = ((MasterListItem)cmbItem.SelectedItem).ID;
                    tempItem.ItemName = ((MasterListItem)cmbItem.SelectedItem).Description;
                    tempItem.IsSelected = true;
                    ItemList.Add(tempItem);

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            }
        }

        private void cmbItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbItem.SelectedItem != null)
                {
                    if (((MasterListItem)cmbItem.SelectedItem).ID == 0)
                    {
                        if (ItemList != null)
                        {
                            if (ItemList.Count > 0)
                            {
                                ItemList.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }
    }
}
