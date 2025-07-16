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
using PalashDynamics.ValueObjects;
using PalashDynamics.Pharmacy.ItemSearch;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.IVF
{
    public partial class MediaDetails : ChildWindow
    {
        public ObservableCollection<clsFemaleMediaDetailsVO> ItemList { get; set; }
        public bool IsUpdate = false;
        public event RoutedEventHandler OnSaveButton_Click;
        public long StoreID=0;
        public MediaDetails()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void FillStore()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;
                    cmbStore.SelectedItem = objList[0];
                    if (StoreID > 0)
                    {
                        cmbStore.SelectedValue = StoreID;
                    }
                  
                   

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (ItemList!=null)
            {
                dgMedia.ItemsSource = ItemList;
            }
            else
            {
                ItemList = new ObservableCollection<clsFemaleMediaDetailsVO>();

                
            }
            FillStore();
            
        }

        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((MasterListItem)cmbStore.SelectedItem).ID > 0)
                {
                    cmbStore.ClearValidationError();
                  

                    ItemListNew ItemsWin = new ItemListNew();

                   // ItemsWin.StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologyStoreID;
                    ItemsWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    ItemsWin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                    ItemsWin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    ItemsWin.ShowBatches = true;
                    ItemsWin.cmbStore.IsEnabled = false;


                    ItemsWin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                    ItemsWin.Show();
                }
                else
                {
                    if (cmbStore.SelectedItem == null)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();

                    }
                    else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();

                    }



                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew Itemswin = (ItemListNew)sender;
            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.ItemBatchList != null)
                {
                    foreach (var item in Itemswin.ItemBatchList)
                    {
                        var item1 = from r in ItemList
                                    where (r.BatchID == item.BatchID)
                                    select new clsFemaleMediaDetailsVO
                                    {
                                        Status = r.Status,
                                        ItemID = r.ItemID,
                                        ItemName = r.ItemName,

                                    };
                        if (item1.ToList().Count == 0)
                        {

                            clsFemaleMediaDetailsVO objItem = new clsFemaleMediaDetailsVO();

                            objItem.ItemID = item.ItemID;
                            objItem.ItemName = item.ItemName;
                            objItem.BatchID = item.BatchID;
                            objItem.BatchCode = item.BatchCode;
                            objItem.ExpiryDate = item.ExpiryDate;
                            objItem.Date = System.DateTime.Now;
                            
                            objItem.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                            StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;

                            ItemList.Add(objItem);
                            dgMedia.ItemsSource = ItemList;
                            dgMedia.Focus();
                            dgMedia.UpdateLayout();
                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                }
            }
        }
        
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
           
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
       
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void DeleteItems_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedia.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ItemList.RemoveAt(dgMedia.SelectedIndex);
                        
                    }
                };

                msgWD.Show();
            }
        }
        
    }
}

