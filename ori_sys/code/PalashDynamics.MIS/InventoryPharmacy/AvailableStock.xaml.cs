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
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
using CIMS;
using System.Runtime.InteropServices.Automation;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class AvailableStock : UserControl
    {
        public ObservableCollection<clsItemStockVO> StockList { get; set; }

        public double PStock { get; set; }
        public double AStock { get; set; }        
      
        public AvailableStock()
        {
            InitializeComponent();
            //dgPharmacyItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgPharmacyItems_CellEditEnded);
        }
        
        //void dgPharmacyItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        //{
        //    //if (e.Column.DisplayIndex == 3)
        //    //{
        //    //    PStock = ((clsItemStockVO)dgPharmacyItems.SelectedItem).PhysicalStock;
        //    //    AStock = ((clsItemStockVO)dgPharmacyItems.SelectedItem).AvailableStock;
        //    //    double CP = ((clsItemStockVO)dgPharmacyItems.SelectedItem).PurchaseRate;

        //        //double VarianceStock = PStock - AStock;
        //        //((clsItemStockVO)dgPharmacyItems.SelectedItem).VarianceStock = VarianceStock;

        //        //double VarianceAmount = VarianceStock * CP;
        //        //((clsItemStockVO)dgPharmacyItems.SelectedItem).VarianceAmount = VarianceAmount;


                
        //    }
        //}
        
        //private void FillGrid()
        //{
        //    clsGetAvailableStockListBizActionVO BizAction = new clsGetAvailableStockListBizActionVO();
        //    BizAction.BatchList = new List<clsItemStockVO>();
        //    if (dtpFromDate.SelectedDate != null)
        //    {
        //        BizAction.FromDate = dtpFromDate.SelectedDate;
        //    }

        //    if (dtpToDate.SelectedDate != null)
        //    {
        //        BizAction.ToDate = dtpToDate.SelectedDate;
        //    }
            
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    dgPharmacyItems.ItemsSource = null;
        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            StockList = new ObservableCollection<clsItemStockVO>();
        //            for (int i = 0; i < ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList.Count; i++)
        //            {
        //                StockList.Add(new clsItemStockVO()
        //                {

        //                    ItemCode = ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList[i].ItemCode,
        //                    ItemName = ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList[i].ItemName,
        //                    BatchCode = ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList[i].BatchCode,
        //                    //PhysicalStock = ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList[i].PhysicalStock,
        //                    AvailableStock = ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList[i].AvailableStock,
        //                    //VarianceStock = ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList[i].VarianceStock,
        //                    PurchaseRate = ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList[i].PurchaseRate,
        //                    //VarianceAmount = ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList[i].VarianceAmount,

        //                }
        //                );

        //            }

        //            dgPharmacyItems.ItemsSource = null;
        //            dgPharmacyItems.ItemsSource = StockList;

        //            //if (((clsGetAvailableStockListBizActionVO)arg.Result).BatchList != null)
        //            //{
        //            //    dgPharmacyItems.ItemsSource = ((clsGetAvailableStockListBizActionVO)arg.Result).BatchList;


        //            //}
        //        }
        //        else
        //        {

                    
        //        }
        //    };

        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
                       
                        

        //}

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            FillClinic();
            //FillGrid();
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
        }

        private void FillClinic()
        {
            try
            {
                clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
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

        private void cmdPrint_Click_1(object sender, RoutedEventArgs e)
        {
            //DateTime? dtpF = null;
            //DateTime? dtpT = null;
            //DateTime? dtpP = null;

            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;

            bool chkToDate = true;
            string msgTitle = "";

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
                    //dtpT = dtpT.Value.Date.AddDays(1);
                    dtpT = dtpT.Value.AddDays(1);
                    dtpToDate.Focus();
                }
            }

            //if (dtpT != null)
            //{
            //    if (dtpF != null)
            //    {
            //        dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

            //        if (dtpF.Equals(dtpT))
            //            dtpT = dtpF.Value.Date.AddDays(1);
            //    }
            //}

            long clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
            long store = ((clsStoreVO)cmbStore.SelectedItem).StoreId;

            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/InventoryPharmacy/AvailableStock.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&clinic=" + clinic + "&store=" + store;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/InventoryPharmacy/AvailableStock.aspx?clinic=" + clinic + "&store=" + store;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");


                }
            }
            else
            {
                string msgText = "Incorrect Date Range. To Date cannot be greater than From Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }            
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            //FillGrid();
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;

            FillStores(clinicId);
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
                    //BizActionObj.ItemMatserDetails.Insert(0, Default);
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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       

       

       
    }
}
