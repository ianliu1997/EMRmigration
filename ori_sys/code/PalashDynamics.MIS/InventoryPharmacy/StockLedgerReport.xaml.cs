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
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using System.Reflection;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class StockLedgerReport : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser; 
        List<long> SelectedItemsList = new List<long>();
        List<long> SelectedItemsList1 = new List<long>();
        public ObservableCollection<MasterListItem> StoreList { get; set; }
      
        public long ClinicID { get; set; }
       
        public StockLedgerReport()
        {
            InitializeComponent();
        }

        private void FillUnitList()
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
            
            dtpFromDate.SelectedDate = DateTime.Now.Date;

            FillUnitList();


            
            
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            
            if(chkValidation())
            {
                DateTime? Date = dtpFromDate.SelectedDate;
                long clinic = 0;

                //if (objList != null)
                //{
                //    foreach (var item in objList)
                //    {
                //        if (item.Status == true)
                //        {
                //            SelectedItemsList.Add(item.ID);
                //        }
                //        else
                //            SelectedItemsList1.Add(item.ID);
                //    }

                //}

                //string StoreID = "0";
                //if (SelectedItemsList.Count != 0)
                //{
                //    for (int i = 0; i <= SelectedItemsList.Count - 1; i++)
                //    {
                //        StoreID = StoreID + (StoreID == "" ? (SelectedItemsList[i].ToString()) : "," + (SelectedItemsList[i].ToString()));

                //    }
                //}

                //else
                //{
                //    for (int i = 0; i <= SelectedItemsList1.Count - 1; i++)
                //    {
                //        StoreID = StoreID + (StoreID == "" ? (SelectedItemsList1[i].ToString()) : "," + (SelectedItemsList1[i].ToString()));
                //    }
                //}

                //SelectedItemsList.Clear();
                //SelectedItemsList1.Clear();


                #region added by priyanka
                string StoreID = "";


                //if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                //{
                //    StoreID = null;
                //}
                //else 
                if (StoreList != null && StoreList.Count > 0)
                {
                    foreach (var item in StoreList)
                    {
                        StoreID = StoreID + item.ID;
                        StoreID = StoreID + ",";
                    }

                    if (StoreID.EndsWith(","))
                        StoreID = StoreID.Remove(StoreID.Length - 1, 1);

                }

                #endregion



                if (cmbClinic.SelectedItem != null)
                {
                    clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }

                string URL = "../Reports/InventoryPharmacy/StockLedgerReport.aspx?FromDate=" + Date + "&StoreID=" + StoreID + "&clinic=" + clinic;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            //else
            //{
            //    string msgTitle = "";
            //    string msgText = "Please Select Date";

            //    MessageBoxControl.MessageBoxChildWindow msgWD =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgWD.Show();
            //}

        }

        private bool chkValidation()
        {
            bool result = true;
            if (dtpFromDate.SelectedDate == null)
            {
                string msgTitle = "";
                string msgText = "Please Select Date";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
                result = false;
            }
            else if (cmbClinic.SelectedItem == null)
            {
                string msgTitle = "";
                string msgText = "Please Select Clinic";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
                result = false;

            }
            else if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
            {
                string msgTitle = "";
                string msgText = "Please Select Clinic";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
                result = false;
            }
            return result;
        }

        private void GetStore(long pClinicID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            if (pClinicID > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
            }


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> ObjList1 = new List<MasterListItem>();
                    ObjList1.Add(new MasterListItem { ID = 0, Description = "--All Stores--" });
                    ObjList1.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = ObjList1;
                    cmbStore.SelectedItem = ObjList1[0];

                    if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                    {

                        List<MasterListItem> objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        if (StoreList == null)
                            StoreList = new ObservableCollection<MasterListItem>();
                        foreach (MasterListItem item in objList)
                        {
                            item.Status = true;
                            StoreList.Add(item);
                        }

                        lstStore.ItemsSource = null;
                        lstStore.ItemsSource = StoreList;
                    }
                    
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbClinic.SelectedItem != null)
            {
                GetStore(((MasterListItem)cmbClinic.SelectedItem).ID);
                
                if (StoreList!=null && StoreList.Count >0)
                {
                    StoreList = new ObservableCollection<MasterListItem>();
                    lstStore.ItemsSource = null;
                }
            }
        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStore.SelectedItem != null && ((MasterListItem)cmbStore.SelectedItem).ID > 0)
            {
                MasterListItem tempstore = new MasterListItem();
                var item1 = from r in StoreList
                            where (r.ID == ((MasterListItem)cmbStore.SelectedItem).ID)
                            select new MasterListItem
                            {
                                Status = r.Status,
                                ID = r.ID,
                                Description = r.Description
                            };

                if (item1.ToList().Count == 0)
                {

                    tempstore.ID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    tempstore.Description = ((MasterListItem)cmbStore.SelectedItem).Description;
                    tempstore.Status = true;
                    StoreList.Add(tempstore);

                    lstStore.ItemsSource = null;
                    lstStore.ItemsSource = StoreList;

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Store already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            }
            else
            {
                if (cmbClinic.SelectedItem != null)
                {
                    GetStore(((MasterListItem)cmbClinic.SelectedItem).ID);
                }
            }


        }

        private void chkStores_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstStore.SelectedItem != null)
                {
                    StoreList.RemoveAt(lstStore.SelectedIndex);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
