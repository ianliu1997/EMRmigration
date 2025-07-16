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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;

namespace PalashDynamics.IPD
{
    public partial class PrimarySymptomsSearchWindowForIPD : ChildWindow
    {
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        #region 'Paging'

        public PagedSortableCollectionView<MasterListItem> DataList { get; private set; }

        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillMasterSearchList();

        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        private void FillMasterSearchList()
        {
            indicator.Show();

            clsGetSearchMasterListBizActionVO BizAction = new clsGetSearchMasterListBizActionVO();

            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            //BizAction.IsActive = true;


            BizAction.MasterTable = MasterTableNameList.M_PrimarySymptoms;
            BizAction.Description = txtPrimarySymptom.Text;


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetSearchMasterListBizActionVO result = e.Result as clsGetSearchMasterListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;

                    if (result.MasterList != null)
                    {
                        DataList.Clear();
                        foreach (var item in result.MasterList)
                        {
                            DataList.Add(item);
                        }

                        dgList.ItemsSource = null;
                        dgList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList;
                    }

                }
                indicator.Close();

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #endregion
        int ClickedFlag = 0;


        public event RoutedEventHandler OnSaveButton_Click;

        private ObservableCollection<MasterListItem> _ItemSelected;
        public ObservableCollection<MasterListItem> SelectedItems { get { return _ItemSelected; } }


        public PrimarySymptomsSearchWindowForIPD()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PrimarySymptomsSearchWindowForIPD_Loaded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<MasterListItem>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 8;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
        }

        void PrimarySymptomsSearchWindowForIPD_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //======================================================
            //Paging
            FillMasterSearchList();
            //======================================================
        }

        private void dgList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            bool IsValid = true;

            if (ClickedFlag == 1)
            {
                if (_ItemSelected == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Symptoms not Selected", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValid = false;
                    msgW1.Show();

                }
                else if (_ItemSelected.Count == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Symptoms not Selected", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValid = false;
                    msgW1.Show();
                }

                if (IsValid)
                {

                    this.DialogResult = true;
                    if (OnSaveButton_Click != null)
                        OnSaveButton_Click(this, new RoutedEventArgs());
                }
                else
                    ClickedFlag = 0;
            }
        }

        private void cmdCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            //======================================================
            //Paging
            dgDataPager.PageIndex = 0;
            FillMasterSearchList();
            //======================================================
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgList.SelectedItem != null)
            {
                if (_ItemSelected == null)
                    _ItemSelected = new ObservableCollection<MasterListItem>();

                CheckBox chk = (CheckBox)sender;
                if (chk.IsChecked == true)

                    _ItemSelected.Add((MasterListItem)dgList.SelectedItem);
                else
                    _ItemSelected.Remove((MasterListItem)dgList.SelectedItem);

            }
        }
    }
}

