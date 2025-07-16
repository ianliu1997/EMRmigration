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
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.UserControls;

namespace OPDModule.Forms
{
    public partial class frmMultiBatchChildWinForCS : ChildWindow
    {
        private List<clsItemStockVO> BatchList;
        private List<clsItemStockVO> _BatchList;
        public clsItemStockVO SelectedBatch { get; set; }
        public event RoutedEventHandler OnBatchSelection;
        WaitIndicator indicator = new WaitIndicator();
        public long StoreID { get; set; }

        //public clsItemMasterVO SelectedItemObj { get; set; }   //For Item Selection Control

        public frmMultiBatchChildWinForCS()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmMultiBatchChildWinForCS_Loaded);
            this.Unloaded += new RoutedEventHandler(frmMultiBatchChildWinForCS_Unloaded);
        }

        public frmMultiBatchChildWinForCS(List<clsItemStockVO> BatchList)
        {
            InitializeComponent();

            this.BatchList = BatchList;
            this.StoreID = StoreID;

            this.Loaded += new RoutedEventHandler(frmMultiBatchChildWinForCS_Loaded);
            this.Unloaded += new RoutedEventHandler(frmMultiBatchChildWinForCS_Unloaded);
        }

        void frmMultiBatchChildWinForCS_Unloaded(object sender, RoutedEventArgs e)
        {
            if (OnBatchSelection != null)
            {
                if (ItemBatches.SelectedItem != null)
                {
                    SelectedBatch = (clsItemStockVO)ItemBatches.SelectedItem;
                }
                else
                {
                    if (_BatchList != null)
                    {
                        if (_BatchList.Count > 0)
                            SelectedBatch = _BatchList[0];
                    }
                }

                OnBatchSelection(this, new RoutedEventArgs());
            }
        }

        void frmMultiBatchChildWinForCS_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.BatchList != null)
            {
                _BatchList = new List<clsItemStockVO>();
                //_BatchList.Add(new clsItemStockVO() { BatchID = 0, BatchCode = "New Batch" });
                List<clsItemStockVO> BatchList1 = BatchList.OrderBy(S => S.ExpiryDate).ToList<clsItemStockVO>();
                _BatchList.AddRange(BatchList1);
                ItemBatches.ItemsSource = _BatchList;
                if (BatchList.Count > 0)
                {
                    ItemBatches.SelectedItem = _BatchList[0];
                    SelectedBatch = _BatchList[0];
                }
            }
        }

        private void ItemBatches_KeyDown(object sender, KeyEventArgs e)
        {
            #region Commented
            //if (e.Key.Equals(Key.Enter))
            //{
            //    if (OnBatchSelection != null)
            //    {
            //        if (ItemBatches.SelectedItem != null)
            //        {
            //            SelectedBatch = (clsItemStockVO)ItemBatches.SelectedItem;
            //        }
            //        else
            //        {
            //            if (_BatchList != null)
            //            {
            //                if (_BatchList.Count > 0)
            //                    SelectedBatch = _BatchList[0];
            //            }
            //        }

            //        OnBatchSelection(this, new RoutedEventArgs());
            //    }
            //}
            #endregion

            if (e.Key.Equals(Key.Enter))
            {
                if (OnBatchSelection != null)
                {
                    if (ItemBatches.SelectedItem != null)
                    {
                        SelectedBatch = (clsItemStockVO)ItemBatches.SelectedItem;

                        #region For Item Selection Control

                        try
                        {
                            indicator.Show();
                            clsGetItemDetailsByIDBizActionVO BizActionObject = new clsGetItemDetailsByIDBizActionVO();

                            //BizActionObject.ItemList = new List<clsItemMasterVO>();
                            //BizActionObject.MasterList = new List<MasterListItem>();

                            BizActionObject.StoreID = Convert.ToInt64(SelectedBatch.StoreID);
                            BizActionObject.ItemID = SelectedBatch.ItemID;

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, ea) =>
                            {
                                if (ea.Result != null && ea.Error == null)
                                {
                                    clsGetItemDetailsByIDBizActionVO result = ea.Result as clsGetItemDetailsByIDBizActionVO;

                                    //SelectedItemObj = result.objItem;
                                    SelectedBatch.SelectedItemObj = result.objItem;

                                    indicator.Close();
                                    OnBatchSelection(this, new RoutedEventArgs());
                                }
                                else
                                {
                                    indicator.Close();
                                    OnBatchSelection(this, new RoutedEventArgs());
                                }
                            };
                            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();

                        }
                        catch (Exception ex)
                        {
                            indicator.Close();
                            throw;
                        }

                        #endregion

                    }
                    else
                    {
                        if (_BatchList != null)
                        {
                            if (_BatchList.Count > 0)
                                SelectedBatch = _BatchList[0];
                        }
                    }

                    //OnBatchSelection(this, new RoutedEventArgs());
                }
            }

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

    }
}

