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
using PalashDynamics.ValueObjects.Inventory.Indent;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Inventory;
using System.Windows.Browser;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;


namespace PalashDynamics.Pharmacy
{
    public partial class ItemStoreLocationDetails : ChildWindow
    {
        bool UserStoreAssigned = false;
        int count = 0;
        Boolean IsPageLoded = false;
        public clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        List<clsStoreVO> UserStores = new List<clsStoreVO>();
        public List<ItemStoreLocationDetailsVO> lstItemStore = new List<ItemStoreLocationDetailsVO>();
        public PagedSortableCollectionView<ItemStoreLocationDetailsVO> MasterList { get; private set; }
        public bool flagClickStore = false;
        public bool IsFromNewItem;
        public clsItemMasterVO objMasterVO;
        public List<long> SelectedStore = new List<long>();
        string msgText = "";
        List<clsStoreVO> tempSelectAll { get; set; }
        WaitIndicator objWIndicator;
        public ItemStoreLocationDetails()
        {
            InitializeComponent();
            MasterList = new PagedSortableCollectionView<ItemStoreLocationDetailsVO>();
            objWIndicator = new WaitIndicator();
        }
        public void GetItemDetails(clsItemMasterVO objItemMasterVO)
        {
            objMasterVO = objItemMasterVO;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                if (IsFromNewItem == false)
                {
                    lblItemName.Text = "";
                    if (objMasterVO.ItemName != null)
                    {
                        lblItemName.Text = objMasterVO.ItemName.ToString();
                    }
                }

                FillStore();
                FillRack();
                FillShelf();
                FillContainer();
                SetupPage();
                tempSelectAll = new List<clsStoreVO>();
                //ClearTaxUI();
            }
            IsPageLoded = true;
            count = 0;
        }
        #region Fill AllCombo
        private void FillStore()
        {
            UserStoreAssigned = false;
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = 0; //((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ItemID = objMasterVO.ID;
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            BizActionObj.ToStoreList = new List<clsStoreVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };

