using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.ServiceModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.SearchResultLists
{
    public partial class SearchResultLists : UserControl
    {
        public string LabelOfList { get; set; }
        public List<MasterListItem> lstObjectList = null;
        public List<MasterListItem> grid2ViewList = null;
        public string labelOfDescriptionOnGrid { get; set; }
        private bool allFlag = false;
        public MasterTableNameList TableName { get; set; }
        // public int PageSize { get; set; }

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
                //OnPropertyChanged("PageSize");
            }
        }

        public SearchResultLists()
        {
            InitializeComponent();
            //Paging
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
            //Fill the Grid on Front Panel.
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
                        #region Commented
                        //if (PathoParameterList.Contains((clsPathoTestParameterVO)item))
                        //    continue;
                        //var obj = lstObjectList.FirstOrDefault(q => q.TestID == ((clsPathoProfileTestDetailsVO)item).TestID);
                        //if (obj != null)
                        //    continue;
                        //if (lstObjectList.Contains((MasterListItem)item))
                        //    continue;
                        //lstObjectList.Add((MasterListItem)item);
                        #endregion
                        
                        
                        var obj = lstObjectList.FirstOrDefault(q => q.ID == ((MasterListItem)item).ID);
                        if (obj != null)
                            continue;
                        else
                        {
                            // ((MasterListItem)dgSearch2.ItemsSource).

                            lstObjectList.Add((MasterListItem)item);
                          

                        }
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
                            //clsPathoProfileTestDetailsVO obj = TestList.FirstOrDefault(q => q.TestID == ((clsPathoProfileTestDetailsVO)item).TestID);
                            //if (obj != null)
                            //    continue;
                            item.Status = false;
                            #region Commented
                            //if (lstObjectList.Contains((MasterListItem)item))
                            //      continue;
                            //foreach (var item1 in lstObjectList)
                            //{
                            //    if (item.ID == item1.ID)
                            //        continue;
                            //    else

                            //    lstObjectList.Add((MasterListItem)item);
                            //}
                            #endregion
                            var obj = lstObjectList.FirstOrDefault(q => q.ID == item.ID);
                            if (obj != null)
                            {
                                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //new MessageBoxControl.MessageBoxChildWindow("", "Test Already Exists.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                //msgW1.Show();
                                continue;
                            }
                            else
                            {
                                //by rohini
                                int test = list.IndexOf(item);
                                if (test == 0)
                                {
                                    item.IsDefault = false;
                                }
                                else
                                {
                                    item.IsDefault = true;
                                }
                                //
                                lstObjectList.Add((MasterListItem)item);
                            }
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
                //lstObjectList = (List<MasterListItem>)dgSearch2.ItemsSource;
                PagedSortableCollectionView<MasterListItem> SearchList = null;
                if (dgSearch1.ItemsSource != null)
                {
                    SearchList = (PagedSortableCollectionView<MasterListItem>)dgSearch1.ItemsSource;
                }
                if (dgSearch2.ItemsSource != null)
                {
                    foreach (var item in dgSearch2.ItemsSource)
                    {
                        if (((MasterListItem)item).Status == false)
                        {
                            lstObjectList.Add((MasterListItem)item);
                        }
                        else
                        {
                            if (SearchList != null)
                            {
                                foreach (var SearchItems in SearchList.Where(x => x.ID == ((MasterListItem)item).ID))
                                {
                                    SearchItems.isChecked = false;
                                }
                            }
                        }
                    }
                    dgSearch2.ItemsSource = null;
                    dgSearch2.ItemsSource = lstObjectList;

                    dgSearch1.ItemsSource = null;
                    dgSearch1.ItemsSource = SearchList;
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
                //BizAction.IsActive = true;
                BizAction.MasterTable = TableName;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        //List<clsPathoProfileTestDetailsVO> objList = new List<clsPathoProfileTestDetailsVO>();
                        //foreach (var item in ((clsGetMasterListBizActionVO)e.Result).MasterList)
                        //{                           
                        //}
                        //dgSearch1.ItemsSource = null;
                        //dgSearch1.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        Search();
                        //cmbSearch.ItemsSource = null;
                        //cmbSearch.ItemsSource = objList;
                        //cmbSearch.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, App.SessionUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillGrid1()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = TableName;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        //List<clsPathoProfileTestDetailsVO> objList = new List<clsPathoProfileTestDetailsVO>();
                        //foreach (var item in ((clsGetMasterListBizActionVO)e.Result).MasterList)
                        //{
                        //}
                        //dgSearch1.ItemsSource = null;
                        //dgSearch1.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        //cmbSearch.ItemsSource = null;
                        //cmbSearch.ItemsSource = objList;
                        //cmbSearch.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdAddAll_Click(object sender, RoutedEventArgs e)
        {
            #region tempcommented
            //try
            //{
            //    if (dgSearch1.ItemsSource != null)
            //    {
            //        List<MasterListItem> list1 = (List<MasterListItem>)dgSearch1.ItemsSource;
            //        foreach (var item in list1)
            //        {
            //            item.Status = true;
            //        }
            //        dgSearch1.ItemsSource = null;
            //        dgSearch1.ItemsSource = list1;

            //        List<MasterListItem> list2 = new List<MasterListItem>();
            //        foreach (var item1 in list1)
            //        {

            //            //if (((object)item).Status == true)
            //            //{
            //            //clsPathoProfileTestDetailsVO obj = TestList.FirstOrDefault(q => q.TestID == ((clsPathoProfileTestDetailsVO)item).TestID);
            //            //if (obj != null)
            //            //    continue;
            //            list2.Add(item1);
            //        }

            //        List<MasterListItem> seract2List = (List<MasterListItem>)dgSearch2.ItemsSource;
            //        foreach (var item in list2)
            //        {
            //            item.Status = false;
            //            if (seract2List.Contains((MasterListItem)item))
            //                continue;
            //            seract2List.Add((MasterListItem)item);

            //            //}
            //        }

            //        dgSearch2.ItemsSource = null;

            //        dgSearch2.ItemsSource = seract2List;
            //    }


            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            #endregion
            #region Commented
            //try
            //{
            //    if (dgSearch1.ItemsSource != null)
            //    {
            //        List<MasterListItem> list = (List<MasterListItem>)dgSearch1.ItemsSource;
            //        foreach (var item in list)
            //        {
            //            item.Status = true;
            //        }
            //        dgSearch1.ItemsSource = null;
            //        dgSearch1.ItemsSource = list;                   
            //        allFlag = true;
            //        foreach (var item in list)
            //        {
            //            if (((MasterListItem)item).Status == true)
            //            {
            //                //clsPathoProfileTestDetailsVO obj = TestList.FirstOrDefault(q => q.TestID == ((clsPathoProfileTestDetailsVO)item).TestID);
            //                //if (obj != null)
            //                //    continue;
            //                item.Status = false;
            //                if (lstObjectList.Contains((MasterListItem)item))
            //                    continue;
            //                lstObjectList.Add((MasterListItem)item);
            //            }
            //        }
            //    }
            //    dgSearch2.ItemsSource = null;
            //    dgSearch2.ItemsSource = lstObjectList;
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}       
            #endregion

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
                        //if (PathoParameterList.Contains((clsPathoTestParameterVO)item))
                        //    continue;
                        //var obj = lstObjectList.FirstOrDefault(q => q.TestID == ((clsPathoProfileTestDetailsVO)item).TestID);
                        //if (obj != null)
                        //    continue;
                        if (lstObjectList.Contains((MasterListItem)item))
                            continue;
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
                        item.isChecked = false;
                        item.Status = false;
                        var obj = lstObjectList.FirstOrDefault(q => q.ID == ((MasterListItem)item).ID);
                        if (obj != null)
                            continue;
                        lstObjectList.Add((MasterListItem)item);
                        //foreach (var item1 in lstObjectList)
                        //{
                        //    if (item.ID == item1.ID)
                        //        continue;
                        //    else
                        //        lstObjectList.Add((MasterListItem)item);
                        //}
                    }
                }
                dgSearch2.ItemsSource = null;
                dgSearch2.ItemsSource = lstObjectList;
                if (dgSearch1.ItemsSource != null)
                {
                    PagedSortableCollectionView<MasterListItem> Searchlist = (PagedSortableCollectionView<MasterListItem>)dgSearch1.ItemsSource;
                    foreach (var item in Searchlist)
                    {
                        item.Status = false;
                    }
                    dgSearch1.ItemsSource = null;
                    dgSearch1.ItemsSource = Searchlist;
                }
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
                allFlag = false;
                // dgSearch2.ItemsSource = null;
                PagedSortableCollectionView<MasterListItem> list = new PagedSortableCollectionView<MasterListItem>();
                if (dgSearch2.ItemsSource != null)
                {
                    foreach (var item in dgSearch2.ItemsSource)
                    {
                        if (((MasterListItem)item).Status == false)
                        {
                            list.Add((MasterListItem)item);
                        }
                    }
                    dgSearch2.ItemsSource = null;
                    dgSearch2.ItemsSource = list;
                    //(PagedSortableCollectionView<MasterListItem>)dgSearch1.ItemsSource;
                    //foreach (var item in list)
                    //{
                    //    item.isChecked = false;
                    //    item.Status = false;
                    //}
                    //dgSearch1.ItemsSource = null;
                    //dgSearch1.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ChkdgSearch1_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgSearch1.SelectedItem != null)
                {
                    ((MasterListItem)dgSearch1.SelectedItem).Status = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ChkdgSearch1_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgSearch1.SelectedItem != null)
                {
                    ((MasterListItem)dgSearch1.SelectedItem).Status = false;
                }
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
                //colDescription.Header = labelOfDescriptionOnGrid;
                //ColDg2Description.Header = labelOfDescriptionOnGrid;
                if (TableName != MasterTableNameList.None)
                    FillGrid();
                //dgSearch1.ItemsSource = lstObjectList;               
                //int i = dgSearch1.Columns.Count;
                //int j = dgSearch1.ItemsSource.OfType<MasterListItem>().Count();                
                dgSearch1.Columns[1].Header = labelOfDescriptionOnGrid;
                dgSearch2.Columns[1].Header = labelOfDescriptionOnGrid;
                if (txblSearch.Text.Length <= 6)
                    txblSearch.Text = txblSearch.Text + " " + labelOfDescriptionOnGrid;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void dgSearch1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //try
            //{
            //    if (allFlag == true)
            //    {
            //        FrameworkElement cellContent = dgSearch1.Columns[0].GetCellContent(e.Row);
            //        CheckBox b = cellContent as CheckBox;
            //        if (b != null)
            //            b.IsChecked = true;
            //        //Object Status = dgSearch1.Columns[0].GetCellContent(e.Row).
            //    }
            //    else
            //    {
            //        FrameworkElement cellContent = dgSearch1.Columns[0].GetCellContent(e.Row);
            //        CheckBox b = cellContent as CheckBox;
            //        if (b != null)

            //            b.IsChecked = false;
            //    }
            //}

            //catch (Exception ex)
            //{
            //    throw;
            //}
        }

        private void Search()
        {
            if (objList != null)//if (cmbSearch.ItemsSource != null)
            {
                List<MasterListItem> SearchList = objList;//(List<MasterListItem>)cmbSearch.ItemsSource;
                //PagedSortableCollectionView<MasterListItem> SearchList1 = new PagedSortableCollectionView<MasterListItem>();

                //if (dgSearch1.ItemsSource != null)
                //{
                //    MasterList = new PagedSortableCollectionView<MasterListItem>();
                //    MasterList = (PagedSortableCollectionView<MasterListItem>)dgSearch1.ItemsSource;
                //}
                //else
                //{
                //    MasterList = new PagedSortableCollectionView<MasterListItem>();
                //}
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
                            //if (MasterList.Contains(item))
                            //    continue;
                            var obj = MasterList.FirstOrDefault(q => q.ID == ((MasterListItem)item).ID);
                            if (obj != null)
                                continue;
                        }
                        MasterList.Add(item);
                    }
                }
                //MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 15;
                //this.grdMaster.DataContext = MasterList;
                MasterList.TotalItemCount = MasterList.Count;
                this.dataGrid2Pager.DataContext = MasterList;
                dgSearch1.ItemsSource = null;
                dgSearch1.ItemsSource = MasterList;
            }
        }

        private void cmbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
