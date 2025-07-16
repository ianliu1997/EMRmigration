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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using CIMS;

namespace PalashDynamics.Pharmacy
{
    public partial class ItemStore : ChildWindow
    {
        Boolean IsPageLoded = false;
        clsItemMasterVO objMasterVO = null;
        public bool flagClickStore = false;
        public List<clsItemStoreVO> ItemStoreList;
        public List<clsItemMasterVO> lstItemStore;
        public List<long> lstStore = new List<long>();
        public List<clsItemStoreVO> TestList { get; set; }
        public Boolean IsEditMode { get; set; }
        public long pkID { get; set; }

        public ItemStore()
        {
            InitializeComponent();
            this.DataContext = new clsItemMasterVO();
        }
       
        private void chkAllClinic_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkAllClinic_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                lblItemName.Text = "";
                if (objMasterVO.ItemName != null)
                {
                    lblItemName.Text = objMasterVO.ItemName.ToString();
                }


                FillClinic();
                ((clsItemMasterVO)this.DataContext).ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //cboItemClinic.SelectedValue = ((clsItemMasterVO)this.DataContext).ClinicID;

                //FillStoreList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);



            }
            IsPageLoded = true;

        }
       
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

                       
                       
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        dgClinicList.ItemsSource = null;
                        dgClinicList.ItemsSource = objList;
                        dgClinicList.SelectedIndex = (int)((clsItemMasterVO)this.DataContext).ClinicID;
                        

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
        private void FillStoreList(long clinicID)
        {
            try
            {
                long clnID = clinicID;
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Store;
                BizAction.MasterList = new List<MasterListItem>();
                if (clinicID != 0)
                {
                    BizAction.Parent = new KeyValue { Key = clinicID, Value = "ClinicID" };
                }

                //BizAction.Parent.Key = clinicID;
                //BizAction.Parent.Value = "ClinicId";

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        List<MasterListItem> uList = new List<MasterListItem>();
                        uList = ((clsGetMasterListBizActionVO)ea.Result).MasterList;

                       dgStoreList.ItemsSource = null;

                        ItemStoreList = new List<clsItemStoreVO>();

                        foreach (var item in uList)
                        {

                            bool status;
                            if (clnID == 0)
                            {
                                status = false;
                            }
                            else
                            {
                                status = item.Status;
                            }
                            ItemStoreList.Add(new clsItemStoreVO { ID = item.ID, StoreName = item.Description, status = status, UnitId = item.ID });
                        }

                        dgStoreList.ItemsSource = ItemStoreList;

                        GetItemStoreList();

                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
                client = null;

            }
            catch (Exception ex)
            {

            }


        }
        private void GetItemStoreList()
        {
            try
            {


                clsGetItemClinicBizActionVO BizActionObj = new clsGetItemClinicBizActionVO();
                //False when we want to fetch all items
                long clinicid = 0;
                clsItemMasterVO obj = new clsItemMasterVO();
                obj.RetrieveDataFlag = false;
                BizActionObj.ItemDetails = new clsItemMasterVO();
                BizActionObj.ItemDetails.RetrieveDataFlag = false;
                BizActionObj.ItemDetails.ItemID = objMasterVO.ID;


                BizActionObj.ItemDetails.ClinicID = ((MasterListItem)dgClinicList.SelectedItem) == null ? 0 : ((MasterListItem)dgClinicList.SelectedItem).ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        lstItemStore = null;
                        lstItemStore = new List<clsItemMasterVO>();
                        lstItemStore = ((clsGetItemClinicBizActionVO)(args.Result)).ItemList;
                        CheckExistingStores();

                    }
                    else
                    {
                        HtmlPage.Window.Alert("Error while getting list");
                    }

                };

                client.ProcessAsync(BizActionObj, new clsUserVO());
                client.CloseAsync();
                // return lstItemStore;



            }
            catch (Exception ex)
            {

                throw;
            }
        }
       
        public void GetItemDetails(ValueObjects.Inventory.clsItemMasterVO objItemVO)
        {
            objMasterVO = objItemVO;
        }
        private void CheckExistingStores()
        {

            List<clsItemStoreVO> objList = (List<clsItemStoreVO>)dgStoreList.ItemsSource;

            if (ItemStoreList != null)
            {
                if (objList != null && objList.Count > 0)
                {
                    if (lstItemStore != null && lstItemStore.Count > 0)
                    {
                        foreach (var item in lstItemStore)
                        {
                            foreach (var items in objList)
                            {
                                if (items.ID == item.StoreID)
                                {
                                    items.status = item.Status;
                                    items.ItemClinicID = item.ClinicID ;
                                }
                            }
                        }
                        //foreach (var item in objList)
                        //{
                        //    foreach (var items in lstItemStore)
                        //    {
                        //        if (item.ID==items.StoreID)
                        //        {
                        //            item.status = true;
                        //        }
                        //        else
                        //        {
                        //            item.status = false;
                        //        }
                        //    }

                        //}  
                    }
                }
                dgStoreList.ItemsSource = null;
                dgStoreList.ItemsSource = objList;



            }


        }
        public clsAddItemClinicBizActionVO BizActionobj;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {  
            try
            {


                BizActionobj = new clsAddItemClinicBizActionVO();

                BizActionobj.ItemStore = new clsItemStoreVO();
                BizActionobj.ItemStore.StoreList = new List<clsItemStoreVO>();
               

                BizActionobj.ItemStore.StoreList = ((List<clsItemStoreVO>)dgStoreList.ItemsSource);
                //BizActionobj.ItemStore.ID = ;
                //BizActionobj.ItemStore.status = true;
                BizActionobj.ItemStore.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionobj.ItemStore.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionobj.ItemStore.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizActionobj.ItemStore.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                BizActionobj.ItemStore.AddedDateTime = DateTime.Now;

                BizActionobj.ItemStore.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                
                BizActionobj.ItemStore.ItemID = objMasterVO.ID;
                BizActionobj.ItemStore.UnitID = ((MasterListItem)dgClinicList.SelectedItem).ID;//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                //BizActionobj.ItemMatserDetails.ID = pkID;
                string msgTitle = "";

                string msgText = "";


                if (IsEditMode == false)
                {
                    msgText = "Are you sure you want to Save the  Details";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
                else
                    if (!flagClickStore)
                    {
                        flagClickStore = false;
                        if (objMasterVO != null)
                        {


                            if (((clsItemMasterVO)this.DataContext).EditMode == false)
                            {
                                msgText = "Do you want to apply supplier ?";

                            }
                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                                  new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);

                            msgW2.Show();


                        }
                    }













                //}
            }
            catch (Exception)
            {

                throw;
            }

        }
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
           
        }
        void msgW2_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    ItemSupplier win = new ItemSupplier();
                    win.Show();
                    win.GetItemDetails(objMasterVO);
                    this.Close();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {




                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessAsync(BizActionobj, new clsUserVO());



                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {

                            //IF DATA IS SAVED
                            if (((PalashDynamics.ValueObjects.Inventory.clsAddItemClinicBizActionVO)(arg.Result)).SuccessStatus == 1)
                            {

                                string msgTitle = "";
                                string msgText = "Item Store Saved Successfully for " + ((MasterListItem)dgClinicList.SelectedItem).Description;

                                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                                msgW.Show();
                                //this.Close();

                            }



                        }





                    };


                    client.CloseAsync();
                }

                //clsItemMasterVO objItemVO = new clsItemMasterVO();
                //objItemVO = (clsItemMasterVO)dgItemList.SelectedItem;
              
            }
            catch (Exception)
            {

                throw;
            }


        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!flagClickStore)
            {
                flagClickStore = false;
                if (objMasterVO != null)
                {
                    string msgText = "";

                    if (((clsItemMasterVO)this.DataContext).EditMode == false)
                    {
                        msgText = "Do you want to apply supplier ?";

                    }
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                          new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed);

                    msgW2.Show();


                }
               
                //if (objMasterVO != null)
                //{
                //    ItemSupplier win = new ItemSupplier();
                //    win.Show();
                //    win.GetItemDetails(objMasterVO);
                //    this.Close();
                //}

            }
            this.DialogResult = false;
        }

        private void chkStores_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkStores_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkClinic_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkClinic_Unchecked(object sender, RoutedEventArgs e)
        {
           

        }

        private void chkAllStores_Checked(object sender, RoutedEventArgs e)
        {
            try
            {


                List<clsItemStoreVO> objItemStoreList = new List<clsItemStoreVO>();
                objItemStoreList = (List<clsItemStoreVO>)dgStoreList.ItemsSource;
                if (objItemStoreList != null)
                {
                    foreach (var item in objItemStoreList)
                    {

                        item.status = true;
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void chkAllStores_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {


                List<clsItemStoreVO> objItemStoreList = new List<clsItemStoreVO>();
                objItemStoreList = (List<clsItemStoreVO>)dgStoreList.ItemsSource;
                if (objItemStoreList != null)
                {
                    foreach (var item in objItemStoreList)
                    {

                        item.status = false;
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }

        }

       

        private void chkClinic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkStore_Click(object sender, RoutedEventArgs e)
        {


            //((clsItemStoreVO)dgStoreList.SelectedItem).StoreStatus = true;
            if (((clsItemStoreVO)dgStoreList.SelectedItem) != null)

              ((clsItemStoreVO)dgStoreList.SelectedItem).status = true;
        }

        private void dgClinicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chkAllStores.IsChecked = false;
            if (((MasterListItem)dgClinicList.SelectedItem).ID > 0)
            {
                long clinicID = ((MasterListItem)dgClinicList.SelectedItem).ID;


                FillStoreList(clinicID);



            }
        }

        private void chkStore_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dgStoreList.SelectedItem !=null)
                ((clsItemStoreVO)dgStoreList.SelectedItem).status = false;
        }
    }
}
