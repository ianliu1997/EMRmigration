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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using System.Reflection;
using CIMS;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class GRNReportWithoutItem : UserControl
    {
        List<long> SelectedItemsList = new List<long>();
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public ObservableCollection<clsItemMasterVO> ItemList { get; set; }
        bool IsAllCheck { get; set; }
        public long clinicId;
        public long SelectionCount;

        public long storeId;
        public GRNReportWithoutItem()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemList = new ObservableCollection<clsItemMasterVO>();
                //lstItems.ItemsSource = null;
                //lstItems.ItemsSource = ItemList;

                GetItemMaster();
                FillClinic();
                dtpFromDate.SelectedDate = DateTime.Now;
                dtpToDate.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void GetItemMaster()
        {
            //try
            //{
            //    clsGetItemListBizActionVO BizAction = new clsGetItemListBizActionVO();
            //    BizAction.ItemDetails = new clsItemMasterVO();
            //    BizAction.ItemList = new List<clsItemMasterVO>();
            //    BizAction.ItemDetails.RetrieveDataFlag = false;
            //    BizAction.ForReportFilter = true;

            //    if (SelectionCount > 0)
            //    {
            //        BizAction.FilterCriteria = SelectionCount;

            //        //Added BY Pallavi
            //        BizAction.FilterClinicId = clinicId;
            //        if (cmbStore.SelectedItem != null)
            //        {
            //            BizAction.FilterStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
            //        }
            //        //BizAction.FilterStoreId = storeId;

            //    }

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, args) =>
            //    {
            //        if (args.Error == null && args.Result != null)
            //        {
            //            //if (ItemList == null)
            //            //    ItemList = new ObservableCollection<clsItemMasterVO>();


            //            //foreach (clsItemMasterVO item in ((clsGetItemListBizActionVO)args.Result).ItemList)
            //            //{
            //            //    ItemList.Add(item);
            //            //}

            //            //lstItems.ItemsSource = null;
            //            //lstItems.ItemsSource = ItemList;
            //            if (((clsGetItemListBizActionVO)args.Result).MasterList != null && ((clsGetItemListBizActionVO)args.Result).MasterList.Count > 0)
            //            {
            //                List<MasterListItem> ObjList = new List<MasterListItem>();
            //                ObjList.Add(new MasterListItem { ID = 0, Description = "--All Items--" });
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

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;

            bool chkToDate = true;

            long ClinicID = 0;
            long StoreID = 0;
            long SupplierID = 0;
            string msgTitle = "";

            if (cmbClinic.SelectedItem != null)
            {
                ClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            if (cmbStore.SelectedItem != null)
            {
                StoreID = ((clsStoreVO)cmbStore.SelectedItem).ID;
            }
            // BY bHUSHAN . . . 
            if (cmbSupplier.SelectedItem != null)
            {
                SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
            }

            if (dtpFromDate.SelectedDate != null)
            {
                dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtpT = dtpToDate.SelectedDate.Value.Date.Date;
                if (dtpF.Value > dtpT.Value)
                {
                    dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                    dtpT = dtpF;
                    chkToDate = false;
                }
                else
                {
                    dtpP = dtpT;
                    dtpT = dtpT.Value.AddDays(1);
                    dtpToDate.Focus();
                }
            }

            if (dtpT != null)
            {
                if (dtpF != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
                }
            }
            string ItemIDs = "";

            //if (((MasterListItem)cmbItem.SelectedItem).ID == 0)
            //{
            //    ItemIDs = null;
            //}
            //else if (ItemList != null && ItemList.Count > 0)
            //{
            //    foreach (var item in ItemList)
            //    {
            //        ItemIDs = ItemIDs + item.ID;
            //        ItemIDs = ItemIDs + ",";
            //    }

            //    if (ItemIDs.EndsWith(","))
            //        ItemIDs = ItemIDs.Remove(ItemIDs.Length - 1, 1);
            //}

            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/InventoryPharmacy/GRNReport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&GRNWithItem=" + false + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&ClinicID=" + ClinicID + "&StoreID=" + StoreID + "&SupplierID=" + SupplierID + "&Excel=" + chkExcel.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/InventoryPharmacy/GRNReport.aspx?GRNWithItem=" + false + "&ClinicID=" + ClinicID + "&StoreID=" + StoreID + "&SupplierID=" + SupplierID + "&Excel=" + chkExcel.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
            else
            {
                string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void chkAllStores_Checked(object sender, RoutedEventArgs e)
        {
            //try
            //{

            //    List<clsItemMasterVO> objItemStoreList = new List<clsItemMasterVO>();
            //    // objItemStoreList = (List<clsItemMasterVO>)lstItems.ItemsSource;
            //    //objItemStoreList = lstItems.ItemsSource;
            //    foreach (var item in lstItems.ItemsSource)
            //    {
            //        objItemStoreList.Add((clsItemMasterVO)item);
            //    }
            //    if (objItemStoreList != null)
            //    {

            //        foreach (var item in objItemStoreList)
            //        {
            //            item.Status = true;
            //            item.IsSelected = true;
            //        }



            //    }
            //    lstItems.ItemsSource = null;
            //    lstItems.ItemsSource = objItemStoreList;
            //    IsAllCheck = true;
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
        }

        private void chkAllStores_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (IsAllCheck == true)
                //{
                //    List<clsItemMasterVO> objItemStoreList = new List<clsItemMasterVO>();
                //    //objItemStoreList = (List<clsItemMasterVO>)lstItems.ItemsSource;

                //    foreach (var item in lstItems.ItemsSource)
                //    {
                //        objItemStoreList.Add((clsItemMasterVO)item);
                //    }
                //    if (objItemStoreList != null)
                //    {

                //        foreach (var item in objItemStoreList)
                //        {
                //            item.Status = false;
                //            item.IsSelected = false;
                //        }



                //    }
                //    lstItems.ItemsSource = null;
                //    lstItems.ItemsSource = objItemStoreList;
                //    IsAllCheck = false;
                //if (lstItems.SelectedItem != null)
                //{
                //    ItemList.RemoveAt(lstItems.SelectedIndex);

                //}

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void chkStores_Unchecked(object sender, RoutedEventArgs e)
        {
            ////IsAllCheck = false;
            ////chkAllStores.IsChecked = false;
            //try
            //{
            //    if (lstItems.SelectedItem != null)
            //    {
            //        ItemList.RemoveAt(lstItems.SelectedIndex);

            //    }
            //    //IsAllCheck = false;
            //    //chkAllStores.IsChecked = false;
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
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "-Select--", Status = true };
                    BizActionObj.ToStoreList.Insert(0, Default);
                    BizActionObj.ItemMatserDetails.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == clinicId && item.Status == true
                                 select item;

                    List<clsStoreVO> ClinicWiseStores = new List<clsStoreVO>();
                    ClinicWiseStores = result.ToList();
                    ClinicWiseStores.Insert(0, Default);
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        cmbStore.ItemsSource = null;
                        cmbStore.ItemsSource = ClinicWiseStores.ToList();
                        cmbStore.SelectedItem = ClinicWiseStores.ToList()[0];
                    }
                    else
                    {
                        //User assigned Stores
                        cmbStore.ItemsSource = null;
                        cmbStore.ItemsSource = BizActionObj.ToStoreList;
                        cmbStore.SelectedItem = BizActionObj.ToStoreList[0];
                    }
                }
            };
            client.CloseAsync();
        }
        //private void FillStores(long clinicId)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_Store;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    if (clinicId > 0)
        //    {
        //        BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
        //    }

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

        //            //cmbBloodGroup.ItemsSource = null;
        //            //cmbBloodGroup.ItemsSource = objList;
        //            cmbStore.ItemsSource = null;
        //            cmbStore.ItemsSource = objList;

        //            cmbStore.SelectedItem = objList[0];
        //            //storeId = ((MasterListItem)cmbStore.SelectedItem).ID;


        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null)
                {
                    clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    SelectionCount = SelectionCount + 1;
                    FillStores(clinicId);
                    FillcmbSupplier();
                    GetItemMaster();
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbStore.SelectedItem != null && ((clsStoreVO)cmbStore.SelectedItem).ID != 0)
                {
                    storeId = ((clsStoreVO)cmbStore.SelectedItem).ID;
                    SelectionCount = SelectionCount + 1;
                    GetItemMaster();
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (cmbItem.SelectedItem != null && ((MasterListItem)cmbItem.SelectedItem).ID > 0)
            //    {
            //        clsItemMasterVO tempItem = new clsItemMasterVO();
            //        var item1 = from r in ItemList
            //                    where (r.ID == ((MasterListItem)cmbItem.SelectedItem).ID)
            //                    select new clsItemMasterVO
            //                    {
            //                        Status = r.Status,
            //                        ID = r.ID,
            //                        ItemName = r.ItemName
            //                    };

            //        if (item1.ToList().Count == 0)
            //        {

            //            tempItem.ID = ((MasterListItem)cmbItem.SelectedItem).ID;
            //            tempItem.ItemName = ((MasterListItem)cmbItem.SelectedItem).Description;
            //            tempItem.IsSelected = true;
            //            ItemList.Add(tempItem);

            //        }
            //        else
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW1.Show();
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

        }

        private void cmbItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    if (cmbItem.SelectedItem != null)
            //    {
            //        if (((MasterListItem)cmbItem.SelectedItem).ID == 0)
            //        {
            //            if (ItemList != null)
            //            {
            //                if (ItemList.Count > 0)
            //                {
            //                    ItemList.Clear();
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        // By BHUSHAN . . . . 
        private void FillcmbSupplier()
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

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