                    BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);

                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.Status == true
                                  select item;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    //List<clsStoreVO> UserStores = new List<clsStoreVO>();
                    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                    UserStores.Clear();

                    foreach (var item in UserUnit)
                    {
                        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                        {    //item.StoreDetails       
                            UserStores = item.UserUnitStore;

                            //if (item.IsDefault == true)
                            //{
                            //    //item.UserUnitStore.Insert(0, Default);
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                            //else if (item.IsDefault == false)
                            //{
                            //    //item.UserUnitStore.Insert(0, Default);
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                        }
                    }
                    //UserStores.Insert(0, Default);
                    var resultnew = from item in UserStores select item;
                    if (UserStores != null && UserStores.Count > 0)
                    {
                        UserStoreAssigned = true;
                    }

                    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                    StoreListForClinic.Insert(0, Default);

                    //List<clsStoreVO> StoreListForClinicUser = (List<clsStoreVO>)resultnew.ToList(); //(List<clsStoreVO>)resultnew.ToList(); 
                    // StoreListForClinicUser.Insert(0, Default);
                    // cmbToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                    cmbStore.ItemsSource = result1.ToList();


                    if (result1.ToList().Count > 0)
                    {

                        cmbStore.SelectedItem = result1.ToList()[0];
                    }
                    //cmbAddToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;

                    //if (StoreListForClinicUser.Count > 0)
                    //    cmbAddFromStoreName.SelectedItem = StoreListForClinicUser[0]; //StoreListForClinic[0];
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        cmbStore.ItemsSource = BizActionObj.ToStoreList.ToList();
                        if (BizActionObj.ToStoreList.ToList().Count > 0)
                        {
                            cmbStore.SelectedItem = (BizActionObj.ToStoreList.ToList())[0];
                        }
                        //cmbStore.ItemsSource = result1.ToList();
                        //cmbStore.ItemsSource = BizActionObj.ItemMatserDetails;
                        //cmbStore.ItemsSource = result1.ToList();
                        //if (result1.ToList().Count > 0)
                        //{
                        //    cmbStore.SelectedItem = (result1.ToList())[0];

                        //}
                    }

                    else
                    {
                        cmbStore.ItemsSource = StoreListForClinic;

                        if (StoreListForClinic.Count > 0)
                        {
                            cmbStore.SelectedItem = StoreListForClinic[0];

                        }
                    }
                }
            };

            client.CloseAsync();

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
                        cmbBin.ItemsSource = null;
                        cmbBin.ItemsSource = objList;
                        cmbBin.SelectedItem = objList[0];
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
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (chkAllStore.IsChecked == true || ((clsStoreVO)cmbStore.SelectedItem).StoreId > 0)
            {
                if (MasterList.Count > 0)
                {
                    var item = from r in MasterList.ToList()
                               where r.StoreID == ((clsStoreVO)cmbStore.SelectedItem).StoreId
                               select r;



                    if (item.ToList().Count > 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "You can Add only one Location for a Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    else
                    {
                        if (((MasterListItem)cmbRack.SelectedItem).ID > 0)
                        {
                            if (((MasterListItem)cmbShelf.SelectedItem).ID > 0)
                            {
                                if (((MasterListItem)cmbBin.SelectedItem).ID > 0)
                                {
                                    string msgTitle = "Palash";
                                    msgText = "Are you sure you want to Save ?";
                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedSave);
                                    msgW.Show();
                                }
                                else
                                {
                                    cmbBin.TextBox.SetValidation("Please Select Bin");
                                    cmbBin.TextBox.RaiseValidationError();
                                    cmbBin.Focus();
                                }
                            }
                            else
                            {
                                cmbShelf.TextBox.SetValidation("Please Select Shelf");
                                cmbShelf.TextBox.RaiseValidationError();
                                cmbShelf.Focus();
                            }
                        }
                        else
                        {
                            cmbRack.TextBox.SetValidation("Please Select Rack");
                            cmbRack.TextBox.RaiseValidationError();
                            cmbRack.Focus();
                        }
                    }
                }
                else
                {
                    if (((MasterListItem)cmbRack.SelectedItem).ID > 0)
                    {
                        if (((MasterListItem)cmbShelf.SelectedItem).ID > 0)
                        {
                            if (((MasterListItem)cmbBin.SelectedItem).ID > 0)
                            {
                                string msgTitle = "Palash";
                                msgText = "Are you sure you want to Save ?";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedSave);
                                msgW.Show();
                            }
                            else
                            {
                                cmbBin.TextBox.SetValidation("Please Select Bin");
                                cmbBin.TextBox.RaiseValidationError();
                                cmbBin.Focus();
                            }
                        }
                        else
                        {
                            cmbShelf.TextBox.SetValidation("Please Select Shelf");
                            cmbShelf.TextBox.RaiseValidationError();
                            cmbShelf.Focus();
                        }
                    }
                    else
                    {
                        cmbRack.TextBox.SetValidation("Please Select Rack");
                        cmbRack.TextBox.RaiseValidationError();
                        cmbRack.Focus();
                    }
                }
            }
            else
            {
                cmbStore.TextBox.SetValidation("Please Select Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
            }

        }
        #region SaveMethod Message
        void msgW_OnMessageBoxClosedSave(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {

                SaveItem();
            }
            else
            {
                // SetCommandButtonState("New");
            }
        }
        #endregion
        void msgW_OnMessageBoxClosedModify(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ModifyItem();
            }
            else
            {
                // SetCommandButtonState("New");
            }
        }
        private void SaveItem()
        {
            try
            {

                int flag = 0;
                foreach (var item1 in this.MasterList.ToList())
                {
                    if (item1.StoreID == ((clsStoreVO)cmbStore.SelectedItem).StoreId && item1.RackID == ((MasterListItem)cmbRack.SelectedItem).ID && item1.ShelfID == ((MasterListItem)cmbShelf.SelectedItem).ID && item1.ContainerID == ((MasterListItem)cmbBin.SelectedItem).ID)
                    {
                        flag = 1;
                    }
                    else
                    {

                    }
                }
                if (flag == 1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record Alredy Present. Please Select Another Location ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
                else
                {
                    if (chkAllStore.IsChecked == false)
                    {
                        tempSelectAll.Clear();
                    }
                    if (objWIndicator == null) objWIndicator = new WaitIndicator();
                    objWIndicator.Show();
                    ItemStoreLocationDetailsBizActionVo BizAction = new ItemStoreLocationDetailsBizActionVo();
                    SelectedStore = new List<long>();
                    if (SelectedStore != null)
                        SelectedStore.Clear();
                    foreach (clsStoreVO item in tempSelectAll)//BizAction.ClsStoreVOList)
                    {
                        if (item.Status == true)
                            SelectedStore.Add(item.StoreId);
                    }
                    BizAction.ItemStoreLocationDetail = new ItemStoreLocationDetailsVO();
                    BizAction.ItemStoreLocationDetail.ID = 0;
                    BizAction.ItemStoreLocationDetail.Rackname = ((MasterListItem)cmbRack.SelectedItem).Description;
                    BizAction.ItemStoreLocationDetail.Shelfname = ((MasterListItem)cmbShelf.SelectedItem).Description;
                    BizAction.ItemStoreLocationDetail.Containername = ((MasterListItem)cmbBin.SelectedItem).Description;
                    BizAction.ItemStoreLocationDetail.ItemID = objMasterVO.ID;
                    BizAction.ItemStoreLocationDetail.UnitID = objMasterVO.UnitID;
                    BizAction.ItemStoreLocationDetail.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                    BizAction.ItemStoreLocationDetail.RackID = ((MasterListItem)cmbRack.SelectedItem).ID;
                    BizAction.ItemStoreLocationDetail.Status = true;
                    BizAction.ItemStoreLocationDetail.ShelfID = ((MasterListItem)cmbShelf.SelectedItem).ID;
                    BizAction.ItemStoreLocationDetail.ContainerID = ((MasterListItem)cmbBin.SelectedItem).ID;
                    BizAction.ItemStoreLocationDetail.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizAction.ItemStoreLocationDetail.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizAction.ItemStoreLocationDetail.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    BizAction.ItemStoreLocationDetail.AddedDateTime = System.DateTime.Now;
                    BizAction.ItemStoreLocationDetail.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    BizAction.ItemStoreLocationDetail.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                    // if (chkAllStore.IsChecked == true)
                    // {
                    BizAction.StoreDetails = SelectedStore;
                    //}
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            SetupPage();
                            cmbRack.SelectedValue = (long)0;
                            cmbShelf.SelectedValue = (long)0;
                            cmbBin.SelectedValue = (long)0;
                            cmbStore.SelectedValue = (long)0;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Location Apply Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            //this.DialogResult = false;
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }
                        objWIndicator.Close();
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();

                }
            }
            catch (Exception Ex)
            {
                objWIndicator.Close();
            }
        }

        private void ModifyItem()
        {
            try
            {
                int flag = 0;
                foreach (var item1 in this.MasterList.ToList())
                {
                    if (item1.StoreID == ((clsStoreVO)cmbStore.SelectedItem).StoreId && item1.RackID == ((MasterListItem)cmbRack.SelectedItem).ID && item1.ShelfID == ((MasterListItem)cmbShelf.SelectedItem).ID && item1.ContainerID == ((MasterListItem)cmbBin.SelectedItem).ID)
                    {
                        flag = 1;
                    }
                    else
                    {

                    }
                }

                if (dgItemList.SelectedItem != null)
                {
                    if (flag == 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("", "Record Alredy Present. Please Select Another Location ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();

                    }
                    else
                    {
                        if (objWIndicator == null) objWIndicator = new WaitIndicator();
                        objWIndicator.Show();
                        ItemStoreLocationDetailsBizActionVo BizAction = new ItemStoreLocationDetailsBizActionVo();
                        //SelectedStore = new List<long>();
                        //if (SelectedStore != null)
                        //    SelectedStore.Clear();
                        //foreach (ItemStoreLocationDetailsVO item in lstItemStore)
                        //{
                        //    if (item.Status == true)
                        //        SelectedStore.Add(item.ID);
                        //}
                        BizAction.ItemStoreLocationDetail = new ItemStoreLocationDetailsVO();
                        BizAction.ItemStoreLocationDetail.ID = Convert.ToInt64(((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).ID);
                        BizAction.ItemStoreLocationDetail.Rackname = ((MasterListItem)cmbRack.SelectedItem).Description;
                        BizAction.ItemStoreLocationDetail.Shelfname = ((MasterListItem)cmbShelf.SelectedItem).Description;
                        BizAction.ItemStoreLocationDetail.Containername = ((MasterListItem)cmbBin.SelectedItem).Description;
                        BizAction.ItemStoreLocationDetail.ItemID = objMasterVO.ID;
                        BizAction.ItemStoreLocationDetail.UnitID = objMasterVO.UnitID;
                        //BizAction.ItemStoreLocationDetail.StoreID = ;
                        BizAction.ItemStoreLocationDetail.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                        BizAction.ItemStoreLocationDetail.RackID = ((MasterListItem)cmbRack.SelectedItem).ID;
                        BizAction.ItemStoreLocationDetail.Status = true;
                        BizAction.ItemStoreLocationDetail.ShelfID = ((MasterListItem)cmbShelf.SelectedItem).ID;
                        BizAction.ItemStoreLocationDetail.ContainerID = ((MasterListItem)cmbBin.SelectedItem).ID;
                        BizAction.ItemStoreLocationDetail.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        BizAction.ItemStoreLocationDetail.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        BizAction.ItemStoreLocationDetail.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        BizAction.ItemStoreLocationDetail.AddedDateTime = System.DateTime.Now;
                        BizAction.ItemStoreLocationDetail.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                        BizAction.ItemStoreLocationDetail.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                        //if (chkAllStore.IsChecked == true)
                        //{
                        BizAction.StoreDetails = SelectedStore;
                        //}

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                SetupPage();
                                cmbRack.SelectedValue = (long)0;
                                cmbShelf.SelectedValue = (long)0;
                                cmbBin.SelectedValue = (long)0;
                                cmbStore.SelectedValue = (long)0;
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Location Modify Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                //nthis.DialogResult = false;
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();
                            }
                            objWIndicator.Close();
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                        cmdModify.Visibility = Visibility.Collapsed;
                        cmdSave.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Item Not Selected", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                    objWIndicator.Close();
                }
            }
            catch (Exception Ex)
            {
                objWIndicator.Close();
            }
        }
        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (chkAllStore.IsChecked == true || ((clsStoreVO)cmbStore.SelectedItem).StoreId > 0)
            {
                if (MasterList.Count > 0)
                {
                    long ModifyId = ((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).StoreID;

                    //var item = from r in MasterList.ToList()
                    //           where r.StoreID == ((clsStoreVO)cmbStore.SelectedItem).StoreId && (r.RackID == (cmbRack.SelectedItem as MasterListItem).ID
                    //                               || r.ShelfID == (cmbShelf.SelectedItem as MasterListItem).ID || r.ContainerID == (cmbBin.SelectedItem as MasterListItem).ID)
                    //           select r;

                    var item = from r in MasterList.ToList()
                               where r.StoreID != ModifyId
                               select r;

                    foreach (var item1 in item.ToList())
                    {
                        if (item1.StoreID == ((clsStoreVO)cmbStore.SelectedItem).StoreId)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "You can Add only one Location for a Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            return;
                        }
                    }

                    //if (item.ToList().Count > 0)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //               new MessageBoxControl.MessageBoxChildWindow("Palash", "You can Add only one Location for a Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgW1.Show();
                    //}
                    //else
                    //{
                    if (((MasterListItem)cmbRack.SelectedItem).ID > 0)
                    {
                        if (((MasterListItem)cmbShelf.SelectedItem).ID > 0)
                        {
                            if (((MasterListItem)cmbBin.SelectedItem).ID > 0)
                            {
                                string msgTitle = "Palash";
                                msgText = "Are you sure you want to Modify ?";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedModify);
                                msgW.Show();
                            }
                            else
                            {
                                cmbBin.TextBox.SetValidation("Please Select Bin");
                                cmbBin.TextBox.RaiseValidationError();
                                cmbBin.Focus();
                            }
                        }
                        else
                        {
                            cmbShelf.TextBox.SetValidation("Please Select Shelf");
                            cmbShelf.TextBox.RaiseValidationError();
                            cmbShelf.Focus();
                        }
                    }
                    else
                    {
                        cmbRack.TextBox.SetValidation("Please Select Rack");
                        cmbRack.TextBox.RaiseValidationError();
                        cmbRack.Focus();
                    }
                    // }
                }
                else
                {
                    if (((MasterListItem)cmbRack.SelectedItem).ID > 0)
                    {
                        if (((MasterListItem)cmbShelf.SelectedItem).ID > 0)
                        {
                            if (((MasterListItem)cmbBin.SelectedItem).ID > 0)
                            {
                                string msgTitle = "Palash";
                                msgText = "Are you sure you want to Save ?";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedModify);
                                msgW.Show();
                            }
                            else
                            {
                                cmbBin.TextBox.SetValidation("Please Select Bin");
                                cmbBin.TextBox.RaiseValidationError();
                                cmbBin.Focus();
                            }
                        }
                        else
                        {
                            cmbShelf.TextBox.SetValidation("Please Select Shelf");
                            cmbShelf.TextBox.RaiseValidationError();
                            cmbShelf.Focus();
                        }
                    }
                    else
                    {
                        cmbRack.TextBox.SetValidation("Please Select Rack");
                        cmbRack.TextBox.RaiseValidationError();
                        cmbRack.Focus();
                    }
                }
            }
            else
            {
                cmbStore.TextBox.SetValidation("Please Select Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
            }



            //if (chkAllStore.IsChecked == true || ((clsStoreVO)cmbStore.SelectedItem).StoreId > 0)
            //{
            //    if (((MasterListItem)cmbRack.SelectedItem).ID > 0)
            //    {
            //        if (((MasterListItem)cmbShelf.SelectedItem).ID > 0)
            //        {
            //            if (((MasterListItem)cmbBin.SelectedItem).ID > 0)
            //            {
            //                string msgTitle = "Palash";
            //                msgText = "Are you sure you want to Modify Item Location ?";
            //                MessageBoxControl.MessageBoxChildWindow msgW =
            //                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedModify);
            //                msgW.Show();
            //            }
            //            else
            //            {
            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                new MessageBoxControl.MessageBoxChildWindow("", "Please Select Bin", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //                msgW1.Show();
            //            }
            //        }
            //        else
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //         new MessageBoxControl.MessageBoxChildWindow("", "Please Select Shelf", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

            //            msgW1.Show();
            //        }
            //    }
            //    else
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("", "Please Select Rack", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

            //        msgW1.Show();
            //    }
            //}
            //else
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //          new MessageBoxControl.MessageBoxChildWindow("", "Please Select Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

            //    msgW1.Show();
            //}
        }
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                cmbRack.SelectedValue = Convert.ToInt64(((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).RackID);
                cmbShelf.SelectedValue = Convert.ToInt64(((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).ShelfID);
                cmbBin.SelectedValue = Convert.ToInt64(((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).ContainerID);
                cmbStore.SelectedValue = Convert.ToInt64(((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).StoreID);
                cmdModify.Visibility = Visibility.Visible;
                cmdSave.Visibility = Visibility.Collapsed;


                //chkAllStore.IsEnabled = false;
                //UserStoreAssigned = false;
                //long a = Convert.ToInt64(((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).StoreID);
                //clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
                ////False when we want to fetch all items
                //clsItemMasterVO obj = new clsItemMasterVO();
                //obj.RetrieveDataFlag = false;
                //BizActionObj.ItemMatserDetails = new List<clsStoreVO>();

                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                //client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                //client.ProcessCompleted += (s, args) =>
                //{
                //    if (args.Error == null && args.Result != null)
                //    {
                //        BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;

                //        clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };

                //        BizActionObj.ItemMatserDetails.Insert(0, Default);

                //        var result1 = from item in BizActionObj.ItemMatserDetails
                //                      where item.Status == true
                //                      select item;

                //        var result = from item in BizActionObj.ItemMatserDetails
                //                     where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                //                     select item;

                //        //List<clsStoreVO> UserStores = new List<clsStoreVO>();
                //        List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                //        UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                //        UserStores.Clear();

                //        foreach (var item in UserUnit)
                //        {
                //            if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                //            {    //item.StoreDetails       
                //                UserStores = item.UserUnitStore;

                //                //if (item.IsDefault == true)
                //                //{
                //                //    //item.UserUnitStore.Insert(0, Default);
                //                //    UserStores = item.UserUnitStore;
                //                //    break;
                //                //}
                //                //else if (item.IsDefault == false)
                //                //{
                //                //    //item.UserUnitStore.Insert(0, Default);
                //                //    UserStores = item.UserUnitStore;
                //                //    break;
                //                //}
                //            }
                //        }
                //        //UserStores.Insert(0, Default);
                //        var resultnew = from item in UserStores select item;
                //        if (UserStores != null && UserStores.Count > 0)
                //        {
                //            UserStoreAssigned = true;
                //        }

                //        List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                //        StoreListForClinic.Insert(0, Default);

                //        //List<clsStoreVO> StoreListForClinicUser = (List<clsStoreVO>)resultnew.ToList(); //(List<clsStoreVO>)resultnew.ToList(); 
                //        // StoreListForClinicUser.Insert(0, Default);
                //        // cmbToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                //        cmbStore.ItemsSource = result1.ToList();


                //        if (result1.ToList().Count > 0)
                //        {

                //            cmbStore.SelectedItem = result1.ToList()[(int)a];
                //        }
                //        //cmbAddToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;

                //        //if (StoreListForClinicUser.Count > 0)
                //        //    cmbAddFromStoreName.SelectedItem = StoreListForClinicUser[0]; //StoreListForClinic[0];
                //        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                //        {
                //            cmbStore.ItemsSource = result1.ToList();
                //            cmbStore.ItemsSource = BizActionObj.ItemMatserDetails;
                //            cmbStore.ItemsSource = result1.ToList();
                //            if (result1.ToList().Count > 0)
                //            {
                //                cmbStore.SelectedItem = (result1.ToList())[(int)a];

                //            }
                //        }

                //        else
                //        {
                //            //Commented By Harish on 17 Apr
                //            //cmbFromStoreName.ItemsSource = (List<clsStoreVO>)result.ToList();
                //            //cmbAddFromStoreName.ItemsSource = (List<clsStoreVO>)result.ToList();

                //            //cmbAddFromStoreName.ItemsSource = StoreListForClinicUser; //StoreListForClinic;
                //            // Rohit
                //            //cmbAddFromStoreName.ItemsSource = result1.ToList();
                //            cmbStore.ItemsSource = StoreListForClinic;

                //            if (StoreListForClinic.Count > 0)
                //            {
                //                cmbStore.SelectedItem = StoreListForClinic[(int)a];

                //            }
                //            // End
                //        }
                //    }
                //    //FillUOM();
                //};

                //client.CloseAsync();

                //cmbRack.SelectedValue = Convert.ToInt64(((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).RackID);
                //cmbShelf.SelectedValue = Convert.ToInt64(((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).ShelfID);
                //cmbBin.SelectedValue = Convert.ToInt64(((ItemStoreLocationDetailsVO)dgItemList.SelectedItem).ContainerID);
                //cmdModify.Visibility = Visibility.Visible;
                //cmdSave.Visibility = Visibility.Collapsed;

            }
        }
        void SetupPage()
        {
            if (objWIndicator == null) objWIndicator = new WaitIndicator();
            objWIndicator.Show();
            GetItemStoreLocationDetailsBizActionVO BizAction = new GetItemStoreLocationDetailsBizActionVO();
            BizAction.ObjItemStoreLocationDetails = new ItemStoreLocationDetailsVO();
            BizAction.ObjItemStoreLocationDetails.ItemID = objMasterVO.ID;
            BizAction.ObjItemStoreLocationDetails.UnitID = objMasterVO.UnitID;
            //BizAction.IsPagingEnabled = true;
            //BizAction.MaximumRows = MasterList.PageSize;
            //BizAction.StartIndex = MasterList.PageIndex * MasterList.PageSize;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.ItemStoreLocationDetailslist = (((GetItemStoreLocationDetailsBizActionVO)args.Result).ItemStoreLocationDetailslist);
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((GetItemStoreLocationDetailsBizActionVO)args.Result).TotalRows);
                        foreach (ItemStoreLocationDetailsVO item in BizAction.ItemStoreLocationDetailslist)
                        {
                            MasterList.Add(item);
                        }
                        this.dgItemList.DataContext = MasterList;
                    }
                    objWIndicator.Close();
                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                objWIndicator.Close();
            }
        }


        private void chkStore_Click(object sender, SelectionChangedEventArgs e)
        {
            //ItemStoreLocationDetailsVO selItem = new ItemStoreLocationDetailsVO();
            //selItem.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;

            //foreach (ItemStoreLocationDetailsVO item in lstItemStore)
            //{
            //    if (item == selItem)
            //    {
            //        item.IsSelected = true;

            //    }
            //    else
            //    {
            //        item.IsSelected = false;

            //    }
            //}
            //dgStoreList.ItemsSource = null;
            //dgStoreList.ItemsSource = lstItemStore;
            //dgStoreList.UpdateLayout();
        }

        private void chkAllStore_Click(object sender, RoutedEventArgs e)
        {
            if (chkAllStore.IsChecked == true)
            {
                cmbStore.IsEnabled = false;
                ItemStoreLocationDetailsBizActionVo ItemBizActionObj = new ItemStoreLocationDetailsBizActionVo();

                UserStoreAssigned = false;
                clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
                //False when we want to fetch all items
                clsItemMasterVO obj = new clsItemMasterVO();
                obj.RetrieveDataFlag = false;
                BizActionObj.ItemMatserDetails = new List<clsStoreVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;

                        // clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };

                        //BizActionObj.ItemMatserDetails.Insert(0, Default);

                        var result1 = from item in BizActionObj.ItemMatserDetails
                                      where item.Status == true
                                      select item;

                        var result = from item in BizActionObj.ItemMatserDetails
                                     where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                     select item;

                        //List<clsStoreVO> UserStores = new List<clsStoreVO>();
                        List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                        UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                        UserStores.Clear();

                        foreach (var item in UserUnit)
                        {
                            if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                            {    //item.StoreDetails       
                                UserStores = item.UserUnitStore;

                                //if (item.IsDefault == true)
                                //{
                                //    //item.UserUnitStore.Insert(0, Default);
                                //    UserStores = item.UserUnitStore;
                                //    break;
                                //}
                                //else if (item.IsDefault == false)
                                //{
                                //    //item.UserUnitStore.Insert(0, Default);
                                //    UserStores = item.UserUnitStore;
                                //    break;
                                //}
                            }
                        }
                        //UserStores.Insert(0, Default);
                        tempSelectAll = new List<clsStoreVO>();
                        var resultnew = from item in UserStores select item;
                        if (UserStores != null && UserStores.Count > 0)
                        {
                            UserStoreAssigned = true;
                        }

                        List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                        //StoreListForClinic.Insert(0, Default);

                        //List<clsStoreVO> StoreListForClinicUser = (List<clsStoreVO>)resultnew.ToList(); //(List<clsStoreVO>)resultnew.ToList(); 
                        // StoreListForClinicUser.Insert(0, Default);
                        // cmbToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                        ItemBizActionObj.ClsStoreVOList = result1.ToList();


                        //if (result1.ToList().Count > 0)
                        //{

                        //    cmbStore.SelectedItem = result1.ToList()[0];
                        //}
                        //cmbAddToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;

                        //if (StoreListForClinicUser.Count > 0)
                        //    cmbAddFromStoreName.SelectedItem = StoreListForClinicUser[0]; //StoreListForClinic[0];
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                        {
                            ItemBizActionObj.ClsStoreVOList = result1.ToList();
                            ItemBizActionObj.ClsStoreVOList = BizActionObj.ItemMatserDetails;
                            ItemBizActionObj.ClsStoreVOList = result1.ToList();
                            //if (result1.ToList().Count > 0)
                            //{
                            //    cmbStore.SelectedItem = (result1.ToList())[0];

                            //}
                            tempSelectAll = ItemBizActionObj.ClsStoreVOList;
                            foreach (var item in tempSelectAll.ToList())
                            {
                                foreach (var item1 in this.MasterList.ToList())
                                {
                                    if (item1.StoreID == item.StoreId)
                                    {
                                        tempSelectAll.Remove(item);
                                    }
                                }
                            }
                        }

                        else
                        {
                            //Commented By Harish on 17 Apr
                            //cmbFromStoreName.ItemsSource = (List<clsStoreVO>)result.ToList();
                            //cmbAddFromStoreName.ItemsSource = (List<clsStoreVO>)result.ToList();

                            //cmbAddFromStoreName.ItemsSource = StoreListForClinicUser; //StoreListForClinic;
                            // Rohit
                            //cmbAddFromStoreName.ItemsSource = result1.ToList();
                            //cmbStore.ItemsSource = StoreListForClinic;

                            //if (StoreListForClinic.Count > 0)
                            //{
                            //    cmbStore.SelectedItem = StoreListForClinic[0];

                            //}
                            // End
                        }
                    }
                    //FillUOM();
                };

                client.CloseAsync();
            }
            else
            {
                cmbStore.IsEnabled = true;
                tempSelectAll.Clear();

            }
        }
    }
}

