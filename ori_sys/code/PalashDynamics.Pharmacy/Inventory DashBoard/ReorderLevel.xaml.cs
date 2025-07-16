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
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.UserControls;
using System.Windows.Data;
using PalashDynamics.Pharmacy.ViewModels;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Pharmacy.Inventory;
using System.Collections.ObjectModel;
using System.Windows.Browser;

namespace PalashDynamics.Pharmacy.Inventory_DashBoard
{
    public partial class ReorderLevel : UserControl
    {
        public ReorderLevel()
        {
            InitializeComponent();
            ReorderItemList = new PagedSortableCollectionView<clsItemReorderDetailVO>();
            ReorderItemList.OnRefresh += new EventHandler<RefreshEventArgs>(ReorderItemList_OnRefresh);
            DataListPageSize = 22;
        }

        PagedCollectionView pcv = null;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;

     //   private ObservableCollection<clsIndentDetailVO> _BatchSelected;
        public List<clsIndentDetailVO> objLstIndent = null;
        MessageBoxControl.MessageBoxChildWindow mgbx = null;

       // HomeInventoryViewModel CurrentStock; // = new HomeInventoryViewModel();

        #region Paging

        public PagedSortableCollectionView<clsItemReorderDetailVO> ReorderItemList { get; private set; }

        public int DataListPageSize
        {
            get
            {
                return ReorderItemList.PageSize;
            }
            set
            {
                if (value == ReorderItemList.PageSize) return;
                ReorderItemList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        #endregion

        private bool Validation()
        {
            if (cmbStore.SelectedItem == null || ((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select  Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            return true;
        }

        void ReorderLevel_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                long clinicId = 0;

                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                //{
                //    clinicId = 0;
                //}
                //else
                //{
                    clinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //}
                FillStores(clinicId);
                
                Indicatior.Close();
                cmbStore.Focus();
            }
        }

        public void GetItemReorderList()
        {
            clsGetItemReorderQuantityBizActionVO BizAction = new clsGetItemReorderQuantityBizActionVO();
            BizAction.ItemReorderList = new List<clsItemReorderDetailVO>();

            //if (BizActionObjectForReorder.IsOrderBy != null)
            //    BizAction.IsOrderBy = BizActionObjectForReorder.IsOrderBy;

            //BizAction.Date = null;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.Unit = 0;
            }
            else
            {
                BizAction.Unit = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

            BizAction.IsPagingEnabled = true;
            BizAction.NoOfRecords = ReorderItemList.PageSize;
            BizAction.StartRowIndex = ReorderItemList.PageIndex * ReorderItemList.PageSize;
            BizAction.store = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            BizAction.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
            BizAction.ItemName = txtItemName.Text;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetItemReorderQuantityBizActionVO)arg.Result != null)
                    {
                        clsGetItemReorderQuantityBizActionVO result = arg.Result as clsGetItemReorderQuantityBizActionVO;
                        //result.TotalRow = result.ItemReorderList.Count;
                        ReorderItemList.TotalItemCount = (int)result.TotalRow;
                        if (result.ItemReorderList != null)
                        {
                            ReorderItemList.Clear();
                            foreach (var item in result.ItemReorderList)
                            {
                                ReorderItemList.Add(item);
                            }

                            dgItemReorderList.ItemsSource = ReorderItemList;
                            dgDataPager.Source = ReorderItemList;
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        //private void FillStores(long pClinicID)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
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

        //            //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

        //            //cmbBloodGroup.ItemsSource = null;
        //            //cmbBloodGroup.ItemsSource = objList;
        //            cmbStore.ItemsSource = null;
        //            cmbStore.ItemsSource = objList;

        //            //if (objList.Count > 1)
        //            //    cmbStore.SelectedItem = objList[1];
        //            //else
        //            cmbStore.SelectedItem = objList[0];
        //            if (this.DataContext != null)
        //            {
        //                cmbStore.SelectedValue = ((clsGRNVO)this.DataContext).StoreID;
        //            }
        //        }
        //        FillSupplier();
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        List<clsStoreVO> objlistStore;
        private void FillStores(long pClinicID)
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.StoreType = 2;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    objlistStore = new List<clsStoreVO>();
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = pClinicID, Status = true, IsQuarantineStore = false, Parent = 0 };
                    //BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    BizActionObj.ToStoreList.Insert(0, Default);
                    objlistStore.Add(Default);
                    objlistStore.AddRange(((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails);

                    // cmbFromStore.ItemsSource = BizActionObj.ItemMatserDetails;

                    var result = from item in objlistStore
                                 where item.ClinicId == pClinicID && item.Status == true
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
                    //cmbStore.ItemsSource = null;
                    //cmbStore.ItemsSource = (List<clsStoreVO>)result.ToList();
                    //cmbStore.SelectedItem = objlistStore[0];

                    //if (this.DataContext != null)
                    //{
                    //    cmbStore.SelectedValue = ((clsGRNVO)this.DataContext).StoreID;
                    //}
                    FillSupplier();
                }

            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillSupplier()
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

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbSupplier.ItemsSource = null;
                    cmbSupplier.ItemsSource = objList;
                    cmbSupplier.SelectedItem = objList[0];
                    
                    //if (this.DataContext != null)
                    //{
                    //    cmbSupplier.SelectedValue = ((clsGRNVO)this.DataContext).SupplierID;
                    //}
                }
                GetItemReorderList();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
           // if (Validation())
            GetItemReorderList();
        }

        void ReorderItemList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetItemReorderList();
            //throw new NotImplementedException();
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetItemReorderList();
            }
        }

