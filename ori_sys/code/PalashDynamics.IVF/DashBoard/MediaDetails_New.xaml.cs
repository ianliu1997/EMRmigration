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
using PalashDynamics.ValueObjects.DashBoardVO;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class MediaDetails_New : ChildWindow
    {
        public long MPlanTherapyID;
        public long MPlanTherapyUnitID;
        public clsCoupleVO MCoupleDetails;
        public string MProcedureName ;

        public ObservableCollection<clsIVFDashboard_MediaDetailsVO> ItemList { get; set; }
        public bool IsUpdate = false;
        public long StoreID = 0;


        public MediaDetails_New(long PlanTherapyID, long PlanTherapyUnitID, clsCoupleVO CoupleDetails,string ProcedureName)
        {
            InitializeComponent();
            MPlanTherapyID = PlanTherapyID;
            MPlanTherapyUnitID = PlanTherapyUnitID;
            MCoupleDetails = CoupleDetails;
            MProcedureName = ProcedureName;
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

        private clsIVFDashboard_MediaDetailsVO _MediaDetailsList = new clsIVFDashboard_MediaDetailsVO ();
        public clsIVFDashboard_MediaDetailsVO MediaDetailsList
        {
            get
            {
                return _MediaDetailsList;
            }
            set
            {
                _MediaDetailsList = value;
            }
        }

        private ObservableCollection<clsIVFDashboard_MediaDetailsVO> _SpremFreezingDetails = new ObservableCollection<clsIVFDashboard_MediaDetailsVO>();
        public ObservableCollection<clsIVFDashboard_MediaDetailsVO> SpremFreezingDetails
        {
            get { return _SpremFreezingDetails; }
            set { _SpremFreezingDetails = value; }
        }

        private void FillMediaGrid()
        {
            try
            {
                clsIVFDashboard_GetMediaDetailsBizActionVO BizAction = new clsIVFDashboard_GetMediaDetailsBizActionVO();
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                BizAction.PlanTherapyID = MPlanTherapyID;
                BizAction.PlanTherapyUnitID = MPlanTherapyUnitID;
                BizAction.ProcedureName = MProcedureName;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    ItemList.Clear();
                     
                    if (((clsIVFDashboard_GetMediaDetailsBizActionVO)args.Result).MediaDetailsList.Count >0)
                    {
                        for (int i = 0; i < ((clsIVFDashboard_GetMediaDetailsBizActionVO)args.Result).MediaDetailsList.Count; i++)
                        {
                            //if (((clsIVFDashboard_GetMediaDetailsBizActionVO)args.Result).MediaDetailsList[i].Date == null || ((clsIVFDashboard_GetMediaDetailsBizActionVO)args.Result).MediaDetailsList[i].Date == Convert.ToDateTime("1/1/0001 12:00:00 AM"))

                            ItemList.Add(((clsIVFDashboard_GetMediaDetailsBizActionVO)args.Result).MediaDetailsList[i]);
                        } 
                        dgMedia.ItemsSource = null;
                        dgMedia.ItemsSource = ItemList;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
 
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (ItemList != null)
            {
                dgMedia.ItemsSource = ItemList;
            }
            else
            {
                ItemList = new ObservableCollection<clsIVFDashboard_MediaDetailsVO>();
            }
            FillStore();
            FillMediaGrid();
        }

        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbStore.SelectedItem != null)
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
                    else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                    {
                        cmbStore.TextBox.SetValidation("Please Select The Store");
                        cmbStore.TextBox.RaiseValidationError();

                    }
                }
                else
                {
                    if (cmbStore.SelectedItem == null)
                    {
                        cmbStore.TextBox.SetValidation("Please Select The Store");
                        cmbStore.TextBox.RaiseValidationError();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                                    select new clsIVFDashboard_MediaDetailsVO
                                    {
                                        Status = r.Status,
                                        ItemID = r.ItemID,
                                        ItemName = r.ItemName 
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsIVFDashboard_MediaDetailsVO objItem = new clsIVFDashboard_MediaDetailsVO();

                            objItem.ItemID = item.ItemID;
                            objItem.ItemName = item.ItemName;
                            objItem.BatchID = item.BatchID;
                            objItem.BatchCode = item.BatchCode;
                            objItem.ExpiryDate = item.ExpiryDate;
                            objItem.Date = System.DateTime.Now;
                            objItem.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                            objItem.VolumeUsed = 1;
                            StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                            objItem.AvailableQuantity =Convert.ToInt64(item.AvailableStock);
                            ItemList.Add(objItem);
                            dgMedia.ItemsSource = ItemList;
                            dgMedia.Focus();
                            dgMedia.UpdateLayout();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items Already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                }
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ItemList != null && ItemList.Count >0)
            {
                string msgText = "Are You Sure \n  You Want To Add Media Details ?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();                               
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Add Item To In Media Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                save(); 
            }
        }

        private void save() 
        {
            try
            {
                clsIVFDashboard_AddUpdateMediaDetailsBizActionVO BizAction = new clsIVFDashboard_AddUpdateMediaDetailsBizActionVO();
                BizAction.ObserMediaList = new ObservableCollection<clsIVFDashboard_MediaDetailsVO>();
                BizAction.ObserMediaList = ItemList;
                BizAction.MediaDetails.PlanTherapyID = MPlanTherapyID;
                BizAction.MediaDetails.PlanTherapyUnitID = MPlanTherapyUnitID;
                BizAction.MediaDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.PatientID;
                BizAction.MediaDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.UnitId;
                BizAction.MediaDetails.ProcedureName = MProcedureName;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsIVFDashboard_AddUpdateMediaDetailsBizActionVO)args.Result).ResultStatus != 0)
                        {
                            string msgText = "Details Saved Successfully !";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            FillMediaGrid();
                        }
                        else if (((clsIVFDashboard_AddUpdateMediaDetailsBizActionVO)args.Result).ResultStatus == 0)
                        {
                            string msgText = "Error In Saving.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
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
                string msgText = "Are You Sure \n You Want To Delete Selected Item ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {

                        if (((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).ID == 0 || ((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).ID == null)
                        {
                            ItemList.RemoveAt(dgMedia.SelectedIndex);

                        }
                        else
                        {
                            foreach (var item in ItemList)
                            {
                                if (item.ID == ((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).ID)
                                    item.StatusIS = false;
                            }
                        }
                        save();
                    }
                };
                msgWD.Show();
            }
        }

        private void dgMedia_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (((clsIVFDashboard_MediaDetailsVO)(row.DataContext)).Finalized == true)
            {
                e.Row.IsEnabled = false;
            }
        }

        private void txtVolume_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtVolume_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (dgMedia.SelectedItem != null)
            //{
            //    if (((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).AvailableQuantity < ((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).VolumeUsed)
            //    {
            //        string msgText = "Available stock is less than the quantity used";
            //        MessageBoxControl.MessageBoxChildWindow msgWD =
            //            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgWD.Show();
            //        ((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).VolumeUsed = 0;
            //    }
            //}
        }
        private void dgexpItem_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (dgMedia.SelectedItem != null)
            {
             if (e.Column.DisplayIndex == 6 && ((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).AvailableQuantity < ((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).VolumeUsed)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Available Stock Is Less Than The Quantity Used", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                    ((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).VolumeUsed = 1;
                }
                else if (e.Column.DisplayIndex == 6 && ((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).VolumeUsed <= 0)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Wrong Value Enterd.", "Volume used should be greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                    ((clsIVFDashboard_MediaDetailsVO)dgMedia.SelectedItem).VolumeUsed = 1;
                }
               
            }
        }

    }
}

