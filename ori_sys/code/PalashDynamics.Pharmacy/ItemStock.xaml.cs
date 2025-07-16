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
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.UserRights;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Resources;

namespace PalashDynamics.Pharmacy
{
    public partial class ItemStock : UserControl
    {
        public PagedSortableCollectionView<clsItemStockVO> DataList { get; private set; }
        public RoutedEventHandler OnCancel_Click;
        public bool IsForCentralPurChaseFromApproveIndent=false;
        public long IndentID = 0;
        public long IndentUnitID = 0;
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
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetItemCurrentStockList();
        }
        public string msgTitle = "";
        public string msgText = "";

        public ItemStock()
        {
            InitializeComponent();

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsItemStockVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgItemStock.ItemsSource = DataList;
            dgDataPager.Source = DataList;
            //GetItemCurrentStockList();
            //======================================================
        }

        private void ItemStock_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new clsItemStockVO();

            //dtpStockDate.SelectedDate = DateTime.Now.Date;
            //dtpToDate.SelectedDate = DateTime.Now.Date;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                cmbClinic.IsEnabled = true;
            }
            else
            {
                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
                //cmbClinic.IsEnabled = false;
                ((clsItemStockVO)this.DataContext).UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            cmbClinic.ItemsSource = null;
            cmbStore.ItemsSource = null;

            FillUnitList();

            FillItemGroup();
            FillItemCategory();

            GetItemCurrentStockList();
        }

        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        clsUserRightsVO objUser; 
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;
                        if (objUser.IsCentarlPurchase)
                            cmbClinic.IsEnabled = true;
                        else
                            cmbClinic.IsEnabled = false;
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillUnitList()
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
                    cmbClinic.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbClinic.SelectedValue = ((clsItemStockVO)this.DataContext).UnitId;
                }
                else
                {
                    cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        //private void FillStore(long pClinicID)
        //{
        //    //cmbStore.ItemsSource = null;
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_Store;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    if (pClinicID > 0)
        //    {
        //        BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
        //    }


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

        //            cmbStore.ItemsSource = null;
        //            cmbStore.ItemsSource = objList;
        //            cmbStore.SelectedItem = objList[0];
        //        }
        //        if (this.DataContext != null)
        //        {
        //            //cmbStore.ItemsSource = null;
        //            cmbStore.SelectedValue = ((clsItemStockVO)this.DataContext).StoreID;
        //        }
        //        BizAction.MasterList = null;
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        private void FillStore()
        {
            try
            {
                clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();

                //False when we want to fetch all items
                clsItemMasterVO obj = new clsItemMasterVO();
                List<clsStoreVO> objList = new List<clsStoreVO>();

                obj.RetrieveDataFlag = false;
                BizActionObj.IsUserwiseStores = true;
                BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
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
                     //   BizActionObj.ForItemStock = true;
                        clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((MasterListItem)cmbClinic.SelectedItem).ID, Status = true };
                        Default.ForItemStock = true;
                        var result = from item in BizActionObj.ItemMatserDetails
                                     where item.ClinicId == ((MasterListItem)cmbClinic.SelectedItem).ID && item.Status == true
                                     select item;
                        
                        objList = (List<clsStoreVO>)result.ToList();

                        objList.Insert(0, Default);
                        BizActionObj.ToStoreList.Insert(0, Default);
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

                client.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillItemGroup()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ItemGroup;
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

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cboItemGroup.ItemsSource = null;
                    cboItemGroup.ItemsSource = objList;
                    cboItemGroup.SelectedItem = objList[0];

                }

                //if (this.DataContext != null)
                //{
                //    cboItemGroup.SelectedValue = ((clsItemMasterVO)this.DataContext).ItemGroup;
                //}

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillItemCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ItemCategory;
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

                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        cboItemCategory.ItemsSource = null;
                        cboItemCategory.ItemsSource = objList;
                        cboItemCategory.SelectedItem = objList[0];

                    }


                    //if (this.DataContext != null)
                    //{
                    //    cboItemCategory.SelectedValue = ((clsItemMasterVO)this.DataContext).ItemCategory;
                    //}
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }

            catch (Exception ex)
            {

                throw ex;
            }


        }

        public void GetItemCurrentStockList()
        {
            clsGetItemCurrentStockListBizActionVO BizAction = new clsGetItemCurrentStockListBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();

            //if (dtpStockDate.SelectedDate != null)
            //    BizAction.FromDate = dtpStockDate.SelectedDate.Value.Date;

            //if (dtpToDate.SelectedDate != null)
            //    BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

            if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID != 0)
            {
                BizAction.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;

            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            //if (cmbStore.SelectedItem != null && ((MasterListItem)cmbStore.SelectedItem).ID != 0)
            //{
            //    BizAction.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
            //}

            if (cmbStore.SelectedItem != null && ((clsStoreVO)cmbStore.SelectedItem).StoreId != 0)
            {
                BizAction.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            }

            if (txtItemName.Text != "")
                BizAction.ItemName = txtItemName.Text;

            if (txtBatchCode.Text != "")
                BizAction.BatchCode = txtBatchCode.Text;

            if (chkStockwithZero.IsChecked == true)
                BizAction.IsStockZero = false;
            else
                BizAction.IsStockZero = true;

            if (chkConsignment.IsChecked == true)
                BizAction.IsConsignment = true;
            else
                BizAction.IsConsignment = false;

            //Added by Ashish Z. for Indent wise Stock
            if (this.IsForCentralPurChaseFromApproveIndent)
            {
                BizAction.IndentID = this.IndentID;
                BizAction.IndentUnitID = this.IndentUnitID;
                BizAction.IsForCentralPurChaseFromApproveIndent = true;
            }
            else
            {
                BizAction.IndentID = 0;
                BizAction.IndentUnitID = 0;
                BizAction.IsForCentralPurChaseFromApproveIndent = false;
            }


            //Begin:: Added by AniketK on 10-Dec-2018
            if (chkStockAsOn.IsChecked == true)
            {
                if (dtpToDate.SelectedDate != null)
                    BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
            }
            //End:: Added by AniketK on 10-Dec-2018



            // Begin : added by aniketk on 20-Oct-2018 for Item Group & Category filter
            if (cboItemGroup.SelectedItem != null && ((MasterListItem)cboItemGroup.SelectedItem).ID != 0)
            {
                BizAction.ItemGroupID = ((MasterListItem)cboItemGroup.SelectedItem).ID;
            }

            if (cboItemCategory.SelectedItem != null && ((MasterListItem)cboItemCategory.SelectedItem).ID != 0)
            {
                BizAction.ItemCategoryID = ((MasterListItem)cboItemCategory.SelectedItem).ID;
            }
            // End : added by aniketk on 20-Oct-2018 for Item Group & Category filter


            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetItemCurrentStockListBizActionVO)arg.Result != null)
                    {
                        clsGetItemCurrentStockListBizActionVO result = arg.Result as clsGetItemCurrentStockListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.BatchList != null)
                        {
                            DataList.Clear();
                            foreach (var item in result.BatchList)
                            {
                                DataList.Add(item);
                            }


                            dgItemStock.ItemsSource = null;
                            dgItemStock.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;

                        }
                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            GetItemCurrentStockList();
            dgDataPager.PageIndex = 0;
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbClinic.SelectedItem != null)
            {
                //FillStore(((MasterListItem)cmbClinic.SelectedItem).ID);
                FillStore();
            }
        }

        public void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (DataList.Count > 0)
            {
                bool chkToDate = true;
                bool IsStockZero = false;
                bool IsConsignment = false;
                long StoreID = 0;
                long UnitID = 0;
                string ItemName = null;
                string BatchCode = null;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                DateTime? PrintDate = DateTime.Now.Date.Date;
                long UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

                //Begin:- Added by AniketK on 23-OCT-2018
                long ItemGroupId = 0;
                long ItemCategoryId = 0;
                //End:- Added by AniketK on 23-OCT-2018


                if (chkStockwithZero.IsChecked == true)
                    IsStockZero = false;
                else
                {
                    IsStockZero = true;
                    //Begin:- Added by AniketK on 23-OCT-2018
                   // string msgTitle = "";
                   // string msgText = "Stock is not available";

                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                   // msgW1.Show();
                    //End:- Added by AniketK on 23-OCT-2018
                }
                if (chkConsignment.IsChecked == true)
                    IsConsignment = true;
                else
                    IsConsignment = false;

                //if (dtpFromDate.SelectedDate != null)
                //{
                //    FromDate = dtpFromDate.SelectedDate.Value.Date.Date;
                //}

                //if (dtpToDate.SelectedDate != null)
                //{
                //    ToDate = dtpToDate.SelectedDate.Value.Date.Date;
                //    if (FromDate.Value > ToDate.Value)
                //    {
                //        dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                //        ToDate = FromDate;
                //        chkToDate = false;
                //    }
                //    else
                //    {
                //        PrintDate = ToDate;
                //        ToDate = ToDate.Value.AddDays(1);
                //        dtpToDate.Focus();
                //    }
                //}

                //if (ToDate != null)
                //{
                //    if (FromDate != null)
                //    {
                //        FromDate = dtpFromDate.SelectedDate.Value.Date.Date;
                //    }
                //}

                //if (dtpStockDate.SelectedDate != null)
                //    FromDate = dtpStockDate.SelectedDate.Value.Date;


                //Begin:: Added by AniketK on 10-Dec-2018
                if (dtpToDate.SelectedDate != null)
                    ToDate = dtpToDate.SelectedDate.Value.Date;
                //BizAction.ToDate = dtpToDate.SelectedDate.Value.Date
                //End:: Added by AniketK on 10-Dec-2018


                if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID != 0)
                {
                    UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }
                if (cmbStore.SelectedItem != null && ((clsStoreVO)cmbStore.SelectedItem).StoreId != 0)
                {
                    StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                }
                if (txtItemName.Text != "")
                {
                    ItemName = txtItemName.Text;
                }
                if (txtBatchCode.Text != "")
                {
                    BatchCode = txtBatchCode.Text;
                }


                //Begin:- Added by aniketK on 23-OCT-2018
                if (cboItemGroup.SelectedItem != null && ((MasterListItem)cboItemGroup.SelectedItem).ID != 0)
                {
                    ItemGroupId = ((MasterListItem)cboItemGroup.SelectedItem).ID;
                }

                if (cboItemCategory.SelectedItem != null && ((MasterListItem)cboItemCategory.SelectedItem).ID != 0)
                {
                    ItemCategoryId =((MasterListItem)cboItemCategory.SelectedItem).ID;
                }
                //End:- Added by aniketK on 23-OCT-2018



                if (chkToDate == true)
                {
                    string URL;
                    if (ToDate != null && PrintDate != null)  //&& PrintDate != null //&&  FromDate != null
                    {
                        URL = "../Reports/InventoryPharmacy/CurrentItemStock.aspx?UnitID=" + UnitID + "&StoreID=" + StoreID + "&ItemName=" + ItemName + "&BatchCode=" + BatchCode + "&PrintDate=" + PrintDate.Value.ToString("MM/dd/yyyy") + "&IsStockZero=" + IsStockZero + "&ToDate=" + ToDate.Value.ToString("MM/dd/yyyy") + "&IsConsignment=" + IsConsignment + "&Excel=" + chkExcel.IsChecked + "&UserID=" + UserID + "&ItemGroupID=" + ItemGroupId + "&ItemCategoryID=" + ItemCategoryId;//PrintDate;
                        //string URL = "../Reports/InventoryPharmacy/PurchaseOrderList.aspx?FromDate=" + dtpF + "&ToDate=" + dtpT + "&SupplierIDs=" + SupplierIDs + "&ToDatePrint=" + dtpP;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                    else
                    {
                        //URL = "../Reports/InventoryPharmacy/CurrentItemStock.aspx?UnitID=" + UnitID + "&StoreID=" + StoreID + "&ItemName=" + ItemName + "&BatchCode=" + BatchCode + "&PrintDate=" + "&IsStockZero=" + IsStockZero + "&IsConsignment=" + IsConsignment + "&Excel=" + chkExcel.IsChecked;   //PrintDate.Value.ToString("MM/dd/yyyy") + // + "&FromDate=" + FromDate.Value.ToString("MM/dd/yyyy") + "&ToDate=" + ToDate.Value.ToString("MM/dd/yyyy")
                        //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                        URL = "../Reports/InventoryPharmacy/CurrentItemStock.aspx?UnitID=" + UnitID + "&StoreID=" + StoreID + "&ItemName=" + ItemName + "&BatchCode=" + BatchCode + "&PrintDate=" + PrintDate.Value.ToString("MM/dd/yyyy") + "&IsStockZero=" + IsStockZero + "&Excel=" + chkExcel.IsChecked + "&UserID=" + UserID + "&ItemGroupID=" + ItemGroupId + "&ItemCategoryID=" + ItemCategoryId;   //PrintDate.Value.ToString("MM/dd/yyyy") + // + "&FromDate=" + FromDate.Value.ToString("MM/dd/yyyy") + "&ToDate=" + ToDate.Value.ToString("MM/dd/yyyy")
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                }
                else
                {
                    string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }

                //string URL = "../Reports/InventoryPharmacy/CurrentItemStock.aspx?UnitID=" + UnitID + "&StoreID=" + StoreID + "&ItemName=" + ItemName + "&BatchCode=" + BatchCode + "&PrintDate=" + PrintDate.Value.ToString("MM/dd/yyyy") + "&IsStockZero=" + IsStockZero + "&FromDate=" + FromDate.Value.ToString("MM/dd/yyyy") + "&ToDate=" + ToDate.Value.ToString("MM/dd/yyyy");   //PrintDate;
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else
            {
                string msgTitle = "";
                string msgText = "Records not Found";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }


        }

        private void txtBatchCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                WaitIndicator w = new WaitIndicator();
                w.Show();
                try
                {
                    txtBatchCode.Focus();

                    string[] str = txtBatchCode.Text.Split('-');

                    if (str.Length > 1)
                    {
                        //BizActionObj.ItemID = Convert.ToInt64(str[0]);
                        string inputString = null;
                        string[] str1 = null;
                        bool blnFlag = false;
                        if (str[1].Contains("/"))
                        {
                            str1 = str[1].Split('/');
                            inputString = str1[1];
                        }
                        else
                        {
                            str1 = str[1].Split();
                            // str1[0] = str[1];
                            inputString = str1[0];
                        }


                        string BatchID = null;

                        string lastCharacter = inputString.Substring(inputString.Length - 1);
                        string lastCharacter1 = inputString.Substring(inputString.Length - 2);

                        if (lastCharacter == "B")
                        {
                            BatchID = inputString.Substring(0, inputString.Length - 1);
                            //BizActionObj.BatchCode = str1[0];
                            //BizActionObj.BatchID = Convert.ToInt64(BatchID);

                            txtBatchCode.Text = str1[0];

                            //foreach (var item in PharmacyItems.Where(x => x.ItemID == Convert.ToInt64(str[0]) & x.BatchID == Convert.ToInt64(BatchID)))
                            //{
                            //    if (Convert.ToDouble(item.AvailableQuantity) > item.Quantity)
                            //    {
                            //        item.Quantity = item.Quantity + 1;
                            //    }
                            //    else
                            //    {
                            //        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                            //        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            //        mgbx.Show();
                            //    }
                            //    blnFlag = true;
                            //}
                        }
                        else if (lastCharacter == "I")
                        {
                            if (lastCharacter1 == "BI")
                            {
                                str[1] = inputString.Substring(0, inputString.Length - 2);
                                //BizActionObj.BatchID = Convert.ToInt64(str[1]);

                                //foreach (var item in PharmacyItems.Where(x => x.ItemID == Convert.ToInt64(str[0]) & x.BatchID == Convert.ToInt64(str[1])))
                                //{
                                //    if (Convert.ToDouble(item.AvailableQuantity) > item.Quantity)
                                //    {
                                //        item.Quantity = item.Quantity + 1;
                                //    }
                                //    else
                                //    {
                                //        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                //        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                //        mgbx.Show();
                                //    }
                                //    blnFlag = true;
                                //}

                            }
                            else
                            {
                                str[1] = inputString.Substring(0, inputString.Length - 1);
                                //BizActionObj.ItemCode = str[1];
                                //foreach (var item in PharmacyItems.Where(x => x.ItemID == Convert.ToInt64(str[0])))
                                //{
                                //    if (Convert.ToDouble(item.AvailableQuantity) > item.Quantity)
                                //    {
                                //        item.Quantity = item.Quantity + 1;
                                //    }
                                //    else
                                //    {
                                //        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                //        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                //        mgbx.Show();
                                //    }
                                //    blnFlag = true;
                                //}
                            }
                        }

                        txtBatchCode.Focus();

                        GetItemCurrentStockList();
                        dgDataPager.PageIndex = 0;

                        //txtBatchCode.Text = "";



                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    w.Close();
                }
            }
        }

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetItemCurrentStockList();
            }
        }

        public string ModuleName { get; set; }
        public string Action { get; set; }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow objItemStock = ((ChildWindow)((ItemStock)this).Parent);
            objItemStock.DialogResult = false;

            this.OnCancel_Click(sender, e);
            //ModuleName = "PalashDynamics.Pharmacy";
            //Action = "PalashDynamics.Pharmacy.StoreIndent";
            //UserControl rootPage = Application.Current.RootVisual as UserControl;

            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            ////mElement.Text = "Aprove";

            //WebClient c = new WebClient();
            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }



        }


        //Begin:: Added by AniketK on 10-Dec-2018
        private void chkStockAsOn_Checked(object sender, RoutedEventArgs e)
        {
            if (chkStockAsOn.IsChecked == true)
            {
                dtpToDate.Visibility = Visibility.Visible;
                //dtpToDate.SelectedDate = DateTime.Now.Date;
                dtpToDate.DisplayDateEnd = DateTime.Now.Date;
            }
            else
            {
                dtpToDate.Visibility = Visibility.Collapsed;
            }
        }

        private void chkStockAsOn_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkStockAsOn.IsChecked == true)
                dtpToDate.Visibility = Visibility.Visible;
            else
                dtpToDate.Visibility = Visibility.Collapsed;
        }
        //End:: Added by AniketK on 10-Dec-2018

    }
}
