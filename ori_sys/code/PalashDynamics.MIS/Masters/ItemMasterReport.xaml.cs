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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using PalashDynamics.Collections;
using MessageBoxControl;
using System.Reflection;
using System.IO;
using System.Windows.Resources;
using System.Windows.Browser;



namespace PalashDynamics.MIS.Masters
{
    public partial class ItemMasterReport : UserControl
    {
        WaitIndicator wait = new WaitIndicator();
        public ItemMasterReport()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillMoleculeName();
            FillItemGroup();
            FillItemCategory();
            FillDispensingType();
            FillStoreageType();
            FillPreganancyClass();
            FillTheraputicClass();
            FillManufacturedBy();
        }

        private void FillMoleculeName()
        {
            wait.Show();
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Molecule;
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

                        cboMoleculeName.ItemsSource = null;
                        cboMoleculeName.ItemsSource = objList;
                        cboMoleculeName.SelectedItem = objList[0];

                    }


                    if (this.DataContext != null)
                    {
                        cboMoleculeName.SelectedValue = ((clsItemMasterVO)this.DataContext).MoleculeName;
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }

            catch (Exception ex)
            {

                throw ex;
                wait.Close();
            }


        }

        private void FillItemGroup()
        {
            WaitIndicator wait1 = new WaitIndicator();
            wait1.Show();
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

                if (this.DataContext != null)
                {
                    cboItemGroup.SelectedValue = ((clsItemMasterVO)this.DataContext).ItemGroup;
                }
                wait1.Close();
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillItemCategory()
        {
            wait.Show();
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


                    if (this.DataContext != null)
                    {
                        cboItemCategory.SelectedValue = ((clsItemMasterVO)this.DataContext).ItemCategory;
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }

            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void FillDispensingType()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DispensingType;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizAction, new clsUserVO());


            client.ProcessCompleted += (s, args) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();
                if (args.Error == null && args.Result != null)
                {
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cboDispensingType.ItemsSource = null;
                    cboDispensingType.ItemsSource = objList;
                    cboDispensingType.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cboDispensingType.SelectedValue = ((clsItemMasterVO)this.DataContext).DispencingType;
                    }
                }

                client.CloseAsync();
                wait.Close();
            };

        }

        private void FillStoreageType()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_StoreageType;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizAction, new clsUserVO());


            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cboStoreageType.ItemsSource = null;
                    cboStoreageType.ItemsSource = objList;
                    cboStoreageType.SelectedItem = objList[0];

                }

                if (this.DataContext != null)
                {
                    cboStoreageType.SelectedValue = ((clsItemMasterVO)this.DataContext).StoreageType;
                }

                client.CloseAsync();
                wait.Close();
            };

        }

        private void FillPreganancyClass()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PreganancyClass;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizAction, new clsUserVO());
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cbopregClass.ItemsSource = null;
                    cbopregClass.ItemsSource = objList;
                    cbopregClass.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cbopregClass.SelectedValue = ((clsItemMasterVO)this.DataContext).PregClass;
                }
                client.CloseAsync();
                wait.Close();
            };
        }
        private void FillTheraputicClass()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TherapeuticClass;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizAction, new clsUserVO());
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cboTheraClass.ItemsSource = null;
                    cboTheraClass.ItemsSource = objList;
                    cboTheraClass.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cboTheraClass.SelectedValue = ((clsItemMasterVO)this.DataContext).TherClass;
                }

                client.CloseAsync();
                wait.Close();
            };
        }
        private void FillManufacturedBy()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ItemCompany;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizAction, new clsUserVO());
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cboMfg.ItemsSource = null;
                    cboMfg.ItemsSource = objList;
                    cboMfg.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cboMfg.SelectedValue = ((clsItemMasterVO)this.DataContext).MfgBy;
                }
                client.CloseAsync();
                wait.Close();
            };
        }

        private void cboItemGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            FillDispensingType();
            if (cboItemGroup.SelectedItem != null)
            {
                if (((MasterListItem)cboItemGroup.SelectedItem).ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
                {
                    if (cboDispensingType.ItemsSource != null)
                        cboDispensingType.SelectedItem = ((List<MasterListItem>)(cboDispensingType.ItemsSource))[0];
                    //((clsItemMasterVO)this.DataContext).DispencingType = 0;

                    cboMoleculeName.SelectedItem = new MasterListItem(0, "-- Select --");
                    cboMoleculeName.SelectedValue = 0;
                    cboMoleculeName.SelectedValue = 0;
                    cboDispensingType.IsEnabled = false;
                    cboMoleculeName.IsEnabled = false;

                }
                else
                {

                    cboMoleculeName.IsEnabled = true;
                    cboDispensingType.IsEnabled = true;
                }
            }
        }

        long a, b, c, d, f, g, h, i;
        string ItemName = "";
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            ItemName = txtItemName.Text;
            a = ((MasterListItem)(cboMoleculeName.SelectedItem)).ID;
            b = ((MasterListItem)(cboItemGroup.SelectedItem)).ID;
            c = ((MasterListItem)(cboItemCategory.SelectedItem)).ID;
            d = ((MasterListItem)(cboDispensingType.SelectedItem)).ID;
            f = ((MasterListItem)(cboStoreageType.SelectedItem)).ID;
            g = ((MasterListItem)(cbopregClass.SelectedItem)).ID;
            h = ((MasterListItem)(cboTheraClass.SelectedItem)).ID;
            i = ((MasterListItem)(cboMfg.SelectedItem)).ID;


            long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            string URL = "../Reports/Administrator/ItemMasterReport.aspx?a=" + a + "&b=" + b + "&c=" + c + "&d=" + d + "&Uid=" + lUnitID + "&f=" + f + "&g=" + g + "&h=" + h + "&i=" + i + "&ItemName=" + ItemName + "&Excel=" + chkExcel.IsChecked;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");     
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Masters.frmRptMaster") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }
    }
}
