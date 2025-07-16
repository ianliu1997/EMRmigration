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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Collections;

namespace PalashDynamics.Pharmacy
{
    public partial class ApplyLocationToItem : ChildWindow
    {
        #region Variable
        Boolean IsPageLoded = false;
        clsItemMasterVO objMasterVO = null;
        public bool flagClickStore = false;
        public List<clsItemStoreVO> ItemStoreList;
        public List<clsItemMasterVO> lstItemStore = new List<clsItemMasterVO>();
        public List<long> SelectedStore = new List<long>();
        public PagedSortableCollectionView<clsItemMasterVO> DataList { get; private set; }
        public List<clsItemStoreVO> ExistingStoreItems;
        public List<long> lstStore = new List<long>();
        public List<clsItemStoreVO> TestList { get; set; }
        List<clsItemTaxVO> ItemTaxList = null;
        public Boolean IsEditMode { get; set; }
        public long pkID { get; set; }
        private List<clsStoreStaus> StoreStatus { get; set; }
        public List<clsItemMasterVO> SelectedStoreList = null;
        List<MasterListItem> objList = new List<MasterListItem>();
        public string msgText = String.Empty;
        #endregion

        public ApplyLocationToItem()
        {
            InitializeComponent();
            FillClinic();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            bool StoreChecked = false;
            foreach (clsItemMasterVO item in lstItemStore)
            {
                if (item.Status == true)
                    StoreChecked = true;
            }
            if (StoreChecked == true)
            {
                if (((MasterListItem)cmbRack.SelectedItem).ID != 0)
                {
                    if (((MasterListItem)cmbContainer.SelectedItem).ID != 0)
                    {
                        if (((MasterListItem)cmbShelf.SelectedItem).ID != 0)
                        {
                            List<clsItemLocationVO> objList = new List<clsItemLocationVO>();
                            msgText = "Do you want to save location for selected Item ?";
                            MessageBoxControl.MessageBoxChildWindow msgW = 
                                new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                    Savelocation();
                            };
                            msgW.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "Please Select Shelf", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Please Select Container", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Select Rack", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please Select Srore", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ApplyLocationToItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                lblItemName.Text = "";
                if (objMasterVO.ItemName != null)
                {
                    lblItemName.Text = objMasterVO.ItemName.ToString();
                }

               
            }
            IsPageLoded = true;
            DataList = new PagedSortableCollectionView<clsItemMasterVO>();
        }

        private void dgStoreList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void GetItemDetails(clsItemMasterVO objItemMasterVO)
        {
            objMasterVO = objItemMasterVO;

        }

        private void chkStore_Click(object sender, RoutedEventArgs e)
        {
            if (dgStoreList.SelectedItem != null)
            {

                clsItemMasterVO selItem = new clsItemMasterVO();
                selItem = (clsItemMasterVO)dgStoreList.SelectedItem;
                foreach (clsItemMasterVO item in lstItemStore)
                {
                    if (item == selItem)
                    {
                        item.IsSelected = true;

                    }
                    else
                    {
                        item.IsSelected = false;

                    }
                }
                dgStoreList.ItemsSource = null;
                dgStoreList.ItemsSource = lstItemStore;
                dgStoreList.UpdateLayout();

            }
        }

        private void ViewerStore_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        long iCount = 0;
        private void cmdLocation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillGrdStore();
        }

        private void chkAllStores_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            List<clsItemMasterVO> objItemStoreList = new List<clsItemMasterVO>();
            objItemStoreList = (List<clsItemMasterVO>)dgStoreList.ItemsSource;
            if (chk.IsChecked == true)
            {
                try
                {
                    if (objItemStoreList != null)
                    {
                        foreach (var item in objItemStoreList)
                        {
                            item.Status = true;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    if (objItemStoreList != null)
                    {
                        foreach (var item in objItemStoreList)
                        {
                            item.Status = false;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            dgStoreList.ItemsSource = null;
            dgStoreList.ItemsSource = objItemStoreList;
            dgStoreList.UpdateLayout();
        }

        #region Private Methods
        private void FillClinic()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbClinic.ItemsSource = null;
                        cmbClinic.ItemsSource = objList;
                        cmbClinic.SelectedItem = objList[(int)((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId];
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillRack()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_RackMaster;
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbRack.ItemsSource = null;
                        cmbRack.ItemsSource = objList;
                        cmbRack.SelectedItem = objList[0];
                        FillShelf();
                        
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillContainer()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ContainerMaster;
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbContainer.ItemsSource = null;
                        cmbContainer.ItemsSource = objList;
                        cmbContainer.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillShelf()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ShelfMaster;
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbShelf.ItemsSource = null;
                        cmbShelf.ItemsSource = objList;
                        cmbShelf.SelectedItem = objList[0];
                        FillContainer();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillGrdStore()
        {
            try
            {
                clsGetStoresBizActionVO BizAction = new clsGetStoresBizActionVO();
                BizAction.ItemList = new List<clsItemMasterVO>();
                BizAction.ItemDetails = new clsItemMasterVO();
                BizAction.ItemDetails.ClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                BizAction.ItemDetails.ItemID = objMasterVO.ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = (clsGetStoresBizActionVO)e.Result;
                        BizAction.ItemList = ((clsGetStoresBizActionVO)e.Result).ItemList;
                        lstItemStore.Clear();
                        foreach (var item in BizAction.ItemList)
                        {
                            item.Status = false;
                            lstItemStore.Add(item);
                        }
                        dgStoreList.ItemsSource = null;
                        dgStoreList.ItemsSource = lstItemStore;
                        FillRack();  
                        GetLocationDetails(objMasterVO.ID);
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ValidateLocation()
        {
            if (iCount > 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("PALASH", "You Can not Add another Location To this Item.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }
            return false;
        }

        private void Savelocation()
        {
            clsAddUpdateItemLocationBizActionVO BizAction = new clsAddUpdateItemLocationBizActionVO();
            SelectedStore = new List<long>();
            if(SelectedStore!=null)
                SelectedStore.Clear();
            foreach (clsItemMasterVO item in lstItemStore)
            {
                if (item.Status == true)
                    SelectedStore.Add(item.ID);
            }
            BizAction.ItemLocationDetails = new clsItemLocationVO();
            BizAction.ItemLocationDetails.Rackname = ((MasterListItem)cmbRack.SelectedItem).Description;
            BizAction.ItemLocationDetails.Shelfname = ((MasterListItem)cmbShelf.SelectedItem).Description;
            BizAction.ItemLocationDetails.Containername = ((MasterListItem)cmbContainer.SelectedItem).Description;
            BizAction.ItemLocationDetails.ItemID = objMasterVO.ID;
            BizAction.ItemLocationDetails.UnitID = objMasterVO.UnitID;
            BizAction.ItemLocationDetails.RackID = ((MasterListItem)cmbRack.SelectedItem).ID;
            BizAction.ItemLocationDetails.Status = true;
            BizAction.ItemLocationDetails.ShelfID = ((MasterListItem)cmbShelf.SelectedItem).ID;
            BizAction.ItemLocationDetails.ContainerID = ((MasterListItem)cmbContainer.SelectedItem).ID;
            BizAction.ItemLocationDetails.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.ItemLocationDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizAction.ItemLocationDetails.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
            BizAction.ItemLocationDetails.AddedDateTime = System.DateTime.Now;
            BizAction.ItemLocationDetails.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

            BizAction.StoreDetails = SelectedStore;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null)
                {
                    //FetchData();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Location Apply Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    this.DialogResult = false;
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }



            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();



        }

        private void GetLocationDetails(long ItemID)
        {
            try
            {
                clsGetItemLocationBizActionVO BizAction = new clsGetItemLocationBizActionVO();
                BizAction.ItemLocationDetails = new clsItemLocationVO();
                BizAction.ItemLocationDetails.ItemID = ItemID;
                BizAction.ItemLocationDetails.UnitID = 1;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {                       
                        SelectedStore = (((clsGetItemLocationBizActionVO)args.Result).StoreDetails);
                        if (lstItemStore != null && SelectedStore != null)
                        {
                            foreach (clsItemMasterVO item in lstItemStore)
                            {
                                foreach (long items in SelectedStore)
                                {
                                    if (items == item.ID)
                                        item.Status = true;
                                }
                            }
                        }
                        if ((((clsGetItemLocationBizActionVO)args.Result).ItemLocationDetails.Rackname)!=null)
                            cmbRack.SelectedItem = (((clsGetItemLocationBizActionVO)args.Result).ItemLocationDetails.Rackname);
                        if ((((clsGetItemLocationBizActionVO)args.Result).ItemLocationDetails.Containername) != null)
                        cmbContainer.SelectedItem = (((clsGetItemLocationBizActionVO)args.Result).ItemLocationDetails.Containername);
                        if ((((clsGetItemLocationBizActionVO)args.Result).ItemLocationDetails.Shelfname) != null)
                        cmbShelf.SelectedItem = (((clsGetItemLocationBizActionVO)args.Result).ItemLocationDetails.Shelfname);
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private void cmdModifyItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

