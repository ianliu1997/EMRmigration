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
using System.Windows.Browser;
using CIMS;
using System.Reflection;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class ReturnsFromClinic : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public long StoreID { get; set; }
        public ReturnsFromClinic()
        {
            InitializeComponent();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
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
                    dtpT = dtpT.Value.AddDays(1);
                    dtpToDate.Focus();
                }
            }

            if (dtpT != null)
            {
                if (dtpF != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

                    //if (dtpF.Equals(dtpT))
                    //dtpT = dtpT.Value.Date.AddDays(1);
                }
            }

            long FromStore = 0;
            long ToStore = 0;
            long UnitID = 0;

            if (cmbFromStore.SelectedItem != null)
            {
                FromStore = ((clsStoreVO)cmbFromStore.SelectedItem).ID;
            }

            if (cmbToStore.SelectedItem != null)
            {
                ToStore = ((clsStoreVO)cmbToStore.SelectedItem).ID;
            }

            if (cmbClinic.SelectedItem != null)
            {
                UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }

            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/InventoryPharmacy/ReturnsFromClinic.aspx?FromStore=" + FromStore + "&ToStore=" + ToStore + "&FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&UnitID=" + UnitID + "&Excel=" + chkExcel.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/InventoryPharmacy/ReturnsFromClinic.aspx?FromStore=" + FromStore + "&ToStore=" + ToStore + "&UnitID=" + UnitID + "&Excel=" + chkExcel.IsChecked;
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
                        cmbFromStore.ItemsSource = null;
                        cmbFromStore.ItemsSource = ClinicWiseStores.ToList();
                        cmbFromStore.SelectedItem = ClinicWiseStores.ToList()[0];

                        cmbToStore.ItemsSource = null;
                        cmbToStore.ItemsSource = ClinicWiseStores.ToList();
                        cmbToStore.SelectedItem = ClinicWiseStores.ToList()[0];
                    }
                    else
                    {
                        //User assigned Stores
                        cmbFromStore.ItemsSource = null;
                        cmbFromStore.ItemsSource = BizActionObj.ToStoreList;
                        cmbFromStore.SelectedItem = BizActionObj.ToStoreList[0];

                        cmbToStore.ItemsSource = null;
                        cmbToStore.ItemsSource = ClinicWiseStores.ToList();
                        cmbToStore.SelectedItem = ClinicWiseStores.ToList()[0];
                    }
                }
            };
            client.CloseAsync();
        }


        //private void FillFromStore(long ClinicID)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_Store;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = ClinicID.ToString() };


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);


        //            cmbFromStore.ItemsSource = null;
        //            cmbFromStore.ItemsSource = objList;

        //            if (StoreID > 0)
        //            {
        //                cmbFromStore.SelectedValue = StoreID;
        //            }
        //            else
        //            {
        //                if (objList.Count > 1)
        //                    cmbFromStore.SelectedItem = objList[1];
        //                else
        //                    cmbFromStore.SelectedItem = objList[0];
        //            }
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();

        //}

        //private void FillToStore()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_Store;
        //    BizAction.MasterList = new List<MasterListItem>();


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);


        //            cmbToStore.ItemsSource = null;
        //            cmbToStore.ItemsSource = objList;

        //            if (StoreID > 0)
        //            {
        //                cmbToStore.SelectedValue = StoreID;
        //            }
        //            else
        //            {
        //                if (objList.Count > 1)
        //                    cmbToStore.SelectedItem = objList[1];
        //                else
        //                    cmbToStore.SelectedItem = objList[0];
        //            }
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            FillStores(ClinicId);
            //FillFromStore(ClinicId);
            //FillToStore();
            FillClinic();
        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        long ClinicId = 0;
        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null)
                {
                    ClinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    //FillFromStore(ClinicId);
                    if (ClinicId > 0)
                        FillStores(ClinicId);
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }
    }
}