        private void cmdIndent_Clicked(object sender, RoutedEventArgs e)
        {
            if (objLstIndent != null && objLstIndent.Count > 0)
            {
                FrmStoreIndent Indent = new FrmStoreIndent();
                //  Indent.IndentAddedItems = new ObservableCollection<clsIndentDetailVO>(objLstIndent);// objLstIndent ObservableCollection<clsIndentDetailVO>
                Indent.IsFromDashBoard = true;
                Indent.LstIndent = new List<clsIndentDetailVO>();
                Indent.LstIndent = objLstIndent.ToList();

                //   StoreIndent.FillIndentListForApprove(SelectedIndent.IndentNumber);
                //Indent.IsApprove = true;
                //Indent.IndentDetail = SelectedIndent;
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Store Indent";
                ((IApplicationConfiguration)App.Current).OpenMainContent(Indent);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Atleast One Item.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbox.Show();
            }
        }

        private void chkAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemReorderList.SelectedItem != null)
            {
                if (objLstIndent == null)
                    objLstIndent = new List<clsIndentDetailVO>();

                CheckBox chk = (CheckBox)sender;
                clsIndentDetailVO objindent = new clsIndentDetailVO();
                if (chk.IsChecked == true)
                {
                    objindent.ItemID = ((clsItemReorderDetailVO)dgItemReorderList.SelectedItem).ItemID;
                    objindent.ItemName = ((clsItemReorderDetailVO)dgItemReorderList.SelectedItem).ItemName;
                    objindent.UOMID = ((clsItemReorderDetailVO)dgItemReorderList.SelectedItem).PUMID;
                    objindent.SUOM = ((clsItemReorderDetailVO)dgItemReorderList.SelectedItem).SUM;
                    objindent.ConversionFactor = ((clsItemReorderDetailVO)dgItemReorderList.SelectedItem).ConversionFactor;
                    
                    objLstIndent.Add(objindent);
                }
                else
                {
                    foreach (clsIndentDetailVO item in objLstIndent.ToList())
                    {
                        bool status = false;
                        if (item.ItemID == ((clsItemReorderDetailVO)dgItemReorderList.SelectedItem).ItemID)
                        {
                            objLstIndent.Remove(item);
                            chk.IsChecked = false;
                            status = true;
                        }
                        if(status==true)
                        { 
                            break; 
                        }
                    }
                
                    
                }
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //if ((((MasterListItem)cmbStore.SelectedItem) != null && ((MasterListItem)cmbStore.SelectedItem).ID != 0) || (((MasterListItem)cmbSupplier.SelectedItem) != null &&
            //    ((MasterListItem)cmbSupplier.SelectedItem).ID != 0) || txtItemName.Text.Trim() != "")
            //{
                //string Parameters = "";
                long StoreID = 0;
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                long SuplierID = 0;
                string ItemName = "";

                //    ID = ((clsGRNVO)dgGRNList.SelectedItem).ID;
                StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                SuplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                ItemName = txtItemName.Text;
                // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

                string URL = "../Reports/InventoryPharmacy/ReOrderLevel.aspx?StoreID=" + StoreID + "&SuplierID=" + SuplierID + "&ItemName=" + ItemName + "&UnitID=" + UnitID + "&UserID="+UserID;// +"&GRNType=" + GRNType;

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
          //  }

            //else
            //{
            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select  Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //}
        }

        //private void chkAllStores_Click(object sender, RoutedEventArgs e)
        //{
        //    CheckBox chk = sender as CheckBox;
        //    List<clsItemStoreVO> objItemStoreList = new List<clsItemStoreVO>();
        //    objItemStoreList = (List<clsItemStoreVO>)dgStoreList.ItemsSource;
        //    if (chk.IsChecked == true)
        //    {
        //        try
        //        {
        //            if (objItemStoreList != null)
        //            {
        //                foreach (var item in objItemStoreList)
        //                {
        //                    item.status = true;
        //                    SelectedStoreList.Add(item);
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            if (objItemStoreList != null)
        //            {
        //                SelectedStoreList.Clear();
        //                foreach (var item in objItemStoreList)
        //                {
        //                    item.status = false;
        //                    //SelectedStoreList.Remove(item);
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    dgStoreList.ItemsSource = null;
        //    dgStoreList.ItemsSource = objItemStoreList;
        //    dgStoreList.UpdateLayout();
        //}

    }
}
