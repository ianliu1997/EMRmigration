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
using CIMS;
using System.Collections.ObjectModel;
using System.Windows.Browser;
using System.Reflection;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class PurchaseorderList : UserControl
    {
        List<long> SelectedItemsList = new List<long>();
        public ObservableCollection<MasterListItem> ItemList { get; set; }
        public PurchaseorderList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillSupplier();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            FillClinic();
        }

        private void FillSupplier()
        {
            try
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
                        List<MasterListItem> ObjList1 = new List<MasterListItem>();
                        ObjList1.Add(new MasterListItem { ID = 0, Description = "--All Suppliers--" });
                        ObjList1.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = ObjList1;
                        // cmbSupplier.SelectedItem = ObjList1[0];


                        //if ((MasterListItem)cmbSupplier.ItemsSource != null)
                        //{

                        List<MasterListItem> objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        if (ItemList == null)
                            ItemList = new ObservableCollection<MasterListItem>();
                        foreach (MasterListItem item in objList)
                        {
                            item.Status = true;
                            ItemList.Add(item);
                        }

                        //lstSupplier.ItemsSource = null;
                        //lstSupplier.ItemsSource = ItemList;
                        cmbSupplier.SelectedItem = ObjList1[0];
                        //}
                    }


                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
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
                                      where r.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
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
                    //BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == clinicId && item.Status == true//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    List<clsStoreVO> ClinicWiseStores = new List<clsStoreVO>();
                    ClinicWiseStores = result.ToList();
                    ClinicWiseStores.Insert(0, Default);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cmbStore.ItemsSource = ClinicWiseStores.ToList();
                        }
                        cmbStore.SelectedItem = ClinicWiseStores.ToList()[0];
                    }
                    else
                    {
                        if (BizActionObj.ToStoreList.ToList().Count > 0)
                        {
                            cmbStore.ItemsSource = BizActionObj.ToStoreList.ToList();
                        }
                        cmbStore.SelectedItem = BizActionObj.ToStoreList.ToList()[0];
                    }
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtpF = null;
            //DateTime? dtpT = null;
            //DateTime? dtpP = null;

            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;
            long UnitID = 0;
            long StoreID = 0;

            if (cmbClinic.SelectedItem != null)
                UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (cmbStore.SelectedItem != null)
                StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;


            long UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

            string msgTitle = "";
            bool chkToDate = true;

            if (dtpFromDate.SelectedDate != null)
            {
                dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtpT = dtpToDate.SelectedDate.Value.Date.Date;
                if (dtpF > dtpT)
                {
                    dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                    dtpT = dtpF;
                    chkToDate = false;
                }
                else
                {
                    dtpP = dtpT;
                    dtpT = dtpT.Value.Date.AddDays(1);
                    dtpToDate.Focus();
                }
            }

            if (dtpT != null)
            {
                if (dtpF != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
                    //  if (dtpF.Equals(dtpT))
                    //dtpT = dtpT.Value.Date.AddDays(1);
                }
            }

            //List<MasterListItem> objList = ItemList.ToList<MasterListItem>();

            //if (objList != null)
            //{
            //    foreach (var item in objList)
            //    {
            //        if (item.Status == true)
            //        {
            //            SelectedItemsList.Add(item.ID);
            //        }
            //    }
            //}

            //string SupplierIDs = "0";
            //if (SelectedItemsList != null)
            //{
            //    for (int i = 0; i <= SelectedItemsList.Count - 1; i++)
            //    {
            //        SupplierIDs = SupplierIDs + (SupplierIDs == "" ? (SelectedItemsList[i].ToString()) : "," + (SelectedItemsList[i].ToString()));
            //    }
            //}
            //SelectedItemsList.Clear();


            #region added by priyanka
            string SupplierIDs = "";


            //if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
            //{
            //    SupplierIDs = null;
            //}
            //else 
            if (ItemList != null && ItemList.Count > 0)
            {
                foreach (var item in ItemList)
                {
                    SupplierIDs = SupplierIDs + item.ID;
                    SupplierIDs = SupplierIDs + ",";
                }

                if (SupplierIDs.EndsWith(","))
                    SupplierIDs = SupplierIDs.Remove(SupplierIDs.Length - 1, 1);

            }

            #endregion


            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/InventoryPharmacy/PurchaseOrderList.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&SupplierIDs=" + SupplierIDs + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&UnitID=" + UnitID + "&Excel=" + chkExcel.IsChecked + "&UserID=" + UserID + "&StoreID=" + StoreID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/InventoryPharmacy/PurchaseOrderList.aspx?SupplierIDs=" + SupplierIDs + "&UnitID=" + UnitID + "&Excel=" + chkExcel.IsChecked + "&UserID=" + UserID + "&StoreID=" + StoreID;
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

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSupplier.SelectedItem != null && ((MasterListItem)cmbSupplier.SelectedItem).ID > 0)
            {
                MasterListItem tempstore = new MasterListItem();
                var item1 = from r in ItemList
                            where (r.ID == ((MasterListItem)cmbSupplier.SelectedItem).ID)
                            select new MasterListItem
                            {
                                Status = r.Status,
                                ID = r.ID,
                                Description = r.Description
                            };

                if (item1.ToList().Count == 0)
                {

                    tempstore.ID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    tempstore.Description = ((MasterListItem)cmbSupplier.SelectedItem).Description;
                    tempstore.Status = true;
                    ItemList.Add(tempstore);

                    lstSupplier.ItemsSource = null;
                    lstSupplier.ItemsSource = ItemList;

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Supplier already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            }
            else
            {
                if (cmbSupplier.SelectedItem != null)
                {
                    FillSupplier();
                }
            }

        }

        private void chkSupplier_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSupplier.SelectedItem != null && ItemList.Count > 0)
                {
                    CheckBox chk = (CheckBox)sender;
                    if (chk.IsChecked == false)
                    {
                        ItemList.Remove((MasterListItem)lstSupplier.SelectedItem);
                    }


                    // ItemList.RemoveAt(lstSupplier.SelectedIndex);
                    //foreach (var item in ItemList.ToList())
                    //  {
                    //      if (item.ID == ((MasterListItem)lstSupplier.SelectedItem).ID)
                    //      {
                    //          ItemList.Remove(item);
                    //      }
                    //  }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbSupplier.SelectedItem).ID != 0)
            {

                //List<MasterListItem> objtempList=new List<MasterListItem>();
                //objtempList =((ObservableCollection<MasterListItem>)lstSupplier.ItemsSource).ToList();
                //if (objtempList != null && ((MasterListItem)cmbSupplier.SelectedItem).ID != 0)
                //{

                //    foreach (var item in lstSupplier.ItemsSource)
                //    {
                //        if (((MasterListItem)item).ID == ((MasterListItem)cmbSupplier.SelectedItem).ID)
                //        {
                //            objtempList.Add((MasterListItem)item);
                //        }
                //    }
                //}


                //ItemList = new ObservableCollection<MasterListItem>();

                //lstSupplier.ItemsSource = null;
                //lstSupplier.ItemsSource = objtempList;
            }
            try
            {
                if (cmbSupplier.SelectedItem != null)
                {
                    if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
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
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null)
                {
                    FillStores(((MasterListItem)cmbClinic.SelectedItem).ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
