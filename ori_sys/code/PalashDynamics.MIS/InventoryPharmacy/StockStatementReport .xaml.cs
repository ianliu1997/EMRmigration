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
using PalashDynamics.ValueObjects.Master;
using CIMS;
using System.Windows.Browser;
using System.Reflection;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class StockStatementReport : UserControl
    {
        List<long> SelectedItemsList = new List<long>();
        public ObservableCollection<clsItemMasterVO> ItemList { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public long StoreID { get; set; }
        public long ClinicID { get; set; }
        public long SelectionCount;
        public long clinicId;
        public long storeId;


        public StockStatementReport()
        {
            InitializeComponent();
        }

        private void GetItemMaster()
        {
            try
            {
                clsGetItemListBizActionVO BizAction = new clsGetItemListBizActionVO();
                BizAction.ItemList = new List<clsItemMasterVO>();
                BizAction.ItemDetails = new clsItemMasterVO();
                BizAction.ItemDetails.RetrieveDataFlag = false;
                BizAction.ForReportFilter = true;

                if (SelectionCount > 0)
                {
                    BizAction.FilterCriteria = SelectionCount;

                    //Added BY Pallavi
                    BizAction.FilterClinicId = clinicId;
                    if (cmbStore.SelectedItem != null)
                    {
                        BizAction.FilterStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                    }
                    //BizAction.FilterStoreId = storeId;

                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {

                        if (((clsGetItemListBizActionVO)args.Result).MasterList != null && ((clsGetItemListBizActionVO)args.Result).MasterList.Count > 0)
                        {

                            List<MasterListItem> ObjList = new List<MasterListItem>();
                            ObjList.Add(new MasterListItem { ID = 0, Description = "--All Items--" });
                            ObjList.AddRange(((clsGetItemListBizActionVO)args.Result).MasterList);
                            cmbItem.ItemsSource = null;
                            cmbItem.ItemsSource = ObjList;
                            cmbItem.SelectedItem = ObjList[0];
                        }
                        

                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
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
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            if (clinicId > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                   
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    cmbStore.SelectedItem = objList[0];

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
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


            if (((MasterListItem)cmbItem.SelectedItem).ID == 0)
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


            long clinic = 0;
            long store = 0;
            
            if (cmbClinic.SelectedItem != null)
            {
                clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }

            if (cmbStore.SelectedItem != null)
            {
                store = ((MasterListItem)cmbStore.SelectedItem).ID;
            }

            string URL = "../Reports/InventoryPharmacy/StockStatement.aspx?ClinicID=" + clinic + "&StoreID=" + store + "&ItemIDs=" + ItemIDs;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ItemList = new ObservableCollection<clsItemMasterVO>();
            lstItems.ItemsSource = null;
            lstItems.ItemsSource = ItemList;
            FillClinic();
            GetItemMaster();
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
            if (cmbStore.SelectedItem != null && ((MasterListItem)cmbStore.SelectedItem).ID != 0)
            {
                storeId = ((MasterListItem)cmbStore.SelectedItem).ID;
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
