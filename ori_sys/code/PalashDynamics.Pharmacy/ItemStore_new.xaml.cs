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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.UserControls;

namespace PalashDynamics.Pharmacy
{
    public partial class ItemStore_new : ChildWindow
    {
        Boolean IsPageLoded = false;
        public clsItemMasterVO objMasterVO;
        public bool flagClickStore = false;
        public List<clsItemStoreVO> ItemStoreList;
        public List<clsItemMasterVO> lstItemStore;
        public List<clsItemStoreVO> ExistingStoreItems;
        public List<long> lstStore = new List<long>();
        public List<clsItemStoreVO> TestList { get; set; }
        List<clsItemTaxVO> ItemTaxList = null;
        public Boolean IsEditMode { get; set; }
        public long pkID { get; set; }
        private List<clsStoreStaus> StoreStatus { get; set; }
        public List<clsItemStoreVO> SelectedStoreList = null;
        public List<clsItemStoreVO> DeletedStoreList = null;
        List<MasterListItem> objList = new List<MasterListItem>();
        public string msgText = String.Empty;
        public ItemStore_new()
        {
            InitializeComponent();
            this.DataContext = new clsItemMasterVO();
            StoreStatus = new List<clsStoreStaus>();
            SelectedStoreList = new List<clsItemStoreVO>();
            DeletedStoreList = new List<clsItemStoreVO>();
            //  objSelectedTax = ((clsItemTaxVO)dgTaxList.SelectedItem);
        }
        public bool IsFromNewItem;
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //objMasterVO = new clsItemMasterVO();
            if (!IsPageLoded)
            {
                if (IsFromNewItem == false)
                {
                    lblItemName.Text = "";
                    if (objMasterVO.ItemName != null)
                    {
                        lblItemName.Text = "Item Name: " + objMasterVO.ItemName.ToString();
                    }
                }
                FillClinic();
                FillTax();
                ClearTaxUI();
            }
            IsPageLoded = true;
            count = 0;
        }

