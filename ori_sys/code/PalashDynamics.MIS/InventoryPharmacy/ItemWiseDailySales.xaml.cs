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
using PalashDynamics.ValueObjects.Inventory;
using System.Windows.Browser;
using System.Collections.ObjectModel;
using CIMS;
using System.Reflection;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class ItemWiseDailySales : UserControl
    {
        #region Public Variables

        List<long> SelectedItemsList = new List<long>();
        public long SelectionCount;
        public long FilterBy;
        public long clinicId;
        public long storeId;
        public long IGroupId;
        public long ICatId;
        public long IDispensingId;
        public long TherClassId;
        public long MoleculeNameId;
        #endregion

        public ObservableCollection<clsItemMasterVO> ItemList { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public ItemWiseDailySales()
        {
            InitializeComponent();
        }

        private void GetItemMaster()
        {
            //try
            //{
            //    clsGetItemListBizActionVO BizAction = new clsGetItemListBizActionVO();
            //    BizAction.ItemList = new List<clsItemMasterVO>();
            //    BizAction.ItemDetails = new clsItemMasterVO();
            //    BizAction.ItemDetails.RetrieveDataFlag = false;
            //    BizAction.ForReportFilter = true;

            //    if (SelectionCount > 0)
            //    {
            //        BizAction.FilterCriteria = SelectionCount;
            //        BizAction.FilterIGroupID = IGroupId;
            //        BizAction.FilterICatId = ICatId;
            //        BizAction.FilterIDispensingId = IDispensingId;
            //        BizAction.FilterITherClassId = TherClassId;
            //        BizAction.FilterIMoleculeNameId = MoleculeNameId;

            //        //Added BY Pallavi
            //        BizAction.FilterClinicId = clinicId;
            //        if (cmbStore.SelectedItem != null)
            //        {
            //            BizAction.FilterStoreId = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            //        }
            //        //BizAction.FilterStoreId = storeId;

            //    }

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, args) =>
            //    {
            //        if (args.Error == null && args.Result != null)
            //        {


            //                List<MasterListItem> ObjList = new List<MasterListItem>();
            //                ObjList.Add(new MasterListItem { ID = 0, Description = "--All Items--" });
            //                if (((clsGetItemListBizActionVO)args.Result).MasterList != null && ((clsGetItemListBizActionVO)args.Result).MasterList.Count > 0)
            //                {
            //                    ObjList.AddRange(((clsGetItemListBizActionVO)args.Result).MasterList);
            //                }
            //                cmbItem.ItemsSource = null;
            //                cmbItem.ItemsSource = ObjList;
            //                cmbItem.SelectedItem = ObjList[0];






            //        }
            //    };

            //    client.ProcessAsync(BizAction, new clsUserVO());
            //    client.CloseAsync();
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Added By Pallavi
            ItemList = new ObservableCollection<clsItemMasterVO>();
            lstItems.ItemsSource = null;
            lstItems.ItemsSource = ItemList;
            FillClinic();
            FillDoctor();//***//
            GetItemMaster();
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            FillListItemGroup();
            FillListItemCategory();
            FillListItemDispensing();
            FillListTheraputicclass();
            FillListMoleculeName();
        }

        #region Added BY Pallavi
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
        #endregion

        private void FillListItemGroup()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            BizAction.MasterTable = MasterTableNameList.M_ItemGroup;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSelectItemGroup.ItemsSource = null;
                    cmbSelectItemGroup.ItemsSource = objList;
                    cmbSelectItemGroup.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbSelectItemGroup.SelectedValue = ((clsItemMasterVO)this.DataContext).ItemGroup;
                    //  cmbEmail.SelectedValue = ((clsCampMasterVO)this.DataContext).EmailTemplateID;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillListItemCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            BizAction.MasterTable = MasterTableNameList.M_ItemCategory;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSelectItemCategory.ItemsSource = null;
                    cmbSelectItemCategory.ItemsSource = objList;
                    cmbSelectItemCategory.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbSelectItemCategory.SelectedValue = ((clsItemMasterVO)this.DataContext).ItemCategory;
                    //  cmbEmail.SelectedValue = ((clsCampMasterVO)this.DataContext).EmailTemplateID;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillListItemDispensing()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            BizAction.MasterTable = MasterTableNameList.M_DispensingType;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- All--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSelectItemDispensing.ItemsSource = null;
                    cmbSelectItemDispensing.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbSelectItemDispensing.SelectedValue = ((clsItemMasterVO)this.DataContext).DispencingType;
                    //  cmbEmail.SelectedValue = ((clsCampMasterVO)this.DataContext).EmailTemplateID;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillListTheraputicclass()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            BizAction.MasterTable = MasterTableNameList.M_TherapeuticClass;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- All--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSelectTherClass.ItemsSource = null;
                    cmbSelectTherClass.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbSelectTherClass.SelectedValue = ((clsItemMasterVO)this.DataContext).TherClass;
                    //  cmbEmail.SelectedValue = ((clsCampMasterVO)this.DataContext).EmailTemplateID;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillListMoleculeName()
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            BizAction.MasterTable = MasterTableNameList.M_Molecule;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- All--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSelectMoleculeName.ItemsSource = null;
                    cmbSelectMoleculeName.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbSelectMoleculeName.SelectedValue = ((clsItemMasterVO)this.DataContext).MoleculeName;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long UnitID = 0;
            long UserID = 0;
            long StoreID = 0;
            long ItemGroup = 0;
            long ItemCategory = 0;
            long DispencingType = 0;
            long TherClass = 0;
            long MoleculeName = 0;
            long DoctorId = 0;
            string ItemName = string.Empty;
            

            if (!string.IsNullOrEmpty(txtItemName.Text))
            {
                ItemName = txtItemName.Text.ToString();
            }

            UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

            if (cmbClinic.SelectedItem != null)
            {
                UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            if (cmbStore.SelectedItem != null)
            {
                StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            }

            if (cmbSelectItemGroup.SelectedItem != null)
            {
                ItemGroup = ((MasterListItem)cmbSelectItemGroup.SelectedItem).ID;
            }
            if (cmbSelectItemCategory.SelectedItem != null)
            {
                ItemCategory = ((MasterListItem)cmbSelectItemCategory.SelectedItem).ID;
            }
            if (cmbSelectItemDispensing.SelectedItem != null)
            {
                DispencingType = ((MasterListItem)cmbSelectItemDispensing.SelectedItem).ID;
            }
            if (cmbSelectTherClass.SelectedItem != null)
            {
                TherClass = ((MasterListItem)cmbSelectTherClass.SelectedItem).ID;
            }
            if (cmbSelectMoleculeName.SelectedItem != null)
            {
                MoleculeName = ((MasterListItem)cmbSelectMoleculeName.SelectedItem).ID;
            }

            if (cmbDoctor.SelectedItem != null) //***//
            {
                DoctorId = ((MasterListItem)cmbDoctor.SelectedItem).ID;
            }

            #region commented

            //string Items = "";
            //if (SelectedItemsList!=null)
            //{
            //    if (SelectedItemsList.Count > 0)
            //    {
            //        for (int i = 0; i < SelectedItemsList.Count-1; i++)
            //        {
            //            Items = Items + SelectedItemsList[i].ToString();
            //        }
            //    }  
            //}

            //List<clsItemMasterVO> objList = ItemList.ToList<clsItemMasterVO>();

            //    if (objList != null)
            //    {
            //        foreach (var item in objList)
            //        {
            //            if (item.IsSelected == true)
            //            {
            //                SelectedItemsList.Add(item.ID);
            //            }

            //        }
            //    }
            #endregion

            //DateTime? dtpF = null;
            //DateTime? dtpT = null;
            //DateTime? dtpP = null;
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;

            string msgTitle = "";
            bool chkToDate = true;

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
            #region Commented
            //if (IsAllCheck==false)
            //{
            //    if (SelectedItemsList.Count != 0)
            //    {
            //        for (int i = 0; i <= SelectedItemsList.Count - 1; i++)
            //        {
            //            // ItemIDs = ItemIDs + (ItemIDs == "" ? ("'" + SelectedItemsList[i].ToString() + "''") : "," + ("''" + SelectedItemsList[i].ToString() + "''"));
            //            ItemIDs = ItemIDs + (ItemIDs == "" ? (SelectedItemsList[i].ToString()) : "," + (SelectedItemsList[i].ToString()));
            //        }
            //    }
            //    else
            //    {
            //        ItemIDs = null;
            //    }

            //}
            //else
            //{

            //    ItemIDs = null;
            //}

            //SelectedItemsList.Clear();
            #endregion

            List<clsItemMasterVO> objList = ItemList.ToList<clsItemMasterVO>();


            if (objList != null)
            {
                foreach (var item in objList)
                {
                    if (item.IsSelected == true)
                    {
                        SelectedItemsList.Add(item.ID);
                    }
                }
            }

            string ItemIDs = "";


            if (cmbItem.SelectedItem != null && ((MasterListItem)cmbItem.SelectedItem).ID == 0)
            {
                ItemIDs = null;
            }
            else if (ItemList != null && ItemList.Count > 0)
            {
                foreach (var item in ItemList)
                {
                    ItemIDs = ItemIDs + item.ID;
                    ItemIDs = ItemIDs + ",";
                }

                if (ItemIDs.EndsWith(","))
                    ItemIDs = ItemIDs.Remove(ItemIDs.Length - 1, 1);

            }
            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/InventoryPharmacy/ItemWiseDailySales.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ItemIDs=" + ItemIDs + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&UnitID=" + UnitID + "&StoreID=" + StoreID + "&ItemGroup=" + ItemGroup + "&ItemCategory=" + ItemCategory + "&DispencingType=" + DispencingType + "&TherClass=" + TherClass + "&MoleculeName=" + MoleculeName + "&Excel=" + chkExcel.IsChecked + "&UserID=" + UserID + "&DoctorId=" + DoctorId + "&ItemName=" + ItemName + "&IsPackageSalesShow=" + chkIsPackage.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/InventoryPharmacy/ItemWiseDailySales.aspx?ItemIDs=" + ItemIDs + "&UnitID=" + UnitID + "&StoreID=" + StoreID + "&ItemGroup=" + ItemGroup + "&ItemCategory=" + ItemCategory + "&DispencingType=" + DispencingType + "&TherClass=" + TherClass + "&MoleculeName=" + MoleculeName + "&Excel=" + chkExcel.IsChecked + "&UserID=" + UserID + "&DoctorId=" + DoctorId + "&ItemName=" + ItemName + "&IsPackageSalesShow=" + chkIsPackage.IsChecked;
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

        //private void chkStores_Checked(object sender, RoutedEventArgs e)
        //{

        //CheckBox objCheckBox = (CheckBox)sender;
        //if (objCheckBox.IsChecked==true)
        //{
        //    long tag = 0;
        //    if (objCheckBox.Tag != null)
        //    {
        //        tag = (long)objCheckBox.Tag;
        //        SelectedItemsList.Add(tag);
        //    }
        //}

        //  clsItemMasterVO objItemVo = (clsItemMasterVO)lstStores.ItemsSource;

        //}

        //private void chkStores_Unchecked(object sender, RoutedEventArgs e)
        //{

        //CheckBox objCheckBox = (CheckBox)sender;
        //if (objCheckBox.IsChecked == false)
        //{
        //    if (objCheckBox.Tag != null)
        //    {
        //        long tag = (long)objCheckBox.Tag;
        //        SelectedItemsList.Remove(tag);
        //    }
        //}

        //}

        //private void chkAllStores_Checked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        CheckBox objCheckBox = (CheckBox)sender;
        //        List<clsItemMasterVO> objList = (List<clsItemMasterVO>)lstItems.ItemsSource;
        //        if (objCheckBox != null)
        //        {
        //            if (objCheckBox.IsChecked == true)
        //            {
        //                if (objList != null && objList.Count > 0)
        //                {
        //                    foreach (var item in objList)
        //                    {
        //                        SelectedItemsList.Add(item.ID);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //private void chkAllStores_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        CheckBox objCheckBox = (CheckBox)sender;
        //        List<clsItemMasterVO> objList = (List<clsItemMasterVO>)lstItems.ItemsSource;
        //        if (objCheckBox != null)
        //        {
        //            if (objCheckBox.IsChecked == true)
        //            {
        //                if (objList != null && objList.Count > 0)
        //                {
        //                    foreach (var item in objList)
        //                    {
        //                        SelectedItemsList.Remove(item.ID);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //private void chkAllStores_Click(object sender, RoutedEventArgs e)
        //{

        //}


        private void cmbSelectItemGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get the id of the selected item and filter the item list .
            if (cmbSelectItemGroup.SelectedItem != null)
            {
                SelectionCount = SelectionCount + 1;
                IGroupId = ((MasterListItem)cmbSelectItemGroup.SelectedItem).ID;
                GetItemMaster();
            }
        }

        private void cmbSelectItemCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelectItemCategory.SelectedItem != null)
            {
                SelectionCount = SelectionCount + 1;
                ICatId = ((MasterListItem)cmbSelectItemCategory.SelectedItem).ID;
                GetItemMaster();
            }
        }

        private void cmbSelectItemDispensing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelectItemDispensing.SelectedItem != null)
            {
                SelectionCount = SelectionCount + 1;
                IDispensingId = ((MasterListItem)cmbSelectItemDispensing.SelectedItem).ID;
                GetItemMaster();
            }
        }

        private void cmbSelectTherClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelectTherClass.SelectedItem != null)
            {
                SelectionCount = SelectionCount + 1;
                TherClassId = ((MasterListItem)cmbSelectTherClass.SelectedItem).ID;
                GetItemMaster();
            }
        }

        private void cmbSelectMoleculeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelectMoleculeName.SelectedItem != null)
            {
                SelectionCount = SelectionCount + 1;
                MoleculeNameId = ((MasterListItem)cmbSelectMoleculeName.SelectedItem).ID;
                GetItemMaster();
            }
        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        #region Added By Pallavi
        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbClinic.SelectedItem != null)
            {
                clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
                SelectionCount = SelectionCount + 1;
                FillStores(clinicId);
                GetItemMaster();
            }
        }
        private void FillStores(long clinicId)
        {
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            ////BizAction.IsActive = true;
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.MasterList = new List<MasterListItem>();

            //if (clinicId > 0)
            //{
            //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
            //}

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();

            //        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
            //        objList.Add(new MasterListItem(0, "- Select -"));
            //        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

            //        //cmbBloodGroup.ItemsSource = null;
            //        //cmbBloodGroup.ItemsSource = objList;
            //        cmbStore.ItemsSource = null;
            //        cmbStore.ItemsSource = objList;

            //        cmbStore.SelectedItem = objList[0];

            //    }
            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();

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
                    //  BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == clinicId && item.Status == true//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;
                    List<clsStoreVO> tmpList = new List<clsStoreVO>();
                    tmpList = result.ToList();
                    tmpList.Insert(0, Default);
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        //if (result.ToList().Count > 0)
                        //{
                        cmbStore.ItemsSource = tmpList.ToList();
                        cmbStore.SelectedItem = tmpList.ToList()[0];
                        //}
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
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStore.SelectedItem != null && ((clsStoreVO)cmbStore.SelectedItem).StoreId != 0)
            {
                storeId = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                SelectionCount = SelectionCount + 1;
                GetItemMaster();
            }
        }
        #endregion

        private void chkAllStores_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                List<clsItemMasterVO> objItemStoreList = new List<clsItemMasterVO>();
                // objItemStoreList = (List<clsItemMasterVO>)lstItems.ItemsSource;
                //objItemStoreList = lstItems.ItemsSource;
                foreach (var item in lstItems.ItemsSource)
                {
                    objItemStoreList.Add((clsItemMasterVO)item);
                }
                if (objItemStoreList != null)
                {

                    foreach (var item in objItemStoreList)
                    {
                        item.Status = true;
                        item.IsSelected = true;
                    }



                }
                lstItems.ItemsSource = null;
                lstItems.ItemsSource = objItemStoreList;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void chkAllStores_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {

                List<clsItemMasterVO> objItemStoreList = new List<clsItemMasterVO>();
                //objItemStoreList = (List<clsItemMasterVO>)lstItems.ItemsSource;

                foreach (var item in lstItems.ItemsSource)
                {
                    objItemStoreList.Add((clsItemMasterVO)item);
                }
                if (objItemStoreList != null)
                {

                    foreach (var item in objItemStoreList)
                    {
                        item.Status = false;
                        item.IsSelected = false;
                    }



                }
                lstItems.ItemsSource = null;
                lstItems.ItemsSource = objItemStoreList;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        private void chkStores_Unchecked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstItems.SelectedItem != null)
                {
                    ItemList.RemoveAt(lstItems.SelectedIndex);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbItem.SelectedItem != null && ((MasterListItem)cmbItem.SelectedItem).ID > 0)
            {
                clsItemMasterVO tempItem = new clsItemMasterVO();
                var item1 = from r in ItemList
                            where (r.ID == ((MasterListItem)cmbItem.SelectedItem).ID)
                            select new clsItemMasterVO
                            {
                                Status = r.Status,
                                ID = r.ID,
                                ItemName = r.ItemName
                            };

                if (item1.ToList().Count == 0)
                {

                    tempItem.ID = ((MasterListItem)cmbItem.SelectedItem).ID;
                    tempItem.ItemName = ((MasterListItem)cmbItem.SelectedItem).Description;
                    tempItem.IsSelected = true;
                    ItemList.Add(tempItem);

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            }
        }

        private void cmbItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbItem.SelectedItem != null)
                {
                    if (((MasterListItem)cmbItem.SelectedItem).ID == 0)
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

        private void FillDoctor() //***//
        {
            try
            {
                clsGetDoctorListForComboBizActionVO BizAction = new clsGetDoctorListForComboBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetDoctorListForComboBizActionVO)arg.Result).MasterList);

                        cmbDoctor.ItemsSource = null;
                        cmbDoctor.ItemsSource = objList;
                        cmbDoctor.SelectedItem = objList[0];
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

    }
}
