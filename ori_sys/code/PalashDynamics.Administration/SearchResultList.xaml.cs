using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;

namespace PalashDynamics.Administration
{
    public partial class SearchResultList : UserControl
    {
        public string LabelOfList { get; set; }
        public List<MasterListItem> lstObjectList = null;
        public List<MasterListItem> grid2ViewList = null;
        public string labelOfDescriptionOnGrid { get; set; }
        private bool allFlag = false;
        public MasterTableNameList TableName { get; set; }

        public PagedSortableCollectionView<MasterListItem> MasterList { get; private set; }

        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
            }
        }


        public SearchResultList()
        {
            InitializeComponent();
            MasterList = new PagedSortableCollectionView<MasterListItem>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;
            dataGrid2Pager.PageSize = PageSize;
            dataGrid2Pager.Source = MasterList;
            //======================================================
        }

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillGrid();
        }

        private void cmdAdd1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstObjectList = null;

                if (dgSearch2.ItemsSource != null)
                {
                    if (lstObjectList == null)
                    {
                        lstObjectList = new List<MasterListItem>();
                    }
                    foreach (var item in dgSearch2.ItemsSource)
                    {
                        var obj = lstObjectList.FirstOrDefault(q => q.ID == ((MasterListItem)item).ID);
                        if (obj != null)
                            continue;
                        else
                            lstObjectList.Add((MasterListItem)item);
                    }
                }
                else
                    if (lstObjectList == null)
                        lstObjectList = new List<MasterListItem>();

                if (dgSearch1.ItemsSource != null)
                {
                    PagedSortableCollectionView<MasterListItem> list = (PagedSortableCollectionView<MasterListItem>)dgSearch1.ItemsSource;
                    foreach (var item in list)
                    {
                        if (((MasterListItem)item).Status == true)
                        {
                            item.Status = false;
                            var obj = lstObjectList.FirstOrDefault(q => q.ID == item.ID);
                            if (obj != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Record Already Added.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW1.Show();
                                continue;
                            }
                            else
                                lstObjectList.Add((MasterListItem)item);
                        }
                    }
                }
                dgSearch2.ItemsSource = null;
                dgSearch2.ItemsSource = lstObjectList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstObjectList = new List<MasterListItem>();
                PagedSortableCollectionView<MasterListItem> SearchList = null;
                if (dgSearch1.ItemsSource != null)
                {
                    SearchList = (PagedSortableCollectionView<MasterListItem>)dgSearch1.ItemsSource;
                }
                if (dgSearch2.ItemsSource != null)
                {
                    lstObjectList = (from list in ((List<MasterListItem>)dgSearch2.ItemsSource) where list.Status == false select list).ToList();

                    dgSearch2.ItemsSource = null;
                    dgSearch2.ItemsSource = lstObjectList;

                    MasterList = new PagedSortableCollectionView<MasterListItem>();
                    foreach (var item in SearchList)
                    {
                        item.Status = false;
                        MasterList.Add(item);
                    }

                    dgSearch1.ItemsSource = null;
                    dgSearch1.ItemsSource = MasterList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        List<MasterListItem> objList = null;
        private void FillGrid()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = TableName;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        Search();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdAddAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstObjectList = new List<MasterListItem>();
                foreach (MasterListItem item in ((PagedSortableCollectionView<MasterListItem>)dgSearch1.ItemsSource))
                {
                    item.Status = false;
                    lstObjectList.Add(item);
                }

                dgSearch2.ItemsSource = null;
                dgSearch2.ItemsSource = lstObjectList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PagedSortableCollectionView<MasterListItem> SearchList = null;
                if (dgSearch1.ItemsSource != null)
                {
                    SearchList = (PagedSortableCollectionView<MasterListItem>)dgSearch1.ItemsSource;
                }
                allFlag = false;

                lstObjectList = new List<MasterListItem>();
                dgSearch2.ItemsSource = null;
                dgSearch2.ItemsSource = lstObjectList;

                MasterList = new PagedSortableCollectionView<MasterListItem>();
                foreach (var item in SearchList)
                {
                    item.Status = false;
                    MasterList.Add(item);
                }

                dgSearch1.ItemsSource = null;
                dgSearch1.ItemsSource = MasterList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void dgSearch2ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgSearch2.SelectedItem != null)
                {
                    ((MasterListItem)dgSearch2.SelectedItem).Status = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void dgSearch2ChkBox_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgSearch2.SelectedItem != null)
                {
                    ((MasterListItem)dgSearch2.SelectedItem).Status = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TableName != MasterTableNameList.None)
                    FillGrid();

                if (txblSearch.Text.Length <= 6)
                    txblSearch.Text = txblSearch.Text + " " + labelOfDescriptionOnGrid;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void Search()
        {
            if (objList != null)//if (cmbSearch.ItemsSource != null)
            {
                List<MasterListItem> SearchList = objList;//(List<MasterListItem>)cmbSearch.ItemsSource;

                MasterList = new PagedSortableCollectionView<MasterListItem>();
                string SearchText = cmbSearch.Text;
                cmbSearch.Text = "";
                foreach (var item in SearchList)
                {
                    if (item.Description.Contains(SearchText))
                    {
                        if (item.ID == 0)
                            continue;
                        if (MasterList.Count > 0)
                        {
                            var obj = MasterList.FirstOrDefault(q => q.ID == ((MasterListItem)item).ID);
                            if (obj != null)
                                continue;
                        }
                        item.Status = false;
                        MasterList.Add(item);
                    }
                }
                PageSize = 15;
                MasterList.TotalItemCount = MasterList.Count;
                this.dataGrid2Pager.DataContext = MasterList;
                dgSearch1.ItemsSource = null;
                dgSearch1.ItemsSource = MasterList;
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void FillGrid2()
        {
            try
            {
                dgSearch2.ItemsSource = null;
                dgSearch2.ItemsSource = grid2ViewList;
                lstObjectList = null;
                lstObjectList = grid2ViewList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