        private void dgTaxList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void chkStore_Click_1(object sender, RoutedEventArgs e)
        {

        }
        private void FillStoreList(long clinicID)
        {
            try
            {
                long clnID = clinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Store;
                BizAction.MasterList = new List<MasterListItem>();
                if (clinicID != 0)
                {
                    BizAction.Parent = new KeyValue { Key = clinicID, Value = "ClinicID" };
                }
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
                            bool status = false;
                            if (clnID == 0)
                            {
                                status = false;
                            }
                            else
                            {
                                status = item.Status;
                            }
                            ItemStoreList.Add(new clsItemStoreVO { StoreID = item.ID, StoreName = item.Description, status = status, UnitId = clinicID });
                        }
                        dgStoreList.ItemsSource = ItemStoreList;
                        dgTaxList.ItemsSource = null;
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
        bool bStatus = false;
        bool cStatus = false;
        private void GetItemStoreList()
        {
            try
            {
                clsGetItemClinicBizActionVO BizActionObj = new clsGetItemClinicBizActionVO();
                long clinicid = 0;
                clsItemMasterVO obj = new clsItemMasterVO();
                obj.RetrieveDataFlag = false;
                BizActionObj.ItemDetails = new clsItemMasterVO();
                BizActionObj.ItemDetails.RetrieveDataFlag = false;
                if (objMasterVO != null)
                    BizActionObj.ItemDetails.ItemID = objMasterVO.ID;
                BizActionObj.ItemDetails.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && ((clsGetItemClinicBizActionVO)(args.Result)) != null)
                    {
                        lstItemStore = new List<clsItemMasterVO>();
                        lstItemStore = ((clsGetItemClinicBizActionVO)(args.Result)).ItemList;
                        if (lstItemStore != null)
                        {
                            foreach (var item in lstItemStore)
                            {
                                foreach (var item1 in ItemStoreList)
                                {
                                    if (item.ItemID == objMasterVO.ID && item1.StoreID == item.StoreID && item.Status == true)
                                    {
                                        item1.ID = item.ID;
                                        item1.Min = item.Min;
                                        item1.Max = item.Max;
                                        item1.Re_Order = item.Re_Order;
                                        item1.status = true;
                                        OKButton.Content = "Modify Store";
                                        bStatus = true;

                                    }
                                }
                            }
                            if (lstItemStore.Count == ItemStoreList.Count)
                            {
                                bool flag = false;
                                foreach (var item in ItemStoreList)
                                {
                                    if (item.status == false)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (!flag)
                                    chkAllStores.IsChecked = true;
                                else
                                    chkAllStores.IsChecked = false;
                            }
                            else
                            {
                                chkAllStores.IsChecked = false;
                            }
                        }
                        else
                        {
                            OKButton.Content = "Apply Store";
                            cStatus = true;
                        }
                        dgStoreList.ItemsSource = null;
                        dgStoreList.ItemsSource = ItemStoreList;
                        dgStoreList.SelectedIndex = 0;
                        foreach (clsItemStoreVO store in ItemStoreList)
                        {
                            if (store.status == true)
                            {
                                SelectedStoreList.Add(store);
                            }
                        }
                        if (SelectedStoreList.Count > 0)
                            //  { SelectedStoreList.RemoveAt(0); }
                            cmbStore.ItemsSource = null;
                        SelectedStoreList.Insert(0, new clsItemStoreVO { StoreID = 0, StoreName = "--Select--", status = true, UnitId = 0 });
                        cmbStore.ItemsSource = SelectedStoreList.ToList();
                        cmbStore.SelectedItem = SelectedStoreList[0];
                        CheckForExistingTax(true);
                    }
                    else
                    {
                        msgText = "Error occurred while processing.";
                        HtmlPage.Window.Alert(msgText);
                    }
                };
                client.ProcessAsync(BizActionObj, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void FillDataGrid()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TaxMaster;
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
                        objList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        ItemTaxList = new List<clsItemTaxVO>();
                        clsItemTaxVO obj = new clsItemTaxVO();

                        foreach (var item in objList)
                        {
                            ItemTaxList.Add(new clsItemTaxVO
                            {
                                ID = item.ID,
                                TaxName = item.Description,
                                ApplicableForList = obj.ApplicableForList,
                                ApplicableOnList = obj.ApplicableOnList,
                                status = item.Status
                            });
                        }
                        dgTaxList.ItemsSource = null;
                        dgTaxList.ItemsSource = ItemTaxList;
                        if (dgStoreList.SelectedItem != null)
                        {
                            GetItemTaxList();
                        }
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
        List<clsItemTaxVO> _ItemTax = null;
        private void GetItemTaxList()
        {
            clsGetItemClinicDetailBizActionVO objBizActionVO = new clsGetItemClinicDetailBizActionVO();
            objBizActionVO.ItemClinicId = ((clsItemStoreVO)dgStoreList.SelectedItem).ID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    _ItemTax = null;
                    _ItemTax = new List<clsItemTaxVO>();
                    _ItemTax = ((clsGetItemClinicDetailBizActionVO)args.Result).ItemTaxList;
                    CheckForExistingTax(false);
                }
            };
            client.ProcessAsync(objBizActionVO, new clsUserVO());
            client.CloseAsync();
        }

        private void CheckForExistingTax(bool IsForAllStore)
        {
            try
            {
                clsGetItemStoreTaxListBizActionVO BizAction = new clsGetItemStoreTaxListBizActionVO();
                BizAction.StoreItemTaxList = new List<clsItemTaxVO>();
                BizAction.StoreItemTaxDetails = new clsItemTaxVO();
                if (dgStoreList.SelectedItem != null)
                    BizAction.StoreItemTaxDetails.ID = ((clsItemStoreVO)dgStoreList.SelectedItem).StoreID;
                BizAction.StoreItemTaxDetails.ItemID = objMasterVO.ID;

                BizAction.IsForAllStore = IsForAllStore;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = (clsGetItemStoreTaxListBizActionVO)e.Result;
                        BizAction.StoreItemTaxList = ((clsGetItemStoreTaxListBizActionVO)e.Result).StoreItemTaxList;
                        objTaxList.Clear();

                        foreach (var item in BizAction.StoreItemTaxList)
                        {
                            if (dgStoreList.SelectedItem != null && item.StoreID == ((clsItemStoreVO)dgStoreList.SelectedItem).StoreID)
                                objTaxList.Add(item);
                        }
                        dgTaxList.ItemsSource = null;
                        dgTaxList.ItemsSource = objTaxList;


                        if (BizAction.IsForAllStore == true)
                        {
                            List<clsItemStoreVO> lstStore = dgStoreList.ItemsSource as List<clsItemStoreVO>;

                            foreach (clsItemStoreVO item2 in lstStore)
                            {
                                item2.objStoreTaxList = new List<clsItemTaxVO>();

                                foreach (var item in BizAction.StoreItemTaxList)
                                {
                                    if (item2.StoreID == item.StoreID)
                                    {
                                        item2.objStoreTaxList.Add(item);
                                    }
                                }
                            }
                        }
                        //else
                        //{
                        //    List<clsItemStoreVO> lstStore = dgStoreList.ItemsSource as List<clsItemStoreVO>;

                        //    foreach (clsItemStoreVO item21 in lstStore)
                        //    {
                        //        foreach (clsItemStoreVO item23 in SelectedStoreList)
                        //        {
                        //            if (dgStoreList.SelectedItem != null && item21.StoreID == ((clsItemStoreVO)dgStoreList.SelectedItem).StoreID && item23.StoreID == item21.StoreID)
                        //            {
                        //                //item21.objStoreTaxList = item23.objStoreTaxList;

                        //                dgTaxList.ItemsSource = null;
                        //                dgTaxList.ItemsSource = item23.objStoreTaxList.DeepCopy();
                        //                dgTaxList.UpdateLayout();
                        //                break;
                        //            }

                        //        }

                        //    }
                        //}

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

        private void ApplyStore()
        {
            try
            {
                clsAddItemClinicBizActionVO BizActionobj = new clsAddItemClinicBizActionVO();
                BizActionobj.ItemStore = new clsItemStoreVO();
                BizActionobj.ItemStore.StoreList = new List<clsItemStoreVO>();
                BizActionobj.ItemTaxList = new List<clsItemTaxVO>();
                BizActionobj.ItemStore.DeletedStoreList = new List<clsItemStoreVO>();
                if (SaveStore == true)   //if else By Umesh
                {
                    BizActionobj.ItemTaxList.Clear();
                    if (SelectedStoreList.Where(z => z.StoreName.Equals("--Select--")).Any())
                    { SelectedStoreList.RemoveAt(0); }
                    BizActionobj.ItemStore.StoreList = SelectedStoreList;//((List<clsItemStoreVO>)dgStoreList.ItemsSource).Where(z=>z.status == true).ToList();
                    //if (SelectedStoreList.Count>0)
                    //foreach (var item in SelectedStoreList)
                    //{
                    //    if (DeletedStoreList.Where(z => z.StoreID.Equals(item.StoreID)).Any())
                    //    {
                    //        DeletedStoreList.Remove(item);
                    //    }
                    //}
                    BizActionobj.ItemStore.DeletedStoreList = DeletedStoreList;
                }
                else
                {
                    BizActionobj.ItemStore.StoreList.Clear();
                    BizActionobj.ItemTaxList = objTaxList;
                }
                BizActionobj.ItemStore.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionobj.ItemStore.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionobj.ItemStore.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizActionobj.ItemStore.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                BizActionobj.ItemStore.AddedDateTime = DateTime.Now;
                BizActionobj.ItemStore.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                BizActionobj.ItemStore.ItemID = objMasterVO.ID;
                BizActionobj.ItemStore.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                BizActionobj.IsForDelete = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddItemClinicBizActionVO)(arg.Result)).SuccessStatus == 1)
                        {
                            string msgtxt = "";
                            if (SaveStore == true)
                            {
                                if (cStatus == true)
                                {
                                    msgtxt = "Stores Applied Successfully";
                                    cStatus = false;
                                }
                                else
                                    msgtxt = "Stores Modified Successfully";

                            }
                            else
                            {
                                if (((clsAddItemClinicBizActionVO)(arg.Result)).ResultStatus == 2)
                                {
                                    msgtxt = "Tax Already Define For This Item";
                                }
                                else
                                {
                                    msgtxt = "Tax Added/Modify Successfully";
                                }
                            }
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("PALASH", msgtxt, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    ClearTaxUI();
                                    DeletedStoreList.Clear();
                                    dgStoreList_SelectionChanged(null, null);
                                }
                            };
                            msgW.Show();
                        }
                    }
                };
                client.ProcessAsync(BizActionobj, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool SaveStore = false;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bMin = CheckminMax();
                if (bMin)
                {
                    SaveStore = true;
                    if (SelectedStoreList != null && SelectedStoreList.Count > 0) //&& objTaxList != null  && objTaxList.Count > 0 && SelectedStoreList.Count > 0)
                    {
                        if (bStatus == true)
                        {
                            msgText = "Do you want to Update Stores for Item?";
                            bStatus = false;
                        }
                        else
                        {
                            msgText = "Do you want to Apply Selected Stores for Item?";
                        }
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                ApplyStore();
                                cmbStore.IsEnabled = true;
                                cmbStore.ItemsSource = null;
                                SelectedStoreList.Insert(0, new clsItemStoreVO { StoreID = 0, StoreName = "--Select--", status = true, UnitId = 0 });
                                cmbStore.ItemsSource = SelectedStoreList.ToList();
                                cmbStore.SelectedItem = SelectedStoreList[0];
                            }

                            //   CancelButton_Click(null, null);
                        };
                        msgW.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Select Atleast One Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                }
                else
                {
                    string text = "Minimum Quantity Should Be Less Than Maximum Quantity Or Reorder Should Be Between Minimum Quantity And Maximum Quantity";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow("", text, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    bMin = true;
                    msgW.Show();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void GetItemDetails(clsItemMasterVO objItemMasterVO)
        {
            objMasterVO = objItemMasterVO;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Subtract || e.Key == Key.Decimal && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleDecimal(sender, e);
            }
        }
        private void ViewerStore_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
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

        private void FillTax()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TaxMaster;
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbTax.ItemsSource = null;
                        cmbTax.ItemsSource = objList;
                        cmbTax.SelectedItem = objList[0];
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
        private bool CheckValidation()
        {
            foreach (var item in (List<clsItemTaxVO>)dgTaxList.ItemsSource)
            {
                if (item.status)
                {
                    if (item.ApplicableFor == null)
                    {
                        msgText = "Please Select Applicable For";
                        MessageBoxControl.MessageBoxChildWindow msgW5 = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW5.Show();
                        return false;
                    }
                    else if (item.ApplicableFor.ID == 0)
                    {
                        msgText = "Please Select Applicable For";
                        MessageBoxControl.MessageBoxChildWindow msgW2 = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW2.Show();
                        return false;
                    }
                    if (item.ApplicableOn == null)
                    {
                        msgText = "Please select applicable on.";
                        MessageBoxControl.MessageBoxChildWindow msgW3 = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        return false;
                    }
                    else if (item.ApplicableOn.ID == 0)
                    {
                        msgText = "Please select applicable on.";
                        MessageBoxControl.MessageBoxChildWindow msgW4 = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW4.Show();
                        return false;
                    }
                    if (item.Percentage == 0)
                    {
                        msgText = "Please enter percentage.";
                        MessageBoxControl.MessageBoxChildWindow msgW = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Error);
                        msgW.Show();
                        return false;
                    }
                    foreach (var item1 in ((List<clsItemTaxVO>)dgTaxList.ItemsSource).ToList())
                    {
                        if (item1.status == false)
                        {
                            msgText = "Please Select Appropriate Tax.";
                            MessageBoxControl.MessageBoxChildWindow msgW = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                            break;
                        }
                        return false;
                    }
                    if (dtpApplicableFromDate.SelectedDate != null || dtpApplicableToDate.SelectedDate != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Can Not Be Greater Than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    if (dtpApplicableFromDate.SelectedDate > dtpApplicableToDate.SelectedDate)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Can Not Be Greater Than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                }
            }
            
            return true;
        }

        private bool ValidateTaxUI()
        {
            bool blnValidate = true;
            List<clsItemTaxVO> lstItemTax = (List<clsItemTaxVO>)dgTaxList.ItemsSource;

            clsItemTaxVO objItemTax = new clsItemTaxVO();
            //if (dgTaxList.SelectedItem != null)
            //    objItemTax = dgTaxList.SelectedItem as clsItemTaxVO;
            //else
            //{
            objItemTax.ApplicableForDesc = ((MasterListItem)cmbApplicableFor.SelectedItem).Description;
            objItemTax.ApplicableOnDesc = ((MasterListItem)cmbApplicableOn.SelectedItem).Description;
            objItemTax.Percentage = Convert.ToDecimal(txtPercentage.Text);
            objItemTax.TaxName = ((MasterListItem)cmbTax.SelectedItem).Description;
            objItemTax.StoreID = ((clsItemStoreVO)cmbStore.SelectedItem).StoreID;

            if (taxseletion.IsChecked == true)
            {
                objItemTax.TaxType = 1;
                // objTax.TaxTypeName = "Inclusive";
            }
            else
            {
                objItemTax.TaxType = 2;
                //  objTax.TaxTypeName = "Exclusive";
            }

            //}
            if (objItemTax != null)
            {
                if (IsModify == true)                                                                       //&& z.ApplicableOnDesc.Equals(objItemTax.ApplicableOnDesc) && z.TaxType.Equals(objItemTax.TaxType)
                {
                    objItemTax.ID = objSelectedTax.ID;
                    //if (lstItemTax.Where(z => z.TaxName.Equals(objItemTax.TaxName) && z.ApplicableForDesc.Equals(objItemTax.ApplicableForDesc) && z.StoreID.Equals(objItemTax.StoreID) && z.ApplicableOnDesc.Equals(objItemTax.ApplicableOnDesc) && z.ID != (objItemTax.ID)).Any())  //&& z.Percentage.Equals(objItemTax.Percentage)
                    if (lstItemTax.Where(z => z.TaxName.Equals(objItemTax.TaxName) && z.ApplicableForDesc.Equals(objItemTax.ApplicableForDesc) && z.StoreID.Equals(objItemTax.StoreID) && z.ID != (objItemTax.ID)).Any())  //&& z.Percentage.Equals(objItemTax.Percentage)
                    {
                        //if (lstItemTax.Where(z => z.Percentage.Equals(objItemTax.Percentage)).Any())
                        //{

                        msgText = "You can not Add Same Tax Details";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                cmdAddTax.Content = "Modify Tax";
                            }
                        };
                        msgW.Show();
                        blnValidate = false;
                        //  IsModify = false;
                        //}
                        //else
                        //{
                        //    blnValidate = true;
                        //  //  IsModify = false;
                        //    objItemTax.ID = objSelectedTax.ID;
                        //}
                    }

                    else
                    {
                        blnValidate = true;
                        // IsModify = false;
                        objItemTax.ID = objSelectedTax.ID;
                    }
                }
                //  && z.ApplicableOnDesc.Equals(objItemTax.ApplicableOnDesc) && z.TaxType.Equals(objItemTax.TaxType)
                else if (lstItemTax.Where(z => z.TaxName.Equals(objItemTax.TaxName) && z.ApplicableForDesc.Equals(objItemTax.ApplicableForDesc) && z.StoreID.Equals(objItemTax.StoreID)).Any())  //&& z.Percentage.Equals(objItemTax.Percentage)
                {
                    //if (lstItemTax.Where(z => z.TaxType != (objItemTax.TaxType)).Any())
                    //{
                    msgText = "You can not Add Same Tax Details";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            cmdAddTax.Content = "Add Tax";
                            cmbTax.IsEnabled = true;
                            taxseletion.IsEnabled = true;
                            taxseletion1.IsEnabled = true;
                        }
                    };
                    msgW.Show();
                    blnValidate = false;
                    // }
                }
            }
            return blnValidate;
        }

        private bool ValidateAddTax()
        {
            bool IsValidate = true;
            if (((MasterListItem)cmbTax.SelectedItem).ID == 0)
            {
                cmbTax.TextBox.SetValidation("Please Select Tax.");
                cmbTax.TextBox.RaiseValidationError();
                cmbTax.TextBox.Focus();
                IsValidate = false;
            }
            else
                cmbTax.TextBox.ClearValidationError();
            if (((MasterListItem)cmbApplicableFor.SelectedItem).ID == 0)
            {
                cmbApplicableFor.TextBox.SetValidation("Please Select Applicable For.");
                cmbApplicableFor.TextBox.RaiseValidationError();
                cmbApplicableFor.TextBox.Focus();
                IsValidate = false;
            }
            else
                cmbApplicableFor.TextBox.ClearValidationError();
            if (((MasterListItem)cmbApplicableOn.SelectedItem).ID == 0)
            {
                cmbApplicableOn.TextBox.SetValidation("Please Select Applicable On.");
                cmbApplicableOn.TextBox.RaiseValidationError();
                cmbApplicableOn.TextBox.Focus();
                IsValidate = false;
            }
            else
                cmbApplicableOn.TextBox.ClearValidationError();

            if (String.IsNullOrEmpty(txtPercentage.Text))
            {
                txtPercentage.SetValidation("Please Add Tax Percentage");
                txtPercentage.RaiseValidationError();
                txtPercentage.Focus();
            }
            if (Convert.ToDouble(txtPercentage.Text) > 100)
            {
                txtPercentage.SetValidation("Tax Percentage Should Be Less Than 100");
                txtPercentage.RaiseValidationError();
                txtPercentage.Focus();
                IsValidate = false;
            }
            //else if (Convert.ToSingle(String.IsNullOrEmpty(txtPercentage.Text)) > 100)
            //{
            //    txtPercentage.SetValidation("Tax Percentage Should Be Less Than 100");
            //    txtPercentage.RaiseValidationError();
            //    txtPercentage.Focus();
            //}
            else
                txtPercentage.ClearValidationError();
            if (((clsItemStoreVO)cmbStore.SelectedItem).StoreID == 0)
            {
                cmbStore.TextBox.SetValidation("Please Select Store.");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.TextBox.Focus();
                IsValidate = false;
            }
            else
                cmbStore.TextBox.ClearValidationError();

            //if (dtpApplicableFromDate.SelectedDate == null)
            //{
            //    dtpApplicableFromDate.SetValidation("Please Select Applicable From Date.");
            //    dtpApplicableFromDate.RaiseValidationError();
            //    dtpApplicableFromDate.Focus();
            //    IsValidate = false;
            //}
            //else
            //    dtpApplicableFromDate.ClearValidationError();

            //if (dtpApplicableToDate.SelectedDate == null)
            //{
            //    dtpApplicableToDate.SetValidation("Please Select Applicable To Date.");
            //    dtpApplicableToDate.RaiseValidationError();
            //    dtpApplicableToDate.Focus();
            //    IsValidate = false;
            //}
            //else
            //    dtpApplicableToDate.ClearValidationError();

            //if (dtpApplicableFromDate.SelectedDate > dtpApplicableToDate.SelectedDate)
            //{
            //    msgText = "Applicable From Date Can Not Be Greater Than Applicable To Date.";
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //        new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW.Show();
            //    IsValidate = false;
            //}

            return IsValidate;
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedStoreList.Clear();
            FillStoreList(((MasterListItem)cmbClinic.SelectedItem).ID);
        }

        private void chkStore_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (dgStoreList.SelectedItem != null)
            {
                if (chk.IsChecked == true)
                {
                    //  ((clsItemStoreVO)dgStoreList.SelectedItem).status = true;
                    SelectedStoreList.Add((clsItemStoreVO)dgStoreList.SelectedItem);
                    if (SelectedStoreList.Count > 0)
                        foreach (var item in SelectedStoreList)
                        {
                            if (DeletedStoreList.Where(z => z.StoreID.Equals(item.StoreID)).Any())
                            {
                                DeletedStoreList.Remove(item);
                            }
                        }
                }
                else
                {
                    // ((clsItemStoreVO)dgStoreList.SelectedItem).status = false;
                    if (SelectedStoreList.Count > 2)
                    {
                        SelectedStoreList.Remove((clsItemStoreVO)dgStoreList.SelectedItem);
                        ((clsItemStoreVO)dgStoreList.SelectedItem).ItemID = objMasterVO.ID;
                        DeletedStoreList.Add((clsItemStoreVO)dgStoreList.SelectedItem);
                    }
                    else
                    {
                        chk.IsChecked = true;
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("", "Atleast One Store Should Be Selected", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                }
            }
        }
        int iIndex = 0;
        int PreIndex = 0;
        int count = 0;
        private void dgStoreList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStoreList.SelectedItem != null)
            {
                ClearTaxUI();
                // cmbStore.SelectedItem = SelectedStoreList[0];
                CheckForExistingTax(false);
                cmdAddTax.Content = "Add Tax";
                cmbTax.IsEnabled = true;
                taxseletion.IsEnabled = true;
                taxseletion1.IsEnabled = true;
            }
            //if (SelectedStoreList.Count == 0)
            //{
            //    SelectedStoreList.Add((clsItemStoreVO)dgStoreList.SelectedItem);
            //}
            //else if (dgStoreList.SelectedItem != null && (dgStoreList.SelectedIndex != 0 || count > 0))
            //{
            //    PreIndex = dgStoreList.SelectedIndex;
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure You want to change the Store?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
            //    {
            //        if (res == MessageBoxResult.Yes)
            //        {
            //            SelectedStoreList.Clear();
            //            SelectedStoreList = new List<clsItemStoreVO>();
            //            SelectedStoreList.Add((clsItemStoreVO)dgStoreList.SelectedItem);
            //            SelectedStoreList.Where(z => z.status = true);
            //            CheckForExistingTax();
            //        }
            //        else
            //        {
            //            dgStoreList.SelectedIndex = PreIndex;
            //            dgStoreList.UpdateLayout();
            //            dgStoreList.Focus();
            //        }
            //        count++;
            //    };
            //    msgW1.Show();
            //    chkAllStores.IsChecked = false;
            //    ClearTaxUI();
            //}
        }

        private void CmdSearch_Click(object sender, RoutedEventArgs e)
        {
            GetItemStoreList();
        }
        private void ApplyTaxToItem()
        {
            try
            {
                WaitIndicator wt = new WaitIndicator();
                wt.Show();
                clsAddUpdateItemClinicDetailBizActionVO BizAction = new clsAddUpdateItemClinicDetailBizActionVO();
                BizAction.StoreClinicID = objMasterVO.ID;
                BizAction.ItemTax = new clsItemTaxVO();
                BizAction.ItemTaxList = ((List<clsItemTaxVO>)dgTaxList.ItemsSource).ToList();
                BizAction.ItemTax.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.ItemTax.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.ItemTax.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID; ;
                BizAction.ItemTax.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                BizAction.ItemTax.AddedDateTime = DateTime.Now;
                BizAction.ItemTax.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                BizAction.ItemTax.ItemID = objMasterVO.ID;
                BizAction.ItemTax.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessAsync(BizAction, new clsUserVO());
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsAddUpdateItemClinicDetailBizActionVO)(arg.Result)).SuccessStatus == 1)
                    {
                        msgText = "Taxes Applied Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (cmbClinic.SelectedItem != null)
                            {
                                if (((MasterListItem)cmbClinic.SelectedItem).ID > 0)
                                {
                                    long clinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                                    GetItemStoreList();
                                }
                            }
                        };
                        msgW.Show();
                    }
                    wt.Close();
                };
                client.CloseAsync();
            }
            catch (Exception Ex)
            {

            }
        }
        private void cmdApplyTax_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                msgText = "Are you sure you want to apply tax to Item ?";
                MessageBoxControl.MessageBoxChildWindow SaveMsg = new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                SaveMsg.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ApplyTaxToItem();
                    }
                };
                SaveMsg.Show();
            }
        }

        private void chkAllStores_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            List<clsItemStoreVO> objItemStoreList = new List<clsItemStoreVO>();
            objItemStoreList = (List<clsItemStoreVO>)dgStoreList.ItemsSource;
            if (chk.IsChecked == true)
            {
                try
                {
                    if (objItemStoreList != null)
                    {
                        SelectedStoreList.Clear();
                        foreach (var item in objItemStoreList)
                        {
                            item.status = true;
                            SelectedStoreList.Add(item);
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
                        SelectedStoreList.Clear();
                        foreach (var item in objItemStoreList)
                        {
                            item.status = false;
                            item.Max = 0;
                            item.Min = 0;
                            item.Re_Order = 0;
                            item.ItemID = objMasterVO.ID;
                            SelectedStoreList.Remove(item);
                            DeletedStoreList.Add(item);
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
        List<clsItemTaxVO> objTaxList = new List<clsItemTaxVO>();
        private void cmdAddTax_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAddTax())
            {
                SaveStore = false;
                List<clsItemTaxVO> lstItemTax = dgTaxList.ItemsSource as List<clsItemTaxVO>;

                //if(lstItemTax.Where(z=>z.StoreID.()))
                if (((clsItemStoreVO)dgStoreList.SelectedItem).StoreID != ((clsItemStoreVO)cmbStore.SelectedItem).StoreID)
                {
                    lstItemTax.Clear();
                }
                clsItemTaxVO objTax = new clsItemTaxVO();
                objTax.TaxID = ((MasterListItem)cmbTax.SelectedItem).ID;
                objTax.TaxName = ((MasterListItem)cmbTax.SelectedItem).Description;
                objTax.ApplicableForId = ((MasterListItem)cmbApplicableFor.SelectedItem).ID;
                objTax.ApplicableOnId = ((MasterListItem)cmbApplicableOn.SelectedItem).ID;
                objTax.ApplicableForDesc = ((MasterListItem)cmbApplicableFor.SelectedItem).Description;
                objTax.ApplicableOnDesc = ((MasterListItem)cmbApplicableOn.SelectedItem).Description;
                objTax.status = true;
                objTax.Percentage = Convert.ToDecimal(txtPercentage.Text);
                objTax.StoreID = ((clsItemStoreVO)cmbStore.SelectedItem).StoreID;

                //----------Added By Bhushanp 20062017---------------------
                objTax.ApplicableFrom = Convert.ToDateTime(DateTime.Now);//(dtpApplicableFromDate.Text);
                objTax.ApplicableTo = Convert.ToDateTime(DateTime.Now); //(dtpApplicableToDate.Text);
                objTax.IsGST = Convert.ToBoolean(chkIsGST.IsChecked);
                //-----------------------------------------------------------

                if (IsModify == true)
                {
                    objTax.ID = objSelectedTax.ID;
                }
                //if (taxseletion.IsChecked == true)
                //{
                //    objTax.TaxType = 1;
                //    objTax.TaxTypeName = "Inclusive Tax";
                //}
                //else
                //{
                //    objTax.TaxType = 2;
                //    objTax.TaxTypeName = "Exclusive Tax";
                //}

                if (taxseletion.IsChecked == true)
                {
                    objTax.TaxType = 1;
                    objTax.TaxTypeName = "Inclusive";
                }
                else
                {
                    objTax.TaxType = 2;
                    objTax.TaxTypeName = "Exclusive";
                }

                if (ValidateTaxUI())
                {
                    if (dgTaxList.SelectedItem != null)
                    {
                        lstItemTax[dgTaxList.SelectedIndex] = objTax;
                    }
                    else
                    {
                        lstItemTax.Add(objTax);
                    }
                    //dgTaxList.ItemsSource = null;
                    //dgTaxList.ItemsSource = lstItemTax;
                    cmdAddTax.Content = "Add Tax";
                    cmbTax.IsEnabled = true;
                    taxseletion.IsEnabled = true;
                    taxseletion1.IsEnabled = true;

                    foreach (clsItemStoreVO item in SelectedStoreList)
                    {
                        //if (chkTaxForAllStores.IsChecked == true)
                        //{
                        //    if (item.status == true)     //  if (item.StoreID == ((clsItemStoreVO)dgStoreList.SelectedItem).StoreID)   
                        //    {
                        //        item.objStoreTaxList = new List<clsItemTaxVO>();
                        //        item.objStoreTaxList = lstItemTax.DeepCopy();
                        //    }
                        //}
                        //else
                        //{

                        //if (item.StoreID == ((clsItemStoreVO)dgStoreList.SelectedItem).StoreID)
                        if (item.StoreID == ((clsItemStoreVO)cmbStore.SelectedItem).StoreID)
                        {
                            item.objStoreTaxList = new List<clsItemTaxVO>();
                            item.objStoreTaxList = lstItemTax.DeepCopy();
                        }
                        //}

                    }
                    if (IsModify == true)
                    {
                        objTaxList.Clear();
                        objTaxList.Add(objTax);
                    }
                    else
                    {
                        objTaxList.Clear();
                        objTaxList.Add(objTax);
                    }
                    chkTaxForAllStores.IsChecked = false;
                    ApplyStore();
                    //if (IsModify == true)
                    //{
                    //   dgStoreList_SelectionChanged(null,null);
                    // }
                }
                ClearTaxUI();
                cmbStore.SelectedItem = SelectedStoreList[0];
                cmbTax.SelectedItem = objList[0];
            }
        }

        private void ClearTaxUI()
        {
            clsItemTaxVO obj = new clsItemTaxVO();
            cmbApplicableFor.ItemsSource = null;
            cmbApplicableFor.ItemsSource = obj.ApplicableForList;
            cmbApplicableFor.SelectedItem = obj.ApplicableForList[0];
            cmbApplicableOn.ItemsSource = null;
            cmbApplicableOn.ItemsSource = obj.ApplicableOnList;
            cmbApplicableOn.SelectedItem = obj.ApplicableOnList[0];
            //cmbStore.ItemsSource = SelectedStoreList;
            //cmbStore.SelectedItem = SelectedStoreList[0];
            //cmbTax.SelectedValue = (long)0;
            //cmbTax.UpdateLayout();
            //cmbTax.ItemsSource = objList;
            //cmbTax.SelectedItem = objList[0];
            txtPercentage.Text = obj.Percentage.ToString();
            //cmbApplicableFor.UpdateLayout();
            //cmbApplicableOn.UpdateLayout();
        }
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {

            if ((clsItemTaxVO)dgTaxList.SelectedItem != null)
            {
                List<clsItemTaxVO> objList = new List<clsItemTaxVO>();
                msgText = "Do You Want To Remove Selected Tax For Item ?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        //objList = ((List<clsItemTaxVO>)dgTaxList.ItemsSource).ToList();
                        //objList.Remove((clsItemTaxVO)dgTaxList.SelectedItem);
                        //dgTaxList.ItemsSource = null;
                        //dgTaxList.ItemsSource = objList;
                        //objTaxList = objList;

                        //foreach (clsItemStoreVO item in SelectedStoreList)
                        //{
                        //    if (item.StoreID == ((clsItemStoreVO)dgStoreList.SelectedItem).StoreID)
                        //    {
                        //        item.objStoreTaxList = new List<clsItemTaxVO>();
                        //        item.objStoreTaxList = objTaxList.DeepCopy();
                        //    }
                        //}

                        clsAddItemClinicBizActionVO BizActionobj = new clsAddItemClinicBizActionVO();
                        BizActionobj.IsForDelete = true;
                        BizActionobj.DeleteTaxID = ((clsItemTaxVO)dgTaxList.SelectedItem).ID;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                objList = ((List<clsItemTaxVO>)dgTaxList.ItemsSource).ToList();
                                objList.Remove((clsItemTaxVO)dgTaxList.SelectedItem);
                                dgTaxList.ItemsSource = null;
                                dgTaxList.ItemsSource = objList;
                                objTaxList = objList;

                                foreach (clsItemStoreVO item in SelectedStoreList)
                                {
                                    if (item.StoreID == ((clsItemStoreVO)dgStoreList.SelectedItem).StoreID)
                                    {
                                        item.objStoreTaxList = new List<clsItemTaxVO>();
                                        item.objStoreTaxList = objTaxList.DeepCopy();
                                    }
                                }
                            }
                            ClearTaxUI();
                            cmbStore.SelectedItem = SelectedStoreList[0];
                            //   cmbTax.SelectedItem = objList[0];
                        };
                        client.ProcessAsync(BizActionobj, new clsUserVO());
                        client.CloseAsync();
                    }
                };
                msgW.Show();
            }
        }
        clsItemTaxVO objSelectedTax;
        public bool IsModify = false;
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (dgTaxList.SelectedItem != null)
            {
                objSelectedTax = ((clsItemTaxVO)dgTaxList.SelectedItem);

                List<MasterListItem> objTaxList = ((List<MasterListItem>)cmbTax.ItemsSource).ToList();
                List<MasterListItem> objTaxList1 = ((List<MasterListItem>)cmbApplicableFor.ItemsSource).ToList();
                List<MasterListItem> objTaxList2 = ((List<MasterListItem>)cmbApplicableOn.ItemsSource).ToList();
                List<clsItemStoreVO> storelist = ((List<clsItemStoreVO>)cmbStore.ItemsSource).ToList();
                foreach (clsItemStoreVO item in storelist)
                {
                    if (item.StoreID == objSelectedTax.StoreID)
                    {
                        cmbStore.SelectedItem = item;
                        break;
                    }
                }

                foreach (MasterListItem item in objTaxList)
                {
                    if (item.ID == objSelectedTax.TaxID)
                    {
                        cmbTax.SelectedItem = item;
                        break;
                    }
                }
                foreach (MasterListItem item in objTaxList1)
                {
                    if (item.ID == objSelectedTax.ApplicableForId)
                    {
                        cmbApplicableFor.SelectedItem = item;
                        break;
                    }
                }
                foreach (MasterListItem item in objTaxList2)
                {
                    if (item.ID == objSelectedTax.ApplicableOnId)
                    {
                        cmbApplicableOn.SelectedItem = item;
                        break;
                    }
                }
                if (objSelectedTax.TaxType == 2)
                {
                    taxseletion.IsChecked = false;
                    taxseletion1.IsChecked = true;
                }
                else if (objSelectedTax.TaxType == 1)
                {
                    taxseletion1.IsChecked = false;
                    taxseletion.IsChecked = true;
                }

                dtpApplicableFromDate.Text = objSelectedTax.ApplicableFrom.ToString("dd/MM/yyyy");
                dtpApplicableToDate.Text = objSelectedTax.ApplicableTo.ToString("dd/MM/yyyy");
                chkIsGST.IsChecked = Convert.ToBoolean(objSelectedTax.IsGST);
                txtPercentage.Text = Convert.ToString(objSelectedTax.Percentage);
                cmdAddTax.Content = "Modify Tax";
                IsModify = true;
                cmbTax.IsEnabled = false;
                cmbTax.UpdateLayout();
                taxseletion.IsEnabled = false;
                taxseletion1.IsEnabled = false;
                
            }
        }

        bool bMin = true;
        public bool CheckminMax()
        {
            foreach (var item in SelectedStoreList)
            {
                if (item.Min > item.Max || !(item.Re_Order <= item.Max && item.Re_Order >= item.Min))
                {
                    bMin = false;
                    break;
                }
            }
            return bMin;
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtExpirationdays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() || Convert.ToInt64(((TextBox)sender).Text) <= 0 && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        //private void txtPercentage_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!((TextBox)sender).Text.IsItDecimal())
        //    {
        //        ((TextBox)sender).Text = textBefore;
        //        ((TextBox)sender).SelectionStart = selectionStart;
        //        ((TextBox)sender).SelectionLength = selectionLength;
        //        textBefore = "";
        //        selectionStart = 0;
        //        selectionLength = 0;
        //    }
        //}

        private void txtNumber_KeyDown1(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // dgTaxList.ItemsSource = null;
            foreach (var item in ItemStoreList)
            {
                if (item.StoreID == ((clsItemStoreVO)cmbStore.SelectedItem).StoreID)
                {
                    dgStoreList.SelectedItem = item;
                    break;
                }
            }
            //   cmbStore.SelectedItem = SelectedStoreList[0];
            cmbTax.SelectedItem = objList[0];
            cmdAddTax.Content = "Add Tax";
            IsModify = false;
            cmbTax.IsEnabled = true;
            taxseletion.IsEnabled = true;
            taxseletion1.IsEnabled = true;
        }

        //private void txtSearchCriteria_KeyUp(object sender, KeyEventArgs e)
        //{
        //    textBefore = ((TextBox)sender).Text;
        //    selectionStart = ((TextBox)sender).SelectionStart;
        //    selectionLength = ((TextBox)sender).SelectionLength;

        //    if (e.Key == Key.Subtract)
        //    {
        //        textBefore = "";
        //        selectionStart = 0;
        //        selectionLength = 0;
        //    }
        //}

        //private void txtQuantity_KeyPress(object sender, KeyEventArgs e)
        //{
        //    if (char.IsDigit(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || e.KeyChar == '.')
        //    {
        //        if (Regex.IsMatch(txtPercentage.Text, @"\.\d\d\d"))
        //        {
        //            if (e.KeyChar == (char)8)
        //            {
        //                //This makes backspace working
        //            }
        //            else
        //            {
        //                e.Handled = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        e.Handled = true;
        //    }

        //}
    }
    public class clsStoreStaus
    {
        public long ItemClinicID { get; set; }
        public bool status { get; set; }
    }


}

