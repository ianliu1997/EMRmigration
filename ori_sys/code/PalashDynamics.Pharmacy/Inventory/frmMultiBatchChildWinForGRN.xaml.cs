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

namespace PalashDynamics.Pharmacy.ItemSearch
{
    public partial class frmMultiBatchChildWinForGRN : ChildWindow
    {
        private List<clsItemStockVO> BatchList;
        private List<clsItemStockVO> _BatchList;
        public clsItemStockVO SelectedBatch { get; set; }
        public event RoutedEventHandler OnBatchSelection;

        public frmMultiBatchChildWinForGRN()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmMultiBatchChildWinForGRN_Loaded);
            this.Unloaded += new RoutedEventHandler(frmMultiBatchChildWinForGRN_Unloaded);
        }

        public frmMultiBatchChildWinForGRN(List<clsItemStockVO> BatchList)
        {
            InitializeComponent();
            this.BatchList = BatchList;
            this.Loaded += new RoutedEventHandler(frmMultiBatchChildWinForGRN_Loaded);
            this.Unloaded += new RoutedEventHandler(frmMultiBatchChildWinForGRN_Unloaded);
        }

        void frmMultiBatchChildWinForGRN_Unloaded(object sender, RoutedEventArgs e)
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

        void frmMultiBatchChildWinForGRN_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.BatchList != null)
            {
                _BatchList = new List<clsItemStockVO>();
                _BatchList.Add(new clsItemStockVO() {BatchID = 0, BatchCode = "New Batch"  });
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
            if (e.Key.Equals(Key.Enter))
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
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}
